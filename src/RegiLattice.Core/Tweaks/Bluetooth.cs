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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowIdleIrpInD3", 0)],
        },
        new TweakDef
        {
            Id = "bt-manual-start",
            Label = "Bluetooth Service to Manual",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Bluetooth support service to manual start — saves resources on machines that rarely use Bluetooth.",
            Tags = ["bluetooth", "services", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFSrv"],
        },
        new TweakDef
        {
            Id = "bt-high-quality-audio",
            Label = "Bluetooth A2DP High-Quality Audio",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the A2DP SBC bitpool range for higher-fidelity Bluetooth audio streaming.",
            Tags = ["bluetooth", "audio", "quality"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "AllowDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "bt-low-latency",
            Label = "Bluetooth LE Low-Latency Mode",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Tightens BLE connection intervals for lower latency with peripherals.",
            Tags = ["bluetooth", "performance", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum\Parameters"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 3),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthOBEXSrv", "Start", 3),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthPan", "Start", 3),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHMODEM", "Start", 3),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-advertising",
            Label = "Disable Bluetooth LE Advertising",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth Low Energy advertising. Prevents the PC from broadcasting BLE presence. Improves privacy. Default: Enabled. Recommended: Disabled.",
            Tags = ["bluetooth", "ble", "privacy", "advertising"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Bluetooth", "AllowAdvertising", 0)],
        },
        new TweakDef
        {
            Id = "bt-audio-offload",
            Label = "Bluetooth Audio Offload",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Bluetooth A2DP sideband audio offloading to the adapter. Reduces CPU usage for Bluetooth audio streaming. Default: Disabled. Recommended: Enabled.",
            Tags = ["bluetooth", "audio", "performance", "offload"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthA2dp\Parameters", "AllowSidebandAudio", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-le-discovery",
            Label = "Disable Bluetooth LE Device Discovery",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth Low Energy device discovery. Reduces power usage and limits BLE scanning surface. Default: Enabled. Recommended: Disabled if BLE not needed.",
            Tags = ["bluetooth", "ble", "discovery", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters\Devices", "DisableLEDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "bt-disable-handsfree-telephony",
            Label = "Disable Bluetooth Handsfree Telephony",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Bluetooth Handsfree telephony profile via BTHPORT parameter. Prevents low-quality HFP audio mode. Default: Enabled. Recommended: Disabled for A2DP-only use.",
            Tags = ["bluetooth", "handsfree", "telephony", "audio"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthHFPAudio", "Start", 4)],
        },
        new TweakDef
        {
            Id = "bt-disable-auto-pair",
            Label = "Disable Bluetooth Auto-Pairing",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables automatic Bluetooth device pairing. Devices must be paired manually for better security control. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["bluetooth", "auto-pair", "security", "pairing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Bluetooth"],
        },
        new TweakDef
        {
            Id = "bt-disable-advertising",
            Label = "Disable Bluetooth Advertising (Policy)",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Bluetooth advertising via Microsoft policy. Prevents the device from broadcasting its presence. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["bluetooth", "advertising", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Bluetooth"],
        },
        new TweakDef
        {
            Id = "bt-disable-le-scan",
            Label = "Disable Bluetooth LE Scanning",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Bluetooth Low Energy background scanning. Reduces power consumption and radio interference. Default: Enabled. Recommended: Disabled for battery life.",
            Tags = ["bluetooth", "le", "scanning", "power", "battery"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthLEEnum\Parameters"],
        },
        new TweakDef
        {
            Id = "bt-page-timeout",
            Label = "Set Bluetooth Page Timeout",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets Bluetooth page timeout to 16384 slots (~10 seconds). Increases time allowed for device connection establishment. Default: 8192 slots. Recommended: 16384 for reliability.",
            Tags = ["bluetooth", "page", "timeout", "connection", "reliability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 16384),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BTHPORT\Parameters", "PageTimeout", 16384)],
        },
        new TweakDef
        {
            Id = "bt-disable-audio-gateway",
            Label = "Disable Bluetooth Audio Gateway",
            Category = "Bluetooth",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Bluetooth Audio Gateway service driver. Prevents phone call audio routing through the PC. Default: Enabled. Recommended: Disabled if unused.",
            Tags = ["bluetooth", "audio-gateway", "telephony", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BthAGAudio"],
        },
    ];
}
