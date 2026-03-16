using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Direct tests for <see cref="TweakValidator"/> — integrity checks and circular dep detection.</summary>
public sealed class TweakValidatorTests
{
    private static TweakDef Make(
        string id,
        string label = "Tweak",
        string category = "Test",
        IReadOnlyList<string>? dependsOn = null,
        bool hasApplyOps = true
    ) =>
        new()
        {
            Id = id,
            Label = label,
            Category = category,
            DependsOn = dependsOn ?? [],
            ApplyOps = hasApplyOps ? [RegOp.SetDword($@"HKCU\Software\{id}", "V", 1)] : [],
        };

    private static Func<string, TweakDef?> LookupFrom(params TweakDef[] tweaks)
    {
        var dict = tweaks.ToDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase);
        return id => dict.GetValueOrDefault(id);
    }

    // ── Valid tweaks ────────────────────────────────────────────────────

    [Fact]
    public void Validate_ValidTweaks_ReturnsNoErrors()
    {
        var td1 = Make("valid-1");
        var td2 = Make("valid-2", dependsOn: ["valid-1"]);
        var errors = TweakValidator.Validate([td1, td2], LookupFrom(td1, td2));
        Assert.Empty(errors);
    }

    [Fact]
    public void Validate_SingleValidTweak_ReturnsNoErrors()
    {
        var td = Make("solo");
        var errors = TweakValidator.Validate([td], LookupFrom(td));
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
        var td = Make("", hasApplyOps: true);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Id", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_WhitespaceId_ReturnsError()
    {
        var td = Make("   ", hasApplyOps: true);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Id", StringComparison.OrdinalIgnoreCase));
    }

    // ── Empty Label ─────────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyLabel_ReturnsError()
    {
        var td = Make("lbl-test", label: "");
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Label", StringComparison.OrdinalIgnoreCase));
    }

    // ── Empty Category ──────────────────────────────────────────────────

    [Fact]
    public void Validate_EmptyCategory_ReturnsError()
    {
        var td = Make("cat-test", category: "");
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("empty Category", StringComparison.OrdinalIgnoreCase));
    }

    // ── Duplicate IDs ───────────────────────────────────────────────────

    [Fact]
    public void Validate_DuplicateIds_ReturnsError()
    {
        var td1 = Make("dup-id");
        var td2 = Make("dup-id");
        var errors = TweakValidator.Validate([td1, td2], _ => null);
        Assert.Contains(errors, e => e.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_DuplicateIds_CaseInsensitive()
    {
        var td1 = Make("DUP-CASE");
        var td2 = Make("dup-case");
        var errors = TweakValidator.Validate([td1, td2], _ => null);
        Assert.Contains(errors, e => e.Contains("Duplicate", StringComparison.OrdinalIgnoreCase));
    }

    // ── No ApplyOps ─────────────────────────────────────────────────────

    [Fact]
    public void Validate_NoApplyOps_ReturnsError()
    {
        var td = Make("no-ops", hasApplyOps: false);
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
        var errors = TweakValidator.Validate([td], LookupFrom(td));
        Assert.DoesNotContain(errors, e => e.Contains("no ApplyOps or ApplyAction", StringComparison.OrdinalIgnoreCase));
    }

    // ── Broken DependsOn ────────────────────────────────────────────────

    [Fact]
    public void Validate_BrokenDependsOn_ReturnsError()
    {
        var td = Make("dep-broken", dependsOn: ["nonexistent-dep"]);
        var errors = TweakValidator.Validate([td], _ => null);
        Assert.Contains(errors, e => e.Contains("unknown tweak", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(errors, e => e.Contains("nonexistent-dep"));
    }

    [Fact]
    public void Validate_ValidDependsOn_NoError()
    {
        var dep = Make("dep-exists");
        var td = Make("dep-parent", dependsOn: ["dep-exists"]);
        var errors = TweakValidator.Validate([dep, td], LookupFrom(dep, td));
        Assert.DoesNotContain(errors, e => e.Contains("unknown tweak", StringComparison.OrdinalIgnoreCase));
    }

    // ── Circular dependencies ───────────────────────────────────────────

    [Fact]
    public void Validate_CircularDep_TwoNodes_ReturnsError()
    {
        var td1 = Make("circ-x", dependsOn: ["circ-y"]);
        var td2 = Make("circ-y", dependsOn: ["circ-x"]);
        var errors = TweakValidator.Validate([td1, td2], LookupFrom(td1, td2));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_CircularDep_ThreeNodes_ReturnsError()
    {
        var td1 = Make("circ-a", dependsOn: ["circ-b"]);
        var td2 = Make("circ-b", dependsOn: ["circ-c"]);
        var td3 = Make("circ-c", dependsOn: ["circ-a"]);
        var errors = TweakValidator.Validate([td1, td2, td3], LookupFrom(td1, td2, td3));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_SelfDep_ReturnsCircularError()
    {
        var td = Make("self-dep", dependsOn: ["self-dep"]);
        var errors = TweakValidator.Validate([td], LookupFrom(td));
        Assert.Contains(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Validate_LinearChain_NoCircularError()
    {
        var td1 = Make("chain-1");
        var td2 = Make("chain-2", dependsOn: ["chain-1"]);
        var td3 = Make("chain-3", dependsOn: ["chain-2"]);
        var errors = TweakValidator.Validate([td1, td2, td3], LookupFrom(td1, td2, td3));
        Assert.DoesNotContain(errors, e => e.Contains("circular", StringComparison.OrdinalIgnoreCase));
    }

    // ── Multiple errors at once ─────────────────────────────────────────

    [Fact]
    public void Validate_MultipleIssues_ReturnsAll()
    {
        var td1 = Make("", hasApplyOps: true); // empty ID
        var td2 = Make("multi-err", label: ""); // empty label
        var td3 = Make("multi-err", dependsOn: ["ghost"]); // duplicate + broken dep
        var errors = TweakValidator.Validate([td1, td2, td3], _ => null);
        Assert.True(errors.Count >= 3, $"Expected at least 3 errors, got {errors.Count}");
    }
}
