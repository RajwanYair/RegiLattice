// tests/RegiLattice.Core.Tests/AutoUpdaterTests.cs
// Sprint 61 — AutoUpdater.IsNewer() semantic version comparison tests.

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 61: AutoUpdater semantic version comparison.</summary>
public sealed class AutoUpdaterTests
{
    // ── IsNewer — true cases ──────────────────────────────────────────────

    [Theory]
    [InlineData("4.1.0", "4.0.0")]
    [InlineData("4.0.1", "4.0.0")]
    [InlineData("5.0.0", "4.9.9")]
    [InlineData("4.0.0", "3.99.99")]
    [InlineData("10.0.0", "9.9.9")]
    public void IsNewer_CandidateNewer_ReturnsTrue(string candidate, string current)
    {
        Assert.True(AutoUpdater.IsNewer(candidate, current));
    }

    // ── IsNewer — false cases ─────────────────────────────────────────────

    [Theory]
    [InlineData("4.0.0", "4.0.0")] // equal
    [InlineData("3.9.0", "4.0.0")] // older
    [InlineData("4.0.0", "4.0.1")] // older patch
    [InlineData("0.0.1", "1.0.0")] // much older
    public void IsNewer_CandidateNotNewer_ReturnsFalse(string candidate, string current)
    {
        Assert.False(AutoUpdater.IsNewer(candidate, current));
    }

    // ── IsNewer — v-prefix tolerance ─────────────────────────────────────

    [Theory]
    [InlineData("v4.1.0", "4.0.0")]
    [InlineData("v4.1.0", "v4.0.0")]
    [InlineData("4.1.0", "v4.0.0")]
    public void IsNewer_VPrefix_HandledCorrectly(string candidate, string current)
    {
        Assert.True(AutoUpdater.IsNewer(candidate, current));
    }

    // ── IsNewer — malformed input ─────────────────────────────────────────

    [Theory]
    [InlineData("not-a-version", "4.0.0")]
    [InlineData("4.0.0", "not-a-version")]
    [InlineData("", "4.0.0")]
    [InlineData("4.0.0", "")]
    public void IsNewer_MalformedVersion_ReturnsFalse(string candidate, string current)
    {
        // Malformed input should never throw; result is false
        Assert.False(AutoUpdater.IsNewer(candidate, current));
    }
}
