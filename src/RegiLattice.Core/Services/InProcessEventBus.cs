// RegiLattice.Core — Services/InProcessEventBus.cs
// Thread-safe, in-process IEventBus implementation (B.5).

namespace RegiLattice.Core.Services;

/// <summary>
/// Thread-safe, in-process implementation of <see cref="IEventBus"/>.
/// Subscribers are invoked synchronously on the calling thread.
/// Exceptions thrown by a subscriber are caught and do not prevent
/// delivery to remaining subscribers.
/// </summary>
public sealed class InProcessEventBus : IEventBus
{
    private readonly object _lock = new();

    // Map from event type → list of weakly-typed handlers (Action<object>)
    private readonly Dictionary<Type, List<WeakHandler>> _handlers = [];

    /// <inheritdoc/>
    public void Publish<T>(T evt)
    {
        List<WeakHandler>? snapshot;
        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(T), out var handlers))
                return;
            snapshot = [..handlers];
        }

        foreach (var wh in snapshot)
        {
            try
            {
                wh.Invoke(evt!);
            }
            catch
            {
                // Swallow — a failing subscriber must not break other subscribers
                // or the engine operation that triggered the event.
            }
        }
    }

    /// <inheritdoc/>
    public IDisposable Subscribe<T>(Action<T> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var wh = new WeakHandler(o => handler((T)o), handler);
        lock (_lock)
        {
            if (!_handlers.TryGetValue(typeof(T), out var list))
            {
                list = [];
                _handlers[typeof(T)] = list;
            }
            list.Add(wh);
        }

        return new Subscription(() =>
        {
            lock (_lock)
            {
                if (_handlers.TryGetValue(typeof(T), out var l))
                    l.Remove(wh);
            }
        });
    }

    // ── Inner types ──────────────────────────────────────────────────────

    private sealed class WeakHandler(Action<object> invoke, object identity)
    {
        private readonly Action<object> _invoke = invoke;
        private readonly object _identity = identity;

        public void Invoke(object evt) => _invoke(evt);

        // Use object identity for Remove() matching
        public override bool Equals(object? obj) => obj is WeakHandler w && ReferenceEquals(w._identity, _identity);
        public override int GetHashCode() => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(_identity);
    }

    private sealed class Subscription(Action onDispose) : IDisposable
    {
        private readonly Action _onDispose = onDispose;
        private int _disposed;

        public void Dispose()
        {
            if (System.Threading.Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
                _onDispose();
        }
    }
}
