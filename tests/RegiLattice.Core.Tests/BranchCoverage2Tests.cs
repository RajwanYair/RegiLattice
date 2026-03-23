// tests/RegiLattice.Core.Tests/BranchCoverage2Tests.cs
// Sprint 121 T6.1 — second-pass branch-coverage push targeting:
//   · ComplianceReportExporter (0% → ~90%)
//   · StartupManager HKCU write/delete/toggle (14% → ~60%)
//   · PowerPlanManager live read-only calls (60% → ~80%)
//   · ComplianceDrift / ComplianceService uncovered paths
//   · ConfigExporter additional import branches

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

// ═══════════════════════════════════════════════════════════════════════════
// 1.  ComplianceReportExporter — currently 0 % branch coverage
// ═══════════════════════════════════════════════════════════════════════════

public sealed class ComplianceReportExporterBranchTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public ComplianceReportExporterBranchTests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    // ── null statusMap → all tweaks treated as Unknown ───────────────────

    [Fact]
    public void BuildHtml_NullStatusMap_ReturnsHtmlWithUnknownBadges()
    {
        string html = ComplianceReportExporter.BuildHtml(_engine, null);

        Assert.Contains("Unknown", html);
        Assert.Contains("Compliance Report", html);
    }

    // ── empty statusMap → same Unknown behaviour, healthPct = 0 ─────────

    [Fact]
    public void BuildHtml_EmptyStatusMap_HealthPercentIsZero()
    {
        string html = ComplianceReportExporter.BuildHtml(_engine, new Dictionary<string, TweakResult>());

        // All tweaks are Unknown → evaluated = 0 → healthPct = 0
        Assert.Contains("Health: 0%", html);
    }

    // ── Applied status → r-applied class, Applied badge, applied counter ─

    [Fact]
    public void BuildHtml_SomeApplied_ContainsAppliedBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Applied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-applied", html);
        Assert.Contains("Applied", html);
    }

    // ── NotApplied status → r-pending class, Pending badge ───────────────

    [Fact]
    public void BuildHtml_SomeNotApplied_ContainsPendingBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.NotApplied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-pending", html);
        Assert.Contains("Pending", html);
    }

    // ── Unknown status explicitly set ─────────────────────────────────────

    [Fact]
    public void BuildHtml_UnknownStatusExplicit_ContainsUnknownBadge()
    {
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Unknown };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("b-unknown", html);
    }

    // ── healthPct > 0 when at least one tweak is Applied ─────────────────

    [Fact]
    public void BuildHtml_AllApplied_HealthIs100()
    {
        var tweaks = _engine.AllTweaks();
        var status = tweaks.ToDictionary(t => t.Id, _ => TweakResult.Applied);

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains("Health: 100%", html);
    }

    // ── mixed Applied + NotApplied → healthPct between 0 and 100 ─────────

    [Fact]
    public void BuildHtml_MixedAppliedAndPending_HealthIsBetween0And100()
    {
        var tweaks = _engine.AllTweaks();
        var half = tweaks.Count / 2;
        var status = new Dictionary<string, TweakResult>();
        for (int i = 0; i < half; i++)
            status[tweaks[i].Id] = TweakResult.Applied;
        for (int i = half; i < tweaks.Count; i++)
            status[tweaks[i].Id] = TweakResult.NotApplied;

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        // Should contain some health percentage
        Assert.Contains("Health:", html);
        Assert.DoesNotContain("Health: 0%", html);
        Assert.DoesNotContain("Health: 100%", html);
    }

    // ── NeedsAdmin=false → "No" in the Admin column ──────────────────────

    [Fact]
    public void BuildHtml_TweakWithNeedsAdminFalse_ShowsNo()
    {
        // Find any tweak where NeedsAdmin = false
        var noAdmin = _engine.AllTweaks().FirstOrDefault(t => !t.NeedsAdmin);
        if (noAdmin is null)
        {
            // Skip gracefully if all tweaks require admin (unlikely but safe)
            return;
        }

        var status = new Dictionary<string, TweakResult> { [noAdmin.Id] = TweakResult.Applied };
        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        Assert.Contains(">No<", html);
    }

    // ── ExportHtml writes to a file ───────────────────────────────────────

    [Fact]
    public void ExportHtml_WritesToFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl_compliance_{Guid.NewGuid():N}.html");
        try
        {
            ComplianceReportExporter.ExportHtml(_engine, null, path);
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            Assert.Contains("<!DOCTYPE html>", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── ExportHtml with explicit status map ──────────────────────────────

    [Fact]
    public void ExportHtml_WithStatusMap_WritesFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl_compliance_{Guid.NewGuid():N}.html");
        var first = _engine.AllTweaks()[0];
        var status = new Dictionary<string, TweakResult> { [first.Id] = TweakResult.Applied };
        try
        {
            ComplianceReportExporter.ExportHtml(_engine, status, path);
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── Status id NOT in dictionary → Unknown (ContainsKey=false branch) ─

    [Fact]
    public void BuildHtml_TweakIdNotInStatusMap_TreatedAsUnknown()
    {
        // Use a status map that has no entries matching engine's tweaks
        var status = new Dictionary<string, TweakResult> { ["nonexistent-id"] = TweakResult.Applied };

        string html = ComplianceReportExporter.BuildHtml(_engine, status);

        // All real tweaks should be Unknown
        Assert.Contains("b-unknown", html);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 2.  StartupManager — currently 14 % branch coverage (7 tests cover GetAllEntries only)
//     All writes go to HKCU which requires no admin; cleaned up via Delete().
// ═══════════════════════════════════════════════════════════════════════════

public sealed class StartupManagerBranchTests
{
    // Unique test key prefix to avoid collisions if tests run in parallel
    private static string UniqueKey(string suffix) => $"_RegiLatticeTest_{suffix}";

    // ── AddRegistryEntry — happy path ─────────────────────────────────────

    [Fact]
    public void AddRegistryEntry_ValidArgs_AppearsInGetAllEntries()
    {
        string name = UniqueKey(Guid.NewGuid().ToString("N")[..8]);
        try
        {
            StartupManager.AddRegistryEntry(name, "notepad.exe --test");
            var entries = StartupManager.GetAllEntries();
            Assert.Contains(entries, e => e.Name == name);
        }
        finally
        {
            // Cleanup: Delete the entry we added
            var entries = StartupManager.GetAllEntries();
            var added = entries.FirstOrDefault(e => e.Name == name);
            if (added is not null)
                StartupManager.Delete(added);
        }
    }

    // ── AddRegistryEntry — duplicate throws ──────────────────────────────

    [Fact]
    public void AddRegistryEntry_DuplicateName_ThrowsArgumentException()
    {
        string name = UniqueKey(Guid.NewGuid().ToString("N")[..8]);
        try
        {
            StartupManager.AddRegistryEntry(name, "first.exe");
            Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry(name, "second.exe"));
        }
        finally
        {
            var entries = StartupManager.GetAllEntries();
            var added = entries.FirstOrDefault(e => e.Name == name);
            if (added is not null)
                StartupManager.Delete(added);
        }
    }

    // ── AddRegistryEntry — blank name / command throws ───────────────────

    [Fact]
    public void AddRegistryEntry_BlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("   ", "cmd.exe"));
    }

    [Fact]
    public void AddRegistryEntry_BlankCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("SomeName", "   "));
    }

    // ── Delete — registry user entry ─────────────────────────────────────

    [Fact]
    public void Delete_RegistryUserEntry_RemovedFromGetAllEntries()
    {
        string name = UniqueKey(Guid.NewGuid().ToString("N")[..8]);
        StartupManager.AddRegistryEntry(name, "toDelete.exe");

        var entries = StartupManager.GetAllEntries();
        var entry = entries.First(e => e.Name == name);
        StartupManager.Delete(entry);

        entries = StartupManager.GetAllEntries();
        Assert.DoesNotContain(entries, e => e.Name == name);
    }

    // ── SetEnabled — no-op when state matches ────────────────────────────

    [Fact]
    public void SetEnabled_SameStateAsAlreadyEnabled_DoesNotThrow()
    {
        // Create an entry in a known state (enabled = true)
        var entry = new StartupEntry(
            Id: "RegistryUser|NoOp",
            Name: "NoOp",
            Command: "noop.exe",
            Location: StartupLocation.RegistryUser,
            IsEnabled: true
        );
        // Calling SetEnabled with same value as IsEnabled → early return, no registry access
        StartupManager.SetEnabled(entry, true);
    }

    [Fact]
    public void SetEnabled_SameStateAsAlreadyDisabled_DoesNotThrow()
    {
        var entry = new StartupEntry(
            Id: "RegistryUser|NoOp2",
            Name: "NoOp2",
            Command: "noop.exe",
            Location: StartupLocation.RegistryUser,
            IsEnabled: false
        );
        StartupManager.SetEnabled(entry, false);
    }

    // ── Folder-based entry: Delete (file exists) ─────────────────────────

    [Fact]
    public void Delete_FolderEntry_FileExists_DeletesFile()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"RL_startup_{Guid.NewGuid():N}.lnk");
        File.WriteAllText(tempFile, "placeholder");

        var entry = new StartupEntry(
            Id: $"FolderUser|{Path.GetFileNameWithoutExtension(tempFile)}",
            Name: Path.GetFileNameWithoutExtension(tempFile),
            Command: tempFile,
            Location: StartupLocation.FolderUser,
            IsEnabled: true
        );

        StartupManager.Delete(entry);

        Assert.False(File.Exists(tempFile));
    }

    // ── Folder-based entry: Delete (file does not exist) ─────────────────

    [Fact]
    public void Delete_FolderEntry_FileMissing_DoesNotThrow()
    {
        string nonExistent = Path.Combine(Path.GetTempPath(), "RL_nonexistent_startup.lnk");
        var entry = new StartupEntry(
            Id: "FolderUser|Missing",
            Name: "Missing",
            Command: nonExistent,
            Location: StartupLocation.FolderUser,
            IsEnabled: true
        );

        // Should not throw even though file doesn't exist
        StartupManager.Delete(entry);
    }

    // ── Folder-based entry: SetEnabled (disable → renames to .disabled) ──

    [Fact]
    public void SetEnabled_FolderEntry_Disable_RenamesFileWithDisabledExtension()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), $"RL_startup_{Guid.NewGuid():N}.lnk");
        string disabledPath = tempFile + ".disabled";
        File.WriteAllText(tempFile, "placeholder");

        try
        {
            var entry = new StartupEntry(
                Id: $"FolderUser|{Path.GetFileNameWithoutExtension(tempFile)}",
                Name: Path.GetFileNameWithoutExtension(tempFile),
                Command: tempFile,
                Location: StartupLocation.FolderUser,
                IsEnabled: true
            );

            StartupManager.SetEnabled(entry, false);

            Assert.False(File.Exists(tempFile));
            Assert.True(File.Exists(disabledPath));
        }
        finally
        {
            if (File.Exists(disabledPath))
                File.Delete(disabledPath);
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    // ── Folder-based entry: SetEnabled (enable → renames from .disabled) ─

    [Fact]
    public void SetEnabled_FolderEntry_Enable_RemovesDisabledExtension()
    {
        string baseName = Path.Combine(Path.GetTempPath(), $"RL_startup_{Guid.NewGuid():N}.lnk");
        string disabledPath = baseName + ".disabled";
        File.WriteAllText(disabledPath, "placeholder");

        try
        {
            var entry = new StartupEntry(
                Id: $"FolderUser|{Path.GetFileNameWithoutExtension(baseName)}",
                Name: Path.GetFileNameWithoutExtension(baseName),
                Command: disabledPath,
                Location: StartupLocation.FolderAllUsers,
                IsEnabled: false
            );

            StartupManager.SetEnabled(entry, true);

            Assert.True(File.Exists(baseName));
            Assert.False(File.Exists(disabledPath));
        }
        finally
        {
            if (File.Exists(baseName))
                File.Delete(baseName);
            if (File.Exists(disabledPath))
                File.Delete(disabledPath);
        }
    }

    // ── ExportEntriesAsync: writes valid JSON ─────────────────────────────

    [Fact]
    public async Task ExportEntriesAsync_WritesJsonFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"RL_startup_export_{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(path);
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            // The JSON is an array of StartupEntry objects
            Assert.Contains("[", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    // ── ReadFolderEntries — covers isDisabled = true branch ──────────────

    [Fact]
    public void GetAllEntries_WithDisabledFileInStartupFolder_IncludesDisabledEntry()
    {
        // Create a temp .lnk.disabled file in the user startup folder
        string startupFolder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        if (!Directory.Exists(startupFolder))
            return; // skip if folder doesn't exist (CI environment)

        string disabledFile = Path.Combine(startupFolder, $"RL_test_{Guid.NewGuid():N}.lnk.disabled");
        try
        {
            File.WriteAllText(disabledFile, "test");
            var entries = StartupManager.GetAllEntries();
            // There should be at least one disabled folder entry
            var disabledEntry = entries.FirstOrDefault(e => e.Location == StartupLocation.FolderUser && !e.IsEnabled);
            Assert.NotNull(disabledEntry);
        }
        finally
        {
            if (File.Exists(disabledFile))
                File.Delete(disabledFile);
        }
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 3.  PowerPlanManager — read-only live calls (safe, powercfg.exe exists on Windows)
//     GetAllPlans() and GetActivePlanGuid() are currently uncovered.
// ═══════════════════════════════════════════════════════════════════════════

public sealed class PowerPlanManagerLiveTests
{
    [Fact]
    public void GetAllPlans_ReturnsAtLeastOnePlan()
    {
        // powercfg.exe is present on all Windows installations.
        var plans = PowerPlanManager.GetAllPlans();
        Assert.NotEmpty(plans);
    }

    [Fact]
    public void GetAllPlans_AllEntries_HaveNonEmptyName()
    {
        var plans = PowerPlanManager.GetAllPlans();
        Assert.All(plans, p => Assert.False(string.IsNullOrWhiteSpace(p.Name)));
    }

    [Fact]
    public void GetActivePlanGuid_ReturnsNonNullGuid()
    {
        Guid? active = PowerPlanManager.GetActivePlanGuid();
        Assert.NotNull(active);
        Assert.NotEqual(Guid.Empty, active.Value);
    }

    [Fact]
    public void GetAllPlans_ExactlyOneIsActive()
    {
        var plans = PowerPlanManager.GetAllPlans();
        int activeCount = plans.Count(p => p.IsActive);
        // There should be exactly one active plan
        Assert.Equal(1, activeCount);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 4.  ComplianceDrift / ComplianceService — uncovered branch paths
// ═══════════════════════════════════════════════════════════════════════════

public sealed class ComplianceDriftAdditionalTests : IClassFixture<BuiltinsFixture>
{
    private readonly TweakEngine _engine;

    public ComplianceDriftAdditionalTests(BuiltinsFixture fixture) => _engine = fixture.Engine;

    // ── IsViolation when BaselineStatus != Applied (uncovered branch) ────

    [Fact]
    public void ComplianceDrift_IsViolation_FalseWhenBaselineIsNotApplied()
    {
        // BaselineStatus = NotApplied → IsViolation = false (first condition fails)
        var drift = new ComplianceDrift
        {
            TweakId = "x",
            Label = "X",
            Category = "C",
            BaselineStatus = TweakResult.NotApplied,
            CurrentStatus = TweakResult.NotApplied,
        };
        Assert.False(drift.IsViolation);
    }

    [Fact]
    public void ComplianceDrift_IsViolation_FalseWhenBaselineIsUnknown()
    {
        var drift = new ComplianceDrift
        {
            TweakId = "x",
            Label = "X",
            Category = "C",
            BaselineStatus = TweakResult.Unknown,
            CurrentStatus = TweakResult.NotApplied,
        };
        Assert.False(drift.IsViolation);
    }

    // ── ComplianceReport.ViolationCount = 0 when Drifted is empty ────────

    [Fact]
    public void ComplianceReport_ViolationCount_ZeroWhenNoDrift()
    {
        var report = new ComplianceReport
        {
            Drifted = [],
            TotalChecked = 0,
            CheckedAt = DateTime.UtcNow,
        };
        Assert.Equal(0, report.ViolationCount);
        Assert.True(report.IsCompliant);
    }

    // ── ComplianceService.Check: baseline id missing from engine ─────────

    [Fact]
    public void ComplianceService_Check_BaselineIdNotInEngine_SkipsGracefully()
    {
        // Build a small engine with no tweaks matching the baseline ids
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "dummy-tweak",
                Label = "Dummy",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\Test", "Dummy", 1)],
            },
        ]);

        // Baseline contains an id that's NOT in the engine → `if (td is null) continue`
        var baseline = new Dictionary<string, string> { ["nonexistent-id"] = "applied" };
        var report = ComplianceService.Check(engine, baseline);

        // The non-existent id should be skipped, result should be empty or minimal
        Assert.NotNull(report);
    }

    // ── ComplianceService.Check: case-insensitive "APPLIED" recognition ──

    [Fact]
    public void ComplianceService_Check_BaselineWithUppercaseApplied_IsRecognized()
    {
        var engine = new TweakEngine();
        engine.Register([
            new TweakDef
            {
                Id = "ci-test-tweak",
                Label = "CI Test",
                Category = "Test",
                ApplyOps = [RegOp.SetDword(@"HKCU\Software\RegiLattice\CITest", "Val", 1)],
            },
        ]);

        // "APPLIED" (uppercase) should be recognised as the applied state
        var baseline = new Dictionary<string, string> { ["ci-test-tweak"] = "APPLIED" };
        var report = ComplianceService.Check(engine, baseline);

        // TotalChecked = 1 (one baseline-applied tweak was found)
        Assert.Equal(1, report.TotalChecked);
    }

    // ── ComplianceService.CheckFromFile: non-existent file ───────────────

    [Fact]
    public void ComplianceService_CheckFromFile_FileNotFound_ReturnsSentinelReport()
    {
        var report = ComplianceService.CheckFromFile(_engine, @"C:\nonexistent\path\snap.json");
        // The method returns a report with TotalChecked = -1 when file can't be loaded
        Assert.Equal(-1, report.TotalChecked);
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// 5.  ServiceManager async export / SetStartType uncovered paths
// ═══════════════════════════════════════════════════════════════════════════

public sealed class ServiceManagerBranchTests
{
    // ServiceManager.GetAllServices() is a read-only WMI/sc.exe query — safe to call.
    [Fact]
    public void GetAllServices_ReturnsCollection()
    {
        var services = ServiceManager.GetAllServices();
        Assert.NotNull(services);
        // On any Windows machine there will be services
        Assert.NotEmpty(services);
    }

    [Fact]
    public void GetAllServices_AllEntries_HaveNonEmptyServiceName()
    {
        var services = ServiceManager.GetAllServices();
        Assert.All(services, s => Assert.False(string.IsNullOrWhiteSpace(s.ServiceName)));
    }

    [Fact]
    public void ServiceEntry_Record_CanBeConstructed()
    {
        var entry = new ServiceEntry(
            "Spooler",
            "Print Spooler",
            "Manages print jobs",
            System.ServiceProcess.ServiceControllerStatus.Running,
            System.ServiceProcess.ServiceStartMode.Automatic,
            CanStop: true,
            CanPauseAndContinue: false
        );
        Assert.Equal("Spooler", entry.ServiceName);
        Assert.Equal("Print Spooler", entry.DisplayName);
        Assert.Equal(System.ServiceProcess.ServiceControllerStatus.Running, entry.Status);
        Assert.Equal(System.ServiceProcess.ServiceStartMode.Automatic, entry.StartType);
        Assert.True(entry.CanStop);
        Assert.False(entry.CanPauseAndContinue);
    }

    [Fact]
    public async Task ExportToCsvAsync_WritesValidCsv()
    {
        string path = Path.Combine(Path.GetTempPath(), $"RL_services_{Guid.NewGuid():N}.csv");
        try
        {
            await ServiceManager.ExportToCsvAsync(path);
            Assert.True(File.Exists(path));
            string content = File.ReadAllText(path);
            // CSV should have a header row
            Assert.Contains("Name", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
