using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Direct tests for <see cref="DependencyResolver"/> — topological sort and reverse lookup.</summary>
public sealed class DependencyResolverTests
{
    private static TweakDef Make(string id, params string[] dependsOn) =>
        new()
        {
            Id = id,
            Label = $"Tweak {id}",
            Category = "Test",
            DependsOn = dependsOn,
            ApplyOps = [RegOp.SetDword($@"HKCU\Software\{id}", "V", 1)],
        };

    private static Func<string, TweakDef?> LookupFrom(params TweakDef[] tweaks)
    {
        var dict = tweaks.ToDictionary(t => t.Id, StringComparer.OrdinalIgnoreCase);
        return id => dict.GetValueOrDefault(id);
    }

    // ── Resolve: no dependencies ────────────────────────────────────────

    [Fact]
    public void Resolve_NoDeps_ReturnsSelfOnly()
    {
        var td = Make("independent");
        var result = DependencyResolver.Resolve(td, _ => null);
        Assert.Single(result);
        Assert.Equal("independent", result[0].Id);
    }

    // ── Resolve: simple chain ───────────────────────────────────────────

    [Fact]
    public void Resolve_SingleDep_ReturnsTwoInOrder()
    {
        var tdA = Make("dep-a");
        var tdB = Make("dep-b", "dep-a");
        var result = DependencyResolver.Resolve(tdB, LookupFrom(tdA, tdB));
        Assert.Equal(2, result.Count);
        Assert.Equal("dep-a", result[0].Id);
        Assert.Equal("dep-b", result[1].Id);
    }

    [Fact]
    public void Resolve_ThreeNodeChain_ReturnsCorrectOrder()
    {
        var tdA = Make("chain-a");
        var tdB = Make("chain-b", "chain-a");
        var tdC = Make("chain-c", "chain-b");
        var result = DependencyResolver.Resolve(tdC, LookupFrom(tdA, tdB, tdC));
        Assert.Equal(3, result.Count);
        Assert.Equal("chain-a", result[0].Id);
        Assert.Equal("chain-b", result[1].Id);
        Assert.Equal("chain-c", result[2].Id);
    }

    // ── Resolve: diamond dependency ─────────────────────────────────────

    [Fact]
    public void Resolve_DiamondDep_NoDuplicates()
    {
        // A < B, A < C, B < D, C < D (diamond: D depends on B and C, both depend on A)
        var tdA = Make("dia-a");
        var tdB = Make("dia-b", "dia-a");
        var tdC = Make("dia-c", "dia-a");
        var tdD = Make("dia-d", "dia-b", "dia-c");
        var result = DependencyResolver.Resolve(tdD, LookupFrom(tdA, tdB, tdC, tdD));

        Assert.Equal(4, result.Count);
        // A must come before B and C; B and C must come before D
        var aIdx = result.ToList().FindIndex(t => t.Id == "dia-a");
        var bIdx = result.ToList().FindIndex(t => t.Id == "dia-b");
        var cIdx = result.ToList().FindIndex(t => t.Id == "dia-c");
        var dIdx = result.ToList().FindIndex(t => t.Id == "dia-d");
        Assert.True(aIdx < bIdx);
        Assert.True(aIdx < cIdx);
        Assert.True(bIdx < dIdx);
        Assert.True(cIdx < dIdx);
    }

    // ── Resolve: missing dependency ─────────────────────────────────────

    [Fact]
    public void Resolve_MissingDep_SkipsGracefully()
    {
        var td = Make("broken-dep", "nonexistent");
        var result = DependencyResolver.Resolve(td, _ => null);
        // Should still return the target tweak, just skip the missing dep
        Assert.Single(result);
        Assert.Equal("broken-dep", result[0].Id);
    }

    // ── Resolve: circular dependency ────────────────────────────────────

    [Fact]
    public void Resolve_CircularDep_TwoNodes_Throws()
    {
        var tdX = Make("circ-x", "circ-y");
        var tdY = Make("circ-y", "circ-x");
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(tdX, LookupFrom(tdX, tdY)));
    }

    [Fact]
    public void Resolve_CircularDep_ThreeNodes_Throws()
    {
        var td1 = Make("loop-1", "loop-3");
        var td2 = Make("loop-2", "loop-1");
        var td3 = Make("loop-3", "loop-2");
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(td1, LookupFrom(td1, td2, td3)));
    }

    [Fact]
    public void Resolve_SelfDep_Throws()
    {
        var td = Make("self-loop", "self-loop");
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(td, LookupFrom(td)));
    }

    [Fact]
    public void Resolve_CircularDep_ErrorMessageContainsId()
    {
        var tdX = Make("err-a", "err-b");
        var tdY = Make("err-b", "err-a");
        var ex = Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(tdX, LookupFrom(tdX, tdY)));
        Assert.Contains("Circular", ex.Message);
    }

    // ── Dependents: reverse lookup ──────────────────────────────────────

    [Fact]
    public void Dependents_NoDependents_ReturnsEmpty()
    {
        var td = Make("leaf");
        var result = DependencyResolver.Dependents("leaf", [td]);
        Assert.Empty(result);
    }

    [Fact]
    public void Dependents_SingleDependent_ReturnsOne()
    {
        var parent = Make("parent");
        var child = Make("child", "parent");
        var result = DependencyResolver.Dependents("parent", [parent, child]);
        Assert.Single(result);
        Assert.Equal("child", result[0].Id);
    }

    [Fact]
    public void Dependents_MultipleDependents_ReturnsAll()
    {
        var parent = Make("base");
        var c1 = Make("c1", "base");
        var c2 = Make("c2", "base");
        var c3 = Make("c3", "base");
        var other = Make("other");
        var result = DependencyResolver.Dependents("base", [parent, c1, c2, c3, other]);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, t => t.Id == "c1");
        Assert.Contains(result, t => t.Id == "c2");
        Assert.Contains(result, t => t.Id == "c3");
    }

    [Fact]
    public void Dependents_UnknownId_ReturnsEmpty()
    {
        var td = Make("only-one");
        var result = DependencyResolver.Dependents("nonexistent", [td]);
        Assert.Empty(result);
    }

    [Fact]
    public void Dependents_CaseInsensitive()
    {
        var parent = Make("PARENT-UPPER");
        var child = Make("child-lower", "parent-upper");
        var result = DependencyResolver.Dependents("parent-upper", [parent, child]);
        Assert.Single(result);
        Assert.Equal("child-lower", result[0].Id);
    }

    // ── Resolve: multiple dependencies at same level ────────────────────

    [Fact]
    public void Resolve_MultipleDepsAtSameLevel_AllIncluded()
    {
        var tdA = Make("multi-a");
        var tdB = Make("multi-b");
        var tdC = Make("multi-c", "multi-a", "multi-b");
        var result = DependencyResolver.Resolve(tdC, LookupFrom(tdA, tdB, tdC));
        Assert.Equal(3, result.Count);
        // C must be last
        Assert.Equal("multi-c", result[^1].Id);
        // Both A and B must be before C
        Assert.Contains(result, t => t.Id == "multi-a");
        Assert.Contains(result, t => t.Id == "multi-b");
    }
}
