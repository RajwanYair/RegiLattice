// tests/RegiLattice.Core.Tests/TweakMigrationServiceTests.cs
// Phase 6.5 coverage — TweakMigrationService: resolve, migrate, query, and TweakEngine proxy.

using RegiLattice.Core.Registry;
using RegiLattice.Core.Services;
using Xunit;

namespace RegiLattice.Core.Tests;

/// <summary>Tests for TweakMigrationService: built-in migrations, ResolveMigration, MigrateIds, and TweakEngine delegation.</summary>
public sealed class TweakMigrationServiceTests
{
    // ── Migrations property ───────────────────────────────────────────────────

    [Fact]
    public void Migrations_Default_IsNotEmpty()
    {
        var svc = new TweakMigrationService();
        Assert.NotEmpty(svc.Migrations);
    }

    [Fact]
    public void Migrations_ExtraSupplied_AreMergedWithBuiltins()
    {
        var extra = new TweakMigration
        {
            OldId = "custom-old",
            NewId = "custom-new",
            Version = "6.28.0",
            Reason = "Test",
        };
        var svc = new TweakMigrationService([extra]);

        Assert.Contains(svc.Migrations, m => m.OldId == "custom-old");
        // Built-ins still present
        Assert.True(svc.Migrations.Count > 1);
    }

    // ── ResolveMigration ─────────────────────────────────────────────────────

    [Fact]
    public void ResolveMigration_KnownOldId_ReturnsNewId()
    {
        var svc = new TweakMigrationService();
        // priv-disable-telemetry-basic → priv-disable-telemetry (from v5.95.0)
        string? result = svc.ResolveMigration("priv-disable-telemetry-basic");
        Assert.Equal("priv-disable-telemetry", result);
    }

    [Fact]
    public void ResolveMigration_DeprecatedId_ReturnsNull()
    {
        var svc = new TweakMigrationService();
        // harden-disable-smb1-legacy was deprecated with no replacement
        string? result = svc.ResolveMigration("harden-disable-smb1-legacy");
        Assert.Null(result);
    }

    [Fact]
    public void ResolveMigration_UnknownId_ReturnsOriginalId()
    {
        var svc = new TweakMigrationService();
        string? result = svc.ResolveMigration("no-migration-exists-for-this-id");
        Assert.Equal("no-migration-exists-for-this-id", result);
    }

    [Fact]
    public void ResolveMigration_CaseInsensitive_Resolves()
    {
        var svc = new TweakMigrationService();
        // Known old ID tested in mixed case
        string? lower = svc.ResolveMigration("priv-disable-telemetry-basic");
        string? upper = svc.ResolveMigration("PRIV-DISABLE-TELEMETRY-BASIC");
        string? mixed = svc.ResolveMigration("Priv-Disable-Telemetry-Basic");

        Assert.Equal(lower, upper);
        Assert.Equal(lower, mixed);
    }

    [Fact]
    public void ResolveMigration_NullOrWhitespace_ThrowsArgumentException()
    {
        var svc = new TweakMigrationService();
        Assert.Throws<ArgumentException>(() => svc.ResolveMigration(""));
        Assert.Throws<ArgumentException>(() => svc.ResolveMigration("   "));
    }

    // ── MigrateIds ───────────────────────────────────────────────────────────

    [Fact]
    public void MigrateIds_EmptyList_ReturnsEmpty()
    {
        var svc = new TweakMigrationService();
        var result = svc.MigrateIds([]);
        Assert.Empty(result);
    }

    [Fact]
    public void MigrateIds_UnknownIds_PassThrough()
    {
        var svc = new TweakMigrationService();
        var result = svc.MigrateIds(["my-id-a", "my-id-b"]);
        Assert.Equal(new[] { "my-id-a", "my-id-b" }, result);
    }

    [Fact]
    public void MigrateIds_KnownOldId_IsResolved()
    {
        var svc = new TweakMigrationService();
        var result = svc.MigrateIds(["priv-disable-telemetry-basic", "my-other-id"]);
        Assert.Contains("priv-disable-telemetry", result);
        Assert.DoesNotContain("priv-disable-telemetry-basic", result);
        Assert.Contains("my-other-id", result);
    }

    [Fact]
    public void MigrateIds_DeprecatedId_IsSkipped()
    {
        var svc = new TweakMigrationService();
        var result = svc.MigrateIds(["harden-disable-smb1-legacy", "priv-disable-telemetry-basic"]);
        // Deprecated ID omitted; renamed ID resolved
        Assert.DoesNotContain("harden-disable-smb1-legacy", result);
        Assert.Contains("priv-disable-telemetry", result);
        Assert.Single(result);
    }

    [Fact]
    public void MigrateIds_Deduplicates_WhenMultipleMigrateToSameId()
    {
        var extra1 = new TweakMigration
        {
            OldId = "old-id-1",
            NewId = "shared-new",
            Version = "6.28.0",
            Reason = "Merge 1",
        };
        var extra2 = new TweakMigration
        {
            OldId = "old-id-2",
            NewId = "shared-new",
            Version = "6.28.0",
            Reason = "Merge 2",
        };
        var svc = new TweakMigrationService([extra1, extra2]);

        var result = svc.MigrateIds(["old-id-1", "old-id-2"]);

        // Both map to "shared-new"; deduplication should yield only one entry
        Assert.Single(result);
        Assert.Equal("shared-new", result[0]);
    }

    [Fact]
    public void MigrateIds_NullInput_ThrowsArgumentNullException()
    {
        var svc = new TweakMigrationService();
        Assert.Throws<ArgumentNullException>(() => svc.MigrateIds(null!));
    }

    // ── HasMigration / GetMigration ──────────────────────────────────────────

    [Fact]
    public void HasMigration_KnownId_ReturnsTrue()
    {
        var svc = new TweakMigrationService();
        Assert.True(svc.HasMigration("priv-disable-telemetry-basic"));
    }

    [Fact]
    public void HasMigration_UnknownId_ReturnsFalse()
    {
        var svc = new TweakMigrationService();
        Assert.False(svc.HasMigration("completely-unknown-tweak-id"));
    }

    [Fact]
    public void GetMigration_KnownId_ReturnsMigrationRecord()
    {
        var svc = new TweakMigrationService();
        var m = svc.GetMigration("priv-disable-telemetry-basic");

        Assert.NotNull(m);
        Assert.Equal("priv-disable-telemetry-basic", m!.OldId);
        Assert.Equal("priv-disable-telemetry", m.NewId);
        Assert.False(string.IsNullOrWhiteSpace(m.Version));
        Assert.False(string.IsNullOrWhiteSpace(m.Reason));
    }

    [Fact]
    public void GetMigration_UnknownId_ReturnsNull()
    {
        var svc = new TweakMigrationService();
        Assert.Null(svc.GetMigration("no-such-id"));
    }

    // ── TweakEngine proxy ────────────────────────────────────────────────────

    [Fact]
    public void TweakEngine_Migrations_IsNotEmpty()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        Assert.NotEmpty(engine.Migrations);
    }

    [Fact]
    public void TweakEngine_ResolveMigration_KnownOldId_ReturnsNewId()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        string? resolved = engine.ResolveMigration("priv-disable-telemetry-basic");
        Assert.Equal("priv-disable-telemetry", resolved);
    }

    [Fact]
    public void TweakEngine_ResolveMigration_DeprecatedId_ReturnsNull()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        Assert.Null(engine.ResolveMigration("harden-disable-smb1-legacy"));
    }

    [Fact]
    public void TweakEngine_ResolveMigration_UnknownId_ReturnsOriginal()
    {
        var engine = new TweakEngine(new RegistrySession(dryRun: true));
        string? resolved = engine.ResolveMigration("totally-unknown-xxx");
        Assert.Equal("totally-unknown-xxx", resolved);
    }
}
