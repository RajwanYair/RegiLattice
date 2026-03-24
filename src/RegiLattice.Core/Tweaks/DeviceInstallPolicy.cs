#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class DeviceInstallPolicy
{
    private const string DiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";
    private const string DiRestrictKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "devinstall-deny-unspecified-devices",
            Label = "Deny Installation of Unlisted Device Classes",
            Category = "Device Install Policy",
            Description = "Prevents Windows from installing devices whose class is not explicitly permitted by device installation policy.",
            Tags = ["device-install", "device-class", "restriction", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Blocks any device not on an allow-list; aggressive setting — combine with device class allow-lists.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyUnspecified", 1)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyUnspecified")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyUnspecified", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-deny-removable-devices",
            Label = "Deny Installation of Removable Storage Devices",
            Category = "Device Install Policy",
            Description = "Blocks Windows from installing USB drives, SD cards, and other removable storage devices.",
            Tags = ["device-install", "removable-storage", "usb", "restriction", "dlp", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Key DLP control; blocks USB exfiltration. Removable storage that was already installed still works.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyRemovableDevices", 1)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyRemovableDevices")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyRemovableDevices", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-no-admin-override",
            Label = "Prevent Admins from Overriding Device Installation Restrictions",
            Category = "Device Install Policy",
            Description = "Removes the administrator privilege that normally allows bypassing device installation policy restrictions.",
            Tags = ["device-install", "admin", "restriction", "override", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Even local admins cannot install blocked device classes; requires GPO change to allow an exception.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "AllowAdminInstall", 0)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "AllowAdminInstall")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "AllowAdminInstall", 0)],
        },
        new TweakDef
        {
            Id = "devinstall-enable-setup-logging",
            Label = "Enable Verbose Device Installation Event Logging",
            Category = "Device Install Policy",
            Description = "Enables detailed event logging in the Windows device installation subsystem for auditing and diagnostics.",
            Tags = ["device-install", "logging", "audit", "events", "monitoring"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Enables verbose installation logs in the Windows Device Setup event channel; minimal performance impact.",
            ApplyOps = [RegOp.SetDword(DiKey, "EnableSetupSystemRestoreCheckpoints", 1)],
            RemoveOps = [RegOp.DeleteValue(DiKey, "EnableSetupSystemRestoreCheckpoints")],
            DetectOps = [RegOp.CheckDword(DiKey, "EnableSetupSystemRestoreCheckpoints", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-disable-driver-search-online",
            Label = "Disable Online Driver Search During Device Install",
            Category = "Device Install Policy",
            Description = "Prevents Windows from searching the Internet (Windows Update) for drivers during device installation.",
            Tags = ["device-install", "driver", "windows-update", "online", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks unsigned or unvetted driver downloads from WU; IT manages and deploys approved drivers.",
            ApplyOps = [RegOp.SetDword(DiKey, "SearchOrderConfig", 1)],
            RemoveOps = [RegOp.DeleteValue(DiKey, "SearchOrderConfig")],
            DetectOps = [RegOp.CheckDword(DiKey, "SearchOrderConfig", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-no-driver-store-from-wer",
            Label = "Disable WER-Triggered Driver Package Downloads",
            Category = "Device Install Policy",
            Description = "Prevents Windows Error Reporting from triggering automatic driver package downloads from the Internet.",
            Tags = ["device-install", "wer", "driver", "download", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Closes a secondary driver download path triggered by crash events; driver management stays in IT control.",
            ApplyOps = [RegOp.SetDword(DiKey, "AllowUserPnP", 0)],
            RemoveOps = [RegOp.DeleteValue(DiKey, "AllowUserPnP")],
            DetectOps = [RegOp.CheckDword(DiKey, "AllowUserPnP", 0)],
        },
        new TweakDef
        {
            Id = "devinstall-block-legacy-ieee1394",
            Label = "Restrict IEEE 1394 (FireWire) Device Installation",
            Category = "Device Install Policy",
            Description = "Blocks installation of IEEE 1394 (FireWire) bus controllers, which support DMA and can bypass OS memory protection.",
            Tags = ["device-install", "firewire", "ieee1394", "dma", "security", "hardware"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "FireWire DMA attacks allow direct memory access bypassing the OS; only impacts systems with legacy ports.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyDeviceIDs", 1)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyDeviceIDs")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyDeviceIDs", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-create-restore-point-on-install",
            Label = "Create System Restore Point During Driver Installation",
            Category = "Device Install Policy",
            Description = "Forces Windows to create a system restore point before installing any new device driver, enabling rollback.",
            Tags = ["device-install", "driver", "restore-point", "rollback", "safety"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Restore point created before each driver install; enables quick recovery from bad driver installations.",
            ApplyOps = [RegOp.SetDword(DiKey, "DisableSystemRestore", 0)],
            RemoveOps = [RegOp.DeleteValue(DiKey, "DisableSystemRestore")],
            DetectOps = [RegOp.CheckDword(DiKey, "DisableSystemRestore", 0)],
        },
        new TweakDef
        {
            Id = "devinstall-disable-drivers-from-cd",
            Label = "Disable Driver Installation from Optical Media",
            Category = "Device Install Policy",
            Description = "Prevents Windows from using drivers stored on removable optical media (CD/DVD) during device installation.",
            Tags = ["device-install", "cd", "dvd", "optical", "driver", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks CD/DVD as a driver source; relevant on systems with optical drives that accept physical media.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "DenyRemovableDevicesRetroactive", 1)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "DenyRemovableDevicesRetroactive")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "DenyRemovableDevicesRetroactive", 1)],
        },
        new TweakDef
        {
            Id = "devinstall-notify-admin-on-block",
            Label = "Notify Admins When Device Installation Is Blocked",
            Category = "Device Install Policy",
            Description = "Sends a notification to administrators when a device installation attempt is blocked by policy.",
            Tags = ["device-install", "notification", "admin", "audit", "monitoring"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Generates a Windows event log entry when device install is denied; helps with security monitoring.",
            ApplyOps = [RegOp.SetDword(DiRestrictKey, "AlertOnDeviceInstallation", 1)],
            RemoveOps = [RegOp.DeleteValue(DiRestrictKey, "AlertOnDeviceInstallation")],
            DetectOps = [RegOp.CheckDword(DiRestrictKey, "AlertOnDeviceInstallation", 1)],
        },
    ];
}
