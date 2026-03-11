namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UsbPeripherals
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "usb-disable-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevent USB devices from entering low-power suspend. Fixes disconnect issues. Default: enabled. Recommended: disabled.",
            Tags = ["usb", "suspend", "power", "disconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
        },
        new TweakDef
        {
            Id = "usb-disable-autoplay",
            Label = "Disable AutoPlay (User)",
            Category = "USB & Peripherals",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disable automatic playback dialog when inserting media. Default: enabled. Recommended: disabled.",
            Tags = ["autoplay", "media", "usb"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-autoplay-policy",
            Label = "Disable AutoPlay (Machine Policy)",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Machine-wide policy: disable AutoPlay for all drive types. Default: partial (145). Recommended: full disable (255).",
            Tags = ["autoplay", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-mass-storage",
            Label = "Disable USB Mass Storage Driver",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevent USB flash drives from being mounted. Security hardening. Default: enabled (Start=3).",
            Tags = ["usb", "storage", "security", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR"],
        },
        new TweakDef
        {
            Id = "usb-deny-removable-write",
            Label = "Deny Write to Removable Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block writing to removable storage devices via policy. Default: allowed.",
            Tags = ["removable", "write", "policy", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usb-deny-removable-read",
            Label = "Deny Read from Removable Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block reading from removable storage devices via policy. Default: allowed.",
            Tags = ["removable", "read", "policy", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "usb-deny-removable-execute",
            Label = "Deny Execute from Removable Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block execution of programs from removable storage. Default: allowed. Recommended: enabled for security.",
            Tags = ["removable", "execute", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Execute", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Execute"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Execute", 1)],
        },
        new TweakDef
        {
            Id = "usb-deny-wpd-access",
            Label = "Deny WPD (Portable Device) Access",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block read/write to Windows Portable Devices (phones, cameras). Default: allowed.",
            Tags = ["wpd", "portable", "phone", "camera", "mtp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Read", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Write", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Read"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Write"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-all-removable",
            Label = "Deny All Removable Storage",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block all removable storage device classes. Maximum lockdown. Default: allowed.",
            Tags = ["removable", "all", "block", "lockdown"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-autorun",
            Label = "Disable AutoRun for All Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable AutoRun.inf processing for all drive types. Prevents malware auto-execution. Default: enabled. Recommended: disabled.",
            Tags = ["autorun", "security", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "usb-force-safe-removal",
            Label = "Force Safe Removal Notification",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Always show 'Safe to Remove Hardware' notification. Default: conditional.",
            Tags = ["safe-remove", "eject", "notification"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoplayHandlers"],
        },
        new TweakDef
        {
            Id = "usb-disable-usb3-power-save",
            Label = "Disable USB 3.0 Link Power Management",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables USB 3.0 enhanced power management / link power management. Improves USB stability at cost of power. Default: Enabled. Recommended: Disabled for desktops.",
            Tags = ["usb", "usb3", "power", "lpm", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
        },
        new TweakDef
        {
            Id = "usb-disable-removable-write",
            Label = "Disable Write to Removable Storage",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables write protection on all removable storage devices. Prevents data exfiltration via USB drives. Default: Disabled. Recommended: Enabled for secure envs.",
            Tags = ["usb", "removable", "write-protect", "security", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"],
        },
        new TweakDef
        {
            Id = "usb-disable-hub-power-saving",
            Label = "Disable USB Hub Power Saving",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables selective suspend on USB hubs to prevent device disconnects. Default: Enabled. Recommended: Disabled for desktops.",
            Tags = ["usb", "hub", "power", "suspend", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\usbhub\HubG"],
        },
        new TweakDef
        {
            Id = "usb-turbo-transfer-mode",
            Label = "Set USB Transfer Mode to Turbo (Write Cache)",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables write caching on USB storage for faster transfers. Requires safe removal. Default: Disabled. Recommended: Enabled.",
            Tags = ["usb", "transfer", "turbo", "write-cache", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR"],
        },
        new TweakDef
        {
            Id = "usb-disable-legacy-warning",
            Label = "Disable USB Legacy Support Warning",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses USB legacy compatibility warnings in the system tray. Default: Shown. Recommended: Hidden.",
            Tags = ["usb", "legacy", "warning", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 1)],
        },
    ];
}
