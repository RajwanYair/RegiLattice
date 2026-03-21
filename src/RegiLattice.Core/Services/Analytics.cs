// RegiLattice.Core — Services/Analytics.cs
// Local-only usage analytics — replaces Python analytics.py.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

public sealed class AnalyticsData
{
    [JsonPropertyName("total_applies")]
    public int TotalApplies { get; set; }

    [JsonPropertyName("total_removes")]
    public int TotalRemoves { get; set; }

    [JsonPropertyName("total_errors")]
    public int TotalErrors { get; set; }

    [JsonPropertyName("total_sessions")]
    public int TotalSessions { get; set; }

    [JsonPropertyName("most_applied")]
    public Dictionary<string, int> MostApplied { get; set; } = [];

    [JsonPropertyName("most_removed")]
    public Dictionary<string, int> MostRemoved { get; set; } = [];

    [JsonPropertyName("error_counts")]
    public Dictionary<string, int> ErrorCounts { get; set; } = [];

    [JsonPropertyName("last_session")]
    public double LastSession { get; set; }
}

public static class Analytics
{
    private static readonly string FilePath = Path.Combine(AppConfig.ConfigDir, "analytics.json");
    private static readonly object Lock = new();
    private static AnalyticsData? _cache;
    private static bool _dirty;

    public static AnalyticsData GetStats()
    {
        lock (Lock)
        {
            if (_cache is not null)
                return _cache;
            if (!File.Exists(FilePath))
            {
                _cache = new AnalyticsData();
                return _cache;
            }
            try
            {
                var json = File.ReadAllText(FilePath);
                _cache = JsonSerializer.Deserialize<AnalyticsData>(json) ?? new AnalyticsData();
                return _cache;
            }
            catch
            {
                _cache = new AnalyticsData();
                return _cache;
            }
        }
    }

    public static void RecordApply(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalApplies++;
            data.MostApplied[tweakId] = data.MostApplied.GetValueOrDefault(tweakId) + 1;
            _dirty = true;
        }
    }

    public static void RecordRemove(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalRemoves++;
            data.MostRemoved[tweakId] = data.MostRemoved.GetValueOrDefault(tweakId) + 1;
            _dirty = true;
        }
    }

    public static void RecordError(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalErrors++;
            data.ErrorCounts[tweakId] = data.ErrorCounts.GetValueOrDefault(tweakId) + 1;
            _dirty = true;
        }
    }

    public static void RecordSession()
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalSessions++;
            data.LastSession = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            _dirty = true;
        }
    }

    /// <summary>Flush any pending analytics data to disk. Call at session end or periodically.</summary>
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

    public static IReadOnlyList<(string Id, int Count)> TopTweaks(int n = 10)
    {
        var data = GetStats();
        return data.MostApplied.OrderByDescending(kv => kv.Value).Take(n).Select(kv => (kv.Key, kv.Value)).ToList();
    }

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

    private static void Save(AnalyticsData data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
