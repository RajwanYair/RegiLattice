namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ─────────────────────────────────────────────────────────────────────────────
// Sprint 645 — PolicyCloudClipboard (Cloud Clipboard & Clipboard History Policy)

[TweakModule]
internal static class PolicyCloudClipboard
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clipol-disable-phone-clipboard-sync",
            Label = "Disable Phone-to-PC Clipboard Sync",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being shared between a paired Android/iPhone and the Windows PC via Phone Link. Disables the mobile-to-desktop clipboard relay channel.",
            Tags = ["clipboard", "phone", "android", "phone-link", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Phone-to-PC clipboard bridge disabled; mobile clipboard items not transferred to PC.",
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardForPhone", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardForPhone")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardForPhone", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-gpt-integration",
            Label = "Disable Clipboard AI / Copilot Integration",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks Windows Copilot and AI features from reading clipboard content for contextual suggestions. Prevents AI models from processing clipboard data including passwords, banking information, or confidential documents inadvertently copied.",
            Tags = ["clipboard", "ai", "copilot", "gpt", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "AI/Copilot clipboard access blocked; sensitive copied data not processed by AI.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCopilotClipboardAccess", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCopilotClipboardAccess")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCopilotClipboardAccess", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-hello-sync",
            Label = "Disable Clipboard Sync via Windows Hello",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard content from being relayed between devices using Windows Hello companion device authentication. Stops clipboard sharing initiated through the Hello companion device framework.",
            Tags = ["clipboard", "windows-hello", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hello companion-device clipboard relay disabled.",
            ApplyOps = [RegOp.SetDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossDeviceClipboardViaWindowsHello")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCrossDeviceClipboardViaWindowsHello", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-rdp-passthrough",
            Label = "Disable RDP Clipboard Passthrough",
            Category = "Security — Privacy Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard redirection between an RDP session and the remote machine. Prevents users from pasting data from a remote desktop session into local applications, blocking a common data-exfiltration channel.",
            Tags = ["clipboard", "rdp", "remote-desktop", "exfiltration", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "RDP clipboard redirect blocked; copy-paste between RDP and local machine disabled.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "DisableClipboardRedirection", 1),
            ],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-remote-viewer",
            Label = "Disable Clipboard in Remote Assistance Sessions",
            Category = "Security — Privacy Hardening",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks clipboard access during Windows Remote Assistance sessions. Prevents the remote assistant from copying sensitive data from the user's clipboard during a coached support session.",
            Tags = ["clipboard", "remote-assistance", "security", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Remote assistance helper cannot access clipboard; data exfiltration during support blocked.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "clipol-clear-clipboard-on-lock",
            Label = "Clear Clipboard on Screen Lock",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Automatically clears all clipboard history and the current clipboard when the screen locks. Prevents sensitive information (passwords, tokens, PII) from remaining in clipboard after the user leaves their desk.",
            Tags = ["clipboard", "lock-screen", "clear", "privacy", "policy", "security"],
            RegistryKeys = [Key],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Clipboard contents wiped on every screen lock; sensitive data never retained when unattended.",
            ApplyOps = [RegOp.SetDword(Key, "ClearClipboardOnLock", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearClipboardOnLock")],
            DetectOps = [RegOp.CheckDword(Key, "ClearClipboardOnLock", 1)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-audit",
            Label = "Disable Clipboard Audit Logging",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard operation audit logging in the Windows Security event log. Stops clipboard read/write events from being written to Security log for privacy-focused deployments without audit requirements.",
            Tags = ["clipboard", "audit", "logging", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Clipboard operations not logged to Security audit log.",
            ApplyOps = [RegOp.SetDword(Key, "ClipboardAuditLogging", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClipboardAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "ClipboardAuditLogging", 0)],
        },
        new TweakDef
        {
            Id = "clipol-disable-clipboard-suggested-actions",
            Label = "Disable Clipboard Smart Actions / Suggested Text Actions",
            Category = "Privacy — Sensor",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Clipboard smart actions feature that analyses copied text and suggests contextual actions (add to calendar, call a phone number, open a URL). Stops clipboard content from being sent to local AI processing pipelines.",
            Tags = ["clipboard", "smart-actions", "ai", "privacy", "policy"],
            RegistryKeys = [Key],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard content analysis for smart actions disabled; no AI text interpretation of copied data.",
            ApplyOps = [RegOp.SetDword(Key, "AllowClipboardSuggestedActions", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowClipboardSuggestedActions")],
            DetectOps = [RegOp.CheckDword(Key, "AllowClipboardSuggestedActions", 0)],
        },
    ];
}
