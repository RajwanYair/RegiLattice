namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ContextMenu
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
