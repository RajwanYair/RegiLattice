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
public sealed class NetworkManagerBranchTests
{
    [Fact]
    public void GetActiveAdapterNames_ReturnsListOrEmpty()
    {
        var names = NetworkManager.GetActiveAdapterNames();
        Assert.NotNull(names); // may be empty if no adapters, but non-null
    }

    [Fact]
    public void GetNetworkInterfaceStats_ReturnsListOrEmpty()
    {
        var stats = NetworkManager.GetNetworkInterfaceStats();
        Assert.NotNull(stats);
    }

    [Fact]
    public void GetCurrentDns_NonExistentAdapter_ReturnsBothEmpty()
    {
        var (primary, secondary) = NetworkManager.GetCurrentDns("NONEXISTENT_ADAPTER_XYZ_999");
        Assert.Equal("", primary);
        Assert.Equal("", secondary);
    }

    [Fact]
    public async Task PingAsync_NullHost_ThrowsArgumentException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(() => NetworkManager.PingAsync(null!));
    }

    [Fact]
    public async Task PingAsync_EmptyHost_ThrowsArgumentException()
    {
        await Assert.ThrowsAnyAsync<ArgumentException>(() => NetworkManager.PingAsync(""));
    }

    [Fact]
    public async Task PingAsync_CountZero_ThrowsArgumentOutOfRangeException()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => NetworkManager.PingAsync("localhost", 0));
    }

    [Fact]
    public async Task PingAsync_Count101_ThrowsArgumentOutOfRangeException()
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => NetworkManager.PingAsync("localhost", 101));
    }

    [Fact]
    public void DnsPreset_BuiltIn_ContainsAtLeastOneEntry()
    {
        Assert.NotEmpty(DnsPreset.BuiltIn);
    }

    [Fact]
    public void DnsPreset_Cloudflare_HasCorrectIp()
    {
        var cf = DnsPreset.BuiltIn.First(p => p.Name.StartsWith("Cloudflare", StringComparison.OrdinalIgnoreCase));
        Assert.Equal("1.1.1.1", cf.Primary);
    }

    [Fact]
    public void PingResult_LossPercent_WhenPartialLoss_IsCorrect()
    {
        var pr = new PingResult("test.host", Sent: 4, Received: 3, Lost: 1, AverageMs: 10, MinMs: 8, MaxMs: 12);
        Assert.Equal(25.0, pr.LossPercent, precision: 5);
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// HardwareInfo — software detection method branches
// ═══════════════════════════════════════════════════════════════════════════════

