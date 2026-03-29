// RegiLattice.Core — Tweaks/ClipboardSensitivityPolicy.cs
// Clipboard DLP, monitoring, PII protection, and sensitivity controls — Sprint 446.
// Uses DataCollection key for diagnostic/DLP values and unique System key values
// distinct from all other clipboard policy modules.
// Category: "Clipboard Sensitivity Policy" | Slug: clipsens
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ClipboardSensitivityPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DataCollection";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "clipsens-block-sensitive-data-dlp",
                Label = "Block Sensitive Data in Clipboard (DLP)",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Enables DLP-style clipboard content blocking that prevents sensitive data (PII, credentials, financial info) from being copied to the clipboard by monitored apps.",
                Tags = ["clipboard", "dlp", "sensitive", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Sensitive-data clipboard block; may interrupt legitimate copy operations for data classes.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardDLP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardDLP")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardDLP", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-disable-diagnostic-monitoring",
                Label = "Disable Clipboard Monitoring by Diagnostic Services",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Disables clipboard monitoring by Windows diagnostic data services, preventing clipboard usage data (not content) from being collected as diagnostic telemetry.",
                Tags = ["clipboard", "monitoring", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Diagnostic clipboard monitoring stopped; reduces telemetry without functional impact.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMonitoringDiag", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMonitoringDiag")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMonitoringDiag", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-block-pii-in-history",
                Label = "Block PII from Clipboard History",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Prevents personally identifiable information from being stored in clipboard history entries, stripping or blocking PII items before they enter the history store.",
                Tags = ["clipboard", "pii", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "PII-matching clipboard items excluded from history; requires content scanning.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPIIInClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPIIInClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPIIInClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-disable-usage-analytics",
                Label = "Disable Clipboard Usage Analytics",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Disables collection of clipboard usage analytics (copy/paste frequency, format types, app usage) sent to Microsoft for product improvement.",
                Tags = ["clipboard", "analytics", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Clipboard analytics not collected; no functional impact.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMetrics", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMetrics")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMetrics", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-restrict-secure-desktops",
                Label = "Restrict Clipboard to Secure Desktops Only",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Restricts clipboard operations to secure desktop contexts only, preventing clipboard data from crossing the security boundary between secure and non-secure desktops.",
                Tags = ["clipboard", "secure-desktop", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Clipboard may not cross secure/non-secure desktop boundary.",
                ApplyOps = [RegOp.SetDword(Key, "SecureDesktopClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "SecureDesktopClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "SecureDesktopClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-block-bluetooth-share",
                Label = "Block Clipboard Sharing via Bluetooth",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Blocks clipboard content from being shared over Bluetooth connections (e.g., via Swift Pair or Nearby Sharing with Bluetooth transport).",
                Tags = ["clipboard", "bluetooth", "sharing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard not shared via Bluetooth; nearby Bluetooth devices cannot receive clipboard data.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardBluetoothShare", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardBluetoothShare")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardBluetoothShare", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-disable-kiosk-clipboard",
                Label = "Disable Clipboard Access in Kiosk Mode",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Disables clipboard access in Kiosk (Assigned Access) mode, preventing kiosk users from copying data from the kiosk session to other applications.",
                Tags = ["clipboard", "kiosk", "assigned-access", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Kiosk clipboard blocked; kiosk users cannot copy data out of the session.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableKioskModeClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableKioskModeClipboard")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableKioskModeClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-prevent-password-paste",
                Label = "Prevent Password Paste from Clipboard Manager",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Blocks clipboard managers from injecting stored passwords into password fields via paste, requiring direct typing or approved password manager integration.",
                Tags = ["clipboard", "password", "paste", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Password paste from clipboard managers blocked; users must type or use approved password manager.",
                ApplyOps = [RegOp.SetDword(Key2, "PreventPasswordPasteFromClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "PreventPasswordPasteFromClipboard")],
                DetectOps = [RegOp.CheckDword(Key2, "PreventPasswordPasteFromClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clipsens-max-data-size-64kb",
                Label = "Restrict Clipboard Max Data Size to 64 KB",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Caps the maximum size of a single clipboard entry at 64 KB (65536 bytes), limiting the volume of bulk data that can be exfiltrated in a single clipboard operation.",
                Tags = ["clipboard", "size-limit", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Large clipboard copies (e.g., image, file list) truncated or blocked above 64 KB.",
                ApplyOps = [RegOp.SetDword(Key2, "ClipboardMaxDataSizeKB", 64)],
                RemoveOps = [RegOp.DeleteValue(Key2, "ClipboardMaxDataSizeKB")],
                DetectOps = [RegOp.CheckDword(Key2, "ClipboardMaxDataSizeKB", 64)],
            },
            new TweakDef
            {
                Id = "clipsens-disable-encryption-bypass",
                Label = "Disable Clipboard Encryption Bypass",
                Category = "Clipboard Sensitivity Policy",
                Description =
                    "Disables clipboard encryption bypass mechanisms that allow certain privileged processes to read encrypted clipboard contents without proper decryption.",
                Tags = ["clipboard", "encryption", "bypass", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Encryption bypass paths blocked; all clipboard access goes through standard decryption.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardEncryptionBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardEncryptionBypass")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardEncryptionBypass", 1)],
            },
        ];
}
