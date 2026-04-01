// RegiLattice.Core — TweakEngine.cs
// Central engine: manages all tweaks, profiles, search, batch operations, snapshots.
// Replaces Python tweaks/__init__.py entirely.

using System.Collections.Concurrent;
using System.Collections.Frozen;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.Json;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core;

/// <summary>
/// The tweak engine: loads tweak definitions, executes apply/remove/detect,
/// manages profiles, search, snapshots, and batch operations.
/// </summary>
public sealed class TweakEngine
{
    private readonly RegistrySession _session;
    private readonly List<TweakDef> _allTweaks = new(10_000);
    private readonly Dictionary<string, TweakDef> _tweakById = new(10_000, StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<TweakDef>> _tweaksByCat = new(64, StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<TweakDef>> _tweaksByTag = new(256, StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<TweakScope, List<TweakDef>> _tweaksByScope = new(4);
    private readonly ConcurrentDictionary<string, TweakScope> _scopeCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(string Lower, TweakDef Tweak)> _searchPairs = new(10_000);
    private readonly Dictionary<string, string> _tweakSearchText = new(10_000, StringComparer.OrdinalIgnoreCase);

    // Cached post-registration data (populated by Freeze())
    private string[]? _cachedCategories;
    private Dictionary<string, int>? _cachedCategoryCounts;
    private Dictionary<TweakScope, int>? _cachedScopeCounts;
    private FrozenDictionary<string, TweakDef>? _frozenById;
    private IReadOnlyDictionary<string, IReadOnlyList<TweakDef>>? _cachedByCat;
    private bool _frozen;

    public RegistrySession Session => _session;
    public int TweakCount => _allTweaks.Count;

    /// <summary>
    /// Elapsed milliseconds of the last <see cref="RegisterBuiltins"/> call.
    /// Zero until <see cref="RegisterBuiltins"/> has been called at least once.
    /// </summary>
    public long LastRegistrationMs { get; private set; }

    public TweakEngine(RegistrySession? session = null)
    {
        _session = session ?? new RegistrySession();
    }

    // ── Registration ────────────────────────────────────────────────────

    /// <summary>Register a batch of tweaks from a category module.</summary>
    public void Register(IEnumerable<TweakDef> tweaks)
    {
        foreach (var td in tweaks)
        {
            // Skip stub tweaks that have no operations — they would silently no-op.
            if (!td.HasOperations)
                continue;

            if (_tweakById.ContainsKey(td.Id))
                throw new InvalidOperationException($"Duplicate tweak ID: {td.Id}");

            _allTweaks.Add(td);
            _tweakById[td.Id] = td;

            if (!_tweaksByCat.TryGetValue(td.Category, out var catList))
            {
                catList = [];
                _tweaksByCat[td.Category] = catList;
            }
            catList.Add(td);

            foreach (var tag in td.Tags)
            {
                if (!_tweaksByTag.TryGetValue(tag, out var tagList))
                {
                    tagList = [];
                    _tweaksByTag[tag] = tagList;
                }
                tagList.Add(td);
            }

            var scope = td.Scope;
            _scopeCache[td.Id] = scope;
            if (!_tweaksByScope.TryGetValue(scope, out var scopeList))
            {
                scopeList = [];
                _tweaksByScope[scope] = scopeList;
            }
            scopeList.Add(td);

            // Build search index once, reused by Search() and Filter(query: ...).
            // Use a shared StringBuilder to avoid per-tweak intermediate string allocations.
            var searchText = BuildSearchText(td);
            _searchPairs.Add((searchText, td));
            _tweakSearchText[td.Id] = searchText;
        }
    }

    /// <summary>Register all built-in tweaks from the Tweaks namespace via reflection.</summary>
    public void RegisterBuiltins()
    {
        var sw = Stopwatch.StartNew();

        // Discover all internal static classes in RegiLattice.Core.Tweaks that expose
        // a static "Tweaks" property returning IReadOnlyList<TweakDef>.
        var tweaksNs = typeof(Tweaks.Accessibility).Namespace!;
        var bindingFlags = System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public;

        foreach (var type in typeof(Tweaks.Accessibility).Assembly.GetTypes())
        {
            if (type.Namespace != tweaksNs || !type.IsClass || !type.IsAbstract || !type.IsSealed || type.IsNested)
                continue; // skip non-static classes and nested helper classes

            var prop = type.GetProperty("Tweaks", bindingFlags);
            if (prop?.GetValue(null) is IReadOnlyList<TweakDef> tweaks)
                Register(tweaks);
        }

        Freeze();
        sw.Stop();
        LastRegistrationMs = sw.ElapsedMilliseconds;
    }

    // Thread-local StringBuilder avoids per-tweak allocation during Register().
    [ThreadStatic]
    private static System.Text.StringBuilder? _searchSb;

    /// <summary>
    /// Builds the lowercased search text for a tweak using a reusable StringBuilder.
    /// Eliminates ~36K intermediate string allocations during RegisterBuiltins().
    /// </summary>
    private static string BuildSearchText(TweakDef td)
    {
        var sb = _searchSb ??= new System.Text.StringBuilder(512);
        sb.Clear();
        sb.Append(td.Id);
        sb.Append(' ');
        sb.Append(td.Label);
        sb.Append(' ');
        sb.Append(td.Category);
        sb.Append(' ');
        sb.Append(td.Description);
        sb.Append(' ');
        sb.Append(td.GetExpectedResult());
        foreach (var tag in td.Tags)
        {
            sb.Append(' ');
            sb.Append(tag);
        }

        // Lower-case in-place using string.Create to avoid a second allocation.
        return string.Create(
            sb.Length,
            sb,
            static (span, src) =>
            {
                src.CopyTo(0, span, src.Length);
                for (int i = 0; i < span.Length; i++)
                    span[i] = char.ToLowerInvariant(span[i]);
            }
        );
    }

    /// <summary>
    /// Freeze internal data structures for optimal read performance.
    /// Called automatically after RegisterBuiltins(). Can also be called
    /// manually after Register() if building a custom engine.
    /// </summary>
    public void Freeze()
    {
        _frozenById = _tweakById.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
        _cachedCategories = [.. _tweaksByCat.Keys.Order()];
        _cachedCategoryCounts = _tweaksByCat.ToDictionary(kv => kv.Key, kv => kv.Value.Count);
        _cachedScopeCounts = _tweaksByScope.ToDictionary(kv => kv.Key, kv => kv.Value.Count);
        _cachedByCat = _tweaksByCat.ToDictionary(kv => kv.Key, kv => (IReadOnlyList<TweakDef>)kv.Value);
        _frozen = true;
    }

    /// <summary>Register tweaks from an installed Tweak Pack (plugin).</summary>
    public void RegisterPack(IReadOnlyList<TweakDef> packTweaks)
    {
        Register(packTweaks);
    }

    /// <summary>Register all installed packs from %LOCALAPPDATA%\RegiLattice\packs\.</summary>
    public int RegisterInstalledPacks()
    {
        var mgr = new PackManager();
        var tweaks = mgr.LoadAllInstalledTweaks();
        if (tweaks.Count == 0)
            return 0;
        Register(tweaks);
        return tweaks.Count;
    }

    // ── Lookup & enumeration ────────────────────────────────────────────

    public IReadOnlyList<TweakDef> AllTweaks() => _allTweaks;

    public TweakDef? GetTweak(string id) => _frozen ? _frozenById!.GetValueOrDefault(id) : _tweakById.GetValueOrDefault(id);

    public IReadOnlyList<string> Categories() => _cachedCategories ?? [.. _tweaksByCat.Keys.Order()];

    public IReadOnlyDictionary<string, IReadOnlyList<TweakDef>> TweaksByCategory() =>
        _cachedByCat ?? _tweaksByCat.ToDictionary(kv => kv.Key, kv => (IReadOnlyList<TweakDef>)kv.Value);

    public int CategoryCount => _tweaksByCat.Count;

    public IReadOnlyList<TweakDef> TweaksByIds(IEnumerable<string> ids) =>
        ids.Select(id => _tweakById.GetValueOrDefault(id)).Where(t => t is not null).Cast<TweakDef>().ToList();

    public IReadOnlyList<TweakDef> TweaksByTag(string tag) => _tweaksByTag.TryGetValue(tag, out var list) ? list : [];

    public IReadOnlyList<TweakDef> TweaksByScope(TweakScope scope) => _tweaksByScope.GetValueOrDefault(scope) ?? [];

    public TweakScope GetScope(TweakDef td) => _scopeCache.GetOrAdd(td.Id, _ => td.Scope);

    // ── Search ──────────────────────────────────────────────────────────

    // NLP synonym map: user-friendly term → canonical indexed term(s).
    // Expands informal queries so users can type naturally (e.g. "speed" finds "performance" tweaks).
    private static readonly IReadOnlyDictionary<string, string[]> _synonyms = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
    {
        // Performance synonyms
        ["fast"] = ["performance", "perf", "speed"],
        ["faster"] = ["performance", "perf", "speed"],
        ["speed"] = ["performance", "perf"],
        ["slow"] = ["performance", "perf", "optimize"],
        ["boost"] = ["performance", "gaming", "gpu"],
        ["optimize"] = ["performance", "perf", "ssd", "memory"],
        ["lag"] = ["performance", "gaming", "latency"],
        ["latency"] = ["network", "gaming", "performance"],
        ["ram"] = ["memory", "mem", "virtual"],
        ["memory"] = ["memory", "mem", "ram"],
        ["cpu"] = ["performance", "processor", "perf"],
        ["disk"] = ["storage", "ssd", "filesystem", "file system"],
        // Privacy synonyms
        ["spy"] = ["telemetry", "privacy", "tracking"],
        ["spying"] = ["telemetry", "privacy", "tracking"],
        ["tracking"] = ["telemetry", "privacy", "tracking"],
        ["telemetry"] = ["telemetry", "diagnostics"],
        ["private"] = ["privacy", "priv", "telemetry"],
        ["data"] = ["privacy", "telemetry", "cloud"],
        ["microsoft"] = ["telemetry", "cortana", "onedrive", "edge"],
        // Bloatware / cleanup synonyms
        ["bloat"] = ["debloat", "uninstall", "remove"],
        ["bloatware"] = ["debloat", "uninstall", "remove"],
        ["junk"] = ["debloat", "cleanup", "temp"],
        ["clean"] = ["cleanup", "debloat", "temp", "disk"],
        ["cleaner"] = ["cleanup", "temp", "disk"],
        ["ads"] = ["advertising", "debloat", "tips", "notification"],
        ["advert"] = ["advertising", "debloat", "tips"],
        // Network synonyms
        ["wifi"] = ["network", "wireless", "wi-fi"],
        ["internet"] = ["network", "dns", "proxy"],
        ["vpn"] = ["proxy", "vpn", "network"],
        ["bandwidth"] = ["network", "netopt", "optimization"],
        ["connection"] = ["network", "dns", "netopt"],
        ["firewall"] = ["firewall", "fw", "security"],
        ["dns"] = ["dns", "network", "doh"],
        // Security synonyms
        ["safe"] = ["security", "hardening", "encryption"],
        ["secure"] = ["security", "hardening", "sec"],
        ["harden"] = ["hardening", "security", "gpo"],
        ["antivirus"] = ["defender", "security", "malware"],
        ["malware"] = ["defender", "security", "firewall"],
        ["uac"] = ["user account", "uac", "elevation"],
        // Gaming synonyms
        ["game"] = ["gaming", "gpu", "performance", "latency"],
        ["games"] = ["gaming", "gpu", "game mode"],
        ["fps"] = ["gaming", "gpu", "performance", "game"],
        ["gaming"] = ["gaming", "gpu", "game bar"],
        // Startup / boot synonyms
        ["startup"] = ["startup", "boot", "autostart"],
        ["boot"] = ["boot", "startup", "bcd"],
        ["autostart"] = ["startup", "boot"],
        ["launch"] = ["startup", "boot", "autostart"],
        // Power synonyms
        ["battery"] = ["power", "battery", "energy"],
        ["power"] = ["power", "energy", "sleep"],
        ["sleep"] = ["sleep", "hibernate", "power"],
        ["hibernate"] = ["hibernate", "sleep", "power"],
        // UI / appearance synonyms
        ["theme"] = ["appearance", "dark", "light", "visual"],
        ["dark"] = ["dark mode", "appearance", "theme"],
        ["taskbar"] = ["taskbar", "tb", "shell"],
        ["notification"] = ["notification", "notif", "toast"],
        ["animation"] = ["animation", "visual", "performance"],
        // Explorer synonyms
        ["explorer"] = ["explorer", "shell", "context menu", "file"],
        ["file"] = ["explorer", "filesystem", "file system", "storage"],
        ["folder"] = ["explorer", "filesystem", "shell"],
        ["search"] = ["search", "indexing", "cortana"],
        // Update synonyms
        ["update"] = ["windows update", "wu", "patch"],
        ["patch"] = ["windows update", "wu", "update"],
        // Input synonyms
        ["mouse"] = ["input", "mouse", "pointer"],
        ["keyboard"] = ["input", "keyboard", "hotkey"],
        ["touchpad"] = ["input", "touch", "touchpad"],
        // Service synonyms
        ["service"] = ["services", "svc", "background"],
        ["background"] = ["services", "svc", "startup"],
        // Misc
        ["clipboard"] = ["clipboard", "clip", "copy paste"],
        ["fonts"] = ["fonts", "font", "text"],
        ["printer"] = ["printing", "print", "spooler"],
    };

    /// <summary>
    /// Expands a raw query token using the NLP synonym map.
    /// Returns all canonical terms to search for (including the original token).
    /// </summary>
    private static IReadOnlyList<string> ExpandSynonyms(string token)
    {
        if (_synonyms.TryGetValue(token, out var synonyms))
        {
            var expanded = new List<string> { token };
            expanded.AddRange(synonyms);
            return expanded;
        }
        return [token];
    }

    public IReadOnlyList<TweakDef> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return _allTweaks;
        var lower = query.ToLowerInvariant();
        var tokens = lower.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        // Expand each token through the synonym map, then match tweaks
        // where ALL expanded-token groups are satisfied (AND logic between groups).
        var expandedGroups = tokens.Select(ExpandSynonyms).ToList();

        return _searchPairs.Where(p => expandedGroups.All(group => group.Any(term => p.Lower.Contains(term)))).Select(p => p.Tweak).ToList();
    }

    public IReadOnlyList<TweakDef> Filter(
        bool? corpSafe = null,
        bool? needsAdmin = null,
        TweakScope? scope = null,
        string? category = null,
        int? minBuild = null,
        string? query = null
    )
    {
        // Use _tweaksByCat as the starting set if a category is specified — avoids a full O(n) scan.
        IEnumerable<TweakDef> results = category is not null ? (_tweaksByCat.TryGetValue(category, out var catList) ? catList : []) : _allTweaks;

        if (corpSafe.HasValue)
            results = results.Where(t => t.CorpSafe == corpSafe.Value);
        if (needsAdmin.HasValue)
            results = results.Where(t => t.NeedsAdmin == needsAdmin.Value);
        if (scope.HasValue)
            results = results.Where(t => t.Scope == scope.Value);
        if (minBuild.HasValue)
            results = results.Where(t => t.MinBuild <= minBuild.Value);
        if (query is not null)
        {
            var q = query.ToLowerInvariant();
            results = results.Where(t => _tweakSearchText.TryGetValue(t.Id, out var text) && text.Contains(q));
        }
        return results.ToList();
    }

    // ── Status detection ────────────────────────────────────────────────

    public TweakResult DetectStatus(TweakDef td)
    {
        try
        {
            if (td.DetectAction is not null)
                return td.DetectAction() ? TweakResult.Applied : TweakResult.NotApplied;
            if (td.DetectOps.Count > 0)
                return _session.Evaluate(td.DetectOps) ? TweakResult.Applied : TweakResult.NotApplied;
            return TweakResult.Unknown;
        }
        catch
        {
            return TweakResult.Error;
        }
    }

    public Dictionary<string, TweakResult> StatusMap(bool parallel = false, IEnumerable<string>? ids = null)
    {
        var tweaks = ids is not null ? TweaksByIds(ids) : _allTweaks;
        var result = new ConcurrentDictionary<string, TweakResult>();

        if (parallel)
        {
            Parallel.ForEach(tweaks, td => result[td.Id] = DetectStatus(td));
        }
        else
        {
            foreach (var td in tweaks)
                result[td.Id] = DetectStatus(td);
        }
        return new Dictionary<string, TweakResult>(result);
    }

    // ── Hardware applicability ─────────────────────────────────────────

    /// <summary>
    /// Returns true if the tweak is applicable to the current machine.
    /// Checks the custom <see cref="TweakDef.IsApplicable"/> predicate first,
    /// then auto-detects from category and tags.
    /// </summary>
    public static bool IsApplicableOnHardware(TweakDef td)
    {
        // Custom predicate takes priority
        if (td.IsApplicable is not null)
            return td.IsApplicable();

        // Auto-detect from category
        return td.Category switch
        {
            "WSL" => HardwareInfo.HasWslInstalled(),
            "Virtualization" => HardwareInfo.HasHyperVAvailable(),
            "Chrome" => HardwareInfo.IsChromeInstalled(),
            "Firefox" => HardwareInfo.IsFirefoxInstalled(),
            "Edge" => HardwareInfo.IsEdgeInstalled(),
            "Java" => HardwareInfo.IsJavaInstalled(),
            "Adobe" => HardwareInfo.IsAdobeInstalled(),
            "LibreOffice" => HardwareInfo.IsLibreOfficeInstalled(),
            "Office" => HardwareInfo.IsOfficeInstalled(),
            "M365 Copilot" => HardwareInfo.IsOfficeInstalled(),
            "RealVNC" => HardwareInfo.IsRealVncInstalled(),
            "VS Code" => HardwareInfo.IsVsCodeInstalled(),
            "Scoop Tools" => HardwareInfo.IsScoopInstalled(),
            _ => AutoDetectFromTags(td),
        };
    }

    private static bool AutoDetectFromTags(TweakDef td)
    {
        if (td.Tags.Contains("nvidia", StringComparer.OrdinalIgnoreCase))
            return HardwareInfo.HasNvidiaGpu();
        if (td.Tags.Contains("amd-gpu", StringComparer.OrdinalIgnoreCase))
            return HardwareInfo.HasAmdGpu();
        if (td.Tags.Contains("docker", StringComparer.OrdinalIgnoreCase))
            return HardwareInfo.IsDockerInstalled();
        if (td.Tags.Contains("laptop", StringComparer.OrdinalIgnoreCase))
            return HardwareInfo.HasBatteryPresent();

        return true;
    }

    // ── Apply / Remove ──────────────────────────────────────────────────

    public TweakResult Apply(TweakDef td, bool requireAdmin = true, bool forceCorp = false)
    {
        if (!forceCorp && !td.CorpSafe && CorporateGuard.IsCorporateNetwork())
            return TweakResult.SkippedCorp;
        if (td.MinBuild > 0 && td.MinBuild > WindowsBuild())
            return TweakResult.SkippedBuild;
        if (!IsApplicableOnHardware(td))
            return TweakResult.SkippedHw;

        try
        {
            if (td.ApplyAction is not null)
            {
                td.ApplyAction(requireAdmin);
            }
            else if (td.ApplyOps.Count > 0)
            {
                _session.Backup(td.RegistryKeys, td.Id);
                _session.Execute(td.ApplyOps);
            }
            return TweakResult.Applied;
        }
        catch (Exception ex)
        {
            _session.WriteLog($"ERROR applying {td.Id}: {ex.Message}");
            return TweakResult.Error;
        }
    }

    public TweakResult Remove(TweakDef td, bool requireAdmin = true, bool forceCorp = false)
    {
        if (!forceCorp && !td.CorpSafe && CorporateGuard.IsCorporateNetwork())
            return TweakResult.SkippedCorp;

        try
        {
            if (td.RemoveAction is not null)
            {
                td.RemoveAction(requireAdmin);
            }
            else if (td.RemoveOps.Count > 0)
            {
                _session.Backup(td.RegistryKeys, td.Id);
                _session.Execute(td.RemoveOps);
            }
            return TweakResult.NotApplied;
        }
        catch (Exception ex)
        {
            _session.WriteLog($"ERROR removing {td.Id}: {ex.Message}");
            return TweakResult.Error;
        }
    }

    /// <summary>
    /// Update a package-manager tweak (e.g. scoop update, pip upgrade).
    /// Returns <see cref="TweakResult.Applied"/> on success, or <see cref="TweakResult.Error"/> on failure.
    /// Falls back to <see cref="Apply"/> if no <see cref="TweakDef.UpdateAction"/> is defined.
    /// </summary>
    public TweakResult Update(TweakDef td, bool requireAdmin = true, bool forceCorp = false)
    {
        if (td.UpdateAction is null)
            return Apply(td, requireAdmin, forceCorp);

        if (!forceCorp && !td.CorpSafe && CorporateGuard.IsCorporateNetwork())
            return TweakResult.SkippedCorp;

        try
        {
            td.UpdateAction(requireAdmin);
            return TweakResult.Applied;
        }
        catch (Exception ex)
        {
            _session.WriteLog($"ERROR updating {td.Id}: {ex.Message}");
            return TweakResult.Error;
        }
    }

    public Dictionary<string, TweakResult> ApplyBatch(IEnumerable<TweakDef> tweaks, bool forceCorp = false, bool parallel = false)
    {
        var result = new ConcurrentDictionary<string, TweakResult>();
        if (parallel)
            Parallel.ForEach(tweaks, td => result[td.Id] = Apply(td, forceCorp: forceCorp));
        else
            foreach (var td in tweaks)
                result[td.Id] = Apply(td, forceCorp: forceCorp);
        return new Dictionary<string, TweakResult>(result);
    }

    public Dictionary<string, TweakResult> RemoveBatch(IEnumerable<TweakDef> tweaks, bool forceCorp = false, bool parallel = false)
    {
        var result = new ConcurrentDictionary<string, TweakResult>();
        if (parallel)
            Parallel.ForEach(tweaks, td => result[td.Id] = Remove(td, forceCorp: forceCorp));
        else
            foreach (var td in tweaks)
                result[td.Id] = Remove(td, forceCorp: forceCorp);
        return new Dictionary<string, TweakResult>(result);
    }

    // ── Profiles ────────────────────────────────────────────────────────

    public static IReadOnlyList<ProfileDef> Profiles { get; } = ProfileDefinitions.All;

    public static ProfileDef? GetProfile(string name) => Profiles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<TweakDef> TweaksForProfile(string name)
    {
        var profile = GetProfile(name);
        if (profile is null)
            return [];
        // Harvest from per-category buckets — avoids a full O(n) scan of _allTweaks.
        var cats = new HashSet<string>(profile.ApplyCategories, StringComparer.OrdinalIgnoreCase);
        var result = new List<TweakDef>();
        foreach (var cat in cats)
        {
            if (_tweaksByCat.TryGetValue(cat, out var bucket))
                result.AddRange(bucket);
        }
        return result;
    }

    public Dictionary<string, TweakResult> ApplyProfile(string name, bool forceCorp = false, bool parallel = false) =>
        ApplyBatch(TweaksForProfile(name), forceCorp, parallel);

    // ── Snapshots (delegated to SnapshotManager) ──────────────────────

    private SnapshotManager? _snapshotManager;
    private SnapshotManager Snapshots => _snapshotManager ??= new SnapshotManager(this);

    public void SaveSnapshot(string path) => Snapshots.Save(path);

    public Dictionary<string, string> LoadSnapshot(string path) => SnapshotManager.Load(path);

    public Dictionary<string, TweakResult> RestoreSnapshot(string path, bool forceCorp = false) => Snapshots.Restore(path, forceCorp);

    // ── Windows Build ───────────────────────────────────────────────────

    private static int? _windowsBuild;

    public static int WindowsBuild()
    {
        if (_windowsBuild.HasValue)
            return _windowsBuild.Value;
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var build = key?.GetValue("CurrentBuildNumber")?.ToString();
            _windowsBuild = int.TryParse(build, out var b) ? b : 0;
        }
        catch
        {
            _windowsBuild = 0;
        }
        return _windowsBuild.Value;
    }

    // ── Category statistics ─────────────────────────────────────────────

    public Dictionary<string, int> CategoryCounts() => _cachedCategoryCounts ?? _tweaksByCat.ToDictionary(kv => kv.Key, kv => kv.Value.Count);

    public Dictionary<TweakScope, int> ScopeCounts() => _cachedScopeCounts ?? _tweaksByScope.ToDictionary(kv => kv.Key, kv => kv.Value.Count);

    // ── Export for GUI ──────────────────────────────────────────────────

    public void ExportJson(string path, Dictionary<string, TweakResult>? cachedStatus = null)
    {
        var list = _allTweaks
            .Select(t => new
            {
                t.Id,
                t.Label,
                t.Category,
                status = (cachedStatus is not null && cachedStatus.TryGetValue(t.Id, out var s) ? s : DetectStatus(t)).ToString().ToLowerInvariant(),
                needs_admin = t.NeedsAdmin,
                corp_safe = t.CorpSafe,
                t.Tags,
                registry_keys = t.RegistryKeys,
                t.Description,
            })
            .ToList();
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    // ── Validation (delegated to TweakValidator) ──────────────────────

    public IReadOnlyList<string> ValidateTweaks() => TweakValidator.Validate(_allTweaks, GetTweak);

    public IReadOnlyList<string> DetectDuplicateRegistryOps() => TweakValidator.DetectDuplicateRegistryOps(_allTweaks);

    // ── Dependency resolution (delegated to DependencyResolver) ─────

    public IReadOnlyList<TweakDef> ResolveDependencies(string tweakId)
    {
        var td = GetTweak(tweakId) ?? throw new ArgumentException($"Unknown tweak: {tweakId}");
        return DependencyResolver.Resolve(td, GetTweak);
    }

    public IReadOnlyList<TweakDef> Dependents(string tweakId) => DependencyResolver.Dependents(tweakId, _allTweaks);

    // ── Batch operations with progress ──────────────────────────────────

    /// <summary>
    /// Apply a batch of tweaks with progress reporting.
    /// <paramref name="onProgress"/> is called after each tweak with (completed, total, id, result).
    /// </summary>
    public Dictionary<string, TweakResult> ApplyBatch(IEnumerable<TweakDef> tweaks, bool forceCorp, Action<int, int, string, TweakResult> onProgress)
    {
        var list = tweaks as IReadOnlyList<TweakDef> ?? tweaks.ToList();
        var result = new Dictionary<string, TweakResult>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            var td = list[i];
            var r = Apply(td, forceCorp: forceCorp);
            result[td.Id] = r;
            onProgress(i + 1, list.Count, td.Id, r);
        }
        return result;
    }

    /// <summary>
    /// Remove a batch of tweaks with progress reporting.
    /// </summary>
    public Dictionary<string, TweakResult> RemoveBatch(IEnumerable<TweakDef> tweaks, bool forceCorp, Action<int, int, string, TweakResult> onProgress)
    {
        var list = tweaks as IReadOnlyList<TweakDef> ?? tweaks.ToList();
        var result = new Dictionary<string, TweakResult>(list.Count);
        for (int i = 0; i < list.Count; i++)
        {
            var td = list[i];
            var r = Remove(td, forceCorp: forceCorp);
            result[td.Id] = r;
            onProgress(i + 1, list.Count, td.Id, r);
        }
        return result;
    }
}
