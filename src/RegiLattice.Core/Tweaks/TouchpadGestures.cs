// RegiLattice.Core — Tweaks/TouchpadGestures.cs
// Windows Precision Touchpad gesture and sensitivity settings (Sprint 86).
// Slug "tpad" — HKCU\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad.
// Distinct from Touch.cs (touch screen) and Input.cs (mouse/keyboard basics).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TouchpadGestures
{
    private const string Ptp =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

    private const string PtpSettings =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Settings";

    private const string EaseTouchpad =
        @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "tpad-disable-three-finger-tap-cortana",
            Label = "Disable Three-Finger Tap (Cortana / Search)",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "gestures", "three-finger", "cortana"],
            Description =
                "Disables the three-finger tap gesture that opens Cortana or Search "
                + "on Precision Touchpad devices. Useful for users who frequently "
                + "trigger it accidentally.",
            ApplyOps = [RegOp.SetDword(Ptp, "ThreeFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "ThreeFingerTapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "ThreeFingerTapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-disable-four-finger-tap",
            Label = "Disable Four-Finger Tap (Action Center)",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "gestures", "four-finger", "action center"],
            Description =
                "Disables the four-finger tap gesture that opens the Action Center "
                + "on Precision Touchpad. Prevents accidental notification panel openings "
                + "during normal typing and navigation.",
            ApplyOps = [RegOp.SetDword(Ptp, "FourFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "FourFingerTapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "FourFingerTapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-disable-three-finger-slide-task-view",
            Label = "Disable Three-Finger Slide (Task View / Switch Apps)",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "gestures", "three-finger", "task view"],
            Description =
                "Disables the three-finger swipe gesture that triggers Task View on "
                + "upward swipe or switches apps on horizontal swipe. "
                + "Value 0 = disabled for three-finger swipe actions.",
            ApplyOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "ThreeFingerSlideEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-disable-four-finger-slide",
            Label = "Disable Four-Finger Slide (Virtual Desktop Navigation)",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "gestures", "four-finger", "virtual desktops"],
            Description =
                "Disables the four-finger horizontal swipe that switches between virtual "
                + "desktops. Useful on compact touchpads where four-finger gestures are "
                + "easily triggered accidentally.",
            ApplyOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "FourFingerSlideEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-reverse-scroll-direction",
            Label = "Enable Reverse (Natural) Scroll Direction",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Tags = ["touchpad", "scroll", "natural scroll", "direction"],
            Description =
                "Reverses the touchpad scroll direction to match natural/trackpad-style "
                + "scrolling (content follows finger direction, like macOS). "
                + "ReverseScrollingEnabled=1.",
            ApplyOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 1)],
            RemoveOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 0)],
            DetectOps = [RegOp.CheckDword(Ptp, "ReverseScrollingEnabled", 1)],
        },
        new TweakDef
        {
            Id = "tpad-disable-tap-to-click",
            Label = "Disable Tap to Click",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "tap", "click", "disable"],
            Description =
                "Disables tap-to-click on the touchpad, requiring a physical button press "
                + "for all clicks. Reduces accidental clicks while typing. "
                + "TapEnabled=0.",
            ApplyOps = [RegOp.SetDword(Ptp, "TapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "TapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "TapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-disable-two-finger-tap-right-click",
            Label = "Disable Two-Finger Tap Right-Click",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "two-finger", "right-click", "tap"],
            Description =
                "Disables two-finger tap as a right-click gesture. Useful for users who "
                + "prefer to right-click only via the physical right button. "
                + "TwoFingerTapEnabled=0.",
            ApplyOps = [RegOp.SetDword(Ptp, "TwoFingerTapEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "TwoFingerTapEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "TwoFingerTapEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-disable-pinch-to-zoom",
            Label = "Disable Pinch-to-Zoom Gesture",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "pinch", "zoom", "gesture"],
            Description =
                "Disables the pinch-to-zoom gesture on Precision Touchpad. Useful for "
                + "users who accidentally trigger zoom while scrolling or using two-finger "
                + "gestures. ZoomEnabled=0.",
            ApplyOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "ZoomEnabled", 0)],
        },
        new TweakDef
        {
            Id = "tpad-set-sensitivity-most-sensitive",
            Label = "Set Touchpad Sensitivity to Most Sensitive",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "sensitivity", "cursor", "speed"],
            Description =
                "Sets touchpad cursor speed/sensitivity to maximum (10) for fast, "
                + "responsive cursor movement across large displays. "
                + "CursorSpeed=10.",
            ApplyOps = [RegOp.SetDword(Ptp, "CursorSpeed", 10)],
            RemoveOps = [RegOp.SetDword(Ptp, "CursorSpeed", 5)],
            DetectOps = [RegOp.CheckDword(Ptp, "CursorSpeed", 10)],
        },
        new TweakDef
        {
            Id = "tpad-disable-edge-swipe",
            Label = "Disable Edge Swipe for Action Center / Widgets",
            Category = "Touchpad & Gestures",
            NeedsAdmin = false,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Tags = ["touchpad", "edge swipe", "action center", "widgets"],
            Description =
                "Disables the right-edge swipe gesture that opens the Action Center "
                + "and swipe from left that shows widgets. Prevents accidental panel "
                + "openings on touch-sensitive laptop bezels. "
                + "EdgeEnabled=0.",
            ApplyOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 0)],
            RemoveOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 1)],
            DetectOps = [RegOp.CheckDword(Ptp, "EdgeEnabled", 0)],
        },
    ];
}
