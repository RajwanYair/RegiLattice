// RegiLattice.Core — Tweaks/NfcPolicy.cs
// Near Field Communication (NFC) machine-scope GPO controls — Sprint 213.
// Controls NFC radio enable/disable, tap-to-pay, tap-to-connect, and NFC proximity.
// Category: "NFC Policy" | Slug: nfcpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NFC

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NfcPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NFC";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nfcpol-disable-nfc-radio",
                Label = "Disable NFC Radio",
                Category = "NFC Policy",
                Description =
                    "Disables the NFC radio on devices that have one. Prevents unauthorised tap-to-transfer, tap-to-pay, and proximity-based pairing without physical control removal. Default: NFC enabled if hardware present. Recommended: 1 on corporate laptops without approved NFC use cases.",
                Tags = ["nfc", "radio", "wireless", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NFC radio is turned off; tap-to-pay, tap-to-connect, and NFC tag reading are all disabled.",
                ApplyOps = [RegOp.SetDword(Key, "AllowNFC", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowNFC")],
                DetectOps = [RegOp.CheckDword(Key, "AllowNFC", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-tap-to-pay",
                Label = "Disable NFC Tap-to-Pay",
                Category = "NFC Policy",
                Description =
                    "Prevents Windows Wallet and third-party payment apps from using NFC for contactless payments. Removes financial transaction risk from NFC proximity attacks. Default: tap-to-pay enabled if NFC hardware present. Recommended: 1 on managed devices.",
                Tags = ["nfc", "payment", "tap-to-pay", "wallet", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NFC payment is blocked; tap-to-pay via Samsung Pay, Microsoft Wallet, or other NFC payment apps is disabled.",
                ApplyOps = [RegOp.SetDword(Key, "AllowPayment", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowPayment")],
                DetectOps = [RegOp.CheckDword(Key, "AllowPayment", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-tap-to-connect",
                Label = "Disable NFC Tap-to-Connect (Wi-Fi/BT Pairing)",
                Category = "NFC Policy",
                Description =
                    "Prevents NFC from being used to automatically pair Bluetooth headsets, speakers, or configure Wi-Fi on another device via WPS. Reduces the attack surface of proximity-based device pairing. Default: tap-to-connect enabled. Recommended: 1.",
                Tags = ["nfc", "tap-to-connect", "bluetooth", "wifi", "pairing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NFC-initiated Bluetooth pairing and Wi-Fi exchange are blocked; manual pairing through Settings is unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "AllowHandshake", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowHandshake")],
                DetectOps = [RegOp.CheckDword(Key, "AllowHandshake", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-nfc-tag-reading",
                Label = "Disable NFC Tag Reading",
                Category = "NFC Policy",
                Description =
                    "Prevents Windows from reading data from passive NFC tags (such as NFC smart posters, RFID access badges used as open-URL triggers). Eliminates malicious-tag attack vectors. Default: tag reading enabled. Recommended: 1.",
                Tags = ["nfc", "tag", "rfid", "read", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NFC passive tag reading is disabled; smart poster / rogue-tag attacks are blocked.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTagReading", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTagReading")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTagReading", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-secure-element",
                Label = "Disable NFC Secure Element Access",
                Category = "NFC Policy",
                Description =
                    "Blocks applications from accessing the NFC Secure Element (SE) — the tamper-resistant chip used for contactless payment credentials. Prevents SE-based payment credential theft. Default: SE access controlled per-app. Recommended: 1 unless approved payment apps are deployed.",
                Tags = ["nfc", "secure-element", "payment", "credentials", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "No app can access the NFC Secure Element; payment credentials stored there are inaccessible.",
                ApplyOps = [RegOp.SetDword(Key, "AllowSecureElement", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSecureElement")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSecureElement", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-block-nfc-in-enterprise",
                Label = "Block All NFC in Enterprise Mode",
                Category = "NFC Policy",
                Description =
                    "Master switch to disable all NFC functionality system-wide when the device is on a corporate/enterprise network. Provides a blanket NFC lockdown without needing individual element controls. Default: not restricted. Recommended: 1.",
                Tags = ["nfc", "enterprise", "lockdown", "wireless", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "All NFC features (read, write, payment, pairing, SE) are locked down in enterprise environments.",
                ApplyOps = [RegOp.SetDword(Key, "AllowNFCInEnterprise", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowNFCInEnterprise")],
                DetectOps = [RegOp.CheckDword(Key, "AllowNFCInEnterprise", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-nfc-sharing",
                Label = "Disable NFC Proximity Data Sharing",
                Category = "NFC Policy",
                Description =
                    "Prevents Windows from sharing files, contacts, or links via NFC proximity transfer (similar to Android Beam). Stops inadvertent or malicious near-field data exfiltration. Default: sharing enabled on NFC hardware. Recommended: 1.",
                Tags = ["nfc", "sharing", "proximity", "dlp", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NFC proximity data sharing is blocked; files and URLs cannot be transferred via NFC tap.",
                ApplyOps = [RegOp.SetDword(Key, "AllowSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSharing")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSharing", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-nfc-host-card-emulation",
                Label = "Disable NFC Host Card Emulation (HCE)",
                Category = "NFC Policy",
                Description =
                    "Blocks Host Card Emulation — the mode that allows a device to emulate an NFC smart card (e.g., transit card, building access badge) in software. Prevents rogue HCE apps from cloning or spoofing credential cards. Default: HCE enabled on supported hardware. Recommended: 1.",
                Tags = ["nfc", "hce", "card-emulation", "credentials", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "HCE is blocked; no app can emulate an NFC card, preventing badge cloning or transit card spoofing.",
                ApplyOps = [RegOp.SetDword(Key, "AllowHostCardEmulation", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowHostCardEmulation")],
                DetectOps = [RegOp.CheckDword(Key, "AllowHostCardEmulation", 0)],
            },
            new TweakDef
            {
                Id = "nfcpol-log-nfc-activity",
                Label = "Enable NFC Activity Audit Logging",
                Category = "NFC Policy",
                Description =
                    "Records NFC tap events, tag reads, and connection establishments to the Security audit log. Provides a forensic trail of physical proximity events for DLP and incident investigations. Default: not logged. Recommended: 1 on monitored endpoints.",
                Tags = ["nfc", "audit", "logging", "forensics", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "All NFC tap and connection events are written to the Security event log.",
                ApplyOps = [RegOp.SetDword(Key, "AuditNFCActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNFCActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNFCActivity", 1)],
            },
            new TweakDef
            {
                Id = "nfcpol-disable-nfc-user-toggle",
                Label = "Block Users from Toggling NFC in Settings",
                Category = "NFC Policy",
                Description =
                    "Removes the NFC toggle from Settings → Network & Internet → Airplane Mode and NFC. Users cannot re-enable NFC regardless of the hardware switch state. Default: user toggle available. Recommended: 1 when NFC is disabled by policy.",
                Tags = ["nfc", "settings", "user-restriction", "toggle", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "NFC Settings toggle is hidden/greyed out; policy setting is preserved regardless of user interaction.",
                ApplyOps = [RegOp.SetDword(Key, "BlockUserNFCToggle", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockUserNFCToggle")],
                DetectOps = [RegOp.CheckDword(Key, "BlockUserNFCToggle", 1)],
            },
        ];
}
