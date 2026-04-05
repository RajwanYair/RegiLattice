namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// === Merged from: Shell.cs ===

internal static class Shell
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "shell-set-console-buffer-9999",
            Label = "Set Console Screen Buffer to 9999 Lines",
            Category = "System 1",
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
            Id = "shell-disable-cmd-autorun",
            Label = "Disable CMD AutoRun Commands",
            Category = "System 1",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Category = "System 2",
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
            Id = "shell-enable-numlock-startup",
            Label = "Enable Num Lock on Startup",
            Category = "System 2",
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
            Id = "shell-restore-previous-folders",
            Label = "Reopen Previous Folder Windows on Login",
            Category = "System 2",
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
            Id = "shell-disable-recent-docs-policy",
            Label = "Disable Recent Documents Tracking (Policy)",
            Category = "System 2",
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
    ];
}
