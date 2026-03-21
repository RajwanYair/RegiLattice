// RegiLattice.Core — Services/ConflictDetector.cs
// Static registry of known-conflicting tweak pairs with human-readable reasons.
// Usage: ConflictDetector.Detect(selectedIds) → list of (id1, id2, reason) triples.

#nullable enable
namespace RegiLattice.Core.Services;

/// <summary>Describes a detected conflict between two tweaks.</summary>
public readonly record struct TweakConflict(string Id1, string Id2, string Reason);

/// <summary>
/// Detects known-conflicting tweak pairs in a set of IDs.
/// The conflict list is maintained as a compile-time constant — no runtime I/O.
/// </summary>
public static class ConflictDetector
{
    // ── Known conflict table ─────────────────────────────────────────────────
    // Format: (idA, idB, human-readable reason why they conflict)
    // Kept ordered alphabetically on id1 for easy auditing.
    private static readonly TweakConflict[] _known =
    [
        // ── HVCI / Virtualization-Based Security ─────────────────────────────
        new(
            "energy-enable-hardware-accelerated-gpu-scheduling",
            "sac-disable-hvci",
            "HAGS requires the GPU driver model that HVCI-off also modifies; enabling both may cause display driver instability."
        ),
        new(
            "sac-disable-hvci",
            "sac-disable-virtualization-based-security",
            "HVCI is a VBS sub-feature; disabling VBS already disables HVCI. Setting both targets overlapping policy keys and may produce unexpected state."
        ),
        new(
            "sac-disable-virtualization-based-security",
            "virt-enable-hyper-v",
            "Hyper-V requires VBS. Disabling VBS while enabling Hyper-V will leave Hyper-V in a non-functional or degraded state."
        ),
        // ── Windows Defender / Smart App Control ─────────────────────────────
        new(
            "harden-disable-defender-realtime",
            "sac-set-evaluation-mode",
            "Smart App Control evaluation mode relies on Microsoft Defender's real-time protection for cloud reputation checks; disabling real-time protection defeats SAC evaluation."
        ),
        new(
            "harden-disable-defender-realtime",
            "sac-disable-intelligent-security-graph",
            "ISG is the cloud reputation backend for SAC and Defender; disabling real-time protection and ISG together removes all behavioural blocking."
        ),
        // ── Windows Search / Indexing ─────────────────────────────────────────
        new(
            "svc-disable-winsearch",
            "idx-disable-indexing",
            "Both tweaks target the Windows Search indexer service and search index respectively; applying both is redundant and may conflict during service restart sequences."
        ),
        // ── DNS-over-HTTPS ────────────────────────────────────────────────────
        new(
            "dns-enforce-doh",
            "dns-disable-doh",
            "Enforcing DoH and disabling DoH write to the same EnableAutoDoh registry value with opposite values; whichever applies last wins, producing unpredictable DNS behaviour."
        ),
        // ── Power management ──────────────────────────────────────────────────
        new(
            "energy-disable-energy-saver-on-ac",
            "energy-enable-efficiency-mode-background",
            "Efficiency Mode requires Energy Saver to be active for its process throttling heuristics on AC; disabling Energy Saver on AC partially undermines Efficiency Mode."
        ),
        new(
            "pwrmgmt-enable-fast-startup",
            "boot-disable-fast-startup",
            "Fast startup (Hybrid Boot) is set to enabled by one tweak and disabled by the other; the result depends on apply order and is always inconsistent."
        ),
        // ── Windows Update ────────────────────────────────────────────────────
        new(
            "wu-pause-updates",
            "wu-enable-auto-updates",
            "Pausing updates and enabling auto-updates write to conflicting policy keys; the outcome depends on apply order."
        ),
        // ── Recall / Windows AI ───────────────────────────────────────────────
        new(
            "cplplus-disable-recall-snapshots",
            "recall-enable-storage-sense-recall",
            "One tweak disables Recall snapshot capture while the other enables Storage Sense to manage Recall data; both operate on mutually exclusive Recall policy and service states."
        ),
        // ── Bluetooth ─────────────────────────────────────────────────────────
        new(
            "bt-disable-bluetooth",
            "bt-enable-bt-le-audio",
            "BT LE Audio requires the Bluetooth stack to be active; disabling Bluetooth renders LE Audio tweaks ineffective."
        ),
        // ── Xbox Game Bar / Game DVR ──────────────────────────────────────────
        new(
            "xbgb-disable-game-dvr-policy",
            "game-enable-game-dvr",
            "The game DVR policy tweak disables capture via HKLM policy; the Game DVR enable tweak sets the user-level AppCaptureEnabled flag. The policy key overrides the user key."
        ),
        // ── UAC & Elevation ───────────────────────────────────────────────────
        new(
            "uac-disable-uac",
            "harden-enable-uac-always-notify",
            "Disabling UAC and requiring always-notify are contradictory UAC prompt level settings."
        ),
        // ── Sleep / Hibernate ─────────────────────────────────────────────────
        new(
            "pwrmgmt-disable-hibernate",
            "pwrmgmt-enable-hibernate-fast-startup",
            "Fast startup depends on the hibernate file (hiberfil.sys); disabling hibernation removes this file, making fast startup fall back to a cold boot regardless."
        ),
        // ── Remote Desktop ────────────────────────────────────────────────────
        new(
            "rdp-disable-rdp",
            "rdp-enable-rdp-nla",
            "Network Level Authentication for RDP is a configuration of an enabled RDP service; disabling RDP service makes the NLA setting moot but the conflicting registry state is misleading."
        ),
    ];

    // ── Public API ────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns all known conflicts among the supplied tweak IDs.
    /// The candidate set is matched symmetrically (order-independent).
    /// </summary>
    /// <param name="ids">Set of tweak IDs to check (e.g., currently selected tweaks).</param>
    /// <returns>List of conflict records; empty when no conflicts are detected.</returns>
    public static IReadOnlyList<TweakConflict> Detect(IEnumerable<string> ids)
    {
        var set = new HashSet<string>(ids, StringComparer.OrdinalIgnoreCase);
        if (set.Count < 2)
            return [];

        var results = new List<TweakConflict>();
        foreach (ref readonly var c in _known.AsSpan())
        {
            if (set.Contains(c.Id1) && set.Contains(c.Id2))
                results.Add(c);
        }
        return results;
    }

    /// <summary>
    /// Returns all conflict records that involve <paramref name="id"/> (in either position).
    /// Useful for "check before apply" single-tweak validation.
    /// </summary>
    public static IReadOnlyList<TweakConflict> ConflictsFor(string id, IEnumerable<string> appliedIds)
    {
        var set = new HashSet<string>(appliedIds, StringComparer.OrdinalIgnoreCase);
        var results = new List<TweakConflict>();
        foreach (ref readonly var c in _known.AsSpan())
        {
            bool involvesCurrent =
                string.Equals(c.Id1, id, StringComparison.OrdinalIgnoreCase) || string.Equals(c.Id2, id, StringComparison.OrdinalIgnoreCase);
            if (!involvesCurrent)
                continue;

            string other = string.Equals(c.Id1, id, StringComparison.OrdinalIgnoreCase) ? c.Id2 : c.Id1;
            if (set.Contains(other))
                results.Add(c);
        }
        return results;
    }

    /// <summary>All statically registered conflict pairs (for test enumeration).</summary>
    public static IReadOnlyList<TweakConflict> AllConflicts => _known;
}
