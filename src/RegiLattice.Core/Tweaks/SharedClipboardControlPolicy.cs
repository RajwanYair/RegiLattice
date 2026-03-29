// RegiLattice.Core — Tweaks/SharedClipboardControlPolicy.cs
// Shared clipboard, Phone Link clipboard sharing, and cloud clipboard controls — Sprint 444.
// Uses distinct value names from ClipboardHistoryAdvancedPolicy.cs and
// UniversalClipboardSyncPolicy.cs at the same HKLM\...\Windows\System path.
// Category: "Shared Clipboard Control" | Slug: shrdclip
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\CloudContent

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SharedClipboardControlPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "shrdclip-disable-phone-link",
                Label = "Disable Phone Link Clipboard Share",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables clipboard sharing between Windows and a linked Android/iOS phone via the Phone Link (Your Phone) app, preventing cross-device clipboard leakage.",
                Tags = ["clipboard", "phone-link", "sharing", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Phone Link clipboard sync disabled; clipboard data stays on the PC.",
                ApplyOps = [RegOp.SetDword(Key, "DisablePhoneLinkClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneLinkClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePhoneLinkClipboard", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-sync-across-devices",
                Label = "Disable Clipboard Sync Across Devices",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables device-to-device clipboard synchronization at the policy level, complementing AllowCrossDeviceClipboard by blocking back-end sync service.",
                Tags = ["clipboard", "sync", "devices", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cross-device clipboard sync blocked at policy level.",
                ApplyOps = [RegOp.SetDword(Key, "ClipboardSyncBlock", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClipboardSyncBlock")],
                DetectOps = [RegOp.CheckDword(Key, "ClipboardSyncBlock", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-cloud-clipboard",
                Label = "Disable Cloud Clipboard Sync",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables cloud clipboard synchronization feature that uploads clipboard contents to Microsoft's cloud for cross-device access.",
                Tags = ["clipboard", "cloud", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Cloud clipboard disabled; clipboard items not uploaded to Microsoft cloud.",
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudClipboardContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudClipboardContent")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudClipboardContent", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-tooltip-ads",
                Label = "Disable Clipboard History Tooltip Ads",
                Category = "Shared Clipboard Control",
                Description =
                    "Hides promotional tooltips and advertisements shown in the clipboard history panel (Win+V) that encourage enabling cloud clipboard or other Microsoft services.",
                Tags = ["clipboard", "tooltip", "ads", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes clipboard promotional tooltips; no functional impact.",
                ApplyOps = [RegOp.SetDword(Key, "HideClipboardTooltips", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideClipboardTooltips")],
                DetectOps = [RegOp.CheckDword(Key, "HideClipboardTooltips", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-block-microsoft-apps",
                Label = "Block Clipboard Access by Microsoft Apps",
                Category = "Shared Clipboard Control",
                Description =
                    "Blocks Microsoft first-party applications from accessing the clipboard history API, reducing telemetry and data collection from clipboard contents.",
                Tags = ["clipboard", "microsoft-apps", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Microsoft apps clipboard access restricted; some features may degrade.",
                ApplyOps = [RegOp.SetDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMicrosoftApplicationsClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-contextual-suggestions",
                Label = "Disable Contextual Suggestions in Clipboard",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables contextual content suggestions (e.g., smart replies, URL previews) that appear in the clipboard history panel based on clipboard content analysis.",
                Tags = ["clipboard", "suggestions", "privacy", "cloud-content", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Clipboard content not analysed for suggestions; fully private.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardContextSuggestions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardContextSuggestions")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardContextSuggestions", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-telemetry",
                Label = "Disable Clipboard Telemetry",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables telemetry data collection about clipboard usage patterns, preventing clipboard interaction metadata from being sent to Microsoft.",
                Tags = ["clipboard", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard usage telemetry stopped; no functional impact.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardDiagnosticData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardDiagnosticData")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardDiagnosticData", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-block-sensitive-content-detection",
                Label = "Block Sensitive Clipboard Content Detection",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables content scanning of clipboard items for sensitive data classification (DLP-style detection) by cloud-connected services.",
                Tags = ["clipboard", "sensitive", "dlp", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard content not scanned for sensitive data by cloud services.",
                ApplyOps = [RegOp.SetDword(Key2, "BlockClipboardSensitiveContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "BlockClipboardSensitiveContent")],
                DetectOps = [RegOp.CheckDword(Key2, "BlockClipboardSensitiveContent", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-disable-uwp-clipboard-api",
                Label = "Disable Clipboard API for UWP Apps",
                Category = "Shared Clipboard Control",
                Description =
                    "Disables the WinRT clipboard API access for Universal Windows Platform (UWP) apps, preventing packaged apps from silently reading or writing the clipboard.",
                Tags = ["clipboard", "uwp", "api", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "UWP apps cannot access clipboard API; clipboard-dependent UWP features break.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableUwpClipboardAPI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableUwpClipboardAPI")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableUwpClipboardAPI", 1)],
            },
            new TweakDef
            {
                Id = "shrdclip-restrict-same-process-paste",
                Label = "Restrict Clipboard Paste to Same Process",
                Category = "Shared Clipboard Control",
                Description =
                    "Restricts the clipboard paste operation so that clipboard data can only be pasted within the same process that originally wrote it, preventing cross-process data leakage via clipboard.",
                Tags = ["clipboard", "paste", "isolation", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Cross-process paste restricted; applications that rely on clipboard sharing between processes will break.",
                ApplyOps = [RegOp.SetDword(Key2, "RestrictSameProcessClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "RestrictSameProcessClipboard")],
                DetectOps = [RegOp.CheckDword(Key2, "RestrictSameProcessClipboard", 1)],
            },
        ];
}
