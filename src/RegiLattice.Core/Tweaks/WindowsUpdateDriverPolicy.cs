// RegiLattice.Core — Tweaks/WindowsUpdateDriverPolicy.cs
// Device driver installation restriction and signing policy (Sprint 599).
// Category: "WU Driver Update Policy" | Slug: wudrv
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions
// All tweaks are CorpSafe = true (Group Policy path).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsUpdateDriverPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";
    private const string SignKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Driver Signing";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "wudrv-deny-unidentified-device-installation",
            Label = "WU Driver: Block Installation of Unidentified Device Drivers",
            Category = "WU Driver Update Policy",
            Description = "Sets DenyUnidentifiedDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from installing drivers for hardware devices that are not in the Windows Driver Store and do not have a matching entry in Windows Update. " +
                "Unidentified devices are a common attack vector — malicious USB devices can present as unknown hardware that auto-installs a malicious driver. This policy requires all devices to have a recognized driver before they can function.",
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
            Category = "WU Driver Update Policy",
            Description = "Sets DenyRemovableDeviceInstallation=1 in DeviceInstall\\Restrictions policy. Prevents Windows from automatically installing drivers for any removable device. " +
                "Removable devices (USB storage, USB hubs, card readers, portable audio devices) are frequently connected in enterprise environments. Without this policy, each new removable device triggers an automatic driver installation from WU, bypassing IT-managed driver sets and potentially installing unsigned or vulnerable drivers.",
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
            Category = "WU Driver Update Policy",
            Description = "Sets BehaviorOnFailedVerify=2 in Driver Signing policy. Configures Windows to silently block the installation of any device driver that fails digital signature verification. " +
                "Value 2 = Block (value 1 = Warn, value 0 = Ignore). Blocking unsigned drivers prevents rootkits and malicious kernel-mode code from loading under the guise of a hardware driver. This is a critical defence-in-depth control alongside Secure Boot and HVCI.",
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
            Id = "wudrv-prevent-device-class-installations",
            Label = "WU Driver: Enable Device Class Installation Restriction Policy",
            Category = "WU Driver Update Policy",
            Description = "Sets DenyDeviceClasses=1 in DeviceInstall\\Restrictions policy. Activates the device class restriction feature that, when combined with a list of blocked device class GUIDs, prevents installation of entire categories of devices. " +
                "This policy enables the enforcement of device class blocklists (e.g., blocking all Bluetooth adapters, all wireless adapters, or all imaging devices) across the enterprise without per-device ID management.",
            Tags = ["driver", "device-class", "restriction", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates device class restriction framework; prerequisite for GUID-based device category blocklists.",
            ApplyOps = [RegOp.SetDword(Key, "DenyDeviceClasses", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceClasses")],
            DetectOps = [RegOp.CheckDword(Key, "DenyDeviceClasses", 1)],
        },
        new TweakDef
        {
            Id = "wudrv-enable-device-id-restriction-policy",
            Label = "WU Driver: Enable Device ID-Based Installation Restriction",
            Category = "WU Driver Update Policy",
            Description = "Sets DenyDeviceIDs=1 in DeviceInstall\\Restrictions policy. Activates the device ID restriction feature. When enabled, Windows checks all device hardware IDs against a configured deny list. " +
                "Device ID restrictions are more granular than class restrictions and allow blocking specific problematic hardware models (e.g., a specific USB key brand with a known firmware vulnerability) while permitting similar hardware from other vendors.",
            Tags = ["driver", "device-id", "restriction", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates device ID restriction; enables HWID-based device blocklists for targeted hardware exclusions.",
            ApplyOps = [RegOp.SetDword(Key, "DenyDeviceIDs", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyDeviceIDs")],
            DetectOps = [RegOp.CheckDword(Key, "DenyDeviceIDs", 1)],
        },
        new TweakDef
        {
            Id = "wudrv-log-driver-install-restriction-events",
            Label = "WU Driver: Enable Event Logging for Blocked Driver Installations",
            Category = "WU Driver Update Policy",
            Description = "Sets WritePolicy=1 in DeviceInstall\\Restrictions policy. Enables Windows to write an event log entry whenever a device installation is blocked by Device Installation Policy. " +
                "Without this, blocked installations fail silently, making it impossible to audit what hardware was attempted and blocked. With logging enabled, security teams can monitor for repeated installation attempts which may indicate hardware-based persistence attempts.",
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
            Category = "WU Driver Update Policy",
            Description = "Sets DisableDriverLookup=1 in DeviceInstall\\Restrictions policy. Prevents Windows from looking up driver information and uploading crash data to the Microsoft Windows Error Reporting service when a device driver causes an error. " +
                "In regulated environments, data sovereignty requirements may prohibit telemetry of driver crash details (device type, hardware ID, crash context) from being transmitted to Microsoft's cloud infrastructure.",
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
            Category = "WU Driver Update Policy",
            Description = "Sets PreventInstallationOfDevicesNotDescribedByOtherPolicySettings=1 in DeviceInstall\\Restrictions policy. Sets a default-deny posture for device installation: only devices explicitly permitted by an allowlist policy are installed. All others are blocked. " +
                "This inverts the default Windows behaviour (allow-by-default) into a deny-by-default stance that requires active IT involvement to introduce any new device type into the environment.",
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
            Category = "WU Driver Update Policy",
            Description = "Sets PreventDeviceMetadataFromNetwork=1 in DeviceInstall policy. Prevents Windows from searching the Windows Update network service for device metadata (device icons, model pages, UWP companion apps). " +
                "Device metadata retrieval can prompt automatic download of companion apps without explicit user action. In locked-down environments, all device metadata should be pre-staged via WSUS rather than retrieved on-demand from Microsoft servers.",
            Tags = ["driver", "metadata", "privacy", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks network-sourced device metadata; prevents unsolicited companion app downloads on device connection.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings", "PreventDeviceMetadataFromNetwork", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings", "PreventDeviceMetadataFromNetwork")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings", "PreventDeviceMetadataFromNetwork", 1)],
        },
        new TweakDef
        {
            Id = "wudrv-allow-admin-override-device-restriction",
            Label = "WU Driver: Allow Administrators to Override Device Installation Policy",
            Category = "WU Driver Update Policy",
            Description = "Sets AllowAdminInstall=1 in DeviceInstall\\Restrictions policy. When device installation restrictions are in effect (including deny-by-default), this allows users in the local Administrators group to install any device regardless of policy restrictions. " +
                "This maintains an escape hatch for IT staff to provision new hardware on managed endpoints without requiring a Group Policy update cycle, while standard users remain restricted.",
            Tags = ["driver", "admin", "override", "device", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Allows admins to bypass device installation restrictions; provides IT escape hatch without weakening user-level controls.",
            ApplyOps = [RegOp.SetDword(Key, "AllowAdminInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAdminInstall")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAdminInstall", 1)],
        },
    ];
}
