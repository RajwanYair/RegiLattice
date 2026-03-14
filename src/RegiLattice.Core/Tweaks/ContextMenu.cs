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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Classes\Directory\Background\shell\cmd_here", "", "Open Command Prompt Here"),
            ],
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Id = "ctx-remove-send-to-compressed",
            Label = "Remove 'Compressed (zipped) folder' from Send To",
            Category = "Context Menu",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the obsolete 'Compressed (zipped) folder' from the Send To menu. Use 7-Zip instead. Default: shown.",
            Tags = ["context-menu", "send-to", "zip", "remove"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "ctx-remove-previous-versions",
            Label = "Remove 'Restore Previous Versions' from Context Menu",
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
        // ── Restored stubs with real registry operations ──────────────────

        new TweakDef
        {
            Id = "ctx-classic-context-menu",
            Label = "Force Classic Context Menu (User)",
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
            Category = "Context Menu",
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
