using System.Diagnostics;
using System.Reflection;
using System.Reflection.PortableExecutable;
using Xunit;

namespace RegiLattice.GUI.Tests;

/// <summary>
/// Validates that compiled executables are structurally valid, have correct
/// metadata, and that all theme fonts produce usable GDI+ handles.
/// Catches issues like the v4.7.0 Font.ToHfont() crash on published builds.
/// </summary>
[Collection("AppTheme")]
public sealed class ExecutableValidationTests
{
    // ── PE / Assembly validation ─────────────────────────────────────────

    [Fact]
    public void GuiExe_ExistsInBuildOutput()
    {
        var guiPath = FindBuildOutput("RegiLattice.GUI.exe");
        Assert.True(File.Exists(guiPath), $"GUI EXE not found at {guiPath}");
    }

    [Fact]
    public void CliExe_ExistsInBuildOutput()
    {
        var cliPath = FindBuildOutput("RegiLattice.exe", "RegiLattice.CLI");
        Assert.True(File.Exists(cliPath), $"CLI EXE not found at {cliPath}");
    }

    [Fact]
    public void GuiExe_IsValidPE()
    {
        var guiPath = FindBuildOutput("RegiLattice.GUI.exe");
        if (!File.Exists(guiPath))
            return; // Skip if not built — GuiExe_ExistsInBuildOutput covers this

        using var stream = File.OpenRead(guiPath);
        using var reader = new PEReader(stream);

        // .NET app host EXE is a native PE stub — metadata lives in the DLL.
        // Verify it's a valid PE with correct machine architecture.
        Assert.True(reader.PEHeaders.PEHeader is not null, "GUI EXE has no PE header");
        Assert.True(
            reader.PEHeaders.CoffHeader.Machine is Machine.Amd64 or Machine.I386,
            $"Unexpected machine type: {reader.PEHeaders.CoffHeader.Machine}"
        );
    }

    [Fact]
    public void GuiDll_HasManagedMetadata()
    {
        var dllPath = FindBuildOutput("RegiLattice.GUI.dll");
        if (!File.Exists(dllPath))
            return;

        using var stream = File.OpenRead(dllPath);
        using var reader = new PEReader(stream);

        Assert.True(reader.HasMetadata, "GUI DLL has no .NET metadata");
    }

    [Fact]
    public void CliExe_IsValidPE()
    {
        var cliPath = FindBuildOutput("RegiLattice.exe", "RegiLattice.CLI");
        if (!File.Exists(cliPath))
            return;

        using var stream = File.OpenRead(cliPath);
        using var reader = new PEReader(stream);

        Assert.True(reader.PEHeaders.PEHeader is not null, "CLI EXE has no PE header");
        Assert.True(
            reader.PEHeaders.CoffHeader.Machine is Machine.Amd64 or Machine.I386,
            $"Unexpected machine type: {reader.PEHeaders.CoffHeader.Machine}"
        );
    }

    [Fact]
    public void CliDll_HasManagedMetadata()
    {
        // CLI assembly name is "RegiLattice" (not "RegiLattice.CLI") per csproj AssemblyName
        var dllPath = FindBuildOutput("RegiLattice.dll", "RegiLattice.CLI");
        if (!File.Exists(dllPath))
            return;

        using var stream = File.OpenRead(dllPath);
        using var reader = new PEReader(stream);

        Assert.True(reader.HasMetadata, "CLI DLL has no .NET metadata");
    }

    [Fact]
    public void GuiExe_HasCorrectVersion()
    {
        var guiPath = FindBuildOutput("RegiLattice.GUI.exe");
        if (!File.Exists(guiPath))
            return;

        var versionInfo = FileVersionInfo.GetVersionInfo(guiPath);
        Assert.NotNull(versionInfo.FileVersion);
        Assert.NotEqual("0.0.0.0", versionInfo.FileVersion);
        var expectedMajor = typeof(RegiLattice.Core.TweakEngine).Assembly.GetName().Version!.Major;
        Assert.StartsWith($"{expectedMajor}.", versionInfo.FileVersion);
    }

    [Fact]
    public void CliExe_HasCorrectVersion()
    {
        var cliPath = FindBuildOutput("RegiLattice.exe", "RegiLattice.CLI");
        if (!File.Exists(cliPath))
            return;

        var versionInfo = FileVersionInfo.GetVersionInfo(cliPath);
        Assert.NotNull(versionInfo.FileVersion);
        Assert.NotEqual("0.0.0.0", versionInfo.FileVersion);
        var expectedMajor = typeof(RegiLattice.Core.TweakEngine).Assembly.GetName().Version!.Major;
        Assert.StartsWith($"{expectedMajor}.", versionInfo.FileVersion);
    }

    [Fact]
    public void CoreDll_HasCorrectVersion()
    {
        var corePath = FindBuildOutput("RegiLattice.Core.dll", "RegiLattice.Core");
        if (!File.Exists(corePath))
            return;

        var versionInfo = FileVersionInfo.GetVersionInfo(corePath);
        Assert.NotNull(versionInfo.FileVersion);
        Assert.NotEqual("0.0.0.0", versionInfo.FileVersion);
        var expectedMajor = typeof(RegiLattice.Core.TweakEngine).Assembly.GetName().Version!.Major;
        Assert.StartsWith($"{expectedMajor}.", versionInfo.FileVersion);
    }

    [Fact]
    public void GuiExe_LoadedAssembly_VersionMatches()
    {
        var guiAssembly = typeof(AppTheme).Assembly;
        var version = guiAssembly.GetName().Version;

        Assert.NotNull(version);
        Assert.NotEqual(new Version(0, 0, 0, 0), version);
        Assert.True(version.Major >= 5, $"GUI major version {version.Major} unexpectedly low");
    }

    [Fact]
    public void CoreDll_LoadedAssembly_VersionMatches()
    {
        var coreAssembly = typeof(RegiLattice.Core.TweakEngine).Assembly;
        var version = coreAssembly.GetName().Version;

        Assert.NotNull(version);
        Assert.NotEqual(new Version(0, 0, 0, 0), version);
        Assert.True(version.Major >= 5, $"Core major version {version.Major} unexpectedly low");
    }

    // ── CLI smoke test ──────────────────────────────────────────────────

    [Fact]
    public void CliExe_RunWithHelp_ExitsCleanly()
    {
        var cliPath = FindBuildOutput("RegiLattice.exe", "RegiLattice.CLI");
        if (!File.Exists(cliPath))
            return;

        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = cliPath,
            Arguments = "--help",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        process.Start();
        // Drain stdout before WaitForExit — B3 help output is large and fills the
        // pipe buffer, causing the CLI to block on Write while the test waits for
        // exit: classic pipe-buffer deadlock.  ReadToEnd() completes once the
        // process closes its stdout (i.e. exits), so WaitForExit() returns instantly.
        string _ = process.StandardOutput.ReadToEnd();
        bool exited = process.WaitForExit(10_000);

        Assert.True(exited, "CLI process did not exit within 10 seconds");
        Assert.Equal(0, process.ExitCode);
    }

    [Fact]
    public void CliExe_RunWithVersion_OutputsVersion()
    {
        var cliPath = FindBuildOutput("RegiLattice.exe", "RegiLattice.CLI");
        if (!File.Exists(cliPath))
            return;

        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = cliPath,
            Arguments = "--version",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };

        process.Start();
        string output = process.StandardOutput.ReadToEnd();
        bool exited = process.WaitForExit(10_000);

        Assert.True(exited, "CLI process did not exit within 10 seconds");
        // Check that a semantic-version string is present in the output,
        // rather than hardcoding a specific version that becomes stale.
        Assert.Matches(@"\d+\.\d+\.\d+", output);
    }

    // ── Font handle validation (catches Font.ToHfont crashes) ───────────

    [Fact]
    public void ThemeFont_Regular_ProducesValidHandle()
    {
        var handle = AppTheme.Regular.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ThemeFont_Small_ProducesValidHandle()
    {
        var handle = AppTheme.Small.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ThemeFont_Bold_ProducesValidHandle()
    {
        var handle = AppTheme.Bold.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ThemeFont_Title_ProducesValidHandle()
    {
        var handle = AppTheme.Title.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ThemeFont_Mono_ProducesValidHandle()
    {
        var handle = AppTheme.Mono.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Fact]
    public void ThemeFont_SmallBold_ProducesValidHandle()
    {
        var handle = AppTheme.SmallBold.ToHfont();
        Assert.NotEqual(IntPtr.Zero, handle);
    }

    [Theory]
    [InlineData("catppuccin-mocha")]
    [InlineData("catppuccin-latte")]
    [InlineData("nord")]
    [InlineData("dracula")]
    [InlineData("tokyo-night")]
    [InlineData("gruvbox-dark")]
    [InlineData("solarized-dark")]
    [InlineData("one-dark")]
    [InlineData("rose-pine")]
    [InlineData("everforest")]
    [InlineData("cyberpunk")]
    public void AllThemes_FontsProduceValidHandles(string themeName)
    {
        try
        {
            AppTheme.SetTheme(themeName);
            // Every font must produce a valid HFONT — the v4.7.0 crash was
            // Font.ToHfont() throwing ArgumentException during TreeView creation.
            Assert.NotEqual(IntPtr.Zero, AppTheme.Regular.ToHfont());
            Assert.NotEqual(IntPtr.Zero, AppTheme.Bold.ToHfont());
            Assert.NotEqual(IntPtr.Zero, AppTheme.Title.ToHfont());
            Assert.NotEqual(IntPtr.Zero, AppTheme.Mono.ToHfont());
            Assert.NotEqual(IntPtr.Zero, AppTheme.Small.ToHfont());
            Assert.NotEqual(IntPtr.Zero, AppTheme.SmallBold.ToHfont());
        }
        finally
        {
            AppTheme.SetTheme("catppuccin-mocha");
        }
    }

    [Fact]
    public void ThemeFonts_SetFontSize_AllProduceValidHandles()
    {
        try
        {
            // Exercise the min, default, and max font sizes
            foreach (var size in new[] { 7f, 9f, 12f, 16f })
            {
                AppTheme.SetFontSize(size);
                Assert.NotEqual(IntPtr.Zero, AppTheme.Regular.ToHfont());
                Assert.NotEqual(IntPtr.Zero, AppTheme.Bold.ToHfont());
                Assert.NotEqual(IntPtr.Zero, AppTheme.Mono.ToHfont());
                Assert.NotEqual(IntPtr.Zero, AppTheme.SmallBold.ToHfont());
            }
        }
        finally
        {
            AppTheme.SetFontSize(9f);
        }
    }

    [Fact]
    public void ThemeFonts_UsedFontFamilies_AreInstalled()
    {
        // The two font families used by the theme engine
        using var segoe = new System.Drawing.FontFamily("Segoe UI");
        using var consolas = new System.Drawing.FontFamily("Consolas");

        Assert.Equal("Segoe UI", segoe.Name);
        Assert.Equal("Consolas", consolas.Name);
    }

    // ── Helpers ──────────────────────────────────────────────────────────

    /// <summary>
    /// Finds a build output file by scanning known output directories.
    /// Build output goes to %TEMP%\RegiLattice-build\{project}\bin\...,
    /// but the test runner may land in a different path.
    /// </summary>
    private static string FindBuildOutput(string fileName, string? projectName = null)
    {
        projectName ??= Path.GetFileNameWithoutExtension(fileName);

        // Strategy 1: Adjacent assembly location (test runner copies dependencies)
        var testAssemblyDir = Path.GetDirectoryName(typeof(ExecutableValidationTests).Assembly.Location) ?? "";
        var adjacentPath = Path.Combine(testAssemblyDir, fileName);
        if (File.Exists(adjacentPath))
            return adjacentPath;

        // Strategy 2: %TEMP%\RegiLattice-build\{project}\bin\ tree
        var tempRoot = Path.Combine(Path.GetTempPath(), "RegiLattice-build", projectName, "bin");
        if (Directory.Exists(tempRoot))
        {
            var found = Directory.GetFiles(tempRoot, fileName, SearchOption.AllDirectories).FirstOrDefault();
            if (found is not null)
                return found;
        }

        // Strategy 3: Workspace-relative bin/ (fallback for non-redirected builds)
        var workspaceRoot = FindWorkspaceRoot();
        if (workspaceRoot is not null)
        {
            var srcPath = Path.Combine(workspaceRoot, "src", projectName, "bin");
            if (Directory.Exists(srcPath))
            {
                var found = Directory.GetFiles(srcPath, fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (found is not null)
                    return found;
            }
        }

        // Return the most likely path even if it doesn't exist (test will Assert.True on File.Exists)
        return adjacentPath;
    }

    private static string? FindWorkspaceRoot()
    {
        var dir = Path.GetDirectoryName(typeof(ExecutableValidationTests).Assembly.Location);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir, "RegiLattice.sln")))
                return dir;
            dir = Path.GetDirectoryName(dir);
        }
        return null;
    }
}
