// RegiLattice.Core — Tweaks/ClipboardHistoryPolicy.cs
// Sprint 272: Clipboard History Group Policy (10 tweaks)
// Category: "Clipboard History Policy" | Slug: clphist
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ClipboardHistory

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ClipboardHistoryPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ClipboardHistory";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clphist-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableClipboardHistory=1 in the ClipboardHistory policy key. Prevents "
                + "Windows from storing a multi-item clipboard history accessible via Win+V. "
                + "Clipboard history retains copied text, images, and HTML fragments in memory "
                + "across application boundaries. Disabling it limits the clipboard to one "
                + "item at a time, reducing the surface area for data leakage. "
                + "Default: 0. Recommended: 1 on shared or high-security machines.",
            Tags = ["clipboard", "history", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardHistory")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardHistory", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-cloud-sync",
            Label = "Disable Clipboard Cloud Sync",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 4, SafetyRating = 5,
            Description =
                "Sets DisableClipboardSync=1 in the ClipboardHistory policy key. Stops "
                + "clipboard content from being synchronised to Microsoft's cloud and "
                + "distributed to other devices signed in with the same Microsoft account. "
                + "Cloud sync can exfiltrate copied passwords, keys, and PII to additional "
                + "devices the user owns or shares. Default: 0. Recommended: 1 always.",
            Tags = ["clipboard", "cloud", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardSync", 1)],
        },
        new TweakDef
        {
            Id = "clphist-clear-on-logoff",
            Label = "Clear Clipboard History on Logoff",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets ClearClipboardOnLogoff=1 in the ClipboardHistory policy key. Purges "
                + "the entire stored clipboard history when the user logs off. Without this "
                + "policy the history persists across sessions, meaning a subsequent user "
                + "on a shared machine can inspect copied content from the previous session "
                + "via Win+V. Default: 0. Recommended: 1 on shared workstations.",
            Tags = ["clipboard", "logoff", "clear", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ClearClipboardOnLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ClearClipboardOnLogoff")],
            DetectOps = [RegOp.CheckDword(Key, "ClearClipboardOnLogoff", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-enterprise-sync",
            Label = "Disable Clipboard Enterprise Roaming",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 3, SafetyRating = 5,
            Description =
                "Sets DisableEnterpriseSync=1 in the ClipboardHistory policy key. Prevents "
                + "clipboard history from roaming across enterprise devices enrolled in the "
                + "same Azure AD tenant via the enterprise clipboard sync service. Roaming "
                + "clipboard in an enterprise context can propagate sensitive data from a "
                + "secure workstation to a less-secure shared device. "
                + "Default: 0. Recommended: 1 in regulated industries.",
            Tags = ["clipboard", "enterprise", "sync", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseSync", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-pin-items",
            Label = "Disable Clipboard Pin Persistent Items",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisablePinItems=1 in the ClipboardHistory policy key. Prevents users "
                + "from pinning clipboard items, blocking indefinite retention of specific "
                + "copied fragments in the history viewer. Pinned items are never evicted "
                + "by the normal rotation algorithm, meaning sensitive data could persist "
                + "across many sessions. Default: 0. Recommended: 1.",
            Tags = ["clipboard", "pin", "retention", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePinItems", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePinItems")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePinItems", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-image-data",
            Label = "Disable Clipboard Image Data Retention",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableImageData=1 in the ClipboardHistory policy key. Prevents "
                + "Windows from storing bitmap and image data in the clipboard history. "
                + "Image clipboard entries can be large and may contain screenshots of "
                + "confidential documents. Text-only clipboard history is significantly "
                + "smaller and less sensitive. Default: 0. Recommended: 1.",
            Tags = ["clipboard", "image", "screenshot", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableImageData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableImageData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableImageData", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-html-data",
            Label = "Disable Clipboard HTML Fragment Retention",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableHtmlData=1 in the ClipboardHistory policy key. Prevents the "
                + "history from storing HTML-format clipboard entries produced by browsers "
                + "and rich-text editors. HTML clipboard data can include embedded form "
                + "field values, session tokens, and styling metadata that goes beyond the "
                + "visible copied text. Default: 0. Recommended: 1.",
            Tags = ["clipboard", "html", "browser", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHtmlData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHtmlData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHtmlData", 1)],
        },
        new TweakDef
        {
            Id = "clphist-disable-thumbnail-preview",
            Label = "Disable Clipboard History Thumbnail Preview",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 1, SafetyRating = 5,
            Description =
                "Sets DisableThumbnailPreview=1 in the ClipboardHistory policy key. Removes "
                + "the visual thumbnail preview shown in the Win+V clipboard picker. Thumbnail "
                + "previews generate on-demand renders of previously copied images and "
                + "documents, caching them for rapid display. Disabling removes the cache "
                + "and reduces memory consumption. Default: 0. Recommended: 1.",
            Tags = ["clipboard", "thumbnail", "preview", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableThumbnailPreview", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableThumbnailPreview")],
            DetectOps = [RegOp.CheckDword(Key, "DisableThumbnailPreview", 1)],
        },
        new TweakDef
        {
            Id = "clphist-limit-history-size",
            Label = "Limit Clipboard History Size",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets MaxHistorySize=10 in the ClipboardHistory policy key. Caps the number "
                + "of items retained in clipboard history to 10 entries (default system "
                + "maximum is 25). A smaller history window reduces the amount of data "
                + "available to an attacker who briefly accesses the machine and reviews "
                + "history via Win+V. Default: not set (25 items). Recommended: 10.",
            Tags = ["clipboard", "history", "limit", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxHistorySize", 10)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxHistorySize")],
            DetectOps = [RegOp.CheckDword(Key, "MaxHistorySize", 10)],
        },
        new TweakDef
        {
            Id = "clphist-disable-telemetry",
            Label = "Disable Clipboard History Telemetry",
            Category = "Clipboard History Policy",
            NeedsAdmin = true, CorpSafe = true, ImpactScore = 2, SafetyRating = 5,
            Description =
                "Sets DisableClipboardTelemetry=1 in the ClipboardHistory policy key. "
                + "Prevents Windows from reporting clipboard history usage analytics "
                + "(feature engagement, copy-paste frequency, sync events) to Microsoft's "
                + "telemetry pipeline. These signals inform product improvements but transmit "
                + "behavioural metadata outside of normal diagnostic data consent. "
                + "Default: 0. Recommended: 1.",
            Tags = ["clipboard", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableClipboardTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClipboardTelemetry", 1)],
        },
    ];
}
