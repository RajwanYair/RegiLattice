using RegiLattice.Core.Models;

namespace RegiLattice.CLI;

/// <summary>Parsed CLI arguments for the RegiLattice command-line interface.</summary>
internal sealed class CliArgs
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
}
