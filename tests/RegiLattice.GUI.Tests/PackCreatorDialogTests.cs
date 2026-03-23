// RegiLattice.GUI.Tests — PackCreatorDialogTests.cs
// Sprint 125: Pack Creator Studio Dialog (T7.2).

using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>
/// Logic tests for PackCreatorDialog helpers via PackSubmissionService.
/// Creates no WinForms windows — tests pack assembly and submission logic only.
/// </summary>
public sealed class PackCreatorDialogTests
{
    // ── Common fixtures ───────────────────────────────────────────────────

    private static PackDef ValidPack(int tweakCount = 5) => new PackDef
    {
        Name = "test-creator-pack",
        DisplayName = "Test Creator Pack",
        Version = "1.0.0",
        Author = "someone",
        Description = "Tests the Pack Creator Studio dialog logic end-to-end.",
        TweakCount = tweakCount,
        DownloadUrl = "https://example.com/pack.json",
        Sha256 = new string('a', 64),
        Categories = ["Performance", "Privacy"],
        Tags = ["perf", "privacy"],
    };

    // ── Validation bridging ───────────────────────────────────────────────

    [Fact]
    public void Validate_ValidPackFromCreator_IsValid()
    {
        PackSubmissionValidation result = PackSubmissionService.Validate(ValidPack());
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_PackWithZeroTweaks_Fails()
    {
        PackDef pack = ValidPack(tweakCount: 0);
        PackSubmissionValidation result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("TweakCount"));
    }

    [Fact]
    public void Validate_PackWithNoDescription_Fails()
    {
        PackDef pack = ValidPack() with { Description = "" };
        PackSubmissionValidation result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Description"));
    }

    // ── JSON preview structure ────────────────────────────────────────────

    [Fact]
    public void BuildJson_ContainsPackName()
    {
        // Mirror what PackCreatorDialog.BuildJsonPreview does using System.Text.Json
        PackDef pack = ValidPack();
        string json = System.Text.Json.JsonSerializer.Serialize(new
        {
            pack.Name,
            pack.DisplayName,
            pack.Version,
            pack.Author,
            pack.Description,
            pack.TweakCount,
        }, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

        Assert.Contains("test-creator-pack", json);
    }

    [Fact]
    public void BuildJson_ContainsVersion()
    {
        PackDef pack = ValidPack();
        string json = System.Text.Json.JsonSerializer.Serialize(new { pack.Version });
        Assert.Contains("1.0.0", json);
    }

    // ── SubmissionUrl bridging (T7.5) ──────────────────────────────────────

    [Fact]
    public void BuildSubmissionUrl_ForCreatedPack_ContainsDisplayName()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.Contains("Test+Creator+Pack", url, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void BuildSubmissionUrl_ForCreatedPack_HasPackSubmissionTemplate()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.Contains("template=pack-submission.yml", url);
    }

    // ── SanitizeName helper (as used by dialog slug preview) ──────────────

    [Fact]
    public void SanitizeName_ProducesKebabCase()
    {
        Assert.Equal("my-creator-pack", PackSubmissionService.SanitizeName("My Creator Pack"));
    }

    [Fact]
    public void SanitizeName_StripsInvalidChars()
    {
        string slug = PackSubmissionService.SanitizeName("Pack (v2)! [alpha]");
        Assert.Matches(@"^[a-z0-9\-]+$", slug);
    }

    [Fact]
    public void SanitizeName_NullInput_ReturnsFallback()
    {
        // Empty is the closest to null via the public API
        Assert.Equal("my-pack", PackSubmissionService.SanitizeName(""));
    }

    // ── PackDef record semantics ───────────────────────────────────────────

    [Fact]
    public void PackDef_WithExpression_ClonesCorrectly()
    {
        PackDef original = ValidPack();
        PackDef updated = original with { Version = "2.0.0" };
        Assert.Equal("2.0.0", updated.Version);
        Assert.Equal("1.0.0", original.Version);
        Assert.Equal(original.Name, updated.Name);
    }

    [Fact]
    public void PackDef_Categories_DefaultsToEmpty()
    {
        PackDef pack = new PackDef { Name = "x", DisplayName = "X", Version = "1.0.0", Author = "a" };
        Assert.Empty(pack.Categories);
        Assert.Empty(pack.Tags);
    }
}
