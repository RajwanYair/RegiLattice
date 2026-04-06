using RegiLattice.Core.Models;

namespace RegiLattice.CLI;

/// <summary>Parsed CLI arguments for the RegiLattice command-line interface.</summary>
internal sealed class CliArgs
{
    public string? Mode { get; set; }
    public string? Tweak { get; set; }

    // ── B1: Verb-noun subcommand routing ─────────────────────────────────
    /// <summary>The top-level verb when using subcommand syntax (e.g. "tweak", "search", "profile").</summary>
    public string? SubVerb { get; set; }
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
    public bool NoColor { get; set; }
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
    public string? Marketplace { get; set; }
    public string? MarketplaceArg { get; set; }
    public string? DependsOn { get; set; }
    public string? ExportConfig { get; set; }
    public string? ImportConfig { get; set; }
    public bool ShowFavorites { get; set; }
    public string? FavoriteAdd { get; set; }
    public string? FavoriteRemove { get; set; }
    public bool ShowHistory { get; set; }
    public int HistoryCount { get; set; } = 20;
    public string? Compliance { get; set; }
    public string? ExportGpo { get; set; }
    public string? ExportIntune { get; set; }
    public string? NewPack { get; set; }
    public string? Manager { get; set; }

    // ── Sprint 59/60 – portable & silent modes ───────────────────────────
    /// <summary>
    /// When true, all data is rooted at .\data\ relative to the executable
    /// instead of %LOCALAPPDATA%\RegiLattice (portable-mode).
    /// </summary>
    public bool Portable { get; set; }

    /// <summary>
    /// When true, no Console output is produced; results are written to
    /// <see cref="LogFile"/> as a JSON array of operation records.
    /// Exit code: 0 = all applied/no-op, 1 = any failure.
    /// </summary>
    public bool Silent { get; set; }

    /// <summary>
    /// Optional path for the JSON result log written in --silent mode.
    /// When left empty and --silent is active, results are discarded (exit code only).
    /// </summary>
    public string? LogFile { get; set; }

    // ── Sprint 70 – PowerShell module generation ─────────────────────────
    /// <summary>
    /// When true, generates RegiLattice.psd1 / RegiLattice.psm1 in <see cref="PsModuleOutput"/>.
    /// </summary>
    public bool GeneratePsModule { get; set; }

    /// <summary>Output directory for the generated PowerShell module files.</summary>
    public string? PsModuleOutput { get; set; }

    // ── Sprint 72 – HTML report ───────────────────────────────────────────
    /// <summary>
    /// When non-null, writes a full HTML tweak-status report to this path
    /// then exits.  The report includes summary cards and per-category tables.
    /// </summary>
    public string? HtmlReport { get; set; }

    // ── Sprint 105 – compliance history ─────────────────────────────────────
    /// <summary>When true, prints the compliance check history log and exits.</summary>
    public bool ComplianceHistory { get; set; }

    /// <summary>
    /// When non-null and equals "auto", runs a compliance check against the most
    /// recent snapshot in the data directory and saves the result to history.
    /// </summary>
    public string? ComplianceReportMode { get; set; }

    // ── Sprint 127 – custom user profiles ───────────────────────────────
    /// <summary>Name of the user profile to create (--profile-create).</summary>
    public string? ProfileCreate { get; set; }

    /// <summary>Name of the user profile to delete (--profile-delete).</summary>
    public string? ProfileDelete { get; set; }

    /// <summary>Source profile name for clone/rename (--profile-clone, --profile-rename).</summary>
    public string? ProfileFrom { get; set; }

    /// <summary>When true, the ProfileFrom/Profile pair is used for a clone operation.</summary>
    public bool IsProfileClone { get; set; }

    /// <summary>When true, the ProfileFrom/Profile pair is used for a rename operation.</summary>
    public bool IsProfileRename { get; set; }

    /// <summary>
    /// Comma-separated tweak IDs used during profile creation or update
    /// (--profile-tweaks id1,id2,id3).
    /// </summary>
    public string? ProfileTweaks { get; set; }

    /// <summary>Optional description text for --profile-create.</summary>
    public string? ProfileDesc { get; set; }

    /// <summary>When true, lists only user-defined (custom) profiles.</summary>
    public bool ListUserProfiles { get; set; }

    // ── Sprint 132 – plugin sandboxing ──────────────────────────────────
    /// <summary>
    /// When <c>true</c>, the process was spawned by <see cref="RegiLattice.Core.Plugins.PluginSandbox"/>
    /// and should run in plugin-host mode: read ops from the named pipe, execute them, return result.
    /// </summary>
    public bool PluginHost { get; set; }

    /// <summary>Named pipe name passed via <c>--plugin-host &lt;pipeName&gt;</c>.</summary>
    public string? PluginPipeName { get; set; }

    // ── B7: Batch apply/remove mode ──────────────────────────────────────
    /// <summary>
    /// When non-null, reads tweak IDs from a file and applies or removes each in sequence.
    /// Accepts a text file with one ID per line, a JSON array of IDs, or a snapshot JSON.
    /// Set via <c>batch apply &lt;file&gt;</c> or <c>batch remove &lt;file&gt;</c>.
    /// </summary>
    public string? BatchFile { get; set; }

    /// <summary>"apply" or "remove" — the batch operation to perform.</summary>
    public string? BatchMode { get; set; }

    // ── Phase 3.1 — global --json output flag ────────────────────────
    /// <summary>
    /// When true, all command output is written as a single JSON object rather
    /// than human-readable text. Equivalent to <c>--output json</c>.
    /// </summary>
    public bool JsonOutput { get; set; }

    // ── Phase 3.3 — conditional apply flags ─────────────────────────
    /// <summary>Skip apply/remove if tweak is already in the target state.</summary>
    public bool IfNotApplied { get; set; }

    /// <summary>Skip apply/remove if the process is not running elevated (admin).</summary>
    public bool IfAdmin { get; set; }

    /// <summary>
    /// Skip apply/remove if the current Windows build number is below this value.
    /// 0 means the check is disabled.
    /// </summary>
    public int IfBuildMin { get; set; }

    /// <summary>
    /// Skip apply/remove if <see cref="CorporateGuard.IsCorporateNetwork"/> returns true.
    /// </summary>
    public bool IfNotCorp { get; set; }

    // ── Phase 3.4 — interactive wizard ──────────────────────────────
    /// <summary>When true, launches the interactive profile setup wizard.</summary>
    public bool Wizard { get; set; }

    // ── Phase 3.2 — batch recipe executor ───────────────────────────
    /// <summary>
    /// When non-null, reads a JSON recipe file (.rl.json) and executes its
    /// named steps sequentially (apply, apply-profile, remove, verify).
    /// Distinct from <see cref="BatchFile"/> which reads a flat ID list.
    /// </summary>
    public string? BatchRecipe { get; set; }

    // ── Phase 3.5 — watch mode drift detection ───────────────────────
    /// <summary>When true, continuously monitors applied tweaks for registry drift.</summary>
    public bool Watch { get; set; }

    /// <summary>Interval in seconds between drift-detection polls (default: 300).</summary>
    public int WatchInterval { get; set; } = 300;

    /// <summary>When true, automatically re-applies drifted tweaks instead of just logging.</summary>
    public bool WatchAutoFix { get; set; }

    /// <summary>When non-null, limits drift monitoring to the tweak IDs read from this file.</summary>
    public string? WatchFile { get; set; }
}
