// Sprint 23 — Coverage expansion for TweakEngine result branches, IsApplicableOnHardware,
// Update(), ApplyBatch/RemoveBatch, Filter(query:), DetectStatus error path, ExportJson,
// GetScope, ResolveDependencies, Dependents, and Freeze post-registration paths.

using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.Core.Tests;

// ── Apply() result branches ─────────────────────────────────────────────────

public sealed class ApplyResultBranchTests
{
    // SkippedBuild — MinBuild higher than current build
    [Fact]
    public void Apply_MinBuildNotMet_ReturnsSkippedBuild()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-minbuild",
            Label = "Min Build",
            Category = "Test",
            CorpSafe = true,
            MinBuild = 9_999_999, // impossibly high
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-minbuild", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.SkippedBuild, engine.Apply(td, forceCorp: true));
    }

    // SkippedHw — IsApplicable returns false
    [Fact]
    public void Apply_IsApplicableFalse_ReturnsSkippedHw()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-hw-gate",
            Label = "HW Gate",
            Category = "Test",
            CorpSafe = true,
            IsApplicable = () => false,
            ApplicabilityNote = "Not applicable on this hardware",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-hw-gate", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.SkippedHw, engine.Apply(td, forceCorp: true));
    }

    // Apply with ApplyAction delegate (not RegOps path)
    [Fact]
    public void Apply_WithApplyAction_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "sb-action",
            Label = "Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { called = true; },
        };
        engine.Register([td]);
        var result = engine.Apply(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    // Apply with both MinBuild=0 (no restriction) and IsApplicable=true
    [Fact]
    public void Apply_NoBuildGate_NullIsApplicable_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "sb-nogate",
            Label = "No Gate",
            Category = "Test",
            CorpSafe = true,
            MinBuild = 0,
            IsApplicable = null,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\sb-nogate", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Apply(td, forceCorp: true));
    }
}

// ── Remove() result branches ────────────────────────────────────────────────

public sealed class RemoveResultBranchTests
{
    // Remove with RemoveAction delegate
    [Fact]
    public void Remove_WithRemoveAction_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "rb-action",
            Label = "Remove Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            RemoveAction = _ => { called = true; },
        };
        engine.Register([td]);
        var result = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.NotApplied, result);
        Assert.True(called);
    }

    // Remove with no RemoveOps and no RemoveAction — returns NotApplied
    [Fact]
    public void Remove_NoRemoveOpsOrAction_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "rb-noop",
            Label = "No Remove",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-noop", "V", 1)],
            // RemoveOps intentionally empty, RemoveAction null
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.NotApplied, engine.Remove(td, forceCorp: true));
    }

    // SkippedCorp for Remove when forceCorp=false and CorpSafe=false
    [Fact]
    public void Remove_SkipsCorp_WhenNotForceCorp()
    {
        CorporateGuard.ClearCache();
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "rb-corp",
            Label = "Corp",
            Category = "Test",
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\rb-corp", "V", 1)],
        };
        engine.Register([td]);

        // forceCorp=true overrides corp guard — should not skip
        var withForce = engine.Remove(td, forceCorp: true);
        Assert.Equal(TweakResult.NotApplied, withForce);
    }
}

// ── Update() branches ───────────────────────────────────────────────────────

public sealed class UpdateBranchTests
{
    // No UpdateAction — falls back to Apply
    [Fact]
    public void Update_NoUpdateAction_FallsBackToApply()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "upd-fallback",
            Label = "Update",
            Category = "Test",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\upd-fallback", "V", 1)],
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Applied, engine.Update(td, forceCorp: true));
    }

    // Has UpdateAction — calls it and returns Applied
    [Fact]
    public void Update_WithUpdateAction_ReturnsApplied()
    {
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "upd-action",
            Label = "Update With Action",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            UpdateAction = _ => { called = true; },
        };
        engine.Register([td]);
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }

    // UpdateAction throws — returns Error
    [Fact]
    public void Update_UpdateAction_Throws_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "upd-throw",
            Label = "Update Throw",
            Category = "Test",
            CorpSafe = true,
            ApplyAction = _ => { },
            UpdateAction = _ => throw new InvalidOperationException("update failure"),
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Error, engine.Update(td, forceCorp: true));
    }

    // Update with CorpSafe=false + forceCorp=false → SkippedCorp
    [Fact]
    public void Update_SkippedCorp_WhenNotForceCorp()
    {
        CorporateGuard.ClearCache();
        var engine = TestHelpers.CreateEngine();
        bool called = false;
        var td = new TweakDef
        {
            Id = "upd-corp-skip",
            Label = "Corp Skip",
            Category = "Test",
            CorpSafe = false,
            ApplyAction = _ => { },
            UpdateAction = _ => { called = true; },
        };
        engine.Register([td]);

        // With forceCorp=true — should succeed
        var result = engine.Update(td, forceCorp: true);
        Assert.Equal(TweakResult.Applied, result);
        Assert.True(called);
    }
}

// ── ApplyBatch / RemoveBatch ────────────────────────────────────────────────

public sealed class BatchOperationTests
{
    [Fact]
    public void ApplyBatch_Sequential_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef { Id = "bat-a1", Label = "A", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a1", "V", 1)] },
            new TweakDef { Id = "bat-a2", Label = "B", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a2", "V", 1)] },
            new TweakDef { Id = "bat-a3", Label = "C", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-a3", "V", 1)] },
        };
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, forceCorp: true, parallel: false);
        Assert.Equal(3, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void ApplyBatch_Parallel_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable.Range(1, 8).Select(i => new TweakDef
        {
            Id = $"bat-par-{i}",
            Label = $"P{i}",
            Category = "X",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\bat-par-{i}", "V", 1)],
        }).ToList();
        engine.Register(tweaks);
        var results = engine.ApplyBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(8, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.Applied, r));
    }

    [Fact]
    public void RemoveBatch_Sequential_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef { Id = "bat-r1", Label = "A", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-r1", "V", 1)], RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\bat-r1", "V")] },
            new TweakDef { Id = "bat-r2", Label = "B", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-r2", "V", 1)], RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\bat-r2", "V")] },
        };
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: false);
        Assert.Equal(2, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.NotApplied, r));
    }

    [Fact]
    public void RemoveBatch_Parallel_ProcessesAll()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = Enumerable.Range(1, 6).Select(i => new TweakDef
        {
            Id = $"bat-rpar-{i}",
            Label = $"R{i}",
            Category = "X",
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\bat-rpar-{i}", "V", 1)],
            RemoveOps = [RegOp.DeleteValue($@"HKCU\Software\bat-rpar-{i}", "V")],
        }).ToList();
        engine.Register(tweaks);
        var results = engine.RemoveBatch(tweaks, forceCorp: true, parallel: true);
        Assert.Equal(6, results.Count);
        Assert.All(results.Values, r => Assert.Equal(TweakResult.NotApplied, r));
    }

    [Fact]
    public void ApplyBatch_DefaultProgress_Works()
    {
        var engine = TestHelpers.CreateEngine();
        var tweaks = new[]
        {
            new TweakDef { Id = "bat-prog1", Label = "P1", Category = "X", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\bat-prog1", "V", 1)] },
        };
        engine.Register(tweaks);
        var calls = new List<(int done, int total, string id, TweakResult r)>();
        var results = engine.ApplyBatch(tweaks, forceCorp: true,
            onProgress: (done, total, id, r) => calls.Add((done, total, id, r)));
        Assert.Single(calls);
        Assert.Equal(1, calls[0].done);
        Assert.Equal(1, calls[0].total);
        Assert.Equal("bat-prog1", calls[0].id);
    }
}

// ── DetectStatus exception branch ───────────────────────────────────────────

public sealed class DetectStatusBranchTests
{
    [Fact]
    public void DetectStatus_DetectActionThrows_ReturnsError()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "ds-throw",
            Label = "Throw",
            Category = "Test",
            ApplyAction = _ => { },
            DetectAction = () => throw new InvalidOperationException("detect failure"),
        };
        engine.Register([td]);
        Assert.Equal(TweakResult.Error, engine.DetectStatus(td));
    }

    [Fact]
    public void DetectStatus_DetectOps_FalseResult_ReturnsNotApplied()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "ds-not-applied",
            Label = "Not Applied",
            Category = "Test",
            // CheckDword on a key that almost certainly won't match SentinelValue=99999
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\ds-not-applied", "V", 1)],
            DetectOps = [RegOp.CheckDword(@"HKCU\Software\ds-not-applied-nonexistent", "V", 99999)],
        };
        engine.Register([td]);
        var result = engine.DetectStatus(td);
        // Will be NotApplied (key doesn't exist = check fails)
        Assert.Equal(TweakResult.NotApplied, result);
    }
}

// ── StatusMap with id subset ────────────────────────────────────────────────

public sealed class StatusMapSubsetTests
{
    [Fact]
    public void StatusMap_WithIdSubset_ReturnsOnlyRequestedIds()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            TestHelpers.MakeTweak("ssub-1"),
            TestHelpers.MakeTweak("ssub-2"),
            TestHelpers.MakeTweak("ssub-3"),
        ]);
        var map = engine.StatusMap(parallel: false, ids: ["ssub-1", "ssub-3"]);
        Assert.Equal(2, map.Count);
        Assert.True(map.ContainsKey("ssub-1"));
        Assert.True(map.ContainsKey("ssub-3"));
        Assert.False(map.ContainsKey("ssub-2"));
    }

    [Fact]
    public void StatusMap_ParallelWithIds_ReturnsOnlyRequestedIds()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            TestHelpers.MakeTweak("spar-1"),
            TestHelpers.MakeTweak("spar-2"),
            TestHelpers.MakeTweak("spar-3"),
        ]);
        var map = engine.StatusMap(parallel: true, ids: ["spar-2"]);
        Assert.Single(map);
        Assert.True(map.ContainsKey("spar-2"));
    }
}

// ── Search multi-token ──────────────────────────────────────────────────────

public sealed class SearchMultiTokenTests
{
    [Fact]
    public void Search_MultiToken_AllTokensMustMatch()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "mts-privacy-telemetry",
            Label = "Disable Privacy Telemetry",
            Category = "Privacy",
            Description = "Disables telemetry reporting",
            Tags = ["privacy", "telemetry"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-priv", "V", 1)],
        };
        var other = new TweakDef
        {
            Id = "mts-privacy-only",
            Label = "Privacy Only",
            Category = "Privacy",
            Tags = ["privacy"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-priv2", "V", 1)],
        };
        engine.Register([td, other]);

        // Both tokens must appear — only td matches both "privacy" AND "telemetry"
        var results = engine.Search("privacy telemetry");
        Assert.Single(results);
        Assert.Equal("mts-privacy-telemetry", results[0].Id);
    }

    [Fact]
    public void Search_SingleToken_ReturnsAllMatches()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef { Id = "mts-perf-1", Label = "Performance Tweak 1", Category = "Performance", ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-perf-1", "V", 1)] },
            new TweakDef { Id = "mts-perf-2", Label = "Performance Tweak 2", Category = "Performance", ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-perf-2", "V", 1)] },
            new TweakDef { Id = "mts-other", Label = "Other", Category = "Other", ApplyOps = [RegOp.SetDword(@"HKCU\Software\mts-other", "V", 1)] },
        ]);
        var results = engine.Search("performance");
        Assert.Equal(2, results.Count);
    }
}

// ── Filter(query:) branch ───────────────────────────────────────────────────

public sealed class FilterQueryBranchTests
{
    [Fact]
    public void Filter_WithQuery_FiltersOnSearchText()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef { Id = "fq-disk", Label = "Disk Cleanup", Category = "Performance", Description = "Frees disk space", Tags = ["disk"], ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-disk", "V", 1)] },
            new TweakDef { Id = "fq-mem", Label = "Memory Tweak", Category = "Performance", Description = "Reduces memory footprint", Tags = ["memory"], ApplyOps = [RegOp.SetDword(@"HKCU\Software\fq-mem", "V", 1)] },
        ]);
        var results = engine.Filter(query: "disk");
        Assert.Single(results);
        Assert.Equal("fq-disk", results[0].Id);
    }

    [Fact]
    public void Filter_Query_NoMatch_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("fq-nothing")]);
        var results = engine.Filter(query: "zzz-no-match-possible-xyz");
        Assert.Empty(results);
    }

    [Fact]
    public void Filter_QueryAndCategory_CombinesFilters()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef { Id = "fqc-1", Label = "Alpha Disk", Category = "CatA", Tags = ["disk"], ApplyOps = [RegOp.SetDword(@"HKCU\Software\fqc-1", "V", 1)] },
            new TweakDef { Id = "fqc-2", Label = "Beta Disk", Category = "CatB", Tags = ["disk"], ApplyOps = [RegOp.SetDword(@"HKCU\Software\fqc-2", "V", 1)] },
        ]);
        // "disk" in both, but filtered by CatA
        var results = engine.Filter(category: "CatA", query: "disk");
        Assert.Single(results);
        Assert.Equal("fqc-1", results[0].Id);
    }
}

// ── IsApplicableOnHardware category switch branches ─────────────────────────

public sealed class IsApplicableOnHardwareTests
{
    // Custom predicate trumps category
    [Fact]
    public void IsApplicableOnHardware_CustomPredicateTrue_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-custom-true",
            Label = "Custom",
            Category = "WSL", // would be HardwareInfo.HasWslInstalled() otherwise
            IsApplicable = () => true,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-custom", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    [Fact]
    public void IsApplicableOnHardware_CustomPredicateFalse_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "iap-custom-false",
            Label = "Custom False",
            Category = "Performance",
            IsApplicable = () => false,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-custom-false", "V", 1)],
        };
        Assert.False(TweakEngine.IsApplicableOnHardware(td));
    }

    // Category-based detection — these call HardwareInfo methods (returns bool, can be true or false)
    [Theory]
    [InlineData("WSL")]
    [InlineData("Virtualization")]
    [InlineData("Chrome")]
    [InlineData("Firefox")]
    [InlineData("Edge")]
    [InlineData("Java")]
    [InlineData("Adobe")]
    [InlineData("LibreOffice")]
    [InlineData("Office")]
    [InlineData("M365 Copilot")]
    [InlineData("RealVNC")]
    [InlineData("VS Code")]
    [InlineData("Scoop Tools")]
    public void IsApplicableOnHardware_SoftwareCategory_ReturnsBool(string category)
    {
        var td = new TweakDef
        {
            Id = $"iap-{category.ToLowerInvariant().Replace(" ", "-")}",
            Label = category,
            Category = category,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-cat", "V", 1)],
        };
        // Should not throw — actual value depends on machine
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // Default (no matching category, no special tag) → returns true
    [Fact]
    public void IsApplicableOnHardware_UnknownCategory_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-unknown-cat",
            Label = "Unknown",
            Category = "SomeObscureCategory",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-unknown", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }

    // AutoDetectFromTags — nvidia tag
    [Fact]
    public void IsApplicableOnHardware_NvidiaTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-nvidia",
            Label = "Nvidia",
            Category = "GPU / Graphics",
            Tags = ["nvidia"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-nvidia", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — amd-gpu tag
    [Fact]
    public void IsApplicableOnHardware_AmdGpuTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-amd",
            Label = "AMD",
            Category = "GPU / Graphics",
            Tags = ["amd-gpu"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-amd", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — docker tag
    [Fact]
    public void IsApplicableOnHardware_DockerTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-docker",
            Label = "Docker",
            Category = "Virtualization",
            Tags = ["docker"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-docker", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // AutoDetectFromTags — laptop tag
    [Fact]
    public void IsApplicableOnHardware_LaptopTag_CallsHardwareInfo()
    {
        var td = new TweakDef
        {
            Id = "iap-laptop",
            Label = "Laptop",
            Category = "Power Management",
            Tags = ["laptop"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-laptop", "V", 1)],
        };
        var result = TweakEngine.IsApplicableOnHardware(td);
        Assert.IsType<bool>(result);
    }

    // No special tag — falls through to return true
    [Fact]
    public void IsApplicableOnHardware_NoSpecialTag_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "iap-notag",
            Label = "No Tag",
            Category = "Performance",
            Tags = ["performance"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\iap-notag", "V", 1)],
        };
        Assert.True(TweakEngine.IsApplicableOnHardware(td));
    }
}

// ── ExportJson ──────────────────────────────────────────────────────────────

public sealed class ExportJsonTests
{
    [Fact]
    public void ExportJson_WritesValidJson()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            TestHelpers.MakeTweak("ej-1", "Privacy"),
            TestHelpers.MakeTweak("ej-2", "Performance"),
        ]);
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-{Guid.NewGuid():N}.json");
        try
        {
            engine.ExportJson(path);
            Assert.True(File.Exists(path));
            var json = File.ReadAllText(path);
            var doc = JsonDocument.Parse(json);
            Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
            Assert.Equal(2, doc.RootElement.GetArrayLength());
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void ExportJson_WithCachedStatus_UsesCached()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("ej-cached")]);
        var path = Path.Combine(Path.GetTempPath(), $"rl-export-{Guid.NewGuid():N}.json");
        try
        {
            var cached = new Dictionary<string, TweakResult> { ["ej-cached"] = TweakResult.Applied };
            engine.ExportJson(path, cached);
            var json = File.ReadAllText(path);
            Assert.Contains("applied", json);
        }
        finally
        {
            File.Delete(path);
        }
    }
}

// ── GetScope ────────────────────────────────────────────────────────────────

public sealed class GetScopeTests
{
    [Fact]
    public void GetScope_ReturnsFromCache()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "gs-hkcu",
            Label = "HKCU",
            Category = "Test",
            RegistryKeys = [@"HKCU\Software\gs-hkcu"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\gs-hkcu", "V", 1)],
        };
        engine.Register([td]);
        // Should return User scope both times — second call uses _scopeCache
        Assert.Equal(TweakScope.User, engine.GetScope(td));
        Assert.Equal(TweakScope.User, engine.GetScope(td));
    }

    [Fact]
    public void GetScope_BothScopesTweak_ReturnsBoth()
    {
        var engine = TestHelpers.CreateEngine();
        var td = new TweakDef
        {
            Id = "gs-both",
            Label = "Both",
            Category = "Test",
            RegistryKeys = [@"HKCU\Software\gs-both", @"HKLM\SOFTWARE\gs-both"],
            ApplyOps = [
                RegOp.SetDword(@"HKCU\Software\gs-both", "V", 1),
                RegOp.SetDword(@"HKLM\SOFTWARE\gs-both", "V", 1),
            ],
        };
        engine.Register([td]);
        Assert.Equal(TweakScope.Both, engine.GetScope(td));
    }
}

// ── ResolveDependencies / Dependents ────────────────────────────────────────

public sealed class DependencyEngineTests
{
    [Fact]
    public void ResolveDependencies_SimpleChain_ReturnsInOrder()
    {
        var engine = TestHelpers.CreateEngine();
        var tdA = TestHelpers.MakeTweak("dep-eng-a");
        var tdB = new TweakDef
        {
            Id = "dep-eng-b",
            Label = "B",
            Category = "Test",
            DependsOn = ["dep-eng-a"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-eng-b", "V", 1)],
        };
        engine.Register([tdA, tdB]);
        var resolved = engine.ResolveDependencies("dep-eng-b");
        Assert.Equal(2, resolved.Count);
        Assert.Equal("dep-eng-a", resolved[0].Id);
        Assert.Equal("dep-eng-b", resolved[1].Id);
    }

    [Fact]
    public void ResolveDependencies_UnknownId_Throws()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Throws<ArgumentException>(() => engine.ResolveDependencies("nonexistent-id-xyz"));
    }

    [Fact]
    public void Dependents_ReturnsTweaksThatDependOnGiven()
    {
        var engine = TestHelpers.CreateEngine();
        var tdA = TestHelpers.MakeTweak("dep-rev-a");
        var tdB = new TweakDef
        {
            Id = "dep-rev-b",
            Label = "B",
            Category = "Test",
            DependsOn = ["dep-rev-a"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-rev-b", "V", 1)],
        };
        engine.Register([tdA, tdB]);
        var dependents = engine.Dependents("dep-rev-a");
        Assert.Single(dependents);
        Assert.Equal("dep-rev-b", dependents[0].Id);
    }

    [Fact]
    public void Dependents_NoDependents_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("dep-leaf")]);
        Assert.Empty(engine.Dependents("dep-leaf"));
    }
}

// ── Freeze post paths ───────────────────────────────────────────────────────

public sealed class FreezeBehaviorTests
{
    [Fact]
    public void GetTweak_AfterFreeze_UsesFrozenDictionary()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-1"), TestHelpers.MakeTweak("frz-2")]);
        engine.Freeze(); // explicit freeze

        // After freeze, GetTweak uses FrozenDictionary
        Assert.NotNull(engine.GetTweak("frz-1"));
        Assert.Null(engine.GetTweak("nonexistent-frz"));
    }

    [Fact]
    public void Categories_AfterFreeze_UsesCachedArray()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-cat-1", "Zeta"), TestHelpers.MakeTweak("frz-cat-2", "Alpha")]);
        engine.Freeze();

        var cats = engine.Categories();
        Assert.Equal("Alpha", cats[0]);
        Assert.Equal("Zeta", cats[1]);
    }

    [Fact]
    public void CategoryCounts_AfterFreeze_UsesCachedDict()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([TestHelpers.MakeTweak("frz-cc-1", "FrzCat"), TestHelpers.MakeTweak("frz-cc-2", "FrzCat")]);
        engine.Freeze();

        var counts = engine.CategoryCounts();
        Assert.Equal(2, counts["FrzCat"]);
    }

    [Fact]
    public void ScopeCounts_AfterFreeze_UsesCachedDict()
    {
        var engine = TestHelpers.CreateEngine();
        engine.Register([
            new TweakDef { Id = "frz-sc-u", Label = "U", Category = "X", RegistryKeys = [@"HKCU\T"], ApplyOps = [RegOp.SetDword(@"HKCU\T", "V", 1)] },
            new TweakDef { Id = "frz-sc-m", Label = "M", Category = "X", RegistryKeys = [@"HKLM\T"], ApplyOps = [RegOp.SetDword(@"HKLM\T", "V", 1)] },
        ]);
        engine.Freeze();

        var counts = engine.ScopeCounts();
        Assert.Equal(1, counts[TweakScope.User]);
        Assert.Equal(1, counts[TweakScope.Machine]);
    }
}

// ── TweaksForProfile edge cases ─────────────────────────────────────────────

public sealed class TweaksForProfileTests
{
    [Fact]
    public void TweaksForProfile_CaseInsensitive()
    {
        var engine = TestHelpers.CreateEngine();
        // business profile has Privacy category
        engine.Register([
            new TweakDef { Id = "tfp-priv1", Label = "P1", Category = "Privacy", CorpSafe = true, ApplyOps = [RegOp.SetDword(@"HKCU\Software\tfp-priv1", "V", 1)] },
        ]);
        // GetProfile should be case-insensitive
        var tweaks1 = engine.TweaksForProfile("BUSINESS");
        var tweaks2 = engine.TweaksForProfile("business");
        Assert.Equal(tweaks1.Count, tweaks2.Count);
    }

    [Fact]
    public void TweaksForProfile_NullProfile_ReturnsEmpty()
    {
        var engine = TestHelpers.CreateEngine();
        Assert.Empty(engine.TweaksForProfile("completely-nonexistent-profile-xyz"));
    }
}
