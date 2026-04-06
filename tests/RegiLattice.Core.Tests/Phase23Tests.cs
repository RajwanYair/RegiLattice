// RegiLattice.Core.Tests — Phase23Tests.cs
// Tests for Phase 2.3 (confirm-apply threshold) and Phase 2.4 (batch ETA).

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>
/// Tests for Phase 2.3 (ConfirmApplyThreshold) and Phase 2.4 (EstimatedApplyTimeMs /
/// CalculateBatchEtaMs).  All tests run without registry writes or WinForms instances.
/// </summary>
public sealed class Phase23Tests
{
    // ── EstimatedApplyTimeMs —————————————————————————————————————————————

    [Theory]
    [InlineData(TweakKind.Registry, 50)]
    [InlineData(TweakKind.GroupPolicy, 50)]
    [InlineData(TweakKind.FileConfig, 200)]
    [InlineData(TweakKind.ScheduledTask, 500)]
    [InlineData(TweakKind.PowerShell, 500)]
    [InlineData(TweakKind.SystemCommand, 1_000)]
    [InlineData(TweakKind.ServiceControl, 2_000)]
    [InlineData(TweakKind.PackageManager, 3_000)]
    public void EstimatedApplyTimeMs_ForEachKind_ReturnsExpectedMs(TweakKind kind, int expectedMs)
    {
        var td = new TweakDef
        {
            Id = $"eta-kind-{kind.ToString().ToLowerInvariant()}",
            Label = "ETA Test",
            Category = "Test",
            KindHint = kind,
            ApplyAction = _ => { },  // needed so HasOperations = true
        };
        Assert.Equal(expectedMs, td.EstimatedApplyTimeMs);
    }

    [Fact]
    public void EstimatedApplyTimeMs_RegistryOps_AutoDetectsRegistry_Returns50()
    {
        var td = new TweakDef
        {
            Id = "eta-auto-registry",
            Label = "Auto Registry",
            Category = "Privacy",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "V", 1)],
        };
        // TweakKind.Registry auto-detected from HKCU path
        Assert.Equal(50, td.EstimatedApplyTimeMs);
    }

    [Fact]
    public void EstimatedApplyTimeMs_IsNonNegative_ForAllBuiltins()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        Assert.All(engine.AllTweaks(), td => Assert.True(td.EstimatedApplyTimeMs >= 50));
    }

    // ── CalculateBatchEtaMs ——————————————————————————————————————————————

    [Fact]
    public void CalculateBatchEtaMs_EmptyList_ReturnsZero()
    {
        var engine = new TweakEngine();
        Assert.Equal(0, engine.CalculateBatchEtaMs([]));
    }

    [Fact]
    public void CalculateBatchEtaMs_AllUnknownIds_ReturnsZero()
    {
        var engine = new TweakEngine();
        Assert.Equal(0, engine.CalculateBatchEtaMs(["unknown-1", "unknown-2"]));
    }

    [Fact]
    public void CalculateBatchEtaMs_SingleRegistryTweak_Returns50()
    {
        var engine = new TweakEngine();
        engine.Register([new TweakDef
        {
            Id = "eta-single-reg",
            Label = "Single Reg",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\EtaTest", "V", 1)],
        }]);
        Assert.Equal(50, engine.CalculateBatchEtaMs(["eta-single-reg"]));
    }

    [Fact]
    public void CalculateBatchEtaMs_MixedKinds_SumsCorrectly()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "eta-mixed-reg",
                Label = "Reg",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],  // Registry = 50ms
            },
            new TweakDef
            {
                Id = "eta-mixed-svc",
                Label = "Svc",
                Category = "Test",
                KindHint = TweakKind.ServiceControl,
                ApplyAction = _ => { },  // ServiceControl = 2000ms
            },
        ]);
        int eta = engine.CalculateBatchEtaMs(["eta-mixed-reg", "eta-mixed-svc"]);
        Assert.Equal(2050, eta);
    }

    [Fact]
    public void CalculateBatchEtaMs_DuplicateIds_SumsEachOccurrence()
    {
        var engine = new TweakEngine();
        engine.Register([new TweakDef
        {
            Id = "eta-dup",
            Label = "Dup",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],  // 50ms
        }]);
        // Passing the same id twice counts 50+50 = 100
        int eta = engine.CalculateBatchEtaMs(["eta-dup", "eta-dup"]);
        Assert.Equal(100, eta);
    }

    [Fact]
    public void CalculateBatchEtaMs_BuiltinsFirstTweak_ReturnsPositive()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var id = engine.AllTweaks().First().Id;
        Assert.True(engine.CalculateBatchEtaMs([id]) > 0);
    }

    [Fact]
    public void CalculateBatchEtaMs_FullBuiltinSet_IsPositiveAndReasonable()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();
        var ids = engine.AllTweaks().Select(t => t.Id);
        int total = engine.CalculateBatchEtaMs(ids);
        // With 7000+ mostly-Registry tweaks at 50ms each: total ≥ 100 000ms (100s)
        Assert.True(total > 100_000, $"Expected > 100 000 ms but got {total}");
    }

    // ── ConfirmApplyThreshold.ShouldConfirm ——————————————————————————————

    [Fact]
    public void ShouldConfirm_SafetyRating5_NoDangerousFlags_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "safe-5",
            Label = "Very Safe",
            Category = "Test",
            SafetyRating = 5,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Safe", "V", 1)],
        };
        Assert.False(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_SafetyRating4_NoDangerousFlags_ReturnsFalse()
    {
        var td = new TweakDef
        {
            Id = "safe-4",
            Label = "Safe 4",
            Category = "Test",
            SafetyRating = 4,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Safe4", "V", 1)],
        };
        Assert.False(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_SafetyRating3_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "risky-3",
            Label = "Risky 3",
            Category = "Test",
            SafetyRating = 3,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Risky", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_SafetyRating2_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "risky-2",
            Label = "Risky 2",
            Category = "Test",
            SafetyRating = 2,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Risky2", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_SafetyRating1_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "risky-1",
            Label = "Risky 1",
            Category = "Test",
            SafetyRating = 1,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Risky1", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_DeletesKeyOp_ReturnsTrue()
    {
        // RegOpKind.DeleteTree sets TweakRisk.DeletesKey in EffectiveRiskFlags
        var td = new TweakDef
        {
            Id = "deletes-key-test",
            Label = "Delete Key",
            Category = "Test",
            SafetyRating = 4,  // above threshold
            ApplyOps = [RegOp.DeleteTree(@"HKCU\Software\SomeApp")],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_ExplicitRequiresReboot_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "reboot-explicit",
            Label = "Reboot Required",
            Category = "Test",
            SafetyRating = 4,
            RiskFlags = TweakRisk.RequiresReboot,
            ApplyOps = [RegOp.SetDword(@"HKLM\System\Test", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_SecurityCategory_AutodetectsAffectsSecurity_ReturnsTrue()
    {
        // "Security" category → EffectiveRiskFlags includes AffectsSecurity
        var td = new TweakDef
        {
            Id = "security-category-test",
            Label = "Security Cat",
            Category = "Security",
            SafetyRating = 4,
            ApplyOps = [RegOp.SetDword(@"HKLM\Software\Security\Test", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    [Fact]
    public void ShouldConfirm_PotentialDataLossFlag_ReturnsTrue()
    {
        var td = new TweakDef
        {
            Id = "data-loss-explicit",
            Label = "Data Loss Risk",
            Category = "Test",
            SafetyRating = 4,
            RiskFlags = TweakRisk.PotentialDataLoss,
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\DL", "V", 1)],
        };
        Assert.True(ConfirmApplyThreshold.ShouldConfirm(td));
    }

    // ── ConfirmApplyThreshold constants ─────────────────────────────────

    [Fact]
    public void ConfirmApplyThreshold_SafetyRatingThreshold_Is3()
    {
        Assert.Equal(3, ConfirmApplyThreshold.SafetyRatingThreshold);
    }

    [Fact]
    public void ConfirmApplyThreshold_ConfirmationFlags_IncludesExpectedFlags()
    {
        var flags = ConfirmApplyThreshold.ConfirmationFlags;
        Assert.True(flags.HasFlag(TweakRisk.DeletesKey));
        Assert.True(flags.HasFlag(TweakRisk.RequiresReboot));
        Assert.True(flags.HasFlag(TweakRisk.AffectsSecurity));
        Assert.True(flags.HasFlag(TweakRisk.PotentialDataLoss));
    }
}
