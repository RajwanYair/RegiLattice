// RegiLattice.Core — Tweaks/RecallAiSnapshotPolicy.cs
// Windows Recall AI snapshot capture, storage, and search policy controls — Sprint 482.
// Category: "Recall AI Snapshot Policy" | Slug: rcsnap
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsAI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RecallAiSnapshotPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsAI";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "rcsnap-disable-recall",
                Label = "Disable Windows Recall AI Snapshots",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Disables the Windows Recall feature entirely, preventing the AI from taking periodic screenshots ('snapshots') of the user's screen for semantic search indexing on Copilot+ PCs.",
                Tags = ["recall", "ai", "copilot-plus", "privacy", "snapshot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Windows Recall disabled; no AI snapshots taken. Recall search feature unavailable.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAIDataAnalysis", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAIDataAnalysis")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAIDataAnalysis", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-block-sensitive-content-capture",
                Label = "Block Recall from Capturing Sensitive Content",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Enables the sensitive content filter for Windows Recall, blocking the AI snapshot engine from capturing screens containing passwords, financial information, and other sensitive data.",
                Tags = ["recall", "ai", "privacy", "sensitive-content", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Recall sensitive content filter enabled; DLP-classified content excluded from AI snapshots.",
                ApplyOps = [RegOp.SetDword(Key, "FilterSensitiveContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "FilterSensitiveContent")],
                DetectOps = [RegOp.CheckDword(Key, "FilterSensitiveContent", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-set-max-storage-1gb",
                Label = "Set Recall Snapshot Storage Limit to 1 GB",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Limits the disk space allocated for Windows Recall snapshot storage to 1 GB, reducing the volume of AI-searchable screen content retained on the device.",
                Tags = ["recall", "ai", "storage", "disk-quota", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Recall storage capped at 1 GB; oldest snapshots are purged first when cap is reached.",
                ApplyOps = [RegOp.SetDword(Key, "SnapshotStorageLimitMB", 1024)],
                RemoveOps = [RegOp.DeleteValue(Key, "SnapshotStorageLimitMB")],
                DetectOps = [RegOp.CheckDword(Key, "SnapshotStorageLimitMB", 1024)],
            },
            new TweakDef
            {
                Id = "rcsnap-block-incognito-capture",
                Label = "Block Recall Snapshots in InPrivate/Incognito Browser Windows",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Prevents Windows Recall from capturing screenshots of InPrivate (Edge) and Incognito (Chrome) browser windows, protecting private browsing content from AI indexing.",
                Tags = ["recall", "ai", "browser", "incognito", "privacy", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "InPrivate/Incognito browser windows excluded from Recall AI snapshots.",
                ApplyOps = [RegOp.SetDword(Key, "ExcludePrivateBrowsing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ExcludePrivateBrowsing")],
                DetectOps = [RegOp.CheckDword(Key, "ExcludePrivateBrowsing", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-disable-ai-optical-character-recognition",
                Label = "Disable AI OCR for Recall Snapshot Indexing",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Disables the Optical Character Recognition (OCR) pass that Windows Recall applies to snapshots for full-text indexing, reducing AI compute load and preventing text extraction from captured screenshots.",
                Tags = ["recall", "ai", "ocr", "indexing", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "OCR disabled for Recall; text within screenshots not searchable by Recall's semantic engine.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSnapshotOCR", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSnapshotOCR")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSnapshotOCR", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-require-wdp-for-recall-db",
                Label = "Require Windows Data Protection for Recall Snapshot DB",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Requires that the Windows Recall snapshot database is encrypted using Windows Data Protection API (DPAPI) and additional credential guard protection, preventing raw disk access to the snapshot index.",
                Tags = ["recall", "ai", "encryption", "dpapi", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Recall database protected by DPAPI; snapshot index inaccessible without user credential.",
                ApplyOps = [RegOp.SetDword(Key, "RequireDBEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireDBEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "RequireDBEncryption", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-disable-recall-on-battery",
                Label = "Disable Recall AI Snapshots on Battery Power",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Suspends Windows Recall snapshot capture when the device is running on battery power, reducing AI compute drain and extending battery life by halting background NPU-based screenshot analysis.",
                Tags = ["recall", "ai", "battery", "performance", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Recall paused on battery; AI snapshots resume when AC charger is connected.",
                ApplyOps = [RegOp.SetDword(Key, "DisableRecallOnBattery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRecallOnBattery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRecallOnBattery", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-purge-snapshots-on-signout",
                Label = "Purge Recall Snapshots on User Sign-Out",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Automatically deletes all Windows Recall snapshots from the current session when the user signs out, preventing accumulated AI snapshot data from persisting between sessions on shared devices.",
                Tags = ["recall", "ai", "privacy", "sign-out", "shared-device", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Recall snapshots purged on sign-out; AI history does not persist across sessions.",
                ApplyOps = [RegOp.SetDword(Key, "PurgeSnapshotsOnSignOut", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PurgeSnapshotsOnSignOut")],
                DetectOps = [RegOp.CheckDword(Key, "PurgeSnapshotsOnSignOut", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-block-app-exclusion-override",
                Label = "Block Users from Modifying Recall App Exclusion List",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Prevents users from adding or removing applications from the Recall snapshot exclusion list, ensuring that IT-defined exclusions (e.g., banking apps, password managers) cannot be overridden by the user.",
                Tags = ["recall", "ai", "app-exclusion", "user-restriction", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Recall app exclusion list locked; users cannot add or remove apps from the exclusion list.",
                ApplyOps = [RegOp.SetDword(Key, "LockAppExclusionList", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockAppExclusionList")],
                DetectOps = [RegOp.CheckDword(Key, "LockAppExclusionList", 1)],
            },
            new TweakDef
            {
                Id = "rcsnap-disable-recall-timeline-view",
                Label = "Disable Recall AI Timeline Timeline View",
                Category = "Recall AI Snapshot Policy",
                Description =
                    "Disables the visual timeline view in Windows Recall that allows users to scroll back through past AI snapshots, removing the UI entry point while optionally allowing background capture to continue.",
                Tags = ["recall", "ai", "timeline", "ui", "copilot-plus", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Recall timeline UI disabled; users cannot browse past AI snapshots visually.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTimelineView", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTimelineView")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTimelineView", 1)],
            },
        ];
}
