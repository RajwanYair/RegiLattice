// RegiLattice.Core.Tests — AppConfigValidationTests.cs
// Tests for AppConfig.Validate() — Sprint 110.

using RegiLattice.Core;
using Xunit;

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
