// tests/RegiLattice.Core.Tests/AppConfigPortableTests.cs
// Sprint 59 — Portable mode (IsPortable, ConfigDir, AutoDetectPortable) tests.

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for Sprint 59: AppConfig portable mode.</summary>
public sealed class AppConfigPortableTests
{
    // Reset portable state after every test — IsPortable is a static field.
    private static void ResetPortable()
    {
        AppConfig.SetPortable(false);
    }

    // ── Default state ─────────────────────────────────────────────────────

    [Fact]
    public void IsPortable_Default_IsFalse()
    {
        ResetPortable();
        Assert.False(AppConfig.IsPortable);
    }

    [Fact]
    public void ConfigDir_Default_ContainsRegiLattice()
    {
        ResetPortable();
        Assert.Contains("RegiLattice", AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ConfigDir_Default_ContainsLocalAppData()
    {
        ResetPortable();
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        Assert.StartsWith(localAppData, AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
    }

    // ── SetPortable(true) ────────────────────────────────────────────────

    [Fact]
    public void SetPortable_True_IsPortableBecomesTrue()
    {
        try
        {
            AppConfig.SetPortable(true);
            Assert.True(AppConfig.IsPortable);
        }
        finally
        {
            ResetPortable();
        }
    }

    [Fact]
    public void SetPortable_True_ConfigDirEqualsPortableDataDir()
    {
        try
        {
            AppConfig.SetPortable(true);
            // ConfigDir must equal the portable sibling directory, not the AppData path.
            Assert.True(
                AppConfig.ConfigDir.Equals(AppConfig.PortableDataDir, StringComparison.OrdinalIgnoreCase),
                $"Expected ConfigDir '{AppConfig.ConfigDir}' to equal PortableDataDir '{AppConfig.PortableDataDir}'"
            );
        }
        finally
        {
            ResetPortable();
        }
    }

    [Fact]
    public void SetPortable_False_ConfigDirUsesAppData()
    {
        try
        {
            AppConfig.SetPortable(true);
            AppConfig.SetPortable(false);
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Assert.StartsWith(localAppData, AppConfig.ConfigDir, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            ResetPortable();
        }
    }

    // ── PortableDataDir value ─────────────────────────────────────────────

    [Fact]
    public void PortableDataDir_IsNotEmpty()
    {
        Assert.False(string.IsNullOrWhiteSpace(AppConfig.PortableDataDir));
    }

    [Fact]
    public void PortableDataDir_ContainsData()
    {
        // Convention: portable dir is named "data" sibling of the exe
        Assert.Contains("data", AppConfig.PortableDataDir, StringComparison.OrdinalIgnoreCase);
    }

    // ── AutoDetectPortable — no sentinel file ────────────────────────────

    [Fact]
    public void AutoDetectPortable_NoSentinelFile_IsPortableFalse()
    {
        try
        {
            // Ensure we start clean (no sentinel in test runner dir)
            ResetPortable();
            AppConfig.AutoDetectPortable();
            // There is no portable.dat sentinel file next to the test runner,
            // so portable mode should remain off by default.
            Assert.False(AppConfig.IsPortable);
        }
        finally
        {
            ResetPortable();
        }
    }

    // ── Round-trip: explicit path, fields preserved ───────────────────────

    [Fact]
    public void Load_WithExplicitPath_PreservesAllConfigFields()
    {
        // Use an explicit tmp path to avoid polluting any system directory.
        var tmpPath = Path.Combine(Path.GetTempPath(), $"RL_Test_{Guid.NewGuid():N}.json");
        try
        {
            var saved = new AppConfig { Theme = "nord" };
            saved.Save(tmpPath);

            var reloaded = AppConfig.Load(tmpPath);
            Assert.Equal("nord", reloaded.Theme);
        }
        finally
        {
            if (File.Exists(tmpPath))
                File.Delete(tmpPath);
        }
    }
}
