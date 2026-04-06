namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Clipboard.cs ────────────────────────────────────────
internal static class Clipboard
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "clip-increase-drag-threshold",
            Label = "Increase Drag-Drop Threshold (10 px)",
            Category = "System 1",
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
            Id = "clip-instant-drag-delay",
            Label = "Set Instant Drag Delay (0 ms)",
            Category = "System 1",
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
            Category = "System 1",
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
            Id = "clip-disable-clipboard-roaming",
            Label = "Disable Clipboard Roaming (Policy)",
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Id = "clip-disable-clipboard-history",
            Label = "Disable Clipboard History",
            Category = "System 1",
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
            Id = "clip-disable-drop-target-hovering",
            Label = "Disable Drop Target Window Activation",
            Category = "System 1",
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
            Id = "clip-disable-clipboard-experience",
            Label = "Disable Clipboard Experience UI",
            Category = "System 1",
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
            Id = "clip-disable-suggestions",
            Label = "Disable Clipboard Suggestions",
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
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
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the Smart Paste feature that intelligently reformats pasted content. Default: off.",
            Tags = ["clipboard", "smart-paste", "formatting", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Clipboard", "EnableSmartPaste", 1)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "clip-disable-emoji-panel",
            Label = "Disable Emoji Panel",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the emoji panel that appears when pressing Windows+. (period).",
            Tags = ["clipboard", "emoji", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableExpressiveInputShellHotkey", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-sync-across-devices",
            Label = "Disable Clipboard Sync Across Devices",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from syncing clipboard data to other signed-in devices via Microsoft account.",
            Tags = ["clipboard", "privacy", "sync"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "EnableCloudClipboard", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-paste-preview",
            Label = "Disable Clipboard Paste Preview Suggestions",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the paste format suggestion tooltip that appears after pasting content.",
            Tags = ["clipboard", "paste", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "ShowPasteOptions", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-snip-sketch-clipboard-auto",
            Label = "Disable Snip & Sketch Auto-Copy to Clipboard",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Snipping Tool / Snip & Sketch from automatically copying screenshots to the clipboard.",
            Tags = ["clipboard", "snip", "screenshot"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\ScreenshotIndex", "AutoCopyToClipboard", 0),
            ],
        },
        new TweakDef
        {
            Id = "clip-disable-gif-panel",
            Label = "Disable GIF Panel",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the GIF search pane that appears in the emoji picker (Windows+. panel).",
            Tags = ["clipboard", "gif", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableGifSearch", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-clipboard-notifications",
            Label = "Disable Clipboard Copy Notifications",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the toast notification that appears after saving a screenshot with Print Screen.",
            Tags = ["clipboard", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "EnableSnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-sticker-panel",
            Label = "Disable Sticker / Meme Panel",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the sticker and meme categories from the emoji picker panel.",
            Tags = ["clipboard", "stickers", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "EnableStickerPanel", 0)],
        },
        new TweakDef
        {
            Id = "clip-disable-cloud-clipboard-prompt",
            Label = "Disable Cloud Clipboard Activation Prompt",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from asking you to enable cloud clipboard (cross-device sync) in Windows 10/11.",
            Tags = ["clipboard", "cloud", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Clipboard", "SessionWistenessConsent", 1)],
        },
        new TweakDef
        {
            Id = "clip-disable-typing-insights",
            Label = "Disable Typing Insights Collection",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows collecting typing and touch keyboard usage patterns for improvement feedback.",
            Tags = ["clipboard", "privacy", "keyboard", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\TIPC", "Enabled", 0)],
        },
        // ── merged from: ContextMenu.cs ──────────────────────────────────────────────────
        new TweakDef
        {
            Id = "ctx-add-powershell-here",
            Label = "Add 'Open PowerShell Here' to Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds an 'Open PowerShell Here' entry to the directory background context menu for quick terminal access. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "powershell", "terminal", "productivity"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-wt-here",
            Label = "Add 'Open Windows Terminal Here' to Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds 'Open Windows Terminal Here' to the folder background context menu. Opens a new terminal window in the current directory. Default: not present. Recommended: add for developer workflows.",
            Tags = ["context-menu", "terminal", "wt", "developer", "shell"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", "Icon", "wt.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", "Icon", "wt.exe")],
        },
        new TweakDef
        {
            Id = "ctx-add-cmd-here",
            Label = "Add 'Open Command Prompt Here' to Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds 'Open Command Prompt Here' to the folder background context menu. Opens cmd.exe in the current directory. Default: not present. Recommended: add for quick access.",
            Tags = ["context-menu", "cmd", "command-prompt", "shell", "developer"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd\command",
            ],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", "Icon", "cmd.exe")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", "Icon", "cmd.exe")],
        },
        new TweakDef
        {
            Id = "ctx-add-copy-path",
            Label = "Add 'Copy Path' to File Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds a 'Copy Path' option to the right-click menu for all files. Copies the full file path (with quotes) to the clipboard. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "copy-path", "clipboard", "file", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", @"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath\command"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", "Icon", "shell32.dll,-265")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", "Icon", "shell32.dll,-265")],
        },
        new TweakDef
        {
            Id = "ctx-add-open-cmd-here",
            Label = "Add Open Command Prompt Here",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an 'Open Command Prompt Here' option to the folder context menu. Default: removed in Win11.",
            Tags = ["context-menu", "command-prompt", "cmd", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "Icon", "cmd.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here\command", "", "cmd.exe /k cd /d \"%V\""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-share-context-menu",
            Label = "Remove Share Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Share' option from the right-click context menu. Default: visible.",
            Tags = ["context-menu", "share", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access-to",
            Label = "Remove Give Access To Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Give access to' sharing option from the context menu. Default: visible.",
            Tags = ["context-menu", "give-access", "sharing", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compatibility",
            Label = "Remove 'Troubleshoot Compatibility' from Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' option from file context menus. Default: shown.",
            Tags = ["context-menu", "troubleshoot", "compatibility", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-cast-to-device",
            Label = "Remove 'Cast to Device' from Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Cast to device' (Play To) option from the context menu. Default: shown.",
            Tags = ["context-menu", "cast", "device", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-edit-with-paint3d",
            Label = "Remove 'Edit with Paint 3D' from Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Edit with Paint 3D' from the right-click menu for image files. Default: shown.",
            Tags = ["context-menu", "paint3d", "image", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{D2B1A1A9-0B2A-4ED6-B4D7-679E2A5D2B8E}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-in-library",
            Label = "Remove 'Include in Library' from Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Include in library' option from folder context menus. Default: shown.",
            Tags = ["context-menu", "library", "folder", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-previous-versions",
            Label = "Remove 'Restore Previous Versions' from Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Restore previous versions' tab from file/folder properties. Default: shown.",
            Tags = ["context-menu", "previous-versions", "restore", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{596AB062-B4D2-4215-9F74-E9109B0A8153}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-take-ownership",
            Label = "Add 'Take Ownership' to Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Adds a 'Take Ownership' option to the file/folder context menu. Runs takeown + icacls to grant full permissions. Default: not present.",
            Tags = ["context-menu", "take-ownership", "permissions", "security"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership",
                @"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "Icon", "shield.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "HasLUAShield", ""),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" && icacls ""%1"" /grant administrators:F"
                ),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership", "Icon", "shield.exe"),
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" /r /d y && icacls ""%1"" /grant administrators:F /t"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership"),
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\shell\TakeOwnership"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", "", "Take Ownership")],
        },
        new TweakDef
        {
            Id = "ctx-add-open-with-notepad",
            Label = "Add 'Open with Notepad' to Context Menu",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds 'Open with Notepad' to the right-click menu for all files. Quick way to view file contents. Default: not present.",
            Tags = ["context-menu", "notepad", "open-with", "text"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad",
                @"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad\command",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "", "Open with Notepad"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "Icon", "notepad.exe"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad\command", "", @"notepad.exe ""%1"""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\OpenWithNotepad", "", "Open with Notepad")],
        },

        new TweakDef
        {
            Id = "ctx-classic-context-menu",
            Label = "Force Classic Context Menu (User)",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the Windows 10 full context menu by overriding the Windows 11 compact menu CLSID.",
            Tags = ["context-menu", "classic", "win11", "right-click"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-defender-scan",
            Label = "Remove 'Scan with Defender' Context",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the 'Scan with Microsoft Defender' entry from the right-click context menu.",
            Tags = ["context-menu", "defender", "security", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\CLSID\{09A47860-11B0-4DA5-AFA5-26D86198A780}\InprocServer32", "", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access",
            Label = "Remove 'Give Access To' Context",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Give access to' sharing menu from the right-click context menu.",
            Tags = ["context-menu", "give-access", "sharing", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing",
                    "",
                    "{f81e9010-6ea4-11ce-a7ff-00aa003ca9f6}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shellex\ContextMenuHandlers\Sharing", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-library",
            Label = "Remove 'Include in Library' Context",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Include in library' entry from the folder context menu.",
            Tags = ["context-menu", "library", "folder", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location",
                    "",
                    "{3dad6c5d-2167-4cae-9914-f99e41c12cfa}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\Library Location", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-paint3d-edit",
            Label = "Remove 'Edit with Paint 3D' Context",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Edit with Paint 3D' option from image file context menus.",
            Tags = ["context-menu", "paint-3d", "image", "clean"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.jpg\Shell\3D Edit", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.png\Shell\3D Edit", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\.bmp\Shell\3D Edit", "ProgrammaticAccessOnly", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-photos-edit",
            Label = "Remove 'Edit with Photos' Context",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Edit with Photos' option from image file context menus.",
            Tags = ["context-menu", "photos", "image", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AppX43hnxtbyyps62jhe9sqpdzxn1790zetc\Shell\ShellEdit",
                    "ProgrammaticAccessOnly",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-pin-to-start",
            Label = "Remove 'Pin to Start' Context Entry",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Pin to Start' context menu option from files and folders.",
            Tags = ["context-menu", "pin", "start-menu", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen",
                    "",
                    "{470C0EBD-5D73-4d58-9CED-E91E22E23282}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Folder\shellex\ContextMenuHandlers\PintoStartScreen", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-print",
            Label = "Remove 'Print' Context Menu Entry",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Print' entry from the right-click context menu for supported file types.",
            Tags = ["context-menu", "print", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\image\shell\print", "ProgrammaticAccessOnly", ""),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-send-to",
            Label = "Remove 'Send To' Context Menu",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Send to' cascading menu from the right-click context menu.",
            Tags = ["context-menu", "send-to", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo",
                    "",
                    "{7BA4C740-9E81-11CF-99D3-00AA004AE837}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\AllFilesystemObjects\shellex\ContextMenuHandlers\SendTo", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-share",
            Label = "Remove 'Share' Context Menu Entry",
            Category = "System 1",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Share' modern sharing entry from the right-click context menu.",
            Tags = ["context-menu", "share", "modern", "clean"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\*\shellex\ContextMenuHandlers\ModernSharing", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compat",
            Label = "Remove 'Troubleshoot Compatibility'",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' entry from the context menu for executables.",
            Tags = ["context-menu", "troubleshoot", "compatibility", "clean"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility", "", "")],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility",
                    "",
                    "{1d27f844-3a1f-4410-85ac-14651078412d}"
                ),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\exefile\shellex\ContextMenuHandlers\Compatibility", "", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-wmp-context",
            Label = "Remove Windows Media Context Entries",
            Category = "System 1",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes Windows Media Player context menu entries (Play, Add to playlist, etc.).",
            Tags = ["context-menu", "media-player", "wmp", "clean"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly", ""),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play", "ProgrammaticAccessOnly", ""),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Play", "ProgrammaticAccessOnly"),
            ],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\SystemFileAssociations\audio\shell\Enqueue", "ProgrammaticAccessOnly", ""),
            ],
        },
    ];
}
