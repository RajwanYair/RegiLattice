namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
    ];
}
