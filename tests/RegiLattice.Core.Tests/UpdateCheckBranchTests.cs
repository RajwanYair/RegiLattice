#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
public sealed class UpdateCheckServiceBranchTests
{
    [Fact]
    public void CompareVersions_AGreaterThanB_ReturnsPositive()
    {
        int result = UpdateCheckService.CompareVersions("2.0.0", "1.9.9");
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareVersions_ALessThanB_ReturnsNegative()
    {
        int result = UpdateCheckService.CompareVersions("1.0.0", "2.0.0");
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareVersions_Equal_ReturnsZero()
    {
        int result = UpdateCheckService.CompareVersions("3.5.0", "3.5.0");
        Assert.Equal(0, result);
    }

    [Fact]
    public void CompareVersions_UnparseableA_TreatsAsZero()
    {
        // Unparseable "a" → 0.0.0, parseable "1.0.0" is bigger
        int result = UpdateCheckService.CompareVersions("not-a-version", "1.0.0");
        Assert.True(result < 0);
    }

    [Fact]
    public void CompareVersions_UnparseableB_TreatsAsZero()
    {
        int result = UpdateCheckService.CompareVersions("1.0.0", "not-a-version");
        Assert.True(result > 0);
    }

    [Fact]
    public void CompareVersions_BothUnparseable_ReturnsZero()
    {
        int result = UpdateCheckService.CompareVersions("bad", "also-bad");
        Assert.Equal(0, result);
    }

    [Fact]
    public void CurrentVersion_ReturnsNonEmptyString()
    {
        var version = UpdateCheckService.CurrentVersion;
        Assert.False(string.IsNullOrWhiteSpace(version));
        // Should be in X.Y.Z format or "0.0.0"
        Assert.Matches(@"^\d+\.\d+\.\d+$", version);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// RegistrySession — CheckValueMatch type branches
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class UpdateCheckServiceBranchTests2
{
    [Fact]
    public void CurrentVersion_ReturnsNonEmptyString()
    {
        var v = UpdateCheckService.CurrentVersion;
        Assert.NotEmpty(v);
        // Should look like "X.Y.Z"
        Assert.Matches(@"^\d+\.\d+\.\d+", v);
    }

    [Fact]
    public void UpdateInfo_DefaultRecord_HasExpectedDefaults()
    {
        var info = new UpdateInfo();
        Assert.False(info.UpdateAvailable);
        Assert.Equal("", info.CurrentVersion);
        Assert.Equal("", info.LatestVersion);
        Assert.Null(info.Error);
        Assert.Null(info.PublishedAt);
    }

    [Fact]
    public void UpdateInfo_WithAllProperties_StoresCorrectly()
    {
        var now = DateTime.UtcNow;
        var info = new UpdateInfo
        {
            UpdateAvailable = true,
            CurrentVersion = "3.0.0",
            LatestVersion = "4.0.0",
            ReleaseNotes = "Big update",
            DownloadUrl = "https://example.com/download",
            PublishedAt = now,
            Error = null,
        };
        Assert.True(info.UpdateAvailable);
        Assert.Equal("3.0.0", info.CurrentVersion);
        Assert.Equal("4.0.0", info.LatestVersion);
        Assert.Equal(now, info.PublishedAt);
    }

    [Fact]
    public void UpdateInfo_WithError_ErrorIsSet()
    {
        var info = new UpdateInfo { Error = "Network timeout", CurrentVersion = "1.0.0" };
        Assert.NotNull(info.Error);
        Assert.Equal("Network timeout", info.Error);
    }
}

// ── 10. PackConflict & PackDef Record Branch Tests ──────────────────────────

public sealed class GitHubReleaseJsonTests
{
    [Fact]
    public void Deserialize_AllFieldsPresent_PopulatesAllProperties()
    {
        const string json = """
            {"tag_name":"v4.5.0","body":"release notes text","html_url":"https://github.com/RajwanYair/RegiLattice","published_at":"2025-01-15T12:00:00Z"}
            """;
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v4.5.0", r.TagName);
        Assert.Equal("release notes text", r.Body);
        Assert.NotNull(r.HtmlUrl);
        Assert.NotNull(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_EmptyObject_AllPropertiesNull()
    {
        var r = JsonSerializer.Deserialize("{}", UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.TagName);
        Assert.Null(r.Body);
        Assert.Null(r.HtmlUrl);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_TagNameOnly_OtherFieldsNull()
    {
        const string json = """{"tag_name":"v1.0.0"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v1.0.0", r.TagName);
        Assert.Null(r.Body);
        Assert.Null(r.HtmlUrl);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_NullTagName_TagNameIsNull()
    {
        const string json = """{"tag_name":null,"body":"notes","html_url":"https://x.com"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.TagName);
        Assert.Equal("notes", r.Body);
    }

    [Fact]
    public void Deserialize_NullBody_BodyIsNull()
    {
        const string json = """{"tag_name":"v2.0.0","body":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v2.0.0", r.TagName);
        Assert.Null(r.Body);
    }

    [Fact]
    public void Deserialize_PublishedAtDate_ParsedCorrectly()
    {
        const string json = """{"tag_name":"v3.0.0","published_at":"2024-06-15T00:00:00Z"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.NotNull(r.PublishedAt);
        Assert.Equal(2024, r.PublishedAt!.Value.Year);
    }

    [Fact]
    public void Deserialize_NullPublishedAt_PublishedAtIsNull()
    {
        const string json = """{"tag_name":"v3.0.0","published_at":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_ExtraUnknownFields_Ignored()
    {
        const string json = """{"tag_name":"v4.0.0","unknown_field":"ignored","prerelease":false}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v4.0.0", r.TagName);
    }

    [Fact]
    public void Deserialize_HtmlUrl_Populated()
    {
        const string json = """{"html_url":"https://github.com/RajwanYair/RegiLattice/releases/tag/v5.0.0"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.NotNull(r.HtmlUrl);
        Assert.Contains("v5.0.0", r.HtmlUrl);
    }

    [Fact]
    public void Deserialize_NullHtmlUrl_HtmlUrlIsNull()
    {
        const string json = """{"tag_name":"v4.0.0","html_url":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.HtmlUrl);
    }
}

// ── 2. CompareVersions + CurrentVersion ────────────────────────────────────

public sealed class CompareVersionsBranchTests
{
    [Fact]
    public void CompareVersions_AGreaterThanB_ReturnsPositive()
    {
        Assert.True(UpdateCheckService.CompareVersions("4.5.0", "4.4.0") > 0);
    }

    [Fact]
    public void CompareVersions_ALessThanB_ReturnsNegative()
    {
        Assert.True(UpdateCheckService.CompareVersions("4.3.0", "4.4.0") < 0);
    }

    [Fact]
    public void CompareVersions_AEqualsB_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("4.4.0", "4.4.0"));
    }

    [Fact]
    public void CompareVersions_InvalidFirstVersion_TreatedAsZeroZeroZero()
    {
        // TryParse(a) fails → va = 0.0.0; b = 1.0.0 → result < 0
        Assert.True(UpdateCheckService.CompareVersions("not-a-version", "1.0.0") < 0);
    }

    [Fact]
    public void CompareVersions_InvalidSecondVersion_TreatedAsZeroZeroZero()
    {
        // TryParse(b) fails → vb = 0.0.0; a = 1.0.0 → result > 0
        Assert.True(UpdateCheckService.CompareVersions("1.0.0", "not-a-version") > 0);
    }

    [Fact]
    public void CompareVersions_BothInvalid_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("invalid", "bad"));
    }

    [Fact]
    public void CompareVersions_EmptyString_TreatedAsZeroZeroZero()
    {
        Assert.True(UpdateCheckService.CompareVersions("", "1.0.0") < 0);
    }

    [Fact]
    public void CurrentVersion_ReturnsNonEmptyVersionFormat()
    {
        var v = UpdateCheckService.CurrentVersion;
        Assert.NotEmpty(v);
        Assert.Matches(@"^\d+\.\d+\.\d+$", v);
    }
}

// ── 3. Scheduled Task DetectAction Sweep ───────────────────────────────────
// NOTE: We verify structure only (non-null DetectAction), NOT invoke it.
// Invoking schtasks.exe/PowerShell per tweak takes 2-5 s each and hangs suites.

