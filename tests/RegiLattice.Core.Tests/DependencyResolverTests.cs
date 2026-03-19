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

    [Fact]
    public void Resolve_FourLevelChain_ReturnsCorrectOrder()
    {
        var td1 = TestHelpers.Make("lvl-1");
        var td2 = TestHelpers.Make("lvl-2", dependsOn: ["lvl-1"]);
        var td3 = TestHelpers.Make("lvl-3", dependsOn: ["lvl-2"]);
        var td4 = TestHelpers.Make("lvl-4", dependsOn: ["lvl-3"]);
        var result = DependencyResolver.Resolve(td4, TestHelpers.LookupFrom(td1, td2, td3, td4));
        Assert.Equal(4, result.Count);
        Assert.Equal("lvl-1", result[0].Id);
        Assert.Equal("lvl-4", result[3].Id);
    }

    [Fact]
    public void Resolve_AlreadyVisitedNode_IsNotDuplicated()
    {
        // B and C both depend on A. D depends on B and C. A should appear only once.
        var tdA = TestHelpers.Make("shared-a");
        var tdB = TestHelpers.Make("shared-b", dependsOn: ["shared-a"]);
        var tdC = TestHelpers.Make("shared-c", dependsOn: ["shared-a"]);
        var tdD = TestHelpers.Make("shared-d", dependsOn: ["shared-b", "shared-c"]);
        var result = DependencyResolver.Resolve(tdD, TestHelpers.LookupFrom(tdA, tdB, tdC, tdD));
        var aCount = result.Count(t => t.Id == "shared-a");
        Assert.Equal(1, aCount);
    }

    [Fact]
    public void Dependents_EmptyCollection_ReturnsEmpty()
    {
        var result = DependencyResolver.Dependents("any-id", []);
        Assert.Empty(result);
    }

    [Fact]
    public void Dependents_TransitiveDependents_NotIncluded()
    {
        // C depends on B, B depends on A — Dependents(A) should only return B (direct), not C
        var tdA = TestHelpers.Make("trans-a");
        var tdB = TestHelpers.Make("trans-b", dependsOn: ["trans-a"]);
        var tdC = TestHelpers.Make("trans-c", dependsOn: ["trans-b"]);
        var result = DependencyResolver.Dependents("trans-a", [tdA, tdB, tdC]);
        Assert.Single(result);
        Assert.Equal("trans-b", result[0].Id);
    }

    [Fact]
    public void Resolve_OneMissingOnePresentDep_SkipsMissingIncludesPresent()
    {
        var present = TestHelpers.Make("present-dep");
        var target = TestHelpers.Make("target", dependsOn: ["missing-dep", "present-dep"]);
        var result = DependencyResolver.Resolve(target, TestHelpers.LookupFrom(present, target));
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Id == "present-dep");
        Assert.Contains(result, t => t.Id == "target");
    }

    [Fact]
    public void Resolve_TargetHasNoDeps_ReturnsSelf()
    {
        var td = TestHelpers.Make("no-deps-v2");
        var result = DependencyResolver.Resolve(td, TestHelpers.LookupFrom(td));
        Assert.Single(result);
        Assert.Equal("no-deps-v2", result[0].Id);
    }

    [Fact]
    public void Dependents_CaseInsensitive_Id_Match()
    {
        var parent = TestHelpers.Make("MixedCase-Parent");
        var child = TestHelpers.Make("child-of-mixed", dependsOn: ["MixedCase-Parent"]);
        var result = DependencyResolver.Dependents("mixedcase-parent", [parent, child]);
        Assert.Single(result);
    }

    [Fact]
    public void Dependents_MultipleLevel_OnlyDirectDeps()
    {
        var a = TestHelpers.Make("dep-level-a");
        var b = TestHelpers.Make("dep-level-b", dependsOn: ["dep-level-a"]);
        var c = TestHelpers.Make("dep-level-c", dependsOn: ["dep-level-b"]);
        var d = TestHelpers.Make("dep-level-d", dependsOn: ["dep-level-a"]);
        var result = DependencyResolver.Dependents("dep-level-a", [a, b, c, d]);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, t => t.Id == "dep-level-b");
        Assert.Contains(result, t => t.Id == "dep-level-d");
    }

    // ── Resolve: deep chain (10 levels) ─────────────────────────────────

    [Fact]
    public void Resolve_TenLevelChain_ReturnsAllInOrder()
    {
        var tweaks = new TweakDef[10];
        tweaks[0] = TestHelpers.Make("deep-0");
        for (int i = 1; i < 10; i++)
            tweaks[i] = TestHelpers.Make($"deep-{i}", dependsOn: [$"deep-{i - 1}"]);

        var result = DependencyResolver.Resolve(tweaks[9], TestHelpers.LookupFrom(tweaks));
        Assert.Equal(10, result.Count);
        Assert.Equal("deep-0", result[0].Id);
        Assert.Equal("deep-9", result[9].Id);
    }

    // ── Resolve: wide fan-out ───────────────────────────────────────────

    [Fact]
    public void Resolve_WideFanOut_AllDepsBeforeTarget()
    {
        // Target depends on 5 independent tweaks
        var deps = Enumerable.Range(0, 5).Select(i => TestHelpers.Make($"fan-{i}")).ToArray();
        var target = TestHelpers.Make("fan-root", dependsOn: deps.Select(d => d.Id).ToList());
        var all = deps.Append(target).ToArray();
        var result = DependencyResolver.Resolve(target, TestHelpers.LookupFrom(all));
        Assert.Equal(6, result.Count);
        Assert.Equal("fan-root", result[^1].Id);
        foreach (var dep in deps)
            Assert.Contains(result, t => t.Id == dep.Id);
    }

    // ── Resolve: all deps missing ───────────────────────────────────────

    [Fact]
    public void Resolve_AllDepsMissing_ReturnsSelfOnly()
    {
        var td = TestHelpers.Make("lonely", dependsOn: ["ghost-1", "ghost-2", "ghost-3"]);
        var result = DependencyResolver.Resolve(td, _ => null);
        Assert.Single(result);
        Assert.Equal("lonely", result[0].Id);
    }

    // ── Dependents: multiple incoming edges ─────────────────────────────

    [Fact]
    public void Dependents_NodeDependsOnMultipleParents_AppearsOnce()
    {
        var p1 = TestHelpers.Make("parent-1");
        var p2 = TestHelpers.Make("parent-2");
        var child = TestHelpers.Make("multi-child", dependsOn: ["parent-1", "parent-2"]);
        var result1 = DependencyResolver.Dependents("parent-1", [p1, p2, child]);
        var result2 = DependencyResolver.Dependents("parent-2", [p1, p2, child]);
        Assert.Single(result1);
        Assert.Single(result2);
        Assert.Equal("multi-child", result1[0].Id);
        Assert.Equal("multi-child", result2[0].Id);
    }

    // ── Resolve: diamond with 5 nodes ───────────────────────────────────

    [Fact]
    public void Resolve_DoubleDiamond_NoDuplicates()
    {
        // A -> B -> D, A -> C -> D, D -> E
        var a = TestHelpers.Make("dd-a");
        var b = TestHelpers.Make("dd-b", dependsOn: ["dd-a"]);
        var c = TestHelpers.Make("dd-c", dependsOn: ["dd-a"]);
        var d = TestHelpers.Make("dd-d", dependsOn: ["dd-b", "dd-c"]);
        var e = TestHelpers.Make("dd-e", dependsOn: ["dd-d"]);
        var result = DependencyResolver.Resolve(e, TestHelpers.LookupFrom(a, b, c, d, e));
        Assert.Equal(5, result.Count);
        Assert.Equal(5, result.Select(t => t.Id).Distinct().Count());
        Assert.Equal("dd-e", result[^1].Id);
    }
}
