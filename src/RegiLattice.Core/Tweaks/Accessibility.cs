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
        // ── Sprint 21 additions ─────────────────────────────────────────────

        new TweakDef
        {
            Id = "acc-disable-sticky-keys",
            Label = "Disable Sticky Keys",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Sticky Keys which allows modifier keys to remain active after being released. Eliminates the common 5× Shift annoyance during gaming.",
            Tags = ["accessibility", "sticky-keys", "gaming", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-bounce-keys",
            Label = "Disable Bounce Keys",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Bounce Keys which ignores repeated keystrokes within a short time. Removes unwanted key filtering for fast typists.",
            Tags = ["accessibility", "bounce-keys", "keyboard", "filter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "100")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\Keyboard Response", "BounceTime", "0")],
        },
        new TweakDef
        {
            Id = "acc-increase-repeat-rate",
            Label = "Maximize Keyboard Repeat Rate",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the keyboard repeat rate to the fastest setting (31 repeats/sec). Characters repeat quicker when a key is held.",
            Tags = ["accessibility", "keyboard", "repeat-rate", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "20")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31")],
        },
        new TweakDef
        {
            Id = "acc-disable-ease-of-access-hotkey",
            Label = "Disable Ease of Access Center Hotkey",
            Category = "Accessibility",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Win+U hotkey that opens the Ease of Access Center. Frees the shortcut for other uses and prevents accidental opening.",
            Tags = ["accessibility", "hotkey", "ease-of-access", "keyboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "NoDispEasyAccessCPL", 1)],
        },
        new TweakDef
        {
            Id = "acc-disable-auto-correct",
            Label = "Disable Auto-Correct for Touch Keyboard",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic word correction in the Windows touch keyboard. Prevents unwanted text changes during touch input.",
            Tags = ["accessibility", "auto-correct", "touch-keyboard", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableAutocorrection", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-high-contrast-hotkey",
            Label = "Disable High Contrast Hotkey",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Left Alt+Left Shift+Print Screen hotkey for toggling high contrast mode. Prevents accidental theme changes.",
            Tags = ["accessibility", "high-contrast", "hotkey", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\HighContrast", "Flags", "0")],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "acc-disable-narrator-key-echo",
            Label = "Disable Narrator Key Echo",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Narrator from reading aloud each key you press while typing.",
            Tags = ["accessibility", "narrator", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator", "StringKeyEcho", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-magnifier-caret-follow",
            Label = "Disable Magnifier Caret Following",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Magnifier lens from following the text cursor as you type.",
            Tags = ["accessibility", "magnifier"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ScreenMagnifier", "FollowCaret", 0)],
        },
        new TweakDef
        {
            Id = "acc-set-wider-caret",
            Label = "Set Wider Text Cursor (Caret Width)",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases the text cursor (caret) width to 3 pixels for improved visibility.",
            Tags = ["accessibility", "cursor", "text"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", "3")],
        },
        new TweakDef
        {
            Id = "acc-disable-mouse-trails",
            Label = "Disable Mouse Pointer Trails",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes mouse cursor trail effect that can be distracting or cause visual issues.",
            Tags = ["accessibility", "mouse", "cursor"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "-1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "-1")],
        },
        new TweakDef
        {
            Id = "acc-disable-pointer-shadow",
            Label = "Disable Mouse Pointer Shadow",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the drop shadow effect rendered under the mouse pointer.",
            Tags = ["accessibility", "mouse", "cursor", "visuals"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorShadow", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-pointer-precision",
            Label = "Disable Enhanced Pointer Precision",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off Enhanced Pointer Precision (mouse acceleration). Recommended for gaming and precise work.",
            Tags = ["accessibility", "mouse", "gaming", "precision"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "0"),
            ],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "acc-disable-color-filters",
            Label = "Disable Colour Filters",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off colour filter overlays (grayscale, red-green, blue-yellow) used for colour-blind support.",
            Tags = ["accessibility", "colour-filters", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "Active", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-color-filter-hotkey",
            Label = "Disable Colour Filter Hotkey",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Windows+Ctrl+C hotkey that toggles colour filters on/off.",
            Tags = ["accessibility", "colour-filters", "hotkey", "keyboard"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "FilterType", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered", 0),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\ColorFiltering", "HotkeyRegistered", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-audio-description",
            Label = "Disable Audio Descriptions",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Turns off automatic audio descriptions for videos in supported apps.",
            Tags = ["accessibility", "audio", "media"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Narrator\NoRoam", "DuckAudio", 0)],
        },
        new TweakDef
        {
            Id = "acc-disable-visual-notifications",
            Label = "Disable Visual Sound Notifications",
            Category = "Accessibility",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the flash visual alerts that substitute for sound cues (SoundSentry).",
            Tags = ["accessibility", "sound", "visuals", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect", "0")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SoundSentry", "WindowsEffect", "0")],
        },
    ];
}
