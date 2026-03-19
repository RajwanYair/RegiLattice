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
}
