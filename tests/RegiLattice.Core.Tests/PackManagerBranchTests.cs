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

public sealed class PackManagerUrlAndConflictBranchTests
{
    private const string PkgAName = "rl-unit-test-bc4-conflict-a";
    private const string PkgBName = "rl-unit-test-bc4-conflict-b";

    private static string MakePackJson(string name) =>
        $$"""
            {
                "name": "{{name}}", "displayName": "Conflict Test {{name}}", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "{{name}}-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\bc4conflicttest","name":"ConflictVal","dwordValue":1}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\bc4conflicttest","name":"ConflictVal","dwordValue":1}]
                }]
            }
            """;

    [Fact]
    public async Task InstallFromUrlAsync_InvalidUrl_ThrowsArgumentException()
    {
        var pm = new PackManager();
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => pm.InstallFromUrlAsync("not-a-url-at-all"));
        Assert.Contains("Invalid", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task InstallFromUrlAsync_FileSchemeUrl_ThrowsArgumentException()
    {
        var pm = new PackManager();
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => pm.InstallFromUrlAsync("file:///C:/test.json"));
        Assert.Contains("Invalid", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task InstallFromUrlAsync_FtpSchemeUrl_ThrowsArgumentException()
    {
        var pm = new PackManager();
        var ex = await Assert.ThrowsAsync<ArgumentException>(() => pm.InstallFromUrlAsync("ftp://example.com/test.json"));
        Assert.Contains("Invalid", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void InstalledPacks_CorruptMetaJson_SkipsSilently()
    {
        var packsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "packs");
        var corruptDir = Path.Combine(packsDir, "rl-unit-test-bc4-corrupt-meta");
        Directory.CreateDirectory(corruptDir);
        File.WriteAllText(Path.Combine(corruptDir, "meta.json"), "<<<INVALID JSON>>>");
        try
        {
            var packs = new PackManager().InstalledPacks();
            Assert.NotNull(packs); // Corrupt pack silently skipped
        }
        finally
        {
            Directory.Delete(corruptDir, recursive: true);
        }
    }

    [Fact]
    public void InstalledPacks_NullDeserializedMeta_SkipsSilently()
    {
        var packsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "packs");
        var nullDir = Path.Combine(packsDir, "rl-unit-test-bc4-null-meta");
        Directory.CreateDirectory(nullDir);
        File.WriteAllText(Path.Combine(nullDir, "meta.json"), "null");
        try
        {
            var packs = new PackManager().InstalledPacks();
            Assert.NotNull(packs);
        }
        finally
        {
            Directory.Delete(nullDir, recursive: true);
        }
    }

    [Fact]
    public void InstalledPacks_DirWithNoMetaJson_Skips()
    {
        var packsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "RegiLattice", "packs");
        var noMetaDir = Path.Combine(packsDir, "rl-unit-test-bc4-no-meta");
        Directory.CreateDirectory(noMetaDir);
        File.WriteAllText(Path.Combine(noMetaDir, "pack.json"), "{}");
        try
        {
            var packs = new PackManager().InstalledPacks();
            Assert.NotNull(packs);
        }
        finally
        {
            Directory.Delete(noMetaDir, recursive: true);
        }
    }

    [Fact]
    public void DetectConflicts_NoPacksInstalled_ReturnsNonNull()
    {
        var conflicts = new PackManager().DetectConflicts();
        Assert.NotNull(conflicts);
    }

    [Fact]
    public void DetectConflicts_TwoPacksShareRegistryOp_ReturnsConflict()
    {
        var pm = new PackManager();
        var tmpA = Path.GetTempFileName() + ".json";
        var tmpB = Path.GetTempFileName() + ".json";
        try
        {
            File.WriteAllText(tmpA, MakePackJson(PkgAName));
            File.WriteAllText(tmpB, MakePackJson(PkgBName));
            var (packA, _) = pm.InstallFromFile(tmpA);
            var (packB, _) = pm.InstallFromFile(tmpB);
            try
            {
                var conflicts = pm.DetectConflicts();
                Assert.NotEmpty(conflicts);
                Assert.Contains(
                    conflicts,
                    c =>
                        c.ConflictingPacks.Count >= 2
                        && c.ConflictingPacks.Contains(PkgAName, StringComparer.OrdinalIgnoreCase)
                        && c.ConflictingPacks.Contains(PkgBName, StringComparer.OrdinalIgnoreCase)
                );
            }
            finally
            {
                pm.UninstallPack(packA.Name);
                pm.UninstallPack(packB.Name);
            }
        }
        finally
        {
            if (File.Exists(tmpA))
                File.Delete(tmpA);
            if (File.Exists(tmpB))
                File.Delete(tmpB);
        }
    }

    [Fact]
    public void LoadAllInstalledTweaks_WithOnePack_ReturnsTweaks()
    {
        const string uniqueName = "rl-unit-test-bc4-loadall";
        var packJson = $$"""
            {
                "name": "{{uniqueName}}", "displayName": "LoadAll Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "{{uniqueName}}-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\bc4loadall","name":"V","dwordValue":1}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\bc4loadall","name":"V","dwordValue":1}]
                }]
            }
            """;
        var pm = new PackManager();
        var tmp = Path.GetTempFileName() + ".json";
        try
        {
            File.WriteAllText(tmp, packJson);
            var (pack, _) = pm.InstallFromFile(tmp);
            try
            {
                var allTweaks = pm.LoadAllInstalledTweaks();
                Assert.NotEmpty(allTweaks);
            }
            finally
            {
                pm.UninstallPack(pack.Name);
            }
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void LoadInstalledPack_WithInstallThenUninstall_RoundTrip()
    {
        const string uniqueName = "rl-unit-test-bc4-loadinstalledpack";
        var packJson = $$"""
            {
                "name": "{{uniqueName}}", "displayName": "LIP Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "{{uniqueName}}-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\bc4lip","name":"V","dwordValue":1}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\bc4lip","name":"V","dwordValue":1}]
                }]
            }
            """;
        var pm = new PackManager();
        var tmp = Path.GetTempFileName() + ".json";
        try
        {
            File.WriteAllText(tmp, packJson);
            var (pack, _) = pm.InstallFromFile(tmp);
            try
            {
                var loaded = pm.LoadInstalledPack(pack.Name);
                Assert.NotNull(loaded);
                Assert.NotEmpty(loaded!);
            }
            finally
            {
                pm.UninstallPack(pack.Name);
            }
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }
}

// ── 4. StartupManager Branch Tests ──────────────────────────────────────────

public sealed class PackConflictBranchTests
{
    [Fact]
    public void PackConflict_ConstructorAndDeconstruct_WorksCorrectly()
    {
        var packs = new List<string> { "pack-a", "pack-b" };
        var conflict = new PackConflict(@"HKCU\Software\Test", "ValueName", packs);
        Assert.Equal(@"HKCU\Software\Test", conflict.RegistryPath);
        Assert.Equal("ValueName", conflict.ValueName);
        Assert.Equal(2, conflict.ConflictingPacks.Count);
        Assert.Contains("pack-a", conflict.ConflictingPacks);
    }

    [Fact]
    public void PackConflict_EmptyValueName_StillValid()
    {
        // The DetectConflicts code passes "" when no '\0' separator found
        var conflict = new PackConflict(@"HKCU\Software\Test", "", new List<string> { "pack-a" });
        Assert.Equal("", conflict.ValueName);
    }
}

// ── merged from BranchCoverage5Tests.cs ──────────────────────────────────

public sealed class TweakEngineRegisterInstalledPacksBranchTests
{
    [Fact]
    public void RegisterInstalledPacks_NoPacks_ReturnsZero()
    {
        // A fresh engine with no packs installed calls PackManager.LoadAllInstalledTweaks()
        // which returns an empty list → tweaks.Count == 0 → T-branch of `if (count==0) return 0`
        var engine = new TweakEngine();
        int count = engine.RegisterInstalledPacks();
        Assert.Equal(0, count);
    }
}

// ── 4. AppConfig — AutoDetectPortable, Validate brightness times, Load null json ──

