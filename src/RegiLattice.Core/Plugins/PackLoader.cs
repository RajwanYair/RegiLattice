// RegiLattice.Core — Plugins/PackLoader.cs
// Deserialises pack.json files into TweakDef arrays with validation.

using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Loads a Tweak Pack JSON file and converts it to an array of <see cref="TweakDef"/>.
/// Only declarative RegOp tweaks are supported — no custom ApplyAction/DetectAction delegates.
/// </summary>
public static class PackLoader
{
    private const int MaxPackSizeBytes = 1_048_576; // 1 MB
    private const int MaxTweaksPerPack = 100;

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>
    /// Load tweaks from a pack JSON string. Returns the pack metadata and tweak list.
    /// Throws <see cref="InvalidOperationException"/> on validation failure.
    /// </summary>
    public static (PackDef Pack, IReadOnlyList<TweakDef> Tweaks) LoadFromJson(string json, string? expectedSha256 = null)
    {
        if (json.Length > MaxPackSizeBytes)
            throw new InvalidOperationException($"Pack JSON exceeds maximum size of {MaxPackSizeBytes / 1024}KB.");

        if (expectedSha256 is not null)
        {
            var actualHash = ComputeSha256(json);
            if (!string.Equals(actualHash, expectedSha256, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException($"SHA-256 mismatch: expected {expectedSha256}, got {actualHash}.");
        }

        var raw = JsonSerializer.Deserialize<RawPack>(json, s_jsonOptions) ?? throw new InvalidOperationException("Failed to deserialise pack JSON.");

        var errors = Validate(raw);
        if (errors.Count > 0)
            throw new InvalidOperationException($"Pack validation failed:\n{string.Join('\n', errors)}");

        // Validate() above guarantees Name/DisplayName/Version/Author are non-null.
        var packDef = new PackDef
        {
            Name = raw.Name!,
            DisplayName = raw.DisplayName!,
            Version = raw.Version!,
            Author = raw.Author!,
            Description = raw.Description ?? "",
            TweakCount = raw.Tweaks?.Count ?? 0,
            Categories = raw.Categories ?? [],
            Tags = raw.Tags ?? [],
            MinRegiLatticeVersion = raw.MinRegiLatticeVersion ?? "3.3.0",
            MinWindowsBuild = raw.MinWindowsBuild,
        };

        var tweaks = new List<TweakDef>();
        foreach (var rt in raw.Tweaks ?? [])
        {
            var td = ConvertTweak(rt, raw.Name!);
            tweaks.Add(td);
        }

        return (packDef, tweaks);
    }

    /// <summary>Validate a pack JSON string without loading it. Returns a list of errors (empty = valid).</summary>
    public static IReadOnlyList<string> ValidatePackJson(string json)
    {
        try
        {
            var raw = JsonSerializer.Deserialize<RawPack>(json, s_jsonOptions);
            if (raw is null)
                return ["Failed to parse JSON."];
            return Validate(raw);
        }
        catch (JsonException ex)
        {
            return [$"JSON parse error: {ex.Message}"];
        }
    }

    /// <summary>Compute SHA-256 hash of a string (UTF-8).</summary>
    public static string ComputeSha256(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexStringLower(hash);
    }

    // ── Validation ──────────────────────────────────────────────────────

    private static List<string> Validate(RawPack pack)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(pack.Name))
            errors.Add("Pack 'name' is required.");
        if (string.IsNullOrWhiteSpace(pack.DisplayName))
            errors.Add("Pack 'displayName' is required.");
        if (string.IsNullOrWhiteSpace(pack.Version))
            errors.Add("Pack 'version' is required.");
        if (string.IsNullOrWhiteSpace(pack.Author))
            errors.Add("Pack 'author' is required.");

        if (pack.Tweaks is null || pack.Tweaks.Count == 0)
            errors.Add("Pack must contain at least one tweak.");
        else if (pack.Tweaks.Count > MaxTweaksPerPack)
            errors.Add($"Pack exceeds maximum of {MaxTweaksPerPack} tweaks (has {pack.Tweaks.Count}).");

        var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var t in pack.Tweaks ?? [])
        {
            if (string.IsNullOrWhiteSpace(t.Id))
            {
                errors.Add("Tweak missing 'id'.");
                continue;
            }

            // IDs must be prefixed with pack name
            if (pack.Name is not null && !t.Id.StartsWith(pack.Name + "-", StringComparison.OrdinalIgnoreCase))
                errors.Add($"Tweak '{t.Id}' must be prefixed with pack name '{pack.Name}-'.");

            if (!seenIds.Add(t.Id))
                errors.Add($"Duplicate tweak ID: '{t.Id}'.");

            if (string.IsNullOrWhiteSpace(t.Label))
                errors.Add($"Tweak '{t.Id}' missing 'label'.");
            if (string.IsNullOrWhiteSpace(t.Category))
                errors.Add($"Tweak '{t.Id}' missing 'category'.");

            if ((t.ApplyOps is null || t.ApplyOps.Count == 0) && (t.RemoveOps is null || t.RemoveOps.Count == 0))
                errors.Add($"Tweak '{t.Id}' must have applyOps or removeOps.");
            if (t.DetectOps is null || t.DetectOps.Count == 0)
                errors.Add($"Tweak '{t.Id}' must have detectOps.");

            // Validate registry paths
            foreach (var op in (t.ApplyOps ?? []).Concat(t.RemoveOps ?? []).Concat(t.DetectOps ?? []))
            {
                if (
                    !string.IsNullOrEmpty(op.Path)
                    && !op.Path.StartsWith("HKEY_", StringComparison.OrdinalIgnoreCase)
                    && !op.Path.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase)
                    && !op.Path.StartsWith("HKLM", StringComparison.OrdinalIgnoreCase)
                    && !op.Path.StartsWith("HKCR", StringComparison.OrdinalIgnoreCase)
                )
                {
                    errors.Add($"Tweak '{t.Id}' has invalid registry path: '{op.Path}' (must start with HKEY_ or HKCU/HKLM/HKCR).");
                }
            }
        }

        return errors;
    }

    // ── Conversion ──────────────────────────────────────────────────────

    private static TweakDef ConvertTweak(RawTweak rt, string packName)
    {
        return new TweakDef
        {
            Id = rt.Id!,
            Label = rt.Label ?? rt.Id!,
            Category = rt.Category ?? packName,
            Description = rt.Description ?? "",
            Tags = rt.Tags ?? [],
            NeedsAdmin = rt.NeedsAdmin,
            CorpSafe = rt.CorpSafe,
            MinBuild = rt.MinBuild,
            RegistryKeys = ExtractRegistryKeys(rt),
            ApplyOps = ConvertOps(rt.ApplyOps),
            RemoveOps = ConvertOps(rt.RemoveOps),
            DetectOps = ConvertOps(rt.DetectOps),
            PackSource = packName,
        };
    }

    private static IReadOnlyList<string> ExtractRegistryKeys(RawTweak rt)
    {
        var keys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var op in (rt.ApplyOps ?? []).Concat(rt.RemoveOps ?? []).Concat(rt.DetectOps ?? []))
        {
            if (!string.IsNullOrWhiteSpace(op.Path))
                keys.Add(op.Path);
        }
        return [.. keys];
    }

    private static IReadOnlyList<RegOp> ConvertOps(List<RawRegOp>? ops)
    {
        if (ops is null || ops.Count == 0)
            return [];
        return ops.Select(ConvertOp).ToList();
    }

    private static RegOp ConvertOp(RawRegOp raw)
    {
        return raw.Kind?.ToLowerInvariant() switch
        {
            "setdword" => RegOp.SetDword(raw.Path ?? "", raw.Name ?? "", raw.DwordValue ?? 0),
            "setstring" => RegOp.SetString(raw.Path ?? "", raw.Name ?? "", raw.StringValue ?? ""),
            "setexpandstring" => RegOp.SetExpandString(raw.Path ?? "", raw.Name ?? "", raw.StringValue ?? ""),
            "setqword" => RegOp.SetQword(raw.Path ?? "", raw.Name ?? "", raw.QwordValue ?? 0),
            "setbinary" => RegOp.SetBinary(raw.Path ?? "", raw.Name ?? "", Convert.FromBase64String(raw.BinaryValue ?? "")),
            "setmultisz" => RegOp.SetMultiSz(raw.Path ?? "", raw.Name ?? "", [.. (raw.MultiSzValue ?? [])]),
            "deletevalue" => RegOp.DeleteValue(raw.Path ?? "", raw.Name ?? ""),
            "deletetree" => RegOp.DeleteTree(raw.Path ?? ""),
            "checkdword" => RegOp.CheckDword(raw.Path ?? "", raw.Name ?? "", raw.DwordValue ?? 0),
            "checkstring" => RegOp.CheckString(raw.Path ?? "", raw.Name ?? "", raw.StringValue ?? ""),
            "checkmissing" => RegOp.CheckMissing(raw.Path ?? "", raw.Name ?? ""),
            "checkkeymissing" => RegOp.CheckKeyMissing(raw.Path ?? ""),
            _ => throw new InvalidOperationException($"Unknown RegOp kind: '{raw.Kind}'."),
        };
    }

    // ── Raw JSON models ─────────────────────────────────────────────────

    internal sealed class RawPack
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("displayName")]
        public string? DisplayName { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("author")]
        public string? Author { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("minRegiLatticeVersion")]
        public string? MinRegiLatticeVersion { get; set; }

        [JsonPropertyName("minWindowsBuild")]
        public int MinWindowsBuild { get; set; }

        [JsonPropertyName("categories")]
        public List<string>? Categories { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("tweaks")]
        public List<RawTweak>? Tweaks { get; set; }
    }

    internal sealed class RawTweak
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("label")]
        public string? Label { get; set; }

        [JsonPropertyName("category")]
        public string? Category { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        [JsonPropertyName("needsAdmin")]
        public bool NeedsAdmin { get; set; } = true;

        [JsonPropertyName("corpSafe")]
        public bool CorpSafe { get; set; }

        [JsonPropertyName("minBuild")]
        public int MinBuild { get; set; }

        [JsonPropertyName("applyOps")]
        public List<RawRegOp>? ApplyOps { get; set; }

        [JsonPropertyName("removeOps")]
        public List<RawRegOp>? RemoveOps { get; set; }

        [JsonPropertyName("detectOps")]
        public List<RawRegOp>? DetectOps { get; set; }
    }

    internal sealed class RawRegOp
    {
        [JsonPropertyName("kind")]
        public string? Kind { get; set; }

        [JsonPropertyName("path")]
        public string? Path { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("dwordValue")]
        public int? DwordValue { get; set; }

        [JsonPropertyName("stringValue")]
        public string? StringValue { get; set; }

        [JsonPropertyName("qwordValue")]
        public long? QwordValue { get; set; }

        [JsonPropertyName("binaryValue")]
        public string? BinaryValue { get; set; }

        [JsonPropertyName("multiSzValue")]
        public List<string>? MultiSzValue { get; set; }
    }
}
