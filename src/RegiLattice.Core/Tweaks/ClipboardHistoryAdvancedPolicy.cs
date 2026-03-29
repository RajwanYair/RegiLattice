// RegiLattice.Core — Tweaks/ClipboardHistoryAdvancedPolicy.cs
// Clipboard history feature controls via System Group Policy — Sprint 442.
// Disables clipboard history, cross-device sync, session sharing, app access,
// logging, rich text, background apps clipboard, item count cap, and lock-screen access.
// Category: "Clipboard History Policy" | Slug: clipadv
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ClipboardHistoryAdvancedPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "clipadv-disable-clipboard-history",
                Label = "Disable Clipboard History Feature",
                Category = "Clipboard History Policy",
                Description =
                    "Sets AllowClipboardHistory=0 to disable the Windows clipboard history feature (Win+V), preventing clipboard contents from being stored in history.",
                Tags = ["clipboard", "history", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables clipboard history; Win+V no longer shows recent clipboard items.",
                ApplyOps = [RegOp.SetDword(Key, "AllowClipboardHistory", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardHistory")],
                DetectOps = [RegOp.CheckDword(Key, "AllowClipboardHistory", 0)],
            },
            new TweakDef
            {
                Id = "clipadv-disable-cross-device-sync",
                Label = "Disable Cross-Device Clipboard Sync",
                Category = "Clipboard History Policy",
                Description =
                    "Sets AllowCrossDeviceClipboard=0 to prevent clipboard content from syncing between devices linked to the same Microsoft account via the cloud.",
                Tags = ["clipboard", "sync", "cloud", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Clipboard contents stay on-device; no cross-device sync via Microsoft account.",
                ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboard", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboard", 0)],
            },
            new TweakDef
            {
                Id = "clipadv-disable-history-across-sessions",
                Label = "Disable Clipboard History Across Sessions",
                Category = "Clipboard History Policy",
                Description = "Disables clipboard history persistence across logon sessions so clipboard items are cleared when a user logs off.",
                Tags = ["clipboard", "session", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard cleared on logoff; sensitive data does not persist between sessions.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistoryAcrossSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistoryAcrossSessions")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistoryAcrossSessions", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-block-app-clipboard-access",
                Label = "Block Clipboard Access from Apps",
                Category = "Clipboard History Policy",
                Description =
                    "Blocks background application access to clipboard contents unless the application is in the foreground, preventing silent clipboard exfiltration.",
                Tags = ["clipboard", "apps", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Background apps blocked from reading clipboard; may break clipboard managers.",
                ApplyOps = [RegOp.SetDword(Key, "BlockClipboardAccessApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardAccessApps")],
                DetectOps = [RegOp.CheckDword(Key, "BlockClipboardAccessApps", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-disable-history-logging",
                Label = "Disable Clipboard History Logging",
                Category = "Clipboard History Policy",
                Description =
                    "Disables event logging of clipboard history operations, preventing clipboard contents from appearing in diagnostic logs.",
                Tags = ["clipboard", "logging", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard history operations not logged; reduces diagnostic data.",
                ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistoryLog", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistoryLog")],
                DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistoryLog", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-restrict-to-current-user",
                Label = "Restrict Clipboard History to Current User Only",
                Category = "Clipboard History Policy",
                Description =
                    "Restricts clipboard history storage so that entries are isolated to the current user's session and cannot be accessed by other users on the same machine.",
                Tags = ["clipboard", "user", "isolation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard history is per-user only; no shared clipboard history between accounts.",
                ApplyOps = [RegOp.SetDword(Key, "ClipboardCurrentUserOnly", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClipboardCurrentUserOnly")],
                DetectOps = [RegOp.CheckDword(Key, "ClipboardCurrentUserOnly", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-disable-rich-text-clipboard",
                Label = "Disable Rich Text Clipboard Format",
                Category = "Clipboard History Policy",
                Description =
                    "Disables the rich text (RTF) clipboard format, forcing text copies to plain text and reducing the metadata stored in clipboard entries.",
                Tags = ["clipboard", "rich-text", "format", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "RTF formatting stripped on copy; pasted text is plain. May affect Word/Office workflows.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRichTextClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRichTextClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRichTextClipboard", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-block-bg-app-clipboard",
                Label = "Block Clipboard API for Background Apps",
                Category = "Clipboard History Policy",
                Description =
                    "Prevents background applications from using the clipboard API for reads or writes, limiting clipboard exposure to foreground processes only.",
                Tags = ["clipboard", "background", "api", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Background clipboard API blocked; clipboard managers and automation tools may break.",
                ApplyOps = [RegOp.SetDword(Key, "BlockClipboardBgApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockClipboardBgApps")],
                DetectOps = [RegOp.CheckDword(Key, "BlockClipboardBgApps", 1)],
            },
            new TweakDef
            {
                Id = "clipadv-max-item-count-25",
                Label = "Set Clipboard History Max Item Count to 25",
                Category = "Clipboard History Policy",
                Description = "Caps the clipboard history list at 25 entries to limit on-disk footprint of potentially sensitive copied data.",
                Tags = ["clipboard", "limit", "history", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Older clipboard items purged after 25; reduces sensitive data retention.",
                ApplyOps = [RegOp.SetDword(Key, "ClipboardMaxItemCount", 25)],
                RemoveOps = [RegOp.DeleteValue(Key, "ClipboardMaxItemCount")],
                DetectOps = [RegOp.CheckDword(Key, "ClipboardMaxItemCount", 25)],
            },
            new TweakDef
            {
                Id = "clipadv-disable-lock-screen-clipboard",
                Label = "Disable Clipboard History on Lock Screen",
                Category = "Clipboard History Policy",
                Description =
                    "Disables access to clipboard history from the lock screen, preventing unauthenticated users from viewing previously copied content.",
                Tags = ["clipboard", "lock-screen", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Clipboard history inaccessible on lock screen; prevents physical access leakage.",
                ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenClipboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenClipboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenClipboard", 1)],
            },
        ];
}
