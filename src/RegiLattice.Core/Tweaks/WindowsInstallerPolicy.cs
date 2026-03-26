namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInstallerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "msipl-disable-always-install-elevated",
                Label = "Disable Always Install With Elevated Privileges",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents Windows Installer from using SYSTEM privileges when installing any program, closing a well-known local privilege escalation vector (MSI AlwaysInstallElevated).",
                Tags = ["msi", "installer", "privilege", "security", "elevation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Critical: eliminates the AlwaysInstallElevated local privilege escalation path.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AlwaysInstallElevated", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AlwaysInstallElevated")],
                DetectOps = [RegOp.CheckDword(Key, "AlwaysInstallElevated", 0)],
            },
            new TweakDef
            {
                Id = "msipl-disable-user-installs",
                Label = "Restrict MSI Installation to Administrators",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents standard users from running Windows Installer packages, requiring administrator authorization for all MSI-based software installations.",
                Tags = ["msi", "installer", "users", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Reduces attack surface; standard users must request IT to deploy software.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserInstalls", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserInstalls")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserInstalls", 1)],
            },
            new TweakDef
            {
                Id = "msipl-deny-browsing-elevated-installs",
                Label = "Deny Source Browsing During Elevated MSI Installs",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents elevated (lockdown) Windows Installer installations from browsing to an alternate source, closing a privilege escalation path where a user redirects an elevated install to a malicious package.",
                Tags = ["msi", "installer", "browse", "lockdown", "elevation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Closes browse-redirect privilege escalation during lockdown installs.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowLockdownBrowse", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownBrowse")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLockdownBrowse", 0)],
            },
            new TweakDef
            {
                Id = "msipl-deny-media-elevated-installs",
                Label = "Deny Removable Media Sources During Elevated MSI Installs",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents elevated (lockdown) Windows Installer installations from using removable media as an installation source, blocking disc- or USB-swap attacks on privileged installs.",
                Tags = ["msi", "installer", "media", "usb", "lockdown", "elevation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents USB/disc swap attacks against elevated MSI sessions.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowLockdownMedia", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownMedia")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLockdownMedia", 0)],
            },
            new TweakDef
            {
                Id = "msipl-deny-patch-elevated-installs",
                Label = "Deny Patching During Elevated MSI Installs",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents elevated (lockdown) Windows Installer installations from applying patches, ensuring patches cannot be injected during a privileged install session.",
                Tags = ["msi", "installer", "patch", "lockdown", "elevation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents patch injection during elevated MSI sessions.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowLockdownPatch", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLockdownPatch")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLockdownPatch", 0)],
            },
            new TweakDef
            {
                Id = "msipl-disable-patch-caching",
                Label = "Disable MSI Patch File Caching",
                Category = "Windows Installer Policy",
                Description =
                    "Sets the maximum patch cache size to zero, preventing Windows Installer from caching patch files on disk and reclaiming storage consumed by stale .msp files.",
                Tags = ["msi", "installer", "patch", "cache", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reclaims disk space; patch re-application may require the original source.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxPatchCacheSize", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxPatchCacheSize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxPatchCacheSize", 0)],
            },
            new TweakDef
            {
                Id = "msipl-disable-user-control",
                Label = "Disable User Control Over Installation Options",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents users from overriding installation settings defined by system policy, ensuring enterprise MSI configurations remain authoritative and cannot be bypassed.",
                Tags = ["msi", "installer", "control", "policy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enforces policy-defined install settings; users cannot override feature selection or directories.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableUserControl", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableUserControl")],
                DetectOps = [RegOp.CheckDword(Key, "EnableUserControl", 0)],
            },
            new TweakDef
            {
                Id = "msipl-restrict-source-search-network",
                Label = "Restrict MSI Source Search to Network Locations Only",
                Category = "Windows Installer Policy",
                Description =
                    "Configures Windows Installer to search only network locations (n) when resolving missing installation sources, preventing Installer from falling back to local drives or removable media.",
                Tags = ["msi", "installer", "source", "network", "search", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Ensures managed installs resolve only from authorised network shares.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "SearchOrder", "n")],
                RemoveOps = [RegOp.DeleteValue(Key, "SearchOrder")],
                DetectOps = [RegOp.CheckString(Key, "SearchOrder", "n")],
            },
            new TweakDef
            {
                Id = "msipl-disable-rollback",
                Label = "Disable MSI Installation Rollback",
                Category = "Windows Installer Policy",
                Description =
                    "Disables the creation of rollback files during MSI installations, reducing disk usage at the cost of installation recovery capability if an install fails mid-way.",
                Tags = ["msi", "installer", "rollback", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Frees disk space during installs; failed installs may leave partial state with no rollback option.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRollback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRollback")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRollback", 1)],
            },
            new TweakDef
            {
                Id = "msipl-enable-verbose-event-logging",
                Label = "Enable Verbose MSI Event Logging",
                Category = "Windows Installer Policy",
                Description =
                    "Enables detailed Windows Installer event logging (voicewarmupx mode) to the Application event log, providing a comprehensive audit trail for all software install and remove operations.",
                Tags = ["msi", "installer", "logging", "audit", "events", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Improves software audit trail; negligible performance overhead.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetString(Key, "Logging", "voicewarmupx")],
                RemoveOps = [RegOp.DeleteValue(Key, "Logging")],
                DetectOps = [RegOp.CheckString(Key, "Logging", "voicewarmupx")],
            },
        ];
}
