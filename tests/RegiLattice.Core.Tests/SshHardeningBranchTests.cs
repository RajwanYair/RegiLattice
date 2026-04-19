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
public sealed class SshHardeningBranchTests2
{
    // sshd_config does NOT exist on this machine → exercises the early-return paths
    // in SetSshdDirective / RemoveSshdDirective / DetectSshdDirective.

    private static (TweakDef? tweak, TweakEngine engine) GetSshTweak()
    {
        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var tweak = engine.AllTweaks().FirstOrDefault(t => t.Category == "SSH Configuration");
        return (tweak, engine);
    }

    [Fact]
    public void ApplyAction_DryRun_True_ReturnsImmediately_NoException()
    {
        // Calls SetSshdDirective(directive, value, dryRun: true) → hits `if (dryRun) return;` branch
        var (td, _) = GetSshTweak();
        if (td?.ApplyAction is null)
            return; // SSH not registered, skip

        // dryRun=true → SetSshdDirective returns immediately, no file access
        td.ApplyAction(true);
    }

    [Fact]
    public void ApplyAction_DryRun_False_NoFile_ReturnsImmediately_NoException()
    {
        // Calls SetSshdDirective(directive, value, dryRun: false)
        // → `if (dryRun) return;` F-branch → `if (!File.Exists(SshdConfig)) return;` T-branch
        var (td, _) = GetSshTweak();
        if (td?.ApplyAction is null)
            return;

        td.ApplyAction(false); // file doesn't exist → returns without modifying anything
    }

    [Fact]
    public void RemoveAction_DryRun_True_ReturnsImmediately_NoException()
    {
        // Calls RemoveSshdDirective(directive, dryRun: true) → early return on dryRun
        var (td, _) = GetSshTweak();
        if (td?.RemoveAction is null)
            return;

        td.RemoveAction(true);
    }

    [Fact]
    public void RemoveAction_DryRun_False_NoFile_ReturnsImmediately_NoException()
    {
        // Calls RemoveSshdDirective(directive, dryRun: false)
        // → dryRun F-branch → file doesn't exist → returns safely
        var (td, _) = GetSshTweak();
        if (td?.RemoveAction is null)
            return;

        td.RemoveAction(false);
    }

    [Fact]
    public void DetectAction_NoSshdConfig_ReturnsFalse()
    {
        // Calls DetectSshdDirective(directive, expectedValue)
        // → `if (!File.Exists(SshdConfig)) return false;` T-branch
        // Skip on machines that have OpenSSH Server installed (e.g. GitHub windows-latest runners).
        if (System.IO.File.Exists(@"C:\ProgramData\ssh\sshd_config"))
            return;

        var (td, _) = GetSshTweak();
        if (td?.DetectAction is null)
            return;

        var result = td.DetectAction();
        Assert.False(result); // sshd_config does not exist
    }

    [Fact]
    public void AllSshTweaks_DetectAction_ReturnsFalseWhenNoSshdConfig()
    {
        // Skip on machines that have OpenSSH Server installed (e.g. GitHub windows-latest runners).
        if (System.IO.File.Exists(@"C:\ProgramData\ssh\sshd_config"))
            return;

        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var sshTweaks = engine.AllTweaks().Where(t => t.Category == "SSH Configuration").ToList();

        foreach (var td in sshTweaks)
        {
            if (td.DetectAction is not null)
            {
                var result = td.DetectAction();
                Assert.False(result); // no sshd_config
            }
        }
    }

    [Fact]
    public void AllSshTweaks_ApplyAction_DryRun_DoNotThrow()
    {
        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var sshTweaks = engine.AllTweaks().Where(t => t.Category == "SSH Configuration").ToList();

        foreach (var td in sshTweaks)
        {
            if (td.ApplyAction is not null)
                td.ApplyAction(true); // dryRun=true → safe early return for all SSH tweaks
        }
    }
}

// ── 9. Elevation RunElevated — allowed-command path ────────────────────────

