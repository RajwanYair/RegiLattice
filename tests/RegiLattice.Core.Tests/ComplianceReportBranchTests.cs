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
[Collection("Builtins")]
public sealed class ComplianceReportExporterBranchTests
{
    private readonly TweakEngine _engine;

    public ComplianceReportExporterBranchTests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    // ── null statusMap → all tweaks treated as Unknown ───────────────────

    [Fact]
    public void BuildHtml_NullStatusMap_ReturnsHtmlWithUnknownBadges()
    {
        string html = ComplianceReportExporter.BuildHtml(_engine, null);

        Assert.Contains("Unknown", html);
        Assert.Contains("Compliance Report", html);
    }

    // ── empty statusMap → same Unknown behaviour, healthPct = 0 ─────────

    [Fact]
    public void BuildHtml_EmptyStatusMap_HealthPercentIsZero()
    {
        string html = ComplianceReportExporter.BuildHtml(_engine, new Dictionary<string, TweakResult>());

        // All tweaks are Unknown → evaluated = 0 → healthPct = 0
        Assert.Contains("Health: 0%", html);
    }

    // ── Applied status → r-applied class, Applied badge, applied counter ─

    [Fact]
    public void BuildHtml_SomeApplied_ContainsAppliedBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Applied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-applied", html);
        Assert.Contains("Applied", html);
    }

    // ── NotApplied status → r-pending class, Pending badge ───────────────

    [Fact]
    public void BuildHtml_SomeNotApplied_ContainsPendingBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.NotApplied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-pending", html);
        Assert.Contains("Pending", html);
    }

    // ── Unknown status explicitly set ─────────────────────────────────────

    [Fact]
    public void BuildHtml_UnknownStatusExplicit_ContainsUnknownBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Unknown };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-unknown", html);
    }

    // ── healthPct > 0 when at least one tweak is Applied ─────────────────

    [Fact]
    public void BuildHtml_AllApplied_HealthIs100()
    {
        var tweaks = _engine.AllTweaks();
        var status = tweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("Health: 100%", html);
    }

    // ── mixed Applied + NotApplied → healthPct between 0 and 100 ─────────

    [Fact]
    public void BuildHtml_MixedAppliedAndPending_HealthIsBetween0And100()
    {
        var tweaks = _engine.AllTweaks();
        var half = tweaks.Count / 2;
        var status = new Dictionary<string, TweakResult>();
        for (int i = 0; i < half; i++)
            status[tweaks[i].Id] = TweakResult.Applied;
        for (int i = half; i < tweaks.Count; i++)
            status[tweaks[i].Id] = TweakResult.NotApplied;

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        // Should contain some health percentage
        Assert.Contains("Health:", html);
        Assert.DoesNotContain("Health: 0%", html);
        Assert.DoesNotContain("Health: 100%", html);
    }

    // ── NeedsAdmin=false → "No" in the Admin column ──────────────────────

    [Fact]
    public void BuildHtml_TweakWithNeedsAdminFalse_ShowsNo()
    {
        // Find any tweak where NeedsAdmin = false
        var noAdmin = _engine.AllTweaks().FirstOrDefault(t => !t.NeedsAdmin);
        if (noAdmin is null)
        {
            // Skip gracefully if all tweaks require admin (unlikely but safe)
            return;
        }

        var status = new Dictionary<string, TweakResult> { [noAdmin.Id] = TweakResult.Applied };
        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains(">No<", html);
    }

    // ── ExportHtml writes to a file ───────────────────────────────────────

    [Fact]
    public void ExportHtml_WritesToFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl_compliance_{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(_engine, null, path);
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            Assert.Contains("<!DOCTYPE html>", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── ExportHtml with explicit status map ──────────────────────────────

    [Fact]
    public void ExportHtml_WithStatusMap_WritesFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl_compliance_{Guid.NewGuid():N}.html");
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Applied };
        try
        {
            ComplianceReportExporter.ExportHtml(_engine, status, path);
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Status id NOT in dictionary → Unknown (ContainsKey=false branch) ─

    [Fact]
    public void BuildHtml_TweakIdNotInStatusMap_TreatedAsUnknown()
    {
        // Use a status map that has no entries matching engine's tweaks
        var status = new Dictionary<string, TweakResult> { ["nonexistent-id"] = TweakResult.Applied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        // All real tweaks should be Unknown
        Assert.Contains("b-unknown", html);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 2.  StartupManager — currently 14 % branch coverage (7 tests cover GetAllEntries only)
//     All writes go to HKCU which requires no admin; cleaned up via Delete().
// ═══════════════════════════════════════════════════════════════════════════

