using RegiLattice.GUI.PackageManagers;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>Tests for package manager input validation (pure logic, no I/O).</summary>
public sealed class PackageManagerValidationTests
{
    // ── ScoopManager.ValidateName ──────────────────────────────────────

    [Theory]
    [InlineData("7zip")]
    [InlineData("git")]
    [InlineData("ripgrep")]
    [InlineData("posh-git")]
    [InlineData("my.package")]
    [InlineData("my_package")]
    [InlineData("abc123")]
    public void ScoopValidateName_Valid_ReturnsName(string name)
        => Assert.Equal(name, ScoopManager.ValidateName(name));

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc def")]
    [InlineData("pkg;rm -rf")]
    [InlineData("pkg|bad")]
    [InlineData("pkg$(evil)")]
    public void ScoopValidateName_Invalid_Throws(string name)
        => Assert.Throws<ArgumentException>(() => ScoopManager.ValidateName(name));

    // ── PSModuleManager.ValidateName ────────────────────────────────────

    [Theory]
    [InlineData("PSReadLine")]
    [InlineData("posh-git")]
    [InlineData("Az")]
    [InlineData("Microsoft.Graph")]
    public void PSModuleValidateName_Valid_ReturnsName(string name)
        => Assert.Equal(name, PSModuleManager.ValidateName(name));

    [Theory]
    [InlineData("")]
    [InlineData("mod name")]
    [InlineData("mod;evil")]
    public void PSModuleValidateName_Invalid_Throws(string name)
        => Assert.Throws<ArgumentException>(() => PSModuleManager.ValidateName(name));

    // ── PSModuleManager.ValidateScope ────────────────────────────────────

    [Fact]
    public void PSModuleValidateScope_CurrentUser_OK()
        => Assert.Equal("CurrentUser", PSModuleManager.ValidateScope("CurrentUser"));

    [Fact]
    public void PSModuleValidateScope_AllUsers_OK()
        => Assert.Equal("AllUsers", PSModuleManager.ValidateScope("AllUsers"));

    [Fact]
    public void PSModuleValidateScope_Invalid_Throws()
        => Assert.Throws<ArgumentException>(() => PSModuleManager.ValidateScope("System"));

    // ── ScoopManager.IsScoopInstalled ────────────────────────────────────

    [Fact]
    public void ScoopIsScoopInstalled_ReturnsBool()
    {
        _ = ScoopManager.IsScoopInstalled(); // just ensure no throw
    }

    // ── Popular lists are non-empty ────────────────────────────────────

    [Fact]
    public void ScoopPopularTools_NotEmpty()
        => Assert.NotEmpty(ScoopManager.PopularTools);

    [Fact]
    public void PSModulePopularModules_NotEmpty()
        => Assert.NotEmpty(PSModuleManager.PopularModules);
}
