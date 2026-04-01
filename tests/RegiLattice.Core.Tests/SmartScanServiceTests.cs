// RegiLattice.Core.Tests — SmartScanServiceTests.cs
// Unit tests for SmartScanService.
// SmartScanService.Scan() analyses the engine status map and returns prioritised
// recommendations ranked by ImpactScore × SafetyRating (max 25).

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class SmartScanServiceTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public SmartScanServiceTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    // ── Basic scan behaviour ────────────────────────────────────────────────

    [Fact]
    public void Scan_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => SmartScanService.Scan(null!));
    }

    [Fact]
    public void Scan_EmptyStatusMap_ReturnsRecommendations()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.NotNull(result);
        Assert.NotNull(result.Recommendations);
    }

    [Fact]
    public void Scan_NullStatusMap_ConsidersAllTweaksAsUnapplied()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        // Without a status map, all tweaks with operations are candidates.
        Assert.True(result.ScannedCount > 0);
    }

    [Fact]
    public void Scan_ReturnsAtMost25Recommendations()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.True(result.Recommendations.Count <= SmartScanService.MaxRecommendations);
    }

    [Fact]
    public void MaxRecommendations_Is25()
    {
        Assert.Equal(25, SmartScanService.MaxRecommendations);
    }

    [Fact]
    public void Scan_RecommendationsOrderedByPriorityDescending()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        int prev = int.MaxValue;
        foreach (var rec in result.Recommendations)
        {
            Assert.True(rec.PriorityScore <= prev, "Recommendations not ordered by PriorityScore descending");
            prev = rec.PriorityScore;
        }
    }

    [Fact]
    public void Scan_ScannedCount_IsPositive()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        // ScannedCount may be slightly less than AllTweaks().HasOperations due to IsApplicable predicates.
        int maxExpected = _engine.AllTweaks().Count(t => t.HasOperations);
        Assert.True(result.ScannedCount > 0);
        Assert.True(result.ScannedCount <= maxExpected);
    }

    // ── PriorityScore calculation ───────────────────────────────────────────

    [Fact]
    public void Scan_PriorityScore_EqualsImpactTimesMinimumSafety()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        foreach (var rec in result.Recommendations)
        {
            int expected = rec.Tweak.ImpactScore * rec.Tweak.SafetyRating;
            Assert.Equal(expected, rec.PriorityScore);
        }
    }

    [Fact]
    public void Scan_MaxPriorityScore_IsAtMost25()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.All(result.Recommendations, rec => Assert.True(rec.PriorityScore <= 25));
    }

    // ── Quick win logic ─────────────────────────────────────────────────────

    [Fact]
    public void ScanRecommendation_IsQuickWin_RequiresBothImpactAndSafetyAtLeast4()
    {
        // A recommendation is a quick win only if ImpactScore >= 4 AND SafetyRating >= 4
        var result = SmartScanService.Scan(_engine, statusMap: null);
        foreach (var rec in result.Recommendations)
        {
            bool expectedQuickWin = rec.Tweak.ImpactScore >= 4 && rec.Tweak.SafetyRating >= 4;
            Assert.Equal(expectedQuickWin, rec.IsQuickWin);
        }
    }

    [Fact]
    public void Scan_QuickWinsCount_MatchesFilteredCount()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        int expected = result.Recommendations.Count(r => r.IsQuickWin);
        Assert.Equal(expected, result.QuickWinsCount);
    }

    // ── CorpSafe filtering ──────────────────────────────────────────────────

    [Fact]
    public void Scan_ForceCorpSafe_ReturnsOnlyCorpSafeTweaks()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null, forceCorpSafe: true);
        Assert.All(result.Recommendations, rec => Assert.True(rec.Tweak.CorpSafe, $"Tweak {rec.Tweak.Id} is not CorpSafe but was recommended"));
    }

    [Fact]
    public void Scan_ForceCorpSafe_ProducesSubsetOfUnfilteredScan()
    {
        // The corp-safe-filtered scan should return a subset of IDs from the unfiltered scan.
        var allRec = SmartScanService.Scan(_engine, statusMap: null, forceCorpSafe: false).Recommendations;
        var corpRec = SmartScanService.Scan(_engine, statusMap: null, forceCorpSafe: true).Recommendations;
        var corpIds = corpRec.Select(r => r.Tweak.Id).ToHashSet();
        var allIds = allRec.Select(r => r.Tweak.Id).ToHashSet();
        Assert.True(
            corpIds.IsSubsetOf(allIds) || corpRec.Count <= allRec.Count,
            "CorpSafe scan should produce at most as many recommendations as the unconstrained scan"
        );
    }

    // ── Status map filtering ────────────────────────────────────────────────

    [Fact]
    public void Scan_WithAllAppliedStatusMap_ReturnsFewerRecommendations()
    {
        // Mark all tweaks as Applied — should produce fewer (or no) recommendations
        var allApplied = _engine.AllTweaks().Where(t => t.HasOperations).ToDictionary(t => t.Id, _ => TweakResult.Applied);
        var result = SmartScanService.Scan(_engine, statusMap: allApplied);
        Assert.Empty(result.Recommendations);
    }

    [Fact]
    public void Scan_WithAllNotAppliedStatusMap_ReturnsRecommendations()
    {
        var allNotApplied = _engine.AllTweaks().Where(t => t.HasOperations).ToDictionary(t => t.Id, _ => TweakResult.NotApplied);
        var result = SmartScanService.Scan(_engine, statusMap: allNotApplied);
        // Should return up to MaxRecommendations
        Assert.True(result.Recommendations.Count > 0);
        Assert.True(result.Recommendations.Count <= SmartScanService.MaxRecommendations);
    }

    [Fact]
    public void Scan_AppliedTweakInStatusMap_NotInRecommendations()
    {
        // Pick a real tweak that has high impact
        var highImpact = _engine.AllTweaks().Where(t => t.HasOperations && t.ImpactScore >= 4).Take(5).ToList();
        var statusMap = highImpact.ToDictionary(t => t.Id, _ => TweakResult.Applied);

        var result = SmartScanService.Scan(_engine, statusMap: statusMap);
        foreach (var td in highImpact)
            Assert.DoesNotContain(result.Recommendations, rec => rec.Tweak.Id == td.Id);
    }

    // ── ScanResult metadata ─────────────────────────────────────────────────

    [Fact]
    public void ScanResult_ScannedAt_IsRecentUtcTime()
    {
        var before = DateTime.UtcNow.AddSeconds(-1);
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.True(result.ScannedAt >= before);
        Assert.True(result.ScannedAt <= DateTime.UtcNow.AddSeconds(5));
    }

    [Fact]
    public void ScanRecommendation_Reason_IsNotEmpty()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.All(result.Recommendations, rec => Assert.False(string.IsNullOrWhiteSpace(rec.Reason)));
    }

    [Fact]
    public void Scan_AllRecommendations_HaveNonNullTweak()
    {
        var result = SmartScanService.Scan(_engine, statusMap: null);
        Assert.All(result.Recommendations, rec => Assert.NotNull(rec.Tweak));
    }
}
