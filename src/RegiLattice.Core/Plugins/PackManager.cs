// RegiLattice.Core — Plugins/PackManager.cs
// Manages installation, removal, listing, and updating of Tweak Packs.

using System.Net.Http;
using System.Text.Json;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Manages Tweak Packs: install from remote URL, uninstall, list installed,
/// fetch index from GitHub, and update packs.
/// </summary>
public sealed class PackManager
{
    private static readonly string s_packsDir = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "RegiLattice", "packs");

    private static readonly string s_indexUrl =
        "https://raw.githubusercontent.com/RajwanYair/regilattice-marketplace/main/index.json";

    private static readonly HttpClient s_http = new()
    {
        Timeout = TimeSpan.FromSeconds(30),
    };

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private PackIndex? _cachedIndex;

    public PackManager()
    {
        Directory.CreateDirectory(s_packsDir);
    }

    // ── Index ───────────────────────────────────────────────────────────

    /// <summary>Fetch the marketplace index from GitHub.</summary>
    public async Task<PackIndex> FetchIndexAsync(CancellationToken ct = default)
    {
        var json = await s_http.GetStringAsync(s_indexUrl, ct);
        _cachedIndex = PackIndex.FromJson(json) ?? new PackIndex();
        return _cachedIndex;
    }

    /// <summary>Return the cached index, or fetch if not yet loaded.</summary>
    public async Task<PackIndex> GetIndexAsync(CancellationToken ct = default)
    {
        return _cachedIndex ?? await FetchIndexAsync(ct);
    }

    /// <summary>Search packs in the index by name, tag, or description.</summary>
    public async Task<IReadOnlyList<PackDef>> SearchPacksAsync(string query, CancellationToken ct = default)
    {
        var index = await GetIndexAsync(ct);
        var lower = query.ToLowerInvariant();
        return index.Packs
            .Where(p =>
                p.Name.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                p.DisplayName.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                p.Tags.Any(t => t.Contains(lower, StringComparison.OrdinalIgnoreCase)))
            .ToList();
    }

    // ── Install ─────────────────────────────────────────────────────────

    /// <summary>
    /// Download and install a pack by name from the marketplace index.
    /// Returns the loaded TweakDefs ready for registration.
    /// </summary>
    public async Task<(PackDef Pack, IReadOnlyList<TweakDef> Tweaks)> InstallPackAsync(
        string name, CancellationToken ct = default)
    {
        var index = await GetIndexAsync(ct);
        var entry = index.Packs.FirstOrDefault(p =>
            p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            ?? throw new ArgumentException($"Pack '{name}' not found in marketplace index.");

        if (string.IsNullOrWhiteSpace(entry.DownloadUrl))
            throw new InvalidOperationException($"Pack '{name}' has no download URL.");

        var json = await s_http.GetStringAsync(entry.DownloadUrl, ct);
        var (pack, tweaks) = PackLoader.LoadFromJson(json, entry.Sha256);

        // Write to local packs directory
        var packDir = Path.Combine(s_packsDir, pack.Name);
        Directory.CreateDirectory(packDir);
        await File.WriteAllTextAsync(Path.Combine(packDir, "pack.json"), json, ct);

        // Write metadata for quick listing
        var meta = JsonSerializer.Serialize(pack, s_jsonOptions);
        await File.WriteAllTextAsync(Path.Combine(packDir, "meta.json"), meta, ct);

        return (pack, tweaks);
    }

    /// <summary>Install a pack from a local JSON file path.</summary>
    public (PackDef Pack, IReadOnlyList<TweakDef> Tweaks) InstallFromFile(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var (pack, tweaks) = PackLoader.LoadFromJson(json);

        var packDir = Path.Combine(s_packsDir, pack.Name);
        Directory.CreateDirectory(packDir);
        File.WriteAllText(Path.Combine(packDir, "pack.json"), json);

        var meta = JsonSerializer.Serialize(pack, s_jsonOptions);
        File.WriteAllText(Path.Combine(packDir, "meta.json"), meta);

        return (pack, tweaks);
    }

    // ── Uninstall ───────────────────────────────────────────────────────

    /// <summary>Remove an installed pack by name.</summary>
    public bool UninstallPack(string name)
    {
        var packDir = Path.Combine(s_packsDir, name);
        if (!Directory.Exists(packDir))
            return false;

        Directory.Delete(packDir, recursive: true);
        return true;
    }

    // ── List installed ──────────────────────────────────────────────────

    /// <summary>List all locally installed packs.</summary>
    public IReadOnlyList<PackDef> InstalledPacks()
    {
        if (!Directory.Exists(s_packsDir))
            return [];

        var packs = new List<PackDef>();
        foreach (var dir in Directory.GetDirectories(s_packsDir))
        {
            var metaPath = Path.Combine(dir, "meta.json");
            if (!File.Exists(metaPath)) continue;

            try
            {
                var json = File.ReadAllText(metaPath);
                var pack = JsonSerializer.Deserialize<PackDef>(json, s_jsonOptions);
                if (pack is not null)
                    packs.Add(pack);
            }
            catch
            {
                // Skip corrupt pack metadata
            }
        }
        return packs;
    }

    /// <summary>Load tweaks from an installed pack by name.</summary>
    public IReadOnlyList<TweakDef>? LoadInstalledPack(string name)
    {
        var packPath = Path.Combine(s_packsDir, name, "pack.json");
        if (!File.Exists(packPath))
            return null;

        var json = File.ReadAllText(packPath);
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        return tweaks;
    }

    /// <summary>Load all installed packs and return their tweaks.</summary>
    public IReadOnlyList<TweakDef> LoadAllInstalledTweaks()
    {
        var allTweaks = new List<TweakDef>();
        foreach (var pack in InstalledPacks())
        {
            var tweaks = LoadInstalledPack(pack.Name);
            if (tweaks is not null)
                allTweaks.AddRange(tweaks);
        }
        return allTweaks;
    }

    // ── Update ──────────────────────────────────────────────────────────

    /// <summary>Check which installed packs have updates available.</summary>
    public async Task<IReadOnlyList<(PackDef Installed, PackDef Available)>> CheckUpdatesAsync(
        CancellationToken ct = default)
    {
        var index = await GetIndexAsync(ct);
        var installed = InstalledPacks();
        var updates = new List<(PackDef, PackDef)>();

        foreach (var local in installed)
        {
            var remote = index.Packs.FirstOrDefault(p =>
                p.Name.Equals(local.Name, StringComparison.OrdinalIgnoreCase));
            if (remote is not null && CompareVersions(remote.Version, local.Version) > 0)
                updates.Add((local, remote));
        }
        return updates;
    }

    /// <summary>Update a pack to the latest version from the index.</summary>
    public async Task<(PackDef Pack, IReadOnlyList<TweakDef> Tweaks)> UpdatePackAsync(
        string name, CancellationToken ct = default)
    {
        UninstallPack(name);
        return await InstallPackAsync(name, ct);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    /// <summary>Simple semver comparison (major.minor.patch).</summary>
    internal static int CompareVersions(string a, string b)
    {
        var aParts = a.Split('.').Select(s => int.TryParse(s, out var v) ? v : 0).ToArray();
        var bParts = b.Split('.').Select(s => int.TryParse(s, out var v) ? v : 0).ToArray();

        for (int i = 0; i < Math.Max(aParts.Length, bParts.Length); i++)
        {
            var av = i < aParts.Length ? aParts[i] : 0;
            var bv = i < bParts.Length ? bParts[i] : 0;
            if (av != bv) return av.CompareTo(bv);
        }
        return 0;
    }
}
