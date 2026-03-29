// tests/RegiLattice.Core.Tests/ComplianceReportExporterTests.cs
// Tests for ComplianceReportExporter — HTML compliance report generation (Sprint 102 service).

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Unit tests for <see cref="ComplianceReportExporter"/>.</summary>
public sealed class ComplianceReportExporterTests
{
    // ── Shared test engine ─────────────────────────────────────────────

    /// <summary>Build a minimal 3-tweak engine used by most tests.</summary>
    private static TweakEngine BuildEngine()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        engine.Register([
            new TweakDef
            {
                Id = "cre-test-applied",
                Label = "Applied Tweak",
                Category = "Compliance Test",
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\CRE\Test", "A", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\CRE\Test", "A")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\CRE\Test", "A", 1)],
            },
            new TweakDef
            {
                Id = "cre-test-pending",
                Label = "Pending Tweak",
                Category = "Compliance Test",
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\CRE\Test", "B", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\CRE\Test", "B")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\CRE\Test", "B", 1)],
            },
            new TweakDef
            {
                Id = "cre-test-unknown",
                Label = "Unknown Tweak",
                Category = "Compliance Test",
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\CRE\Test", "C", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\CRE\Test", "C")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\CRE\Test", "C", 1)],
            },
        ]);
        return engine;
    }

    // ── BuildHtml — structure ──────────────────────────────────────────

    [Fact]
    public void BuildHtml_WithTweaks_ReturnsNonEmptyString()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.False(string.IsNullOrWhiteSpace(html));
    }

    [Fact]
    public void BuildHtml_ReturnsWellFormedHtmlDocument()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.Contains("<!DOCTYPE html>", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<html>", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("</html>", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("<body>", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_ContainsReportTitle()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.Contains("RegiLattice Compliance Report", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_ContainsTweakCategoryName()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.Contains("Compliance Test", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_ContainsTweakLabel()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.Contains("Applied Tweak", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Pending Tweak", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Unknown Tweak", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_EmptyEngine_ReturnsHtmlWithZeroInHeader()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        string html = ComplianceReportExporter.BuildHtml(engine);
        Assert.Contains("0 tweaks", html, StringComparison.OrdinalIgnoreCase);
    }

    // ── BuildHtml — health percentage ─────────────────────────────────

    [Fact]
    public void BuildHtml_NullStatusMap_TreatsAllAsUnknown_ShowsZeroHealth()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine(), null);
        Assert.Contains("Health: 0%", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_AllApplied_Shows100PercentHealth()
    {
        var status = new Dictionary<string, TweakResult>
        {
            ["cre-test-applied"] = TweakResult.Applied,
            ["cre-test-pending"] = TweakResult.Applied,
            ["cre-test-unknown"] = TweakResult.Applied,
        };
        string html = ComplianceReportExporter.BuildHtml(BuildEngine(), status);
        Assert.Contains("Health: 100%", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_HalfApplied_Shows50PercentHealth()
    {
        var status = new Dictionary<string, TweakResult>
        {
            ["cre-test-applied"] = TweakResult.Applied,
            ["cre-test-pending"] = TweakResult.NotApplied,
            // cre-test-unknown is absent → treated as Unknown (not counted in denominator)
        };
        // 1 applied / 2 evaluated (applied+pending) → 50%
        string html = ComplianceReportExporter.BuildHtml(BuildEngine(), status);
        Assert.Contains("Health: 50%", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_AllPending_ShowsZeroPercentHealth()
    {
        var status = new Dictionary<string, TweakResult>
        {
            ["cre-test-applied"] = TweakResult.NotApplied,
            ["cre-test-pending"] = TweakResult.NotApplied,
            ["cre-test-unknown"] = TweakResult.NotApplied,
        };
        string html = ComplianceReportExporter.BuildHtml(BuildEngine(), status);
        Assert.Contains("Health: 0%", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_ReturnsHtml_ContainsAllStatusBadgeClasses()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        // CSS badge classes must be present
        Assert.Contains("b-applied", html, StringComparison.Ordinal);
        Assert.Contains("b-pending", html, StringComparison.Ordinal);
        Assert.Contains("b-unknown", html, StringComparison.Ordinal);
    }

    [Fact]
    public void BuildHtml_ContainsGaugeBar()
    {
        string html = ComplianceReportExporter.BuildHtml(BuildEngine());
        Assert.Contains("gauge-bar", html, StringComparison.Ordinal);
    }

    // ── BuildHtml — null / guard contracts ────────────────────────────

    [Fact]
    public void BuildHtml_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => ComplianceReportExporter.BuildHtml(null!));
    }

    [Fact]
    public void BuildHtml_EmptyStatusMap_BehavesLikeNullStatusMap()
    {
        var engine = BuildEngine();
        var empty = new Dictionary<string, TweakResult>();
        string htmlEmpty = ComplianceReportExporter.BuildHtml(engine, empty);
        string htmlNull = ComplianceReportExporter.BuildHtml(engine, null);

        // Both should report 0% health (no evaluated tweaks)
        Assert.Contains("Health: 0%", htmlEmpty, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Health: 0%", htmlNull, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildHtml_UnknownTweakIdInStatusMap_IsIgnored()
    {
        var status = new Dictionary<string, TweakResult> { ["completely-unknown-id-xyz"] = TweakResult.Applied };
        // Should not throw — unknown IDs in the map are just extra data
        string html = ComplianceReportExporter.BuildHtml(BuildEngine(), status);
        Assert.False(string.IsNullOrWhiteSpace(html));
    }

    // ── ExportHtml — file I/O ──────────────────────────────────────────

    [Fact]
    public void ExportHtml_CreatesFileAtOutputPath()
    {
        string path = Path.Combine(Path.GetTempPath(), $"cre-test-{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(BuildEngine(), null, path);
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ExportHtml_FileContainsDocType()
    {
        string path = Path.Combine(Path.GetTempPath(), $"cre-test-{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(BuildEngine(), null, path);
            string content = File.ReadAllText(path);
            Assert.Contains("<!DOCTYPE html>", content, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ExportHtml_FileContentMatchesBuildHtml()
    {
        var engine = BuildEngine();
        var status = new Dictionary<string, TweakResult> { ["cre-test-applied"] = TweakResult.Applied };
        string path = Path.Combine(Path.GetTempPath(), $"cre-test-{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(engine, status, path);
            string written = File.ReadAllText(path, System.Text.Encoding.UTF8);
            string built = ComplianceReportExporter.BuildHtml(engine, status);

            // Strip dynamic timestamp before comparing
            string Pattern(string html) => System.Text.RegularExpressions.Regex.Replace(html, @"\d{4}-\d{2}-\d{2} \d{2}:\d{2}", "TIMESTAMP");

            Assert.Equal(Pattern(built), Pattern(written));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void ExportHtml_WithStatusMap_WritesCorrectHealthPercent()
    {
        var status = new Dictionary<string, TweakResult>
        {
            ["cre-test-applied"] = TweakResult.Applied,
            ["cre-test-pending"] = TweakResult.NotApplied,
        };
        string path = Path.Combine(Path.GetTempPath(), $"cre-test-{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(BuildEngine(), status, path);
            string content = File.ReadAllText(path);
            Assert.Contains("Health: 50%", content, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── ExportHtml — guard contracts ──────────────────────────────────

    [Fact]
    public void ExportHtml_NullEngine_ThrowsArgumentNullException()
    {
        string tmpPath = Path.Combine(Path.GetTempPath(), $"cre-null-{Guid.NewGuid():N}.html");
        Assert.Throws<ArgumentNullException>(() => ComplianceReportExporter.ExportHtml(null!, null, tmpPath));
    }

    [Fact]
    public void ExportHtml_EmptyOutputPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ComplianceReportExporter.ExportHtml(BuildEngine(), null, ""));
    }

    [Fact]
    public void ExportHtml_WhitespaceOutputPath_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => ComplianceReportExporter.ExportHtml(BuildEngine(), null, "   "));
    }

    [Fact]
    public void ExportHtml_WritesUtf8File()
    {
        string path = Path.Combine(Path.GetTempPath(), $"cre-utf8-{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(BuildEngine(), null, path);
            // File must be readable as UTF-8 without BOM
            byte[] bytes = File.ReadAllBytes(path);
            Assert.True(bytes.Length > 0);
            string decoded = System.Text.Encoding.UTF8.GetString(bytes);
            Assert.Contains("<!DOCTYPE html>", decoded, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
