// App Privacy Policy (Advanced) — Sprint 150
// Slug "appprv2" — additional HKLM AppPrivacy LetApps* values not covered by
// AppPrivacyPolicy.cs (which covers Camera, Microphone, Notifications, AccountInfo,
// Background, SyncWithDevices, Phone, Tasks, Messaging, VideoLibrary).
//
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy
#nullable enable
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class AppPrivacyPolicyAdv
{
    private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appprv2-deny-call-history",
            Label = "App Privacy: Block all UWP apps from accessing call history",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessCallHistory=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "reading the device call history (incoming/outgoing calls log) at machine level.",
            Tags = ["privacy", "call-history", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessCallHistory", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessCallHistory")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessCallHistory", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-calendar",
            Label = "App Privacy: Block all UWP apps from accessing the calendar",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessCalendar=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "reading or writing calendar events at the machine policy level.",
            Tags = ["privacy", "calendar", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessCalendar", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessCalendar")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-contacts",
            Label = "App Privacy: Block all UWP apps from accessing contacts",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessContacts=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "reading or modifying the contacts/people store at the machine policy level.",
            Tags = ["privacy", "contacts", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessContacts", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessContacts")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-radios",
            Label = "App Privacy: Block all UWP apps from controlling radios (Wi-Fi/BT)",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessRadios=2 in AppPrivacy policy. Prevents all UWP apps from turning "
                + "Wi-Fi, Bluetooth, or other radio hardware on/off at the machine policy level.",
            Tags = ["privacy", "radios", "bluetooth", "wifi", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessRadios", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessRadios")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessRadios", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-trusted-devices",
            Label = "App Privacy: Block all UWP apps from accessing trusted devices",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessTrustedDevices=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "communicating with previously paired/trusted Bluetooth and USB devices.",
            Tags = ["privacy", "trusted-devices", "bluetooth", "usb", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessTrustedDevices", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessTrustedDevices")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessTrustedDevices", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-get-diagnostic-info",
            Label = "App Privacy: Block all UWP apps from reading diagnostic information",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsGetDiagnosticInfo=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "retrieving diagnostic/health data from other apps running on the device.",
            Tags = ["privacy", "diagnostic-info", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsGetDiagnosticInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsGetDiagnosticInfo")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsGetDiagnosticInfo", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-email",
            Label = "App Privacy: Block all UWP apps from accessing email",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessEmail=2 in AppPrivacy policy. Prevents all UWP apps from reading "
                + "or sending email through the Windows Mail provider at the machine policy level.",
            Tags = ["privacy", "email", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessEmail", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessEmail")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessEmail", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-gaze-input",
            Label = "App Privacy: Block all UWP apps from accessing gaze/eye-tracking input",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsAccessGazeInput=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "reading gaze or eye-tracking data from supported hardware at machine policy level.",
            Tags = ["privacy", "gaze", "eye-tracking", "input", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessGazeInput", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessGazeInput")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessGazeInput", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-activate-with-voice",
            Label = "App Privacy: Block all UWP apps from background voice activation",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsActivateWithVoice=2 in AppPrivacy policy. Prevents all UWP apps from "
                + "using wake-word / voice activation to start from a background or suspended state.",
            Tags = ["privacy", "voice", "activation", "background", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsActivateWithVoice", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsActivateWithVoice")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsActivateWithVoice", 2)],
        },
        new TweakDef
        {
            Id = "appprv2-deny-activate-with-voice-above-lock",
            Label = "App Privacy: Block voice activation above the lock screen",
            Category = "App Privacy Policy Advanced",
            Description =
                "Sets LetAppsActivateWithVoiceAboveLock=2 in AppPrivacy policy. Prevents all UWP apps "
                + "from responding to wake-word voice commands when the device is locked.",
            Tags = ["privacy", "voice", "activation", "lock-screen", "app-privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsActivateWithVoiceAboveLock", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsActivateWithVoiceAboveLock")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsActivateWithVoiceAboveLock", 2)],
        },
    ];
}
