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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowRecent", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer", "ShowFrequent", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseCompactMode", 0),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 2),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "Hidden", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-aero-shake",
            Label = "Disable Aero Shake",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Aero Shake (shaking a window to minimize others). Prevents accidental minimization. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "aero", "shake", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1)],
        },
        new TweakDef
        {
            Id = "shell-disable-snap-flyout",
            Label = "Disable Snap Assist Flyout",
            Category = "Shell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Snap Assist suggestion flyout when snapping windows. Windows still snap but without the layout suggestion popup. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["shell", "snap", "flyout", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "DisallowShaking"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "SnapAssist", 0)],
        },
        new TweakDef
        {
            Id = "shell-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Shell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Ink Workspace (pen/touch drawing overlay). Frees resources on non-touch devices. Default: Enabled. Recommended: Disabled.",
            Tags = ["shell", "ink", "workspace", "pen"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace"),
            ],
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
            Description = "Sets the machine-level PowerShell execution policy to RemoteSigned. Local scripts run freely; downloaded scripts need a signature. Default: Restricted.",
            Tags = ["shell", "powershell", "execution-policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", "RemoteSigned")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", "Restricted")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1\ShellIds\Microsoft.PowerShell", "ExecutionPolicy", "RemoteSigned")],
        },
    ];
}
