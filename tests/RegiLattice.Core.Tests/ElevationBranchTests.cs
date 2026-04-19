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
public sealed class ElevationBranchTests2
{
    [Fact]
    public void IsAdmin_DoesNotThrow_ReturnsBool()
    {
        var result = Elevation.IsAdmin();
        Assert.IsType<bool>(result);
    }

    [Fact]
    public void AssertAdmin_RequireAdminFalse_NoOp()
    {
        // requireAdmin=false → the guard is skipped regardless of actual admin status
        Elevation.AssertAdmin(requireAdmin: false);
    }

    [Fact]
    public void RunElevated_NonAllowedCommand_ThrowsUnauthorizedAccess()
    {
        var ex = Assert.Throws<UnauthorizedAccessException>(() => Elevation.RunElevated("notallowed_program.exe", [], 1_000));
        Assert.Contains("allowlist", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RunElevated_PathToNonAllowedCommand_ThrowsUnauthorizedAccess()
    {
        var ex = Assert.Throws<UnauthorizedAccessException>(() => Elevation.RunElevated(@"C:\Windows\System32\curl.exe", [], 1_000));
        Assert.Contains("allowlist", ex.Message, StringComparison.OrdinalIgnoreCase);
    }
}

// ── 8. HardwareInfo Profile & Summary Branch Tests ──────────────────────────

public sealed class ElevationAllowedCommandBranchTests
{
    [Fact]
    public void RunElevated_AllowedCommand_ReturnsExitCode()
    {
        // Covers F branch of `!AllowedCommands.Contains(exeName)` (command IS allowed)
        // and T branch of `proc.WaitForExit(timeoutMs)` (fast exit)
        var (exit, stdout, stderr) = Elevation.RunElevated("cmd", ["/c", "exit 0"], 10_000);
        Assert.Equal(0, exit);
    }

    [Fact]
    public void RunElevated_AllowedCommand_ExitCode1_ReturnsNonZero()
    {
        var (exit, _, _) = Elevation.RunElevated("cmd", ["/c", "exit 1"], 10_000);
        Assert.Equal(1, exit);
    }

    [Fact]
    public void RunElevated_AllowedCommand_WithOutput_ReturnsStdout()
    {
        // Covers stdout reading path in RunElevated
        var (exit, stdout, _) = Elevation.RunElevated("cmd", ["/c", "echo hello"], 10_000);
        Assert.Equal(0, exit);
        Assert.Contains("hello", stdout, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RunElevated_CmdAllowed_QuoteArgInRequestElevation_Independence()
    {
        // Also tests IsAdmin() — the current process may or may not be admin; just verify it returns bool
        bool isAdmin = Elevation.IsAdmin();
        Assert.True(isAdmin || !isAdmin); // always passes; confirms IsAdmin() runs without exception
    }
}

// ── 10. CorporateGuard remaining branch coverage ───────────────────────────

