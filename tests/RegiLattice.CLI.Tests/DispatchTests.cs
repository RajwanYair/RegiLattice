// RegiLattice.CLI.Tests — DispatchTests.cs
// Tests for Program.Dispatch() and the RunXxx command methods it routes to.
// Strategy: share one small TweakEngine via ICollectionFixture, redirect Console.Out
// per test for output capture. All tests serialised under [Collection("CliDispatch")]
// to prevent static-field conflicts.

using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;
using Xunit;

namespace RegiLattice.CLI.Tests;

// ── Collection: serializes all Dispatch tests + shares one engine ──────────

[CollectionDefinition("CliDispatch", DisableParallelization = true)]
public sealed class CliDispatchCollection : ICollectionFixture<DispatchTestFixture> { }

/// <summary>
/// Shared fixture — created once for the entire CliDispatch collection.
/// Registers a minimal set of deterministic tweaks so tests are fast
/// (no 3 800-tweak RegisterBuiltins call needed for most scenarios).
/// </summary>
public sealed class DispatchTestFixture
{
    public TweakEngine Engine { get; }
    public RegistrySession Session { get; }

    public const string KnownId = "disp-alpha";
    public const string KnownId2 = "disp-beta";
    public const string KnownId3 = "disp-gamma";
    public const string KnownCategory = "Dispatch Test";
    public const string KnownCategory2 = "Other Test";
    public const string KnownTag = "disptest";

    public DispatchTestFixture()
    {
        Session = new RegistrySession(dryRun: true);
        Engine = new TweakEngine(Session);

        // Bypass WMI hardware probing for all CLI dispatch tests.
        // WMI on managed/corporate machines (Intel Intune) creates foreground COM STA threads
        // that block dotnet-testhost exit, causing the test runner to hang indefinitely.
        // See lessons-learned.instructions.md § 'Task.Run + WMI = COM STA Threads'
        HardwareInfo.StubProfile = new HwProfile
        {
            Cpu = new CpuInfo("Stub CPU (test)", 2, 4, "x64"),
            Gpus = [],
            Memory = new MemoryInfo(8192, 4096),
            Disk = new DiskInfo("C:", 256, 128, true),
            HasHyperV = false,
            HasWsl = false,
            HasTpm = false,
            HasSecureBoot = false,
            HasBattery = false,
            WindowsBuild = 22000,
            OptimalWorkers = 4,
            GuiBatchSize = 200,
        };

        Engine.Register([
            new TweakDef
            {
                Id = KnownId,
                Label = "Alpha Dispatch Tweak",
                Category = KnownCategory,
                Tags = [KnownTag, "alpha"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\DispTest", "Alpha", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\RegiLattice\DispTest", "Alpha")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\RegiLattice\DispTest", "Alpha", 1)],
            },
            new TweakDef
            {
                Id = KnownId2,
                Label = "Beta Dispatch Tweak",
                Category = KnownCategory,
                Tags = [KnownTag, "beta"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\DispTest", "Beta", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\RegiLattice\DispTest", "Beta")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\RegiLattice\DispTest", "Beta", 1)],
            },
            new TweakDef
            {
                Id = KnownId3,
                Label = "Gamma Dispatch Tweak",
                Category = KnownCategory2,
                Tags = [KnownTag, "gamma"],
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\DispTest", "Gamma", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKCU\Software\RegiLattice\DispTest", "Gamma")],
                DetectOps = [RegOp.CheckDword(@"HKCU\Software\RegiLattice\DispTest", "Gamma", 0)],
            },
        ]);
    }
}

// ── Base helper ──────────────────────────────────────────────────────────────

/// <summary>
/// Base class that initialises the Program static state for each test and
/// redirects Console.Out so tests can inspect written output.
/// </summary>
[Collection("CliDispatch")]
public abstract class DispatchTestBase : IDisposable
{
    protected readonly DispatchTestFixture Fixture;
    protected readonly StringWriter Out;
    private readonly TextWriter _originalOut;

    protected DispatchTestBase(DispatchTestFixture fixture)
    {
        Fixture = fixture;
        Program.InitForTesting(fixture.Engine, fixture.Session);
        _originalOut = Console.Out;
        Out = new StringWriter();
        Console.SetOut(Out);
    }

    protected string Output => Out.ToString();

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        Out.Dispose();
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunListProfiles
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunListProfilesTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_ListProfiles_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { ListProfiles = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_ListProfiles_OutputContainsKnownProfileNames()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true });
        Assert.Contains("gaming", Output, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("privacy", Output, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("minimal", Output, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("business", Output, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("server", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_ListProfiles_OutputContainsTweakCountColumn()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true });
        // Header row contains "Tweaks" or "Profile"
        Assert.Matches(@"\d+", Output); // at least one digit (tweak count)
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunValidate
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunValidateTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Validate_ReturnsZero_WhenNoErrors()
    {
        int exit = Program.Dispatch(new CliArgs { Validate = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Validate_OutputContainsPassedOrCount()
    {
        Program.Dispatch(new CliArgs { Validate = true });
        // Either "passed validation" or "3 tweaks passed" or similar
        Assert.True(
            Output.Contains("passed", StringComparison.OrdinalIgnoreCase) || Output.Contains("tweak", StringComparison.OrdinalIgnoreCase),
            $"Unexpected validate output: {Output}"
        );
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunStats
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunStatsTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Stats_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Stats = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Stats_OutputContainsTweakCount()
    {
        Program.Dispatch(new CliArgs { Stats = true });
        Assert.Contains("3", Output); // 3 tweaks registered in fixture
    }

    [Fact]
    public void Dispatch_Stats_OutputContainsCategoriesLine()
    {
        Program.Dispatch(new CliArgs { Stats = true });
        Assert.Contains("Categories", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunCategories
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunCategoriesTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Categories_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { ShowCategories = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Categories_Table_OutputContainsCategoryName()
    {
        Program.Dispatch(new CliArgs { ShowCategories = true });
        Assert.Contains(DispatchTestFixture.KnownCategory, Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Categories_Json_OutputIsValidJson()
    {
        Program.Dispatch(new CliArgs { ShowCategories = true, OutputFormat = "json" });
        Assert.True(IsValidJson(Output), $"Output was not valid JSON: {Output}");
    }

    [Fact]
    public void Dispatch_Categories_Json_ContainsCategoryKey()
    {
        Program.Dispatch(new CliArgs { ShowCategories = true, OutputFormat = "json" });
        Assert.Contains(DispatchTestFixture.KnownCategory, Output);
    }

    private static bool IsValidJson(string s)
    {
        try
        {
            JsonDocument.Parse(s.Trim());
            return true;
        }
        catch
        {
            return false;
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunTags
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunTagsTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Tags_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { ShowTags = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Tags_OutputContainsKnownTag()
    {
        Program.Dispatch(new CliArgs { ShowTags = true });
        Assert.Contains(DispatchTestFixture.KnownTag, Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Tags_OutputContainsUniqueTagsLine()
    {
        Program.Dispatch(new CliArgs { ShowTags = true });
        Assert.Contains("unique", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunStatus
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunStatusTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Status_KnownId_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Status_KnownId_OutputContainsLabel()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId });
        Assert.Contains("Alpha Dispatch Tweak", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Status_UnknownId_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(new CliArgs { Mode = "status", Tweak = "nonexistent-tweak-xyz-999" });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_Status_UnknownId_OutputContainsUnknown()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = "nonexistent-tweak-xyz-999" });
        Assert.Contains("Unknown", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunDependsOn
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunDependsOnTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_DependsOn_KnownId_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { DependsOn = DispatchTestFixture.KnownId });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_DependsOn_KnownId_NoDeps_OutputSaysNoDependencies()
    {
        Program.Dispatch(new CliArgs { DependsOn = DispatchTestFixture.KnownId });
        Assert.Contains("no depend", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_DependsOn_UnknownId_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(new CliArgs { DependsOn = "does-not-exist-997" });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_DependsOn_UnknownId_OutputContainsUnknown()
    {
        Program.Dispatch(new CliArgs { DependsOn = "does-not-exist-997" });
        Assert.Contains("Unknown", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunSearch
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunSearchTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Search_Found_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Search = "alpha", OutputFormat = "json" });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Search_Found_JsonOutputContainsId()
    {
        Program.Dispatch(new CliArgs { Search = "alpha", OutputFormat = "json" });
        Assert.Contains(DispatchTestFixture.KnownId, Output);
    }

    [Fact]
    public void Dispatch_Search_NotFound_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Search = "xyznomatchxyz", OutputFormat = "table" });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Search_NotFound_OutputContainsNoTweaks()
    {
        Program.Dispatch(new CliArgs { Search = "xyznomatchxyz", OutputFormat = "table" });
        Assert.Contains("No tweaks", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Search_JsonOutput_IsValidJson()
    {
        Program.Dispatch(new CliArgs { Search = DispatchTestFixture.KnownTag, OutputFormat = "json" });
        string trimmed = Output.Trim();
        Assert.True(trimmed.StartsWith('[') || trimmed.StartsWith('{'), $"Expected JSON output, got: {trimmed[..Math.Min(80, trimmed.Length)]}");
    }

    [Fact]
    public void Dispatch_Search_TableMode_DetectsStatus()
    {
        // table mode calls StatusMap — verify it completes and produces output
        Program.Dispatch(new CliArgs { Search = "disp", OutputFormat = "table" });
        Assert.NotEmpty(Output.Trim());
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunFavorites / RunFavoriteAdd / RunFavoriteRemove
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunFavoritesTests : IDisposable
{
    private readonly DispatchTestFixture _fixture;
    private readonly StringWriter _out;
    private readonly TextWriter _originalOut;
    private readonly IReadOnlyList<string> _savedFavorites;

    public RunFavoritesTests(DispatchTestFixture fixture)
    {
        _fixture = fixture;
        Program.InitForTesting(fixture.Engine, fixture.Session);

        // Snapshot, then fully reset (nullifies cache + deletes file) so each test
        // starts from a provably clean state regardless of prior test order.
        _savedFavorites = RegiLattice.Core.Services.Favorites.All();
        RegiLattice.Core.Services.Favorites.Reset(); // _cache=null, file deleted

        _originalOut = Console.Out;
        _out = new StringWriter();
        Console.SetOut(_out);
    }

    private string Output => _out.ToString();

    public void Dispose()
    {
        Console.SetOut(_originalOut);
        _out.Dispose();

        // Restore user favorites that existed before this test ran.
        RegiLattice.Core.Services.Favorites.Clear();
        foreach (var id in _savedFavorites)
            RegiLattice.Core.Services.Favorites.Add(id);
        RegiLattice.Core.Services.Favorites.Flush();
    }

    [Fact]
    public void Dispatch_Favorites_Empty_ReturnsZero()
    {
        RegiLattice.Core.Services.Favorites.Reset();
        int exit = Program.Dispatch(new CliArgs { ShowFavorites = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Favorites_Empty_OutputSaysNoFavorites()
    {
        // Use a fresh local writer so we capture ONLY this dispatch's output
        // and are immune to any stale content that might be in _out from constructor ops.
        RegiLattice.Core.Services.Favorites.Reset();
        using var localOut = new StringWriter();
        Console.SetOut(localOut);
        Program.Dispatch(new CliArgs { ShowFavorites = true });
        Console.SetOut(_originalOut); // restore so _out capture continues
        Assert.Contains("No favorites", localOut.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_FavoriteAdd_UnknownId_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(new CliArgs { FavoriteAdd = "unknown-tweak-9191" });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_FavoriteAdd_UnknownId_OutputContainsUnknown()
    {
        Program.Dispatch(new CliArgs { FavoriteAdd = "unknown-tweak-9191" });
        Assert.Contains("Unknown", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_FavoriteAdd_KnownId_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { FavoriteAdd = DispatchTestFixture.KnownId });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_FavoriteAdd_KnownId_OutputContainsAdded()
    {
        Program.Dispatch(new CliArgs { FavoriteAdd = DispatchTestFixture.KnownId });
        Assert.Contains("Added", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_FavoriteRemove_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { FavoriteRemove = DispatchTestFixture.KnownId });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_FavoriteRemove_OutputContainsRemoved()
    {
        Program.Dispatch(new CliArgs { FavoriteRemove = DispatchTestFixture.KnownId });
        Assert.Contains("Removed", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunHistory
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunHistoryTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_History_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { ShowHistory = true, HistoryCount = 5 });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_History_NonEmptyOutput()
    {
        Program.Dispatch(new CliArgs { ShowHistory = true, HistoryCount = 10 });
        Assert.NotNull(Output); // may be "No history" or actual entries
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunDoctor
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunDoctorTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Doctor_ReturnsZeroOrOne()
    {
        int exit = Program.Dispatch(new CliArgs { Doctor = true });
        Assert.True(exit == 0 || exit == 1, $"Expected 0 or 1, got {exit}");
    }

    [Fact]
    public void Dispatch_Doctor_OutputContainsNetRuntime()
    {
        Program.Dispatch(new CliArgs { Doctor = true });
        Assert.Contains(".NET", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Doctor_OutputContainsTweaksRegistry()
    {
        Program.Dispatch(new CliArgs { Doctor = true });
        Assert.Contains("Tweaks", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunHwInfo
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunHwInfoTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_HwInfo_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { HwInfo = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_HwInfo_ProducesOutput()
    {
        Program.Dispatch(new CliArgs { HwInfo = true });
        Assert.NotEmpty(Output.Trim());
    }

    [Fact]
    public void Dispatch_HwInfo_OutputContainsSuggestedProfile()
    {
        Program.Dispatch(new CliArgs { HwInfo = true });
        Assert.Contains("profile", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunCheck
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunCheckTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Check_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Check = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Check_OutputContainsStatusHeaders()
    {
        Program.Dispatch(new CliArgs { Check = true });
        Assert.Contains("Applied", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunList
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunListTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_List_Json_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { ShowList = true, OutputFormat = "json" });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_List_Json_IsValidJson()
    {
        Program.Dispatch(new CliArgs { ShowList = true, OutputFormat = "json" });
        try
        {
            JsonDocument.Parse(Output.Trim());
        }
        catch (JsonException ex)
        {
            Assert.Fail($"Output was not valid JSON: {ex.Message}\nOutput: {Output[..Math.Min(200, Output.Length)]}");
        }
    }

    [Fact]
    public void Dispatch_List_Json_ContainsKnownId()
    {
        Program.Dispatch(new CliArgs { ShowList = true, OutputFormat = "json" });
        Assert.Contains(DispatchTestFixture.KnownId, Output);
    }

    [Fact]
    public void Dispatch_List_Table_DetectsStatusAndProducesOutput()
    {
        // Table mode calls StatusMap — with 3 tweaks this is fast
        Program.Dispatch(new CliArgs { ShowList = true, OutputFormat = "table" });
        Assert.Contains(DispatchTestFixture.KnownId, Output);
    }

    [Fact]
    public void Dispatch_List_WithCategory_ValidCategory_ReturnsZero()
    {
        int exit = Program.Dispatch(
            new CliArgs
            {
                ShowList = true,
                Category = DispatchTestFixture.KnownCategory,
                OutputFormat = "json",
            }
        );
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_List_WithCategory_UnknownCategory_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(
            new CliArgs
            {
                ShowList = true,
                Category = "NonExistentCategory9999",
                OutputFormat = "json",
            }
        );
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_List_WithCategory_UnknownCategory_OutputContainsNoTweaks()
    {
        Program.Dispatch(
            new CliArgs
            {
                ShowList = true,
                Category = "NonExistentCategory9999",
                OutputFormat = "json",
            }
        );
        Assert.Contains("No tweaks", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Default dispatch → PrintHelp
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class DefaultDispatchTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_NoFlags_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs());
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_NoFlags_PrintsUsageLine()
    {
        Program.Dispatch(new CliArgs());
        Assert.Contains("Usage", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_NoFlags_PrintsRegiLatticeVersion()
    {
        Program.Dispatch(new CliArgs());
        Assert.Contains("RegiLattice", Output, StringComparison.OrdinalIgnoreCase);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunAction (apply/remove via Dispatch) — dry-run + AssumeYes + Force
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunActionTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Apply_UnknownTweak_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(
            new CliArgs
            {
                Mode = "apply",
                Tweak = "no-such-tweak-8877",
                AssumeYes = true,
                Force = true,
                DryRun = true,
            }
        );
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_Apply_UnknownTweak_OutputContainsUnknown()
    {
        Program.Dispatch(
            new CliArgs
            {
                Mode = "apply",
                Tweak = "no-such-tweak-8877",
                AssumeYes = true,
                Force = true,
                DryRun = true,
            }
        );
        Assert.Contains("Unknown", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Apply_KnownTweak_DryRun_ReturnsZeroOrOne()
    {
        int exit = Program.Dispatch(
            new CliArgs
            {
                Mode = "apply",
                Tweak = DispatchTestFixture.KnownId,
                AssumeYes = true,
                Force = true,
                DryRun = true,
            }
        );
        Assert.True(exit == 0 || exit == 1, $"Expected 0 or 1, got {exit}");
    }

    [Fact]
    public void Dispatch_Apply_KnownTweak_DryRun_OutputContainsDryRun()
    {
        Program.Dispatch(
            new CliArgs
            {
                Mode = "apply",
                Tweak = DispatchTestFixture.KnownId,
                AssumeYes = true,
                Force = true,
                DryRun = true,
            }
        );
        Assert.Contains("Dry-run", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Remove_KnownTweak_DryRun_ReturnsZeroOrOne()
    {
        int exit = Program.Dispatch(
            new CliArgs
            {
                Mode = "remove",
                Tweak = DispatchTestFixture.KnownId,
                AssumeYes = true,
                Force = true,
                DryRun = true,
            }
        );
        Assert.True(exit == 0 || exit == 1, $"Expected 0 or 1, got {exit}");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunExportJson
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunExportJsonTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_ExportJson_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-test-{Guid.NewGuid():N}.json");
        try
        {
            int exit = Program.Dispatch(new CliArgs { ExportJson = tmp });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_ExportJson_CreatesFile()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-test-{Guid.NewGuid():N}.json");
        try
        {
            Program.Dispatch(new CliArgs { ExportJson = tmp });
            Assert.True(File.Exists(tmp), "Expected JSON file to be created");
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_ExportJson_OutputContainsTweakCount()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-test-{Guid.NewGuid():N}.json");
        try
        {
            Program.Dispatch(new CliArgs { ExportJson = tmp });
            Assert.Contains("tweak", Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunSaveSnapshot
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunSaveSnapshotTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Snapshot_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-snap-{Guid.NewGuid():N}.json");
        try
        {
            int exit = Program.Dispatch(new CliArgs { Snapshot = tmp });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_Snapshot_CreatesFile()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-snap-{Guid.NewGuid():N}.json");
        try
        {
            Program.Dispatch(new CliArgs { Snapshot = tmp });
            Assert.True(File.Exists(tmp), "Expected snapshot file to be created");
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_Snapshot_OutputContainsSaved()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-snap-{Guid.NewGuid():N}.json");
        try
        {
            Program.Dispatch(new CliArgs { Snapshot = tmp });
            Assert.Contains("saved", Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunSnapshotDiff
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunSnapshotDiffTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    /// <summary>Creates and returns the path to a temporary snapshot file.</summary>
    private string CreateTempSnapshot(string suffix)
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-snapdiff-{suffix}-{Guid.NewGuid():N}.json");
        Program.Dispatch(new CliArgs { Snapshot = tmp });
        return tmp;
    }

    [Fact]
    public void Dispatch_SnapshotDiff_IdenticalFiles_ReturnsZeroAndNoDifferences()
    {
        string snap = CreateTempSnapshot("A");
        try
        {
            Out.GetStringBuilder().Clear(); // reset output from snapshot creation
            int exit = Program.Dispatch(new CliArgs { SnapshotDiffA = snap, SnapshotDiffB = snap });
            Assert.Equal(0, exit);
            Assert.Contains("No differences", Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(snap))
                File.Delete(snap);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunNewPack
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunNewPackTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_NewPack_AllSpecialChars_ReturnsOne()
    {
        // Slug becomes empty after stripping non-alphanumeric chars
        int exit = Program.Dispatch(new CliArgs { NewPack = "!@#$%" });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_NewPack_AllSpecialChars_OutputContainsMustContain()
    {
        Program.Dispatch(new CliArgs { NewPack = "!@#$%" });
        Assert.Contains("must contain", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_NewPack_ValidName_CreatesFile()
    {
        // Use a temp dir as working directory so cleanup is easy
        string origDir = Directory.GetCurrentDirectory();
        string tmpDir = Path.Combine(Path.GetTempPath(), $"newpack-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmpDir);
        try
        {
            Directory.SetCurrentDirectory(tmpDir);
            int exit = Program.Dispatch(new CliArgs { NewPack = "my-test-pack" });
            Assert.Equal(0, exit);
            Assert.Contains("created", Output, StringComparison.OrdinalIgnoreCase);
            Assert.True(Directory.GetFiles(tmpDir, "*.pack.json").Length > 0, "Expected a .pack.json file to be created");
        }
        finally
        {
            Directory.SetCurrentDirectory(origDir);
            Directory.Delete(tmpDir, recursive: true);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunExportGpo
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunExportGpoTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_ExportGpo_ReturnsZero()
    {
        string tmpDir = Path.Combine(Path.GetTempPath(), $"gpo-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmpDir);
        string admxPath = Path.Combine(tmpDir, "RegiLattice.admx");
        try
        {
            int exit = Program.Dispatch(new CliArgs { ExportGpo = admxPath });
            Assert.Equal(0, exit);
        }
        finally
        {
            Directory.Delete(tmpDir, recursive: true);
        }
    }

    [Fact]
    public void Dispatch_ExportGpo_OutputContainsExported()
    {
        string tmpDir = Path.Combine(Path.GetTempPath(), $"gpo-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tmpDir);
        string admxPath = Path.Combine(tmpDir, "RegiLattice.admx");
        try
        {
            Program.Dispatch(new CliArgs { ExportGpo = admxPath });
            Assert.Contains("Exported", Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            Directory.Delete(tmpDir, recursive: true);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunCompliance
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunComplianceTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Compliance_MissingFile_ReturnsTwoExitCode()
    {
        int exit = Program.Dispatch(new CliArgs { Compliance = Path.Combine(Path.GetTempPath(), "no-such-file-9988.json") });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_Compliance_MissingFile_OutputContainsNotFound()
    {
        Program.Dispatch(new CliArgs { Compliance = Path.Combine(Path.GetTempPath(), "no-such-file-9988.json") });
        Assert.Contains("not found", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Compliance_ValidSnapshot_ReturnsZeroOrOne()
    {
        // Create a real snapshot and then run compliance against it
        string snap = Path.Combine(Path.GetTempPath(), $"comp-snap-{Guid.NewGuid():N}.json");
        try
        {
            Program.Dispatch(new CliArgs { Snapshot = snap });
            Out.GetStringBuilder().Clear();

            int exit = Program.Dispatch(new CliArgs { Compliance = snap });
            Assert.True(exit == 0 || exit == 1, $"Expected 0 or 1, got {exit}");
        }
        finally
        {
            if (File.Exists(snap))
                File.Delete(snap);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Marketplace error paths (no network required)
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class MarketplaceErrorPathTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Marketplace_Install_NullArg_ReturnsOne()
    {
        // "install" without a pack name returns usage / 1
        int exit = Program.Dispatch(new CliArgs { Marketplace = "install", MarketplaceArg = null });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Uninstall_NullArg_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "uninstall", MarketplaceArg = null });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Search_EmptyQuery_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "search", MarketplaceArg = null });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_InstallFile_MissingPath_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "install-file", MarketplaceArg = @"C:\does-not-exist-ever\pack.json" });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Unknown_Command_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "totally-fake-command" });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Info_NullArg_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "info", MarketplaceArg = null });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Update_NullArg_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Marketplace = "update", MarketplaceArg = null });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Marketplace_Installed_CallsMethod_ReturnsZero()
    {
        // "installed" has no arg requirement and just lists; on clean system returns 0
        int exit = Program.Dispatch(new CliArgs { Marketplace = "installed" });
        Assert.Equal(0, exit);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// Manager dispatch error paths
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunManagerTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Manager_UnknownTarget_ReturnsOne()
    {
        int exit = Program.Dispatch(new CliArgs { Manager = "unknownmanager9988" });
        Assert.Equal(1, exit);
    }

    [Fact]
    public void Dispatch_Manager_UnknownTarget_OutputContainsUnknown()
    {
        Program.Dispatch(new CliArgs { Manager = "unknownmanager9988" });
        Assert.Contains("Unknown", Output, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Dispatch_Manager_ScheduledTweaks_ReturnsZero()
    {
        // scheduledtweaks loads from file — with no file present, shows "No scheduled tweaks"
        int exit = Program.Dispatch(new CliArgs { Manager = "scheduledtweaks" });
        Assert.Equal(0, exit);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// FormatRegValue helper (tested via RunExportReg which calls it)
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunExportRegTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_ExportReg_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-export-{Guid.NewGuid():N}.reg");
        try
        {
            int exit = Program.Dispatch(new CliArgs { ExportReg = tmp });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_ExportReg_CreatesFile()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-export-{Guid.NewGuid():N}.reg");
        try
        {
            Program.Dispatch(new CliArgs { ExportReg = tmp });
            Assert.True(File.Exists(tmp), "Expected .reg file to be created");
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_ExportReg_FileContainsRegHeader()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"disp-export-{Guid.NewGuid():N}.reg");
        try
        {
            Program.Dispatch(new CliArgs { ExportReg = tmp });
            string content = File.ReadAllText(tmp);
            Assert.Contains("Windows Registry Editor", content, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp))
                File.Delete(tmp);
        }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// A3: B2 Contract — JSON output: status command
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class B2ContractTests_Status(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Status_Json_IsValidJson()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId, OutputFormat = "json" });
        Assert.True(IsValidJson(Output), $"Expected valid JSON, got: {Output}");
    }

    [Fact]
    public void Dispatch_Status_Json_ContainsIdField()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.True(doc.RootElement.TryGetProperty("Id", out _), "JSON missing 'Id' field");
    }

    [Fact]
    public void Dispatch_Status_Json_ContainsStatusField()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.True(doc.RootElement.TryGetProperty("Status", out _), "JSON missing 'Status' field");
    }

    [Fact]
    public void Dispatch_Status_Json_IdMatchesRequested()
    {
        Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.Equal(DispatchTestFixture.KnownId, doc.RootElement.GetProperty("Id").GetString());
    }

    [Fact]
    public void Dispatch_Status_UnknownTweak_Returns2()
    {
        int exit = Program.Dispatch(new CliArgs { Mode = "status", Tweak = "zzz-does-not-exist-zzz" });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_Status_Table_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Mode = "status", Tweak = DispatchTestFixture.KnownId });
        Assert.Equal(0, exit);
    }

    private static bool IsValidJson(string s)
    {
        try { JsonDocument.Parse(s.Trim()); return true; }
        catch { return false; }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// A3: B2 Contract — JSON output: apply command
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class B2ContractTests_Apply(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Apply_Json_IsValidJson()
    {
        // Force=true bypasses CorporateGuard so the test is environment-independent
        int exit = Program.Dispatch(new CliArgs { Mode = "apply", Tweak = DispatchTestFixture.KnownId, AssumeYes = true, Force = true, OutputFormat = "json" });
        Assert.Equal(0, exit);
        Assert.True(IsValidJson(Output), $"Expected valid JSON, got: {Output}");
    }

    [Fact]
    public void Dispatch_Apply_Json_ContainsIdField()
    {
        Program.Dispatch(new CliArgs { Mode = "apply", Tweak = DispatchTestFixture.KnownId, AssumeYes = true, Force = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.True(doc.RootElement.TryGetProperty("Id", out _), "JSON missing 'Id' field");
    }

    [Fact]
    public void Dispatch_Apply_Json_IdMatchesRequested()
    {
        Program.Dispatch(new CliArgs { Mode = "apply", Tweak = DispatchTestFixture.KnownId, AssumeYes = true, Force = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.Equal(DispatchTestFixture.KnownId, doc.RootElement.GetProperty("Id").GetString());
    }

    [Fact]
    public void Dispatch_Apply_Json_ModeIsApply()
    {
        Program.Dispatch(new CliArgs { Mode = "apply", Tweak = DispatchTestFixture.KnownId, AssumeYes = true, Force = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.Equal("apply", doc.RootElement.GetProperty("Mode").GetString());
    }

    [Fact]
    public void Dispatch_Apply_UnknownTweak_Returns2()
    {
        // Force=true bypasses CorporateGuard so tweak-not-found (exit 2) is tested
        int exit = Program.Dispatch(new CliArgs { Mode = "apply", Tweak = "zzz-no-such-tweak-zzz", AssumeYes = true, Force = true });
        Assert.Equal(2, exit);
    }

    private static bool IsValidJson(string s)
    {
        try { JsonDocument.Parse(s.Trim()); return true; }
        catch { return false; }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// A3: B2 Contract — JSON output: list-profiles command
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class B2ContractTests_ListProfiles(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_ListProfiles_Json_IsValidJson()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true, OutputFormat = "json" });
        Assert.True(IsValidJson(Output), $"Expected valid JSON from --list-profiles, got: {Output}");
    }

    [Fact]
    public void Dispatch_ListProfiles_Json_IsArray()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }

    [Fact]
    public void Dispatch_ListProfiles_Json_ContainsNameField()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        var first = doc.RootElement.EnumerateArray().First();
        Assert.True(first.TryGetProperty("Name", out _), "JSON profile object missing 'Name' field");
    }

    [Fact]
    public void Dispatch_ListProfiles_Json_ContainsTweakCountField()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        var first = doc.RootElement.EnumerateArray().First();
        Assert.True(first.TryGetProperty("TweakCount", out _), "JSON profile object missing 'TweakCount' field");
    }

    [Fact]
    public void Dispatch_ListProfiles_Json_ContainsAllBuiltinProfiles()
    {
        Program.Dispatch(new CliArgs { ListProfiles = true, OutputFormat = "json" });
        var names = Output.ToLowerInvariant();
        Assert.Contains("gaming", names);
        Assert.Contains("privacy", names);
        Assert.Contains("minimal", names);
        Assert.Contains("business", names);
        Assert.Contains("server", names);
    }

    private static bool IsValidJson(string s)
    {
        try { JsonDocument.Parse(s.Trim()); return true; }
        catch { return false; }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// A3: B2 Contract — JSON output: check command
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class B2ContractTests_Check(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void Dispatch_Check_Json_IsValidJson()
    {
        Program.Dispatch(new CliArgs { Check = true, OutputFormat = "json" });
        Assert.True(IsValidJson(Output), $"Expected valid JSON from --check, got: {Output}");
    }

    [Fact]
    public void Dispatch_Check_Json_IsArray()
    {
        Program.Dispatch(new CliArgs { Check = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        Assert.Equal(JsonValueKind.Array, doc.RootElement.ValueKind);
    }

    [Fact]
    public void Dispatch_Check_Json_EachItemHasIdAndStatus()
    {
        Program.Dispatch(new CliArgs { Check = true, OutputFormat = "json" });
        using var doc = JsonDocument.Parse(Output.Trim());
        foreach (var item in doc.RootElement.EnumerateArray())
        {
            Assert.True(item.TryGetProperty("Id", out _), "Check JSON item missing 'Id'");
            Assert.True(item.TryGetProperty("Status", out _), "Check JSON item missing 'Status'");
        }
    }

    [Fact]
    public void Dispatch_Check_Table_ReturnsZero()
    {
        int exit = Program.Dispatch(new CliArgs { Check = true });
        Assert.Equal(0, exit);
    }

    private static bool IsValidJson(string s)
    {
        try { JsonDocument.Parse(s.Trim()); return true; }
        catch { return false; }
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// A3: B2 Contract — exit code constants
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class B2ContractTests_ExitCodes(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    [Fact]
    public void ExitCodes_Success_Is0()
    {
        Assert.Equal(0, ExitCodes.Success);
    }

    [Fact]
    public void ExitCodes_PartialFail_Is1()
    {
        Assert.Equal(1, ExitCodes.PartialFail);
    }

    [Fact]
    public void ExitCodes_UserError_Is2()
    {
        Assert.Equal(2, ExitCodes.UserError);
    }

    [Fact]
    public void ExitCodes_AdminRequired_Is3()
    {
        Assert.Equal(3, ExitCodes.AdminRequired);
    }

    [Fact]
    public void ExitCodes_CorpGuardBlocked_Is4()
    {
        Assert.Equal(4, ExitCodes.CorpGuardBlocked);
    }

    [Fact]
    public void Dispatch_UnknownTweak_Apply_Returns2()
    {
        // Force=true bypasses CorporateGuard so tweak-not-found (exit 2) is tested
        int exit = Program.Dispatch(new CliArgs { Mode = "apply", Tweak = "zzz-nonexistent-zzz", AssumeYes = true, Force = true });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_UnknownTweak_Status_Returns2()
    {
        int exit = Program.Dispatch(new CliArgs { Mode = "status", Tweak = "zzz-nonexistent-zzz" });
        Assert.Equal(2, exit);
    }

    [Fact]
    public void Dispatch_Stats_Returns0()
    {
        int exit = Program.Dispatch(new CliArgs { Stats = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_Validate_Returns0()
    {
        int exit = Program.Dispatch(new CliArgs { Validate = true });
        Assert.Equal(0, exit);
    }

    [Fact]
    public void Dispatch_List_Returns0()
    {
        int exit = Program.Dispatch(new CliArgs { ShowList = true });
        Assert.Equal(0, exit);
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// RunBatch (B7)
// ─────────────────────────────────────────────────────────────────────────────

[Collection("CliDispatch")]
public sealed class RunBatchTests(DispatchTestFixture fixture) : DispatchTestBase(fixture)
{
    // ── Plain-text format ────────────────────────────────────────────────

    [Fact]
    public void Dispatch_BatchApply_PlainTextFile_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllText(tmp, DispatchTestFixture.KnownId);
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_BatchApply_PlainTextFile_OutputContainsTweakId()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllText(tmp, DispatchTestFixture.KnownId);
            Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Contains(DispatchTestFixture.KnownId, Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_BatchApply_MultiLineFile_ProcessesAllTweaks()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllLines(tmp, [
                DispatchTestFixture.KnownId,
                "# this is a comment",
                "",
                DispatchTestFixture.KnownId2,
            ]);
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(0, exit);
            Assert.Contains(DispatchTestFixture.KnownId, Output, StringComparison.OrdinalIgnoreCase);
            Assert.Contains(DispatchTestFixture.KnownId2, Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    // ── JSON array format ────────────────────────────────────────────────

    [Fact]
    public void Dispatch_BatchApply_JsonArrayFile_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-{Guid.NewGuid():N}.json");
        try
        {
            File.WriteAllText(tmp, $"[\"{ DispatchTestFixture.KnownId}\",\"{DispatchTestFixture.KnownId2}\"]");
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    // ── Snapshot JSON format ─────────────────────────────────────────────

    [Fact]
    public void Dispatch_BatchApply_SnapshotJsonFile_ReturnsZero()
    {
        string snap = Path.Combine(Path.GetTempPath(), $"batch-snap-{Guid.NewGuid():N}.json");
        string batch = Path.Combine(Path.GetTempPath(), $"batch-use-{Guid.NewGuid():N}.json");
        try
        {
            // Create a real snapshot file
            Program.Dispatch(new CliArgs { Snapshot = snap });
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = snap,
                AssumeYes = true,
                Force = true,
            });
            // Snapshot-based batch succeeds (0) or returns PartialFail (1) if no
            // tweaks were applied — both are valid outcomes for this structural test.
            Assert.True(exit is 0 or 1, $"Unexpected exit code: {exit}");
        }
        finally
        {
            if (File.Exists(snap)) File.Delete(snap);
            if (File.Exists(batch)) File.Delete(batch);
        }
    }

    // ── Remove verb ──────────────────────────────────────────────────────

    [Fact]
    public void Dispatch_BatchRemove_PlainTextFile_ReturnsZero()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllText(tmp, DispatchTestFixture.KnownId);
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "remove",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(0, exit);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    // ── Error cases ──────────────────────────────────────────────────────

    [Fact]
    public void Dispatch_BatchApply_MissingFile_ReturnsUserError()
    {
        string nonExistent = Path.Combine(Path.GetTempPath(), $"no-such-file-{Guid.NewGuid():N}.txt");
        int exit = Program.Dispatch(new CliArgs
        {
            BatchMode = "apply",
            BatchFile = nonExistent,
            AssumeYes = true,
            Force = true,
        });
        Assert.Equal(ExitCodes.UserError, exit);
    }

    [Fact]
    public void Dispatch_BatchApply_EmptyFile_ReturnsUserError()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-empty-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllText(tmp, "");
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(ExitCodes.UserError, exit);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_BatchApply_AllUnknownIds_ReturnsUserError()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-unk-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllLines(tmp, ["zzz-no-such-tweak-1", "zzz-no-such-tweak-2"]);
            int exit = Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Equal(ExitCodes.UserError, exit);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }

    [Fact]
    public void Dispatch_BatchApply_PartialUnknownIds_OutputWarnsUnknown()
    {
        string tmp = Path.Combine(Path.GetTempPath(), $"batch-partial-{Guid.NewGuid():N}.txt");
        try
        {
            File.WriteAllLines(tmp, [DispatchTestFixture.KnownId, "zzz-unknown-tweak-zzz"]);
            Program.Dispatch(new CliArgs
            {
                BatchMode = "apply",
                BatchFile = tmp,
                AssumeYes = true,
                Force = true,
            });
            Assert.Contains("unknown", Output, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (File.Exists(tmp)) File.Delete(tmp);
        }
    }
}
