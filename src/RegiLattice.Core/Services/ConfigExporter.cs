using System.Text.Json;
using System.Text.Json.Serialization;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// A portable tweak selection config — the set of tweak IDs a user wants applied.
/// Can be shared between machines or used for repeatable setups.
/// </summary>
public sealed class TweakConfig
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    [JsonPropertyName("created")]
    public string Created { get; set; } = DateTimeOffset.UtcNow.ToString("o");

    [JsonPropertyName("regilattice_version")]
    public string RegiLatticeVersion { get; set; } = "3.4.0";

    [JsonPropertyName("tweaks")]
    public List<string> Tweaks { get; set; } = [];
}

/// <summary>
/// Export and import portable tweak selection configs as JSON files.
/// </summary>
public static class ConfigExporter
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    /// <summary>
    /// Export a set of tweak IDs to a portable JSON config file.
    /// </summary>
    public static void Export(string path, IEnumerable<string> tweakIds, string name = "", string description = "")
    {
        ArgumentNullException.ThrowIfNull(path);
        var config = new TweakConfig
        {
            Name = name,
            Description = description,
            Tweaks = tweakIds.ToList(),
        };
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        var json = JsonSerializer.Serialize(config, JsonOptions);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Export only the currently-applied tweak IDs (by detecting status).
    /// </summary>
    public static void ExportApplied(string path, TweakEngine engine, string name = "Applied tweaks")
    {
        var statusMap = engine.StatusMap(parallel: true);
        var appliedIds = statusMap
            .Where(kv => kv.Value == TweakResult.Applied)
            .Select(kv => kv.Key)
            .OrderBy(id => id, StringComparer.OrdinalIgnoreCase)
            .ToList();
        Export(path, appliedIds, name, $"Exported {appliedIds.Count} applied tweaks");
    }

    /// <summary>
    /// Import a tweak config from a JSON file and return the parsed config.
    /// Returns null if the file is invalid.
    /// </summary>
    public static TweakConfig? Import(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        if (!File.Exists(path))
            return null;

        var json = File.ReadAllText(path);

        // Try full TweakConfig format first
        try
        {
            return JsonSerializer.Deserialize<TweakConfig>(json);
        }
        catch
        {
            // Fall through to try simpler formats
        }

        // Try plain array of IDs: ["id1", "id2"]
        try
        {
            var ids = JsonSerializer.Deserialize<List<string>>(json);
            if (ids is not null)
                return new TweakConfig { Tweaks = ids };
        }
        catch
        {
            // Fall through
        }

        // Try {"tweaks": ["id1", "id2"]} format
        try
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (dict is not null && dict.TryGetValue("tweaks", out var elem))
            {
                var ids = elem.Deserialize<List<string>>();
                if (ids is not null)
                    return new TweakConfig { Tweaks = ids };
            }
        }
        catch
        {
            // All formats failed
        }

        return null;
    }

    /// <summary>
    /// Validate imported config against an engine — returns (valid IDs, unknown IDs).
    /// </summary>
    public static (IReadOnlyList<string> ValidIds, IReadOnlyList<string> UnknownIds) Validate(TweakConfig config, TweakEngine engine)
    {
        var valid = new List<string>();
        var unknown = new List<string>();

        foreach (var id in config.Tweaks)
        {
            if (engine.GetTweak(id) is not null)
                valid.Add(id);
            else
                unknown.Add(id);
        }

        return (valid, unknown);
    }
}
