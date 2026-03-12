// RegiLattice.Core — TweakEngine.cs
// Central engine: manages all tweaks, profiles, search, batch operations, snapshots.
// Replaces Python tweaks/__init__.py entirely.

using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text.Json;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core;

/// <summary>
/// The tweak engine: loads tweak definitions, executes apply/remove/detect,
/// manages profiles, search, snapshots, and batch operations.
/// </summary>
public sealed class TweakEngine
{
    private readonly RegistrySession _session;
    private readonly List<TweakDef> _allTweaks = [];
    private readonly Dictionary<string, TweakDef> _tweakById = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<TweakDef>> _tweaksByCat = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, List<TweakDef>> _tweaksByScope = [];
    private readonly ConcurrentDictionary<string, TweakScope> _scopeCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly List<(string Lower, TweakDef Tweak)> _searchPairs = [];
    private readonly Dictionary<TweakDef, string> _tweakSearchText = [];

    public RegistrySession Session => _session;
    public int TweakCount => _allTweaks.Count;

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

            var scope = td.Scope;
            _scopeCache[td.Id] = scope;
            var scopeKey = scope.ToString().ToLowerInvariant();
            if (!_tweaksByScope.TryGetValue(scopeKey, out var scopeList))
            {
                scopeList = [];
                _tweaksByScope[scopeKey] = scopeList;
            }
            scopeList.Add(td);

            // Build search index
            var searchText = $"{td.Id} {td.Label} {td.Category} {td.Description} {string.Join(' ', td.Tags)}".ToLowerInvariant();
            _searchPairs.Add((searchText, td));
            _tweakSearchText[td] = searchText;
        }
    }

    /// <summary>Register all built-in tweaks from the Tweaks namespace.</summary>
    public void RegisterBuiltins()
    {
        // Each category module provides a static Tweaks property
        Register(Tweaks.Accessibility.Tweaks);
        Register(Tweaks.Adobe.Tweaks);
        Register(Tweaks.Audio.Tweaks);
        Register(Tweaks.Backup.Tweaks);
        Register(Tweaks.Bluetooth.Tweaks);
        Register(Tweaks.Boot.Tweaks);
        Register(Tweaks.Chrome.Tweaks);
        Register(Tweaks.Clipboard.Tweaks);
        Register(Tweaks.CloudStorage.Tweaks);
        Register(Tweaks.Communication.Tweaks);
        Register(Tweaks.ContextMenu.Tweaks);
        Register(Tweaks.Copilot.Tweaks);
        Register(Tweaks.Cortana.Tweaks);
        Register(Tweaks.CrashDiagnostics.Tweaks);
        Register(Tweaks.Defender.Tweaks);
        Register(Tweaks.DevDrive.Tweaks);
        Register(Tweaks.Display.Tweaks);
        Register(Tweaks.DnsNetworking.Tweaks);
        Register(Tweaks.Edge.Tweaks);
        Register(Tweaks.Encryption.Tweaks);
        Register(Tweaks.Explorer.Tweaks);
        Register(Tweaks.FileSystem.Tweaks);
        Register(Tweaks.Firewall.Tweaks);
        Register(Tweaks.Firefox.Tweaks);
        Register(Tweaks.Fonts.Tweaks);
        Register(Tweaks.Gaming.Tweaks);
        Register(Tweaks.Gpu.Tweaks);
        Register(Tweaks.IndexingSearch.Tweaks);
        Register(Tweaks.Input.Tweaks);
        Register(Tweaks.Java.Tweaks);
        Register(Tweaks.LibreOffice.Tweaks);
        Register(Tweaks.LockScreen.Tweaks);
        Register(Tweaks.Maintenance.Tweaks);
        Register(Tweaks.Ms365Copilot.Tweaks);
        Register(Tweaks.MsStore.Tweaks);
        Register(Tweaks.Multimedia.Tweaks);
        Register(Tweaks.Network.Tweaks);
        Register(Tweaks.NightLight.Tweaks);
        Register(Tweaks.Notifications.Tweaks);
        Register(Tweaks.Office.Tweaks);
        Register(Tweaks.OneDrive.Tweaks);
        Register(Tweaks.Performance.Tweaks);
        Register(Tweaks.PhoneLink.Tweaks);
        Register(Tweaks.PackageManagement.Tweaks);
        Register(Tweaks.Power.Tweaks);
        Register(Tweaks.Printing.Tweaks);
        Register(Tweaks.Privacy.Tweaks);
        Register(Tweaks.RealVnc.Tweaks);
        Register(Tweaks.Recovery.Tweaks);
        Register(Tweaks.RemoteDesktop.Tweaks);
        Register(Tweaks.ScheduledTasks.Tweaks);
        Register(Tweaks.ScoopTools.Tweaks);
        Register(Tweaks.Screensaver.Tweaks);
        Register(Tweaks.Services.Tweaks);
        Register(Tweaks.Shell.Tweaks);
        Register(Tweaks.SnapMultitasking.Tweaks);
        Register(Tweaks.Speech.Tweaks);
        Register(Tweaks.Startup.Tweaks);
        Register(Tweaks.Storage.Tweaks);
        Register(Tweaks.SystemTweaks.Tweaks);
        Register(Tweaks.Taskbar.Tweaks);
        Register(Tweaks.TelemetryAdvanced.Tweaks);
        Register(Tweaks.TouchPen.Tweaks);
        Register(Tweaks.UsbPeripherals.Tweaks);
        Register(Tweaks.Virtualization.Tweaks);
        Register(Tweaks.VsCode.Tweaks);
        Register(Tweaks.Widgets.Tweaks);
        Register(Tweaks.Win11.Tweaks);
        Register(Tweaks.WindowsTerminal.Tweaks);
        Register(Tweaks.WindowsUpdate.Tweaks);
        Register(Tweaks.Wsl.Tweaks);
    }

    // ── Lookup & enumeration ────────────────────────────────────────────

    public IReadOnlyList<TweakDef> AllTweaks() => _allTweaks;
    public TweakDef? GetTweak(string id) => _tweakById.GetValueOrDefault(id);
    public IReadOnlyList<string> Categories() => [.. _tweaksByCat.Keys.Order()];
    public IReadOnlyDictionary<string, List<TweakDef>> TweaksByCategory() => _tweaksByCat;
    public int CategoryCount => _tweaksByCat.Count;

    public IReadOnlyList<TweakDef> TweaksByIds(IEnumerable<string> ids)
        => ids.Select(id => _tweakById.GetValueOrDefault(id)).Where(t => t is not null).Cast<TweakDef>().ToList();

    public IReadOnlyList<TweakDef> TweaksByTag(string tag)
        => _allTweaks.Where(t => t.Tags.Contains(tag, StringComparer.OrdinalIgnoreCase)).ToList();

    public IReadOnlyList<TweakDef> TweaksByScope(TweakScope scope)
        => _tweaksByScope.GetValueOrDefault(scope.ToString().ToLowerInvariant()) ?? [];

    public TweakScope GetScope(TweakDef td) => _scopeCache.GetOrAdd(td.Id, _ => td.Scope);

    // ── Search ──────────────────────────────────────────────────────────

    public IReadOnlyList<TweakDef> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return _allTweaks;
        var lower = query.ToLowerInvariant();
        var tokens = lower.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return _searchPairs
            .Where(p => tokens.All(t => p.Lower.Contains(t)))
            .Select(p => p.Tweak)
            .ToList();
    }

    public IReadOnlyList<TweakDef> Filter(
        bool? corpSafe = null, bool? needsAdmin = null,
        TweakScope? scope = null, string? category = null,
        int? minBuild = null, string? query = null)
    {
        IEnumerable<TweakDef> results = _allTweaks;
        if (corpSafe.HasValue) results = results.Where(t => t.CorpSafe == corpSafe.Value);
        if (needsAdmin.HasValue) results = results.Where(t => t.NeedsAdmin == needsAdmin.Value);
        if (scope.HasValue) results = results.Where(t => t.Scope == scope.Value);
        if (category is not null) results = results.Where(t => t.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        if (minBuild.HasValue) results = results.Where(t => t.MinBuild <= minBuild.Value);
        if (query is not null) { var q = query.ToLowerInvariant(); results = results.Where(t => _tweakSearchText.TryGetValue(t, out var text) && text.Contains(q)); }
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
        catch { return TweakResult.Error; }
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
            foreach (var td in tweaks) result[td.Id] = Apply(td, forceCorp: forceCorp);
        return new Dictionary<string, TweakResult>(result);
    }

    public Dictionary<string, TweakResult> RemoveBatch(IEnumerable<TweakDef> tweaks, bool forceCorp = false, bool parallel = false)
    {
        var result = new ConcurrentDictionary<string, TweakResult>();
        if (parallel)
            Parallel.ForEach(tweaks, td => result[td.Id] = Remove(td, forceCorp: forceCorp));
        else
            foreach (var td in tweaks) result[td.Id] = Remove(td, forceCorp: forceCorp);
        return new Dictionary<string, TweakResult>(result);
    }

    // ── Profiles ────────────────────────────────────────────────────────

    public static IReadOnlyList<ProfileDef> Profiles { get; } = ProfileDefinitions.All;

    public static ProfileDef? GetProfile(string name)
        => Profiles.FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public IReadOnlyList<TweakDef> TweaksForProfile(string name)
    {
        var profile = GetProfile(name);
        if (profile is null) return [];
        var cats = new HashSet<string>(profile.ApplyCategories, StringComparer.OrdinalIgnoreCase);
        return _allTweaks.Where(t => cats.Contains(t.Category)).ToList();
    }

    public Dictionary<string, TweakResult> ApplyProfile(string name, bool forceCorp = false, bool parallel = false)
        => ApplyBatch(TweaksForProfile(name), forceCorp, parallel);

    // ── Snapshots ───────────────────────────────────────────────────────

    public void SaveSnapshot(string path)
    {
        var status = StatusMap();
        var snapshot = status.ToDictionary(kv => kv.Key, kv => kv.Value.ToString().ToLowerInvariant());
        var json = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public Dictionary<string, string> LoadSnapshot(string path)
    {
        var json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
    }

    public Dictionary<string, TweakResult> RestoreSnapshot(string path, bool forceCorp = false)
    {
        var snapshot = LoadSnapshot(path);
        var results = new Dictionary<string, TweakResult>();
        foreach (var (id, state) in snapshot)
        {
            var td = GetTweak(id);
            if (td is null) continue;
            results[id] = state switch
            {
                "applied" => Apply(td, forceCorp: forceCorp),
                "notapplied" => Remove(td, forceCorp: forceCorp),
                _ => TweakResult.Unknown,
            };
        }
        return results;
    }

    // ── Windows Build ───────────────────────────────────────────────────

    private static int? _windowsBuild;
    public static int WindowsBuild()
    {
        if (_windowsBuild.HasValue) return _windowsBuild.Value;
        try
        {
            using var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            var build = key?.GetValue("CurrentBuildNumber")?.ToString();
            _windowsBuild = int.TryParse(build, out var b) ? b : 0;
        }
        catch { _windowsBuild = 0; }
        return _windowsBuild.Value;
    }

    // ── Category statistics ─────────────────────────────────────────────

    public Dictionary<string, int> CategoryCounts()
        => _tweaksByCat.ToDictionary(kv => kv.Key, kv => kv.Value.Count);

    public Dictionary<TweakScope, int> ScopeCounts()
        => _allTweaks.GroupBy(t => t.Scope).ToDictionary(g => g.Key, g => g.Count());

    // ── Export for GUI ──────────────────────────────────────────────────

    public void ExportJson(string path)
    {
        var list = _allTweaks.Select(t => new
        {
            t.Id,
            t.Label,
            t.Category,
            status = DetectStatus(t).ToString().ToLowerInvariant(),
            needs_admin = t.NeedsAdmin,
            corp_safe = t.CorpSafe,
            t.Tags,
            registry_keys = t.RegistryKeys,
            t.Description,
        }).ToList();
        var json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}
