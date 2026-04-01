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
        // ── Network / IPv6 / Proxy ────────────────────────────────────────────
        new(
            "net-disable-ipv6",
            "net-enable-teredo",
            "Teredo tunneling is an IPv6 transition technology; disabling IPv6 on all interfaces renders Teredo tunneling completely non-functional."
        ),
        new(
            "proxy-disable-proxy-autodetect",
            "proxy-enable-wpad",
            "WPAD (Web Proxy Auto-Discovery) is the underlying protocol for automatic proxy detection; disabling WPAD while enabling auto-detect writes contradictory values to the same AutoDetect policy key."
        ),
        new(
            "net-enable-large-send-offload",
            "netopt-disable-tcp-offload",
            "Large Send Offload is one component of TCP hardware offloading; enabling LSO while disabling TCP offload globally writes conflicting values to the same NIC driver parameters."
        ),
        new(
            "net-disable-netbios",
            "net-enable-netbios-over-tcp",
            "These tweaks write opposing values to the NetbiosOptions registry key (Disabled vs. Enabled); the last operation applied wins and the other takes no effect."
        ),
        // ── Firewall ──────────────────────────────────────────────────────────
        new(
            "fw-disable-windows-firewall",
            "fw-enable-firewall-logging",
            "Firewall packet logging requires the Windows Firewall service to be active; disabling the firewall service makes logging settings inoperative."
        ),
        new(
            "fw-disable-windows-firewall",
            "fw-block-outbound-by-default",
            "Default outbound blocking is a firewall rule enforced by the active firewall service; disabling the firewall service overrides all outbound block rules."
        ),
        new(
            "fw-disable-windows-firewall",
            "harden-enable-advanced-firewall-profiles",
            "Domain, Private, and Public firewall profiles cannot be enabled when the firewall service is stopped; both tweaks target the same service and produce an inconsistent state."
        ),
        // ── Defender (extended) ───────────────────────────────────────────────
        new(
            "harden-disable-defender-realtime",
            "harden-enable-defender-cloud-protection",
            "Cloud protection pushes real-time signature updates to the resident scanning engine; disabling real-time protection removes the delivery target for cloud-based detections."
        ),
        new(
            "harden-disable-defender-realtime",
            "sec-enable-exploit-protection",
            "Exploit protection integrates with the Defender real-time scanning pipeline for behavioural detections; disabling real-time scanning weakens exploit detection in-process."
        ),
        new(
            "harden-disable-tamper-protection",
            "harden-enable-defender-realtime",
            "Tamper protection prevents policy changes from disabling Defender real-time scanning; disabling tamper protection while enabling real-time via policy creates a contradictory enforcement state that is easily reversed."
        ),
        // ── Power plans ───────────────────────────────────────────────────────
        new(
            "pwrmgmt-enable-performance-power-plan",
            "pwrmgmt-enable-power-saver-plan",
            "These activate mutually exclusive power plan GUIDs; the plan applied last is active and the earlier one has no effect, leading to an unexpected power configuration."
        ),
        new(
            "power-disable-connected-standby",
            "pwrmgmt-enable-modern-standby",
            "Connected Standby and Modern Standby target the same S0 low-power idle state; disabling one while enabling the other writes conflicting values to the PlatformAoAcOverride registry key."
        ),
        // ── Print Spooler ─────────────────────────────────────────────────────
        new(
            "svc-disable-print-spooler",
            "printing-enable-printer-sharing",
            "Printer sharing over SMB requires the Print Spooler service to be running; disabling Spooler prevents any shared printer from being visible to other machines."
        ),
        new(
            "svc-disable-print-spooler",
            "printing-disable-print-to-pdf",
            "Print to PDF (Microsoft Print to PDF) is a virtual printer rendered by the XPS Writer which depends on the Print Spooler; disabling Spooler deactivates all virtual printers including Print to PDF."
        ),
        // ── Privacy / Telemetry (extended) ────────────────────────────────────
        new(
            "priv-disable-activity-history",
            "ai-enable-copilot-timeline",
            "Copilot Timeline features are powered by Activity History data; disabling Activity History removes the data source that Timeline requires to display past activity."
        ),
        new(
            "telem-disable-diagnostic-data",
            "priv-enable-error-reporting",
            "Diagnostic data collection and error reporting share the Connected User Experience and Telemetry (diagtrack) pipeline; setting AllowTelemetry=0 also blocks the error reporting data path."
        ),
        new(
            "priv-disable-telemetry",
            "crash-enable-wer-upload",
            "The AllowTelemetry=0 HKLM policy enforced by disable-telemetry also blocks the WER data upload path, making the crash enable-wer-upload tweak ineffective."
        ),
        // ── Display / Graphics ────────────────────────────────────────────────
        new(
            "gpu-disable-hardware-accelerated-gpu-scheduling",
            "energy-enable-hardware-accelerated-gpu-scheduling",
            "These tweaks write directly opposing values to the same HwSchMode registry value; whichever is applied last wins."
        ),
        new(
            "display-enable-hdr",
            "night-enable-night-light",
            "Night Light applies an orange colour temperature shift that overrides HDR colour calibration; using both simultaneously degrades HDR colour accuracy and tone-mapping."
        ),
        // ── Explorer / Shell ──────────────────────────────────────────────────
        new(
            "explorer-disable-file-extensions",
            "explorer-show-file-extensions",
            "These tweaks write opposite values (1 vs 0) to the HideFileExt DWORD; the last one applied controls the visible state."
        ),
        new(
            "shell-enable-legacy-context-menu",
            "ctx-enable-modern-context-menu",
            "The legacy and modern context menus are toggled by the same UxTheme registry key; enabling both writes contradictory values and the visible menu depends on apply order."
        ),
        new(
            "explorer-disable-thumbnail-cache",
            "explorer-enable-thumbnail-cache",
            "Direct inversion of the DisableThumbnailCache DWORD; the last tweak applied determines whether thumbnails are cached."
        ),
        // ── Clipboard / Input ─────────────────────────────────────────────────
        new(
            "clip-disable-clipboard-history",
            "clip-enable-cloud-clipboard-sync",
            "Cross-device Cloud Clipboard (clipboard sync) is built on top of the local Clipboard History feature; disabling Clipboard History removes the storage layer that cloud sync depends on."
        ),
        new(
            "input-disable-predictive-text",
            "input-enable-hardware-keyboard-suggestions",
            "Physical-keyboard text suggestions are generated by the same predictive text engine; disabling predictions silently disables suggestions as well, leaving the suggestion setting in a misleadingly enabled state."
        ),
        // ── Storage / File System ─────────────────────────────────────────────
        new(
            "fs-disable-8dot3-names",
            "ssd-enable-8dot3-ntfs",
            "These write opposing values to the NtfsDisable8dot3NameCreation registry DWORD; the last tweak applied determines whether short file names are created."
        ),
        new(
            "ssd-disable-ntfs-last-access",
            "fs-enable-ntfs-last-access-time",
            "Direct inversion of the NtfsDisableLastAccessUpdate DWORD; disabling last-access timestamps and explicitly enabling them write contradictory values."
        ),
        // ── UAC (extended) ────────────────────────────────────────────────────
        new(
            "uac-disable-uac",
            "uac-enable-virtualization",
            "UAC Virtualisation (registry and file system redirection for legacy apps) requires UAC to be active; disabling UAC suppresses the virtualisation layer entirely."
        ),
        new(
            "uac-set-prompt-behavior-1",
            "uac-set-prompt-behavior-5",
            "These tweaks write mutually exclusive ConsentPromptBehaviorUser values (1=prompt for credentials vs 5=prompt for consent); the last one applied controls the UAC prompt level."
        ),
        // ── Windows Update (extended) ─────────────────────────────────────────
        new(
            "wu-defer-feature-updates",
            "wu-enable-insider-preview",
            "Deferring feature updates applies a DeferFeatureUpdatesPeriodInDays policy that blocks the Insider Preview delivery channel from pushing pre-release builds."
        ),
        new(
            "wu-disable-automatic-restart",
            "wu-enable-scheduled-restart",
            "Disabling automatic restart sets NoAutoRebootWithLoggedOnUsers=1 while enabling scheduled restart requires it to be 0; both values cannot be effective simultaneously."
        ),
        // ── Cortana / Voice ───────────────────────────────────────────────────
        new(
            "cortana-disable-cortana",
            "cortana-enable-voice-activation",
            "Voice activation requires the Cortana app and service to be running; disabling Cortana also disables the voice activation wake-word listener."
        ),
        // ── Startup / Logon ───────────────────────────────────────────────────
        new(
            "startup-disable-fast-logon",
            "startup-enable-verbose-logon-messages",
            "These tweaks write conflicting values to the verbosestatus Group Policy key; fast logon suppresses status messages while verbose messages require them to display."
        ),
        // ── Debloat / Game Bar ────────────────────────────────────────────────
        new(
            "debloat-remove-xbox-identity-provider",
            "game-enable-game-bar",
            "Xbox Game Bar requires Xbox Identity Provider (XBIP) for Microsoft account sign-in to save clips and access social features; removing XBIP disables Game Bar's account-linked functionality."
        ),
        // ── Virtualization ────────────────────────────────────────────────────
        new(
            "virt-disable-hypervisor",
            "virt-enable-hyper-v",
            "Disabling the hypervisor removes the bcdedit hypervisorlaunchtype boot entry while enabling Hyper-V sets it to Auto; these two writes directly contradict each other on the same boot configuration value."
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
