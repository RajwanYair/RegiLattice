namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class PolicyWindowsInstaller
{
    private const string MsiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msipol-disable-user-installs",
            Label = "Disable Per-User MSI Installs",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableUserInstalls=1 in Windows Installer policy. Prevents non-admin users "
                + "from installing MSI packages to per-user locations. Only admin-elevated installs "
                + "to per-machine locations are permitted, reducing shadow IT installations.",
            Tags = ["msi", "installer", "per-user", "policy", "security"],
            RegistryKeys = [MsiKey],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Users cannot install MSI packages without admin elevation.",
            ApplyOps = [RegOp.SetDword(MsiKey, "DisableUserInstalls", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "DisableUserInstalls")],
            DetectOps = [RegOp.CheckDword(MsiKey, "DisableUserInstalls", 1)],
        },
        new TweakDef
        {
            Id = "msipol-always-install-elevated-off",
            Label = "Disable Always Install Elevated",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AlwaysInstallElevated=0 in Windows Installer policy. Ensures that MSI packages "
                + "are NOT installed with elevated privileges by default. Leaving this enabled is a "
                + "well-known privilege escalation vector (CVE-2021-1727 and similar).",
            Tags = ["msi", "installer", "elevation", "privilege", "policy", "security"],
            RegistryKeys = [MsiKey],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Closes the AlwaysInstallElevated privilege escalation vector.",
            ApplyOps = [RegOp.SetDword(MsiKey, "AlwaysInstallElevated", 0)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "AlwaysInstallElevated")],
            DetectOps = [RegOp.CheckDword(MsiKey, "AlwaysInstallElevated", 0)],
        },
        new TweakDef
        {
            Id = "msipol-disable-msi-rollback",
            Label = "Disable MSI Rollback on Failure",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableRollback=1 in Windows Installer policy. Prevents Windows Installer from "
                + "creating rollback script files during installations. Saves disk space and speeds up "
                + "large installations, but failed installs cannot be automatically reverted.",
            Tags = ["msi", "installer", "rollback", "performance", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Faster installs but no automatic rollback on failure; manual cleanup needed.",
            ApplyOps = [RegOp.SetDword(MsiKey, "DisableRollback", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "DisableRollback")],
            DetectOps = [RegOp.CheckDword(MsiKey, "DisableRollback", 1)],
        },
        new TweakDef
        {
            Id = "msipol-enable-logging",
            Label = "Enable Verbose MSI Logging",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Logging=voicewarmupx in Windows Installer policy. Enables verbose Windows "
                + "Installer logging for all MSI operations. The log flags (v=verbose, o=out-of-disk, "
                + "i=status, c=initial UI, e=error, w=warning, a=action, r=record, m=memory, u=user, "
                + "p=terminal, x=extra debug) produce detailed logs in %TEMP% for troubleshooting.",
            Tags = ["msi", "installer", "logging", "troubleshooting", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Verbose MSI logs generated in %TEMP%; helps troubleshoot install failures.",
            ApplyOps = [RegOp.SetString(MsiKey, "Logging", "voicewarmupx")],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "Logging")],
            DetectOps = [RegOp.CheckString(MsiKey, "Logging", "voicewarmupx")],
        },
        new TweakDef
        {
            Id = "msipol-disable-browse-dialog",
            Label = "Disable MSI Browse Dialog",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableBrowse=1 in Windows Installer policy. Prevents users from browsing "
                + "for MSI source files during installations. In managed environments this forces "
                + "installations to use the configured source paths only.",
            Tags = ["msi", "installer", "browse", "policy", "lockdown"],
            RegistryKeys = [MsiKey],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Users cannot browse for MSI source files during repair/install.",
            ApplyOps = [RegOp.SetDword(MsiKey, "DisableBrowse", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "DisableBrowse")],
            DetectOps = [RegOp.CheckDword(MsiKey, "DisableBrowse", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-patching",
            Label = "Disable MSI Patching for Unmanaged Apps",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePatch=1 in Windows Installer policy. Prevents the application of MSI "
                + "patches (.msp) to applications not managed by Group Policy. Reduces the risk of "
                + "malicious MSP files being applied to modify installed software.",
            Tags = ["msi", "installer", "patch", "security", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "MSP patches blocked for unmanaged apps; managed apps still patchable.",
            ApplyOps = [RegOp.SetDword(MsiKey, "DisablePatch", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "DisablePatch")],
            DetectOps = [RegOp.CheckDword(MsiKey, "DisablePatch", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-embedded-ui",
            Label = "Disable MSI Embedded UI Handlers",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableUserControl=0 in Windows Installer policy. Prevents installation packages "
                + "from presenting a custom embedded user interface, forcing all installs to use the "
                + "standard Windows Installer UI. Reduces the risk of phishing or deceptive install UIs.",
            Tags = ["msi", "installer", "embedded-ui", "security", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Custom MSI install UIs blocked; standard Windows Installer UI used.",
            ApplyOps = [RegOp.SetDword(MsiKey, "EnableUserControl", 0)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "EnableUserControl")],
            DetectOps = [RegOp.CheckDword(MsiKey, "EnableUserControl", 0)],
        },
        new TweakDef
        {
            Id = "msipol-disable-media-source",
            Label = "Disable MSI Media Source During Install",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableMedia=1 in Windows Installer policy. Prevents Windows Installer from "
                + "using removable media (CD/DVD/USB) as a source for installations. Forces installations "
                + "to use network or local cached sources only.",
            Tags = ["msi", "installer", "media", "removable", "policy", "security"],
            RegistryKeys = [MsiKey],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "MSI cannot install from removable media; network/cache sources only.",
            ApplyOps = [RegOp.SetDword(MsiKey, "DisableMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "DisableMedia")],
            DetectOps = [RegOp.CheckDword(MsiKey, "DisableMedia", 1)],
        },
        new TweakDef
        {
            Id = "msipol-safe-for-scripting-off",
            Label = "Disable MSI Safe for Scripting",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SafeForScripting=0 in Windows Installer policy. Prevents ActiveX controls "
                + "embedded in MSI custom actions from executing within Internet Explorer's security "
                + "context. Closes a legacy attack vector for web-delivered MSI packages.",
            Tags = ["msi", "installer", "scripting", "activex", "security", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "ActiveX scripting in MSI custom actions blocked; defence-in-depth.",
            ApplyOps = [RegOp.SetDword(MsiKey, "SafeForScripting", 0)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "SafeForScripting")],
            DetectOps = [RegOp.CheckDword(MsiKey, "SafeForScripting", 0)],
        },
        new TweakDef
        {
            Id = "msipol-max-patch-cache-50",
            Label = "Limit MSI Patch Cache to 50% of Disk",
            Category = "Windows Installer Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxPatchCacheSize=50 in Windows Installer policy. Limits the baseline cache "
                + "used for MSI patch rollback to 50% of disk space. The default (10%) is often "
                + "too low for heavily patched enterprise apps, causing repair failures.",
            Tags = ["msi", "installer", "cache", "disk", "policy"],
            RegistryKeys = [MsiKey],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "MSI patch cache can use up to 50% of disk; prevents repair failures.",
            ApplyOps = [RegOp.SetDword(MsiKey, "MaxPatchCacheSize", 50)],
            RemoveOps = [RegOp.DeleteValue(MsiKey, "MaxPatchCacheSize")],
            DetectOps = [RegOp.CheckDword(MsiKey, "MaxPatchCacheSize", 50)],
        },
    ];
}
