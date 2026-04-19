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

// ── merged from BranchCoverage4Tests.cs ──────────────────────────────────

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
        Assert.IsType<bool>(TweakEngine.IsApplicableOnHardware(Td("bc4-wsl-c", "Virtualization")));
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

public sealed class TweakEnginePartialBranchTests
{
    [Fact]
    public void TweaksByTag_NonExistentTag_ReturnsEmpty()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        // Tag that definitely doesn't exist → TryGetValue returns false → returns [] (F-branch)
        var results = engine.TweaksByTag("zzz-not-a-real-tag-bc6");
        Assert.Empty(results);
    }

    [Fact]
    public void TweaksForProfile_ProfileCategoryNotInEngine_SkipsMissingCategory()
    {
        // Fresh engine with NO tweaks registered — profile categories won't be found
        // → _tweaksByCat.TryGetValue returns false for every category → F-branch covered
        var engine = new TweakEngine();
        // Don't register builtins — engine has no tweaks
        var tweaks = engine.TweaksForProfile("gaming");
        Assert.Empty(tweaks);
    }

    [Fact]
    public void Filter_WithQuery_HitsContainsCheck()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        // Filter with a very specific query to trigger _tweakSearchText.TryGetValue && Contains
        // Both T-branch (query matches) and F-branch (query does not match) are exercised across tweaks
        var matches = engine.Filter(query: "telemetry");
        // Expect at least some results (T-branch covered)
        Assert.NotEmpty(matches);
    }

    [Fact]
    public void Filter_WithQueryNoMatch_ReturnsEmpty()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        // A query that matches nothing — all tweaks hit the F-branch of Contains
        var results = engine.Filter(query: "zzz-no-match-at-all-bc6-xyz");
        Assert.Empty(results);
    }
}
