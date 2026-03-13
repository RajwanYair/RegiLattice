// RegiLattice.Core — Plugins/PackIndex.cs
// Model for the remote marketplace index (index.json hosted on GitHub).

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Represents the marketplace index fetched from the GitHub repository.
/// Contains metadata for all available packs without their full tweak definitions.
/// </summary>
public sealed class PackIndex
{
    [JsonPropertyName("version")]
    public int Version { get; init; } = 1;

    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; init; }

    [JsonPropertyName("packs")]
    public IReadOnlyList<PackDef> Packs { get; init; } = [];

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>Deserialise an index.json string into a PackIndex.</summary>
    public static PackIndex? FromJson(string json)
    {
        return JsonSerializer.Deserialize<PackIndex>(json, s_jsonOptions);
    }

    /// <summary>Serialise this index to a JSON string.</summary>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
    }
}
