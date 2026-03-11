// RegiLattice.CLI — Program.cs
// RegiLattice command-line interface.
// All features: apply, remove, list, status, search, profiles, snapshots, doctor, etc.

using System.Runtime.InteropServices;
using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.CLI;

internal static class Program
{
    private static readonly string Version = "3.0.0";
    private static TweakEngine _engine = null!;
    private static RegistrySession _session = null!;

    [STAThread]
    internal static int Main(string[] args)
    {
        // Graceful shutdown on Ctrl-C
        Console.CancelKeyPress += (_, e) => { e.Cancel = false; Console.WriteLine("\nShutting down…"); };

        var parsed = ParseArgs(args);
        if (parsed is null) return 0; // --help or --version handled

        _session = new RegistrySession(dryRun: parsed.DryRun);
        _engine = new TweakEngine(_session);
        _engine.RegisterBuiltins();

        var cfg = AppConfig.Load(parsed.ConfigPath);
        if (cfg.ForceCorp && !parsed.Force) parsed.Force = true;

        if (parsed.DryRun)
            Console.WriteLine("\U0001f50d Dry-run mode — no registry changes will be made.");

        return Dispatch(parsed);
    }

    // ── Dispatch ────────────────────────────────────────────────────────

    private static int Dispatch(CliArgs a)
    {
        // Standalone flags (order matters — first match wins)
        if (a.Doctor) return RunDoctor();
        if (a.HwInfo) return RunHwInfo();
        if (a.ListProfiles) return RunListProfiles();
        if (a.Validate) return RunValidate();
        if (a.Stats) return RunStats();
        if (a.ShowCategories) return RunCategories(a);
        if (a.ShowTags) return RunTags();
        if (a.ExportReg is not null) return RunExportReg(a.ExportReg);
        if (a.Report) return RunReport(a);
        if (a.Check) return RunCheck();
        if (a.Diff is not null) return RunDiff(a.Diff);
        if (a.ShowList) return RunList(a);
        if (a.Search is not null) return RunSearch(a);
        if (a.ExportJson is not null) return RunExportJson(a.ExportJson);
        if (a.Snapshot is not null) return RunSaveSnapshot(a.Snapshot);
        if (a.Restore is not null) return RunRestoreSnapshot(a.Restore, a.Force);
        if (a.SnapshotDiffA is not null && a.SnapshotDiffB is not null)
            return RunSnapshotDiff(a.SnapshotDiffA, a.SnapshotDiffB, a.HtmlPath);
        if (a.Profile is not null) return RunApplyProfile(a);
        if (a.Category is not null && a.Mode is "apply" or "remove")
            return RunCategoryAction(a);
        if (a.ImportJson is not null && a.Mode is "apply" or "remove")
            return RunImportJson(a);
        if (a.Gui) return RunGui();
        if (a.Menu) return RunMenu(a.Force);

        // Positional: mode + tweak
        if (a.Mode == "status" && a.Tweak is not null) return RunStatus(a.Tweak);
        if (a.Mode is "apply" or "remove" && a.Tweak is not null) return RunAction(a);

        // Default: show help
        PrintHelp();
        return 0;
    }

    // ── Doctor ──────────────────────────────────────────────────────────

    private static int RunDoctor()
    {
        var checks = new List<(string Label, bool Passed, string Detail)>();

        // 1. .NET version
        checks.Add((".NET Runtime", true, RuntimeInformation.FrameworkDescription));

        // 2. Windows
        bool isWin = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        checks.Add(("Windows OS", isWin, Environment.OSVersion.ToString()));

        // 3. Admin status
        bool isAdmin = Elevation.IsAdmin();
        checks.Add(("Running as admin", isAdmin, isAdmin ? "yes" : "no (some tweaks unavailable)"));

        // 4. Config
        string cfgDetail = "OK";
        bool cfgOk = true;
        try { AppConfig.Load(); }
        catch (Exception ex) { cfgOk = false; cfgDetail = ex.Message[..Math.Min(80, ex.Message.Length)]; }
        checks.Add(("Config file", cfgOk, cfgDetail));

        // 5. Tweaks loaded
        bool tweaksOk = true;
        string tweakDetail = $"{_engine.TweakCount} tweaks loaded";
        var ids = _engine.AllTweaks().Select(t => t.Id).ToList();
        var dups = ids.GroupBy(i => i).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        if (dups.Count > 0) { tweaksOk = false; tweakDetail = $"Duplicate IDs: {string.Join(", ", dups)}"; }
        checks.Add(("Tweaks registry", tweaksOk, tweakDetail));

        // 6. Corporate guard
        string corpDetail = "not detected";
        bool corpOk = true;
        try
        {
            var (isCorp, reason) = CorporateGuard.Status();
            if (isCorp) corpDetail = reason;
        }
        catch (Exception ex) { corpOk = false; corpDetail = ex.Message[..Math.Min(80, ex.Message.Length)]; }
        checks.Add(("Corp guard", corpOk, corpDetail));

        // 7. Windows build
        int build = TweakEngine.WindowsBuild();
        checks.Add(("Windows build", build > 0, build.ToString()));

        // Report
        Console.WriteLine();
        Console.WriteLine($"  {"RegiLattice Doctor",38}");
        Console.WriteLine($"  {PlatformSummary()}");
        Console.WriteLine();
        bool allOk = true;
        foreach (var (label, passed, detail) in checks)
        {
            string icon = passed ? "\u2705" : "\u274c";
            allOk &= passed;
            Console.WriteLine($"  {icon}  {label,-30}  {detail}");
        }
        Console.WriteLine();
        Console.WriteLine(allOk
            ? "  All checks passed \u2014 RegiLattice is healthy."
            : "  \u26a0\ufe0f  Some checks failed. Review the items marked with \u274c above.");
        Console.WriteLine();
        return allOk ? 0 : 1;
    }

    // ── Hardware Info ────────────────────────────────────────────────────

    private static int RunHwInfo()
    {
        Console.WriteLine("Detecting hardware…");
        Console.WriteLine(HardwareInfo.Summary());
        Console.WriteLine($"\nSuggested profile: {HardwareInfo.SuggestProfile()}");
        return 0;
    }

    // ── List profiles ───────────────────────────────────────────────────

    private static int RunListProfiles()
    {
        Console.WriteLine($"{"Profile",-12} {"Tweaks",-8} Description");
        Console.WriteLine(new string('-', 60));
        foreach (var p in TweakEngine.Profiles)
        {
            int count = _engine.TweaksForProfile(p.Name).Count;
            Console.WriteLine($"{p.Name,-12} {count,-8} {p.Description}");
        }
        return 0;
    }

    // ── Validate ────────────────────────────────────────────────────────

    private static int RunValidate()
    {
        var errors = new List<string>();
        var seen = new HashSet<string>();
        foreach (var td in _engine.AllTweaks())
        {
            if (string.IsNullOrWhiteSpace(td.Id)) errors.Add($"[{td.Label}] empty id");
            else if (!seen.Add(td.Id)) errors.Add($"Duplicate id: {td.Id}");
            if (string.IsNullOrWhiteSpace(td.Label)) errors.Add($"[{td.Id}] empty label");
            if (string.IsNullOrWhiteSpace(td.Category)) errors.Add($"[{td.Id}] empty category");
        }
        if (errors.Count > 0)
        {
            Console.WriteLine($"\u274c Validation found {errors.Count} issue(s):");
            foreach (var e in errors) Console.WriteLine($"  \u2022 {e}");
            return 1;
        }
        Console.WriteLine($"\u2705 All {_engine.TweakCount} tweaks passed validation.");
        return 0;
    }

    // ── Stats ───────────────────────────────────────────────────────────

    private static int RunStats()
    {
        var tweaks = _engine.AllTweaks();
        var byCat = _engine.TweaksByCategory();
        var scopeCounts = _engine.ScopeCounts();

        Console.WriteLine("\u2500\u2500 RegiLattice Stats \u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500\u2500");
        Console.WriteLine($"  Total tweaks   : {tweaks.Count}");
        Console.WriteLine($"  Categories     : {byCat.Count}");
        Console.WriteLine($"  Profiles       : {TweakEngine.Profiles.Count}");
        Console.WriteLine();
        Console.WriteLine("  Scope breakdown:");
        foreach (TweakScope s in Enum.GetValues<TweakScope>())
            Console.WriteLine($"    {s,-10} {scopeCounts.GetValueOrDefault(s)}");
        Console.WriteLine();
        Console.WriteLine($"  Corp-safe      : {tweaks.Count(t => t.CorpSafe)}");
        Console.WriteLine($"  Needs admin    : {tweaks.Count(t => t.NeedsAdmin)}");
        Console.WriteLine($"  Has detect     : {tweaks.Count(t => t.DetectOps.Count > 0 || t.DetectAction is not null)}");
        Console.WriteLine($"  Has description: {tweaks.Count(t => !string.IsNullOrWhiteSpace(t.Description))}");
        Console.WriteLine($"  Has depends_on : {tweaks.Count(t => t.DependsOn.Count > 0)}");
        Console.WriteLine();
        Console.WriteLine($"{"Category",-30} Tweaks");
        Console.WriteLine("  " + new string('-', 38));
        foreach (var cat in byCat.Keys.Order())
            Console.WriteLine($"  {cat,-28} {byCat[cat].Count}");
        return 0;
    }

    // ── Categories ──────────────────────────────────────────────────────

    private static int RunCategories(CliArgs a)
    {
        var byCat = _engine.TweaksByCategory();
        if (a.OutputFormat == "json")
        {
            var dict = byCat.ToDictionary(kv => kv.Key, kv => kv.Value.Count);
            Console.WriteLine(JsonSerializer.Serialize(dict, new JsonSerializerOptions { WriteIndented = true }));
        }
        else
        {
            Console.WriteLine($"{"Category",-25} Tweaks");
            Console.WriteLine(new string('-', 35));
            foreach (var cat in byCat.Keys.Order())
                Console.WriteLine($"{cat,-25} {byCat[cat].Count}");
            Console.WriteLine($"\n{byCat.Count} categories, {byCat.Values.Sum(v => v.Count)} tweaks total.");
        }
        return 0;
    }

    // ── Tags ────────────────────────────────────────────────────────────

    private static int RunTags()
    {
        var counts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        foreach (var td in _engine.AllTweaks())
            foreach (var tag in td.Tags)
                counts[tag] = counts.GetValueOrDefault(tag) + 1;

        Console.WriteLine($"{"Tag",-25} Tweaks");
        Console.WriteLine(new string('-', 35));
        foreach (var (tag, cnt) in counts.OrderBy(kv => kv.Key))
            Console.WriteLine($"{tag,-25} {cnt}");
        Console.WriteLine($"\n{counts.Count} unique tags across {counts.Values.Sum()} tag usages.");
        return 0;
    }

    // ── Export .reg ─────────────────────────────────────────────────────

    private static int RunExportReg(string path)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("Windows Registry Editor Version 5.00");
        sb.AppendLine($"; RegiLattice export — {DateTime.Now:yyyy-MM-dd HH:mm}");
        sb.AppendLine();

        foreach (var td in _engine.AllTweaks())
        {
            foreach (var key in td.RegistryKeys)
            {
                // Convert HKLM/HKCU to full names for .reg format
                var regPath = key
                    .Replace("HKLM\\", "HKEY_LOCAL_MACHINE\\", StringComparison.OrdinalIgnoreCase)
                    .Replace("HKCU\\", "HKEY_CURRENT_USER\\", StringComparison.OrdinalIgnoreCase);
                sb.AppendLine($"[{regPath}]");

                try
                {
                    var (root, subKey) = RegistrySession.ParsePath(key);
                    using var k = root.OpenSubKey(subKey);
                    if (k is not null)
                    {
                        foreach (var name in k.GetValueNames())
                        {
                            var val = k.GetValue(name);
                            var kind = k.GetValueKind(name);
                            sb.AppendLine(FormatRegValue(name, val, kind));
                        }
                    }
                }
                catch { /* skip inaccessible keys */ }
                sb.AppendLine();
            }
        }

        File.WriteAllText(path, sb.ToString(), System.Text.Encoding.Unicode);
        Console.WriteLine($"\u2705 Exported registry state to {path}");
        return 0;
    }

    private static string FormatRegValue(string name, object? val, Microsoft.Win32.RegistryValueKind kind)
    {
        var quotedName = string.IsNullOrEmpty(name) ? "@" : $"\"{name}\"";
        return kind switch
        {
            Microsoft.Win32.RegistryValueKind.DWord => $"{quotedName}=dword:{(int)(val ?? 0):x8}",
            Microsoft.Win32.RegistryValueKind.String => $"{quotedName}=\"{val}\"",
            Microsoft.Win32.RegistryValueKind.ExpandString => $"{quotedName}=hex(2):{HexBytes(System.Text.Encoding.Unicode.GetBytes((string)(val ?? "") + "\0"))}",
            Microsoft.Win32.RegistryValueKind.QWord => $"{quotedName}=hex(b):{HexBytes(BitConverter.GetBytes((long)(val ?? 0L)))}",
            Microsoft.Win32.RegistryValueKind.Binary when val is byte[] b => $"{quotedName}=hex:{HexBytes(b)}",
            _ => $"; {quotedName}=(unsupported type {kind})",
        };
    }

    private static string HexBytes(byte[] data)
        => string.Join(",", data.Select(b => b.ToString("x2")));

    // ── Report ──────────────────────────────────────────────────────────

    private static int RunReport(CliArgs a)
    {
        Console.Write("Detecting status");
        var smap = _engine.StatusMap(parallel: true);
        Console.WriteLine(" done.");

        var byCat = _engine.TweaksByCategory();
        string? filterStatus = a.FilterStatus; // "enabled" or "disabled" from a filter flag

        if (a.OutputFormat == "json")
        {
            var report = byCat.Keys.Order().Select(cat =>
            {
                var tweaks = byCat[cat]
                    .Where(t => filterStatus is null ||
                                (filterStatus == "enabled" && smap.GetValueOrDefault(t.Id) == TweakResult.Applied) ||
                                (filterStatus == "disabled" && smap.GetValueOrDefault(t.Id) == TweakResult.NotApplied))
                    .Select(t => new { t.Id, t.Label, status = smap.GetValueOrDefault(t.Id).ToString() })
                    .ToList();
                return new { category = cat, tweaks };
            }).ToList();
            Console.WriteLine(JsonSerializer.Serialize(report, new JsonSerializerOptions { WriteIndented = true }));
        }
        else
        {
            foreach (var cat in byCat.Keys.Order())
            {
                var tweaks = byCat[cat];
                var filtered = tweaks.Where(t =>
                    filterStatus is null ||
                    (filterStatus == "enabled" && smap.GetValueOrDefault(t.Id) == TweakResult.Applied) ||
                    (filterStatus == "disabled" && smap.GetValueOrDefault(t.Id) == TweakResult.NotApplied));

                var list = filtered.ToList();
                if (list.Count == 0) continue;

                int applied = list.Count(t => smap.GetValueOrDefault(t.Id) == TweakResult.Applied);
                Console.WriteLine($"\n\u2500\u2500 {cat}  ({applied}/{list.Count} applied) \u2500\u2500");
                foreach (var td in list)
                {
                    var st = smap.GetValueOrDefault(td.Id);
                    string icon = st == TweakResult.Applied ? "\u2713" : st == TweakResult.NotApplied ? "\u2717" : "?";
                    Console.WriteLine($"  {icon} {td.Id,-35} {td.Label}");
                }
            }
        }
        return 0;
    }

    // ── Check (audit) ───────────────────────────────────────────────────

    private static int RunCheck()
    {
        Console.Write("Checking");
        var smap = _engine.StatusMap(parallel: true);
        Console.WriteLine(" done.");

        int applied = smap.Count(kv => kv.Value == TweakResult.Applied);
        int def = smap.Count(kv => kv.Value == TweakResult.NotApplied);
        int unknown = smap.Count(kv => kv.Value == TweakResult.Unknown);

        Console.WriteLine($"{"Status",-14} {"Count",-8}");
        Console.WriteLine(new string('-', 22));
        Console.WriteLine($"{"Applied",-14} {applied,-8}");
        Console.WriteLine($"{"Default",-14} {def,-8}");
        Console.WriteLine($"{"Unknown",-14} {unknown,-8}");
        Console.WriteLine($"{"Total",-14} {smap.Count,-8}");

        if (applied > 0)
        {
            Console.WriteLine($"\nApplied tweaks ({applied}):");
            foreach (var (id, _) in smap.Where(kv => kv.Value == TweakResult.Applied).OrderBy(kv => kv.Key))
            {
                var td = _engine.GetTweak(id);
                Console.WriteLine($"  \u2713 {id,-35} {td?.Label ?? id}");
            }
        }
        return 0;
    }

    // ── Diff vs. profile ────────────────────────────────────────────────

    private static int RunDiff(string profileName)
    {
        var profileTweakIds = _engine.TweaksForProfile(profileName).Select(t => t.Id).ToHashSet();
        Console.Write("Detecting status");
        var smap = _engine.StatusMap(parallel: true);
        Console.WriteLine(" done.");

        var toApply = profileTweakIds.Where(id => smap.GetValueOrDefault(id) != TweakResult.Applied).Order().ToList();
        var toRemove = smap
            .Where(kv => !profileTweakIds.Contains(kv.Key) && kv.Value == TweakResult.Applied)
            .Select(kv => kv.Key).Order().ToList();

        if (toApply.Count == 0 && toRemove.Count == 0)
        {
            Console.WriteLine($"System matches '{profileName}' profile \u2014 no changes needed.");
            return 0;
        }

        if (toApply.Count > 0)
        {
            Console.WriteLine($"Tweaks to APPLY for '{profileName}' profile ({toApply.Count}):");
            foreach (var id in toApply)
            {
                var td = _engine.GetTweak(id);
                Console.WriteLine($"  + {id,-35} {td?.Label ?? id}");
            }
        }
        if (toRemove.Count > 0)
        {
            Console.WriteLine($"\nApplied tweaks NOT in '{profileName}' profile ({toRemove.Count}):");
            foreach (var id in toRemove)
            {
                var td = _engine.GetTweak(id);
                Console.WriteLine($"  - {id,-35} {td?.Label ?? id}");
            }
        }
        Console.WriteLine($"\nSummary: {toApply.Count} to apply, {toRemove.Count} extra applied.");
        return 0;
    }

    // ── List tweaks ─────────────────────────────────────────────────────

    private static int RunList(CliArgs a)
    {
        IEnumerable<TweakDef> tweaks = _engine.AllTweaks();
        if (a.Category is not null)
        {
            var cat = _engine.TweaksByCategory();
            if (!cat.TryGetValue(a.Category, out var catList))
            {
                Console.WriteLine($"\u274c No tweaks found in category '{a.Category}'.");
                return 2;
            }
            tweaks = catList;
        }
        if (a.ScopeFilter is not null) tweaks = tweaks.Where(t => t.Scope == a.ScopeFilter.Value);
        if (a.MinBuild > 0) tweaks = tweaks.Where(t => t.MinBuild <= a.MinBuild);
        if (a.CorpSafe) tweaks = tweaks.Where(t => t.CorpSafe);
        if (a.NeedsAdmin) tweaks = tweaks.Where(t => t.NeedsAdmin);

        var list = tweaks.ToList();

        if (a.OutputFormat == "json")
        {
            var data = list.Select(t => new { t.Id, t.Label, t.Category, t.NeedsAdmin, t.CorpSafe }).ToList();
            Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }
        else
        {
            Console.Write("Detecting status");
            var smap = _engine.StatusMap(parallel: true);
            Console.WriteLine(" done.");
            Console.WriteLine($"{"ID",-30} {"Category",-14} {"Status",-14} Label");
            Console.WriteLine(new string('-', 80));
            foreach (var td in list)
            {
                var st = smap.GetValueOrDefault(td.Id, TweakResult.Unknown);
                Console.WriteLine($"{td.Id,-30} {td.Category,-14} {st,-14} {td.Label}");
            }
        }
        return 0;
    }

    // ── Search ──────────────────────────────────────────────────────────

    private static int RunSearch(CliArgs a)
    {
        IEnumerable<TweakDef> results = _engine.Search(a.Search!);
        if (a.ScopeFilter is not null) results = results.Where(t => t.Scope == a.ScopeFilter.Value);
        if (a.MinBuild > 0) results = results.Where(t => t.MinBuild <= a.MinBuild);
        if (a.CorpSafe) results = results.Where(t => t.CorpSafe);
        if (a.NeedsAdmin) results = results.Where(t => t.NeedsAdmin);

        var list = results.ToList();
        if (list.Count == 0)
        {
            Console.WriteLine($"No tweaks matching '{a.Search}'.");
            return 0;
        }

        if (a.OutputFormat == "json")
        {
            var data = list.Select(t => new { t.Id, t.Label, t.Category, t.Tags }).ToList();
            Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
        }
        else
        {
            Console.Write("Detecting status");
            var smap = _engine.StatusMap(parallel: true);
            Console.WriteLine(" done.");
            Console.WriteLine($"{"ID",-30} {"Category",-14} {"Status",-14} Label");
            Console.WriteLine(new string('-', 80));
            foreach (var td in list)
            {
                var st = smap.GetValueOrDefault(td.Id, TweakResult.Unknown);
                Console.WriteLine($"{td.Id,-30} {td.Category,-14} {st,-14} {td.Label}");
            }
            Console.WriteLine($"\n{list.Count} tweak(s) found.");
        }
        return 0;
    }

    // ── Export JSON ─────────────────────────────────────────────────────

    private static int RunExportJson(string path)
    {
        _engine.ExportJson(path);
        Console.WriteLine($"\u2705 Exported {_engine.TweakCount} tweaks to {path}");
        return 0;
    }

    // ── Snapshots ───────────────────────────────────────────────────────

    private static int RunSaveSnapshot(string path)
    {
        _engine.SaveSnapshot(path);
        Console.WriteLine($"\u2705 Snapshot saved to {path}");
        return 0;
    }

    private static int RunRestoreSnapshot(string path, bool force)
    {
        var results = _engine.RestoreSnapshot(path, forceCorp: force);
        foreach (var (id, result) in results)
            Console.WriteLine($"  {id}: {result}");
        return 0;
    }

    private static int RunSnapshotDiff(string fileA, string fileB, string? htmlPath)
    {
        var snapA = _engine.LoadSnapshot(fileA);
        var snapB = _engine.LoadSnapshot(fileB);
        var allIds = snapA.Keys.Union(snapB.Keys).Order().ToList();

        var diffs = new List<(string Id, string StateA, string StateB)>();
        foreach (var id in allIds)
        {
            var a = snapA.GetValueOrDefault(id, "missing");
            var b = snapB.GetValueOrDefault(id, "missing");
            if (a != b) diffs.Add((id, a, b));
        }

        if (diffs.Count == 0)
        {
            Console.WriteLine("No differences found.");
            return 0;
        }

        if (htmlPath is not null)
        {
            WriteDiffHtml(diffs, htmlPath, fileA, fileB);
            Console.WriteLine($"\u2705 HTML diff report written to {htmlPath}");
        }
        else
        {
            Console.WriteLine($"Snapshot diff: {Path.GetFileName(fileA)} vs {Path.GetFileName(fileB)}");
            Console.WriteLine($"{"ID",-35} {"File A",-15} {"File B",-15}");
            Console.WriteLine(new string('-', 65));
            foreach (var (id, stA, stB) in diffs)
                Console.WriteLine($"{id,-35} {stA,-15} {stB,-15}");
            Console.WriteLine($"\n{diffs.Count} difference(s).");
        }
        return 0;
    }

    private static void WriteDiffHtml(List<(string Id, string A, string B)> diffs, string path, string fileA, string fileB)
    {
        var sb = new System.Text.StringBuilder();
        sb.AppendLine("<!DOCTYPE html><html><head><meta charset='utf-8'>");
        sb.AppendLine("<title>RegiLattice Snapshot Diff</title>");
        sb.AppendLine("<style>body{font-family:Segoe UI,sans-serif;background:#1e1e2e;color:#cdd6f4;padding:20px}");
        sb.AppendLine("table{border-collapse:collapse;width:100%}th,td{padding:8px 12px;border:1px solid #45475a;text-align:left}");
        sb.AppendLine("th{background:#313244}.applied{color:#a6e3a1}.notapplied{color:#f38ba8}.missing{color:#f9e2af}</style></head><body>");
        sb.AppendLine($"<h1>RegiLattice Snapshot Diff</h1>");
        sb.AppendLine($"<p>File A: <code>{fileA}</code><br>File B: <code>{fileB}</code></p>");
        sb.AppendLine("<table><tr><th>Tweak ID</th><th>File A</th><th>File B</th></tr>");
        foreach (var (id, a, b) in diffs)
            sb.AppendLine($"<tr><td>{id}</td><td class='{a}'>{a}</td><td class='{b}'>{b}</td></tr>");
        sb.AppendLine("</table></body></html>");
        File.WriteAllText(path, sb.ToString());
    }

    // ── Apply profile ───────────────────────────────────────────────────

    private static int RunApplyProfile(CliArgs a)
    {
        if (!a.Force && CorporateGuard.IsCorporateNetwork())
        {
            Console.WriteLine("\U0001f6d1 Corporate network detected. Use --force to override.");
            return 6;
        }

        var targets = _engine.TweaksForProfile(a.Profile!);
        string label = $"Apply '{a.Profile}' profile ({targets.Count} tweaks)";
        if (!a.AssumeYes && !Confirm(label)) { Console.WriteLine("Aborted."); return 1; }

        var results = _engine.ApplyProfile(a.Profile!, forceCorp: a.Force, parallel: true);
        int ok = results.Count(kv => kv.Value == TweakResult.Applied);

        if (_session.DryRun)
            Console.WriteLine($"\U0001f50d Dry-run: {ok}/{results.Count} tweaks would be applied ({_session.DryOps} ops skipped).");
        else
            Console.WriteLine($"\u2705 Profile '{a.Profile}': {ok}/{results.Count} tweaks applied.");

        foreach (var (id, res) in results)
            Console.WriteLine($"  {id}: {res}");
        return 0;
    }

    // ── Category action ─────────────────────────────────────────────────

    private static int RunCategoryAction(CliArgs a)
    {
        var byCat = _engine.TweaksByCategory();
        if (!byCat.TryGetValue(a.Category!, out var catTweaks))
        {
            Console.WriteLine($"\u274c Unknown category '{a.Category}'. Use --list to see categories.");
            return 2;
        }

        if (!a.Force && CorporateGuard.IsCorporateNetwork())
        {
            Console.WriteLine("\U0001f6d1 Corporate network detected. Use --force to override.");
            return 6;
        }

        string label = $"{(a.Mode == "apply" ? "Apply" : "Remove")} {catTweaks.Count} tweaks in '{a.Category}'";
        if (!a.AssumeYes && !Confirm(label)) { Console.WriteLine("Aborted."); return 1; }

        var results = a.Mode == "apply"
            ? _engine.ApplyBatch(catTweaks, forceCorp: a.Force)
            : _engine.RemoveBatch(catTweaks, forceCorp: a.Force);

        int ok = results.Count(kv => kv.Value is TweakResult.Applied or TweakResult.NotApplied);
        var errors = results.Where(kv => kv.Value == TweakResult.Error).ToList();

        if (_session.DryRun)
            Console.WriteLine($"\U0001f50d Dry-run: {ok}/{catTweaks.Count} tweaks ({_session.DryOps} ops skipped).");
        else
            Console.WriteLine($"\u2705 {ok}/{catTweaks.Count} tweaks processed in '{a.Category}'.");

        foreach (var (id, _) in errors)
            Console.WriteLine($"  \u274c {id}");
        return 0;
    }

    // ── Import JSON action ──────────────────────────────────────────────

    private static int RunImportJson(CliArgs a)
    {
        if (!a.Force && CorporateGuard.IsCorporateNetwork())
        {
            Console.WriteLine("\U0001f6d1 Corporate network detected. Use --force to override.");
            return 6;
        }

        List<string>? ids;
        try
        {
            var raw = File.ReadAllText(a.ImportJson!);
            // Support both ["id1","id2"] and {"tweaks":["id1","id2"]} formats
            try
            {
                ids = JsonSerializer.Deserialize<List<string>>(raw);
            }
            catch
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(raw);
                ids = dict?["tweaks"].Deserialize<List<string>>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\u274c Failed to read JSON: {ex.Message}");
            return 3;
        }

        if (ids is null || ids.Count == 0) { Console.WriteLine("No valid tweaks found in JSON."); return 2; }

        var targets = new List<TweakDef>();
        foreach (var id in ids)
        {
            var td = _engine.GetTweak(id);
            if (td is null) Console.WriteLine($"\u26a0\ufe0f Skipping unknown tweak '{id}'");
            else targets.Add(td);
        }
        if (targets.Count == 0) { Console.WriteLine("No valid tweaks found in JSON."); return 2; }

        string label = $"{(a.Mode == "apply" ? "Apply" : "Remove")} {targets.Count} tweaks from {Path.GetFileName(a.ImportJson)}";
        if (!a.AssumeYes && !Confirm(label)) { Console.WriteLine("Aborted."); return 1; }

        var results = a.Mode == "apply"
            ? _engine.ApplyBatch(targets, forceCorp: a.Force)
            : _engine.RemoveBatch(targets, forceCorp: a.Force);

        int ok = results.Count(kv => kv.Value is TweakResult.Applied or TweakResult.NotApplied);
        if (_session.DryRun)
            Console.WriteLine($"\U0001f50d Dry-run: {ok}/{targets.Count} tweaks from JSON ({_session.DryOps} ops skipped).");
        else
            Console.WriteLine($"\u2705 {ok}/{targets.Count} tweaks processed from JSON.");

        return 0;
    }

    // ── Status ──────────────────────────────────────────────────────────

    private static int RunStatus(string tweakId)
    {
        var td = _engine.GetTweak(tweakId);
        if (td is null) { Console.WriteLine($"\u274c Unknown tweak '{tweakId}'."); return 2; }
        var status = _engine.DetectStatus(td);
        Console.WriteLine($"{td.Label}: {status}");
        return 0;
    }

    // ── Apply / Remove single or all ────────────────────────────────────

    private static int RunAction(CliArgs a)
    {
        if (!a.Force && CorporateGuard.IsCorporateNetwork())
        {
            Console.WriteLine("\U0001f6d1 Corporate network detected. Use --force to override.");
            return 6;
        }

        bool isApply = a.Mode == "apply";

        if (a.Tweak == "all")
        {
            var all = _engine.AllTweaks();
            string label = $"{(isApply ? "Apply" : "Remove")} ALL {all.Count} tweaks";
            if (!a.AssumeYes && !Confirm(label)) { Console.WriteLine("Aborted."); return 1; }

            var results = isApply
                ? _engine.ApplyBatch(all, forceCorp: a.Force, parallel: true)
                : _engine.RemoveBatch(all, forceCorp: a.Force, parallel: true);

            int ok = results.Count(kv => kv.Value is TweakResult.Applied or TweakResult.NotApplied);
            int errs = results.Count(kv => kv.Value == TweakResult.Error);
            if (_session.DryRun)
                Console.WriteLine($"\U0001f50d Dry-run: {ok}/{all.Count} tweaks ({_session.DryOps} ops skipped).");
            else
                Console.WriteLine($"\u2705 {(isApply ? "Applied" : "Removed")} {ok}/{all.Count} tweaks ({errs} errors).");
            return errs > 0 ? 1 : 0;
        }

        var td = _engine.GetTweak(a.Tweak!);
        if (td is null) { Console.WriteLine($"\u274c Unknown tweak '{a.Tweak}'."); return 2; }

        if (!a.AssumeYes && !Confirm($"{(isApply ? "Apply" : "Remove")} '{td.Label}'"))
        { Console.WriteLine("Aborted."); return 1; }

        var result = isApply ? _engine.Apply(td, forceCorp: a.Force) : _engine.Remove(td, forceCorp: a.Force);

        if (_session.DryRun)
            Console.WriteLine($"\U0001f50d Dry-run: {td.Label} — {result} ({_session.DryOps} ops skipped).");
        else
            Console.WriteLine($"\u2705 {td.Label}: {result}");

        return result is TweakResult.Applied or TweakResult.NotApplied ? 0 : 1;
    }

    // ── GUI launcher ────────────────────────────────────────────────────

    private static int RunGui()
    {
        // Look for the GUI executable next to us
        var guiExe = Path.Combine(AppContext.BaseDirectory, "RegiLattice.GUI.exe");
        if (!File.Exists(guiExe))
        {
            Console.WriteLine($"\u274c GUI executable not found at {guiExe}");
            Console.WriteLine("Build RegiLattice.GUI project first or use the GUI directly.");
            return 1;
        }
        var psi = new System.Diagnostics.ProcessStartInfo(guiExe) { UseShellExecute = true };
        System.Diagnostics.Process.Start(psi);
        return 0;
    }

    // ── Interactive menu ────────────────────────────────────────────────

    private static int RunMenu(bool force)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            Console.WriteLine($"\u26a0\ufe0f This menu is intended for Windows. Detected: {RuntimeInformation.OSDescription}");
            return 4;
        }

        var categories = _engine.Categories();
        while (true)
        {
            Console.Clear();
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine("     RegiLattice — Interactive Menu");
            Console.WriteLine("══════════════════════════════════════════");
            Console.WriteLine();
            for (int i = 0; i < categories.Count; i++)
            {
                var cat = categories[i];
                var count = _engine.TweaksByCategory()[cat].Count;
                Console.WriteLine($"  {i + 1,3}. {cat,-30} ({count} tweaks)");
            }
            Console.WriteLine($"\n    0. Exit\n");
            Console.Write("Select category: ");

            var input = Console.ReadLine()?.Trim();
            if (input == "0" || string.IsNullOrEmpty(input)) break;
            if (!int.TryParse(input, out int choice) || choice < 1 || choice > categories.Count)
            {
                Console.WriteLine("Invalid choice. Press any key…");
                Console.ReadKey(true);
                continue;
            }

            var selectedCat = categories[choice - 1];
            var tweaks = _engine.TweaksByCategory()[selectedCat];
            Console.Clear();
            Console.WriteLine($"\n── {selectedCat} ({tweaks.Count} tweaks) ──\n");

            var smap = _engine.StatusMap(ids: tweaks.Select(t => t.Id));
            for (int i = 0; i < tweaks.Count; i++)
            {
                var td = tweaks[i];
                var st = smap.GetValueOrDefault(td.Id, TweakResult.Unknown);
                string icon = st == TweakResult.Applied ? "\u2713" : st == TweakResult.NotApplied ? "\u2717" : "?";
                Console.WriteLine($"  {i + 1,3}. [{icon}] {td.Label}");
            }

            Console.WriteLine("\n  A = Apply all  |  R = Remove all  |  B = Back\n");
            Console.Write("Select tweak # or command: ");
            var cmd = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(cmd) || cmd.Equals("B", StringComparison.OrdinalIgnoreCase)) continue;

            if (cmd.Equals("A", StringComparison.OrdinalIgnoreCase))
            {
                _engine.ApplyBatch(tweaks, forceCorp: force);
                Console.WriteLine($"\u2705 Applied {tweaks.Count} tweaks. Press any key…");
                Console.ReadKey(true);
            }
            else if (cmd.Equals("R", StringComparison.OrdinalIgnoreCase))
            {
                _engine.RemoveBatch(tweaks, forceCorp: force);
                Console.WriteLine($"\u2705 Removed {tweaks.Count} tweaks. Press any key…");
                Console.ReadKey(true);
            }
            else if (int.TryParse(cmd, out int tweakChoice) && tweakChoice >= 1 && tweakChoice <= tweaks.Count)
            {
                var td = tweaks[tweakChoice - 1];
                Console.Write($"(A)pply or (R)emove '{td.Label}'? ");
                var action = Console.ReadLine()?.Trim().ToUpperInvariant();
                if (action == "A") _engine.Apply(td, forceCorp: force);
                else if (action == "R") _engine.Remove(td, forceCorp: force);
                Console.WriteLine("Done. Press any key…");
                Console.ReadKey(true);
            }
        }
        return 0;
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    private static bool Confirm(string label)
    {
        Console.Write($"{label}? [y/N] ");
        var response = Console.ReadLine()?.Trim();
        return response is "y" or "Y" or "yes" or "Yes";
    }

    private static string PlatformSummary()
        => $".NET {Environment.Version} | {RuntimeInformation.OSDescription} | Build {TweakEngine.WindowsBuild()}";

    private static void PrintHelp()
    {
        Console.WriteLine($"""
            RegiLattice v{Version} — Windows registry tweak toolkit

            Usage: regilattice [mode] [tweak] [options]

            Modes:
              apply <id|all>     Apply a tweak or all tweaks
              remove <id|all>    Remove a tweak or all tweaks
              status <id>        Show status of a specific tweak

            Options:
              --list              List all available tweaks
              --search <query>    Search tweaks by keyword
              --profile <name>    Apply a profile (business|gaming|privacy|minimal|server)
              --list-profiles     List available profiles
              --categories        List tweak categories with counts
              --tags              List all unique tags
              --stats             Show comprehensive statistics
              --validate          Validate all tweak definitions
              --check             Audit: show which tweaks are applied/default/unknown
              --report            Status report by category
              --diff <profile>    Compare current state against a profile
              --doctor            System health check
              --hwinfo            Detect and display hardware info
              --gui               Launch the graphical interface
              --menu              Launch the interactive terminal menu

            Export / Import:
              --export-json <path>   Export tweak definitions to JSON
              --export-reg <path>    Export registry state to .reg file
              --import-json <path>   Import tweak IDs from JSON (use with apply/remove)

            Snapshots:
              --snapshot <path>       Save current state snapshot
              --restore <path>        Restore tweaks from snapshot
              --snapshot-diff A B     Compare two snapshots
              --html <path>           HTML output for snapshot-diff

            Filters (use with --list/--search):
              --scope <user|machine|both>  Filter by registry scope
              --min-build <number>         Filter by Windows build
              --corp-safe                  Only corporate-safe tweaks
              --needs-admin                Only admin-required tweaks
              --output <table|json>        Output format (default: table)
              --category <name>            Filter by category / apply-remove category

            General:
              --force             Bypass corporate network guard
              --dry-run           Preview without modifying registry
              --config <path>     Specify config file path
              -y, --assume-yes   Skip confirmation prompts
              --version           Show version info
              --help, -h          Show this help
            """);
    }

    // ── Argument parsing ────────────────────────────────────────────────

    private static CliArgs? ParseArgs(string[] args)
    {
        var p = new CliArgs();
        int i = 0;

        while (i < args.Length)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--help" or "-h":
                    PrintHelp();
                    return null;

                case "--version" or "-V":
                    Console.WriteLine($"regilattice {Version} ({PlatformSummaryStatic()})");
                    return null;

                case "--list": p.ShowList = true; break;
                case "--force": p.Force = true; break;
                case "--gui": p.Gui = true; break;
                case "--menu": p.Menu = true; break;
                case "--dry-run": p.DryRun = true; break;
                case "-y" or "--assume-yes": p.AssumeYes = true; break;
                case "--doctor": p.Doctor = true; break;
                case "--hwinfo": p.HwInfo = true; break;
                case "--list-profiles": p.ListProfiles = true; break;
                case "--validate": p.Validate = true; break;
                case "--stats": p.Stats = true; break;
                case "--categories" or "--list-categories": p.ShowCategories = true; break;
                case "--tags": p.ShowTags = true; break;
                case "--report": p.Report = true; break;
                case "--check": p.Check = true; break;
                case "--corp-safe": p.CorpSafe = true; break;
                case "--needs-admin": p.NeedsAdmin = true; break;

                case "--search":
                    if (++i < args.Length) p.Search = args[i];
                    break;
                case "--profile":
                    if (++i < args.Length) p.Profile = args[i];
                    break;
                case "--config":
                    if (++i < args.Length) p.ConfigPath = args[i];
                    break;
                case "--snapshot":
                    if (++i < args.Length) p.Snapshot = args[i];
                    break;
                case "--restore":
                    if (++i < args.Length) p.Restore = args[i];
                    break;
                case "--export-json":
                    if (++i < args.Length) p.ExportJson = args[i];
                    break;
                case "--export-reg":
                    if (++i < args.Length) p.ExportReg = args[i];
                    break;
                case "--import-json":
                    if (++i < args.Length) p.ImportJson = args[i];
                    break;
                case "--diff":
                    if (++i < args.Length) p.Diff = args[i];
                    break;
                case "--category":
                    if (++i < args.Length) p.Category = args[i];
                    break;
                case "--output":
                    if (++i < args.Length) p.OutputFormat = args[i];
                    break;
                case "--html":
                    if (++i < args.Length) p.HtmlPath = args[i];
                    break;
                case "--scope":
                    if (++i < args.Length) p.ScopeFilter = args[i].ToLowerInvariant() switch
                    {
                        "user" => TweakScope.User,
                        "machine" => TweakScope.Machine,
                        "both" => TweakScope.Both,
                        _ => null,
                    };
                    break;
                case "--min-build":
                    if (++i < args.Length && int.TryParse(args[i], out var build))
                        p.MinBuild = build;
                    break;
                case "--snapshot-diff":
                    if (i + 2 < args.Length)
                    {
                        p.SnapshotDiffA = args[++i];
                        p.SnapshotDiffB = args[++i];
                    }
                    break;

                default:
                    // Positional: mode, then tweak
                    if (p.Mode is null && arg is "apply" or "remove" or "status")
                        p.Mode = arg;
                    else if (p.Tweak is null && p.Mode is not null)
                        p.Tweak = arg;
                    break;
            }
            i++;
        }
        return p;
    }

    private static string PlatformSummaryStatic()
        => $".NET {Environment.Version} | {RuntimeInformation.OSDescription}";

    // ── Parsed arguments ────────────────────────────────────────────────

    private sealed class CliArgs
    {
        public string? Mode { get; set; }
        public string? Tweak { get; set; }
        public bool ShowList { get; set; }
        public bool Force { get; set; }
        public bool Gui { get; set; }
        public bool Menu { get; set; }
        public bool DryRun { get; set; }
        public bool AssumeYes { get; set; }
        public bool Doctor { get; set; }
        public bool HwInfo { get; set; }
        public bool ListProfiles { get; set; }
        public bool Validate { get; set; }
        public bool Stats { get; set; }
        public bool ShowCategories { get; set; }
        public bool ShowTags { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool CorpSafe { get; set; }
        public bool NeedsAdmin { get; set; }
        public string? Search { get; set; }
        public string? Profile { get; set; }
        public string? ConfigPath { get; set; }
        public string? Snapshot { get; set; }
        public string? Restore { get; set; }
        public string? ExportJson { get; set; }
        public string? ExportReg { get; set; }
        public string? ImportJson { get; set; }
        public string? Diff { get; set; }
        public string? Category { get; set; }
        public string OutputFormat { get; set; } = "table";
        public string? HtmlPath { get; set; }
        public string? SnapshotDiffA { get; set; }
        public string? SnapshotDiffB { get; set; }
        public TweakScope? ScopeFilter { get; set; }
        public int MinBuild { get; set; }
        public string? FilterStatus { get; set; }
    }
}
