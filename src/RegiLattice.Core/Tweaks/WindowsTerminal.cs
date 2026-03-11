namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsTerminal
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "term-enable-console-v2",
            Label = "Enable Console V2 Host",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces the new Console V2 host with ANSI support, line wrapping, and improved rendering. Default: 1 (enabled). Recommended: 1.",
            Tags = ["terminal", "console", "v2", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-vt-processing",
            Label = "Enable Virtual Terminal (ANSI) Processing",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables VT100/ANSI escape sequence processing in the console. Required for colored output in many CLI tools. Default: 0 (off). Recommended: 1 (on).",
            Tags = ["terminal", "vt100", "ansi", "colors", "escape"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Quick Edit so clicking the console window does not pause running commands. Prevents accidental hangs. Default: 1 (on). Recommended: 0 (off).",
            Tags = ["terminal", "quickedit", "console", "hang"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-insert-mode",
            Label = "Enable Insert Mode by Default",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets insert mode as the default typing mode in consoles. Default: 1 (insert). Recommended: 1.",
            Tags = ["terminal", "insert", "mode", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-line-wrap",
            Label = "Enable Line Wrapping",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic line wrapping when resizing the console. Default: 1. Recommended: 1.",
            Tags = ["terminal", "wrap", "resize", "console"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-legacy-console",
            Label = "Disable Legacy Console Mode",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the legacy console subsystem. Required for Console V2 features like ANSI escape support. Default: 0 (modern). Recommended: 0.",
            Tags = ["terminal", "legacy", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
        },
        new TweakDef
        {
            Id = "term-console-font-cascadia",
            Label = "Set Console Font to Cascadia Mono",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default console font to Cascadia Mono at 16pt. Bundled with Windows Terminal; supports ligatures. Default: Consolas 14pt. Recommended: Cascadia Mono 16pt.",
            Tags = ["terminal", "font", "cascadia", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontFamily", 54),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Console", "FaceName", "Consolas"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontFamily", 54),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console", "FaceName", "Cascadia Mono")],
        },
        new TweakDef
        {
            Id = "term-enable-ctrl-cv",
            Label = "Enable Ctrl+Shift+C/V Copy-Paste",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Ctrl+Shift+C / Ctrl+Shift+V clipboard shortcuts in the classic console. Also enables filter-on-paste and line selection. Default: enabled. Recommended: enabled.",
            Tags = ["terminal", "copy", "paste", "keyboard", "shortcuts"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FilterOnPaste", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineSelection", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FilterOnPaste"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "LineSelection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 0)],
        },
        new TweakDef
        {
            Id = "term-set-window-opacity",
            Label = "Set Console Window Opacity (95%)",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets console window to 95% opacity for slight transparency. Default: 255 (opaque). Recommended: 242 (95%).",
            Tags = ["terminal", "opacity", "transparency", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
        },
        new TweakDef
        {
            Id = "term-set-default-wt",
            Label = "Set Default Terminal to Windows Terminal (Win11)",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows Terminal as the default terminal via the Win11 UseNewTerminal setting. Default: Let Windows decide (0). Recommended: Windows Terminal (1).",
            Tags = ["terminal", "default", "windows-terminal", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationTerminal"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-splash",
            Label = "Disable Terminal Splash Screen",
            Category = "Windows Terminal",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Windows Terminal splash/startup screen via policy. Default: Enabled. Recommended: Disabled for faster launch.",
            Tags = ["terminal", "splash", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
        },
        new TweakDef
        {
            Id = "term-set-default-terminal-wt",
            Label = "Set Windows Terminal as Default Console App",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows Terminal as the default terminal application for all console programs. Default: Windows Console Host.",
            Tags = ["terminal", "default", "console", "wt"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", "{2EACA947-7F5F-4CFA-BA87-8F7FBEEFBE69}")],
        },
        new TweakDef
        {
            Id = "term-enable-acrylic-background",
            Label = "Enable Terminal Acrylic Background via Policy",
            Category = "Windows Terminal",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables acrylic (translucent) background in Windows Terminal via machine policy. Default: disabled.",
            Tags = ["terminal", "acrylic", "background", "appearance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "UseAcrylic", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-bell",
            Label = "Disable Terminal Bell Sound via Policy",
            Category = "Windows Terminal",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the bell (beep) sound in Windows Terminal. Default: enabled.",
            Tags = ["terminal", "bell", "beep", "sound"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "BellStyle", 0)],
        },
        new TweakDef
        {
            Id = "term-set-default-profile-pwsh",
            Label = "Set Default Shell to PowerShell 7 via Policy",
            Category = "Windows Terminal",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default shell profile in Windows Terminal to PowerShell 7 via machine policy. Default: Windows PowerShell 5.1.",
            Tags = ["terminal", "default", "powershell", "profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile", "{574e775e-4f2a-5b96-ac1e-a2962a402336}")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile", "{574e775e-4f2a-5b96-ac1e-a2962a402336}")],
        },
        new TweakDef
        {
            Id = "term-disable-automatic-updates",
            Label = "Disable Windows Terminal Auto-Update",
            Category = "Windows Terminal",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic updates for Windows Terminal. Use winget or manual update instead. Default: auto-update.",
            Tags = ["terminal", "update", "auto", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "term-campbell-color-scheme",
            Label = "Set Windows Terminal Campbell Theme",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the Windows Terminal default color scheme to Campbell. Provides a consistent dark theme across profiles. Default: system default.",
            Tags = ["terminal", "campbell", "theme", "color"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ColorTable00")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ColorTable00", unchecked((int)0x0C0C0C))],
        },
        new TweakDef
        {
            Id = "term-default-windows-terminal",
            Label = "Set Windows Terminal as Default Console",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Windows Terminal as the default console host instead of legacy conhost.exe. Enables modern features and tabs. Default: conhost.",
            Tags = ["terminal", "default", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console\%%Startup"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console\%%Startup", "DelegationConsole", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-always-on-top",
            Label = "Enable Terminal Always On Top",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables always-on-top mode for console windows. Terminal stays above other windows. Useful for monitoring output. Default: disabled.",
            Tags = ["terminal", "always-on-top", "window", "pin"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "AlwaysOnTop", 1)],
        },
        new TweakDef
        {
            Id = "term-large-buffer",
            Label = "Set Terminal Large Scrollback Buffer",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the console scrollback buffer to 9999 lines. Allows reviewing more terminal output history. Default: 300 lines.",
            Tags = ["terminal", "buffer", "scrollback", "history"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 300)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 9999)],
        },
        new TweakDef
        {
            Id = "term-set-cursor-block",
            Label = "Set Terminal Block Cursor",
            Category = "Windows Terminal",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console cursor shape to a solid block. More visible than the default underscore cursor. Default: underscore.",
            Tags = ["terminal", "cursor", "block", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CursorType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
        },
    ];
}
