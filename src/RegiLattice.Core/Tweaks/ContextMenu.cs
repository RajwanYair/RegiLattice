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

// === Merged from: Shell.cs ===


internal static class Shell
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [

        new TweakDef
        {
            Id = "shell-compact-file-explorer",
            Label = "Enable Compact View in File Explorer",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables the compact layout in File Explorer, reducing padding between items.",
            Tags = ["shell", "explorer", "compact", "layout"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1)],
        },

        new TweakDef
        {
            Id = "shell-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Ink Workspace (pen/touch drawing overlay). Frees resources on non-touch devices. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "ink", "workspace", "pen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "shell-enable-dark-mode-console",
            Label = "Enable Dark Mode for Console Windows",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables dark mode for legacy console host windows. Default: light.",
            Tags = ["shell", "console", "dark-mode", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ForceV2")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "shell-set-console-buffer-9999",
            Label = "Set Console Screen Buffer to 9999 Lines",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console screen buffer size to 9999 lines. More scrollback history. Default: 300.",
            Tags = ["shell", "console", "buffer", "scrollback"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 655279999)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 19660920)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ScreenBufferSize", 655279999)],
        },
        new TweakDef
        {
            Id = "shell-enable-quickedit-mode",
            Label = "Enable Console QuickEdit Mode",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables QuickEdit mode in console windows. Allows mouse text selection. Default: enabled.",
            Tags = ["shell", "console", "quickedit", "mouse"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-cmd-autorun",
            Label = "Disable CMD AutoRun Commands",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Clears the CMD AutoRun registry value. Prevents potentially malicious auto-execution. Default: not set.",
            Tags = ["shell", "cmd", "autorun", "security"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Software\Microsoft\Command Processor", "AutoRun", "")],
        },
        new TweakDef
        {
            Id = "shell-set-powershell-execution-remotesigned",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the machine-level PowerShell execution policy to RemoteSigned. Local scripts run freely; downloaded scripts need a signature. Default: Restricted.",
            Tags = ["shell", "powershell", "execution-policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", "Restricted"),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell",
                    "ExecutionPolicy",
                    "RemoteSigned"
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-add-python-to-path",
            Label = "Add Python App Installer to PATH",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Adds the WindowsApps Python alias directory to the user PATH. Enables running 'python' from any terminal without the Store alias redirect. Default: not in PATH.",
            Tags = ["shell", "python", "path", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetExpandString(@"HKEY_CURRENT_USER\Environment", "Path", @"%LOCALAPPDATA%\Microsoft\WindowsApps;%PATH%")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "Path")],
            DetectAction = () =>
            {
                var path = Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.User) ?? "";
                return path.Contains("WindowsApps", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "shell-classic-context-menu",
            Label = "Restore Classic Context Menu (Shell)",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Restores the classic full right-click context menu by overriding the Windows 11 modern context menu via shell registry key. Default: modern menu.",
            Tags = ["shell", "context-menu", "classic", "win11"],
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
            Id = "shell-cmd-autocomplete",
            Label = "Enable Command Prompt AutoComplete",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Tab-key autocomplete in Command Prompt (cmd.exe). Sets CompletionChar and PathCompletionChar to Tab (0x9). Default: disabled.",
            Tags = ["shell", "cmd", "autocomplete", "tab"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x9),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "PathCompletionChar", 0x9),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x40),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "PathCompletionChar", 0x40),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Command Processor", "CompletionChar", 0x9)],
        },
        new TweakDef
        {
            Id = "shell-disable-python-store-alias",
            Label = "Disable Python Store Redirect Alias",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows Store app execution alias for 'python.exe' and 'python3.exe'. Prevents the Store redirect when Python is already installed. Default: enabled.",
            Tags = ["shell", "python", "store", "alias"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\python.exe"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppXdfn65rtf6m01bfv5r26cj8xtf0jclb0p",
                    "Disabled",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled"
                ),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppXdfn65rtf6m01bfv5r26cj8xtf0jclb0p",
                    "Disabled"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\PackageRepository\Extensions\ProgIDs\AppX9rkaq77s0jzh1tyccadx9ghba15r6t3h",
                    "Disabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-file-hash-context",
            Label = "Add File Hash to Context Menu",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds a 'Calculate File Hash' option to the right-click context menu using PowerShell's Get-FileHash. Default: not available.",
            Tags = ["shell", "hash", "context-menu", "powershell"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "", "Calculate File Hash"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "Icon", "shell32.dll,23"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash\command",
                    "",
                    @"powershell.exe -NoProfile -Command ""Get-FileHash -Algorithm SHA256 '%1' | Format-List; pause"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\GetFileHash", "", "Calculate File Hash")],
        },
        new TweakDef
        {
            Id = "shell-open-cmd-here",
            Label = "Add 'Open CMD Here' to Context Menu",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds 'Open Command Prompt Here' to the directory background context menu. Default: not available.",
            Tags = ["shell", "cmd", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "", "Open Command Prompt Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "Icon", "cmd.exe"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere\command",
                    "",
                    @"cmd.exe /k cd /d ""%V"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenCmdHere", "", "Open Command Prompt Here"),
            ],
        },
        new TweakDef
        {
            Id = "shell-open-ps-here",
            Label = "Add 'Open PowerShell Here' to Context Menu",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Adds 'Open PowerShell Here' to the directory background context menu. Default: not available.",
            Tags = ["shell", "powershell", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "", "Open PowerShell Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "Icon", "powershell.exe"),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere\command",
                    "",
                    @"powershell.exe -NoExit -Command ""Set-Location '%V'"""
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenPSHere", "", "Open PowerShell Here")],
        },
        new TweakDef
        {
            Id = "shell-open-wt-here",
            Label = "Add 'Open Windows Terminal Here'",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds 'Open Windows Terminal Here' to the directory background context menu. Requires Windows Terminal to be installed. Default: not available.",
            Tags = ["shell", "terminal", "context-menu", "directory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "", "Open Windows Terminal Here"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "Icon", "wt.exe"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere\command", "", @"wt.exe -d ""%V"""),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere")],
            DetectOps =
            [
                RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Directory\Background\shell\OpenWTHere", "", "Open Windows Terminal Here"),
            ],
        },
        new TweakDef
        {
            Id = "shell-take-ownership",
            Label = "Add 'Take Ownership' to Context Menu",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds a 'Take Ownership' option to the right-click context menu for files and folders. Uses takeown and icacls to reclaim NTFS permissions. Default: not available.",
            Tags = ["shell", "ownership", "context-menu", "permissions"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "", "Take Ownership"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "Icon", "imageres.dll,101"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "HasLUAShield", ""),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership\command",
                    "",
                    @"cmd.exe /c takeown /f ""%1"" && icacls ""%1"" /grant administrators:F"
                ),
            ],
            RemoveOps = [RegOp.DeleteTree(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Classes\*\shell\TakeOwnership", "", "Take Ownership")],
        },
        new TweakDef
        {
            Id = "shell-disable-thumbnail-net-cache",
            Label = "Disable Thumbs.db on Network Folders",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets DisableThumbsDBOnNetworkFolders=1 in Explorer Advanced. Stops Windows from creating Thumbs.db thumbnail cache files on UNC and mapped network drives, which can cause file-locking issues for other users.",
            Tags = ["shell", "explorer", "thumbnail", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced",
                    "DisableThumbsDBOnNetworkFolders",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "shell-enable-numlock-startup",
            Label = "Enable Num Lock on Startup",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets InitialKeyboardIndicators=2 in the Keyboard control panel key. Ensures Num Lock is enabled each time the desktop session starts.",
            Tags = ["shell", "keyboard", "numlock", "startup"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "InitialKeyboardIndicators", "2")],
        },
        new TweakDef
        {
            Id = "shell-disable-folder-info-tips",
            Label = "Disable Folder Info Tips in Explorer",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FolderContentsInfoTip=0 in Explorer Advanced. Removes the tooltip that appears when hovering over a folder showing its file count and size — reduces Explorer popups.",
            Tags = ["shell", "explorer", "tooltip", "info"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "FolderContentsInfoTip", 0),
            ],
        },
        new TweakDef
        {
            Id = "shell-disable-sharing-wizard",
            Label = "Disable File Sharing Wizard",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SharingWizardOn=0 in Explorer Advanced. Removes the simplified sharing wizard from context menus, giving direct access to advanced sharing permissions without the guided wizard.",
            Tags = ["shell", "explorer", "sharing", "network"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SharingWizardOn", 0)],
        },

        new TweakDef
        {
            Id = "shell-restore-previous-folders",
            Label = "Reopen Previous Folder Windows on Login",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PersistBrowsers=1 in Explorer Advanced. Restores all previously open Explorer folder windows after a reboot or sign-in, resuming your workspace automatically.",
            Tags = ["shell", "explorer", "folders", "restore"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "PersistBrowsers", 1)],
        },
        new TweakDef
        {
            Id = "shell-show-encrypted-color",
            Label = "Show Encrypted & Compressed Files in Color",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowEncryptCompressedColor=1 in Explorer Advanced. Displays encrypted files in green and NTFS-compressed files in blue in Explorer, making protected content visually distinct.",
            Tags = ["shell", "explorer", "encryption", "color"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowEncryptCompressedColor", 1),
            ],
        },

        new TweakDef
        {
            Id = "shell-disable-recent-docs-policy",
            Label = "Disable Recent Documents Tracking (Policy)",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoRecentDocsHistory=1 in Explorer policies. Prevents Windows from recording history of recently opened files in the shell across all users on the machine via Group Policy.",
            Tags = ["shell", "privacy", "recent-docs", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoRecentDocsHistory", 1),
            ],
        },
        new TweakDef
        {
            Id = "shell-show-status-bar",
            Label = "Show Status Bar in Explorer Windows",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ShowStatusBar=1 in Explorer Advanced. Enables the bottom status bar in Explorer windows that shows selected item counts, sizes, and folder details.",
            Tags = ["shell", "explorer", "status-bar", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ShowStatusBar", 1)],
        },
    ];
}
