// RegiLattice.Core — Tweaks/SharedPCPolicy.cs
// Sprint 292: Shared PC Policy tweaks (10 tweaks)
// Category: "Shared PC Policy" | Slug: shpc
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SharedPC

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SharedPCPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SharedPC";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shpc-disable-shared-pc-mode",
            Label = "Disable Shared PC Mode",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Shared PC mode optimizes Windows for use by multiple users on a single device, enabling account deletion and profile cleanup between sessions. Disabling Shared PC mode ensures a standard single-user workstation configuration is enforced on dedicated corporate devices. Shared PC mode behaviors including account deletion and profile compression are inappropriate for dedicated workstations where user data persistence is required. This policy is relevant for environments that may inadvertently inherit shared PC settings from imported operating system images. Dedicated workstations should operate in standard single-user mode to preserve user profile data and application settings between sessions. All standard user profile and session management behaviors remain active when Shared PC mode is disabled.",
            Tags = ["shared-pc", "kiosk", "profiles", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSharedPCMode", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSharedPCMode")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSharedPCMode", 0)],
        },
        new TweakDef
        {
            Id = "shpc-zero-disk-deletion-level",
            Label = "Disable Disk Level Deletion",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Shared PC disk level deletion determines how aggressively user profiles and cached data are removed when disk space falls below configured thresholds. Setting disk level deletion to zero disables automatic account and profile deletion based on disk pressure. Dedicated workstations with persistent user profiles require that data not be deleted without explicit administrative action. Automatic deletion of user profiles in shared PC mode can cause data loss if users inadvertently leave unsynchronized files on the local device. Enterprises managing user data retention policies should handle profile lifecycle through MDM or Group Policy rather than automatic deletion thresholds. This setting preserves user data integrity on devices transitioned out of shared mode.",
            Tags = ["shared-pc", "disk", "profiles", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DiskLevelDeletion", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelDeletion")],
            DetectOps = [RegOp.CheckDword(Key, "DiskLevelDeletion", 0)],
        },
        new TweakDef
        {
            Id = "shpc-zero-disk-caching-level",
            Label = "Disable Disk Level Caching",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Shared PC disk level caching controls the threshold at which the shared PC cleanup service begins compressing or removing cached profile data. Setting disk level caching to zero disables threshold-triggered caching operations managed by the Shared PC service. Dedicated workstations do not require disk caching thresholds as profile cleanup is managed through standard operating system mechanisms. Shared PC caching behaviors can unexpectedly compress user profile directories, causing application state loss. Disabling this threshold prevents the Shared PC cache management service from interfering with normal profile operations. Standard Windows disk management and profile management policies govern storage usage when shared PC caching is off.",
            Tags = ["shared-pc", "caching", "profiles", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DiskLevelCaching", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DiskLevelCaching")],
            DetectOps = [RegOp.CheckDword(Key, "DiskLevelCaching", 0)],
        },
        new TweakDef
        {
            Id = "shpc-zero-inactive-threshold",
            Label = "Disable Inactive User Threshold",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The inactive threshold in Shared PC mode determines how many days of inactivity trigger automatic account deletion during maintenance windows. Setting this to zero disables time-based account deletion entirely in the Shared PC policy framework. Dedicated workstations maintain persistent user profiles and should not automatically delete accounts based on inactivity. User account lifecycle management on dedicated devices is handled through Active Directory and HR-driven deprovisioning processes. Automatic account deletion without enterprise coordination can violate data governance and audit requirements. This setting ensures user accounts persist until explicitly deprovisioned through proper IT workflows.",
            Tags = ["shared-pc", "accounts", "profiles", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "InactiveThreshold", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "InactiveThreshold")],
            DetectOps = [RegOp.CheckDword(Key, "InactiveThreshold", 0)],
        },
        new TweakDef
        {
            Id = "shpc-zero-max-page-file-mb",
            Label = "Disable Shared PC Max Page File Size",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Shared PC mode can impose a maximum page file size restriction to conserve disk space when multiple user profiles share limited storage. Setting this to zero removes the Shared PC imposed page file size ceiling. Dedicated workstations managed by separate page file policies should not have an additional Shared PC constraint overriding the configured page file size. Conflicting page file size policies can result in insufficient virtual memory for workloads that exceed the Shared PC imposed ceiling. Dedicated workstation page file sizing should be governed by the PageFile policy settings or system defaults exclusively. Removing the Shared PC page file restriction ensures only the authoritative page file policy applies.",
            Tags = ["shared-pc", "pagefile", "memory", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSizeMB", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSizeMB")],
            DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSizeMB", 0)],
        },
        new TweakDef
        {
            Id = "shpc-delete-guest-on-logoff",
            Label = "Delete Guest Account on Logoff",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Guest accounts created for temporary access accumulate profile data and application state that persists across sessions if not cleaned up. Enabling deletion of the guest account on logoff ensures no data from guest sessions persists on the device after the guest logs out. This policy is a security best practice for devices used in public or semi-public environments where guest access is permitted. Residual guest profile data could contain sensitive information browsed or downloaded during the guest session. Devices in public access areas such as lobbies, libraries, and conference rooms benefit most from automatic guest cleanup. Combining this setting with other Shared PC policies creates a comprehensive ephemeral session environment.",
            Tags = ["shared-pc", "guest", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DeleteGuestAccountOnLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeleteGuestAccountOnLogoff")],
            DetectOps = [RegOp.CheckDword(Key, "DeleteGuestAccountOnLogoff", 1)],
        },
        new TweakDef
        {
            Id = "shpc-restrict-local-storage",
            Label = "Restrict Local Storage Access",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Restricting local storage in Shared PC environments prevents users from saving large amounts of data to the local disk, preserving space for system operations. This policy is particularly important for shared devices where multiple user profiles compete for limited storage capacity. Combining local storage restriction with cloud synchronization policies directs user data to centrally managed repositories. Shared devices in classroom or kiosk configurations benefit from storage restrictions that prevent individual users from consuming the entire disk. Users on restricted devices still have access to their cloud-synchronized documents and files through mobile clients. The restriction does not prevent normal application usage, only limits the growth of user-created local content.",
            Tags = ["shared-pc", "storage", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLocalStorage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalStorage")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLocalStorage", 1)],
        },
        new TweakDef
        {
            Id = "shpc-disable-enabled-flag",
            Label = "Disable Shared PC Enabled Flag",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The Enabled flag in Shared PC policy acts as a master switch that activates all other Shared PC behaviors including account management and cleanup. Setting this flag to zero disables the entire Shared PC policy framework on the device. Dedicated workstations should have Shared PC mode fully disabled to prevent any unintended overlap with shared device behaviors. Disabling the Enabled flag supersedes other individual Shared PC policy settings. This setting is appropriate as a cleanup measure when migrating devices from shared configurations to dedicated single-user deployments. Standard enterprise workstation management takes over all session and account management when Shared PC is fully disabled.",
            Tags = ["shared-pc", "kiosk", "accounts", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
            DetectOps = [RegOp.CheckDword(Key, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "shpc-clear-kiosk-aumid",
            Label = "Clear Kiosk Mode Application ID",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "The Kiosk Mode AUMID specifies the application that runs in full-screen kiosk mode when Shared PC kiosk configuration is active. Clearing this value removes any configured kiosk application assignment from the Shared PC policy. Dedicated workstations operating in standard desktop mode should not have a kiosk AUMID configured. An accidentally inherited kiosk AUMID can cause unexpected single-application lockout behavior if Shared PC mode is re-enabled. Clearing this value ensures devices transitioned from kiosk to standard configuration do not retain kiosk application assignments. Standard multi-application desktop behavior is preserved when no AUMID is configured.",
            Tags = ["shared-pc", "kiosk", "applications", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "KioskModeAUMID", "")],
            RemoveOps = [RegOp.DeleteValue(Key, "KioskModeAUMID")],
            DetectOps = [RegOp.CheckString(Key, "KioskModeAUMID", "")],
        },
        new TweakDef
        {
            Id = "shpc-require-signin-on-resume",
            Label = "Require Sign-In on Resume",
            Category = "Shared PC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Requiring sign-in on resume from sleep or hibernation ensures that unauthorized users cannot access a shared device left unattended in a locked state. This policy enforces password prompt on wake for all users on shared devices, preventing unauthorized session hijacking. Shared public access devices are particularly vulnerable to unauthorized access between legitimate user sessions. Combined with short screen lock timeouts, this setting provides a strong access control baseline for multi-user environments. Sign-in on resume also ensures any screen content from the previous session is cleared before the new user can view the display. This security measure is aligned with CIS benchmark recommendations for shared computing environments.",
            Tags = ["shared-pc", "security", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SignInOnResume", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "SignInOnResume")],
            DetectOps = [RegOp.CheckDword(Key, "SignInOnResume", 1)],
        },
    ];
}
