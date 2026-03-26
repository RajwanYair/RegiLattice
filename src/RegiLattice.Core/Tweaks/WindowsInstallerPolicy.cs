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
                Id = "msipl-block-removable-media-installs",
                Label = "Block MSI Installation From Removable Media",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents Windows Installer from installing programs from removable media such as USB drives and optical discs, reducing supply-chain and physical-access attack risk.",
                Tags = ["msi", "installer", "usb", "removable", "media", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks rogue USB or disc-based MSI packages; all installs must come from network or approved sources.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMedia")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMedia", 1)],
            },
            new TweakDef
            {
                Id = "msipl-block-patch-application",
                Label = "Block Users From Applying MSI Patches",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents standard users from applying patches (.msp files) to installed applications, requiring administrator authorization for all MSI patch operations.",
                Tags = ["msi", "installer", "patch", "msp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents unauthorized software patching; IT must push patches through managed channels.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePatch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePatch")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePatch", 1)],
            },
            new TweakDef
            {
                Id = "msipl-disable-browse-dialog",
                Label = "Disable MSI Browse Dialog for Installation Source",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents users from browsing to an alternate installation source when Windows Installer cannot locate the original package, closing a social-engineering vector.",
                Tags = ["msi", "installer", "browse", "source", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents redirection of the installer to a malicious source package.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBrowse", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBrowse")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBrowse", 1)],
            },
            new TweakDef
            {
                Id = "msipl-secure-msi-transforms",
                Label = "Require Secure MSI Transform Storage",
                Category = "Windows Installer Policy",
                Description =
                    "Forces Windows Installer to cache transforms in a secured per-machine location rather than the original source path, preventing transform-substitution tampering attacks.",
                Tags = ["msi", "installer", "transform", "mst", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents transform-substitution attacks on managed MSI deployments.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TransformsSecure", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TransformsSecure")],
                DetectOps = [RegOp.CheckDword(Key, "TransformsSecure", 1)],
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
                Id = "msipl-suppress-system-restore-checkpoints",
                Label = "Suppress Installer System Restore Checkpoints",
                Category = "Windows Installer Policy",
                Description =
                    "Prevents Windows Installer from automatically creating System Restore checkpoints before installations, reducing disk I/O and checkpoint sprawl on managed machines.",
                Tags = ["msi", "installer", "system restore", "checkpoint", "disk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "Marginally faster installs; removes pre-install restore points — exercise caution on personal machines.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LimitSystemRestoreCheckpointing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LimitSystemRestoreCheckpointing")],
                DetectOps = [RegOp.CheckDword(Key, "LimitSystemRestoreCheckpointing", 1)],
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
