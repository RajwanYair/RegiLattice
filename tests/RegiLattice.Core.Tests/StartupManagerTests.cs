#nullable enable

using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for the Sprint-28 StartupManager service (read-only operations).</summary>
public sealed class StartupManagerTests
{
    [Fact]
    public void GetAllEntries_ReturnsCollection()
    {
        // On any real Windows machine there is at least one startup entry.
        // The test just validates the method doesn't throw and returns a list.
        var entries = StartupManager.GetAllEntries();
        Assert.NotNull(entries);
    }

    [Fact]
    public void GetAllEntries_AllEntries_HaveNonEmptyId()
    {
        var entries = StartupManager.GetAllEntries();
        Assert.All(entries, e => Assert.False(string.IsNullOrEmpty(e.Id)));
    }

    [Fact]
    public void GetAllEntries_AllEntries_HaveNonEmptyName()
    {
        var entries = StartupManager.GetAllEntries();
        Assert.All(entries, e => Assert.False(string.IsNullOrEmpty(e.Name)));
    }

    [Fact]
    public void GetAllEntries_AllEntries_HaveValidLocation()
    {
        var validLocations = new[]
        {
            StartupLocation.RegistryUser,
            StartupLocation.RegistryMachine,
            StartupLocation.FolderUser,
            StartupLocation.FolderAllUsers,
        };
        var entries = StartupManager.GetAllEntries();
        Assert.All(entries, e => Assert.Contains(e.Location, validLocations));
    }

    [Fact]
    public void StartupLocation_FolderUser_PathIsResolvable()
    {
        // Verify the Startup folder path can be resolved on this machine
        string folder = Environment.GetFolderPath(Environment.SpecialFolder.Startup);
        Assert.False(string.IsNullOrEmpty(folder));
    }

    [Fact]
    public void StartupEntry_Record_CanBeConstructed()
    {
        var entry = new StartupEntry(
            Id: "reg:user:TestApp",
            Name: "TestApp",
            Command: @"C:\test\app.exe",
            Location: StartupLocation.RegistryUser,
            IsEnabled: true
        );

        Assert.Equal("reg:user:TestApp", entry.Id);
        Assert.Equal("TestApp", entry.Name);
        Assert.Equal(StartupLocation.RegistryUser, entry.Location);
        Assert.True(entry.IsEnabled);
    }

    [Fact]
    public void StartupEntry_EqualityByValue()
    {
        var a = new StartupEntry("id1", "App", "cmd.exe", StartupLocation.RegistryUser, true);
        var b = new StartupEntry("id1", "App", "cmd.exe", StartupLocation.RegistryUser, true);
        Assert.Equal(a, b);
    }

    // ── AddRegistryEntry — input validation ──────────────────────────

    [Fact]
    public void AddRegistryEntry_NullName_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StartupManager.AddRegistryEntry(null!, @"C:\test.exe"));
    }

    [Fact]
    public void AddRegistryEntry_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("", @"C:\test.exe"));
    }

    [Fact]
    public void AddRegistryEntry_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("   ", @"C:\test.exe"));
    }

    [Fact]
    public void AddRegistryEntry_NullCommand_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() => StartupManager.AddRegistryEntry("TestApp", null!));
    }

    [Fact]
    public void AddRegistryEntry_EmptyCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("TestApp", ""));
    }

    [Fact]
    public void AddRegistryEntry_WhitespaceCommand_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry("TestApp", "   "));
    }

    // ── AddRegistryEntry — round-trip (add + cleanup) ────────────────

    [Fact]
    public void AddRegistryEntry_NewEntry_AppearsInGetAllEntries()
    {
        const string testName = "RegiLatticeTestEntry_Do_Not_Use";
        // Ensure clean state
        try
        {
            StartupManager.Delete(new StartupEntry($"RegistryUser|{testName}", testName, "", StartupLocation.RegistryUser, true));
        }
        catch
        { /* ignore if not present */
        }

        try
        {
            StartupManager.AddRegistryEntry(testName, @"C:\Windows\System32\notepad.exe");
            var entries = StartupManager.GetAllEntries();
            Assert.Contains(
                entries,
                e => e.Name.Equals(testName, StringComparison.OrdinalIgnoreCase) && e.Location == StartupLocation.RegistryUser && e.IsEnabled
            );
        }
        finally
        {
            // Clean up: delete the test entry regardless of outcome
            try
            {
                var toDelete = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name.Equals(testName, StringComparison.OrdinalIgnoreCase));
                if (toDelete is not null)
                    StartupManager.Delete(toDelete);
            }
            catch
            { /* best-effort cleanup */
            }
        }
    }

    [Fact]
    public void AddRegistryEntry_DuplicateName_ThrowsArgumentException()
    {
        const string testName = "RegiLatticeTestDupe_Do_Not_Use";
        // Ensure clean state
        try
        {
            StartupManager.Delete(new StartupEntry($"RegistryUser|{testName}", testName, "", StartupLocation.RegistryUser, true));
        }
        catch
        { /* ignore */
        }

        try
        {
            StartupManager.AddRegistryEntry(testName, @"C:\Windows\System32\notepad.exe");
            Assert.Throws<ArgumentException>(() => StartupManager.AddRegistryEntry(testName, @"C:\Windows\System32\notepad.exe"));
        }
        finally
        {
            try
            {
                var toDelete = StartupManager.GetAllEntries().FirstOrDefault(e => e.Name.Equals(testName, StringComparison.OrdinalIgnoreCase));
                if (toDelete is not null)
                    StartupManager.Delete(toDelete);
            }
            catch
            { /* best-effort cleanup */
            }
        }
    }

    // ── ExportEntriesAsync ────────────────────────────────────────────

    [Fact]
    public async Task ExportEntriesAsync_CreatesJsonFile()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl-startup-test-{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(path);
            Assert.True(File.Exists(path));
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public async Task ExportEntriesAsync_FileContainsValidJson()
    {
        string path = Path.Combine(Path.GetTempPath(), $"rl-startup-test-{Guid.NewGuid():N}.json");
        try
        {
            await StartupManager.ExportEntriesAsync(path);
            string json = await File.ReadAllTextAsync(path);
            Assert.False(string.IsNullOrWhiteSpace(json));
            // Must be a JSON array (can be empty [] or populated)
            string trimmed = json.Trim();
            Assert.True(trimmed.StartsWith('['), $"Expected JSON array, got: {trimmed[..Math.Min(50, trimmed.Length)]}");
        }
        finally
        {
            if (File.Exists(path))
                File.Delete(path);
        }
    }

    [Fact]
    public async Task ExportEntriesAsync_EmptyPath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => StartupManager.ExportEntriesAsync(""));
    }

    [Fact]
    public async Task ExportEntriesAsync_WhitespacePath_ThrowsArgumentException()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => StartupManager.ExportEntriesAsync("   "));
    }

    // ── SetEnabled — no-op guard ──────────────────────────────────────

    [Fact]
    public void SetEnabled_WhenAlreadyEnabled_DoesNotThrow()
    {
        // Create a folder entry (already enabled) — SetEnabled(enable=true) is a no-op
        var entry = new StartupEntry(
            Id: "FolderUser|FakeApp",
            Name: "FakeApp",
            Command: @"C:\nonexistent\app.lnk",
            Location: StartupLocation.FolderUser,
            IsEnabled: true
        );
        // Should return immediately without throwing (IsEnabled already matches)
        var ex = Record.Exception(() => StartupManager.SetEnabled(entry, enable: true));
        Assert.Null(ex);
    }

    [Fact]
    public void SetEnabled_WhenAlreadyDisabled_DoesNotThrow()
    {
        var entry = new StartupEntry(
            Id: "FolderUser|FakeApp",
            Name: "FakeApp",
            Command: @"C:\nonexistent\app.disabled",
            Location: StartupLocation.FolderUser,
            IsEnabled: false
        );
        var ex = Record.Exception(() => StartupManager.SetEnabled(entry, enable: false));
        Assert.Null(ex);
    }

    // ── StartupLocation enum ─────────────────────────────────────────

    [Theory]
    [InlineData(StartupLocation.RegistryUser)]
    [InlineData(StartupLocation.RegistryMachine)]
    [InlineData(StartupLocation.FolderUser)]
    [InlineData(StartupLocation.FolderAllUsers)]
    public void StartupLocation_AllValues_AreDefined(StartupLocation loc)
    {
        Assert.True(Enum.IsDefined(loc));
    }

    [Fact]
    public void StartupLocation_HasExactlyFourValues()
    {
        Assert.Equal(4, Enum.GetValues<StartupLocation>().Length);
    }
}
