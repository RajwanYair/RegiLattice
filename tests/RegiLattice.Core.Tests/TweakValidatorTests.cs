using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Direct tests for <see cref="TweakValidator"/> — integrity checks and circular dep detection.</summary>
public sealed class TweakValidatorTests
{

    // ── Valid tweaks ────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidTweaks_ReturnsNoErrors()
    {
        var td1 = TestHelpers.Make("valid-1");
        var td2 = TestHelpers.Make("valid-2", dependsOn: ["valid-1"]);
        var errors = TweakValidator.Validate([td1, td2], TestHelpers.LookupFrom(td1, td2));
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_SingleValidTweak_ReturnsNoErrors()
    {
        var td = TestHelpers.Make("solo");
        var errors = TweakValidator.Validate([td], TestHelpers.LookupFrom(td));
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_EmptyList_ReturnsNoErrors()
    {
        var errors = TweakValidator.Validate([], _ => null);
        Assert.Empty(errors);
    }

    // ── Empty ID ────────────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyId_ReturnsError()
    {
        var td = TestHelpers.Make("", hasApplyOps: true);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Id", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_WhitespaceId_ReturnsError()
    {
        var td = TestHelpers.Make("   ", hasApplyOps: true);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Id", StringComparison.OrdinalIgnoreCase));
    }

    // ── Empty Label ─────────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyLabel_ReturnsError()
    {
        var td = TestHelpers.Make("lbl-test", label: "");
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Label", StringComparison.OrdinalIgnoreCase));
    }

    // ── Empty Category ──────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyCategory_ReturnsError()
    {
        var td = TestHelpers.Make("cat-test", category: "");
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Category", StringComparison.OrdinalIgnoreCase));
    }

    // ── Duplicate IDs ───────────────────────────────────────────────────

    [Fact]
    public void Validate_DuplicateIds_ReturnsError()
    {
        var td1 = TestHelpers.Make("dup-id");
        var td2 = TestHelpers.Make("dup-id");
        var errors = TweakValidator.Validate([td1, td2], _ => null);
        Assert.Contains(errors, e => e.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_DuplicateIds_CaseInsensitive()
    {
        var td1 = TestHelpers.Make("DUP-CASE");
        var td2 = TestHelpers.Make("dup-case");
        var errors = TweakValidator.Validate([td1, td2], _ => null);
        Assert.Contains(errors, e => e.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
    }

    // ── No ApplyOps ─────────────────────────────────────────────────────

    [Fact]
    public void Validate_NoApplyOps_ReturnsError()
    {
        var td = TestHelpers.Make("no-ops", hasApplyOps: false);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("no ApplyOps or ApplyAction", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_WithApplyAction_NoApplyOps_NoError()
    {
        var td = new TweakDef
        {
            Id = "action-only",
            Label = "Action",
            Category = "Test",
            ApplyAction = _ => { },
        };
        var errors = TweakValidator.Validate([td], TestHelpers.LookupFrom(td));
        Assert.DoesNotContain(errors, e => e.Contains("no ApplyOps or ApplyAction", StringComparison.OrdinalIgnoreCase));
    }

    // ── Broken DependsOn ────────────────────────────────────────────────

    [Fact]
    public void Validate_BrokenDependsOn_ReturnsError()
    {
        var td = TestHelpers.Make("dep-broken", dependsOn: ["nonexistent-dep"]);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("unknown tweak", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, e => e.Contains("nonexistent-dep"));
    }

    [Fact]
    public void Validate_ValidDependsOn_NoError()
    {
        var dep = TestHelpers.Make("dep-exists");
        var td = TestHelpers.Make("dep-parent", dependsOn: ["dep-exists"]);
        var errors = TweakValidator.Validate([dep, td], TestHelpers.LookupFrom(dep, td));
        Assert.DoesNotContain(errors, e => e.Contains("unknown tweak", StringComparison.OrdinalIgnoreCase));
    }

    // ── Circular dependencies ───────────────────────────────────────────

    [Fact]
    public void Validate_CircularDep_TwoNodes_ReturnsError()
    {
        var td1 = TestHelpers.Make("circ-x", dependsOn: ["circ-y"]);
        var td2 = TestHelpers.Make("circ-y", dependsOn: ["circ-x"]);
        var errors = TweakValidator.Validate([td1, td2], TestHelpers.LookupFrom(td1, td2));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_CircularDep_ThreeNodes_ReturnsError()
    {
        var td1 = TestHelpers.Make("circ-a", dependsOn: ["circ-b"]);
        var td2 = TestHelpers.Make("circ-b", dependsOn: ["circ-c"]);
        var td3 = TestHelpers.Make("circ-c", dependsOn: ["circ-a"]);
        var errors = TweakValidator.Validate([td1, td2, td3], TestHelpers.LookupFrom(td1, td2, td3));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_SelfDep_ReturnsCircularError()
    {
        var td = TestHelpers.Make("self-dep", dependsOn: ["self-dep"]);
        var errors = TweakValidator.Validate([td], TestHelpers.LookupFrom(td));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_LinearChain_NoCircularError()
    {
        var td1 = TestHelpers.Make("chain-1");
        var td2 = TestHelpers.Make("chain-2", dependsOn: ["chain-1"]);
        var td3 = TestHelpers.Make("chain-3", dependsOn: ["chain-2"]);
        var errors = TweakValidator.Validate([td1, td2, td3], TestHelpers.LookupFrom(td1, td2, td3));
        Assert.DoesNotContain(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    // ── Multiple errors at once ─────────────────────────────────────────

    [Fact]
    public void Validate_MultipleIssues_ReturnsAll()
    {
        var td1 = TestHelpers.Make("", hasApplyOps: true); // empty ID
        var td2 = TestHelpers.Make("multi-err", label: ""); // empty label
        var td3 = TestHelpers.Make("multi-err", dependsOn: ["ghost"]); // duplicate + broken dep
        var errors = TweakValidator.Validate([td1, td2, td3], _ => null);
        Assert.True(errors.Count >= 3, $"Expected at least 3 errors, got {errors.Count}");
    }

    // ── Duplicate registry operations ───────────────────────────────────

    [Fact]
    public void DetectDuplicateRegistryOps_DuplicateTarget_ReturnsWarning()
    {
        var td1 = new TweakDef
        {
            Id = "dup-reg-1",
            Label = "Dup Reg 1",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Value", 1)],
        };
        var td2 = new TweakDef
        {
            Id = "dup-reg-2",
            Label = "Dup Reg 2",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Value", 0)],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Contains(warnings, e => e.Contains("Duplicate registry target") && e.Contains("dup-reg-1") && e.Contains("dup-reg-2"));
    }

    [Fact]
    public void DetectDuplicateRegistryOps_SamePathDifferentNames_NoWarning()
    {
        var td1 = new TweakDef
        {
            Id = "diff-name-1",
            Label = "Diff Name 1",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "ValueA", 1)],
        };
        var td2 = new TweakDef
        {
            Id = "diff-name-2",
            Label = "Diff Name 2",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "ValueB", 1)],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Empty(warnings);
    }

    [Fact]
    public void DetectDuplicateRegistryOps_CaseInsensitive()
    {
        var td1 = new TweakDef
        {
            Id = "case-reg-1",
            Label = "Case Reg 1",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\SOFTWARE\Test", "Value", 1)],
        };
        var td2 = new TweakDef
        {
            Id = "case-reg-2",
            Label = "Case Reg 2",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\software\test", "value", 0)],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Contains(warnings, e => e.Contains("Duplicate registry target"));
    }

    [Fact]
    public void DetectDuplicateRegistryOps_SameTweakMultipleOps_NoWarning()
    {
        var td = new TweakDef
        {
            Id = "multi-ops",
            Label = "Multi Ops",
            Category = "Test",
            ApplyOps = [RegOp.SetDword(@"HKCU\Software\Test", "Value", 1), RegOp.SetDword(@"HKCU\Software\Test", "Value", 2)],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td]);
        Assert.Empty(warnings);
    }

    [Fact]
    public void DetectDuplicateRegistryOps_DeleteTreeDuplicate_ReturnsWarning()
    {
        var td1 = new TweakDef
        {
            Id = "tree-1",
            Label = "Tree 1",
            Category = "Test",
            ApplyOps = [RegOp.DeleteTree(@"HKCU\Software\Cleanup")],
        };
        var td2 = new TweakDef
        {
            Id = "tree-2",
            Label = "Tree 2",
            Category = "Test",
            ApplyOps = [RegOp.DeleteTree(@"HKCU\Software\Cleanup")],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Contains(warnings, e => e.Contains("Duplicate registry target") && e.Contains("tree-1") && e.Contains("tree-2"));
    }

    [Fact]
    public void DetectDuplicateRegistryOps_NoOverlap_NoWarning()
    {
        var td1 = TestHelpers.Make("unique-1");
        var td2 = TestHelpers.Make("unique-2");
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Empty(warnings);
    }

    [Fact]
    public void DetectDuplicateRegistryOps_EmptyList_ReturnsNoWarnings()
    {
        var warnings = TweakValidator.DetectDuplicateRegistryOps([]);
        Assert.Empty(warnings);
    }

    [Fact]
    public void DetectDuplicateRegistryOps_DetectionOpsExcluded()
    {
        // CheckDword/CheckMissing/CheckKeyMissing should NOT count as duplicates
        var td1 = new TweakDef
        {
            Id = "detect-only-1",
            Label = "Detect 1",
            Category = "Test",
            ApplyOps = [RegOp.CheckDword(@"HKCU\Software\Test", "Value", 1)],
        };
        var td2 = new TweakDef
        {
            Id = "detect-only-2",
            Label = "Detect 2",
            Category = "Test",
            ApplyOps = [RegOp.CheckDword(@"HKCU\Software\Test", "Value", 1)],
        };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([td1, td2]);
        Assert.Empty(warnings);
    }

    [Theory]
    [InlineData("white-label", " ", "Test")]
    [InlineData("white-cat", "Label", "   ")]
    public void Validate_WhitespaceFields_ReturnError(string id, string label, string category)
    {
        var td = new TweakDef
        {
            Id = id,
            Label = label,
            Category = category,
            ApplyOps = [RegOp.SetDword(@"HKCU\Test", "V", 1)],
        };
        var errors = TweakValidator.Validate([td], TestHelpers.LookupFrom(td));
        Assert.NotEmpty(errors);
    }

    [Fact]
    public void Validate_MultipleBrokenDeps_AllErrorsReported()
    {
        var td = TestHelpers.Make("multi-broken-dep", dependsOn: ["ghost-1", "ghost-2"]);
        var errors = TweakValidator.Validate([td], _ => null);
        var depErrors = errors.Where(e => e.Contains("unknown tweak")).ToList();
        Assert.Equal(2, depErrors.Count);
    }

    [Fact]
    public void DetectDuplicateRegistryOps_ThreeTweaksSameKey_AllNamed()
    {
        TweakDef Make(string id, int val) =>
            new()
            {
                Id = id,
                Label = id,
                Category = "T",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\Shared", "SameValue", val)],
            };
        var warnings = TweakValidator.DetectDuplicateRegistryOps([Make("x1", 1), Make("x2", 0), Make("x3", 2)]);
        Assert.Single(warnings);
        Assert.Contains("x1", warnings[0]);
        Assert.Contains("x2", warnings[0]);
        Assert.Contains("x3", warnings[0]);
    }

    [Fact]
    public void Validate_WithApplyAction_BrokenDep_ReportsDepError()
    {
        var td = new TweakDef
        {
            Id = "action-broken",
            Label = "Action",
            Category = "Test",
            ApplyAction = _ => { },
            DependsOn = ["nonexistent"],
        };
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("unknown tweak"));
        Assert.DoesNotContain(errors, e => e.Contains("no ApplyOps"));
    }
}
