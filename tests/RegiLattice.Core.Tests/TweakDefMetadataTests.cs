// tests/RegiLattice.Core.Tests/TweakDefMetadataTests.cs
// Sprint 57 — ImpactScore and SafetyRating field tests.

using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 57: ImpactScore and SafetyRating metadata fields.</summary>
public sealed class TweakDefMetadataTests
{
    private static TweakDef MakeWithOps(string id = "meta-test") =>
        new()
        {
            Id = id,
            Label = "Metadata Test Tweak",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };

    // ── Defaults ──────────────────────────────────────────────────────────

    [Fact]
    public void ImpactScore_Default_IsThree()
    {
        var td = MakeWithOps();
        Assert.Equal(3, td.ImpactScore);
    }

    [Fact]
    public void SafetyRating_Default_IsFour()
    {
        var td = MakeWithOps();
        Assert.Equal(4, td.SafetyRating);
    }

    // ── Explicit values ───────────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ImpactScore_ExplicitValue_Roundtrips(int score)
    {
        var td = new TweakDef
        {
            Id = "meta-test",
            Label = "Metadata Test Tweak",
            Category = "Test",
            ImpactScore = score,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        Assert.Equal(score, td.ImpactScore);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void SafetyRating_ExplicitValue_Roundtrips(int rating)
    {
        var td = new TweakDef
        {
            Id = "meta-test",
            Label = "Metadata Test Tweak",
            Category = "Test",
            SafetyRating = rating,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        Assert.Equal(rating, td.SafetyRating);
    }

    // ── Validator range checks ────────────────────────────────────────────

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Validator_InvalidImpactScore_ReportsError(int score)
    {
        var td = new TweakDef
        {
            Id = "validator-impact",
            Label = "Impact Test",
            Category = "Test",
            ImpactScore = score,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("ImpactScore") && e.Contains("validator-impact"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Validator_InvalidSafetyRating_ReportsError(int rating)
    {
        var td = new TweakDef
        {
            Id = "validator-safety",
            Label = "Safety Test",
            Category = "Test",
            SafetyRating = rating,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("SafetyRating") && e.Contains("validator-safety"));
    }

    [Fact]
    public void Validator_ValidScores_NoErrors()
    {
        var td = new TweakDef
        {
            Id = "valid-scores",
            Label = "Valid Scores Test",
            Category = "Test",
            ImpactScore = 5,
            SafetyRating = 1,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.DoesNotContain(errors, e => e.Contains("ImpactScore") || e.Contains("SafetyRating"));
    }

    // ── Builtin tweaks carry default scores ───────────────────────────────

    [Fact]
    public void RegisterBuiltins_AllTweaks_HaveScoresInRange()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        Assert.All(
            engine.AllTweaks(),
            td =>
            {
                Assert.InRange(td.ImpactScore, 1, 5);
                Assert.InRange(td.SafetyRating, 1, 5);
            }
        );
    }
}
