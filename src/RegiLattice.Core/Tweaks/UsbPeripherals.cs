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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
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
            Category = "USB & Peripherals",
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
            Category = "USB & Peripherals",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices\{WPD Devices}", "Deny_Read", 1),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemovableStorageDevices", "Deny_All", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-autorun",
            Label = "Disable AutoRun for All Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disable AutoRun.inf processing for all drive types. Prevents malware auto-execution. Default: enabled. Recommended: disabled.",
            Tags = ["autorun", "security", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\USB", "DisableLegacyWarning", 0)],
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
            Description =
                "Disables the USB mass storage driver. Blocks access to USB flash drives and external disks. Security hardening measure. Default: enabled.",
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
            Description =
                "Disables power saving on USB hub controllers. Prevents USB devices from disconnecting due to power management. Default: enabled.",
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
            Id = "usb-disable-removable-write",
            Label = "Disable Removable Storage Write",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks write access to removable storage devices. Prevents data exfiltration via USB drives while allowing read access. Default: allowed.",
            Tags = ["usb", "removable", "write", "security"],
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
            Id = "usb-disable-selective-suspend",
            Label = "Disable USB Selective Suspend",
            Category = "USB & Peripherals",
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
            Category = "USB & Peripherals",
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
            Id = "usb-force-safe-removal",
            Label = "Force USB Safe Removal Default",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets all removable USB storage to 'Better performance' mode requiring safe removal. Enables write caching on USB drives. Default: quick removal.",
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
        new TweakDef
        {
            Id = "usb-block-mass-storage",
            Label = "Disable USB Mass Storage",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables USB mass storage driver to prevent data exfiltration via USB drives. Default: enabled (Start=3).",
            Tags = ["usb", "mass-storage", "security", "data-loss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USBSTOR", "Start", 4)],
        },
        new TweakDef
        {
            Id = "usb-readonly-removable-storage",
            Label = "Set Removable Storage to Read-Only",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Makes all removable storage devices read-only, preventing writes to USB drives. Default: read-write.",
            Tags = ["usb", "removable", "read-only", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\StorageDevicePolicies", "WriteProtect", 1)],
        },
        new TweakDef
        {
            Id = "usb-disable-autoplay-all-drives",
            Label = "Disable AutoPlay for All Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = false,
            Description = "Disables AutoPlay for all drive types (USB, CD/DVD, network). Default: enabled.",
            Tags = ["usb", "autoplay", "security", "cd"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
        },
        new TweakDef
        {
            Id = "usb-disable-autorun-all-policy",
            Label = "Disable AutoRun for All Drives",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables AutoRun via Group Policy, preventing automatic execution of autorun.inf on any drive. Default: enabled.",
            Tags = ["usb", "autorun", "security", "malware"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoDriveTypeAutoRun", 255),
            ],
        },
        new TweakDef
        {
            Id = "usb-disable-selective-suspend-global",
            Label = "Disable USB Selective Suspend",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            Description =
                "Disables USB selective suspend policy to prevent devices from being powered down. Fixes USB disconnect issues. Default: enabled.",
            Tags = ["usb", "selective-suspend", "power", "disconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB\DisableSelectiveSuspend"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\USB", "DisableSelectiveSuspend", 1)],
        },
        new TweakDef
        {
            Id = "usb-increase-transfer-size",
            Label = "Increase USB Transfer Size to 64K",
            Category = "USB & Peripherals",
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
            Category = "USB & Peripherals",
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
            Category = "USB & Peripherals",
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
            Id = "usb-disable-remote-desktop-autoplay",
            Label = "Disable AutoPlay on Remote Desktop",
            Category = "USB & Peripherals",
            NeedsAdmin = true,
            Description = "Disables AutoPlay on remote desktop redirected drives. Prevents malicious autorun over RDP. Default: enabled.",
            Tags = ["usb", "autoplay", "rdp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "NoAutoplayfornonVolume", 1)],
        },
        new TweakDef
        {
            Id = "usb-enable-safe-removal-icon",
            Label = "Always Show Safe Removal Icon",
            Category = "USB & Peripherals",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Id = "bt-disable-handsfree-telephony",
            Label = "Disable Bluetooth Handsfree Telephony",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Handsfree telephony profile via BTHPORT parameter. Prevents low-quality HFP audio mode. Default: Enabled. Recommended: Disabled for A2DP-only use.",
            Tags = ["bluetooth", "handsfree", "telephony", "audio"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-page-timeout",
            Label = "Set Bluetooth Page Timeout",
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Id = "bt-high-quality-audio",
            Label = "Force Bluetooth High Quality Audio",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Bluetooth audio to use A2DP high-quality profile instead of hands-free. Improves audio quality at the expense of microphone. Default: auto-select.",
            Tags = ["bluetooth", "audio", "quality", "a2dp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1),
            ],
        },
        new TweakDef
        {
            Id = "bt-low-latency",
            Label = "Enable Bluetooth Low Latency Mode",
            Category = "Bluetooth",
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
        new TweakDef
        {
            Id = "bt-manual-start",
            Label = "Set Bluetooth Service to Manual Start",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Bluetooth Support Service to manual start. Bluetooth starts only when needed instead of running always. Default: automatic.",
            Tags = ["bluetooth", "service", "manual", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\bthserv", "Start", 3)],
        },
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "bt-disable-bt-audio-router",
            Label = "Disable Bluetooth Audio Gateway Router",
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Id = "bt-disable-bt-bnep",
            Label = "Disable Bluetooth Network Protocol (BNEP)",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth BNEP driver used for PAN networking. Not needed when Bluetooth tethering is unused.",
            Tags = ["bluetooth", "bnep", "networking", "tethering"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-set-inquiry-timeout",
            Label = "Reduce Bluetooth Inquiry Timeout",
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Id = "bt-disable-bt-le-proximity",
            Label = "Disable Bluetooth LE Proximity Detection",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth Low Energy proximity detection events. Reduces background radio activity and improves battery life.",
            Tags = ["bluetooth", "le", "proximity", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-set-selective-suspend",
            Label = "Enable Bluetooth Selective Suspend",
            Category = "Bluetooth",
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
            Id = "bt-disable-absolute-volume",
            Label = "Disable Bluetooth Absolute Volume",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables absolute volume control over Bluetooth. Prevents device from controlling system volume directly. Default: enabled.",
            Tags = ["bluetooth", "audio", "volume"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1),
            ],
        },
        new TweakDef
        {
            Id = "bt-disable-le-advertising-policy",
            Label = "Disable Bluetooth LE Advertising (Policy)",
            Category = "Bluetooth",
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
            Id = "bt-disable-personal-area-network",
            Label = "Disable Bluetooth Personal Area Network",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Bluetooth PAN (Personal Area Network) service. Prevents internet sharing over Bluetooth. Default: enabled.",
            Tags = ["bluetooth", "pan", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-avrcp-metadata",
            Label = "Disable Bluetooth AVRCP Metadata",
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Category = "Bluetooth",
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
            Id = "bt-disable-a2dp-service",
            Label = "Disable Bluetooth A2DP Audio Service",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Advanced Audio Distribution Profile (A2DP) service. Prevents high-quality stereo streaming. Use only if Bluetooth audio is never needed. Default: enabled.",
            Tags = ["bluetooth", "audio", "a2dp", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-avrcp-service",
            Label = "Disable Bluetooth AVRCP Remote Control Service",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Audio/Video Remote Control Profile (AVRCP) target service. Removes ability for BT devices to control playback. Default: enabled.",
            Tags = ["bluetooth", "avrcp", "remote-control", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAvrcpTg", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-audio-gateway-svc",
            Label = "Disable Bluetooth HFP Audio Gateway Service",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth Telephony Audio Gateway (BTAGService) used for headset microphone routing. Prevents BT headsets acting as call devices. Default: enabled.",
            Tags = ["bluetooth", "audio", "hfp", "gateway", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTAGService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-pan-service",
            Label = "Disable Bluetooth Personal Area Network Service",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Bluetooth PAN (Personal Area Network/tethering) service. Prevents using a phone or device as a BT network gateway. Default: enabled.",
            Tags = ["bluetooth", "pan", "network", "tethering", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-set-inquiry-length-reduced",
            Label = "Reduce Bluetooth Inquiry Scan Duration",
            Category = "Bluetooth",
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
            Id = "bt-set-page-timeout-reduced",
            Label = "Reduce Bluetooth Page Timeout",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Reduces the Bluetooth page timeout from the default 5000ms to 2000ms. Fails BT connection attempts faster, reducing hang time when connecting to an unavailable device. Default: 5000ms.",
            Tags = ["bluetooth", "connection", "performance", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 2000)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 5000)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 2000)],
        },
        new TweakDef
        {
            Id = "bt-disable-rfcomm-service",
            Label = "Disable Bluetooth RFCOMM Service",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Bluetooth RFCOMM (serial port emulation) service. Removes legacy Bluetooth COM port functionality. Required for some BT printers/GPS devices. Default: enabled.",
            Tags = ["bluetooth", "rfcomm", "serial", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\RFCOMM", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-bluetooth-radio-policy",
            Label = "Disable Bluetooth Radio via Policy",
            Category = "Bluetooth",
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
        new TweakDef
        {
            Id = "bt-disable-bt-user-service-autostart",
            Label = "Set Bluetooth User Service to Manual Start",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Bluetooth User Service to manual start instead of automatic. Reduces startup overhead on systems where Bluetooth is rarely used. Default: automatic.",
            Tags = ["bluetooth", "services", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BluetoothUserService", "Start", 3)],
        },
    ];
}
