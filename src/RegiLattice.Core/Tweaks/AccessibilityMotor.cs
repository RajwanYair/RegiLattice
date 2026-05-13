// AccessibilityMotor.cs — motor accessibility tweaks for keyboard / mouse / input aids
// Category: Accessibility  |  IDs: accmotor-*  |  10 tweaks

#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

[TweakModule]
internal static class AccessibilityMotor
{
    private const string EaseKey = @"HKEY_CURRENT_USER\Control Panel\Accessibility";
    private const string MouseKey = @"HKEY_CURRENT_USER\Control Panel\Mouse";
    private const string KbdKey = @"HKEY_CURRENT_USER\Control Panel\Keyboard";
    private const string MouseKeysKey = @"HKEY_CURRENT_USER\Control Panel\Accessibility\MouseKeys";
    private const string ToggleKeysKey = @"HKEY_CURRENT_USER\Control Panel\Accessibility\ToggleKeys";
    private const string SerialKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Sermouse";
    private const string BrailleKey = @"HKEY_CURRENT_USER\Software\Microsoft\Windows NT\CurrentVersion\AccessibilityTemp";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "accmotor-toggle-keys-sound-off",
            Label = "Disable Toggle Keys Sound",
            Category = "Accessibility",
            Description = "Disables the audio beep that plays when Caps Lock, Num Lock, or Scroll Lock is pressed.",
            Tags = ["accessibility", "keyboard", "sound", "toggle-keys"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Silences the Toggle Keys beep without disabling the key functionality.",
            ApplyOps = [RegOp.SetString(ToggleKeysKey, "Flags", "58")],
            RemoveOps = [RegOp.SetString(ToggleKeysKey, "Flags", "62")],
            DetectOps = [RegOp.CheckString(ToggleKeysKey, "Flags", "58")],
        },
        new TweakDef
        {
            Id = "accmotor-increase-mouse-pointer-speed",
            Label = "Set Mouse Pointer Speed to 10 (Gaming)",
            Category = "Accessibility",
            Description =
                "Sets mouse pointer speed to 10 for users who need higher cursor speed without relying on EPP (enhanced pointer precision).",
            Tags = ["accessibility", "mouse", "pointer", "speed"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Increases cursor speed for users with motor limitations requiring fast pointer movement.",
            ApplyOps = [RegOp.SetString(MouseKey, "MouseSensitivity", "10")],
            RemoveOps = [RegOp.SetString(MouseKey, "MouseSensitivity", "6")],
            DetectOps = [RegOp.CheckString(MouseKey, "MouseSensitivity", "10")],
        },
        new TweakDef
        {
            Id = "accmotor-disable-pointer-precision",
            Label = "Disable Enhanced Pointer Precision",
            Category = "Accessibility",
            Description =
                "Disables Enhanced Pointer Precision (mouse acceleration), providing a linear 1:1 relationship between physical and on-screen mouse movement.",
            Tags = ["accessibility", "mouse", "pointer", "precision", "acceleration"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Removes variable acceleration; motor-impaired users get predictable pointer movement.",
            ApplyOps =
            [
                RegOp.SetString(MouseKey, "MouseSpeed", "0"),
                RegOp.SetString(MouseKey, "MouseThreshold1", "0"),
                RegOp.SetString(MouseKey, "MouseThreshold2", "0"),
            ],
            RemoveOps =
            [
                RegOp.SetString(MouseKey, "MouseSpeed", "1"),
                RegOp.SetString(MouseKey, "MouseThreshold1", "6"),
                RegOp.SetString(MouseKey, "MouseThreshold2", "10"),
            ],
            DetectOps = [RegOp.CheckString(MouseKey, "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "accmotor-slow-keys-delay",
            Label = "Set Keyboard Repeat Delay to Slowest",
            Category = "Accessibility",
            Description = "Sets keyboard auto-repeat delay to longest interval, helping users with motor tremors avoid unintended key repetitions.",
            Tags = ["accessibility", "keyboard", "repeat", "delay", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces accidental key repeats for users with limited fine motor control.",
            ApplyOps = [RegOp.SetString(KbdKey, "KeyboardDelay", "3")],
            RemoveOps = [RegOp.SetString(KbdKey, "KeyboardDelay", "1")],
            DetectOps = [RegOp.CheckString(KbdKey, "KeyboardDelay", "3")],
        },
        new TweakDef
        {
            Id = "accmotor-slow-keys-speed",
            Label = "Set Keyboard Repeat Speed to Slowest",
            Category = "Accessibility",
            Description = "Slows keyboard auto-repeat rate to the minimum, preventing rapid input repetition for users with limited motor dexterity.",
            Tags = ["accessibility", "keyboard", "repeat", "speed", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Slows key repeat so users with tremors have more time to release keys.",
            ApplyOps = [RegOp.SetString(KbdKey, "KeyboardSpeed", "0")],
            RemoveOps = [RegOp.SetString(KbdKey, "KeyboardSpeed", "31")],
            DetectOps = [RegOp.CheckString(KbdKey, "KeyboardSpeed", "0")],
        },
        new TweakDef
        {
            Id = "accmotor-mouse-keys-enable",
            Label = "Enable MouseKeys (Numpad Controls Pointer)",
            Category = "Accessibility",
            Description =
                "Enables MouseKeys so the numeric keypad can be used to move the mouse pointer, aiding users who cannot use a physical mouse.",
            Tags = ["accessibility", "mouse", "mouse-keys", "numpad", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables full pointer control from keyboard for users without physical mouse access.",
            ApplyOps = [RegOp.SetString(MouseKeysKey, "Flags", "63")],
            RemoveOps = [RegOp.SetString(MouseKeysKey, "Flags", "58")],
            DetectOps = [RegOp.CheckString(MouseKeysKey, "Flags", "63")],
        },
        new TweakDef
        {
            Id = "accmotor-mouse-keys-max-speed",
            Label = "Increase MouseKeys Maximum Pointer Speed",
            Category = "Accessibility",
            Description =
                "Raises the maximum pointer speed when using MouseKeys (numpad navigation), reducing the time needed to traverse large screen distances.",
            Tags = ["accessibility", "mouse", "mouse-keys", "speed", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Faster maximum MouseKeys speed reduces fatigue for users with motor limitations.",
            ApplyOps = [RegOp.SetString(MouseKeysKey, "MaximumSpeed", "80")],
            RemoveOps = [RegOp.SetString(MouseKeysKey, "MaximumSpeed", "40")],
            DetectOps = [RegOp.CheckString(MouseKeysKey, "MaximumSpeed", "80")],
        },
        new TweakDef
        {
            Id = "accmotor-sticky-keys-confirmation-off",
            Label = "Disable Sticky Keys Confirmation Dialog",
            Category = "Accessibility",
            Description =
                "Suppresses the confirmation dialog that appears when Sticky Keys is activated via 5× Shift presses, preventing accidental activation interruptions.",
            Tags = ["accessibility", "sticky-keys", "dialog", "keyboard"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Removes the disruptive popup when Sticky Keys shortcut is triggered.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "510")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Accessibility\StickyKeys", "Flags", "506")],
        },
        new TweakDef
        {
            Id = "accmotor-scroll-lines",
            Label = "Increase Mouse Wheel Scroll Lines to 5",
            Category = "Accessibility",
            Description =
                "Sets the mouse wheel scroll amount to 5 lines per notch, reducing the number of wheel rotations needed to scroll long documents.",
            Tags = ["accessibility", "mouse", "scroll", "wheel", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Fewer scroll wheel movements needed; reduces wrist strain for users with limited hand mobility.",
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "3")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "WheelScrollLines", "5")],
        },
        new TweakDef
        {
            Id = "accmotor-double-click-speed",
            Label = "Decrease Double-Click Speed for Motor Accessibility",
            Category = "Accessibility",
            Description =
                "Increases the double-click tolerance interval, giving users with slower motor response more time between two clicks to register as a double-click.",
            Tags = ["accessibility", "mouse", "double-click", "motor"],
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Extends double-click window so users with slow motor response can activate items reliably.",
            ApplyOps = [RegOp.SetString(MouseKey, "DoubleClickSpeed", "900")],
            RemoveOps = [RegOp.SetString(MouseKey, "DoubleClickSpeed", "500")],
            DetectOps = [RegOp.CheckString(MouseKey, "DoubleClickSpeed", "900")],
        },
    ];
}
