// RegiLattice.Core — Services/JsonHistoryRepository.cs
// JSON-backed IHistoryRepository implementation (B.3 — decouple TweakHistory from file I/O).

using System.Text.Json;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// <see cref="IHistoryRepository"/> backed by a JSON file on disk.
/// Defaults to the user-scoped path used by the static <see cref="TweakHistory"/> helper;
/// can be constructed with a custom path for testing or multi-profile scenarios.
/// Keeps at most <see cref="MaxEntries"/> entries (oldest are dropped when the cap is reached).
/// </summary>
public sealed class JsonHistoryRepository : IHistoryRepository
{
    /// <summary>Maximum entries to retain. Matches <see cref="TweakHistory.MaxEntries"/>.</summary>
    public const int MaxEntries = 500;

    private readonly string _filePath;
    private readonly object _lock = new();
    private List<HistoryEntry>? _cache;
    private bool _dirty;

    // Per-instance session ID for audit grouping (matches TweakHistory pattern).
    private readonly string _sessionId = Guid.NewGuid().ToString("N")[..8];

    /// <summary>
    /// Creates a repository backed by the default history file
    /// (<c>%LOCALAPPDATA%\RegiLattice\history.json</c>).
    /// </summary>
    public JsonHistoryRepository()
        : this(Path.Combine(AppConfig.ConfigDir, "history.json")) { }

    /// <summary>Creates a repository backed by a custom file path (useful in tests).</summary>
    public JsonHistoryRepository(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        _filePath = filePath;
    }

    /// <inheritdoc/>
    public void RecordApply(string tweakId, TweakResult result) =>
        Record(tweakId, "apply", result.ToString());

    /// <inheritdoc/>
    public void RecordRemove(string tweakId, TweakResult result) =>
        Record(tweakId, "remove", result.ToString());

    /// <inheritdoc/>
    public void RecordUpdate(string tweakId, TweakResult result) =>
        Record(tweakId, "update", result.ToString());

    /// <inheritdoc/>
    public void Record(string tweakId, string action, string result)
    {
        lock (_lock)
        {
            var list = LoadList();
            list.Add(new HistoryEntry
            {
                TweakId = tweakId,
                Action = action,
                Result = result,
                Timestamp = DateTimeOffset.UtcNow.ToString("o"),
                Username = Environment.UserName,
                MachineName = Environment.MachineName,
                SessionId = _sessionId,
            });

            if (list.Count > MaxEntries)
                list.RemoveRange(0, list.Count - MaxEntries);

            _dirty = true;
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<HistoryEntry> Recent(int count = 20)
    {
        lock (_lock)
        {
            return LoadList().AsEnumerable().Reverse().Take(count).ToList();
        }
    }

    /// <inheritdoc/>
    public IReadOnlyList<HistoryEntry> ForTweak(string tweakId)
    {
        lock (_lock)
        {
            return LoadList()
                .Where(e => e.TweakId.Equals(tweakId, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }

    /// <inheritdoc/>
    public void Clear()
    {
        lock (_lock)
        {
            _cache = [];
            _dirty = true;
        }
    }

    /// <inheritdoc/>
    public int Count
    {
        get
        {
            lock (_lock)
                return LoadList().Count;
        }
    }

    /// <inheritdoc/>
    public void Flush()
    {
        lock (_lock)
        {
            if (_dirty && _cache is not null)
                PersistToFile(_cache);
            _dirty = false;
        }
    }

    // ── Private helpers ───────────────────────────────────────────────────────

    private List<HistoryEntry> LoadList()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(_filePath))
        {
            _cache = [];
            return _cache;
        }

        try
        {
            var json = File.ReadAllText(_filePath);
            _cache = JsonSerializer.Deserialize<List<HistoryEntry>>(json) ?? [];
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            _cache = [];
        }

        return _cache;
    }

    private void PersistToFile(List<HistoryEntry> data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        var json = JsonSerializer.Serialize(data, JsonOptions.Indented);
        const int maxRetries = 5;
        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                File.WriteAllText(_filePath, json);
                return;
            }
            catch (IOException) when (attempt < maxRetries - 1)
            {
                Thread.Sleep(60 * (attempt + 1));
            }
        }
    }
}
