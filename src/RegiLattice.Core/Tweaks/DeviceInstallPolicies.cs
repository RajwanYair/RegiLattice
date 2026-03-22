// RegiLattice.Core — Tweaks/DeviceInstallPolicies.cs
// Device installation restriction and driver search policy tweaks (Sprint 106).
// Slug: "dinst" — controls which devices Windows is allowed to install and how drivers are sourced.
// Distinct from Usb.cs (USB-specific device tweaks) and Hardware.cs (general hardware settings).
// Registry bases:
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\DriverSearching
//   HKLM\SOFTWARE\Policies\Microsoft\Windows\Device Metadata
//   HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Installer

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeviceInstallPolicies
{
    private const string Restrictions =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

    private const string Settings =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings";

    private const string DriverSearching =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverSearching";

    private const string DeviceMetadata =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Device Metadata";

    private const string DeviceInstaller =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Installer";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dinst-deny-removable-install",
            Label = "Device Install Policy: Deny Installation of Removable Devices",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Tags = ["device-install", "removable", "policy", "security", "dlp"],
            Description =
                "Sets DenyRemovableDevices=1 in the DeviceInstall Restrictions policy. "
                + "Prevents Windows from installing any device driver for a removable device class. "
                + "Blocks USB storage drives, external HDDs, SD card readers, and other removable media "
                + "from being added to the system as new devices.",
            SideEffects = "Prevents installation of new USB storage and removable media devices. Existing already-installed devices continue to work.",
            ApplyOps = [RegOp.SetDword(Restrictions, "DenyRemovableDevices", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyRemovableDevices")],
            DetectOps = [RegOp.CheckDword(Restrictions, "DenyRemovableDevices", 1)],
        },
        new TweakDef
        {
            Id = "dinst-enable-device-id-block",
            Label = "Device Install Policy: Enable Hardware Device ID Restriction List",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["device-install", "device-id", "block-list", "policy"],
            Description =
                "Sets DenyDeviceIDs=1 in the DeviceInstall Restrictions policy. "
                + "Activates the hardware device ID restriction list, allowing administrators to block "
                + "specific devices by their hardware ID strings (e.g., 'USB\\VID_XXXX&PID_XXXX'). "
                + "This flag enables the list; devices to block are configured separately via Group Policy.",
            ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceIDs", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceIDs")],
            DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceIDs", 1)],
        },
        new TweakDef
        {
            Id = "dinst-enable-class-block",
            Label = "Device Install Policy: Enable Setup Class GUID Restriction List",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["device-install", "class-guid", "block-list", "policy"],
            Description =
                "Sets DenyDeviceClasses=1 in the DeviceInstall Restrictions policy. "
                + "Activates the setup class GUID restriction list, allowing administrators to block entire "
                + "categories of devices (e.g., USB storage class {36FC9E60-C465-11CF-8056-444553540000}). "
                + "This flag enables the list; class GUIDs to block are configured separately.",
            ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceClasses", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceClasses")],
            DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceClasses", 1)],
        },
        new TweakDef
        {
            Id = "dinst-admin-override-allowed",
            Label = "Device Install Policy: Allow Administrators to Override Device Restrictions",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["device-install", "admin", "override", "policy"],
            Description =
                "Sets AllowAdminInstall=1 in the DeviceInstall Restrictions policy. "
                + "Allows members of the local Administrators group to install any device driver, "
                + "bypassing the device ID and class GUID restriction lists. "
                + "Enables admins to add exceptions without modifying Group Policy.",
            ApplyOps = [RegOp.SetDword(Restrictions, "AllowAdminInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "AllowAdminInstall")],
            DetectOps = [RegOp.CheckDword(Restrictions, "AllowAdminInstall", 1)],
        },
        new TweakDef
        {
            Id = "dinst-retroactive-id-block",
            Label = "Device Install Policy: Apply Device ID Blocks Retroactively",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Tags = ["device-install", "device-id", "retroactive", "policy"],
            Description =
                "Sets DenyDeviceIDsRetroactive=1 in the DeviceInstall Restrictions policy. "
                + "Extends the hardware device ID block list to affect devices that were already installed "
                + "before the policy was applied. Without this, only new device installations are blocked. "
                + "Retroactive blocking disables already-installed matched devices.",
            SideEffects = "Can disable currently working devices that match the device ID block list.",
            ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceIDsRetroactive", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceIDsRetroactive")],
            DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceIDsRetroactive", 1)],
        },
        new TweakDef
        {
            Id = "dinst-retroactive-class-block",
            Label = "Device Install Policy: Apply Class GUID Blocks Retroactively",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Tags = ["device-install", "class-guid", "retroactive", "policy"],
            Description =
                "Sets DenyDeviceClassesRetroactive=1 in the DeviceInstall Restrictions policy. "
                + "Extends the class GUID block list to also disable devices already installed before the "
                + "policy was enforced. Combined with 'dinst-enable-class-block' to fully restrict an entire "
                + "device class including previously installed instances.",
            SideEffects = "Can disable currently working devices in blocked setup classes.",
            ApplyOps = [RegOp.SetDword(Restrictions, "DenyDeviceClassesRetroactive", 1)],
            RemoveOps = [RegOp.DeleteValue(Restrictions, "DenyDeviceClassesRetroactive")],
            DetectOps = [RegOp.CheckDword(Restrictions, "DenyDeviceClassesRetroactive", 1)],
        },
        new TweakDef
        {
            Id = "dinst-disable-driver-web-search",
            Label = "Device Install Policy: Disable Driver Search via Windows Update",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["device-install", "driver", "windows-update", "network", "policy"],
            Description =
                "Sets SearchOrderConfig=0 in the DriverSearching policy key. "
                + "Prevents Windows from searching Windows Update to locate and download device drivers. "
                + "Requires administrators to pre-stage drivers or use WSUS/SCCM for driver distribution. "
                + "Prevents unknown or unvetted drivers from being automatically pulled from the internet.",
            ApplyOps = [RegOp.SetDword(DriverSearching, "SearchOrderConfig", 0)],
            RemoveOps = [RegOp.DeleteValue(DriverSearching, "SearchOrderConfig")],
            DetectOps = [RegOp.CheckDword(DriverSearching, "SearchOrderConfig", 0)],
        },
        new TweakDef
        {
            Id = "dinst-disable-co-installers",
            Label = "Device Install Policy: Disable Third-Party Co-Installer Loading",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 3,
            SafetyRating = 3,
            Tags = ["device-install", "co-installer", "driver", "security"],
            SideEffects = "Some hardware drivers (e.g., printers, audio interfaces) require co-installers for full functionality.",
            Description =
                "Sets DisableCoInstallers=1 in the Device Installer key. "
                + "Blocks device co-installers — DLLs registered by driver packages to run additional code "
                + "during device setup. Co-installers are a common attack vector: malicious or vulnerable "
                + "co-installers can escalate privileges or install persistent malware during device installation.",
            ApplyOps = [RegOp.SetDword(DeviceInstaller, "DisableCoInstallers", 1)],
            RemoveOps = [RegOp.DeleteValue(DeviceInstaller, "DisableCoInstallers")],
            DetectOps = [RegOp.CheckDword(DeviceInstaller, "DisableCoInstallers", 1)],
        },
        new TweakDef
        {
            Id = "dinst-disable-wer-missing-driver",
            Label = "Device Install Policy: Disable WER Reports for Missing Drivers",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["device-install", "wer", "error-reporting", "privacy"],
            Description =
                "Sets DisableSendGenericDriverNotFoundToWER=1 in the DeviceInstall Settings policy. "
                + "Prevents Windows Error Reporting from sending problem reports when a device driver is not "
                + "found during Plug and Play device detection. "
                + "Reduces unsolicited telemetry uploads to Microsoft servers.",
            ApplyOps = [RegOp.SetDword(Settings, "DisableSendGenericDriverNotFoundToWER", 1)],
            RemoveOps = [RegOp.DeleteValue(Settings, "DisableSendGenericDriverNotFoundToWER")],
            DetectOps = [RegOp.CheckDword(Settings, "DisableSendGenericDriverNotFoundToWER", 1)],
        },
        new TweakDef
        {
            Id = "dinst-block-device-metadata-internet",
            Label = "Device Install Policy: Block Device Metadata Downloads from the Internet",
            Category = "Device Installation Policies",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["device-install", "metadata", "privacy", "network", "policy"],
            Description =
                "Sets PreventDeviceMetadataFromNetwork=1 in the Device Metadata policy key. "
                + "Prevents Windows Device Stage from downloading device metadata (icons, descriptions, "
                + "software links) from the Windows Metadata and Internet Services (WMIS) server. "
                + "Stops unnecessary network connections to Microsoft servers triggered by new device insertion.",
            ApplyOps = [RegOp.SetDword(DeviceMetadata, "PreventDeviceMetadataFromNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(DeviceMetadata, "PreventDeviceMetadataFromNetwork")],
            DetectOps = [RegOp.CheckDword(DeviceMetadata, "PreventDeviceMetadataFromNetwork", 1)],
        },
    ];
}
