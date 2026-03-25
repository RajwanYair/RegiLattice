// RegiLattice.Core — Tweaks/PortableDevicesPolicy.cs
// Sprint 301: Portable Devices Policy tweaks (10 tweaks)
// Category: "Portable Devices Policy" | Slug: portdev
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableDevices

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class PortableDevicesPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableDevices";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "portdev-disable-autoplay",
            Label = "Disable AutoPlay for Portable Devices",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "AutoPlay automatically launches program content from portable devices like cameras, phones, and media players when they are connected. Disabling AutoPlay for portable devices prevents any automatic execution or media presentation when a portable device is connected. AutoPlay has historically been a vector for malware distribution through infected portable media. Even without AutoRun execution, AutoPlay dialog prompts can trigger user-initiated malware installation through social engineering. Disabling AutoPlay is a foundational security hardening step recommended by security benchmarks including CIS controls. Users requiring access to portable device content can browse it manually through File Explorer.",
            Tags = ["portable-devices", "autoplay", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoPlay", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoPlay")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoPlay", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-sync",
            Label = "Disable Portable Device Sync",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Portable device sync transfers content between Windows and MTP/PTP compliant devices such as smartphones and cameras. Disabling portable device sync prevents automatic data synchronization when portable devices are connected to managed endpoints. Sync operations can transfer corporate data from managed endpoints to unmanaged portable devices creating data leakage risks. Conversely sync can introduce malicious files from personal portable devices into the corporate endpoint filesystem. Enterprise DLP controls for portable devices should include both read and write restriction capabilities. This policy does not prevent USB charging but does prevent file system access through MTP/PTP protocol.",
            Tags = ["portable-devices", "sync", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceSync", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-media-acquisition",
            Label = "Disable Portable Device Media Acquisition",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Media acquisition allows Windows to automatically import photos, videos, and audio from cameras and other MTP devices connected to the system. Disabling media acquisition prevents automatic media transfer from portable devices including smartphones and digital cameras. Automatic media import can inadvertently transfer confidential photos or videos to the enterprise endpoint from personal devices. Enterprise endpoints used by regulated industry workers should not auto-import media from unmanaged devices. Media acquired from personal devices may contain content that violates enterprise acceptable use policies. Disabling media acquisition requires users to manually manage any legitimate data transfer from authorized portable devices.",
            Tags = ["portable-devices", "media", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMediaAcquisition", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaAcquisition")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMediaAcquisition", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-wpdautorun",
            Label = "Disable Windows Portable Device AutoRun",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Windows Portable Device AutoRun allows programs on portable devices to automatically execute when the device is connected. Disabling WPD AutoRun prevents any executable content on portable devices from running automatically upon connection. AutoRun-based malware has been one of the most prevalent endpoint compromise mechanisms throughout computing history. Even modern Windows systems with AutoRun disabled may still be vulnerable to device-triggered execution through driver-level attack paths. Disabling WPD AutoRun removes this entire class of automatic execution vectors from portable device connections. This setting is a prerequisite for any enterprise environment running security baseline configurations.",
            Tags = ["portable-devices", "autorun", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWpdAutoRun", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWpdAutoRun")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWpdAutoRun", 1)],
        },
        new TweakDef
        {
            Id = "portdev-deny-read",
            Label = "Deny Read Access to Portable Devices",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Portable device read access controls whether Windows allows users to read files from MTP and WPD devices connected to the system. Denying read access prevents users from copying data from portable devices to the managed endpoint. This control is used in high-security environments where no data should flow from unmanaged devices to managed endpoints. Even when read is denied, charging via USB is not blocked by this policy setting. This setting is most effective when combined with write denial to create a full bidirectional portable device data isolation policy. Organizations deploying this control should communicate the restriction to users and provide approved data transfer procedures.",
            Tags = ["portable-devices", "read-access", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DenyPortableDeviceRead", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDeviceRead")],
            DetectOps = [RegOp.CheckDword(Key, "DenyPortableDeviceRead", 1)],
        },
        new TweakDef
        {
            Id = "portdev-deny-write",
            Label = "Deny Write Access to Portable Devices",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Portable device write access controls whether Windows allows users to transfer files to MTP and WPD devices connected to the system. Denying write access prevents users from copying data from the managed endpoint to portable devices. This is a primary data loss prevention control for organizations at risk of intentional or inadvertent data exfiltration via portable storage. Corporate documents, database exports, source code, and other sensitive data must not be movable to unmanaged personal devices. Write denial is more critical than read denial from a DLP perspective since outbound data movement represents the primary exfiltration vector. Organizations should pair this policy with removable storage write denial for comprehensive portable device data control.",
            Tags = ["portable-devices", "write-access", "dlp", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DenyPortableDeviceWrite", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDeviceWrite")],
            DetectOps = [RegOp.CheckDword(Key, "DenyPortableDeviceWrite", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-camera-access",
            Label = "Disable Camera Portable Device Access",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Camera portable device access allows Windows to connect to digital cameras and import photos through the Windows Portable Device framework. Disabling camera access prevents Windows from enumerating and accessing digital cameras connected via USB or wireless protocols. In secure facility environments, personal cameras and recording devices are commonly prohibited to prevent capture of sensitive displays or infrastructure. Disabling camera device access provides a software enforcement layer complementary to physical camera possession policies. Enterprise endpoints in secure rooms and data centers should not be connectable to camera devices. This policy applies to the WPD camera device class and does not affect integrated webcam functionality.",
            Tags = ["portable-devices", "camera", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCameraDeviceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCameraDeviceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCameraDeviceAccess", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-wpd-driver-install",
            Label = "Disable WPD Driver Auto-Installation",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Windows Portable Device driver installation automatically installs WPD drivers when new portable devices are discovered for the first time. Disabling WPD driver auto-installation prevents new portable device drivers from being loaded when unrecognized devices are connected. Without approved drivers, unrecognized portable devices cannot access the WPD/MTP protocol stack and file access is prevented. This prevents unknown portable devices from becoming functional even if connected to the endpoint. Approved device drivers for authorized portable devices should be deployed through managed software distribution. Combining driver installation control with explicit device ID policies creates a comprehensive portable device access control framework.",
            Tags = ["portable-devices", "drivers", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWpdDriverAutoInstall", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWpdDriverAutoInstall")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWpdDriverAutoInstall", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-wpd-notification",
            Label = "Disable Portable Device Connection Notifications",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Windows displays toast notifications and AutoPlay prompts when portable devices are connected to the system. Disabling portable device connection notifications suppresses the pop-up dialogs and notification center alerts triggered by device connection. Notification suppression prevents user interaction with connection prompts that could lead to inadvertent data transfer. Eliminating connection prompts reduces distraction and improves the user experience on shared or kiosk endpoints. Notification suppression is a non-essential cosmetic enhancement that does not affect the underlying device access block policies. Blocking notifications does not independently prevent device access and should be deployed alongside substantive access control policies.",
            Tags = ["portable-devices", "notifications", "usability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceNotifications", 1)],
        },
        new TweakDef
        {
            Id = "portdev-disable-telemetry",
            Label = "Disable Portable Device Telemetry",
            Category = "Portable Devices Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Portable device telemetry collects data about device types, connection frequency, and transfer statistics when portable devices are connected. This telemetry is used to improve Windows compatibility with a wide range of portable devices and optimize the connection experience. Disabling this telemetry prevents information about connected portable devices from being reported to Microsoft. Device type and model information sent through telemetry reveals what personal devices are connected to enterprise endpoints. Asset and inventory information about personal devices is not appropriate for external data collection in enterprise environments. Portable device functionality is entirely unaffected by disabling this telemetry stream.",
            Tags = ["portable-devices", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePortableDeviceTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePortableDeviceTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePortableDeviceTelemetry", 1)],
        },
    ];
}
