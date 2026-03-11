namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Input
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "input-disable-mouse-accel",
            Label = "Disable Mouse Acceleration",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets MouseSpeed/Threshold to zero for raw 1:1 input.",
            Tags = ["input", "mouse", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
        },
        new TweakDef
        {
            Id = "input-fast-keyboard-repeat",
            Label = "Maximize Keyboard Repeat Rate",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets keyboard repeat speed to maximum and delay to shortest.",
            Tags = ["input", "keyboard", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-sticky-keys-prompt",
            Label = "Disable Sticky Keys Prompt (5x Shift)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the Sticky Keys dialog from appearing when pressing Shift 5 times.",
            Tags = ["input", "accessibility", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-tap",
            Label = "Disable Touchpad Tap-to-Click (Perf)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables tap-to-click on precision touchpads to reduce accidental clicks and improve input accuracy. Default: Enabled. Recommended: Disabled for desktop users.",
            Tags = ["input", "touchpad", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-increase-hover-time",
            Label = "Increase Mouse Hover Time (1s)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Increases mouse hover delay from 400ms to 1000ms. Reduces accidental tooltip popups. Options: 400ms (default) / 1000ms. Recommended: 1000ms.",
            Tags = ["input", "mouse", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "1000"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "400"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "1000")],
        },
        new TweakDef
        {
            Id = "input-filter-keys",
            Label = "Disable Filter Keys",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Filter Keys accessibility shortcut that can interfere with gaming.",
            Tags = ["input", "accessibility", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "126"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "input-toggle-keys",
            Label = "Disable Toggle Keys Beep",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Toggle Keys beep when pressing Num/Caps/Scroll Lock.",
            Tags = ["input", "accessibility", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "62"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys", "Flags", "0")],
        },
        new TweakDef
        {
            Id = "input-enhanced-pointer-precision",
            Label = "Disable Enhanced Pointer Precision",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables mouse smoothing for raw 1:1 pointer input.",
            Tags = ["input", "mouse", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
        },
        new TweakDef
        {
            Id = "input-mouse-scroll-lines",
            Label = "Set Mouse Scroll to 5 Lines",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets mouse wheel scroll amount to 5 lines (default 3).",
            Tags = ["input", "mouse", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "3"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
        },
        new TweakDef
        {
            Id = "input-keyboard-delay-zero",
            Label = "Set Keyboard Repeat Delay to Minimum",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets keyboard repeat delay to 0 (minimum) for faster key repeat.",
            Tags = ["input", "keyboard", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "1"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
        },
        new TweakDef
        {
            Id = "input-touch-keyboard-disable",
            Label = "Disable Touch Keyboard Auto-Launch",
            Category = "Input",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables automatic touch keyboard launch via Group Policy.",
            Tags = ["input", "touch", "keyboard"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"],
        },
        new TweakDef
        {
            Id = "input-disable-touch-kb-auto",
            Label = "Disable Touch Keyboard Auto-Invoke",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the touch keyboard from automatically appearing on desktops without touchscreens. Default: Enabled. Recommended: Disabled.",
            Tags = ["input", "touch", "keyboard", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-sticky-keys",
            Label = "Disable Sticky Keys Shortcut",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Sticky Keys shortcut (pressing Shift 5 times). Prevents accidental activation during gaming. Default: Enabled (510). Recommended: Disabled (506).",
            Tags = ["input", "sticky-keys", "gaming", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },
        new TweakDef
        {
            Id = "input-disable-spell-check",
            Label = "Disable Spell Checking",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic spell checking in Windows text input. Reduces CPU usage from background spell-check processing. Default: Enabled. Recommended: Disabled for developers.",
            Tags = ["input", "spell-check", "typing", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-text-suggestions",
            Label = "Disable Text Suggestions",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables text prediction and suggestions while typing. Prevents the suggestion bar from appearing above the keyboard. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["input", "text-prediction", "suggestions", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "input-set-cursor-blink-rate",
            Label = "Set Fast Cursor Blink Rate",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the cursor blink rate to 400 ms (faster than default). Makes the text cursor more visible and responsive. Default: 530 ms. Recommended: 400 ms for faster feedback.",
            Tags = ["input", "cursor", "blink-rate", "typing", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "530"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400")],
        },
        new TweakDef
        {
            Id = "input-increase-double-click-speed",
            Label = "Increase Double-Click Speed",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the double-click detection interval to 200 ms for faster response. Requires quicker double-clicks but feels responsive. Default: 500 ms. Recommended: 200 ms for power users.",
            Tags = ["input", "mouse", "double-click", "speed", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "500"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200")],
        },
        new TweakDef
        {
            Id = "input-disable-touch-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables visual feedback animations for touch and pen input. Removes the circle/ripple effect on touch and gesture indicators. Default: Enabled. Recommended: Disabled on non-touch devices.",
            Tags = ["input", "touch", "pen", "feedback", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 31),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-sticky-keys-shortcut",
            Label = "Disable Sticky Keys Shortcut",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Shift×5 shortcut that activates Sticky Keys. Prevents accidental activation during gaming. Default: enabled.",
            Tags = ["input", "sticky-keys", "shortcut", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },
        new TweakDef
        {
            Id = "input-increase-keyboard-repeat-rate",
            Label = "Increase Keyboard Repeat Rate",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets keyboard repeat rate to maximum (31). Makes held keys repeat faster. Default: 31.",
            Tags = ["input", "keyboard", "repeat", "rate"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardSpeed", "31")],
        },
        new TweakDef
        {
            Id = "input-reduce-keyboard-delay",
            Label = "Reduce Keyboard Repeat Delay",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the delay before a held key starts repeating to minimum (0 = ~250ms). Default: 1 (~500ms).",
            Tags = ["input", "keyboard", "delay", "repeat"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Keyboard"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-trails",
            Label = "Disable Mouse Pointer Trails",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the mouse pointer trail effect. Default: disabled.",
            Tags = ["input", "mouse", "trails", "pointer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseTrails", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-snap-mouse-to-button",
            Label = "Disable Snap Mouse to Default Button",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatically snapping the mouse cursor to the default button in dialog boxes. Default: disabled.",
            Tags = ["input", "mouse", "snap", "dialog"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "SnapToDefaultButton", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "SnapToDefaultButton", "0")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "SnapToDefaultButton", "0")],
        },
    ];
}
