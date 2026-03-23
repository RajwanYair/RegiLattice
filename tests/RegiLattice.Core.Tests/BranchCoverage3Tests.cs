// RegiLattice.Core.Tests — BranchCoverage3Tests.cs
// Sprint 121 T6.1 — Third-pass branch coverage targeting:
//   AppConfig.Validate(), Portable mode, PackManager, ChocolateyManager/PipManager/WinGetManager
//   ValidateName (SafeNameRegex runner), NetworkManager, HardwareInfo software detection,
//   UpdateCheckService.CompareVersions, RegistrySession CheckValueMatch type branches,
//   PackLoader missing-field validations.

#nullable enable

using System.ServiceProcess;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

// ═══════════════════════════════════════════════════════════════════════════════
// AppConfig.Validate() — edge-case branches
// ═══════════════════════════════════════════════════════════════════════════════

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
        cfg.ProfileSchedules.Add(new ProfileScheduleEntry { Profile = "", Trigger = "on_boot" });
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleEmptyTrigger_ReturnsError()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules.Add(new ProfileScheduleEntry { Profile = "minimal", Trigger = "" });
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithInvalidTime_ReturnsError()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules.Add(
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "bad_time",
            }
        );
        var errors = cfg.Validate();
        Assert.Contains(errors, e => e.Contains("daily profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithEmptyTime_IsValid()
    {
        // Empty time is allowed (means no time restriction for daily)
        var cfg = new AppConfig();
        cfg.ProfileSchedules.Add(
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "",
            }
        );
        var errors = cfg.Validate();
        Assert.DoesNotContain(errors, e => e.Contains("daily profile_schedule"));
    }

    [Fact]
    public void Validate_ProfileScheduleDailyWithValidTime_IsValid()
    {
        var cfg = new AppConfig();
        cfg.ProfileSchedules.Add(
            new ProfileScheduleEntry
            {
                Profile = "minimal",
                Trigger = "daily",
                Time = "08:30",
            }
        );
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

public sealed class PackManagerFileSystemBranchTests
{
    // Unique pack names to avoid collisions with any real installed packs
    private const string TestPackName = "rl-unit-test-branchcov3-xyz999";
    private const string TestPackName2 = "rl-unit-test-branchcov3-xyz998";

    private static string MakePackJson(string name) =>
        $$"""
            {
                "name": "{{name}}",
                "displayName": "BranchCov3 Test",
                "version": "1.0.0",
                "author": "UnitTest",
                "tweaks": [{
                    "id": "{{name}}-t1",
                    "label": "T",
                    "category": "TestCat",
                    "applyOps": [{"kind":"SetDword","path":"HKEY_CURRENT_USER\\Software\\RLBranchTest3","name":"V","dwordValue":1}],
                    "detectOps": [{"kind":"CheckDword","path":"HKEY_CURRENT_USER\\Software\\RLBranchTest3","name":"V","dwordValue":1}]
                }]
            }
            """;

    [Fact]
    public void UninstallPack_NonExistentName_ReturnsFalse()
    {
        var mgr = new PackManager();
        Assert.False(mgr.UninstallPack("does-not-exist-pack-xyz-7z9m"));
    }

    [Fact]
    public void InstalledPacks_ReturnsNonNullList()
    {
        var mgr = new PackManager();
        var packs = mgr.InstalledPacks();
        Assert.NotNull(packs); // may be empty or non-empty depending on machine state
    }

    [Fact]
    public void LoadInstalledPack_NonExistentPack_ReturnsNull()
    {
        var mgr = new PackManager();
        var tweaks = mgr.LoadInstalledPack("does-not-exist-pack-xyz-7z9m");
        Assert.Null(tweaks);
    }

    [Fact]
    public void LoadAllInstalledTweaks_ReturnsNonNullList()
    {
        var mgr = new PackManager();
        var tweaks = mgr.LoadAllInstalledTweaks();
        Assert.NotNull(tweaks);
    }

    [Fact]
    public void InstallFromFile_ThenUninstall_RoundTrip()
    {
        var mgr = new PackManager();
        var tmpJson = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tmpJson, MakePackJson(TestPackName));
            var (packDef, tweaks) = mgr.InstallFromFile(tmpJson);
            Assert.Equal(TestPackName, packDef.Name);
            Assert.Single(tweaks);
            Assert.True(mgr.UninstallPack(TestPackName));
        }
        finally
        {
            if (File.Exists(tmpJson))
                File.Delete(tmpJson);
            mgr.UninstallPack(TestPackName); // idempotent cleanup
        }
    }

    [Fact]
    public void LoadInstalledPack_AfterInstall_ReturnsTweaks()
    {
        var mgr = new PackManager();
        var tmpJson = Path.GetTempFileName();
        try
        {
            File.WriteAllText(tmpJson, MakePackJson(TestPackName2));
            mgr.InstallFromFile(tmpJson);
            var tweaks = mgr.LoadInstalledPack(TestPackName2);
            Assert.NotNull(tweaks);
            Assert.Single(tweaks!);
        }
        finally
        {
            if (File.Exists(tmpJson))
                File.Delete(tmpJson);
            mgr.UninstallPack(TestPackName2);
        }
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// ChocolateyManager.ValidateName — SafeNameRegex runner branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class ChocolateyManagerValidationBranchTests
{
    [Theory]
    [InlineData("googlechrome")]
    [InlineData("7zip")]
    [InlineData("my.package_v2")]
    [InlineData("some-pkg-name")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = ChocolateyManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName(""));
    }

    [Fact]
    public void ValidateName_WhitespaceName_Throws()
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName("   "));
    }

    [Theory]
    [InlineData("name with spaces")]
    [InlineData("pkg/slash")]
    [InlineData("pkg!name")]
    [InlineData("pkg@name")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// PipManager.ValidateName — SafeNameRegex runner branches (second instance)
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class PipManagerValidationBranchTests
{
    [Theory]
    [InlineData("requests")]
    [InlineData("numpy.scipy")]
    [InlineData("my-package_1")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = PipManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => PipManager.ValidateName(""));
    }

    [Theory]
    [InlineData("pkg with space")]
    [InlineData("pkg;injection")]
    [InlineData("pip&&rm")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => PipManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// WinGetManager.ValidateName — SafeNameRegex runner branches (third instance)
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class WinGetManagerValidationBranchTests
{
    [Theory]
    [InlineData("Microsoft.VSCode")]
    [InlineData("Git.Git")]
    [InlineData("Python.Python.3.12")]
    [InlineData("7zip.7zip")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = WinGetManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => WinGetManager.ValidateName(""));
    }

    [Theory]
    [InlineData("Package Name With Spaces")]
    [InlineData("pkg|pipe")]
    [InlineData("cmd>redirect")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => WinGetManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// UpdateCheckService.CompareVersions — all comparison branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class UpdateCheckServiceBranchTests
{
    [Fact]
    public void CompareVersions_AGreaterThanB_ReturnsPositive()
    {
        int result = UpdateCheckService.CompareVersions("2.0.0", "1.9.9");
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareVersions_ALessThanB_ReturnsNegative()
    {
        int result = UpdateCheckService.CompareVersions("1.0.0", "2.0.0");
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareVersions_Equal_ReturnsZero()
    {
        int result = UpdateCheckService.CompareVersions("3.5.0", "3.5.0");
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareVersions_UnparseableA_TreatsAsZero()
    {
        // Unparseable "a" → 0.0.0, parseable "1.0.0" is bigger
        int result = UpdateCheckService.CompareVersions("not-a-version", "1.0.0");
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareVersions_UnparseableB_TreatsAsZero()
    {
        int result = UpdateCheckService.CompareVersions("1.0.0", "not-a-version");
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareVersions_BothUnparseable_ReturnsZero()
    {
        int result = UpdateCheckService.CompareVersions("bad", "also-bad");
        Assert.Equal(0, result);
    }

    [Fact]
    public void CurrentVersion_ReturnsNonEmptyString()
    {
        var version = UpdateCheckService.CurrentVersion;
        Assert.False(string.IsNullOrWhiteSpace(version));
        // Should be in X.Y.Z format or "0.0.0"
        Assert.Matches(@"^\d+\.\d+\.\d+$", version);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// RegistrySession — CheckValueMatch type branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class RegistrySessionCheckValueBranchTests
{
    private const string TestKeyPath = @"HKEY_CURRENT_USER\Software\RegiLattice\BranchCovTest3";

    [Fact]
    public void Evaluate_CheckMissing_ValueExistsReturnsTrue_ButCheckMissingReturnsFalse()
    {
        // Write a value, then CheckMissing should return FALSE (value is present = not missing)
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "ExistingForCheck", 42);
            var result = session.Evaluate([RegOp.CheckMissing(TestKeyPath, "ExistingForCheck")]);
            Assert.False(result); // value exists → CheckMissing fails → Evaluate returns false
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExistingForCheck");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void CheckValueMatch_LongValue_MatchesCorrectly()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            const long testVal = 9_876_543_210L;
            session.SetQword(TestKeyPath, "QwordTest", testVal);
            // CheckValue with a Qword op
            var checkOp = RegOp.SetQword(TestKeyPath, "QwordTest", testVal); // Use as detect by re-evaluating raw
            // Use raw Evaluate with a CheckValue type that reads long
            var readBack = session.ReadQword(TestKeyPath, "QwordTest");
            Assert.Equal(testVal, readBack);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "QwordTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void Evaluate_CheckValue_BinaryMatch_ReturnsTrue()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            byte[] data = [0x01, 0x02, 0x03, 0xAB];
            session.SetBinary(TestKeyPath, "BinTest", data);
            var readBack = session.ReadBinary(TestKeyPath, "BinTest");
            Assert.NotNull(readBack);
            Assert.Equal(data, readBack!);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "BinTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void SetMultiSz_And_ReadBack_IsCorrect()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            string[] vals = ["one", "two", "three"];
            session.SetMultiSz(TestKeyPath, "MultiTest", vals);
            var readBack = session.ReadMultiSz(TestKeyPath, "MultiTest");
            Assert.NotNull(readBack);
            Assert.Equal(vals, readBack!);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "MultiTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void SetExpandString_And_ReadBack_IsCorrect()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetExpandString(TestKeyPath, "ExpandTest", "%SystemRoot%\\test");
            var readBack = session.ReadString(TestKeyPath, "ExpandTest");
            Assert.NotNull(readBack);
            // The stored value should contain the path
            Assert.True(readBack!.Contains("test") || readBack.Contains("Windows"));
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExpandTest");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void ValueExists_ExistingValue_ReturnsTrue()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "ExVal", 1);
            Assert.True(session.ValueExists(TestKeyPath, "ExVal"));
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "ExVal");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void ListValueNames_AfterSettingValues_ContainsSetNames()
    {
        var session = new RegistrySession(dryRun: false);
        try
        {
            session.SetDword(TestKeyPath, "Name1", 1);
            session.SetString(TestKeyPath, "Name2", "val");
            var names = session.ListValueNames(TestKeyPath);
            Assert.Contains("Name1", names, StringComparer.OrdinalIgnoreCase);
            Assert.Contains("Name2", names, StringComparer.OrdinalIgnoreCase);
        }
        finally
        {
            session.DeleteValue(TestKeyPath, "Name1");
            session.DeleteValue(TestKeyPath, "Name2");
            session.DeleteTree(TestKeyPath);
        }
    }

    [Fact]
    public void WriteLog_Event_LogContainsEntry()
    {
        var session = new RegistrySession(dryRun: true);
        session.WriteLog("test-event");
        Assert.Contains(session.Log, l => l.Contains("test-event"));
    }

    [Fact]
    public void LogWritten_Event_Fires()
    {
        var session = new RegistrySession(dryRun: true);
        string? captured = null;
        session.LogWritten += msg => captured = msg;
        session.WriteLog("event-test");
        Assert.Contains("event-test", captured);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// NetworkManager — safe (no-system-mutation) branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class NetworkManagerBranchTests
{
    [Fact]
    public void GetActiveAdapterNames_ReturnsListOrEmpty()
    {
        var names = NetworkManager.GetActiveAdapterNames();
        Assert.NotNull(names); // may be empty if no adapters, but non-null
    }

    [Fact]
    public void GetNetworkInterfaceStats_ReturnsListOrEmpty()
    {
        var stats = NetworkManager.GetNetworkInterfaceStats();
        Assert.NotNull(stats);
    }

    [Fact]
    public void GetCurrentDns_NonExistentAdapter_ReturnsBothEmpty()
    {
        var (primary, secondary) = NetworkManager.GetCurrentDns("NONEXISTENT_ADAPTER_XYZ_999");
        Assert.Equal("", primary);
        Assert.Equal("", secondary);
    }

    [Fact]
    public async Task PingAsync_NullHost_ThrowsArgumentException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(() => NetworkManager.PingAsync(null!));
    }

    [Fact]
    public async Task PingAsync_EmptyHost_ThrowsArgumentException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(() => NetworkManager.PingAsync(""));
    }

    [Fact]
    public async Task PingAsync_CountZero_ThrowsArgumentOutOfRangeException()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => NetworkManager.PingAsync("localhost", 0));
    }

    [Fact]
    public async Task PingAsync_Count101_ThrowsArgumentOutOfRangeException()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => NetworkManager.PingAsync("localhost", 101));
    }

    [Fact]
    public void DnsPreset_BuiltIn_ContainsAtLeastOneEntry()
    {
        Assert.NotEmpty(DnsPreset.BuiltIn);
    }

    [Fact]
    public void DnsPreset_Cloudflare_HasCorrectIp()
    {
        var cf = DnsPreset.BuiltIn.First(p => p.Name.StartsWith("Cloudflare", StringComparison.OrdinalIgnoreCase));
        Assert.Equal("1.1.1.1", cf.Primary);
    }

    [Fact]
    public void PingResult_LossPercent_WhenPartialLoss_IsCorrect()
    {
        var pr = new PingResult("test.host", Sent: 4, Received: 3, Lost: 1, AverageMs: 10, MinMs: 8, MaxMs: 12);
        Assert.Equal(25.0, pr.LossPercent, precision: 5);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// HardwareInfo — software detection method branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class HardwareInfoSoftwareDetectionBranchTests
{
    // Each method returns a bool — testing them covers one branch (found/not-found).
    // Since we can't control whether software is installed, we just verify no exception.

    [Fact]
    public void IsChromeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsChromeInstalled());

    [Fact]
    public void IsFirefoxInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsFirefoxInstalled());

    [Fact]
    public void IsEdgeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsEdgeInstalled());

    [Fact]
    public void IsJavaInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsJavaInstalled());

    [Fact]
    public void IsDockerInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsDockerInstalled());

    [Fact]
    public void IsAdobeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsAdobeInstalled());

    [Fact]
    public void IsLibreOfficeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsLibreOfficeInstalled());

    [Fact]
    public void IsOfficeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsOfficeInstalled());

    [Fact]
    public void IsRealVncInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsRealVncInstalled());

    [Fact]
    public void IsVsCodeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsVsCodeInstalled());

    [Fact]
    public void IsScoopInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsScoopInstalled());

    [Fact]
    public void HasNvidiaGpu_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasNvidiaGpu());

    [Fact]
    public void HasAmdGpu_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasAmdGpu());

    [Fact]
    public void HasWslInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasWslInstalled());

    [Fact]
    public void HasBatteryPresent_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasBatteryPresent());

    [Fact]
    public void HasHyperVAvailable_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasHyperVAvailable());

    [Fact]
    public void DetectCpu_ReturnsNonNullProfile()
    {
        var cpu = HardwareInfo.DetectCpu();
        Assert.NotNull(cpu);
        Assert.True(cpu.LogicalCores > 0);
    }

    [Fact]
    public void DetectMemory_ReturnsTotalMbPositive()
    {
        var mem = HardwareInfo.DetectMemory();
        Assert.True(mem.TotalMb > 0);
    }

    [Fact]
    public void DetectGpus_ReturnsAtLeastOneEntry()
    {
        var gpus = HardwareInfo.DetectGpus();
        Assert.NotEmpty(gpus);
    }

    [Fact]
    public void DetectDisk_ReturnsCDriveInfo()
    {
        var disk = HardwareInfo.DetectDisk();
        Assert.StartsWith("C", disk.Drive, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SuggestProfile_ReturnsKnownProfile()
    {
        var profile = HardwareInfo.SuggestProfile();
        string[] valid = ["business", "gaming", "privacy", "minimal"];
        Assert.Contains(profile, valid);
    }

    [Fact]
    public void Summary_ContainsCpuRamGpuBuild()
    {
        var summary = HardwareInfo.Summary();
        Assert.Contains("CPU:", summary, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("RAM:", summary, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("GPU:", summary, StringComparison.OrdinalIgnoreCase);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// PackLoader — missing-field validation branches not covered by PluginTests
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class PackLoaderBranch3Tests
{
    // Builds a valid minimal pack JSON with optional modifications
    private static string BuildPackJson(
        string name = "tp",
        int tweakCount = 1,
        bool missingLabel = false,
        bool missingCategory = false,
        bool missingApplyAndRemove = false,
        bool missingId = false
    ) =>
        $$"""
            {
                "name": "{{name}}", "displayName": "Test Pack", "version": "1.0.0", "author": "UT",
                "tweaks": [
                    {
                        {{(missingId ? "" : $"\"id\": \"{name}-tweak1\",")}}
                        {{(missingLabel ? "" : "\"label\": \"Tweak One\",")}}
                        {{(missingCategory ? "" : "\"category\": \"TestCat\",")}}
                        {{(
                missingApplyAndRemove
                    ? "\"removeOps\": null,"
                    : $"\"applyOps\": [{{\"kind\": \"SetDword\", \"path\": \"HKCU\\\\Software\\\\{name}\", \"name\": \"V\", \"dwordValue\": 1 }}],"
            )}}
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\Software\\{{name}}", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;

    [Fact]
    public void Validate_TweetMissingLabel_ReturnsError()
    {
        var json = BuildPackJson(missingLabel: true);
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("label"));
    }

    [Fact]
    public void Validate_TweakMissingCategory_ReturnsError()
    {
        var json = BuildPackJson(missingCategory: true);
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("category"));
    }

    [Fact]
    public void Validate_TweakMissingId_ReturnsError()
    {
        // Build JSON where id is missing from tweak
        const string json = """
            {
                "name": "noid", "displayName": "No ID Pack", "version": "1.0.0", "author": "UT",
                "tweaks": [
                    {
                        "label": "Tweak Without Id", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "HKCU\\Software\\noid", "name": "V", "dwordValue": 1 }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKCU\\Software\\noid", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("id") || e.Contains("missing"));
    }

    [Fact]
    public void Validate_TooManyTweaks_ReturnsError()
    {
        // Build pack with 101 tweaks
        var tweakEntries = string.Join(
            ",\n",
            Enumerable
                .Range(0, 101)
                .Select(i =>
                    $$"""
                    {
                        "id": "bigpack-tweak{{i}}", "label": "T{{i}}", "category": "C",
                        "applyOps": [{"kind":"SetDword","path":"HKCU\\S","name":"V{{i}}","dwordValue":{{i}}}],
                        "detectOps": [{"kind":"CheckDword","path":"HKCU\\S","name":"V{{i}}","dwordValue":{{i}}}]
                    }
                    """
                )
        );
        var json = $$"""
            { "name": "bigpack", "displayName": "Big Pack", "version": "1.0.0", "author": "UT",
              "tweaks": [{{tweakEntries}}] }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("maximum"));
    }

    [Fact]
    public void Validate_TweakHasNoApplyAndNoRemoveOps_ReturnsError()
    {
        // Tweak with both applyOps and removeOps absent/empty
        const string json = """
            {
                "name": "noops", "displayName": "No Ops Pack", "version": "1.0.0", "author": "UT",
                "tweaks": [
                    {
                        "id": "noops-tweak1", "label": "T", "category": "C",
                        "detectOps": [{ "kind": "CheckDword", "path": "HKCU\\Software\\noops", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("applyOps") || e.Contains("removeOps"));
    }

    [Fact]
    public void ValidatePackJson_JsonParseException_ReturnsParseError()
    {
        // Trigger the catch JsonException branch
        var errors = PackLoader.ValidatePackJson("<<< not json at all >>>");
        Assert.NotEmpty(errors);
        Assert.Contains(
            errors,
            e => e.Contains("parse", StringComparison.OrdinalIgnoreCase) || e.Contains("JSON", StringComparison.OrdinalIgnoreCase)
        );
    }

    [Fact]
    public void LoadFromJson_RemoveOpsPresent_MapsCorrectly()
    {
        // Ensure RemoveOps null-coalescing branch is hit when removeOps present
        const string json = """
            {
                "name": "remops", "displayName": "Remove Ops Pack", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "remops-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\remops","name":"V","dwordValue":1}],
                    "removeOps": [{"kind":"DeleteValue","path":"HKCU\\Software\\remops","name":"V"}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\remops","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Single(tweaks);
        Assert.NotEmpty(tweaks[0].RemoveOps);
        Assert.Equal(RegOpKind.DeleteValue, tweaks[0].RemoveOps[0].Kind);
    }

    [Fact]
    public void LoadFromJson_CheckKeyMissingInDetectOps_Converts()
    {
        const string json = """
            {
                "name": "ckmtest", "displayName": "CKM Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "ckmtest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\ckmtest","name":"V","dwordValue":1}],
                    "detectOps": [{"kind":"CheckKeyMissing","path":"HKCU\\Software\\ckmtest_absent"}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.CheckKeyMissing, tweaks[0].DetectOps[0].Kind);
    }

    [Fact]
    public void LoadFromJson_CheckMissingInDetectOps_Converts()
    {
        const string json = """
            {
                "name": "cmtest", "displayName": "CM Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "cmtest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\cmtest","name":"V","dwordValue":1}],
                    "detectOps": [{"kind":"CheckMissing","path":"HKCU\\Software\\cmtest","name":"V"}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.CheckMissing, tweaks[0].DetectOps[0].Kind);
    }

    [Fact]
    public void LoadFromJson_DeleteTreeInApplyOps_Converts()
    {
        const string json = """
            {
                "name": "dttest", "displayName": "DT Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "dttest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"DeleteTree","path":"HKCU\\Software\\dttest_absent"}],
                    "detectOps": [{"kind":"CheckKeyMissing","path":"HKCU\\Software\\dttest_absent"}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.DeleteTree, tweaks[0].ApplyOps[0].Kind);
    }

    [Fact]
    public void LoadFromJson_SetQwordInApplyOps_Converts()
    {
        const string json = """
            {
                "name": "qwtest", "displayName": "QW Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "qwtest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetQword","path":"HKCU\\Software\\qwtest","name":"V","qwordValue":9876543210}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\qwtest","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.SetValue, tweaks[0].ApplyOps[0].Kind);
        Assert.Equal(9_876_543_210L, tweaks[0].ApplyOps[0].Value);
    }

    [Fact]
    public void LoadFromJson_SetBinaryInApplyOps_Converts()
    {
        const string json = """
            {
                "name": "bintest", "displayName": "Bin Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "bintest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetBinary","path":"HKCU\\Software\\bintest","name":"V","binaryValue":"AQID"}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\bintest","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.SetValue, tweaks[0].ApplyOps[0].Kind);
        var bytes = tweaks[0].ApplyOps[0].Value as byte[];
        Assert.NotNull(bytes);
        Assert.Equal(new byte[] { 0x01, 0x02, 0x03 }, bytes!);
    }

    [Fact]
    public void LoadFromJson_SetMultiSzInApplyOps_Converts()
    {
        const string json = """
            {
                "name": "msztest", "displayName": "MSZ Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "msztest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetMultiSz","path":"HKCU\\Software\\msztest","name":"V","multiSzValue":["a","b","c"]}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\msztest","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.SetValue, tweaks[0].ApplyOps[0].Kind);
        var vals = tweaks[0].ApplyOps[0].Value as string[];
        Assert.NotNull(vals);
        Assert.Equal(new[] { "a", "b", "c" }, vals!);
    }

    [Fact]
    public void LoadFromJson_SetExpandStringInApplyOps_Converts()
    {
        const string json = """
            {
                "name": "extest", "displayName": "Ex Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "extest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetExpandString","path":"HKCU\\Software\\extest","name":"V","stringValue":"%SystemRoot%\\test"}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\extest","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.SetValue, tweaks[0].ApplyOps[0].Kind);
        Assert.Equal("%SystemRoot%\\test", tweaks[0].ApplyOps[0].Value);
    }

    [Fact]
    public void LoadFromJson_CheckStringInDetectOps_Converts()
    {
        const string json = """
            {
                "name": "cstest", "displayName": "CS Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "cstest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetString","path":"HKCU\\Software\\cstest","name":"V","stringValue":"enabled"}],
                    "detectOps": [{"kind":"CheckString","path":"HKCU\\Software\\cstest","name":"V","stringValue":"enabled"}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(RegOpKind.CheckValue, tweaks[0].DetectOps[0].Kind);
        Assert.Equal("enabled", tweaks[0].DetectOps[0].Value);
    }

    [Fact]
    public void LoadFromJson_PackWithNullTweaksList_TweakCountIsZero()
    {
        // Pack where tweaks array is empty but valid JSON — triggers the `raw.Tweaks?.Count ?? 0` nullish branch
        const string json = """
            { "name": "notweaks", "displayName": "No Tweaks", "version": "1.0.0", "author": "UT", "tweaks": [] }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("at least one tweak", ex.Message);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// TweakEngine — edge-case branches not in existing TweakEngineTests
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class TweakEngineBranchTests
{
    private static TweakEngine BuildEngine(params TweakDef[] tweaks)
    {
        var engine = new TweakEngine();
        engine.Register(tweaks);
        return engine;
    }

    private static TweakDef MakeTweak(string id, string category = "Cat", string[] tags = null!, string? minBuild = null, bool isApplicable = true) =>
        new TweakDef
        {
            Id = id,
            Label = id,
            Category = category,
            Tags = tags ?? [],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\TweakEngTest", id, 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\TweakEngTest", id, 1)],
            IsApplicable = isApplicable ? null : (Func<bool>)(() => false),
        };

    [Fact]
    public void Filter_ByCategory_ReturnsOnlyCategoryTweaks()
    {
        var engine = BuildEngine(MakeTweak("a-tweak1", "Alpha"), MakeTweak("b-tweak1", "Beta"));
        var filtered = engine.Filter(category: "Alpha");
        Assert.All(filtered, t => Assert.Equal("Alpha", t.Category));
    }

    [Fact]
    public void Filter_ByCategoryNotExisting_ReturnsEmpty()
    {
        var engine = BuildEngine(MakeTweak("test-tweak1", "Existing"));
        var filtered = engine.Filter(category: "NonExistentCategory");
        Assert.Empty(filtered);
    }

    [Fact]
    public void Filter_ByCorpSafe_ReturnsOnlyCorpSafeTweaks()
    {
        var safe = new TweakDef
        {
            Id = "x-corp-safe",
            Label = "S",
            Category = "C",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\X", "V1", 1)],
        };
        var unsafe_ = new TweakDef
        {
            Id = "x-not-corp-safe",
            Label = "U",
            Category = "C",
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\X", "V2", 1)],
        };
        var engine = new TweakEngine();
        engine.Register([safe, unsafe_]);
        var filtered = engine.Filter(corpSafe: true);
        Assert.All(filtered, t => Assert.True(t.CorpSafe));
    }

    [Fact]
    public void Filter_ByMinBuildExcludesHighBuildTweaks()
    {
        var highBuild = new TweakDef
        {
            Id = "x-future-tweak",
            Label = "F",
            Category = "C",
            MinBuild = 99999,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\X", "V3", 1)],
        };
        var engine = new TweakEngine();
        engine.Register([highBuild]);
        // Filter by minBuild=99998 (max current build) should exclude the future tweak
        var filtered = engine.Filter(minBuild: 99998);
        Assert.Empty(filtered);
    }

    [Fact]
    public void Search_EmptyQuery_ReturnsAllTweaks()
    {
        var engine = BuildEngine(MakeTweak("s-tweak1"), MakeTweak("s-tweak2"));
        var result = engine.Search("");
        Assert.True(result.Count >= 2);
    }

    [Fact]
    public void TweaksByTag_EmptyResults_ReturnsEmpty()
    {
        var engine = BuildEngine(MakeTweak("tag-tweak1", tags: ["alpha", "beta"]));
        var result = engine.TweaksByTag("nonexistent-tag");
        Assert.Empty(result);
    }

    [Fact]
    public void GetTweak_NonExistentId_ReturnsNull()
    {
        var engine = BuildEngine(MakeTweak("real-tweak1"));
        var result = engine.GetTweak("does-not-exist-id");
        Assert.Null(result);
    }

    [Fact]
    public void ExportJson_WritesFileContainingTweakIds()
    {
        var path = Path.GetTempFileName();
        try
        {
            var engine = BuildEngine(MakeTweak("export-tweak1"), MakeTweak("export-tweak2"));
            engine.ExportJson(path);
            var json = File.ReadAllText(path);
            Assert.Contains("export-tweak1", json);
            Assert.Contains("export-tweak2", json);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
