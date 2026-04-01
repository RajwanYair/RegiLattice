// RegiLattice.Core.Tests — PackSubmissionServiceTests.cs
// Tests for PackSubmissionService.

using RegiLattice.Core.Plugins;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class PackSubmissionServiceTests
{
    // ── Helpers ───────────────────────────────────────────────────────────

    private static PackDef ValidPack() =>
        new PackDef
        {
            Name = "gaming-booster-pro",
            DisplayName = "Gaming Booster Pro",
            Version = "1.0.0",
            Author = "testuser",
            Description = "A collection of 20 tweaks targeting competitive gaming performance.",
            TweakCount = 20,
            DownloadUrl = "https://github.com/testuser/gaming-booster-pro/releases/download/v1.0.0/pack.json",
            Sha256 = "e3b0c44298fc1c149afbf4c8996fb92427ae41e4649b934ca495991b7852b855",
            Categories = ["Gaming", "Performance"],
            Tags = ["gaming", "latency"],
        };

    // ── Validate — happy path ─────────────────────────────────────────────

    [Fact]
    public void Validate_ValidPack_IsValid()
    {
        var result = PackSubmissionService.Validate(ValidPack());
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    // ── Validate — individual field failures ──────────────────────────────

    [Fact]
    public void Validate_EmptyName_ReturnsError()
    {
        var pack = ValidPack() with { Name = "" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Name"));
    }

    [Fact]
    public void Validate_UpperCaseName_ReturnsError()
    {
        var pack = ValidPack() with { Name = "Gaming-Booster" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("kebab-case"));
    }

    [Fact]
    public void Validate_EmptyDisplayName_ReturnsError()
    {
        var pack = ValidPack() with { DisplayName = "" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("DisplayName"));
    }

    [Fact]
    public void Validate_EmptyAuthor_ReturnsError()
    {
        var pack = ValidPack() with { Author = "" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Author"));
    }

    [Fact]
    public void Validate_BadVersion_ReturnsError()
    {
        var pack = ValidPack() with { Version = "v1.0" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("Semantic Versioning"));
    }

    [Fact]
    public void Validate_MissingDownloadUrl_ReturnsError()
    {
        var pack = ValidPack() with { DownloadUrl = "" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("DownloadUrl"));
    }

    [Fact]
    public void Validate_HttpDownloadUrl_ReturnsError()
    {
        var pack = ValidPack() with { DownloadUrl = "ftp://example.com/pack.json" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("https://"));
    }

    [Fact]
    public void Validate_ShortDescription_ReturnsError()
    {
        var pack = ValidPack() with { Description = "Too short" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("20 characters"));
    }

    [Fact]
    public void Validate_ZeroTweakCount_ReturnsError()
    {
        var pack = ValidPack() with { TweakCount = 0 };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("TweakCount"));
    }

    [Fact]
    public void Validate_BadSha256_ReturnsError()
    {
        var pack = ValidPack() with { Sha256 = "not-a-sha256" };
        var result = PackSubmissionService.Validate(pack);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.Contains("SHA-256") || e.Contains("Sha256"));
    }

    // ── BuildSubmissionUrl ────────────────────────────────────────────────

    [Fact]
    public void BuildSubmissionUrl_StartsWithGitHubIssueBase()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.StartsWith("https://github.com/RajwanYair/RegiLattice/issues/new?template=pack-submission.yml", url);
    }

    [Fact]
    public void BuildSubmissionUrl_ContainsTitleParam()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.Contains("title=", url);
    }

    [Fact]
    public void BuildSubmissionUrl_ContainsPackName()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.Contains("gaming-booster", url);
    }

    [Fact]
    public void BuildSubmissionUrl_ContainsAuthor()
    {
        string url = PackSubmissionService.BuildSubmissionUrl(ValidPack());
        Assert.Contains("testuser", url);
    }

    // ── SanitizeName ─────────────────────────────────────────────────────

    [Fact]
    public void SanitizeName_SpacesConvertedToHyphens()
    {
        Assert.Equal("gaming-booster-pro", PackSubmissionService.SanitizeName("Gaming Booster Pro"));
    }

    [Fact]
    public void SanitizeName_EmptyString_ReturnsFallback()
    {
        Assert.Equal("my-pack", PackSubmissionService.SanitizeName(""));
    }

    [Fact]
    public void SanitizeName_SpecialCharsRemoved()
    {
        string result = PackSubmissionService.SanitizeName("My (Special!) Pack v2");
        Assert.Matches(@"^[a-z0-9\-]+$", result);
    }

    [Fact]
    public void SanitizeName_AlreadyValidSlug_Unchanged()
    {
        Assert.Equal("my-pack", PackSubmissionService.SanitizeName("my-pack"));
    }
}
