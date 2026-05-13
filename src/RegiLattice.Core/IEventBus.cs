// RegiLattice.Core — IEventBus.cs
// Simple pub/sub event bus abstraction (B.5 — decouple engine events from consumers).

namespace RegiLattice.Core;

/// <summary>
/// Lightweight in-process event bus for decoupled communication between
/// the tweak engine and UI/service consumers.
/// </summary>
public interface IEventBus
{
    /// <summary>
    /// Publish an event of type <typeparamref name="T"/> to all current subscribers.
    /// Must not throw if there are no subscribers.
    /// </summary>
    void Publish<T>(T evt);

    /// <summary>
    /// Subscribe to events of type <typeparamref name="T"/>.
    /// Returns a disposable token; dispose it to unsubscribe.
    /// </summary>
    IDisposable Subscribe<T>(Action<T> handler);
}
