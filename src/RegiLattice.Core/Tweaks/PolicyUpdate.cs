namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyUpdate
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _CbsUpdatePolicy.Data,
            .. _UpdateAutoRestartPolicy.Data,
            .. _WindowsPauseUpdatesPolicy.Data,
            .. _WindowsUpdateAdvanced.Data,
            .. _WindowsUpdateDriverPolicy.Data,
            .. _WindowsUpdateNotificationPolicy.Data,
            .. _WindowsUpdatePolicy.Data,
            .. _WindowsUpdateScanPolicy.Data,
            .. _WindowsUpdateUsoPolicy.Data,
            // ── merged from: WindowsUpdate.cs ───────────────────────────────────────
            new TweakDef
            {
                Id = "wu-no-auto-restart",
                Label = "Disable Forced Auto-Restart",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Prevents Windows from automatically restarting while a user is logged in after update installation.",
                Tags = ["update", "restart"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "NoAutoRebootWithLoggedOnUsers", 1),
                ],
            },
            new TweakDef
            {
                Id = "wu-set-active-hours-au",
                Label = "Set Active Hours (8 AM - 11 PM)",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Sets Windows Update active hours to 8 AM - 11 PM to prevent restart during work.",
                Tags = ["update", "active-hours", "restart"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart", 8),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd", 23),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursStart"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "SetActiveHours", 1)],
            },
            new TweakDef
            {
                Id = "wu-block-driver-search",
                Label = "Block Driver Search via Windows Update",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows from searching for driver updates through Windows Update. Different from WU driver exclusion policy. Default: enabled. Recommended: disabled for stability.",
                Tags = ["update", "driver", "search", "block"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 1)],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DriverSearching", "SearchOrderConfig", 0),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-os-upgrade",
                Label = "Disable Windows OS Upgrade via Update",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from offering or installing OS version upgrades. Blocks W10 to W11 upgrades being pushed silently. Default: Enabled. Recommended: Disabled for production stability.",
                Tags = ["update", "upgrade", "os", "block"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableOSUpgrade", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-safeguard-hold",
                Label = "Disable Windows Update Safeguard Holds",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Disables Microsoft's safeguard holds that block updates on incompatible hardware. Use only if you understand the update risks for your system. Default: Enabled. Recommended: Enabled (disable only if blocked).",
                Tags = ["update", "safeguard", "hold", "compatibility"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWUfBSafeguards", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-optional-updates",
                Label = "Disable Auto-Install of Optional Updates",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from automatically installing optional/minor updates. Gives you manual control over optional update installations. Default: Enabled. Recommended: Disabled.",
                Tags = ["update", "optional", "minor", "auto-install"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AutoInstallMinorUpdates", 0),
                ],
            },
            new TweakDef
            {
                Id = "wu-set-active-hours-8-20",
                Label = "Set Windows Update Active Hours (8 AM – 8 PM)",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Sets Windows Update active hours to 8 AM – 8 PM. No restart prompts during this window. Default: auto.",
                Tags = ["update", "active-hours", "restart", "schedule"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd", 20),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursEnd"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WindowsUpdate\UX\Settings", "ActiveHoursStart", 8)],
            },
            new TweakDef
            {
                Id = "wu-disable-seeker-updates",
                Label = "Disable Optional Update Seeker",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description = "Prevents Windows from seeking optional quality updates. Only mandatory updates are installed. Default: seeks all.",
                Tags = ["update", "optional", "seeker", "quality"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetDisableUXWUAccess", 1)],
            },
            new TweakDef
            {
                Id = "wu-disable-update-notifications",
                Label = "Disable Update Notifications",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Suppresses Windows Update restart notifications and nagging prompts. Updates still install but silently. Default: notifications shown.",
                Tags = ["update", "notifications", "nag", "quiet"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetUpdateNotificationLevel", 2),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-update-orchestrator",
                Label = "Disable Update Orchestrator Service",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Disables the Update Orchestrator Service (UsoSvc). Prevents Windows from automatically checking for and installing updates. Default: automatic.",
                Tags = ["update", "orchestrator", "service", "disable"],
                SideEffects = "Windows will not automatically check for security updates.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsoSvc", "Start", 4)],
            },
            new TweakDef
            {
                Id = "wu-disable-wus-medic",
                Label = "Disable Windows Update Medic Service",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Disables the Windows Update Medic Service (WaaSMedicSvc) that repairs Windows Update components. Prevents forced re-enablement. Default: automatic.",
                Tags = ["update", "medic", "service", "disable"],
                SideEffects = "Windows Update cannot self-repair if components become corrupted.",
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 3)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WaaSMedicSvc", "Start", 4)],
            },
            new TweakDef
            {
                Id = "wu-set-schedule-day-saturday",
                Label = "Schedule Updates for Saturday Installation",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Configures Windows Update to install scheduled updates on Saturday at 3:00 AM, minimising disruption during working hours.",
                Tags = ["windows-update", "schedule", "maintenance", "saturday"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime", 3),
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions", 4),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallTime"),
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "AUOptions"),
                ],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU", "ScheduledInstallDay", 7)],
            },
            new TweakDef
            {
                Id = "wu-set-update-service-manual",
                Label = "Set Windows Update Service to Manual Start",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = false,
                Description =
                    "Changes the Windows Update service (wuauserv) to manual start so it only runs when you initiate a check, preventing background update scans from consuming resources.",
                Tags = ["windows-update", "service", "manual", "background"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 2)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\wuauserv", "Start", 3)],
            },
            new TweakDef
            {
                Id = "wu-require-admin-for-updates",
                Label = "Require Admin Approval for Update Installation",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from installing updates without explicit administrator approval. Useful on shared systems to maintain control over when patches are applied.",
                Tags = ["windows-update", "admin", "approval", "control"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "ElevateNonAdmins", 0)],
            },
            new TweakDef
            {
                Id = "wu-disable-metered-update-download",
                Label = "Block Updates on Metered Connections",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Prevents Windows Update from downloading updates when the network connection is marked as metered (mobile hotspot, limited data plans), saving mobile data costs.",
                Tags = ["windows-update", "metered", "mobile", "data", "network"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings", "DownloadMode"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\DeliveryOptimization\Settings",
                        "DownloadMode",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-reboot-required-notification",
                Label = "Disable Post-Update Reboot Notifications",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Suppresses the nagging 'Restart Required' toast notifications that appear after Windows Update installs patches. Reboots can still be performed manually.",
                Tags = ["windows-update", "reboot", "notification", "toast"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "SetAutoRestartNotificationConfig", 1),
                ],
            },
            new TweakDef
            {
                Id = "wu-disable-third-party-preview",
                Label = "Disable Third-Party Windows Update Preview Consent",
                Category = "Maintenance 3",
                NeedsAdmin = true,
                CorpSafe = true,
                Description =
                    "Blocks the consent dialog that prompts users to participate in Windows Update previews from third-party software publishers.",
                Tags = ["windows-update", "preview", "third-party", "consent"],
                RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate"],
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate", "DisableWindowsUpdateAccess", 0),
                ],
            },
        ];

    // ── CbsUpdatePolicy ──
    private static class _CbsUpdatePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CBS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "cbsupd-enable-auto-repair",
                Label = "Enable Automatic Component-Based Servicing Repair",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Component-Based Servicing (CBS) is the Windows component store infrastructure that manages OS component installation, updates, and repairs through the DISM subsystem. Enabling automatic CBS repair ensures that corrupted or missing system components are automatically detected and repaired from the Windows component store without manual intervention. CBS corruption can prevent Windows Update from installing updates and security patches creating security vulnerabilities from missed patching cycles. Automatic repair through CBS uses the component manifest store to verify component integrity and restore damaged components to their correct state. Organizations should enable automatic CBS repair to ensure that system component corruption does not cause persistent patching failures or security gaps. CBS repair events are logged in the CBS.log file which should be reviewed during system health checks to identify recurring repair needs.",
                Tags = ["cbs", "component-repair", "system-integrity", "update", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutomaticRepair", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutomaticRepair")],
                DetectOps = [RegOp.CheckDword(Key, "AutomaticRepair", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-component-hash-verification",
                Label = "Enforce Cryptographic Hash Verification for CBS Components",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component hash verification validates the cryptographic hash of each system component against the component manifest preventing installation of tampered or corrupted components. Enforcing hash verification for CBS operations ensures that only genuine Microsoft-signed components are installed as part of servicing operations. Component hash bypass attacks attempt to install modified system files by manipulating the CBS manifest or hash database to accept attacker-controlled components. CBS hash verification provides a layer of protection against supply chain attacks that attempt to replace legitimate system files with backdoored versions. Organizations should ensure that CBS integrity checking is enabled and that the component store hash database has not been modified through monitoring. CBS hash verification failures generate events in the CBS.log that should be treated as high-severity alerts indicating potential system tampering.",
                Tags = ["cbs", "hash-verification", "integrity", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceHashVerification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceHashVerification")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceHashVerification", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-offline-servicing",
                Label = "Restrict CBS Offline Servicing to Authorized Administrators",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "CBS offline servicing allows modification of Windows component store contents from offline boot environments which is a powerful capability that can be used to bypass OS-level security controls. Restricting CBS offline servicing to authorized administrators prevents unauthorized use of offline tools to modify system components outside the normal OS boot environment. BitLocker full-disk encryption is the primary defense against offline servicing attacks as it prevents booting from external media to access the encrypted drive. Organizations running Secure Boot with TPM-based integrity measurement provide additional protection against offline servicing attacks by detecting changes to the boot environment. CBS offline servicing is a legitimate maintenance capability used for repair scenarios but should be restricted through physical security and encryption rather than software policy alone. Organizations should include CBS offline servicing in their threat model and ensure physical security controls prevent unauthorized access to servers.",
                Tags = ["cbs", "offline-servicing", "admin-restriction", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictOfflineServicing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictOfflineServicing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictOfflineServicing", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-cleanup-scheduled",
                Label = "Enable Scheduled Cleanup of Superseded CBS Components",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "The Windows component store retains superseded component versions after updates to support rollback capability but accumulates significant disk space over time. Enabling scheduled CBS cleanup removes superseded components after a defined retention period freeing disk space while retaining recent versions for rollback. System drives running close to capacity due to component store accumulation can cause update failures when insufficient space exists for patch installation. The DISM cleanup task removes components that can no longer be uninstalled based on the uninstall window policy reducing disk usage by 10-20% on long-running systems. Organizations should balance component store cleanup with rollback requirements as aggressive cleanup prevents rolling back recent updates if problems are discovered. WSFC clusters during CBS cleanup and fail-safe mechanisms prevent critical system failures due to premature component removal.",
                Tags = ["cbs", "cleanup", "disk-space", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScheduledCleanup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScheduledCleanup")],
                DetectOps = [RegOp.CheckDword(Key, "ScheduledCleanup", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enforce-manifest-signing",
                Label = "Enforce Digital Signature on CBS Component Manifests",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS component manifests describe the contents and attributes of system components and are signed by Microsoft to ensure their integrity and prevent modification. Enforcing manifest signature verification ensures that modified manifests that attempt to introduce malicious components or disable security features are rejected. Component manifest signing is part of the Windows Trusted Installer infrastructure that protects system file integrity against unauthorized modification. Manifests that have been tampered with to override hash values or add unauthorized components will be rejected when signature enforcement is active. Manifest signature enforcement is a defense-in-depth measure complementing Windows Resource Protection (WRP) and other component store integrity mechanisms. Organizations should treat CBS manifest signature verification failures as critical security events indicating potential kernel-level or bootkit-level compromise.",
                Tags = ["cbs", "manifest-signing", "code-signing", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceManifestSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceManifestSigning")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceManifestSigning", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-cbs-verbose-logging",
                Label = "Enable Verbose CBS Logging for Update Failure Diagnostics",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "CBS verbose logging captures detailed information about servicing operations including component installations, updates, and failures in the CBS.log file for troubleshooting. Enabling verbose CBS logging provides the detailed diagnostic data needed to identify root causes of Windows Update failures that may indicate security patching gaps. CBS.log is typically several hundred megabytes to gigabytes in size with verbose logging and should be captured and analyzed as part of update compliance monitoring. Update failures identified through CBS verbose logging should be cross-referenced with security vulnerability databases to prioritize remediation of security-relevant failures. Organizations with update compliance requirements should monitor CBS logs for persistent failures that indicate systems are not receiving security patches. Verbose CBS logging helps distinguish between installation failures caused by disk space, compatibility, corruption, or other factors to inform targeted remediation.",
                Tags = ["cbs", "verbose-logging", "diagnostics", "update-compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "VerboseLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "VerboseLogging")],
                DetectOps = [RegOp.CheckDword(Key, "VerboseLogging", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-set-cbs-store-health-check-interval",
                Label = "Set Scheduled Interval for CBS Component Store Health Verification",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "CBS component store health checks verify the integrity of the Windows component store by comparing installed component hashes against the reference values in the component manifest. Setting regular health check intervals ensures that component store corruption is detected promptly before it leads to update failures or security vulnerabilities. Component store corruption can occur due to disk errors, unexpected shutdowns, or malware modification of system files. Regular health verification similar to running DISM /CheckHealth provides ongoing assurance that the system components match their expected values. Health check interval policies complement automatic repair by detecting corruption early before it causes operational problems. Organizations should define health check intervals based on their risk posture with more frequent checks for high-security systems and critical infrastructure.",
                Tags = ["cbs", "health-check", "component-integrity", "maintenance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HealthCheckInterval", 7)],
                RemoveOps = [RegOp.DeleteValue(Key, "HealthCheckInterval")],
                DetectOps = [RegOp.CheckDword(Key, "HealthCheckInterval", 7)],
            },
            new TweakDef
            {
                Id = "cbsupd-block-unsigned-packages",
                Label = "Block Installation of Unsigned or Untrusted CBS Packages",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "CBS package signature verification ensures that only packages signed by trusted certificate authorities including Microsoft and hardware vendors can be installed through the CBS servicing infrastructure. Blocking unsigned CBS packages prevents installation of tampered or third-party packages that could introduce vulnerabilities or backdoors into the system component store. Unsigned packages submitted to CBS represent a significant threat vector for supply chain attacks where unauthorized components are installed as system components. CBS package signature enforcement should apply to both online and offline servicing operations to prevent bypass through offline tools. Organizations running Windows Server should audit the custom packages installed through CBS to identify any unsigned or questionable packages in the component store. CBS signature enforcement is complementary to Windows code signing policies and should be aligned with the organization's overall application trust model.",
                Tags = ["cbs", "unsigned-packages", "code-signing", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPackages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPackages")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPackages", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-restrict-cbs-to-trusted-sources",
                Label = "Restrict CBS Package Sources to Microsoft Update and WSUS Only",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "CBS package source restriction limits where the CBS servicing infrastructure can obtain component packages to Microsoft Update or organizational WSUS servers. Restricting CBS to trusted sources prevents the use of arbitrary package sources that could deliver malicious components masked as system updates. Third-party package sources for CBS are rarely needed in enterprise environments where updates are managed through WSUS or Configuration Manager. Source restriction for CBS complements Windows Update source restrictions to create a consistent update trust chain from Microsoft to the endpoint. Organizations should configure both Windows Update and CBS source policies together to ensure coherent update supply chain protection. Audit CB package installation events to detect any packages sourced from unexpected origins that may indicate a source restriction bypass.",
                Tags = ["cbs", "trusted-sources", "update-chain", "wsus", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToTrustedSources", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToTrustedSources")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToTrustedSources", 1)],
            },
            new TweakDef
            {
                Id = "cbsupd-enable-servicing-stack-updates-priority",
                Label = "Enable Priority Installation of Servicing Stack Updates",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Servicing Stack Updates (SSUs) update the foundational CBS infrastructure itself and must be installed before cumulative updates that depend on the updated servicing stack. Enabling priority installation of SSUs ensures that the servicing stack is always current before applying other updates preventing installation failures from an outdated stack. Outdated servicing stacks are a common cause of Windows Update failure where cumulative updates cannot be installed because they require SSU capabilities not yet present. SSU prioritization is implemented in Windows 10 1903 and later through the Unified Update Platform that automatically handles SSU installation order. Organizations running older Windows versions should prioritize SSU installation in their WSUS or Configuration Manager patch deployment groups. Servicing stack currency is a prerequisite for comprehensive security patching and should be verified during update compliance audits.",
                Tags = ["cbs", "servicing-stack", "update-priority", "patching", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PrioritizeServicingStackUpdates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PrioritizeServicingStackUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "PrioritizeServicingStackUpdates", 1)],
            },
        ];
    }

    // ── UpdateAutoRestartPolicy ──
    private static class _UpdateAutoRestartPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-deadline-7days",
                    Label = "WU Auto-Restart: Set Engaged Restart Deadline to 7 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EngagedRestartDeadline=7 in WU policy. After a quality update is downloaded, Windows enters 'engaged restart' mode where users are repeatedly notified. "
                        + "This value sets the absolute deadline after which Windows will force a restart regardless of user activity. "
                        + "7 days is a balance that gives users a full work week to schedule the restart while ensuring machines don't stay un-patched indefinitely.",
                    Tags = ["windows-update", "restart", "deadline", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Forces restart after 7 days; ensures machines are patched while giving users a workweek to choose their own restart time.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartDeadline", 7)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartDeadline")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartDeadline", 7)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-snooze-3days",
                    Label = "WU Auto-Restart: Set Engaged Restart Snooze Interval to 3 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EngagedRestartSnoozeSchedule=3 in WU policy. Controls how frequently Windows re-displays the engaged restart notification after a user dismisses it. "
                        + "Value of 3 means the reminder returns every 3 days, ensuring users don't forget a pending restart while avoiding daily interruptions that lead to notification fatigue and dismissal without action.",
                    Tags = ["windows-update", "restart", "snooze", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "3-day snooze interval for restart reminders; balances user awareness with notification fatigue.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartSnoozeSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartSnoozeSchedule", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-engaged-restart-transition-2days",
                    Label = "WU Auto-Restart: Set Engaged Restart Transition Schedule to 2 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EngagedRestartTransitionSchedule=2 in WU policy. Controls how many days after an update becomes ready-to-install that Windows transitions from passive notifications to the more prominent 'engaged restart' mode. "
                        + "Setting this to 2 days means the first two days show soft notifications, after which the full engaged restart UI (with deadline counter) takes over.",
                    Tags = ["windows-update", "restart", "transition", "policy", "engaged"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transitions to engaged restart mode after 2 days; earlier transition increases restart compliance rate.",
                    ApplyOps = [RegOp.SetDword(Key, "EngagedRestartTransitionSchedule", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EngagedRestartTransitionSchedule")],
                    DetectOps = [RegOp.CheckDword(Key, "EngagedRestartTransitionSchedule", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-quality-update-deadline-3days",
                    Label = "WU Auto-Restart: Set Quality Update Install Deadline to 3 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ConfigureDeadlineForQualityUpdates=3 in WU policy. Establishes a hard deadline of 3 days from when a quality (security + non-security) update is offered before Windows must restart to install it. "
                        + "For security teams managing patch compliance under CIS or NIST 800-53 patch SLAs, a 3-day restart deadline for quality updates ensures critical CVE patches are active within the compliance window.",
                    Tags = ["windows-update", "deadline", "quality", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "3-day hard restart deadline for quality updates; supports NIST 800-53 and CIS patch compliance SLAs.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForQualityUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForQualityUpdates", 3)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-feature-update-deadline-14days",
                    Label = "WU Auto-Restart: Set Feature Update Install Deadline to 14 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ConfigureDeadlineForFeatureUpdates=14 in WU policy. Establishes a 14-day hard deadline from when a feature update is offered before Windows must restart to complete installation. "
                        + "Feature updates are far more disruptive than quality updates (longer restart time, possible app compatibility breaks), so a longer 14-day window gives users and IT departments time to validate and prepare.",
                    Tags = ["windows-update", "deadline", "feature", "upgrade", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "14-day deadline for feature updates; longer window accommodates compatibility validation before forced restart.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineForFeatureUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineForFeatureUpdates", 14)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-deadline-grace-period-2days",
                    Label = "WU Auto-Restart: Set Post-Deadline Grace Period to 2 Days",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ConfigureDeadlineGracePeriod=2 in WU policy. After the restart deadline passes, this grace period gives users an additional 2 days before the machine will restart outside of active hours. "
                        + "The grace period prevents the deadline enforcement from causing a disruptive forced restart mid-workday as soon as the deadline hits. The machine will restart during the next scheduled non-active hours window within the grace period.",
                    Tags = ["windows-update", "deadline", "grace", "restart", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "2-day grace period post-deadline; restart deferred to next active-hours window reducing in-day disruption.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineGracePeriod")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineGracePeriod", 2)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-disable-no-auto-reboot-after-deadline",
                    Label = "WU Auto-Restart: Allow Auto-Reboot After Deadline Expires",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ConfigureDeadlineNoAutoReboot=0 in WU policy. Ensures that once the deadline and grace period pass, Windows WILL automatically restart to apply the update. "
                        + "Value=0 means no moratorium on auto-reboot after the deadline. This overrides any 'NoAutoRebootWithLoggedOnUsers' policy for machines that have exceeded their deadline, ensuring patching is never blocked indefinitely by a persistent logged-on session.",
                    Tags = ["windows-update", "restart", "deadline", "enforcement", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Post-deadline auto-reboot enabled; overrides logged-on user protection once deadline expires for compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConfigureDeadlineNoAutoReboot")],
                    DetectOps = [RegOp.CheckDword(Key, "ConfigureDeadlineNoAutoReboot", 0)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-set-restart-warning-4hours",
                    Label = "WU Auto-Restart: Set Pre-Restart Warning to 4 Hours",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ScheduleRestartWarning=4 in WU policy. When Windows schedules an automatic restart, this setting controls how many hours in advance users receive a prominent restart warning notification. "
                        + "A 4-hour advance warning gives users time to save work, close applications, and plan the restart, significantly reducing data loss from unexpected restarts.",
                    Tags = ["windows-update", "restart", "warning", "notification", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "4-hour advance restart warning; gives users time to save work and plan restart timing.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleRestartWarning", 4)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleRestartWarning", 4)],
                },
                new TweakDef
                {
                    Id = "wuarstrt-enable-auto-restart-required-notification",
                    Label = "WU Auto-Restart: Enable Mandatory Restart Required Notification",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets SetAutoRestartRequiredNotificationDismissal=1 in WU policy. Configures Windows to show a non-dismissable restart required notification when a patch deadline is imminent. "
                        + "Without this, users can indefinitely dismiss restart prompts. With value=1, close-to-deadline notifications must be acknowledged with a concrete restart time selection rather than a simple dismiss.",
                    Tags = ["windows-update", "restart", "notification", "mandatory", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Non-dismissable restart notification near deadline; forces users to choose restart time, increasing compliance.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartRequiredNotificationDismissal")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartRequiredNotificationDismissal", 1)],
                },
            ];
    }

    // ── WindowsPauseUpdatesPolicy ──
    private static class _WindowsPauseUpdatesPolicy
    {
        private const string PauseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pauseupd-disable-auto-install-on-shutdown",
                Label = "Windows Update Pause: Disable Auto-Install Updates on Shutdown",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Prevents Windows Update from automatically installing updates when the user initiates a shutdown. "
                    + "Auto-install-on-shutdown can extend shutdown times and cause unexpected restarts, especially on laptops before meetings. "
                    + "Updates are controlled through scheduled windows instead, giving IT full control over the timing. "
                    + "Removing this policy re-enables automatic installation during shutdown sequences.",
                Tags = ["windows-update", "shutdown", "auto-install", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "NoAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoUpdate")],
                DetectOps = [RegOp.CheckDword(AuKey, "NoAutoUpdate", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents updates installing on shutdown; avoids unexpected extended shutdown times.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-start",
                Label = "Windows Update Pause: Set Active Hours Start (8 AM)",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Configures the Windows Update active hours start time to 8 AM, preventing reboots for updates during business hours. "
                    + "Active hours protect users from unexpected reboots during the configured working hours window. "
                    + "Setting an explicit start ensures policy is enforced rather than relying on user configuration. "
                    + "Removing this policy reverts to Windows default or user-configured active hours.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursStart", 8)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursStart")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursStart", 8)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours start to 8 AM; prevents update reboots interrupting morning workflows.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-active-hours-end",
                Label = "Windows Update Pause: Set Active Hours End (6 PM)",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Configures the Windows Update active hours end time to 6 PM (18:00), ensuring reboots cannot occur during standard business hours. "
                    + "With start fixed at 8 AM and end at 6 PM, the full working day is protected from forced reboots. "
                    + "Updates can install after 6 PM via the scheduled maintenance window. "
                    + "Removing this policy reverts to Windows default or user-configured active hours end.",
                Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursEnd", 18)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursEnd")],
                DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursEnd", 18)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Locks active hours end to 6 PM; complete 8 AM–6 PM protection from forced reboots.",
            },
            new TweakDef
            {
                Id = "pauseupd-disable-upgrade-notifications",
                Label = "Windows Update Pause: Disable Upgrade Notification Toasts",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Suppresses the Windows Update toast notifications that prompt users to restart for pending updates. "
                    + "In a managed environment, restart timing is controlled by IT policy — user-visible prompts are redundant and disruptive. "
                    + "Suppressing notifications prevents users from inadvertently triggering reboots outside the maintenance window. "
                    + "Removing this policy re-enables Windows Update restart notification toasts.",
                Tags = ["windows-update", "notifications", "restart", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "SetDisableUXWUAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "SetDisableUXWUAccess")],
                DetectOps = [RegOp.CheckDword(AuKey, "SetDisableUXWUAccess", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WU restart prompts from users; IT maintains full control of update timing.",
            },
            new TweakDef
            {
                Id = "pauseupd-set-update-detection-frequency",
                Label = "Windows Update Pause: Set Update Detection Frequency (22 Hours)",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Sets the Windows Update service to check for updates every 22 hours instead of the default automatic random interval. "
                    + "A predictable 22-hour check interval prevents multiple machines on the same network from surging the update server simultaneously. "
                    + "Combined with an WSUS/SCCM deployment, this ensures consistent, manageable update bandwidth. "
                    + "Removing this policy reverts to Windows' random detection frequency.",
                Tags = ["windows-update", "detection", "frequency", "bandwidth", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
                DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequencyEnabled", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Predictable 22-hour WU check interval; prevents bandwidth surge on shared networks.",
            },
            new TweakDef
            {
                Id = "pauseupd-allow-mu-updates",
                Label = "Windows Update Pause: Allow Microsoft Update for Other Products",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Configures Windows Update to also deliver updates for other Microsoft products (Office, .NET, Visual C++) alongside OS patches. "
                    + "Receiving all Microsoft product updates through a single channel simplifies patch management and reduces the attack surface. "
                    + "This is equivalent to enabling 'Give me updates for other Microsoft products' in Windows Update settings. "
                    + "Removing this policy reverts to OS-only updates via Windows Update.",
                Tags = ["windows-update", "microsoft-update", "office", "patch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [AuKey],
                ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
                RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
                DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enables Microsoft Update for all products; consolidates patching into a single channel.",
            },
            new TweakDef
            {
                Id = "pauseupd-enforce-restart-deadline",
                Label = "Windows Update Pause: Enforce 72-Hour Restart Deadline",
                Category = "Maintenance — Cbs Update",
                Description =
                    "Sets a 72-hour mandatory restart deadline after Windows Update installs updates requiring a reboot. "
                    + "Without a deadline, users can indefinitely postpone required restarts, leaving the system vulnerable to active exploits. "
                    + "72 hours provides reasonable flexibility for users to save work while ensuring security patches are applied promptly. "
                    + "Removing this policy removes the forced restart deadline.",
                Tags = ["windows-update", "restart", "deadline", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PauseKey],
                ApplyOps = [RegOp.SetDword(PauseKey, "SetAutoRestartDeadline", 72)],
                RemoveOps = [RegOp.DeleteValue(PauseKey, "SetAutoRestartDeadline")],
                DetectOps = [RegOp.CheckDword(PauseKey, "SetAutoRestartDeadline", 72)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Forces restart within 72 hours of patch install; prevents indefinite deferral of security updates.",
            },
        ];
    }

    // ── WindowsUpdateAdvanced ──
    private static class _WindowsUpdateAdvanced
    {
        private const string WuPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        private const string WuAu = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        private const string DeliveryOpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeliveryOptimization";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wuadv-require-update-signature",
                Label = "Require Code-Signed Updates from WSUS",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "wsus", "signing", "security"],
                Description =
                    "Requires that all updates from a WSUS server are signed by a trusted publisher "
                    + "in the local machine certificate store. UsePolicyBasedQosMarkings=1 is the "
                    + "underlying policy; AcceptTrustedPublisherCerts=1 enables the WSUS signing check.",
                ApplyOps = [RegOp.SetDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
                RemoveOps = [RegOp.DeleteValue(WuPolicy, "AcceptTrustedPublisherCerts")],
                DetectOps = [RegOp.CheckDword(WuPolicy, "AcceptTrustedPublisherCerts", 1)],
            },
            new TweakDef
            {
                Id = "wuadv-allow-mu-updates-with-wu",
                Label = "Enable Microsoft Update (Office + Products) via Windows Update",
                Category = "Maintenance — Cbs Update",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["windows update", "microsoft update", "office", "products"],
                Description =
                    "Enables Microsoft Update service via the Windows Update policy — allows Office, "
                    + "Visual Studio, and other Microsoft products to receive updates through "
                    + "Windows Update instead of requiring separate update channels. "
                    + "EnableFeaturedSoftware=1.",
                ApplyOps = [RegOp.SetDword(WuAu, "EnableFeaturedSoftware", 1)],
                RemoveOps = [RegOp.DeleteValue(WuAu, "EnableFeaturedSoftware")],
                DetectOps = [RegOp.CheckDword(WuAu, "EnableFeaturedSoftware", 1)],
            },
        ];
    }

    // ── WindowsUpdateDriverPolicy ──
    private static class _WindowsUpdateDriverPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";
        private const string SignKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Driver Signing";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wudrv-deny-unidentified-device-installation",
                    Label = "WU Driver: Block Installation of Unidentified Device Drivers",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets DenyUnidentifiedDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from installing drivers for hardware devices that are not in the Windows Driver Store and do not have a matching entry in Windows Update. "
                        + "Unidentified devices are a common attack vector — malicious USB devices can present as unknown hardware that auto-installs a malicious driver. This policy requires all devices to have a recognized driver before they can function.",
                    Tags = ["driver", "device", "security", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks unidentified device driver installs; prevents USB hardware-based driver injection attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyUnidentifiedDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyUnidentifiedDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-deny-removable-device-driver-install",
                    Label = "WU Driver: Block Automatic Driver Installation for Removable Devices",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets DenyRemovableDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from automatically installing drivers for any removable device. "
                        + "Removable devices (USB storage, USB hubs, card readers, portable audio devices) are frequently connected in enterprise environments. Without this policy, each new removable device triggers an automatic driver installation from WU, bypassing IT-managed driver sets and potentially installing unsigned or vulnerable drivers.",
                    Tags = ["driver", "removable", "usb", "device", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks auto-install of removable device drivers via WU; requires IT-managed driver pre-staging for new devices.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyRemovableDeviceInstallation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyRemovableDeviceInstallation")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyRemovableDeviceInstallation", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enforce-driver-signing-block-unsigned",
                    Label = "WU Driver: Block Installation of Unsigned Device Drivers",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets BehaviorOnFailedVerify=2 in Driver Signing policy. Configures Windows to silently block the installation of any device driver that fails digital signature verification. "
                        + "Value 2 = Block (value 1 = Warn, value 0 = Ignore). Blocking unsigned drivers prevents rootkits and malicious kernel-mode code from loading under the guise of a hardware driver. This is a critical defence-in-depth control alongside Secure Boot and HVCI.",
                    Tags = ["driver", "signing", "security", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Silently blocks unsigned drivers; prevents rootkits and kernel-level malware from installing via driver packages.",
                    ApplyOps = [RegOp.SetDword(SignKey, "BehaviorOnFailedVerify", 2)],
                    RemoveOps = [RegOp.DeleteValue(SignKey, "BehaviorOnFailedVerify")],
                    DetectOps = [RegOp.CheckDword(SignKey, "BehaviorOnFailedVerify", 2)],
                },
                new TweakDef
                {
                    Id = "wudrv-log-driver-install-restriction-events",
                    Label = "WU Driver: Enable Event Logging for Blocked Driver Installations",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets WritePolicy=1 in DeviceInstall\\Restrictions policy. Enables Windows to write an event log entry whenever a device installation is blocked by Device Installation Policy. "
                        + "Without this, blocked installations fail silently, making it impossible to audit what hardware was attempted and blocked. With logging enabled, security teams can monitor for repeated installation attempts which may indicate hardware-based persistence attempts.",
                    Tags = ["driver", "logging", "audit", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Logs blocked driver installations to event log; enables audit trail for hardware-based attack detection.",
                    ApplyOps = [RegOp.SetDword(Key, "WritePolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "WritePolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "WritePolicy", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-disable-windows-error-reporting-driver",
                    Label = "WU Driver: Disable Driver Crash Data Upload to Microsoft",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets DisableDriverLookup=1 in DeviceInstall\\Restrictions policy. Prevents Windows from looking up driver information and uploading crash data to the Microsoft Windows Error Reporting service when a device driver causes an error. "
                        + "In regulated environments, data sovereignty requirements may prohibit telemetry of driver crash details (device type, hardware ID, crash context) from being transmitted to Microsoft's cloud infrastructure.",
                    Tags = ["driver", "telemetry", "privacy", "wer", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks driver crash data upload to Microsoft; supports data sovereignty requirements for regulated industries.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDriverLookup", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverLookup")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDriverLookup", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-prevent-non-admin-driver-install",
                    Label = "WU Driver: Restrict Driver Installation to Administrators Only",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets PreventInstallationOfDevicesNotDescribedByOtherPolicySettings=1 in DeviceInstall\\Restrictions policy. Sets a default-deny posture for device installation: only devices explicitly permitted by an allowlist policy are installed. All others are blocked. "
                        + "This inverts the default Windows behaviour (allow-by-default) into a deny-by-default stance that requires active IT involvement to introduce any new device type into the environment.",
                    Tags = ["driver", "device", "allowlist", "default-deny", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Default-deny for new device types; requires IT-managed allowlist for any new hardware class to function.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicySettings", 1)],
                },
                new TweakDef
                {
                    Id = "wudrv-enable-device-metadata-retrieval-block",
                    Label = "WU Driver: Block Device Metadata Retrieval from Windows Update",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets PreventDeviceMetadataFromNetwork=1 in DeviceInstall policy. Prevents Windows from searching the Windows Update network service for device metadata (device icons, model pages, UWP companion apps). "
                        + "Device metadata retrieval can prompt automatic download of companion apps without explicit user action. In locked-down environments, all device metadata should be pre-staged via WSUS rather than retrieved on-demand from Microsoft servers.",
                    Tags = ["driver", "metadata", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks network-sourced device metadata; prevents unsolicited companion app downloads on device connection.",
                    ApplyOps =
                    [
                        RegOp.SetDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork"
                        ),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings",
                            "PreventDeviceMetadataFromNetwork",
                            1
                        ),
                    ],
                },
            ];
    }

    // ── WindowsUpdateNotificationPolicy ──
    private static class _WindowsUpdateNotificationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wunotif-set-update-notification-level-standard",
                    Label = "WU Notification: Set Update Notification Level to Standard",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets UpdateNotificationLevel=1 in WU policy. Configures the Windows Update notification level presented to users. "
                        + "Level 1 = Standard Notifications (users see action centre notifications and system tray alerts for pending updates). Level 2 = Disable all restart notifications. "
                        + "Setting level 1 ensures users are informed without overly aggressive interruptions, and is the baseline for notification management before other more specific controls are applied.",
                    Tags = ["windows-update", "notification", "level", "action-centre", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Sets base notification level; ensures users are informed of pending updates without restart interruptions.",
                    ApplyOps = [RegOp.SetDword(Key, "UpdateNotificationLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UpdateNotificationLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "UpdateNotificationLevel", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-suppress-restart-notification-when-busy",
                    Label = "WU Notification: Suppress Auto-Restart Notifications During Active Use",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets SuppressRestartNotification=1 in WU policy. Instructs Windows to suppress automatic restart notifications while the user is actively using the computer (mouse/keyboard activity detected). "
                        + "This prevents the restart prompt from appearing mid-presentation or mid-call, reducing user frustration while still allowing notifications when the device is idle.",
                    Tags = ["windows-update", "notification", "restart", "suppress", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses restart notifications during device activity; notifications appear only when user is idle.",
                    ApplyOps = [RegOp.SetDword(Key, "SuppressRestartNotification", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SuppressRestartNotification")],
                    DetectOps = [RegOp.CheckDword(Key, "SuppressRestartNotification", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-update-availability-popup",
                    Label = "WU Notification: Disable Update Availability Pop-Up Toast",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets SetAutoRestartNotificationExclusion=1 in WU policy. Disables the 'restart to update' toast notification pop-up that appears in the bottom-right corner of the screen. "
                        + "In enterprise SCCM/Intune-managed environments, the deployment tool provides its own notification and deadline management. The built-in WU toast in these environments creates duplicate, confusing messages that contradict the managed deployment window.",
                    Tags = ["windows-update", "notification", "toast", "popup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WU toast pop-ups; eliminates duplicate notifications in SCCM/Intune managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetAutoRestartNotificationExclusion")],
                    DetectOps = [RegOp.CheckDword(Key, "SetAutoRestartNotificationExclusion", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-set-reboot-warning-timeout-15min",
                    Label = "WU Notification: Set Reboot Warning Timeout to 15 Minutes",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets ScheduleImminentRestartWarning=15 in WU policy. Sets the duration of the imminent-restart countdown dialog to 15 minutes. "
                        + "When Windows determines a restart is imminent (e.g., deadline approaching), this countdown gives users exactly 15 minutes to save their work before the restart proceeds. This is shorter than the ScheduleRestartWarning (advance warning hours) and is the 'last chance' save reminder.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "15-minute last-chance countdown before restart; reduces data loss from unwarned forced restarts.",
                    ApplyOps = [RegOp.SetDword(Key, "ScheduleImminentRestartWarning", 15)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScheduleImminentRestartWarning")],
                    DetectOps = [RegOp.CheckDword(Key, "ScheduleImminentRestartWarning", 15)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-windows-update-log-events",
                    Label = "WU Notification: Enable Verbose Windows Update Event Logging",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EnableDetailedLogging=1 in WU policy. Enables detailed verbose logging of Windows Update events to the Windows Event Log under the WindowsUpdateClient/Operational channel. "
                        + "By default, Windows Update logs minimal information. Detailed logs capture download start/stop, error codes, and deployment decisions, enabling IT to troubleshoot why updates fail, succeed late, or trigger unexpected restarts on specific machines.",
                    Tags = ["windows-update", "logging", "audit", "diagnostics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables verbose WU logging to event log; critical for diagnosing update failures and compliance audit trails.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableDetailedLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableDetailedLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-enable-update-health-tools-reporting",
                    Label = "WU Notification: Enable Update Health Tools Status Reporting",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EnableUpdateHealthTools=1 in WU policy. Activates the Update Compliance Health Tools which report patch status, restart compliance, and update health metrics to Azure Monitor, Microsoft Endpoint Manager, or custom OMS workspaces. "
                        + "Without health tools enabled, IT dashboards show no patch status for affected machines, making it impossible to identify non-compliant devices in the estate.",
                    Tags = ["windows-update", "health", "reporting", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables patch status reporting to endpoint management platforms; provides patch compliance visibility.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableUpdateHealthTools", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableUpdateHealthTools")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableUpdateHealthTools", 1)],
                },
                new TweakDef
                {
                    Id = "wunotif-disable-outdated-browser-notifications",
                    Label = "WU Notification: Disable Outdated Browser/App Update Notifications from WU",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets AllowNonMicrosoftSignedUpdate=0 in WU policy. Prevents Windows Update from delivering and notifying about updates from non-Microsoft third-party publishers via the Microsoft Update service. "
                        + "Third-party update notifications through Windows Update are not needed when dedicated application management tools (SCCM, Intune, Chocolatey) are already used for non-OS software, reducing noise and preventing IT-unmanaged software updates.",
                    Tags = ["windows-update", "notification", "third-party", "apps", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Blocks third-party software update notifications via WU; channel reserved for OS updates only in managed environments.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowNonMicrosoftSignedUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowNonMicrosoftSignedUpdate", 0)],
                },
            ];
    }

    // ── WindowsUpdatePolicy ──
    private static class _WindowsUpdatePolicy
    {
        private const string WuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wupol-block-internet-wu-locations",
                    Label = "Block Direct Connection to Windows Update Internet Locations",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Forces all update traffic through an internal catalog; prevents the client from contacting Microsoft update servers directly.",
                    Tags = ["windows-update", "internet", "policy", "wsus"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Devices only receive updates through the configured internal source; requires WUServer to be set.",
                    ApplyOps = [RegOp.SetDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                    RemoveOps = [RegOp.DeleteValue(WuKey, "DoNotConnectToWindowsUpdateInternetLocations")],
                    DetectOps = [RegOp.CheckDword(WuKey, "DoNotConnectToWindowsUpdateInternetLocations", 1)],
                },
            ];
    }

    // ── WindowsUpdateScanPolicy ──
    private static class _WindowsUpdateScanPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
        private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuscan-enable-wsus-server-mode",
                    Label = "WU Scan: Route Update Scanning Through WSUS Server",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets UseWUServer=1 in WU AU policy. Configures the Windows Update client to scan against the WSUS server configured in WUServer, rather than the public Windows Update service. "
                        + "This is the primary switch that activates WSUS-based update management. Without this flag set to 1, WUServer and WUStatusServer URL values are present in the registry but ignored by the WU client, which continues to scan against Microsoft's cloud endpoint.",
                    Tags = ["windows-update", "wsus", "server", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Activates WSUS-sourced scanning; all updates sourced from and approved via internal WSUS server.",
                    ApplyOps = [RegOp.SetDword(AuKey, "UseWUServer", 1)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "UseWUServer")],
                    DetectOps = [RegOp.CheckDword(AuKey, "UseWUServer", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-enable-intranet-update-service-stats",
                    Label = "WU Scan: Enable Intranet Update Statistics Reporting",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets UseWUServer=1 and IntranetServerInternetOptions=3 in WU AU policy. Configures the WU client to send update scan statistics (detection results, download progress, installation outcomes) to the WSUS status server rather than Microsoft. "
                        + "This populates the WSUS server's reporting database, enabling IT administrators to view an accurate picture of update compliance across the enterprise from the WSUS console.",
                    Tags = ["windows-update", "wsus", "reporting", "statistics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes update scan stats to WSUS; populates compliance reports in WSUS console.",
                    ApplyOps = [RegOp.SetDword(AuKey, "IntranetServerInternetOptions", 3)],
                    RemoveOps = [RegOp.DeleteValue(AuKey, "IntranetServerInternetOptions")],
                    DetectOps = [RegOp.CheckDword(AuKey, "IntranetServerInternetOptions", 3)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-launch-timeout-5min",
                    Label = "WU Scan: Set Post-Install Reboot Launch Timeout to 5 Minutes",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets RebootLaunchTimeout=5 and RebootLaunchTimeoutEnabled=1 in WU policy. After updates are installed during a scheduled maintenance window and a restart is required, Windows waits this many minutes before initiating the restart automatically. "
                        + "5 minutes gives any background processes time to complete gracefully while keeping the restart within the maintenance window. Without a timeout, the restart may be postponed indefinitely if a user was actively logged in during the overnight window.",
                    Tags = ["windows-update", "restart", "timeout", "maintenance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "5-minute post-install restart timeout; keeps restart within maintenance window while allowing graceful process shutdown.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootLaunchTimeout", 5), RegOp.SetDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootLaunchTimeout"), RegOp.DeleteValue(Key, "RebootLaunchTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootLaunchTimeoutEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wuscan-set-reboot-warning-timeout-30min",
                    Label = "WU Scan: Set Pre-Restart Warning Timeout to 30 Minutes",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets RebootWarningTimeout=30 and RebootWarningTimeoutEnabled=1 in WU policy. Configures Windows to display a countdown restart warning 30 minutes before the scheduled restart. "
                        + "30 minutes provides a comfortable window for users to save work and close applications before the restart. This setting complements ScheduleRestartWarning (hours-in-advance general notice) — the 30-minute warning is the final specific countdown before imminent restart.",
                    Tags = ["windows-update", "restart", "warning", "countdown", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "30-minute final restart countdown; gives users time to save before scheduled maintenance restart.",
                    ApplyOps = [RegOp.SetDword(Key, "RebootWarningTimeout", 30), RegOp.SetDword(Key, "RebootWarningTimeoutEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RebootWarningTimeout"), RegOp.DeleteValue(Key, "RebootWarningTimeoutEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "RebootWarningTimeoutEnabled", 1)],
                },
            ];
    }

    // ── WindowsUpdateUsoPolicy ──
    private static class _WindowsUpdateUsoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wuuso-block-wu-downloads-metered-network",
                    Label = "WU USO: Block Windows Update Downloads on Metered Networks",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets AllowAutoWindowsUpdateDownloadOverMeteredNetwork=0 in WU policy. Prevents Windows Update from automatically downloading update packages when the active network connection is marked as metered. "
                        + "On mobile devices and machines on cellular or satellite connections, unrestricted WU downloads can exhaust data allowances or incur substantial overage charges. This policy applies to both background and foreground download scenarios.",
                    Tags = ["windows-update", "metered", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Blocks WU auto-downloads on metered connections; prevents data-plan exhaustion on mobile/satellite links.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAutoWindowsUpdateDownloadOverMeteredNetwork", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-temporary-enterprise-feature-drops",
                    Label = "WU USO: Block In-Period Temporary Enterprise Feature Drops",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets AllowTemporaryEnterpriseFeatureControl=0 in WU policy. Disables the delivery of optional 'temporary enterprise feature' updates — incremental functionality enhancements that Microsoft ships between major version releases. "
                        + "These in-period feature drops are not security updates and can change application behaviour mid-support-lifecycle. Blocking them keeps the OS in a stable, enterprise-validated state between planned upgrade windows.",
                    Tags = ["windows-update", "features", "enterprise", "stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks temporary enterprise feature drops; keeps OS behaviour predictable between scheduled upgrade events.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowTemporaryEnterpriseFeatureControl")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowTemporaryEnterpriseFeatureControl", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-prevent-user-pausing-updates",
                    Label = "WU USO: Prevent Users from Pausing Windows Updates",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets SetDisablePauseUXAccess=1 in WU policy (AU subkey). Removes the 'Pause Updates' option from the Windows Update settings UI. "
                        + "Without this policy, standard users can pause updates for up to 5 weeks, leaving machines unpatched and out of compliance. This is a key control in corporate environments operating under patch management SLAs where user-initiated update deferrals are not permitted.",
                    Tags = ["windows-update", "pause", "user", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removes pause updates control from user UI; ensures patch compliance SLAs are not bypassed by users.",
                    ApplyOps = [RegOp.SetDword(Key, "SetDisablePauseUXAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetDisablePauseUXAccess")],
                    DetectOps = [RegOp.CheckDword(Key, "SetDisablePauseUXAccess", 1)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-recommended-updates-auto-install",
                    Label = "WU USO: Block Automatic Installation of Recommended Updates",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets IncludeRecommendedUpdates=0 in WU policy. Prevents Windows Update from automatically installing 'recommended' updates which include non-security improvements, application updates, and optional Windows features. "
                        + "In enterprise environments, recommended updates should be reviewed and approved through a patch management process rather than automatically deployed, as they can change application behaviour without a security justification.",
                    Tags = ["windows-update", "recommended", "auto-install", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks auto-install of recommended updates; only critical and security updates deploy automatically.",
                    ApplyOps = [RegOp.SetDword(Key, "IncludeRecommendedUpdates", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IncludeRecommendedUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "IncludeRecommendedUpdates", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-featured-software-via-wu",
                    Label = "WU USO: Block Automatic Installation of Featured Software",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets EnableFeaturedSoftware=0 in WU policy. Stops Windows Update from offering and automatically installing 'featured software' — typically free Microsoft utilities, game trials, and promotional apps. "
                        + "Without this setting, WU silently installs marketing-tied software packages that were never requested by the user or IT administrator, increasing the installed application footprint and creating an unexpected change management event.",
                    Tags = ["windows-update", "featured", "software", "bloat", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks OEM/Microsoft featured software installs via WU; prevents unsolicited app additions on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableFeaturedSoftware", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableFeaturedSoftware")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableFeaturedSoftware", 0)],
                },
                new TweakDef
                {
                    Id = "wuuso-block-policy-driven-other-update-source",
                    Label = "WU USO: Force Policy-Driven Update Source for Other Updates",
                    Category = "Maintenance — Cbs Update",
                    Description =
                        "Sets SetPolicyDrivenUpdateSourceForOtherUpdates=1 in WU policy. Ensures that non-feature, non-quality updates (such as drivers from the 'Other' category in WU) are sourced exclusively through the configured policy-driven update source (WSUS/SCCM). "
                        + "Without this setting, updates in the 'Other' category may still be retrieved directly from Microsoft Update regardless of the WSUS or DeliveryOptimization configuration.",
                    Tags = ["windows-update", "wsus", "policy-driven", "other-updates", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Routes 'Other' category updates through policy-driven source; closes WSUS bypass for non-standard update types.",
                    ApplyOps = [RegOp.SetDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates")],
                    DetectOps = [RegOp.CheckDword(Key, "SetPolicyDrivenUpdateSourceForOtherUpdates", 1)],
                },
            ];
    }
}
