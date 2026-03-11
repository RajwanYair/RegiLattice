namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ContextMenu
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ctx-classic-context-menu",
            Label = "Restore Classic Context Menu (Win11)",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Restores the full Windows 10 right-click context menu in Windows 11. Removes the truncated 'Show more options' menu. Default: Win11 menu. Recommended: classic.",
            Tags = ["context-menu", "win11", "classic", "right-click"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\CLSID\{86ca1aa0-34aa-4e8b-a509-50c905bae2a2}\InprocServer32"],
        },
        new TweakDef
        {
            Id = "ctx-remove-share",
            Label = "Remove 'Share' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks the Share shell extension from appearing in the context menu. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "share", "shell-extension", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-cast-to-device",
            Label = "Remove 'Cast to Device' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Cast to Device' (Play To) from the right-click menu. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "cast", "miracast", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access",
            Label = "Remove 'Give Access To' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Give access to' (Share with) from the context menu. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "give-access", "share", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-in-library",
            Label = "Remove 'Include in Library' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Include in library' from the folder context menu. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "library", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-paint3d-edit",
            Label = "Remove 'Edit with Paint 3D' from Images",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Edit with Paint 3D' from .bmp, .jpg, .png context menus. Uses ProgrammaticAccessOnly flag. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "paint3d", "images", "cleanup"],
            RegistryKeys = [@"HKEY_CLASSES_ROOT\SystemFileAssociations\.bmp\Shell\3D Edit", @"HKEY_CLASSES_ROOT\SystemFileAssociations\.jpg\Shell\3D Edit", @"HKEY_CLASSES_ROOT\SystemFileAssociations\.png\Shell\3D Edit"],
            RemoveOps =
            [
                RegOp.DeleteValue(@"key", "ProgrammaticAccessOnly"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-photos-edit",
            Label = "Remove 'Edit with Photos' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Edit' option added by the Photos app from image context menus. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "photos", "edit", "images", "cleanup"],
            RegistryKeys = [@"HKEY_CLASSES_ROOT\AppX43ztkmn2e2q6vhzjqeps9v44v72r52m3\Shell\ShellEdit"],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CLASSES_ROOT\AppX43ztkmn2e2q6vhzjqeps9v44v72r52m3\Shell\ShellEdit", "ProgrammaticAccessOnly"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compat",
            Label = "Remove 'Troubleshoot Compatibility'",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' option from executable context menus. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "compatibility", "troubleshoot", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-previous-versions",
            Label = "Remove 'Restore Previous Versions'",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Restore previous versions' from file/folder context menus. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "previous-versions", "shadow-copy", "cleanup"],
            RegistryKeys = [@"HKEY_CLASSES_ROOT\AllFilesystemObjects\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}", @"HKEY_CLASSES_ROOT\CLSID\{450D8FBA-AD25-11D0-98A8-0800361B1103}\shellex\ContextMenuHandlers\{596AB062-B4D2-4215-9F74-E9109B0A8153}"],
        },
        new TweakDef
        {
            Id = "ctx-remove-pin-to-start",
            Label = "Remove 'Pin to Start' from Folders",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes 'Pin to Start' from the folder right-click menu. Default: shown. Recommended: hidden.",
            Tags = ["context-menu", "pin", "start", "folder", "cleanup"],
            RegistryKeys = [@"HKEY_CLASSES_ROOT\Folder\shellex\ContextMenuHandlers\PintoStartScreen"],
        },
        new TweakDef
        {
            Id = "ctx-add-take-ownership",
            Label = "Add 'Take Ownership' to Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds a 'Take Ownership' option to the right-click context menu. Runs takeown and icacls to grant full control. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "ownership", "takeown", "permissions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership", @"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership\command"],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\TakeOwnership"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-include-library",
            Label = "Remove 'Include in Library' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the 'Include in Library' option from the folder context menu by blocking its shell extension CLSID. Default: shown. Recommended: hidden if unused.",
            Tags = ["context-menu", "library", "include", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-send-to",
            Label = "Remove 'Send to' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the 'Send to' cascading menu from the right-click context menu by blocking its shell extension CLSID. Default: shown. Recommended: hidden if unused.",
            Tags = ["context-menu", "send-to", "cleanup", "shell"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-print",
            Label = "Remove 'Print' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Removes the 'Print' option from the right-click context menu by blocking its shell extension CLSID. Default: shown. Recommended: hidden if no printer is used.",
            Tags = ["context-menu", "print", "cleanup", "shell"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-add-powershell-here",
            Label = "Add 'Open PowerShell Here' to Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds an 'Open PowerShell Here' entry to the directory background context menu for quick terminal access. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "powershell", "terminal", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere\command"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere", "Icon", "powershell.exe"),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\PowerShellHere"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-wt-here",
            Label = "Add 'Open Windows Terminal Here' to Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds 'Open Windows Terminal Here' to the folder background context menu. Opens a new terminal window in the current directory. Default: not present. Recommended: add for developer workflows.",
            Tags = ["context-menu", "terminal", "wt", "developer", "shell"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt\command"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt", "Icon", "wt.exe"),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\wt"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-cmd-here",
            Label = "Add 'Open Command Prompt Here' to Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds 'Open Command Prompt Here' to the folder background context menu. Opens cmd.exe in the current directory. Default: not present. Recommended: add for quick access.",
            Tags = ["context-menu", "cmd", "command-prompt", "shell", "developer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", @"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd\command"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd", "Icon", "cmd.exe"),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-remove-defender-scan",
            Label = "Remove 'Scan with Microsoft Defender' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks the Windows Defender context menu shell extension. Files can still be scanned via Security Center. Default: shown. Recommended: hidden if context menu is cluttered.",
            Tags = ["context-menu", "defender", "antivirus", "scan", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-remove-wmp-context",
            Label = "Remove Windows Media Player from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks the Windows Media Player shell extension from the context menu. Removes 'Play with Windows Media Player' from media file menus. Default: shown. Recommended: hidden if WMP is unused.",
            Tags = ["context-menu", "wmp", "media-player", "cleanup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
        },
        new TweakDef
        {
            Id = "ctx-add-copy-path",
            Label = "Add 'Copy Path' to File Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Adds a 'Copy Path' option to the right-click menu for all files. Copies the full file path (with quotes) to the clipboard. Default: not present. Recommended: add for power users.",
            Tags = ["context-menu", "copy-path", "clipboard", "file", "productivity"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", @"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath\command"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath", "Icon", "shell32.dll,-265"),
            ],
            RemoveOps =
            [
                RegOp.DeleteTree(@"HKEY_CURRENT_USER\Software\Classes\*\shell\CopyPath"),
            ],
        },
        new TweakDef
        {
            Id = "ctx-add-open-cmd-here",
            Label = "Add Open Command Prompt Here",
            Category = "Context Menu",
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
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here")],
        },
        new TweakDef
        {
            Id = "ctx-remove-share-context-menu",
            Label = "Remove Share Context Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Share' option from the right-click context menu. Default: visible.",
            Tags = ["context-menu", "share", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{E2BF9676-5F8F-435C-97EB-11607A5BEDF7}", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-give-access-to",
            Label = "Remove Give Access To Menu",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the 'Give access to' sharing option from the context menu. Default: visible.",
            Tags = ["context-menu", "give-access", "sharing", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{F81E9010-6EA4-11CE-A7FF-00AA003CA9F6}", "")],
        },
        new TweakDef
        {
            Id = "ctx-remove-troubleshoot-compatibility",
            Label = "Remove 'Troubleshoot Compatibility' from Context Menu",
            Category = "Context Menu",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Troubleshoot compatibility' option from file context menus. Default: shown.",
            Tags = ["context-menu", "troubleshoot", "compatibility", "remove"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{1d27f844-3a1f-4410-85ac-14651078412d}", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{1d27f844-3a1f-4410-85ac-14651078412d}")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked", "{1d27f844-3a1f-4410-85ac-14651078412d}", "")],
        },
    ];
}
