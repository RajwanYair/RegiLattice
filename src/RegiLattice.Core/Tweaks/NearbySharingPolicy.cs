// RegiLattice.Core — Tweaks/NearbySharingPolicy.cs
// Nearby Sharing & Cross-Device Platform Group Policy — Sprint 194.
// Controls Nearby Sharing, Phone Link, cross-device clipboard, message
// sync, Bluetooth sharing, and connected-device platform features via GPO.
// Category: "Nearby Sharing Policy" | Slug: nshpol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\NearbySharing

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NearbySharingPolicy
{
    private const string NearbyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NearbySharing";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nshpol-disable-nearby-sharing",
                Label = "Disable Nearby Sharing (GPO)",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets DisableNearbySharing=1 to disable the Windows Nearby Sharing feature via Group Policy. Prevents file and URL transfers between nearby devices over Bluetooth and Wi-Fi Direct.",
                Tags = ["nearby", "sharing", "bluetooth", "wifi-direct", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Nearby Sharing from Action Center and context menus; local file moves unaffected.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "DisableNearbySharing", 1)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisableNearbySharing")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "DisableNearbySharing", 1)],
            },
            new TweakDef
            {
                Id = "nshpol-block-paired-devices",
                Label = "Block Paired Device File Sharing",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowPairedDevices=0. Prevents this device from sharing files or URLs with paired Bluetooth devices through the Nearby Sharing subsystem, even when the feature itself is enabled.",
                Tags = ["nearby", "paired", "bluetooth", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks file transfers to paired Bluetooth devices; standard Bluetooth audio unaffected.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowPairedDevices", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowPairedDevices")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowPairedDevices", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-disable-message-sync",
                Label = "Disable Phone Link Message Sync",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowMessageSync=0 in the Nearby Sharing policy. Prevents SMS and MMS messages from being synced from a paired Android device to this PC through the Phone Link (formerly Your Phone) application.",
                Tags = ["nearby", "phone", "message", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks phone SMS sync in Phone Link; call notifications and other features may still work.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowMessageSync", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowMessageSync")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowMessageSync", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-block-contacts-sync",
                Label = "Block Phone Link Contacts Sync",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowContactsSync=0. Prevents the Phone Link app from synchronising device contacts from a paired phone to the Windows People app or contact suggestions across the OS.",
                Tags = ["nearby", "phone", "contacts", "sync", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks contact sync from paired phone; standalone Windows contacts unaffected.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowContactsSync", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowContactsSync")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowContactsSync", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-disable-phone-link",
                Label = "Disable Phone Link Feature (GPO)",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets DisablePhoneLinkFromSettings=1. Removes the Phone Link pairing option from the Windows Settings app, preventing users from linking a mobile phone to this PC through the connected-devices platform.",
                Tags = ["nearby", "phone-link", "pairing", "policy", "settings"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Phone Link from Settings; existing phone pairings may persist until removed manually.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisablePhoneLinkFromSettings")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
            },
            new TweakDef
            {
                Id = "nshpol-restrict-to-my-devices",
                Label = "Restrict Nearby Sharing to My Devices Only",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets SharingScope=1 to restrict Nearby Sharing to devices signed in with the same Microsoft or Azure AD account. Prevents sharing with unknown nearby devices while preserving same-account file transfers.",
                Tags = ["nearby", "scope", "trusted", "devices", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Limits sharing to own devices only; removes 'Everyone nearby' option from sharing scope.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "SharingScope", 1)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "SharingScope")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "SharingScope", 1)],
            },
            new TweakDef
            {
                Id = "nshpol-block-bluetooth-sharing",
                Label = "Block Bluetooth File Sharing",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowBluetoothSharing=0 in the Nearby Sharing policy. Blocks the Bluetooth Object Push Profile (OPP) used by Nearby Sharing, preventing file reception from any Bluetooth source.",
                Tags = ["nearby", "bluetooth", "opp", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Bluetooth file receipt via OPP; Bluetooth audio and peripherals unaffected.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowBluetoothSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowBluetoothSharing")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowBluetoothSharing", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-block-wifi-direct-sharing",
                Label = "Block Wi-Fi Direct Nearby Sharing",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowWifiDirectNearbySharing=0. Disables the Wi-Fi Direct transport layer used for Nearby Sharing. Prevents high-speed peer-to-peer file transfers that bypass the corporate network.",
                Tags = ["nearby", "wifi-direct", "p2p", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks Wi-Fi Direct used for large file transfers in Nearby Sharing.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowWifiDirectNearbySharing")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-disable-activity-feed-sharing",
                Label = "Disable Nearby Activity Feed Sharing",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowActivityFeed=0 in the Nearby Sharing policy scope. Prevents the connected-devices platform from broadcasting recently-used documents and activities to nearby enrolled devices via the activity feed.",
                Tags = ["nearby", "activity", "feed", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Stops nearby activity broadcasting; Timeline/Activity History within this PC unchanged.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowActivityFeed", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowActivityFeed")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowActivityFeed", 0)],
            },
            new TweakDef
            {
                Id = "nshpol-block-cross-device-clipboard",
                Label = "Block Cross-Device Clipboard via Nearby Sharing",
                Category = "Nearby Sharing Policy",
                Description =
                    "Sets AllowCrossDeviceClipboard=0 in the Nearby Sharing policy. Prevents clipboard content from being sent or received between nearby Windows devices through the connected-devices platform, blocking near-field clipboard data exfiltration.",
                Tags = ["nearby", "clipboard", "cross-device", "sharing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks near-field clipboard transfers; cloud clipboard sync is a separate setting.",
                ApplyOps = [RegOp.SetDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
                RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowCrossDeviceClipboard")],
                DetectOps = [RegOp.CheckDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
            },
        ];
}
