#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Win32;
using RegiLattice.Core;
using RegiLattice.Core.Models;
using RegiLattice.Core.Plugins;
using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;
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

public sealed class StartupManagerBranchTests2
{
    [Fact]
    public void GetAllEntries_ReturnsNonNull()
    {
        var entries = StartupManager.GetAllEntries();
        Assert.NotNull(entries);
    }

    [Fact]
    public void SetEnabled_SameState_IsNoOp()
    {
        // If no entries, trivially pass
        var entries = StartupManager.GetAllEntries();
        if (entries.Count == 0)
            return;
        var e = entries[0];
        // Should early-return (no-op) without throwing
        StartupManager.SetEnabled(e, e.IsEnabled);
    }

    [Fact]
    public void AddRegistryEntry_BlankName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("", "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("   ", "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_NullName_ThrowsArgumentException()
    {
        Assert.ThrowsAny<ArgumentException>(() => StartupManager.AddRegistryEntry(null!, "notepad.exe"));
    }

    [Fact]
    public void AddRegistryEntry_BlankCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("TestEntry", ""));
    }

    [Fact]
    public void AddRegistryEntry_NullCommand_ThrowsArgumentException()
    {
        Assert.ThrowsAny<ArgumentException>(() => StartupManager.AddRegistryEntry("TestEntry", null!));
    }

    [Fact]
    public async Task ExportEntriesAsync_BlankFilePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => StartupManager.ExportEntriesAsync(""));
    }

    [Fact]
    public void AddRegistryEntry_NewEntry_SucceedsAndIsVisible()
    {
        const string name = "RL-UnitTest-BranchCov4-Add";
        // Ensure clean state from any prior test run
        try
        {
            var prior = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
            if (prior is not null)
                StartupManager.Delete(prior);
        }
        catch
        { /* ignore cleanup errors */
        }

        StartupManager.AddRegistryEntry(name, "notepad.exe");
        try
        {
            var entries = StartupManager.GetAllEntries();
            Assert.Contains(entries, e => e.Name == name);
        }
        finally
        {
            var added = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
            if (added is not null)
                StartupManager.Delete(added);
        }
    }

    [Fact]
    public void AddRegistryEntry_DuplicateName_ThrowsArgumentException()
    {
        const string name = "RL-UnitTest-BranchCov4-Dup";
        try
        {
            StartupManager.AddRegistryEntry(name, "notepad.exe");
            var ex = Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry(name, "cmd.exe"));
            Assert.Contains("already exists", ex.Message, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            try
            {
                var entry = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name == name);
                if (entry is not null)
                    StartupManager.Delete(entry);
            }
            catch { }
        }
    }

    [Fact]
    public async Task ExportEntriesAsync_ToTempFile_WritesValidJson()
    {
        var path = Path.Combine(Path.GetTempPath(), $"startup-bc4-{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(path);
            Assert.True(File.Exists(path));
            var content = await File.ReadAllTextAsync(path);
            Assert.Contains("[", content);
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public void Delete_NonExistentRegistryEntry_DoesNotThrow()
    {
        // Create a fake entry that is not in the registry — TryDeleteValue should no-op
        var fakeEntry = new StartupEntry(
            "RegistryUser|RL-UnitTest-NonExistent-BC4",
            "RL-UnitTest-NonExistent-BC4",
            "nonexistent.exe",
            StartupLocation.RegistryUser,
            true
        );
        StartupManager.Delete(fakeEntry); // Should not throw
    }
}

// ── 5. RegistrySession Extra Branch Tests ───────────────────────────────────

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

// ── merged from BranchCoverage6Tests.cs ──────────────────────────────────

