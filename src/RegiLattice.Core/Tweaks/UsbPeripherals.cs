namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UsbPeripherals
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
        new TweakDef
        {
            Id = "usb-disable-usb-storage-write",
            Label = "Disable USB Storage Write Access",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Makes all USB storage devices read-only. Prevents data from being written to USB drives. Default: read-write.",
            Tags = ["usb", "storage", "write-protect", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-usb-storage-install",
            Label = "Block USB Storage Device Installation",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks installation of new USB storage devices. Existing drives still work. Default: allowed.",
            Tags = ["usb", "storage", "block", "install", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsbStor"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsbStor", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsbStor", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\UsbStor", "Start", 4)],
        },
        new TweakDef
        {
            Id = "usb-enable-enhanced-power-management",
            Label = "Enable USB Enhanced Power Management",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables enhanced power management for USB hub devices. Saves power by suspending idle USB ports. Default: enabled.",
            Tags = ["usb", "power", "management", "suspend"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "EnhancedPowerManagementEnabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "EnhancedPowerManagementEnabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "EnhancedPowerManagementEnabled", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-bluetooth-radio",
            Label = "Disable Bluetooth Radio via USB",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth radio by setting the Bluetooth Support service to disabled. Default: auto.",
            Tags = ["usb", "bluetooth", "radio", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 4)],
        },
        new TweakDef
        {
            Id = "usb-disable-mass-storage-driver",
            Label = "Disable USB Mass Storage Driver",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the USB mass storage driver. Blocks access to USB flash drives and external disks. Security hardening measure. Default: enabled.",
            Tags = ["usb", "mass-storage", "block", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4)],
        },
        new TweakDef
        {
            Id = "usb-disable-hub-power-saving",
            Label = "Disable USB Hub Power Saving",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables power saving on USB hub controllers. Prevents USB devices from disconnecting due to power management. Default: enabled.",
            Tags = ["usb", "hub", "power", "disconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-mass-storage",
            Label = "Disable USB Mass Storage (Policy)",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks USB mass storage devices via policy. Prevents unauthorised data transfer via USB drives. Default: allowed.",
            Tags = ["usb", "mass-storage", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Read", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-removable-write",
            Label = "Disable Removable Storage Write",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks write access to removable storage devices. Prevents data exfiltration via USB drives while allowing read access. Default: allowed.",
            Tags = ["usb", "removable", "write", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}", "Deny_Write", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables USB selective suspend globally. Prevents USB devices from entering low-power state. Fixes intermittent device disconnects. Default: enabled.",
            Tags = ["usb", "selective-suspend", "power", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBHUB3", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-usb3-power-save",
            Label = "Disable USB 3.0 Power Saving",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables USB 3.0 link power management. Prevents U1/U2 power states that can cause latency or disconnects. Default: enabled.",
            Tags = ["usb", "usb3", "power", "link-state"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1", 0)],
        },
        new TweakDef
        {
            Id = "usb-force-safe-removal",
            Label = "Force USB Safe Removal Default",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets all removable USB storage to 'Better performance' mode requiring safe removal. Enables write caching on USB drives. Default: quick removal.",
            Tags = ["usb", "safe-removal", "write-cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 0)],
        },
        new TweakDef
        {
            Id = "usb-turbo-transfer-mode",
            Label = "Enable USB Turbo Transfer Mode",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables USB turbo/boost transfer mode for faster large file copies. Increases buffer sizes. Default: standard.",
            Tags = ["usb", "turbo", "transfer", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled", 0)],
        },
    ];
}
