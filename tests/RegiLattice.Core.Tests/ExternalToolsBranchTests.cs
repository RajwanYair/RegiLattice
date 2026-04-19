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
public sealed class ChocolateyManagerValidationBranchTests
{
    [Theory]
    [InlineData("googlechrome")]
    [InlineData("7zip")]
    [InlineData("my.package_v2")]
    [InlineData("some-pkg-name")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = ChocolateyManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName(""));
    }

    [Fact]
    public void ValidateName_WhitespaceName_Throws()
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName("   "));
    }

    [Theory]
    [InlineData("name with spaces")]
    [InlineData("pkg/slash")]
    [InlineData("pkg!name")]
    [InlineData("pkg@name")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => ChocolateyManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// PipManager.ValidateName — SafeNameRegex runner branches (second instance)
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class PipManagerValidationBranchTests
{
    [Theory]
    [InlineData("requests")]
    [InlineData("numpy.scipy")]
    [InlineData("my-package_1")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = PipManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => PipManager.ValidateName(""));
    }

    [Theory]
    [InlineData("pkg with space")]
    [InlineData("pkg;injection")]
    [InlineData("pip&&rm")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => PipManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// WinGetManager.ValidateName — SafeNameRegex runner branches (third instance)
// ═══════════════════════════════════════════════════════════════════════════════

public sealed class WinGetManagerValidationBranchTests
{
    [Theory]
    [InlineData("Microsoft.VSCode")]
    [InlineData("Git.Git")]
    [InlineData("Python.Python.3.12")]
    [InlineData("7zip.7zip")]
    public void ValidateName_ValidName_DoesNotThrow(string name)
    {
        var result = WinGetManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Fact]
    public void ValidateName_EmptyName_Throws()
    {
        Assert.Throws<ArgumentException>(() => WinGetManager.ValidateName(""));
    }

    [Theory]
    [InlineData("Package Name With Spaces")]
    [InlineData("pkg|pipe")]
    [InlineData("cmd>redirect")]
    public void ValidateName_InvalidName_Throws(string name)
    {
        Assert.Throws<ArgumentException>(() => WinGetManager.ValidateName(name));
    }
}

// ═══════════════════════════════════════════════════════════════════════════════
// UpdateCheckService.CompareVersions — all comparison branches
// ═══════════════════════════════════════════════════════════════════════════════

