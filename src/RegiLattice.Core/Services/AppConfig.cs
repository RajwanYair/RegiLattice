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

    // ── Profile Scheduler ────────────────────────────────────────────────
    /// <summary>List of scheduled profile switches persisted with the config.</summary>
    [JsonPropertyName("profile_schedules")]
    public IReadOnlyList<ProfileScheduleEntry> ProfileSchedules { get; set; } = [];

    /// <summary>Profile name to apply automatically when the power plan changes (empty = disabled).</summary>
    [JsonPropertyName("profile_on_plan_switch")]
    public string? ProfileOnPlanSwitch { get; set; }

    /// <summary>
    /// When <see langword="true"/> (default), the First-Run Wizard has not yet been
    /// shown to the user.  Set to <see langword="false"/> after the wizard completes.
    /// </summary>
    [JsonPropertyName("first_run_wizard_pending")]
    public bool FirstRunWizardPending { get; set; } = true;

    /// <summary>
    /// Profile name shown pre-selected in the GUI toolbar on startup
    /// (e.g., "minimal", "privacy", "gaming").  Does not auto-apply the profile.
    /// </summary>
    [JsonPropertyName("default_profile")]
    public string DefaultProfile { get; set; } = "minimal";

    /// <summary>
    /// When <see langword="true"/>, the GUI and engine operate in dry-run mode:
    /// all registry operations are captured but never written.
    /// Persisted so the preference survives restarts.
    /// </summary>
    [JsonPropertyName("dry_run")]
    public bool DryRun { get; set; }

    // ── Portable mode ────────────────────────────────────────────────────────
    // When enabled, ALL data (config, backups, snapshots, history, favorites …)
    // is rooted at .\data\ relative to the executable instead of %LOCALAPPDATA%.
    // Activate by calling SetPortable(true) early in the entry point, or by
    // placing a sentinel file <exe-dir>\data\.portable before launch.

    private static bool _isPortable;

    /// <summary>
    /// Returns <see langword="true"/> when the application is running in portable
    /// mode.  In this mode every data-path property returns a path beneath
    /// <see cref="PortableDataDir"/> rather than <c>%LOCALAPPDATA%\RegiLattice</c>.
    /// </summary>
    public static bool IsPortable => _isPortable;

    /// <summary>
    /// Root directory for all data in portable mode: <c>&lt;exe-dir&gt;\data\</c>.
    /// </summary>
    public static string PortableDataDir => Path.Combine(AppContext.BaseDirectory, "data");

    /// <summary>
    /// Activates or deactivates portable mode.  Call before any config paths are
    /// resolved — typically at the very start of <c>Main()</c>.
    /// </summary>
    public static void SetPortable(bool value) => _isPortable = value;

    /// <summary>
    /// Auto-detects portable mode from a sentinel file
    /// <c>&lt;exe-dir&gt;\data\.portable</c>.  Call once at startup before any
    /// other config access.
    /// </summary>
    public static void AutoDetectPortable()
    {
        var sentinel = Path.Combine(AppContext.BaseDirectory, "data", ".portable");
        if (File.Exists(sentinel))
            _isPortable = true;
    }

    /// <summary>Default config directory: %LOCALAPPDATA%\RegiLattice (or portable root).</summary>
    public static string ConfigDir =>
        _isPortable ? PortableDataDir : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice");

    public static string DefaultConfigPath => Path.Combine(ConfigDir, "config.json");

    /// <summary>
    /// Validates all config fields and returns a list of human-readable error messages.
    /// Returns an empty list when configuration is valid.
    /// </summary>
    public IReadOnlyList<string> Validate()
    {
        var errors = new List<string>();

        if (MaxWorkers < 1 || MaxWorkers > 32)
            errors.Add($"max_workers must be between 1 and 32 (current: {MaxWorkers}).");

        if (string.IsNullOrWhiteSpace(Theme))
            errors.Add("theme cannot be empty.");

        if (string.IsNullOrWhiteSpace(Locale))
            errors.Add("locale cannot be empty.");

        if (FontSize < 6f || FontSize > 36f)
            errors.Add($"font_size must be between 6 and 36 (current: {FontSize:F1}).");

        if (DetailPanelHeight is < 50 or > 1600)
            errors.Add($"detail_panel_height must be between 50 and 1600 (current: {DetailPanelHeight}).");

        if (LogPanelHeight is < 50 or > 1600)
            errors.Add($"log_panel_height must be between 50 and 1600 (current: {LogPanelHeight}).");

        if (HistoryMaxEntries < 10 || HistoryMaxEntries > 100_000)
            errors.Add($"history_max_entries must be between 10 and 100 000 (current: {HistoryMaxEntries}).");

        if (AutoCleanMemoryThreshold < 0 || AutoCleanMemoryThreshold > 100)
            errors.Add($"auto_clean_memory_threshold must be between 0 and 100 (current: {AutoCleanMemoryThreshold}).");

        if (BrightnessDayPct < 0 || BrightnessDayPct > 100)
            errors.Add($"brightness_day_pct must be between 0 and 100 (current: {BrightnessDayPct}).");

        if (BrightnessNightPct < 0 || BrightnessNightPct > 100)
            errors.Add($"brightness_night_pct must be between 0 and 100 (current: {BrightnessNightPct}).");

        if (!string.IsNullOrEmpty(BrightnessDayTime) && !IsValidHhmm(BrightnessDayTime))
            errors.Add($"brightness_day_time must be in HH:mm format (current: '{BrightnessDayTime}').");

        if (!string.IsNullOrEmpty(BrightnessNightTime) && !IsValidHhmm(BrightnessNightTime))
            errors.Add($"brightness_night_time must be in HH:mm format (current: '{BrightnessNightTime}').");

        if (!string.IsNullOrWhiteSpace(BackupDir) && BackupDir.IndexOfAny(Path.GetInvalidPathChars()) >= 0)
            errors.Add($"backup_dir contains invalid path characters.");

        foreach (ProfileScheduleEntry entry in ProfileSchedules)
        {
            if (string.IsNullOrWhiteSpace(entry.Profile))
                errors.Add("A profile_schedule entry has an empty profile name.");
            if (string.IsNullOrWhiteSpace(entry.Trigger))
                errors.Add("A profile_schedule entry has an empty trigger.");
            if (entry.Trigger == "daily" && !string.IsNullOrEmpty(entry.Time) && !IsValidHhmm(entry.Time))
                errors.Add($"A daily profile_schedule has invalid time format '{entry.Time}' (expected HH:mm).");
        }

        return errors.AsReadOnly();
    }

    private static bool IsValidHhmm(string s)
    {
        if (s.Length != 5 || s[2] != ':')
            return false;
        return int.TryParse(s.AsSpan(0, 2), out int hh) && int.TryParse(s.AsSpan(3, 2), out int mm) && hh is >= 0 and <= 23 && mm is >= 0 and <= 59;
    }

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
        var json = JsonSerializer.Serialize(this, JsonOptions.Indented);
        File.WriteAllText(path, json);
    }
}

/// <summary>Represents one scheduled profile-switch entry persisted in AppConfig.</summary>
public sealed record ProfileScheduleEntry
{
    [JsonPropertyName("profile")]
    public required string Profile { get; init; }

    [JsonPropertyName("trigger")]
    public required string Trigger { get; init; } // "daily", "on_boot", "on_login"

    [JsonPropertyName("time")]
    public string Time { get; init; } = ""; // "HH:mm" for daily trigger

    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}
