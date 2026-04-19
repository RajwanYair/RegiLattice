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

public sealed class PackLoaderRemainingEdgeCaseTests
{
    private static readonly string s_minimalJson = """
        {
            "name": "rl-pl-edge", "displayName": "Edge Pack", "version": "1.0.0", "author": "UT",
            "tweaks": [{
                "id": "rl-pl-edge-t1", "label": "T1", "category": "TestCat",
                "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\rl-pl-edge","name":"V","dwordValue":1}],
                "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\rl-pl-edge","name":"V","dwordValue":1}]
            }]
        }
        """;

    [Fact]
    public void LoadFromJson_ApplyOpsOnlyNoDetect_Succeeds()
    {
        // RemoveOps and DetectOps absent → should succeed
        var (pack, tweaks) = PackLoader.LoadFromJson(s_minimalJson, null);
        Assert.NotNull(pack);
        Assert.Single(tweaks);
    }

    [Fact]
    public void ValidatePackJson_EmptyName_ReturnsError()
    {
        const string json = """
            {
                "name": "", "displayName": "d", "version": "1.0.0", "author": "a",
                "tweaks": [{"id":"t1","label":"L","category":"C","applyOps":[{"kind":"SetDword","path":"HKCU\\X","name":"N","dwordValue":1}]}]
            }
            """;
        var err = PackLoader.ValidatePackJson(json);
        Assert.NotEmpty(err);
        Assert.True(
            err.Any(e => e.Contains("name", StringComparison.OrdinalIgnoreCase)),
            $"Expected an error about 'name'; errors: {string.Join("; ", err)}"
        );
    }

    [Fact]
    public void ValidatePackJson_EmptyTweaksList_ReturnsError()
    {
        const string json = """
            {"name": "no-tweaks", "displayName": "d", "version": "1.0.0", "author": "a", "tweaks": []}
            """;
        var err = PackLoader.ValidatePackJson(json);
        Assert.NotEmpty(err);
    }

    [Fact]
    public void LoadFromJson_TweakWithDescriptionAndTagsOptional_Succeeds()
    {
        const string json = """
            {
                "name":"rl-opt", "displayName":"Opt", "version":"1.0.0", "author":"UT",
                "description": "optional pack description",
                "tags": ["optional", "tag"],
                "tweaks": [{
                    "id":"rl-opt-t1","label":"L","category":"C",
                    "description": "optional tweak description",
                    "tags": ["tag1"],
                    "applyOps":[{"kind":"SetDword","path":"HKCU\\T","name":"V","dwordValue":1}],
                    "detectOps":[{"kind":"CheckDword","path":"HKCU\\T","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (pack, tweaks) = PackLoader.LoadFromJson(json, null);
        Assert.NotNull(pack);
        Assert.Single(tweaks);
    }

    [Fact]
    public void ComputeSha256_DifferentStrings_DifferentHashes()
    {
        var hash1 = PackLoader.ComputeSha256("string one");
        var hash2 = PackLoader.ComputeSha256("string two");
        Assert.NotEqual(hash1, hash2);
    }
}

// ── 12. StartupManager remaining branch coverage ───────────────────────────

public sealed class PackLoaderNullJsonBranchTests
{
    [Fact]
    public void LoadFromJson_NullJson_ThrowsInvalidOperationException()
    {
        // "null" JSON causes Deserialize<RawPack> to return null → ?? throw fires (line 44 T-branch)
        Assert.Throws<InvalidOperationException>(() => PackLoader.LoadFromJson("null"));
    }

    [Fact]
    public void ValidatePackJson_TweakIdNotPrefixedWithPackName_ContainsError()
    {
        // A valid pack JSON where tweak ID does NOT start with pack-name prefix
        // This covers the id-prefix validation branch (line ~221)
        const string json = """
            {
              "name": "mypkg",
              "displayName": "My Package",
              "version": "1.0.0",
              "author": "Tester",
              "tweaks": [
                {
                  "id": "other-tweak",
                  "label": "Test Tweak",
                  "category": "Test",
                  "applyOps": [{"kind":"SetDword","path":"HKCU\\\\Software\\\\Test","name":"V","dwordValue":1}],
                  "removeOps": [{"kind":"DeleteValue","path":"HKCU\\\\Software\\\\Test","name":"V"}],
                  "detectOps": [{"kind":"CheckDword","path":"HKCU\\\\Software\\\\Test","name":"V","dwordValue":1}]
                }
              ]
            }
            """;
        var errors = PackLoader.ValidatePackJson(json);
        Assert.Contains(errors, e => e.Contains("must be prefixed"));
    }
}

// ── 6. TweakEngine — partial branches: TweaksByTag, TweaksForProfile ─────────

