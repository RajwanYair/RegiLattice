using System.Text.Json;
using System.Text.Json.Serialization;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>A single entry in the tweak operation history.</summary>
public sealed class HistoryEntry
{
    [JsonPropertyName("id")]
    public string TweakId { get; set; } = "";

    [JsonPropertyName("action")]
    public string Action { get; set; } = "";

    [JsonPropertyName("result")]
    public string Result { get; set; } = "";

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = "";
}

/// <summary>
/// Tracks a rolling history of tweak apply/remove operations with timestamps.
/// Persisted to %LOCALAPPDATA%\RegiLattice\history.json.
/// Keeps the most recent 500 entries.
/// </summary>
public static class TweakHistory
{
    private static readonly string FilePath = Path.Combine(AppConfig.ConfigDir, "history.json");
    private static readonly object Lock = new();
    private static List<HistoryEntry>? _cache;
    private static bool _dirty;

    /// <summary>Maximum entries to keep in history.</summary>
    public const int MaxEntries = 500;

    /// <summary>Record an apply operation.</summary>
    public static void RecordApply(string tweakId, TweakResult result)
    {
        Record(tweakId, "apply", result.ToString());
    }

    /// <summary>Record a remove operation.</summary>
    public static void RecordRemove(string tweakId, TweakResult result)
    {
        Record(tweakId, "remove", result.ToString());
    }

    /// <summary>Record an update operation.</summary>
    public static void RecordUpdate(string tweakId, TweakResult result)
    {
        Record(tweakId, "update", result.ToString());
    }

    /// <summary>Record a generic operation.</summary>
    public static void Record(string tweakId, string action, string result)
    {
        lock (Lock)
        {
            var list = LoadList();
            list.Add(
                new HistoryEntry
                {
                    TweakId = tweakId,
                    Action = action,
                    Result = result,
                    Timestamp = DateTimeOffset.UtcNow.ToString("o"),
                }
            );

            // Trim to max entries
            if (list.Count > MaxEntries)
                list.RemoveRange(0, list.Count - MaxEntries);

            _dirty = true;
        }
    }

    /// <summary>Get all history entries (oldest first).</summary>
    public static IReadOnlyList<HistoryEntry> All()
    {
        lock (Lock)
            return LoadList().ToList();
    }

    /// <summary>Get the most recent N entries (newest first).</summary>
    public static IReadOnlyList<HistoryEntry> Recent(int count = 20)
    {
        lock (Lock)
        {
            var list = LoadList();
            return list.AsEnumerable().Reverse().Take(count).ToList();
        }
    }

    /// <summary>Get history for a specific tweak ID.</summary>
    public static IReadOnlyList<HistoryEntry> ForTweak(string tweakId)
    {
        lock (Lock)
        {
            return LoadList().Where(e => e.TweakId.Equals(tweakId, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }

    /// <summary>Get the count of history entries.</summary>
    public static int Count
    {
        get
        {
            lock (Lock)
                return LoadList().Count;
        }
    }

    /// <summary>Write pending changes to disk.</summary>
    public static void Flush()
    {
        lock (Lock)
        {
            if (!_dirty || _cache is null)
                return;
            Save(_cache);
            _dirty = false;
        }
    }

    /// <summary>Clear all history.</summary>
    public static void Clear()
    {
        lock (Lock)
        {
            _cache = [];
            _dirty = true;
        }
    }

    /// <summary>Reset in-memory cache (for testing).</summary>
    public static void Reset()
    {
        lock (Lock)
        {
            _cache = null;
            _dirty = false;
            if (File.Exists(FilePath))
                File.Delete(FilePath);
        }
    }

    // ── Sprint 47 enhancements ─────────────────────────────────────────────

    /// <summary>Summary statistics record for the current history window.</summary>
    public sealed record HistorySummaryStats(
        int TotalEntries,
        int ApplyCount,
        int RemoveCount,
        int UpdateCount,
        IReadOnlyList<(string TweakId, int Count)> TopTweaks
    );

    /// <summary>
    /// Returns aggregate statistics: total entries, counts by action type, and the top-5 most
    /// frequently operated tweaks (any action).
    /// </summary>
    public static HistorySummaryStats GetSummaryStats()
    {
        lock (Lock)
        {
            var list = LoadList();
            int apply = 0, remove = 0, update = 0;
            var freq = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

            foreach (var e in list)
            {
                switch (e.Action.ToLowerInvariant())
                {
                    case "apply": apply++; break;
                    case "remove": remove++; break;
                    case "update": update++; break;
                }
                freq[e.TweakId] = freq.GetValueOrDefault(e.TweakId, 0) + 1;
            }

            var top = freq.OrderByDescending(kv => kv.Value).Take(5)
                         .Select(kv => (kv.Key, kv.Value)).ToList();

            return new HistorySummaryStats(list.Count, apply, remove, update, top);
        }
    }

    /// <summary>
    /// Exports the current history to a JSON file at <paramref name="filePath"/>.
    /// </summary>
    public static async Task ExportToJsonAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        IReadOnlyList<HistoryEntry> snapshot;
        lock (Lock)
            snapshot = LoadList().ToList();

        var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });
        Directory.CreateDirectory(Path.GetDirectoryName(filePath) ?? ".");
        await File.WriteAllTextAsync(filePath, json).ConfigureAwait(false);
    }

    private static List<HistoryEntry> LoadList()
    {
        if (_cache is not null)
            return _cache;

        if (!File.Exists(FilePath))
        {
            _cache = [];
            return _cache;
        }

        try
        {
            var json = File.ReadAllText(FilePath);
            _cache = JsonSerializer.Deserialize<List<HistoryEntry>>(json) ?? [];
        }
        catch
        {
            _cache = [];
        }

        return _cache;
    }

    private static void Save(List<HistoryEntry> data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
