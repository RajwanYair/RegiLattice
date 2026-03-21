// RegiLattice.Core — Services/Ratings.cs
// Local tweak rating system — replaces Python ratings.py.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

public sealed class TweakRating
{
    [JsonPropertyName("stars")]
    public int Stars { get; set; }

    [JsonPropertyName("note")]
    public string Note { get; set; } = "";
}

public static class Ratings
{
    private static readonly string FilePath = Path.Combine(AppConfig.ConfigDir, "ratings.json");
    private static readonly object Lock = new();

    public static void Rate(string tweakId, int stars, string note = "")
    {
        if (stars < 1 || stars > 5)
            throw new ArgumentOutOfRangeException(nameof(stars), "Stars must be 1-5.");
        lock (Lock)
        {
            var all = AllRatings();
            all[tweakId] = new TweakRating { Stars = stars, Note = note };
            Save(all);
        }
    }

    public static TweakRating? GetRating(string tweakId) => AllRatings().GetValueOrDefault(tweakId);

    public static Dictionary<string, TweakRating> AllRatings()
    {
        lock (Lock)
        {
            if (!File.Exists(FilePath))
                return [];
            try
            {
                var json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<Dictionary<string, TweakRating>>(json) ?? [];
            }
            catch
            {
                return [];
            }
        }
    }

    public static void RemoveRating(string tweakId)
    {
        lock (Lock)
        {
            var all = AllRatings();
            all.Remove(tweakId);
            Save(all);
        }
    }

    public static IReadOnlyList<(string Id, TweakRating Rating)> TopRated(int n = 10) =>
        AllRatings().OrderByDescending(kv => kv.Value.Stars).Take(n).Select(kv => (kv.Key, kv.Value)).ToList();

    public static double? AverageRating()
    {
        var all = AllRatings();
        return all.Count == 0 ? null : all.Values.Average(r => r.Stars);
    }

    private static void Save(Dictionary<string, TweakRating> data)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(FilePath, json);
    }
}
