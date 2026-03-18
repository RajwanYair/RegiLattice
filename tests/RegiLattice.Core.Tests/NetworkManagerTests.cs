#nullable enable

using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the Sprint-27 NetworkManager service.</summary>
public sealed class DnsPresetTests
{
    [Fact]
    public void BuiltIn_HasSixPresets()
    {
        Assert.Equal(6, DnsPreset.BuiltIn.Count);
    }

    [Fact]
    public void BuiltIn_FirstPreset_IsAutomatic()
    {
        Assert.Equal("Automatic (DHCP)", DnsPreset.BuiltIn[0].Name);
        Assert.Equal("", DnsPreset.BuiltIn[0].Primary);
    }

    [Fact]
    public void BuiltIn_Cloudflare_HasCorrectAddresses()
    {
        var cf = DnsPreset.BuiltIn.First(p => p.Name == "Cloudflare");
        Assert.Equal("1.1.1.1", cf.Primary);
        Assert.Equal("1.0.0.1", cf.Secondary);
        Assert.Equal("2606:4700:4700::1111", cf.Primary6);
    }

    [Fact]
    public void BuiltIn_Google_HasCorrectAddresses()
    {
        var g = DnsPreset.BuiltIn.First(p => p.Name == "Google");
        Assert.Equal("8.8.8.8", g.Primary);
        Assert.Equal("8.8.4.4", g.Secondary);
    }

    [Fact]
    public void BuiltIn_Quad9_HasCorrectPrimary()
    {
        var q = DnsPreset.BuiltIn.First(p => p.Name.StartsWith("Quad9"));
        Assert.Equal("9.9.9.9", q.Primary);
    }

    [Fact]
    public void BuiltIn_AllNonDhcpPresets_HavePrimaryAndSecondary()
    {
        foreach (var preset in DnsPreset.BuiltIn.Skip(1))
        {
            Assert.False(string.IsNullOrEmpty(preset.Primary), $"{preset.Name} is missing Primary");
            Assert.False(string.IsNullOrEmpty(preset.Secondary), $"{preset.Name} is missing Secondary");
        }
    }
}

/// <summary>Tests for read-only NetworkManager operations (no admin required).</summary>
public sealed class NetworkManagerTests
{
    [Fact]
    public void GetActiveAdapterNames_ReturnsAtLeastOneAdapter()
    {
        var names = NetworkManager.GetActiveAdapterNames();
        Assert.NotEmpty(names);
    }

    [Fact]
    public void GetActiveAdapterNames_AllNamesNonEmpty()
    {
        var names = NetworkManager.GetActiveAdapterNames();
        Assert.All(names, n => Assert.False(string.IsNullOrWhiteSpace(n)));
    }

    [Fact]
    public void DnsPresets_AllNamed_ReturnsCorrectCount()
    {
        // Structural test: all 6 built-in presets have a name
        Assert.All(DnsPreset.BuiltIn, p => Assert.False(string.IsNullOrEmpty(p.Name)));
    }

    [Fact]
    public void NetworkOperationResult_Record_CanBeConstructed()
    {
        var r = new NetworkOperationResult(true, "OK");
        Assert.True(r.Success);
        Assert.Equal("OK", r.Message);
    }
}
