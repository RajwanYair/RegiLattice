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
    [InlineData("--html", "report.html", nameof(CliArgs.HtmlPath))]
    public void ParseArgs_OptionWithValue_SetsProperty(string option, string value, string propertyName)
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

    [Fact]
    public void ParseArgs_NewPack_SetsNewPackProperty()
    {
        var result = Program.ParseArgs(["--new-pack", "my-custom-pack"]);
        Assert.NotNull(result);
        Assert.NotNull(result.NewPack);
        Assert.Equal("my-custom-pack", result.NewPack);
    }

    // ── Portable / Silent / LogFile ─────────────────────────────────────

    [Fact]
    public void ParseArgs_Portable_SetsPortableTrue()
    {
        var a = Program.ParseArgs(["--portable"]);
        Assert.NotNull(a);
        Assert.True(a.Portable);
    }

    [Fact]
    public void ParseArgs_Silent_SetsSilentTrue()
    {
        var a = Program.ParseArgs(["--silent"]);
        Assert.NotNull(a);
        Assert.True(a.Silent);
    }

    [Fact]
    public void ParseArgs_LogFile_SetsLogFilePath()
    {
        var a = Program.ParseArgs(["--log-file", "output.json"]);
        Assert.NotNull(a);
        Assert.Equal("output.json", a.LogFile);
    }

    [Fact]
    public void ParseArgs_Portable_DefaultsFalse()
    {
        var a = Program.ParseArgs([]);
        Assert.NotNull(a);
        Assert.False(a.Portable);
    }

    [Fact]
    public void ParseArgs_Silent_DefaultsFalse()
    {
        var a = Program.ParseArgs([]);
        Assert.NotNull(a);
        Assert.False(a.Silent);
    }

    // ── Favorites ───────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Favorites_SetsShowFavoritesTrue()
    {
        var a = Program.ParseArgs(["--favorites"]);
        Assert.NotNull(a);
        Assert.True(a.ShowFavorites);
    }

    [Fact]
    public void ParseArgs_FavoriteAdd_SetsFavoriteAddId()
    {
        var a = Program.ParseArgs(["--favorite-add", "priv-disable-telemetry"]);
        Assert.NotNull(a);
        Assert.Equal("priv-disable-telemetry", a.FavoriteAdd);
    }

    [Fact]
    public void ParseArgs_FavoriteRemove_SetsFavoriteRemoveId()
    {
        var a = Program.ParseArgs(["--favorite-remove", "priv-disable-telemetry"]);
        Assert.NotNull(a);
        Assert.Equal("priv-disable-telemetry", a.FavoriteRemove);
    }

    [Fact]
    public void ParseArgs_FavoriteAdd_WithoutValue_StaysNull()
    {
        var a = Program.ParseArgs(["--favorite-add"]);
        Assert.NotNull(a);
        Assert.Null(a.FavoriteAdd);
    }

    // ── History ─────────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_History_SetsShowHistoryTrue()
    {
        var a = Program.ParseArgs(["--history"]);
        Assert.NotNull(a);
        Assert.True(a.ShowHistory);
    }

    [Fact]
    public void ParseArgs_History_DefaultCount_IsTwenty()
    {
        var a = Program.ParseArgs(["--history"]);
        Assert.NotNull(a);
        Assert.Equal(20, a.HistoryCount);
    }

    [Fact]
    public void ParseArgs_History_WithCount_SetsHistoryCount()
    {
        var a = Program.ParseArgs(["--history", "50"]);
        Assert.NotNull(a);
        Assert.True(a.ShowHistory);
        Assert.Equal(50, a.HistoryCount);
    }

    [Fact]
    public void ParseArgs_HistoryCount_DefaultIsTwenty()
    {
        var a = Program.ParseArgs([]);
        Assert.NotNull(a);
        Assert.Equal(20, a.HistoryCount);
    }

    // ── Compliance ──────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Compliance_SetsCompliancePath()
    {
        var a = Program.ParseArgs(["--compliance", "snapshot.json"]);
        Assert.NotNull(a);
        Assert.Equal("snapshot.json", a.Compliance);
    }

    [Fact]
    public void ParseArgs_ComplianceHistory_SetsComplianceHistoryTrue()
    {
        var a = Program.ParseArgs(["--compliance-history"]);
        Assert.NotNull(a);
        Assert.True(a.ComplianceHistory);
    }

    [Fact]
    public void ParseArgs_ComplianceReport_SetsComplianceReportMode()
    {
        var a = Program.ParseArgs(["--compliance-report", "auto"]);
        Assert.NotNull(a);
        Assert.Equal("auto", a.ComplianceReportMode);
    }

    [Fact]
    public void ParseArgs_ComplianceHistory_DefaultsFalse()
    {
        var a = Program.ParseArgs([]);
        Assert.NotNull(a);
        Assert.False(a.ComplianceHistory);
    }

    // ── Export GPO / Manager ────────────────────────────────────────────

    [Fact]
    public void ParseArgs_ExportGpo_SetsExportGpoPath()
    {
        var a = Program.ParseArgs(["--export-gpo", "policy.admx"]);
        Assert.NotNull(a);
        Assert.Equal("policy.admx", a.ExportGpo);
    }

    [Fact]
    public void ParseArgs_Manager_SetsManagerTarget()
    {
        var a = Program.ParseArgs(["--manager", "scheduledtweaks"]);
        Assert.NotNull(a);
        Assert.Equal("scheduledtweaks", a.Manager);
    }

    // ── Config export / import ──────────────────────────────────────────

    [Fact]
    public void ParseArgs_ExportConfig_SetsExportConfigPath()
    {
        var a = Program.ParseArgs(["--export-config", "config.json"]);
        Assert.NotNull(a);
        Assert.Equal("config.json", a.ExportConfig);
    }

    [Fact]
    public void ParseArgs_ImportConfig_SetsImportConfigPath()
    {
        var a = Program.ParseArgs(["--import-config", "config.json"]);
        Assert.NotNull(a);
        Assert.Equal("config.json", a.ImportConfig);
    }

    // ── HTML report ─────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_HtmlReport_SetsHtmlReportPath()
    {
        var a = Program.ParseArgs(["--html-report", "report.html"]);
        Assert.NotNull(a);
        Assert.Equal("report.html", a.HtmlReport);
    }

    // ── PowerShell module generation ────────────────────────────────────

    [Fact]
    public void ParseArgs_GeneratePsModule_SetsGeneratePsModuleTrue()
    {
        var a = Program.ParseArgs(["--generate-ps-module"]);
        Assert.NotNull(a);
        Assert.True(a.GeneratePsModule);
    }

    [Fact]
    public void ParseArgs_GeneratePsModule_WithDir_SetsPsModuleOutput()
    {
        var a = Program.ParseArgs(["--generate-ps-module", @"C:\modules"]);
        Assert.NotNull(a);
        Assert.True(a.GeneratePsModule);
        Assert.Equal(@"C:\modules", a.PsModuleOutput);
    }

    [Fact]
    public void ParseArgs_GeneratePsModule_WithFlag_DoesNotConsumeFlag()
    {
        // The next arg starts with '--' so should NOT be treated as output dir
        var a = Program.ParseArgs(["--generate-ps-module", "--force"]);
        Assert.NotNull(a);
        Assert.True(a.GeneratePsModule);
        Assert.Null(a.PsModuleOutput);
        Assert.True(a.Force);
    }
}

/// <summary>Tests for the ConsoleColorizer utility.</summary>
[Collection("ConsoleColorizer")]
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

    [Fact]
    public void ColourisedStatus_NotApplied_IsRed()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.NotApplied);
        Assert.Contains("\x1b[31m", result);
    }

    [Fact]
    public void ColourisedStatus_SkippedBuild_IsYellow()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.SkippedBuild);
        Assert.Contains("\x1b[33m", result);
    }

    [Fact]
    public void ColourisedStatus_SkippedHw_IsYellow()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.SkippedHw);
        Assert.Contains("\x1b[33m", result);
    }

    [Fact]
    public void ColourisedStatus_Unknown_IsDim()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.ColourisedStatus(TweakResult.Unknown);
        Assert.Contains("\x1b[90m", result);
    }

    [Fact]
    public void Green_EmptyString_ReturnsAnsiWrapped()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Green("");
        Assert.StartsWith("\x1b[32m", result);
        Assert.EndsWith("\x1b[0m", result);
    }
}

// ── Favorites & History CLI argument tests ──────────────────────────────

public sealed class FavoritesAndHistoryParseTests
{
    [Fact]
    public void ParseArgs_ExportConfig_SetsPath()
    {
        var result = Program.ParseArgs(["--export-config", "my-config.json"]);
        Assert.NotNull(result);
        Assert.Equal("my-config.json", result.ExportConfig);
    }

    [Fact]
    public void ParseArgs_ImportConfig_SetsPath()
    {
        var result = Program.ParseArgs(["--import-config", "my-config.json"]);
        Assert.NotNull(result);
        Assert.Equal("my-config.json", result.ImportConfig);
    }

    [Fact]
    public void ParseArgs_Favorites_SetsFlag()
    {
        var result = Program.ParseArgs(["--favorites"]);
        Assert.NotNull(result);
        Assert.True(result.ShowFavorites);
    }

    [Fact]
    public void ParseArgs_FavoriteAdd_SetsId()
    {
        var result = Program.ParseArgs(["--favorite-add", "perf-disable-animations"]);
        Assert.NotNull(result);
        Assert.Equal("perf-disable-animations", result.FavoriteAdd);
    }

    [Fact]
    public void ParseArgs_FavoriteRemove_SetsId()
    {
        var result = Program.ParseArgs(["--favorite-remove", "perf-disable-animations"]);
        Assert.NotNull(result);
        Assert.Equal("perf-disable-animations", result.FavoriteRemove);
    }

    [Fact]
    public void ParseArgs_History_SetsFlag()
    {
        var result = Program.ParseArgs(["--history"]);
        Assert.NotNull(result);
        Assert.True(result.ShowHistory);
    }

    [Fact]
    public void ParseArgs_HistoryCount_SetsValue()
    {
        var result = Program.ParseArgs(["--history", "50"]);
        Assert.NotNull(result);
        Assert.True(result.ShowHistory);
        Assert.Equal(50, result.HistoryCount);
    }

    [Fact]
    public void ParseArgs_HistoryCount_Default_Is20()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Equal(20, result.HistoryCount);
    }
}

// ── Sprint 24: Additional CLI arg parsing edge cases ─────────────────────

public sealed class CliArgEdgeCaseTests
{
    [Fact]
    public void ParseArgs_AllDefaults_NullableStringsAreNull()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Null(result.Search);
        Assert.Null(result.Category);
        Assert.Null(result.Profile);
        Assert.Null(result.Snapshot);
        Assert.Null(result.Restore);
        Assert.Null(result.ExportJson);
        Assert.Null(result.ExportReg);
        Assert.Null(result.ImportJson);
    }

    [Fact]
    public void ParseArgs_DryRun_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.DryRun);
    }

    [Fact]
    public void ParseArgs_Force_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.Force);
    }

    [Fact]
    public void ParseArgs_ShowList_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.ShowList);
    }

    [Fact]
    public void ParseArgs_ShowCategories_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.ShowCategories);
    }

    [Fact]
    public void ParseArgs_ShowTags_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.ShowTags);
    }

    [Fact]
    public void ParseArgs_Validate_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.Validate);
    }

    [Fact]
    public void ParseArgs_Stats_DefaultFalse()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.False(result.Stats);
    }

    [Theory]
    [InlineData("--list")]
    [InlineData("--show-categories")]
    [InlineData("--show-tags")]
    [InlineData("--validate")]
    [InlineData("--stats")]
    [InlineData("--dry-run")]
    [InlineData("--force")]
    [InlineData("--no-color")]
    [InlineData("--favorites")]
    [InlineData("--history")]
    public void ParseArgs_EachInfoFlag_DoesNotReturnNull(string flag)
    {
        var result = Program.ParseArgs([flag]);
        Assert.NotNull(result);
    }

    [Fact]
    public void ParseArgs_MultipleSearchFlags_LastWins()
    {
        var result = Program.ParseArgs(["--search", "first", "--search", "second"]);
        Assert.NotNull(result);
        Assert.Equal("second", result.Search);
    }

    [Fact]
    public void ParseArgs_CategoryWithAction_ParsesBoth()
    {
        var result = Program.ParseArgs(["--category", "Privacy", "apply"]);
        Assert.NotNull(result);
        Assert.Equal("Privacy", result.Category);
    }

    [Fact]
    public void ParseArgs_Profile_SetsFlagAndName()
    {
        var result = Program.ParseArgs(["--profile", "gaming"]);
        Assert.NotNull(result);
        Assert.Equal("gaming", result.Profile);
    }

    // ── Sprint 127 — user profile CRUD args ─────────────────────────────

    [Fact]
    public void ParseArgs_ProfileCreate_SetsName()
    {
        var result = Program.ParseArgs(["--profile-create", "my-work"]);
        Assert.NotNull(result);
        Assert.Equal("my-work", result.ProfileCreate);
    }

    [Fact]
    public void ParseArgs_ProfileCreate_WithTweaksAndDesc_SetsAll()
    {
        var result = Program.ParseArgs(
            ["--profile-create", "my-work", "--profile-tweaks", "perf-disable-animations,priv-disable-telemetry", "--profile-desc", "Work setup"]
        );
        Assert.NotNull(result);
        Assert.Equal("my-work", result.ProfileCreate);
        Assert.Equal("perf-disable-animations,priv-disable-telemetry", result.ProfileTweaks);
        Assert.Equal("Work setup", result.ProfileDesc);
    }

    [Fact]
    public void ParseArgs_ProfileDelete_SetsName()
    {
        var result = Program.ParseArgs(["--profile-delete", "old-profile"]);
        Assert.NotNull(result);
        Assert.Equal("old-profile", result.ProfileDelete);
    }

    [Fact]
    public void ParseArgs_ProfileDelete_WithoutValue_StaysNull()
    {
        var result = Program.ParseArgs(["--profile-delete"]);
        Assert.NotNull(result);
        Assert.Null(result.ProfileDelete);
    }

    [Fact]
    public void ParseArgs_ProfileClone_SetsBothNames_AndIsProfileCloneTrue()
    {
        var result = Program.ParseArgs(["--profile-clone", "source-prof", "dest-prof"]);
        Assert.NotNull(result);
        Assert.Equal("source-prof", result.ProfileFrom);
        Assert.Equal("dest-prof", result.Profile);
        Assert.True(result.IsProfileClone);
        Assert.False(result.IsProfileRename);
    }

    [Fact]
    public void ParseArgs_ProfileRename_SetsBothNames_AndIsProfileRenameTrue()
    {
        var result = Program.ParseArgs(["--profile-rename", "old-name", "new-name"]);
        Assert.NotNull(result);
        Assert.Equal("old-name", result.ProfileFrom);
        Assert.Equal("new-name", result.Profile);
        Assert.True(result.IsProfileRename);
        Assert.False(result.IsProfileClone);
    }

    [Fact]
    public void ParseArgs_ListUserProfiles_SetsFlag()
    {
        var result = Program.ParseArgs(["--list-user-profiles"]);
        Assert.NotNull(result);
        Assert.True(result.ListUserProfiles);
    }

    [Fact]
    public void ParseArgs_ProfileTweaks_SetsValue()
    {
        var result = Program.ParseArgs(["--profile-tweaks", "a,b,c"]);
        Assert.NotNull(result);
        Assert.Equal("a,b,c", result.ProfileTweaks);
    }

    [Fact]
    public void ParseArgs_ProfileDesc_SetsValue()
    {
        var result = Program.ParseArgs(["--profile-desc", "My description"]);
        Assert.NotNull(result);
        Assert.Equal("My description", result.ProfileDesc);
    }

    [Fact]
    public void ParseArgs_ProfileCreate_Default_ProfileCreateIsNull()
    {
        var result = Program.ParseArgs(["--list"]);
        Assert.NotNull(result);
        Assert.Null(result.ProfileCreate);
        Assert.Null(result.ProfileDelete);
        Assert.False(result.IsProfileClone);
        Assert.False(result.IsProfileRename);
        Assert.False(result.ListUserProfiles);
    }
}

// ── Sprint 24: ConsoleColorizer additional coverage ───────────────────────

/// <summary>Sprint-era ConsoleColorizer tests — share the same collection to prevent
/// race conditions on the shared NoColor static property.</summary>
[Collection("ConsoleColorizer")]
public sealed class ConsoleColorizerSprintTests
{
    [Theory]
    [InlineData(TweakResult.Applied)]
    [InlineData(TweakResult.NotApplied)]
    [InlineData(TweakResult.Unknown)]
    [InlineData(TweakResult.Error)]
    [InlineData(TweakResult.SkippedCorp)]
    [InlineData(TweakResult.SkippedBuild)]
    [InlineData(TweakResult.SkippedHw)]
    public void ColourisedStatus_WithColor_ReturnsNonEmptyString(TweakResult result)
    {
        ConsoleColorizer.NoColor = false;
        var text = ConsoleColorizer.ColourisedStatus(result);
        Assert.NotEmpty(text);
    }

    [Theory]
    [InlineData(TweakResult.Applied)]
    [InlineData(TweakResult.NotApplied)]
    [InlineData(TweakResult.Unknown)]
    [InlineData(TweakResult.Error)]
    [InlineData(TweakResult.SkippedCorp)]
    [InlineData(TweakResult.SkippedBuild)]
    [InlineData(TweakResult.SkippedHw)]
    public void ColourisedStatus_NoColor_ContainsResultName(TweakResult result)
    {
        ConsoleColorizer.NoColor = true;
        try
        {
            var text = ConsoleColorizer.ColourisedStatus(result);
            Assert.Contains(result.ToString(), text, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            ConsoleColorizer.NoColor = false;
        }
    }

    [Fact]
    public void Green_LongString_ContainsOriginalText()
    {
        ConsoleColorizer.NoColor = false;
        var long_ = new string('a', 200);
        var result = ConsoleColorizer.Green(long_);
        Assert.Contains(long_, result);
    }

    [Fact]
    public void Dim_WithColor_StartsWith_ESC()
    {
        ConsoleColorizer.NoColor = false;
        var result = ConsoleColorizer.Dim("test");
        Assert.StartsWith("\u001b[", result);
    }

    [Fact]
    public void Red_NoColor_ReturnsSameText()
    {
        ConsoleColorizer.NoColor = true;
        try
        {
            var input = "error message here";
            Assert.Equal(input, ConsoleColorizer.Red(input));
        }
        finally
        {
            ConsoleColorizer.NoColor = false;
        }
    }
}

// ── Compliance, ExportGpo, Manager argument parsing ───────────────────────

/// <summary>
/// Tests for CLI arguments added to support compliance reporting, GPO export,
/// and package manager selection — properties present in <see cref="CliArgs"/>
/// but not previously covered by any test class.
/// </summary>
public sealed class ComplianceAndManagerParseTests
{
    // ── Default values ────────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Compliance_DefaultsNull()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Null(result.Compliance);
    }

    [Fact]
    public void ParseArgs_ExportGpo_DefaultsNull()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Null(result.ExportGpo);
    }

    [Fact]
    public void ParseArgs_Manager_DefaultsNull()
    {
        var result = Program.ParseArgs([]);
        Assert.NotNull(result);
        Assert.Null(result.Manager);
    }

    // ── Parsing --compliance ──────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Compliance_SetsProperty()
    {
        var result = Program.ParseArgs(["--compliance", "cis"]);
        Assert.NotNull(result);
        Assert.Equal("cis", result.Compliance);
    }

    [Theory]
    [InlineData("cis")]
    [InlineData("stig")]
    [InlineData("custom-policy")]
    public void ParseArgs_Compliance_VariousValues_ParsesCorrectly(string value)
    {
        var result = Program.ParseArgs(["--compliance", value]);
        Assert.NotNull(result);
        Assert.Equal(value, result.Compliance);
    }

    [Fact]
    public void ParseArgs_Compliance_WithoutValue_StaysNull()
    {
        var result = Program.ParseArgs(["--compliance"]);
        Assert.NotNull(result);
        Assert.Null(result.Compliance);
    }

    // ── Parsing --export-gpo ─────────────────────────────────────────────

    [Fact]
    public void ParseArgs_ExportGpo_SetsProperty()
    {
        var result = Program.ParseArgs(["--export-gpo", "policies.reg"]);
        Assert.NotNull(result);
        Assert.Equal("policies.reg", result.ExportGpo);
    }

    [Fact]
    public void ParseArgs_ExportGpo_WithPath_ParsesCorrectly()
    {
        var result = Program.ParseArgs(["--export-gpo", @"C:\output\gpo-export.reg"]);
        Assert.NotNull(result);
        Assert.Equal(@"C:\output\gpo-export.reg", result.ExportGpo);
    }

    [Fact]
    public void ParseArgs_ExportGpo_WithoutValue_StaysNull()
    {
        var result = Program.ParseArgs(["--export-gpo"]);
        Assert.NotNull(result);
        Assert.Null(result.ExportGpo);
    }

    // ── Parsing --manager ─────────────────────────────────────────────────

    [Fact]
    public void ParseArgs_Manager_SetsProperty()
    {
        var result = Program.ParseArgs(["--manager", "scoop"]);
        Assert.NotNull(result);
        Assert.Equal("scoop", result.Manager);
    }

    [Theory]
    [InlineData("scoop")]
    [InlineData("choco")]
    [InlineData("winget")]
    [InlineData("pip")]
    public void ParseArgs_Manager_VariousManagers_ParsesCorrectly(string manager)
    {
        var result = Program.ParseArgs(["--manager", manager]);
        Assert.NotNull(result);
        Assert.Equal(manager, result.Manager);
    }

    [Fact]
    public void ParseArgs_Manager_WithoutValue_StaysNull()
    {
        var result = Program.ParseArgs(["--manager"]);
        Assert.NotNull(result);
        Assert.Null(result.Manager);
    }

    // ── Combined argument tests ───────────────────────────────────────────

    [Fact]
    public void ParseArgs_Compliance_WithForce_BothSet()
    {
        var result = Program.ParseArgs(["--compliance", "stig", "--force"]);
        Assert.NotNull(result);
        Assert.Equal("stig", result.Compliance);
        Assert.True(result.Force);
    }

    [Fact]
    public void ParseArgs_ExportGpo_WithDryRun_BothSet()
    {
        var result = Program.ParseArgs(["--export-gpo", "out.reg", "--dry-run"]);
        Assert.NotNull(result);
        Assert.Equal("out.reg", result.ExportGpo);
        Assert.True(result.DryRun);
    }

    [Fact]
    public void ParseArgs_Manager_WithCategory_BothSet()
    {
        var result = Program.ParseArgs(["--manager", "winget", "--category", "Package Management"]);
        Assert.NotNull(result);
        Assert.Equal("winget", result.Manager);
        Assert.Equal("Package Management", result.Category);
    }
}
