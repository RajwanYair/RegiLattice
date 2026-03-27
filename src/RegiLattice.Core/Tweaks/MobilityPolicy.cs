// MobilityPolicy.cs — Windows Mobility, roaming, tethering, and carrier policy enforcement
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\Mobility
// Slug: mob
// Category: Mobility Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MobilityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Mobility";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "mob-disable-cellular-data-roaming",
            Label = "Mobility Policy: Disable Cellular Data Roaming",
            Category = "Mobility Policy",
            Description =
                "Prevents Windows from enabling cellular data roaming, which connects to and uses foreign carrier networks at potentially extreme per-MB charges. "
                + "On enterprise-managed endpoints with cellular adapters, roaming data costs can accumulate without user awareness. "
                + "Disabling via policy overrides any SIM-level or carrier profile allowing roaming. "
                + "Removing this policy reverts cellular data roaming to device/SIM defaults.",
            Tags = ["mobility", "cellular", "roaming", "cost", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCellularDataRoaming", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularDataRoaming")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCellularDataRoaming", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Blocks cellular roaming; prevents unexpected international data charges on managed endpoints.",
        },
        new TweakDef
        {
            Id = "mob-disable-mobile-hotspot",
            Label = "Mobility Policy: Disable Mobile Hotspot Sharing",
            Category = "Mobility Policy",
            Description =
                "Prevents the device from being configured as a mobile hotspot that shares its cellular or Wi-Fi connection with other devices. "
                + "Mobile hotspot sharing bypasses network access controls and can expose the corporate network to unauthorised connected devices. "
                + "Removing this policy allows mobile hotspot sharing to be configured.",
            Tags = ["mobility", "hotspot", "tethering", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMobileHotspot", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileHotspot")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMobileHotspot", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks mobile hotspot; prevents unauthorised network sharing from managed endpoints.",
        },
        new TweakDef
        {
            Id = "mob-disable-usb-tethering",
            Label = "Mobility Policy: Disable USB Tethering",
            Category = "Mobility Policy",
            Description =
                "Prevents the device from being used as a USB tethering gateway, sharing its internet connection via USB to other devices. "
                + "USB tethering creates a NAT bridge that can leak network traffic around firewall controls. "
                + "Removing this policy allows USB tethering configuration.",
            Tags = ["mobility", "usb", "tethering", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableUsbTethering", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableUsbTethering")],
            DetectOps = [RegOp.CheckDword(Key, "DisableUsbTethering", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks USB tethering; prevents NAT bridge that could route around network firewalls.",
        },
        new TweakDef
        {
            Id = "mob-disable-automatic-connection-switch",
            Label = "Mobility Policy: Disable Auto WiFi-to-Cellular Switch",
            Category = "Mobility Policy",
            Description =
                "Prevents Windows from automatically switching the active network connection from Wi-Fi to cellular when Wi-Fi signal drops. "
                + "Automatic switching can result in cellular data consumption and unexpected data charges on limited data plans. "
                + "On enterprise machines the network handover should be manual or policy-driven. "
                + "Removing this policy re-enables automatic Wi-Fi to cellular failover.",
            Tags = ["mobility", "wifi", "cellular", "auto-switch", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoConnectionSwitch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConnectionSwitch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoConnectionSwitch", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents auto Wi-Fi→cellular failover; avoids unintended cellular data consumption.",
        },
        new TweakDef
        {
            Id = "mob-disable-bluetooth-tethering",
            Label = "Mobility Policy: Disable Bluetooth Tethering",
            Category = "Mobility Policy",
            Description =
                "Prevents the device from sharing its internet connection via Bluetooth DUN (Dial-Up Networking) to devices paired over Bluetooth. "
                + "Bluetooth tethering is a lower-visibility bridging path that can expose sensitive traffic without user awareness. "
                + "Removing this policy allows Bluetooth tethering to be configured.",
            Tags = ["mobility", "bluetooth", "tethering", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBluetoothTethering", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBluetoothTethering")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBluetoothTethering", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks Bluetooth internet sharing; closes low-visibility network bridging path.",
        },
        new TweakDef
        {
            Id = "mob-disable-data-sense",
            Label = "Mobility Policy: Disable Data Sense Usage Monitoring",
            Category = "Mobility Policy",
            Description =
                "Disables the Data Sense feature that monitors per-app cellular usage and restricts background data on metered connections. "
                + "On managed endpoints, data usage enforcement should come from network policy rather than per-device Data Sense heuristics. "
                + "Removing this policy re-enables Data Sense monitoring and throttling.",
            Tags = ["mobility", "data-sense", "metered", "monitoring", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDataSense", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSense")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDataSense", 1)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Turns off Data Sense heuristics; network policy controls take over data management.",
        },
        new TweakDef
        {
            Id = "mob-disable-carrier-provisioning",
            Label = "Mobility Policy: Disable Carrier Provisioning Updates",
            Category = "Mobility Policy",
            Description =
                "Prevents mobile carriers from remotely pushing provisioning XML updates to the device that can change network settings, APN configurations, and restrictions. "
                + "Carrier provisioning is an automated out-of-band configuration channel that can override IT-managed network settings. "
                + "Removing this policy allows carriers to provision the device with their default network profiles.",
            Tags = ["mobility", "carrier", "provisioning", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCarrierProvisioning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCarrierProvisioning")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCarrierProvisioning", 1)],
            ImpactScore = 3,
            SafetyRating = 3,
            ImpactNote = "Blocks OTA carrier provisioning; prevents carriers from overriding IT network settings. May break initial cellular setup.",
        },
        new TweakDef
        {
            Id = "mob-disable-wifi-sense",
            Label = "Mobility Policy: Disable Wi-Fi Sense Auto-Connect",
            Category = "Mobility Policy",
            Description =
                "Disables Wi-Fi Sense, which automatically connects to crowdsourced open Wi-Fi hotspots and can share Wi-Fi credentials with contacts. "
                + "Wi-Fi Sense credential-sharing is a privacy and security risk on enterprise networks — credentials can be propagated to users' personal device contacts. "
                + "Removing this policy re-enables Wi-Fi Sense auto-connect behaviour.",
            Tags = ["mobility", "wifi-sense", "credentials", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWifiSense", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiSense")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWifiSense", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables Wi-Fi Sense; prevents automatic hotspot connection and Wi-Fi credential sharing.",
        },
        new TweakDef
        {
            Id = "mob-disable-network-roaming-policy",
            Label = "Mobility Policy: Disable Network Roaming Profiles Sync",
            Category = "Mobility Policy",
            Description =
                "Prevents user roaming profiles from synchronising over cellular connections when roaming on a foreign network. "
                + "Syncing large roaming profiles over cellular roaming can incur significant data charges and slow the logon process. "
                + "Removing this policy allows roaming profile sync over any active connection.",
            Tags = ["mobility", "roaming-profile", "cellular", "logon", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRoamingProfileSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRoamingProfileSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRoamingProfileSync", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Blocks profile sync over cellular roaming; avoids data charges during international travel.",
        },
        new TweakDef
        {
            Id = "mob-disable-wwan-ui",
            Label = "Mobility Policy: Disable WWAN Cellular UI Controls",
            Category = "Mobility Policy",
            Description =
                "Hides the cellular (WWAN) control panel and settings UI from users, preventing manual changes to cellular configuration on managed endpoints. "
                + "On enterprise endpoints where cellular settings are managed via MDM or IT policy, user-facing cellular UI is redundant and can lead to misconfiguration. "
                + "Removing this policy restores the WWAN settings UI.",
            Tags = ["mobility", "wwan", "ui", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWwanUI", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWwanUI")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWwanUI", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hides WWAN settings UI; prevents users from manually reconfiguring managed cellular connections.",
        },
    ];
}
