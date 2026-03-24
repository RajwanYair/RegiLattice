// RegiLattice.Core — Tweaks/WirelessDisplayPolicy.cs
// Wireless Display (Miracast / Wi-Fi Direct projection) GPO controls — Sprint 199.
// Controls projection-to-PC, PIN pairing, receiver input, and Miracast security settings.
// Category: "Wireless Display Policy" | Slug: wdsply
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessDisplay

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WirelessDisplayPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessDisplay";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wdsply-block-projection-to-pc",
                Label = "Block Wireless Projection To This PC",
                Category = "Wireless Display Policy",
                Description =
                    "Prevents other devices from wirelessly projecting their screen to this PC via Miracast. Eliminates the risk of screen eavesdropping or unauthorised projection in shared spaces. Default: 1 (allow). Recommended: 0 in open environments.",
                Tags = ["wireless-display", "miracast", "projection", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "No other device can project its screen here; eliminates physical screen-capture risk.",
                ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPC", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPC")],
                DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPC", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-require-pin-pairing",
                Label = "Always Require PIN for Wireless Display Pairing",
                Category = "Wireless Display Policy",
                Description =
                    "Sets PIN requirement to 'Always' (2) for Miracast pairing. Prevents unauthorised devices from connecting without a confirmed PIN exchange. Default: 0 (never). Recommended: 2 (always).",
                Tags = ["wireless-display", "pin", "pairing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All Miracast connections require PIN confirmation; prevents rogue device connections.",
                ApplyOps = [RegOp.SetDword(Key, "RequirePinForPairing", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequirePinForPairing")],
                DetectOps = [RegOp.CheckDword(Key, "RequirePinForPairing", 2)],
            },
            new TweakDef
            {
                Id = "wdsply-block-receiver-input",
                Label = "Block User Input From Wireless Display Receiver",
                Category = "Wireless Display Policy",
                Description =
                    "Prevents keyboard and mouse input from being relayed back to the PC from a wireless display receiver. Stops remote HID injection via a Miracast receiver. Default: 1 (allow). Recommended: 0.",
                Tags = ["wireless-display", "input", "hid", "receiver", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Wireless display is output-only; the receiver cannot send keystrokes or mouse events.",
                ApplyOps = [RegOp.SetDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowUserInputFromWirelessDisplayReceiver")],
                DetectOps = [RegOp.CheckDword(Key, "AllowUserInputFromWirelessDisplayReceiver", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-disable-auto-discovery",
                Label = "Disable Automatic Wireless Display Discovery",
                Category = "Wireless Display Policy",
                Description =
                    "Prevents this PC from automatically discovering and advertising itself to nearby Miracast-capable devices. Reduces exposure on shared networks or public spaces. Default: 1. Recommended: 0.",
                Tags = ["wireless-display", "discovery", "miracast", "advertisement", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Device is not visible to Miracast senders until discovery is explicitly initiated.",
                ApplyOps = [RegOp.SetDword(Key, "EnableAutoDiscovery", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAutoDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAutoDiscovery", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-block-infra-projection",
                Label = "Block Infrastructure-Mode Wireless Projection",
                Category = "Wireless Display Policy",
                Description =
                    "Disables Miracast projection via the local Wi-Fi infrastructure (access point). Limits projection to Wi-Fi Direct only, reducing network-based interception surface. Default: 1. Recommended: 0.",
                Tags = ["wireless-display", "infrastructure", "wifi", "projection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Infrastructure-mode Miracast (via Wi-Fi AP) is blocked; Wi-Fi Direct is the only projection path.",
                ApplyOps = [RegOp.SetDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowProjectionToPCOverInfrastructure")],
                DetectOps = [RegOp.CheckDword(Key, "AllowProjectionToPCOverInfrastructure", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-block-miracast-broadcast",
                Label = "Block Miracast Broadcast Advertisement",
                Category = "Wireless Display Policy",
                Description =
                    "Prevents this PC from broadcasting Miracast availability beacons. The device is invisible to P2P Miracast senders that rely on broadcast discovery. Default: 1. Recommended: 0 in secure offices.",
                Tags = ["wireless-display", "broadcast", "miracast", "discovery", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Device does not advertise Miracast availability; projection must be manually initiated on sender.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMiracastBroadcast", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMiracastBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMiracastBroadcast", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-block-ble-pairing",
                Label = "Disable Bluetooth LE Pairing for Wireless Display",
                Category = "Wireless Display Policy",
                Description =
                    "Disallows Bluetooth Low Energy (BLE) as a pairing mechanism for Miracast connections. Reduces BLE attack surface during wireless display setup. Default: 1. Recommended: 0.",
                Tags = ["wireless-display", "bluetooth", "ble", "pairing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "BLE-initiated Miracast pairing is disabled; Wi-Fi Direct PIN is the only pairing method.",
                ApplyOps = [RegOp.SetDword(Key, "AllowBleForPairing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBleForPairing")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBleForPairing", 0)],
            },
            new TweakDef
            {
                Id = "wdsply-limit-connection-count",
                Label = "Limit Simultaneous Wireless Display Connections",
                Category = "Wireless Display Policy",
                Description =
                    "Sets the maximum number of simultaneous wireless display connections to 1. Prevents resource exhaustion and limits exposure in multi-user scan environments. Default: not restricted. Recommended: 1.",
                Tags = ["wireless-display", "connection-limit", "resource", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Only one device can connect at a time; others must wait for the session to end.",
                ApplyOps = [RegOp.SetDword(Key, "MaxConnectionCount", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxConnectionCount")],
                DetectOps = [RegOp.CheckDword(Key, "MaxConnectionCount", 1)],
            },
            new TweakDef
            {
                Id = "wdsply-require-wpa2",
                Label = "Require WPA2 Encryption for Wireless Display",
                Category = "Wireless Display Policy",
                Description =
                    "Enforces WPA2 (or later) encryption for all Miracast Wi-Fi Direct connections. Prevents unencrypted or WEP-protected wireless display sessions. Default: not enforced. Recommended: 1.",
                Tags = ["wireless-display", "wpa2", "encryption", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "All Miracast sessions must use WPA2 encryption; WEP or open sessions are rejected.",
                ApplyOps = [RegOp.SetDword(Key, "RequireWPA2ForPairing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireWPA2ForPairing")],
                DetectOps = [RegOp.CheckDword(Key, "RequireWPA2ForPairing", 1)],
            },
            new TweakDef
            {
                Id = "wdsply-block-mdm-input",
                Label = "Block MDM Input Commands from Wireless Display",
                Category = "Wireless Display Policy",
                Description =
                    "Prevents an MDM management profile delivered via a wireless display receiver from sending input or configuration commands to this device. Closes an MDM-over-Miracast injection vector. Default: 1. Recommended: 0.",
                Tags = ["wireless-display", "mdm", "input", "injection", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "MDM commands cannot arrive through a wireless display receiver; eliminates that attack vector.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMDMInputFromWirelessDisplayReceiver")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMDMInputFromWirelessDisplayReceiver", 0)],
            },
        ];
}
