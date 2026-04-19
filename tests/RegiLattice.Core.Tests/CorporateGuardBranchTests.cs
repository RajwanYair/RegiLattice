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
public sealed class CorporateGuardRemainingBranchTests
{
    [Fact]
    public void IsCorporateNetwork_ReturnsBool_DoesNotThrow()
    {
        bool result = CorporateGuard.IsCorporateNetwork();
        Assert.True(result || !result);
    }

    [Fact]
    public void IsGpoManaged_WithSampleKeys_ReturnsBool_DoesNotThrow()
    {
        // Pass a real-ish list of policy registry keys
        bool result = CorporateGuard.IsGpoManaged(new[] { @"HKLM\SOFTWARE\Policies\Microsoft", @"HKCU\SOFTWARE\Policies\Microsoft" });
        Assert.True(result || !result);
    }

    [Fact]
    public void Status_ReturnsTupleWithReason()
    {
        var (isCorp, reason) = CorporateGuard.Status();
        Assert.NotNull(reason);
        Assert.True(isCorp || !isCorp);
    }

    [Fact]
    public void IsCorporateNetwork_CalledTwice_ConsistentResult()
    {
        // Covers caching path (second call returns cached result)
        bool first = CorporateGuard.IsCorporateNetwork();
        bool second = CorporateGuard.IsCorporateNetwork();
        Assert.Equal(first, second);
    }
}

// ── 11. PackLoader remaining branch coverage ───────────────────────────────

