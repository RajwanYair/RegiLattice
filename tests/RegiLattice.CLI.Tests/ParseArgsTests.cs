using RegiLattice.Core.Models;
using Xunit;

namespace RegiLattice.CLI.Tests;

public sealed class ParseArgsTests
{
    // ── Flags (boolean switches) ────────────────────────────────────────

    [Fact]
    public void ParseArgs_NoArgs_ReturnsEmptyCliArgs()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
    }

    [Theory]
    [InlineData("--list", nameof(Program.CliArgs.ShowList))]
    [InlineData("--force", nameof(Program.CliArgs.Force))]
    [InlineData("--gui", nameof(Program.CliArgs.Gui))]
    [InlineData("--menu", nameof(Program.CliArgs.Menu))]
    [InlineData("--dry-run", nameof(Program.CliArgs.DryRun))]
    [InlineData("--doctor", nameof(Program.CliArgs.Doctor))]
    [InlineData("--hwinfo", nameof(Program.CliArgs.HwInfo))]
    [InlineData("--list-profiles", nameof(Program.CliArgs.ListProfiles))]
    [InlineData("--validate", nameof(Program.CliArgs.Validate))]
    [InlineData("--stats", nameof(Program.CliArgs.Stats))]
    [InlineData("--categories", nameof(Program.CliArgs.ShowCategories))]
    [InlineData("--list-categories", nameof(Program.CliArgs.ShowCategories))]
    [InlineData("--tags", nameof(Program.CliArgs.ShowTags))]
    [InlineData("--report", nameof(Program.CliArgs.Report))]
    [InlineData("--check", nameof(Program.CliArgs.Check))]
    [InlineData("--corp-safe", nameof(Program.CliArgs.CorpSafe))]
    [InlineData("--needs-admin", nameof(Program.CliArgs.NeedsAdmin))]
    [InlineData("--no-color", nameof(Program.CliArgs.NoColor))]
    public void ParseArgs_Flag_SetsProperty(string flag, string propertyName)
    {
        var result = Program.ParseArgs([flag]);

        Assert.NotNull(result);
        var prop = typeof(Program.CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.True((bool)prop.GetValue(result)!);
    }

    [Theory]
    [InlineData("-y", nameof(Program.CliArgs.AssumeYes))]
    [InlineData("--assume-yes", nameof(Program.CliArgs.AssumeYes))]
    public void ParseArgs_ShortFlag_SetsProperty(string flag, string propertyName)
    {
        var result = Program.ParseArgs([flag]);

        Assert.NotNull(result);
        var prop = typeof(Program.CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.True((bool)prop.GetValue(result)!);
    }

    // ── Options with values ─────────────────────────────────────────────

    [Theory]
    [InlineData("--search", "telemetry", nameof(Program.CliArgs.Search))]
    [InlineData("--profile", "gaming", nameof(Program.CliArgs.Profile))]
    [InlineData("--config", @"C:\config.json", nameof(Program.CliArgs.ConfigPath))]
    [InlineData("--snapshot", "snap.json", nameof(Program.CliArgs.Snapshot))]
    [InlineData("--restore", "snap.json", nameof(Program.CliArgs.Restore))]
    [InlineData("--export-json", "out.json", nameof(Program.CliArgs.ExportJson))]
    [InlineData("--export-reg", "out.reg", nameof(Program.CliArgs.ExportReg))]
    [InlineData("--import-json", "in.json", nameof(Program.CliArgs.ImportJson))]
    [InlineData("--diff", "diff.json", nameof(Program.CliArgs.Diff))]
    [InlineData("--category", "Privacy", nameof(Program.CliArgs.Category))]
    [InlineData("--output", "json", nameof(Program.CliArgs.OutputFormat))]
    [InlineData("--html", "report.html", nameof(Program.CliArgs.HtmlPath))]
    public void ParseArgs_OptionWithValue_SetsProperty(string option, string value, string propertyName)
    {
        var result = Program.ParseArgs([option, value]);

        Assert.NotNull(result);
        var prop = typeof(Program.CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.Equal(value, (string?)prop.GetValue(result));
    }

    [Fact]
    public void ParseArgs_OptionWithoutValue_DoesNotThrow()
    {
        // --search is the last arg, no value follows — should not crash
        var result = Program.ParseArgs(["--search"]);
        Assert.NotNull(result);
        Assert.Null(result.Search);
    }

    // ── Scope parsing ───────────────────────────────────────────────────

    [Theory]
    [InlineData("user", TweakScope.User)]
    [InlineData("machine", TweakScope.Machine)]
    [InlineData("both", TweakScope.Both)]
    [InlineData("USER", TweakScope.User)]
    [InlineData("Machine", TweakScope.Machine)]
    public void ParseArgs_Scope_ParsesCorrectly(string scopeArg, TweakScope expected)
    {
        var result = Program.ParseArgs(["--scope", scopeArg]);

        Assert.NotNull(result);
        Assert.Equal(expected, result.ScopeFilter);
    }

    [Fact]
    public void ParseArgs_Scope_InvalidValue_ReturnsNull()
    {
        var result = Program.ParseArgs(["--scope", "invalid"]);

        Assert.NotNull(result);
        Assert.Null(result.ScopeFilter);
    }

    // ── MinBuild parsing ────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_MinBuild_ParsesInteger()
    {
        var result = Program.ParseArgs(["--min-build", "22000"]);

        Assert.NotNull(result);
        Assert.Equal(22000, result.MinBuild);
    }

    [Fact]
    public void ParseArgs_MinBuild_InvalidNumber_KeepsDefault()
    {
        var result = Program.ParseArgs(["--min-build", "abc"]);

        Assert.NotNull(result);
        Assert.Equal(0, result.MinBuild);
    }

    // ── Snapshot diff (dual argument) ───────────────────────────────────

    [Fact]
    public void ParseArgs_SnapshotDiff_ParsesBothPaths()
    {
        var result = Program.ParseArgs(["--snapshot-diff", "a.json", "b.json"]);

        Assert.NotNull(result);
        Assert.Equal("a.json", result.SnapshotDiffA);
        Assert.Equal("b.json", result.SnapshotDiffB);
    }

    [Fact]
    public void ParseArgs_SnapshotDiff_NotEnoughArgs_DoesNotCrash()
    {
        var result = Program.ParseArgs(["--snapshot-diff", "a.json"]);

        Assert.NotNull(result);
        Assert.Null(result.SnapshotDiffA);
        Assert.Null(result.SnapshotDiffB);
    }

    // ── Positional arguments (mode + tweak) ─────────────────────────────

    [Theory]
    [InlineData("apply")]
    [InlineData("remove")]
    [InlineData("status")]
    public void ParseArgs_Mode_ParsesPositional(string mode)
    {
        var result = Program.ParseArgs([mode]);

        Assert.NotNull(result);
        Assert.Equal(mode, result.Mode);
    }

    [Fact]
    public void ParseArgs_ModeAndTweak_ParsesBoth()
    {
        var result = Program.ParseArgs(["apply", "priv-disable-telemetry"]);

        Assert.NotNull(result);
        Assert.Equal("apply", result.Mode);
        Assert.Equal("priv-disable-telemetry", result.Tweak);
    }

    [Fact]
    public void ParseArgs_UnknownPositional_WithoutMode_Ignored()
    {
        var result = Program.ParseArgs(["unknown-word"]);

        Assert.NotNull(result);
        Assert.Null(result.Mode);
        Assert.Null(result.Tweak);
    }

    // ── Combined arguments ──────────────────────────────────────────────

    [Fact]
    public void ParseArgs_MultipleFlags_AllSet()
    {
        var result = Program.ParseArgs(["--list", "--force", "--dry-run"]);

        Assert.NotNull(result);
        Assert.True(result.ShowList);
        Assert.True(result.Force);
        Assert.True(result.DryRun);
    }

    [Fact]
    public void ParseArgs_FlagsAndOptions_Combined()
    {
        var result = Program.ParseArgs(["--force", "--dry-run", "--search", "telemetry", "--scope", "user", "--min-build", "22000"]);

        Assert.NotNull(result);
        Assert.True(result.Force);
        Assert.True(result.DryRun);
        Assert.Equal("telemetry", result.Search);
        Assert.Equal(TweakScope.User, result.ScopeFilter);
        Assert.Equal(22000, result.MinBuild);
    }

    [Fact]
    public void ParseArgs_ModeWithFlags_Combined()
    {
        var result = Program.ParseArgs(["apply", "perf-disable-animations", "--force", "--dry-run"]);

        Assert.NotNull(result);
        Assert.Equal("apply", result.Mode);
        Assert.Equal("perf-disable-animations", result.Tweak);
        Assert.True(result.Force);
        Assert.True(result.DryRun);
    }

    // ── Default values ──────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Defaults_AreCorrect()
    {
        var result = Program.ParseArgs([]);

        Assert.NotNull(result);
        Assert.Null(result.Mode);
        Assert.Null(result.Tweak);
        Assert.False(result.ShowList);
        Assert.False(result.Force);
        Assert.False(result.Gui);
        Assert.False(result.DryRun);
        Assert.Null(result.Search);
        Assert.Null(result.Profile);
        Assert.Null(result.ScopeFilter);
        Assert.Equal(0, result.MinBuild);
        Assert.Equal("table", result.OutputFormat);
    }

    [Fact]
    public void ParseArgs_DependsOn_SetsProperty()
    {
        var result = Program.ParseArgs(["--depends-on", "perf-disable-animations"]);
        Assert.NotNull(result);
        Assert.Equal("perf-disable-animations", result.DependsOn);
    }

    [Fact]
    public void ParseArgs_NoColor_DefaultsFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.NoColor);
    }

    [Fact]
    public void ParseArgs_DependsOn_DefaultsNull()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Null(result.DependsOn);
    }
}
