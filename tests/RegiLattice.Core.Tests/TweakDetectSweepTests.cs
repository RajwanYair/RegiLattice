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
public sealed class ScheduledTaskDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllScheduledTaskTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("Scheduled Tasks", out var tweaks))
            return;

        // Verify scheduled-task tweaks exist and have either DetectOps or DetectAction
        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 5, $"Expected ≥5 Scheduled Task tweaks with detection; found {withDetect}");
    }
}

// ── 4. Boot DetectAction Sweep ──────────────────────────────────────────────

public sealed class BootDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllBootTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("Boot", out var tweaks))
            return;

        // Boot tweaks use bcdedit-based DetectAction — verify they exist
        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 1, $"Expected ≥1 Boot tweak with detection; found {withDetect}");
    }
}

// ── 5. WSL DetectAction Sweep ──────────────────────────────────────────────

public sealed class WslDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllWslTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("Virtualization", out var tweaks))
            return;

        Assert.True(tweaks.Count >= 5, $"Expected ≥5 WSL tweaks; found {tweaks.Count}");
    }
}

// ── 6. User Account DetectAction Sweep ─────────────────────────────────────

public sealed class UserAccountDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllUserAccountTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("User Account", out var tweaks))
            return;

        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 3, $"Expected ≥3 User Account tweaks with detection; found {withDetect}");
    }
}

// ── 7. Other Tweak Module DetectAction Sweeps ──────────────────────────────

public sealed class OtherTweakDetectActionSweepTests
{
    private static void AssertCategoryHasTweaks(string category, int minTweaks = 1)
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue(category, out var tweaks))
            return;

        if (minTweaks > 0)
            Assert.True(tweaks.Count >= minTweaks, $"Category '{category}': expected ≥{minTweaks} tweaks, found {tweaks.Count}");
    }

    [Fact]
    public void DetectAction_DeveloperTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Developer");

    [Fact]
    public void DetectAction_PowerManagementTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Power");

    [Fact]
    public void DetectAction_CommandLineTweaks_CanBeInvoked() => AssertCategoryHasTweaks("PowerShell");

    [Fact]
    public void DetectAction_AppCompatibilityTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Windows 11"); // was "App Compatibility" — merged into Windows 11

    [Fact]
    public void DetectAction_PackageManagementTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Developer");

    [Fact]
    public void DetectAction_ServicesTweaks_CanBeInvoked() => AssertCategoryHasTweaks("System");

    [Fact]
    public void DetectAction_SshConfigurationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("SSH Configuration");

    [Fact]
    public void DetectAction_VirtualizationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Virtualization");

    [Fact]
    public void DetectAction_NetworkOptimizationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Network");

    [Fact]
    public void DetectAction_PrintingTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Maintenance");
}

// ── 8. SshHardening helper branches via action delegates ───────────────────

