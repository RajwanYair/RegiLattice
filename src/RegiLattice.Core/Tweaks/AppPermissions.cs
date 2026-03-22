// RegiLattice.Core — Tweaks/AppPermissions.cs
// Machine-wide app permission policies: camera, mic, location, contacts, calendar, etc.
// Slug: "aperm" — HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy GPO keys.
// Each LetAppsAccess* value: 0=user-controlled, 1=force-allow, 2=force-deny.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppPermissions
{
    private const string AppPrivacy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "aperm-deny-camera-access",
            Label = "Block All Apps from Accessing Camera (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["camera", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the camera. "
                + "LetAppsAccessCamera=2. Overrides per-app user consent settings. "
                + "Useful on shared or high-security PCs where camera access should be blocked.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCamera", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCamera")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCamera", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-microphone-access",
            Label = "Block All Apps from Accessing Microphone (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["microphone", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the microphone. "
                + "LetAppsAccessMicrophone=2. Prevents unauthorized recording even if a "
                + "user accidentally grants app permission.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessMicrophone", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessMicrophone")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessMicrophone", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-location-access",
            Label = "Block All Apps from Accessing Location (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["location", "gps", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the location service. "
                + "LetAppsAccessLocation=2. Prevents location-based tracking by any app "
                + "regardless of per-app user consent.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessLocation", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessLocation")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessLocation", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-contacts-access",
            Label = "Block All Apps from Accessing Contacts (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["contacts", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the contacts/address "
                + "book. LetAppsAccessContacts=2. Prevents apps from harvesting contact data.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessContacts", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessContacts")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-calendar-access",
            Label = "Block All Apps from Accessing Calendar (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["calendar", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the calendar. "
                + "LetAppsAccessCalendar=2. Prevents scheduled-meeting and appointment "
                + "data from being read by third-party apps.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCalendar", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCalendar")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-call-history-access",
            Label = "Block All Apps from Accessing Call History (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["call history", "privacy", "app permissions", "phone", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to call history. "
                + "LetAppsAccessCallHistory=2. Prevents phone-linked apps from reading "
                + "incoming/outgoing call logs.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessCallHistory", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessCallHistory")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessCallHistory", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-email-access",
            Label = "Block All Apps from Accessing Email (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["email", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps access to the user's email. "
                + "LetAppsAccessEmail=2. Prevents third-party apps from reading email content "
                + "via the Windows email capability.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessEmail", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessEmail")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessEmail", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-documents-access",
            Label = "Block All Apps from Accessing Documents Library (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["documents", "privacy", "app permissions", "gpo", "files"],
            Description =
                "Enforces machine-wide policy denying all apps broad access to the Documents "
                + "library. LetAppsAccessDocumentsLibrary=2. Note: apps may still access files "
                + "via the file-picker dialog; this only blocks broad programmatic access.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessDocumentsLibrary", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessDocumentsLibrary")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessDocumentsLibrary", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-diagnostic-info",
            Label = "Block Apps from Accessing Diagnostic Information (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["diagnostics", "privacy", "app permissions", "gpo", "telemetry"],
            Description =
                "Enforces machine-wide policy denying all apps access to diagnostic information "
                + "(app list, battery status, usage data). LetAppsGetDiagnosticInfo=2. "
                + "Reduces the amount of device metadata apps can harvest for fingerprinting.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsGetDiagnosticInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsGetDiagnosticInfo")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsGetDiagnosticInfo", 2)],
        },
        new TweakDef
        {
            Id = "aperm-deny-radio-access",
            Label = "Block All Apps from Controlling Radios (GPO)",
            Category = "App Permissions",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Tags = ["radio", "bluetooth", "wifi", "privacy", "app permissions", "gpo"],
            Description =
                "Enforces machine-wide policy denying all apps the ability to turn on/off "
                + "radios (Wi-Fi, Bluetooth, NFC). LetAppsAccessRadios=2. Prevents rogue apps "
                + "from toggling wireless interfaces without user knowledge.",
            ApplyOps = [RegOp.SetDword(AppPrivacy, "LetAppsAccessRadios", 2)],
            RemoveOps = [RegOp.DeleteValue(AppPrivacy, "LetAppsAccessRadios")],
            DetectOps = [RegOp.CheckDword(AppPrivacy, "LetAppsAccessRadios", 2)],
        },
    ];
}
