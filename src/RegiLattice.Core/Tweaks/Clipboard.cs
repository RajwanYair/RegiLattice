namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Clipboard
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clip-increase-drag-threshold",
            Label = "Increase Drag-Drop Threshold (10 px)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases drag start threshold to 10 pixels. Prevents accidental drag on high-DPI screens. Default: 4 pixels. Recommended: 10.",
            Tags = ["clipboard", "drag", "drop", "sensitivity", "dpi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "10"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10")],
        },
        new TweakDef
        {
            Id = "clip-decrease-drag-threshold",
            Label = "Decrease Drag-Drop Threshold (2 px)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Decreases drag start threshold to 2 pixels for easier dragging. Default: 4 pixels. Recommended: 2 (for touchscreen/pen).",
            Tags = ["clipboard", "drag", "drop", "sensitivity", "touch"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "2"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "2"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "2")],
        },
        new TweakDef
        {
            Id = "clip-disable-rdp-clipboard",
            Label = "Disable RDP Clipboard Redirection",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard sharing in Remote Desktop sessions. Prevents data leakage via copy/paste in RDP. Default: 0 (allowed). Recommended: 1 (disabled) for security.",
            Tags = ["clipboard", "rdp", "security", "remote"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "clip-instant-drag-delay",
            Label = "Set Instant Drag Delay (0 ms)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the 200 ms delay before a drag operation begins. Makes drag-and-drop feel more responsive. Default: 200 ms. Recommended: 0.",
            Tags = ["clipboard", "drag", "delay", "responsiveness"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "200")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragDelay", "0")],
        },
        new TweakDef
        {
            Id = "clip-disable-suggested-actions",
            Label = "Disable Clipboard Suggested Actions",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the suggested actions popup that appears when copying phone numbers/dates (Win11 22H2+). Default: enabled. Recommended: 0 (disabled).",
            Tags = ["clipboard", "suggestions", "popup", "win11"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "EnableClipboardSuggestedActions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-cloud-clipboard",
            Label = "Disable Cloud Clipboard Sync",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables cloud clipboard sync and automatic upload. Prevents clipboard data from being sent to Microsoft cloud services. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["clipboard", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-roaming",
            Label = "Disable Clipboard Roaming (Policy)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables cross-device clipboard roaming via Group Policy. Prevents clipboard content from syncing across devices. Default: allowed. Recommended: disabled for security.",
            Tags = ["clipboard", "roaming", "policy", "cross-device"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-drag-full-windows",
            Label = "Disable Full Window Drag",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows window outline instead of full content while dragging. Reduces GPU load and improves drag responsiveness. Default: Full window. Recommended: Outline for performance.",
            Tags = ["clipboard", "drag", "window", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFullWindows", "0")],
        },
        new TweakDef
        {
            Id = "clip-max-history-items",
            Label = "Increase Clipboard History Limit",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets maximum clipboard history entries to 50 via policy. Allows storing more copied items in clipboard history. Default: 25 items. Recommended: 50 for productivity.",
            Tags = ["clipboard", "history", "limit", "policy", "productivity"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems", 50)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "MaxClipboardHistoryItems", 50)],
        },
        new TweakDef
        {
            Id = "clip-drag-threshold-medium",
            Label = "Set Drag-Drop Minimum Distance (8 px)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the minimum mouse movement required to initiate a drag-drop operation from 4 px to 8 px. Prevents accidental dragging. Default: 4 px.",
            Tags = ["clipboard", "drag", "drop", "threshold", "mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "8"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "8"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "8")],
        },
        new TweakDef
        {
            Id = "clip-disable-drag-shadow",
            Label = "Disable Drop Shadow on Dragged Items",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets visual effects to custom mode which removes the drop shadow on dragged objects. Improves drag-drop rendering performance on older GPUs. Default: Enabled.",
            Tags = ["clipboard", "drag", "shadow", "visual", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 0)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects", "VisualFXSetting", 2),
            ],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows clipboard history via Group Policy. Only the last copied item is kept. Default: user setting.",
            Tags = ["clipboard", "history", "disable", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-increase-drag-sensitivity",
            Label = "Increase Drag Sensitivity to 10px",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the drag threshold from 4 to 10 pixels. Prevents accidental drag-and-drop. Default: 4 pixels.",
            Tags = ["clipboard", "drag", "sensitivity", "threshold"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "10"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragHeight", "4"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "10")],
        },
        new TweakDef
        {
            Id = "clip-disable-drop-target-hovering",
            Label = "Disable Drop Target Window Activation",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents windows from coming to the foreground when hovering a drag item over a taskbar button. Default: enabled.",
            Tags = ["clipboard", "drag-drop", "window", "activation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragFromMaximize", "0")],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-history-roaming",
            Label = "Disable Clipboard History Roaming",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents clipboard history from roaming across devices signed into the same Microsoft account. Default: user-configurable.",
            Tags = ["clipboard", "history", "roaming", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-set-drag-sensitivity-6",
            Label = "Increase Drag Sensitivity to 6px",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the drag threshold to 6 pixels. Prevents accidental drag when clicking. Default: 4px.",
            Tags = ["clipboard", "drag", "sensitivity", "threshold"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "6")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "4")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "DragWidth", "6")],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-experience",
            Label = "Disable Clipboard Experience UI",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows 10/11 Clipboard Experience feature (Win+V panel). Falls back to traditional clipboard. Default: enabled.",
            Tags = ["clipboard", "experience", "win-v", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardContentParsing", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-cloud-sync",
            Label = "Disable Clipboard Cloud Sync",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard cloud synchronisation across devices via Group Policy. Prevents clipboard data from leaving the device. Default: allowed.",
            Tags = ["clipboard", "cloud", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowCrossDeviceClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-history-policy",
            Label = "Disable Clipboard History (Policy)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Clipboard History via Group Policy. Prevents Windows from storing clipboard entries. Default: allowed.",
            Tags = ["clipboard", "history", "policy", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System", "AllowClipboardHistory", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-roaming",
            Label = "Disable Clipboard Roaming",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables clipboard roaming (syncing clipboard content to other signed-in devices). Keeps clipboard data local. Default: enabled.",
            Tags = ["clipboard", "roaming", "sync", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "CloudClipboardAutomaticUpload", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-suggestions",
            Label = "Disable Clipboard Suggestions",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables clipboard content suggestions and recommended actions. Prevents UI pop-ups when copying content. Default: enabled.",
            Tags = ["clipboard", "suggestions", "actions", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSuggestedClipboardActions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-text-suggestions",
            Label = "Disable Text Suggestions (Input)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables text suggestions and autocomplete for hardware keyboard input. Reduces background processing. Default: enabled.",
            Tags = ["clipboard", "text", "suggestions", "input"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwkbTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "clip-enable-history-user",
            Label = "Enable Clipboard History (User)",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Clipboard History feature at the user level. Allows Win+V to show clipboard history. Default: off.",
            Tags = ["clipboard", "history", "user", "enable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableClipboardHistory", 1)],
        },
        new TweakDef
        {
            Id = "clip-enable-smart-paste",
            Label = "Enable Smart Paste",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Smart Paste feature that intelligently reformats pasted content. Default: off.",
            Tags = ["clipboard", "smart-paste", "formatting", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
        },
        new TweakDef
        {
            Id = "clip-rdp-policy-no-redirect",
            Label = "Disable Clipboard Redirect in RDP",
            Category = "Clipboard & Drag-Drop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard redirection during Remote Desktop sessions. Prevents clipboard data from crossing the RDP boundary. Default: allowed.",
            Tags = ["clipboard", "rdp", "redirect", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
    ];
}
