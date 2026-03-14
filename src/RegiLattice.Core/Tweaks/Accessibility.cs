namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Accessibility
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "acc-disable-accessibility-shortcuts",
            Label = "Disable Sticky/Toggle/Filter Keys",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the Sticky Keys, Toggle Keys, and Filter Keys popups triggered by repeated key presses.",
            Tags = ["accessibility", "keyboard", "gaming"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys",
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys",
                @"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response",
            ],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "126"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },
        new TweakDef
        {
            Id = "acc-force-dark-mode",
            Label = "Force System-Wide Dark Mode",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables dark mode for both Windows system chrome and applications via the Personalize theme registry.",
            Tags = ["accessibility", "theme", "dark-mode"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "AppsUseLightTheme", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-animations",
            Label = "Disable Window Animations",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables desktop window animations (Aero Peek, minimize/maximize effects) for snappier UI.",
            Tags = ["accessibility", "performance", "animation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", @"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "UserPreferencesMask", "90 12 03 80 10 00 00 00"),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 1),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "UserPreferencesMask", "9E 3E 07 80 12 01 00 00"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM", "EnableAeroPeek", 0)],
        },
        new TweakDef
        {
            Id = "acc-enable-cleartype",
            Label = "Enable ClearType Font Smoothing",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables ClearType sub-pixel font rendering for sharper text.",
            Tags = ["accessibility", "font", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 2),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "0"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothingType", 0),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FontSmoothing", "2")],
        },
        new TweakDef
        {
            Id = "acc-wide-scrollbar",
            Label = "Increase Scroll Bar Width",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases scroll bar width from default (17px) to 25px for easier targeting with mouse or touch.",
            Tags = ["accessibility", "ui", "scrollbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-400"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-400"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-255"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollHeight", "-255"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop\WindowMetrics", "ScrollWidth", "-400")],
        },
        new TweakDef
        {
            Id = "acc-disable-narrator",
            Label = "Disable Narrator Auto-Start",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from starting at login or via Win+Enter.",
            Tags = ["accessibility", "narrator", "screen-reader"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-high-contrast-mode",
            Label = "Enable High Contrast Mode",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables high contrast mode for improved visual clarity.",
            Tags = ["accessibility", "contrast", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "127")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "127")],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-hotkey",
            Label = "Disable Magnifier Win+Plus Hotkey",
            Category = "Accessibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Win+Plus hotkey that launches Magnifier and prevents it from running.",
            Tags = ["accessibility", "magnifier", "hotkey"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier",
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableMagnifier", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableMagnifier"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "RunningState", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-osk-auto-launch",
            Label = "Disable On-Screen Keyboard Auto-Launch",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the On-Screen Keyboard from automatically launching at startup or when entering tablet mode.",
            Tags = ["accessibility", "keyboard", "osk", "tablet"],
            RegistryKeys =
            [
                @"HKEY_CURRENT_USER\Software\Microsoft\Osk",
                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode", "OpenStandby", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\TabletMode", "OpenStandby", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Osk", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-underline-shortcuts",
            Label = "Disable Menu Access Key Underlines",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the underline indicators on menu access keys (keyboard shortcuts) for a cleaner UI.",
            Tags = ["accessibility", "keyboard", "menu", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-sound-sentry",
            Label = "Disable Visual Sound Alerts",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables SoundSentry visual alerts that flash the screen or window when a system sound plays.",
            Tags = ["accessibility", "sound", "visual-alert"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "2")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-access-disable-narrator-autostart",
            Label = "Disable Narrator Autostart",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from launching with Win+Enter shortcut. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["accessibility", "narrator", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-filter-keys",
            Label = "Disable Filter Keys Shortcut",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Filter Keys shortcut, preventing accidental activation that can interfere with typing and gaming.",
            Tags = ["accessibility", "filter-keys", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-toggle-keys",
            Label = "Disable Toggle Keys Shortcut",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Toggle Keys shortcut that plays a tone when Caps Lock, Num Lock, or Scroll Lock is pressed.",
            Tags = ["accessibility", "toggle-keys", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-narrator-hotkey",
            Label = "Disable Narrator Hotkey (Win+Enter)",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Win+Enter keyboard shortcut that launches Narrator. Prevents accidental Narrator activation. Default: Enabled. Recommended: Disabled.",
            Tags = ["accessibility", "narrator", "hotkey", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled"),
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "RunNarratorAtLogon"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "WinEnterLaunchEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-increase-hover-time",
            Label = "Increase Tooltip Hover Time",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the mouse hover delay before tooltips appear from 400ms to 1000ms. Reduces accidental tooltip popups. Default: 400ms. Recommended: 1000ms for accessibility.",
            Tags = ["accessibility", "mouse", "hover", "tooltip", "delay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "1000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "400")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "MouseHoverTime", "1000")],
        },
        new TweakDef
        {
            Id = "acc-widen-caret",
            Label = "Widen Text Cursor (Caret)",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the text cursor (caret) width from 1px to 3px. Makes the blinking text cursor easier to locate visually. Default: 1px. Recommended: 3px for visibility.",
            Tags = ["accessibility", "caret", "cursor", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 3)],
        },
        new TweakDef
        {
            Id = "acc-increase-focus-border",
            Label = "Increase Focus Border Width",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases the focus rectangle border from 1px to 3px wide. Makes keyboard-focused controls easier to identify visually. Default: 1px. Recommended: 3px for low vision.",
            Tags = ["accessibility", "focus", "border", "keyboard", "visibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 3),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderHeight", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderHeight", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "FocusBorderWidth", 3)],
        },
        new TweakDef
        {
            Id = "acc-disable-narrator-auto-start",
            Label = "Disable Narrator Auto-Start",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Narrator from starting automatically at sign-in. Default: not auto-started. Useful if accidentally enabled.",
            Tags = ["accessibility", "narrator", "auto-start", "screen-reader"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Narrator\NoRoam", "RunNarratorOnLogon", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-filter-keys-shortcut",
            Label = "Disable Filter Keys Shortcut",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the keyboard shortcut (hold right Shift 8 sec) that activates Filter Keys. Prevents accidental activation during gaming. Default: enabled.",
            Tags = ["accessibility", "filter-keys", "shortcut", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "Flags", "122")],
        },
        new TweakDef
        {
            Id = "acc-enable-underline-access-keys",
            Label = "Always Underline Access Keys",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Shows underlined keyboard shortcuts in menus and dialogs at all times, not just when Alt is pressed. Default: hidden until Alt. Recommended: visible.",
            Tags = ["accessibility", "access-keys", "underline", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Preference", "On", "1")],
        },
        new TweakDef
        {
            Id = "acc-disable-toggle-keys-sound",
            Label = "Disable Toggle Keys Sound",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the audible tone when pressing Caps Lock, Num Lock, or Scroll Lock. Default: enabled. Recommended: disabled.",
            Tags = ["accessibility", "toggle-keys", "sound", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "58")],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-follows-keyboard",
            Label = "Disable Magnifier Follows Keyboard",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Stops the magnifier view from following the keyboard cursor. Useful for users who prefer manual control. Default: follows keyboard.",
            Tags = ["accessibility", "magnifier", "keyboard", "follow"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
        },
        new TweakDef
        {
            Id = "acc-access-disable-magnifier",
            Label = "Disable Magnifier Auto-Start",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents the Magnifier from auto-starting with Windows. Disables the magnifier startup accessibility feature. Default: depends on accessibility settings.",
            Tags = ["accessibility", "magnifier", "autostart", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\ScreenMagnifier", "IsAutoStartEnabled", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-mouse-keys",
            Label = "Disable Mouse Keys",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Mouse Keys accessibility feature that lets you control the cursor with the numeric keypad. Default: user setting.",
            Tags = ["accessibility", "mouse-keys", "numpad", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "62")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys", "Flags", "0")],
        },
    ];
}
