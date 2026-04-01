// RegiLattice.Core.Tests — AppConfigValidationTests.cs
// Tests for AppConfig.Validate().

using RegiLattice.Core;
using Xunit;
using RegiLattice.Core.Services;

namespace RegiLattice.Core.Tests;

public sealed class AppConfigValidationTests
{
    // ── Valid default config ─────────────────────────────────────────────

    [Fact]
    public void Validate_DefaultConfig_ReturnsNoErrors()
    {
        var cfg = new AppConfig();
        Assert.Empty(cfg.Validate());
    }

    // ── MaxWorkers ───────────────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(8)]
    [InlineData(32)]
    public void Validate_MaxWorkers_ValidValues_NoError(int value)
    {
        var cfg = new AppConfig { MaxWorkers = value };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("max_workers"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(33)]
    [InlineData(100)]
    public void Validate_MaxWorkers_InvalidValues_ReturnsError(int value)
    {
        var cfg = new AppConfig { MaxWorkers = value };
        Assert.Contains(cfg.Validate(), e => e.Contains("max_workers"));
    }

    // ── Theme / Locale ───────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyTheme_ReturnsError()
    {
        var cfg = new AppConfig { Theme = "" };
        Assert.Contains(cfg.Validate(), e => e.Contains("theme"));
    }

    [Fact]
    public void Validate_EmptyLocale_ReturnsError()
    {
        var cfg = new AppConfig { Locale = "" };
        Assert.Contains(cfg.Validate(), e => e.Contains("locale"));
    }

    // ── FontSize ─────────────────────────────────────────────────────────

    [Theory]
    [InlineData(6f)]
    [InlineData(9f)]
    [InlineData(36f)]
    public void Validate_FontSize_ValidValues_NoError(float value)
    {
        var cfg = new AppConfig { FontSize = value };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("font_size"));
    }

    [Theory]
    [InlineData(5f)]
    [InlineData(37f)]
    [InlineData(0f)]
    public void Validate_FontSize_InvalidValues_ReturnsError(float value)
    {
        var cfg = new AppConfig { FontSize = value };
        Assert.Contains(cfg.Validate(), e => e.Contains("font_size"));
    }

    // ── DetailPanelHeight ────────────────────────────────────────────────

    [Fact]
    public void Validate_DetailPanelHeight_TooSmall_ReturnsError()
    {
        var cfg = new AppConfig { DetailPanelHeight = 10 };
        Assert.Contains(cfg.Validate(), e => e.Contains("detail_panel_height"));
    }

    [Fact]
    public void Validate_DetailPanelHeight_ValidValue_NoError()
    {
        var cfg = new AppConfig { DetailPanelHeight = 130 };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("detail_panel_height"));
    }

    // ── LogPanelHeight ───────────────────────────────────────────────────

    [Fact]
    public void Validate_LogPanelHeight_TooLarge_ReturnsError()
    {
        var cfg = new AppConfig { LogPanelHeight = 2000 };
        Assert.Contains(cfg.Validate(), e => e.Contains("log_panel_height"));
    }

    // ── HistoryMaxEntries ────────────────────────────────────────────────

    [Fact]
    public void Validate_HistoryMaxEntries_BelowMinimum_ReturnsError()
    {
        var cfg = new AppConfig { HistoryMaxEntries = 5 };
        Assert.Contains(cfg.Validate(), e => e.Contains("history_max_entries"));
    }

    [Fact]
    public void Validate_HistoryMaxEntries_Valid_NoError()
    {
        var cfg = new AppConfig { HistoryMaxEntries = 500 };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("history_max_entries"));
    }

    // ── AutoCleanMemoryThreshold ─────────────────────────────────────────

    [Fact]
    public void Validate_AutoCleanMemoryThreshold_Above100_ReturnsError()
    {
        var cfg = new AppConfig { AutoCleanMemoryThreshold = 101 };
        Assert.Contains(cfg.Validate(), e => e.Contains("auto_clean_memory_threshold"));
    }

    [Fact]
    public void Validate_AutoCleanMemoryThreshold_Zero_NoError()
    {
        var cfg = new AppConfig { AutoCleanMemoryThreshold = 0 };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("auto_clean_memory_threshold"));
    }

    // ── BrightnessPct ────────────────────────────────────────────────────

    [Fact]
    public void Validate_BrightnessDayPct_Above100_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessDayPct = 110 };
        Assert.Contains(cfg.Validate(), e => e.Contains("brightness_day_pct"));
    }

    [Fact]
    public void Validate_BrightnessNightPct_Negative_ReturnsError()
    {
        var cfg = new AppConfig { BrightnessNightPct = -1 };
        Assert.Contains(cfg.Validate(), e => e.Contains("brightness_night_pct"));
    }

    // ── Brightness times ─────────────────────────────────────────────────

    [Theory]
    [InlineData("00:00")]
    [InlineData("07:30")]
    [InlineData("23:59")]
    public void Validate_BrightnessDayTime_ValidFormats_NoError(string time)
    {
        var cfg = new AppConfig { BrightnessDayTime = time };
        Assert.DoesNotContain(cfg.Validate(), e => e.Contains("brightness_day_time"));
    }

    [Theory]
    [InlineData("7:00")] // missing leading zero
    [InlineData("25:00")] // invalid hour
    [InlineData("07:60")] // invalid minute
    [InlineData("07-00")] // wrong separator
    [InlineData("noon")] // non-numeric
    public void Validate_BrightnessDayTime_InvalidFormats_ReturnsError(string time)
    {
        var cfg = new AppConfig { BrightnessDayTime = time };
        Assert.Contains(cfg.Validate(), e => e.Contains("brightness_day_time"));
    }

    // ── ProfileSchedules ─────────────────────────────────────────────────

    [Fact]
    public void Validate_ProfileScheduleEntry_EmptyProfile_ReturnsError()
    {
        var cfg = new AppConfig
        {
            ProfileSchedules =
            [
                new ProfileScheduleEntry
                {
                    Profile = "",
                    Trigger = "daily",
                    Time = "08:00",
                },
            ],
        };
        Assert.Contains(cfg.Validate(), e => e.Contains("profile_schedule") && e.Contains("profile name"));
    }

    [Fact]
    public void Validate_ProfileScheduleEntry_DailyInvalidTime_ReturnsError()
    {
        var cfg = new AppConfig
        {
            ProfileSchedules =
            [
                new ProfileScheduleEntry
                {
                    Profile = "gaming",
                    Trigger = "daily",
                    Time = "bad",
                },
            ],
        };
        Assert.Contains(cfg.Validate(), e => e.Contains("daily profile_schedule") && e.Contains("bad"));
    }

    [Fact]
    public void Validate_ProfileScheduleEntry_ValidDailyEntry_NoError()
    {
        var cfg = new AppConfig
        {
            ProfileSchedules =
            [
                new ProfileScheduleEntry
                {
                    Profile = "gaming",
                    Trigger = "daily",
                    Time = "22:00",
                },
            ],
        };
        Assert.Empty(cfg.Validate());
    }

    // ── Multiple errors returned ─────────────────────────────────────────

    [Fact]
    public void Validate_MultipleInvalidFields_ReturnsMultipleErrors()
    {
        var cfg = new AppConfig
        {
            MaxWorkers = 0,
            Theme = "",
            FontSize = 100f,
        };
        var errors = cfg.Validate();
        Assert.True(errors.Count >= 3);
    }
}
// ── merged from AppConfigPortableTests.cs ──────────────────────────────────
/// <summary>AppConfig portable mode tests.</summary>
public sealed class AppConfigPortableTests
{
    // Reset portable state after every test — IsPortable is a static field.
    private static void ResetPortable()
    {
        AppConfig.SetPortable(false);
    }

    // ── Default state ─────────────────────────────────────────────────────

    [Fact]
    public void IsPortable_Default_IsFalse()
    {
        ResetPortable();
        Assert.False(AppConfig.IsPortable);
    }

    [Fact]
    public void ConfigDir_Default_ContainsRegiLattice()
    {
        ResetPortable();
        Assert.Contains("RegiLattice", AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigDir_Default_ContainsLocalAppData()
    {
        ResetPortable();
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Assert.StartsWith(localAppData, AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
    }

    // ── SetPortable(true) ────────────────────────────────────────────────

    [Fact]
    public void SetPortable_True_IsPortableBecomesTrue()
    {
        try
        {
            AppConfig.SetPortable(true);
            Assert.True(AppConfig.IsPortable);
        }
        finally
        {
            ResetPortable();
        }
    }

    [Fact]
    public void SetPortable_True_ConfigDirEqualsPortableDataDir()
    {
        try
        {
            AppConfig.SetPortable(true);
            // ConfigDir must equal the portable sibling directory, not the AppData path.
            Assert.True(
                AppConfig.ConfigDir.Equals(AppConfig.PortableDataDir, StringComparison.OrdinalIgnoreCase),
                $"Expected ConfigDir '{AppConfig.ConfigDir}' to equal PortableDataDir '{AppConfig.PortableDataDir}'"
            );
        }
        finally
        {
            ResetPortable();
        }
    }

    [Fact]
    public void SetPortable_False_ConfigDirUsesAppData()
    {
        try
        {
            AppConfig.SetPortable(true);
            AppConfig.SetPortable(false);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Assert.StartsWith(localAppData, AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            ResetPortable();
        }
    }

    // ── PortableDataDir value ─────────────────────────────────────────────

    [Fact]
    public void PortableDataDir_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(AppConfig.PortableDataDir));
    }

    [Fact]
    public void PortableDataDir_ContainsData()
    {
        // Convention: portable dir is named "data" sibling of the exe
        Assert.Contains("data", AppConfig.PortableDataDir, StringComparison.OrdinalIgnoreCase);
    }

    // ── AutoDetectPortable — no sentinel file ────────────────────────────

    [Fact]
    public void AutoDetectPortable_NoSentinelFile_IsPortableFalse()
    {
        try
        {
            // Ensure we start clean (no sentinel in test runner dir)
            ResetPortable();
            AppConfig.AutoDetectPortable();
            // There is no portable.dat sentinel file next to the test runner,
            // so portable mode should remain off by default.
            Assert.False(AppConfig.IsPortable);
        }
        finally
        {
            ResetPortable();
        }
    }

    // ── Round-trip: explicit path, fields preserved ───────────────────────

    [Fact]
    public void Load_WithExplicitPath_PreservesAllConfigFields()
    {
        // Use an explicit tmp path to avoid polluting any system directory.
        var tmpPath = Path.Combine(Path.GetTempPath(), $"RL_Test_{Guid.NewGuid():N}.json");
        try
        {
            var saved = new AppConfig { Theme = "nord" };
            saved.Save(tmpPath);

            var reloaded = AppConfig.Load(tmpPath);
            Assert.Equal("nord", reloaded.Theme);
        }
        finally
        {
            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
        }
    }
}
