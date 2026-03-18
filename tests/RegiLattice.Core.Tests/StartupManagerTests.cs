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
}
