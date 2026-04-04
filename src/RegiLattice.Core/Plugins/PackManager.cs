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
        "RegiLattice",
        "packs"
    );

    private static readonly string s_indexUrl = "https://raw.githubusercontent.com/RajwanYair/regilattice-marketplace/main/index.json";

    // NOTE: Do NOT call System.Net.WebRequest.GetSystemWebProxy() here eagerly.
    // On Intel/Intune corporate machines, eager WPAD/proxy discovery via that API
    // blocks for 20-30+ seconds during class initialisation (first `new PackManager()`
    // call), hanging the test runner and CLI.  UseProxy=true with Proxy=null lets
    // HttpClientHandler resolve the system proxy lazily, per-request.
    private static readonly HttpClient s_http = new(new System.Net.Http.HttpClientHandler { UseDefaultCredentials = true, UseProxy = true })
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
        try
        {
            var json = await s_http.GetStringAsync(s_indexUrl, ct);
            _cachedIndex = PackIndex.FromJson(json) ?? new PackIndex();
        }
        catch (System.Net.Http.HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Marketplace repo not yet available — return empty index.
            _cachedIndex = new PackIndex();
        }
        return _cachedIndex!;
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
        return index
            .Packs.Where(p =>
                p.Name.Contains(lower, StringComparison.OrdinalIgnoreCase)
                || p.DisplayName.Contains(lower, StringComparison.OrdinalIgnoreCase)
                || p.Description.Contains(lower, StringComparison.OrdinalIgnoreCase)
                || p.Tags.Any(t => t.Contains(lower, StringComparison.OrdinalIgnoreCase))
            )
            .ToList();
    }

    // ── Install ─────────────────────────────────────────────────────────

    /// <summary>
    /// Download and install a pack by name from the marketplace index.
    /// Returns the loaded TweakDefs ready for registration.
    /// </summary>
    public async Task<(PackDef Pack, IReadOnlyList<TweakDef> Tweaks)> InstallPackAsync(string name, CancellationToken ct = default)
    {
        var index = await GetIndexAsync(ct);
        var entry =
            index.Packs.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
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
            if (!File.Exists(metaPath))
                continue;

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
    public async Task<IReadOnlyList<(PackDef Installed, PackDef Available)>> CheckUpdatesAsync(CancellationToken ct = default)
    {
        var index = await GetIndexAsync(ct);
        var installed = InstalledPacks();
        var updates = new List<(PackDef, PackDef)>();

        foreach (var local in installed)
        {
            var remote = index.Packs.FirstOrDefault(p => p.Name.Equals(local.Name, StringComparison.OrdinalIgnoreCase));
            if (remote is not null && CompareVersions(remote.Version, local.Version) > 0)
                updates.Add((local, remote));
        }
        return updates;
    }

    /// <summary>Update a pack to the latest version from the index.</summary>
    public async Task<(PackDef Pack, IReadOnlyList<TweakDef> Tweaks)> UpdatePackAsync(string name, CancellationToken ct = default)
    {
        UninstallPack(name);
        return await InstallPackAsync(name, ct);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    /// <summary>
    /// Install a pack from a direct URL pointing to a pack JSON file.
    /// Downloads, validates (via PackLoader), and installs as a local pack.
    /// </summary>
    public async Task<(PackDef Pack, IReadOnlyList<TweakDef> Tweaks)> InstallFromUrlAsync(string url, CancellationToken ct = default)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri) || (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
            throw new ArgumentException($"Invalid or non-HTTP(S) URL: {url}", nameof(url));

        var json = await s_http.GetStringAsync(uri, ct);
        var (pack, tweaks) = PackLoader.LoadFromJson(json);

        var packDir = Path.Combine(s_packsDir, pack.Name);
        Directory.CreateDirectory(packDir);
        await File.WriteAllTextAsync(Path.Combine(packDir, "pack.json"), json, ct);

        var meta = JsonSerializer.Serialize(pack, s_jsonOptions);
        await File.WriteAllTextAsync(Path.Combine(packDir, "meta.json"), meta, ct);

        return (pack, tweaks);
    }

    /// <summary>
    /// Detect registry key conflicts between installed packs.
    /// Returns a list of (RegistryPath, PackName[]) pairs where two or more packs
    /// modify the same registry key/value combination.
    /// </summary>
    public IReadOnlyList<PackConflict> DetectConflicts()
    {
        var installed = InstalledPacks();
        // path\0name → list of pack names that touch it
        var keyOwners = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);

        foreach (var pack in installed)
        {
            var tweaks = LoadInstalledPack(pack.Name);
            if (tweaks is null)
                continue;

            foreach (var tweak in tweaks)
            {
                foreach (var op in tweak.ApplyOps)
                {
                    string key = $"{op.Path}\0{op.Name}";
                    if (!keyOwners.TryGetValue(key, out var owners))
                        keyOwners[key] = owners = new List<string>();
                    if (!owners.Contains(pack.Name, StringComparer.OrdinalIgnoreCase))
                        owners.Add(pack.Name);
                }
            }
        }

        return keyOwners
            .Where(kv => kv.Value.Count > 1)
            .Select(kv =>
            {
                var parts = kv.Key.Split('\0', 2);
                return new PackConflict(parts[0], parts.Length > 1 ? parts[1] : "", kv.Value);
            })
            .ToList();
    }

    /// <summary>Simple semver comparison (major.minor.patch).</summary>
    internal static int CompareVersions(string a, string b)
    {
        var aParts = a.Split('.').Select(s => int.TryParse(s, out var v) ? v : 0).ToArray();
        var bParts = b.Split('.').Select(s => int.TryParse(s, out var v) ? v : 0).ToArray();

        for (int i = 0; i < Math.Max(aParts.Length, bParts.Length); i++)
        {
            var av = i < aParts.Length ? aParts[i] : 0;
            var bv = i < bParts.Length ? bParts[i] : 0;
            if (av != bv)
                return av.CompareTo(bv);
        }
        return 0;
    }
}
