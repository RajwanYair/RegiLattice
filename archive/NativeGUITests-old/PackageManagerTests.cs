using RegiLattice.Native.PackageManagers;
using Xunit;

namespace NativeGUITests;

/// <summary>Unit tests for package manager validation and helpers (no I/O).</summary>
public sealed class PackageManagerTests
{
    // ── ScoopManager.ValidateName ──────────────────────────────────────────
    [Theory]
    [InlineData("7zip")]
    [InlineData("git")]
    [InlineData("ripgrep")]
    [InlineData("posh-git")]
    [InlineData("my.package")]
    [InlineData("my_package")]
    [InlineData("abc123")]
    public void ScoopValidateName_ValidNames_ReturnsName(string name)
    {
        string result = ScoopManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("abc def")]
    [InlineData("pkg;rm -rf")]
    [InlineData("pkg|bad")]
    [InlineData("pkg$(evil)")]
    public void ScoopValidateName_InvalidNames_Throws(string name)
        => Assert.Throws<ArgumentException>(() => ScoopManager.ValidateName(name));

    // ── PSModuleManager.ValidateName ────────────────────────────────────────
    [Theory]
    [InlineData("PSReadLine")]
    [InlineData("posh-git")]
    [InlineData("Az")]
    [InlineData("Microsoft.Graph")]
    public void PSModuleValidateName_ValidNames_ReturnsName(string name)
    {
        string result = PSModuleManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("mod name")]
    [InlineData("mod;evil")]
    public void PSModuleValidateName_InvalidNames_Throws(string name)
        => Assert.Throws<ArgumentException>(() => PSModuleManager.ValidateName(name));

    // ── PSModuleManager.ValidateScope ────────────────────────────────────────
    [Fact]
    public void PSModuleValidateScope_CurrentUser_OK()
        => Assert.Equal("CurrentUser", PSModuleManager.ValidateScope("CurrentUser"));

    [Fact]
    public void PSModuleValidateScope_AllUsers_OK()
        => Assert.Equal("AllUsers", PSModuleManager.ValidateScope("AllUsers"));

    [Fact]
    public void PSModuleValidateScope_Invalid_Throws()
        => Assert.Throws<ArgumentException>(() => PSModuleManager.ValidateScope("System"));

    // ── PipManager.ValidateName ────────────────────────────────────────────
    [Theory]
    [InlineData("requests")]
    [InlineData("rich")]
    [InlineData("pydantic")]
    [InlineData("httpx[http2]")]
    [InlineData("my-package")]
    [InlineData("my_package")]
    [InlineData("pkg.sub")]
    public void PipValidateName_ValidNames_ReturnsName(string name)
    {
        string result = PipManager.ValidateName(name);
        Assert.Equal(name, result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("pkg name")]
    [InlineData("pkg;evil")]
    [InlineData("pkg|bad")]
    public void PipValidateName_InvalidNames_Throws(string name)
        => Assert.Throws<ArgumentException>(() => PipManager.ValidateName(name));

    // ── ScoopManager.IsScoopInstalled ────────────────────────────────────────
    [Fact]
    public void ScoopIsScoopInstalled_ReturnsBool()
    {
        // Just ensure it doesn't throw; result depends on environment
        bool result = ScoopManager.IsScoopInstalled();
        Assert.IsType<bool>(result);
    }

    // ── Popular lists ─────────────────────────────────────────────────────
    [Fact]
    public void ScoopPopularTools_NonEmpty()
        => Assert.NotEmpty(ScoopManager.PopularTools);

    [Fact]
    public void PSModulePopularModules_NonEmpty()
        => Assert.NotEmpty(PSModuleManager.PopularModules);

    [Fact]
    public void PipPopularPackages_NonEmpty()
        => Assert.NotEmpty(PipManager.PopularPackages);
}
