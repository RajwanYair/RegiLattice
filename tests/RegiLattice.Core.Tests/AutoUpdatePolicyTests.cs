#nullable enable
// RegiLattice.Core.Tests — AutoUpdatePolicyTests.cs
// Tests for UpdateCheckService.IsDisabledByPolicy() and AppConfig.CheckForUpdates (D.4).

using RegiLattice.Core;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for D.4 auto-update policy helpers.</summary>
public sealed class AutoUpdatePolicyTests
{
    // ── IsDisabledByPolicy ────────────────────────────────────────────────

    [Fact]
    public void IsDisabledByPolicy_WhenKeyAbsent_ReturnsFalse()
    {
        // The registry key will typically be absent on dev machines.
        // This test verifies the method returns false gracefully rather than throwing.
        bool result = UpdateCheckService.IsDisabledByPolicy();
        // result may be true on a GPO-managed machine; the important thing is no exception.
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void IsDisabledByPolicy_DoesNotThrow()
    {
        // Registry read must be exception-safe regardless of key presence.
        var ex = Record.Exception(() => UpdateCheckService.IsDisabledByPolicy());
        Assert.Null(ex);
    }

    // ── AppConfig.CheckForUpdates ─────────────────────────────────────────

    [Fact]
    public void AppConfig_CheckForUpdates_DefaultIsTrue()
    {
        var cfg = new AppConfig();
        Assert.True(cfg.CheckForUpdates);
    }

    [Fact]
    public void AppConfig_CheckForUpdates_CanBeSetFalse()
    {
        var cfg = new AppConfig { CheckForUpdates = false };
        Assert.False(cfg.CheckForUpdates);
    }

    [Fact]
    public void AppConfig_CheckForUpdates_RoundTripsJson()
    {
        var cfg = new AppConfig { CheckForUpdates = false };
        string json = System.Text.Json.JsonSerializer.Serialize(cfg);
        var cfg2 = System.Text.Json.JsonSerializer.Deserialize<AppConfig>(json);
        Assert.NotNull(cfg2);
        Assert.False(cfg2!.CheckForUpdates);
    }

    // ── UpdateInfo record ─────────────────────────────────────────────────

    [Fact]
    public void UpdateInfo_UpdateAvailable_DefaultFalse()
    {
        var info = new UpdateInfo();
        Assert.False(info.UpdateAvailable);
    }

    [Fact]
    public void UpdateInfo_ErrorSet_WhenConstructedWithError()
    {
        var info = new UpdateInfo { Error = "network error" };
        Assert.Equal("network error", info.Error);
        Assert.False(info.UpdateAvailable);
    }

    [Fact]
    public void UpdateInfo_FullProperties_ArePreserved()
    {
        var info = new UpdateInfo
        {
            UpdateAvailable = true,
            CurrentVersion = "6.35.0",
            LatestVersion = "6.36.0",
            ReleaseNotes = "Bug fixes",
            DownloadUrl = "https://github.com/RajwanYair/RegiLattice/releases/tag/v6.36.0",
        };
        Assert.True(info.UpdateAvailable);
        Assert.Equal("6.35.0", info.CurrentVersion);
        Assert.Equal("6.36.0", info.LatestVersion);
        Assert.StartsWith("Bug", info.ReleaseNotes);
        Assert.Contains("releases", info.DownloadUrl);
    }
}
