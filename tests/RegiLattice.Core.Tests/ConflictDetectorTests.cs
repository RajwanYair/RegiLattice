// tests/RegiLattice.Core.Tests/ConflictDetectorTests.cs
// Sprint 66 — ConflictDetector.Detect / ConflictsFor / AllConflicts tests.

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 66: ConflictDetector.</summary>
public sealed class ConflictDetectorTests
{
    // Known conflicting pair used across multiple tests.
    private const string IdA = "energy-enable-hardware-accelerated-gpu-scheduling";
    private const string IdB = "sac-disable-hvci";

    // ── AllConflicts ──────────────────────────────────────────────────────

    [Fact]
    public void AllConflicts_IsNonEmpty()
    {
        Assert.NotEmpty(ConflictDetector.AllConflicts);
    }

    [Fact]
    public void AllConflicts_AllRecordsHaveNonEmptyIds()
    {
        Assert.All(
            ConflictDetector.AllConflicts,
            c =>
            {
                Assert.False(string.IsNullOrWhiteSpace(c.Id1), $"Id1 is empty in conflict ({c.Id1}, {c.Id2})");
                Assert.False(string.IsNullOrWhiteSpace(c.Id2), $"Id2 is empty in conflict ({c.Id1}, {c.Id2})");
                Assert.False(string.IsNullOrWhiteSpace(c.Reason), $"Reason is empty in conflict ({c.Id1}, {c.Id2})");
            }
        );
    }

    [Fact]
    public void AllConflicts_NoDuplicatePairs()
    {
        // Use a normalised-order key to detect (A,B) == (B,A).
        var pairs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var c in ConflictDetector.AllConflicts)
        {
            var key = string.CompareOrdinal(c.Id1, c.Id2) <= 0 ? $"{c.Id1}|{c.Id2}" : $"{c.Id2}|{c.Id1}";
            Assert.True(pairs.Add(key), $"Duplicate conflict pair registered: {c.Id1} / {c.Id2}");
        }
    }

    // ── Detect — empty / singleton ────────────────────────────────────────

    [Fact]
    public void Detect_EmptyList_ReturnsEmpty()
    {
        var conflicts = ConflictDetector.Detect([]);
        Assert.Empty(conflicts);
    }

    [Fact]
    public void Detect_SingleId_ReturnsEmpty()
    {
        var conflicts = ConflictDetector.Detect([IdA]);
        Assert.Empty(conflicts);
    }

    // ── Detect — known conflict ───────────────────────────────────────────

    [Fact]
    public void Detect_KnownConflictingPair_ReturnsOneConflict()
    {
        var conflicts = ConflictDetector.Detect([IdA, IdB]);
        Assert.Single(conflicts);
    }

    [Fact]
    public void Detect_KnownConflictingPair_ConflictContainsBothIds()
    {
        var conflict = ConflictDetector.Detect([IdA, IdB]).Single();
        var bothIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { conflict.Id1, conflict.Id2 };
        Assert.Contains(IdA, bothIds);
        Assert.Contains(IdB, bothIds);
    }

    [Fact]
    public void Detect_IsSymmetric_SameResultRegardlessOfOrder()
    {
        var ab = ConflictDetector.Detect([IdA, IdB]);
        var ba = ConflictDetector.Detect([IdB, IdA]);
        Assert.Equal(ab.Count, ba.Count);
    }

    // ── Detect — no conflict ──────────────────────────────────────────────

    [Fact]
    public void Detect_UnrelatedIds_ReturnsEmpty()
    {
        // Two IDs that share no known conflict pair.
        var conflicts = ConflictDetector.Detect(["explorer-show-hidden-files", "audio-disable-sound-scheme"]);
        Assert.Empty(conflicts);
    }

    // ── ConflictsFor ──────────────────────────────────────────────────────

    [Fact]
    public void ConflictsFor_IdWithConflictInApplied_ReturnsOneResult()
    {
        var result = ConflictDetector.ConflictsFor(IdA, [IdB, "some-other-tweak"]);
        Assert.Single(result);
    }

    [Fact]
    public void ConflictsFor_IdWithNoConflictInApplied_ReturnsEmpty()
    {
        var result = ConflictDetector.ConflictsFor(IdA, ["explorer-show-hidden-files"]);
        Assert.Empty(result);
    }

    [Fact]
    public void ConflictsFor_EmptyApplied_ReturnsEmpty()
    {
        var result = ConflictDetector.ConflictsFor(IdA, []);
        Assert.Empty(result);
    }

    [Fact]
    public void ConflictsFor_IdNotInAnyConflict_AlwaysReturnsEmpty()
    {
        // An ID that is not part of any known pair.
        var result = ConflictDetector.ConflictsFor("explorer-show-hidden-files", [IdA, IdB, "sac-disable-virtualization-based-security"]);
        Assert.Empty(result);
    }

    // ── Reason is descriptive ─────────────────────────────────────────────

    [Fact]
    public void Detect_KnownPair_ReasonIsNonTrivial()
    {
        var conflict = ConflictDetector.Detect([IdA, IdB]).Single();
        Assert.True(conflict.Reason.Length >= 10, $"Conflict reason '{conflict.Reason}' is too short to be descriptive.");
    }
}
