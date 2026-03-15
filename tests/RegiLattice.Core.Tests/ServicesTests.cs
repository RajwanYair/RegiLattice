using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for AppConfig, Locale, Ratings, Analytics, and Elevation services.</summary>
public sealed class AppConfigTests
{
    [Fact]
    public void Default_HasExpectedDefaults()
    {
        var cfg = new AppConfig();
        Assert.False(cfg.ForceCorp);
        Assert.Equal(8, cfg.MaxWorkers);
        Assert.True(cfg.AutoBackup);
        Assert.Equal("catppuccin-mocha", cfg.Theme);
        Assert.Equal("en", cfg.Locale);
        Assert.True(cfg.CheckToolUpdates);
    }

    [Fact]
    public void Load_MissingFile_ReturnsDefaults()
    {
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nope.json"));
        Assert.Equal("catppuccin-mocha", cfg.Theme);
    }

    [Fact]
    public void SaveAndLoad_RoundTrips()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-test-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "config.json");
        try
        {
            var cfg = new AppConfig
            {
                Theme = "nord",
                MaxWorkers = 16,
                ForceCorp = true,
            };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.Equal("nord", loaded.Theme);
            Assert.Equal(16, loaded.MaxWorkers);
            Assert.True(loaded.ForceCorp);
        }
        finally
        {
            try
            {
                Directory.Delete(dir, true);
            }
            catch { }
        }
    }

    [Fact]
    public void ConfigDir_IsInLocalAppData()
    {
        Assert.Contains("RegiLattice", AppConfig.ConfigDir);
    }
}

public sealed class LocaleTests
{
    [Fact]
    public void T_KnownKey_ReturnsTranslation()
    {
        Locale.SetLocale("en");
        Assert.Equal("Apply All", Locale.T("apply_all"));
    }

    [Fact]
    public void T_UnknownKey_ReturnsKeyItself()
    {
        Assert.Equal("nonexistent_key", Locale.T("nonexistent_key"));
    }

    [Fact]
    public void T_WithFormatArgs_FormatsString()
    {
        Assert.Equal("Apply 5 selected tweaks?", Locale.T("confirm_apply", 5));
    }

    [Fact]
    public void SetLocale_WithOverrides_ReplacesValue()
    {
        Locale.SetLocale("custom", new Dictionary<string, string> { ["apply_all"] = "Anwenden" });
        Assert.Equal("Anwenden", Locale.T("apply_all"));
        Locale.SetLocale("en"); // reset
    }

    [Fact]
    public void CurrentLocale_ReflectsSetLocale()
    {
        Locale.SetLocale("fr");
        Assert.Equal("fr", Locale.CurrentLocale);
        Locale.SetLocale("en");
    }

    [Fact]
    public void AvailableKeys_NotEmpty()
    {
        Assert.True(Locale.AvailableKeys.Count > 0);
    }
}

public sealed class RatingsTests
{
    private static string SetupTempRatings()
    {
        // Ratings uses AppConfig.ConfigDir static path — we can't easily redirect it.
        // Instead, we test the public API (which uses the default path).
        return $"test-{Guid.NewGuid():N}";
    }

    [Fact]
    public void Rate_ValidStars_CreatesRating()
    {
        var id = SetupTempRatings();
        try
        {
            Ratings.Rate(id, 4, "good");
            var r = Ratings.GetRating(id);
            Assert.NotNull(r);
            Assert.Equal(4, r.Stars);
            Assert.Equal("good", r.Note);
        }
        finally
        {
            Ratings.RemoveRating(id);
        }
    }

    [Fact]
    public void Rate_InvalidStars_Throws()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Ratings.Rate("x", 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Ratings.Rate("x", 6));
    }

    [Fact]
    public void GetRating_Missing_ReturnsNull()
    {
        Assert.Null(Ratings.GetRating($"missing-{Guid.NewGuid():N}"));
    }

    [Fact]
    public void RemoveRating_DeletesEntry()
    {
        var id = $"rm-{Guid.NewGuid():N}";
        Ratings.Rate(id, 3);
        Ratings.RemoveRating(id);
        Assert.Null(Ratings.GetRating(id));
    }

    [Fact]
    public void TopRated_ReturnsOrderedByStars()
    {
        var id1 = $"tr1-{Guid.NewGuid():N}";
        var id2 = $"tr2-{Guid.NewGuid():N}";
        try
        {
            Ratings.Rate(id1, 2);
            Ratings.Rate(id2, 5);
            var top = Ratings.TopRated(100);
            var idx1 = top.ToList().FindIndex(t => t.Id == id1);
            var idx2 = top.ToList().FindIndex(t => t.Id == id2);
            if (idx1 >= 0 && idx2 >= 0)
                Assert.True(idx2 < idx1);
        }
        finally
        {
            Ratings.RemoveRating(id1);
            Ratings.RemoveRating(id2);
        }
    }
}

public sealed class AnalyticsTests
{
    [Fact]
    public void GetStats_Defaults()
    {
        var stats = Analytics.GetStats();
        Assert.NotNull(stats);
        Assert.True(stats.TotalApplies >= 0);
    }

    [Fact]
    public void RecordApply_IncrementsTotalApplies()
    {
        var before = Analytics.GetStats().TotalApplies;
        Analytics.RecordApply($"test-{Guid.NewGuid():N}");
        var after = Analytics.GetStats().TotalApplies;
        Assert.Equal(before + 1, after);
    }

    [Fact]
    public void TopTweaks_ReturnsOrderedList()
    {
        var top = Analytics.TopTweaks(5);
        Assert.NotNull(top);
        Assert.True(top.Count <= 5);
    }

    [Fact]
    public void RecordRemove_IncrementsTotalRemoves()
    {
        var before = Analytics.GetStats().TotalRemoves;
        Analytics.RecordRemove($"test-{Guid.NewGuid():N}");
        var after = Analytics.GetStats().TotalRemoves;
        Assert.Equal(before + 1, after);
    }

    [Fact]
    public void RecordError_IncrementsTotalErrors()
    {
        var before = Analytics.GetStats().TotalErrors;
        Analytics.RecordError($"test-{Guid.NewGuid():N}");
        var after = Analytics.GetStats().TotalErrors;
        Assert.Equal(before + 1, after);
    }

    [Fact]
    public void RecordSession_IncrementsTotalSessions()
    {
        var before = Analytics.GetStats().TotalSessions;
        Analytics.RecordSession();
        var after = Analytics.GetStats().TotalSessions;
        Assert.Equal(before + 1, after);
    }

    [Fact]
    public void Flush_PersistsData()
    {
        // Record something unique so we know it persisted
        var uniqueId = $"flush-test-{Guid.NewGuid():N}";
        Analytics.RecordApply(uniqueId);
        Analytics.Flush();

        // Verify the analytics file exists
        var filePath = Path.Combine(AppConfig.ConfigDir, "analytics.json");
        Assert.True(File.Exists(filePath));

        // Read and verify the unique ID is in the persisted data
        var json = File.ReadAllText(filePath);
        Assert.Contains(uniqueId, json);
    }

    [Fact]
    public void Flush_NoDirtyData_DoesNotThrow()
    {
        // Call Flush when nothing has been recorded — should be a no-op
        Analytics.Flush();
    }
}

public sealed class ElevationTests
{
    [Fact]
    public void IsAdmin_ReturnsBool()
    {
        // Just ensure it doesn't throw — actual value depends on environment
        _ = Elevation.IsAdmin();
    }

    [Fact]
    public void AssertAdmin_WhenNotRequired_DoesNotThrow()
    {
        Elevation.AssertAdmin(requireAdmin: false);
    }

    [Fact]
    public void RunElevated_DisallowedCommand_Throws()
    {
        Assert.Throws<UnauthorizedAccessException>(() => Elevation.RunElevated("arbitrary-command.exe", []));
    }

    [Fact]
    public void RunElevated_AllowedCommand_DoesNotThrowAuth()
    {
        // 'reg' is in allowlist — it will try to run but won't throw UnauthorizedAccessException
        try
        {
            Elevation.RunElevated("reg", ["query", @"HKCU\Software"]);
        }
        catch (UnauthorizedAccessException)
        {
            Assert.Fail("Should not throw for allowed command");
        }
        catch
        { /* other exceptions OK (e.g. process issues) */
        }
    }
}

/// <summary>Tests for HardwareInfo service.</summary>
public sealed class HardwareInfoTests
{
    [Fact]
    public void DetectHardware_ReturnsNonNull()
    {
        var info = HardwareInfo.DetectHardware();
        Assert.NotNull(info);
    }

    [Fact]
    public void Summary_ReturnsNonEmptyString()
    {
        var summary = HardwareInfo.Summary();
        Assert.NotNull(summary);
        Assert.NotEmpty(summary);
    }

    [Fact]
    public void SuggestProfile_ReturnsValidProfile()
    {
        string profile = HardwareInfo.SuggestProfile();
        Assert.NotNull(profile);
        Assert.Contains(profile, new[] { "business", "gaming", "privacy", "minimal", "server" });
    }

    [Fact]
    public void IsEdgeInstalled_ReturnsBool()
    {
        bool result = HardwareInfo.IsEdgeInstalled();
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void DetectHardware_HasCpuAndMemory()
    {
        var hw = HardwareInfo.DetectHardware();
        Assert.NotNull(hw.Cpu);
        Assert.NotNull(hw.Memory);
        Assert.True(hw.Memory.TotalMb > 0, "Expected positive RAM");
        Assert.NotEmpty(hw.Cpu.Name);
    }
}

/// <summary>Tests for CorporateGuard service.</summary>
public sealed class CorporateGuardTests
{
    [Fact]
    public void IsCorporateNetwork_ReturnsBool()
    {
        // Should not throw — returns true/false
        bool result = CorporateGuard.IsCorporateNetwork();
        // Just verifying it runs without exception
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void Status_ReturnsTuple()
    {
        var (isCorp, reason) = CorporateGuard.Status();
        Assert.IsType<bool>(isCorp);
        Assert.NotNull(reason);
    }

    [Fact]
    public void IsGpoManaged_ReturnsBool()
    {
        bool result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Policies\Microsoft\Windows"]);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void ClearCache_DoesNotThrow()
    {
        CorporateGuard.ClearCache();
        // Second call should also work
        CorporateGuard.ClearCache();
    }
}
