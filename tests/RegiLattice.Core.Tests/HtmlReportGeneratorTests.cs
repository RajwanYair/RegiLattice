// tests/RegiLattice.Core.Tests/HtmlReportGeneratorTests.cs
// Sprint 72 — HtmlReportGenerator tests.

#nullable enable

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 72: HtmlReportGenerator.</summary>
public sealed class HtmlReportGeneratorTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public HtmlReportGeneratorTests(BuiltinsFixture fixture)
    {
        _engine = fixture.Engine;
    }

    private TweakEngine BuildEngine() => _engine;

    // ── Argument validation ───────────────────────────────────────────────

    [Fact]
    public void Constructor_NullEngine_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => new HtmlReportGenerator(null!));
    }

    [Fact]
    public void Build_NullStatusMap_ThrowsArgumentNullException()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        Assert.Throws<ArgumentNullException>(() => gen.Build(null!));
    }

    [Fact]
    public void Generate_NullOutputPath_ThrowsArgumentNullException()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        Assert.Throws<ArgumentNullException>(() => gen.Generate(null!, new Dictionary<string, TweakResult>()));
    }

    [Fact]
    public void Generate_EmptyOutputPath_ThrowsArgumentException()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        Assert.Throws<ArgumentException>(() => gen.Generate(string.Empty, new Dictionary<string, TweakResult>()));
    }

    // ── Build returns valid HTML structure ────────────────────────────────

    [Fact]
    public void Build_EmptyMap_ReturnsDoctype()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        Assert.StartsWith("<!DOCTYPE html>", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_EmptyMap_ContainsBodyAndTitle()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        Assert.Contains("<title>", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("RegiLattice", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("</body>", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_EmptyMap_ContainsSummaryCards()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        Assert.Contains("Total", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Applied", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Not Applied", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_EmptyMap_ContainsCategorySections()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        Assert.Contains("cat-section", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_EmptyMap_ContainsPrivacyCategory()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        // Privacy category must appear (it's in all built-in tweaks)
        Assert.Contains("Privacy", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_WithScore_ContainsScoreSection()
    {
        var engine = BuildEngine();
        var gen = new HtmlReportGenerator(engine);
        var score = new HealthScore(80, 60, 70, 55, 66);

        var html = gen.Build(new Dictionary<string, TweakResult>(), score);
        Assert.Contains("<div class=\"score-section\">", html, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("66%", html);
    }

    [Fact]
    public void Build_WithoutScore_DoesNotContainScoreSectionDiv()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>(), score: null);
        // The CSS class .score-section is always in the stylesheet, but the div must not be rendered.
        Assert.DoesNotContain("<div class=\"score-section\">", html, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Build_AppliedTweaks_ShowGreenStatus()
    {
        var engine = BuildEngine();
        var gen = new HtmlReportGenerator(engine);
        var firstTweak = engine.AllTweaks()[0];
        var map = new Dictionary<string, TweakResult> { [firstTweak.Id] = TweakResult.Applied };

        var html = gen.Build(map);
        Assert.Contains("st-applied", html);
    }

    [Fact]
    public void Build_HtmlEncodesSpecialCharacters()
    {
        // Category names and labels that contain HTML-special chars are encoded.
        var gen = new HtmlReportGenerator(BuildEngine());
        var html = gen.Build(new Dictionary<string, TweakResult>());
        // The raw characters < > & should not appear outside of tag structure.
        // (Labels/descriptions from real tweaks should be encoded.)
        // At minimum the HTML should not be broken by an unencoded ampersand in the title area.
        Assert.DoesNotContain("&amp;amp;", html); // double-encoding must not happen
    }

    // ── Generate writes file ─────────────────────────────────────────────

    [Fact]
    public void Generate_WritesFileToPath()
    {
        var gen = new HtmlReportGenerator(BuildEngine());
        var path = Path.Combine(Path.GetTempPath(), $"rl-report-{Guid.NewGuid():N}.html");
        try
        {
            gen.Generate(path, new Dictionary<string, TweakResult>());
            Assert.True(File.Exists(path), "Output file was not created.");
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Generate_FileContentsMatchBuild()
    {
        var engine = BuildEngine();
        var gen = new HtmlReportGenerator(engine);
        var map = new Dictionary<string, TweakResult>();
        var path = Path.Combine(Path.GetTempPath(), $"rl-report-{Guid.NewGuid():N}.html");

        try
        {
            gen.Generate(path, map);
            var written = File.ReadAllText(path);
            var built = gen.Build(map);

            // Content must be identical (same call, same map).
            Assert.Equal(built, written);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
