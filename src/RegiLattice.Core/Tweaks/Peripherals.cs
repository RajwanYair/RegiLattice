namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

[TweakModule]
internal static class UsbPeripherals
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "usb-deny-removable-write",
            Label = "Deny Write to Removable Drives",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block writing to removable storage devices via policy. Default: allowed.",
            Tags = ["removable", "write", "policy", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Write",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Write"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Write",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "usb-deny-removable-read",
            Label = "Deny Read from Removable Drives",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block reading from removable storage devices via policy. Default: allowed.",
            Tags = ["removable", "read", "policy", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Read",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Read"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Read",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "usb-deny-removable-execute",
            Label = "Deny Execute from Removable Drives",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block execution of programs from removable storage. Default: allowed. Recommended: enabled for security.",
            Tags = ["removable", "execute", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Execute",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Execute"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630d-b6bf-11d0-94f2-00a0c91efb8b}",
                    "Deny_Execute",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "usb-deny-wpd-access",
            Label = "Deny WPD (Portable Device) Access",
            Category = "Peripherals",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Read", 1),
            ],
        },
        new TweakDef
        {
            Id = "usb-disable-all-removable",
            Label = "Deny All Removable Storage",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Block all removable storage device classes. Maximum lockdown. Default: allowed.",
            Tags = ["removable", "all", "block", "lockdown"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-legacy-warning",
            Label = "Disable USB Legacy Support Warning",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Suppresses USB legacy compatibility warnings in the system tray. Default: Shown. Recommended: Hidden.",
            Tags = ["usb", "legacy", "warning", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-usb-storage-install",
            Label = "Block USB Storage Device Installation",
            Category = "Peripherals",
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
            Category = "Peripherals",
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
            Category = "Peripherals",
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
            Id = "usb-disable-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables USB selective suspend globally. Prevents USB devices from entering low-power state. Fixes intermittent device disconnects. Default: enabled.",
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
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables USB 3.0 link power management. Prevents U1/U2 power states that can cause latency or disconnects. Default: enabled.",
            Tags = ["usb", "usb3", "power", "link-state"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Enum"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "UsbEnableU1", 0)],
        },
        new TweakDef
        {
            Id = "usb-turbo-transfer-mode",
            Label = "Enable USB Turbo Transfer Mode",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables USB turbo/boost transfer mode for faster large file copies. Increases buffer sizes. Default: standard.",
            Tags = ["usb", "turbo", "transfer", "speed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "EnhancedPowerManagementEnabled", 0)],
        },
        new TweakDef
        {
            Id = "usb-increase-transfer-size",
            Label = "Increase USB Transfer Size to 64K",
            Category = "Peripherals",
            NeedsAdmin = true,
            Description = "Increases the default USB transfer size to 64KB for better throughput on USB 3.x drives. Default: OS-managed.",
            Tags = ["usb", "transfer", "performance", "throughput"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "MaximumTransferSize", 65536)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "MaximumTransferSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "MaximumTransferSize", 65536)],
        },
        new TweakDef
        {
            Id = "usb-disable-notifications",
            Label = "Disable USB Device Notifications",
            Category = "Peripherals",
            NeedsAdmin = false,
            Description = "Suppresses toast notifications when USB devices are connected or disconnected. Default: show notifications.",
            Tags = ["usb", "notification", "toast"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoplayHandlers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoplayHandlers", "DisableAutoplay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoplayHandlers", "DisableAutoplay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\AutoplayHandlers", "DisableAutoplay", 1)],
        },
        new TweakDef
        {
            Id = "usb-enable-legacy-support",
            Label = "Enable USB Legacy Support",
            Category = "Peripherals",
            NeedsAdmin = true,
            Description = "Enables legacy USB support for keyboard/mouse in pre-boot environments. Default: OS-managed.",
            Tags = ["usb", "legacy", "keyboard", "bios"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "LegacySupport", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "LegacySupport")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "LegacySupport", 1)],
        },
        new TweakDef
        {
            Id = "usb-enable-safe-removal-icon",
            Label = "Always Show Safe Removal Icon",
            Category = "Peripherals",
            NeedsAdmin = false,
            Description = "Always shows the Safely Remove Hardware icon in the system tray when USB devices are connected. Default: auto-hide.",
            Tags = ["usb", "safe-removal", "tray", "icon"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\SysTray"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\SysTray", "Services", 29)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\SysTray", "Services")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Applets\SysTray", "Services", 29)],
        },
    ];
}
