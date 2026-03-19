// RegiLattice.Core — Services/AppConfig.cs
// User configuration — replaces Python config.py. Uses JSON for simplicity.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

public sealed class AppConfig
{
    [JsonPropertyName("force_corp")]
    public bool ForceCorp { get; set; }

    [JsonPropertyName("max_workers")]
    public int MaxWorkers { get; set; } = 8;

    [JsonPropertyName("backup_dir")]
    public string BackupDir { get; set; } = "";

    [JsonPropertyName("auto_backup")]
    public bool AutoBackup { get; set; } = true;

    [JsonPropertyName("theme")]
    public string Theme { get; set; } = "catppuccin-mocha";

    [JsonPropertyName("locale")]
    public string Locale { get; set; } = "en";

    [JsonPropertyName("check_tool_updates")]
    public bool CheckToolUpdates { get; set; } = true;

    [JsonPropertyName("minimize_to_tray")]
    public bool MinimizeToTray { get; set; } = true;

    [JsonPropertyName("confirm_apply")]
    public bool ConfirmApply { get; set; } = true;

    [JsonPropertyName("confirm_remove")]
    public bool ConfirmRemove { get; set; } = true;

    [JsonPropertyName("show_inapplicable")]
    public bool ShowInapplicable { get; set; } = true;

    [JsonPropertyName("status_bar_monitor")]
    public bool StatusBarMonitor { get; set; } = true;

    [JsonPropertyName("detail_panel_height")]
    public int DetailPanelHeight { get; set; } = 130;

    [JsonPropertyName("last_seen_version")]
    public string LastSeenVersion { get; set; } = "";

    // ── GUI display preferences ──────────────────────────────────────────
    [JsonPropertyName("font_size")]
    public float FontSize { get; set; } = 9f;

    [JsonPropertyName("show_log_panel")]
    public bool ShowLogPanel { get; set; } = true;

    [JsonPropertyName("log_panel_height")]
    public int LogPanelHeight { get; set; } = 150;

    [JsonPropertyName("auto_refresh_startup")]
    public bool AutoRefreshOnStartup { get; set; } = true;

    [JsonPropertyName("launch_minimized")]
    public bool LaunchMinimized { get; set; }

    // ── Advanced display / behaviour ─────────────────────────────────────
    /// <summary>
    /// When true, the tree-view pane width is remembered across sessions.
    /// </summary>
    [JsonPropertyName("remember_splitter")]
    public bool RememberSplitter { get; set; } = true;

    /// <summary>Last-saved tree splitter distance (pixels).</summary>
    [JsonPropertyName("splitter_distance")]
    public int SplitterDistance { get; set; }

    /// <summary>
    /// When true, tweaks that are already applied are shown with a visual indicator
    /// but the checkbox is unchecked (prevents accidental re-apply).
    /// </summary>
    [JsonPropertyName("skip_applied_on_batch")]
    public bool SkipAppliedOnBatch { get; set; } = true;

    // ── Brightness Scheduler ─────────────────────────────────────────────
    [JsonPropertyName("brightness_scheduler_enabled")]
    public bool BrightnessSchedulerEnabled { get; set; }

    [JsonPropertyName("brightness_day_pct")]
    public int BrightnessDayPct { get; set; } = 80;

    [JsonPropertyName("brightness_night_pct")]
    public int BrightnessNightPct { get; set; } = 40;

    [JsonPropertyName("brightness_day_time")]
    public string BrightnessDayTime { get; set; } = "07:00";

    [JsonPropertyName("brightness_night_time")]
    public string BrightnessNightTime { get; set; } = "21:00";

    /// <summary>
    /// Maximum number of tweak history entries to retain.
    /// Configurable so power users can increase the rolling window.
    /// </summary>
    [JsonPropertyName("history_max_entries")]
    public int HistoryMaxEntries { get; set; } = 500;

    /// <summary>
    /// When true, the status bar CPU/RAM monitor colours change based on load level
    /// (green → yellow → red above 80%).
    /// </summary>
    [JsonPropertyName("monitor_color_coded")]
    public bool MonitorColorCoded { get; set; } = true;

    /// <summary>
    /// RAM percentage threshold (0–100) above which auto memory clean fires.
    /// 0 = disabled.
    /// </summary>
    [JsonPropertyName("auto_clean_memory_threshold")]
    public int AutoCleanMemoryThreshold { get; set; } = 0;

    // ── Sprint 47 enhancements ────────────────────────────────────────────

    /// <summary>
    /// When true, RegiLattice automatically creates a registry backup (JSON) in
    /// <see cref="BackupDir"/> before applying any tweak batch.  This provides a
    /// one-click rollback point without requiring a manual snapshot.
    /// </summary>
    [JsonPropertyName("auto_backup_on_apply")]
    public bool AutoBackupOnApply { get; set; } = true;

    /// <summary>
    /// When true, a full state snapshot is saved to <c>%LOCALAPPDATA%\RegiLattice\snapshots\</c>
    /// automatically before a profile is applied.  Enables single-click profile rollback.
    /// </summary>
    [JsonPropertyName("snapshot_on_profile_change")]
    public bool SnapshotOnProfileChange { get; set; } = true;

    /// <summary>Default config directory: %LOCALAPPDATA%\RegiLattice</summary>
    public static string ConfigDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice");

    // ── Profile Scheduler ────────────────────────────────────────────────
    /// <summary>List of scheduled profile switches persisted with the config.</summary>
    [JsonPropertyName("profile_schedules")]
    public List<ProfileScheduleEntry> ProfileSchedules { get; set; } = [];

    /// <summary>Profile name to apply automatically when the power plan changes (empty = disabled).</summary>
    [JsonPropertyName("profile_on_plan_switch")]
    public string? ProfileOnPlanSwitch { get; set; }

    public static string DefaultConfigPath => Path.Combine(ConfigDir, "config.json");

    public static AppConfig Load(string? path = null)
    {
        path ??= DefaultConfigPath;
        if (!File.Exists(path))
            return new AppConfig();
        try
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
        }
        catch
        {
            return new AppConfig();
        }
    }

    public void Save(string? path = null)
    {
        path ??= DefaultConfigPath;
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }
}

/// <summary>Represents one scheduled profile-switch entry persisted in AppConfig.</summary>
public sealed record ProfileScheduleEntry
{
    [JsonPropertyName("profile")]
    public required string Profile { get; init; }

    [JsonPropertyName("trigger")]
    public required string Trigger { get; init; }  // "daily", "on_boot", "on_login"

    [JsonPropertyName("time")]
    public string Time { get; init; } = "";  // "HH:mm" for daily trigger

    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
