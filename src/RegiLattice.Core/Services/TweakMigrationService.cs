// RegiLattice.Core — Services/TweakMigrationService.cs
// Phase 6.5: Tweak versioning & deprecation — migration registry for renamed/merged/deprecated tweak IDs.
// Enables snapshots and profiles saved with old IDs to be automatically resolved to current IDs.

#nullable enable

using System;
using System.Collections.Generic;
using System.Linq;

namespace RegiLattice.Core.Services;

/// <summary>Represents a renamed, merged, or deprecated tweak ID migration.</summary>
public sealed record TweakMigration
{
    /// <summary>The old (deprecated) tweak ID.</summary>
    public required string OldId { get; init; }

    /// <summary>
    /// The replacement tweak ID, or <c>null</c> if the tweak was fully deprecated with no replacement.
    /// </summary>
    public string? NewId { get; init; }

    /// <summary>The product version string when this migration was introduced, e.g. <c>"6.28.0"</c>.</summary>
    public required string Version { get; init; }

    /// <summary>Human-readable explanation of why the ID changed.</summary>
    public required string Reason { get; init; }
}

/// <summary>
/// Maintains a registry of known tweak ID migrations (renames, merges, deprecations).
/// Consumers can resolve stale IDs from saved snapshots, profiles, and favourites to
/// their current equivalents before passing them to <see cref="TweakEngine"/>.
/// </summary>
public sealed class TweakMigrationService
{
    // ── Built-in migration table ─────────────────────────────────────────────
    // Historical renames and merges from the v5→v6 consolidation campaigns.
    private static readonly IReadOnlyList<TweakMigration> s_builtinMigrations =
    [
        // v5.95.0 — removed 21 duplicate tweaks (consolidated into primary module owners)
        new TweakMigration
        {
            OldId = "priv-disable-telemetry-basic",
            NewId = "priv-disable-telemetry",
            Version = "5.95.0",
            Reason = "Merged: duplicate of priv-disable-telemetry",
        },
        new TweakMigration
        {
            OldId = "perf-vis-effects-legacy",
            NewId = "perf-visual-effects",
            Version = "5.95.0",
            Reason = "Renamed for slug consistency",
        },

        // v5.97.0 — cross-module moves (wrong category assignment fixed)
        new TweakMigration
        {
            OldId = "sec-disable-wifi-sense-sharing",
            NewId = "net-disable-wifi-sense",
            Version = "5.97.0",
            Reason = "Moved to net module: WiFi Sense is a network feature, not a security tweak",
        },
        new TweakMigration
        {
            OldId = "svc-disable-remote-registry",
            NewId = "rdp-disable-remote-registry",
            Version = "5.97.0",
            Reason = "Moved to rdp module: Remote Registry is an RDP-adjacent service",
        },

        // v5.98.0 — Explorer-related tweaks moved out of perf/debloat modules
        new TweakMigration
        {
            OldId = "perf-disable-thumbnail-cache",
            NewId = "explorer-disable-thumbnail-cache",
            Version = "5.98.0",
            Reason = "Moved to explorer module: thumbnail cache is an Explorer feature",
        },
        new TweakMigration
        {
            OldId = "debloat-remove-cortana",
            NewId = "cortana-disable-cortana",
            Version = "5.98.0",
            Reason = "Moved to cortana module; merged into cortana-disable-cortana",
        },

        // v6.8.0 — slug pattern normalisation (svc-disable-<service>)
        new TweakMigration
        {
            OldId = "svc-bits-disable",
            NewId = "svc-disable-bits",
            Version = "6.8.0",
            Reason = "Renamed for consistent slug pattern: svc-disable-<service>",
        },
        new TweakMigration
        {
            OldId = "svc-wer-disable",
            NewId = "svc-disable-wer",
            Version = "6.8.0",
            Reason = "Renamed for consistent slug pattern: svc-disable-<service>",
        },
        new TweakMigration
        {
            OldId = "harden-disable-netbios",
            NewId = "net-disable-netbios",
            Version = "6.8.0",
            Reason = "Moved to net module: NetBIOS is a network protocol",
        },

        // v6.10.0 — scoop tool-install tweaks removed (not registry operations)
        new TweakMigration
        {
            OldId = "scoop-install-everything",
            NewId = null,
            Version = "6.10.0",
            Reason = "Deprecated: scoop tool-install tweaks removed in Phase 6 (not registry operations)",
        },
        new TweakMigration
        {
            OldId = "scoop-install-fzf",
            NewId = null,
            Version = "6.10.0",
            Reason = "Deprecated: scoop tool-install tweaks removed in Phase 6 (not registry operations)",
        },

        // v6.12.0 — redundant policy tweaks removed (Windows Update already handles these)
        new TweakMigration
        {
            OldId = "harden-disable-smb1-legacy",
            NewId = null,
            Version = "6.12.0",
            Reason = "Deprecated: Windows Update already disables SMB1 on all supported builds",
        },
    ];

    private readonly Dictionary<string, TweakMigration> _byOldId;
    private readonly IReadOnlyList<TweakMigration> _allMigrations;

    /// <param name="extraMigrations">Optional caller-supplied migrations (e.g., from plugin packs).</param>
    public TweakMigrationService(IEnumerable<TweakMigration>? extraMigrations = null)
    {
        var all = new List<TweakMigration>(s_builtinMigrations);
        if (extraMigrations is not null)
            all.AddRange(extraMigrations);
        _allMigrations = all;
        _byOldId = all.ToDictionary(m => m.OldId, m => m, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>All registered migrations (built-in + any extras supplied at construction).</summary>
    public IReadOnlyList<TweakMigration> Migrations => _allMigrations;

    /// <summary>
    /// Resolve <paramref name="oldId"/> to its current tweak ID.
    /// <list type="bullet">
    ///   <item>Returns <c>null</c> if the tweak is deprecated with no replacement.</item>
    ///   <item>Returns <paramref name="oldId"/> unchanged if no migration is registered for it.</item>
    ///   <item>Returns the <see cref="TweakMigration.NewId"/> when a rename/merge is registered.</item>
    /// </list>
    /// </summary>
    public string? ResolveMigration(string oldId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(oldId);
        return _byOldId.TryGetValue(oldId, out var migration) ? migration.NewId : oldId;
    }

    /// <summary>
    /// Migrate a collection of tweak IDs, resolving known renames and filtering out deprecations.
    /// Preserves order; removes duplicates (case-insensitive comparison).
    /// </summary>
    /// <returns>Resolved IDs with duplicates removed; deprecated IDs are omitted entirely.</returns>
    public IReadOnlyList<string> MigrateIds(IEnumerable<string> ids)
    {
        ArgumentNullException.ThrowIfNull(ids);
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var result = new List<string>();
        foreach (var id in ids)
        {
            string? resolved = ResolveMigration(id);
            if (resolved is not null && seen.Add(resolved))
                result.Add(resolved);
        }

        return result;
    }

    /// <summary>Returns <c>true</c> if a migration entry is registered for <paramref name="oldId"/>.</summary>
    public bool HasMigration(string oldId) => _byOldId.ContainsKey(oldId);

    /// <summary>
    /// Returns the <see cref="TweakMigration"/> for <paramref name="oldId"/>, or <c>null</c>
    /// if no migration is registered.
    /// </summary>
    public TweakMigration? GetMigration(string oldId) => _byOldId.GetValueOrDefault(oldId);
}
