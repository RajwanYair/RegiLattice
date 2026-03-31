namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Input
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
            Id = "input-disable-touchpad-tap",
            Label = "Disable Touchpad Tap-to-Click (Perf)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables tap-to-click on precision touchpads to reduce accidental clicks and improve input accuracy. Default: Enabled. Recommended: Disabled for desktop users.",
            Tags = ["input", "touchpad", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-increase-hover-time",
            Label = "Increase Mouse Hover Time (1s)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Increases mouse hover delay from 400ms to 1000ms. Reduces accidental tooltip popups. Options: 400ms (default) / 1000ms. Recommended: 1000ms.",
            Tags = ["input", "mouse", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "1000")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverTime", "400")],
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
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "126")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\FilterKeys", "Flags", "0")],
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
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "3")],
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
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Keyboard", "KeyboardDelay", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-spell-check",
            Label = "Disable Spell Checking",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic spell checking in Windows text input. Reduces CPU usage from background spell-check processing. Default: Enabled. Recommended: Disabled for developers.",
            Tags = ["input", "spell-check", "typing", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-text-suggestions",
            Label = "Disable Text Suggestions",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables text prediction and suggestions while typing. Prevents the suggestion bar from appearing above the keyboard. Default: Enabled. Recommended: Disabled for power users.",
            Tags = ["input", "text-prediction", "suggestions", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "input-set-cursor-blink-rate",
            Label = "Set Fast Cursor Blink Rate",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the cursor blink rate to 400 ms (faster than default). Makes the text cursor more visible and responsive. Default: 530 ms. Recommended: 400 ms for faster feedback.",
            Tags = ["input", "cursor", "blink-rate", "typing", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "530")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CursorBlinkRate", "400")],
        },
        new TweakDef
        {
            Id = "input-increase-double-click-speed",
            Label = "Increase Double-Click Speed",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reduces the double-click detection interval to 200 ms for faster response. Requires quicker double-clicks but feels responsive. Default: 500 ms. Recommended: 200 ms for power users.",
            Tags = ["input", "mouse", "double-click", "speed", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "500")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "DoubleClickSpeed", "200")],
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
        new TweakDef
        {
            Id = "input-disable-mouse-accel",
            Label = "Disable Mouse Acceleration",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables mouse acceleration (enhanced pointer precision). Provides 1:1 mouse movement for gaming and precision work. Default: acceleration enabled.",
            Tags = ["input", "mouse", "acceleration", "gaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "0"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold1", "6"),
                RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseThreshold2", "10"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "input-enhanced-pointer-precision",
            Label = "Disable Enhanced Pointer Precision",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Windows Enhanced Pointer Precision feature. Provides raw unfiltered mouse input for consistent cursor movement. Default: enabled.",
            Tags = ["input", "pointer", "precision", "raw"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSensitivity", "10")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSensitivity", "10")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSensitivity", "10")],
        },
        // ── Sprint 19 additions ────────────────────────────────────────────
        new TweakDef
        {
            Id = "input-disable-feedback-hub",
            Label = "Disable Feedback Hub Prompts",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Stops Windows from periodically asking for feedback via the Feedback Hub app. Default: enabled.",
            Tags = ["input", "feedback", "hub", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Siuf\Rules", "NumberOfSIUFInPeriod", 0)],
        },
        new TweakDef
        {
            Id = "input-set-wheel-scroll-chars",
            Label = "Set Horizontal Scroll to 3 Characters",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the horizontal mouse wheel scroll distance to 3 characters per notch. Default: 3.",
            Tags = ["input", "mouse", "scroll", "horizontal"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollChars", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollChars", "3")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollChars", "3")],
        },
        new TweakDef
        {
            Id = "input-disable-pen-workspace",
            Label = "Disable Pen Workspace",
            Category = "Input",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Pen Workspace (Screen Sketch, Sticky Notes shortcut). Useful on non-pen devices. Default: enabled.",
            Tags = ["input", "pen", "workspace", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-handwriting-panel",
            Label = "Disable Handwriting Panel",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the handwriting input panel from appearing on touch/pen input. Default: enabled on pen devices.",
            Tags = ["input", "handwriting", "panel", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "input-set-mouse-hover-width",
            Label = "Increase Mouse Hover Detection Area",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Widens the mouse hover detection rectangle from 4 to 10 pixels. Reduces accidental tooltip triggering. Default: 4.",
            Tags = ["input", "mouse", "hover", "detection"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "10")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "4")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverWidth", "10")],
        },
        new TweakDef
        {
            Id = "input-disable-writing-insights",
            Label = "Disable Writing Insights",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Writing Insights that analyse text input for suggestions. Improves privacy. Default: enabled.",
            Tags = ["input", "writing", "insights", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Input\Settings", "InsightsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-set-mouse-pointer-speed",
            Label = "Set Mouse Pointer Speed to Maximum",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the mouse pointer speed to maximum (20). Useful for high-resolution displays. Default: 10.",
            Tags = ["input", "mouse", "speed", "pointer"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "2")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "2")],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-sonar",
            Label = "Disable Mouse Sonar (Show Pointer on Ctrl Press)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MouseSonar=0 in Control Panel\\Mouse. Disables the feature that briefly shows animated rings around the mouse pointer when Ctrl is pressed to locate it.",
            Tags = ["input", "mouse", "sonar", "animation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSonar", "0")],
        },
        new TweakDef
        {
            Id = "input-disable-mouse-vanish",
            Label = "Disable Mouse Vanish (Hide Pointer While Typing)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MouseVanish=0 in Control Panel\\Mouse. Stops Windows from hiding the mouse pointer whenever the user is typing. Useful when working across multiple monitors.",
            Tags = ["input", "mouse", "vanish", "typing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseVanish", "0")],
        },
        new TweakDef
        {
            Id = "input-set-caret-width-2",
            Label = "Set Text Cursor (Caret) Width to 2 px",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets CaretWidth=2 in Desktop. Widens the blinking text insertion caret from the default 1 px to 2 px, making it easier to spot in dense code or document editors.",
            Tags = ["input", "caret", "cursor", "accessibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "CaretWidth", 2)],
        },
        new TweakDef
        {
            Id = "input-disable-lang-switch-hotkey",
            Label = "Disable Keyboard Language Switch Hotkey (Alt+Shift)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ToggleHotkey=3 in Keyboard Layout\\Toggle. Disables the hotkey that switches between input languages (default: Left Alt+Shift or Ctrl+Shift), preventing accidental layout changes mid-sentence.",
            Tags = ["input", "keyboard", "language", "hotkey"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Keyboard Layout\Toggle"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "3")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Keyboard Layout\Toggle", "ToggleHotkey", "3")],
        },
        new TweakDef
        {
            Id = "input-set-mouse-hover-height",
            Label = "Set Mouse Hover Area Height to 4 px",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets MouseHoverHeight=4 in Control Panel\\Mouse. Defines the vertical size of the hover rectangle; keeping it small prevents unintended hover activations on dense UIs.",
            Tags = ["input", "mouse", "hover", "precision"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverHeight", "4")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverHeight", "4")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseHoverHeight", "4")],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-two-finger-tap",
            Label = "Disable Precision Touchpad Two-Finger Tap (Right-Click)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets TwoFingerTapEnabled=0 in PrecisionTouchPad. Disables the two-finger tap gesture that normally triggers a right-click, useful when two-finger scrolling also fires unintended context menus.",
            Tags = ["input", "touchpad", "gesture", "tap"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-edge-swipe",
            Label = "Disable Precision Touchpad Edge Swipe Gestures",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets EdgeSwipesEnabled=0 in PrecisionTouchPad. Disables swipe-from-edge gestures (open Action Center, Task View, etc.) that can fire accidentally near the trackpad border.",
            Tags = ["input", "touchpad", "edge", "swipe"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "EdgeSwipesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-three-finger-tap",
            Label = "Disable Precision Touchpad Three-Finger Tap",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ThreeFingerTapEnabled=0 in PrecisionTouchPad. Prevents the three-finger tap from opening Cortana/Search, a common source of unintended gesture activations during fast typing.",
            Tags = ["input", "touchpad", "gesture", "three-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-four-finger-tap",
            Label = "Disable Precision Touchpad Four-Finger Tap",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets FourFingerTapEnabled=0 in PrecisionTouchPad. Disables the four-finger tap gesture that opens the Action Center, preventing accidental panel opens.",
            Tags = ["input", "touchpad", "gesture", "four-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-right-zone",
            Label = "Disable Precision Touchpad Right-Click Corner Zone",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets RightClickZoneEnabled=0 in PrecisionTouchPad. Removes the dedicated bottom-right corner tap zone that fires a right-click, helping users who accidentally trigger it.",
            Tags = ["input", "touchpad", "right-click", "zone"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 0)],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "RightClickZoneEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-swipe-nav",
            Label = "Disable Precision Touchpad Swipe Navigation (Back/Forward)",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SwipeNavigationEnabled=0 in PrecisionTouchPad. Disables three-finger left/right swipe gestures that navigate back and forward in browsers and File Explorer.",
            Tags = ["input", "touchpad", "swipe", "navigation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "SwipeNavigationEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "input-disable-touchpad-pinch-zoom",
            Label = "Disable Precision Touchpad Pinch-to-Zoom",
            Category = "Input",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets PinchZoomEnabled=0 in PrecisionTouchPad. Disables the two-finger pinch gesture that zooms in/out in supported apps and the browser, preventing accidental zoom changes.",
            Tags = ["input", "touchpad", "pinch", "zoom"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "PinchZoomEnabled", 0)],
        },
    ];
}

// === Merged from: TouchPen.cs ===


internal static class TouchPen
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "touch-disable-ink-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Ink Workspace via Group Policy. Hides the Ink Workspace button and features.",
            Tags = ["touch", "pen", "ink", "workspace", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-button",
            Label = "Hide Pen Workspace Taskbar Button",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar. Pen still works; only the shortcut button is hidden.",
            Tags = ["touch", "pen", "taskbar", "button"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-ink-suggestions",
            Label = "Disable Ink Work Suggested Apps",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes suggested apps from the Windows Ink Workspace. Policy setting.",
            Tags = ["touch", "pen", "ink", "suggestions", "ads"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace", "AllowSuggestedAppsInWindowsInkWorkspace", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-pen-screenshot",
            Label = "Pen Double-Click: Screen Sketch",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Maps pen button double-click to Screen Sketch (screenshot annotation). Default: Nothing.",
            Tags = ["touch", "pen", "screenshot", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "DoubleClickAction", 4)],
        },
        new TweakDef
        {
            Id = "touch-pen-longpress",
            Label = "Pen Long-Press: Ink Workspace",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Maps pen button long-press to open the Ink Workspace. Default: Nothing.",
            Tags = ["touch", "pen", "workspace", "shortcut"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen", "LongPressAction", 3)],
        },
        new TweakDef
        {
            Id = "touch-disable-tablet-auto",
            Label = "Disable Tablet Mode Auto-Switch",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from switching to tablet mode when a keyboard is detached or folded.",
            Tags = ["touch", "tablet", "mode", "convertible"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1),
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell",
                    "ConvertibleSlateModePromptPreference",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode"),
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell",
                    "ConvertibleSlateModePromptPreference"
                ),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "SignInMode", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-3finger",
            Label = "Disable Three-Finger Gestures",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables three-finger tap and slide gestures on precision touchpads (task view, volume, etc.).",
            Tags = ["touch", "touchpad", "gestures", "three-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ThreeFingerSlideEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-4finger",
            Label = "Disable Four-Finger Gestures",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables four-finger tap and slide gestures on precision touchpads (desktop switch, etc.).",
            Tags = ["touch", "touchpad", "gestures", "four-finger"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerTapEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "FourFingerSlideEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-edge-swipe",
            Label = "Disable Edge Swipe Gesture (Policy)",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the screen-edge swipe gesture that opens Action Centre / notification pane. Prevents accidental triggers.",
            Tags = ["touch", "edge", "swipe", "gesture", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EdgeUI", "DisableSwipe", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-flicks",
            Label = "Disable Pen Flick Gestures",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables pen flick gestures (quick strokes for scroll, back, forward). Prevents accidental navigation.",
            Tags = ["touch", "pen", "flicks", "gestures"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Pen\SysEventParameters", "FlicksEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-2finger-tap",
            Label = "Disable Two-Finger Tap (Right-Click)",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables two-finger tap as a right-click gesture on precision touchpads. Prevents accidental right-click menus while typing. Default: Enabled.",
            Tags = ["touch", "touchpad", "two-finger", "right-click", "gesture"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 1)],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "TwoFingerTapEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-pinch-zoom",
            Label = "Disable Touchpad Pinch-to-Zoom",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the two-finger pinch-to-zoom gesture on precision touchpads. Prevents accidental zoom changes. Default: Enabled.",
            Tags = ["touch", "touchpad", "pinch", "zoom", "gesture"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ZoomEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-reverse-scroll",
            Label = "Enable Reverse (Natural) Scrolling",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Reverses touchpad scroll direction so content moves in the same direction as your fingers (natural/Mac-style scrolling). Default: Traditional.",
            Tags = ["touch", "touchpad", "scroll", "direction", "natural"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "ScrollDirection", 0)],
        },
        new TweakDef
        {
            Id = "touch-sensitivity-high",
            Label = "Set Touchpad Sensitivity to High",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets precision touchpad sensitivity to maximum (AAPThreshold=0). Registers even the lightest touch. Default: Medium (2). Recommended: High for light-touch users.",
            Tags = ["touch", "touchpad", "sensitivity", "threshold"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "AAPThreshold", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-palm-rejection",
            Label = "Disable Touchpad Palm Rejection",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables touchpad palm rejection and cursor-leave detection. Useful when palm rejection causes missed inputs. Default: Enabled.",
            Tags = ["touch", "touchpad", "palm", "rejection"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad", "LeaveOnEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the visual ripple effect when touching the screen. Default: enabled.",
            Tags = ["touch", "feedback", "visual", "disable"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-gesture-feedback",
            Label = "Disable Touch Gesture Visual Feedback",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the visual feedback for multi-finger gestures (pinch, swipe). Default: enabled.",
            Tags = ["touch", "gesture", "feedback", "visual"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 31)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "GestureVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-screen",
            Label = "Disable Touch Screen Input (HID)",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the HID-compliant touch screen. Touch will not work until re-enabled. Default: enabled.",
            Tags = ["touch", "screen", "hid", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "TurnOffTouchInput", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-handwriting-panel",
            Label = "Disable Pen Handwriting Panel Auto-Invoke",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents the handwriting panel from auto-appearing when using a pen in text fields. Default: auto.",
            Tags = ["touch", "pen", "handwriting", "panel"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-workspace",
            Label = "Disable Pen Workspace Button",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Hides the Windows Ink Workspace button from the taskbar system tray. Default: visible.",
            Tags = ["touch", "pen", "ink-workspace", "taskbar"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-visual-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the visual feedback animations shown when touching the screen. Removes the touch ripple effects. Default: enabled.",
            Tags = ["touch", "visual", "feedback", "animation"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Cursors"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Cursors", "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-flicks-policy",
            Label = "Disable Pen Flicks via Group Policy",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables pen flicks (gesture shortcuts) system-wide via Group Policy. Default: pen flicks enabled.",
            Tags = ["touch", "pen", "flicks", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC", "TurnOffFlicks", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-ink-workspace-app-suggestions",
            Label = "Disable Ink Workspace App Suggestions",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables suggested app recommendations inside the Windows Ink Workspace. Default: enabled.",
            Tags = ["touch", "pen", "ink", "suggestions"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceAppSuggestionsEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-handwriting-panel-auto",
            Label = "Disable Handwriting Panel Auto-Invoke",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic pop-up of the touch handwriting panel when a text field is focussed with a pen. Default: auto-show enabled.",
            Tags = ["touch", "pen", "handwriting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-keyboard-deploy",
            Label = "Disable Auto Touch Keyboard Deployment",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic deployment of the touch keyboard when a text field is tapped. Useful when using an external keyboard with touch input. Default: auto-deploy.",
            Tags = ["touch", "keyboard", "auto"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableDesktopModeAutoInvoke", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-keyboard-suggestions",
            Label = "Disable Touch Keyboard Autocomplete Suggestions",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables word suggestions shown above the touch keyboard. Reduces distraction and speeds up touch typing. Default: suggestions enabled.",
            Tags = ["touch", "keyboard", "suggestions", "autocomplete"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Input\Settings", "EnableHwKeyboardAutocorrect", 0)],
        },
        new TweakDef
        {
            Id = "touch-set-double-tap-speed",
            Label = "Reduce Pen Double-Tap Detection Speed",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Reduces the time window for pen double-tap detection. Makes double-tap feel snappier. Default: 500ms window.",
            Tags = ["touch", "pen", "double-tap", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout", 200)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Accessibility\SlateLaunch", "ATWRelaxTimeout", 200)],
        },
        new TweakDef
        {
            Id = "touch-disable-tablet-pc-input-service",
            Label = "Disable Tablet PC Input Service",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Tablet PC Input Service which supports pen and touch functionality on non-tablet PCs. Frees resources on desktop systems. Default: enabled.",
            Tags = ["touch", "tablet-pc", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TabletInputService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-flick-sound",
            Label = "Disable Pen Flick Sound",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the sound effect played when performing pen flick gestures. Default: sound enabled.",
            Tags = ["touch", "pen", "sound", "flick"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Tablet", "FlickSoundEnabled", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-touch-prediction",
            Label = "Disable Touch Input Prediction",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables predictive touch movement smoothing. Reduces latency for precise stylus work. Default: prediction enabled.",
            Tags = ["touch", "pen", "prediction", "latency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Precision Touchpad", "TouchPredictionEnabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-pen-workspace-button",
            Label = "Hide Pen Workspace Button from Taskbar",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Removes the pen workspace icon from the taskbar notification area. Default: visible when pen is attached.",
            Tags = ["touch", "pen", "taskbar", "ink"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace", "PenWorkspaceButtonDesiredVisibility"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\PenWorkspace",
                    "PenWorkspaceButtonDesiredVisibility",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-autocorrect",
            Label = "Disable Touch Keyboard Auto-Correct",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic text correction on the touch keyboard. Keeps typed words exactly as entered. Default: autocorrect enabled.",
            Tags = ["touch", "keyboard", "autocorrect"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableAutoCorrection", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-text-suggestions",
            Label = "Disable Touch Keyboard Text Suggestions",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables word prediction suggestions on the touch keyboard. Reduces distraction during touch typing. Default: text suggestions enabled.",
            Tags = ["touch", "keyboard", "suggestions", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableTextPrediction", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableTextPrediction", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-spell-check",
            Label = "Disable Touch Keyboard Spell Check",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables spell checking on the touch keyboard. Removes red underlines for misspelled words during touch input. Default: spell check enabled.",
            Tags = ["touch", "keyboard", "spellcheck"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableSpellchecking", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "EnableSpellchecking", 0)],
        },
        new TweakDef
        {
            Id = "touch-hide-tip-band",
            Label = "Hide Touch Keyboard Tip Band",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Hides the handwriting panel tip band that appears above the touch keyboard. Gives more screen space during handwriting input. Default: tip band visible.",
            Tags = ["touch", "keyboard", "handwriting", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\TabletTip\1.7", "TipbandDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "touch-restrict-handwriting-personalization",
            Label = "Restrict Handwriting Personalization via Policy",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows from learning your handwriting style. Disables handwriting personalization data collection. Default: personalization enabled.",
            Tags = ["touch", "pen", "handwriting", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingPersonalization", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingPersonalization")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC", "PreventHandwritingPersonalization", 1),
            ],
        },
        new TweakDef
        {
            Id = "touch-disable-tablet-mode-auto-switch",
            Label = "Disable Automatic Tablet Mode Switching",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically switching to tablet mode when a keyboard is detached. Keeps desktop mode at all times. Default: auto-switch enabled on convertibles.",
            Tags = ["touch", "tablet-mode", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell", "TabletMode", 0)],
        },
        new TweakDef
        {
            Id = "touch-disable-auto-keyboard-invoke",
            Label = "Disable Automatic Touch Keyboard Pop-Up",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the touch keyboard from automatically appearing when a text input field is focused on a touchscreen. Requires manual keyboard invocation. Default: auto-invoke enabled.",
            Tags = ["touch", "keyboard", "auto-invoke", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsTouchKeyboard", "DisableAutoKeyboard", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-input-personalization",
            Label = "Disable Text Input Personalisation Data Collection",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Stops Windows from collecting text input samples to improve touch/keyboard recognition. Removes implicit text data upload. Default: collection enabled.",
            Tags = ["touch", "keyboard", "privacy", "personalization"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\InputPersonalization", "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "touch-disable-edge-gesture",
            Label = "Disable Touch Edge Gesture Swipe",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the edge swipe gesture that opens Action Center or other system panels on touchscreen devices. Reduces accidental activation. Default: edge gestures enabled.",
            Tags = ["touch", "gesture", "edge-swipe", "ui"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ImmersiveShell\EdgeUi", "DisabledEdges", 3)],
        },
    ];
}

// ── Merged from WindowsInk.cs ──────────────────────────────────────────────────

internal static class WindowsInk
{
    private const string InkWorkspace = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    private const string InkUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PenWorkspace";

    private const string PenSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Pen";

    private const string InkPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

    private const string SuggestionPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InputPersonalization";

    private const string CursorFeedback = @"HKEY_CURRENT_USER\Control Panel\Cursors";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "ink-disable-workspace",
            Label = "Disable Windows Ink Workspace",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "workspace", "pen", "stylus", "disable"],
            Description =
                "Disables the Windows Ink Workspace button in the system tray and the "
                + "entire Ink Workspace feature. AllowWindowsInkWorkspace=0. "
                + "Useful on non-pen devices where it just adds clutter.",
            ApplyOps = [RegOp.SetDword(InkWorkspace, "AllowWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(InkWorkspace, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(InkWorkspace, "AllowWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-workspace-above-lock",
            Label = "Disable Windows Ink Workspace Access Above Lock Screen",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["ink", "workspace", "lock screen", "security"],
            Description =
                "Prevents the Windows Ink Workspace from being opened from the lock screen "
                + "(AllowWindowsInkWorkspace=1, accessible above lock=no). "
                + "Enforces that ink features require a signed-in session.",
            ApplyOps = [RegOp.SetDword(InkWorkspace, "AllowWindowsInkWorkspace", 1)],
            RemoveOps = [RegOp.DeleteValue(InkWorkspace, "AllowWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(InkWorkspace, "AllowWindowsInkWorkspace", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-workspace-button",
            Label = "Disable Pen and Ink Workspace Taskbar Button",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "taskbar", "pen button", "ui"],
            Description = "Hides the Pen Workspace button from the Windows taskbar notification area. " + "PenWorkspaceButtonDesiredVisibility=0.",
            ApplyOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-handwriting-panel",
            Label = "Disable Touch Keyboard / Handwriting Panel Auto-Show",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "handwriting", "touch keyboard", "panel"],
            Description =
                "Disables the touch keyboard and handwriting panel from automatically "
                + "appearing when a text field is tapped. Prevents TABLETPC features "
                + "from activating on non-tablet devices.",
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisableHandwritingPanel", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisableHandwritingPanel")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisableHandwritingPanel", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-ink-personalization",
            Label = "Disable Ink Personalization Data Collection",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "personalization", "privacy", "telemetry"],
            Description =
                "Prevents Windows from collecting handwriting and ink input samples "
                + "for improving the recognition engine. "
                + "RestrictImplicitInkCollection=1.",
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitInkCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-text-prediction",
            Label = "Disable Ink Text Prediction and Recommendations",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "text prediction", "recommendations", "privacy"],
            Description =
                "Disables text prediction and autocorrect suggestions in the Windows "
                + "handwriting and ink text input panel. Keeps ink recognition "
                + "as-is without real-time suggestions.",
            ApplyOps = [RegOp.SetDword(PenSettings, "TextInputLocked", 1)],
            RemoveOps = [RegOp.DeleteValue(PenSettings, "TextInputLocked")],
            DetectOps = [RegOp.CheckDword(PenSettings, "TextInputLocked", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-flicks",
            Label = "Disable Pen Flicks (Swipe Gestures)",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "flicks", "gestures", "pen", "swipe"],
            Description =
                "Disables pen flick gestures (quick swipe actions that trigger scroll, "
                + "delete, copy etc.). Prevents accidental flick detection when using "
                + "a stylus for precision drawing or writing.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Mouse", "FlicksDisabled", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-press-and-hold",
            Label = "Disable Pen Press-and-Hold for Right-Click",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["ink", "press and hold", "right-click", "stylus"],
            Description =
                "Disables the press-and-hold gesture that triggers a right-click when "
                + "using a stylus. Useful for artists and designers who hold the pen tip "
                + "on the canvas for extended periods without wanting a context menu.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\ImmersiveShell", "DisablePressAndHold", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-typing-data-collection",
            Label = "Disable Typing and Text Input Data Collection",
            Category = "Touch & Pen",
            NeedsAdmin = true,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "typing data", "privacy", "text input", "collection"],
            Description =
                "Prevents Windows from collecting typing and text input data to improve "
                + "personalised features. RestrictImplicitTextCollection=1 via policy.",
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitTextCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-learn-from-this-device",
            Label = "Disable 'Learn from This Device' for Input Personalization",
            Category = "Touch & Pen",
            NeedsAdmin = false,
            CorpSafe = false,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["ink", "personalization", "learn", "inking", "privacy"],
            Description =
                "Disables the 'Get to know me' input personalization feature that "
                + "builds a personal inking and typing model from your inputs. "
                + "Prevents local model accumulation and cloud sync.",
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\InputPersonalization\TrainedDataStore", "HarvestContacts", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-touch-visual-feedback",
            Label = "Disable Touch Visual Feedback",
            Category = "Touch & Pen",
            Description =
                "Removes the contact-point circle animations shown when touching the screen. "
                + "Reduces visual noise and slightly improves rendering performance on touch-enabled displays.",
            Tags = ["touch", "pen", "visual", "performance"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(CursorFeedback, "ContactVisualization", 0)],
            RemoveOps = [RegOp.SetDword(CursorFeedback, "ContactVisualization", 1)],
            DetectOps = [RegOp.CheckDword(CursorFeedback, "ContactVisualization", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-visual-feedback",
            Label = "Disable Pen Visual Feedback",
            Category = "Touch & Pen",
            Description =
                "Removes the cursor/halo effects shown when using a stylus. "
                + "Useful on high-DPI tablets where the glow overlay obscures fine linework.",
            Tags = ["pen", "stylus", "visual", "tablet"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(CursorFeedback, "PenVisualization", 0)],
            RemoveOps = [RegOp.SetDword(CursorFeedback, "PenVisualization", 1)],
            DetectOps = [RegOp.CheckDword(CursorFeedback, "PenVisualization", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-workspace-startup",
            Label = "Hide Pen Workspace Button on Startup",
            Category = "Touch & Pen",
            Description =
                "Prevents the Pen Workspace button from appearing in the taskbar notification area. "
                + "Equivalent to unchecking 'Show the pen workspace button' in Pen & Windows Ink settings.",
            Tags = ["pen", "workspace", "taskbar", "tablet"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
            RemoveOps = [RegOp.SetDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 1)],
            DetectOps = [RegOp.CheckDword(InkUser, "PenWorkspaceButtonDesiredVisibility", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-workspace-app-launch",
            Label = "Block App Launches from Pen Workspace",
            Category = "Touch & Pen",
            Description =
                "Disables the ability to launch apps from the Pen Workspace panel. "
                + "Reduces attack surface on shared devices where pen-launched apps should be restricted.",
            Tags = ["pen", "workspace", "launch", "security"],
            NeedsAdmin = false,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkUser, "PenWorkspaceAppLaunchAllowed", 0)],
            RemoveOps = [RegOp.SetDword(InkUser, "PenWorkspaceAppLaunchAllowed", 1)],
            DetectOps = [RegOp.CheckDword(InkUser, "PenWorkspaceAppLaunchAllowed", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-right-click-hold",
            Label = "Disable Pen Press-and-Hold for Right-Click",
            Category = "Touch & Pen",
            Description =
                "Disables the 'press and hold' pen gesture that simulates a right-click. "
                + "Prevents accidental context menus when resting the pen on the screen.",
            Tags = ["pen", "gesture", "right-click", "tablet"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Wisp\Touch", "TouchMode_hold", 0)],
        },
        new TweakDef
        {
            Id = "ink-disable-tablet-ink-policy",
            Label = "Disable Ink Programs via Group Policy",
            Category = "Touch & Pen",
            Description =
                "Applies the TabletPC policy to disable InkBall and other built-in ink-based games/apps. "
                + "Recommended for corporate tablets to prevent access to entertainment apps.",
            Tags = ["pen", "tablet", "policy", "games"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisableInkball", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisableInkball")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisableInkball", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-handwriting-error-reports",
            Label = "Disable Handwriting Error Reporting",
            Category = "Touch & Pen",
            Description =
                "Blocks implicit collection of ink samples (strokes) used to improve handwriting recognition. "
                + "Prevents ink input data from being sent to Microsoft for model training.",
            Tags = ["pen", "handwriting", "privacy", "telemetry"],
            NeedsAdmin = true,
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitInkCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitInkCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-disable-handwriting-text-collection",
            Label = "Disable Handwriting Text Data Collection",
            Category = "Touch & Pen",
            Description =
                "Blocks implicit collection of handwritten text samples used to improve recognition accuracy. "
                + "Complements 'ink-disable-handwriting-error-reports' for full input-data privacy.",
            Tags = ["pen", "handwriting", "privacy", "text"],
            NeedsAdmin = true,
            ApplyOps = [RegOp.SetDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(SuggestionPolicy, "RestrictImplicitTextCollection")],
            DetectOps = [RegOp.CheckDword(SuggestionPolicy, "RestrictImplicitTextCollection", 1)],
        },
        new TweakDef
        {
            Id = "ink-set-pen-double-tap-speed",
            Label = "Set Pen Double-Tap Speed to 400 ms",
            Category = "Touch & Pen",
            Description =
                "Increases the pen double-tap recognition window to 400 ms (default 200 ms). "
                + "Helps users with motor impairment or those using thick-nib styluses to register double-taps reliably.",
            Tags = ["pen", "accessibility", "double-tap", "input"],
            NeedsAdmin = false,
            ApplyOps = [RegOp.SetDword(PenSettings, "DoubleTapTime", 400)],
            RemoveOps = [RegOp.DeleteValue(PenSettings, "DoubleTapTime")],
            DetectOps = [RegOp.CheckDword(PenSettings, "DoubleTapTime", 400)],
        },
        new TweakDef
        {
            Id = "ink-disable-pen-customization-page",
            Label = "Disable Pen Customization Settings Page",
            Category = "Touch & Pen",
            Description =
                "Hides the Pen Customization settings page from the Settings app via Group Policy. "
                + "Prevents users from rebinding pen buttons on managed/shared devices.",
            Tags = ["pen", "policy", "settings", "managed"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(InkPolicy, "DisablePenCustomization", 1)],
            RemoveOps = [RegOp.DeleteValue(InkPolicy, "DisablePenCustomization")],
            DetectOps = [RegOp.CheckDword(InkPolicy, "DisablePenCustomization", 1)],
        },
    ];
}
