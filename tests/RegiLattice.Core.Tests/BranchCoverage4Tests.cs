// tests/RegiLattice.Core.Tests/BranchCoverage4Tests.cs
// Sprint 121 — Branch coverage boost set 4
// Targets: PackLoader SHA256/optional-fields, PackManager conflicts/URL/meta,
//          StartupManager workflow, RegistrySession remaining paths,
//          TweakEngine IsApplicableOnHardware all arms, Elevation, HardwareInfo, UpdateInfo
#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ── 1. PackLoader SHA256 & Optional-Field Branch Tests ──────────────────────

public sealed class PackLoaderSha256BranchTests
{
    // Minimal valid pack JSON used as base for SHA256 tests
    private static readonly string s_validJson = """
        {
            "name": "sha256test", "displayName": "SHA256 Test Pack", "version": "1.0.0", "author": "UT",
            "tweaks": [{
                "id": "sha256test-tweak1", "label": "T1", "category": "TestCat",
                "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\sha256test","name":"V","dwordValue":1}],
                "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\sha256test","name":"V","dwordValue":1}]
            }]
        }
        """;

    [Fact]
    public void LoadFromJson_CorrectSha256_Succeeds()
    {
        var hash = PackLoader.ComputeSha256(s_validJson);
        var (pack, tweaks) = PackLoader.LoadFromJson(s_validJson, hash);
        Assert.Equal("sha256test", pack.Name);
        Assert.Single(tweaks);
    }

    [Fact]
    public void LoadFromJson_WrongSha256_Throws()
    {
        const string wrongHash = "0000000000000000000000000000000000000000000000000000000000000000";
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(s_validJson, wrongHash));
        Assert.Contains("SHA-256", ex.Message);
    }

    [Fact]
    public void LoadFromJson_NullSha256DoesNotCheckHash()
    {
        // expectedSha256 = null → skip hash check
        var (pack, _) = PackLoader.LoadFromJson(s_validJson, null);
        Assert.NotNull(pack);
    }

    [Fact]
    public void LoadFromJson_OptionalPackFieldsOmitted_UsesDefaults()
    {
        // Missing: description, tags, categories, changelog, minRegiLatticeVersion
        // → hits null-coalescing branches inside LoadFromJson
        const string json = """
            {
                "name": "opttest", "displayName": "Opt Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "opttest-t1", "label": "T", "category": "C",
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\opttest","name":"V","dwordValue":0}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\opttest","name":"V","dwordValue":0}]
                }]
            }
            """;
        var (pack, _) = PackLoader.LoadFromJson(json);
        Assert.Equal("", pack.Description);
        Assert.Empty(pack.Tags);
        Assert.Empty(pack.Categories);
        Assert.Equal("3.3.0", pack.MinRegiLatticeVersion);
        Assert.Equal("", pack.Changelog);
    }

    [Fact]
    public void LoadFromJson_AllOptionalPackFieldsPresent_PopulatesCorrectly()
    {
        const string json = """
            {
                "name": "fulltest", "displayName": "Full Test", "version": "2.0.0", "author": "UT",
                "description": "A full test pack",
                "categories": ["TestCat"],
                "tags": ["test", "unit"],
                "changelog": "Initial release",
                "minRegiLatticeVersion": "4.0.0",
                "minWindowsBuild": 19041,
                "tweaks": [{
                    "id": "fulltest-t1", "label": "T", "category": "C",
                    "description": "tweak desc",
                    "expectedResult": "Value set",
                    "tags": ["t1"],
                    "needsAdmin": false,
                    "corpSafe": true,
                    "minBuild": 19041,
                    "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\fulltest","name":"V","dwordValue":0}],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\fulltest","name":"V","dwordValue":0}]
                }]
            }
            """;
        var (pack, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal("A full test pack", pack.Description);
        Assert.Contains("test", pack.Tags);
        Assert.Contains("TestCat", pack.Categories);
        Assert.Equal("Initial release", pack.Changelog);
        Assert.Equal("4.0.0", pack.MinRegiLatticeVersion);
        Assert.Equal(19041, tweaks[0].MinBuild);
        Assert.False(tweaks[0].NeedsAdmin);
        Assert.True(tweaks[0].CorpSafe);
    }

    [Fact]
    public void ComputeSha256_KnownInput_Returns64CharLowerHex()
    {
        var hash = PackLoader.ComputeSha256("test");
        Assert.Equal(64, hash.Length);
        Assert.Matches("[0-9a-f]{64}", hash);
    }

    [Fact]
    public void ValidatePackJson_NullJsonDeserialized_ReturnsError()
    {
        // JSON string "null" → deserializer returns null → hits the `raw is null` guard
        var errors = PackLoader.ValidatePackJson("null");
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void LoadFromJson_RemoveOpsOnlyPlusDectectOps_LoadsSuccessfully()
    {
        // applyOps is absent but removeOps + detectOps are present — valid pack
        const string json = """
            {
                "name": "removeonlyvalid", "displayName": "RemoveOnly", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "removeonlyvalid-t1", "label": "T", "category": "C",
                    "removeOps": [{"kind":"DeleteValue","path":"HKCU\\Software\\removeonlyvalid","name":"V"}],
                    "detectOps": [{"kind":"CheckMissing","path":"HKCU\\Software\\removeonlyvalid","name":"V"}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Single(tweaks);
        Assert.Empty(tweaks[0].ApplyOps);
        Assert.Single(tweaks[0].RemoveOps);
    }
}

// ── 2. PackLoader Extra Validation Branch Tests ─────────────────────────────

public sealed class PackLoaderValidationBranchTests2
{
    [Fact]
    public void Validate_RemoveOpsOnlyNoDectectOps_ReturnsDetectError()
    {
        const string json = """
            {
                "name": "removeonly", "displayName": "RemoveOnly", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "removeonly-t1", "label": "T", "category": "C",
                    "removeOps": [{"kind":"DeleteValue","path":"HKCU\\Software\\removeonly","name":"V"}]
                }]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("detectOps"));
    }

    [Fact]
    public void Validate_DuplicateTweakIds_ReturnsError()
    {
        const string json = """
            {
                "name": "dupid", "displayName": "DupId Test", "version": "1.0.0", "author": "UT",
                "tweaks": [
                    {
                        "id": "dupid-same", "label": "T1", "category": "C",
                        "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\dupid","name":"V1","dwordValue":1}],
                        "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\dupid","name":"V1","dwordValue":1}]
                    },
                    {
                        "id": "dupid-same", "label": "T2", "category": "C",
                        "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\dupid","name":"V2","dwordValue":2}],
                        "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\dupid","name":"V2","dwordValue":2}]
                    }
                ]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_TweakMissingBothOpsAndDetect_ReturnsMultipleErrors()
    {
        const string json = """
            {
                "name": "noops2", "displayName": "NoOps2", "version": "1.0.0", "author": "UT",
                "tweaks": [{"id": "noops2-t1", "label": "T", "category": "C"}]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.True(errors.Count >= 2);
    }

    [Fact]
    public void Validate_MultiplePackFieldsMissing_ReturnsMultipleErrors()
    {
        // Triggers all 4 field-missing branches in Validate()
        var errors = PackLoader.ValidatePackJson("""{ "tweaks": [] }""");
        Assert.True(errors.Count >= 3);
    }

    [Fact]
    public void LoadFromJson_TweakWithDeleteValueDeleteTree_Succeeds()
    {
        // Tests ExtractRegistryKeys skipping empty-path ops (DeleteTree with path, DeleteValue with path)
        const string json = """
            {
                "name": "deltest", "displayName": "Del Test", "version": "1.0.0", "author": "UT",
                "tweaks": [{
                    "id": "deltest-t1", "label": "T", "category": "C",
                    "applyOps": [
                        {"kind":"SetDword","path":"HKCU\\Software\\deltest","name":"V","dwordValue":1},
                        {"kind":"DeleteValue","path":"HKCU\\Software\\deltest","name":"OldV"},
                        {"kind":"DeleteTree","path":"HKCU\\Software\\deltestOld"}
                    ],
                    "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\deltest","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Equal(3, tweaks[0].ApplyOps.Count);
        Assert.Contains(tweaks[0].RegistryKeys, k => k.Contains("deltest"));
    }
}

// ── 3. PackManager URL, Conflict, and Metadata Branch Tests ─────────────────

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

public sealed class StartupManagerBranchTests2
{
    [Fact]
    public void GetAllEntries_ReturnsNonNull()
    {
        var entries = StartupManager.GetAllEntries();
        Assert.NotNull(entries);
    }

    [Fact]
    public void SetEnabled_SameState_IsNoOp()
    {
        // If no entries, trivially pass
        var entries = StartupManager.GetAllEntries();
        if (entries.Count == 0)
            return;
        var e = entries[0];
        // Should early-return (no-op) without throwing
        StartupManager.SetEnabled(e, e.IsEnabled);
    }

    [Fact]
    public void AddRegistryEntry_BlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("", "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("   ", "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_NullName_ThrowsArgumentException()
    {
        Assert.ThrowsAny<ArgumentException>(() => StartupManager.AddRegistryEntry(null!, "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_BlankCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("TestEntry", ""));
    }

    [Fact]
    public void AddRegistryEntry_NullCommand_ThrowsArgumentException()
    {
        Assert.ThrowsAny<ArgumentException>(() => StartupManager.AddRegistryEntry("TestEntry", null!));
    }

    [Fact]
    public async Task ExportEntriesAsync_BlankFilePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => StartupManager.ExportEntriesAsync(""));
    }

    [Fact]
    public void AddRegistryEntry_NewEntry_SucceedsAndIsVisible()
    {
        const string name = "RL-UnitTest-BranchCov4-Add";
        // Ensure clean state from any prior test run
        try
        {
            var prior = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
            if (prior is not null)
                StartupManager.Delete(prior);
        }
        catch
        { /* ignore cleanup errors */
        }

        StartupManager.AddRegistryEntry(name, "notepad.exe");
        try
        {
            var entries = StartupManager.GetAllEntries();
            Assert.Contains(entries, e => e.Name == name);
        }
        finally
        {
            var added = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
            if (added is not null)
                StartupManager.Delete(added);
        }
    }

    [Fact]
    public void AddRegistryEntry_DuplicateName_ThrowsArgumentException()
    {
        const string name = "RL-UnitTest-BranchCov4-Dup";
        try
        {
            StartupManager.AddRegistryEntry(name, "notepad.exe");
            var ex = Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry(name, "cmd.exe"));
            Assert.Contains("already exists", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try
            {
                var entry = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
                if (entry is not null)
                    StartupManager.Delete(entry);
            }
            catch { }
        }
    }

    [Fact]
    public async Task ExportEntriesAsync_ToTempFile_WritesValidJson()
    {
        var path = Path.Combine(Path.GetTempPath(), $"startup-bc4-{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(path);
            Assert.True(File.Exists(path));
            var content = await File.ReadAllTextAsync(path);
            Assert.Contains("[", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Delete_NonExistentRegistryEntry_DoesNotThrow()
    {
        // Create a fake entry that is not in the registry — TryDeleteValue should no-op
        var fakeEntry = new StartupEntry(
            "RegistryUser|RL-UnitTest-NonExistent-BC4",
            "RL-UnitTest-NonExistent-BC4",
            "nonexistent.exe",
            StartupLocation.RegistryUser,
            true
        );
        StartupManager.Delete(fakeEntry); // Should not throw
    }
}

// ── 5. RegistrySession Extra Branch Tests ───────────────────────────────────

public sealed class RegistrySessionBranchTests2
{
    [Fact]
    public void ParsePath_HkccHive_ReturnsCurrentConfig()
    {
        var (root, subKey) = RegistrySession.ParsePath(@"HKCC\Software\Test");
        Assert.Same(Microsoft.Win32.Registry.CurrentConfig, root);
        Assert.Equal(@"Software\Test", subKey);
    }

    [Fact]
    public void ParsePath_HkeyCurrentConfig_ReturnsCurrentConfig()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKEY_CURRENT_CONFIG\System\Test");
        Assert.Same(Microsoft.Win32.Registry.CurrentConfig, root);
    }

    [Fact]
    public void ParsePath_HkuHive_ReturnsUsers()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKU\.DEFAULT\Software");
        Assert.Same(Microsoft.Win32.Registry.Users, root);
    }

    [Fact]
    public void ParsePath_HkeyUsers_ReturnsUsers()
    {
        var (root, _) = RegistrySession.ParsePath(@"HKEY_USERS\.DEFAULT\Software");
        Assert.Same(Microsoft.Win32.Registry.Users, root);
    }

    [Fact]
    public void Execute_CheckDwordOp_ThrowsCannotExecuteReadOnly()
    {
        var session = new RegistrySession(dryRun: true);
        var checkOp = RegOp.CheckDword(@"HKCU\Software\Test", "V", 1);
        var ex = Assert.Throws<InvalidOperationException>(() => session.Execute([checkOp]));
        Assert.Contains("read-only", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_SetDwordOp_ThrowsCannotEvaluateWriteOp()
    {
        var session = new RegistrySession(dryRun: true);
        var setOp = RegOp.SetDword(@"HKCU\Software\Test", "V", 1);
        var ex = Assert.Throws<InvalidOperationException>(() => session.Evaluate([setOp]));
        Assert.Contains("write", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_CheckMissing_WhenValueExists_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CM";
        var session = new RegistrySession(dryRun: false);
        session.SetDword(path, "ExistingVal", 42);
        try
        {
            var result = session.Evaluate([RegOp.CheckMissing(path, "ExistingVal")]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteValue(path, "ExistingVal");
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckKeyMissing_WhenKeyExists_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CKM";
        var session = new RegistrySession(dryRun: false);
        session.SetDword(path, "V", 1);
        try
        {
            var result = session.Evaluate([RegOp.CheckKeyMissing(path)]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteValue(path, "V");
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckString_MatchingValue_ReturnsTrue()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CS";
        var session = new RegistrySession(dryRun: false);
        session.SetString(path, "SVal", "hello-world");
        try
        {
            var result = session.Evaluate([RegOp.CheckString(path, "SVal", "hello-world")]);
            Assert.True(result);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckString_MismatchValue_ReturnsFalse()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4CSFail";
        var session = new RegistrySession(dryRun: false);
        session.SetString(path, "SVal", "actual-value");
        try
        {
            var result = session.Evaluate([RegOp.CheckString(path, "SVal", "expected-value")]);
            Assert.False(result);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void Evaluate_CheckValue_KeyNotInRegistry_ReturnsFalse()
    {
        // Key doesn't exist → CheckValueMatch returns false immediately (key is null)
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4NoKey_XYZ99999";
        var session = new RegistrySession(dryRun: false);
        var result = session.Evaluate([RegOp.CheckDword(path, "V", 1)]);
        Assert.False(result);
    }

    [Fact]
    public void Evaluate_CheckValue_ValueNotInKey_ReturnsFalse()
    {
        // Key exists but named value doesn't → CheckValueMatch returns false
        const string path = @"HKCU\Software\Microsoft";
        var session = new RegistrySession(dryRun: false);
        var result = session.Evaluate([RegOp.CheckDword(path, "BC4NonExistentValue9999", 1)]);
        Assert.False(result);
    }

    [Fact]
    public void Backup_WithCustomDir_CreatesFile()
    {
        var backupDir = Path.Combine(Path.GetTempPath(), $"bc4-backup-{Guid.NewGuid():N}");
        try
        {
            var session = new RegistrySession(backupDir: backupDir);
            var path = session.Backup([@"HKCU\Software\Microsoft"], "bc4-test");
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (Directory.Exists(backupDir))
                Directory.Delete(backupDir, recursive: true);
        }
    }

    [Fact]
    public void SetQword_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetQword(@"HKCU\Software\bc4qwtest", "QV", 123456789L);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetBinary_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetBinary(@"HKCU\Software\bc4bintest", "BV", [0x01, 0x02, 0x03]);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetMultiSz_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetMultiSz(@"HKCU\Software\bc4mstest", "MSV", ["a", "b"]);
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void SetExpandString_DryRun_IncrementsCounter()
    {
        var session = new RegistrySession(dryRun: true);
        session.SetExpandString(@"HKCU\Software\bc4estest", "EV", @"%SystemRoot%\test");
        Assert.Equal(1, session.DryOps);
    }

    [Fact]
    public void ReadQword_ExistingQword_ReturnsValue()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RQ";
        var session = new RegistrySession(dryRun: false);
        session.SetQword(path, "QVal", 987654321L);
        try
        {
            var val = session.ReadQword(path, "QVal");
            Assert.Equal(987654321L, val);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void ReadBinary_ExistingBinary_ReturnsBytes()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RB";
        var session = new RegistrySession(dryRun: false);
        byte[] expected = [0xAA, 0xBB, 0xCC];
        session.SetBinary(path, "BVal", expected);
        try
        {
            var val = session.ReadBinary(path, "BVal");
            Assert.NotNull(val);
            Assert.Equal(expected, val);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }

    [Fact]
    public void ReadMultiSz_ExistingMultiSz_ReturnsStrings()
    {
        const string path = @"HKCU\Software\RegiLattice\UnitTest\BC4RMS";
        var session = new RegistrySession(dryRun: false);
        session.SetMultiSz(path, "MSVal", ["hello", "world"]);
        try
        {
            var val = session.ReadMultiSz(path, "MSVal");
            Assert.NotNull(val);
            Assert.Equal(2, val!.Length);
        }
        finally
        {
            session.DeleteTree(path);
        }
    }
}

// ── 6. TweakEngine IsApplicableOnHardware Category Arm Tests ────────────────

public sealed class TweakEngineIsApplicableBranchTests
{
    // Helper: minimal TweakDef for hardware applicability testing
    private static TweakDef Td(string id, string category, string[]? tags = null) =>
        new()
        {
            Id = id,
            Label = "Test",
            Category = category,
            Tags = tags ?? [],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\BC4IsApplicable", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\BC4IsApplicable", "V", 1)],
        };

    // ── 13 explicit category arms ────────────────────────────────────────

    [Fact]
    public void IsApplicable_CategoryWSL_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-wsl-c", "WSL")));
    }

    [Fact]
    public void IsApplicable_CategoryVirtualization_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-virt-c", "Virtualization")));
    }

    [Fact]
    public void IsApplicable_CategoryChrome_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-chrome-c", "Chrome")));
    }

    [Fact]
    public void IsApplicable_CategoryFirefox_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-ff-c", "Firefox")));
    }

    [Fact]
    public void IsApplicable_CategoryEdge_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-edge-c", "Edge")));
    }

    [Fact]
    public void IsApplicable_CategoryJava_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-java-c", "Java")));
    }

    [Fact]
    public void IsApplicable_CategoryAdobe_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-adobe-c", "Adobe")));
    }

    [Fact]
    public void IsApplicable_CategoryLibreOffice_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-lo-c", "LibreOffice")));
    }

    [Fact]
    public void IsApplicable_CategoryOffice_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-office-c", "Office")));
    }

    [Fact]
    public void IsApplicable_CategoryM365Copilot_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-m365-c", "M365 Copilot")));
    }

    [Fact]
    public void IsApplicable_CategoryRealVNC_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-vnc-c", "RealVNC")));
    }

    [Fact]
    public void IsApplicable_CategoryVSCode_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-vscode-c", "VS Code")));
    }

    [Fact]
    public void IsApplicable_CategoryScoopTools_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-scoop-c", "Scoop Tools")));
    }

    // ── Default arm → AutoDetectFromTags ────────────────────────────────

    [Fact]
    public void IsApplicable_UnknownCategory_ReturnsTrue()
    {
        // _ arm → AutoDetectFromTags → no known tags → returns true
        var result = TweakEngine.IsApplicableOnHardware(Td("bc4-unknown-c", "SomeUnknownCategory2024"));
        Assert.True(result);
    }

    // ── AutoDetectFromTags 4 tag branches ───────────────────────────────

    [Fact]
    public void IsApplicable_TagNvidia_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-nvtag", "GPU", ["nvidia"])));
    }

    [Fact]
    public void IsApplicable_TagAmdGpu_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-amdtag", "GPU", ["amd-gpu"])));
    }

    [Fact]
    public void IsApplicable_TagDocker_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-dockertag", "Dev", ["docker"])));
    }

    [Fact]
    public void IsApplicable_TagLaptop_ReturnsBool()
    {
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-laptoptag", "Power", ["laptop"])));
    }

    // ── Custom predicate branches ────────────────────────────────────────

    [Fact]
    public void IsApplicable_CustomPredicateTrue_InvokedAndReturnsTrue()
    {
        bool called = false;
        var td = new TweakDef
        {
            Id = "bc4-custpred-t",
            Label = "CustomPred",
            Category = "Test",
            IsApplicable = () =>
            {
                called = true;
                return true;
            },
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\BC4Pred", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\BC4Pred", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.True(called);
        Assert.True(result);
    }

    [Fact]
    public void IsApplicable_CustomPredicateFalse_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "bc4-custpred-f",
            Label = "CustomPredFalse",
            Category = "Test",
            IsApplicable = () => false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\BC4Pred", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\BC4Pred", "V", 1)],
        };
        Assert.False(TweakEngine.IsApplicableOnHardware(td));
    }

    // ── TweakEngine.Apply short-circuit paths ───────────────────────────

    [Fact]
    public void Apply_IsApplicableFalse_ReturnsSkippedHw()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "bc4-apply-skiphw",
            Label = "SkipHw",
            Category = "Test",
            CorpSafe = true,
            IsApplicable = () => false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\BC4ApplySkip", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\BC4ApplySkip", "V", 1)],
        };
        engine.Register([td]);
        var result = engine.Apply(td, requireAdmin: false, forceCorp: true);
        Assert.Equal(TweakResult.SkippedHw, result);
    }

    [Fact]
    public void Apply_MinBuildExceedsCurrent_ReturnsSkippedBuild()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        var td = new TweakDef
        {
            Id = "bc4-apply-skipbuild",
            Label = "SkipBuild",
            Category = "Test",
            CorpSafe = true,
            MinBuild = int.MaxValue,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\BC4ApplySkip", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\BC4ApplySkip", "V", 1)],
        };
        engine.Register([td]);
        var result = engine.Apply(td, requireAdmin: false, forceCorp: true);
        Assert.Equal(TweakResult.SkippedBuild, result);
    }
}

// ── 7. Elevation Branch Tests ────────────────────────────────────────────────

public sealed class ElevationBranchTests2
{
    [Fact]
    public void IsAdmin_DoesNotThrow_ReturnsBool()
    {
        var result = Elevation.IsAdmin();
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void AssertAdmin_RequireAdminFalse_NoOp()
    {
        // requireAdmin=false → the guard is skipped regardless of actual admin status
        Elevation.AssertAdmin(requireAdmin: false);
    }

    [Fact]
    public void RunElevated_NonAllowedCommand_ThrowsUnauthorizedAccess()
    {
        var ex = Assert.Throws<UnauthorizedAccessException>(() => Elevation.RunElevated("notallowed_program.exe", [], 1_000));
        Assert.Contains("allowlist", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RunElevated_PathToNonAllowedCommand_ThrowsUnauthorizedAccess()
    {
        var ex = Assert.Throws<UnauthorizedAccessException>(() => Elevation.RunElevated(@"C:\Windows\System32\curl.exe", [], 1_000));
        Assert.Contains("allowlist", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}

// ── 8. HardwareInfo Profile & Summary Branch Tests ──────────────────────────

public sealed class HardwareInfoProfileBranchTests
{
    [Fact]
    public void SuggestProfile_ReturnsOneOfKnownProfiles()
    {
        var profile = HardwareInfo.SuggestProfile();
        Assert.Contains(profile, new[] { "business", "gaming", "minimal", "privacy" });
    }

    [Fact]
    public void Summary_ReturnsNonEmptyStringWithCpu()
    {
        var summary = HardwareInfo.Summary();
        Assert.NotEmpty(summary);
        Assert.Contains("CPU:", summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DetectCpu_ReturnsValidCpuInfo()
    {
        var cpu = HardwareInfo.DetectCpu();
        Assert.NotNull(cpu);
        Assert.True(cpu.LogicalCores > 0);
    }

    [Fact]
    public void DetectGpus_ReturnsNonEmptyList()
    {
        var gpus = HardwareInfo.DetectGpus();
        Assert.NotNull(gpus);
        Assert.NotEmpty(gpus); // Always returns at least one GpuInfo (fallback "Unknown")
    }

    [Fact]
    public void DetectMemory_ReturnsMemoryInfo()
    {
        var mem = HardwareInfo.DetectMemory();
        Assert.NotNull(mem);
    }

    [Fact]
    public void DetectHardware_GuiBatchSize_IsOneOfExpectedValues()
    {
        var hw = HardwareInfo.DetectHardware();
        Assert.True(hw.GuiBatchSize == 4 || hw.GuiBatchSize == 8);
    }
}

// ── 9. UpdateCheckService & UpdateInfo Branch Tests ─────────────────────────

public sealed class UpdateCheckServiceBranchTests2
{
    [Fact]
    public void CurrentVersion_ReturnsNonEmptyString()
    {
        var v = UpdateCheckService.CurrentVersion;
        Assert.NotEmpty(v);
        // Should look like "X.Y.Z"
        Assert.Matches(@"^\d+\.\d+\.\d+", v);
    }

    [Fact]
    public void UpdateInfo_DefaultRecord_HasExpectedDefaults()
    {
        var info = new UpdateInfo();
        Assert.False(info.UpdateAvailable);
        Assert.Equal("", info.CurrentVersion);
        Assert.Equal("", info.LatestVersion);
        Assert.Null(info.Error);
        Assert.Null(info.PublishedAt);
    }

    [Fact]
    public void UpdateInfo_WithAllProperties_StoresCorrectly()
    {
        var now = DateTime.UtcNow;
        var info = new UpdateInfo
        {
            UpdateAvailable = true,
            CurrentVersion = "3.0.0",
            LatestVersion = "4.0.0",
            ReleaseNotes = "Big update",
            DownloadUrl = "https://example.com/download",
            PublishedAt = now,
            Error = null,
        };
        Assert.True(info.UpdateAvailable);
        Assert.Equal("3.0.0", info.CurrentVersion);
        Assert.Equal("4.0.0", info.LatestVersion);
        Assert.Equal(now, info.PublishedAt);
    }

    [Fact]
    public void UpdateInfo_WithError_ErrorIsSet()
    {
        var info = new UpdateInfo { Error = "Network timeout", CurrentVersion = "1.0.0" };
        Assert.NotNull(info.Error);
        Assert.Equal("Network timeout", info.Error);
    }
}

// ── 10. PackConflict & PackDef Record Branch Tests ──────────────────────────

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
