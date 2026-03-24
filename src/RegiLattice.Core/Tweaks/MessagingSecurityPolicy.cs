// RegiLattice.Core — Tweaks/MessagingSecurityPolicy.cs
// Windows Messaging app and SMS/MMS machine-scope GPO controls — Sprint 208.
// Controls Messaging app access, sync, MMS capabilities, and data retention policies.
// Category: "Messaging Security Policy" | Slug: msgsec
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Messaging

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class MessagingSecurityPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Messaging";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "msgsec-disable-messaging-sync",
                Label = "Disable Messaging Cloud Sync",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents the Windows Messaging app from synchronising SMS / MMS messages to the Microsoft cloud for cross-device access. Keeps message content on-device only. Default: sync enabled. Recommended: 1 for data sovereignty requirements.",
                Tags = ["messaging", "sms", "sync", "cloud", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SMS/MMS messages are not uploaded to Microsoft's cloud; cross-device message continuity is disabled.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMessageSync", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMessageSync")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMessageSync", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-disable-mms-support",
                Label = "Disable MMS / Rich Communication Support",
                Category = "Messaging Security Policy",
                Description =
                    "Blocks the Windows Messaging application from sending or receiving MMS messages (picture messages, group messages). Limits messaging to plain SMS text only. Default: MMS enabled. Recommended: 1 on devices without approved MMS plans.",
                Tags = ["messaging", "mms", "picture", "restrict", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "MMS (picture/multimedia messages) are blocked; plain SMS text messages are unaffected.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMMS", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMMS")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMMS", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-disable-rich-communication",
                Label = "Disable RCS / Rich Communication Services",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents the Messaging app from using RCS (Rich Communication Services), which offers read receipts, typing indicators, and high-res file transfer over mobile data. Contains metadata leakage risks. Default: RCS enabled when supported. Recommended: 1.",
                Tags = ["messaging", "rcs", "rich-communication", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "RCS features (read receipts, typing indicators, large file share) are disabled.",
                ApplyOps = [RegOp.SetDword(Key, "AllowRCS", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowRCS")],
                DetectOps = [RegOp.CheckDword(Key, "AllowRCS", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-block-messaging-backup",
                Label = "Block Messaging App Cloud Backup",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents the Windows Messaging app from backing up message content to OneDrive or other cloud storage. Ensures message data remains local and is not stored in cloud accounts. Default: backup allowed. Recommended: 1.",
                Tags = ["messaging", "backup", "cloud", "onedrive", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Messaging backup to cloud storage is blocked; message history is device-local only.",
                ApplyOps = [RegOp.SetDword(Key, "AllowMessageBackup", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowMessageBackup")],
                DetectOps = [RegOp.CheckDword(Key, "AllowMessageBackup", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-set-retention-90days",
                Label = "Set Message Retention to 90 Days",
                Category = "Messaging Security Policy",
                Description =
                    "Configures the Messaging app to automatically delete messages older than 90 days. Limits the on-device message data footprint and reduces exposure in the event of device compromise. Default: messages kept indefinitely. Recommended: 90.",
                Tags = ["messaging", "retention", "deletion", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Messages older than 90 days are automatically purged; historic messages are not recoverable after deletion.",
                ApplyOps = [RegOp.SetDword(Key, "MessageRetentionDays", 90)],
                RemoveOps = [RegOp.DeleteValue(Key, "MessageRetentionDays")],
                DetectOps = [RegOp.CheckDword(Key, "MessageRetentionDays", 90)],
            },
            new TweakDef
            {
                Id = "msgsec-disable-message-notification-preview",
                Label = "Disable Message Content Preview in Notifications",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents message text from being displayed in lock screen or toast notifications. Only 'New message' is shown, not the sender or content. Default: content preview shown. Recommended: 1 for screen-sharing and shared-workspace environments.",
                Tags = ["messaging", "notification", "preview", "privacy", "lock-screen", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Message content is hidden from notifications; only the app name is shown on lock screen.",
                ApplyOps = [RegOp.SetDword(Key, "ShowMessagePreview", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShowMessagePreview")],
                DetectOps = [RegOp.CheckDword(Key, "ShowMessagePreview", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-block-group-messaging",
                Label = "Block Group Messaging",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents creation of or participation in group SMS/MMS conversations. Reduces risk of accidental data disclosure to an unintended recipient set. Default: group messaging allowed. Recommended: 1 in regulated environments.",
                Tags = ["messaging", "group", "sms", "compliance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Group messaging is blocked; messages can only be sent to individual recipients.",
                ApplyOps = [RegOp.SetDword(Key, "AllowGroupMessaging", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowGroupMessaging")],
                DetectOps = [RegOp.CheckDword(Key, "AllowGroupMessaging", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-disable-read-receipts",
                Label = "Disable SMS/MMS Read Receipts",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents the Messaging app from sending read receipts to senders when messages are opened. Stops informing external parties when the user has read a message. Default: receipts sent. Recommended: 1 for privacy.",
                Tags = ["messaging", "read-receipt", "privacy", "sms", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Read receipts are not transmitted; senders cannot tell when messages have been read.",
                ApplyOps = [RegOp.SetDword(Key, "SendReadReceipts", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SendReadReceipts")],
                DetectOps = [RegOp.CheckDword(Key, "SendReadReceipts", 0)],
            },
            new TweakDef
            {
                Id = "msgsec-block-premium-sms",
                Label = "Block Premium SMS / Reverse-Charge Messages",
                Category = "Messaging Security Policy",
                Description =
                    "Prevents apps from sending messages to premium-rate or reverse-charge SMS numbers without explicit user confirmation for each message. Protects against malware-driven premium SMS charges. Default: apps can send freely. Recommended: 1.",
                Tags = ["messaging", "premium-sms", "billing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Premium SMS messages require explicit user confirmation; silent premium SMS by apps is blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockPremiumSms", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockPremiumSms")],
                DetectOps = [RegOp.CheckDword(Key, "BlockPremiumSms", 1)],
            },
            new TweakDef
            {
                Id = "msgsec-disable-smart-reply",
                Label = "Disable Messaging Smart Reply Suggestions",
                Category = "Messaging Security Policy",
                Description =
                    "Disables the AI-powered smart reply suggestions in the Messaging app that analyse incoming message content to offer quick replies. Prevents message content from being processed by suggestion models. Default: smart replies on. Recommended: 1.",
                Tags = ["messaging", "smart-reply", "ai", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Smart reply suggestions are hidden; message content is not analysed for quick-reply AI features.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSmartReply", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSmartReply")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSmartReply", 1)],
            },
        ];
}
