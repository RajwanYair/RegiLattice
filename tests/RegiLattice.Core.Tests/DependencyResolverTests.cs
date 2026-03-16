using RegiLattice.Core;
using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Direct tests for <see cref="DependencyResolver"/> — topological sort and reverse lookup.</summary>
public sealed class DependencyResolverTests
{

    // ── Resolve: no dependencies ────────────────────────────────────────

    [Fact]
    public void Resolve_NoDeps_ReturnsSelfOnly()
    {
        var td = TestHelpers.Make("independent");
        var result = DependencyResolver.Resolve(td, _ => null);
        Assert.Single(result);
        Assert.Equal("independent", result[0].Id);
    }

    // ── Resolve: simple chain ───────────────────────────────────────────

    [Fact]
    public void Resolve_SingleDep_ReturnsTwoInOrder()
    {
        var tdA = TestHelpers.Make("dep-a");
        var tdB = TestHelpers.Make("dep-b", dependsOn: ["dep-a"]);
        var result = DependencyResolver.Resolve(tdB, TestHelpers.LookupFrom(tdA, tdB));
        Assert.Equal(2, result.Count);
        Assert.Equal("dep-a", result[0].Id);
        Assert.Equal("dep-b", result[1].Id);
    }

    [Fact]
    public void Resolve_ThreeNodeChain_ReturnsCorrectOrder()
    {
        var tdA = TestHelpers.Make("chain-a");
        var tdB = TestHelpers.Make("chain-b", dependsOn: ["chain-a"]);
        var tdC = TestHelpers.Make("chain-c", dependsOn: ["chain-b"]);
        var result = DependencyResolver.Resolve(tdC, TestHelpers.LookupFrom(tdA, tdB, tdC));
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
        var tdA = TestHelpers.Make("dia-a");
        var tdB = TestHelpers.Make("dia-b", dependsOn: ["dia-a"]);
        var tdC = TestHelpers.Make("dia-c", dependsOn: ["dia-a"]);
        var tdD = TestHelpers.Make("dia-d", dependsOn: ["dia-b", "dia-c"]);
        var result = DependencyResolver.Resolve(tdD, TestHelpers.LookupFrom(tdA, tdB, tdC, tdD));

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
        var td = TestHelpers.Make("broken-dep", dependsOn: ["nonexistent"]);
        var result = DependencyResolver.Resolve(td, _ => null);
        // Should still return the target tweak, just skip the missing dep
        Assert.Single(result);
        Assert.Equal("broken-dep", result[0].Id);
    }

    // ── Resolve: circular dependency ────────────────────────────────────

    [Fact]
    public void Resolve_CircularDep_TwoNodes_Throws()
    {
        var tdX = TestHelpers.Make("circ-x", dependsOn: ["circ-y"]);
        var tdY = TestHelpers.Make("circ-y", dependsOn: ["circ-x"]);
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(tdX, TestHelpers.LookupFrom(tdX, tdY)));
    }

    [Fact]
    public void Resolve_CircularDep_ThreeNodes_Throws()
    {
        var td1 = TestHelpers.Make("loop-1", dependsOn: ["loop-3"]);
        var td2 = TestHelpers.Make("loop-2", dependsOn: ["loop-1"]);
        var td3 = TestHelpers.Make("loop-3", dependsOn: ["loop-2"]);
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(td1, TestHelpers.LookupFrom(td1, td2, td3)));
    }

    [Fact]
    public void Resolve_SelfDep_Throws()
    {
        var td = TestHelpers.Make("self-loop", dependsOn: ["self-loop"]);
        Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(td, TestHelpers.LookupFrom(td)));
    }

    [Fact]
    public void Resolve_CircularDep_ErrorMessageContainsId()
    {
        var tdX = TestHelpers.Make("err-a", dependsOn: ["err-b"]);
        var tdY = TestHelpers.Make("err-b", dependsOn: ["err-a"]);
        var ex = Assert.Throws<InvalidOperationException>(() => DependencyResolver.Resolve(tdX, TestHelpers.LookupFrom(tdX, tdY)));
        Assert.Contains("Circular", ex.Message);
    }

    // ── Dependents: reverse lookup ──────────────────────────────────────

    [Fact]
    public void Dependents_NoDependents_ReturnsEmpty()
    {
        var td = TestHelpers.Make("leaf");
        var result = DependencyResolver.Dependents("leaf", [td]);
        Assert.Empty(result);
    }

    [Fact]
    public void Dependents_SingleDependent_ReturnsOne()
    {
        var parent = TestHelpers.Make("parent");
        var child = TestHelpers.Make("child", dependsOn: ["parent"]);
        var result = DependencyResolver.Dependents("parent", [parent, child]);
        Assert.Single(result);
        Assert.Equal("child", result[0].Id);
    }

    [Fact]
    public void Dependents_MultipleDependents_ReturnsAll()
    {
        var parent = TestHelpers.Make("base");
        var c1 = TestHelpers.Make("c1", dependsOn: ["base"]);
        var c2 = TestHelpers.Make("c2", dependsOn: ["base"]);
        var c3 = TestHelpers.Make("c3", dependsOn: ["base"]);
        var other = TestHelpers.Make("other");
        var result = DependencyResolver.Dependents("base", [parent, c1, c2, c3, other]);
        Assert.Equal(3, result.Count);
        Assert.Contains(result, t => t.Id == "c1");
        Assert.Contains(result, t => t.Id == "c2");
        Assert.Contains(result, t => t.Id == "c3");
    }

    [Fact]
    public void Dependents_UnknownId_ReturnsEmpty()
    {
        var td = TestHelpers.Make("only-one");
        var result = DependencyResolver.Dependents("nonexistent", [td]);
        Assert.Empty(result);
    }

    [Fact]
    public void Dependents_CaseInsensitive()
    {
        var parent = TestHelpers.Make("PARENT-UPPER");
        var child = TestHelpers.Make("child-lower", dependsOn: ["parent-upper"]);
        var result = DependencyResolver.Dependents("parent-upper", [parent, child]);
        Assert.Single(result);
        Assert.Equal("child-lower", result[0].Id);
    }

    // ── Resolve: multiple dependencies at same level ────────────────────

    [Fact]
    public void Resolve_MultipleDepsAtSameLevel_AllIncluded()
    {
        var tdA = TestHelpers.Make("multi-a");
        var tdB = TestHelpers.Make("multi-b");
        var tdC = TestHelpers.Make("multi-c", dependsOn: ["multi-a", "multi-b"]);
        var result = DependencyResolver.Resolve(tdC, TestHelpers.LookupFrom(tdA, tdB, tdC));
        Assert.Equal(3, result.Count);
        // C must be last
        Assert.Equal("multi-c", result[^1].Id);
        // Both A and B must be before C
        Assert.Contains(result, t => t.Id == "multi-a");
        Assert.Contains(result, t => t.Id == "multi-b");
    }
}
