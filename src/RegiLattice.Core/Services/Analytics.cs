// RegiLattice.Core — Services/Analytics.cs
// Local-only usage analytics — replaces Python analytics.py.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

public sealed class AnalyticsData
{
    [JsonPropertyName("total_applies")] public int TotalApplies { get; set; }
    [JsonPropertyName("total_removes")] public int TotalRemoves { get; set; }
    [JsonPropertyName("total_errors")] public int TotalErrors { get; set; }
    [JsonPropertyName("total_sessions")] public int TotalSessions { get; set; }
    [JsonPropertyName("most_applied")] public Dictionary<string, int> MostApplied { get; set; } = [];
    [JsonPropertyName("most_removed")] public Dictionary<string, int> MostRemoved { get; set; } = [];
    [JsonPropertyName("error_counts")] public Dictionary<string, int> ErrorCounts { get; set; } = [];
    [JsonPropertyName("last_session")] public double LastSession { get; set; }
}

public static class Analytics
{
    private static readonly string FilePath = Path.Combine(AppConfig.ConfigDir, "analytics.json");
    private static readonly object Lock = new();

    public static AnalyticsData GetStats()
    {
        lock (Lock)
        {
            if (!File.Exists(FilePath)) return new AnalyticsData();
            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<AnalyticsData>(json) ?? new AnalyticsData();
            }
            catch { return new AnalyticsData(); }
        }
    }

    public static void RecordApply(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalApplies++;
            data.MostApplied[tweakId] = data.MostApplied.GetValueOrDefault(tweakId) + 1;
            Save(data);
        }
    }

    public static void RecordRemove(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalRemoves++;
            data.MostRemoved[tweakId] = data.MostRemoved.GetValueOrDefault(tweakId) + 1;
            Save(data);
        }
    }

    public static void RecordError(string tweakId)
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalErrors++;
            data.ErrorCounts[tweakId] = data.ErrorCounts.GetValueOrDefault(tweakId) + 1;
            Save(data);
        }
    }

    public static void RecordSession()
    {
        lock (Lock)
        {
            var data = GetStats();
            data.TotalSessions++;
            data.LastSession = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            Save(data);
        }
    }

    public static IReadOnlyList<(string Id, int Count)> TopTweaks(int n = 10)
    {
        var data = GetStats();
        return data.MostApplied
            .OrderByDescending(kv => kv.Value)
            .Take(n)
            .Select(kv => (kv.Key, kv.Value))
            .ToList();
    }

    public static void Reset()
    {
        lock (Lock)
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
        }
    }

    private static void Save(AnalyticsData data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
