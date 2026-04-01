// RegiLattice.Core.Tests — PowerShellModuleGeneratorTests.cs
// PowerShell module generator tests.

using System;
using System.IO;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class PowerShellModuleGeneratorTests
{
    // ── BuildManifest ─────────────────────────────────────────────────────

    [Fact]
    public void BuildManifest_ContainsModuleName()
    {
        var manifest = PowerShellModuleGenerator.BuildManifest();
        Assert.Contains("RegiLattice", manifest);
    }

    [Theory]
    [InlineData("Get-Tweak")]
    [InlineData("Search-Tweak")]
    [InlineData("Get-TweakStatus")]
    [InlineData("Get-TweakCategory")]
    [InlineData("Apply-Tweak")]
    [InlineData("Remove-Tweak")]
    [InlineData("Get-TweakProfile")]
    [InlineData("Apply-TweakProfile")]
    [InlineData("Get-HealthScore")]
    [InlineData("Export-TweakSelection")]
    [InlineData("Import-TweakSelection")]
    public void BuildManifest_ContainsAllFunctions(string functionName)
    {
        var manifest = PowerShellModuleGenerator.BuildManifest();
        Assert.Contains(functionName, manifest);
    }

    [Fact]
    public void BuildManifest_ContainsFunctionsToExport()
    {
        var manifest = PowerShellModuleGenerator.BuildManifest();
        Assert.Contains("FunctionsToExport", manifest);
    }

    [Fact]
    public void BuildManifest_ContainsVersionAndGuid()
    {
        var manifest = PowerShellModuleGenerator.BuildManifest();
        Assert.Contains("ModuleVersion", manifest);
        Assert.Contains("GUID", manifest);
    }

    [Fact]
    public void BuildManifest_ContainsProjectUri()
    {
        var manifest = PowerShellModuleGenerator.BuildManifest();
        Assert.Contains("github.com/RajwanYair/RegiLattice", manifest);
    }

    [Fact]
    public void BuildManifest_StartsWithAtBrace()
    {
        var manifest = PowerShellModuleGenerator.BuildManifest().TrimStart();
        Assert.StartsWith("@{", manifest);
    }

    // ── BuildScript ───────────────────────────────────────────────────────

    [Fact]
    public void BuildScript_ContainsCorePath()
    {
        const string fakePath = @"C:\Tools\RegiLattice.Core.dll";
        var script = PowerShellModuleGenerator.BuildScript(fakePath);
        Assert.Contains(fakePath, script);
    }

    [Theory]
    [InlineData("Get-Tweak")]
    [InlineData("Search-Tweak")]
    [InlineData("Get-TweakStatus")]
    [InlineData("Get-TweakCategory")]
    [InlineData("Apply-Tweak")]
    [InlineData("Remove-Tweak")]
    [InlineData("Get-TweakProfile")]
    [InlineData("Apply-TweakProfile")]
    [InlineData("Get-HealthScore")]
    [InlineData("Export-TweakSelection")]
    [InlineData("Import-TweakSelection")]
    public void BuildScript_ContainsCmdletDefinitions(string functionName)
    {
        var script = PowerShellModuleGenerator.BuildScript(@"C:\fake\core.dll");
        // Each function must be defined with the 'function' keyword
        Assert.Contains($"function {functionName}", script);
    }

    [Fact]
    public void BuildScript_ContainsAddType()
    {
        var script = PowerShellModuleGenerator.BuildScript(@"C:\fake\core.dll");
        Assert.Contains("Add-Type", script);
    }

    [Fact]
    public void BuildScript_ContainsTweakEngineReference()
    {
        var script = PowerShellModuleGenerator.BuildScript(@"C:\fake\core.dll");
        Assert.Contains("TweakEngine", script);
    }

    [Fact]
    public void BuildScript_EscapesSingleQuoteInPath()
    {
        // Paths with single quotes must be PowerShell-escaped as ''
        const string pathWithQuote = @"C:\My 'Tools'\core.dll";
        var script = PowerShellModuleGenerator.BuildScript(pathWithQuote);
        // The escaped form should appear, not the raw single quote inside string context
        Assert.Contains("''", script);
    }

    // ── Generate ──────────────────────────────────────────────────────────

    [Fact]
    public void Generate_CreatesExpectedFiles()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"RL_PsTest_{Guid.NewGuid():N}");
        try
        {
            PowerShellModuleGenerator.Generate(tempDir, @"C:\fake\RegiLattice.Core.dll");

            Assert.True(File.Exists(Path.Combine(tempDir, "RegiLattice.psd1")), "RegiLattice.psd1 should be created");
            Assert.True(File.Exists(Path.Combine(tempDir, "RegiLattice.psm1")), "RegiLattice.psm1 should be created");
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void Generate_CreatesOutputDirectory_WhenMissing()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"RL_PsTest_{Guid.NewGuid():N}", "sub", "nested");
        try
        {
            PowerShellModuleGenerator.Generate(tempDir, @"C:\fake\core.dll");
            Assert.True(Directory.Exists(tempDir));
        }
        finally
        {
            // Walk up and delete the root temp dir
            var root = tempDir;
            while (Path.GetDirectoryName(root)?.StartsWith(Path.GetTempPath(), StringComparison.OrdinalIgnoreCase) == true)
                root = Path.GetDirectoryName(root)!;
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void Generate_ThrowsForNullOutputDir()
    {
        Assert.Throws<ArgumentNullException>(() => PowerShellModuleGenerator.Generate(null!, @"C:\fake\core.dll"));
    }

    [Fact]
    public void Generate_ThrowsForNullCoreAssemblyPath()
    {
        Assert.Throws<ArgumentNullException>(() => PowerShellModuleGenerator.Generate(Path.GetTempPath(), null!));
    }

    [Fact]
    public void Generate_ThrowsForEmptyOutputDir()
    {
        Assert.Throws<ArgumentException>(() => PowerShellModuleGenerator.Generate("", @"C:\fake\core.dll"));
    }

    [Fact]
    public void Generate_Psd1ContentMatchesBuildManifest()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"RL_PsTest_{Guid.NewGuid():N}");
        try
        {
            PowerShellModuleGenerator.Generate(tempDir, @"C:\fake\core.dll");
            var psd1Content = File.ReadAllText(Path.Combine(tempDir, "RegiLattice.psd1"));
            var expected = PowerShellModuleGenerator.BuildManifest();
            Assert.Equal(expected, psd1Content);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }

    [Fact]
    public void Generate_Psm1ContentMatchesBuildScript()
    {
        const string fakeDll = @"C:\fake\RegiLattice.Core.dll";
        var tempDir = Path.Combine(Path.GetTempPath(), $"RL_PsTest_{Guid.NewGuid():N}");
        try
        {
            PowerShellModuleGenerator.Generate(tempDir, fakeDll);
            var psm1Content = File.ReadAllText(Path.Combine(tempDir, "RegiLattice.psm1"));
            var expected = PowerShellModuleGenerator.BuildScript(fakeDll);
            Assert.Equal(expected, psm1Content);
        }
        finally
        {
            if (Directory.Exists(tempDir))
                Directory.Delete(tempDir, recursive: true);
        }
    }
}
