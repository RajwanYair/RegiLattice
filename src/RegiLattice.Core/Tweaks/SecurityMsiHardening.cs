namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SecurityMsiHardening
{
    private const string InstallerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "msiinst-disable-always-install-elevated",
                Label = "Block AlwaysInstallElevated MSI Privilege Escalation",
                Category = "Security — Windows Installer",
                Description =
                    "Prevents Windows Installer from always elevating to SYSTEM privilege during package installation. "
                    + "AlwaysInstallElevated=1 is a known privilege escalation vector that allows any user to install packages with SYSTEM rights. "
                    + "Default: 0 (disabled). Recommended: 0 — ensure this is never set to 1.",
                Tags = ["msi", "installer", "privilege-escalation", "alwaysinstallelevated", "system-privilege", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "AlwaysInstallElevated", 0)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "AlwaysInstallElevated")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "AlwaysInstallElevated", 0)],
            },
            new TweakDef
            {
                Id = "msiinst-disable-user-alwaysinstallelevated",
                Label = "Block AlwaysInstallElevated in User Policy",
                Category = "Security — Windows Installer",
                Description =
                    "Disables the user-side AlwaysInstallElevated policy that can work in combination with the machine key. "
                    + "Both HKLM and HKCU must be set for the elevation to take effect; this ensures the HKCU side is hardened. "
                    + "Default: 0. Recommended: 0.",
                Tags = ["msi", "installer", "hkcu", "alwaysinstallelevated", "per-user", "privilege"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated")],
                DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows\Installer", "AlwaysInstallElevated", 0)],
            },
            new TweakDef
            {
                Id = "msiinst-disable-admin-install-override",
                Label = "Disable User Ability to Override MSI Installation Options",
                Category = "Security — Windows Installer",
                Description =
                    "Prevents users from overriding installation options set by the system administrator, "
                    + "such as installation directories or feature selections in MSI packages. "
                    + "Default: override may be allowed. Recommended: disabled.",
                Tags = ["msi", "installer", "user-override", "admin-control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "EnableUserControl", 0)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "EnableUserControl")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "EnableUserControl", 0)],
            },
            new TweakDef
            {
                Id = "msiinst-require-admin-for-patches",
                Label = "Require Admin Approval for MSI Patch Installation",
                Category = "Security — Windows Installer",
                Description =
                    "Disables the ability for non-administrative users to install MSI patches (MSP files). "
                    + "Prevents supply-chain attacks where a user applies an unauthorised patch to a privileged product. "
                    + "Default: patches may be installed by users. Recommended: admin required.",
                Tags = ["msi", "patch", "msp", "admin-approval", "supply-chain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "DisablePatchUninstall", 1)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "DisablePatchUninstall")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "DisablePatchUninstall", 1)],
            },
            new TweakDef
            {
                Id = "msiinst-disable-browser-install",
                Label = "Disable MSI Installation Triggered by Browser",
                Category = "Security — Windows Installer",
                Description =
                    "Blocks Internet Explorer and WebBrowser components from triggering MSI package installations. "
                    + "Prevents drive-by installer attacks initiated from malicious web pages. "
                    + "Default: browser-triggered installs allowed. Recommended: disabled.",
                Tags = ["msi", "browser", "drive-by", "install", "internet-explorer"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "SafeForScripting", 0)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "SafeForScripting")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "SafeForScripting", 0)],
            },
            new TweakDef
            {
                Id = "msiinst-log-all-operations",
                Label = "Enable Full Verbose Logging for MSI Installations",
                Category = "Security — Windows Installer",
                Description =
                    "Enables detailed MSI event logging for all install, uninstall, and patch operations. "
                    + "Logs are written to %TEMP%\\MSI*.log files and are invaluable for forensic investigation after unexpected installs. "
                    + "Default: minimal or no logging. Recommended: enabled.",
                Tags = ["msi", "logging", "audit", "forensic", "installer-log"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "Logging", 1)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "Logging")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "Logging", 1)],
            },
            new TweakDef
            {
                Id = "msiinst-block-removable-media",
                Label = "Block MSI Install from Removable Media",
                Category = "Security — Windows Installer",
                Description =
                    "Prevents Windows Installer from installing software packages sourced from removable or removable-network drives. "
                    + "Mitigates USB-based software planting attacks on locked-down workstations. "
                    + "Default: removable media install allowed. Recommended: blocked.",
                Tags = ["msi", "usb", "removable-media", "portable-device", "supply-chain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "DisableMedia", 1)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "DisableMedia")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "DisableMedia", 1)],
            },
            new TweakDef
            {
                Id = "msiinst-block-rollback-env",
                Label = "Disable MSI Unsafe Rollback Environment Variable Exposure",
                Category = "Security — Windows Installer",
                Description =
                    "Prevents Windows Installer from writing unencrypted environment variable data to rollback script files on disk. "
                    + "Rollback scripts written to %TEMP% with DisableRollback=0 may expose sensitive environment variables to other users. "
                    + "Default: rollback script always created. Recommended: minimise exposure.",
                Tags = ["msi", "rollback", "environment-variable", "temp-file", "info-disclosure"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(InstallerKey, "DisableRollback", 0)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "DisableRollback")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "DisableRollback", 0)],
            },
            new TweakDef
            {
                Id = "msiinst-block-advertised-installs",
                Label = "Block Advertised MSI Package Elevation",
                Category = "Security — Windows Installer",
                Description =
                    "Prevents non-administrative users from installing advertised packages that require elevation. "
                    + "Standard users should not be able to install products that were advertised to them with elevated permissions. "
                    + "Default: advertised install elevation allowed. Recommended: blocked.",
                Tags = ["msi", "advertised-install", "elevation", "standard-user", "privilege"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ApplyOps = [RegOp.SetDword(InstallerKey, "DisableUserInstalls", 1)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "DisableUserInstalls")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "DisableUserInstalls", 1)],
            },
            new TweakDef
            {
                Id = "msiinst-hide-install-ui",
                Label = "Disable MSI Reduced UI Bypass",
                Category = "Security — Windows Installer",
                Description =
                    "Requires all MSI installs to present their full user interface rather than reduced or hidden UI. "
                    + "Silent/hidden installs are a common technique for stealthy malware deployment. "
                    + "Default: reduced UI allowed. Recommended: full UI required.",
                Tags = ["msi", "silent-install", "hidden-install", "malware", "detection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ApplyOps = [RegOp.SetDword(InstallerKey, "DisableFlyweightPatching", 1)],
                RemoveOps = [RegOp.DeleteValue(InstallerKey, "DisableFlyweightPatching")],
                DetectOps = [RegOp.CheckDword(InstallerKey, "DisableFlyweightPatching", 1)],
            },
        ];
}
