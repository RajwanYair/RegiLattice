// RegiLattice.Core — Tweaks/MsiInstallerPolicy.cs
// Category: "MSI Installer Policy" — Slug "msipol"
// HKLM\SOFTWARE\Policies\Microsoft\Windows\Installer
// Windows Installer security and behaviour policy beyond the basic AlwaysInstallElevated
// and DisableRollback keys already covered in AppxPolicy.cs and DiskCleanup.cs.

#nullable enable

using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class MsiInstallerPolicy
{
    private const string Inst = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Installer";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "msipol-disable-patch-install",
            Label = "Prevent Users from Patching MSI Packages",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisablePatch=1 in Windows Installer policy. Prevents users from patching any MSI application by blocking the application of .msp patch files. Only administrators can apply patches. Stops untrusted patches from silently modifying installed applications.",
            Tags = ["msi", "installer", "patch", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisablePatch", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisablePatch")],
            DetectOps = [RegOp.CheckDword(Inst, "DisablePatch", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-source-browsing",
            Label = "Prevent Users from Browsing Install Sources",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisableBrowse=1 in Windows Installer policy. Prevents the Windows Installer from allowing users to browse for an installation source (e.g., a different CD or network share) when a product is being repaired or re-installed. All installs must use the cached or registered source path.",
            Tags = ["msi", "installer", "browse", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisableBrowse", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisableBrowse")],
            DetectOps = [RegOp.CheckDword(Inst, "DisableBrowse", 1)],
        },
        new TweakDef
        {
            Id = "msipol-restrict-user-installs",
            Label = "Restrict MSI Installs to Elevated Users Only",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisableMSI=1 in Windows Installer policy. Restricts Windows Installer so that only administrators can install MSI packages (standard users receive an error). Value 0=allow all, 1=admins only, 2=block all MSI. Setting 1 prevents software installation by standard accounts.",
            Tags = ["msi", "installer", "restrict", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisableMSI", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisableMSI")],
            DetectOps = [RegOp.CheckDword(Inst, "DisableMSI", 1)],
        },
        new TweakDef
        {
            Id = "msipol-secure-transforms",
            Label = "Secure MSI Transform Files in User Profile",
            Category = "MSI Installer Policy",
            Description =
                "Sets TransformsSecure=1 in Windows Installer policy. Instructs the Windows Installer to store MSI transform (.mst) files in a secure location in the user profile rather than in the TEMP directory. Prevents other users from tampering with transform files used during product re-installation.",
            Tags = ["msi", "installer", "transforms", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "TransformsSecure", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "TransformsSecure")],
            DetectOps = [RegOp.CheckDword(Inst, "TransformsSecure", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-scripting",
            Label = "Disable Unsafe MSI Script Execution",
            Category = "MSI Installer Policy",
            Description =
                "Sets SafeForScripting=0 in Windows Installer policy. Disables the ability for web-based content or scripts to silently invoke the Windows Installer COM object to install software. Prevents drive-by installations triggered by browser scripts or malicious web pages.",
            Tags = ["msi", "installer", "scripting", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "SafeForScripting", 0)],
            RemoveOps = [RegOp.DeleteValue(Inst, "SafeForScripting")],
            DetectOps = [RegOp.CheckDword(Inst, "SafeForScripting", 0)],
        },
        new TweakDef
        {
            Id = "msipol-enforce-upgrade-component-rules",
            Label = "Enforce MSI Upgrade Component Rules",
            Category = "MSI Installer Policy",
            Description =
                "Sets EnforceUpgradeComponentRules=1 in Windows Installer policy. Causes the Windows Installer to reject patches that would violate component rules during an upgrade sequence. Prevents improperly authored patches from corrupting installed applications by adding or removing component references outside of the product's authorised upgrade path.",
            Tags = ["msi", "installer", "upgrade", "policy", "integrity"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "EnforceUpgradeComponentRules", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "EnforceUpgradeComponentRules")],
            DetectOps = [RegOp.CheckDword(Inst, "EnforceUpgradeComponentRules", 1)],
        },
        new TweakDef
        {
            Id = "msipol-limit-restore-checkpoints",
            Label = "Limit System Restore Points During MSI Install",
            Category = "MSI Installer Policy",
            Description =
                "Sets LimitSystemRestoreCheckpointing=1 in Windows Installer policy. Prevents the Windows Installer from creating a System Restore checkpoint before every package installation. Reduces System Restore disk space consumption and write activity on machines where MSI packages are frequently deployed.",
            Tags = ["msi", "installer", "restore", "policy", "performance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "LimitSystemRestoreCheckpointing", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "LimitSystemRestoreCheckpointing")],
            DetectOps = [RegOp.CheckDword(Inst, "LimitSystemRestoreCheckpointing", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-lockdown-browse-ui",
            Label = "Restrict Browse UI in Lockdown Mode",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisableLockdownBrowseUI=1 in Windows Installer policy. When an MSI package runs in locked-down mode (elevated), this setting prevents the installer from displaying any file-browse dialogs that would let the user navigate the file system during setup. Closes a potential path-traversal risk in privileged installer contexts.",
            Tags = ["msi", "installer", "lockdown", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisableLockdownBrowseUI", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisableLockdownBrowseUI")],
            DetectOps = [RegOp.CheckDword(Inst, "DisableLockdownBrowseUI", 1)],
        },
        new TweakDef
        {
            Id = "msipol-disable-forbidden-patch",
            Label = "Restrict Patching to Authorised Patch Lists",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisableForbidPatch=0 in Windows Installer policy. Ensures that patch policies (AllowedPatchList / ForbiddenPatchList) are honoured by the Windows Installer, so only administrator-approved patches can be applied to managed MSI products. Value 1 would disable the forbidden list enforcement.",
            Tags = ["msi", "installer", "patch", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisableForbidPatch", 0)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisableForbidPatch")],
            DetectOps = [RegOp.CheckDword(Inst, "DisableForbidPatch", 0)],
        },
        new TweakDef
        {
            Id = "msipol-disable-media-source-fallback",
            Label = "Disable MSI Source Fallback to Removable Media",
            Category = "MSI Installer Policy",
            Description =
                "Sets DisableMedia=1 in Windows Installer policy. Prevents the Windows Installer from falling back to removable media (CD/DVD/USB) as an installation source when the cached or network source is unavailable. Stops users from introducing software from removable media during repair or re-installation.",
            Tags = ["msi", "installer", "media", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Inst, "DisableMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(Inst, "DisableMedia")],
            DetectOps = [RegOp.CheckDword(Inst, "DisableMedia", 1)],
        },
    ];
}
