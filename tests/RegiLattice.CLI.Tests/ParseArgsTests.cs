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
    [InlineData("--list", nameof(CliArgs.ShowList))]
    [InlineData("--force", nameof(CliArgs.Force))]
    [InlineData("--gui", nameof(CliArgs.Gui))]
    [InlineData("--menu", nameof(CliArgs.Menu))]
    [InlineData("--dry-run", nameof(CliArgs.DryRun))]
    [InlineData("--doctor", nameof(CliArgs.Doctor))]
    [InlineData("--hwinfo", nameof(CliArgs.HwInfo))]
    [InlineData("--list-profiles", nameof(CliArgs.ListProfiles))]
    [InlineData("--validate", nameof(CliArgs.Validate))]
    [InlineData("--stats", nameof(CliArgs.Stats))]
    [InlineData("--categories", nameof(CliArgs.ShowCategories))]
    [InlineData("--list-categories", nameof(CliArgs.ShowCategories))]
    [InlineData("--tags", nameof(CliArgs.ShowTags))]
    [InlineData("--report", nameof(CliArgs.Report))]
    [InlineData("--check", nameof(CliArgs.Check))]
    [InlineData("--corp-safe", nameof(CliArgs.CorpSafe))]
    [InlineData("--needs-admin", nameof(CliArgs.NeedsAdmin))]
    [InlineData("--no-color", nameof(CliArgs.NoColor))]
    public void ParseArgs_Flag_SetsProperty(string flag, string propertyName)
    {
        var result = Program.ParseArgs([flag]);

        Assert.NotNull(result);
        var prop = typeof(CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.True((bool)prop.GetValue(result)!);
    }

    [Theory]
    [InlineData("-y", nameof(CliArgs.AssumeYes))]
    [InlineData("--assume-yes", nameof(CliArgs.AssumeYes))]
    public void ParseArgs_ShortFlag_SetsProperty(string flag, string propertyName)
    {
        var result = Program.ParseArgs([flag]);

        Assert.NotNull(result);
        var prop = typeof(CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.True((bool)prop.GetValue(result)!);
    }

    // ── Options with values ─────────────────────────────────────────────

    [Theory]
    [InlineData("--search", "telemetry", nameof(CliArgs.Search))]
    [InlineData("--profile", "gaming", nameof(CliArgs.Profile))]
    [InlineData("--config", @"C:\config.json", nameof(CliArgs.ConfigPath))]
    [InlineData("--snapshot", "snap.json", nameof(CliArgs.Snapshot))]
    [InlineData("--restore", "snap.json", nameof(CliArgs.Restore))]
    [InlineData("--export-json", "out.json", nameof(CliArgs.ExportJson))]
    [InlineData("--export-reg", "out.reg", nameof(CliArgs.ExportReg))]
    [InlineData("--import-json", "in.json", nameof(CliArgs.ImportJson))]
    [InlineData("--diff", "diff.json", nameof(CliArgs.Diff))]
    [InlineData("--category", "Privacy", nameof(CliArgs.Category))]
    [InlineData("--output", "json", nameof(CliArgs.OutputFormat))]
    [InlineData("--html", "report.html", nameof(CliArgs.HtmlPath))]    public void ParseArgs_OptionWithValue_SetsProperty(string option, string value, string propertyName)
    {
        var result = Program.ParseArgs([option, value]);

        Assert.NotNull(result);
        var prop = typeof(CliArgs).GetProperty(propertyName);
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

    // ── Update mode ─────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_UpdateMode_ParsesPositional()
    {
        var result = Program.ParseArgs(["update", "perf-disable-animations"]);
        Assert.NotNull(result);
        Assert.Equal("update", result.Mode);
        Assert.Equal("perf-disable-animations", result.Tweak);
    }

    [Fact]
    public void ParseArgs_UpdateModeWithFlags_Combined()
    {
        var result = Program.ParseArgs(["update", "priv-disable-telemetry", "--force", "--dry-run"]);
        Assert.NotNull(result);
        Assert.Equal("update", result.Mode);
        Assert.Equal("priv-disable-telemetry", result.Tweak);
        Assert.True(result.Force);
        Assert.True(result.DryRun);
    }

    // ── Marketplace ─────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Marketplace_ParsesCommandAndArg()
    {
        var result = Program.ParseArgs(["--marketplace", "install", "my-pack"]);
        Assert.NotNull(result);
        Assert.Equal("install", result.Marketplace);
        Assert.Equal("my-pack", result.MarketplaceArg);
    }

    // ── Help / Version (returns null) ───────────────────────────────────

    [Fact]
    public void ParseArgs_Help_ReturnsNull()
    {
        var result = Program.ParseArgs(["--help"]);
        Assert.Null(result);
    }

    [Fact]
    public void ParseArgs_HelpShort_ReturnsNull()
    {
        var result = Program.ParseArgs(["-h"]);
        Assert.Null(result);
    }

    [Fact]
    public void ParseArgs_Version_ReturnsNull()
    {
        var result = Program.ParseArgs(["--version"]);
        Assert.Null(result);
    }

    [Fact]
    public void ParseArgs_VersionShort_ReturnsNull()
    {
        var result = Program.ParseArgs(["-V"]);
        Assert.Null(result);
    }

    // ── Options without value (graceful null) ───────────────────────────

    [Theory]
    [InlineData("--profile", nameof(CliArgs.Profile))]
    [InlineData("--config", nameof(CliArgs.ConfigPath))]
    [InlineData("--snapshot", nameof(CliArgs.Snapshot))]
    [InlineData("--restore", nameof(CliArgs.Restore))]
    [InlineData("--export-json", nameof(CliArgs.ExportJson))]
    [InlineData("--export-reg", nameof(CliArgs.ExportReg))]
    [InlineData("--import-json", nameof(CliArgs.ImportJson))]
    [InlineData("--diff", nameof(CliArgs.Diff))]
    [InlineData("--category", nameof(CliArgs.Category))]
    [InlineData("--html", nameof(CliArgs.HtmlPath))]
    [InlineData("--depends-on", nameof(CliArgs.DependsOn))]
    public void ParseArgs_OptionWithoutValue_StaysNull(string option, string propertyName)
    {
        var result = Program.ParseArgs([option]);
        Assert.NotNull(result);
        var prop = typeof(CliArgs).GetProperty(propertyName);
        Assert.NotNull(prop);
        Assert.Null((string?)prop.GetValue(result));
    }

    // ── MinBuild edge cases ─────────────────────────────────────────────

    [Fact]
    public void ParseArgs_MinBuild_NegativeValue_Parsed()
    {
        var result = Program.ParseArgs(["--min-build", "-1"]);
        Assert.NotNull(result);
        Assert.Equal(-1, result.MinBuild);
    }

    [Fact]
    public void ParseArgs_MinBuild_Overflow_KeepsDefault()
    {
        var result = Program.ParseArgs(["--min-build", "99999999999"]);
        Assert.NotNull(result);
        Assert.Equal(0, result.MinBuild);
    }

    [Fact]
    public void ParseArgs_MinBuild_WithoutValue_KeepsDefault()
    {
        var result = Program.ParseArgs(["--min-build"]);
        Assert.NotNull(result);
        Assert.Equal(0, result.MinBuild);
    }

    // ── Scope edge cases ────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Scope_AllCaps_ParsesCorrectly()
    {
        var result = Program.ParseArgs(["--scope", "BOTH"]);
        Assert.NotNull(result);
        Assert.Equal(TweakScope.Both, result.ScopeFilter);
    }

    [Fact]
    public void ParseArgs_Scope_WithoutValue_StaysNull()
    {
        var result = Program.ParseArgs(["--scope"]);
        Assert.NotNull(result);
        Assert.Null(result.ScopeFilter);
    }

    // ── Marketplace edge cases ──────────────────────────────────────────

    [Fact]
    public void ParseArgs_Marketplace_CommandOnly_NoPackageName()
    {
        var result = Program.ParseArgs(["--marketplace", "list"]);
        Assert.NotNull(result);
        Assert.Equal("list", result.Marketplace);
        Assert.Null(result.MarketplaceArg);
    }

    [Fact]
    public void ParseArgs_Marketplace_ArgStartingWithDash_NotConsumed()
    {
        var result = Program.ParseArgs(["--marketplace", "install", "--force"]);
        Assert.NotNull(result);
        Assert.Equal("install", result.Marketplace);
        Assert.Null(result.MarketplaceArg);
        Assert.True(result.Force);
    }

    // ── SnapshotDiff edge cases ─────────────────────────────────────────

    [Fact]
    public void ParseArgs_SnapshotDiff_NoArgs_DoesNotCrash()
    {
        var result = Program.ParseArgs(["--snapshot-diff"]);
        Assert.NotNull(result);
        Assert.Null(result.SnapshotDiffA);
        Assert.Null(result.SnapshotDiffB);
    }

    // ── Flag order independence ─────────────────────────────────────────

    [Fact]
    public void ParseArgs_OptionsBeforeMode_AllParsed()
    {
        var result = Program.ParseArgs(["--force", "--dry-run", "apply", "test-tweak"]);
        Assert.NotNull(result);
        Assert.True(result.Force);
        Assert.True(result.DryRun);
        Assert.Equal("apply", result.Mode);
        Assert.Equal("test-tweak", result.Tweak);
    }

    // ── Duplicate flags ─────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_DuplicateSearch_LastWins()
    {
        var result = Program.ParseArgs(["--search", "first", "--search", "second"]);
        Assert.NotNull(result);
        Assert.Equal("second", result.Search);
    }

    // ── OutputFormat default ─────────────────────────────────────────────

    [Fact]
    public void ParseArgs_OutputFormat_CustomValue()
    {
        var result = Program.ParseArgs(["--output", "csv"]);
        Assert.NotNull(result);
        Assert.Equal("csv", result.OutputFormat);
    }
}

/// <summary>Tests for the ConsoleColorizer utility.</summary>
public sealed class ConsoleColorizerTests
{
    [Fact]
    public void Green_NoColor_ReturnsPlainText()
    {
        ConsoleColorizer.NoColor = true;
        Assert.Equal("hello", ConsoleColorizer.Green("hello"));
    }

    [Fact]
    public void Green_WithColor_ContainsAnsiCodes()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Green("hello");
        Assert.Contains("\x1b[32m", result);
        Assert.Contains("hello", result);
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Red_NoColor_ReturnsPlainText()
    {
        ConsoleColorizer.NoColor = true;
        Assert.Equal("error", ConsoleColorizer.Red("error"));
    }

    [Fact]
    public void Yellow_NoColor_ReturnsPlainText()
    {
        ConsoleColorizer.NoColor = true;
        Assert.Equal("warn", ConsoleColorizer.Yellow("warn"));
    }

    [Fact]
    public void Dim_NoColor_ReturnsPlainText()
    {
        ConsoleColorizer.NoColor = true;
        Assert.Equal("dim", ConsoleColorizer.Dim("dim"));
    }

    [Theory]
    [InlineData(TweakResult.Applied)]
    [InlineData(TweakResult.NotApplied)]
    [InlineData(TweakResult.Unknown)]
    [InlineData(TweakResult.Error)]
    [InlineData(TweakResult.SkippedCorp)]
    [InlineData(TweakResult.SkippedBuild)]
    [InlineData(TweakResult.SkippedHw)]
    public void ColourisedStatus_NoColor_ContainsStatusName(TweakResult status)
    {
        ConsoleColorizer.NoColor = true;
        var result = ConsoleColorizer.ColourisedStatus(status);
        Assert.Equal(status.ToString(), result);
    }

    [Fact]
    public void ColourisedStatus_Applied_IsGreen()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.Applied);
        Assert.Contains("\x1b[32m", result); // green ANSI code
    }

    [Fact]
    public void ColourisedStatus_Error_IsRed()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.Error);
        Assert.Contains("\x1b[31m", result); // red ANSI code
    }

    [Fact]
    public void ColourisedStatus_SkippedCorp_IsYellow()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.SkippedCorp);
        Assert.Contains("\x1b[33m", result); // yellow ANSI code
    }

    [Fact]
    public void Red_WithColor_ContainsAnsiCodes()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Red("error");
        Assert.Contains("\x1b[31m", result);
        Assert.Contains("error", result);
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Yellow_WithColor_ContainsAnsiCodes()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Yellow("warn");
        Assert.Contains("\x1b[33m", result);
        Assert.Contains("warn", result);
        Assert.Contains("\x1b[0m", result);
    }

    [Fact]
    public void Dim_WithColor_ContainsAnsiDimCode()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Dim("info");
        Assert.Contains("\x1b[90m", result);
        Assert.Contains("info", result);
    }
}
