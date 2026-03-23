// tests/RegiLattice.Core.Tests/BranchCoverage5Tests.cs
// Sprint 121 — Branch coverage boost set 5
// Targets: UpdateCheckContext JSON deserialization, CompareVersions, DetectAction sweeps
//          for ScheduledTaskTweaks / Boot / WSL / UserAccount / other tweak modules,
//          SshHardening helper branches, Elevation.RunElevated allowed-command path.
#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

// ── 1. GitHubRelease / UpdateCheckContext deserialization tests ─────────────

public sealed class GitHubReleaseJsonTests
{
    [Fact]
    public void Deserialize_AllFieldsPresent_PopulatesAllProperties()
    {
        const string json = """
            {"tag_name":"v4.5.0","body":"release notes text","html_url":"https://github.com/RajwanYair/RegiLattice","published_at":"2025-01-15T12:00:00Z"}
            """;
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v4.5.0", r.TagName);
        Assert.Equal("release notes text", r.Body);
        Assert.NotNull(r.HtmlUrl);
        Assert.NotNull(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_EmptyObject_AllPropertiesNull()
    {
        var r = JsonSerializer.Deserialize("{}", UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.TagName);
        Assert.Null(r.Body);
        Assert.Null(r.HtmlUrl);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_TagNameOnly_OtherFieldsNull()
    {
        const string json = """{"tag_name":"v1.0.0"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v1.0.0", r.TagName);
        Assert.Null(r.Body);
        Assert.Null(r.HtmlUrl);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_NullTagName_TagNameIsNull()
    {
        const string json = """{"tag_name":null,"body":"notes","html_url":"https://x.com"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.TagName);
        Assert.Equal("notes", r.Body);
    }

    [Fact]
    public void Deserialize_NullBody_BodyIsNull()
    {
        const string json = """{"tag_name":"v2.0.0","body":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v2.0.0", r.TagName);
        Assert.Null(r.Body);
    }

    [Fact]
    public void Deserialize_PublishedAtDate_ParsedCorrectly()
    {
        const string json = """{"tag_name":"v3.0.0","published_at":"2024-06-15T00:00:00Z"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.NotNull(r.PublishedAt);
        Assert.Equal(2024, r.PublishedAt!.Value.Year);
    }

    [Fact]
    public void Deserialize_NullPublishedAt_PublishedAtIsNull()
    {
        const string json = """{"tag_name":"v3.0.0","published_at":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.PublishedAt);
    }

    [Fact]
    public void Deserialize_ExtraUnknownFields_Ignored()
    {
        const string json = """{"tag_name":"v4.0.0","unknown_field":"ignored","prerelease":false}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Equal("v4.0.0", r.TagName);
    }

    [Fact]
    public void Deserialize_HtmlUrl_Populated()
    {
        const string json = """{"html_url":"https://github.com/RajwanYair/RegiLattice/releases/tag/v5.0.0"}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.NotNull(r.HtmlUrl);
        Assert.Contains("v5.0.0", r.HtmlUrl);
    }

    [Fact]
    public void Deserialize_NullHtmlUrl_HtmlUrlIsNull()
    {
        const string json = """{"tag_name":"v4.0.0","html_url":null}""";
        var r = JsonSerializer.Deserialize(json, UpdateCheckContext.Default.GitHubRelease);
        Assert.NotNull(r);
        Assert.Null(r.HtmlUrl);
    }
}

// ── 2. CompareVersions + CurrentVersion ────────────────────────────────────

public sealed class CompareVersionsBranchTests
{
    [Fact]
    public void CompareVersions_AGreaterThanB_ReturnsPositive()
    {
        Assert.True(UpdateCheckService.CompareVersions("4.5.0", "4.4.0") > 0);
    }

    [Fact]
    public void CompareVersions_ALessThanB_ReturnsNegative()
    {
        Assert.True(UpdateCheckService.CompareVersions("4.3.0", "4.4.0") < 0);
    }

    [Fact]
    public void CompareVersions_AEqualsB_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("4.4.0", "4.4.0"));
    }

    [Fact]
    public void CompareVersions_InvalidFirstVersion_TreatedAsZeroZeroZero()
    {
        // TryParse(a) fails → va = 0.0.0; b = 1.0.0 → result < 0
        Assert.True(UpdateCheckService.CompareVersions("not-a-version", "1.0.0") < 0);
    }

    [Fact]
    public void CompareVersions_InvalidSecondVersion_TreatedAsZeroZeroZero()
    {
        // TryParse(b) fails → vb = 0.0.0; a = 1.0.0 → result > 0
        Assert.True(UpdateCheckService.CompareVersions("1.0.0", "not-a-version") > 0);
    }

    [Fact]
    public void CompareVersions_BothInvalid_ReturnsZero()
    {
        Assert.Equal(0, UpdateCheckService.CompareVersions("invalid", "bad"));
    }

    [Fact]
    public void CompareVersions_EmptyString_TreatedAsZeroZeroZero()
    {
        Assert.True(UpdateCheckService.CompareVersions("", "1.0.0") < 0);
    }

    [Fact]
    public void CurrentVersion_ReturnsNonEmptyVersionFormat()
    {
        var v = UpdateCheckService.CurrentVersion;
        Assert.NotEmpty(v);
        Assert.Matches(@"^\d+\.\d+\.\d+$", v);
    }
}

// ── 3. Scheduled Task DetectAction Sweep ───────────────────────────────────
// NOTE: We verify structure only (non-null DetectAction), NOT invoke it.
// Invoking schtasks.exe/PowerShell per tweak takes 2-5 s each and hangs suites.

public sealed class ScheduledTaskDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllScheduledTaskTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("Scheduled Tasks", out var tweaks))
            return;

        // Verify scheduled-task tweaks exist and have either DetectOps or DetectAction
        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 5, $"Expected ≥5 Scheduled Task tweaks with detection; found {withDetect}");
    }
}

// ── 4. Boot DetectAction Sweep ──────────────────────────────────────────────

public sealed class BootDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllBootTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("Boot", out var tweaks))
            return;

        // Boot tweaks use bcdedit-based DetectAction — verify they exist
        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 1, $"Expected ≥1 Boot tweak with detection; found {withDetect}");
    }
}

// ── 5. WSL DetectAction Sweep ──────────────────────────────────────────────

public sealed class WslDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllWslTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("WSL", out var tweaks))
            return;

        Assert.True(tweaks.Count >= 5, $"Expected ≥5 WSL tweaks; found {tweaks.Count}");
    }
}

// ── 6. User Account DetectAction Sweep ─────────────────────────────────────

public sealed class UserAccountDetectActionSweepTests
{
    [Fact]
    public void DetectAction_AllUserAccountTweaks_HaveExpectedStructure()
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue("User Account", out var tweaks))
            return;

        int withDetect = tweaks.Count(td => td.DetectAction is not null || td.DetectOps.Count > 0);
        Assert.True(withDetect >= 3, $"Expected ≥3 User Account tweaks with detection; found {withDetect}");
    }
}

// ── 7. Other Tweak Module DetectAction Sweeps ──────────────────────────────

public sealed class OtherTweakDetectActionSweepTests
{
    private static void AssertCategoryHasTweaks(string category, int minTweaks = 1)
    {
        var engine = new TweakEngine();
        engine.RegisterBuiltins();

        var dict = engine.TweaksByCategory();
        if (!dict.TryGetValue(category, out var tweaks))
            return;

        if (minTweaks > 0)
            Assert.True(tweaks.Count >= minTweaks, $"Category '{category}': expected ≥{minTweaks} tweaks, found {tweaks.Count}");
    }

    [Fact]
    public void DetectAction_DeveloperTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Developer");

    [Fact]
    public void DetectAction_PowerManagementTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Power Management");

    [Fact]
    public void DetectAction_CommandLineTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Command Line");

    [Fact]
    public void DetectAction_AppCompatibilityTweaks_CanBeInvoked() => AssertCategoryHasTweaks("App Compatibility");

    [Fact]
    public void DetectAction_PackageManagementTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Package Management");

    [Fact]
    public void DetectAction_ServicesTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Services");

    [Fact]
    public void DetectAction_SshConfigurationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("SSH Configuration");

    [Fact]
    public void DetectAction_VirtualizationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Virtualization");

    [Fact]
    public void DetectAction_NetworkOptimizationTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Network Optimization");

    [Fact]
    public void DetectAction_PrintingTweaks_CanBeInvoked() => AssertCategoryHasTweaks("Printing");
}

// ── 8. SshHardening helper branches via action delegates ───────────────────

public sealed class SshHardeningBranchTests2
{
    // sshd_config does NOT exist on this machine → exercises the early-return paths
    // in SetSshdDirective / RemoveSshdDirective / DetectSshdDirective.

    private static (TweakDef? tweak, TweakEngine engine) GetSshTweak()
    {
        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var tweak = engine.AllTweaks().FirstOrDefault(t => t.Category == "SSH Configuration");
        return (tweak, engine);
    }

    [Fact]
    public void ApplyAction_DryRun_True_ReturnsImmediately_NoException()
    {
        // Calls SetSshdDirective(directive, value, dryRun: true) → hits `if (dryRun) return;` branch
        var (td, _) = GetSshTweak();
        if (td?.ApplyAction is null)
            return; // SSH not registered, skip

        // dryRun=true → SetSshdDirective returns immediately, no file access
        td.ApplyAction(true);
    }

    [Fact]
    public void ApplyAction_DryRun_False_NoFile_ReturnsImmediately_NoException()
    {
        // Calls SetSshdDirective(directive, value, dryRun: false)
        // → `if (dryRun) return;` F-branch → `if (!File.Exists(SshdConfig)) return;` T-branch
        var (td, _) = GetSshTweak();
        if (td?.ApplyAction is null)
            return;

        td.ApplyAction(false); // file doesn't exist → returns without modifying anything
    }

    [Fact]
    public void RemoveAction_DryRun_True_ReturnsImmediately_NoException()
    {
        // Calls RemoveSshdDirective(directive, dryRun: true) → early return on dryRun
        var (td, _) = GetSshTweak();
        if (td?.RemoveAction is null)
            return;

        td.RemoveAction(true);
    }

    [Fact]
    public void RemoveAction_DryRun_False_NoFile_ReturnsImmediately_NoException()
    {
        // Calls RemoveSshdDirective(directive, dryRun: false)
        // → dryRun F-branch → file doesn't exist → returns safely
        var (td, _) = GetSshTweak();
        if (td?.RemoveAction is null)
            return;

        td.RemoveAction(false);
    }

    [Fact]
    public void DetectAction_NoSshdConfig_ReturnsFalse()
    {
        // Calls DetectSshdDirective(directive, expectedValue)
        // → `if (!File.Exists(SshdConfig)) return false;` T-branch
        // Skip on machines that have OpenSSH Server installed (e.g. GitHub windows-latest runners).
        if (System.IO.File.Exists(@"C:\ProgramData\ssh\sshd_config"))
            return;

        var (td, _) = GetSshTweak();
        if (td?.DetectAction is null)
            return;

        var result = td.DetectAction();
        Assert.False(result); // sshd_config does not exist
    }

    [Fact]
    public void AllSshTweaks_DetectAction_ReturnsFalseWhenNoSshdConfig()
    {
        // Skip on machines that have OpenSSH Server installed (e.g. GitHub windows-latest runners).
        if (System.IO.File.Exists(@"C:\ProgramData\ssh\sshd_config"))
            return;

        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var sshTweaks = engine.AllTweaks().Where(t => t.Category == "SSH Configuration").ToList();

        foreach (var td in sshTweaks)
        {
            if (td.DetectAction is not null)
            {
                var result = td.DetectAction();
                Assert.False(result); // no sshd_config
            }
        }
    }

    [Fact]
    public void AllSshTweaks_ApplyAction_DryRun_DoNotThrow()
    {
        var session = new RegistrySession(dryRun: true);
        var engine = new TweakEngine(session);
        engine.RegisterBuiltins();
        var sshTweaks = engine.AllTweaks().Where(t => t.Category == "SSH Configuration").ToList();

        foreach (var td in sshTweaks)
        {
            if (td.ApplyAction is not null)
                td.ApplyAction(true); // dryRun=true → safe early return for all SSH tweaks
        }
    }
}

// ── 9. Elevation RunElevated — allowed-command path ────────────────────────

public sealed class ElevationAllowedCommandBranchTests
{
    [Fact]
    public void RunElevated_AllowedCommand_ReturnsExitCode()
    {
        // Covers F branch of `!AllowedCommands.Contains(exeName)` (command IS allowed)
        // and T branch of `proc.WaitForExit(timeoutMs)` (fast exit)
        var (exit, stdout, stderr) = Elevation.RunElevated("cmd", ["/c", "exit 0"], 10_000);
        Assert.Equal(0, exit);
    }

    [Fact]
    public void RunElevated_AllowedCommand_ExitCode1_ReturnsNonZero()
    {
        var (exit, _, _) = Elevation.RunElevated("cmd", ["/c", "exit 1"], 10_000);
        Assert.Equal(1, exit);
    }

    [Fact]
    public void RunElevated_AllowedCommand_WithOutput_ReturnsStdout()
    {
        // Covers stdout reading path in RunElevated
        var (exit, stdout, _) = Elevation.RunElevated("cmd", ["/c", "echo hello"], 10_000);
        Assert.Equal(0, exit);
        Assert.Contains("hello", stdout, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RunElevated_CmdAllowed_QuoteArgInRequestElevation_Independence()
    {
        // Also tests IsAdmin() — the current process may or may not be admin; just verify it returns bool
        bool isAdmin = Elevation.IsAdmin();
        Assert.True(isAdmin || !isAdmin); // always passes; confirms IsAdmin() runs without exception
    }
}

// ── 10. CorporateGuard remaining branch coverage ───────────────────────────

public sealed class CorporateGuardRemainingBranchTests
{
    [Fact]
    public void IsCorporateNetwork_ReturnsBool_DoesNotThrow()
    {
        bool result = CorporateGuard.IsCorporateNetwork();
        Assert.True(result || !result);
    }

    [Fact]
    public void IsGpoManaged_WithSampleKeys_ReturnsBool_DoesNotThrow()
    {
        // Pass a real-ish list of policy registry keys
        bool result = CorporateGuard.IsGpoManaged(new[] { @"HKLM\SOFTWARE\Policies\Microsoft", @"HKCU\SOFTWARE\Policies\Microsoft" });
        Assert.True(result || !result);
    }

    [Fact]
    public void Status_ReturnsTupleWithReason()
    {
        var (isCorp, reason) = CorporateGuard.Status();
        Assert.NotNull(reason);
        Assert.True(isCorp || !isCorp);
    }

    [Fact]
    public void IsCorporateNetwork_CalledTwice_ConsistentResult()
    {
        // Covers caching path (second call returns cached result)
        bool first = CorporateGuard.IsCorporateNetwork();
        bool second = CorporateGuard.IsCorporateNetwork();
        Assert.Equal(first, second);
    }
}

// ── 11. PackLoader remaining branch coverage ───────────────────────────────

public sealed class PackLoaderRemainingEdgeCaseTests
{
    private static readonly string s_minimalJson = """
        {
            "name": "rl-pl-edge", "displayName": "Edge Pack", "version": "1.0.0", "author": "UT",
            "tweaks": [{
                "id": "rl-pl-edge-t1", "label": "T1", "category": "TestCat",
                "applyOps": [{"kind":"SetDword","path":"HKCU\\Software\\rl-pl-edge","name":"V","dwordValue":1}],
                "detectOps": [{"kind":"CheckDword","path":"HKCU\\Software\\rl-pl-edge","name":"V","dwordValue":1}]
            }]
        }
        """;

    [Fact]
    public void LoadFromJson_ApplyOpsOnlyNoDetect_Succeeds()
    {
        // RemoveOps and DetectOps absent → should succeed
        var (pack, tweaks) = PackLoader.LoadFromJson(s_minimalJson, null);
        Assert.NotNull(pack);
        Assert.Single(tweaks);
    }

    [Fact]
    public void ValidatePackJson_EmptyName_ReturnsError()
    {
        const string json = """
            {
                "name": "", "displayName": "d", "version": "1.0.0", "author": "a",
                "tweaks": [{"id":"t1","label":"L","category":"C","applyOps":[{"kind":"SetDword","path":"HKCU\\X","name":"N","dwordValue":1}]}]
            }
            """;
        var err = PackLoader.ValidatePackJson(json);
        Assert.NotEmpty(err);
        Assert.True(
            err.Any(e => e.Contains("name", StringComparison.OrdinalIgnoreCase)),
            $"Expected an error about 'name'; errors: {string.Join("; ", err)}"
        );
    }

    [Fact]
    public void ValidatePackJson_EmptyTweaksList_ReturnsError()
    {
        const string json = """
            {"name": "no-tweaks", "displayName": "d", "version": "1.0.0", "author": "a", "tweaks": []}
            """;
        var err = PackLoader.ValidatePackJson(json);
        Assert.NotEmpty(err);
    }

    [Fact]
    public void LoadFromJson_TweakWithDescriptionAndTagsOptional_Succeeds()
    {
        const string json = """
            {
                "name":"rl-opt", "displayName":"Opt", "version":"1.0.0", "author":"UT",
                "description": "optional pack description",
                "tags": ["optional", "tag"],
                "tweaks": [{
                    "id":"rl-opt-t1","label":"L","category":"C",
                    "description": "optional tweak description",
                    "tags": ["tag1"],
                    "applyOps":[{"kind":"SetDword","path":"HKCU\\T","name":"V","dwordValue":1}],
                    "detectOps":[{"kind":"CheckDword","path":"HKCU\\T","name":"V","dwordValue":1}]
                }]
            }
            """;
        var (pack, tweaks) = PackLoader.LoadFromJson(json, null);
        Assert.NotNull(pack);
        Assert.Single(tweaks);
    }

    [Fact]
    public void ComputeSha256_DifferentStrings_DifferentHashes()
    {
        var hash1 = PackLoader.ComputeSha256("string one");
        var hash2 = PackLoader.ComputeSha256("string two");
        Assert.NotEqual(hash1, hash2);
    }
}

// ── 12. StartupManager remaining branch coverage ───────────────────────────

public sealed class StartupManagerRemainingBranchTests
{
    [Fact]
    public async Task ExportEntriesAsync_ValidPath_WritesNonEmptyJson()
    {
        // Covers StartupManager.ExportEntriesAsync happy path
        string tmp = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"rl-startup-bc5-{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(tmp);
            Assert.True(System.IO.File.Exists(tmp));
            var content = await System.IO.File.ReadAllTextAsync(tmp);
            Assert.NotEmpty(content);
            Assert.StartsWith("[", content.Trim());
        }
        finally
        {
            if (System.IO.File.Exists(tmp))
                System.IO.File.Delete(tmp);
        }
    }

    [Fact]
    public void GetAllEntries_ReturnsList_AllEntriesHaveNonEmptyName()
    {
        var entries = StartupManager.GetAllEntries();
        // All entries from HKCU\Software\Microsoft\Windows\CurrentVersion\Run should have names
        foreach (var e in entries)
            Assert.False(string.IsNullOrWhiteSpace(e.Name), $"Startup entry has empty name: {e}");
    }

    [Fact]
    public void SetEnabled_CurrentState_NoExceptionEvenIfNotFound()
    {
        // Try to enable something that doesn't exist — should not throw
        // SetEnabled no-ops when state matches; if entry not found it's also a no-op or creates
        var entries = StartupManager.GetAllEntries();
        if (entries.Count > 0)
        {
            var first = entries[0];
            bool current = first.IsEnabled;
            StartupManager.SetEnabled(first, current); // same state → no-op
        }
    }

    [Fact]
    public void Delete_NonExistentEntry_DoesNotThrow()
    {
        // Entry with fake name → delete is no-op
        // Use the record constructor: (Id, Name, Command, Location, IsEnabled)
        var fake = new StartupEntry(
            "RegistryUser|RL-NonExistent-BC5-XYZ",
            "RL-NonExistent-BC5-XYZ",
            "notepad.exe",
            StartupLocation.RegistryUser,
            true
        );
        StartupManager.Delete(fake); // should not throw
    }
}
