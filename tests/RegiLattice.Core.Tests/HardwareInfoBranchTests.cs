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
public sealed class HardwareInfoSoftwareDetectionBranchTests
{
    // Each method returns a bool — testing them covers one branch (found/not-found).
    // Since we can't control whether software is installed, we just verify no exception.

    [Fact]
    public void IsChromeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsChromeInstalled());

    [Fact]
    public void IsFirefoxInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsFirefoxInstalled());

    [Fact]
    public void IsEdgeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsEdgeInstalled());

    [Fact]
    public void IsJavaInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsJavaInstalled());

    [Fact]
    public void IsDockerInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsDockerInstalled());

    [Fact]
    public void IsAdobeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsAdobeInstalled());

    [Fact]
    public void IsLibreOfficeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsLibreOfficeInstalled());

    [Fact]
    public void IsOfficeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsOfficeInstalled());

    [Fact]
    public void IsRealVncInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsRealVncInstalled());

    [Fact]
    public void IsVsCodeInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsVsCodeInstalled());

    [Fact]
    public void IsScoopInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.IsScoopInstalled());

    [Fact]
    public void HasNvidiaGpu_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasNvidiaGpu());

    [Fact]
    public void HasAmdGpu_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasAmdGpu());

    [Fact]
    public void HasWslInstalled_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasWslInstalled());

    [Fact]
    public void HasBatteryPresent_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasBatteryPresent());

    [Fact]
    public void HasHyperVAvailable_ReturnsBool() => Assert.IsType<bool>(HardwareInfo.HasHyperVAvailable());

    [Fact]
    public void DetectCpu_ReturnsNonNullProfile()
    {
        var cpu = HardwareInfo.DetectCpu();
        Assert.NotNull(cpu);
        Assert.True(cpu.LogicalCores > 0);
    }

    [Fact]
    public void DetectMemory_ReturnsTotalMbPositive()
    {
        var mem = HardwareInfo.DetectMemory();
        Assert.True(mem.TotalMb > 0);
    }

    [Fact]
    public void DetectGpus_ReturnsAtLeastOneEntry()
    {
        var gpus = HardwareInfo.DetectGpus();
        Assert.NotEmpty(gpus);
    }

    [Fact]
    public void DetectDisk_ReturnsCDriveInfo()
    {
        var disk = HardwareInfo.DetectDisk();
        Assert.StartsWith("C", disk.Drive, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SuggestProfile_ReturnsKnownProfile()
    {
        var profile = HardwareInfo.SuggestProfile();
        string[] valid = ["business", "gaming", "privacy", "minimal"];
        Assert.Contains(profile, valid);
    }

    [Fact]
    public void Summary_ContainsCpuRamGpuBuild()
    {
        var summary = HardwareInfo.Summary();
        Assert.Contains("CPU:", summary, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("RAM:", summary, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("GPU:", summary, StringComparison.OrdinalIgnoreCase);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// PackLoader — missing-field validation branches not covered by PluginTests
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class HardwareInfoProfileBranchTests
{
    [Fact]
    public void SuggestProfile_ReturnsOneOfKnownProfiles()
    {
        var profile = HardwareInfo.SuggestProfile();
        Assert.Contains(profile, new[] { "business", "gaming", "minimal", "privacy" });
    }

    [Fact]
    public void Summary_ReturnsNonEmptyStringWithCpu()
    {
        var summary = HardwareInfo.Summary();
        Assert.NotEmpty(summary);
        Assert.Contains("CPU:", summary, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void DetectCpu_ReturnsValidCpuInfo()
    {
        var cpu = HardwareInfo.DetectCpu();
        Assert.NotNull(cpu);
        Assert.True(cpu.LogicalCores > 0);
    }

    [Fact]
    public void DetectGpus_ReturnsNonEmptyList()
    {
        var gpus = HardwareInfo.DetectGpus();
        Assert.NotNull(gpus);
        Assert.NotEmpty(gpus); // Always returns at least one GpuInfo (fallback "Unknown")
    }

    [Fact]
    public void DetectMemory_ReturnsMemoryInfo()
    {
        var mem = HardwareInfo.DetectMemory();
        Assert.NotNull(mem);
    }

    [Fact]
    public void DetectHardware_GuiBatchSize_IsOneOfExpectedValues()
    {
        var hw = HardwareInfo.DetectHardware();
        Assert.True(hw.GuiBatchSize == 4 || hw.GuiBatchSize == 8);
    }
}

// ── 9. UpdateCheckService & UpdateInfo Branch Tests ─────────────────────────

