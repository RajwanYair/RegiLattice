using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Native.Models;

/// <summary>
/// Matches the JSON object shape produced by
/// <c>python -m regilattice --export-json</c>.
///
/// Fields correspond 1-to-1 to Python's TweakDef dataclass as serialised
/// by the <c>cli.py</c> export handler.
/// </summary>
public sealed class TweakInfo
{
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    [JsonPropertyName("label")]
    public string Label { get; init; } = string.Empty;

    [JsonPropertyName("category")]
    public string Category { get; init; } = string.Empty;

    /// <summary>"applied", "not_applied", or "unknown".</summary>
    [JsonPropertyName("status")]
    public string Status { get; set; } = "unknown";

    [JsonPropertyName("needs_admin")]
    public bool NeedsAdmin { get; init; }

    [JsonPropertyName("corp_safe")]
    public bool CorpSafe { get; init; }

    [JsonPropertyName("tags")]
    public IReadOnlyList<string> Tags { get; init; } = [];

    [JsonPropertyName("registry_keys")]
    public IReadOnlyList<string> RegistryKeys { get; init; } = [];

    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;

    // ── Derived helpers used by the UI ────────────────────────────────────

    /// <summary>True if the tweak is currently applied.</summary>
    [JsonIgnore]
    public bool IsApplied => Status == "applied";

    /// <summary>Scope badge text shown in the list view.</summary>
    [JsonIgnore]
    public string ScopeBadge
    {
        get
        {
            bool hasUser    = RegistryKeys.Any(k => k.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase) ||
                                                    k.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase));
            bool hasMachine = RegistryKeys.Any(k => k.StartsWith("HKLM", StringComparison.OrdinalIgnoreCase) ||
                                                    k.StartsWith("HKEY_LOCAL_MACHINE", StringComparison.OrdinalIgnoreCase) ||
                                                    k.StartsWith("HKCR", StringComparison.OrdinalIgnoreCase) ||
                                                    k.StartsWith("HKEY_CLASSES_ROOT", StringComparison.OrdinalIgnoreCase));
            return (hasUser, hasMachine) switch
            {
                (true, true)  => "BOTH",
                (true, false) => "USER",
                _             => "MACHINE",
            };
        }
    }
}

/// <summary>Source-generated JSON serialiser context (trimming-safe).</summary>
[JsonSerializable(typeof(List<TweakInfo>))]
[JsonSerializable(typeof(TweakInfo))]
[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
internal partial class TweakInfoSerializerContext : JsonSerializerContext { }
