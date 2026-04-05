namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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

// === Merged from: Bluetooth.cs ===

internal static class Bluetooth
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "bt-disable-bt-power-mgmt",
            Label = "Disable Bluetooth Power Management",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows from suspending the Bluetooth adapter to save power, reducing connection drops.",
            Tags = ["bluetooth", "power", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-discoverable",
            Label = "Disable Bluetooth Discoverability",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the Bluetooth adapter from being discoverable by nearby devices.",
            Tags = ["bluetooth", "security", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-handsfree",
            Label = "Disable Bluetooth Handsfree Profile",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth Handsfree Profile (HFP) service, preventing hands-free audio device connections.",
            Tags = ["bluetooth", "handsfree", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-obex",
            Label = "Disable Bluetooth OBEX File Transfer",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth OBEX service, blocking file transfer over Bluetooth to reduce attack surface.",
            Tags = ["bluetooth", "obex", "security", "file-transfer"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-pan",
            Label = "Disable Bluetooth PAN Networking",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth Personal Area Networking (PAN), preventing network sharing over Bluetooth.",
            Tags = ["bluetooth", "network", "pan", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-serial",
            Label = "Disable Bluetooth Serial Port (COM)",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth BTHMODEM serial port driver, preventing legacy COM-port connections over Bluetooth.",
            Tags = ["bluetooth", "serial", "com", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-a2dp-sink",
            Label = "Disable Bluetooth A2DP Sink (Receive Audio)",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth A2DP Sink service, preventing the PC from receiving audio streams over Bluetooth.",
            Tags = ["bluetooth", "a2dp", "audio", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-advertising",
            Label = "Disable Bluetooth LE Advertising",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Bluetooth Low Energy advertising. Prevents the PC from broadcasting BLE presence. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["bluetooth", "ble", "privacy", "advertising"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising", 0)],
        },
        new TweakDef
        {
            Id = "bt-audio-offload",
            Label = "Bluetooth Audio Offload",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Bluetooth A2DP sideband audio offloading to the adapter. Reduces CPU usage for Bluetooth audio streaming. Default: Disabled. Recommended: Enabled.",
            Tags = ["bluetooth", "audio", "performance", "offload"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-discovery",
            Label = "Disable Bluetooth LE Device Discovery",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Bluetooth Low Energy device discovery. Reduces power usage and limits BLE scanning surface. Default: Enabled. Recommended: Disabled if BLE not needed.",
            Tags = ["bluetooth", "ble", "discovery", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery", 1),
            ],
        },
        new TweakDef
        {
            Id = "bt-page-timeout",
            Label = "Set Bluetooth Page Timeout",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets Bluetooth page timeout to 16384 slots (~10 seconds). Increases time allowed for device connection establishment. Default: 8192 slots. Recommended: 16384 for reliability.",
            Tags = ["bluetooth", "page", "timeout", "connection", "reliability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 16384)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 16384)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-pre-pairing",
            Label = "Disable BLE Pre-Pairing",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth Low Energy pre-pairing. Prevents automatic pairing with nearby BLE devices. Default: enabled.",
            Tags = ["bluetooth", "ble", "pre-pairing", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "DisablePrePairing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "DisablePrePairing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "DisablePrePairing", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-swift-pair-notifications",
            Label = "Disable Swift Pair Notifications",
            Category = "Peripherals",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Swift Pair notification popups when new Bluetooth devices are detected nearby. Default: enabled.",
            Tags = ["bluetooth", "swift-pair", "notifications", "popup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickPair", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickPair")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickPair", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-remote-control-avrcp",
            Label = "Disable AVRCP Remote Control",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Bluetooth AVRCP (Audio/Video Remote Control) profile. Prevents media control via Bluetooth. Default: enabled.",
            Tags = ["bluetooth", "avrcp", "remote-control", "media"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-audio-service",
            Label = "Disable Bluetooth LE Audio Service",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Bluetooth Low Energy Audio service driver. Reduces BT resource usage. Default: enabled.",
            Tags = ["bluetooth", "le-audio", "service", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-device-association",
            Label = "Disable Bluetooth Device Association Service",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Device Association Service that auto-pairs Bluetooth devices. Manual pairing still works. Default: enabled.",
            Tags = ["bluetooth", "device", "association", "pairing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DeviceAssociationService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DeviceAssociationService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DeviceAssociationService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\DeviceAssociationService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-advertising",
            Label = "Disable Bluetooth Advertising",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Bluetooth advertising beacons. Prevents the device from broadcasting its presence to other Bluetooth devices. Default: enabled.",
            Tags = ["bluetooth", "advertising", "beacon", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowAdvertising"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowAdvertising", "value", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowAdvertising", "value")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowAdvertising", "value", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-audio-gateway",
            Label = "Disable Bluetooth Audio Gateway",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Audio Gateway service. Prevents Bluetooth hands-free/headset profile from running. Default: automatic.",
            Tags = ["bluetooth", "audio", "gateway", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-auto-pair",
            Label = "Disable Bluetooth Auto-Pairing",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic pairing with nearby Bluetooth devices. Requires manual pairing only. Default: auto-pair enabled.",
            Tags = ["bluetooth", "auto-pair", "security", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowPromptedProximalConnections"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowPromptedProximalConnections", "value", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowPromptedProximalConnections", "value"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Bluetooth\AllowPromptedProximalConnections",
                    "value",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "bt-disable-le-scan",
            Label = "Disable Bluetooth LE Scanning",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Bluetooth Low Energy background scanning. Reduces power consumption and prevents BLE device tracking. Default: enabled.",
            Tags = ["bluetooth", "ble", "scan", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableBluetoothLEAdScanning", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableBluetoothLEAdScanning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableBluetoothLEAdScanning", 0)],
        },
        new TweakDef
        {
            Id = "bt-low-latency",
            Label = "Enable Bluetooth Low Latency Mode",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures Bluetooth for lower latency connections. Improves responsiveness for gaming peripherals and real-time audio. Default: standard.",
            Tags = ["bluetooth", "latency", "gaming", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "SystemLocalFeatures", 63)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "SystemLocalFeatures")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "SystemLocalFeatures", 63)],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "bt-disable-bt-audio-router",
            Label = "Disable Bluetooth Audio Gateway Router",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Audio Gateway service used for hands-free audio routing. Reduces overhead when only A2DP audio is needed.",
            Tags = ["bluetooth", "audio", "gateway", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthhfAud"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthhfAud", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthhfAud", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthhfAud", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-enum-service",
            Label = "Disable Bluetooth Device Enumeration Service",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth user-mode device enumeration service. Reduces background scanning when Bluetooth devices are not actively paired.",
            Tags = ["bluetooth", "enumeration", "service", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-rfcomm",
            Label = "Disable Bluetooth RFCOMM Protocol",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Bluetooth RFCOMM driver used for serial port emulation. Breaks SPP-based connections (e.g., serial GPS, OBD-II).",
            Tags = ["bluetooth", "rfcomm", "serial", "protocol"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-set-inquiry-timeout",
            Label = "Reduce Bluetooth Inquiry Timeout",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Bluetooth inquiry scan timeout to a shorter window (5 seconds). Speeds up device discovery at the cost of finding slower-responding devices.",
            Tags = ["bluetooth", "inquiry", "timeout", "discovery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "InquiryTimeout", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "InquiryTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "InquiryTimeout", 5)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-hid-service",
            Label = "Disable Bluetooth HID Service",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Bluetooth Human Interface Device service. Breaks Bluetooth keyboards, mice, and game controllers.",
            Tags = ["bluetooth", "hid", "keyboard", "mouse"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidBth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidBth", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidBth", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\HidBth", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-amp-manager",
            Label = "Disable Bluetooth AMP Manager",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth Alternate MAC/PHY Manager protocol. AMP (802.11) is rarely used by consumer devices.",
            Tags = ["bluetooth", "amp", "protocol", "overhead"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAMPManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAMPManager", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAMPManager", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAMPManager", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-bt-connection-notifications",
            Label = "Disable Bluetooth Connection Notifications",
            Category = "Peripherals",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables toast notifications when Bluetooth devices connect or disconnect. Reduces notification noise.",
            Tags = ["bluetooth", "notifications", "toast", "connect"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Bluetooth", "ShowNotification", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Bluetooth", "ShowNotification")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Bluetooth", "ShowNotification", 0)],
        },
        new TweakDef
        {
            Id = "bt-set-selective-suspend",
            Label = "Enable Bluetooth Selective Suspend",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables selective suspend for the Bluetooth USB adapter. Allows the adapter to enter low-power state when idle, saving battery.",
            Tags = ["bluetooth", "selective-suspend", "usb", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "SelectiveSuspendEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "SelectiveSuspendEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "SelectiveSuspendEnabled", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-advertising-policy",
            Label = "Disable Bluetooth LE Advertising (Policy)",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Applies a Group Policy restriction to disable Bluetooth Low Energy peripheral advertisement scanning. Reduces background radio activity. Default: enabled.",
            Tags = ["bluetooth", "le", "advertising", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowAdvertising", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowAdvertising")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowAdvertising", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-avrcp-metadata",
            Label = "Disable Bluetooth AVRCP Metadata",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AVRCP metadata exchange (song title, artist) over Bluetooth. Slightly reduces audio latency. Default: enabled.",
            Tags = ["bluetooth", "avrcp", "audio", "metadata"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisablePlayerStateNAL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisablePlayerStateNAL")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisablePlayerStateNAL", 1),
            ],
        },
        new TweakDef
        {
            Id = "bt-disable-pairing-button",
            Label = "Disable Bluetooth Auto-Pair Button",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Bluetooth auto-pairing feature that allows pairing without a PIN. Improves security. Default: enabled.",
            Tags = ["bluetooth", "pairing", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPromptedProximalConnections", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPromptedProximalConnections")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPromptedProximalConnections", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-discoverable",
            Label = "Disable Bluetooth Discoverability",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents the system from being discoverable via Bluetooth by other devices. Improves privacy in public spaces. Default: enabled.",
            Tags = ["bluetooth", "discovery", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowDiscoverableMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowDiscoverableMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowDiscoverableMode", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-prepairing",
            Label = "Disable Bluetooth Pre-Pairing",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables pre-pairing of Microsoft Bluetooth peripherals. Prevents automatic silent pairing with Microsoft hardware. Default: enabled.",
            Tags = ["bluetooth", "pairing", "microsoft"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPrepairing", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPrepairing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowPrepairing", 0)],
        },
        new TweakDef
        {
            Id = "bt-set-power-management-strict",
            Label = "Enable Strict Bluetooth Power Management",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables strict power management for the Bluetooth radio. Reduces power consumption when idle. Default: normal power management.",
            Tags = ["bluetooth", "power", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "StrictIdleTimeout", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "StrictIdleTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHUSB\Parameters", "StrictIdleTimeout", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-swift-pair",
            Label = "Disable Bluetooth Swift Pair",
            Category = "Peripherals",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Swift Pair notifications for nearby Bluetooth devices. Prevents distracting pairing popups. Default: enabled.",
            Tags = ["bluetooth", "swift-pair", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickConnect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickConnect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth", "QuickConnect", 0)],
        },
        new TweakDef
        {
            Id = "bt-disable-bluetooth-hands-free",
            Label = "Disable Bluetooth Hands-Free Profile Service",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Hands-Free Profile (HFP) audio service. Audio stays on headset rather than switching to mono HFP mode. Default: enabled.",
            Tags = ["bluetooth", "audio", "hands-free", "hfp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFSrv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFSrv", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFSrv", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFSrv", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-file-send-policy",
            Label = "Disable Bluetooth File Transfer",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks Bluetooth file transfers via Group Policy (AllowFileSend=0). Prevents data exfiltration over Bluetooth. Default: file send allowed.",
            Tags = ["bluetooth", "file-transfer", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowFileSend", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowFileSend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowFileSend", 0)],
        },
        new TweakDef
        {
            Id = "bt-set-inquiry-length-reduced",
            Label = "Reduce Bluetooth Inquiry Scan Duration",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Reduces the Bluetooth device inquiry window from the default 12 seconds to 5 seconds. Speeds up device discovery while still finding nearby devices. Default: 12 seconds.",
            Tags = ["bluetooth", "discovery", "performance", "scan"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "InquiryLength", 5)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "InquiryLength", 12)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "InquiryLength", 5)],
        },
        new TweakDef
        {
            Id = "bt-disable-bluetooth-radio-policy",
            Label = "Disable Bluetooth Radio via Policy",
            Category = "Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Bluetooth entirely via Group Policy (AllowBluetooth=0). Prevents all Bluetooth hardware usage. Best for high-security workstations. Default: Bluetooth allowed.",
            Tags = ["bluetooth", "radio", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowBluetooth", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowBluetooth")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth", "AllowBluetooth", 0)],
        },
    ];
}

// ── merged from PolicyDevice.cs ──
// RegiLattice.Core — Tweaks/PolicyDevice.cs
// Device installation, enrollment, guard, firmware, hardware, portable devices, USB storage, and kernel DMA protection policies
// Category: "Device & Hardware Policy"
// Consolidated from 23 modules.

internal static class PolicyDevice
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _BluetoothAdvPolicy.Data,
            .. _DeviceCompliancePolicy.Data,
            .. _DeviceEnrollmentLimitPolicy.Data,
            .. _DeviceEnrollmentPolicy.Data,
            .. _DeviceGuardPolicy.Data,
            .. _DeviceGuardVbs.Data,
            .. _DeviceHealthCheckPolicy.Data,
            .. _DeviceInstallPolicies.Data,
            .. _DeviceInstallPolicy.Data,
            .. _DeviceLockGpoPolicy.Data,
            .. _DeviceProvisioningPolicy.Data,
            .. _DeviceRegistrationPolicy.Data,
            .. _FirmwareUpdatePolicy.Data,
            .. _HardwareDevicePolicy.Data,
            .. _KernelDmaProtectionPolicy.Data,
            .. _MemoryDiagnostics.Data,
            .. _PageFilePolicy.Data,
            .. _PortableDevicePolicy.Data,
            .. _PortableDevicesPolicy.Data,
            .. _ProcessorPolicy.Data,
            .. _SuperFetchSysmainPolicy.Data,
            .. _UsbStoragePolicy.Data,
            .. _VirtualDiskServicePolicy.Data,
        ];

    // ── BluetoothAdvPolicy ──
    private static class _BluetoothAdvPolicy
    {
        private const string BtPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth";
        private const string BthPort = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters";
        private const string BtHub = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BluetoothDeviceEnumerator";
        private const string BtPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\BlueTooth";
        private const string BtPhoneBook = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BlueTooth\PhoneBook";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-advertising",
                Label = "BT Advertising: Disable Bluetooth Advertising (BLE Beacon)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "advertising", "ble", "beacon", "privacy", "security"],
                Description =
                    "Sets DisableAdvertising=1 in Bluetooth policy. Stops the Bluetooth adapter from "
                    + "broadcasting advertising packets (BLE beacon). Prevents passive tracking and "
                    + "reduces RF attack surface. Default: advertising enabled.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableAdvertising", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableAdvertising")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableAdvertising", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-promiscuous-mode",
                Label = "BT Advertising: Disable Bluetooth Promiscuous Mode",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "promiscuous", "sniffing", "security", "hardening"],
                Description =
                    "Sets PromiscuousMode=0 in BTHPORT Parameters. Prevents the Bluetooth adapter from "
                    + "entering promiscuous receive mode which would capture all BT packets in range. "
                    + "Default: 0 (already off). Explicit enforcement ensures the value is never changed.",
                ApplyOps = [RegOp.SetDword(BthPort, "PromiscuousMode", 0)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "PromiscuousMode")],
                DetectOps = [RegOp.CheckDword(BthPort, "PromiscuousMode", 0)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-pairing-notification",
                Label = "BT Advertising: Disable Bluetooth Auto-Pairing Notifications",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "pairing", "notification", "lockdown", "security"],
                Description =
                    "Sets DisablePairingNotifications=1 in Bluetooth policy. Suppresses automatic "
                    + "pairing prompts when Bluetooth devices are discovered nearby. "
                    + "Default: notifications enabled. Prevents social engineering via proximity.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePairingNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePairingNotifications")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePairingNotifications", 1)],
            },
            new TweakDef
            {
                Id = "btadv-set-connectable-timeout-short",
                Label = "BT Advertising: Limit Bluetooth Discoverable/Connectable Timeout",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "discoverable", "timeout", "privacy", "security"],
                Description =
                    "Sets ConnectableTimeout=30 in BTHPORT Parameters. Limits how long the adapter "
                    + "remains in connectable mode after being made visible. "
                    + "Default: 180 seconds. Shorter window reduces passive attack exposure.",
                ApplyOps = [RegOp.SetDword(BthPort, "ConnectableTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "ConnectableTimeout")],
                DetectOps = [RegOp.CheckDword(BthPort, "ConnectableTimeout", 30)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-file-transfer",
                Label = "BT Advertising: Disable Bluetooth OBEX File Transfer",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "obex", "file-transfer", "security", "lockdown"],
                Description =
                    "Sets DisableFileTransfer=1 in Bluetooth policy. Blocks OBEX-based file exchange "
                    + "over Bluetooth (Push and FTP profiles). Prevents data exfiltration via wireless. "
                    + "Default: file transfer enabled. Recommended in high-security environments.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableFileTransfer", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableFileTransfer")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableFileTransfer", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-phonebook-access",
                Label = "BT Advertising: Disable Bluetooth Phone Book Access",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "phonebook", "pbap", "privacy", "security"],
                Description =
                    "Sets DisablePhoneBookAccess=1 in Bluetooth policy. Blocks the Phone Book Access "
                    + "Profile (PBAP). Prevents paired devices from reading local contacts. "
                    + "Default: PBAP enabled. Disabling protects contact data from BT-paired devices.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisablePhoneBookAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisablePhoneBookAccess")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisablePhoneBookAccess", 1)],
            },
            new TweakDef
            {
                Id = "btadv-require-bt-encryption",
                Label = "BT Advertising: Require Encryption on Bluetooth Connections",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BthPort],
                Tags = ["bluetooth", "encryption", "security", "hardening"],
                Description =
                    "Sets EncryptionEnabled=1 in BTHPORT Parameters. Enforces that all Bluetooth "
                    + "connections use link-layer encryption. Unencrypted pairing attempts are rejected. "
                    + "Default: optional. Explicit enforcement prevents plaintext BT sessions.",
                ApplyOps = [RegOp.SetDword(BthPort, "EncryptionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(BthPort, "EncryptionEnabled")],
                DetectOps = [RegOp.CheckDword(BthPort, "EncryptionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-remote-audio-playback",
                Label = "BT Advertising: Disable Remote Audio Playback over Bluetooth",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "audio", "a2dp", "remote", "security", "lockdown"],
                Description =
                    "Sets DisableRemoteAudioPlayback=1 in Bluetooth policy. Prevents audio streaming "
                    + "to remote Bluetooth devices (A2DP sink). Disables Bluetooth speakers as data channel. "
                    + "Default: audio playback allowed. Recommended in air-gapped/classified environments.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableRemoteAudioPlayback")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableRemoteAudioPlayback", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bt-discoverable-state",
                Label = "BT Advertising: Force Bluetooth Always Non-Discoverable",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "discoverable", "visibility", "privacy", "security"],
                Description =
                    "Sets ForceNonDiscoverable=1 in Bluetooth policy. Keeps the Bluetooth adapter in "
                    + "non-discoverable state at all times. Prevents detection from BT scanning tools. "
                    + "Default: users can toggle discoverability. Policy lock prevents exposure.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "ForceNonDiscoverable", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "ForceNonDiscoverable")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "ForceNonDiscoverable", 1)],
            },
            new TweakDef
            {
                Id = "btadv-disable-bluetooth-shared-experiences",
                Label = "BT Advertising: Disable Bluetooth Shared Experiences",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [BtPolicy],
                Tags = ["bluetooth", "shared-experiences", "nearby-share", "privacy", "security"],
                Description =
                    "Sets DisableSharedExperiences=1 in Bluetooth policy. Blocks the Bluetooth-based "
                    + "'Shared Experiences' feature which is used for Nearby Share file transfers. "
                    + "Default: enabled. Disabling removes an additional passive data transfer vector.",
                ApplyOps = [RegOp.SetDword(BtPolicy, "DisableSharedExperiences", 1)],
                RemoveOps = [RegOp.DeleteValue(BtPolicy, "DisableSharedExperiences")],
                DetectOps = [RegOp.CheckDword(BtPolicy, "DisableSharedExperiences", 1)],
            },
        ];
    }

    // ── DeviceCompliancePolicy ──
    private static class _DeviceCompliancePolicy
    {
        private const string DhaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string HcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthCenter";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devcpl-enable-health-attestation",
                    Label = "Device Compliance: Enable Device Health Attestation",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableHealthAttestation=1 in DeviceHealthAttestation policy. Enables the Windows Device Health Attestation (DHA) service which uses the device's TPM to cryptographically attest its boot sequence. The DHA service generates a health certificate that can be consumed by MDM providers (Intune, SCCM) and conditional access systems to verify that the device booted without tampering: Secure Boot was enabled, BitLocker is active, the boot path was not modified, and no ELAM-detected malware was present. Without DHA, conditional access can only rely on OS-reported state — which malware can spoof.",
                    Tags = ["dha", "health-attestation", "tpm", "conditional-access", "boot-integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device boot integrity is cryptographically attested using the TPM. Requires TPM 2.0. Health certificates are generated and periodically sent to the configured DHA server. Enables hardware-backed conditional access decisions.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "EnableHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "EnableHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "EnableHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-bitlocker-for-compliance",
                    Label = "Device Compliance: Require BitLocker Encryption for Compliance",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireBitLockerForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if BitLocker Drive Encryption is not enabled on the system drive. Compliance status is reported to MDM (Intune/SCCM) and can trigger conditional access policies that block the device from connecting to corporate resources until BitLocker is enabled. Data loss from stolen or lost unencrypted laptops is one of the most common sources of data breaches. Requiring BitLocker for compliance ensures all mobile devices connecting to corporate resources are encrypted.",
                    Tags = ["compliance", "bitlocker", "encryption", "conditional-access", "data-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without BitLocker on the system drive report as non-compliant. Non-compliant devices may be blocked from corporate resources via conditional access. Requires MDM enrolment and conditional access policies to enforce the compliance gate.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireBitLockerForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireBitLockerForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireBitLockerForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-antivirus-for-compliance",
                    Label = "Device Compliance: Require Active Antivirus for Compliance",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireAntivirusForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if a registered and up-to-date antivirus product is not detected by the Security Center. Real-time protection must be active and signatures cannot be critically outdated. Devices that have disabled antivirus, have expired protection subscriptions, or have antivirus that is consuming no CPU (indicative of process termination by malware) are flagged. Security Center status is checked periodically and on every MDM compliance check cycle.",
                    Tags = ["compliance", "antivirus", "security-center", "real-time-protection", "endpoint"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without active, up-to-date antivirus report as non-compliant. Devices with disabled or expired AV may lose access to corporate resources. Requires MDM and conditional access to enforce. Windows Defender Antivirus or any ELAM-registered product satisfies the requirement.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireAntivirusForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireAntivirusForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireAntivirusForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-set-compliance-check-interval-4h",
                    Label = "Device Compliance: Set Compliance Check Interval to 4 Hours",
                    Category = "Peripherals",
                    Description =
                        "Sets ComplianceCheckIntervalHours=4 in HealthCenter policy. Sets the interval at which Windows re-evaluates device compliance state and sends the current status to the MDM provider. A default compliance check interval that is too long (24+ hours) means a device that becomes non-compliant (user disables BitLocker, AV signs expire, firewall turned off) continues to access corporate resources for up to a day before its compliance status is updated. 4 hours ensures compliance violations are detected and reflected in conditional access within the business day after they occur.",
                    Tags = ["compliance", "check-interval", "mdm", "detection", "conditional-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Compliance state is evaluated every 4 hours. A device that becomes non-compliant is detected within 4 hours. Slightly higher MDM service check-in frequency — negligible network overhead.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceCheckIntervalHours")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceCheckIntervalHours", 4)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-secure-boot-for-compliance",
                    Label = "Device Compliance: Require Secure Boot for Compliance",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireSecureBootForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if UEFI Secure Boot is not enabled. Secure Boot prevents bootkit malware and rootkits from replacing the boot path with untrusted code — without Secure Boot, an attacker with brief physical access can boot from a USB drive to bypass Windows authentication or install a persistent bootkit. Devices with Secure Boot disabled cannot be trusted to run an uncompromised OS. This check complements DHA attestation with a policy-layer enforcement.",
                    Tags = ["compliance", "secure-boot", "uefi", "bootkit", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without Secure Boot enabled report as non-compliant. Very old hardware (pre-2012) may not support Secure Boot. Devices that were deliberately configured without Secure Boot for BIOS compatibility reasons must be re-evaluated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireSecureBootForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireSecureBootForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireSecureBootForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-compliance-grace-period-7days",
                    Label = "Device Compliance: Enable 7-Day Grace Period for Non-Compliant Devices",
                    Category = "Peripherals",
                    Description =
                        "Sets ComplianceGracePeriodDays=7 in HealthCenter policy. Grants newly enrolled devices or devices that first become non-compliant a 7-day grace period before conditional access blocks are enforced. Without a grace period, a device that enrolls in MDM but has not yet completed all compliance remediation (BitLocker encrypting, definitions updating) is immediately blocked from corporate resources — creating a chicken-and-egg problem. The grace period allows IT to remediate the device before it loses access. After 7 days without remediation, access restrictions are enforced.",
                    Tags = ["compliance", "grace-period", "enrolment", "remediation", "mdm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Non-compliant devices have 7 days to reach compliance before access restrictions are applied. Provides IT time for remediation without disrupting new enrolments. After 7 days, non-compliant devices are subject to conditional access blocks.",
                    ApplyOps = [RegOp.SetDword(HcKey, "ComplianceGracePeriodDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "ComplianceGracePeriodDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "ComplianceGracePeriodDays", 7)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-minimum-os-build",
                    Label = "Device Compliance: Require Minimum OS Build for Compliance",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireMinimumOsBuild=1 in HealthCenter policy. Enables minimum OS build checking as a compliance criterion. When enabled, devices running OS builds older than the configured minimum (set separately as MinimumBuildNumber) report as non-compliant. This policy ensures that devices running versions of Windows that are out of Microsoft's support cycle (no security patches) or that have known unpatched critical vulnerabilities are flagged before they access corporate resources. Combined with Windows Update policies, this creates an enforced minimum security baseline.",
                    Tags = ["compliance", "os-build", "patch-level", "security-baseline", "outdated-os"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices below the minimum OS build report as non-compliant. Requires configuring MinimumBuildNumber separately. Devices on unsupported or unpatched OS build are blocked pending upgrade. Coordinate with Windows Update deadline policies.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireMinimumOsBuild", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireMinimumOsBuild")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireMinimumOsBuild", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-require-firewall-for-compliance",
                    Label = "Device Compliance: Require Windows Firewall Active for Compliance",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireFirewallForCompliance=1 in HealthCenter policy. Marks a device as non-compliant if Windows Defender Firewall (or a registered third-party firewall) is not active on all network profiles (domain, private, public). The Windows Firewall is a critical network-based attack prevention control. Users may disable the firewall when troubleshooting connection issues and forget to re-enable it. A device with no host firewall on a public network is exposed to direct network attacks. This compliance check ensures firewalls stay active.",
                    Tags = ["compliance", "firewall", "network-protection", "security-center", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices with disabled Windows Defender Firewall or no registered firewall are non-compliant. Third-party firewalls registered with Security Center satisfy the requirement. Devices that turned off the firewall for temporary diagnostics and forgot to restore will be flagged.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireFirewallForCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireFirewallForCompliance")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireFirewallForCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-block-noncompliant-resource-access",
                    Label = "Device Compliance: Block Non-Compliant Devices from Joining AD Resources",
                    Category = "Peripherals",
                    Description =
                        "Sets BlockNonCompliantNetworkAccess=1 in HealthCenter policy. Enables a local enforcement hook that checks compliance state before allowing the device to connect to protected network resources. When this is enabled and the device is marked non-compliant by the health centre, outbound connections to domain-classified resources can be blocked at the Windows Filtering Platform (WFP) layer. This provides local enforcement independent of whether external conditional access (AAD, MFA, proxy) is in place — useful as defence-in-depth for environments where some legacy resources lack conditional access support.",
                    Tags = ["compliance", "network-access", "block", "conditional-access", "wfp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Non-compliant devices are blocked from accessing domain network resources at the WFP layer. This is a local enforcement on the device itself — not a network-layer block. A device misidentifying its compliance state may block its own legitimate access. Test thoroughly before broad deployment.",
                    ApplyOps = [RegOp.SetDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "BlockNonCompliantNetworkAccess")],
                    DetectOps = [RegOp.CheckDword(HcKey, "BlockNonCompliantNetworkAccess", 1)],
                },
                new TweakDef
                {
                    Id = "devcpl-enable-tpm-attestation-logging",
                    Label = "Device Compliance: Enable TPM Health Attestation Event Logging",
                    Category = "Peripherals",
                    Description =
                        "Sets TpmAttestationLogging=1 in DeviceHealthAttestation policy. Enables event log entries for TPM health attestation operations: TPM measurement capture, health certificate request, health certificate delivery, and health attestation failures. Without attestation logging, diagnosing why a device cannot obtain a health certificate (TPM in reduced functionality mode, endorsement key provisioning failure, attestation service unreachable) is difficult. Log entries enable IT helpdesk to diagnose attestation failures and restore compliance without escalating to infrastructure teams.",
                    Tags = ["tpm", "attestation", "logging", "compliance", "diagnostics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM attestation events are logged. Events include certificate request, success, failure, and failure reasons. Negligible disk overhead. Enables rapid helpdesk diagnosis of attestation failures without advanced tooling.",
                    ApplyOps = [RegOp.SetDword(DhaKey, "TpmAttestationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(DhaKey, "TpmAttestationLogging")],
                    DetectOps = [RegOp.CheckDword(DhaKey, "TpmAttestationLogging", 1)],
                },
            ];
    }

    // ── DeviceEnrollmentLimitPolicy ──
    private static class _DeviceEnrollmentLimitPolicy
    {
        private const string EnlKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceEnrollment";

        private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devenl-set-max-devices-per-user-5",
                    Label = "Device Enrollment Limit: Set Maximum Devices per User to 5",
                    Category = "Peripherals",
                    Description =
                        "Sets MaxDevicesPerUser=5 in DeviceEnrollment policy. Limits the number of devices a single user account can enroll in MDM to 5. Without per-user limits, a single compromised account can be used to enroll large numbers of devices into the MDM tenant, consuming Intune licenses, polluting the device inventory, and potentially using the MDM service to push malware to enrolled devices. A limit of 5 is generous enough for users with a phone, tablet, laptop, home PC, and a spare device, while preventing bulk enrollment abuse.",
                    Tags = ["enrollment", "device-limit", "per-user", "abuse-prevention", "inventory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Each user can enroll a maximum of 5 devices. Attempts to enroll a 6th device are rejected until an existing device is unenrolled. Adjust the limit if your organisation has users with more than 5 managed devices (e.g., kiosk operators managing multiple shared devices with a single service account).",
                    ApplyOps = [RegOp.SetDword(EnlKey, "MaxDevicesPerUser", 5)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "MaxDevicesPerUser")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "MaxDevicesPerUser", 5)],
                },
                new TweakDef
                {
                    Id = "devenl-block-byod-personal-enrollment",
                    Label = "Device Enrollment Limit: Block Personal BYOD Devices from Enrolling",
                    Category = "Peripherals",
                    Description =
                        "Sets BlockPersonalDeviceEnrollment=1 in DeviceEnrollment policy. Prevents devices that are registered as personal devices (not Azure AD Joined or Hybrid Joined) from enrolling in corporate MDM. A personally-owned device that enrolls in corporate MDM becomes subject to remote wipe commands — which could irreversibly delete personal data. Blocking personal device enrollment prevents accidental enrollment of personal hardware into MDM while protecting users' personal devices from corporate management actions. Users who need BYOD access should use Workplace Join with limited MDM (MAM without device enrollment) instead.",
                    Tags = ["byod", "personal-device", "enrollment-block", "remote-wipe-protection", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Personal (non-AAD-Joined) devices cannot enroll in corporate MDM. Users who attempt to add a work account on a personal device get a generic failure. BYOD users should sign in with MAM-only (app-level management) via Outlook or Teams apps instead. Requires AAD Join for full MDM enrollment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "BlockPersonalDeviceEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "BlockPersonalDeviceEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-device-category-on-enroll",
                    Label = "Device Enrollment Limit: Require Device Category Assignment at Enrollment",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireDeviceCategoryOnEnrollment=1 in DeviceEnrollment policy. Requires administrators to assign a device category (e.g., Corporate Laptop, Kiosk, Shared Workstation) at enrollment time. Device categories in Intune are used to automatically assign devices to dynamic groups, which in turn receive different policy sets. Without mandatory category assignment, all devices land in the uncategorised default group and receive a single policy set. Mandatory categories ensure that kiosk devices, shared workstations, and executive laptops each receive appropriately scoped policies from the moment of enrollment.",
                    Tags = ["enrollment", "device-category", "dynamic-group", "policy-scoping", "intune"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Device category must be assigned before enrollment completes. Enrollment fails if no category is selected. Category assignment is performed by the enrolling admin or in automated flows by the Autopilot assignment group. No user-facing UI change for standard users.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireDeviceCategoryOnEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireDeviceCategoryOnEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-unused-enrollment-profiles",
                    Label = "Device Enrollment Limit: Block Devices Without an Enrollment Profile",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireEnrollmentProfile=1 in DeviceEnrollment policy. Prevents devices from enrolling unless they match a pre-configured Intune enrollment profile (Device Enrollment Program, Autopilot profile, or bulk enrollment token). Without this restriction, any device that has credentials for a licensed user can self-enroll in MDM using the standard Settings > Accounts flow. Pre-requiring an enrollment profile means that only devices that IT has explicitly authorized for enrollment (by creating or assigning a profile) can join MDM — unknown or unauthorized devices are rejected.",
                    Tags = ["enrollment", "enrollment-profile", "autopilot", "authorization", "unknown-devices"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only devices that match a pre-configured enrollment profile can enroll. Devices without a matching profile are rejected at enrollment. Devices must be registered in Intune/Autopilot before attempting enrollment. Prevents rogue devices from enrolling with valid user credentials.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireEnrollmentProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireEnrollmentProfile")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireEnrollmentProfile", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-restrict-enrollment-to-aad-join",
                    Label = "Device Enrollment Limit: Restrict MDM Enrollment to AAD Joined Devices Only",
                    Category = "Peripherals",
                    Description =
                        "Sets RestrictEnrollmentToAadJoin=1 in MDM policy. Prevents MDM enrollment from completing unless the device is Azure AD Joined (not just Workplace Joined). Workplace Join provides a limited form of registration that does not require the device to be AAD-joined — this allows personal devices to register without a full AAD Join. By restricting enrollment to AAD Join, this policy ensures that enrolled devices are fully registered in Azure AD with a machine account, which is required for Hybrid Join, Conditional Access device trust, and all domain-level group policies backed by AAD.",
                    Tags = ["enrollment", "aad-join", "device-trust", "conditional-access", "hybrid-join"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enrollment is restricted to devices that complete Azure AD Join. Workplace Join-only devices cannot enroll. Hybrid Joined devices (on-premises AD + AAD) satisfy the AAD Join requirement. Purely on-premises AD-joined devices without AAD sync must Hybrid-Join before they can enroll.",
                    ApplyOps = [RegOp.SetDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                    RemoveOps = [RegOp.DeleteValue(MdmKey, "RestrictEnrollmentToAadJoin")],
                    DetectOps = [RegOp.CheckDword(MdmKey, "RestrictEnrollmentToAadJoin", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-enable-enrollment-status-page",
                    Label = "Device Enrollment Limit: Enable MDM Enrollment Status Page During OOBE",
                    Category = "Peripherals",
                    Description =
                        "Sets ShowEnrollmentStatusPage=1 in DeviceEnrollment policy. Enables the Enrollment Status Page (ESP) during Autopilot or standard OOBE enrollment. The ESP shows the user (and IT) the real-time progress of device setup: account provisioning, app installations, policy applications, and certificate enrollments. Without the ESP, the user is deposited at the desktop while apps are still installing or policies are still applying — the device may appear functional but actually be in an incomplete configuration state. The ESP holds the user at the setup screen until all critical configurations are complete.",
                    Tags = ["enrollment", "esp", "oobe", "autopilot", "setup-progress"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enrollment Status Page is shown during Autopilot/OOBE. Users are blocked at the ESP until all required apps and policies are applied. Prevents users from using a partially-configured device. Increases initial setup time by the duration of app installations.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "ShowEnrollmentStatusPage")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "ShowEnrollmentStatusPage", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-block-enrollment-from-unknown-networks",
                    Label = "Device Enrollment Limit: Block Enrollment Attempts from Non-Corporate Networks",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireCorporateNetworkForEnrollment=1 in DeviceEnrollment policy. Restricts MDM enrollment to devices on corporate networks (defined by the network location awareness profile). Enrollment attempts from unclassified or public networks are blocked. This prevents bulk enrollment of devices by an attacker using stolen credentials from outside the corporate network perimeter. While this is most relevant for legacy MDM setups without Azure AD conditional access, it adds network perimeter enforcement as an extra enrollment control — enrollment over public networks requires re-evaluation of the risk posture.",
                    Tags = ["enrollment", "network-restriction", "corporate-network", "nla", "perimeter"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "MDM enrollment is only permitted from networks classified as corporate (domain controller reachable, NLA domain profile active). Devices on guest, public, or unclassified networks cannot enroll. This may prevent legitimate remote onboarding — coordinate with VPN policies to ensure remote enrollment is still possible via corporate VPN.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireCorporateNetworkForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireCorporateNetworkForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-log-enrollment-failures",
                    Label = "Device Enrollment Limit: Enable Detailed Logging of Enrollment Failures",
                    Category = "Peripherals",
                    Description =
                        "Sets LogEnrollmentFailures=1 in DeviceEnrollment policy. Enables detailed logging of MDM enrollment failure events to the Windows Event Log. Enrollment failures are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel with structured error codes (HRESULT), the enrollment phase that failed (token acquisition, DRS discovery, enrollment registration, certificate acquisition), and whether the failure was a network error, authentication error, or server error. This significantly accelerates helpdesk troubleshooting of Autopilot and enrollment failures.",
                    Tags = ["enrollment", "failure-logging", "event-log", "diagnostics", "helpdesk"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "MDM enrollment failures are logged with structured error codes and phase information. Logs written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider channel. No performance impact — logging only occurs on failure paths.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "LogEnrollmentFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "LogEnrollmentFailures")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "LogEnrollmentFailures", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-require-mfa-for-enrollment",
                    Label = "Device Enrollment Limit: Require MFA at MDM Enrollment Time",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireMfaForEnrollment=1 in DeviceEnrollment policy. Requires multi-factor authentication at the time of MDM enrollment in addition to the standard password credential. Without MFA at enrollment, a stolen password is sufficient to enroll an attacker's device into the corporate MDM tenant. With enrollment MFA enforced, the attacker must also have the victim's second factor (phone, hardware key) to complete enrollment. MDM enrollment grants the device significant privileges (policy application, certificate issuance, resource access upon compliance) — requiring MFA at this critical step is essential.",
                    Tags = ["mfa", "enrollment", "authentication", "conditional-access", "identity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users must complete MFA during MDM enrollment. Requires Azure AD MFA or equivalent. Autopilot deployments using device-identity-based enrollment (PPKG or DEM account) may need exemption. Test Autopilot flows before broad deployment.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "RequireMfaForEnrollment", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "RequireMfaForEnrollment")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "RequireMfaForEnrollment", 1)],
                },
                new TweakDef
                {
                    Id = "devenl-audit-enrollment-activity",
                    Label = "Device Enrollment Limit: Audit All Device Enrollment Activity",
                    Category = "Peripherals",
                    Description =
                        "Sets AuditEnrollmentActivity=1 in DeviceEnrollment policy. Enables audit logging for all device enrollment activity: successful enrollments, failed enrollment attempts, enrollment profile matching and rejection, and unenrollment events. Audit records include the user UPN that initiated the enrollment, the device serial number and hardware ID, the enrollment profile matched (or lack thereof), and the outcome. Enrollment audit logs are written to the Microsoft-Windows-DeviceManagement-Enterprise-Diagnostics-Provider/Admin channel and can be forwarded to SIEM for detection of rogue enrollment attempts.",
                    Tags = ["enrollment", "audit", "siem", "monitoring", "security-event"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "All enrollment attempts are logged with user, device, and outcome details. Audit events can be forwarded to SIEM. Detection: unusually high enrollment failures from a single user may indicate credential stuffing. No performance overhead — logging is asynchronous.",
                    ApplyOps = [RegOp.SetDword(EnlKey, "AuditEnrollmentActivity", 1)],
                    RemoveOps = [RegOp.DeleteValue(EnlKey, "AuditEnrollmentActivity")],
                    DetectOps = [RegOp.CheckDword(EnlKey, "AuditEnrollmentActivity", 1)],
                },
            ];
    }

    // ── DeviceEnrollmentPolicy ──
    private static class _DeviceEnrollmentPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\MDMEnrollment";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devenrl-disable-mdm-enrollment",
                Label = "Disable Automatic MDM Enrollment with Azure AD Join",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Automatic MDM enrollment triggers when a device joins Azure Active Directory and automatically enrolls it in the linked Intune Mobile Device Management tenant. Disabling automatic MDM enrollment prevents devices from auto-enrolling in MDM when users join their Azure AD accounts to devices. In managed environments where all devices should be enrolled, automatic enrollment is desirable, but in specialized scenarios enrollment may need to be controlled. Specialized devices like developer workstations, lab systems, or shared equipment may have specific reasons to avoid automatic MDM enrollment. Disabling auto-enrollment does not prevent manual IT-initiated enrollment which can still occur through IT-directed processes. Organizations should carefully evaluate this setting as it can create unmanaged device gaps in environments expecting universal MDM coverage.",
                Tags = ["mdm", "enrollment", "azure-ad", "device-management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AutoEnrollMDM", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoEnrollMDM")],
                DetectOps = [RegOp.CheckDword(Key, "AutoEnrollMDM", 0)],
            },
            new TweakDef
            {
                Id = "devenrl-disable-bulk-enrollment",
                Label = "Disable Bulk MDM Enrollment via Provisioning Packages",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Bulk enrollment provisioning packages allow an administrator to enroll multiple devices in MDM simultaneously using a pre-configured package. Disabling bulk enrollment prevents provisioning packages from enrolling devices without interactive authentication preventing unauthorized mass enrollment. Bulk enrollment packages contain authentication credentials and if the package is captured it could be used to enroll unauthorized devices into the MDM tenant. IT administrators should use certificate-based bulk enrollment with short-validity certificates rather than username and password provisioning packages. Disabling bulk enrollment forces all device enrollment to use individual authenticated enrollment preventing bulk package replay attacks. Organizations that need bulk enrollment should use Windows Autopilot instead which provides stronger enrollment authentication.",
                Tags = ["mdm", "bulk-enrollment", "provisioning", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBulkEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBulkEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBulkEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-enable-enrollment-status-page",
                Label = "Enable MDM Enrollment Status Page During Autopilot",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The MDM Enrollment Status Page blocks user access to the device until all required MDM policies and applications are successfully applied during Autopilot provisioning. Enabling the enrollment status page ensures that users cannot bypass required security configurations by accessing the device before MDM setup is complete. Without the enrollment status page users can log on before required security applications like endpoint protection are installed creating a window of vulnerability. The enrollment status page prevents devices from entering use without all compliance and security configurations required by MDM policy. Blocking access during enrollment is particularly important for security-critical configurations like full disk encryption that must complete before data is created. Organizations should configure a meaningful timeout and error handling to prevent enrollment failures from permanently blocking device access.",
                Tags = ["mdm", "enrollment-status", "autopilot", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnrollmentStatusPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnrollmentStatusPage")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnrollmentStatusPage", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-block-unknown-unenrollment",
                Label = "Block User-Initiated MDM Unenrollment",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "User-initiated MDM unenrollment allows end users to remove corporate management from their devices through the Settings application. Blocking user-initiated unenrollment prevents employees from removing corporate MDM management to evade security policies or monitoring. Unenrollment from MDM would remove all deployed security policies, applications, and configurations leaving the device non-compliant. Disabling unenrollment ensures that devices remain under corporate management for their operational lifetime without requiring IT intervention to re-enroll. Blocking unenrollment is particularly important for CYOD and COPE scenarios where corporate data must remain protected at all times. IT processes for legitimate device retirement or reassignment should include formal MDM unenrollment through administrative procedures rather than user self-service.",
                Tags = ["mdm", "unenrollment", "device-management", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisallowMDMUnenrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisallowMDMUnenrollment")],
                DetectOps = [RegOp.CheckDword(Key, "DisallowMDMUnenrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-require-enrollment-compliance",
                Label = "Require MDM Enrollment Compliance Before Resource Access",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "MDM enrollment compliance requirements block access to corporate resources from devices that are not enrolled in MDM management. Requiring enrollment compliance implements zero-trust access principles by ensuring only managed devices can access corporate email, applications, and data. Non-enrolled devices lack the security configuration baselines, endpoint protection, and monitoring that managed endpoints provide. Compliance-gated resource access forces all devices seeking corporate data to register under management before receiving access. Organizations should combine enrollment requirements with conditional access policies in Azure AD and Intune for comprehensive enforcement. Compliance requirements should be communicated to users during device provisioning so they understand the enrollment requirement for resource access.",
                Tags = ["mdm", "compliance", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireComplianceCheck", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireComplianceCheck")],
                DetectOps = [RegOp.CheckDword(Key, "RequireComplianceCheck", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-enable-enrollment-certificate-auth",
                Label = "Require Certificate Authentication for MDM Enrollment",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Certificate authentication for MDM enrollment replaces username and password credentials with device certificates that are more resistant to phishing and credential theft. Requiring certificate authentication ensures that only devices with valid enterprise certificates issued by the organizational PKI can enroll in MDM. Strong device identity through certificates ensures that MDM enrollment is limited to devices that have gone through the IT provisioning process. Certificate-based enrollment prevents attackers from enrolling unauthorized devices using stolen credentials. Enterprise certificates for device enrollment should be issued with appropriate validity periods and revocation capabilities. Certificate authentication for MDM enrollment aligns with zero-trust principles of verified device identity before granting management access.",
                Tags = ["mdm", "certificate-auth", "enrollment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireCertificateAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCertificateAuth")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCertificateAuth", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-audit-enrollment-events",
                Label = "Enable Audit Logging for MDM Enrollment Events",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "MDM enrollment audit logging records all enrollment actions including successful enrollments, failed attempts, and unenrollment operations. Enabling enrollment audit logging provides a complete record of device management changes for security investigation and compliance reporting. Enrollment logs help detect unauthorized enrollment attempts by unauthorized users trying to add managed credentials to devices they should not access. Unenrollment events in audit logs can alert security teams when devices are removed from management unexpectedly. MDM enrollment audit events should be forwarded to SIEM for correlation with other device and identity events. Regular review of enrollment audit logs helps identify devices that have been enrolled multiple times which may indicate credential theft or device cloning attempts.",
                Tags = ["mdm", "audit", "enrollment-logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditEnrollmentEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditEnrollmentEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditEnrollmentEvents", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-set-enrollment-retry-limit",
                Label = "Set Maximum MDM Enrollment Retry Limit",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "MDM enrollment retry limits prevent automated brute-force enrollment attempts by limiting the number of consecutive enrollment failures before locking the enrollment channel. Setting enrollment retry limits reduces the effectiveness of automated attacks against MDM enrollment endpoints using credential stuffing or brute force. Excessive enrollment failures may indicate a misconfigured provisioning package or an unauthorized device attempting to enroll using stolen credentials. After reaching the retry limit the device should require IT intervention to reset before enrollment can be attempted again. The retry limit should be set high enough to accommodate legitimate transient network failures but low enough to detect automated attack patterns. Enrollment retry events should be monitored and alerts triggered when retry limits are approached or reached.",
                Tags = ["mdm", "retry-limit", "brute-force-protection", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxEnrollmentRetries", 5)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxEnrollmentRetries")],
                DetectOps = [RegOp.CheckDword(Key, "MaxEnrollmentRetries", 5)],
            },
            new TweakDef
            {
                Id = "devenrl-enforce-enrollment-encryption",
                Label = "Enforce BitLocker Before MDM Enrollment Completion",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "BitLocker enforcement during MDM enrollment ensures that full disk encryption is enabled before the device is classified as enrolled and compliant. Requiring BitLocker during enrollment ensures that corporate data cannot be created on unencrypted devices that could bypass data loss prevention policies. Enrollment-time BitLocker enforcement prevents devices from accessing corporate resources until encryption is fully enabled and the recovery key is escrowed to Azure AD or Active Directory. Without enrollment-time BitLocker enforcement a device could access corporate data with a temporary compliance bypass before encryption is configured. BitLocker Silent Encryption using key escrow to Azure AD provides zero-touch encryption during Autopilot enrollment without user interaction. BitLocker enforcement requirements should be defined in the MDM compliance policy and verified before granting access to sensitive resources.",
                Tags = ["mdm", "bitlocker", "encryption", "enrollment", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerAtEnrollment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerAtEnrollment")],
                DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerAtEnrollment", 1)],
            },
            new TweakDef
            {
                Id = "devenrl-restrict-enrollment-to-approved-tenant",
                Label = "Restrict MDM Enrollment to Approved Tenant Only",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Tenant enrollment restriction limits MDM enrollment to a specific Azure AD tenant preventing devices from being enrolled in unauthorized or attacker-controlled tenants. Restricting enrollment to the approved tenant prevents adversaries from enrolling corporate devices in a rogue MDM tenant to gain management control. Tenant-restricted enrollment is particularly important for corporate shared devices and lab equipment that may be accessed by multiple users. Without tenant restrictions a user with global administrator rights in a different tenant could enroll a device they have physical access to. Enrollment tenant restrictions should be configured through Windows Registry as a machine-wide policy that applies regardless of the currently signed-in user. Organizations should combine tenant enrollment restrictions with Windows Defender ATP device enrollment to ensure all devices are enrolled in the correct tenant.",
                Tags = ["mdm", "tenant-restriction", "enrollment", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToApprovedTenant", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToApprovedTenant")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToApprovedTenant", 1)],
            },
        ];
    }

    // ── DeviceGuardPolicy ──
    private static class _DeviceGuardPolicy
    {
        private const string DgKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devguard-require-uefi-mat",
                    Label = "Require UEFI Memory Attributes Table for HVCI",
                    Category = "Peripherals",
                    Description = "Requires the firmware to expose a UEFI Memory Attributes Table, enabling stricter HVCI enforcement.",
                    Tags = ["hvci", "uefi", "mat", "device-guard", "firmware", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Ensures UEFI properly marks regions; old firmware without UEFI MAT will fail HVCI initialisation.",
                    ApplyOps = [RegOp.SetDword(DgKey, "HVCIMATRequired", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "HVCIMATRequired")],
                    DetectOps = [RegOp.CheckDword(DgKey, "HVCIMATRequired", 1)],
                },
                new TweakDef
                {
                    Id = "devguard-enable-system-guard",
                    Label = "Enable System Guard Secure Launch",
                    Category = "Peripherals",
                    Description = "Enables System Guard Secure Launch to verify platform integrity at boot using Dynamic Root of Trust (DRTM).",
                    Tags = ["system-guard", "drtm", "secure-launch", "device-guard", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Requires Intel TXT or AMD SKINIT; boot verified via secure measurement; no effect on unsupported hardware.",
                    ApplyOps = [RegOp.SetDword(DgKey, "ConfigureSystemGuardLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "ConfigureSystemGuardLaunch")],
                    DetectOps = [RegOp.CheckDword(DgKey, "ConfigureSystemGuardLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "devguard-enable-kernel-shadow-stack",
                    Label = "Enable Kernel Mode Hardware-Enforced Stack Protection",
                    Category = "Peripherals",
                    Description = "Activates Hardware-Enforced Call Stack Protection (CET Shadow Stack) for kernel mode to resist ROP attacks.",
                    Tags = ["cet", "shadow-stack", "device-guard", "kernel", "security", "rop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Requires Intel CET (Tiger Lake+) or AMD equivalent; blocks ROP/JOP exploits in kernel; may conflict with old drivers.",
                    ApplyOps = [RegOp.SetDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "ConfigureKernelShadowStacksLaunchControl")],
                    DetectOps = [RegOp.CheckDword(DgKey, "ConfigureKernelShadowStacksLaunchControl", 1)],
                },
                new TweakDef
                {
                    Id = "devguard-enable-hvci-audit-mode",
                    Label = "Disable HVCI Audit Mode (Enforce Mode)",
                    Category = "Peripherals",
                    Description = "Ensures HVCI operates in enforcement mode rather than audit-only mode for active code integrity protection.",
                    Tags = ["hvci", "audit-mode", "device-guard", "enforce", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Audit mode only logs violations; enforce mode blocks them. Disabling audit ensures active protection.",
                    ApplyOps = [RegOp.SetDword(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled")],
                    DetectOps = [RegOp.CheckDword(DgKey, "HypervisorEnforcedCodeIntegrityAuditModeEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "devguard-block-unsigned-drivers",
                    Label = "Block Unsigned Kernel Drivers via Policy",
                    Category = "Peripherals",
                    Description = "Prevents loading of unsigned kernel-mode drivers, supplementing HVCI at the policy layer.",
                    Tags = ["device-guard", "drivers", "signing", "kernel", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Blocks unsigned drivers; WHQL or Microsoft signing required; may break niche hardware with unsigned drivers.",
                    ApplyOps = [RegOp.SetDword(DgKey, "RequireDriverSignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "RequireDriverSignature")],
                    DetectOps = [RegOp.CheckDword(DgKey, "RequireDriverSignature", 1)],
                },
                new TweakDef
                {
                    Id = "devguard-audit-device-guard-status",
                    Label = "Enable Device Guard Status Auditing",
                    Category = "Peripherals",
                    Description = "Logs Device Guard and Credential Guard startup status to the event log for compliance monitoring.",
                    Tags = ["device-guard", "audit", "logging", "compliance", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Writes Device Guard status events at boot; purely informational, no security side effects.",
                    ApplyOps = [RegOp.SetDword(DgKey, "AuditDeviceGuardStatus", 1)],
                    RemoveOps = [RegOp.DeleteValue(DgKey, "AuditDeviceGuardStatus")],
                    DetectOps = [RegOp.CheckDword(DgKey, "AuditDeviceGuardStatus", 1)],
                },
            ];
    }

    // ── DeviceGuardVbs ──
    private static class _DeviceGuardVbs
    {
        private const string DeviceGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard";

        private const string Scenarios = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios";

        private const string HvciScenario =
            @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity";

        private const string CredentialGuard = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa";

        private const string CodeIntegrity = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CI\Config";

        private const string DeviceGuardPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vbs-enable-hvci",
                Label = "Enable Hypervisor-Protected Code Integrity (HVCI)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "hvci", "code integrity", "security", "virtualization"],
                Description =
                    "Enables HVCI (Memory Integrity) which uses the Hyper-V hypervisor to "
                    + "verify kernel-mode code signatures at runtime. Prevents kernel exploits "
                    + "and driver code injection. Requires a reboot. May impact performance by 5–15% "
                    + "on systems without MBEC hardware support.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "Enabled", 1)],
                RemoveOps = [RegOp.SetDword(HvciScenario, "Enabled", 0)],
                DetectOps = [RegOp.CheckDword(HvciScenario, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "vbs-require-secure-boot-dma",
                Label = "Require Secure Boot + DMA Protection for VBS",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Tags = ["vbs", "secure boot", "dma", "protection", "policy"],
                Description =
                    "Sets RequirePlatformSecurityFeatures=3 (require both Secure Boot and "
                    + "DMA protection). Prevents VBS from running on systems that lack "
                    + "IOMMU/VT-d DMA protection, ensuring the highest security level.",
                ApplyOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
                RemoveOps = [RegOp.SetDword(DeviceGuard, "RequirePlatformSecurityFeatures", 1)],
                DetectOps = [RegOp.CheckDword(DeviceGuard, "RequirePlatformSecurityFeatures", 3)],
            },
            new TweakDef
            {
                Id = "vbs-enable-config-ci-policy",
                Label = "Enable Configurable Code Integrity (WDAC Boot Policy)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                Tags = ["vbs", "wdac", "code integrity", "kernel", "policy"],
                Description =
                    "Sets the CodeIntegrity configurable policy option. When Enabled=1, "
                    + "the Windows Defender Application Control boot policy is loaded. "
                    + "WARNING: requires a valid WDAC policy file to be present, or the "
                    + "system may fail to boot drivers not covered by the policy.",
                ApplyOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 1)],
                RemoveOps = [RegOp.SetDword(CodeIntegrity, "Enabled", 0)],
                DetectOps = [RegOp.CheckDword(CodeIntegrity, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "vbs-enable-kernel-shadow-stacks",
                Label = "Enable Kernel Shadow Stacks (Control Flow Guard Enforcement)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["vbs", "shadow stack", "cfg", "control flow guard", "exploit"],
                Description =
                    "Enables kernel shadow stacks (also called Kernel CFG Enforcement) which "
                    + "uses hardware CET (Control-flow Enforcement Technology) to harden "
                    + "return address integrity in kernel mode. Intel Tiger Lake and later. "
                    + "KernelShadowStacksEnabled=1.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "WasEnabledBy", 1)],
                RemoveOps = [RegOp.DeleteValue(HvciScenario, "WasEnabledBy")],
                DetectOps = [RegOp.CheckDword(HvciScenario, "WasEnabledBy", 1)],
            },
            new TweakDef
            {
                Id = "vbs-lock-hvci",
                Label = "Lock HVCI to Prevent Disable Without Reboot",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["vbs", "hvci", "lock", "tamper protection"],
                Description =
                    "Sets HVCI Locked=1, preventing the policy from being disabled at runtime "
                    + "without a reboot. Once locked, changes to Memory Integrity require "
                    + "a system restart to take effect, protecting against live tampering.",
                ApplyOps = [RegOp.SetDword(HvciScenario, "Locked", 1)],
                RemoveOps = [RegOp.SetDword(HvciScenario, "Locked", 0)],
                DetectOps = [RegOp.CheckDword(HvciScenario, "Locked", 1)],
            },
            new TweakDef
            {
                Id = "vbs-disable-lsa-protection-audit-mode",
                Label = "Disable LSA Protected Process Audit Mode",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["vbs", "lsa", "protected process", "audit"],
                Description =
                    "Disables LSA Protected Process audit mode (RunAsPPL audit mode = 0). "
                    + "Audit mode logs what would be blocked if LSA Protection were enabled "
                    + "without actually enabling protection. Disable once LSA Protection "
                    + "is confirmed stable on the system.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RunAsPPLBoot", 2)],
            },
        ];
    }

    // ── DeviceHealthCheckPolicy ──
    private static class _DeviceHealthCheckPolicy
    {
        private const string HcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";

        private const string TpmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devhc-enable-tpm-health-check",
                    Label = "Device Health: Enable TPM Health State Evaluation",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableTpmHealthCheck=1 in DeviceHealthAttestation policy. Enables evaluation of TPM health state as part of the device health check. The TPM health check evaluates whether the TPM is enabled, activated, owned, and in a known-good state. TPMs can enter a reduced-functionality mode (e.g., after detecting too many failed PIN attempts or a firmware update that changes the platform configuration registers). A TPM in degraded state cannot attest the boot chain, which can silently cause attestation failures unless the health check actively reports the degraded status.",
                    Tags = ["tpm", "health-check", "attestation", "degraded-state", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM health is evaluated on every DHA cycle. Degraded or disabled TPM is reported as a health issue. Enables IT to detect TPM lockout or firmware-changed PCR states before they cause silent attestation failures.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableTpmHealthCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableTpmHealthCheck")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableTpmHealthCheck", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-elam-driver-for-health",
                    Label = "Device Health: Require ELAM Driver Active for Healthy State",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireElamDriverForHealth=1 in DeviceHealthAttestation policy. Reports the device as unhealthy if an Early Launch Anti-Malware (ELAM) driver is not loaded and active at boot. ELAM drivers are loaded before all other non-Microsoft drivers, giving them the ability to evaluate and classify boot drivers as trusted, untrusted, or unknown before they are allowed to initialize. Without an active ELAM driver, the device's pre-OS environment cannot be assessed for rootkits or boot drivers installed by malware. Windows Defender is an ELAM-registered product and satisfies this requirement.",
                    Tags = ["elam", "health", "boot-security", "early-launch", "malware-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Devices without an active ELAM driver are reported as unhealthy by DHA. Windows Defender satisfies this requirement by default. Third-party ELAM-registered AV products also satisfy it. Devices with all AV disabled will fail this check.",
                    ApplyOps = [RegOp.SetDword(HcKey, "RequireElamDriverForHealth", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "RequireElamDriverForHealth")],
                    DetectOps = [RegOp.CheckDword(HcKey, "RequireElamDriverForHealth", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-evaluate-secure-boot-measurement",
                    Label = "Device Health: Evaluate Secure Boot PCR Measurement Consistency",
                    Category = "Peripherals",
                    Description =
                        "Sets EvaluateSecureBootMeasurement=1 in DeviceHealthAttestation policy. Enables DHA to evaluate the consistency of Secure Boot Platform Configuration Register (PCR) measurements. TPM PCR values record hashes of every component in the boot chain. If the PCR values in the most recent health certificate differ from the baseline (e.g., a firmware update changed a boot component hash), the attestation service can detect this deviation and flag the device. This catches scenarios where a firmware update inadvertently introduced an unsigned component or where a bootkit altered a measured value.",
                    Tags = ["secure-boot", "pcr", "measurement", "attestation", "boot-chain"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Secure Boot PCR measurements are included in DHA health certificates. Changes to PCR values (firmware update, boot component change) are detected. Legitimate firmware updates may transiently mark the device as unhealthy until the DHA baseline is updated.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EvaluateSecureBootMeasurement")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EvaluateSecureBootMeasurement", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-set-health-report-retention-30days",
                    Label = "Device Health: Retain Health Reports for 30 Days",
                    Category = "Peripherals",
                    Description =
                        "Sets HealthReportRetentionDays=30 in DeviceHealthAttestation policy. Sets the number of days that device health reports are retained locally before being purged. Retaining health reports for 30 days provides a rolling audit of the device's health state history. This is useful for post-incident forensics: if a device was compromised, the health report history can show the exact point at which the TPM measurements changed, when Secure Boot was disabled, or when the ELAM driver was removed — correlating health state changes with suspicious events in the device's event log.",
                    Tags = ["health-report", "retention", "forensics", "audit", "30-days"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Health report data is retained for 30 days locally. Provides 30 days of health state history for forensic investigation. Small disk footprint — health reports are compact JSON structures, typically a few KB each.",
                    ApplyOps = [RegOp.SetDword(HcKey, "HealthReportRetentionDays", 30)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "HealthReportRetentionDays")],
                    DetectOps = [RegOp.CheckDword(HcKey, "HealthReportRetentionDays", 30)],
                },
                new TweakDef
                {
                    Id = "devhc-disable-health-check-bypass",
                    Label = "Device Health: Disable Health Check Bypass for Non-Compliant State",
                    Category = "Peripherals",
                    Description =
                        "Sets DisableHealthCheckBypass=1 in DeviceHealthAttestation policy. Prevents clients (including local administrators) from bypassing or suppressing the device health check. Without this policy, a sophisticated user or malware with admin privileges can modify the health state cache or suppress health certificate requests, causing the device to appear healthy to conditional access systems while actually being compromised. Disabling the bypass ensures that the DHA client cannot be locally tampered with to present a false healthy state.",
                    Tags = ["health-check", "bypass-prevention", "anti-tampering", "admin-restriction", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Health check processes cannot be bypassed or suppressed by local admins. Prevents malware or sophisticated users from spoofing a healthy state to conditional access systems. May complicate debugging of attestation issues in development environments.",
                    ApplyOps = [RegOp.SetDword(HcKey, "DisableHealthCheckBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "DisableHealthCheckBypass")],
                    DetectOps = [RegOp.CheckDword(HcKey, "DisableHealthCheckBypass", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-health-check-auto-remediation",
                    Label = "Device Health: Enable Automatic Remediation for Known Health Issues",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableHealthAutoRemediation=1 in DeviceHealthAttestation policy. Enables the Device Health agent to attempt automatic remediation for known, non-critical health issues. Remediable issues include re-enabling Windows Defender real-time protection that was automatically disabled by a third-party AV (after that AV was uninstalled), re-enrolling the TPM endorsement key if the certificate expired, or restarting stalled health service processes. Automatic remediation reduces helpdesk tickets for transient compliance failures caused by installation or configuration drift.",
                    Tags = ["health", "auto-remediation", "defender", "tpm", "service-restart"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "The health agent automatically resolves known fixable issues (re-enables AV, restarts health services, re-provisions TPM EK). Only remediates known, low-risk issues — it will never force-enable BitLocker or change user-configured settings. Review the list of supported remediations for your OS build.",
                    ApplyOps = [RegOp.SetDword(HcKey, "EnableHealthAutoRemediation", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "EnableHealthAutoRemediation")],
                    DetectOps = [RegOp.CheckDword(HcKey, "EnableHealthAutoRemediation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-tpm-endorsement-key-validation",
                    Label = "Device Health: Enable TPM Endorsement Key Validation",
                    Category = "Peripherals",
                    Description =
                        "Sets ValidateTpmEndorsementKey=1 in TPM policy. Enables validation that the TPM's Endorsement Key (EK) certificate is in a known-valid certificate chain rooted at a trusted TPM manufacturer CA. The EK uniquely identifies the physical TPM chip. If EK validation is disabled or skipped, software-based fake TPM implementations (used in virtual machines without vTPM, or malicious virtual TPM drivers) can pass attestation checks. EK validation ensures the attestation chain is anchored to a real hardware chip with a manufacturer-issued certificate.",
                    Tags = ["tpm", "endorsement-key", "ek-validation", "hardware-anchor", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "TPM endorsement key certificates are validated against the manufacturer CA chain. VMs with software vTPM (Hyper-V vTPM, VMware vTPM) have EK certificates signed by Microsoft or the platform vendor and will pass if those CAs are trusted. Non-certified TPMs in custom hardware may fail.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "ValidateTpmEndorsementKey")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "ValidateTpmEndorsementKey", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-require-tpm-version-20",
                    Label = "Device Health: Require TPM 2.0 for Health Attestation",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireTpm20ForHealthAttestation=1 in TPM policy. Marks devices as unable to provide health attestation if they only have a TPM 1.2 chip (as opposed to a TPM 2.0). TPM 1.2 supports SHA-1 algorithm measurement banks. TPM 2.0 adds SHA-256 banks, algorithm agility, and enhanced authorization structures. Modern DHA services require TPM 2.0's enhanced capabilities for accurate, tamper-resistant attestation. TPM 1.2 attestation can be spoofed more easily and lacks support for Credential Guard, Device Guard, and Virtualization-Based Security measurements.",
                    Tags = ["tpm", "tpm-20", "attestation", "sha256", "vbs"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Devices with only TPM 1.2 cannot provide health attestation and are treated as unhealthy. Hardware manufactured before 2016 may only have TPM 1.2. Devices with no TPM are already unable to attest. Review device fleet hardware compatibility before enforcing.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "RequireTpm20ForHealthAttestation")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "RequireTpm20ForHealthAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-code-integrity-measurement",
                    Label = "Device Health: Enable Code Integrity State in Health Reports",
                    Category = "Peripherals",
                    Description =
                        "Sets IncludeCodeIntegrityInReport=1 in DeviceHealthAttestation policy. Includes Windows Code Integrity (CI) enforcement state in the DHA health certificate. Code Integrity state records whether Windows Defender Application Control (WDAC) or Device Guard is active, whether CI is in audit vs. enforcement mode, and whether User-Mode Code Integrity (UMCI) is enabled in addition to HVCI (Hypervisor-Protected Code Integrity). Including CI state in the attestation report allows conditional access systems to require not just that the device is healthy but that it is actively enforcing application whitelisting.",
                    Tags = ["code-integrity", "wdac", "device-guard", "hvci", "attestation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Code Integrity (WDAC/HVCI) state is included in the DHA health certificate. Conditional access can now require that a device have CI enforcement mode active. Devices in CI audit-only mode can be flagged as less secure than those in enforcement mode.",
                    ApplyOps = [RegOp.SetDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeCodeIntegrityInReport")],
                    DetectOps = [RegOp.CheckDword(HcKey, "IncludeCodeIntegrityInReport", 1)],
                },
                new TweakDef
                {
                    Id = "devhc-enable-vbs-state-measurement",
                    Label = "Device Health: Include VBS/Credential Guard State in Health Reports",
                    Category = "Peripherals",
                    Description =
                        "Sets IncludeVbsStateInReport=1 in DeviceHealthAttestation policy. Includes Virtualization-Based Security (VBS) and Credential Guard state in the DHA health certificate. VBS isolates critical OS components (LSA, UEFI variable writes) inside a secure virtual machine backed by the CPU hypervisor, making credential theft attacks (Pass-the-Hash, Pass-the-Ticket) significantly harder. Including VBS state in attestation reports allows conditional access to enforce that only VBS-enabled devices handle sensitive workloads — for example, requiring VBS for devices that access privileged admin consoles.",
                    Tags = ["vbs", "credential-guard", "hypervisor", "attestation", "lsa-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "VBS and Credential Guard state is included in DHA health certificates. Conditional access can require VBS/Credential Guard for high-privilege resource access. Devices without hardware VBS support (no hardware-enforced DEP, SLAT, or IOMMU) cannot satisfy this requirement.",
                    ApplyOps = [RegOp.SetDword(HcKey, "IncludeVbsStateInReport", 1)],
                    RemoveOps = [RegOp.DeleteValue(HcKey, "IncludeVbsStateInReport")],
                    DetectOps = [RegOp.CheckDword(HcKey, "IncludeVbsStateInReport", 1)],
                },
            ];
    }

    // ── DeviceInstallPolicies ──
    private static class _DeviceInstallPolicies
    {
        private const string Restrictions = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        private const string Settings = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Settings";

        private const string DriverSearching = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverSearching";

        private const string DeviceMetadata = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Device Metadata";

        private const string DeviceInstaller = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Installer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dinst-enable-class-block",
                Label = "Device Install Policy: Enable Setup Class GUID Restriction List",
                Category = "Peripherals",
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
                Id = "dinst-retroactive-id-block",
                Label = "Device Install Policy: Apply Device ID Blocks Retroactively",
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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

    // ── DeviceInstallPolicy ──
    private static class _DeviceInstallPolicy
    {
        private const string DiKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";
        private const string DiRestrictKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devinstall-deny-unspecified-devices",
                    Label = "Deny Installation of Unlisted Device Classes",
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
                    Description =
                        "Blocks installation of IEEE 1394 (FireWire) bus controllers, which support DMA and can bypass OS memory protection.",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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
                    Category = "Peripherals",
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

    // ── DeviceLockGpoPolicy ──
    private static class _DeviceLockGpoPolicy
    {
        private const string PassportKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PassportForWork";
        private const string DesktopKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Control Panel\Desktop";
        private const string SystemKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devlockgpo-disable-hello-pin-recovery",
                Label = "Device Lock GPO: Disable Windows Hello PIN Recovery Service",
                Category = "Peripherals",
                Description =
                    "Disables the cloud-based Windows Hello PIN recovery service that allows users to reset their device PIN via their Microsoft account or Azure AD credentials. The PIN recovery service sends encrypted PIN reset data to Microsoft cloud servers. In high-security environments where no cloud dependencies are allowed, this service should be disabled.",
                Tags = ["windows hello", "pin", "recovery", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PassportKey],
                ApplyOps = [RegOp.SetDword(PassportKey, "DisablePinRecovery", 1)],
                RemoveOps = [RegOp.DeleteValue(PassportKey, "DisablePinRecovery")],
                DetectOps = [RegOp.CheckDword(PassportKey, "DisablePinRecovery", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables cloud PIN reset; users must reset via IT admin if they forget their PIN.",
            },
            new TweakDef
            {
                Id = "devlockgpo-require-screensaver-password",
                Label = "Device Lock GPO: Require Password When Resuming from Screen Saver",
                Category = "Peripherals",
                Description =
                    "Forces Windows to require the user's password (or PIN) when resuming from a screen saver or after a period of inactivity. This is a foundational physical security control that prevents unauthorized access to unattended workstations. Without this policy, an unlocked workstation can be accessed by anyone who sits down at the keyboard.",
                Tags = ["screen saver", "lock", "password", "unattended", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DesktopKey],
                ApplyOps = [RegOp.SetString(DesktopKey, "ScreenSaverIsSecure", "1")],
                RemoveOps = [RegOp.DeleteValue(DesktopKey, "ScreenSaverIsSecure")],
                DetectOps = [RegOp.CheckString(DesktopKey, "ScreenSaverIsSecure", "1")],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents access to unattended workstations after screen saver activates.",
            },
            new TweakDef
            {
                Id = "devlockgpo-set-screensaver-timeout-600",
                Label = "Device Lock GPO: Set Screen Saver Timeout to 10 Minutes (600 s)",
                Category = "Peripherals",
                Description =
                    "Sets the screen saver / auto-lock timeout to 600 seconds (10 minutes). Industry security frameworks (CIS, NIST SP 800-53, PCI DSS) recommend an idle timeout of 10–15 minutes for standard workstations. A 10-minute timeout balances security with productivity, locking unattended machines before a brief absence creates risk.",
                Tags = ["screen saver", "timeout", "idle", "lock", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [DesktopKey],
                ApplyOps = [RegOp.SetString(DesktopKey, "ScreenSaveTimeOut", "600")],
                RemoveOps = [RegOp.DeleteValue(DesktopKey, "ScreenSaveTimeOut")],
                DetectOps = [RegOp.CheckString(DesktopKey, "ScreenSaveTimeOut", "600")],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "10-minute idle lock; CIS Benchmark recommended value for standard workstations.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-lock-screen-notifications",
                Label = "Device Lock GPO: Disable Notifications on Lock Screen",
                Category = "Peripherals",
                Description =
                    "Prevents Windows from displaying app notifications (toast notifications) on the lock screen. Lock-screen notifications can expose sensitive information to passersby — email previews, chat messages, calendar events — without requiring authentication. This policy disables all notification content from appearing while the screen is locked.",
                Tags = ["lock screen", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "NoLockScreenNotificationsTitle", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "NoLockScreenNotificationsTitle")],
                DetectOps = [RegOp.CheckDword(SystemKey, "NoLockScreenNotificationsTitle", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides notification content on the lock screen; prevents data exposure to unauthenticated viewers.",
            },
            new TweakDef
            {
                Id = "devlockgpo-disable-camera-on-lockscreen",
                Label = "Device Lock GPO: Disable Camera Access on Lock Screen",
                Category = "Peripherals",
                Description =
                    "Prevents cameras (webcams, built-in laptop cameras) from being activated while the workstation is locked. Some Windows Hello facial recognition implementations allow the camera to be used on the lock screen, but malicious code or physical manipulation could trigger unauthorized image capture. Disabling the camera on the lock screen closes this attack surface.",
                Tags = ["lock screen", "camera", "privacy", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SystemKey],
                ApplyOps = [RegOp.SetDword(SystemKey, "NoLockScreenCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(SystemKey, "NoLockScreenCamera")],
                DetectOps = [RegOp.CheckDword(SystemKey, "NoLockScreenCamera", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables camera hardware access from the lock screen; Windows Hello face recognition still works at login.",
            },
        ];
    }

    // ── DeviceProvisioningPolicy ──
    private static class _DeviceProvisioningPolicy
    {
        private const string Oobe = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\OOBE";
        private const string HomeGrp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HomeGroup";
        private const string WpjPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WorkplaceJoin";
        private const string CloudContent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "devprov-skip-machine-oobe",
                Label = "OOBE: Skip the machine out-of-box experience setup",
                Category = "Peripherals",
                Description =
                    "Sets SkipMachineOOBE=1 in the OOBE policy key. Prevents the machine-level OOBE "
                    + "wizard from running, useful for pre-provisioned enterprise devices.",
                Tags = ["oobe", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "SkipMachineOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "SkipMachineOOBE")],
                DetectOps = [RegOp.CheckDword(Oobe, "SkipMachineOOBE", 1)],
            },
            new TweakDef
            {
                Id = "devprov-skip-user-oobe",
                Label = "OOBE: Skip the user out-of-box experience setup",
                Category = "Peripherals",
                Description =
                    "Sets SkipUserOOBE=1 in the OOBE policy key. Skips the per-user OOBE wizard that "
                    + "prompts for Cortana, account sign-in, and other optional setup steps.",
                Tags = ["oobe", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "SkipUserOOBE", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "SkipUserOOBE")],
                DetectOps = [RegOp.CheckDword(Oobe, "SkipUserOOBE", 1)],
            },
            new TweakDef
            {
                Id = "devprov-no-connected-oobe",
                Label = "OOBE: Disable cloud-connected experience during OOBE",
                Category = "Peripherals",
                Description =
                    "Sets DisableOOBEWithNetworkConnectivity=1 in the OOBE policy key. Prevents the "
                    + "OOBE wizard from triggering cloud-connected steps when network connectivity is detected.",
                Tags = ["oobe", "cloud", "setup", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Oobe, "DisableOOBEWithNetworkConnectivity")],
                DetectOps = [RegOp.CheckDword(Oobe, "DisableOOBEWithNetworkConnectivity", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-homegroup",
                Label = "HomeGroup: Prevent computers from joining a HomeGroup",
                Category = "Peripherals",
                Description =
                    "Sets DisableHomeGroup=1 in the HomeGroup policy key. Prevents users from joining "
                    + "or creating HomeGroups. Recommended on domain-joined and managed devices.",
                Tags = ["homegroup", "sharing", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(HomeGrp, "DisableHomeGroup", 1)],
                RemoveOps = [RegOp.DeleteValue(HomeGrp, "DisableHomeGroup")],
                DetectOps = [RegOp.CheckDword(HomeGrp, "DisableHomeGroup", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-wpj-flyout",
                Label = "Workplace Join: Disable the 'Connect to work or school' flyout",
                Category = "Peripherals",
                Description =
                    "Sets FlyoutDisabled=1 in the WorkplaceJoin policy key. Hides the Workplace Join "
                    + "notification flyout from the Action Center and Settings entry point.",
                Tags = ["workplace-join", "flyout", "notification", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(WpjPol, "FlyoutDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(WpjPol, "FlyoutDisabled")],
                DetectOps = [RegOp.CheckDword(WpjPol, "FlyoutDisabled", 1)],
            },
            new TweakDef
            {
                Id = "devprov-disable-find-my-device",
                Label = "Cloud Content: Disable the Find My Device feature",
                Category = "Peripherals",
                Description =
                    "Sets DisableFindMyDevice=1 in the CloudContent policy key. Prevents Windows from "
                    + "registering the device with Microsoft's Find My Device location tracking service.",
                Tags = ["find-my-device", "cloud", "location", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(CloudContent, "DisableFindMyDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(CloudContent, "DisableFindMyDevice")],
                DetectOps = [RegOp.CheckDword(CloudContent, "DisableFindMyDevice", 1)],
            },
        ];
    }

    // ── DeviceRegistrationPolicy ──
    private static class _DeviceRegistrationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceRegistration";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "devreg-disable-auto-device-registration",
                    Label = "Disable Automatic Azure AD Device Registration",
                    Category = "Peripherals",
                    Description =
                        "Prevents the device from automatically registering with Azure Active Directory / Entra ID during domain join or user sign-in. Gives IT full control over when and how devices are registered. Default: auto-register on domain join. Recommended: 1 when phased registration is required.",
                    Tags = ["device-registration", "azure-ad", "entra", "mdm", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Device does not automatically register with Azure AD/Entra on domain join; manual or scripted registration is required.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-require-tpm-for-registration",
                    Label = "Require TPM for Device Registration",
                    Category = "Peripherals",
                    Description =
                        "Mandates that a TPM 2.0 chip is present and functional before the device can complete Azure AD registration. Ensures only hardware-attested devices can enrol; blocks VMs and devices without TPM. Default: TPM not required. Recommended: 1 for Zero Trust deployments.",
                    Tags = ["device-registration", "tpm", "hardware-attestation", "zero-trust", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Devices without TPM 2.0 cannot register with Azure AD; hardware attestation is mandatory.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTpmForRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmForRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTpmForRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-set-registration-retry-3",
                    Label = "Set Device Registration Retry Count to 3",
                    Category = "Peripherals",
                    Description =
                        "Limits the number of automatic re-registration attempts when initial Azure AD registration fails (e.g., due to network error) to 3 before stopping. Prevents persistent registration loops. Default: unlimited retries. Recommended: 3.",
                    Tags = ["device-registration", "retry", "enrollment", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Device stops attempting re-registration after 3 failures; reduces background registration loop network noise.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxRegistrationRetries", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxRegistrationRetries")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxRegistrationRetries", 3)],
                },
                new TweakDef
                {
                    Id = "devreg-block-personal-account-registration",
                    Label = "Block Personal MSA Device Registration",
                    Category = "Peripherals",
                    Description =
                        "Prevents users from registering the device with their personal Microsoft Account (MSA). Only corporate Azure AD / Entra accounts can register the device. Default: MSA registration allowed. Recommended: 1 on managed corporate endpoints.",
                    Tags = ["device-registration", "msa", "personal-account", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Personal MSA device registration is blocked; only Entra ID / Azure AD corporate accounts can register.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPersonalAccountDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPersonalAccountDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-disable-user-initiated-registration",
                    Label = "Block Users from Initiating Device Registration",
                    Category = "Peripherals",
                    Description =
                        "Prevents standard users from accessing the 'Join this device to Azure AD' and 'Connect to work or school' flows in Settings. Only administrators can register the device. Default: users allowed. Recommended: 1 on shared/kiosk endpoints.",
                    Tags = ["device-registration", "user-restriction", "settings", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Settings → Accounts → Access work or school registration flows are hidden for standard users.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUserInitiatedDeviceRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUserInitiatedDeviceRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-enable-registration-audit-log",
                    Label = "Enable Device Registration Audit Logging",
                    Category = "Peripherals",
                    Description =
                        "Enables Security audit events for device registration and de-registration actions. Allows SOC/SIEM correlation of device lifecycle events with user authentication. Default: not audited. Recommended: 1 in SOC-monitored environments.",
                    Tags = ["device-registration", "audit", "logging", "security", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device join/leave events are written to the Security event log; consumable by SIEM platforms.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRegistrationAudit", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRegistrationAudit")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRegistrationAudit", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-enforce-compliant-device-only",
                    Label = "Require Device Compliance for Registration",
                    Category = "Peripherals",
                    Description =
                        "Enforces that the device must meet Intune / Endpoint Manager compliance policies before completing Azure AD Hybrid registration. Non-compliant devices are blocked until they satisfy the compliance posture. Default: not enforced. Recommended: 1 for Conditional Access deployments.",
                    Tags = ["device-registration", "compliance", "intune", "conditional-access", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Non-compliant devices (missing patches, disabled Defender) cannot complete registration; gate for Conditional Access.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireDeviceCompliance", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireDeviceCompliance")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireDeviceCompliance", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-certificate-validity-days-365",
                    Label = "Set Device Certificate Validity to 365 Days",
                    Category = "Peripherals",
                    Description =
                        "Configures the maximum validity period for the device authentication certificate issued during Azure AD registration to 365 days. Forces annual certificate renewal, reducing the window of credential exposure. Default: 180 days. Recommended: 365 for balance.",
                    Tags = ["device-registration", "certificate", "validity", "renewal", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device certificates are valid for 1 year; renewal is required annually to maintain device trust.",
                    ApplyOps = [RegOp.SetDword(Key, "DeviceCertValidityDays", 365)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DeviceCertValidityDays")],
                    DetectOps = [RegOp.CheckDword(Key, "DeviceCertValidityDays", 365)],
                },
                new TweakDef
                {
                    Id = "devreg-block-stale-device-reuse",
                    Label = "Block Re-Registration of Already-Registered Device Record",
                    Category = "Peripherals",
                    Description =
                        "Prevents a device from creating a new Azure AD registration record if a record for the same device already exists (stale object). Requires IT to clean up the old object before re-registration. Default: new record created silently. Recommended: 1.",
                    Tags = ["device-registration", "stale", "reuse", "hygiene", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Device will not create a duplicate Azure AD object; IT must retire the stale record before re-registration.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStaleDeviceReRegistration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStaleDeviceReRegistration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStaleDeviceReRegistration", 1)],
                },
                new TweakDef
                {
                    Id = "devreg-disable-registration-status-page-skip",
                    Label = "Block Skipping Device Registration Status Page (OOBE)",
                    Category = "Peripherals",
                    Description =
                        "Prevents Autopilot/OOBE from skipping the device registration status page (ESP — Enrollment Status Page). Ensures the device fully completes registration before the user can log in. Default: ESP may be skipped. Recommended: 1 during Autopilot deployments.",
                    Tags = ["device-registration", "oobe", "autopilot", "esp", "enrollment", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "OOBE/Autopilot ESP is not skipped; device is fully enrolled before the first user login.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockEnrollmentStatusPageSkip")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockEnrollmentStatusPageSkip", 1)],
                },
            ];
    }

    // ── FirmwareUpdatePolicy ──
    private static class _FirmwareUpdatePolicy
    {
        private const string FwUpdateKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UEFI\FirmwareUpdate";

        private const string WuPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "fwupd-enable-firmware-update-via-wu",
                    Label = "Firmware Update: Enable UEFI Firmware Updates via Windows Update",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableFirmwareUpdates=1 in the Firmware Update policy key. Enables delivery of UEFI firmware, microcode, and driver firmware updates via the Windows Update UEFI firmware update mechanism (ESRT — UEFI System Resource Table). Microsoft and OEMs publish firmware updates as Windows Update packages. Enabling this ensures that critical security firmware updates (CPU microcode for Spectre/Meltdown, firmware CVEs, NIC firmware security patches) are delivered automatically alongside Windows updates. Without this, firmware updates must be manually applied from OEM download pages — creating a persistent firmware patching gap in enterprise environments.",
                    Tags = ["firmware", "windows-update", "esrt", "microcode", "uefi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "UEFI firmware updates delivered via Windows Update. Firmware updates are applied on next restart. In environments with strict change management, firmware updates should be deferred and tested before broad deployment (use Windows Update deferral policies for feature/quality updates, but firmware updates should be tested separately). Some firmware updates are irreversible — maintain firmware rollback documentation.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareUpdates")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareUpdates", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-require-capsule-signing",
                    Label = "Firmware Update: Require Signed Capsule Delivery for All Firmware Updates",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireCapsuleSigning=1 in the Firmware Update policy key. Requires that all UEFI firmware update capsules are digitally signed before they are accepted for delivery. An unsigned firmware update capsule (delivered via Windows Update or a local installer) that passes through this check unchallenged could be a malicious replacement firmware (firmware implant). Requiring capsule signing ensures that only OEM or Microsoft-signed firmware capsules are accepted — unauthenticated replacement firmware is rejected.",
                    Tags = ["firmware", "capsule", "signing", "firmware-implant", "uefi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Firmware update capsules must be signed. OEM firmware update tools that deliver unsigned local firmware packages will fail. All major OEM firmware updates from Dell Update, HP Support Assistant, and Lenovo System Update use signed capsules. Older OEM tools (pre-2020) may use unsigned capsule delivery.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "RequireCapsuleSigning", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "RequireCapsuleSigning")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "RequireCapsuleSigning", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-enable-firmware-version-audit",
                    Label = "Firmware Update: Enable UEFI Firmware Version Reporting to WU",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableFirmwareVersionReporting=1 in the Firmware Update policy key. Enables reporting of the current UEFI firmware version to Windows Update/SCCM/Intune. Firmware version reporting allows IT administrators to audit which firmware versions are deployed across the fleet and identify devices that are behind on firmware updates. Without this reporting, enterprise firmware version visibility requires per-device BIOS queries or WMI polling. Centralised firmware version reporting via Windows Update enables proactive identification of devices vulnerable to known firmware CVEs.",
                    Tags = ["firmware", "version-reporting", "audit", "intune", "vulnerability-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Firmware version reported to Windows Update and Intune. Firmware model and version recorded in WU client record. No PII — only hardware identifiers and firmware version. Enables firmware compliance dashboards in Intune. Not available on all OEMs — firmware version reporting depends on ESRT table availability in device firmware.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareVersionReporting", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareVersionReporting")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareVersionReporting", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-disable-os-downgrade-firmware-flag",
                    Label = "Firmware Update: Disable Firmware-Controlled OS Downgrade (SetOsIndications Clear)",
                    Category = "Peripherals",
                    Description =
                        "Sets PreventFirmwareOSIndications=1 in the Firmware Update policy key. Prevents software from setting UEFI OS Indications variables that request the firmware to perform privileged OS-downgrade or firmware recovery operations on next boot. Some firmware implementations respond to OS-set OsIndications values by entering a special recovery or setup mode. An attacker with kernel access who can write UEFI variables could set OsIndications to trigger firmware-level recovery or OS reinstallation on next boot — effectively causing a denial-of-service by re-imaging the OS. Blocking OsIndications writes prevents this DOS vector.",
                    Tags = ["firmware", "os-indications", "uefi-variables", "dos-prevention", "kernel"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "OS-controlled UEFI OsIndications blocked. Windows Recovery Environment (WinRE) and advanced boot options that use OS-firmware communication via OsIndications may be affected. Windows Update and normal OEM firmware update tools do not use OS-controlled OsIndications for normal operations — only recovery tools use this mechanism.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "PreventFirmwareOSIndications", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "PreventFirmwareOSIndications")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "PreventFirmwareOSIndications", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-set-firmware-update-scan-interval-7days",
                    Label = "Firmware Update: Set Firmware Update Scan Frequency to 7 Days",
                    Category = "Peripherals",
                    Description =
                        "Sets FirmwareUpdateScanFrequency=7 in the Firmware Update policy key (units: days). Sets the frequency at which the Windows Update client checks for new firmware updates to every 7 days. By default, firmware update check frequency follows the general Windows Update schedule. Setting an explicit 7-day cadence ensures that new firmware security updates are picked up within one week of publication — balancing prompt patching against the operational cost of weekly firmware update deployments. For high-security environments, reduce to 1–3 days.",
                    Tags = ["firmware", "update-scan", "cadence", "patch-management", "windows-update"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Firmware update check performed weekly. Devices not connected to WU for more than 7 days may miss a firmware update check cycle. No visible user impact — check runs in the background. Firm update availability does not automatically install the firmware; installation requires restart approval per the restart policy.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "FirmwareUpdateScanFrequency", 7)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "FirmwareUpdateScanFrequency")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "FirmwareUpdateScanFrequency", 7)],
                },
                new TweakDef
                {
                    Id = "fwupd-block-user-firmware-rollback",
                    Label = "Firmware Update: Block User-Initiated Firmware Version Rollback",
                    Category = "Peripherals",
                    Description =
                        "Sets BlockFirmwareRollback=1 in the Firmware Update policy key. Prevents users and non-admin processes from rolling back UEFI firmware to an older version once a newer version has been applied. Firmware rollback (to a version with known, unpatched vulnerabilities) is a prerequisite step for many firmware persistence attacks — an attacker who can roll back to a vulnerable firmware version can then exploit the known vulnerability to plant a firmware implant. Blocking rollback ensures that once a security firmware update is applied, the device cannot be returned to a less-secure firmware state by an attacker.",
                    Tags = ["firmware", "rollback-prevention", "downgrade", "firmware-implant", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Firmware version rollback blocked. If a firmware update causes hardware compatibility issues, rolling back requires physical UEFI intervention (OEM recovery tools, BIOS recovery switch). Document the rollback procedure before enabling this policy. IT must maintain OEM firmware recovery tooling and recovery USB media for all device models in fleet.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "BlockFirmwareRollback", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "BlockFirmwareRollback")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "BlockFirmwareRollback", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-enable-firmware-update-eventlog",
                    Label = "Firmware Update: Enable Firmware Update Event Logging to Windows Event Log",
                    Category = "Peripherals",
                    Description =
                        "Sets EnableFirmwareUpdateEventLog=1 in the Firmware Update policy key. Enables logging of UEFI firmware update application events to the Windows event log (System event log, source: UFIUpdate). Events include the firmware component updated, the from/to version, the update status (success/failure), and the reason for failure if applicable. Firmware update event logging supports change management tracking — every firmware update on any enterprise device generates an auditable event that SIEM and asset management tools can correlate with approved firmware change records.",
                    Tags = ["firmware", "event-log", "audit", "change-management", "siem"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Firmware update events logged to Windows event log. Events appear in System log with source UFIUpdate. Minimal event volume — firmware updates are infrequent. SIEM rules can correlate unexpected firmware changes (unapproved firmware version change) as a security anomaly indicator.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "EnableFirmwareUpdateEventLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "EnableFirmwareUpdateEventLog")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "EnableFirmwareUpdateEventLog", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-require-admin-for-manual-firmware-update",
                    Label = "Firmware Update: Require Admin Approval for Manual Firmware Update Execution",
                    Category = "Peripherals",
                    Description =
                        "Sets RequireAdminForFirmwareUpdate=1 in the Firmware Update policy key. Requires administrator approval before a manually initiated firmware update (e.g., via OEM firmware update tool run locally) can execute. Without this requirement, a standard user who can run an OEM firmware update utility can potentially flash a modified firmware — especially if the OEM tool accepts a firmware image file path. Requiring admin approval ensures that firmware updates not delivered via Windows Update must be explicitly authorised by an administrator, preventing social engineering attacks where users are deceived into running a firmware installer.",
                    Tags = ["firmware", "admin-approval", "manual-update", "uac", "social-engineering"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Manual firmware updates require administrator approval. Standard users who attempt to run OEM firmware update tools receive UAC elevation prompts. All firmware updates via Windows Update (capsule delivery) are performed by the Windows Update service under SYSTEM and are not affected.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "RequireAdminForFirmwareUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "RequireAdminForFirmwareUpdate")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "RequireAdminForFirmwareUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "fwupd-set-firmware-update-defer-days-14",
                    Label = "Firmware Update: Set Firmware Update Deferral to 14 Days After Release",
                    Category = "Peripherals",
                    Description =
                        "Sets DeferFirmwareUpdatesDays=14 in the Windows Update policy key. Defers firmware update installation by 14 days from the date Microsoft or OEM first publishes them to Windows Update. The 14-day deferral period allows time for reported deployment issues to surface and for regression reports to be filed before the update reaches the enterprise fleet. Firmware updates with critical compatibility issues (e.g., BSOD-inducing microcode updates, display driver firmware breaking external monitors) are often reported in the first 3–7 days post-release. 14 days provides a reasonable canary window without creating an unacceptable security gap.",
                    Tags = ["firmware", "deferral", "quality-gate", "canary", "windows-update"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Firmware updates deferred 14 days from release. Security-critical firmware updates (Spectre-class microcode) are delayed for 14 days. For high-severity firmware CVEs, consider reducing or bypassing the deferral via Windows Update exemption group. For routine firmware maintenance updates, 14 days is appropriate.",
                    ApplyOps = [RegOp.SetDword(WuPolicyKey, "DeferFirmwareUpdatesDays", 14)],
                    RemoveOps = [RegOp.DeleteValue(WuPolicyKey, "DeferFirmwareUpdatesDays")],
                    DetectOps = [RegOp.CheckDword(WuPolicyKey, "DeferFirmwareUpdatesDays", 14)],
                },
                new TweakDef
                {
                    Id = "fwupd-enable-legacy-bios-update-block",
                    Label = "Firmware Update: Block Legacy BIOS (Non-UEFI) Firmware Update Installation",
                    Category = "Peripherals",
                    Description =
                        "Sets BlockLegacyBiosUpdate=1 in the Firmware Update policy key. Prevents the installation of firmware updates designed for legacy BIOS (MBR/CSM-mode) systems. Enterprise Secure Boot and UEFI-based systems should not accept legacy BIOS firmware packages — they are built for different firmware architectures. A malicious actor who delivers a forged 'legacy BIOS update' to a UEFI system may attempt to exploit the UEFI CSM (Compatibility Support Module) or subvert firmware update routing. Blocking legacy BIOS updates ensures only proper UEFI capsule updates are accepted.",
                    Tags = ["firmware", "legacy-bios", "csm", "update-filter", "uefi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Legacy BIOS firmware updates blocked. Only UEFI capsule firmware updates accepted. Relevant only for organisations with mixed BIOS/UEFI fleets. On UEFI-native systems (post-2015 hardware), this policy has no practical impact since legacy BIOS updates are not delivered to UEFI systems by default.",
                    ApplyOps = [RegOp.SetDword(FwUpdateKey, "BlockLegacyBiosUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(FwUpdateKey, "BlockLegacyBiosUpdate")],
                    DetectOps = [RegOp.CheckDword(FwUpdateKey, "BlockLegacyBiosUpdate", 1)],
                },
            ];
    }

    // ── HardwareDevicePolicy ──
    private static class _HardwareDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "hwdev-prevent-unknown-install",
                Label = "Prevent Installation of Devices Not Described by Other Policies",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "Windows device installation policies can be configured to deny installation of any device not explicitly permitted by other device policy rules. Enabling this restriction prevents users and even administrators from installing hardware not covered by an explicit allow rule. This whitelist enforcement model requires that all permitted device types and IDs be enumerated in device installation policies first. Without a comprehensive allow list this setting will block most device installations and degrade usability significantly. Enterprise USB security programs use this setting combined with device ID allow lists to create an approved-device-only environment. This is one of the most effective controls for preventing data theft via USB mass storage insertion.",
                Tags = ["hardware", "device-install", "usb", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies")],
                DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfDevicesNotDescribedByOtherPolicies", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-prevent-removable-install",
                Label = "Prevent Installation of Removable Devices",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Removable devices such as USB drives, external hard disks, and SD card readers represent significant data exfiltration and malware introduction vectors. Preventing the installation of removable devices blocks new removable storage from being registered and usable on managed endpoints. This is a critical DLP control in environments handling classified, sensitive, or regulated data. Previously installed removable devices are not affected until the next reinstallation attempt on a clean device state. Enterprise environments with legitimate removable storage requirements should use device ID allow lists in conjunction with this policy. Endpoint users requiring USB connectivity for approved devices should use centrally managed device exemptions.",
                Tags = ["hardware", "removable", "usb", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreventInstallationOfRemovableDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreventInstallationOfRemovableDevices")],
                DetectOps = [RegOp.CheckDword(Key, "PreventInstallationOfRemovableDevices", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-drv-store-copy",
                Label = "Disable Device Driver File Store Copy",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows maintains a driver file store in the DriverStore which preserves driver packages after installation. Disabling the copy of device driver files to the driver store prevents accumulation of driver packages in the system-managed store. A large driver store consumes significant disk space and may include drivers for hardware no longer present. Enterprise systems with controlled hardware configurations do not benefit from storing drivers for every previously connected device. Reducing driver store size improves disk utilization on systems with limited storage capacity. Driver management can be handled through centralized driver deployment tools rather than per-device accumulation.",
                Tags = ["hardware", "drivers", "storage", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDriverStoreCopy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverStoreCopy")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDriverStoreCopy", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-windows-update-drivers",
                Label = "Disable Driver Download from Windows Update",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows Update includes a driver distribution service that automatically downloads and installs drivers for newly connected hardware. Disabling driver downloads from Windows Update prevents unapproved drivers from being retrieved and installed from external sources. Enterprise driver management requires that all deployed drivers be tested, signed, and distributed through controlled channels. Automatic driver downloads from Windows Update bypass change management processes for hardware driver updates. Driver updates should go through IT validation to ensure compatibility with enterprise applications and security baseline compliance. Approved drivers should be distributed through SCCM, Intune, or similar management tools after validation.",
                Tags = ["hardware", "drivers", "windows-update", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdateDriverSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdateDriverSearch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdateDriverSearch", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-generic-drivers",
                Label = "Disable Installation of Generic USB Drivers",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Generic USB drivers provide basic functionality for USB devices that do not have device-specific drivers available. Disabling generic USB driver installation prevents Windows from loading fallback generic drivers for unrecognized USB devices. Unrecognized USB devices loaded through generic drivers have not been validated against enterprise security and compatibility requirements. Generic drivers may allow partial functionality of unauthorized USB devices that would otherwise be blocked. Enterprise USB whitelisting programs are more effective when generic driver loading is also disabled. Specifically approved and tested device-specific drivers should be deployed for all hardware requiring USB connectivity.",
                Tags = ["hardware", "usb", "drivers", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGenericUsbDriverInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGenericUsbDriverInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGenericUsbDriverInstall", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-dev-metadata-internet",
                Label = "Disable Device Metadata Retrieval from Internet",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows retrieves device metadata from the Windows Metadata and Internet Services (WMIS) to display enhanced device information and icons in Device Manager. Disabling internet-based device metadata retrieval prevents hardware identifiers and device types from being sent to Microsoft's metadata services. Device type and hardware model information sent to external services represents sensitive asset inventory data in enterprise environments. Device metadata retrieval is unnecessary for functional device operation and serves primarily a cosmetic purpose. Enterprise device management information should be sourced from SCCM or Intune inventory rather than consumer-facing metadata services. Disabling this has no impact on device driver functionality, only on display metadata in Device Manager.",
                Tags = ["hardware", "metadata", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDeviceMetadataFromNetwork", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceMetadataFromNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDeviceMetadataFromNetwork", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-auto-install",
                Label = "Disable Automatic Device Installation",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Windows automatically installs drivers for newly connected hardware, even for devices not previously approved by IT. Disabling automatic device installation requires administrator action before any new hardware device becomes functional. Preventing silent automatic installation ensures that device driver changes go through change management review. Unapproved hardware connected to enterprise endpoints may load drivers that conflict with security software or introduce vulnerabilities. Automatic installation of USB HID devices including keyboards and mice in PnP scenarios can be used to launch HID injection attacks. Disabling automatic installation provides a choke point to validate new hardware before it becomes operational.",
                Tags = ["hardware", "installation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-devinstall-telemetry",
                Label = "Disable Device Installation Telemetry",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Device installation telemetry reports hardware type information, driver versions, and installation success or failure data to Microsoft. This data helps Microsoft improve driver compatibility and the Windows hardware support experience. Disabling device installation telemetry prevents hardware inventory information from being transmitted during the device installation process. The list of hardware connected to enterprise endpoints constitutes sensitive infrastructure information. Asset inventory data should be managed through enterprise CMDB and MDM tools rather than telemetry pipelines. Device installation and driver function continue to operate normally regardless of this telemetry setting.",
                Tags = ["hardware", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInstallTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInstallTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInstallTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-dev-setup-exceptions",
                Label = "Disable Device Setup Class Exceptions",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Device setup class exceptions allow specific device categories to bypass general device installation restrictions. Disabling setup class exceptions prevents broad category-level exemptions from overriding restrictive device installation policies. Exception-based exemptions for device classes like network adapters or HID devices can inadvertently permit unauthorized device types. A strict enforcement model without exceptions provides more predictable and auditable device control outcomes. Enterprise device programs should use explicit device ID allow lists rather than broad class exemptions. Disabling class exceptions requires that all permitted devices be explicitly enumerated in device allow lists rather than relying on category-level bypass.",
                Tags = ["hardware", "device-class", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSetupClassExceptions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSetupClassExceptions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSetupClassExceptions", 1)],
            },
            new TweakDef
            {
                Id = "hwdev-disable-drv-search-online",
                Label = "Disable Online Driver Search",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows driver installation can search online sources including Windows Update and third-party driver repositories for device drivers. Disabling online driver search prevents the operating system from reaching external sources to find drivers for newly connected hardware. External driver repositories are not subject to the same security validation requirements as enterprise-managed driver packages. Drivers obtained from uncontrolled online sources may contain malware, poorly implemented security controls, or known vulnerabilities. Enterprise hardware configurations should only use drivers sourced from the device manufacturer and validated by IT. Disabling online search ensures all driver installations use only drivers present in the local driver store or distributed through managed channels.",
                Tags = ["hardware", "drivers", "online", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineDriverSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineDriverSearch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineDriverSearch", 1)],
            },
        ];
    }

    // ── KernelDmaProtectionPolicy ──
    private static class _KernelDmaProtectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Kernel DMA Protection";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DmaSecurity";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "kdmapol-block-pre-boot-dma",
                    Label = "Block Pre-Boot DMA Access on Thunderbolt Ports",
                    Category = "Peripherals",
                    Description =
                        "Blocks all Thunderbolt DMA access during the pre-boot phase, preventing attacks that attach malicious Thunderbolt devices before the OS IOMMU policy is loaded.",
                    Tags = ["kernel-dma", "thunderbolt", "pre-boot", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Pre-boot Thunderbolt DMA blocked; only authorised devices can perform DMA after OS IOMMU policy loads.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowFlexibleLinkPowerManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowFlexibleLinkPowerManagement", 0)],
                },
                new TweakDef
                {
                    Id = "kdmapol-enforce-iommu-all-devices",
                    Label = "Enforce IOMMU DMA Remapping for All PCIe Devices",
                    Category = "Peripherals",
                    Description =
                        "Requires IOMMU DMA remapping to be applied to all PCIe devices regardless of whether they declare DMA support, ensuring legacy storage and network cards are also isolated.",
                    Tags = ["kernel-dma", "iommu", "all-devices", "pcie", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "IOMMU applied universally; legacy PCIe cards may show reduced throughput due to remapping overhead.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnforceIOMMUForAllDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnforceIOMMUForAllDevices")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnforceIOMMUForAllDevices", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-dma-resume-attack",
                    Label = "Block DMA Attack During Sleep/Resume Transition",
                    Category = "Peripherals",
                    Description =
                        "Maintains IOMMU DMA remapping tables across system sleep/hibernate and resume cycles, preventing DMA attacks that exploit the window during which remapping tables are reloaded.",
                    Tags = ["kernel-dma", "sleep", "resume", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "DMA remapping persists across S3/S4 transitions; resume-time DMA attacks blocked.",
                    ApplyOps = [RegOp.SetDword(Key2, "MaintainRemappingOnResume", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "MaintainRemappingOnResume")],
                    DetectOps = [RegOp.CheckDword(Key2, "MaintainRemappingOnResume", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-restrict-tb-autorisation",
                    Label = "Restrict Thunderbolt Authorisation to Admin-Only",
                    Category = "Peripherals",
                    Description =
                        "Restricts the authorisation of new Thunderbolt devices (adding to the trusted device store) to administrators only, preventing standard users from approving new DMA-capable Thunderbolt peripherals.",
                    Tags = ["kernel-dma", "thunderbolt", "authorisation", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot authorise new Thunderbolt devices; admin approval required for each new TB peripheral.",
                    ApplyOps = [RegOp.SetDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RestrictThunderboltAuthToAdmin")],
                    DetectOps = [RegOp.CheckDword(Key2, "RestrictThunderboltAuthToAdmin", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-enable-dma-audit-log",
                    Label = "Enable DMA Remapping Audit Event Logging",
                    Category = "Peripherals",
                    Description =
                        "Enables kernel event logging for DMA remapping policy enforcement actions, recording each blocked or remapped DMA access attempt for forensic analysis.",
                    Tags = ["kernel-dma", "audit-log", "iommu", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DMA remapping events logged; blocked DMA attempts visible in Security/System event log.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableDMAAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableDMAAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableDMAAuditLog", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-expresscard-dma",
                    Label = "Block ExpressCard/PCMCIA DMA Access",
                    Category = "Peripherals",
                    Description =
                        "Blocks DMA access for legacy ExpressCard and PCMCIA devices that pre-date IOMMU support, preventing DMA attacks via older expansion card interfaces on laptops.",
                    Tags = ["kernel-dma", "expresscard", "pcmcia", "legacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ExpressCard/PCMCIA DMA blocked; legacy expansion cards operate in PIO mode only.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockExpressCardDMA", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockExpressCardDMA")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockExpressCardDMA", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-require-vtd-for-tb4",
                    Label = "Require VT-d Active for Thunderbolt 4 Operation",
                    Category = "Peripherals",
                    Description =
                        "Requires Intel VT-d (IOMMU) to be active and enforcing before Thunderbolt 4 devices are enumerated and allowed DMA access, blocking TB4 use on systems with IOMMU disabled in BIOS.",
                    Tags = ["kernel-dma", "vtd", "thunderbolt-4", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TB4 blocked if VT-d disabled; BIOS must enable IOMMU for Thunderbolt 4 to function.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireVTdForTB4", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireVTdForTB4")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireVTdForTB4", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-block-usb4-dma-without-auth",
                    Label = "Block USB4 DMA Without Device Authorisation",
                    Category = "Peripherals",
                    Description =
                        "Requires explicit device authorisation for USB4 (Thunderbolt-tunnelled) DMA access, blocking USB4 tunnelled DMA from unapproved devices until confirmed by an administrator.",
                    Tags = ["kernel-dma", "usb4", "thunderbolt", "authorisation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "USB4 DMA blocked until device is admin-authorised; unauthorised USB4 devices cannot perform DMA.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockUSB4DMAWithoutAuth")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockUSB4DMAWithoutAuth", 1)],
                },
                new TweakDef
                {
                    Id = "kdmapol-set-remapping-timeout",
                    Label = "Set DMA Remapping Table Rebuild Timeout to 5 Seconds",
                    Category = "Peripherals",
                    Description =
                        "Limits the DMA remapping table rebuild timeout to 5 seconds, ensuring that if a device fails to initialise IOMMU remapping within the timeout, it is disconnected rather than granted unrestricted DMA.",
                    Tags = ["kernel-dma", "remapping", "timeout", "iommu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DMA devices failing IOMMU init are disconnected after 5 seconds; no silent fallback to unrestricted DMA.",
                    ApplyOps = [RegOp.SetDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RemappingTableRebuildTimeoutSec")],
                    DetectOps = [RegOp.CheckDword(Key2, "RemappingTableRebuildTimeoutSec", 5)],
                },
            ];
    }

    // ── MemoryDiagnostics ──
    private static class _MemoryDiagnostics
    {
        private const string CrashControl = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\CrashControl";

        private const string Wer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting";

        private const string WerQueue = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Windows Error Reporting\Queue";

        private const string WerConsentPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

        private const string WerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "dump-disable-wer-queue",
                Label = "Disable WER Queuing of Crash Reports",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["wer", "queue", "privacy", "error reporting"],
                Description =
                    "Prevents WER from queuing crash reports for later upload. Stops "
                    + "the background spool of error data that WER accumulates when an "
                    + "internet connection is unavailable.",
                ApplyOps = [RegOp.SetDword(WerQueue, "Disable", 1)],
                RemoveOps = [RegOp.SetDword(WerQueue, "Disable", 0)],
                DetectOps = [RegOp.CheckDword(WerQueue, "Disable", 1)],
            },
        ];
    }

    // ── PageFilePolicy ──
    private static class _PageFilePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PageFile";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pgfpol-ensure-pagefile-enabled",
                Label = "Ensure Page File Is Enabled",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The Windows page file provides virtual memory overflow capacity by extending RAM with disk storage. Ensuring the page file is not forcibly disabled prevents out-of-memory conditions that can crash applications and the operating system. This policy verifies that DisablePageFile is set to zero, meaning the page file is permitted to exist. Systems processing large datasets, running virtual machines, or hosting multiple applications depend on adequate virtual memory. Removing the page file entirely can cause critical system failures on memory-constrained workloads. Maintaining this setting at its safe default ensures system stability across diverse workload profiles.",
                Tags = ["pagefile", "memory", "stability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePageFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePageFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-clear-pagefile-shutdown",
                Label = "Clear Page File at Shutdown",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "The page file can retain sensitive data written there by applications during a session, including encryption keys, passwords, and confidential documents. Clearing the page file at shutdown overwrites the page file contents with zeros, preventing data recovery from the swap space. This is a security hardening measure required by many compliance frameworks including NIST, CIS, and DoD STIGs. The clearing operation adds time to the shutdown sequence proportional to the page file size and storage speed. Systems with large page files on slow HDDs may experience noticeably longer shutdown times. On SSDs the performance impact is minimal, and the security benefit justifies the small delay in all regulated environments.",
                Tags = ["pagefile", "security", "shutdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ClearPageFileAtShutdown", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClearPageFileAtShutdown")],
                DetectOps = [RegOp.CheckDword(Key, "ClearPageFileAtShutdown", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-ensure-swapfile-active",
                Label = "Ensure Swap File Is Active",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Windows swap file is a separate virtual memory file used for background application paging on modern Windows versions. Ensuring the swap file is not disabled preserves the operating system's ability to handle memory pressure through multiple paging mechanisms. This policy sets DisableSwapFile to zero, confirming the swap file remains active for background app memory management. Disabling the swap file on RAM-constrained systems can lead to application termination under memory pressure. The swap file works in conjunction with the page file to provide comprehensive virtual memory management. Maintaining this setting at its default ensures predictable memory behavior for suspended applications.",
                Tags = ["pagefile", "swapfile", "memory", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSwapFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSwapFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSwapFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-set-max-size-4096",
                Label = "Set Maximum Page File Size 4096 MB",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Setting a maximum page file size prevents the page file from growing unboundedly and consuming excessive disk space on system drives. A 4096 MB maximum represents a balanced limit suitable for most enterprise workstations with 16 GB or more of installed RAM. Unbounded page file growth can fill system drives and trigger low-disk-space conditions that destabilize the operating system. This setting must be calibrated against the actual workload requirements of the target machine class. Memory-intensive workloads such as database servers, virtualization hosts, and development environments may require higher limits. Administrators should monitor memory usage before applying this limit to production systems.",
                Tags = ["pagefile", "memory", "disk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxPageFileSize", 4096)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxPageFileSize")],
                DetectOps = [RegOp.CheckDword(Key, "MaxPageFileSize", 4096)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-peak-detection",
                Label = "Disable Automatic Peak Page File Detection",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows automatically adjusts the page file size based on observed peak memory usage patterns over time. This adaptive mechanism can cause the page file to fluctuate in size, leading to disk fragmentation on HDD systems. Disabling automatic peak detection freezes the page file at its configured size, providing predictable storage consumption. Administrators managing server environments with known memory requirements benefit from deterministic page file sizing. The adaptive mechanism is primarily beneficial on consumer devices with widely varying workloads. Enterprise systems with stable application loads gain more from a fixed, well-configured page file size than from dynamic adjustment.",
                Tags = ["pagefile", "memory", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticPeakDetection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticPeakDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticPeakDetection", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-allow-system-managed",
                Label = "Allow System-Managed Page File",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The system-managed page file allows Windows to dynamically size and manage the page file based on available disk space and observed usage patterns. Allowing system management ensures the page file automatically adjusts to unusual workload spikes that exceed manually configured sizes. This policy sets DisableSystemManagedPageFile to zero, preserving the default system management behavior. Organizations relying on Windows to handle memory management automatically benefit from this setting on general-purpose workstations. Environments with strict resource governance may prefer manual page file sizing, but system management provides a reliable fallback. This setting is safe and recommended unless a specific maximum size policy is required.",
                Tags = ["pagefile", "memory", "system", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSystemManagedPageFile", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSystemManagedPageFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSystemManagedPageFile", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-allow-low-memory-detection",
                Label = "Allow Low Memory Detection",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Low memory detection triggers warnings and system responses when available physical and virtual memory falls below critical thresholds. Maintaining the low memory detection mechanism active at its default ensures the system can respond appropriately to memory pressure. This policy sets DisableLowMemoryDetection to zero, preserving protective system behavior. With detection enabled, the system can proactively terminate unresponsive processes and log diagnostic information before a complete memory exhaustion event. Disabling detection removes the safety net and can cause sudden application crashes or system hangs without warning. This setting should remain at the safe default on all production systems.",
                Tags = ["pagefile", "memory", "stability", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLowMemoryDetection", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLowMemoryDetection")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLowMemoryDetection", 0)],
            },
            new TweakDef
            {
                Id = "pgfpol-place-on-system-drive",
                Label = "Place Page File on System Drive",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Placing the page file on the system drive keeps virtual memory on the fastest and most reliable storage device in most configurations. The system drive typically hosts the operating system on an NVMe or SATA SSD with superior I/O characteristics. This policy ensures the page file is configured to use the system drive, providing consistent performance for virtual memory operations. Keeping the page file on the system drive also simplifies disk management by avoiding dependency on secondary drives. On multi-drive configurations, administrators may prefer secondary drives for page file placement to reduce I/O contention on the system drive. This setting is appropriate for workstations with a single high-performance storage device.",
                Tags = ["pagefile", "storage", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PageFileOnSystemDrive", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PageFileOnSystemDrive")],
                DetectOps = [RegOp.CheckDword(Key, "PageFileOnSystemDrive", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-telemetry",
                Label = "Disable Page File Telemetry",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Page file telemetry transmits metrics about virtual memory usage patterns, page fault rates, and page file sizing to Microsoft. This data provides Microsoft with insight into memory pressure scenarios across the Windows user base. Disabling page file telemetry prevents virtual memory utilization data from being transmitted to external services. Organizations with strict data residency requirements or network egress monitoring policies benefit from disabling this telemetry. The page file continues to function identically regardless of whether telemetry is enabled. Administrators can obtain equivalent memory usage insights through Windows Performance Monitor and ETW tracing.",
                Tags = ["pagefile", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePageFileTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePageFileTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePageFileTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "pgfpol-disable-memory-dump",
                Label = "Disable Memory Dump Creation",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Memory dumps capture the contents of RAM to disk following a system crash and are stored in the page file or dedicated dump files. These dump files can contain sensitive data including credentials, encryption keys, and application data present in memory at crash time. Disabling memory dump creation prevents sensitive memory contents from being written to disk where they could be extracted. Security-hardened environments and regulated industries often disable memory dumps as part of data-at-rest protection policies. The tradeoff is reduced diagnostic capability for analyzing crash root causes in post-incident investigations. Environments with stringent memory protection requirements should disable dumps and rely on live debugging or remote crash analysis tools.",
                Tags = ["pagefile", "memory-dump", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMemoryDump", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMemoryDump")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMemoryDump", 1)],
            },
        ];
    }

    // ── PortableDevicePolicy ──
    private static class _PortableDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceInstall\Restrictions";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpd-deny-portable-devices",
                    Label = "Deny Portable Device (MTP/WPD) Access",
                    Category = "Peripherals",
                    Description =
                        "Blocks access to Windows Portable Devices (WPD) via the Media Transfer Protocol (MTP), preventing smartphones, cameras, and media players from accessing or transferring files when connected via USB.",
                    Tags = ["wpd", "mtp", "portable-device", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WPD MTP access blocked; phones/cameras connected via USB cannot transfer files.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyPortableDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyPortableDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyPortableDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-autoplay-portable",
                    Label = "Block AutoPlay for Portable Media Devices",
                    Category = "Peripherals",
                    Description =
                        "Disables the AutoPlay action that launches when a portable device (camera, media player, phone) is connected, preventing automatic media import dialogs and auto-execution of content from portable devices.",
                    Tags = ["wpd", "autoplay", "portable-device", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "AutoPlay disabled for portable devices; no media import dialog when connecting cameras or phones.",
                    ApplyOps = [RegOp.SetDword(Key, "NoAutoplayForPortableDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoAutoplayForPortableDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "NoAutoplayForPortableDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-camera-device-install",
                    Label = "Block Camera Device Installation",
                    Category = "Peripherals",
                    Description =
                        "Prevents installation of USB-connected camera devices (webcams, digital cameras) on the system, useful in secure environments where all photography and video capture must be blocked.",
                    Tags = ["wpd", "camera", "usb", "device-install", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "USB camera devices blocked from installing; no external webcam or digital camera usable.",
                    ApplyOps = [RegOp.SetDword(Key, "DenyCameraDevices", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DenyCameraDevices")],
                    DetectOps = [RegOp.CheckDword(Key, "DenyCameraDevices", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-disable-picture-transfer",
                    Label = "Disable Windows Picture Transfer Protocol",
                    Category = "Peripherals",
                    Description =
                        "Disables the Picture Transfer Protocol (PTP) used by digital cameras and smartphones to transfer photos, preventing photo device discovery and auto-import from cameras.",
                    Tags = ["wpd", "ptp", "picture-transfer", "camera", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PTP picture transfer disabled; digital cameras and phones in PTP mode cannot transfer photos.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePTPTransfer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePTPTransfer")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePTPTransfer", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-apply-audit-on-write",
                    Label = "Enable Write Audit Logging for Removable Media",
                    Category = "Peripherals",
                    Description =
                        "Enables security audit events when files are written to removable storage devices, creating a log trail for data being exfiltrated to USB drives or portable devices.",
                    Tags = ["wpd", "removable-media", "audit-log", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Write-to-removable-media events logged; potential data exfiltration to USB drives is auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditRemovableMediaWrites", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditRemovableMediaWrites")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditRemovableMediaWrites", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-disable-usb-mass-storage",
                    Label = "Disable USB Mass Storage Class Driver",
                    Category = "Peripherals",
                    Description =
                        "Disables the USB Mass Storage class driver (usbstor), preventing USB flash drives, external hard drives, and USB memory sticks from mounting as drive letters in Windows Explorer.",
                    Tags = ["wpd", "usb-mass-storage", "usbstor", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "USB mass storage driver disabled; USB drives, external HDDs, and flash drives cannot be used.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUSBMassStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUSBMassStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUSBMassStorage", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-portable-music-player",
                    Label = "Block Portable Music Player Synchronisation",
                    Category = "Peripherals",
                    Description =
                        "Blocks synchronisation between Windows media players (Windows Media Player sync) and portable MP3 or music players via USB, preventing media content from being exported to portable devices.",
                    Tags = ["wpd", "music-player", "sync", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Portable music player sync blocked; cannot sync music files from WMP to MP3 players.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPortableMusicPlayerSync", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPortableMusicPlayerSync")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPortableMusicPlayerSync", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-readonly-removable-media",
                    Label = "Enforce Read-Only Mode for All Removable Media",
                    Category = "Peripherals",
                    Description =
                        "Mounts all removable storage devices in read-only mode, allowing data to be read from USB drives but blocking any write operations to prevent data exfiltration.",
                    Tags = ["wpd", "removable-media", "read-only", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Removable media read-only; cannot write files to USB drives or SD cards.",
                    ApplyOps = [RegOp.SetDword(Key, "ReadOnlyRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ReadOnlyRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(Key, "ReadOnlyRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "wpd-block-external-thunderbolt-storage",
                    Label = "Block External Thunderbolt Storage Devices",
                    Category = "Peripherals",
                    Description =
                        "Prevents external storage devices connected via Thunderbolt from mounting, closing the high-speed Thunderbolt exfiltration path while allowing other Thunderbolt peripherals like displays and docks.",
                    Tags = ["wpd", "thunderbolt", "storage", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Thunderbolt external storage blocked; TB drives and NVMe enclosures cannot mount.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockThunderboltStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockThunderboltStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockThunderboltStorage", 1)],
                },
            ];
    }

    // ── PortableDevicesPolicy ──
    private static class _PortableDevicesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PortableDevices";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "portdev-disable-autoplay",
                Label = "Disable AutoPlay for Portable Devices",
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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
                Category = "Peripherals",
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

    // ── ProcessorPolicy ──
    private static class _ProcessorPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Processor";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "proccpol-disable-speculative-execution",
                Label = "Enable Spectre/Meltdown Mitigations",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Spectre and Meltdown are hardware vulnerabilities in modern processors that allow malicious code to read arbitrary memory through side-channel attacks. Enabling processor mitigations activates kernel and firmware-level protections that prevent exploitation of these speculative execution vulnerabilities. Without mitigations enabled malicious processes can read kernel memory, other process memory, and hypervisor memory they should not have access to. Intel, AMD, and ARM all released firmware and microcode updates to address these vulnerabilities when combined with OS-level mitigations. Performance impact from mitigations varies by workload but security benefits far outweigh the performance cost for enterprise endpoints. Mitigations must be enabled both through OS policy and microcode/firmware updates for complete protection.",
                Tags = ["processor", "spectre", "meltdown", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-retpoline",
                Label = "Enable Retpoline Spectre Variant 2 Mitigation",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Retpoline is a software mitigation technique that replaces indirect branch instructions with a safer equivalent that prevents branch target injection. Enabling Retpoline activates the compiler-based mitigation for Spectre variant 2 branch target injection vulnerabilities. Spectre variant 2 allows malicious code to manipulate CPU branch prediction to speculatively execute code at arbitrary locations. Retpoline provides Spectre variant 2 protection with significantly lower performance overhead than alternative mitigations. Windows builds include Retpoline when supported by the system configuration including processor microcode and OS version. Retpoline is the preferred mitigation approach and should be enabled wherever the required processor and system support is present.",
                Tags = ["processor", "spectre", "retpoline", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRetpoline", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRetpoline")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRetpoline", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-kva-shadowing",
                Label = "Enable Kernel VA Shadowing (Meltdown Mitigation)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Kernel Virtual Address Shadowing separates kernel and user address spaces to prevent user-mode code from accessing kernel memory through Meltdown. KVA Shadowing ensures that kernel pages are not mapped into user process address space preventing Meltdown-style reads of kernel data. Meltdown allows user processes to read arbitrary kernel memory including passwords, encryption keys, and other sensitive data. KVA Shadowing was introduced in Windows 10 1803 as the primary Meltdown mitigation for Intel CPUs. AMD CPUs are generally not vulnerable to Meltdown but enabling KVA Shadowing provides defense-in-depth. KVA Shadowing does have a performance impact on workloads with frequent kernel transitions but the security benefit is essential.",
                Tags = ["processor", "meltdown", "kva", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableKvaShadowing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableKvaShadowing")],
                DetectOps = [RegOp.CheckDword(Key, "EnableKvaShadowing", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ssbd",
                Label = "Enable Speculative Store Bypass Disable (SSBD)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Speculative Store Bypass is a CPU vulnerability where speculative execution can bypass store-to-load forwarding and read stale data from memory. Enabling SSBD activates hardware mitigation via the SSBD MSR bit that prevents speculative access to data from prior stores. Speculative Store Bypass can be exploited to read data that should have been overwritten or isolated by store operations. SSBD is required for JIT-compiled execution environments including browser JavaScript engines where multiple execution contexts share a process. Enterprise endpoints running JavaScript-based enterprise applications may be vulnerable through browser JIT compilation without SSBD. SSBD has low performance impact and should be enabled on all processors that support the SSBD hardware bit.",
                Tags = ["processor", "ssbd", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSSBD", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSSBD")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSSBD", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-mds-mitigations",
                Label = "Enable Microarchitectural Data Sampling Mitigations",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Microarchitectural Data Sampling vulnerabilities including RIDL and Fallout allow processes to sample data from CPU internal buffers during speculative execution. Enabling MDS mitigations activates CPU buffer clearing operations that flush microarchitectural buffers to prevent cross-domain data leakage. MDS attacks can leak data across process boundaries, hypervisor boundaries, and between SMT sibling threads in Intel processors. On systems with Hyper-Threading or SMT enabled MDS mitigations may include disabling SMT for complete protection. Intel CPUs from Cascade Lake and later include hardware mitigations that reduce the performance impact of software MDS mitigations. MDS mitigations should be enabled on all Intel processors that do not have hardware MDS mitigations built in.",
                Tags = ["processor", "mds", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableMDSMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableMDSMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableMDSMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-disable-hyper-threading-spectre",
                Label = "Configure SMT for Speculative Execution Safety",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Simultaneous Multi-Threading shares processor resources between logical cores which creates side-channel leakage paths for speculative execution attacks. Configuring SMT safely for speculative execution ensures that sibling thread data is isolated through appropriate microarchitectural mitigations. MDS and cache-based side-channel attacks are more effective when the attacker and victim share an SMT core. For extremely high-security workloads where perfect isolation is required SMT disabling may be considered despite the performance impact. Modern processor microcode combined with OS MDS mitigations provides substantial SMT isolation that covers most enterprise threat models. Security teams should evaluate whether remaining SMT-based side-channel exposure is within tolerance for their specific threat environment.",
                Tags = ["processor", "smt", "speculative", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ConfigureSMTForSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ConfigureSMTForSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "ConfigureSMTForSecurity", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-tsx-mitigations",
                Label = "Enable TSX Asynchronous Abort Mitigations",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "TSX Asynchronous Abort is an Intel CPU vulnerability where transactional synchronization extensions can leak data during transactional abort handling. Enabling TAA mitigations prevents exploitation of TSX Asynchronous Abort vulnerabilities through VERW instruction flushing of CPU buffers. TAA is closely related to MDS vulnerabilities and requires similar buffer-clearing mitigations. Systems with Intel TSX disabled through microcode updates are protected against TAA but TAA mitigations should also be enabled as defense-in-depth. The TAA mitigation VERW instruction overhead is minimal on processors that support the enhanced TAA mitigation capability. TAA mitigations do not affect functionality and should be enabled on all Intel processors with TSX capabilities.",
                Tags = ["processor", "taa", "tsx", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTAAMitigations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTAAMitigations")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTAAMitigations", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ibrs",
                Label = "Enable Indirect Branch Restricted Speculation (IBRS)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Indirect Branch Restricted Speculation prevents software running at lower privilege levels from influencing indirect branches in more privileged code. Enabling IBRS prevents user-mode code from poisoning indirect branch predictors used by kernel-mode code for Spectre variant 2 attacks. IBRS provides hardware-level mitigation when combined with appropriate processor microcode that supports the IBRS capability. Enhanced IBRS available in newer processors keeps IBRS active continuously with lower performance overhead than the original IBRS implementation. Retpoline provides an alternative Spectre variant 2 mitigation but IBRS provides hardware-based protection where Retpoline is not available. Both IBRS and Retpoline should be evaluated based on the processor generation present in the enterprise hardware fleet.",
                Tags = ["processor", "ibrs", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIBRS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIBRS")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIBRS", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-ibpb",
                Label = "Enable Indirect Branch Predictor Barrier (IBPB)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Indirect Branch Predictor Barrier flushes indirect branch predictor state when transitioning between different privilege levels or security contexts. Enabling IBPB ensures that predictions accumulated in one security context cannot influence code execution in a different security context. IBPB is particularly important at context switches between processes to prevent cross-process branch prediction poisoning. Without IBPB a malicious process can train the branch predictor before a context switch and influence speculative execution in the victim process. IBPB has some performance overhead at context switches but provides important cross-process isolation for Spectre variant 2. IBPB should be enabled on all processors that support the IBPB mechanism through microcode or architecture.",
                Tags = ["processor", "ibpb", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableIBPB", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableIBPB")],
                DetectOps = [RegOp.CheckDword(Key, "EnableIBPB", 1)],
            },
            new TweakDef
            {
                Id = "proccpol-enable-stibp",
                Label = "Enable Single Thread Indirect Branch Predictors (STIBP)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Single Thread Indirect Branch Predictors isolation prevents branch predictor sharing between sibling hyperthreads on SMT-enabled processors. Enabling STIBP ensures that branch state in one logical processor is isolated from its SMT sibling's branch prediction. Spectre cross-hyperthread attacks allow one logical processor to train the shared branch predictor and affect execution in the sibling thread. STIBP is essential for preventing cross-hyperthread Spectre variant 2 attacks on systems with SMT or Hyper-Threading enabled. The performance overhead of STIBP is process-context-dependent but modern Always-On STIBP implementations have reduced overhead. STIBP should be enabled on SMT-capable processors to prevent the hyperthread-based Spectre attack pathway.",
                Tags = ["processor", "stibp", "spectre", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSTIBP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSTIBP")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSTIBP", 1)],
            },
        ];
    }

    // ── SuperFetchSysmainPolicy ──
    private static class _SuperFetchSysmainPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SuperFetch";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sfetch-disable-superfetch",
                Label = "Disable SuperFetch (SysMain) Service",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets EnableSuperfetch=0 in the SuperFetch policy key. Disables the SysMain "
                    + "service's predictive pre-loading of frequently used application binaries "
                    + "into RAM ahead of launch. On systems with NVMe SSDs, SuperFetch provides "
                    + "negligible benefit because cold-load times are already under 100 ms, "
                    + "while the service continuously writes prefetch metadata to disk and "
                    + "consumes a persistent memory allocation. "
                    + "Default: 3 (all modes). Recommended: 0 on SSD systems.",
                Tags = ["superfetch", "sysmain", "performance", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSuperfetch", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSuperfetch")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSuperfetch", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-prefetch",
                Label = "Disable Application Prefetch",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets EnablePrefetcher=0 in the SuperFetch policy key. Stops Windows from "
                    + "recording application launch traces in %SystemRoot%\\Prefetch and using "
                    + "them to pre-load executable pages before user invocation. On SSDs the "
                    + "prefetch metadata I/O adds unnecessary write amplification without "
                    + "meaningfully reducing startup latency. "
                    + "Default: 3. Recommended: 0 on NVMe/SSD systems.",
                Tags = ["prefetch", "performance", "ssd", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePrefetcher", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePrefetcher")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePrefetcher", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-readyboost",
                Label = "Disable ReadyBoost",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets EnableReadyboost=0 in the SuperFetch policy key. Prevents SysMain "
                    + "from using removable flash storage as a ReadyBoost cache. ReadyBoost "
                    + "was designed for systems with slow HDDs; on systems with SSDs it "
                    + "provides no performance benefit and writes extensively to the flash "
                    + "device, accelerating wear. "
                    + "Default: not set (enabled by policy probe). Recommended: 0.",
                Tags = ["readyboost", "flash", "usb", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableReadyboost", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableReadyboost")],
                DetectOps = [RegOp.CheckDword(Key, "EnableReadyboost", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-readydrive",
                Label = "Disable ReadyDrive Hybrid HDD Cache",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets EnableReadydrive=0 in the SuperFetch policy key. Disables ReadyDrive, "
                    + "the feature that uses the NAND cache on hybrid hard drives to speed up "
                    + "hibernation resume and boot. On systems using pure SSDs there are no "
                    + "hybrid drives and this policy has no effect, but disabling it prevents "
                    + "the SysMain driver from scanning for hybrid device capabilities on each "
                    + "boot. Default: not set. Recommended: 0.",
                Tags = ["readydrive", "hybrid", "hdd", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableReadydrive", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableReadydrive")],
                DetectOps = [RegOp.CheckDword(Key, "EnableReadydrive", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-boot-trace",
                Label = "Disable Boot Trace for Prefetch",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets EnableBootTrace=0 in the SuperFetch policy key. Stops SysMain from "
                    + "collecting an I/O trace during the boot sequence to optimise disk access "
                    + "order for subsequent boots. On SSDs random-access latency is sub-0.1 ms, "
                    + "rendering access-order optimisation meaningless; the trace itself adds "
                    + "kernel overhead during the boot sensitive period. "
                    + "Default: 1. Recommended: 0 on SSD systems.",
                Tags = ["boot", "trace", "prefetch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableBootTrace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableBootTrace")],
                DetectOps = [RegOp.CheckDword(Key, "EnableBootTrace", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-app-launch-prefetch",
                Label = "Disable App-Launch Prefetch Optimisation",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets SuperFetchMaxSecsBeforeSuspend=0 in the SuperFetch policy key. "
                    + "Prevents SysMain from pre-fetching pages for applications that were "
                    + "recently suspended and are about to be re-activated. On SSDs the "
                    + "resume latency is already negligible, and this prefetch phase keeps "
                    + "RAM pages warm that could otherwise be used by actively running code. "
                    + "Default: 90 (seconds). Recommended: 0.",
                Tags = ["superfetch", "launch", "prefetch", "resume", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSecsBeforeSuspend")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSecsBeforeSuspend", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-logon-prefetch",
                Label = "Disable Logon Prefetch Scenario",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets SuperFetchScenarioPolicyHibernate=0 in the SuperFetch policy key. "
                    + "Disables the post-logon SysMain scenario that pre-loads anticipated "
                    + "application pages immediately after the user desktop appears. On high-CPU "
                    + "machines this scenario races with startup applications, creating "
                    + "contention and increasing first-30-second CPU load. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["superfetch", "logon", "scenario", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchScenarioPolicyHibernate")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchScenarioPolicyHibernate", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-memory-profiling",
                Label = "Disable SysMain Memory Profiling",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets SuperFetchMaxSampledPageAge=0 in the SuperFetch policy key. Prevents "
                    + "SysMain from maintaining a per-page age histogram that tracks how recently "
                    + "each physical memory page was accessed. This profiling data guides the "
                    + "pre-loading algorithm but requires SysMain to walk page-frame number "
                    + "tables on a recurring timer, adding kernel-mode overhead. "
                    + "Default: 7 (days). Recommended: 0.",
                Tags = ["superfetch", "memory", "profiling", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchMaxSampledPageAge", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchMaxSampledPageAge")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchMaxSampledPageAge", 0)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-heap-prefetch",
                Label = "Disable Application Heap Prefetch",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets SuperFetchDisableHeapDetect=1 in the SuperFetch policy key. Stops "
                    + "SysMain from recording heap-allocation patterns of active processes and "
                    + "using them to pre-warm heap segments ahead of growth allocations. Heap "
                    + "profiling involves introspecting target process address spaces via kernel "
                    + "callbacks, adding scheduling jitter on highly multi-threaded workloads. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["superfetch", "heap", "prefetch", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableHeapDetect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableHeapDetect")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableHeapDetect", 1)],
            },
            new TweakDef
            {
                Id = "sfetch-disable-telemetry",
                Label = "Disable SuperFetch Telemetry",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets SuperFetchDisableTelemetry=1 in the SuperFetch policy key. Prevents "
                    + "SysMain from emitting ETW events and submitting memory-usage reports to "
                    + "the Windows feedback infrastructure. These reports include page-fault "
                    + "rates, working-set statistics, and popular application lists derived from "
                    + "all user accounts on the machine, which can profile usage patterns. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["superfetch", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SuperFetchDisableTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SuperFetchDisableTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "SuperFetchDisableTelemetry", 1)],
            },
        ];
    }

    // ── UsbStoragePolicy ──
    private static class _UsbStoragePolicy
    {
        private const string StoragePolicy = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies";
        private const string RemovableDevices = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices";
        private const string UsbFloppyClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string CdRomClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f56308-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string TapeClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{53f5630b-b6bf-11d0-94f2-00a0c91efb8b}";
        private const string WpdClass =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{6AC27878-A6FA-4155-BA85-F98F491D4F33}";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "usbstor-write-protect",
                Label = "USB Storage: Enable Hardware Write-Protection on All Removable Drives",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [StoragePolicy],
                Tags = ["usb", "storage", "write-protect", "dlp", "security"],
                Description =
                    "Sets WriteProtect=1 in StorageDevicePolicies. Blocks all write operations to removable "
                    + "storage devices (USB drives, SD cards) at the OS level. "
                    + "Prevents data exfiltration via portable drives. Default: read/write. Recommended for kiosk/corporate.",
                ApplyOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 1)],
                RemoveOps = [RegOp.SetDword(StoragePolicy, "WriteProtect", 0)],
                DetectOps = [RegOp.CheckDword(StoragePolicy, "WriteProtect", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-write",
                Label = "USB Storage: Deny Write Access to All Removable Storage Classes (GPO)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the RemovableStorageDevices class policy. Blocks write access to all "
                    + "removable storage device classes via Group Policy. Complements the hardware WriteProtect flag. "
                    + "Default: write allowed. Recommended for data loss prevention.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-execute",
                Label = "USB Storage: Deny Execution from All Removable Storage Classes (GPO)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "execute", "autorun", "security"],
                Description =
                    "Sets Deny_Execute=1 in the RemovableStorageDevices class policy. Prevents launching "
                    + "executables directly from any removable storage device. E.g., blocks BadUSB payloads. "
                    + "Default: execution allowed. Recommended to block portable malware.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Execute", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Execute")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Execute", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-all-removable-read",
                Label = "USB Storage: Deny Read Access to All Removable Storage (Strict Lockdown)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RemovableDevices],
                Tags = ["usb", "storage", "gpo", "lockdown", "high-security"],
                Description =
                    "Sets Deny_Read=1 in the RemovableStorageDevices class policy. Blocks read access to all "
                    + "removable storage devices. Extreme lockdown for air-gapped or high-security environments. "
                    + "Default: read allowed. WARNING: prevents legitimate USB storage use.",
                ApplyOps = [RegOp.SetDword(RemovableDevices, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(RemovableDevices, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(RemovableDevices, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-usb-disk-write",
                Label = "USB Storage: Deny Write Access to USB Disk Drives (Class Policy)",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [UsbFloppyClass],
                Tags = ["usb", "storage", "disk", "write", "dlp", "class-driver"],
                Description =
                    "Sets Deny_Write=1 in the USB disk drive device class GUID policy. "
                    + "Targets the removable disk class specifically (includes USB flash, external HDD). "
                    + "Default: write allowed. More targeted than the all-classes RemovableDevices key.",
                ApplyOps = [RegOp.SetDword(UsbFloppyClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(UsbFloppyClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(UsbFloppyClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-cdrom-write",
                Label = "USB Storage: Deny Write Access to Optical/CD-ROM Drives",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CdRomClass],
                Tags = ["usb", "cdrom", "optical", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the CD-ROM / optical drive device class GUID policy. "
                    + "Prevents disc burning (ISO, data CD/DVD). Blocks data exfiltration via optical media. "
                    + "Default: write allowed. Effective for blocking burnable media.",
                ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-cdrom-read",
                Label = "USB Storage: Deny Read Access to optical drives",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [CdRomClass],
                Tags = ["usb", "cdrom", "optical", "read", "lockdown"],
                Description =
                    "Sets Deny_Read=1 in the CD-ROM device class GUID policy. "
                    + "Blocks mounting and reading optical discs. "
                    + "Intended for high-security environments where disc insertion is prohibited.",
                ApplyOps = [RegOp.SetDword(CdRomClass, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(CdRomClass, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(CdRomClass, "Deny_Read", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-tape-write",
                Label = "USB Storage: Deny Write Access to Tape Drives",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [TapeClass],
                Tags = ["usb", "tape", "backup", "write", "dlp", "security"],
                Description =
                    "Sets Deny_Write=1 in the tape drive device class GUID policy. "
                    + "Prevents writing to tape backup units from non-authorized users. "
                    + "Default: tape writes allowed. Recommended for environments with tape backup controls.",
                ApplyOps = [RegOp.SetDword(TapeClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(TapeClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(TapeClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-wpd-write",
                Label = "USB Storage: Deny Write Access to WPD (MTP/PTP) Portable Devices",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WpdClass],
                Tags = ["usb", "wpd", "mtp", "ptp", "phone", "write", "dlp"],
                Description =
                    "Sets Deny_Write=1 in the WPD (Windows Portable Devices) class GUID policy. "
                    + "Prevents file transfers to phones, cameras, and MTP/PTP devices connected via USB. "
                    + "Default: write allowed. Blocks data transfer to smartphones and cameras.",
                ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Write", 1)],
                RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Write")],
                DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Write", 1)],
            },
            new TweakDef
            {
                Id = "usbstor-deny-wpd-read",
                Label = "USB Storage: Deny Read Access to WPD (MTP/PTP) Portable Devices",
                Category = "Peripherals",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [WpdClass],
                Tags = ["usb", "wpd", "mtp", "ptp", "phone", "read", "lockdown"],
                Description =
                    "Sets Deny_Read=1 in the WPD device class GUID policy. "
                    + "Prevents reading files from phones, cameras, and MTP/PTP devices. "
                    + "Extreme DLP measure to prevent any data exchange with portable consumer devices.",
                ApplyOps = [RegOp.SetDword(WpdClass, "Deny_Read", 1)],
                RemoveOps = [RegOp.DeleteValue(WpdClass, "Deny_Read")],
                DetectOps = [RegOp.CheckDword(WpdClass, "Deny_Read", 1)],
            },
        ];
    }

    // ── VirtualDiskServicePolicy ──
    private static class _VirtualDiskServicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DiskManagement";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "vdspol-block-vhd-mount",
                    Label = "Block Standard Users from Mounting VHD/VHDX Files",
                    Category = "Peripherals",
                    Description =
                        "Prevents standard (non-admin) users from attaching or mounting Virtual Hard Disk (VHD/VHDX) files, closing the data-exfiltration path of creating an encrypted virtual disk and filling it with sensitive data.",
                    Tags = ["vhd", "virtual-disk", "mount", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "VHD/VHDX mounting restricted to administrators; standard users cannot attach virtual disk files.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowVHDMount", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowVHDMount")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowVHDMount", 0)],
                },
                new TweakDef
                {
                    Id = "vdspol-block-iso-mount",
                    Label = "Block Standard Users from Mounting ISO/IMG Files",
                    Category = "Peripherals",
                    Description =
                        "Prevents standard users from mounting ISO, IMG, and other optical disc image files via the Explorer 'Mount' context menu, restricting virtual drive creation to administrators.",
                    Tags = ["iso", "virtual-drive", "mount", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "ISO/IMG mounting restricted to admins; standard users cannot browse or execute content from disc images.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowISOMount", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowISOMount")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowISOMount", 0)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-disk-management-snap-in",
                    Label = "Disable Disk Management Snap-In for Standard Users",
                    Category = "Peripherals",
                    Description =
                        "Blocks the Disk Management MMC snap-in (diskmgmt.msc) for non-administrator accounts, preventing standard users from viewing, partitioning, formatting, or managing physical and virtual disks.",
                    Tags = ["disk-management", "mmc", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disk Management blocked for standard users; partitioning and disk operations require admin elevation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDiskManagementSnapIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDiskManagementSnapIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDiskManagementSnapIn", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-require-admin-for-partition",
                    Label = "Require Admin for Disk Partitioning Operations",
                    Category = "Peripherals",
                    Description =
                        "Enforces that all disk partitioning operations (create, delete, resize partition) require administrator privileges, preventing accidental or malicious disk modification by standard users.",
                    Tags = ["disk-management", "partition", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Partitioning operations require admin rights; standard users cannot modify partition layout.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForPartitioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForPartitioning")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForPartitioning", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-removable-format",
                    Label = "Disable Formatting of Removable Drives by Standard Users",
                    Category = "Peripherals",
                    Description =
                        "Prevents standard users from formatting removable drives (USB drives, SD cards, external HDDs) through Explorer or Disk Management, avoiding irreversible data loss by users without sufficient knowledge.",
                    Tags = ["disk-management", "format", "removable", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removable media formatting restricted to admins; standard users cannot format USB drives.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemovableMediaFormat", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemovableMediaFormat")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemovableMediaFormat", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-log-vhd-attach-events",
                    Label = "Enable Audit Logging for VHD/VHDX Attach and Detach Events",
                    Category = "Peripherals",
                    Description =
                        "Enables event log entries for every VHD/VHDX mount and unmount operation, recording the file path and user account responsible for each attachment.",
                    Tags = ["vhd", "audit-log", "virtual-disk", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VHD/VHDX attach/detach events logged; virtual disk mount activity is auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditVHDMountEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditVHDMountEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditVHDMountEvents", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-dynamic-disk",
                    Label = "Disable Dynamic Disk Conversions",
                    Category = "Peripherals",
                    Description =
                        "Prevents conversion of basic disks to dynamic disk format, blocking the creation of spanned, striped, or mirrored volumes via Windows dynamic disk — recommending Storage Spaces instead for resilient configurations.",
                    Tags = ["disk-management", "dynamic-disk", "conversion", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Dynamic disk conversion disabled; basic disks cannot be upgraded to dynamic. Use Storage Spaces for resilience.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDynamicDiskConversion", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDynamicDiskConversion")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDynamicDiskConversion", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-block-auto-initialize-disk",
                    Label = "Block Automatic Disk Initialisation on New Disk Detection",
                    Category = "Peripherals",
                    Description =
                        "Prevents Windows from automatically opening the Initialize Disk wizard when a new uninitialized disk is detected, requiring an administrator to manually initiate disk initialisation.",
                    Tags = ["disk-management", "auto-initialize", "new-disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Automatic disk initialisation wizard suppressed; admins must manually initialise new disks.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutoInitializeDisk", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoInitializeDisk")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutoInitializeDisk", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-require-bitlocker-for-external",
                    Label = "Require BitLocker Encryption Before External Drive Writability",
                    Category = "Peripherals",
                    Description =
                        "Requires that external or removable drives be encrypted with BitLocker To Go before allowing write access, preventing unencrypted exfiltration of sensitive data to external media.",
                    Tags = ["disk-management", "bitlocker", "external-drive", "encryption", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "External drives require BitLocker encryption to become writable; unencrypted drives are read-only.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireBitLockerForExternalWritable", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireBitLockerForExternalWritable")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireBitLockerForExternalWritable", 1)],
                },
                new TweakDef
                {
                    Id = "vdspol-disable-wps-disk-provision",
                    Label = "Disable Windows Provisioning Service Disk Auto-Provision",
                    Category = "Peripherals",
                    Description =
                        "Disables the Windows Provisioning Service automatic disk provisioning feature that configures disk topology on first boot, ensuring that enterprise imaging tools retain full control over disk layout.",
                    Tags = ["disk-management", "provisioning", "auto-provision", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows Provisioning disk auto-provision disabled; disk layout managed by enterprise imaging tools.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisionDisk", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisionDisk")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisionDisk", 1)],
                },
            ];
    }
}

internal static class PolicyPrint
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _FaxServicePolicy.Data,
            .. _InternetPrintingPolicy.Data,
            .. _IppEverywherePolicy.Data,
            .. _IppProtocolPolicy.Data,
            .. _PrinterDirectoryServicesPolicy.Data,
            .. _PrinterDriverIsolationPolicy.Data,
            .. _PrinterGpoPolicy.Data,
            .. _PrinterRedirectionPolicy.Data,
            .. _PrintJobManagementPolicy.Data,
            .. _PrintManagementPolicy.Data,
            .. _PrintQueuePolicy.Data,
            .. _PrintSpoolAdvPolicy.Data,
            .. _PrintSpoolerAdvancedPolicy.Data,
            .. _PrintSpoolerPolicy.Data,
            .. _PrintSpoolerSecurity.Data,
            .. _PrintSpoolFinalPolicy.Data,
            .. _PrintTicketPolicy.Data,
            .. _ProtectedPrintModePolicy.Data,
        ];

    // ── FaxServicePolicy ──
    private static class _FaxServicePolicy
    {
        private const string FaxLm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Fax";
        private const string FaxCu = @"HKEY_CURRENT_USER\Software\Policies\Microsoft\Windows NT\Fax";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "faxsvc-disable-fax",
                Label = "Disable Fax Service",
                Category = "Maintenance",
                Description =
                    "Sets Fax=1 in the machine Fax policy key under DisabledComponents. "
                    + "Configures Windows Group Policy to mark the Fax service component as disabled at the policy level. "
                    + "Prevents fax services from being used on machines where faxing functionality is not required. "
                    + "Default: absent (Fax service allowed). Recommended: 1 on machines with no fax hardware or requirements.",
                Tags = ["fax", "service", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Fax service restricted at the Group Policy level; fax send/receive operations are blocked.",
                ApplyOps = [RegOp.SetDword(FaxLm, "Fax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "Fax")],
                DetectOps = [RegOp.CheckDword(FaxLm, "Fax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-online-fax",
                Label = "Disable Online Fax Service",
                Category = "Maintenance",
                Description =
                    "Sets OnlineFax=1 in the machine Fax policy key. "
                    + "Prevents users from sending faxes via online fax providers or cloud-based fax services. "
                    + "Blocks the 'Connect to a fax modem' and online fax integration from the Windows fax tools UI. "
                    + "Default: absent (online fax allowed). Recommended: 1 to prevent unsanctioned cloud fax usage.",
                Tags = ["fax", "online", "cloud", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Online/cloud fax providers blocked at the policy level; only local modem-based fax permitted.",
                ApplyOps = [RegOp.SetDword(FaxLm, "OnlineFax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "OnlineFax")],
                DetectOps = [RegOp.CheckDword(FaxLm, "OnlineFax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-cover-pages",
                Label = "Disable Fax Cover Pages",
                Category = "Maintenance",
                Description =
                    "Sets CoverPages=1 in the machine Fax policy key. "
                    + "Prevents users from attaching cover pages to faxes sent through the Windows fax tool. "
                    + "Useful in environments that use standardised fax headers from the PBX or fax server. "
                    + "Default: absent (cover pages allowed). Recommended: 1 when corporate headers are auto-applied by the fax server.",
                Tags = ["fax", "cover-page", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Fax cover page attachment disabled; outgoing faxes are sent without a Windows-generated cover page.",
                ApplyOps = [RegOp.SetDword(FaxLm, "CoverPages", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "CoverPages")],
                DetectOps = [RegOp.CheckDword(FaxLm, "CoverPages", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-personal-cover-pages",
                Label = "Disable Personal Fax Cover Pages",
                Category = "Maintenance",
                Description =
                    "Sets PersonalCoverPages=1 in the machine Fax policy key. "
                    + "Prevents users from creating or storing personal fax cover page templates ("
                    + ".cov files) on their profile, restricting cover page management to IT-distributed templates. "
                    + "Default: absent (personal covers allowed). Recommended: 1 to enforce corporate cover page standards.",
                Tags = ["fax", "cover-page", "personal", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Personal fax cover page creation and storage blocked; only shared network templates usable.",
                ApplyOps = [RegOp.SetDword(FaxLm, "PersonalCoverPages", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "PersonalCoverPages")],
                DetectOps = [RegOp.CheckDword(FaxLm, "PersonalCoverPages", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-recipients",
                Label = "Disable Fax Recipient Book",
                Category = "Maintenance",
                Description =
                    "Sets DisableRecipients=1 in the machine Fax policy key. "
                    + "Removes the 'Select Recipients' feature from the Windows Fax and Scan UI, "
                    + "preventing users from building a personal fax contacts book. "
                    + "Default: absent (recipient book enabled). Recommended: 1 when fax routing is managed centrally.",
                Tags = ["fax", "recipients", "contacts", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Personal fax recipient book removed from Windows Fax and Scan UI.",
                ApplyOps = [RegOp.SetDword(FaxLm, "DisableRecipients", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "DisableRecipients")],
                DetectOps = [RegOp.CheckDword(FaxLm, "DisableRecipients", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-require-send-tapi",
                Label = "Restrict Fax to TAPI Lines Only",
                Category = "Maintenance",
                Description =
                    "Sets TapiOnly=1 in the machine Fax policy key. "
                    + "Forces the Windows Fax service to use only TAPI-registered lines for sending faxes, "
                    + "preventing direct modem or non-TAPI send paths. Ensures fax traffic flows through audited channels. "
                    + "Default: absent (all send paths allowed). Recommended: 1 to ensure audit trail compliance.",
                Tags = ["fax", "tapi", "modem", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Fax transmission restricted to TAPI-registered phone lines; non-TAPI fax paths are blocked.",
                ApplyOps = [RegOp.SetDword(FaxLm, "TapiOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "TapiOnly")],
                DetectOps = [RegOp.CheckDword(FaxLm, "TapiOnly", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-inbound-routing",
                Label = "Disable Inbound Fax Routing",
                Category = "Maintenance",
                Description =
                    "Sets InboundRouting=1 in the machine Fax policy key. "
                    + "Prevents the Windows fax service from routing incoming faxes to user inboxes or email. "
                    + "Forces a passive receive mode where received faxes are not forwarded or archived automatically. "
                    + "Default: absent (inbound routing enabled). Recommended: 1 when the PBX or upstream fax server handles routing.",
                Tags = ["fax", "inbound", "routing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Inbound fax auto-routing to user mailboxes or folders is disabled.",
                ApplyOps = [RegOp.SetDword(FaxLm, "InboundRouting", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "InboundRouting")],
                DetectOps = [RegOp.CheckDword(FaxLm, "InboundRouting", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-archive",
                Label = "Disable Fax Archive",
                Category = "Maintenance",
                Description =
                    "Sets Archive=1 in the machine Fax policy key. "
                    + "Prevents the Windows fax service from automatically archiving copies of sent and received faxes. "
                    + "Useful when archiving is handled by a dedicated fax compliance server and client-side archive is redundant. "
                    + "Default: absent (archive enabled). Recommended: 1 when a server-side archive is the sole authoritative copy.",
                Tags = ["fax", "archive", "retention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Client-side fax archive disabled; faxes won't be stored locally in the Windows Fax and Scan archive.",
                ApplyOps = [RegOp.SetDword(FaxLm, "Archive", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "Archive")],
                DetectOps = [RegOp.CheckDword(FaxLm, "Archive", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-fax-user",
                Label = "Disable Fax for Current User",
                Category = "Maintenance",
                Description =
                    "Sets Fax=1 in the per-user Fax policy key. "
                    + "Applies the fax disable policy for the current user only, without requiring a machine-wide GPO. "
                    + "Useful in BYOD or per-user policy environments where fax usage is restricted to specific roles. "
                    + "Default: absent (fax allowed for user). Recommended: 1 for non-fax users on a shared machine.",
                Tags = ["fax", "user", "policy"],
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Fax service restricted for the current user profile only.",
                ApplyOps = [RegOp.SetDword(FaxCu, "Fax", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxCu, "Fax")],
                DetectOps = [RegOp.CheckDword(FaxCu, "Fax", 1)],
            },
            new TweakDef
            {
                Id = "faxsvc-disable-new-account",
                Label = "Disable Fax New Account Creation",
                Category = "Maintenance",
                Description =
                    "Sets NewAccounts=1 in the machine Fax policy key. "
                    + "Prevents users from adding new fax accounts or configuring additional fax connections in Windows. "
                    + "Ensures fax account provisioning is controlled by IT and prevents shadow fax connections. "
                    + "Default: absent (new account creation allowed). Recommended: 1 to lock down fax account provisioning.",
                Tags = ["fax", "account", "provisioning", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Users cannot add new fax accounts; all fax connections must be IT-provisioned.",
                ApplyOps = [RegOp.SetDword(FaxLm, "NewAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(FaxLm, "NewAccounts")],
                DetectOps = [RegOp.CheckDword(FaxLm, "NewAccounts", 1)],
            },
        ];
    }

    // ── InternetPrintingPolicy ──
    private static class _InternetPrintingPolicy
    {
        private const string Prnt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
        private const string PnP = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inetprt-disable-web-printing",
                Label = "Disable Web Printing",
                Category = "Maintenance",
                Description = "Prevents users from printing to Internet printers over HTTP.",
                Tags = ["printing", "network", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableWebPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableWebPrinting")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableWebPrinting", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-http-printing",
                Label = "Disable HTTP Printing",
                Category = "Maintenance",
                Description = "Disables use of HTTP for connecting to printers on intranet/internet print servers.",
                Tags = ["printing", "network", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableHTTPPrinting", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-block-spooler-rpc-endpoint",
                Label = "Block Spooler Remote RPC Endpoint Registration",
                Category = "Maintenance",
                Description = "Prevents the print spooler from registering with the remote RPC endpoint mapper, reducing remote attack surface.",
                Tags = ["printing", "security", "group-policy", "hardening", "rpc"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "RegisterSpoolerRemoteRPCEndPoint", 0)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "RegisterSpoolerRemoteRPCEndPoint")],
                DetectOps = [RegOp.CheckDword(Prnt, "RegisterSpoolerRemoteRPCEndPoint", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-block-kernel-mode-drivers",
                Label = "Block Kernel-Mode Printer Drivers",
                Category = "Maintenance",
                Description = "Prevents installation of kernel-mode printer drivers, which can be exploited for privilege escalation.",
                Tags = ["printing", "security", "group-policy", "hardening", "drivers"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "KMPrintersAreBlocked", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "KMPrintersAreBlocked")],
                DetectOps = [RegOp.CheckDword(Prnt, "KMPrintersAreBlocked", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-package-point-and-print-only",
                Label = "Restrict Point and Print to Package-Aware Drivers Only",
                Category = "Maintenance",
                Description = "Requires Point and Print connections to use only package-aware (.inf-packaged) printer drivers.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "PackagePointAndPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(PnP, "PackagePointAndPrintOnly")],
                DetectOps = [RegOp.CheckDword(PnP, "PackagePointAndPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-pnp-no-warning-on-install",
                Label = "Require Warning + Elevation for Point and Print Driver Install",
                Category = "Maintenance",
                Description =
                    "Ensures users are warned and elevation is required when installing Point and Print drivers, mitigating PrintNightmare-class attacks.",
                Tags = ["printing", "security", "group-policy", "hardening", "uac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "NoWarningNoElevationOnInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(PnP, "NoWarningNoElevationOnInstall")],
                DetectOps = [RegOp.CheckDword(PnP, "NoWarningNoElevationOnInstall", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-pnp-require-update-prompt",
                Label = "Require Elevation for Point and Print Driver Updates",
                Category = "Maintenance",
                Description = "Forces elevation prompt when connecting to a print server that requires a newer driver version.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(PnP, "UpdatePromptSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(PnP, "UpdatePromptSettings")],
                DetectOps = [RegOp.CheckDword(PnP, "UpdatePromptSettings", 0)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-print-driver-download",
                Label = "Disable Automatic Print Driver Download from Windows Update",
                Category = "Maintenance",
                Description = "Prevents Windows from automatically downloading printer drivers from Windows Update.",
                Tags = ["printing", "network", "group-policy", "update"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "DisableWebPnPDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "DisableWebPnPDownload")],
                DetectOps = [RegOp.CheckDword(Prnt, "DisableWebPnPDownload", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-restrict-driver-install-to-admins",
                Label = "Restrict Printer Driver Installation to Administrators",
                Category = "Maintenance",
                Description =
                    "Allows only administrators to install printer drivers, preventing non-admins from installing potentially malicious drivers.",
                Tags = ["printing", "security", "group-policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "RestrictDriverInstallationToAdministrators", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "RestrictDriverInstallationToAdministrators")],
                DetectOps = [RegOp.CheckDword(Prnt, "RestrictDriverInstallationToAdministrators", 1)],
            },
            new TweakDef
            {
                Id = "inetprt-disable-v3-printer-driver",
                Label = "Disable v3 User-Mode Printer Drivers",
                Category = "Maintenance",
                Description = "Prevents the use of v3 (user-mode) printer drivers; only v4 (kernel-mode isolated) drivers are allowed.",
                Tags = ["printing", "security", "group-policy", "drivers"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Prnt, "V3DriverPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Prnt, "V3DriverPolicy")],
                DetectOps = [RegOp.CheckDword(Prnt, "V3DriverPolicy", 1)],
            },
        ];
    }

    // ── IppEverywherePolicy ──
    private static class _IppEverywherePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPPEverywhere";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ippevy-disable-ipp-everywhere",
                    Label = "Disable IPP Everywhere Driverless Printing",
                    Category = "Maintenance",
                    Description =
                        "Disables the IPP Everywhere driverless printing framework, forcing Windows to rely on traditional printer drivers instead of the universal IPP print path used by modern printers.",
                    Tags = ["ipp-everywhere", "driverless-printing", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "IPP Everywhere disabled; modern printers may require explicit driver install instead of auto-detection.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPEverywhere", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPEverywhere")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPEverywhere", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-block-cloud-ipp-print",
                    Label = "Block Cloud IPP Print (Universal Cloud Print Path)",
                    Category = "Maintenance",
                    Description =
                        "Blocks cloud-relayed IPP print paths that route print jobs through Microsoft cloud infrastructure, ensuring all print jobs are submitted directly to local network printers without cloud relay.",
                    Tags = ["ipp-everywhere", "cloud-print", "printing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cloud-relayed IPP printing blocked; print jobs only submitted directly to LAN printers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockCloudIPPPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockCloudIPPPrint")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockCloudIPPPrint", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-require-pw-format",
                    Label = "Require PWG Raster Format Validation for IPP Jobs",
                    Category = "Maintenance",
                    Description =
                        "Enforces format validation for PWG Raster print data submitted via IPP Everywhere, rejecting malformed print data that could trigger parsing vulnerabilities in printer firmware.",
                    Tags = ["ipp-everywhere", "pwg-raster", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PWG Raster data validated before forwarding; malformed print payloads rejected at host.",
                    ApplyOps = [RegOp.SetDword(Key, "RequirePWGRasterValidation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequirePWGRasterValidation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequirePWGRasterValidation", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-block-apple-airprint",
                    Label = "Block Apple AirPrint via IPP Everywhere on Windows",
                    Category = "Maintenance",
                    Description =
                        "Blocks the AirPrint protocol layer that allows Apple devices to print to Windows-shared printers using IPP Everywhere, preventing uncontrolled cross-platform printer sharing.",
                    Tags = ["ipp-everywhere", "airprint", "apple", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "AirPrint to Windows-shared printers blocked; Apple devices cannot print to this host via AirPrint.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAirPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAirPrint")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAirPrint", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-disable-ipp-infra-service",
                    Label = "Disable IPP Infrastructure Service (Universal Print Relay)",
                    Category = "Maintenance",
                    Description =
                        "Disables the Windows IPP Infrastructure Background Service that routes IPP jobs to printers registered in Microsoft Universal Print, forcing direct queue usage.",
                    Tags = ["ipp-everywhere", "universal-print", "microsoft", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPP Infrastructure Service disabled; Universal Print routing service inactive.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPInfraService", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPInfraService")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPInfraService", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-block-mopria-print",
                    Label = "Block Mopria Print Discovery and Submission",
                    Category = "Maintenance",
                    Description =
                        "Blocks the Mopria Alliance standard print path that allows Android and other devices to discover and submit print jobs to Windows-shared printers via Mopria-compliant IPP.",
                    Tags = ["ipp-everywhere", "mopria", "android", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Mopria print blocked; Android devices cannot print to this host via Mopria IPP.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMopriaPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMopriaPrint")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMopriaPrint", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-require-tls-12-minimum",
                    Label = "Require TLS 1.2 Minimum for IPP Everywhere HTTPS",
                    Category = "Maintenance",
                    Description =
                        "Enforces a minimum of TLS 1.2 for IPPS connections used in IPP Everywhere print paths, blocking print traffic over TLS 1.0 or 1.1 which are deprecated and cryptographically weak.",
                    Tags = ["ipp-everywhere", "tls", "security", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPP Everywhere IPPS upgraded to TLS 1.2 minimum; old TLS versions rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "MinimumTLSVersionForIPPS", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinimumTLSVersionForIPPS")],
                    DetectOps = [RegOp.CheckDword(Key, "MinimumTLSVersionForIPPS", 2)],
                },
                new TweakDef
                {
                    Id = "ippevy-disable-pdf-print-path",
                    Label = "Disable IPP Everywhere PDF Print Format Path",
                    Category = "Maintenance",
                    Description =
                        "Disables the PDF-based print format path in IPP Everywhere, preventing Windows from generating PDF documents during the print process which avoids PDF parser vulnerabilities in printer firmware.",
                    Tags = ["ipp-everywhere", "pdf", "print-format", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PDF print path disabled in IPP Everywhere; pwg-raster or other formats used instead.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePDFPrintPath", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePDFPrintPath")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePDFPrintPath", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-log-ipp-everywhere-jobs",
                    Label = "Enable Audit Logging for IPP Everywhere Print Jobs",
                    Category = "Maintenance",
                    Description =
                        "Enables event log entries for print jobs submitted via the IPP Everywhere path, providing a record of driverless print activity including job source IP and document metadata.",
                    Tags = ["ipp-everywhere", "audit-log", "printing", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "IPP Everywhere print jobs logged; driverless print activity auditable in event viewer.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditIPPEverywhereJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditIPPEverywhereJobs")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditIPPEverywhereJobs", 1)],
                },
                new TweakDef
                {
                    Id = "ippevy-block-anonymous-ipp-print",
                    Label = "Block Anonymous IPP Everywhere Print Submissions",
                    Category = "Maintenance",
                    Description =
                        "Blocks unauthenticated (anonymous) print job submissions via IPP Everywhere, requiring all IPP Everywhere clients to present credentials before print jobs are accepted.",
                    Tags = ["ipp-everywhere", "authentication", "anonymous", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Anonymous IPP Everywhere print blocked; authentication required for all driverless print submissions.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAnonymousIPPEverywherePrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAnonymousIPPEverywherePrint")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAnonymousIPPEverywherePrint", 1)],
                },
            ];
    }

    // ── IppProtocolPolicy ──
    private static class _IppProtocolPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\IPP";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ipppol-disable-ipp-client",
                    Label = "Disable IPP Printing Client",
                    Category = "Maintenance",
                    Description =
                        "Disables the Windows Internet Printing Protocol (IPP) client, preventing Windows from submitting print jobs to network printers using RFC 8011 IPP over TCP/631.",
                    Tags = ["ipp", "printing", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPP client disabled; cannot print to RFC 8011 IPP-compliant network printers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPClient", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPClient")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPClient", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-enforce-ipp-tls",
                    Label = "Enforce TLS for IPP Print Jobs (IPPS)",
                    Category = "Maintenance",
                    Description =
                        "Forces all IPP print jobs to use IPPS (IPP over TLS, port 443/631), preventing print data from being sent in plaintext over the network where it could be intercepted.",
                    Tags = ["ipp", "ipps", "tls", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "IPP print traffic encrypted via TLS; plaintext IPP on port 631 no longer accepted by client.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireIPPSForPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireIPPSForPrinting")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireIPPSForPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-block-ipp-everywhere-auto-add",
                    Label = "Block IPP Everywhere Auto-Add Network Printers",
                    Category = "Maintenance",
                    Description =
                        "Prevents Windows from automatically adding IPP Everywhere printers discovered on the local network via mDNS/Bonjour, stopping printers from being silently added to the system when connecting to a network.",
                    Tags = ["ipp", "ipp-everywhere", "auto-add", "mdns", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPP Everywhere auto-discovery blocked; new printers must be added manually.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPEverywhereAutoAdd", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPEverywhereAutoAdd")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPEverywhereAutoAdd", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-require-ipp-auth",
                    Label = "Require Authentication for IPP Print Jobs",
                    Category = "Maintenance",
                    Description =
                        "Forces authentication for all IPP print jobs submitted to network printers, preventing anonymous IPP printing that could allow unauthorised print access or queue inspection.",
                    Tags = ["ipp", "authentication", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "IPP print auth required; anonymous print jobs to IPP printers rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireIPPAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireIPPAuthentication")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireIPPAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-block-cross-domain-ipp",
                    Label = "Block IPP Printing to Cross-Domain Servers",
                    Category = "Maintenance",
                    Description =
                        "Restricts IPP printing to print servers within the same domain, preventing print data (which may contain sensitive content) from being submitted to external or untrusted IPP endpoints.",
                    Tags = ["ipp", "domain", "printing", "data-loss-prevention", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "IPP printing restricted to domain servers; print jobs cannot be sent to external IPP endpoints.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockCrossDomainIPP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossDomainIPP")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockCrossDomainIPP", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-disable-ipp-printer-share",
                    Label = "Disable IPP Printer Sharing Outbound from This Host",
                    Category = "Maintenance",
                    Description =
                        "Disables this host from acting as an IPP print server, stopping Windows from advertising locally configured printers as IPP endpoints that other devices can connect to.",
                    Tags = ["ipp", "printer-sharing", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "This host stops advertising printers via IPP; remote IPP print jobs to this machine rejected.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPPrinterSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPPrinterSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPPrinterSharing", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-limit-ipp-max-job-size",
                    Label = "Limit Maximum IPP Print Job Size to 100 MB",
                    Category = "Maintenance",
                    Description =
                        "Caps the maximum size of a single IPP print job at 100 MB, preventing denial-of-service attacks that attempt to exhaust disk space or spooler memory via unexpectedly large print jobs.",
                    Tags = ["ipp", "print-job", "dos-protection", "printing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "IPP print jobs capped at 100 MB; oversized jobs rejected by spooler.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxIPPJobSizeMB", 100)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxIPPJobSizeMB")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxIPPJobSizeMB", 100)],
                },
                new TweakDef
                {
                    Id = "ipppol-disable-ipp-compressed-jobs",
                    Label = "Disable IPP Compressed (GZIP) Job Data",
                    Category = "Maintenance",
                    Description =
                        "Disables compression (gzip/deflate) for IPP print job data, mitigating compression-based timing and oracle attacks against the IPP stream while simplifying spooler job processing.",
                    Tags = ["ipp", "compression", "printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "IPP job compression disabled; print data transmitted uncompressed.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIPPJobCompression", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIPPJobCompression")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIPPJobCompression", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-block-ipp-mdns-advertisement",
                    Label = "Block IPP Printer mDNS/Bonjour Advertisement",
                    Category = "Maintenance",
                    Description =
                        "Prevents this host from broadcasting locally-share printers via mDNS/Bonjour, hiding the presence of connected printers from device discovery on the local network.",
                    Tags = ["ipp", "mdns", "bonjour", "printer-discovery", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "mDNS printer advertisements disabled; connected printers invisible to network discovery tools.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockIPPmDNSAdvertisement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockIPPmDNSAdvertisement")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockIPPmDNSAdvertisement", 1)],
                },
                new TweakDef
                {
                    Id = "ipppol-enable-ipp-audit-log",
                    Label = "Enable IPP Print Job Audit Logging",
                    Category = "Maintenance",
                    Description =
                        "Enables event log entries for IPP print jobs (job start, completion, errors), providing traceability for print operations for security monitoring and compliance.",
                    Tags = ["ipp", "audit-log", "printing", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "IPP job lifecycle events logged; print activity auditable via event viewer.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableIPPAuditLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableIPPAuditLog")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableIPPAuditLog", 1)],
                },
            ];
    }

    // ── PrinterDirectoryServicesPolicy ──
    private static class _PrinterDirectoryServicesPolicy
    {
        private const string DsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DS";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pdssp-disable-printer-publishing",
                    Label = "Disable Automatic Printer Publishing to AD",
                    Category = "Maintenance",
                    Description =
                        "Sets PublishPrinters=0 to prevent Windows from automatically publishing printers to Active Directory "
                        + "Directory Services when they are added to the system. Unpublished printers are not discoverable via "
                        + "AD queries, reducing AD pollution from transient or personal printers on endpoints.",
                    Tags = ["printing", "active-directory", "publishing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops auto-publishing printers to AD; manually published printers and shared printers unaffected.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PublishPrinters", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PublishPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PublishPrinters", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-printer-pruning",
                    Label = "Disable Printer Object Pruning from AD",
                    Category = "Maintenance",
                    Description =
                        "Sets PruningRetries=0 to disable the printer pruning mechanism that removes stale printer objects "
                        + "from Active Directory when the print server is unreachable. Prevents pruning in environments where "
                        + "print servers go offline temporarily (maintenance, DR failover) but should remain resolvable via AD.",
                    Tags = ["printing", "active-directory", "pruning", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Disables AD pruning; stale printer objects persist in AD if print servers are decommissioned.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningRetries", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetries")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningRetries", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-pruning-interval",
                    Label = "Set Printer Pruning Check Interval",
                    Category = "Maintenance",
                    Description =
                        "Sets PruningInterval=480 to check every 8 hours (480 minutes) whether printer objects in Active Directory "
                        + "should be pruned. The default check interval is every 8 hours; a longer interval reduces AD queries "
                        + "from the Directory Service pruning subsystem on domain controllers.",
                    Tags = ["printing", "active-directory", "pruning", "interval", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Sets pruning poll interval to 480 min; reduces printer-related AD query load.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningInterval", 480)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningInterval")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningInterval", 480)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-pruning-priority",
                    Label = "Set Printer Pruning Thread Priority",
                    Category = "Maintenance",
                    Description =
                        "Sets PruningPriority=0 to run the printer pruning thread at low priority. "
                        + "Reduces CPU contention from the background AD pruning process on heavily loaded print servers "
                        + "and domain controllers that share hardware resources.",
                    Tags = ["printing", "active-directory", "pruning", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Low-priority pruning thread; negligible performance impact on print servers.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningPriority", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningPriority")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningPriority", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-log-pruning-events",
                    Label = "Enable Printer Pruning Event Logging",
                    Category = "Maintenance",
                    Description =
                        "Sets PruningRetryLog=1 to record printer pruning retry and failure events to the Windows Application event log. "
                        + "Provides audit visibility into AD printer object lifecycle events for SIEM ingestion and printer infrastructure monitoring.",
                    Tags = ["printing", "active-directory", "pruning", "logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Logs pruning events to Application log; minor increase in event log volume.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PruningRetryLog", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PruningRetryLog")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PruningRetryLog", 1)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-non-published-printer-access",
                    Label = "Block Access to Non-Published AD Printers",
                    Category = "Maintenance",
                    Description =
                        "Sets NonPublishedPrinters=0 to prevent users from connecting to network printers that are not published "
                        + "in Active Directory. Ensures all printer installations go through the AD Directory Services vetting process "
                        + "and prevents rogue or personal printers from being added via direct UNC paths.",
                    Tags = ["printing", "active-directory", "access-control", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Blocks non-AD-published printer connections; only AD-listed printers can be installed.",
                    ApplyOps = [RegOp.SetDword(DsKey, "NonPublishedPrinters", 0)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "NonPublishedPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "NonPublishedPrinters", 0)],
                },
                new TweakDef
                {
                    Id = "pdssp-disable-ipp-web-printing",
                    Label = "Disable IPP Web Printing via AD",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableWebPrinting=1 to prevent users from installing printers via Internet Printing Protocol (IPP) "
                        + "URLs discovered through Active Directory. Web-based printer installation bypasses network printer "
                        + "deployment controls and may allow connection to external or untrusted print services.",
                    Tags = ["printing", "ipp", "web-printing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks IPP web printer discovery via AD; direct \\server\\printer UNC paths still work.",
                    ApplyOps = [RegOp.SetDword(DsKey, "DisableWebPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "DisableWebPrinting")],
                    DetectOps = [RegOp.CheckDword(DsKey, "DisableWebPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "pdssp-set-server-thread-count",
                    Label = "Limit Printer DS Server Thread Count",
                    Category = "Maintenance",
                    Description =
                        "Sets ServerThread=2 to limit the number of concurrent threads used by the spooler for Active Directory "
                        + "printer publishing operations. Reducing thread count lowers CPU usage on print servers with many shared "
                        + "printers during AD bulk-publish events after policy refresh.",
                    Tags = ["printing", "active-directory", "performance", "threads", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Limits AD spooler threads to 2; larger environments may need higher values for timely publishing.",
                    ApplyOps = [RegOp.SetDword(DsKey, "ServerThread", 2)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "ServerThread")],
                    DetectOps = [RegOp.CheckDword(DsKey, "ServerThread", 2)],
                },
                new TweakDef
                {
                    Id = "pdssp-enforce-pre-publish-printers",
                    Label = "Enforce Pre-Publication of Printers to AD",
                    Category = "Maintenance",
                    Description =
                        "Sets PrePublishPrinters=1 to require printers to be pre-published to Active Directory before they "
                        + "become available to clients. Pre-publishing ensures printer metadata is available for directory browsing "
                        + "before the first client connection attempt, reducing discovery latency for distributed print deployments.",
                    Tags = ["printing", "active-directory", "publishing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Requires pre-publishing; AD objects created before printers accept connections.",
                    ApplyOps = [RegOp.SetDword(DsKey, "PrePublishPrinters", 1)],
                    RemoveOps = [RegOp.DeleteValue(DsKey, "PrePublishPrinters")],
                    DetectOps = [RegOp.CheckDword(DsKey, "PrePublishPrinters", 1)],
                },
            ];
    }

    // ── PrinterDriverIsolationPolicy ──
    private static class _PrinterDriverIsolationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\DriverIsolation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "pdrv-enforce-driver-isolation",
                    Label = "Enforce Printer Driver Isolation (Separate Process)",
                    Category = "Maintenance",
                    Description =
                        "Forces printer drivers to run in isolated processes separate from the spooler service, preventing a buggy or malicious printer driver from crashing or compromising the spooler.",
                    Tags = ["printing", "driver-isolation", "spooler", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Printer drivers run isolated from spooler; spooler crash impact from bad driver reduced.",
                    ApplyOps = [RegOp.SetDword(Key, "IsolationMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IsolationMode")],
                    DetectOps = [RegOp.CheckDword(Key, "IsolationMode", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-unsigned-drivers",
                    Label = "Block Installation of Unsigned Printer Drivers",
                    Category = "Maintenance",
                    Description =
                        "Blocks the installation of printer drivers that do not have a valid WHQL or enterprise certificate signature, preventing malicious or vulnerable unsigned printer drivers from loading.",
                    Tags = ["printing", "unsigned-driver", "security", "whql", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unsigned printer drivers rejected; only WHQL or enterprise-signed drivers install.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockUnsignedPrinterDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockUnsignedPrinterDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockUnsignedPrinterDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-limit-driver-install-to-admin",
                    Label = "Restrict Printer Driver Installation to Administrators Only",
                    Category = "Maintenance",
                    Description =
                        "Requires administrator privileges to install any new printer driver, preventing standard users from adding potentially exploitable printer drivers via easy-to-add printer workflows.",
                    Tags = ["printing", "driver-install", "admin", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot install printer drivers; admin credentials required.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireAdminForDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForDriverInstall")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireAdminForDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-v3-kernel-drivers",
                    Label = "Disable Legacy V3 Kernel-Mode Printer Drivers",
                    Category = "Maintenance",
                    Description =
                        "Disables legacy V3 (kernel-mode) printer drivers, allowing only V4 user-mode drivers which run isolated from the kernel and reduce the risk of privilege escalation via printer drivers.",
                    Tags = ["printing", "v3-driver", "kernel-mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "V3 kernel-mode printer drivers blocked; some older printers may require V4 driver wrappers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableV3KernelModePrinterDrivers")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableV3KernelModePrinterDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-network-driver-download",
                    Label = "Block Automatic Printer Driver Download from Network",
                    Category = "Maintenance",
                    Description =
                        "Blocks Windows from automatically downloading printer drivers from remote print servers or Windows Update when a new printer is detected, requiring manual driver installation.",
                    Tags = ["printing", "auto-driver-download", "network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Auto printer driver download blocked; must manually install drivers before adding network printers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAutoPrinterDriverDownload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAutoPrinterDriverDownload")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAutoPrinterDriverDownload", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-enable-enhanced-point-and-print",
                    Label = "Enable Enhanced Point and Print Restriction",
                    Category = "Maintenance",
                    Description =
                        "Enables enhanced Point and Print restrictions requiring that drivers originate from an approved printer server list, preventing attackers from serving malicious drivers via rogue print servers.",
                    Tags = ["printing", "point-and-print", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Point and Print restricted to approved servers; rogue print server attacks blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "Restricted", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Restricted")],
                    DetectOps = [RegOp.CheckDword(Key, "Restricted", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-driver-update-prompt",
                    Label = "Disable Automatic Printer Driver Update Prompts",
                    Category = "Maintenance",
                    Description =
                        "Suppresses automatic driver update prompts from existing printer drivers via Windows Update, preventing unexpected printer driver updates that could introduce vulnerabilities.",
                    Tags = ["printing", "driver-update", "windows-update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Auto driver update prompts suppressed; printer drivers only update via manual action.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDriverUpdatePrompt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDriverUpdatePrompt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDriverUpdatePrompt", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-block-driver-staging-from-drivers-folder",
                    Label = "Block Driver Installation from Drivers Folder Without Inbox",
                    Category = "Maintenance",
                    Description =
                        "Prevents printer drivers from being installed from the Windows Drivers directory without being in the inbox driver store, blocking attack paths that stage evil drivers into the Drivers folder.",
                    Tags = ["printing", "driver-staging", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only inbox-store printer drivers installable; staged evil-driver attack paths closed.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStagedDriverInstalls", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStagedDriverInstalls")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStagedDriverInstalls", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-disable-printer-driver-dcom",
                    Label = "Disable DCOM Access for Printer Driver Processes",
                    Category = "Maintenance",
                    Description =
                        "Prevents printer driver host processes from making DCOM calls to other processes, reducing lateral movement risk if a printer driver process is compromised.",
                    Tags = ["printing", "dcom", "driver-isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "DCOM disabled for printer driver host processes; compromised driver cannot do COM-based lateral movement.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDCOMInDriverHost", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDCOMInDriverHost")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDCOMInDriverHost", 1)],
                },
                new TweakDef
                {
                    Id = "pdrv-log-driver-install-events",
                    Label = "Enable Audit Logging for Printer Driver Installs",
                    Category = "Maintenance",
                    Description =
                        "Enables security audit events whenever a printer driver is installed, updated, or removed, providing a log trail for detecting unauthorized driver installation activity.",
                    Tags = ["printing", "audit-log", "driver-install", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Printer driver installation events logged in Security event log; unauthorised installs detectable.",
                    ApplyOps = [RegOp.SetDword(Key, "AuditDriverInstallEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AuditDriverInstallEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "AuditDriverInstallEvents", 1)],
                },
            ];
    }

    // ── PrinterGpoPolicy ──
    private static class _PrinterGpoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtgpo-disable-pointed-print-warnings",
                Label = "Enforce Point and Print Security Warnings",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Point and Print allows Windows to automatically download printer drivers from print servers when connecting to network printers. Security warnings are displayed when downloading printer drivers from servers that are not on the approved list to inform users of installation risk. Disabling Point and Print security warnings removes the user protection prompt that alerts about third-party driver installation. Removing security prompts allows silent installation of potentially malicious printer drivers from untrusted print servers. The PrintNightmare vulnerability exploited default Point and Print configurations to allow unauthenticated remote code execution. Maintaining security warnings is essential to prevent silent driver installation from untrusted sources.",
                Tags = ["printing", "point-and-print", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoWarningNoElevationOnInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoWarningNoElevationOnInstall")],
                DetectOps = [RegOp.CheckDword(Key, "NoWarningNoElevationOnInstall", 0)],
            },
            new TweakDef
            {
                Id = "prtgpo-disable-v3-driver-priority",
                Label = "Disable V3 Printer Driver Package-Aware Priority",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "V3 printer drivers run in the print spooler process itself whereas V4 drivers use an isolated architecture with reduced privilege. Disabling V3 driver priority preference ensures that the endpoint does not preferentially install lower-isolation V3 drivers over the more secure V4 architecture. V3 drivers running in-process with the spooler were the primary exploitation opportunity in PrintNightmare class vulnerabilities. Limiting V3 driver deployment in favor of the isolated V4 architecture reduces the blast radius of printer driver compromise. Modern printers increasingly support V4 drivers providing equivalent printing functionality with improved security isolation. Enforcing V4 driver preference is part of a defense-in-depth approach to printer subsystem hardening.",
                Tags = ["printing", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PackagePointAndPrintOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PackagePointAndPrintOnly")],
                DetectOps = [RegOp.CheckDword(Key, "PackagePointAndPrintOnly", 1)],
            },
            new TweakDef
            {
                Id = "prtgpo-restrict-print-server-list",
                Label = "Restrict Point and Print to Approved Servers",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Point and Print can be configured to only allow automatic printer driver downloads from a list of enterprise-approved print servers. Restricting Point and Print to approved servers prevents driver installation from unapproved or internet-sourced print server endpoints. Attackers can create rogue print servers and cause endpoints to connect to them and install malicious drivers. An allowlist of trusted internal print servers ensures that driver installation only occurs from IT-managed infrastructure. Combined with driver signing requirements this control creates a complete validation chain for printer driver provenance. Enterprise print server lists should be managed through Group Policy and updated when print infrastructure changes.",
                Tags = ["printing", "point-and-print", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ServerList", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ServerList")],
                DetectOps = [RegOp.CheckDword(Key, "ServerList", 1)],
            },
            new TweakDef
            {
                Id = "prtgpo-disable-print-driver-updates",
                Label = "Disable Automatic Print Driver Updates via Windows Update",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "Windows Update can automatically download and install updated printer drivers when new versions become available for installed print devices. Disabling automatic print driver updates through Windows Update prevents uncontrolled updates to drivers running in the privileged spooler process. Enterprise driver management policies require testing and validation of printer drivers before deployment to production endpoints. Automatic updates can receive untested or incompatible drivers that break printing functionality or introduce new security exposure. Printer driver updates should be evaluated, approved, and deployed through enterprise software management tools on a controlled schedule. This setting redirects driver update control to IT rather than preventing security updates entirely.",
                Tags = ["printing", "updates", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdatePrinterDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdatePrinterDrivers")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdatePrinterDrivers", 1)],
            },
            new TweakDef
            {
                Id = "prtgpo-disable-printer-extension",
                Label = "Disable Printer Extension Apps",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 4,
                Description =
                    "Printer extension apps are UWP applications that can be installed alongside V4 printer drivers to provide enhanced printing UI and management features. Disabling printer extension apps prevents installation and execution of vendor-supplied applications that accompany printer drivers. Third-party printer extension applications can include telemetry, marketing functionality, and network communications not required for printing. Vendor applications that run with elevated context via printer driver installation represent additional attack surface beyond core print functionality. Enterprise endpoints benefit from restricting software installation to explicitly approved applications while still supporting printing. Core printing functionality is fully retained without printer extension applications as they only provide supplemental UI features.",
                Tags = ["printing", "extensions", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinterExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterExtensions")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinterExtensions", 1)],
            },
            new TweakDef
            {
                Id = "prtgpo-disable-rpc-over-namedpipes",
                Label = "Disable Print Spooler RPC over Named Pipes",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "The print spooler communicates with remote print servers using RPC over TCP and over named pipes as transport mechanisms. Disabling RPC over named pipes for the print spooler forces all remote spooler communication to use RPC over TCP. Named pipe-based spooler communication was exploited in several PrintNightmare variants to achieve remote code execution and privilege escalation. Restricting spooler communications to TCP-based RPC makes monitoring and firewall control of spooler communications more straightforward. Named pipe transport is more difficult to restrict and monitor compared to TCP port-based firewall rules. Forcing TCP transport enables precise firewall rules to prevent unauthorized access to the print spooler service.",
                Tags = ["printing", "rpc", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RpcOverNamedPipes", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "RpcOverNamedPipes")],
                DetectOps = [RegOp.CheckDword(Key, "RpcOverNamedPipes", 0)],
            },
        ];
    }

    // ── PrinterRedirectionPolicy ──
    private static class _PrinterRedirectionPolicy
    {
        private const string TsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtred-disable-client-printer-redirect",
                    Label = "Printer Redirection: Disable Client Printer Redirection in RDS Sessions",
                    Category = "Maintenance",
                    Description =
                        "Sets fDisableCam=1 in Terminal Services policy. Prevents client printers from being automatically mapped into Remote Desktop Services sessions. When client printer redirection is enabled, every printer installed on the client machine is mapped into the RDS session as a session-specific printer. In large VDI deployments this creates hundreds of ghost printer objects per session host, causing significant spooler memory consumption, slow logon (each session must enumerate and map client printers), and instability. For environments where users should only print to central print servers, disabling client printer redirection is the recommended configuration.",
                    Tags = ["rds", "printer-redirection", "vdi", "rdp", "logon-speed"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Client local printers are not mapped into RDS/VDI sessions. Users print to centrally managed network printers deployed via Group Policy. Users with home printers or local USB printers cannot print from remote sessions. Best used when central print server coverage is complete.",
                    ApplyOps = [RegOp.SetDword(TsKey, "fDisableCam", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "fDisableCam")],
                    DetectOps = [RegOp.CheckDword(TsKey, "fDisableCam", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-enable-easy-print",
                    Label = "Printer Redirection: Enable Remote Desktop Easy Print Driver",
                    Category = "Maintenance",
                    Description =
                        "Sets UseUniversalPrinter=1 in Terminal Services policy. Enables the Remote Desktop Easy Print driver as the primary driver for redirected client printers. When Easy Print is enabled, redirected client printers use a single universal print driver on the session host rather than requiring the client's specific printer driver to be installed on every session host server. This eliminates the printer driver management burden of server-side driver installation: a 200-server RDS farm no longer needs every printer driver for every model used by clients. The Easy Print driver communicates rendering instructions to the client, which uses its own installed driver.",
                    Tags = ["rds", "easy-print", "universal-driver", "printer-redirection", "vdi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Redirected client printers use the Easy Print universal driver. Printer-specific features (stapling, booklet mode, duplex) may be unavailable through Easy Print. Print rendering is sent to the client; print quality is consistent with local direct printing.",
                    ApplyOps = [RegOp.SetDword(TsKey, "UseUniversalPrinter", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "UseUniversalPrinter")],
                    DetectOps = [RegOp.CheckDword(TsKey, "UseUniversalPrinter", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-set-printer-redirection-timeout-60s",
                    Label = "Printer Redirection: Set Printer Redirection Timeout to 60 Seconds",
                    Category = "Maintenance",
                    Description =
                        "Sets PrinterRedirectionTimeout=60 in Terminal Services policy. Sets the maximum wait time during session logon for redirected printers to become available. When client printer redirection is enabled, the session host waits for the RDP printer redirection channel to report all client printers before proceeding with logon. On slow WAN connections, printer enumeration over RDP can take tens of seconds. If the session host waits indefinitely, logon appears to hang. Setting a 60-second timeout ensures logon proceeds even if some client printers fail to enumerate, preventing printer redirection from delaying session startup.",
                    Tags = ["rds", "printer-redirection", "timeout", "logon-speed", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printer redirection enumeration is limited to 60 seconds. Printers that do not enumerate within 60 seconds are not available in the session. User logon proceeds after 60 seconds regardless of printer redirection status. Slow WAN clients may see fewer redirected printers.",
                    ApplyOps = [RegOp.SetDword(TsKey, "PrinterRedirectionTimeout", 60)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "PrinterRedirectionTimeout")],
                    DetectOps = [RegOp.CheckDword(TsKey, "PrinterRedirectionTimeout", 60)],
                },
                new TweakDef
                {
                    Id = "prtred-disable-xps-redirection",
                    Label = "Printer Redirection: Disable XPS Printer Redirection",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableXpsRedirection=1 in Terminal Services policy. Prevents the Microsoft XPS Document Writer virtual printer from being redirected into user sessions. The XPS Document Writer is a file-generation virtual printer: when a user 'prints' to it, a .XPS file is created on the user's local machine. In RDS sessions, redirected XPS printing places XPS files on the user's local machine through the RDP file system redirection channel. This creates a data exfiltration path: users on session hosts with sensitive application data can 'print' documents as XPS files and take them home. Disabling XPS redirection closes this path.",
                    Tags = ["rds", "xps-printer", "data-exfiltration", "restriction", "virtual-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "XPS Document Writer is not available in RDS sessions. Users cannot create XPS files by printing from within RDS sessions. Physical and network printers are unaffected.",
                    ApplyOps = [RegOp.SetDword(TsKey, "DisableXpsRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "DisableXpsRedirection")],
                    DetectOps = [RegOp.CheckDword(TsKey, "DisableXpsRedirection", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-restrict-auto-printer-creation",
                    Label = "Printer Redirection: Restrict Automatic Session Printer Creation to Default Only",
                    Category = "Maintenance",
                    Description =
                        "Sets LoadDriversForDefaultPrinterOnly=1 in Terminal Services policy. Limits automatic printer creation in RDS sessions to the client's default printer only, rather than all client printers. Mapping every client printer into every session is the primary cause of session host spooler memory exhaustion in large VDI farms. A user with 5 printers on their client machine causes 5 session-specific printer entries on every session host they connect to. 'Default printer only' mode preserves the one-click printing experience for the user's preferred printer while eliminating the overhead of mapping every lesser-used printer.",
                    Tags = ["rds", "printer-auto-creation", "default-printer", "vdi", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Only the client's default printer is mapped into RDS sessions. Secondary client printers are not available in sessions unless manually added by the user. Significant reduction in session host spooler memory usage in VDI deployments.",
                    ApplyOps = [RegOp.SetDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "LoadDriversForDefaultPrinterOnly")],
                    DetectOps = [RegOp.CheckDword(TsKey, "LoadDriversForDefaultPrinterOnly", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-disable-pdf-printer-redirect",
                    Label = "Printer Redirection: Disable PDF Printer Redirection in RDS",
                    Category = "Maintenance",
                    Description =
                        "Sets DisablePDFRedirection=1 in Terminal Services policy. Prevents the Microsoft Print to PDF virtual printer from being redirected into RDS sessions. Microsoft Print to PDF, like XPS, is a file-generation virtual printer that creates PDF files on the user's local machine via the RDP file system redirection channel. This is an equally effective data exfiltration path: users can take sensitive documents from session hosts as PDF files. Enterprise DRM-protected documents that cannot be copied via clipboard or USB may still be 'printed' to local PDF files through this channel.",
                    Tags = ["rds", "pdf-printer", "data-exfiltration", "dlp", "virtual-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Microsoft Print to PDF is not available in RDS sessions. Users cannot create PDF files by printing from within RDS sessions. Physical and network printers are unaffected. Users relying on session-based PDF generation need an alternative (server-side PDF converter).",
                    ApplyOps = [RegOp.SetDword(TsKey, "DisablePDFRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "DisablePDFRedirection")],
                    DetectOps = [RegOp.CheckDword(TsKey, "DisablePDFRedirection", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-enable-bidirectional-communication",
                    Label = "Printer Redirection: Enable Bidirectional Printer Communication",
                    Category = "Maintenance",
                    Description =
                        "Sets BidiComm=1 in Terminal Services policy. Enables bidirectional (bidi) printer communication for redirected printers in RDS sessions. Bidi communication allows the session to query the printer's current status — toner levels, paper jam conditions, available paper sizes, and duplexing capability — from within the session. Without bidi, users cannot see printer status from their RDS session and the print driver cannot adapt to the printer's available options. Bidi requires the Easy Print driver path and the client to support bidi reporting.",
                    Tags = ["rds", "bidi", "printer-status", "toner", "bidirectional"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Redirected printers report toner, paper, and status information to the RDS session. Users see accurate printer capability information in print dialogs. Requires Easy Print driver and a printer with bidi reporting capability.",
                    ApplyOps = [RegOp.SetDword(TsKey, "BidiComm", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "BidiComm")],
                    DetectOps = [RegOp.CheckDword(TsKey, "BidiComm", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-set-max-redirected-printers-5",
                    Label = "Printer Redirection: Limit Redirected Printers Per Session to 5",
                    Category = "Maintenance",
                    Description =
                        "Sets MaxRedirectedPrinters=5 in Terminal Services policy. Caps the maximum number of client printers that can be redirected into a single RDS session. Without this limit, a user with 20+ printers installed (e.g., a power user with many VPN-connected branch printers) will have all 20 mapped into every session — consuming substantial memory and logon time on the session host server. Limiting to 5 redirected printers covers virtually all legitimate printing needs while preventing excessive session host resource consumption from clients with large printer inventories.",
                    Tags = ["rds", "printer-limit", "session", "performance", "resource"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "At most 5 client printers are mapped per RDS session. Clients with more than 5 printers have their first 5 (by enumeration order) mapped. Users rarely need more than 5 printers in a session. Configure in conjunction with default-printer-only policy for maximum benefit.",
                    ApplyOps = [RegOp.SetDword(TsKey, "MaxRedirectedPrinters", 5)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "MaxRedirectedPrinters")],
                    DetectOps = [RegOp.CheckDword(TsKey, "MaxRedirectedPrinters", 5)],
                },
                new TweakDef
                {
                    Id = "prtred-use-compression-for-print-data",
                    Label = "Printer Redirection: Enable Compression for Redirected Print Data",
                    Category = "Maintenance",
                    Description =
                        "Sets CompressPrintData=1 in Terminal Services policy. Enables compression of print job data transmitted through the RDP printer redirection channel. Print job data (especially EMF) can be highly compressible — text-heavy documents may compress by 80%+. Without compression, printing large documents over WAN-connected RDS sessions consumes significant RDP session bandwidth. With compression enabled, the RDP virtual channel compresses the print data stream before transmission, reducing the bandwidth and time required to print large documents over slow connections.",
                    Tags = ["rds", "print-compression", "bandwidth", "wan", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print data is compressed before transmission through the RDP channel. Significant bandwidth savings for text-heavy documents over WAN connections. Minor CPU overhead on the session host for compression. No user-visible impact.",
                    ApplyOps = [RegOp.SetDword(TsKey, "CompressPrintData", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "CompressPrintData")],
                    DetectOps = [RegOp.CheckDword(TsKey, "CompressPrintData", 1)],
                },
                new TweakDef
                {
                    Id = "prtred-allow-only-easy-print-fallback",
                    Label = "Printer Redirection: Use Easy Print as Exclusive Fallback Driver",
                    Category = "Maintenance",
                    Description =
                        "Sets FallbackToEasyPrint=1 in Terminal Services policy. Configures RDS to use the Easy Print driver as the exclusive fallback when the client printer's specific driver is not installed on the session host. Without this setting, if the specific printer driver is absent, redirection may fail entirely or attempt to download the driver automatically. With FallbackToEasyPrint enabled, any printer whose driver is not on the server falls back to Easy Print — ensuring the printer is always usable even if not optimally configured. Eliminates 'Printer unavailable' errors from driver-absent conditions.",
                    Tags = ["rds", "easy-print", "fallback-driver", "printer-availability", "rdp"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printers without a matching server-side driver fall back to Easy Print. All client printers are available in sessions, potentially with reduced feature sets. Eliminates printer redirection failures due to missing drivers without requiring driver installation on session hosts.",
                    ApplyOps = [RegOp.SetDword(TsKey, "FallbackToEasyPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(TsKey, "FallbackToEasyPrint")],
                    DetectOps = [RegOp.CheckDword(TsKey, "FallbackToEasyPrint", 1)],
                },
            ];
    }

    // ── PrintJobManagementPolicy ──
    private static class _PrintJobManagementPolicy
    {
        private const string JobKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\JobManagement";

        private const string PrtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtjob-purge-jobs-on-restart",
                    Label = "Print Job Management: Purge All Print Jobs on Spooler Restart",
                    Category = "Maintenance",
                    Description =
                        "Sets PurgeJobsOnRestart=1 in JobManagement policy. Clears all pending print jobs from all print queues when the Print Spooler service restarts. By default, the spooler preserves queued jobs across restarts, which can cause problems when a restarted spooler encounters corrupted spool files (EMF or RAW) from a failed previous session — leading to an infinite loop where the spooler starts, crashes processing a bad job, and restarts. Purging on restart ensures the spooler always starts with a clean queue. Lost jobs must be resubmitted by users.",
                    Tags = ["print-job", "spooler-restart", "queue-purge", "stability", "recovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Pending print jobs are lost when the spooler restarts (service restart, machine reboot). Users must resubmit their print jobs. Prevents spooler crash loops caused by corrupted spool files. Useful on print servers with history of spooler instability.",
                    ApplyOps = [RegOp.SetDword(JobKey, "PurgeJobsOnRestart", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "PurgeJobsOnRestart")],
                    DetectOps = [RegOp.CheckDword(JobKey, "PurgeJobsOnRestart", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-max-spool-file-size-1gb",
                    Label = "Print Job Management: Set Maximum Spool File Size to 1 GB",
                    Category = "Maintenance",
                    Description =
                        "Sets MaxSpoolFileSize=1073741824 in JobManagement policy (1 GB in bytes). Sets the maximum allowed size for individual print spool files. Without a spool file size limit, a single print job (e.g., a 10,000-page CAD print run or a large PDF) can generate a spool file that consumes all available disk space on the print server, starving all other users' jobs. 1 GB is sufficient for most large-format print jobs while protecting against runaway spool generation. Jobs exceeding the limit are rejected with a 'Spool file too large' error.",
                    Tags = ["print-job", "spool-file", "disk-space", "limit", "print-server"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print jobs exceeding 1 GB spool file size are rejected. Very large print jobs (high-resolution CAD, book-length PDFs) may need to be split into smaller jobs. Protects print server disk from runaway spool consumption.",
                    ApplyOps = [RegOp.SetDword(JobKey, "MaxSpoolFileSize", 1073741824)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "MaxSpoolFileSize")],
                    DetectOps = [RegOp.CheckDword(JobKey, "MaxSpoolFileSize", 1073741824)],
                },
                new TweakDef
                {
                    Id = "prtjob-enable-separator-page",
                    Label = "Print Job Management: Enable Separator Page Between Print Jobs",
                    Category = "Maintenance",
                    Description =
                        "Sets UseSeparatorPage=1 in JobManagement policy. Enables job separator pages (banner pages) between print jobs. A separator page is a printed page inserted before each job containing: user name, date, time, and job ID. In shared printer environments, separator pages allow users to find their document among others' output in the printer tray output bin. Without separator pages, documents from multiple users in a busy shared printer pile together, causing users to accidentally take others' confidential documents — a physical information disclosure risk.",
                    Tags = ["print-job", "separator-page", "banner-page", "physical-security", "shared-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "A separator page is printed before each job. One additional page of toner consumed per job. Users can identify their documents in the output tray. Separator page format is configured per-printer (PCL, PostScript, Windows default).",
                    ApplyOps = [RegOp.SetDword(JobKey, "UseSeparatorPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "UseSeparatorPage")],
                    DetectOps = [RegOp.CheckDword(JobKey, "UseSeparatorPage", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-job-expiry-8hours",
                    Label = "Print Job Management: Expire Unprinted Jobs After 8 Hours",
                    Category = "Maintenance",
                    Description =
                        "Sets JobExpiryHours=8 in JobManagement policy. Automatically removes print jobs that have been queued but not processed (printed) within 8 hours. Jobs can accumulate in a queue when a printer is taken offline, goes into an error state, or is deliberately paused. Without expiry, a queue can accumulate hundreds of stale jobs — some of which may contain sensitive documents submitted by users who no longer need them. 8 hours aligns with a standard business day — a job submitted in the morning and not printed by end of day is auto-purged.",
                    Tags = ["print-job", "expiry", "queue-management", "security", "cleanup"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print jobs not printed within 8 hours are automatically removed. Users must resubmit jobs if the printer was unavailable for more than 8 hours. Prevents accumulation of stale documents in print queues.",
                    ApplyOps = [RegOp.SetDword(JobKey, "JobExpiryHours", 8)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "JobExpiryHours")],
                    DetectOps = [RegOp.CheckDword(JobKey, "JobExpiryHours", 8)],
                },
                new TweakDef
                {
                    Id = "prtjob-disable-interactive-print-sharing",
                    Label = "Print Job Management: Disable Interactive Console Print Sharing",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableInteractivePrinterSharing=1 in Printers policy. Prevents users from interactively sharing printers through the Windows Printer Properties dialog. Without this restriction, any local user can share their local printer to the network — creating unmanaged, unmonitored print shares that bypass central print server controls. Printer sharing should only be managed through Group Policy printer deployment or by administrators. Unmanaged printer shares can also have misconfigured permissions, allowing unauthenticated network print access.",
                    Tags = ["print-job", "printer-sharing", "management", "control", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "The 'Share this printer' checkbox in Printer Properties is disabled. Users cannot share their local printers to the network. Print sharing is only possible by administrators via Print Management or Group Policy.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableInteractivePrinterSharing")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableInteractivePrinterSharing", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-enforce-default-queue-priority",
                    Label = "Print Job Management: Enforce Default Queue Priority Level (49)",
                    Category = "Maintenance",
                    Description =
                        "Sets DefaultPriority=49 in JobManagement policy. Sets the default print job priority to 49 (scale of 1-99, where 99 is highest). When a user does not specify a priority or when they have priority escalation rights, print jobs default to priority 49. This ensures administrators can designate executive or time-critical queues with priority 50+ that will always preempt standard user jobs. Without a defined default, systems may inherit OS defaults that vary between Windows versions, making priority management unpredictable.",
                    Tags = ["print-job", "priority", "queue-management", "fairness", "scheduling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Standard user print jobs use priority 49. Priority 50-99 reserved for administrator-managed high-priority queues. No change in observed behaviour for most users — priority ordering is internal to the print queue scheduler.",
                    ApplyOps = [RegOp.SetDword(JobKey, "DefaultPriority", 49)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "DefaultPriority")],
                    DetectOps = [RegOp.CheckDword(JobKey, "DefaultPriority", 49)],
                },
                new TweakDef
                {
                    Id = "prtjob-set-spool-directory-to-secured",
                    Label = "Print Job Management: Set Secure Spool Directory ACL Enforcement",
                    Category = "Maintenance",
                    Description =
                        "Sets SecureSpoolDirectory=1 in JobManagement policy. Enables ACL enforcement on the print spool directory (%SystemRoot%\\System32\\spool\\PRINTERS). By default this directory has permissive ACLs that allow any authenticated user to read or delete spool files. Spool files contain the raw or EMF rendering of documents being printed — reading them is equivalent to reading the document. With SecureSpoolDirectory enabled, only the SYSTEM account and print administrators can read spool files. Standard users cannot access other users' spool files.",
                    Tags = ["print-job", "spool-directory", "acl", "file-security", "information-disclosure"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "The print spool directory is protected with restrictive ACLs. Standard users cannot read or delete other users' spool files. Third-party print monitoring software that reads spool files directly may require the SYSTEM or print administrator context.",
                    ApplyOps = [RegOp.SetDword(JobKey, "SecureSpoolDirectory", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "SecureSpoolDirectory")],
                    DetectOps = [RegOp.CheckDword(JobKey, "SecureSpoolDirectory", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-disable-printer-status-popup",
                    Label = "Print Job Management: Disable Print Status Notification Popups",
                    Category = "Maintenance",
                    Description =
                        "Sets DisablePrinterstatusNotifications=1 in JobManagement policy. Prevents the print status notification system tray balloon and popup messages from appearing when a print job completes successfully. In enterprise environments with high print volumes, completed print job notifications are a source of notification fatigue — users who print dozens of documents per day receive an equal number of transient notifications that they learn to dismiss immediately. Disabling successful-completion notifications reduces noise; error notifications (failure, out of paper) are separately configurable and should remain enabled.",
                    Tags = ["print-job", "notifications", "user-experience", "task-bar", "status"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print job completion notifications are suppressed in the system tray. Users are not notified when their document successfully prints. Error notifications (print failure, printer offline) can be separately enabled. Reduces notification clutter in high-volume printing environments.",
                    ApplyOps = [RegOp.SetDword(JobKey, "DisablePrinterstatusNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "DisablePrinterstatusNotifications")],
                    DetectOps = [RegOp.CheckDword(JobKey, "DisablePrinterstatusNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-enable-emf-spool-format",
                    Label = "Print Job Management: Use Enhanced Metafile (EMF) Spooling Format",
                    Category = "Maintenance",
                    Description =
                        "Sets UseEMFSpool=1 in JobManagement policy. Configures the print spooler to spool print jobs in Enhanced Metafile (EMF) format rather than the RAW (device-ready) format. EMF spooling returns control to the application faster — the application finishes its print call as soon as the EMF commands are written to the spool file, rather than waiting for the full rasterisation to the printer's native format. The spooler then renders EMF to RAW in the background. Faster application hand-off is the primary benefit; the trade-off is that EMF rendering errors are deferred to the spooler.",
                    Tags = ["print-job", "emf", "spool-format", "performance", "application-responsiveness"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print calls return to the application faster. Rendering errors may not surface until after the application has completed the print call. EMF spool files are larger than RAW until rendered; more temporary disk space is needed during active large print jobs.",
                    ApplyOps = [RegOp.SetDword(JobKey, "UseEMFSpool", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "UseEMFSpool")],
                    DetectOps = [RegOp.CheckDword(JobKey, "UseEMFSpool", 1)],
                },
                new TweakDef
                {
                    Id = "prtjob-block-untrusted-printer-fonts",
                    Label = "Print Job Management: Block Untrusted Fonts in Print Jobs",
                    Category = "Maintenance",
                    Description =
                        "Sets BlockUntrustedFonts=1 in JobManagement policy. Blocks loading of fonts from untrusted sources within print job processing. The Windows font parsing subsystem has historically been a high-value attack target — multiple CVEs involve malformed fonts causing kernel memory corruption during parsing. Print jobs submitted from remote clients can contain embedded fonts. By blocking fonts that are not installed in the trusted Windows font store, the attack surface for font-based exploitation via print jobs is reduced. Print jobs with embedded, untrusted fonts may render with fallback system fonts.",
                    Tags = ["print-job", "font", "untrusted-fonts", "kernel", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Print jobs containing fonts not in the system font store render with fallback fonts. Documents with custom branding fonts may look different when printed if those fonts are not installed on the print server. Impact visible on print servers processing documents with embedded non-standard fonts.",
                    ApplyOps = [RegOp.SetDword(JobKey, "BlockUntrustedFonts", 1)],
                    RemoveOps = [RegOp.DeleteValue(JobKey, "BlockUntrustedFonts")],
                    DetectOps = [RegOp.CheckDword(JobKey, "BlockUntrustedFonts", 1)],
                },
            ];
    }

    // ── PrintManagementPolicy ──
    private static class _PrintManagementPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrintManagement";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtmgmt-disable-mmc",
                Label = "Disable Print Management MMC Console",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Print Management MMC console provides a graphical interface for managing printers, print queues, and printer servers across an organization. Disabling the Print Management MMC console prevents users and non-print-admin accounts from accessing this powerful management interface. Centralized print management is handled by designated print server administrators rather than individual workstation users. Restricting access to the MMC prevents accidental or unauthorized printer configuration changes. Printer management tasks are delegated to specialized IT staff through proper RBAC mechanisms. Standard print functionality including printing documents and managing local print jobs remains fully accessible to users.",
                Tags = ["printing", "management", "mmc", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementMmc", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementMmc")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementMmc", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-driver-autoinstall",
                Label = "Disable Printer Driver Auto-Install",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Printer driver auto-installation automatically downloads and installs printer drivers when a network printer is connected or added. Disabling auto-installation prevents arbitrary printer drivers from being installed from network sources without administrator approval. The Windows print driver ecosystem has historically been a significant source of privilege escalation vulnerabilities including PrintNightmare. Requiring administrator approval for all printer driver installations enforces a curated and tested driver baseline. Enterprise print environments deploy standardized, approved driver packages through SCCM or similar management platforms. Blocking automatic driver installation is a key mitigation for print spooler attack vectors on managed endpoints.",
                Tags = ["printing", "drivers", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDriverAutoInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDriverAutoInstall")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDriverAutoInstall", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-default-mgmt",
                Label = "Disable Default Printer Management",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Windows automatically manages the default printer by changing it to the most recently used printer when the dynamic default printer setting is active. Disabling default printer management through policy prevents Windows from automatically changing the configured default printer. Enterprise print environments rely on precise default printer assignments tied to physical location or department, which should not be dynamically changed. Automatic default printer changes cause user confusion and support calls when the wrong printer is selected for important documents. Lock-in of the default printer to a specific device is commonly required for compliance and operational workflow reasons. Disabling dynamic default management ensures the IT-configured default printer assignment remains stable.",
                Tags = ["printing", "default-printer", "management", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDefaultPrinterManagement", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultPrinterManagement")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDefaultPrinterManagement", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-queue-sharing",
                Label = "Disable Print Queue Sharing",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Print queue sharing allows workstations to act as print servers by sharing locally connected printers to other network clients. Disabling queue sharing prevents workstations from hosting shared print queues, consolidating print spooler exposure to designated print servers. Workstation-based print sharing expands the print spooler attack surface to a larger number of targets on the network. Enterprise printing should be managed through dedicated print servers with hardened configurations and regular patching. Reducing the number of hosts running print server functionality limits the blast radius of print-related vulnerabilities. Centralized print servers with properly configured access controls provide superior audit and management capabilities compared to peer-to-peer sharing.",
                Tags = ["printing", "sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableQueueSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableQueueSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableQueueSharing", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-print-pdf-rdp",
                Label = "Disable Print to PDF from RDP",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Print to PDF from Remote Desktop sessions allows users to redirect print jobs from remote desktop sessions to local PDF files on the connecting client. Disabling this feature prevents document exfiltration through the Print to PDF mechanism in RDP sessions. Sensitive documents viewed in remote desktop sessions can be saved locally on unauthorized or unmanaged client devices via PDF redirection. Data governance policies require that documents accessed through remote desktop remain on the enterprise endpoint and are not redirected to potentially unmanaged clients. Print redirection in general represents a data movement risk in RDP scenarios handling confidential information. Organizations with strict data residency requirements should disable all print redirection in remote desktop sessions.",
                Tags = ["printing", "rdp", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfFromRdp", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfFromRdp")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfFromRdp", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-telemetry",
                Label = "Disable Print Management Telemetry",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Print management telemetry reports usage statistics about printer configurations, print job characteristics, and driver versions to Microsoft. This data helps improve printer compatibility and print subsystem performance in future Windows releases. Disabling print management telemetry prevents printer configuration and usage data from being transmitted outside the enterprise. Print infrastructure details including printer models, drivers, and queue configurations represent sensitive IT asset information. Enterprise print environment information should not be disclosed through telemetry to external parties. Print functionality is completely unaffected by disabling this telemetry stream.",
                Tags = ["printing", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintManagementTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintManagementTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintManagementTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-discovery",
                Label = "Disable Printer Discovery",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Printer discovery uses WS-Discovery, mDNS, and network broadcast protocols to automatically find printers on the local network. Disabling printer discovery prevents workstations from automatically enumerating network printers and presenting them to users for installation. Enterprise printer deployments are managed through Group Policy printer deployment, not automatic discovery. Automatic printer discovery can expose users to rogue printers or allow installation of unapproved printer devices. Centrally managed printer policies ensure users only have access to approved printers with validated drivers. Disabling discovery does not remove previously installed network printers from the device.",
                Tags = ["printing", "discovery", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrinterDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrinterDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-xps-writer",
                Label = "Disable Microsoft XPS Document Writer",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "The Microsoft XPS Document Writer is a virtual printer that saves print output as XPS format files on the local filesystem. Disabling the XPS Document Writer prevents users from saving print jobs as local XPS files, limiting an alternate document export path. XPS files created through the virtual printer can bypass DLP controls that monitor file creation through standard save dialogs. In environments where Microsoft Print to PDF is also disabled, disabling the XPS writer eliminates file-based print redirection. Enterprise document management workflows should use approved document export paths rather than virtual printer mechanisms. Disabling this virtual printer reduces the attack surface while preserving all physical printer functionality.",
                Tags = ["printing", "xps", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMicrosoftXpsDocumentWriter")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMicrosoftXpsDocumentWriter", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-internet-printing",
                Label = "Disable Internet Printing",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Internet Printing Protocol (IPP) allows clients to submit print jobs to remote printers over HTTP connections including those on the public internet. Disabling internet printing prevents the Windows print spooler from acting as an IPP client or accepting IPP-based print requests from the internet. Internet-accessible print services represent an attack vector used in PrintNightmare and related print spooler exploitation chains. Enterprise printers are managed on segmented internal networks and should not be accessible or reachable via internet-routable paths. Removing internet printing capability eliminates printer-based lateral movement vectors used in network attacks. All internal network printing through standard SMB and IPP on the corporate LAN remains unaffected.",
                Tags = ["printing", "internet", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInternetPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetPrinting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInternetPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtmgmt-disable-cloud-print-sharing",
                Label = "Disable Cloud Print Sharing",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Cloud print sharing allows enterprise printers to be shared through Microsoft's cloud printing infrastructure for access from mobile devices and remote workers. Disabling cloud print sharing prevents printers from being registered with cloud print services and accessed through cloud endpoints. Cloud-shared printers send print jobs through Microsoft's cloud infrastructure, which creates data residency and confidentiality concerns for sensitive documents. Organizations handling classified, regulated, or sensitive documents should ensure all print paths remain within the enterprise boundary. Cloud print sharing functionality is appropriate for consumer scenarios but requires careful data governance evaluation for enterprise use. Disabling cloud print sharing ensures all print data stays within the physical enterprise network infrastructure.",
                Tags = ["printing", "cloud", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintSharing", 1)],
            },
        ];
    }

    // ── PrintQueuePolicy ──
    private static class _PrintQueuePolicy
    {
        private const string PrtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        private const string RpcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\RPC";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtq-disable-spooler-on-non-print-servers",
                    Label = "Print Queue: Disable Print Spooler Service on Non-Print Servers",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableSpooler=1 in Printers policy. Disables the Print Spooler service on machines that are not designated print servers. The Print Spooler service has been the subject of critical vulnerabilities including PrintNightmare (CVE-2021-34527) and SpoolFool (CVE-2022-22718). Every machine running the spooler is a potential target. Domain controllers, application servers, and most workstations do not need to act as print servers. Disabling the spooler on these machines eliminates the entire attack surface — the only cost is that users cannot share their local printers with other network users from that machine.",
                    Tags = ["print-spooler", "printnightmare", "cve-2021-34527", "security", "attack-surface"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "The Print Spooler service is disabled. Users cannot print from this machine unless it connects to a remote print server via the Remote Procedure Call path. Local printer installation is blocked. Apply to servers only — do not apply on workstations if users print locally.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableSpooler", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableSpooler")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableSpooler", 1)],
                },
                new TweakDef
                {
                    Id = "prtq-require-rpc-authentication",
                    Label = "Print Queue: Require RPC Authentication for Printer Client Connections",
                    Category = "Maintenance",
                    Description =
                        "Sets RpcUseNamedPipeProtocol=1 in Printers/RPC policy. Requires authenticated named pipe (rather than anonymous TCP) for RPC connections to print servers. Unauthenticated or weakly-authenticated RPC endpoints allow attackers to send RPC calls to printers without valid credentials — exploitable by several PrintNightmare-era attack chains. By requiring authenticated named pipe transport, each print spooler RPC call is associated with a verified security principal, enabling access control and audit logging of all print server interactions.",
                    Tags = ["print-rpc", "authentication", "named-pipe", "security", "printnightmare"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Print server RPC connections use authenticated named pipes only. Domain-joined clients connecting with valid Kerberos/NTLM credentials are unaffected. Non-domain clients or applications using anonymous RPC to print servers may fail to connect.",
                    ApplyOps = [RegOp.SetDword(RpcKey, "RpcUseNamedPipeProtocol", 1)],
                    RemoveOps = [RegOp.DeleteValue(RpcKey, "RpcUseNamedPipeProtocol")],
                    DetectOps = [RegOp.CheckDword(RpcKey, "RpcUseNamedPipeProtocol", 1)],
                },
                new TweakDef
                {
                    Id = "prtq-disable-web-based-printing",
                    Label = "Print Queue: Disable Web-Based Printer Queue Management",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableWebBasedPrinting=1 in Printers policy. Disables the Internet Information Services (IIS)-based web print queue management interface that allows users to manage print jobs via a browser on port 80. The web-based print management component requires IIS and opens an additional HTTP listener. In enterprise environments, print queue management is performed by IT via the Print Management MMC snap-in. Exposing a web interface for print queue management on domain print servers creates an unnecessary attack surface on the internal network.",
                    Tags = ["print", "web-printing", "iis", "attack-surface", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "The IIS-based web print queue manager is disabled. Users cannot manage print jobs via the http://servername/printers web portal. Standard client-side print job management via the taskbar or Print Management console is unaffected.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableWebBasedPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableWebBasedPrinting")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableWebBasedPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "prtq-enable-spooler-event-logging",
                    Label = "Print Queue: Enable Print Spooler Event Logging",
                    Category = "Maintenance",
                    Description =
                        "Sets EnableEventLogging=1 in Printers policy. Enables detailed event logging in the Microsoft-Windows-PrintService/Operational event channel. Print spooler events record: job submitted, job printed, job failed, driver installed, printer added, printer deleted. Without this logging, detecting abuse of the print spooler (lateral movement, privilege escalation attempts, sensitive document printing) is impossible. The operational log is disabled by default to reduce log volume — enabling it on high-value machines (DCs, app servers, HR workstations) provides a forensic trail.",
                    Tags = ["print-spooler", "event-log", "audit", "monitoring", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print spooler operational events are logged. Minor disk overhead for log writes. Events include job processing, driver activity, and printer configuration changes. Useful for DLP monitoring on machines with access to sensitive documents.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "EnableEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "EnableEventLogging")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "EnableEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "prtq-disable-auto-download-of-drivers",
                    Label = "Print Queue: Disable Automatic Download of Printer Drivers from Windows Update",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableWindowsUpdateDriverSearching=1 in Printers policy. Prevents the Print Spooler from automatically downloading and installing printer drivers from Windows Update when a new printer is detected. Automatic driver downloads from Windows Update bypass the enterprise software approval process: the driver may not be tested in the organisation's environment, may contain outdated firmware, or might be a supply-chain compromised update. Enterprise environments should pre-stage approved drivers in driver stores and deploy them via Group Policy or Intune.",
                    Tags = ["printer-driver", "windows-update", "auto-download", "approval", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printer drivers are not auto-downloaded from Windows Update. When a new printer is detected without a local driver, users see 'Driver not found' rather than automatic download. IT must pre-stage or push approved drivers.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableWindowsUpdateDriverSearching", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableWindowsUpdateDriverSearching")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableWindowsUpdateDriverSearching", 1)],
                },
                new TweakDef
                {
                    Id = "prtq-enable-lpd-service-logging",
                    Label = "Print Queue: Enable Line Printer Daemon Service Audit Logging",
                    Category = "Maintenance",
                    Description =
                        "Sets EnableLpdLogging=1 in Printers policy. Enables audit logging for the Line Printer Daemon (LPD) service when it is installed. LPD is the Unix/Linux print protocol listener (TCP port 515) that allows Unix-style lpr/lpq clients to submit print jobs to Windows print servers. LPD lacks authentication and is disabled by default on Windows Server, but legacy environments that enable it for Unix/Linux compatibility should maintain an audit log of all LPD print submissions. The log provides the source IP, user name, and document name for every LPD print job.",
                    Tags = ["lpd", "lpr", "print-audit", "unix-print", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "LPD service print job events are logged if the LPD service is installed and running. No impact if LPD is not installed. LPD service is disabled by default.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "EnableLpdLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "EnableLpdLogging")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "EnableLpdLogging", 1)],
                },
            ];
    }

    // ── PrintSpoolAdvPolicy ──
    private static class _PrintSpoolAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prtspool-disable-point-and-print-unrestricted",
                Label = "Disable Unrestricted Point and Print Driver Installation",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Point and Print allows non-administrative users to install printer drivers from print servers which was exploited in PrintNightmare (CVE-2021-34527) to achieve remote code execution. Disabling unrestricted Point and Print requires administrator approval for all printer driver installations preventing non-admin users from silently installing potentially malicious drivers. The PrintNightmare vulnerability affected all versions of Windows and had critical severity because print spool runs as SYSTEM allowing full system compromise through printer driver installation. Restricting Point and Print to specific approved print servers further limits the attack surface compared to completely unrestricted driver installation. Organizations should combine Point and Print restrictions with disabling the Print Spooler service on systems that do not need printing capability. Microsoft released multiple patches for PrintNightmare and organizations should ensure all patches are applied in addition to implementing this Group Policy restriction.",
                Tags = ["print-spool", "point-and-print", "printnightmare", "driver", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Restricted", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Restricted")],
                DetectOps = [RegOp.CheckDword(Key, "Restricted", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-restrict-point-and-print-servers",
                Label = "Restrict Point and Print to Approved Print Server List",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting Point and Print to a specific allowlist of approved print servers prevents users from installing printer drivers from arbitrary or attacker-controlled print servers. Print server allowlists are managed through the TrustedServers registry value and contain hostnames or IP addresses of organizational print servers. Unrestricted print server access allows an attacker to set up a rogue print server and convince users to connect to it which silently installs malicious drivers. Print server restrictions combined with driver installation approval requirements provide layered defense against print driver based attacks. Organizations should define all authorized print servers in the Group Policy allowlist and review it during server decommissioning to remove stale entries. Users attempting to connect to non-allowlisted print servers should receive an error message directing them to submit a request for an approved print server.",
                Tags = ["print-spool", "server-restriction", "point-and-print", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TrustedServersEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TrustedServersEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "TrustedServersEnabled", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-print-spooler-remote-rpc",
                Label = "Disable Remote Print Spooler RPC Connections",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "The Print Spooler service exposes Remote Procedure Call interfaces on the network that were used to exploit PrintNightmare and the Windows Print Spooler Remote Code Execution vulnerability series. Disabling remote print spooler RPC connections prevents network-based exploitation of print spooler vulnerabilities by blocking the RPC endpoint from remote access. Print spooler RPC is most dangerous on domain controllers where compromise of the print spooler runs as SYSTEM and can lead to full domain compromise. Organizations running Windows Server Core deployments where printing is not required should disable the Print Spooler service entirely along with its RPC exposure. Print workstations and print servers require the print spooler but should restrict it to local connections and authorized traffic only. The RegisterSpoolerRemoteRpcEndPoint value set to 2 disables remote RPC while allowing local printing to continue.",
                Tags = ["print-spool", "rpc", "remote-access", "printnightmare", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RegisterSpoolerRemoteRpcEndPoint", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "RegisterSpoolerRemoteRpcEndPoint")],
                DetectOps = [RegOp.CheckDword(Key, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-web-printing-communication",
                Label = "Disable Windows Internet Printing Protocol Communication",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Internet Printing Protocol (IPP) over HTTP/HTTPS allows printing to printers and print servers over the internet which is rarely needed and creates unnecessary attack surface. Disabling web printing communication prevents users from connecting to internet-based print servers and removes HTTPS-based printer communication channels. Internet printing can be used by attackers to exfiltrate data by printing to an attacker-controlled IPP server over HTTPS bypassing traditional data loss prevention. Windows Internet Printing is implemented through the Internet Printing Client feature which can be removed through Programs and Features for systems that do not require it. Organizations should audit whether any users require internet printing capability and disable it for those who do not. IPP printing to internal corporate printers over secure networks may still use the IPP protocol through internal print servers without requiring the Internet Printing feature.",
                Tags = ["print-spool", "ipp", "internet-printing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWebPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWebPrinting")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWebPrinting", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-redirect-print-spool-directory",
                Label = "Restrict Print Spooler Directory to Non-System Drive",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "The print spooler temporary directory stores print jobs before they are sent to the printer and is a location that historically has been exploited for privilege escalation. Redirecting the print spooler directory to a non-system drive with restricted permissions reduces the risk of print-related privilege escalation through directory manipulation. Print spooler exploitation often involves writing malicious DLLs or executables to the spool directory which gets loaded by the SYSTEM-level spooler process. Configuring a dedicated print spool directory on a non-system partition with appropriate ACLs reduces the ability to inject code into the print spooler execution context. Organizations should set strict permissions on the print spooler directory allowing only the print spooler service account to write to it. Monitoring for unexpected file creation in the print spool directory provides detection capability for attempted print spooler attacks.",
                Tags = ["print-spool", "directory-security", "privilege-escalation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceGuestAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceGuestAccess")],
                DetectOps = [RegOp.CheckDword(Key, "ForceGuestAccess", 0)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-print-spool-named-pipe",
                Label = "Disable Print Spooler Named Pipe Access for Non-Admins",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Print spooler named pipes provide a local IPC channel for print management that can be exploited by local attackers to relay credentials or exploit spooler vulnerabilities. Disabling print spooler named pipe access for non-administrators limits the attack surface available to standard users who want to exploit the print spooler. The spooler named pipe wssprint2 was used in some print spooler exploits to relay authentication from the SYSTEM-level spooler process to attacker-controlled services. Limiting named pipe access to the print spooler reduces attackers' ability to force-authenticate the SYSTEM account through the print spooler using credential relay techniques. Organizations that disable named pipe access should test printing functionality thoroughly as some applications use named pipes to communicate with the print spooler. Monitoring for non-standard named pipe access to the print spooler service helps detect exploitation attempts.",
                Tags = ["print-spool", "named-pipe", "credential-relay", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInstallation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInstallation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInstallation", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-enable-detailed-spool-audit-events",
                Label = "Enable Detailed Audit Events for Print Spooler Operations",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Detailed print spooler audit events capture driver installations, print job submission, and administrative changes to the print infrastructure for security monitoring. Enabling detailed spooler audit events provides forensic data for investigating potential PrintNightmare exploitation attempts and unauthorized driver installations. Print spooler attack events are recorded in the Microsoft-Windows-PrintService/Admin and Operational event logs which should be captured by SIEM. Event ID 808 in the PrintService/Admin log records when a printer driver was added which is a key indicator for print-based attacks. Regular review of print service audit data helps identify unauthorized print server connections and driver installation attempts that may indicate attack attempts. Organizations should configure event log forwarding specifically for print service logs on systems where printing is a potential attack vector.",
                Tags = ["print-spool", "audit", "monitoring", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableDetailedAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableDetailedAudit")],
                DetectOps = [RegOp.CheckDword(Key, "EnableDetailedAudit", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-disable-print-to-file",
                Label = "Disable Print to File Functionality for Standard Users",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Print to File functionality in the print spooler allows output to be redirected to a file rather than a printer which can be used as a data exfiltration method on systems with strict network controls. Disabling print-to-file for standard users reduces this exfiltration vector preventing users from printing sensitive documents to file shares or removable media. Print-to-file combined with DLP policies can help enforce data handling requirements for sensitive documents that must only be printed to controlled printers. Organizations in regulated industries should evaluate whether print-to-file aligns with their data control policies before making individual decisions. PDF printers and virtual print drivers provide similar functionality to print-to-file and should be reviewed as part of a comprehensive print control policy. Disabling print-to-file does not prevent printing to physical printers and has no impact on operational printing activities.",
                Tags = ["print-spool", "print-to-file", "data-exfiltration", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrintToFile", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToFile")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrintToFile", 1)],
            },
            new TweakDef
            {
                Id = "prtspool-enforce-print-driver-signing",
                Label = "Enforce Digital Signature Verification for Printer Drivers",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Printer driver signature enforcement ensures that only drivers signed by trusted certificate authorities can be installed by the print spooler service. Enforcing driver signature verification prevents installation of unsigned or improperly signed printer drivers that could contain rootkit-level malware. The PrintNightmare exploitation path relied on the ability to install arbitrary DLLs as printer drivers even when driver signing was supposed to be enforced. Strict driver signature requirements should require Extended Validation (EV) code signing certificates to minimize the risk of spoofed or stolen certificates being used to sign malicious drivers. Organizations should test driver signing enforcement with their existing printer fleet to ensure all deployed drivers have valid signatures before enforcement. Legacy printer drivers without valid signatures must be replaced with signed alternatives or the printers must be retired before enforcing strict driver signing.",
                Tags = ["print-spool", "driver-signing", "code-signing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InheritedPolicies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "InheritedPolicies")],
                DetectOps = [RegOp.CheckDword(Key, "InheritedPolicies", 1)],
            },
        ];
    }

    // ── PrintSpoolerAdvancedPolicy ──
    private static class _PrintSpoolerAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "spladv-disable-mxdc-rendering",
                    Label = "Disable MXDC Package Rendering in Print Spooler",
                    Category = "Maintenance",
                    Description =
                        "Disables the Microsoft XPS Document Converter (MXDC) rendering path in the spooler, blocking an attack vector where malicious XPS documents exploit the spooler RPC interface.",
                    Tags = ["spooler", "xps", "mxdc", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "MXDC XPS rendering disabled in spooler; XPS print conversion requires an alternative method.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMXDCRendering", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMXDCRendering")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMXDCRendering", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-disable-printer-browse-list",
                    Label = "Disable Printer Browse List on Domain",
                    Category = "Maintenance",
                    Description =
                        "Disables the automatic browse list that advertises available printers across a domain, reducing network discovery noise and preventing spooler-based reconnaissance.",
                    Tags = ["spooler", "printing", "browsing", "domain", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Printer browse list disabled; users must manually add printers by UNC path or IP.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePrinterBrowsing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinterBrowsing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePrinterBrowsing", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-block-print-to-xps",
                    Label = "Block Print to XPS Document Writer",
                    Category = "Maintenance",
                    Description =
                        "Blocks the Microsoft XPS Document Writer virtual printer, preventing users from saving print jobs to XPS format files and closing the XPS writer spooler attack surface.",
                    Tags = ["spooler", "xps", "virtual-printer", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "XPS Document Writer virtual printer disabled; cannot save print output as .xps files.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableXPSDocumentWriter", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableXPSDocumentWriter")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableXPSDocumentWriter", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-block-lpt-port-printing",
                    Label = "Block LPT Parallel Port Printer Access",
                    Category = "Maintenance",
                    Description =
                        "Blocks the spooler from accessing LPT (parallel port) printer connections, removing a legacy attack surface on systems that do not have or use parallel port printers.",
                    Tags = ["spooler", "lpt", "parallel-port", "legacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "LPT parallel port printing blocked; legacy parallel-port printers not usable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLPTPortPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLPTPortPrinting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLPTPortPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-disable-com-port-printing",
                    Label = "Block COM Serial Port Printer Access",
                    Category = "Maintenance",
                    Description =
                        "Blocks the spooler from accessing COM (serial port) printer connections, removing legacy serial printing capability that is not needed on modern systems.",
                    Tags = ["spooler", "com-port", "serial-port", "legacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "COM serial port printing blocked; legacy serial-port printers not usable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCOMPortPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCOMPortPrinting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCOMPortPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-block-outbound-spool-jobs",
                    Label = "Block Outbound Print Job Forwarding from This Machine",
                    Category = "Maintenance",
                    Description =
                        "Prevents this Windows machine from forwarding print jobs to remote printers via the spooler, an attack path used to steal NTLM credentials (printer capture attacks).",
                    Tags = ["spooler", "printing", "ntlm-capture", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Outbound spooler job forwarding blocked; remote printer capture attacks (e.g., RespNTLM) mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "NoRemoteSpooler", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoRemoteSpooler")],
                    DetectOps = [RegOp.CheckDword(Key, "NoRemoteSpooler", 1)],
                },
                new TweakDef
                {
                    Id = "spladv-disable-spooler-inbound-access",
                    Label = "Disable Inbound Print Spooler RPC Access",
                    Category = "Maintenance",
                    Description =
                        "Disables the inbound RPC interface on the Print Spooler, preventing remote machines from submitting print jobs to this machine via the spooler, closing another PrintNightmare-family attack vector.",
                    Tags = ["spooler", "rpc", "security", "printnightmare", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Inbound spooler RPC disabled; this machine cannot be used as a print server by remote clients.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableSpoolerInboundRPC", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableSpoolerInboundRPC")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableSpoolerInboundRPC", 1)],
                },
            ];
    }

    // ── PrintSpoolerPolicy ──
    private static class _PrintSpoolerPolicy
    {
        private const string SpoolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";
        private const string PnPKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prtspool-require-signed-copy-files",
                    Label = "Require Signed Copy Files for PnP Printers",
                    Category = "Maintenance",
                    Description = "Restricts printer driver copy-files during PnP association to only allow digitally signed drivers.",
                    Tags = ["print-spooler", "copy-files", "pnp", "signed-driver", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 1 = allow copy-files only from signed drivers; blocks unsigned driver payloads used in PrintNightmare.",
                    ApplyOps = [RegOp.SetDword(SpoolKey, "CopyFilesPolicy", 1)],
                    RemoveOps = [RegOp.DeleteValue(SpoolKey, "CopyFilesPolicy")],
                    DetectOps = [RegOp.CheckDword(SpoolKey, "CopyFilesPolicy", 1)],
                },
                new TweakDef
                {
                    Id = "prtspool-pnp-no-trusted-servers",
                    Label = "Disable Trusted Print Server Exemption for Point and Print",
                    Category = "Maintenance",
                    Description = "Removes the trusted print server list exemption, requiring admin-level approval for ALL Point and Print servers.",
                    Tags = ["print-spooler", "point-and-print", "trusted-servers", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "No servers are automatically trusted for PnP driver download; all require explicit admin consent.",
                    ApplyOps = [RegOp.SetDword(PnPKey, "TrustedServers", 0)],
                    RemoveOps = [RegOp.DeleteValue(PnPKey, "TrustedServers")],
                    DetectOps = [RegOp.CheckDword(PnPKey, "TrustedServers", 0)],
                },
                new TweakDef
                {
                    Id = "prtspool-pnp-no-forest-trust",
                    Label = "Disable Forest-Level Trust for Point and Print",
                    Category = "Maintenance",
                    Description = "Disables the implicit trust granted to print servers in the same Active Directory forest for Point and Print.",
                    Tags = ["print-spooler", "point-and-print", "forest", "ad", "trust", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Forest-member print servers no longer bypass driver installation prompts; treats all servers equally.",
                    ApplyOps = [RegOp.SetDword(PnPKey, "InForest", 0)],
                    RemoveOps = [RegOp.DeleteValue(PnPKey, "InForest")],
                    DetectOps = [RegOp.CheckDword(PnPKey, "InForest", 0)],
                },
                new TweakDef
                {
                    Id = "prtspool-pnp-elevate-driver-install",
                    Label = "Require Elevation When Installing New Printer Drivers via PnP",
                    Category = "Maintenance",
                    Description = "Forces a UAC elevation prompt when a new printer driver is installed via Point and Print.",
                    Tags = ["print-spooler", "point-and-print", "uac", "elevation", "install", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Value 2 = always require elevation for new driver installs; closes a common PrintNightmare attack vector.",
                    ApplyOps = [RegOp.SetDword(PnPKey, "InstallDriverPromptSetting", 2)],
                    RemoveOps = [RegOp.DeleteValue(PnPKey, "InstallDriverPromptSetting")],
                    DetectOps = [RegOp.CheckDword(PnPKey, "InstallDriverPromptSetting", 2)],
                },
            ];
    }

    // ── PrintSpoolerSecurity ──
    private static class _PrintSpoolerSecurity
    {
        private const string Spooler = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Spooler";

        private const string SpoolerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        private const string SpoolerPointAndPrint = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PointAndPrint";

        private const string PrintNightmare = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Management";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "spool-disable-spooler-service",
                Label = "Disable Print Spooler Service (Non-Print Servers/Workstations)",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["spooler", "print", "service", "disable", "security"],
                Description =
                    "Disables the Print Spooler service (Start=4) on systems that never print. "
                    + "Eliminates the entire PrintNightmare attack surface. "
                    + "WARNING: all printing including PDF will stop working.",
                ApplyOps = [RegOp.SetDword(Spooler, "Start", 4)],
                RemoveOps = [RegOp.SetDword(Spooler, "Start", 2)],
                DetectOps = [RegOp.CheckDword(Spooler, "Start", 4)],
            },
            new TweakDef
            {
                Id = "spool-disable-spooler-remote-rpc",
                Label = "Disable Remote Print Spooler RPC (CVE-2021-1675 Mitigation)",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["spooler", "printnightmare", "rpc", "remote", "cve-2021-1675"],
                Description =
                    "Disables remote access to the print spooler via RPC by setting "
                    + "RegisterSpoolerRemoteRpcEndPoint=2. Mitigates PrintNightmare "
                    + "(CVE-2021-1675 / CVE-2021-34527) without fully disabling printing. "
                    + "Local print continues to work.",
                ApplyOps = [RegOp.SetDword(Spooler, "RegisterSpoolerRemoteRpcEndPoint", 2)],
                RemoveOps = [RegOp.DeleteValue(Spooler, "RegisterSpoolerRemoteRpcEndPoint")],
                DetectOps = [RegOp.CheckDword(Spooler, "RegisterSpoolerRemoteRpcEndPoint", 2)],
            },
            new TweakDef
            {
                Id = "spool-disable-printer-driver-download",
                Label = "Disable Automatic Printer Driver Download from Windows Update",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["spooler", "windows update", "driver download", "security"],
                Description =
                    "Prevents Windows from automatically downloading and installing printer "
                    + "drivers from Windows Update when a new printer is detected. "
                    + "ExcludeWUDriversInQualityUpdate=1. Ensures only manually approved drivers "
                    + "are installed.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions", "AllowUserDeviceClasses", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions", "AllowUserDeviceClasses"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DriverInstall\Restrictions",
                        "AllowUserDeviceClasses",
                        0
                    ),
                ],
            },
            new TweakDef
            {
                Id = "spool-disable-mxdw-pdf-writer",
                Label = "Disable Microsoft XPS Document Writer (MXDW) Printer",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["spooler", "xps", "mxdw", "printer driver", "cleanup"],
                Description =
                    "Prevents the Microsoft XPS Document Writer virtual printer from being "
                    + "added. The XPS format is largely superseded by PDF in Windows 10+. "
                    + "Reduces the number of virtual printers and simplifies the print dialog.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled", 0)],
                RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled")],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\MXDW", "MXDWInstalled", 0)],
            },
            new TweakDef
            {
                Id = "spool-log-spooler-events",
                Label = "Enable Print Spooler Event Logging",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["spooler", "event log", "audit", "logging"],
                Description =
                    "Ensures the Print Spooler logs detailed events to the Windows Event Log. "
                    + "EventLog=1. Enables forensic review of printer driver installations "
                    + "and spooler anomalies for security incident response.",
                ApplyOps = [RegOp.SetDword(SpoolerPolicy, "EventLog", 1)],
                RemoveOps = [RegOp.DeleteValue(SpoolerPolicy, "EventLog")],
                DetectOps = [RegOp.CheckDword(SpoolerPolicy, "EventLog", 1)],
            },
        ];
    }

    // ── PrintSpoolFinalPolicy ──
    private static class _PrintSpoolFinalPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\Cleanup";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "splfinal-enable-print-spooler-cleanup-on-idle",
                Label = "Enable Automatic Print Spooler Cleanup When Print Queue Is Idle",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enabling automatic print spooler cleanup when the print queue is idle removes completed print jobs and temporary spool files from the spooler directory ensuring that document content is not retained in the spool longer than necessary for the print operation. Print spool files contain document images in EMF or RAW format that may include sensitive content and should be removed promptly after the print job completes to minimize exposure. Automatic cleanup on idle conditions ensures that print spool data is cleared during normal operational periods without requiring administrative intervention for routine spool maintenance. Spool file cleanup reduces the attack surface on print servers by minimizing the window during which attackers can access spool files to recover document content. Organizations should verify that spool cleanup policies are applied consistently on all print servers and workstations with local print queues. Spool cleanup events should be logged to provide evidence that print data was disposed of appropriately for compliance reporting purposes.",
                Tags = ["print-spooler", "cleanup", "spool-files", "data-retention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolCleanupOnIdle", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolCleanupOnIdle")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolCleanupOnIdle", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-enforce-immediate-spool-file-deletion",
                Label = "Enforce Immediate Deletion of Print Spool Files After Job Completion",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enforcing immediate deletion of print spool files upon job completion eliminates the retention window during which print spool data would otherwise be recoverable from the spool directory on print servers and workstations. Immediate spool deletion is a defense against forensic recovery of document content from print infrastructure that has been accessed by an attacker. Organizations handling sensitive information under regulatory requirements may need to implement immediate spool deletion to satisfy data minimization requirements for printed document data. Immediate deletion should be applied to all stages of the print spool including temporary intermediate files generated during EMF to device format conversion. The deletion operation should be verified to ensure files are actually removed rather than simply marked for deletion by the file system. Secure deletion using file overwrite operations rather than simple deletion should be considered for high-security environments where forensic recovery of spool data poses a significant risk.",
                Tags = ["print-spooler", "immediate-deletion", "spool-files", "secure-disposal", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceImmediateSpoolFileDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceImmediateSpoolFileDeletion", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-restrict-orphan-spool-file-retention",
                Label = "Restrict Retention of Orphaned Print Spool Files to Mandatory Cleanup Period",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Orphaned print spool files resulting from failed or interrupted print jobs are retained in the spool directory indefinitely without automatic cleanup which creates unnecessary data accumulation and potential sensitive data exposure. Restricting orphaned spool file retention period to a maximum defined duration ensures that print data from failed jobs is automatically removed within a predictable timeframe. Long-term retention of orphaned spool files on print servers can accumulate large volumes of sensitive document data from all users who have sent print jobs to the server. Cleanup of orphaned spool files should be automated through the print spooler service rather than relying on manual administrator cleanup which may not occur regularly. The retention period for orphaned spool files should be set based on the sensitivity of the documents typically printed in the environment with shorter periods for environments processing sensitive regulated data. Cleanup operations for orphaned spool files should be logged to provide an audit trail of data disposal activities.",
                Tags = ["print-spooler", "orphaned-files", "cleanup", "spool-retention", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
                RemoveOps = [RegOp.DeleteValue(Key, "OrphanedSpoolFileRetentionHours")],
                DetectOps = [RegOp.CheckDword(Key, "OrphanedSpoolFileRetentionHours", 24)],
            },
            new TweakDef
            {
                Id = "splfinal-enable-secure-spool-file-overwrite",
                Label = "Enable Secure Multi-Pass Overwrite for Print Spool File Deletion",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enabling secure overwrite for spool files replaces the content of spool files with random data before deletion ensuring that the document data is irrecoverable from the storage media through standard data recovery utilities. Simple deletion of spool files marks the file system entry as free but does not overwrite the underlying disk sectors leaving document content recoverable until those sectors are reused by other files. Organizations that process classified or highly sensitive documents using print infrastructure should implement secure overwrite for spool files to satisfy media sanitization requirements. The performance impact of secure overwrite operations on print servers is generally low because spool files are relatively small but the impact should be tested before deployment in high-volume print environments. Secure overwrite should be applied to all temporary files generated during the print rendering process including intermediate format conversion files that may contain partial document images. Compliance documentation for sensitive data handling programs should reference secure spool file deletion as a control contributing to data disposal assurance.",
                Tags = ["print-spooler", "secure-overwrite", "data-sanitization", "spool-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSecureSpoolFileOverwrite")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSecureSpoolFileOverwrite", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-audit-spool-directory-access",
                Label = "Enable Audit Logging for Print Spool Directory File System Access Events",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enabling audit logging for print spool directory access events records all reads writes and deletions of files in the print spool directory providing visibility into unauthorized access to spool data by processes other than the print spooler service. Unauthorized access to the print spool directory by non-spooler processes may indicate malware attempting to read document content from spool files or an attacker harvesting document data. Access to spool directory files should be restricted to the Print Spooler service and local SYSTEM account with all other access attempts generating security audit events. Spool directory access audit events should be reviewed for access by unusual processes or user identities that do not have legitimate access needs. Security audit rules for the spool directory should be configured at the object access audit level to capture both successful and failed access attempts. Spool directory access audit data should be forwarded to SIEM for correlation with other endpoint security events to identify malicious access patterns.",
                Tags = ["print-spooler", "spool-directory", "audit-logging", "file-access", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditSpoolDirectoryAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditSpoolDirectoryAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AuditSpoolDirectoryAccess", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-restrict-spool-directory-permissions",
                Label = "Restrict File System Permissions on Print Spool Directory to Minimum Required Access",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting file system permissions on the print spool directory ensures that only the Print Spooler service account and local administrators have access to spool files preventing unauthorized reading or modification of print job data. Default Windows configurations allow the Network Service account and some user accounts to read from the spool directory which is broader access than required for normal printing operations. Tightening spool directory ACLs to SYSTEM and Print Spooler service only requires careful testing to ensure that the print spooler functionality is not broken and that legitimate access patterns are maintained. The Windows default spool directory path is %SYSTEMROOT%\\System32\\spool\\PRINTERS which should have restrictive ACLs preventing standard user access. Spool directory permission changes should be performed with care and tested thoroughly before production deployment as misconfigured permissions can prevent printing from functioning. Periodic review of spool directory permissions should verify that ACLs have not been relaxed by software installation or administrative changes.",
                Tags = ["print-spooler", "directory-permissions", "acl", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSpoolDirectoryPermissions")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSpoolDirectoryPermissions", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-block-spool-file-access-by-network",
                Label = "Block Remote Network Access to Print Spooler Spool File Directory",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Blocking remote network access to the print spool directory ensures that network shares and remote file access protocols cannot be used to read or enumerate print spool contents from remote systems without the authorization required for spooler management operations. The PrintNightmare vulnerability family demonstrated that access to the spool directory from remote network connections can be exploited for privilege escalation and remote code execution. Blocking network access to the spool directory at the file system level provides defense in depth complementing the print spooler service access controls. Network firewall rules should also block remote access to the print spooler service on port 445 from systems that are not authorized print clients or print administrators. The printer driver path within the spool directory is particularly sensitive as it can be used to load arbitrary DLLs if network access is permitted. Vulnerability assessments should specifically test for network access to the spool directory as part of print infrastructure security evaluations.",
                Tags = ["print-spooler", "network-access", "printnightmare", "remote-access", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNetworkSpoolFileAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNetworkSpoolFileAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNetworkSpoolFileAccess", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-enable-spool-service-hardening",
                Label = "Enable Additional Security Hardening for Print Spooler Service Operation",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Print spooler service hardening applies additional security restrictions to the spooler process including restricting which DLLs can be loaded controlling network communication capabilities and applying attack surface reduction rules specifically targeting the print spooler attack surface. The print spooler service has historically been a common target for privilege escalation exploit chains and running the spooler with hardened configuration significantly reduces the effectiveness of known exploit techniques. Spooler hardening includes disabling the ability for the Print Spooler to accept remote connections when the system is not intended to serve as a print server which eliminates the network attack surface. Applications on workstations that do not require serving print jobs to other computers should run the print spooler in local-only mode to prevent remote exploitation. Print server configurations that require the remote print spooler functionality should apply spooler hardening in ways that are compatible with the remote printing use case. Microsoft security updates for the print spooler should be applied promptly due to the elevated risk associated with known spooler vulnerabilities.",
                Tags = ["print-spooler", "service-hardening", "attack-surface", "exploit-mitigation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolServiceHardening", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolServiceHardening")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolServiceHardening", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-configure-spool-file-encryption",
                Label = "Configure Encryption for Print Spool Files on Disk at Rest",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Configuring encryption for print spool files on disk ensures that document content written to the spool directory during print operations is protected against unauthorized access by processes that can access the file system but are not authorized to access print data. Spool file encryption can be implemented through EFS Encrypting File System applied to the spool directory or through volume-level BitLocker encryption that covers the system drive where the spool directory resides. EFS applied specifically to the spool directory provides per-file encryption with the Print Spooler service as the authorized accessor while BitLocker provides volume-level protection relevant to physical media attacks. Organizations processing highly sensitive documents should evaluate spool file encryption as a control that complements access control restrictions on the spool directory. Encryption key management for spool file encryption should integrate with organizational key management practices to ensure keys are recoverable in the event of system failure. Performance testing should validate that spool file encryption does not introduce unacceptable latency in the print workflow for high-volume print environments.",
                Tags = ["print-spooler", "spool-encryption", "data-at-rest", "efs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSpoolFileEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSpoolFileEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSpoolFileEncryption", 1)],
            },
            new TweakDef
            {
                Id = "splfinal-disable-persistently-cached-print-jobs",
                Label = "Disable Persistent Caching of Print Jobs in Print Spool for Offline Recovery",
                Category = "Maintenance",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disabling persistent caching of print jobs prevents the print spooler from retaining print job data across system restarts for the purpose of re-submitting jobs that were queued when a printer was offline. Persistent print job caching means that document content can remain in the spool for extended periods including across security-relevant system events such as user logoff or system hibernation. Users who submit print jobs intending them to be printed will have a poor experience if persistent caching is disabled when the target printer is unavailable but the security benefit justifies the workflow impact in high-security environments. Organizations with strict data handling requirements for sensitive document categories should disable persistent print job caching to ensure document data does not accumulate in the spool across operational sessions. Alternative print management approaches including print management software that provides controlled job resubmission with appropriate authentication can address legitimate offline printing requirements. User communication about the impact of disabling persistent print caching should be provided before the policy is deployed.",
                Tags = ["print-spooler", "persistent-cache", "data-minimization", "offline-printing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersistentlyCachedPrintJobs")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersistentlyCachedPrintJobs", 1)],
            },
        ];
    }

    // ── PrintTicketPolicy ──
    private static class _PrintTicketPolicy
    {
        private const string TktKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\PrintTicket";

        private const string PrtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "prttkt-enable-print-ticket-validation",
                    Label = "Print Ticket: Enable Print Ticket Schema Validation",
                    Category = "Maintenance",
                    Description =
                        "Sets ValidatePrintTickets=1 in PrintTicket policy. Enables XML schema validation of Print Tickets before they are processed by the print driver. A Print Ticket is an XML document that describes the desired print job settings (paper size, colour mode, duplex, media type). Malformed or crafted Print Tickets with invalid XML — including oversized attribute values or deeply nested structures — can trigger XML parser vulnerabilities in GDI/XPS rendering code. Enabling validation rejects malformed Print Tickets before they reach the vulnerable parsing code, reducing the attack surface for print-job-based exploits.",
                    Tags = ["print-ticket", "xml", "validation", "security", "schema"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print Tickets are schema-validated before processing. Malformed or non-compliant Print Tickets cause the job to fail with an error. Well-formed Print Tickets from standard Windows print dialogs and Microsoft applications are always valid.",
                    ApplyOps = [RegOp.SetDword(TktKey, "ValidatePrintTickets", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "ValidatePrintTickets")],
                    DetectOps = [RegOp.CheckDword(TktKey, "ValidatePrintTickets", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-disable-xps-rendering-sandbox-bypass",
                    Label = "Print Ticket: Disable XPS Rendering Sandbox Bypass",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableXpsRenderingBypass=1 in PrintTicket policy. Prevents applications from bypassing the XPS rendering pipeline sandbox. When a print job is sent as XPS data (from an application using the XPS Document Interface), the rendering is performed in a sandboxed low-privilege process. Some applications or malicious payloads can attempt to invoke a direct rendering path that bypasses the sandbox — processing XPS content with the full privilege of the calling process. Enabling this setting forces all XPS rendering through the sandboxed pipeline regardless of the caller's request.",
                    Tags = ["print-ticket", "xps", "sandbox", "security", "rendering"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "XPS print jobs cannot bypass the rendering sandbox. All XPS content is processed in the isolated XPS rendering host. No user-visible impact for standard printing. Prevents privilege escalation via malicious XPS payloads in print jobs.",
                    ApplyOps = [RegOp.SetDword(TktKey, "DisableXpsRenderingBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "DisableXpsRenderingBypass")],
                    DetectOps = [RegOp.CheckDword(TktKey, "DisableXpsRenderingBypass", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-restrict-print-ticket-namespace",
                    Label = "Print Ticket: Restrict Print Ticket XML Namespaces to Approved List",
                    Category = "Maintenance",
                    Description =
                        "Sets RestrictCustomNamespaces=1 in PrintTicket policy. Restricts Print Ticket XML namespaces to the standard Print Schema namespace plus explicitly approved vendor extensions. Print Tickets support vendor-defined custom XML namespaces for proprietary printer features. A maliciously crafted Print Ticket can include a large number of custom namespace declarations, causing the XML parser to resolve namespaces recursively (XML namespace expansion attack) or consume excessive memory. Restricting namespaces to known-good ones eliminates this attack vector.",
                    Tags = ["print-ticket", "xml-namespace", "security", "print-schema", "restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print Tickets with unapproved custom XML namespaces are rejected. Standard Print Schema namespaces (Microsoft) are always approved. Printers using proprietary extensions with unlisted namespaces may have those extensions ignored.",
                    ApplyOps = [RegOp.SetDword(TktKey, "RestrictCustomNamespaces", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "RestrictCustomNamespaces")],
                    DetectOps = [RegOp.CheckDword(TktKey, "RestrictCustomNamespaces", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-enable-wsd-printer-discovery-logging",
                    Label = "Print Ticket: Enable WSD Printer Discovery Logging",
                    Category = "Maintenance",
                    Description =
                        "Sets WsdDiscoveryLogging=1 in PrintTicket policy. Enables logging of Web Services on Devices (WSD) printer discovery events. WSD is the network printer discovery protocol used by Windows to automatically find and install network printers. WSD discovery responses are XML documents parsed by the Windows printer subsystem. Logging WSD discovery events provides visibility into which printers the system detected, which printers were installed automatically, and whether any unexpected WSD responses were received — useful for detecting rogue printer injection attacks where an attacker's device responds to WSD probes with a malicious printer description.",
                    Tags = ["wsd", "printer-discovery", "logging", "network", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WSD printer discovery events are logged. Provides audit trail of automatic printer installations. Useful for detecting rogue printer injection in environments with WSD-enabled networks. Minor event log volume in stable environments.",
                    ApplyOps = [RegOp.SetDword(TktKey, "WsdDiscoveryLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "WsdDiscoveryLogging")],
                    DetectOps = [RegOp.CheckDword(TktKey, "WsdDiscoveryLogging", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-disable-auto-wsd-install",
                    Label = "Print Ticket: Disable Automatic WSD Printer Installation",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableAutoWsdInstall=1 in Printers policy. Prevents Windows from automatically installing WSD-discovered printers without user confirmation or administrator intervention. WSD auto-install reads the printer's XML device description and installs a print driver automatically. An attacker on the local network can broadcast crafted WSD printer advertisements causing Windows to auto-install drivers from rogue printers — if the driver installation triggers a code execution vector (custom driver DLL), the auto-install path is exploitable without any user interaction. Disabling auto-install prevents unsolicited printer additions.",
                    Tags = ["wsd", "auto-install", "printer", "security", "rogue-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "WSD printers on the network are not automatically installed. Users or administrators must manually add WSD printers via 'Add a printer'. Prevents rogue WSD printer injection. All new printer additions become explicit IT-authorised actions.",
                    ApplyOps = [RegOp.SetDword(PrtKey, "DisableAutoWsdInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrtKey, "DisableAutoWsdInstall")],
                    DetectOps = [RegOp.CheckDword(PrtKey, "DisableAutoWsdInstall", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-set-max-print-ticket-size-64kb",
                    Label = "Print Ticket: Set Maximum Print Ticket XML Size to 64 KB",
                    Category = "Maintenance",
                    Description =
                        "Sets MaxPrintTicketSize=65536 in PrintTicket policy (bytes). Sets the maximum allowed size for a Print Ticket XML document to 64 KB. A legitimate Print Ticket for a printer with comprehensive feature support (media handling, finishing, stapling options, colour profiles) is typically 5-15 KB. There is no legitimate reason for a Print Ticket to be larger. Oversized Print Tickets that exceed the limit are rejected before being passed to the XML parser — preventing XML bomb attacks (exponential entity expansion) or other size-based parser exploits that would attempt to process megabytes of XML through a kernel component.",
                    Tags = ["print-ticket", "xml-size", "dos", "security", "parser"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print Tickets larger than 64 KB are rejected. All standard Windows print dialogs generate Print Tickets well under 64 KB. Only custom or malformed Print Tickets would exceed this size. No impact on normal printing.",
                    ApplyOps = [RegOp.SetDword(TktKey, "MaxPrintTicketSize", 65536)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "MaxPrintTicketSize")],
                    DetectOps = [RegOp.CheckDword(TktKey, "MaxPrintTicketSize", 65536)],
                },
                new TweakDef
                {
                    Id = "prttkt-enable-capability-schema-enforcement",
                    Label = "Print Ticket: Enforce PrintCapabilities Schema on Driver Provider",
                    Category = "Maintenance",
                    Description =
                        "Sets EnforceCapabilitySchema=1 in PrintTicket policy. Requires print drivers to provide a schema-conformant PrintCapabilities document when queried by the print subsystem. PrintCapabilities is the XML document that describes what a printer can do (available media types, print qualities, finishing options). Some legacy drivers return malformed or empty PrintCapabilities responses causing the Windows XPS/Print Schema layer to fall back to guessed defaults or crash. Enforcing schema compliance causes drivers returning invalid PrintCapabilities to produce a validation error rather than passing corrupt XML further into the stack.",
                    Tags = ["print-ticket", "print-capabilities", "driver", "schema", "validation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Drivers providing non-schema-conformant PrintCapabilities generate an error. Some legacy printer drivers (pre-Vista v3 drivers) may fail this check. The printer appears in print dialogs with limited options rather than crashing. Only affects non-conformant legacy drivers.",
                    ApplyOps = [RegOp.SetDword(TktKey, "EnforceCapabilitySchema", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "EnforceCapabilitySchema")],
                    DetectOps = [RegOp.CheckDword(TktKey, "EnforceCapabilitySchema", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-disable-network-scan-to-print",
                    Label = "Print Ticket: Disable Network Scan-to-Print Direct Integration",
                    Category = "Maintenance",
                    Description =
                        "Sets DisableScanToPrint=1 in PrintTicket policy. Disables the Windows Scan-to-Print direct integration feature that allows WSD-enabled multi-function printers to push scanned documents directly into the Windows print queue for automatic printing. Direct scan-to-print integration accepts document data from network devices without user-initiated authentication. An attacker with access to the local network who can simulate a WSD scanner can push arbitrary document data into the print pipeline by impersonating a scannner. Disabling this feature requires users to initiate scan operations from Windows Fax and Scan or third-party software.",
                    Tags = ["scan-to-print", "wsd", "network-scanner", "security", "injection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Scan-to-Print direct integration is disabled. Scanned documents are not pushed directly to the printer from the scanner. Users must initiate scanning via Windows Fax and Scan or the scanner's software. Prevents unauthorized network device injection of print data.",
                    ApplyOps = [RegOp.SetDword(TktKey, "DisableScanToPrint", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "DisableScanToPrint")],
                    DetectOps = [RegOp.CheckDword(TktKey, "DisableScanToPrint", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-allow-only-v4-xps-print-path",
                    Label = "Print Ticket: Allow Only V4 XPS Print Path for Network Printers",
                    Category = "Maintenance",
                    Description =
                        "Sets EnforceXpsPrintPath=1 in PrintTicket policy. Restricts network printer connections to the v4 XPS print path exclusively. The v4 XPS print path processes all print jobs through the GDI-to-XPS conversion path, running in an isolated XPS rendering host. The legacy v3 GDI direct print path processes documents in the context of the calling application or SYSTEM — a code execution vulnerability in the rendering path is much higher privilege. Enforcing the XPS path for network printers ensures malicious print data processed from network sources is contained in the lower-privilege XPS host.",
                    Tags = ["print-ticket", "v4-driver", "xps-path", "security", "isolation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Network printers must use the v4 XPS print path. Printers with only v3 legacy drivers may fail to connect via network. Most printers released after Windows 8 have v4 driver packages. Legacy specialty printers (label, receipt, industrial) may only have v3 drivers.",
                    ApplyOps = [RegOp.SetDword(TktKey, "EnforceXpsPrintPath", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "EnforceXpsPrintPath")],
                    DetectOps = [RegOp.CheckDword(TktKey, "EnforceXpsPrintPath", 1)],
                },
                new TweakDef
                {
                    Id = "prttkt-restrict-print-ticket-processing-to-users",
                    Label = "Print Ticket: Restrict Print Ticket Processing to Authorised User Sessions",
                    Category = "Maintenance",
                    Description =
                        "Sets RestrictToUserSessions=1 in PrintTicket policy. Restricts Print Ticket processing to originate only from authenticated user sessions (interactive or service sessions with a valid user token). Print Tickets submitted without an associated user session token (e.g., from an anonymous service account or through a NULL session SMB path) are rejected. This prevents attackers from submitting print jobs anonymously that would be processed with SYSTEM-level privileges in the spooler. All legitimate print submissions in enterprise environments originate from authenticated user accounts.",
                    Tags = ["print-ticket", "authentication", "session", "security", "anonymous"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Print Tickets without an authenticated user session token are rejected. Anonymous print submissions are blocked. All printing from authenticated users and services with valid user tokens is unaffected.",
                    ApplyOps = [RegOp.SetDword(TktKey, "RestrictToUserSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(TktKey, "RestrictToUserSessions")],
                    DetectOps = [RegOp.CheckDword(TktKey, "RestrictToUserSessions", 1)],
                },
            ];
    }

    // ── ProtectedPrintModePolicy ──
    private static class _ProtectedPrintModePolicy
    {
        private const string WppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint";
        private const string WppDriverKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ProtectedPrint\DriverPolicy";
        private const string PrintSpoolerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Printers\WPP";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wpp-enable-protected-print-mode",
                    Label = "Enable Windows Protected Print Mode",
                    Category = "Maintenance",
                    Description =
                        "Enables Windows Protected Print (WPP) mode, which restricts printing to only Windows-protected printer drivers that are signed and certified by Microsoft. Prevents malicious print drivers.",
                    Tags = ["wpp", "printing", "protected-print", "driver-security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "Eliminates third-party unsigned print driver attack vectors; only Microsoft-supplied IPP-class drivers are permitted.",
                    RegistryKeys = [WppKey],
                    ApplyOps = [RegOp.SetDword(WppKey, "EnableProtectedPrintMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(WppKey, "EnableProtectedPrintMode")],
                    DetectOps = [RegOp.CheckDword(WppKey, "EnableProtectedPrintMode", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-block-legacy-print-drivers",
                    Label = "Block Legacy (Non-WPP) Print Drivers",
                    Category = "Maintenance",
                    Description =
                        "Prevents Windows from loading or using non-WPP print drivers. Only drivers explicitly certified under the Windows Protected Print certification program are permitted to run.",
                    Tags = ["wpp", "printing", "driver-block", "legacy-driver", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "May prevent older printers without WPP-certified drivers from functioning. Verify printer compatibility before enabling in production.",
                    RegistryKeys = [WppKey],
                    ApplyOps = [RegOp.SetDword(WppKey, "BlockLegacyPrintDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(WppKey, "BlockLegacyPrintDrivers")],
                    DetectOps = [RegOp.CheckDword(WppKey, "BlockLegacyPrintDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-require-driver-signature",
                    Label = "Require Driver Signature Verification for Print Drivers",
                    Category = "Maintenance",
                    Description =
                        "Enforces cryptographic signature verification for all print drivers prior to loading. Drivers without a valid Microsoft-issued signature are rejected, even in a non-WPP environment.",
                    Tags = ["wpp", "printing", "driver-signing", "code-integrity", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Prevents unsigned or self-signed malicious drivers from being loaded by the print spooler service.",
                    RegistryKeys = [WppDriverKey],
                    ApplyOps = [RegOp.SetDword(WppDriverKey, "RequireSignedDrivers", 1)],
                    RemoveOps = [RegOp.DeleteValue(WppDriverKey, "RequireSignedDrivers")],
                    DetectOps = [RegOp.CheckDword(WppDriverKey, "RequireSignedDrivers", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-disable-driver-installation-from-user",
                    Label = "Prevent Users from Installing Print Drivers",
                    Category = "Maintenance",
                    Description =
                        "Restricts print driver installation to administrators only. Standard users cannot add printers with non-WPP drivers via the Windows print management UI or mapped drives.",
                    Tags = ["wpp", "printing", "driver-install", "user-restriction", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "A common attack vector involves tricking users into connecting to rogue printers that install malicious drivers; this policy blocks that path.",
                    RegistryKeys = [WppDriverKey],
                    ApplyOps = [RegOp.SetDword(WppDriverKey, "PreventUserDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(WppDriverKey, "PreventUserDriverInstall")],
                    DetectOps = [RegOp.CheckDword(WppDriverKey, "PreventUserDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-audit-driver-load-events",
                    Label = "Audit Print Driver Load Events",
                    Category = "Maintenance",
                    Description =
                        "Enables audit logging for all print driver load operations. Events include driver name, installer identity, and whether the load was permitted or denied by WPP policy.",
                    Tags = ["wpp", "printing", "driver-audit", "event-log", "compliance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Creates a forensic trail of print driver activity, enabling detection of unexpected driver installations.",
                    RegistryKeys = [WppKey],
                    ApplyOps = [RegOp.SetDword(WppKey, "AuditDriverLoadEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(WppKey, "AuditDriverLoadEvents")],
                    DetectOps = [RegOp.CheckDword(WppKey, "AuditDriverLoadEvents", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-block-raw-printing",
                    Label = "Block RAW Format Print Job Submission",
                    Category = "Maintenance",
                    Description =
                        "Prevents applications from submitting RAW-format print jobs, which bypass the Windows print rendering pipeline and can embed arbitrary data. WPP requires rendering through the IPP stack.",
                    Tags = ["wpp", "printing", "raw-print", "ipp", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "RAW print jobs can exfiltrate data to printers; IPP-rendered jobs pass through the OS pipeline which can be inspected by DLP tools.",
                    RegistryKeys = [PrintSpoolerKey],
                    ApplyOps = [RegOp.SetDword(PrintSpoolerKey, "BlockRawPrintJobs", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "BlockRawPrintJobs")],
                    DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "BlockRawPrintJobs", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-restrict-remote-print-driver-install",
                    Label = "Block Remote Print Driver Installation via RPC",
                    Category = "Maintenance",
                    Description =
                        "Prevents print drivers from being remotely installed via the Print Spooler RPC interface. Remote driver installation was exploited by PrintNightmare (CVE-2021-1675); WPP mode disables this endpoint.",
                    Tags = ["wpp", "printing", "rpc", "print-spooler", "printnightmare", "cve"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Directly mitigates PrintNightmare-class RPC exploitation. Eliminates remote driver install surface from the print spooler.",
                    RegistryKeys = [PrintSpoolerKey],
                    ApplyOps = [RegOp.SetDword(PrintSpoolerKey, "BlockRemoteDriverInstall", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "BlockRemoteDriverInstall")],
                    DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "BlockRemoteDriverInstall", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-require-ipp-protocol-only",
                    Label = "Restrict Print Communication to IPP Protocol Only",
                    Category = "Maintenance",
                    Description =
                        "Configures the Windows print stack to communicate with printers using Internet Printing Protocol (IPP) only, blocking legacy LPR and SMB-based print protocols that WPP does not support.",
                    Tags = ["wpp", "printing", "ipp", "protocol", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "Requires printers to support IPP; legacy network printers using LPR or SMB printing will not work. Test compatibility in a pilot group first.",
                    RegistryKeys = [PrintSpoolerKey],
                    ApplyOps = [RegOp.SetDword(PrintSpoolerKey, "RestrictToIPPOnly", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "RestrictToIPPOnly")],
                    DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "RestrictToIPPOnly", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-disable-printer-redirection-rdp",
                    Label = "Disable Client-Side Print Redirection in Remote Desktop",
                    Category = "Maintenance",
                    Description =
                        "Prevents local printers from being redirected and made available in Remote Desktop sessions. Eliminates the risk of untrusted WPP-non-compliant client drivers being exposed to an RDS server.",
                    Tags = ["wpp", "printing", "rdp", "print-redirection", "remote-desktop"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Users in RDP sessions cannot print to their local printers; they must use printers accessible from the server side.",
                    RegistryKeys = [PrintSpoolerKey],
                    ApplyOps = [RegOp.SetDword(PrintSpoolerKey, "DisableRdpPrinterRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "DisableRdpPrinterRedirection")],
                    DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "DisableRdpPrinterRedirection", 1)],
                },
                new TweakDef
                {
                    Id = "wpp-enable-spooler-process-isolation",
                    Label = "Enable Print Spooler Process Isolation",
                    Category = "Maintenance",
                    Description =
                        "Configures the Windows Print Spooler to run third-party print processors and drivers in isolated job-scoped processes rather than within the main spooler process. Limits the blast radius of a compromised driver.",
                    Tags = ["wpp", "printing", "process-isolation", "spooler", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 26100,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "A malicious or buggy print driver only affects its isolated process rather than the entire spooler, reducing privilege escalation risk.",
                    RegistryKeys = [PrintSpoolerKey],
                    ApplyOps = [RegOp.SetDword(PrintSpoolerKey, "EnableSpoolerProcessIsolation", 1)],
                    RemoveOps = [RegOp.DeleteValue(PrintSpoolerKey, "EnableSpoolerProcessIsolation")],
                    DetectOps = [RegOp.CheckDword(PrintSpoolerKey, "EnableSpoolerProcessIsolation", 1)],
                },
            ];
    }
}
