#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
public sealed class AppConfigLoadNoArgBranchTests
{
    [Fact]
    public void Load_NullArgument_SetsPathFromDefaultConfigPath()
    {
        // No argument → path parameter is null → `path ??= DefaultConfigPath` T-branch.
        // Read-only call: does not write anything; returns default config if file absent.
        var config = AppConfig.Load();
        Assert.NotNull(config);
    }
}

// ── 7. ScheduledTweakService — null JSON (L82 T) and empty array (L82 F) ─────
//    ScheduledTweakService.cs L82:
//       `_schedules = JsonSerializer.Deserialize<List<TweakSchedule>>(json, JsonOptions) ?? [];`
//    Both branches currently at 0% (line never executed by any existing test).
//    · "null" JSON → Deserialize returns null → ?? [] fires (T-branch)
//    · "[]" JSON  → Deserialize returns empty List → ?? not needed (F-branch)

public sealed class AppConfigBrightnessBranchTests
{
    [Fact]
    public void Validate_BrightnessDayPctNegative_HasError_CoversBothBranches()
    {
        // BrightnessDayPct = -1 → left side of || is true → short-circuit → error added.
        // BrightnessDayTime = "12:99" → hh=12 (valid 0-23), mm=99 (> 59) → IsValidHhmm returns false.
        var config = new AppConfig { BrightnessDayPct = -1, BrightnessDayTime = "12:99" };
        var errors = config.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_day_pct"));
        Assert.Contains(errors, e => e.Contains("brightness_day_time"));
    }
}

// ── merged from BranchCoverage2Tests.cs ──────────────────────────────────

[Collection("Builtins")]
public sealed class AppConfigValidateBranchTests
{
    [Fact]
    public void Validate_ValidDefault_ReturnsNoErrors()
    {
        var cfg = new AppConfig();
        var errors = cfg.Validate();
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_MaxWorkersTooLow_ReturnsError()
    {
        var cfg = new AppConfig { MaxWorkers = 0 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("max_workers"));
    }

    [Fact]
    public void Validate_MaxWorkersTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { MaxWorkers = 33 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("max_workers"));
    }

    [Fact]
    public void Validate_MaxWorkersAtBoundary1_IsValid()
    {
        var errors = new AppConfig { MaxWorkers = 1 }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("max_workers"));
    }

    [Fact]
    public void Validate_MaxWorkersAtBoundary32_IsValid()
    {
        var errors = new AppConfig { MaxWorkers = 32 }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("max_workers"));
    }

    [Fact]
    public void Validate_EmptyTheme_ReturnsError()
    {
        var cfg = new AppConfig { Theme = "" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("theme"));
    }

    [Fact]
    public void Validate_EmptyLocale_ReturnsError()
    {
        var cfg = new AppConfig { Locale = "" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("locale"));
    }

    [Fact]
    public void Validate_FontSizeTooSmall_ReturnsError()
    {
        var cfg = new AppConfig { FontSize = 5f };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("font_size"));
    }

    [Fact]
    public void Validate_FontSizeTooLarge_ReturnsError()
    {
        var cfg = new AppConfig { FontSize = 37f };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("font_size"));
    }

    [Fact]
    public void Validate_FontSizeAtBoundary6_IsValid()
    {
        var errors = new AppConfig { FontSize = 6f }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("font_size"));
    }

    [Fact]
    public void Validate_FontSizeAtBoundary36_IsValid()
    {
        var errors = new AppConfig { FontSize = 36f }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("font_size"));
    }

    [Fact]
    public void Validate_DetailPanelHeightTooLow_ReturnsError()
    {
        var cfg = new AppConfig { DetailPanelHeight = 49 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("detail_panel_height"));
    }

    [Fact]
    public void Validate_DetailPanelHeightTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { DetailPanelHeight = 1601 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("detail_panel_height"));
    }

    [Fact]
    public void Validate_LogPanelHeightTooLow_ReturnsError()
    {
        var cfg = new AppConfig { LogPanelHeight = 49 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("log_panel_height"));
    }

    [Fact]
    public void Validate_LogPanelHeightTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { LogPanelHeight = 1601 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("log_panel_height"));
    }

    [Fact]
    public void Validate_HistoryMaxEntriesTooLow_ReturnsError()
    {
        var cfg = new AppConfig { HistoryMaxEntries = 9 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("history_max_entries"));
    }

    [Fact]
    public void Validate_HistoryMaxEntriesTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { HistoryMaxEntries = 100_001 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("history_max_entries"));
    }

    [Fact]
    public void Validate_AutoCleanMemoryThresholdNegative_ReturnsError()
    {
        var cfg = new AppConfig { AutoCleanMemoryThreshold = -1 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("auto_clean_memory_threshold"));
    }

    [Fact]
    public void Validate_AutoCleanMemoryThresholdTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { AutoCleanMemoryThreshold = 101 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("auto_clean_memory_threshold"));
    }

    [Fact]
    public void Validate_AutoCleanMemoryThreshold0_IsValid()
    {
        var errors = new AppConfig { AutoCleanMemoryThreshold = 0 }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("auto_clean_memory_threshold"));
    }

    [Fact]
    public void Validate_BrightnessDayPctTooHigh_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessDayPct = 101 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_day_pct"));
    }

    [Fact]
    public void Validate_BrightnessNightPctNegative_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessNightPct = -1 };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_night_pct"));
    }

    [Fact]
    public void Validate_InvalidBrightnessDayTime_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessDayTime = "BAD" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_day_time"));
    }

    [Fact]
    public void Validate_InvalidBrightnessNightTime_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessNightTime = "25:00" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_night_time"));
    }

    [Fact]
    public void Validate_EmptyBrightnessDayTime_IsValid()
    {
        // Empty string skips the format check
        var errors = new AppConfig { BrightnessDayTime = "" }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("brightness_day_time"));
    }

    [Fact]
    public void Validate_EmptyBrightnessNightTime_IsValid()
    {
        var errors = new AppConfig { BrightnessNightTime = "" }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("brightness_night_time"));
    }

    [Fact]
    public void Validate_ValidTimeFormat_IsValid()
    {
        var errors = new AppConfig { BrightnessDayTime = "07:30", BrightnessNightTime = "21:00" }.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("brightness"));
    }

    [Fact]
    public void Validate_BackupDirWithInvalidChars_ReturnsError()
    {
        string invalidDir = "C:\\path\0invalid";
        var cfg = new AppConfig { BackupDir = invalidDir };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("backup_dir"));
    }

    [Fact]
    public void Validate_ProfileScheduleEmptyProfile_ReturnsError()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules = [new ProfileScheduleEntry { Profile = "", Trigger = "on_boot" }];
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleEmptyTrigger_ReturnsError()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules = [new ProfileScheduleEntry { Profile = "minimal", Trigger = "" }];
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithInvalidTime_ReturnsError()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules =
        [
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "bad_time",
            },
        ];
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("daily profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithEmptyTime_IsValid()
    {
        // Empty time is allowed (means no time restriction for daily)
        var cfg = new AppConfig();
        cfg.ProfileSchedules =
        [
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "",
            },
        ];
        var errors = cfg.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("daily profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithValidTime_IsValid()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules =
        [
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "08:30",
            },
        ];
        var errors = cfg.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("daily profile_schedule"));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// AppConfig — portable mode & Load() edge cases
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class AppConfigPortableBranchTests
{
    [Fact]
    public void SetPortable_True_IsPortableReturnsTrue()
    {
        // Save and restore to avoid polluting other tests
        bool original = AppConfig.IsPortable;
        try
        {
            AppConfig.SetPortable(true);
            Assert.True(AppConfig.IsPortable);
            // ConfigDir should be the portable data dir
            Assert.Equal(AppConfig.PortableDataDir, AppConfig.ConfigDir);
        }
        finally
        {
            AppConfig.SetPortable(original);
        }
    }

    [Fact]
    public void SetPortable_False_IsPortableReturnsFalse()
    {
        bool original = AppConfig.IsPortable;
        try
        {
            AppConfig.SetPortable(false);
            Assert.False(AppConfig.IsPortable);
            // ConfigDir should be under LocalAppData
            Assert.Contains("RegiLattice", AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            AppConfig.SetPortable(original);
        }
    }

    [Fact]
    public void AutoDetectPortable_NoSentinelFile_PortableUnchanged()
    {
        bool original = AppConfig.IsPortable;
        try
        {
            AppConfig.SetPortable(false);
            AppConfig.AutoDetectPortable(); // No sentinel file → stays false
            Assert.False(AppConfig.IsPortable);
        }
        finally
        {
            AppConfig.SetPortable(original);
        }
    }

    [Fact]
    public void Load_FileNotExist_ReturnsDefaultConfig()
    {
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-notexist-{Guid.NewGuid()}.json");
        var cfg = AppConfig.Load(path);
        Assert.NotNull(cfg);
        Assert.Equal("en", cfg.Locale);
        Assert.Equal(8, cfg.MaxWorkers);
    }

    [Fact]
    public void Load_MalformedJson_ReturnsDefaultConfig()
    {
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            File.WriteAllText(path, "{ NOT VALID JSON ////// }");
            var cfg = AppConfig.Load(path);
            Assert.NotNull(cfg);
            // Returns default (not null) even on parse failure
            Assert.Equal("en", cfg.Locale);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Save_And_Load_RoundTrips()
    {
        var path = Path.Combine(Path.GetTempPath(), $"regilattice-test-{Guid.NewGuid()}.json");
        try
        {
            var cfg = new AppConfig
            {
                Theme = "nord",
                MaxWorkers = 4,
                Locale = "de",
            };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.Equal("nord", loaded.Theme);
            Assert.Equal(4, loaded.MaxWorkers);
            Assert.Equal("de", loaded.Locale);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// PackManager — filesystem branches (uses default packs dir — no injectable path)
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class AppConfigBranchTests : IDisposable
{
    private readonly string _tempDir;
    private readonly string _sentinelDir;

    public AppConfigBranchTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"RegiLattice_BC6_AppCfg_{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
        // Save portable state before tests
        _sentinelDir = string.Empty;
    }

    public void Dispose()
    {
        // Restore non-portable state after tests
        AppConfig.SetPortable(false);
        try
        {
            Directory.Delete(_tempDir, recursive: true);
        }
        catch { }
    }

    [Fact]
    public void AutoDetectPortable_SentinelFileExists_SetsPortableTrue()
    {
        // Create .portable sentinel at the exe's data dir
        var sentinelPath = Path.Combine(AppContext.BaseDirectory, "data", ".portable");
        bool existedBefore = File.Exists(sentinelPath);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(sentinelPath)!);
            File.WriteAllText(sentinelPath, "");
            AppConfig.SetPortable(false); // ensure clean state
            AppConfig.AutoDetectPortable();
            // Now portable is true — _isPortable=true covers line 206 T-branch
            Assert.StartsWith(AppContext.BaseDirectory, AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (!existedBefore && File.Exists(sentinelPath))
                File.Delete(sentinelPath);
            AppConfig.SetPortable(false);
        }
    }

    [Fact]
    public void Validate_InvalidBrightnessDayTime_ContainsError()
    {
        var cfg = new AppConfig { BrightnessDayTime = "99:99" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_day_time"));
    }

    [Fact]
    public void Validate_InvalidBrightnessNightTime_ContainsError()
    {
        var cfg = new AppConfig { BrightnessNightTime = "25:00" };
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("brightness_night_time"));
    }

    [Fact]
    public void Load_NullJson_ReturnsDefaultConfig()
    {
        string path = Path.Combine(_tempDir, "null_cfg.json");
        File.WriteAllText(path, "null");
        var cfg = AppConfig.Load(path);
        // Deserialize<AppConfig>("null") returns null → ?? new AppConfig() covers T-branch
        Assert.NotNull(cfg);
    }

    [Fact]
    public void Load_NonExistentFile_ReturnsDefaultConfig()
    {
        string path = Path.Combine(_tempDir, "missing_cfg.json");
        var cfg = AppConfig.Load(path);
        Assert.NotNull(cfg);
    }
}

// ── 5. PackLoader — null JSON throw branch, bad-prefix validation ─────────────

