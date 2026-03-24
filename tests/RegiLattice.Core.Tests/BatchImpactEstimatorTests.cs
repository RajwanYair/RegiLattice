// RegiLattice.Core.Tests — BatchImpactEstimatorTests.cs
// Unit tests for BatchImpactEstimator and its BatchImpactSummary record.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for BatchImpactEstimator: estimation, dependency expansion, and tier classification.</summary>
public sealed class BatchImpactEstimatorTests
{
    // ── Helpers ───────────────────────────────────────────────────────────

    private static TweakDef MakeTweak(
        string id,
        int impact = 3,
        int safety = 4,
        string? impactNote = null,
        string category = "Test",
        IReadOnlyList<string>? dependsOn = null
    ) =>
        new()
        {
            Id = id,
            Label = $"Tweak {id}",
            Category = category,
            ImpactScore = impact,
            SafetyRating = safety,
            ImpactNote = impactNote ?? "",
            DependsOn = dependsOn ?? [],
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\{id}", "V", 1)],
        };

    // ── Empty collection ──────────────────────────────────────────────────

    [Fact]
    public void Estimate_EmptyCollection_ReturnsZeroSummary()
    {
        var result = BatchImpactEstimator.Estimate([]);

        Assert.Equal(0, result.TweakCount);
        Assert.Equal(0, result.QuickWinCount);
        Assert.Equal(0, result.AverageImpact);
        Assert.Equal(ImpactTier.Minimal, result.OverallTier);
        Assert.Empty(result.TopBenefits);
    }

    // ── Single tweak ──────────────────────────────────────────────────────

    [Fact]
    public void Estimate_SingleTweak_TweakCountIsOne()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("single-1", impact: 4, safety: 4)]);

        Assert.Equal(1, result.TweakCount);
        Assert.Equal(1, result.QuickWinCount);
    }

    [Fact]
    public void Estimate_SingleTweak_AverageImpactMatchesScore()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("single-2", impact: 5)]);

        Assert.Equal(5.0, result.AverageImpact);
    }

    // ── Deduplication by ID ───────────────────────────────────────────────

    [Fact]
    public void Estimate_DuplicateIds_CountedOnce()
    {
        var td = MakeTweak("dup-1", impact: 5);
        var result = BatchImpactEstimator.Estimate([td, td]);

        Assert.Equal(1, result.TweakCount);
    }

    // ── Quick win counting ────────────────────────────────────────────────

    [Fact]
    public void Estimate_TweakBelowImpactThreshold_NotQuickWin()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("qw-miss-imp", impact: 3, safety: 5)]);

        Assert.Equal(0, result.QuickWinCount);
    }

    [Fact]
    public void Estimate_TweakBelowSafetyThreshold_NotQuickWin()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("qw-miss-saf", impact: 5, safety: 3)]);

        Assert.Equal(0, result.QuickWinCount);
    }

    [Fact]
    public void Estimate_BothThresholdsMet_CountedAsQuickWin()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("qw-hit", impact: 4, safety: 4)]);

        Assert.Equal(1, result.QuickWinCount);
    }

    // ── Impact tier thresholds ────────────────────────────────────────────

    [Theory]
    [InlineData(1, ImpactTier.Minimal)]
    [InlineData(2, ImpactTier.Low)]
    [InlineData(3, ImpactTier.Moderate)]
    [InlineData(4, ImpactTier.High)]
    [InlineData(5, ImpactTier.Significant)]
    public void Estimate_SingleTweak_TierMatchesScore(int impact, ImpactTier expectedTier)
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak($"tier-{impact}", impact: impact)]);

        Assert.Equal(expectedTier, result.OverallTier);
    }

    // ── Average computation ───────────────────────────────────────────────

    [Fact]
    public void Estimate_MultipleTweaks_AverageIsCorrect()
    {
        var tweaks = new[] { MakeTweak("avg-1", impact: 2), MakeTweak("avg-2", impact: 4) };

        var result = BatchImpactEstimator.Estimate(tweaks);

        Assert.Equal(3.0, result.AverageImpact);
    }

    // ── TopBenefits from ImpactNote ───────────────────────────────────────

    [Fact]
    public void Estimate_TweakWithImpactNote_AppearsInTopBenefits()
    {
        var td = MakeTweak("note-1", impact: 5, impactNote: "Reduces boot time by 3s.");
        var result = BatchImpactEstimator.Estimate([td]);

        Assert.Contains("Reduces boot time by 3s.", result.TopBenefits);
    }

    [Fact]
    public void Estimate_TweakWithEmptyNote_NotInTopBenefits()
    {
        var td = MakeTweak("note-2", impact: 5, impactNote: "");
        var result = BatchImpactEstimator.Estimate([td]);

        Assert.Empty(result.TopBenefits);
    }

    [Fact]
    public void Estimate_TopBenefits_OrderedByImpactDescending()
    {
        var tweaks = new[]
        {
            MakeTweak("nb-low", impact: 2, impactNote: "Minor improvement."),
            MakeTweak("nb-high", impact: 5, impactNote: "Significant improvement."),
            MakeTweak("nb-mid", impact: 3, impactNote: "Moderate improvement."),
        };

        var result = BatchImpactEstimator.Estimate(tweaks);

        Assert.Equal("Significant improvement.", result.TopBenefits[0]);
    }

    [Fact]
    public void Estimate_ManyNotes_TopBenefitsAtMostFive()
    {
        var tweaks = Enumerable.Range(1, 10).Select(i => MakeTweak($"nb-many-{i}", impact: 5, impactNote: $"Benefit {i}.")).ToArray();

        var result = BatchImpactEstimator.Estimate(tweaks);

        Assert.True(result.TopBenefits.Count <= 5);
    }

    // ── Category breakdown ────────────────────────────────────────────────

    [Fact]
    public void Estimate_TweaksAcrossCategories_ByCategoryIsCorrect()
    {
        var tweaks = new[]
        {
            MakeTweak("cat-perf-1", category: "Performance"),
            MakeTweak("cat-perf-2", category: "Performance"),
            MakeTweak("cat-priv-1", category: "Privacy"),
        };

        var result = BatchImpactEstimator.Estimate(tweaks);

        Assert.Equal(2, result.ByCategory["Performance"]);
        Assert.Equal(1, result.ByCategory["Privacy"]);
    }

    // ── UniqueRegistryOps ─────────────────────────────────────────────────

    [Fact]
    public void Estimate_TweakWithApplyOps_CountsRegistryOps()
    {
        var td = MakeTweak("reg-ops-1");
        var result = BatchImpactEstimator.Estimate([td]);

        Assert.Equal(1, result.UniqueRegistryOps);
    }

    [Fact]
    public void Estimate_DuplicateRegistryOps_CountedOnce()
    {
        // Two tweaks writing to the same key+name.
        var t1 = new TweakDef
        {
            Id = "dup-reg-1",
            Label = "Dup reg 1",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Dup", "V", 1)],
        };
        var t2 = new TweakDef
        {
            Id = "dup-reg-2",
            Label = "Dup reg 2",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Dup", "V", 0)],
        };

        var result = BatchImpactEstimator.Estimate([t1, t2]);

        Assert.Equal(1, result.UniqueRegistryOps);
    }

    // ── Dependency expansion ──────────────────────────────────────────────

    [Fact]
    public void Estimate_WithEngine_ExpandsDependsOn()
    {
        var dep = MakeTweak("dep-base", impact: 5, impactNote: "Base tweak.");
        var root = new TweakDef
        {
            Id = "dep-root",
            Label = "Root tweak",
            Category = "Test",
            DependsOn = ["dep-base"],
            ImpactScore = 3,
            SafetyRating = 4,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\dep-root", "V", 1)],
        };

        var engine = new TweakEngine();
        engine.Register([dep, root]);

        var result = BatchImpactEstimator.Estimate([root], engine);

        // dep-base should be included via expansion.
        Assert.Equal(2, result.TweakCount);
    }

    [Fact]
    public void Estimate_WithoutEngine_DoesNotExpandDependsOn()
    {
        var root = new TweakDef
        {
            Id = "no-expand-root",
            Label = "Root",
            Category = "Test",
            DependsOn = ["some-other-id"],
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\no-expand-root", "V", 1)],
        };

        var result = BatchImpactEstimator.Estimate([root], engine: null);

        Assert.Equal(1, result.TweakCount);
    }

    // ── OneLiner formatting ───────────────────────────────────────────────

    [Fact]
    public void BatchImpactSummary_OneLiner_ContainsTweakCount()
    {
        var tweaks = new[] { MakeTweak("oli-1"), MakeTweak("oli-2") };
        var result = BatchImpactEstimator.Estimate(tweaks);

        Assert.Contains("2 tweaks", result.OneLiner);
    }

    // ── FormatReport ─────────────────────────────────────────────────────

    [Fact]
    public void FormatReport_NonEmptySummary_ContainsOverallTier()
    {
        var result = BatchImpactEstimator.Estimate([MakeTweak("rpt-1", impact: 5)]);
        string report = BatchImpactEstimator.FormatReport(result);

        Assert.Contains("Significant", report);
    }

    [Fact]
    public void FormatReport_NullSummary_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => BatchImpactEstimator.FormatReport(null!));
    }

    // ── Builtins integration ──────────────────────────────────────────────

    [Fact]
    public void Estimate_AllBuiltins_TweakCountMatchesEngine()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var all = engine.AllTweaks();

        var result = BatchImpactEstimator.Estimate(all, engine);

        Assert.Equal(all.Count, result.TweakCount);
    }

    [Fact]
    public void Estimate_AllBuiltins_AllScoresInRange()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var result = BatchImpactEstimator.Estimate(engine.AllTweaks());

        Assert.InRange(result.AverageImpact, 1.0, 5.0);
        Assert.InRange(result.AverageSafety, 1.0, 5.0);
    }
}
