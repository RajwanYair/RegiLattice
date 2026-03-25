// RegiLattice.Core — Tweaks/VolumeShadowCopyPolicy.cs
// Sprint 290: Volume Shadow Copy Policy tweaks (10 tweaks)
// Category: "Volume Shadow Copy Policy" | Slug: vscpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VolumeShadowCopy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VolumeShadowCopyPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VolumeShadowCopy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vscpol-disable-vss",
            Label = "Disable Volume Shadow Copy Service",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "The Volume Shadow Copy Service creates point-in-time snapshots of volumes for backup and recovery purposes. Disabling VSS through policy prevents the creation of new shadow copies, which reduces disk space consumption from snapshot storage. This setting is appropriate for systems managed by third-party backup solutions that do not rely on VSS for consistency. Environments using Backup Exec, Veeam, or similar backup products may have their own snapshot mechanisms. Disabling VSS does not remove existing shadow copies but prevents new ones from being created after policy application. Administrators should ensure alternative backup coverage exists before applying this policy to production systems.",
            Tags = ["vss", "shadow-copy", "backup", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVolumeShadowCopy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVolumeShadowCopy")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVolumeShadowCopy", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-zero-max-shadow-copies",
            Label = "Set Maximum Shadow Copies to Zero",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The maximum shadow copies setting limits how many shadow copies can accumulate on a volume before older ones are purged. Setting this to zero removes any policy-defined ceiling, defaulting to the operating system's built-in limit management. This prevents policy conflicts where an explicit maximum would be smaller than what the backup software requires. Backup applications managing their own snapshot lifecycle benefit from having no policy-imposed ceiling. The operating system continues to enforce its own resource-based limits regardless of this policy value. This setting should be coordinated with backup solution requirements to avoid unintended snapshot deletion.",
            Tags = ["vss", "shadow-copy", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxShadowCopies", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxShadowCopies")],
            DetectOps = [RegOp.CheckDword(Key, "MaxShadowCopies", 0)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-notifications",
            Label = "Disable Shadow Copy Notifications",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Volume Shadow Copy generates user-facing notifications about shadow copy creation, deletion, and storage space consumption. These notifications are rarely actionable by end users and create unnecessary interruptions to productivity. Disabling VSS notifications suppresses these system tray and action center alerts. Enterprise users do not need to be aware of shadow copy operations managed by the backup infrastructure. Silencing these notifications reduces cognitive overhead and support requests from confused users. All shadow copy operations continue normally in the background without any impact from suppressing the notifications.",
            Tags = ["vss", "notifications", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyNotifications", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-on-network-shares",
            Label = "Disable Shadow Copy on Network Shares",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Shadow copies of network shares create snapshots of remote file server content visible to users as previous versions in Windows Explorer. Disabling shadow copies on network shares prevents client systems from creating or caching snapshot metadata for mapped drives. File server shadow copies should be managed exclusively at the server level rather than by client machines. Network administrators maintaining file server shadow copies through centralized policies benefit from excluding client-side network share snapshots. Disabling this feature reduces network traffic associated with shadow copy metadata enumeration over SMB connections. Previous versions functionality on network shares is unaffected when shadow copies are managed centrally at the server.",
            Tags = ["vss", "shadow-copy", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyOnNetworkShares", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyOnNetworkShares")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyOnNetworkShares", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-diffarea-growth",
            Label = "Disable Shadow Copy Diff Area Growth",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "The shadow copy diff area stores the changed block data that makes shadow copies valid snapshots in time. Allowing the diff area to grow without bounds can exhaust disk space on busy volumes with high write rates. Disabling unrestricted diff area growth enforces storage constraints that protect the system drive from being filled by shadow copy data. Servers with high-frequency write workloads such as database transaction logs can quickly exhaust diff area space. Administrators should configure explicit diff area size limits appropriate to the volume's change rate rather than allowing unbounded growth. This setting protects system stability at the cost of potentially invalidating shadow copies when storage limits are insufficient.",
            Tags = ["vss", "shadow-copy", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDiffAreaGrowth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDiffAreaGrowth")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDiffAreaGrowth", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-schedule",
            Label = "Disable Shadow Copy Schedule",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows can automatically create shadow copies on a schedule to provide regular recovery points for users. Disabling the shadow copy schedule prevents automatic periodic snapshot creation, reducing uncontrolled storage consumption. Enterprise backup solutions that create VSS snapshots during their own backup windows do not need the Windows scheduler to create additional snapshots. Scheduled shadow copies consume I/O resources during their creation window, which can impact application performance. Centralizing snapshot scheduling within the backup management console gives administrators precise control over snapshot timing and retention. Disabling the built-in schedule while maintaining enterprise backup schedules provides better overall resource management.",
            Tags = ["vss", "shadow-copy", "scheduling", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopySchedule", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopySchedule")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopySchedule", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-telemetry",
            Label = "Disable Shadow Copy Telemetry",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Volume Shadow Copy Service telemetry reports usage statistics and diagnostic information about shadow copy operations to Microsoft. This includes data about snapshot creation frequency, failure rates, and storage utilization. Disabling VSS telemetry prevents this operational data from being transmitted outside the enterprise network boundary. Organizations in regulated industries with data residency requirements benefit from eliminating telemetry streams. VSS functionality and shadow copy quality are not affected by disabling telemetry reporting. Administrators requiring VSS usage metrics can obtain this data through Windows Performance Monitor and event log analysis.",
            Tags = ["vss", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-previous-versions",
            Label = "Disable Previous Versions Feature",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "The Previous Versions feature exposes shadow copy snapshots to users through Windows Explorer's file properties dialog. Disabling Previous Versions removes this user-facing snapshot browsing capability, reducing the risk of unauthorized data recovery by end users. Organizations using enterprise backup solutions for data recovery workflows benefit from directing all restore requests through IT-managed channels. Preventing users from directly restoring files from shadow copies enforces proper change management and audit trail requirements. The underlying shadow copies are not deleted when Previous Versions is disabled; only the user interface for browsing them is suppressed. Administrators can still access shadow copies through administrative tools and backup consoles.",
            Tags = ["vss", "previous-versions", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePreviousVersions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePreviousVersions")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePreviousVersions", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-system-protection",
            Label = "Disable System Protection Shadow Copies",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "System Protection uses VSS to create automatic restore points before system configuration changes such as driver installations and Windows updates. Disabling system protection shadow copies prevents automatic restore point creation, freeing disk space consumed by these snapshots. Enterprise environments using enterprise backup solutions and change management processes have alternative recovery mechanisms. Automatic restore points can accumulate significant storage space on busy systems that receive frequent updates. Organizations with immutable infrastructure approaches that rebuild rather than restore systems benefit from disabling automatic restore points. Prior to disabling this feature, administrators should confirm that adequate recovery mechanisms exist through enterprise backup infrastructure.",
            Tags = ["vss", "system-protection", "restore-points", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSystemProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSystemProtection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSystemProtection", 1)],
        },
        new TweakDef
        {
            Id = "vscpol-disable-on-removable",
            Label = "Disable Shadow Copy on Removable Drives",
            Category = "Volume Shadow Copy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Volume Shadow Copy can be configured to create shadow copies on removable storage media connected to the system. Shadow copies on removable drives consume the limited storage of USB drives and external hard disks. Disabling shadow copies on removable drives prevents VSS from creating snapshots on these transient storage devices. Data on removable drives is typically managed through endpoint DLP policies rather than shadow copy mechanisms. Creating shadow copies on removable media can delay write operations and reduce performance for removable storage workflows. This setting is appropriate for all enterprise environments where removable storage is controlled through USB restriction policies.",
            Tags = ["vss", "removable", "storage", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableShadowCopyOnRemovable", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableShadowCopyOnRemovable")],
            DetectOps = [RegOp.CheckDword(Key, "DisableShadowCopyOnRemovable", 1)],
        },
    ];
}
