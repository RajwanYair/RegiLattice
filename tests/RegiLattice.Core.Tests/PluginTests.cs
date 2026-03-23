using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the plugin/pack system: PackDef, PackLoader, PackIndex, PackManager integration.</summary>
public sealed class PluginTests
{
    // ── Sample pack JSON for testing ────────────────────────────────────

    private const string ValidPackJson = """
        {
            "name": "test-pack",
            "displayName": "Test Pack",
            "version": "1.0.0",
            "author": "Unit Tests",
            "description": "A test pack for unit testing.",
            "minRegiLatticeVersion": "3.2.0",
            "minWindowsBuild": 0,
            "categories": ["TestCat"],
            "tags": ["test", "unit-test"],
            "tweaks": [
                {
                    "id": "test-pack-disable-foo",
                    "label": "Disable Foo",
                    "category": "TestCat",
                    "description": "Disables Foo for testing.",
                    "tags": ["foo"],
                    "needsAdmin": false,
                    "corpSafe": true,
                    "minBuild": 0,
                    "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\Software\\TestPack", "name": "Foo", "dwordValue": 0 }],
                    "removeOps": [{ "kind": "DeleteValue", "path": "HKEY_CURRENT_USER\\Software\\TestPack", "name": "Foo" }],
                    "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\Software\\TestPack", "name": "Foo", "dwordValue": 0 }]
                },
                {
                    "id": "test-pack-enable-bar",
                    "label": "Enable Bar",
                    "category": "TestCat",
                    "description": "Enables Bar for testing.",
                    "tags": ["bar"],
                    "needsAdmin": true,
                    "corpSafe": false,
                    "minBuild": 22000,
                    "applyOps": [{ "kind": "SetString", "path": "HKEY_LOCAL_MACHINE\\Software\\TestPack", "name": "Bar", "stringValue": "enabled" }],
                    "removeOps": [{ "kind": "DeleteValue", "path": "HKEY_LOCAL_MACHINE\\Software\\TestPack", "name": "Bar" }],
                    "detectOps": [{ "kind": "CheckString", "path": "HKEY_LOCAL_MACHINE\\Software\\TestPack", "name": "Bar", "stringValue": "enabled" }]
                }
            ]
        }
        """;

    private const string MinimalPackJson = """
        {
            "name": "minimal-pack",
            "displayName": "Minimal Pack",
            "version": "0.1.0",
            "author": "Tester",
            "tweaks": [
                {
                    "id": "minimal-pack-tweak1",
                    "label": "Tweak 1",
                    "category": "MinCat",
                    "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\Software\\MinPack", "name": "V", "dwordValue": 1 }],
                    "removeOps": [{ "kind": "DeleteValue", "path": "HKEY_CURRENT_USER\\Software\\MinPack", "name": "V" }],
                    "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\Software\\MinPack", "name": "V", "dwordValue": 1 }]
                }
            ]
        }
        """;

    // ── PackDef record tests ────────────────────────────────────────────

    [Fact]
    public void PackDef_RequiredProperties_Populated()
    {
        var pack = new PackDef
        {
            Name = "my-pack",
            DisplayName = "My Pack",
            Version = "1.0.0",
            Author = "Author",
        };

        Assert.Equal("my-pack", pack.Name);
        Assert.Equal("My Pack", pack.DisplayName);
        Assert.Equal("1.0.0", pack.Version);
        Assert.Equal("Author", pack.Author);
    }

    [Fact]
    public void PackDef_DefaultValues_Correct()
    {
        var pack = new PackDef
        {
            Name = "x",
            DisplayName = "X",
            Version = "1.0.0",
            Author = "A",
        };

        Assert.Equal("", pack.Description);
        Assert.Equal(0, pack.TweakCount);
        Assert.Empty(pack.Categories);
        Assert.Empty(pack.Tags);
        Assert.Equal("", pack.Sha256);
        Assert.Equal("", pack.DownloadUrl);
        Assert.Equal("3.3.0", pack.MinRegiLatticeVersion);
        Assert.Equal(0, pack.MinWindowsBuild);
    }

    [Fact]
    public void PackDef_IsRecord_SupportsEquality()
    {
        var a = new PackDef
        {
            Name = "a",
            DisplayName = "A",
            Version = "1.0.0",
            Author = "X",
        };
        var b = new PackDef
        {
            Name = "a",
            DisplayName = "A",
            Version = "1.0.0",
            Author = "X",
        };
        Assert.Equal(a, b);
    }

    // ── PackLoader: LoadFromJson ────────────────────────────────────────

    [Fact]
    public void LoadFromJson_ValidPack_ReturnsTweakDefs()
    {
        var (pack, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        Assert.Equal("test-pack", pack.Name);
        Assert.Equal("Test Pack", pack.DisplayName);
        Assert.Equal("1.0.0", pack.Version);
        Assert.Equal("Unit Tests", pack.Author);
        Assert.Equal(2, pack.TweakCount);
        Assert.Equal(2, tweaks.Count);
    }

    [Fact]
    public void LoadFromJson_TweakProperties_MappedCorrectly()
    {
        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        var tweak1 = tweaks.First(t => t.Id == "test-pack-disable-foo");
        Assert.Equal("Disable Foo", tweak1.Label);
        Assert.Equal("TestCat", tweak1.Category);
        Assert.Equal("Disables Foo for testing.", tweak1.Description);
        Assert.Contains("foo", tweak1.Tags);
        Assert.False(tweak1.NeedsAdmin);
        Assert.True(tweak1.CorpSafe);
        Assert.Equal(0, tweak1.MinBuild);
        Assert.Equal("test-pack", tweak1.PackSource);

        var tweak2 = tweaks.First(t => t.Id == "test-pack-enable-bar");
        Assert.Equal("Enable Bar", tweak2.Label);
        Assert.True(tweak2.NeedsAdmin);
        Assert.False(tweak2.CorpSafe);
        Assert.Equal(22000, tweak2.MinBuild);
    }

    [Fact]
    public void LoadFromJson_RegOps_ConvertedCorrectly()
    {
        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        var tweak1 = tweaks.First(t => t.Id == "test-pack-disable-foo");
        Assert.Single(tweak1.ApplyOps);
        Assert.Equal(RegOpKind.SetValue, tweak1.ApplyOps[0].Kind);
        Assert.Equal(@"HKEY_CURRENT_USER\Software\TestPack", tweak1.ApplyOps[0].Path);
        Assert.Equal("Foo", tweak1.ApplyOps[0].Name);
        Assert.Equal(0, tweak1.ApplyOps[0].Value);

        Assert.Single(tweak1.RemoveOps);
        Assert.Equal(RegOpKind.DeleteValue, tweak1.RemoveOps[0].Kind);

        Assert.Single(tweak1.DetectOps);
        Assert.Equal(RegOpKind.CheckValue, tweak1.DetectOps[0].Kind);
    }

    [Fact]
    public void LoadFromJson_StringOps_ConvertedCorrectly()
    {
        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        var tweak2 = tweaks.First(t => t.Id == "test-pack-enable-bar");
        Assert.Single(tweak2.ApplyOps);
        Assert.Equal(RegOpKind.SetValue, tweak2.ApplyOps[0].Kind);
        Assert.Equal("enabled", tweak2.ApplyOps[0].Value);
    }

    [Fact]
    public void LoadFromJson_MinimalPack_Succeeds()
    {
        var (pack, tweaks) = PackLoader.LoadFromJson(MinimalPackJson);

        Assert.Equal("minimal-pack", pack.Name);
        Assert.Equal("0.1.0", pack.Version);
        Assert.Single(tweaks);
        Assert.Equal("minimal-pack-tweak1", tweaks[0].Id);
    }

    [Fact]
    public void LoadFromJson_RegistryKeys_ExtractedFromOps()
    {
        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        var tweak1 = tweaks.First(t => t.Id == "test-pack-disable-foo");
        Assert.Contains(@"HKEY_CURRENT_USER\Software\TestPack", tweak1.RegistryKeys);
    }

    // ── PackLoader: SHA-256 verification ────────────────────────────────

    [Fact]
    public void LoadFromJson_CorrectSha256_Succeeds()
    {
        var hash = PackLoader.ComputeSha256(ValidPackJson);
        var (pack, _) = PackLoader.LoadFromJson(ValidPackJson, hash);
        Assert.Equal("test-pack", pack.Name);
    }

    [Fact]
    public void LoadFromJson_WrongSha256_Throws()
    {
        var ex = Assert.Throws<InvalidOperationException>(() =>
            PackLoader.LoadFromJson(ValidPackJson, "0000000000000000000000000000000000000000000000000000000000000000")
        );
        Assert.Contains("SHA-256 mismatch", ex.Message);
    }

    [Fact]
    public void ComputeSha256_Deterministic()
    {
        var h1 = PackLoader.ComputeSha256("hello world");
        var h2 = PackLoader.ComputeSha256("hello world");
        Assert.Equal(h1, h2);
        Assert.Equal(64, h1.Length); // SHA-256 hex = 64 chars
    }

    [Fact]
    public void ComputeSha256_DifferentInputs_DifferentHashes()
    {
        var h1 = PackLoader.ComputeSha256("abc");
        var h2 = PackLoader.ComputeSha256("xyz");
        Assert.NotEqual(h1, h2);
    }

    // ── PackLoader: validation failures ─────────────────────────────────

    [Fact]
    public void LoadFromJson_MissingName_Throws()
    {
        var json = """
            {
                "displayName": "X", "version": "1.0.0", "author": "A",
                "tweaks": [{ "id": "x-t", "label": "T", "category": "C",
                    "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }],
                    "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                }]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("name", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadFromJson_MissingVersion_Throws()
    {
        var json = """
            {
                "name": "x", "displayName": "X", "author": "A",
                "tweaks": [{ "id": "x-t", "label": "T", "category": "C",
                    "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }],
                    "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                }]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("version", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadFromJson_NoTweaks_Throws()
    {
        var json = """
            {
                "name": "empty", "displayName": "Empty", "version": "1.0.0", "author": "A",
                "tweaks": []
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("at least one tweak", ex.Message);
    }

    [Fact]
    public void LoadFromJson_DuplicateTweakIds_Throws()
    {
        var json = """
            {
                "name": "dup", "displayName": "Dup", "version": "1.0.0", "author": "A",
                "tweaks": [
                    { "id": "dup-same", "label": "T1", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                    },
                    { "id": "dup-same", "label": "T2", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V2", "dwordValue": 2 }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V2", "dwordValue": 2 }]
                    }
                ]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("Duplicate", ex.Message);
    }

    [Fact]
    public void LoadFromJson_TweakIdNotPrefixed_Throws()
    {
        var json = """
            {
                "name": "mypack", "displayName": "My Pack", "version": "1.0.0", "author": "A",
                "tweaks": [
                    { "id": "wrong-prefix-tweak", "label": "T", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("prefixed with pack name", ex.Message);
    }

    [Fact]
    public void LoadFromJson_InvalidRegistryPath_Throws()
    {
        var json = """
            {
                "name": "badpath", "displayName": "Bad Path", "version": "1.0.0", "author": "A",
                "tweaks": [
                    { "id": "badpath-t1", "label": "T", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "SOFTWARE\\Bad", "name": "V", "dwordValue": 1 }],
                        "detectOps": [{ "kind": "CheckDword", "path": "SOFTWARE\\Bad", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("invalid registry path", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void LoadFromJson_MissingDetectOps_Throws()
    {
        var json = """
            {
                "name": "nodetect", "displayName": "No Detect", "version": "1.0.0", "author": "A",
                "tweaks": [
                    { "id": "nodetect-t1", "label": "T", "category": "C",
                        "applyOps": [{ "kind": "SetDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("detectOps", ex.Message);
    }

    [Fact]
    public void LoadFromJson_UnknownRegOpKind_Throws()
    {
        var json = """
            {
                "name": "unknownop", "displayName": "Unknown Op", "version": "1.0.0", "author": "A",
                "tweaks": [
                    { "id": "unknownop-t1", "label": "T", "category": "C",
                        "applyOps": [{ "kind": "SetMagic", "path": "HKEY_CURRENT_USER\\X", "name": "V" }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\X", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(json));
        Assert.Contains("Unknown RegOp kind", ex.Message);
    }

    [Fact]
    public void LoadFromJson_OversizedJson_Throws()
    {
        // Create a JSON string that exceeds 1 MB
        var huge = new string(' ', 1_048_577);
        var ex = Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson(huge));
        Assert.Contains("maximum size", ex.Message);
    }

    // ── PackLoader: ValidatePackJson ────────────────────────────────────

    [Fact]
    public void ValidatePackJson_ValidPack_ReturnsNoErrors()
    {
        var errors = PackLoader.ValidatePackJson(ValidPackJson);
        Assert.Empty(errors);
    }

    [Fact]
    public void ValidatePackJson_InvalidJson_ReturnsParseError()
    {
        var errors = PackLoader.ValidatePackJson("{ invalid json }}}");
        Assert.NotEmpty(errors);
        Assert.Contains(errors, e => e.Contains("parse", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void ValidatePackJson_MissingFields_ReturnsMultipleErrors()
    {
        var json = """{ "tweaks": [] }""";
        var errors = PackLoader.ValidatePackJson(json);
        Assert.True(errors.Count >= 3); // name, displayName, version, author, at least one tweak
    }

    // ── PackLoader: all 12 RegOp kinds ──────────────────────────────────

    [Theory]
    [InlineData("SetDword", RegOpKind.SetValue)]
    [InlineData("SetString", RegOpKind.SetValue)]
    [InlineData("SetExpandString", RegOpKind.SetValue)]
    [InlineData("SetQword", RegOpKind.SetValue)]
    [InlineData("SetBinary", RegOpKind.SetValue)]
    [InlineData("SetMultiSz", RegOpKind.SetValue)]
    [InlineData("DeleteValue", RegOpKind.DeleteValue)]
    [InlineData("DeleteTree", RegOpKind.DeleteTree)]
    [InlineData("CheckDword", RegOpKind.CheckValue)]
    [InlineData("CheckString", RegOpKind.CheckValue)]
    [InlineData("CheckMissing", RegOpKind.CheckMissing)]
    [InlineData("CheckKeyMissing", RegOpKind.CheckKeyMissing)]
    public void LoadFromJson_AllRegOpKinds_Supported(string kindName, RegOpKind expectedKind)
    {
        var extraFields = kindName switch
        {
            "SetDword" or "CheckDword" => ", \"dwordValue\": 1",
            "SetString" or "SetExpandString" or "CheckString" => ", \"stringValue\": \"v\"",
            "SetQword" => ", \"qwordValue\": 1",
            "SetBinary" => ", \"binaryValue\": \"AQID\"",
            "SetMultiSz" => ", \"multiSzValue\": [\"a\", \"b\"]",
            _ => "",
        };

        var json = $$"""
            {
                "name": "optest", "displayName": "Op Test", "version": "1.0.0", "author": "A",
                "tweaks": [
                    {
                        "id": "optest-t1", "label": "T", "category": "C",
                        "applyOps": [{ "kind": "{{kindName}}", "path": "HKEY_CURRENT_USER\\Software\\Test", "name": "V"{{extraFields}} }],
                        "detectOps": [{ "kind": "CheckDword", "path": "HKEY_CURRENT_USER\\Software\\Test", "name": "V", "dwordValue": 1 }]
                    }
                ]
            }
            """;

        var (_, tweaks) = PackLoader.LoadFromJson(json);
        Assert.Single(tweaks);
        Assert.Single(tweaks[0].ApplyOps);
        Assert.Equal(expectedKind, tweaks[0].ApplyOps[0].Kind);
    }

    // ── PackIndex tests ─────────────────────────────────────────────────

    [Fact]
    public void PackIndex_FromJson_RoundTrips()
    {
        var index = new PackIndex
        {
            Version = 1,
            LastUpdated = new DateTime(2025, 7, 22, 0, 0, 0, DateTimeKind.Utc),
            Packs =
            [
                new PackDef
                {
                    Name = "sample",
                    DisplayName = "Sample Pack",
                    Version = "1.0.0",
                    Author = "Test",
                    TweakCount = 5,
                },
            ],
        };

        var json = index.ToJson();
        var restored = PackIndex.FromJson(json);

        Assert.NotNull(restored);
        Assert.Equal(1, restored.Version);
        Assert.Single(restored.Packs);
        Assert.Equal("sample", restored.Packs[0].Name);
        Assert.Equal(5, restored.Packs[0].TweakCount);
    }

    [Fact]
    public void PackIndex_FromJson_EmptyIndex_Succeeds()
    {
        var json = """{ "version": 1, "packs": [] }""";
        var index = PackIndex.FromJson(json);

        Assert.NotNull(index);
        Assert.Equal(1, index.Version);
        Assert.Empty(index.Packs);
    }

    [Fact]
    public void PackIndex_FromJson_NullJson_ReturnsNull()
    {
        var result = PackIndex.FromJson("null");
        Assert.Null(result);
    }

    // ── PackManager: CompareVersions ────────────────────────────────────

    [Theory]
    [InlineData("1.0.0", "1.0.0", 0)]
    [InlineData("2.0.0", "1.0.0", 1)]
    [InlineData("1.0.0", "2.0.0", -1)]
    [InlineData("1.1.0", "1.0.0", 1)]
    [InlineData("1.0.1", "1.0.0", 1)]
    [InlineData("1.0.0", "1.0.1", -1)]
    [InlineData("1.0", "1.0.0", 0)]
    [InlineData("1", "1.0.0", 0)]
    [InlineData("10.0.0", "9.9.9", 1)]
    public void CompareVersions_ReturnsCorrectResult(string a, string b, int expected)
    {
        var result = PackManager.CompareVersions(a, b);
        Assert.Equal(expected, Math.Sign(result));
    }

    // ── TweakEngine: RegisterPack integration ───────────────────────────

    [Fact]
    public void RegisterPack_AddsPackTweaksToEngine()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));

        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);
        engine.RegisterPack(tweaks);

        Assert.Equal(2, engine.AllTweaks().Count);
        Assert.NotNull(engine.GetTweak("test-pack-disable-foo"));
        Assert.NotNull(engine.GetTweak("test-pack-enable-bar"));
    }

    [Fact]
    public void RegisterPack_PackTweaks_Searchable()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));

        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);
        engine.RegisterPack(tweaks);

        var results = engine.Search("Foo");
        Assert.Contains(results, t => t.Id == "test-pack-disable-foo");
    }

    [Fact]
    public void RegisterPack_PackTweaks_InCategories()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));

        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);
        engine.RegisterPack(tweaks);

        var categories = engine.Categories();
        Assert.Contains("TestCat", categories);
    }

    [Fact]
    public void RegisterPack_PackSourceSet()
    {
        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);

        Assert.All(tweaks, t => Assert.Equal("test-pack", t.PackSource));
    }

    [Fact]
    public void RegisterPack_WithBuiltins_NoConflict()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.RegisterBuiltins();
        var beforeCount = engine.AllTweaks().Count;

        var (_, tweaks) = PackLoader.LoadFromJson(ValidPackJson);
        engine.RegisterPack(tweaks);

        Assert.Equal(beforeCount + 2, engine.AllTweaks().Count);
    }

    // ── PackManager: local install/uninstall lifecycle ──────────────────

    [Fact]
    public void InstallFromFile_And_Uninstall_Lifecycle()
    {
        // Write a temp pack JSON file
        var tempDir = Path.Combine(Path.GetTempPath(), "RegiLattice_PluginTest_" + Guid.NewGuid().ToString("N")[..8]);
        Directory.CreateDirectory(tempDir);
        var tempFile = Path.Combine(tempDir, "test-pack.json");
        File.WriteAllText(tempFile, ValidPackJson);

        var mgr = new PackManager();
        try
        {
            // Install
            var (pack, tweaks) = mgr.InstallFromFile(tempFile);
            Assert.Equal("test-pack", pack.Name);
            Assert.Equal(2, tweaks.Count);

            // Verify listed
            var installed = mgr.InstalledPacks();
            Assert.Contains(installed, p => p.Name == "test-pack");

            // Verify can load tweaks
            var loaded = mgr.LoadInstalledPack("test-pack");
            Assert.NotNull(loaded);
            Assert.Equal(2, loaded.Count);

            // Uninstall
            var removed = mgr.UninstallPack("test-pack");
            Assert.True(removed);

            // Verify gone
            var afterUninstall = mgr.InstalledPacks();
            Assert.DoesNotContain(afterUninstall, p => p.Name == "test-pack");
        }
        finally
        {
            // Cleanup temp file
            try
            {
                Directory.Delete(tempDir, recursive: true);
            }
            catch { }
            // Cleanup installed pack (in case test failed before uninstall)
            try
            {
                mgr.UninstallPack("test-pack");
            }
            catch { }
        }
    }

    [Fact]
    public void UninstallPack_NonExistent_ReturnsFalse()
    {
        var mgr = new PackManager();
        Assert.False(mgr.UninstallPack("non-existent-pack-" + Guid.NewGuid().ToString("N")));
    }

    [Fact]
    public void LoadInstalledPack_NonExistent_ReturnsNull()
    {
        var mgr = new PackManager();
        Assert.Null(mgr.LoadInstalledPack("non-existent-pack-" + Guid.NewGuid().ToString("N")));
    }

    // ── Locale: German locale proof-of-concept ──────────────────────────

    [Fact]
    public void Locale_SetLocale_OverridesKeys()
    {
        var overrides = new Dictionary<string, string>
        {
            ["apply_all"] = "Alle anwenden",
            ["remove_all"] = "Alle entfernen",
            ["search_placeholder"] = "Optimierungen suchen\u2026",
        };

        Locale.SetLocale("de", overrides);

        Assert.Equal("de", Locale.CurrentLocale);
        Assert.Equal("Alle anwenden", Locale.T("apply_all"));
        Assert.Equal("Alle entfernen", Locale.T("remove_all"));
        Assert.Equal("Optimierungen suchen\u2026", Locale.T("search_placeholder"));

        // Non-overridden keys fall back to German built-in
        Assert.Equal("ANGEWENDET", Locale.T("status_applied"));

        // Reset to English
        Locale.SetLocale("en");
    }

    [Fact]
    public void Locale_BuiltInGerman_AllKeysTranslated()
    {
        Locale.SetLocale("de");

        // Spot-check key German translations
        Assert.Equal("Alle anwenden", Locale.T("apply_all"));
        Assert.Equal("Alle entfernen", Locale.T("remove_all"));
        Assert.Equal("ANGEWENDET", Locale.T("status_applied"));
        Assert.Equal("STANDARD", Locale.T("status_not_applied"));
        Assert.Equal("Datei", Locale.T("menu_file"));
        Assert.Equal("Werkzeuge", Locale.T("menu_tools"));
        Assert.Equal("Ansicht", Locale.T("menu_view"));
        Assert.Equal("Hilfe", Locale.T("menu_help"));
        Assert.Equal("Datenschutz", Locale.T("profile_privacy"));
        Assert.Equal("Administratorrechte erforderlich.", Locale.T("admin_required"));

        // Reset to English
        Locale.SetLocale("en");
    }

    [Fact]
    public void Locale_AvailableLocales_ContainsEnAndDe()
    {
        Assert.Contains("en", Locale.AvailableLocales);
        Assert.Contains("de", Locale.AvailableLocales);
    }

    [Fact]
    public void Locale_T_UnknownKey_ReturnsKey()
    {
        Locale.SetLocale("en");
        Assert.Equal("unknown_key_xyz", Locale.T("unknown_key_xyz"));
    }

    [Fact]
    public void Locale_T_FormatArgs_Applied()
    {
        Locale.SetLocale("en");
        var result = Locale.T("confirm_apply", 10);
        Assert.Equal("Apply 10 selected tweaks?", result);
    }

    [Fact]
    public void Locale_LoadLocaleFile_ParsesKeyValuePairs()
    {
        var tempFile = Path.Combine(Path.GetTempPath(), "regilattice-test-locale-" + Guid.NewGuid().ToString("N")[..8] + ".txt");
        try
        {
            File.WriteAllText(tempFile, "apply_all=Alles anwenden\nremove_all=Alles entfernen\n");
            Locale.LoadLocaleFile(tempFile);

            Assert.Equal("Alles anwenden", Locale.T("apply_all"));
            Assert.Equal("Alles entfernen", Locale.T("remove_all"));
        }
        finally
        {
            Locale.SetLocale("en");
            try
            {
                File.Delete(tempFile);
            }
            catch { }
        }
    }

    [Fact]
    public void Locale_LoadLocaleFile_NonExistent_NoOp()
    {
        // Should not throw — just silently returns
        Locale.LoadLocaleFile(@"C:\nonexistent\path\fake-locale.txt");
    }
}

// ── PackSignatureVerifier tests (T7.3) ──────────────────────────────────────

/// <summary>Tests for RSA-SHA256 pack signature verification (Sprint 131, T7.3).</summary>
public sealed class PackSignatureVerifierTests
{
    private const string SamplePackJson = """{"name":"test","version":"1.0.0","author":"A","tweaks":[]}""";

    [Fact]
    public void GenerateKeyPair_ReturnsNonEmptyPems()
    {
        var (pub, priv) = PackSignatureVerifier.GenerateKeyPair(2048);

        Assert.False(string.IsNullOrWhiteSpace(pub));
        Assert.False(string.IsNullOrWhiteSpace(priv));
        Assert.Contains("PUBLIC KEY", pub);
        Assert.Contains("PRIVATE KEY", priv);
    }

    [Fact]
    public void Sign_And_Verify_RoundTrip_ReturnsTrue()
    {
        var (pub, priv) = PackSignatureVerifier.GenerateKeyPair(2048);
        string sig = PackSignatureVerifier.Sign(SamplePackJson, priv);

        Assert.True(PackSignatureVerifier.Verify(SamplePackJson, sig, pub));
    }

    [Fact]
    public void Verify_TamperedContent_ReturnsFalse()
    {
        var (pub, priv) = PackSignatureVerifier.GenerateKeyPair(2048);
        string sig = PackSignatureVerifier.Sign(SamplePackJson, priv);

        Assert.False(PackSignatureVerifier.Verify(SamplePackJson + " ", sig, pub));
    }

    [Fact]
    public void Verify_DifferentKeyPair_ReturnsFalse()
    {
        var (_, priv1) = PackSignatureVerifier.GenerateKeyPair(2048);
        var (pub2, _) = PackSignatureVerifier.GenerateKeyPair(2048);
        string sig = PackSignatureVerifier.Sign(SamplePackJson, priv1);

        Assert.False(PackSignatureVerifier.Verify(SamplePackJson, sig, pub2));
    }

    [Fact]
    public void Verify_EmptySignature_ReturnsFalse()
    {
        var (pub, _) = PackSignatureVerifier.GenerateKeyPair(2048);
        Assert.False(PackSignatureVerifier.Verify(SamplePackJson, "", pub));
    }

    [Fact]
    public void Verify_InvalidBase64Signature_ReturnsFalse()
    {
        var (pub, _) = PackSignatureVerifier.GenerateKeyPair(2048);
        Assert.False(PackSignatureVerifier.Verify(SamplePackJson, "!!!notbase64!!!", pub));
    }

    [Fact]
    public void DetermineTrustLevel_WithValidSignature_ReturnsSigned()
    {
        var (pub, priv) = PackSignatureVerifier.GenerateKeyPair(2048);
        string sig = PackSignatureVerifier.Sign(SamplePackJson, priv);
        string hash = PackLoader.ComputeSha256(SamplePackJson);

        var pack = new PackDef
        {
            Name = "test",
            DisplayName = "Test",
            Version = "1.0.0",
            Author = "A",
            Sha256 = hash,
            SignatureUrl = "https://example.com/test.rlpack.sig",
        };

        var level = PackSignatureVerifier.DetermineTrustLevel(SamplePackJson, pack, sig, pub);

        Assert.Equal(PackTrustLevel.Signed, level);
    }

    [Fact]
    public void DetermineTrustLevel_HashOnlyNoSig_ReturnsHashVerified()
    {
        string hash = PackLoader.ComputeSha256(SamplePackJson);
        var pack = new PackDef
        {
            Name = "test",
            DisplayName = "Test",
            Version = "1.0.0",
            Author = "A",
            Sha256 = hash,
        };

        var level = PackSignatureVerifier.DetermineTrustLevel(SamplePackJson, pack, null, null);

        Assert.Equal(PackTrustLevel.HashVerified, level);
    }

    [Fact]
    public void DetermineTrustLevel_WrongHash_ReturnsNone()
    {
        var pack = new PackDef
        {
            Name = "test",
            DisplayName = "Test",
            Version = "1.0.0",
            Author = "A",
            Sha256 = "0000000000000000000000000000000000000000000000000000000000000000",
        };

        var level = PackSignatureVerifier.DetermineTrustLevel(SamplePackJson, pack, null, null);

        Assert.Equal(PackTrustLevel.None, level);
    }

    [Fact]
    public void DetermineTrustLevel_NoHashNoSig_ReturnsNone()
    {
        var pack = new PackDef
        {
            Name = "test",
            DisplayName = "Test",
            Version = "1.0.0",
            Author = "A",
        };

        var level = PackSignatureVerifier.DetermineTrustLevel(SamplePackJson, pack, null, null);

        Assert.Equal(PackTrustLevel.None, level);
    }

    [Fact]
    public void PackIndex_GetAuthorPublicKey_ReturnsMatchingKey()
    {
        var (pub, _) = PackSignatureVerifier.GenerateKeyPair(2048);
        var index = new PackIndex
        {
            Version = 1,
            Packs = [],
            AuthorKeys = [new AuthorKey { Author = "TestAuthor", PublicKeyPem = pub }],
        };

        string? found = index.GetAuthorPublicKey("TestAuthor");
        string? notFound = index.GetAuthorPublicKey("OtherAuthor");

        Assert.Equal(pub, found);
        Assert.Null(notFound);
    }

    [Fact]
    public void PackIndex_GetAuthorPublicKey_CaseInsensitive()
    {
        var (pub, _) = PackSignatureVerifier.GenerateKeyPair(2048);
        var index = new PackIndex
        {
            Version = 1,
            Packs = [],
            AuthorKeys = [new AuthorKey { Author = "TestAuthor", PublicKeyPem = pub }],
        };

        Assert.Equal(pub, index.GetAuthorPublicKey("testauthor"));
        Assert.Equal(pub, index.GetAuthorPublicKey("TESTAUTHOR"));
    }

    [Fact]
    public void PackDef_SignatureUrl_DefaultsToEmpty()
    {
        var pack = new PackDef
        {
            Name = "test",
            DisplayName = "Test",
            Version = "1.0.0",
            Author = "A",
        };

        Assert.Equal("", pack.SignatureUrl);
        Assert.Equal(PackTrustLevel.None, pack.TrustLevel);
    }
}

// ── PluginSandbox tests (T7.4) ───────────────────────────────────────────────

/// <summary>
/// Tests for <see cref="PluginSandbox"/>: DTO conversions, JSON protocol
/// serialization, and failure handling (Sprint 132–133).
/// </summary>
public sealed class PluginSandboxTests
{
    private static readonly System.Text.Json.JsonSerializerOptions s_json = new()
    {
        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase,
    };

    // ── ToDto unit tests ─────────────────────────────────────────────────────

    [Fact]
    public void ToDto_SetDword_MapsCorrectly()
    {
        var op = RegOp.SetDword(@"HKCU\Software\Test", "Value", 42);
        var dto = PluginSandbox.ToDto([op]);

        Assert.Single(dto);
        Assert.Equal("setdword", dto[0].Kind);
        Assert.Equal(@"HKCU\Software\Test", dto[0].Path);
        Assert.Equal("Value", dto[0].Name);
        Assert.Equal(42, dto[0].DwordValue);
    }

    [Fact]
    public void ToDto_SetString_MapsCorrectly()
    {
        var op = RegOp.SetString(@"HKCU\Software\Test", "Name", "hello");
        var dto = PluginSandbox.ToDto([op]);

        Assert.Single(dto);
        Assert.Equal("setstring", dto[0].Kind);
        Assert.Equal("hello", dto[0].StringValue);
    }

    [Fact]
    public void ToDto_SetBinary_EncodesBase64()
    {
        byte[] data = [0x01, 0x02, 0x03];
        var op = RegOp.SetBinary(@"HKCU\Software\Test", "Bin", data);
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("setbinary", dto[0].Kind);
        Assert.Equal(Convert.ToBase64String(data), dto[0].BinaryValue);
    }

    [Fact]
    public void ToDto_SetMultiSz_MapsArray()
    {
        var op = RegOp.SetMultiSz(@"HKCU\Software\Test", "Multi", ["a", "b", "c"]);
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("setmultisz", dto[0].Kind);
        Assert.NotNull(dto[0].MultiSzValue);
        Assert.Equal(["a", "b", "c"], dto[0].MultiSzValue!.ToArray());
    }

    [Fact]
    public void ToDto_SetQword_MapsCorrectly()
    {
        var op = RegOp.SetQword(@"HKCU\Software\Test", "BigVal", 123456789012345L);
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("setqword", dto[0].Kind);
        Assert.Equal(123456789012345L, dto[0].QwordValue);
    }

    [Fact]
    public void ToDto_SetExpandString_MapsCorrectly()
    {
        var op = RegOp.SetExpandString(@"HKCU\Software\Test", "Expand", @"%TEMP%\file.log");
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("setexpandstring", dto[0].Kind);
        Assert.Equal(@"%TEMP%\file.log", dto[0].StringValue);
    }

    [Fact]
    public void ToDto_DeleteValue_MapsCorrectly()
    {
        var op = RegOp.DeleteValue(@"HKCU\Software\Test", "OldValue");
        var dto = PluginSandbox.ToDto([op]);

        Assert.Single(dto);
        Assert.Equal("deletevalue", dto[0].Kind);
        Assert.Equal("OldValue", dto[0].Name);
    }

    [Fact]
    public void ToDto_DeleteTree_MapsCorrectly()
    {
        var op = RegOp.DeleteTree(@"HKCU\Software\OldApp");
        var dto = PluginSandbox.ToDto([op]);

        Assert.Single(dto);
        Assert.Equal("deletetree", dto[0].Kind);
        Assert.Equal(@"HKCU\Software\OldApp", dto[0].Path);
    }

    [Fact]
    public void ToDto_CheckDword_MapsCorrectly()
    {
        var op = RegOp.CheckDword(@"HKCU\Software\Test", "Flag", 1);
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("checkdword", dto[0].Kind);
        Assert.Equal(1, dto[0].DwordValue);
    }

    [Fact]
    public void ToDto_CheckMissing_MapsCorrectly()
    {
        var op = RegOp.CheckMissing(@"HKCU\Software\Test", "Ghost");
        var dto = PluginSandbox.ToDto([op]);

        Assert.Equal("checkmissing", dto[0].Kind);
        Assert.Equal("Ghost", dto[0].Name);
    }

    // ── FromDto round-trip ───────────────────────────────────────────────────

    [Fact]
    public void FromDto_AllRegularKinds_RoundTrip()
    {
        IReadOnlyList<RegOp> ops =
        [
            RegOp.SetDword(@"HKCU\Test", "D", 7),
            RegOp.SetString(@"HKCU\Test", "S", "val"),
            RegOp.SetExpandString(@"HKCU\Test", "E", @"%WINDIR%\file"),
            RegOp.SetQword(@"HKCU\Test", "Q", 999L),
            RegOp.SetBinary(@"HKCU\Test", "B", [0xAB, 0xCD]),
            RegOp.SetMultiSz(@"HKCU\Test", "M", ["x", "y"]),
            RegOp.DeleteValue(@"HKCU\Test", "Del"),
            RegOp.DeleteTree(@"HKCU\Test\Sub"),
            RegOp.CheckDword(@"HKCU\Test", "CD", 3),
            RegOp.CheckString(@"HKCU\Test", "CS", "expected"),
            RegOp.CheckMissing(@"HKCU\Test", "CM"),
            RegOp.CheckKeyMissing(@"HKCU\Test\Ghost"),
        ];

        var dtos = PluginSandbox.ToDto(ops);
        var restored = PluginSandbox.FromDto(dtos);

        Assert.Equal(ops.Count, restored.Count);
        for (int i = 0; i < ops.Count; i++)
        {
            Assert.Equal(ops[i].Path, restored[i].Path);
            Assert.Equal(ops[i].Name, restored[i].Name);
        }
    }

    // ── JSON protocol serialization tests ────────────────────────────────────

    [Fact]
    public void Request_SerializesAndDeserializes_RoundTrip()
    {
        var request = new PluginSandboxRequest { DryRun = true, Ops = PluginSandbox.ToDto([RegOp.SetDword(@"HKCU\Test", "V", 99)]) };

        string json = System.Text.Json.JsonSerializer.Serialize(request, s_json);
        var restored = System.Text.Json.JsonSerializer.Deserialize<PluginSandboxRequest>(json, s_json);

        Assert.NotNull(restored);
        Assert.True(restored!.DryRun);
        Assert.Single(restored.Ops);
        Assert.Equal("setdword", restored.Ops[0].Kind);
        Assert.Equal(99, restored.Ops[0].DwordValue);
    }

    [Fact]
    public void Response_SerializesAndDeserializes_RoundTrip()
    {
        var response = new PluginSandboxResponse { Success = true, ErrorMessage = "" };

        string json = System.Text.Json.JsonSerializer.Serialize(response, s_json);
        var restored = System.Text.Json.JsonSerializer.Deserialize<PluginSandboxResponse>(json, s_json);

        Assert.NotNull(restored);
        Assert.True(restored!.Success);
        Assert.Equal("", restored.ErrorMessage);
    }

    [Fact]
    public void Response_Error_SerializesAndDeserializes()
    {
        var response = new PluginSandboxResponse { Success = false, ErrorMessage = "Test error" };

        string json = System.Text.Json.JsonSerializer.Serialize(response, s_json);
        var restored = System.Text.Json.JsonSerializer.Deserialize<PluginSandboxResponse>(json, s_json);

        Assert.NotNull(restored);
        Assert.False(restored!.Success);
        Assert.Equal("Test error", restored.ErrorMessage);
    }

    // ── ExecuteAsync error-path test (no real process) ───────────────────────

    [Fact]
    public async Task ExecuteAsync_NonExistentExecutable_ReturnsFailed()
    {
        var ops = new[] { RegOp.SetDword(@"HKCU\Test", "V", 1) };
        var result = await PluginSandbox.ExecuteAsync(
            ops,
            dryRun: true,
            executablePath: @"C:\This\Does\Not\Exist\regilattice.exe",
            timeoutSeconds: 5
        );

        Assert.False(result.Success);
    }

    // ── PluginSandboxResult model tests ──────────────────────────────────────

    [Fact]
    public void PluginSandboxResult_DefaultValues()
    {
        var result = new PluginSandboxResult();

        Assert.False(result.Success);
        Assert.Equal("", result.ErrorMessage);
        Assert.False(result.TimedOut);
    }

    [Fact]
    public void PluginSandboxResult_TimedOut_SetCorrectly()
    {
        var result = new PluginSandboxResult
        {
            Success = false,
            TimedOut = true,
            ErrorMessage = "Timed out after 30 seconds.",
        };

        Assert.True(result.TimedOut);
        Assert.False(result.Success);
    }
}
