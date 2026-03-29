// RegiLattice.Core — Tweaks/UniversalClipboardSyncPolicy.cs
// Universal clipboard sync, browser clipboard integration, and Edge clipboard controls — Sprint 445.
// Uses distinct value names from ClipboardHistoryAdvancedPolicy and SharedClipboardControlPolicy.
// Category: "Universal Clipboard Sync Policy" | Slug: uniclip
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System
//           HKLM\SOFTWARE\Policies\Microsoft\EdgeUpdate

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class UniversalClipboardSyncPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\EdgeUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "uniclip-disable-mobile-device-sync",
                Label = "Disable Windows Mobile Device Clipboard Sync",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables clipboard synchronization between Windows and mobile devices (Android phones, tablets) through the Universal Clipboard infrastructure.",
                Tags = ["clipboard", "mobile", "sync", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Clipboard not synchronized to mobile devices; all clipboard data stays on PC.",
                ApplyOps = [RegOp.SetDword(Key, "DisableMobileClipboardSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileClipboardSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobileClipboardSync", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-disable-clipboard-msa",
                Label = "Disable Clipboard Access for Microsoft Accounts",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Prevents Microsoft account-linked clipboard history from being accessible across devices tied to the same MSA, blocking cloud-backed clipboard sharing.",
                Tags = ["clipboard", "msa", "account", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "MSA-linked clipboard sharing disabled; useful for separating personal/work contexts.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardMicrosoftAccounts")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardMicrosoftAccounts", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-restrict-trusted-apps",
                Label = "Restrict Clipboard to Trusted Apps Only",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Restricts clipboard API access to applications in an approved trust list, blocking unrecognized or unsigned apps from accessing clipboard contents.",
                Tags = ["clipboard", "trusted-apps", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Clipboard restricted to trusted apps; unapproved apps receive empty clipboard reads.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardTrustedApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardTrustedApps")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardTrustedApps", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-block-third-party-managers",
                Label = "Block Third-Party Clipboard Managers",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Blocks third-party clipboard manager applications from accessing the extended clipboard history API, preventing unapproved software from storing clipboard data.",
                Tags = ["clipboard", "third-party", "manager", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Third-party clipboard managers lose access to clipboard history API.",
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyClipboardManagers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyClipboardManagers")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyClipboardManagers", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-disable-html-format",
                Label = "Disable HTML Clipboard Format",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables the HTML clipboard format, forcing web content copies to plain text and preventing HTML metadata (tracking pixels, inline styles) from being stored in clipboard.",
                Tags = ["clipboard", "html", "format", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Web content copied as plain text only; HTML formatting stripped.",
                ApplyOps = [RegOp.SetDword(Key, "DisableHtmlClipboardFormat", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHtmlClipboardFormat")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHtmlClipboardFormat", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-restrict-history-admins",
                Label = "Restrict Clipboard History to Admin Accounts Only",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Limits clipboard history storage and retrieval to administrator accounts only, preventing standard user clipboard data from accumulating in shared history.",
                Tags = ["clipboard", "admin", "restriction", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Standard user clipboard history disabled; only admin accounts retain clipboard history.",
                ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardHistoryAdminsOnly")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardHistoryAdminsOnly", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-disable-prediction-service",
                Label = "Disable Clipboard Prediction Service",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables the clipboard prediction background service that analyses clipboard contents to provide predictive paste suggestions.",
                Tags = ["clipboard", "prediction", "background", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Predictive paste suggestions disabled; clipboard contents not analysed.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardPredictionService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardPredictionService")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardPredictionService", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-block-sync-service",
                Label = "Block Clipboard Sync Background Service",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables the background clipboard synchronization service that maintains clipboard state across devices and cloud endpoints.",
                Tags = ["clipboard", "sync-service", "background", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Background clipboard sync service stopped; universal clipboard fully disabled.",
                ApplyOps = [RegOp.SetDword(Key, "BlockClipboardSyncService", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardSyncService")],
                DetectOps = [RegOp.CheckDword(Key, "BlockClipboardSyncService", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-disable-edge-clipboard-access",
                Label = "Disable Browser Clipboard Integration via EdgeUpdate Policy",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables clipboard access integration for the Edge browser via EdgeUpdate policy, preventing Edge from participating in universal clipboard sync.",
                Tags = ["clipboard", "edge", "browser", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge clipboard integration disabled; browser clipboard not shared via EdgeUpdate.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardAccess")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardAccess", 1)],
            },
            new TweakDef
            {
                Id = "uniclip-disable-edge-clipboard-manager",
                Label = "Disable Edge Clipboard Manager",
                Category = "Universal Clipboard Sync Policy",
                Description =
                    "Disables the Edge browser's built-in clipboard manager feature that maintains browser-side clipboard history and sharing via EdgeUpdate policy.",
                Tags = ["clipboard", "edge", "manager", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Edge clipboard manager disabled; browser clipboard history feature unavailable.",
                ApplyOps = [RegOp.SetDword(Key2, "DisableEdgeClipboardManager", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "DisableEdgeClipboardManager")],
                DetectOps = [RegOp.CheckDword(Key2, "DisableEdgeClipboardManager", 1)],
            },
        ];
}
