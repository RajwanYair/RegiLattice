namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Shell
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shell-disable-recent-files",
            Label = "Disable Recent Files in Quick Access",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents recently opened files from appearing in Quick Access.",
            Tags = ["shell", "explorer", "privacy", "recent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0)],
        },
        new TweakDef
        {
            Id = "shell-disable-frequent-folders",
            Label = "Disable Frequent Folders in Quick Access",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents frequently used folders from appearing in Quick Access.",
            Tags = ["shell", "explorer", "privacy", "frequent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0)],
        },
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
            Id = "shell-show-file-extensions",
            Label = "Show File Extensions in Explorer",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Displays file extensions (e.g. .txt, .exe) in File Explorer.",
            Tags = ["shell", "explorer", "extensions", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)],
        },
        new TweakDef
        {
            Id = "shell-show-hidden-files",
            Label = "Show Hidden Files in Explorer",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Shows hidden files and folders in File Explorer.",
            Tags = ["shell", "explorer", "hidden", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-aero-shake",
            Label = "Disable Aero Shake",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Aero Shake (shaking a window to minimize others). Prevents accidental minimization. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "aero", "shake", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-snap-flyout",
            Label = "Disable Snap Assist Flyout",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Snap Assist suggestion flyout when snapping windows. Windows still snap but without the layout suggestion popup. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["shell", "snap", "flyout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "shell-disable-shake-minimize",
            Label = "Disable Shake to Minimize",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Aero Shake gesture that minimizes all other windows. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "shake", "minimize", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-snap-assist",
            Label = "Disable Snap Assist Suggestions",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Snap Assist window arrangement suggestions. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["shell", "snap", "assist", "multitasking"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
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
            Id = "shell-disable-autoplay",
            Label = "Disable AutoPlay for All Drives",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables AutoPlay for all drive types. Prevents automatic execution of media and programs when removable drives are inserted. Default: enabled.",
            Tags = ["shell", "autoplay", "security", "usb"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", "DisableAutoplay", 1),
            ],
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
    ];
}
