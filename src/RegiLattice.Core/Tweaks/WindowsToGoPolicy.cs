#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 257 — Windows To Go Policy (10 tweaks)
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\PortableOperatingSystem
internal static class WindowsToGoPolicy
{
    private const string WtgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableOperatingSystem";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wtg-disable-sleep",
            Label = "Disable Sleep States for Windows To Go",
            Category = "Windows To Go Policy",
            Description = "Sets EnableSleep=0 in the PortableOperatingSystem policy key. "
                + "Prevents Windows To Go workspaces from entering S1-S3 sleep states while running "
                + "from a USB drive. Sleep states on WTG disks can corrupt the workspace if the USB "
                + "connection is interrupted during wake-up. Applying this ensures the system either "
                + "stays awake or shuts down completely, never entering an intermediate sleep state "
                + "when running from a WTG workspace. Default: absent (sleep allowed).",
            Tags = ["windows-to-go", "sleep", "usb", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Sleep states disabled for WTG workspaces; prevents USB-to-sleep corruption scenarios.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "EnableSleep", 0)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableSleep")],
            DetectOps = [RegOp.CheckDword(WtgKey, "EnableSleep", 0)],
        },
        new TweakDef
        {
            Id = "wtg-disable-hibernation",
            Label = "Disable Hibernation for Windows To Go",
            Category = "Windows To Go Policy",
            Description = "Sets EnableHibernation=0 in the PortableOperatingSystem policy key. "
                + "Prevents Windows To Go workspaces from using the hibernate (S4) power state. "
                + "Hibernation on a WTG USB workspace saves RAM to the hiberfil.sys on the USB disk, "
                + "but wake-up can fail if the USB drive is moved or the system firmware changes. "
                + "Disabling hibernate avoids this by requiring a full shutdown instead. "
                + "Default: absent (hibernation allowed).",
            Tags = ["windows-to-go", "hibernation", "hibernate", "usb", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hibernation disabled for WTG workspaces; prevents hiberfil corruption on USB devices.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "EnableHibernation", 0)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "EnableHibernation")],
            DetectOps = [RegOp.CheckDword(WtgKey, "EnableHibernation", 0)],
        },
        new TweakDef
        {
            Id = "wtg-block-workspace-creation",
            Label = "Block Windows To Go Workspace Creation",
            Category = "Windows To Go Policy",
            Description = "Sets NoWorkspaceCreation=1 in the PortableOperatingSystem policy key. "
                + "Prevents users from using the Windows To Go Workspace Creator wizard to create "
                + "new WTG workspaces from this machine. Ensures WTG environments are only created "
                + "by IT administrators and not by standard users who may inadvertently copy "
                + "sensitive corporate data to an unmanaged USB drive. "
                + "Default: absent (creation allowed). Recommended: 1 on managed corporate endpoints.",
            Tags = ["windows-to-go", "workspace-creation", "usb", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WTG workspace creation wizard blocked; only IT-created workspaces can be used.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "NoWorkspaceCreation", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "NoWorkspaceCreation")],
            DetectOps = [RegOp.CheckDword(WtgKey, "NoWorkspaceCreation", 1)],
        },
        new TweakDef
        {
            Id = "wtg-block-boot-from-external",
            Label = "Block Booting From External WTG Media",
            Category = "Windows To Go Policy",
            Description = "Sets BlockBootFromExternalMedia=1 in the PortableOperatingSystem policy key. "
                + "Prevents this machine from booting a Windows To Go workspace from external USB media. "
                + "Ensures the machine always boots its internal Windows installation and cannot be "
                + "redirected by an inserted WTG USB drive. Protects against using WTG to bypass local "
                + "security controls or Intune/Group Policy enrollment. "
                + "Default: absent (booting from external media allowed).",
            Tags = ["windows-to-go", "boot", "usb", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WTG boot from external USB media blocked; internal Windows always boots instead.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "BlockBootFromExternalMedia", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "BlockBootFromExternalMedia")],
            DetectOps = [RegOp.CheckDword(WtgKey, "BlockBootFromExternalMedia", 1)],
        },
        new TweakDef
        {
            Id = "wtg-disable-host-offline-folders",
            Label = "Disable Host Offline Folders in Windows To Go",
            Category = "Windows To Go Policy",
            Description = "Sets NoOfflineFolders=1 in the PortableOperatingSystem policy key. "
                + "Prevents the WTG workspace from accessing the host machine's Offline Files cache. "
                + "Ensures that when a user boots into a WTG workspace, they cannot read or write "
                + "the Offline Files data of the host machine, preventing data leakage from the "
                + "host's cached network files into the WTG environment. "
                + "Default: absent (offline folder access allowed). Recommended: 1 on shared/kiosk desktops.",
            Tags = ["windows-to-go", "offline-folders", "data-leakage", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Offline Folders cache on host not accessible from WTG workspace.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "NoOfflineFolders", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "NoOfflineFolders")],
            DetectOps = [RegOp.CheckDword(WtgKey, "NoOfflineFolders", 1)],
        },
        new TweakDef
        {
            Id = "wtg-disable-retail-demo",
            Label = "Disable Retail Demo Mode for Windows To Go",
            Category = "Windows To Go Policy",
            Description = "Sets DisableRetailDemo=1 in the PortableOperatingSystem policy key. "
                + "Suppresses the Retail Demo Experience (RDX) from being shown or launched when "
                + "a WTG workspace boots on a retail display or demo machine. Prevents WTG workspaces "
                + "from being used as a kiosk demo mode and ensures productive enterprise use only. "
                + "Default: absent. Recommended: 1 on all non-retail WTG deployments.",
            Tags = ["windows-to-go", "retail-demo", "kiosk", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "Retail demo mode suppressed in WTG workspaces.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "DisableRetailDemo", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableRetailDemo")],
            DetectOps = [RegOp.CheckDword(WtgKey, "DisableRetailDemo", 1)],
        },
        new TweakDef
        {
            Id = "wtg-disable-sync-on-metered",
            Label = "Disable Sync Provider on Metered Connection for WTG",
            Category = "Windows To Go Policy",
            Description = "Sets DisableSyncProviderOnMetered=1 in the PortableOperatingSystem policy key. "
                + "Prevents the WTG workspace from contacting cloud sync providers (OneDrive, Dropbox, etc.) "
                + "when the device is on a metered network connection. Reduces data usage costs for WTG "
                + "workspaces roaming over mobile broadband or tethered hotspots. "
                + "Default: absent (sync allowed on metered). Recommended: 1 to protect data budgets.",
            Tags = ["windows-to-go", "sync", "metered", "onedrive", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Cloud sync providers blocked on metered connections in WTG workspaces.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "DisableSyncProviderOnMetered")],
            DetectOps = [RegOp.CheckDword(WtgKey, "DisableSyncProviderOnMetered", 1)],
        },
        new TweakDef
        {
            Id = "wtg-block-cross-hardware-deploy",
            Label = "Block Cross-Hardware WTG Deployment",
            Category = "Windows To Go Policy",
            Description = "Sets NoCrossHardwareDeploy=1 in the PortableOperatingSystem policy key. "
                + "Prevents the WTG workspace from being moved to a different hardware platform once it "
                + "has been provisioned. Cross-hardware WTG deployment can cause driver conflicts, "
                + "DHCP/MAC-address confusion, or break hardware-specific licensing tied to the original "
                + "provisioning machine. Restricting this ensures workspace integrity. "
                + "Default: absent (cross-hardware allowed). Recommended: 1 in managed enterprise WTG.",
            Tags = ["windows-to-go", "hardware", "deploy", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "WTG workspace cannot be re-provisioned on different hardware.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "NoCrossHardwareDeploy", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "NoCrossHardwareDeploy")],
            DetectOps = [RegOp.CheckDword(WtgKey, "NoCrossHardwareDeploy", 1)],
        },
        new TweakDef
        {
            Id = "wtg-enforce-secure-boot",
            Label = "Enforce Secure Boot for Windows To Go Workspaces",
            Category = "Windows To Go Policy",
            Description = "Sets RequireSecureBoot=1 in the PortableOperatingSystem policy key. "
                + "Requires that the host machine's Secure Boot setting be enabled before a WTG "
                + "workspace will boot. Prevents WTG from being used as an attack vector on machines "
                + "where Secure Boot has been disabled, ensuring the WTG kernel and boot files are "
                + "signed and unmodified. "
                + "Default: absent (Secure Boot not required). Recommended: 1 for security-hardened environments.",
            Tags = ["windows-to-go", "secure-boot", "uefi", "security", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "WTG workspace only boots on machines with Secure Boot enabled.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "RequireSecureBoot", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "RequireSecureBoot")],
            DetectOps = [RegOp.CheckDword(WtgKey, "RequireSecureBoot", 1)],
        },
        new TweakDef
        {
            Id = "wtg-disable-automatic-update",
            Label = "Disable Automatic Windows Update in WTG Workspace",
            Category = "Windows To Go Policy",
            Description = "Sets NoAutoUpdate=1 in the PortableOperatingSystem policy key. "
                + "Prevents the WTG workspace from automatically downloading and installing Windows updates "
                + "while running on the road. Updates in a WTG workspace use the host machine's internet "
                + "connection and can run out of USB drive space or interrupt productivity. "
                + "Updates should be applied via WSUS or a scheduled service window instead. "
                + "Default: absent (automatic updates allowed). Recommended: 1 on managed WTG deployments.",
            Tags = ["windows-to-go", "windows-update", "automatic", "portable", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Automatic Windows Update disabled in WTG workspaces; updates must be pushed manually.",
            ApplyOps  = [RegOp.SetDword(WtgKey, "NoAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(WtgKey, "NoAutoUpdate")],
            DetectOps = [RegOp.CheckDword(WtgKey, "NoAutoUpdate", 1)],
        },
    ];
}
