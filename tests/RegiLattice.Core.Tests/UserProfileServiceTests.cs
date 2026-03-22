// RegiLattice.Core.Tests — UserProfileServiceTests.cs
// Tests for UserProfileService — Sprint 110.
// Uses portable mode + temp directory for file-system isolation.

using RegiLattice.Core;
using Xunit;

namespace RegiLattice.Core.Tests;

public sealed class UserProfileServiceTests : IDisposable
{
    public UserProfileServiceTests()
    {
        // Clean up any stale profile files before each test.
        UserProfileService.DeleteAll();
    }

    public void Dispose()
    {
        // Ensure test profiles are removed after each test.
        UserProfileService.DeleteAll();
    }

    private static readonly string[] FewTweakIds = ["perf-disable-animations", "priv-disable-telemetry"];

    // ── ProfilesDir / ProfilePath ────────────────────────────────────────

    [Fact]
    public void ProfilesDir_ContainsProfilesSubdir()
    {
        Assert.EndsWith("profiles", UserProfileService.ProfilesDir);
    }

    [Fact]
    public void ProfilePath_ReturnsJsonFileInProfilesDir()
    {
        string path = UserProfileService.ProfilePath("my-test");
        Assert.EndsWith("my-test.json", path);
        Assert.StartsWith(UserProfileService.ProfilesDir, path);
    }

    // ── Exists ───────────────────────────────────────────────────────────

    [Fact]
    public void Exists_NonExistentProfile_ReturnsFalse()
    {
        Assert.False(UserProfileService.Exists("no-such-profile"));
    }

    [Fact]
    public void Exists_AfterCreate_ReturnsTrue()
    {
        UserProfileService.Create("my-profile", "desc", FewTweakIds);
        Assert.True(UserProfileService.Exists("my-profile"));
    }

    // ── Create ───────────────────────────────────────────────────────────

    [Fact]
    public void Create_ValidProfile_PersistsFile()
    {
        var profile = UserProfileService.Create("sprint110", "Sprint 110 test", FewTweakIds);
        Assert.NotNull(profile);
        Assert.Equal("sprint110", profile.Name);
        Assert.Equal("Sprint 110 test", profile.Description);
        Assert.Equal(2, profile.TweakIds.Count);
    }

    [Fact]
    public void Create_DuplicateName_ThrowsInvalidOperationException()
    {
        UserProfileService.Create("dup-test", "first", FewTweakIds);
        Assert.Throws<InvalidOperationException>(() => UserProfileService.Create("dup-test", "second", FewTweakIds));
    }

    [Fact]
    public void Create_EmptyName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => UserProfileService.Create("", "desc", FewTweakIds));
    }

    [Fact]
    public void Create_WhitespaceName_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => UserProfileService.Create("   ", "desc", FewTweakIds));
    }

    // ── GetProfile ───────────────────────────────────────────────────────

    [Fact]
    public void GetProfile_ExistingProfile_ReturnsCorrectProfile()
    {
        UserProfileService.Create("fetch-me", "desc", FewTweakIds);
        var fetched = UserProfileService.GetProfile("fetch-me");
        Assert.NotNull(fetched);
        Assert.Equal("fetch-me", fetched.Name);
    }

    [Fact]
    public void GetProfile_NonExistentProfile_ReturnsNull()
    {
        Assert.Null(UserProfileService.GetProfile("does-not-exist"));
    }

    // ── GetProfiles ──────────────────────────────────────────────────────

    [Fact]
    public void GetProfiles_NoProfiles_ReturnsEmpty()
    {
        Assert.Empty(UserProfileService.GetProfiles());
    }

    [Fact]
    public void GetProfiles_MultipleProfiles_ReturnsSortedAlphabetically()
    {
        UserProfileService.Create("zebra", "z", FewTweakIds);
        UserProfileService.Create("alpha", "a", FewTweakIds);
        UserProfileService.Create("middle", "m", FewTweakIds);

        var profiles = UserProfileService.GetProfiles();

        Assert.Equal(3, profiles.Count);
        Assert.Equal("alpha", profiles[0].Name);
        Assert.Equal("middle", profiles[1].Name);
        Assert.Equal("zebra", profiles[2].Name);
    }

    // ── Update ───────────────────────────────────────────────────────────

    [Fact]
    public void Update_ExistingProfile_UpdatesTweakIds()
    {
        UserProfileService.Create("upd-test", "desc", FewTweakIds);
        string[] newIds = ["perf-disable-animations"];
        var updated = UserProfileService.Update("upd-test", newIds);
        Assert.Equal(1, updated.TweakIds.Count);
    }

    [Fact]
    public void Update_NonExistentProfile_ThrowsInvalidOperationException()
    {
        Assert.Throws<InvalidOperationException>(() => UserProfileService.Update("no-such-profile", FewTweakIds));
    }

    // ── Rename ───────────────────────────────────────────────────────────

    [Fact]
    public void Rename_ExistingProfile_NewNameExists()
    {
        UserProfileService.Create("old-name", "desc", FewTweakIds);
        UserProfileService.Rename("old-name", "new-name");
        Assert.True(UserProfileService.Exists("new-name"));
        Assert.False(UserProfileService.Exists("old-name"));
    }

    [Fact]
    public void Rename_TargetAlreadyExists_ThrowsInvalidOperationException()
    {
        UserProfileService.Create("src-profile", "desc", FewTweakIds);
        UserProfileService.Create("dst-profile", "desc", FewTweakIds);
        Assert.Throws<InvalidOperationException>(() => UserProfileService.Rename("src-profile", "dst-profile"));
    }

    // ── Clone ────────────────────────────────────────────────────────────

    [Fact]
    public void Clone_ExistingProfile_BothExist()
    {
        var original = UserProfileService.Create("original", "desc", FewTweakIds);
        var cloned = UserProfileService.Clone("original", "cloned");
        Assert.True(UserProfileService.Exists("original"));
        Assert.True(UserProfileService.Exists("cloned"));
        Assert.Equal(original.TweakIds, cloned.TweakIds);
    }

    [Fact]
    public void Clone_TargetAlreadyExists_ThrowsInvalidOperationException()
    {
        UserProfileService.Create("a", "desc", FewTweakIds);
        UserProfileService.Create("b", "desc", FewTweakIds);
        Assert.Throws<InvalidOperationException>(() => UserProfileService.Clone("a", "b"));
    }

    // ── Delete ───────────────────────────────────────────────────────────

    [Fact]
    public void Delete_ExistingProfile_RemovedFromDisk()
    {
        UserProfileService.Create("to-delete", "desc", FewTweakIds);
        UserProfileService.Delete("to-delete");
        Assert.False(UserProfileService.Exists("to-delete"));
    }

    [Fact]
    public void Delete_NonExistentProfile_DoesNotThrow()
    {
        var ex = Record.Exception(() => UserProfileService.Delete("no-such-profile"));
        Assert.Null(ex);
    }

    // ── DeleteAll ────────────────────────────────────────────────────────

    [Fact]
    public void DeleteAll_ClearsAllProfiles()
    {
        UserProfileService.Create("p1", "desc", FewTweakIds);
        UserProfileService.Create("p2", "desc", FewTweakIds);
        UserProfileService.DeleteAll();
        Assert.Empty(UserProfileService.GetProfiles());
    }

    // ── Save / round-trip ────────────────────────────────────────────────

    [Fact]
    public void Save_ThenGetProfile_ReturnsSameData()
    {
        var created = UserProfileService.Create("roundtrip", "RT", FewTweakIds);
        var fetched = UserProfileService.GetProfile("roundtrip");
        Assert.NotNull(fetched);
        Assert.Equal(created.Name, fetched.Name);
        Assert.Equal(created.Description, fetched.Description);
        Assert.Equal(created.TweakIds, fetched.TweakIds);
    }
}
