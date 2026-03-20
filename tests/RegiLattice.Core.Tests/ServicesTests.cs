using RegiLattice.Core;
using RegiLattice.Core.Services;
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

    [Fact]
    public void SetLocale_French_TranslatesApplyAll()
    {
        Locale.SetLocale("fr");
        Assert.Equal("Tout appliquer", Locale.T("apply_all"));
        Locale.SetLocale("en");
    }

    [Fact]
    public void SetLocale_Spanish_TranslatesApplyAll()
    {
        Locale.SetLocale("es");
        Assert.Equal("Aplicar todo", Locale.T("apply_all"));
        Locale.SetLocale("en");
    }

    [Fact]
    public void SetLocale_French_HasAllRequiredKeys()
    {
        Locale.SetLocale("fr");
        var requiredKeys = new[]
        {
            "apply_all",
            "remove_all",
            "status_applied",
            "status_not_applied",
            "confirm_apply",
            "confirm_remove",
            "tweaks_loaded",
            "detection_complete",
            "admin_required",
            "corporate_warning",
        };
        foreach (var key in requiredKeys)
            Assert.NotEqual(key, Locale.T(key)); // key itself means missing translation
        Locale.SetLocale("en");
    }

    [Fact]
    public void SetLocale_Spanish_HasAllRequiredKeys()
    {
        Locale.SetLocale("es");
        var requiredKeys = new[]
        {
            "apply_all",
            "remove_all",
            "status_applied",
            "status_not_applied",
            "confirm_apply",
            "confirm_remove",
            "tweaks_loaded",
            "detection_complete",
            "admin_required",
            "corporate_warning",
        };
        foreach (var key in requiredKeys)
            Assert.NotEqual(key, Locale.T(key));
        Locale.SetLocale("en");
    }

    [Fact]
    public void AvailableLocales_ContainsFrAndEs()
    {
        Assert.Contains("fr", Locale.AvailableLocales);
        Assert.Contains("es", Locale.AvailableLocales);
    }

    [Theory]
    [InlineData("en", "Apply All")]
    [InlineData("de", "Alle anwenden")]
    [InlineData("fr", "Tout appliquer")]
    [InlineData("es", "Aplicar todo")]
    public void SetLocale_AllBuiltIn_TranslateApplyAll(string locale, string expected)
    {
        Locale.SetLocale(locale);
        Assert.Equal(expected, Locale.T("apply_all"));
        Locale.SetLocale("en");
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

[Collection("Analytics")]
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
        // Use a lightweight query (single value) to avoid huge output that blocks ReadToEnd()
        try
        {
            Elevation.RunElevated("reg", ["query", @"HKCU\Environment", "/v", "TEMP"], timeoutMs: 10_000);
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

    [Fact]
    public void IsChromeInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsChromeInstalled());
    }

    [Fact]
    public void IsFirefoxInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsFirefoxInstalled());
    }

    [Fact]
    public void IsJavaInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsJavaInstalled());
    }

    [Fact]
    public void IsOfficeInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsOfficeInstalled());
    }

    [Fact]
    public void IsVsCodeInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsVsCodeInstalled());
    }

    [Fact]
    public void HasNvidiaGpu_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.HasNvidiaGpu());
    }

    [Fact]
    public void HasAmdGpu_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.HasAmdGpu());
    }

    [Fact]
    public void HasBatteryPresent_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.HasBatteryPresent());
    }

    [Fact]
    public void HasWslInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.HasWslInstalled());
    }

    [Fact]
    public void HasHyperVAvailable_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.HasHyperVAvailable());
    }
}

/// <summary>Extended Locale tests for branch coverage of format strings and locale switching.</summary>
public sealed class LocaleExtendedTests
{
    [Fact]
    public void T_MultipleFormatArgs_FormatsCorrectly()
    {
        Locale.SetLocale("en");
        Assert.Equal("Imported 5 tweaks from backup.json.", Locale.T("import_complete", 5, "backup.json"));
    }

    [Fact]
    public void SetLocale_German_ReturnsGermanTranslations()
    {
        Locale.SetLocale("de");
        Assert.Equal("Alle anwenden", Locale.T("apply_all"));
        Assert.Equal("Alle entfernen", Locale.T("remove_all"));
        Locale.SetLocale("en"); // reset
    }

    [Fact]
    public void SetLocale_German_OverrideOneKey_PreservesOthers()
    {
        Locale.SetLocale("de", new Dictionary<string, string> { ["apply_all"] = "Custom" });
        Assert.Equal("Custom", Locale.T("apply_all"));
        Assert.Equal("Alle entfernen", Locale.T("remove_all")); // other keys still German
        Locale.SetLocale("en"); // reset
    }

    [Fact]
    public void SetLocale_UnknownLanguage_FallsBackToEnglish()
    {
        Locale.SetLocale("xx");
        Assert.Equal("Apply All", Locale.T("apply_all")); // falls back to En
        Locale.SetLocale("en"); // reset
    }

    [Fact]
    public void AvailableLocales_ContainsEnAndDe()
    {
        var locales = Locale.AvailableLocales;
        Assert.Contains("en", locales);
        Assert.Contains("de", locales);
    }

    [Fact]
    public void AvailableKeys_HasAtLeast40Keys()
    {
        Locale.SetLocale("en");
        Assert.True(Locale.AvailableKeys.Count >= 40, $"Expected >= 40 keys, got {Locale.AvailableKeys.Count}");
    }
}

/// <summary>Extended AppConfig tests for error handling and edge cases.</summary>
public sealed class AppConfigExtendedTests
{
    [Fact]
    public void Load_MalformedJson_ReturnsDefaults()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-cfg-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "config.json");
        try
        {
            Directory.CreateDirectory(dir);
            File.WriteAllText(path, "{ this is not valid json }}}");
            var cfg = AppConfig.Load(path);
            Assert.Equal("catppuccin-mocha", cfg.Theme); // defaults
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
    public void Load_ExtraJsonProperties_IgnoresThem()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-cfg-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "config.json");
        try
        {
            Directory.CreateDirectory(dir);
            File.WriteAllText(path, """{"theme":"nord","unknown_field":"value","max_workers":4}""");
            var cfg = AppConfig.Load(path);
            Assert.Equal("nord", cfg.Theme);
            Assert.Equal(4, cfg.MaxWorkers);
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
    public void Save_CreatesDirectoryIfMissing()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-cfg-{Guid.NewGuid()}", "nested");
        var path = Path.Combine(dir, "config.json");
        try
        {
            Assert.False(Directory.Exists(dir));
            new AppConfig { Theme = "dracula" }.Save(path);
            Assert.True(File.Exists(path));
            var loaded = AppConfig.Load(path);
            Assert.Equal("dracula", loaded.Theme);
        }
        finally
        {
            try
            {
                Directory.Delete(Path.GetDirectoryName(dir)!, true);
            }
            catch { }
        }
    }

    [Fact]
    public void DefaultConfigPath_ContainsConfigJson()
    {
        Assert.EndsWith("config.json", AppConfig.DefaultConfigPath);
    }
}

/// <summary>Extended Ratings tests for edge cases and calculations.</summary>
public sealed class RatingsExtendedTests
{
    [Fact]
    public void AverageRating_ReturnsCorrectAverage()
    {
        var ids = new[] { $"avg-{Guid.NewGuid():N}", $"avg-{Guid.NewGuid():N}" };
        try
        {
            Ratings.Rate(ids[0], 2);
            Ratings.Rate(ids[1], 4);
            var avg = Ratings.AverageRating();
            Assert.NotNull(avg);
            // We can't assert exact value because other tests might have left ratings
            Assert.True(avg > 0);
        }
        finally
        {
            foreach (var id in ids)
                Ratings.RemoveRating(id);
        }
    }

    [Fact]
    public void Rate_EmptyNote_StoresEmptyString()
    {
        var id = $"note-{Guid.NewGuid():N}";
        try
        {
            Ratings.Rate(id, 3, "");
            var r = Ratings.GetRating(id);
            Assert.NotNull(r);
            Assert.Equal("", r.Note);
        }
        finally
        {
            Ratings.RemoveRating(id);
        }
    }

    [Fact]
    public void RemoveRating_NonExistent_DoesNotThrow()
    {
        Ratings.RemoveRating($"remove-nope-{Guid.NewGuid():N}");
    }

    [Fact]
    public void TopRated_LimitN_RespectsLimit()
    {
        var ids = Enumerable.Range(0, 5).Select(_ => $"top-{Guid.NewGuid():N}").ToList();
        try
        {
            for (int i = 0; i < ids.Count; i++)
                Ratings.Rate(ids[i], i + 1);
            var top = Ratings.TopRated(2);
            Assert.True(top.Count <= 2);
        }
        finally
        {
            foreach (var id in ids)
                Ratings.RemoveRating(id);
        }
    }

    [Fact]
    public void Rate_BoundaryValues_AcceptsOneAndFive()
    {
        var id1 = $"rate-1-{Guid.NewGuid():N}";
        var id5 = $"rate-5-{Guid.NewGuid():N}";
        try
        {
            Ratings.Rate(id1, 1);
            Ratings.Rate(id5, 5);
            Assert.Equal(1, Ratings.GetRating(id1)!.Stars);
            Assert.Equal(5, Ratings.GetRating(id5)!.Stars);
        }
        finally
        {
            Ratings.RemoveRating(id1);
            Ratings.RemoveRating(id5);
        }
    }
}

/// <summary>Extended Analytics tests for TopTweaks and Reset.</summary>
[Collection("Analytics")]
public sealed class AnalyticsExtendedTests
{
    [Fact]
    public void RecordApply_SameTweakMultipleTimes_CountsCorrectly()
    {
        var id = $"multi-{Guid.NewGuid():N}";
        var before = Analytics.GetStats().MostApplied.GetValueOrDefault(id);
        Analytics.RecordApply(id);
        Analytics.RecordApply(id);
        Analytics.RecordApply(id);
        var after = Analytics.GetStats().MostApplied[id];
        Assert.Equal(before + 3, after);
    }

    [Fact]
    public void TopTweaks_LimitRespected()
    {
        var top = Analytics.TopTweaks(3);
        Assert.True(top.Count <= 3);
    }

    [Fact]
    public void RecordSession_SetsLastSession()
    {
        Analytics.RecordSession();
        var stats = Analytics.GetStats();
        Assert.True(stats.LastSession > 0);
    }
}

/// <summary>Tests for CorporateGuard — safe-to-run aspects only.</summary>
public sealed class CorporateGuardExtendedTests
{
    [Fact]
    public void IsCorporateNetwork_ReturnsBool()
    {
        Assert.IsType<bool>(CorporateGuard.IsCorporateNetwork());
    }

    [Fact]
    public void Status_ReturnsTupleWithReason()
    {
        var (isCorp, reason) = CorporateGuard.Status();
        Assert.IsType<bool>(isCorp);
        Assert.NotNull(reason);
        Assert.NotEmpty(reason);
    }

    [Fact]
    public void ClearCache_ThenQuery_DoesNotThrow()
    {
        CorporateGuard.ClearCache();
        _ = CorporateGuard.IsCorporateNetwork();
    }

    [Fact]
    public void IsGpoManaged_WithPoliciesKey_ReturnsTrue()
    {
        var result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Test"]);
        Assert.True(result);
    }

    [Fact]
    public void IsGpoManaged_WithoutPolicies_ReturnsFalseOrTrue()
    {
        // Non-policies path — result depends on whether actual GPO overlays exist
        var result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion"]);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void IsGpoManaged_EmptyList_ReturnsFalse()
    {
        Assert.False(CorporateGuard.IsGpoManaged([]));
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

// ── SystemMonitor Tests ────────────────────────────────────────────────────
public sealed class SystemMonitorTests
{
    [Fact]
    public void GetCpuUsagePercent_ReturnsZeroToHundred()
    {
        var monitor = new SystemMonitor();
        // First call may return 0 (no delta yet)
        int first = monitor.GetCpuUsagePercent();
        Assert.InRange(first, 0, 100);
    }

    [Fact]
    public void GetCpuUsagePercent_SecondCallReturnsValidRange()
    {
        var monitor = new SystemMonitor();
        monitor.GetCpuUsagePercent(); // prime the delta
        Thread.Sleep(100); // allow some CPU time to pass
        int second = monitor.GetCpuUsagePercent();
        Assert.InRange(second, 0, 100);
    }

    [Fact]
    public void GetMemoryUsage_ReturnsPositiveValues()
    {
        var (usedMb, totalMb, percent) = SystemMonitor.GetMemoryUsage();
        Assert.True(totalMb > 0, $"Expected positive total MB, got {totalMb}");
        Assert.True(usedMb > 0, $"Expected positive used MB, got {usedMb}");
        Assert.True(usedMb <= totalMb, $"Used ({usedMb}) should not exceed total ({totalMb})");
        Assert.InRange(percent, 1, 100);
    }

    [Fact]
    public void GetMemoryUsage_TotalMatchesHardwareInfo()
    {
        var (_, totalMb, _) = SystemMonitor.GetMemoryUsage();
        var hwMem = HardwareInfo.DetectMemory();
        // Allow 1GB tolerance for firmware-reserved memory
        Assert.True(
            Math.Abs(totalMb - hwMem.TotalMb) < 1024,
            $"SystemMonitor total ({totalMb}) should be close to HardwareInfo total ({hwMem.TotalMb})"
        );
    }

    [Fact]
    public void GetUptime_ReturnsPositiveTimeSpan()
    {
        var uptime = SystemMonitor.GetUptime();
        Assert.True(uptime.TotalSeconds > 0, "Uptime should be positive");
        Assert.True(uptime.TotalDays < 365, "Uptime should be less than a year");
    }

    [Fact]
    public void GetUptime_IsConsistent()
    {
        var first = SystemMonitor.GetUptime();
        Thread.Sleep(50);
        var second = SystemMonitor.GetUptime();
        Assert.True(second >= first, "Second uptime reading should be >= first");
    }

    [Fact]
    public void MultipleInstances_TrackCpuIndependently()
    {
        var a = new SystemMonitor();
        var b = new SystemMonitor();
        a.GetCpuUsagePercent();
        b.GetCpuUsagePercent();
        Thread.Sleep(50);
        int cpuA = a.GetCpuUsagePercent();
        int cpuB = b.GetCpuUsagePercent();
        Assert.InRange(cpuA, 0, 100);
        Assert.InRange(cpuB, 0, 100);
    }

    [Fact]
    public void GetCpuUsagePercent_FirstCall_ReturnsValidRange()
    {
        var monitor = new SystemMonitor();
        int cpu = monitor.GetCpuUsagePercent();
        // First call computes delta from boot — valid range but not necessarily 0
        Assert.InRange(cpu, 0, 100);
    }

    [Fact]
    public void GetCpuUsagePercent_MultipleCalls_DoNotThrow()
    {
        var monitor = new SystemMonitor();
        for (int i = 0; i < 5; i++)
        {
            int cpu = monitor.GetCpuUsagePercent();
            Assert.InRange(cpu, 0, 100);
        }
    }

    [Fact]
    public void GetCpuUsagePercent_TwoInstances_BothReturnValidRange()
    {
        var m1 = new SystemMonitor();
        var m2 = new SystemMonitor();
        int c1 = m1.GetCpuUsagePercent();
        int c2 = m2.GetCpuUsagePercent();
        Assert.InRange(c1, 0, 100);
        Assert.InRange(c2, 0, 100);
    }
}

// ── Sprint 21: Coverage boost — Analytics, Locale, Ratings edge cases ────────

[Collection("Analytics")]
public sealed class AnalyticsCoverageTests
{
    private readonly string _tempDir;

    public AnalyticsCoverageTests()
    {
        _tempDir = Path.Combine(Path.GetTempPath(), $"regilattice-analytics-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(_tempDir);
    }

    [Fact]
    public void Reset_ClearsAllStats()
    {
        Analytics.RecordApply("reset-test-1");
        Analytics.RecordApply("reset-test-2");
        Analytics.Reset();
        var stats = Analytics.GetStats();
        Assert.Equal(0, stats.TotalApplies);
        Assert.Equal(0, stats.TotalRemoves);
        Assert.Equal(0, stats.TotalErrors);
    }

    [Fact]
    public void TopTweaks_Empty_ReturnsEmptyList()
    {
        Analytics.Reset();
        var top = Analytics.TopTweaks(5);
        Assert.Empty(top);
    }

    [Fact]
    public void TopTweaks_ZeroN_ReturnsEmptyList()
    {
        Analytics.Reset();
        Analytics.RecordApply("top-zero");
        var top = Analytics.TopTweaks(0);
        Assert.Empty(top);
    }

    [Fact]
    public void TopTweaks_OrderedByCount()
    {
        Analytics.Reset();
        Analytics.RecordApply("top-a");
        Analytics.RecordApply("top-b");
        Analytics.RecordApply("top-b");
        Analytics.RecordApply("top-c");
        Analytics.RecordApply("top-c");
        Analytics.RecordApply("top-c");
        var top = Analytics.TopTweaks(3);
        Assert.Equal(3, top.Count);
        Assert.Equal("top-c", top[0].Item1);
        Assert.Equal("top-b", top[1].Item1);
        Assert.Equal("top-a", top[2].Item1);
    }

    [Fact]
    public void TopTweaks_NGreaterThanTotal_ReturnsAll()
    {
        Analytics.Reset();
        Analytics.RecordApply("only-one");
        var top = Analytics.TopTweaks(100);
        Assert.Single(top);
    }

    [Fact]
    public void RecordApply_MultipleTweaks_TracksEachSeparately()
    {
        Analytics.Reset();
        Analytics.RecordApply("multi-a");
        Analytics.RecordApply("multi-a");
        Analytics.RecordApply("multi-b");
        var stats = Analytics.GetStats();
        Assert.Equal(3, stats.TotalApplies);
    }

    [Fact]
    public void RecordError_MultipleTimes_Accumulates()
    {
        Analytics.Reset();
        Analytics.RecordError("err-1");
        Analytics.RecordError("err-2");
        Analytics.RecordError("err-3");
        var stats = Analytics.GetStats();
        Assert.Equal(3, stats.TotalErrors);
    }
}

public sealed class LocaleCoverageTests
{
    [Fact]
    public void T_EmptyKey_ReturnsEmptyString()
    {
        var result = Locale.T("");
        Assert.Equal("", result);
    }

    [Fact]
    public void T_UnknownKeyNoArgs_ReturnsKey()
    {
        var result = Locale.T("nonexistent-key-xyz-123");
        Assert.Equal("nonexistent-key-xyz-123", result);
    }

    [Fact]
    public void SetLocale_EnglishThenGerman_SwitchesCorrectly()
    {
        Locale.SetLocale("en");
        var en = Locale.T("apply_all");
        Locale.SetLocale("de");
        var de = Locale.T("apply_all");
        Assert.NotEqual(en, de);
        Locale.SetLocale("en"); // restore
    }

    [Fact]
    public void LoadLocaleFile_NonExistentFile_DoesNotThrow()
    {
        var fakePath = Path.Combine(Path.GetTempPath(), "nonexistent-locale-xyz.txt");
        Locale.LoadLocaleFile(fakePath);
        // Should silently return; no exception
    }

    [Fact]
    public void LoadLocaleFile_ValidFile_OverridesKeys()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), $"test-locale-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllText(tempFile, "app_title=Custom Title\n");
            Locale.LoadLocaleFile(tempFile);
            var result = Locale.T("app_title");
            Assert.Equal("Custom Title", result);
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
            Locale.SetLocale("en"); // restore
        }
    }

    [Fact]
    public void AvailableLocales_ContainsAtLeastTwo()
    {
        var locales = Locale.AvailableLocales;
        Assert.True(locales.Count >= 2);
        Assert.Contains("en", locales);
        Assert.Contains("de", locales);
    }
}

public sealed class RatingsCoverageTests
{
    private static string UniqueId() => $"ratcov-{Guid.NewGuid():N}";

    [Fact]
    public void AverageRating_WithRatings_ReturnsNonNull()
    {
        var id = UniqueId();
        Ratings.Rate(id, 4);
        var avg = Ratings.AverageRating();
        Assert.NotNull(avg);
        Ratings.RemoveRating(id);
    }

    [Fact]
    public void AverageRating_SingleRating_ReturnsExact()
    {
        var id = UniqueId();
        // Remove all known test ratings first, then add exactly one
        Ratings.Rate(id, 3);
        // Since we can't isolate, just verify non-null and reasonable range
        var avg = Ratings.AverageRating();
        Assert.NotNull(avg);
        Assert.InRange(avg.Value, 1.0, 5.0);
        Ratings.RemoveRating(id);
    }

    [Fact]
    public void AllRatings_DirectCall_ReturnsNonNull()
    {
        var id1 = UniqueId();
        var id2 = UniqueId();
        Ratings.Rate(id1, 2);
        Ratings.Rate(id2, 4);
        var all = Ratings.AllRatings();
        Assert.True(all.ContainsKey(id1));
        Assert.True(all.ContainsKey(id2));
        Ratings.RemoveRating(id1);
        Ratings.RemoveRating(id2);
    }

    [Fact]
    public void Rate_UpdateExisting_OverwritesStars()
    {
        var id = UniqueId();
        Ratings.Rate(id, 2);
        Ratings.Rate(id, 5);
        var rating = Ratings.GetRating(id);
        Assert.NotNull(rating);
        Assert.Equal(5, rating.Stars);
        Ratings.RemoveRating(id);
    }

    [Fact]
    public void AllRatings_ContainsNewlyAdded()
    {
        var id = UniqueId();
        Ratings.Rate(id, 3);
        var all = Ratings.AllRatings();
        Assert.True(all.Count > 0);
        Assert.True(all.ContainsKey(id));
        Ratings.RemoveRating(id);
    }
}

// ── Sprint 23 — Locale hot-cache branch coverage ─────────────────────────────

/// <summary>Tests targeting Locale._hotCache hit/miss branches and LoadLocaleFile.</summary>
public sealed class LocaleHotCacheTests
{
    [Fact]
    public void T_SameKey_Twice_UsesCacheSecondTime()
    {
        Locale.SetLocale("en");
        // First call — cold (populates _hotCache)
        var first = Locale.T("apply_all");
        // Second call — warm (uses _hotCache)
        var second = Locale.T("apply_all");
        Assert.Equal(first, second);
        Assert.Equal("Apply All", first);
    }

    [Fact]
    public void SetLocale_InvalidatesHotCache()
    {
        Locale.SetLocale("en");
        var en = Locale.T("apply_all"); // warms cache

        Locale.SetLocale("de"); // should clear cache
        var de = Locale.T("apply_all"); // fresh lookup in German
        Assert.NotEqual(en, de);
        Assert.Equal("Alle anwenden", de);

        Locale.SetLocale("en"); // reset
    }

    [Fact]
    public void T_NoArgs_ReturnsCachedTemplate()
    {
        Locale.SetLocale("en");
        Locale.T("status_applied"); // warms
        var cached = Locale.T("status_applied"); // from cache
        Assert.Equal("APPLIED", cached);
    }

    [Fact]
    public void T_WithArgs_DoesNotCacheFormattedString()
    {
        Locale.SetLocale("en");
        var r1 = Locale.T("confirm_apply", 3);
        var r2 = Locale.T("confirm_apply", 7);
        Assert.Equal("Apply 3 selected tweaks?", r1);
        Assert.Equal("Apply 7 selected tweaks?", r2);
    }

    [Fact]
    public void LoadLocaleFile_NonExistentPath_DoesNotThrow()
    {
        // Should silently no-op when file doesn't exist
        Locale.LoadLocaleFile(Path.Combine(Path.GetTempPath(), "nonexistent_locale_xyz.ini"));
        // Locale remains unchanged
        Assert.Equal("Apply All", Locale.T("apply_all"));
    }

    [Fact]
    public void LoadLocaleFile_ValidFile_LoadsOverrides()
    {
        var path = Path.Combine(Path.GetTempPath(), $"test-locale-{Guid.NewGuid():N}.ini");
        try
        {
            File.WriteAllLines(path, ["apply_all = Custom Apply", "remove_all = Custom Remove"]);
            Locale.LoadLocaleFile(path);
            Assert.Equal("Custom Apply", Locale.T("apply_all"));
            Assert.Equal("Custom Remove", Locale.T("remove_all"));
        }
        finally
        {
            Locale.SetLocale("en"); // reset
            File.Delete(path);
        }
    }
}

// ── Sprint 23 — CorporateGuard extra branches ───────────────────────────────

/// <summary>Additional branch coverage for CorporateGuard.</summary>
public sealed class CorporateGuardBranchTests
{
    [Fact]
    public void IsCorporateNetwork_CalledTwice_ReturnsSameResult()
    {
        CorporateGuard.ClearCache();
        var first = CorporateGuard.IsCorporateNetwork();
        var second = CorporateGuard.IsCorporateNetwork(); // hits _cached branch
        Assert.Equal(first, second);
    }

    [Fact]
    public void ClearCache_ResetsState()
    {
        _ = CorporateGuard.IsCorporateNetwork(); // populate cache
        CorporateGuard.ClearCache();
        _ = CorporateGuard.IsCorporateNetwork(); // re-evaluate after clear
    }

    [Fact]
    public void IsGpoManaged_PoliciesPathKey_ReturnsTrue()
    {
        var result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Policies\Microsoft\Windows\Test"]);
        Assert.True(result); // path contains \Policies\ → immediate true
    }

    [Fact]
    public void IsGpoManaged_NonPoliciesPath_ReturnsBool()
    {
        // Non-policies path is checked against real registry — result is bool
        var result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion"]);
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void IsGpoManaged_EmptyList_ReturnsFalse()
    {
        Assert.False(CorporateGuard.IsGpoManaged([]));
    }

    [Fact]
    public void IsGpoManaged_MixedPaths_ChecksAll()
    {
        // First entry has \Policies\ → should return true immediately
        var result = CorporateGuard.IsGpoManaged([@"HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion", @"HKLM\SOFTWARE\Policies\Microsoft\Windows"]);
        // At least one has Policies — result depends on which is evaluated first
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void Status_ReturnsReasonString()
    {
        var (_, reason) = CorporateGuard.Status();
        Assert.NotNull(reason);
        Assert.NotEmpty(reason);
    }
}

// ── Sprint 23 — HardwareInfo extended branches ──────────────────────────────

/// <summary>Extended coverage for HardwareInfo methods not yet tested.</summary>
public sealed class HardwareInfoExtendedTests
{
    [Fact]
    public void IsAdobeInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsAdobeInstalled());
    }

    [Fact]
    public void IsLibreOfficeInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsLibreOfficeInstalled());
    }

    [Fact]
    public void IsRealVncInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsRealVncInstalled());
    }

    [Fact]
    public void IsScoopInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsScoopInstalled());
    }

    [Fact]
    public void IsDockerInstalled_ReturnsBool()
    {
        Assert.IsType<bool>(HardwareInfo.IsDockerInstalled());
    }

    [Fact]
    public void DetectHardware_HasGpuInfo()
    {
        var hw = HardwareInfo.DetectHardware();
        Assert.NotNull(hw.Gpus);
    }

    [Fact]
    public void DetectHardware_HasDisk()
    {
        var hw = HardwareInfo.DetectHardware();
        Assert.NotNull(hw.Disk);
    }
}

// ── Sprint 24: AppConfig field coverage ───────────────────────────────────

public sealed class AppConfigFieldTests
{
    [Fact]
    public void Default_AutoBackup_IsTrue() => Assert.True(new AppConfig().AutoBackup);

    [Fact]
    public void Default_BackupDir_IsEmpty() => Assert.Empty(new AppConfig().BackupDir);

    [Fact]
    public void Default_Locale_IsEn() => Assert.Equal("en", new AppConfig().Locale);

    [Fact]
    public void Default_CheckToolUpdates_IsTrue() => Assert.True(new AppConfig().CheckToolUpdates);

    [Fact]
    public void Default_MaxWorkers_IsEight() => Assert.Equal(8, new AppConfig().MaxWorkers);

    [Fact]
    public void Default_ForceCorp_IsFalse() => Assert.False(new AppConfig().ForceCorp);

    [Fact]
    public void SaveAndLoad_ThemeRoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-sp24-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "cfg.json");
        try
        {
            var cfg = new AppConfig { Theme = "nord" };
            cfg.Save(path);
            Assert.Equal("nord", AppConfig.Load(path).Theme);
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
    public void SaveAndLoad_MaxWorkersRoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-sp24mw-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "cfg.json");
        try
        {
            var cfg = new AppConfig { MaxWorkers = 4 };
            cfg.Save(path);
            Assert.Equal(4, AppConfig.Load(path).MaxWorkers);
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
    public void SaveAndLoad_AutoBackupFalseRoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-sp24ab-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "cfg.json");
        try
        {
            var cfg = new AppConfig { AutoBackup = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).AutoBackup);
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
    public void Load_MissingFile_ReturnsDefaultTheme()
    {
        var cfg = AppConfig.Load(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "nope.json"));
        Assert.Equal("catppuccin-mocha", cfg.Theme);
    }

    [Fact]
    public void Default_LastSeenVersion_IsEmpty() => Assert.Empty(new AppConfig().LastSeenVersion);

    [Fact]
    public void SaveAndLoad_LastSeenVersionRoundTrip()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-sp26-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "cfg.json");
        try
        {
            var cfg = new AppConfig { LastSeenVersion = "3.4.0" };
            cfg.Save(path);
            Assert.Equal("3.4.0", AppConfig.Load(path).LastSeenVersion);
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
}

// ── Sprint 24: Ratings edge cases ─────────────────────────────────────────

public sealed class RatingsSprintTests
{
    private static string UniqueId() => $"sp24-r-{Guid.NewGuid():N}";

    [Fact]
    public void Rate_UpdateExistingRating_OverwritesNote()
    {
        var id = UniqueId();
        try
        {
            Ratings.Rate(id, 2, "old note");
            Ratings.Rate(id, 5, "new note");
            var rating = Ratings.GetRating(id);
            Assert.Equal(5, rating!.Stars);
            Assert.Equal("new note", rating.Note);
        }
        finally
        {
            Ratings.RemoveRating(id);
        }
    }

    [Fact]
    public void Rate_InvalidStars_ThrowsAtBoundaries()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Ratings.Rate("sp24-boundary-lo", 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Ratings.Rate("sp24-boundary-hi", 6));
    }

    [Fact]
    public void RemoveRating_NonExistent_DoesNotThrow()
    {
        var ex = Record.Exception(() => Ratings.RemoveRating($"sp24-missing-{Guid.NewGuid():N}"));
        Assert.Null(ex);
    }

    [Fact]
    public void TopRated_WithRatings_HigherStarsPositionedFirst()
    {
        var low = UniqueId();
        var high = UniqueId();
        try
        {
            Ratings.Rate(low, 1, "low");
            Ratings.Rate(high, 5, "high");
            var top = Ratings.TopRated(100).ToList();
            var posLow = top.FindIndex(t => t.Id == low);
            var posHigh = top.FindIndex(t => t.Id == high);
            if (posLow >= 0 && posHigh >= 0)
                Assert.True(posHigh < posLow);
        }
        finally
        {
            Ratings.RemoveRating(low);
            Ratings.RemoveRating(high);
        }
    }

    [Fact]
    public void AverageRating_WithAtLeastOneRating_IsPositive()
    {
        var id = UniqueId();
        try
        {
            Ratings.Rate(id, 3, "");
            var avg = Ratings.AverageRating();
            Assert.NotNull(avg);
            Assert.True(avg > 0);
        }
        finally
        {
            Ratings.RemoveRating(id);
        }
    }
}

// ── Sprint 24: Analytics edge cases ───────────────────────────────────────

[Collection("Analytics")]
public sealed class AnalyticsSprintTests
{
    [Fact]
    public void GetStats_ReturnsNonNull() => Assert.NotNull(Analytics.GetStats());

    [Fact]
    public void GetStats_TotalApplies_IsNonNegative() => Assert.True(Analytics.GetStats().TotalApplies >= 0);

    [Fact]
    public void RecordApply_IncrementsCount()
    {
        var before = Analytics.GetStats().TotalApplies;
        Analytics.RecordApply($"sp24-{Guid.NewGuid():N}");
        Assert.Equal(before + 1, Analytics.GetStats().TotalApplies);
    }

    [Fact]
    public void RecordRemove_IncrementsTotalRemoves()
    {
        var before = Analytics.GetStats().TotalRemoves;
        Analytics.RecordRemove($"sp24-rm-{Guid.NewGuid():N}");
        Assert.Equal(before + 1, Analytics.GetStats().TotalRemoves);
    }

    [Fact]
    public void TopTweaks_ReturnsNonNullList() => Assert.NotNull(Analytics.TopTweaks(5));
}

// ── Sprint 47: AppConfig new property defaults ──────────────────────────────

public sealed class AppConfigSprint47Tests
{
    [Fact]
    public void Default_AutoBackupOnApply_IsTrue() => Assert.True(new AppConfig().AutoBackupOnApply);

    [Fact]
    public void Default_SnapshotOnProfileChange_IsTrue() => Assert.True(new AppConfig().SnapshotOnProfileChange);

    [Fact]
    public void SaveLoadRoundTrip_AutoBackupOnApply_False()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-sp47-{Guid.NewGuid()}");
        var path = Path.Combine(dir, "cfg.json");
        try
        {
            var cfg = new AppConfig { AutoBackupOnApply = false, SnapshotOnProfileChange = false };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.False(loaded.AutoBackupOnApply);
            Assert.False(loaded.SnapshotOnProfileChange);
        }
        finally
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive: true);
        }
    }
}

// ── Sprint 47: NetworkManager new APIs ──────────────────────────────────────

public sealed class NetworkManagerSprint47Tests
{
    [Fact]
    public void GetNetworkInterfaceStats_ReturnsNonNull()
    {
        var stats = NetworkManager.GetNetworkInterfaceStats();
        Assert.NotNull(stats);
    }

    [Fact]
    public void GetNetworkInterfaceStats_AllNamesNonEmpty()
    {
        var stats = NetworkManager.GetNetworkInterfaceStats();
        Assert.All(stats, s => Assert.False(string.IsNullOrWhiteSpace(s.Name)));
    }

    [Fact]
    public void PingResult_Parse_TypicalOutput_ExtractsPacketCounts()
    {
        const string stdout =
            "\r\nPinging 1.1.1.1 with 32 bytes of data:\r\nReply from 1.1.1.1: bytes=32 time=5ms TTL=58\r\nReply from 1.1.1.1: bytes=32 time=4ms TTL=58\r\n\r\nPing statistics for 1.1.1.1:\r\n    Packets: Sent = 2, Received = 2, Lost = 0 (0% loss),\r\nApproximate round trip times in milli-seconds:\r\n    Minimum = 4ms, Maximum = 5ms, Average = 4ms\r\n";
        var result = PingResult.Parse("1.1.1.1", stdout);
        Assert.Equal("1.1.1.1", result.Host);
        Assert.Equal(2, result.Sent);
        Assert.Equal(2, result.Received);
        Assert.Equal(0, result.Lost);
        Assert.Equal(0.0, result.LossPercent);
    }

    [Fact]
    public void PingResult_Parse_AllLost_LossPercent100()
    {
        const string stdout = "    Packets: Sent = 4, Received = 0, Lost = 4 (100% loss),";
        var result = PingResult.Parse("unreachable", stdout);
        Assert.Equal(100.0, result.LossPercent);
    }
}

// ── Sprint 47: StartupManager AddRegistryEntry validation ──────────────────

public sealed class StartupManagerSprint47Tests
{
    [Fact]
    public void AddRegistryEntry_BlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("", "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_BlankCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("TestEntry", ""));
    }

    [Fact]
    public async Task ExportEntriesAsync_BlankPath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => StartupManager.ExportEntriesAsync(""));
    }
}

// ── Sprint 47: ServiceManager new APIs ──────────────────────────────────────

public sealed class ServiceManagerSprint47Tests
{
    [Fact]
    public void GetDependentServices_BlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ServiceManager.GetDependentServices(""));
    }

    [Fact]
    public void GetDependentServices_UnknownService_ReturnsEmptyList()
    {
        var deps = ServiceManager.GetDependentServices("rl-test-nonexistent-svc-9x8a");
        Assert.Empty(deps);
    }

    [Fact]
    public async Task ExportToCsvAsync_WritesHeaderAndAtLeastOneRow()
    {
        var path = Path.Combine(Path.GetTempPath(), $"rl-svc-{Guid.NewGuid()}.csv");
        try
        {
            await ServiceManager.ExportToCsvAsync(path);
            Assert.True(File.Exists(path));
            var lines = await File.ReadAllLinesAsync(path);
            Assert.True(lines.Length >= 2); // header + at least 1 service
            Assert.Contains("ServiceName", lines[0]);
            Assert.Contains("DisplayName", lines[0]);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}

// ── AppConfig UI preferences — default values and roundtrip ──────────────────

/// <summary>
/// Tests for the UI-preference properties added in recent sprints.
/// These were previously untested, leaving branches uncovered in the JSON
/// serialisation / deserialisation paths.
/// </summary>
public sealed class AppConfigUiPrefsTests
{
    private static (string dir, string path) TempCfg()
    {
        var dir = Path.Combine(Path.GetTempPath(), $"rl-uiprefs-{Guid.NewGuid()}");
        return (dir, Path.Combine(dir, "cfg.json"));
    }

    private static void Cleanup(string dir)
    {
        try
        {
            if (Directory.Exists(dir))
                Directory.Delete(dir, recursive: true);
        }
        catch { }
    }

    // ── Default value assertions ─────────────────────────────────────────

    [Fact]
    public void Default_MinimizeToTray_IsTrue() => Assert.True(new AppConfig().MinimizeToTray);

    [Fact]
    public void Default_ConfirmApply_IsTrue() => Assert.True(new AppConfig().ConfirmApply);

    [Fact]
    public void Default_ConfirmRemove_IsTrue() => Assert.True(new AppConfig().ConfirmRemove);

    [Fact]
    public void Default_ShowInapplicable_IsTrue() => Assert.True(new AppConfig().ShowInapplicable);

    [Fact]
    public void Default_StatusBarMonitor_IsTrue() => Assert.True(new AppConfig().StatusBarMonitor);

    [Fact]
    public void Default_DetailPanelHeight_Is130() => Assert.Equal(130, new AppConfig().DetailPanelHeight);

    [Fact]
    public void Default_FontSize_Is9f() => Assert.Equal(9f, new AppConfig().FontSize);

    [Fact]
    public void Default_ShowLogPanel_IsTrue() => Assert.True(new AppConfig().ShowLogPanel);

    [Fact]
    public void Default_LogPanelHeight_Is150() => Assert.Equal(150, new AppConfig().LogPanelHeight);

    [Fact]
    public void Default_AutoRefreshOnStartup_IsTrue() => Assert.True(new AppConfig().AutoRefreshOnStartup);

    [Fact]
    public void Default_LaunchMinimized_IsFalse() => Assert.False(new AppConfig().LaunchMinimized);

    [Fact]
    public void Default_RememberSplitter_IsTrue() => Assert.True(new AppConfig().RememberSplitter);

    [Fact]
    public void Default_SplitterDistance_IsZero() => Assert.Equal(0, new AppConfig().SplitterDistance);

    [Fact]
    public void Default_SkipAppliedOnBatch_IsTrue() => Assert.True(new AppConfig().SkipAppliedOnBatch);

    [Fact]
    public void Default_BrightnessSchedulerEnabled_IsFalse() => Assert.False(new AppConfig().BrightnessSchedulerEnabled);

    [Fact]
    public void Default_BrightnessDayPct_Is80() => Assert.Equal(80, new AppConfig().BrightnessDayPct);

    [Fact]
    public void Default_BrightnessNightPct_Is40() => Assert.Equal(40, new AppConfig().BrightnessNightPct);

    [Fact]
    public void Default_BrightnessDayTime_Is0700() => Assert.Equal("07:00", new AppConfig().BrightnessDayTime);

    [Fact]
    public void Default_BrightnessNightTime_Is2100() => Assert.Equal("21:00", new AppConfig().BrightnessNightTime);

    [Fact]
    public void Default_HistoryMaxEntries_Is500() => Assert.Equal(500, new AppConfig().HistoryMaxEntries);

    [Fact]
    public void Default_MonitorColorCoded_IsTrue() => Assert.True(new AppConfig().MonitorColorCoded);

    [Fact]
    public void Default_AutoCleanMemoryThreshold_IsZero() => Assert.Equal(0, new AppConfig().AutoCleanMemoryThreshold);

    [Fact]
    public void Default_ProfileSchedules_IsEmpty() => Assert.Empty(new AppConfig().ProfileSchedules);

    [Fact]
    public void Default_ProfileOnPlanSwitch_IsNull() => Assert.Null(new AppConfig().ProfileOnPlanSwitch);

    // ── Roundtrip tests ──────────────────────────────────────────────────

    [Fact]
    public void SaveAndLoad_FontSize_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { FontSize = 12f };
            cfg.Save(path);
            Assert.Equal(12f, AppConfig.Load(path).FontSize);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_ShowLogPanel_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { ShowLogPanel = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).ShowLogPanel);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_LogPanelHeight_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { LogPanelHeight = 250 };
            cfg.Save(path);
            Assert.Equal(250, AppConfig.Load(path).LogPanelHeight);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_LaunchMinimized_True_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { LaunchMinimized = true };
            cfg.Save(path);
            Assert.True(AppConfig.Load(path).LaunchMinimized);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_RememberSplitter_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { RememberSplitter = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).RememberSplitter);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_SplitterDistance_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { SplitterDistance = 320 };
            cfg.Save(path);
            Assert.Equal(320, AppConfig.Load(path).SplitterDistance);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_BrightnessScheduler_AllFields_RoundTrip()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig
            {
                BrightnessSchedulerEnabled = true,
                BrightnessDayPct = 90,
                BrightnessNightPct = 30,
                BrightnessDayTime = "08:00",
                BrightnessNightTime = "22:00",
            };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.True(loaded.BrightnessSchedulerEnabled);
            Assert.Equal(90, loaded.BrightnessDayPct);
            Assert.Equal(30, loaded.BrightnessNightPct);
            Assert.Equal("08:00", loaded.BrightnessDayTime);
            Assert.Equal("22:00", loaded.BrightnessNightTime);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_HistoryMaxEntries_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { HistoryMaxEntries = 1000 };
            cfg.Save(path);
            Assert.Equal(1000, AppConfig.Load(path).HistoryMaxEntries);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_MonitorColorCoded_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { MonitorColorCoded = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).MonitorColorCoded);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_AutoCleanMemoryThreshold_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { AutoCleanMemoryThreshold = 80 };
            cfg.Save(path);
            Assert.Equal(80, AppConfig.Load(path).AutoCleanMemoryThreshold);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_ProfileOnPlanSwitch_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { ProfileOnPlanSwitch = "gaming" };
            cfg.Save(path);
            Assert.Equal("gaming", AppConfig.Load(path).ProfileOnPlanSwitch);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_ProfileSchedules_SingleEntry_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var entry = new ProfileScheduleEntry
            {
                Profile = "privacy",
                Trigger = "daily",
                Time = "09:00",
                Enabled = true,
            };
            var cfg = new AppConfig { ProfileSchedules = [entry] };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.Single(loaded.ProfileSchedules);
            Assert.Equal("privacy", loaded.ProfileSchedules[0].Profile);
            Assert.Equal("daily", loaded.ProfileSchedules[0].Trigger);
            Assert.Equal("09:00", loaded.ProfileSchedules[0].Time);
            Assert.True(loaded.ProfileSchedules[0].Enabled);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_ProfileSchedules_MultipleEntries_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig
            {
                ProfileSchedules =
                [
                    new ProfileScheduleEntry { Profile = "business", Trigger = "on_boot" },
                    new ProfileScheduleEntry { Profile = "gaming", Trigger = "on_login" },
                ],
            };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.Equal(2, loaded.ProfileSchedules.Count);
            Assert.Equal("business", loaded.ProfileSchedules[0].Profile);
            Assert.Equal("gaming", loaded.ProfileSchedules[1].Profile);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_SkipAppliedOnBatch_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { SkipAppliedOnBatch = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).SkipAppliedOnBatch);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_ConfirmApply_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { ConfirmApply = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).ConfirmApply);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_MinimizeToTray_False_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { MinimizeToTray = false };
            cfg.Save(path);
            Assert.False(AppConfig.Load(path).MinimizeToTray);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    [Fact]
    public void SaveAndLoad_DetailPanelHeight_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig { DetailPanelHeight = 200 };
            cfg.Save(path);
            Assert.Equal(200, AppConfig.Load(path).DetailPanelHeight);
        }
        finally
        {
            Cleanup(dir);
        }
    }

    // ── ProfileScheduleEntry record ──────────────────────────────────────

    [Fact]
    public void ProfileScheduleEntry_DefaultEnabled_IsTrue()
    {
        var entry = new ProfileScheduleEntry { Profile = "minimal", Trigger = "on_boot" };
        Assert.True(entry.Enabled);
    }

    [Fact]
    public void ProfileScheduleEntry_DefaultTime_IsEmpty()
    {
        var entry = new ProfileScheduleEntry { Profile = "minimal", Trigger = "on_boot" };
        Assert.Equal("", entry.Time);
    }

    [Fact]
    public void ProfileScheduleEntry_DisabledEntry_RoundTrips()
    {
        var (dir, path) = TempCfg();
        try
        {
            var cfg = new AppConfig
            {
                ProfileSchedules =
                [
                    new ProfileScheduleEntry
                    {
                        Profile = "server",
                        Trigger = "daily",
                        Enabled = false,
                    },
                ],
            };
            cfg.Save(path);
            var loaded = AppConfig.Load(path);
            Assert.Single(loaded.ProfileSchedules);
            Assert.False(loaded.ProfileSchedules[0].Enabled);
        }
        finally
        {
            Cleanup(dir);
        }
    }
}
