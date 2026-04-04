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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces the new Console V2 host with ANSI support, line wrapping, and improved rendering. Default: 1 (enabled). Recommended: 1.",
            Tags = ["terminal", "console", "v2", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForceV2", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-vt-processing",
            Label = "Enable Virtual Terminal (ANSI) Processing",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables VT100/ANSI escape sequence processing in the console. Required for colored output in many CLI tools. Default: 0 (off). Recommended: 1 (on).",
            Tags = ["terminal", "vt100", "ansi", "colors", "escape"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "VirtualTerminalLevel", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Quick Edit so clicking the console window does not pause running commands. Prevents accidental hangs. Default: 1 (on). Recommended: 0 (off).",
            Tags = ["terminal", "quickedit", "console", "hang"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-insert-mode",
            Label = "Enable Insert Mode by Default",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets insert mode as the default typing mode in consoles. Default: 1 (insert). Recommended: 1.",
            Tags = ["terminal", "insert", "mode", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-line-wrap",
            Label = "Enable Line Wrapping",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables automatic line wrapping when resizing the console. Default: 1. Recommended: 1.",
            Tags = ["terminal", "wrap", "resize", "console"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-disable-legacy-console",
            Label = "Disable Legacy Console Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the legacy console subsystem. Required for Console V2 features like ANSI escape support. Default: 0 (modern). Recommended: 0.",
            Tags = ["terminal", "legacy", "console", "modern"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "UseLegacyConsole", 0)],
        },
        new TweakDef
        {
            Id = "term-console-font-cascadia",
            Label = "Set Console Font to Cascadia Mono",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default console font to Cascadia Mono at 16pt. Bundled with Windows Terminal; supports ligatures. Default: Consolas 14pt. Recommended: Cascadia Mono 16pt.",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables Ctrl+Shift+C / Ctrl+Shift+V clipboard shortcuts in the classic console. Also enables filter-on-paste and line selection. Default: enabled. Recommended: enabled.",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets console window to 95% opacity for slight transparency. Default: 255 (opaque). Recommended: 242 (95%).",
            Tags = ["terminal", "opacity", "transparency", "appearance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 242)],
        },
        new TweakDef
        {
            Id = "term-set-default-wt",
            Label = "Set Default Terminal to Windows Terminal (Win11)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default terminal via the Win11 UseNewTerminal setting. Default: Let Windows decide (0). Recommended: Windows Terminal (1).",
            Tags = ["terminal", "default", "windows-terminal", "win11"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "UseNewTerminal", 1)],
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
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Terminal splash/startup screen via policy. Default: Enabled. Recommended: Disabled for faster launch.",
            Tags = ["terminal", "splash", "startup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DisableSplashScreen", 1)],
        },
        new TweakDef
        {
            Id = "term-set-default-terminal-wt",
            Label = "Set Windows Terminal as Default Console App",
            Category = "PowerShell",
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
            Category = "PowerShell",
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
            Category = "PowerShell",
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
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default shell profile in Windows Terminal to PowerShell 7 via machine policy. Default: Windows PowerShell 5.1.",
            Tags = ["terminal", "default", "powershell", "profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal", "DefaultProfile")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal",
                    "DefaultProfile",
                    "{574e775e-4f2a-5b96-ac1e-a2962a402336}"
                ),
            ],
        },
        new TweakDef
        {
            Id = "term-disable-automatic-updates",
            Label = "Disable Windows Terminal Auto-Update",
            Category = "PowerShell",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Windows Terminal default color scheme to Campbell. Provides a consistent dark theme across profiles. Default: system default.",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets Windows Terminal as the default console host instead of legacy conhost.exe. Enables modern features and tabs. Default: conhost.",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables always-on-top mode for console windows. Terminal stays above other windows. Useful for monitoring output. Default: disabled.",
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
            Category = "PowerShell",
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
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the console cursor shape to a solid block. More visible than the default underscore cursor. Default: underscore.",
            Tags = ["terminal", "cursor", "block", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CursorType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CursorType", 2)],
        },
        new TweakDef
        {
            Id = "term-disable-console-quick-edit",
            Label = "Disable Quick Edit Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables Quick Edit mode in the console, preventing accidental selection pauses. Default: enabled.",
            Tags = ["console", "quick-edit", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "QuickEdit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "QuickEdit", 0)],
        },
        new TweakDef
        {
            Id = "term-enable-resize-line-wrap",
            Label = "Enable Line Wrap on Resize",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Enables line wrapping when the console window is resized. Default: disabled (truncate).",
            Tags = ["console", "line-wrap", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "LineWrap")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "LineWrap", 1)],
        },
        new TweakDef
        {
            Id = "term-set-font-weight-bold",
            Label = "Set Console Font Weight to Bold",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the console font weight to bold (700). Improves readability on high-DPI displays. Default: normal (400).",
            Tags = ["console", "font", "bold", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "FontWeight")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "FontWeight", 700)],
        },
        new TweakDef
        {
            Id = "term-disable-scroll-forward",
            Label = "Disable Forward Scrolling",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables the ability to scroll forward past the current output. Default: enabled.",
            Tags = ["console", "scroll", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "ForwardScroll")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "ForwardScroll", 0)],
        },
        new TweakDef
        {
            Id = "term-force-insert-mode",
            Label = "Enable Insert Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Enables insert mode in the console host, so typed text inserts rather than overwrites. Default: enabled.",
            Tags = ["console", "insert", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "InsertMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "InsertMode", 1)],
        },
        new TweakDef
        {
            Id = "term-set-window-alpha-opaque",
            Label = "Set Console Window Fully Opaque",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the console window transparency to fully opaque (255). Default: 255.",
            Tags = ["console", "transparency", "opacity", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "WindowAlpha")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "WindowAlpha", 255)],
        },
        new TweakDef
        {
            Id = "term-disable-ctrl-key-shortcuts",
            Label = "Disable Ctrl Key Shortcuts",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Disables Ctrl+C/Ctrl+V shortcuts in the legacy console host. Useful when Ctrl+C is needed for SIGINT. Default: enabled.",
            Tags = ["console", "ctrl", "shortcuts", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "CtrlKeyShortcutsDisabled", 1)],
        },
        new TweakDef
        {
            Id = "term-enable-trim-leading-zeros",
            Label = "Enable Trim Leading Zeros",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Trims leading zeros when double-clicking to select numbers in the console. Default: disabled.",
            Tags = ["console", "selection", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "TrimLeadingZeros", 1)],
        },
        new TweakDef
        {
            Id = "term-set-history-buffer-999",
            Label = "Set History Buffer Size to 999",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Increases the command history buffer to 999 entries (maximum). Default: 50.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "HistoryBufferSize", 999)],
        },
        new TweakDef
        {
            Id = "term-disable-number-of-history-buffers",
            Label = "Set Number of History Buffers to 4",
            Category = "PowerShell",
            NeedsAdmin = false,
            Description = "Sets the number of history buffers to 4 (one per console process). Default: 4.",
            Tags = ["console", "history", "buffer", "terminal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Console"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Console", "NumberOfHistoryBuffers", 4)],
        },
    ];
}

// === Merged from: PowerShellTweaks.cs ===

/// <summary>
/// Tweaks executed via PowerShell cmdlets (Set-Service, Get-Service, Enable-WindowsOptionalFeature, etc.).
/// These use ApplyAction/RemoveAction/DetectAction delegates via ShellRunner.RunPowerShell.
/// </summary>
internal static class PowerShellTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── Service control via PowerShell ───────────────────────────────
        new TweakDef
        {
            Id = "ps-disable-print-spooler",
            Label = "Disable Print Spooler Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Print Spooler service. Closes the PrintNightmare attack vector on machines without printers.",
            Tags = ["powershell", "service", "security", "print"],
            SideEffects = "Printing will be unavailable.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name Spooler -Force -ErrorAction SilentlyContinue; Set-Service -Name Spooler -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Spooler -StartupType Automatic; Start-Service -Name Spooler"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Spooler -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-remote-registry",
            Label = "Disable Remote Registry Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Stops and disables the Remote Registry service to prevent remote access to the registry.",
            Tags = ["powershell", "service", "security", "remote"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name RemoteRegistry -Force -ErrorAction SilentlyContinue; Set-Service -Name RemoteRegistry -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name RemoteRegistry -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name RemoteRegistry -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-fax-service",
            Label = "Disable Fax Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Fax service — unnecessary on most modern systems.",
            Tags = ["powershell", "service", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell("Stop-Service -Name Fax -Force -ErrorAction SilentlyContinue; Set-Service -Name Fax -StartupType Disabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name Fax -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name Fax -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-xbox-services",
            Label = "Disable Xbox Live Services",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Xbox Live Auth, Networking, and Game Save services. Safe if not using Xbox features.",
            Tags = ["powershell", "service", "xbox", "gaming", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Stop-Service -Name $svc -Force -ErrorAction SilentlyContinue; Set-Service -Name $svc -StartupType Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "foreach ($svc in @('XblAuthManager','XblGameSave','XboxNetApiSvc','XboxGipSvc')) { Set-Service -Name $svc -StartupType Manual -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name XblAuthManager -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── System optimisation via PowerShell ──────────────────────────
        new TweakDef
        {
            Id = "ps-clear-temp-files",
            Label = "Clear Temporary Files",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Removes files from %TEMP%, Windows\\Temp, and prefetch folders to free disk space.",
            Tags = ["powershell", "cleanup", "disk", "maintenance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-Item -Path \"$env:TEMP\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Temp\\*\" -Recurse -Force -ErrorAction SilentlyContinue; "
                        + "Remove-Item -Path \"$env:WINDIR\\Prefetch\\*\" -Recurse -Force -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => { }, // No undo for cleanup
            DetectAction = () =>
            {
                // Check if temp folder has minimal content
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-ChildItem -Path $env:TEMP -ErrorAction SilentlyContinue | Measure-Object).Count"
                );
                return int.TryParse(stdout.Trim(), out var count) && count < 10;
            },
        },
        new TweakDef
        {
            Id = "ps-enable-dev-mode",
            Label = "Enable Developer Mode",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables Windows Developer Mode for sideloading apps and using developer features.",
            Tags = ["powershell", "developer", "sideload"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    0
                ),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock", "AllowAllTrustedApps", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AppModelUnlock",
                    "AllowDevelopmentWithoutDevLicense",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "ps-flush-dns-cache",
            Label = "Flush DNS Cache",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Clears the local DNS resolver cache. Useful after changing DNS servers or troubleshooting resolution issues.",
            Tags = ["powershell", "dns", "network", "troubleshooting"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Clear-DnsClientCache"),
            RemoveAction = _ => { }, // No undo for cache flush
            DetectAction = () => false, // Always shows as not applied — it's a one-shot action
        },
        new TweakDef
        {
            Id = "ps-disable-diagnostics-hub",
            Label = "Disable Diagnostics Hub Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Diagnostics Hub Standard Collector service (DiagTrack helper). Reduces telemetry overhead.",
            Tags = ["powershell", "service", "telemetry", "privacy", "performance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name diagnosticshub.standardcollector.service -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name diagnosticshub.standardcollector.service -StartupType Manual -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-Service -Name diagnosticshub.standardcollector.service -ErrorAction SilentlyContinue).StartType"
                );
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-wmp-network-sharing",
            Label = "Disable WMP Network Sharing Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables Windows Media Player Network Sharing service. Reduces network exposure.",
            Tags = ["powershell", "service", "media", "network", "cleanup"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name WMPNetworkSvc -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name WMPNetworkSvc -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name WMPNetworkSvc -StartupType Manual -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name WMPNetworkSvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-geolocation-service",
            Label = "Disable Geolocation Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the Geolocation service to prevent apps from tracking your physical location.",
            Tags = ["powershell", "service", "privacy", "location"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name lfsvc -Force -ErrorAction SilentlyContinue; Set-Service -Name lfsvc -StartupType Disabled"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name lfsvc -StartupType Manual"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name lfsvc -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-connected-user-experience",
            Label = "Disable Connected User Experience (DiagTrack)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the DiagTrack (Connected User Experiences and Telemetry) service. Major Windows telemetry reducer.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name DiagTrack -Force -ErrorAction SilentlyContinue; Set-Service -Name DiagTrack -StartupType Disabled"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-Service -Name DiagTrack -StartupType Automatic; Start-Service -Name DiagTrack -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name DiagTrack -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-dmwappush-service",
            Label = "Disable Device Management WAP Push Service",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.ServiceControl,
            Description = "Disables the dmwappushservice used for telemetry data collection routing.",
            Tags = ["powershell", "service", "telemetry", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Stop-Service -Name dmwappushservice -Force -ErrorAction SilentlyContinue; "
                        + "Set-Service -Name dmwappushservice -StartupType Disabled -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-Service -Name dmwappushservice -StartupType Automatic -ErrorAction SilentlyContinue"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-Service -Name dmwappushservice -ErrorAction SilentlyContinue).StartType");
                return stdout.Trim().Equals("Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-optimize-network-adapter",
            Label = "Optimize Network Adapter Power Settings",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables power management on all network adapters to prevent them from sleeping and dropping connections.",
            Tags = ["powershell", "network", "power", "performance", "stability"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Disabled -WakeOnPattern Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Enabled -WakeOnPattern Enabled -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-execution-policy-restriction",
            Label = "Set PowerShell Execution Policy to RemoteSigned",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets the machine-scope execution policy to RemoteSigned, allowing local scripts to run without signed status.",
            Tags = ["powershell", "execution-policy", "scripts", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope LocalMachine -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-ExecutionPolicy -ExecutionPolicy Restricted -Scope LocalMachine -Force"),
            DetectAction = () =>
            {
                var (exit, stdout, _) = ShellRunner.Run(
                    "powershell",
                    ["-NoProfile", "-Command", "(Get-ExecutionPolicy -Scope LocalMachine).ToString()"]
                );
                return exit == 0 && stdout.Trim() == "RemoteSigned";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-remoting",
            Label = "Enable PowerShell Remoting",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Enables PowerShell Remoting (WinRM) for remote session management. Required for remote administration.",
            Tags = ["powershell", "remoting", "winrm", "remote"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Enable-PSRemoting -Force -SkipNetworkProfileCheck"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Disable-PSRemoting -Force"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("powershell", ["-NoProfile", "-Command", "(Get-Service WinRM).Status"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "ps-disable-telemetry",
            Label = "Disable PowerShell Telemetry",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Sets POWERSHELL_TELEMETRY_OPTOUT=1 for the current user to opt out of PowerShell telemetry submission to Microsoft.",
            Tags = ["powershell", "telemetry", "privacy"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT','1','User')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('POWERSHELL_TELEMETRY_OPTOUT',$null,'User')"),
            DetectAction = () =>
                System.Environment.GetEnvironmentVariable("POWERSHELL_TELEMETRY_OPTOUT", System.EnvironmentVariableTarget.User) == "1",
        },
        new TweakDef
        {
            Id = "ps-enable-constrained-language-mode",
            Label = "Enable PowerShell Constrained Language Mode",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description =
                "Restricts PowerShell to Constrained Language Mode via environment variable, limiting access to arbitrary .NET types. Hardens against living-off-the-land attacks.",
            Tags = ["powershell", "security", "constrained", "hardening"],
            ApplyAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy','4','Machine')"),
            RemoveAction = _ => ShellRunner.RunPowerShell("[System.Environment]::SetEnvironmentVariable('__PSLockdownPolicy',$null,'Machine')"),
            DetectAction = () => System.Environment.GetEnvironmentVariable("__PSLockdownPolicy", System.EnvironmentVariableTarget.Machine) == "4",
        },
        new TweakDef
        {
            Id = "ps-set-transcript-logging",
            Label = "Disable PowerShell Transcription Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables PowerShell transcript logging which records all session input/output to disk. Reduces privacy exposure.",
            Tags = ["powershell", "transcription", "logging", "privacy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -Value 0 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\PowerShell\\Transcription' -Name 'EnableTranscripting' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-protected-event-logging",
            Label = "Enable Protected Event Logging",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Protected Event Logging (PEL) which encrypts event log content using a certificate. Prevents credential exposure in logs.",
            Tags = ["powershell", "event-log", "security", "encryption"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 1 -Type DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\EventLog\\ProtectedEventLogging' -Name 'EnableProtectedEventLogging' -Value 0 -Type DWord -Force"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-clipboard-history-via-ps",
            Label = "Disable Clipboard History via Policy",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables Win+V clipboard history via group policy registry key, preventing clipboard contents from being saved.",
            Tags = ["powershell", "clipboard", "privacy", "policy"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "New-Item -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Force | New-ItemProperty -Name 'AllowClipboardHistory' -Value 0 -PropertyType DWord -Force"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Remove-ItemProperty -Path 'HKLM:\\SOFTWARE\\Policies\\Microsoft\\Windows\\System' -Name 'AllowClipboardHistory' -ErrorAction SilentlyContinue"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-optimize-page-file",
            Label = "Set Page File to System-Managed",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the page file to be automatically managed by Windows for optimal memory performance.",
            Tags = ["powershell", "pagefile", "memory", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $true; $cs.Put()"),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell("$cs = Get-WmiObject Win32_ComputerSystem; $cs.AutomaticManagedPagefile = $false; $cs.Put()"),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-enable-tls12",
            Label = "Enable TLS 1.2 for .NET Applications",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Configures the .NET Framework 4.x to use TLS 1.2 by default for all outgoing HTTPS connections.",
            Tags = ["powershell", "tls", "security", "network", "dotnet"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { New-Item -Path $_ -Force | New-ItemProperty -Name 'SystemDefaultTlsVersions' -Value 1 -PropertyType DWord -Force | Out-Null; "
                        + "New-ItemProperty -Path $_ -Name 'SchUseStrongCrypto' -Value 1 -PropertyType DWord -Force | Out-Null }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "@('HKLM:\\SOFTWARE\\Microsoft\\.NETFramework\\v4.0.30319','HKLM:\\SOFTWARE\\Wow6432Node\\Microsoft\\.NETFramework\\v4.0.30319') | "
                        + "ForEach-Object { Remove-ItemProperty -Path $_ -Name 'SystemDefaultTlsVersions','SchUseStrongCrypto' -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "ps-disable-powershell-v2-engine",
            Label = "Disable PowerShell v2 Engine (Attack Surface Reduction)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Removes the MicrosoftWindowsPowerShellV2Root optional feature. PowerShell v2 lacks logging, constrained language mode, and ScriptBlock logging — keeping it installed exposes a logging bypass attack vector.",
            Tags = ["powershell", "security", "v2", "dism"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:MicrosoftWindowsPowerShellV2Root", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:MicrosoftWindowsPowerShellV2Root"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-windows-sandbox",
            Label = "Enable Windows Sandbox (Disposable Isolated Environment)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            MinBuild = 18305,
            Description =
                "Enables the Containers-DisposableClientVM optional feature. Provides a lightweight, disposable Windows environment for executing untrusted software safely — no separate licence required.",
            Tags = ["powershell", "sandbox", "isolation", "security"],
            SideEffects = "Requires Hyper-V. Requires a reboot.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-controlled-folder-access",
            Label = "Enable Controlled Folder Access (Ransomware Protection)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Windows Defender Controlled Folder Access via Set-MpPreference. Blocks unauthorised apps from writing to protected user folders (Documents, Desktop, Pictures), providing ransomware protection.",
            Tags = ["powershell", "defender", "ransomware", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableControlledFolderAccess Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableControlledFolderAccess");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-network-protection",
            Label = "Enable Windows Defender Network Protection",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Enables Defender Network Protection via Set-MpPreference. Blocks connections to known malicious IPs, domains, and URLs using the SmartScreen cloud reputation service.",
            Tags = ["powershell", "defender", "network", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Enabled"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -EnableNetworkProtection Disabled"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).EnableNetworkProtection");
                return stdout.Trim() == "1";
            },
        },
        new TweakDef
        {
            Id = "ps-set-defender-scan-cpu-limit",
            Label = "Limit Defender Scans to 50% CPU Average",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets ScanAvgCPULoadFactor=50 via Set-MpPreference. Caps Windows Defender background scan CPU usage at 50%, reducing performance impact on developer workloads during scheduled scans.",
            Tags = ["powershell", "defender", "cpu", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 50"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -ScanAvgCPULoadFactor 80"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).ScanAvgCPULoadFactor");
                return stdout.Trim() == "50";
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-server",
            Label = "Require SMB Signing on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbServerConfiguration. Mandates cryptographic signing on all SMB server sessions, preventing man-in-the-middle relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-signing-client",
            Label = "Require SMB Signing on This Client (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets RequireSecuritySignature=$true via Set-SmbClientConfiguration. Enforces signing on all outbound SMB connections from this machine, blocking NTLM relay attacks.",
            Tags = ["powershell", "smb", "signing", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -RequireSecuritySignature $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).RequireSecuritySignature");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-smb-guest-fallback",
            Label = "Disable SMB Insecure Guest Logon Fallback",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EnableInsecureGuestLogons=$false via Set-SmbClientConfiguration. Prevents Windows from falling back to an unauthenticated guest SMB session when credential negotiation fails.",
            Tags = ["powershell", "smb", "guest", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $false -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbClientConfiguration -EnableInsecureGuestLogons $true -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbClientConfiguration).EnableInsecureGuestLogons");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-smb-encryption-server",
            Label = "Enable SMB Encryption on This Server (via PowerShell)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets EncryptData=$true via Set-SmbServerConfiguration. Encrypts all SMB3 data in transit on this server, protecting file shares on untrusted networks.",
            Tags = ["powershell", "smb", "encryption", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $true -Force"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-SmbServerConfiguration -EncryptData $false -Force"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-SmbServerConfiguration).EncryptData");
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-teredo",
            Label = "Disable Teredo IPv6 Tunnelling",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables Teredo via netsh. Teredo is an IPv6-over-UDP-IPv4 tunnelling protocol that can be exploited to bypass firewall rules restricting IPv6 traffic.",
            Tags = ["powershell", "ipv6", "teredo", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "teredo", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "teredo", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-6to4",
            Label = "Disable 6to4 IPv6 Transition Protocol",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the 6to4 transition mechanism via netsh. 6to4 encapsulates IPv6 packets within IPv4 and can create unexpected outbound routing paths when native IPv6 is absent.",
            Tags = ["powershell", "ipv6", "6to4", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["int", "6to4", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["int", "6to4", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-disable-isatap",
            Label = "Disable ISATAP IPv6 Transition Interface",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description =
                "Disables the Intra-Site Automatic Tunnel Addressing Protocol (ISATAP) via netsh. ISATAP is an IPv6-in-IPv4 tunnelling mechanism that creates hidden IPv6 connectivity channels.",
            Tags = ["powershell", "ipv6", "isatap", "network", "security"],
            ApplyAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh", ["interface", "isatap", "set", "state", "default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh", ["interface", "isatap", "show", "state"]);
                return stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "ps-enable-defender-realtime",
            Label = "Ensure Windows Defender Realtime Protection is On",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description =
                "Sets DisableRealtimeMonitoring=$false via Set-MpPreference. Confirms that Defender real-time scanning is active — useful as a remediation step when Group Policy or another tool has disabled it.",
            Tags = ["powershell", "defender", "realtime", "security"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $false"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-MpPreference -DisableRealtimeMonitoring $true"),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell("(Get-MpPreference).DisableRealtimeMonitoring");
                return stdout.Trim().Equals("False", StringComparison.OrdinalIgnoreCase);
            },
        },
    ];
}

// ── Merged from CommandLineTweaks.cs ──────────────────────────────────────────────────

internal static class CommandLineTweaks
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        // ── bcdedit tweaks ──────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-hyper-v-hypervisor",
            Label = "Disable Hyper-V Hypervisor (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Uses bcdedit to set hypervisorlaunchtype off. Reduces overhead for non-Hyper-V workloads. Requires reboot.",
            Tags = ["bcdedit", "hypervisor", "performance", "gaming"],
            SideEffects = "Disables Hyper-V, WSL 2, Windows Sandbox, and Credential Guard.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "off"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "hypervisorlaunchtype", "auto"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("hypervisorlaunchtype    Off", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-boot-log",
            Label = "Enable Boot Log (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables boot logging to %SystemRoot%\\ntbtlog.txt for troubleshooting driver load order.",
            Tags = ["bcdedit", "boot", "diagnostics"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "{current}", "bootlog", "no"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("bootlog                 Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-increase-tscsyncpolicy",
            Label = "Set TSC Sync Policy to Enhanced (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TSC synchronisation policy to Enhanced for more accurate timers in gaming and real-time workloads.",
            Tags = ["bcdedit", "performance", "gaming", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "tscsyncpolicy", "enhanced"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "tscsyncpolicy"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("tscsyncpolicy           Enhanced", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-dynamic-tick",
            Label = "Disable Dynamic Tick (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables dynamic tick to ensure consistent timer resolution. Beneficial for low-latency audio/gaming.",
            Tags = ["bcdedit", "performance", "gaming", "latency"],
            SideEffects = "May slightly increase power consumption.",
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "disabledynamictick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "disabledynamictick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("disabledynamictick      Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-set-platform-tick-high",
            Label = "Force Platform Clock to High Resolution (bcdedit)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Forces the platform clock to use the highest resolution available. Reduces timer jitter.",
            Tags = ["bcdedit", "performance", "latency", "timer"],
            ApplyAction = _ => ShellRunner.Run("bcdedit.exe", ["/set", "useplatformtick", "yes"]),
            RemoveAction = _ => ShellRunner.Run("bcdedit.exe", ["/deletevalue", "useplatformtick"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("bcdedit.exe", ["/enum", "{current}"]);
                return stdout.Contains("useplatformtick         Yes", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── netsh tweaks ────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the NetBIOS name resolution protocol via Windows Firewall inbound rule. Reduces attack surface.",
            Tags = ["netsh", "security", "network"],
            ApplyAction = _ =>
                ShellRunner.Run(
                    "netsh.exe",
                    ["advfirewall", "firewall", "add", "rule", "name=Block NetBIOS", "dir=in", "action=block", "protocol=TCP", "localport=137-139"]
                ),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "delete", "rule", "name=Block NetBIOS"]),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("netsh.exe", ["advfirewall", "firewall", "show", "rule", "name=Block NetBIOS"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-tcp-autotuning",
            Label = "Set TCP Auto-Tuning to Normal (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Sets TCP receive window auto-tuning level to normal for maximum throughput.",
            Tags = ["netsh", "network", "performance", "tcp"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=normal"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "autotuninglevel=default"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive Window Auto-Tuning Level", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("normal", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-rss",
            Label = "Enable Receive Side Scaling (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables RSS to distribute network processing across multiple CPU cores.",
            Tags = ["netsh", "network", "performance", "rss"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "rss=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Receive-Side Scaling State", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-tcp-timestamps",
            Label = "Disable TCP Timestamps (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables TCP timestamps to reduce packet overhead and prevent OS fingerprinting.",
            Tags = ["netsh", "security", "network", "privacy"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=disabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "timestamps=enabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("Timestamps", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-ecn",
            Label = "Enable ECN Capability (netsh)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables Explicit Congestion Notification for better network congestion handling.",
            Tags = ["netsh", "network", "performance"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=enabled"]),
            RemoveAction = _ => ShellRunner.Run("netsh.exe", ["int", "tcp", "set", "global", "ecncapability=disabled"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("netsh.exe", ["int", "tcp", "show", "global"]);
                return stdout.Contains("ECN Capability", StringComparison.OrdinalIgnoreCase)
                    && stdout.Contains("enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── powercfg tweaks ─────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-set-ultimate-perf-plan",
            Label = "Activate Ultimate Performance Power Plan (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Unhides and activates the Ultimate Performance power plan for maximum CPU/GPU performance.",
            Tags = ["powercfg", "power", "performance", "gaming"],
            ApplyAction = _ =>
            {
                // Enable the hidden plan, then set it active
                ShellRunner.Run("powercfg.exe", ["/duplicatescheme", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
                ShellRunner.Run("powercfg.exe", ["/setactive", "e9a42b02-d5df-448d-aa00-03f14749eb61"]);
            },
            RemoveAction = _ =>
            {
                // Switch back to Balanced
                ShellRunner.Run("powercfg.exe", ["/setactive", "381b4222-f694-41f0-9685-ff5bb260df2e"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("powercfg.exe", ["/getactivescheme"]);
                return stdout.Contains("e9a42b02-d5df-448d-aa00-03f14749eb61", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-usb-selective-suspend",
            Label = "Disable USB Selective Suspend (powercfg)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables USB selective suspend to prevent USB devices from disconnecting during idle.",
            Tags = ["powercfg", "usb", "power", "stability"],
            ApplyAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "0"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setacvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run(
                    "powercfg.exe",
                    ["/setdcvalueindex", "SCHEME_CURRENT", "2a737441-1930-4402-8d77-b2bebba308a3", "48e6b7a6-50f5-4782-a5d4-53bb8f07e226", "1"]
                );
                ShellRunner.Run("powercfg.exe", ["/setactive", "SCHEME_CURRENT"]);
            },
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(powercfg /query SCHEME_CURRENT 2a737441-1930-4402-8d77-b2bebba308a3 48e6b7a6-50f5-4782-a5d4-53bb8f07e226) -match '0x00000000'"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        // ── DISM tweaks ─────────────────────────────────────────────────
        new TweakDef
        {
            Id = "cmd-disable-ie-feature",
            Label = "Disable Internet Explorer (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables the Internet Explorer optional feature via DISM. Reduces attack surface.",
            Tags = ["dism", "security", "ie", "legacy"],
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Internet-Explorer-Optional-amd64", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Internet-Explorer-Optional-amd64"]);
                return stdout.Contains("State : Disabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-sandbox",
            Label = "Enable Windows Sandbox (DISM)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the Windows Sandbox feature for isolated testing environments. Requires Hyper-V support.",
            Tags = ["dism", "security", "sandbox", "virtualization"],
            SideEffects = "Requires reboot after enabling.",
            ApplyAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:Containers-DisposableClientVM", "/All", "/NoRestart"]),
            RemoveAction = _ =>
                ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:Containers-DisposableClientVM", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:Containers-DisposableClientVM"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-enable-net35",
            Label = "Enable .NET Framework 3.5",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Enables the .NET Framework 3.5 feature (includes .NET 2.0 and 3.0) for legacy application support.",
            Tags = ["dism", "dotnet", "framework", "legacy"],
            SideEffects = "Downloads components from Windows Update if not cached.",
            ApplyAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Enable-Feature", "/FeatureName:NetFx3", "/All", "/NoRestart"]),
            RemoveAction = _ => ShellRunner.Run("dism.exe", ["/Online", "/Disable-Feature", "/FeatureName:NetFx3", "/NoRestart"]),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.Run("dism.exe", ["/Online", "/Get-FeatureInfo", "/FeatureName:NetFx3"]);
                return stdout.Contains("State : Enabled", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "cmd-disable-ipv6-tunnel-adapters",
            Label = "Disable IPv6 Tunnel Adapters (6to4, ISATAP, Teredo)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.SystemCommand,
            Description = "Disables IPv6 transition technologies (6to4, ISATAP, Teredo) to reduce attack surface.",
            Tags = ["netsh", "ipv6", "security", "network"],
            ApplyAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "disabled"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "disabled"]);
            },
            RemoveAction = _ =>
            {
                ShellRunner.Run("netsh.exe", ["interface", "6to4", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "isatap", "set", "state", "default"]);
                ShellRunner.Run("netsh.exe", ["interface", "teredo", "set", "state", "default"]);
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "cmd-enable-ntp-high-freq",
            Label = "Set NTP Polling to High Frequency",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Configures the Windows Time service to poll NTP servers more frequently (every 256s instead of 3600s).",
            Tags = ["time", "ntp", "synchronisation"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 8),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 10),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MaxPollInterval", 15),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\W32Time\Config", "MinPollInterval", 6)],
        },
        new TweakDef
        {
            Id = "cmd-set-multi-plane-overlay",
            Label = "Enable Multi-Plane Overlay (MPO)",
            Category = "PowerShell",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Ensures Multi-Plane Overlay is enabled for GPU composition offloading, reducing CPU usage.",
            Tags = ["gpu", "display", "performance", "mpo"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\Dwm", "OverlayTestMode", 5)],
        },
        new TweakDef
        {
            Id = "cmd-disable-game-dvr-background",
            Label = "Disable Background Game Recording (Game DVR)",
            Category = "PowerShell",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Xbox Game DVR background recording to free up GPU and disk resources.",
            Tags = ["gaming", "game-dvr", "xbox", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
    ];
}

// ── merged from PolicyPowerShell.cs ──
// RegiLattice.Core — Tweaks/PolicyPowerShell.cs
// PowerShell execution, ISE deprecation, PS7 modes, script block logging, and scripted diagnostics policies
// Category: "PowerShell & Scripting Policy"
// Consolidated from 6 modules.

internal static class PolicyPowerShell
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _IseDeprecationPolicy.Data,
            .. _PowerShellPolicy.Data,
            .. _Ps7ExecutionModePolicy.Data,
            .. _ScriptBlockLoggingAdvancedPolicy.Data,
            .. _ScriptedDiagnosticsPolicy.Data,
            .. _WindowsTerminalAdvancedPolicy.Data,
        ];

    // ── IseDeprecationPolicy ──
    private static class _IseDeprecationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ProtectedEventLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "isedep-block-ise-launch",
                    Label = "Block PowerShell ISE Launch",
                    Category = "PowerShell",
                    Description =
                        "Blocks launch of the Windows PowerShell ISE (Integrated Scripting Environment), which is end-of-life and lacks modern security controls like AMSI integration.",
                    Tags = ["powershell", "ise", "deprecation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PowerShell ISE cannot be opened; users must use VS Code or PowerShell 7 instead.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableISE", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableISE")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableISE", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-remoting-allsigned",
                    Label = "Block Unsigned Scripts via PS Remoting",
                    Category = "PowerShell",
                    Description =
                        "Sets the remoting script execution policy to AllSigned, so scripts delivered via WinRM PowerShell remoting sessions must be digitally signed.",
                    Tags = ["powershell", "remoting", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned scripts over WinRM remoting blocked; signed scripts required.",
                    ApplyOps = [RegOp.SetDword(Key, "RemotingExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RemotingExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "RemotingExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-v2-engine",
                    Label = "Disable PowerShell v2 Engine",
                    Category = "PowerShell",
                    Description =
                        "Disables the Windows PowerShell version 2 engine (powershell.exe -version 2) which bypasses modern security controls such as AMSI, ETW, and Constrained Language Mode.",
                    Tags = ["powershell", "v2", "downgrade", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "PS v2 engine blocked; attackers cannot downgrade to bypass AMSI. Legacy apps requiring PS2 will break.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePowerShellV2", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerShellV2")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePowerShellV2", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-protected-event-logging",
                    Label = "Enable Protected Event Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables Protected Event Logging (PEL) for PowerShell, which encrypts sensitive PowerShell script block log entries at rest using a certificate, protecting them from unauthorized access.",
                    Tags = ["powershell", "event-logging", "encryption", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Script block logs encrypted; only authorised certificate holders can decrypt and read them.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableProtectedEventLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableProtectedEventLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableProtectedEventLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-credential-prompt",
                    Label = "Disable Credential Prompt in PowerShell Sessions",
                    Category = "PowerShell",
                    Description =
                        "Disables interactive credential prompts within PowerShell sessions, forcing scripts to use pre-provisioned credentials or fail instead of prompting the user.",
                    Tags = ["powershell", "credentials", "prompt", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Credential prompts inside PS sessions blocked; scripts requiring user input must be refactored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCredentialRequestPrompt", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCredentialRequestPrompt")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCredentialRequestPrompt", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-enable-module-logging",
                    Label = "Enable Module Logging for PowerShell",
                    Category = "PowerShell",
                    Description =
                        "Enables module-level logging for all PowerShell modules by default, ensuring that all custom module invocations are captured in the Windows PowerShell/Operational event log.",
                    Tags = ["powershell", "module-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "All loaded PS module commands logged; log volume scales with module usage.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableModuleLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableModuleLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableModuleLogging", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-script-download",
                    Label = "Disable Script Download from Internet in PowerShell",
                    Category = "PowerShell",
                    Description =
                        "Blocks PowerShell from downloading and executing scripts from internet URIs using Invoke-Expression (IEX) with web requests, a common living-off-the-land attack technique.",
                    Tags = ["powershell", "download-cradle", "iex", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "PS internet download-and-exec cradles blocked; IEX/webclient patterns are prevented.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableScriptDownloadFromInternet", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableScriptDownloadFromInternet")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableScriptDownloadFromInternet", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-block-ps-dev-mode",
                    Label = "Block PowerShell Developer Mode",
                    Category = "PowerShell",
                    Description =
                        "Disables the PowerShell developer mode flag that bypasses certain security policies, ensuring that production machines do not inadvertently run in a relaxed-security development mode.",
                    Tags = ["powershell", "developer-mode", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS developer/debug mode disabled; policy overrides cannot be bypassed via dev flags.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSDeveloperMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSDeveloperMode")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSDeveloperMode", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-disable-ps-telemetry",
                    Label = "Disable Windows PowerShell 5 Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables usage telemetry collection in Windows PowerShell 5.1, preventing execution metadata and error statistics from being sent to Microsoft.",
                    Tags = ["powershell", "ps5", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "PS 5.1 telemetry stopped; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "isedep-force-network-restricted-sessions",
                    Label = "Force Network-Restricted PowerShell Remoting Sessions",
                    Category = "PowerShell",
                    Description =
                        "Forces all incoming PowerShell remoting sessions to run as NetworkRestricted, preventing remotely established sessions from making outbound network connections.",
                    Tags = ["powershell", "remoting", "network-restricted", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Remote PS sessions cannot make new network connections; lateral movement via PS remoting reduced.",
                    ApplyOps = [RegOp.SetDword(Key, "ForceNetworkRestrictedSessions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ForceNetworkRestrictedSessions")],
                    DetectOps = [RegOp.CheckDword(Key, "ForceNetworkRestrictedSessions", 1)],
                },
            ];
    }

    // ── PowerShellPolicy ──
    private static class _PowerShellPolicy
    {
        private const string PsRoot = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell";
        private const string ScriptBlockLogging = PsRoot + @"\ScriptBlockLogging";
        private const string ModuleLogging = PsRoot + @"\ModuleLogging";
        private const string Transcription = PsRoot + @"\Transcription";
        private const string ProtectedEventLogging = PsRoot + @"\ProtectedEventLogging";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "pspolicy-script-block-logging",
                Label = "Enable PowerShell Script Block Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables PowerShell Script Block Logging (GPO) so that all script blocks "
                    + "executed by PowerShell are written to the Operational event log "
                    + "(Microsoft-Windows-PowerShell/Operational). Essential for threat hunting.",
                Tags = ["powershell", "logging", "script block", "security", "audit"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-script-invocation-logging",
                Label = "Enable PowerShell Script Invocation Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables logging of script-block invocation start/stop events in addition "
                    + "to the raw script text. Adds invocation sequence to event IDs 4104/4105/4106. "
                    + "EnableScriptBlockInvocationLogging=1.",
                Tags = ["powershell", "logging", "invocation logging", "security"],
                RegistryKeys = [ScriptBlockLogging],
                ApplyOps = [RegOp.SetDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ScriptBlockLogging, "EnableScriptBlockInvocationLogging")],
                DetectOps = [RegOp.CheckDword(ScriptBlockLogging, "EnableScriptBlockInvocationLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-module-logging",
                Label = "Enable PowerShell Module Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Enables Module Logging, which records pipeline execution details for "
                    + "specified (or all) PowerShell modules to the Windows event log. "
                    + "Helps detect malicious module imports. EnableModuleLogging=1.",
                Tags = ["powershell", "module logging", "security", "audit"],
                RegistryKeys = [ModuleLogging],
                ApplyOps = [RegOp.SetDword(ModuleLogging, "EnableModuleLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ModuleLogging, "EnableModuleLogging")],
                DetectOps = [RegOp.CheckDword(ModuleLogging, "EnableModuleLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-on",
                Label = "Enable PowerShell Transcription",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Turns on PowerShell Transcription, writing a plain-text record of every "
                    + "PowerShell session (all commands and output) to a configured output "
                    + "directory. EnableTranscripting=1.",
                Tags = ["powershell", "transcription", "logging", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableTranscripting", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableTranscripting")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableTranscripting", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-header",
                Label = "Include Invocation Header in PowerShell Transcripts",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Adds timestamps, username, and invocation details to each PowerShell "
                    + "transcript header, making audit review significantly easier. "
                    + "EnableInvocationHeader=1.",
                Tags = ["powershell", "transcription", "header", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetDword(Transcription, "EnableInvocationHeader", 1)],
                RemoveOps = [RegOp.DeleteValue(Transcription, "EnableInvocationHeader")],
                DetectOps = [RegOp.CheckDword(Transcription, "EnableInvocationHeader", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-transcription-output-path",
                Label = "Set PowerShell Transcript Output Directory",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets the PowerShell transcript output directory to "
                    + @"%SYSTEMROOT%\Logs\PowerShell so transcripts are centralised and "
                    + "survive user profile deletion. OutputDirectory (REG_SZ).",
                Tags = ["powershell", "transcription", "output path", "audit"],
                RegistryKeys = [Transcription],
                ApplyOps = [RegOp.SetString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
                RemoveOps = [RegOp.DeleteValue(Transcription, "OutputDirectory")],
                DetectOps = [RegOp.CheckString(Transcription, "OutputDirectory", @"%SYSTEMROOT%\Logs\PowerShell")],
            },
            new TweakDef
            {
                Id = "pspolicy-disable-ps2-engine",
                Label = "Disable PowerShell 2.0 Engine",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets EnablePS2=0 via GPO to prevent PowerShell from falling back to the "
                    + "legacy v2 engine, which lacks script block logging and constrained language "
                    + "mode — a known AMSI/logging bypass vector.",
                Tags = ["powershell", "ps2", "downgrade attack", "security", "amsi"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnablePS2", 0)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnablePS2")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnablePS2", 0)],
            },
            new TweakDef
            {
                Id = "pspolicy-protected-event-logging",
                Label = "Enable Protected Event Logging",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Enables Protected Event Logging, which encrypts sensitive event log data "
                    + "(such as PowerShell credentials in transcripts) using a CMS certificate, "
                    + "so only authorized readers can decrypt. EnableProtectedEventLogging=1.",
                Tags = ["powershell", "event log", "encryption", "protected logging", "security"],
                RegistryKeys = [ProtectedEventLogging],
                ApplyOps = [RegOp.SetDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(ProtectedEventLogging, "EnableProtectedEventLogging")],
                DetectOps = [RegOp.CheckDword(ProtectedEventLogging, "EnableProtectedEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-enable-scripts",
                Label = "Enable PowerShell Script Execution (GPO)",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets EnableScripts=1 via GPO, activating the policy-controlled execution "
                    + "policy. Must be enabled before ExecutionPolicy can be enforced via "
                    + "Group Policy. Set together with pspolicy-require-signed-scripts.",
                Tags = ["powershell", "execution policy", "scripts", "gpo"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetDword(PsRoot, "EnableScripts", 1)],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "EnableScripts")],
                DetectOps = [RegOp.CheckDword(PsRoot, "EnableScripts", 1)],
            },
            new TweakDef
            {
                Id = "pspolicy-require-signed-scripts",
                Label = "Require Signed PowerShell Scripts (AllSigned)",
                Category = "PowerShell",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets ExecutionPolicy=AllSigned via GPO, requiring that all scripts and "
                    + "configuration files — including local scripts — be signed by a trusted "
                    + "publisher. Strongest policy-level execution restriction.",
                Tags = ["powershell", "execution policy", "signed", "allsigned", "security"],
                RegistryKeys = [PsRoot],
                ApplyOps = [RegOp.SetString(PsRoot, "ExecutionPolicy", "AllSigned")],
                RemoveOps = [RegOp.DeleteValue(PsRoot, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckString(PsRoot, "ExecutionPolicy", "AllSigned")],
            },
        ];
    }

    // ── Ps7ExecutionModePolicy ──
    private static class _Ps7ExecutionModePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PowerShellCore\ScriptBlockLogging";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ps7exec-enable-constrained-language",
                    Label = "Enable Constrained Language Mode in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Enables Constrained Language Mode (CLM) for PowerShell 7 (pwsh), restricting the .NET types and COM objects that scripts can use and mitigating fileless malware execution.",
                    Tags = ["powershell", "ps7", "constrained-language", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote = "PS7 runs in Constrained Language Mode; complex scripts and .NET interop may break.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableConstrainedLanguageMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableConstrainedLanguageMode")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableConstrainedLanguageMode", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-set-allsigned-policy",
                    Label = "Enforce AllSigned Execution Policy in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Sets the PowerShell 7 execution policy to AllSigned, requiring all scripts (including local scripts) to be digitally signed by a trusted publisher before execution.",
                    Tags = ["powershell", "ps7", "execution-policy", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Only code-signed PS7 scripts run; unsigned dev scripts require explicit bypass.",
                    ApplyOps = [RegOp.SetDword(Key, "ExecutionPolicy", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ExecutionPolicy")],
                    DetectOps = [RegOp.CheckDword(Key, "ExecutionPolicy", 2)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-remoting",
                    Label = "Disable PowerShell 7 Remoting",
                    Category = "PowerShell",
                    Description =
                        "Disables PowerShell 7 remoting (WinRM/SSH transport) via policy, preventing pwsh from being used as a remote administration target.",
                    Tags = ["powershell", "ps7", "remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "PS7 remoting disabled; WinRM/SSH-based remote pwsh sessions blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-implicit-remoting",
                    Label = "Disable PS7 Implicit Remoting Module Import",
                    Category = "PowerShell",
                    Description =
                        "Disables implicit remoting module imports in PowerShell 7, preventing a script from automatically importing and executing remote commands from untrusted sources.",
                    Tags = ["powershell", "ps7", "implicit-remoting", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Implicit remote module imports blocked; remote command invocation requires explicit setup.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableImplicitRemoting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitRemoting")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableImplicitRemoting", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-require-signed-modules",
                    Label = "Require Signed Module Manifests in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Requires all PowerShell 7 module manifests (.psd1) to be signed by a trusted publisher before the module can be loaded, blocking unsigned third-party modules.",
                    Tags = ["powershell", "ps7", "modules", "signing", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Unsigned PS7 modules cannot load; all modules must have trusted code signatures.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireSignedModuleManifests", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireSignedModuleManifests")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireSignedModuleManifests", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-block-ps-gallery",
                    Label = "Block PowerShell Gallery Repository in PS7",
                    Category = "PowerShell",
                    Description =
                        "Disables access to the default PowerShell Gallery online repository in PowerShell 7, forcing module and script installation through an approved internal repository.",
                    Tags = ["powershell", "ps7", "gallery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PSGallery blocked; Install-Module will not reach the public gallery.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockPSGallery", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockPSGallery")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockPSGallery", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-script-block-logging",
                    Label = "Enable Script Block Logging in PowerShell 7",
                    Category = "PowerShell",
                    Description =
                        "Enables script block logging in PowerShell 7 to record all script blocks executed to the event log (Microsoft-Windows-PowerShell/Operational), supporting forensic analysis.",
                    Tags = ["powershell", "ps7", "script-block-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "All executed PS7 script blocks logged; event log volume may increase significantly.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-enable-invocation-logging",
                    Label = "Enable Script Block Invocation Logging in PS7",
                    Category = "PowerShell",
                    Description =
                        "Enables verbose script block invocation logging in PowerShell 7, capturing start and stop events for each script block execution for detailed forensic trails.",
                    Tags = ["powershell", "ps7", "invocation-logging", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Verbose invocation events logged; high-traffic PS7 hosts will generate significant log volume.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-telemetry",
                    Label = "Disable PowerShell 7 Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables the PowerShell 7 telemetry feature that sends usage statistics (command names, error categories, OS info) to Microsoft via opt-out environment variable enforcement at policy level.",
                    Tags = ["powershell", "ps7", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS7 telemetry fully disabled at policy level; no usage data sent.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "ps7exec-disable-update-notif",
                    Label = "Disable PowerShell 7 Update Notifications",
                    Category = "PowerShell",
                    Description =
                        "Suppresses in-session PowerShell 7 update available notifications that prompt users to download newer versions, deferring updates to a managed patching process.",
                    Tags = ["powershell", "ps7", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "PS7 update banners not shown; version management via package manager.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableUpdateNotifications", 1)],
                },
            ];
    }

    // ── ScriptBlockLoggingAdvancedPolicy ──
    private static class _ScriptBlockLoggingAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\ScriptBlockLogging";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PowerShell\Transcription";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sbloga-enable-script-block-logging",
                    Label = "Enable Script Block Logging (Windows PS)",
                    Category = "PowerShell",
                    Description =
                        "Enables PowerShell script block logging for Windows PowerShell 5.1 via the dedicated ScriptBlockLogging policy key, recording all executed script blocks to the PowerShell/Operational event log.",
                    Tags = ["powershell", "script-block-logging", "audit", "siem", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Full script block content logged; essential for threat hunting but increases log volume.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header",
                    Label = "Enable Script Block Invocation Header Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables logging of the start and stop events for each function/script-block invocation, providing timestamped execution boundaries in the event log.",
                    Tags = ["powershell", "script-block-logging", "invocation", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Invocation start/stop events logged; detailed execution trail generated.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableScriptBlockInvocationLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableScriptBlockInvocationLogging", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-transcription",
                    Label = "Enable PowerShell Transcript Logging",
                    Category = "PowerShell",
                    Description =
                        "Enables PowerShell session transcription that saves a full text copy of every PowerShell session to a transcript file on disk, providing a human-readable audit trail.",
                    Tags = ["powershell", "transcription", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Every PS session recorded to transcript file; disk space usage increases with PS activity.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableTranscripting", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableTranscripting")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableTranscripting", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-enable-invocation-header-transcript",
                    Label = "Include Invocation Header in PS Transcripts",
                    Category = "PowerShell",
                    Description =
                        "Adds invocation header information (command name, arguments, timestamps, username, process info) to PowerShell transcript files.",
                    Tags = ["powershell", "transcription", "header", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Transcript files include rich header context for each command.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableInvocationHeader", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableInvocationHeader")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableInvocationHeader", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-output-directory",
                    Label = "Set Centralised PowerShell Transcript Directory",
                    Category = "PowerShell",
                    Description =
                        "Sets the PowerShell transcript output directory to a centralised network share or admin-controlled path so all endpoint transcripts are collected in one location.",
                    Tags = ["powershell", "transcription", "directory", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Transcripts written to admin-specified path; ensure path is writable and monitored.",
                    ApplyOps = [RegOp.SetString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                    RemoveOps = [RegOp.DeleteValue(Key2, "OutputDirectory")],
                    DetectOps = [RegOp.CheckString(Key2, "OutputDirectory", @"C:\Windows\Logs\PowerShell")],
                },
                new TweakDef
                {
                    Id = "sbloga-log-encoded-commands",
                    Label = "Log Encoded PowerShell Command Executions",
                    Category = "PowerShell",
                    Description =
                        "Enables script block logging specifically targeting Base64-encoded commands (-EncodedCommand), which are commonly used by malware to obfuscate payloads.",
                    Tags = ["powershell", "encoded-commands", "obfuscation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Encoded command executions captured in logs; key detection for fileless attacks.",
                    ApplyOps = [RegOp.SetDword(Key, "LogEncodedCommands", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogEncodedCommands")],
                    DetectOps = [RegOp.CheckDword(Key, "LogEncodedCommands", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-log-dynamic-code",
                    Label = "Log Dynamically Generated PowerShell Code",
                    Category = "PowerShell",
                    Description =
                        "Enables logging of dynamically generated PowerShell code (e.g., from Invoke-Expression or Add-Type), capturing obfuscated payloads that are assembled at runtime.",
                    Tags = ["powershell", "dynamic-code", "invoke-expression", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Dynamically generated PS code blocks captured; critical for detecting memory-only malware.",
                    ApplyOps = [RegOp.SetDword(Key, "LogDynamicCode", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogDynamicCode")],
                    DetectOps = [RegOp.CheckDword(Key, "LogDynamicCode", 1)],
                },
                new TweakDef
                {
                    Id = "sbloga-set-max-log-size",
                    Label = "Set PowerShell Operational Log Max Size to 512 MB",
                    Category = "PowerShell",
                    Description =
                        "Increases the Microsoft-Windows-PowerShell/Operational event log maximum size to 512 MB to prevent log overwriting (circular buffer) during high-volume script block logging.",
                    Tags = ["powershell", "event-log", "size", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS operational log grows to 512 MB max; older entries retained longer before cycling.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "MaxSize", 524288),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-retain-on-clear",
                    Label = "Retain PowerShell Log Archive on Clear",
                    Category = "PowerShell",
                    Description =
                        "Configures the PowerShell operational event log to archive before clearing when the log becomes full, preventing permanent log loss during log maintenance.",
                    Tags = ["powershell", "event-log", "archive", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PS log archived to .evtx file before clearing; no historical data lost.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\PowerShellOperational", "Retention", 0),
                    ],
                },
                new TweakDef
                {
                    Id = "sbloga-block-clear-eventlog",
                    Label = "Block Standard Users from Clearing Event Logs",
                    Category = "PowerShell",
                    Description =
                        "Restricts the ability to clear the PowerShell and Windows event logs to administrators only, preventing attackers with standard user access from clearing their tracks.",
                    Tags = ["powershell", "event-log", "clear", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Only admins can clear event logs; standard users get access denied.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security", "RestrictGuestAccess", 1),
                    ],
                },
            ];
    }

    // ── ScriptedDiagnosticsPolicy ──
    private static class _ScriptedDiagnosticsPolicy
    {
        private const string SdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnostics";
        private const string SdProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScriptedDiagnosticsProvider\Policy";
        private const string TshootKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Troubleshooting\AllowRecommendations";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sdiag-disable-scripted-diagnostics",
                Label = "Disable Scripted Diagnostics Execution",
                Category = "PowerShell",
                Description =
                    "Sets ExecutionPolicy=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from executing scripted diagnostic packages (.diagpkg, .diag files), "
                    + "including the automated troubleshooters triggered from 'Troubleshoot settings'. "
                    + "Reduces data collection and prevents unintended automated changes. "
                    + "Default: absent (diagnostics run). Recommended: 1 on managed or high-security systems.",
                Tags = ["diagnostics", "scripted", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic packages (.diagpkg) cannot execute; automated troubleshooters are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "ExecutionPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "ExecutionPolicy")],
                DetectOps = [RegOp.CheckDword(SdKey, "ExecutionPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-online-troubleshooters",
                Label = "Disable Online Troubleshooting Recommendations",
                Category = "PowerShell",
                Description =
                    "Sets EnabledPolicy=0 in the ScriptedDiagnosticsProvider Policy key. "
                    + "Prevents Windows from downloading and applying troubleshooting recommendations from Microsoft's "
                    + "online diagnostic database. Stops automatic remediation steps that could modify system settings. "
                    + "Default: absent (online recommendations enabled). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "online", "recommendations", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Online troubleshooting recommendations from Microsoft not fetched or applied.",
                ApplyOps = [RegOp.SetDword(SdProv, "EnabledPolicy", 0)],
                RemoveOps = [RegOp.DeleteValue(SdProv, "EnabledPolicy")],
                DetectOps = [RegOp.CheckDword(SdProv, "EnabledPolicy", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-recommended-troubleshooting",
                Label = "Disable Windows Recommended Troubleshooting",
                Category = "PowerShell",
                Description =
                    "Sets TurnOffWindowsErrorReportingServer=1 in the AllowRecommendations "
                    + "Troubleshooting policy key. Disables the 'Recommended troubleshooting' feature "
                    + "that automatically diagnoses and resolves common problems. Prevents Windows from "
                    + "silently applying fixes based on crash data from Windows Error Reporting. "
                    + "Default: absent. Recommended: 1 when automated fixes are undesired in production environments.",
                Tags = ["diagnostics", "recommended", "auto-fix", "troubleshooter", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Windows Recommended Troubleshooting feature disabled; no automatic problem fixes applied.",
                ApplyOps = [RegOp.SetDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
                RemoveOps = [RegOp.DeleteValue(TshootKey, "TurnOffWindowsErrorReportingServer")],
                DetectOps = [RegOp.CheckDword(TshootKey, "TurnOffWindowsErrorReportingServer", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-automatic-maintenance-diagnostics",
                Label = "Disable Automatic Maintenance Diagnostics",
                Category = "PowerShell",
                Description =
                    "Sets EnableAutomatedTroubleshooting=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows Automatic Maintenance from running scripted diagnostic jobs "
                    + "in the background during maintenance windows. Avoids unexpected system changes from "
                    + "background maintenance troubleshooters. "
                    + "Default: absent (enabled). Recommended: 0 in change-controlled environments.",
                Tags = ["diagnostics", "maintenance", "automated", "background", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Scripted diagnostic jobs from Windows Automatic Maintenance are disabled.",
                ApplyOps = [RegOp.SetDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "EnableAutomatedTroubleshooting")],
                DetectOps = [RegOp.CheckDword(SdKey, "EnableAutomatedTroubleshooting", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-elevated-troubleshooter",
                Label = "Disable Elevated Scripted Troubleshooter Execution",
                Category = "PowerShell",
                Description =
                    "Sets RunAsHighestAvailablePrivilege=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from automatically requesting elevation to "
                    + "run with highest available privileges. Forces diagnostics to run as standard user "
                    + "unless explicitly elevated by an administrator. "
                    + "Default: absent (auto-elevation allowed). Recommended: 0 on principle-of-least-privilege systems.",
                Tags = ["diagnostics", "elevation", "uac", "privilege", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic packages cannot auto-elevate; admin must explicitly run elevated troubleshooters.",
                ApplyOps = [RegOp.SetDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "RunAsHighestAvailablePrivilege")],
                DetectOps = [RegOp.CheckDword(SdKey, "RunAsHighestAvailablePrivilege", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-results-upload",
                Label = "Disable Diagnostic Results Upload",
                Category = "PowerShell",
                Description =
                    "Sets AllowDiagnosticDataUpload=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents scripted diagnostic packages from uploading their results logs, "
                    + "diagnostic data, or anonymised telemetry to Microsoft or third-party servers. "
                    + "Default: absent (upload allowed). Recommended: 0 on air-gapped or privacy-sensitive systems.",
                Tags = ["diagnostics", "upload", "telemetry", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Diagnostic results not uploaded; data stays on-device.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowDiagnosticDataUpload", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowDiagnosticDataUpload")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowDiagnosticDataUpload", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-user-initiated-troubleshooter",
                Label = "Block User-Initiated Troubleshooters",
                Category = "PowerShell",
                Description =
                    "Sets DisableUserDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents non-administrator users from launching troubleshooters from Settings "
                    + "('Get help', 'Troubleshoot', 'Fix problems'). Only administrators can initiate "
                    + "diagnostic packages. Useful on shared or terminal-server machines. "
                    + "Default: absent (users can launch troubleshooters). Recommended: 1 on kiosk/terminal machines.",
                Tags = ["diagnostics", "user", "kiosk", "restrict", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Non-admin users cannot launch Windows troubleshooters from Settings.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableUserDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableUserDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableUserDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-third-party-diagnostics",
                Label = "Block Third-Party Diagnostic Packages",
                Category = "PowerShell",
                Description =
                    "Sets AllowThirdPartyDiagnostics=0 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from running scripted diagnostic packages (.diagpkg) from publishers "
                    + "other than Microsoft. Only Microsoft-signed diagnostic packages are permitted to run. "
                    + "Default: absent (third-party packages allowed). Recommended: 0 to limit diagnostic execution surface.",
                Tags = ["diagnostics", "third-party", "packages", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Third-party diagnostic packages (.diagpkg) blocked; only Microsoft-signed packages run.",
                ApplyOps = [RegOp.SetDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "AllowThirdPartyDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "AllowThirdPartyDiagnostics", 0)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-scheduled-diagnostics",
                Label = "Disable Scheduled Diagnostic Tasks",
                Category = "PowerShell",
                Description =
                    "Sets DisableScheduledDiagnostics=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents the Scheduled Maintenance Diagnostics task scheduler jobs from creating "
                    + "or running scripted diagnostic tasks in the background on a schedule. "
                    + "Reduces background system load and unexpected modifications. "
                    + "Default: absent (scheduled diagnostics run). Recommended: 1 on optimised/stable systems.",
                Tags = ["diagnostics", "scheduled", "maintenance", "task-scheduler", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Scheduled background diagnostic maintenance tasks are blocked.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableScheduledDiagnostics", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableScheduledDiagnostics")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableScheduledDiagnostics", 1)],
            },
            new TweakDef
            {
                Id = "sdiag-disable-troubleshooting-history",
                Label = "Disable Troubleshooting History Storage",
                Category = "PowerShell",
                Description =
                    "Sets DisableTroubleshootingHistory=1 in the ScriptedDiagnostics policy key. "
                    + "Prevents Windows from writing troubleshooter run results and histories to the "
                    + "machine's troubleshooting log database. Reduces local data accumulation from "
                    + "diagnostic activities. "
                    + "Default: absent (history stored). Recommended: 1 on privacy-focused or ephemeral systems.",
                Tags = ["diagnostics", "history", "privacy", "logging", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                ImpactNote = "Troubleshooter run history and results are not stored in the local database.",
                ApplyOps = [RegOp.SetDword(SdKey, "DisableTroubleshootingHistory", 1)],
                RemoveOps = [RegOp.DeleteValue(SdKey, "DisableTroubleshootingHistory")],
                DetectOps = [RegOp.CheckDword(SdKey, "DisableTroubleshootingHistory", 1)],
            },
        ];
    }

    // ── WindowsTerminalAdvancedPolicy ──
    private static class _WindowsTerminalAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsTerminal\Updates";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "termadv-disable-auto-update",
                    Label = "Disable Windows Terminal Auto-Update",
                    Category = "PowerShell",
                    Description =
                        "Disables automatic update checks and downloads for Windows Terminal, ensuring the terminal version is managed by WSUS or package management rather than in-app updates.",
                    Tags = ["terminal", "update", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Windows Terminal will not auto-update; version management via package manager or WSUS.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-telemetry",
                    Label = "Disable Windows Terminal Telemetry",
                    Category = "PowerShell",
                    Description =
                        "Disables usage telemetry collection in Windows Terminal including keyboard shortcut usage, profile creation frequency, and renderer performance data.",
                    Tags = ["terminal", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal telemetry disabled; no usage data sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-store-launch",
                    Label = "Disable Store Launch from Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Prevents Windows Terminal from launching the Microsoft Store for extensions, themes, or profile suggestions, reducing MS Store telemetry exposure.",
                    Tags = ["terminal", "store", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "MS Store launch button in terminal disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStoreLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStoreLaunch")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStoreLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-startup-tasks",
                    Label = "Disable Windows Terminal Startup Tasks",
                    Category = "PowerShell",
                    Description =
                        "Disables Windows Terminal startup task registration that auto-starts terminal on user login, reducing unnecessary background process startup.",
                    Tags = ["terminal", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal does not auto-launch at logon.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableStartupTasks", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableStartupTasks")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableStartupTasks", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-enforce-restricted-profile",
                    Label = "Enforce Restricted Profile in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Enables restricted profile enforcement in Windows Terminal, blocking users from modifying terminal profiles, settings JSON, or key bindings.",
                    Tags = ["terminal", "profile", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Users cannot modify terminal settings; only admin-defined profiles are available.",
                    ApplyOps = [RegOp.SetDword(Key, "EnforceRestrictedProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnforceRestrictedProfile")],
                    DetectOps = [RegOp.CheckDword(Key, "EnforceRestrictedProfile", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-extensions",
                    Label = "Disable Windows Terminal Extensions",
                    Category = "PowerShell",
                    Description =
                        "Disables the ability to install or run third-party extensions in Windows Terminal, reducing the attack surface from unvetted extension code execution.",
                    Tags = ["terminal", "extensions", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminal extensions disabled; only built-in functionality available.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-ssh-agent",
                    Label = "Block SSH Agent Integration in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Disables the SSH agent forwarding integration in Windows Terminal, preventing terminal sessions from forwarding SSH keys to remote hosts.",
                    Tags = ["terminal", "ssh", "agent", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "SSH agent forwarding blocked from terminal; prevents key forwarding to hostile servers.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockSshAgentIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockSshAgentIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockSshAgentIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-preview-builds",
                    Label = "Disable Windows Terminal Preview Build Channel",
                    Category = "PowerShell",
                    Description =
                        "Forces Windows Terminal to the stable release channel, disabling the Preview and Canary build channels to ensure only stable, vetted versions are used.",
                    Tags = ["terminal", "preview", "channel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Terminal locked to stable channel; Preview/Canary builds not offered.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisablePreviewBuilds", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisablePreviewBuilds")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisablePreviewBuilds", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-disable-update-notifications",
                    Label = "Disable Update Notifications in Windows Terminal",
                    Category = "PowerShell",
                    Description =
                        "Suppresses in-app update available notifications in Windows Terminal, which can distract users and prompt unauthorized manual updates.",
                    Tags = ["terminal", "update", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Update reminder banners not shown in terminal.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUpdateNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUpdateNotifications")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUpdateNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "termadv-block-manual-updates",
                    Label = "Block Manual Windows Terminal Updates by Users",
                    Category = "PowerShell",
                    Description =
                        "Prevents standard users from triggering manual Windows Terminal update checks or downloads, ensuring that all terminal update operations require administrator rights.",
                    Tags = ["terminal", "update", "restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot manually update terminal; admin action required.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockManualUpdates", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockManualUpdates")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockManualUpdates", 1)],
                },
            ];
    }
}
