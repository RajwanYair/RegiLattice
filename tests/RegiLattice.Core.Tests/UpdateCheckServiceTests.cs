// tests/RegiLattice.Core.Tests/UpdateCheckServiceTests.cs
// Sprint coverage — UpdateCheckService.CompareVersions pure-logic tests + UpdateInfo record.

using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for UpdateCheckService: version comparison, CurrentVersion, UpdateInfo record.</summary>
public sealed class UpdateCheckServiceTests
{
    // ── CompareVersions — newer first ────────────────────────────────────────

    [Theory]
    [InlineData("3.5.1", "3.5.0")]
    [InlineData("4.0.0", "3.9.9")]
    [InlineData("10.0.0", "9.9.9")]
    [InlineData("1.0.1", "1.0.0")]
    [InlineData("2.0.0", "1.99.99")]
    public void CompareVersions_NewerFirst_ReturnsPositive(string a, string b)
    {
        Assert.True(UpdateCheckService.CompareVersions(a, b) > 0);
    }

    // ── CompareVersions — equal ───────────────────────────────────────────────

    [Theory]
    [InlineData("3.5.0", "3.5.0")]
    [InlineData("0.0.0", "0.0.0")]
    [InlineData("1.0.0", "1.0.0")]
    public void CompareVersions_SameVersions_ReturnsZero(string a, string b)
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions(a, b));
    }

    // ── CompareVersions — older first ────────────────────────────────────────

    [Theory]
    [InlineData("3.4.9", "3.5.0")]
    [InlineData("3.5.0", "4.0.0")]
    [InlineData("0.9.9", "1.0.0")]
    [InlineData("1.0.0", "1.0.1")]
    public void CompareVersions_OlderFirst_ReturnsNegative(string a, string b)
    {
        Assert.True(UpdateCheckService.CompareVersions(a, b) < 0);
    }

    // ── CompareVersions — invalid inputs map to 0.0.0 ───────────────────────

    [Fact]
    public void CompareVersions_InvalidA_TreatedAsZero()
    {
        // "not-a-version" parses as 0.0.0 which is less than "1.0.0"
        Assert.True(UpdateCheckService.CompareVersions("not-a-version", "1.0.0") < 0);
    }

    [Fact]
    public void CompareVersions_BothInvalid_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("abc", "xyz"));
    }

    [Fact]
    public void CompareVersions_EmptyStrings_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("", ""));
    }

    // ── CurrentVersion ────────────────────────────────────────────────────────

    [Fact]
    public void CurrentVersion_IsNonEmpty()
    {
        Assert.NotEmpty(UpdateCheckService.CurrentVersion);
    }

    [Fact]
    public void CurrentVersion_IsValidSemVer()
    {
        string v = UpdateCheckService.CurrentVersion;
        Assert.True(Version.TryParse(v, out _), $"'{v}' is not a parseable version string");
    }

    [Fact]
    public void CurrentVersion_DoesNotStartWithV()
    {
        // Version strings must not have a 'v' prefix (consumers call CompareVersions directly)
        Assert.False(UpdateCheckService.CurrentVersion.StartsWith('v'));
    }

    // ── UpdateInfo record ─────────────────────────────────────────────────────

    [Fact]
    public void UpdateInfo_DefaultValues_AreExpected()
    {
        var info = new UpdateInfo();
        Assert.False(info.UpdateAvailable);
        Assert.Equal("", info.CurrentVersion);
        Assert.Equal("", info.LatestVersion);
        Assert.Equal("", info.ReleaseNotes);
        Assert.Equal("", info.DownloadUrl);
        Assert.Null(info.PublishedAt);
        Assert.Null(info.Error);
    }

    [Fact]
    public void UpdateInfo_WithAllFields_InitializesCorrectly()
    {
        var published = new DateTime(2025, 6, 1, 0, 0, 0, DateTimeKind.Utc);
        var info = new UpdateInfo
        {
            UpdateAvailable = true,
            CurrentVersion = "3.5.0",
            LatestVersion = "3.6.0",
            ReleaseNotes = "New features added",
            DownloadUrl = "https://github.com/RajwanYair/RegiLattice/releases",
            PublishedAt = published,
        };

        Assert.True(info.UpdateAvailable);
        Assert.Equal("3.5.0", info.CurrentVersion);
        Assert.Equal("3.6.0", info.LatestVersion);
        Assert.Equal("New features added", info.ReleaseNotes);
        Assert.NotEmpty(info.DownloadUrl);
        Assert.Equal(published, info.PublishedAt);
        Assert.Null(info.Error);
    }

    [Fact]
    public void UpdateInfo_WithError_HasNullUpdateAvailable()
    {
        var info = new UpdateInfo { CurrentVersion = "3.5.0", Error = "Network timeout" };

        Assert.False(info.UpdateAvailable);
        Assert.Equal("Network timeout", info.Error);
        Assert.Equal("", info.LatestVersion);
    }
}
