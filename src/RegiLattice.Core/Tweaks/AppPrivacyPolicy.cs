// RegiLattice.Core — Tweaks/AppPrivacyPolicy.cs
// Machine-level UWP app privacy policies via HKLM AppPrivacy LetApps* values (Sprint 136).
// Slug "appp" — HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy.
// These are machine-wide Group Policy force-deny controls. They differ from
// AppPermissions.cs which uses HKCU DeviceAccess GUIDs (per-user consent store).

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppPrivacyPolicy
{
    private const string Policy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appp-deny-camera",
            Label = "Policy: Force-Deny All UWP Apps Camera Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Machine-level group policy that force-denies all UWP apps access to the "
                + "device camera, overriding per-user consent. LetAppsAccessCamera=2. "
                + "Complements aperm-deny-camera-access (user-level GUID).",
            Tags = ["camera", "policy", "app privacy", "uwp", "privacy"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessCamera", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessCamera")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessCamera", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-microphone",
            Label = "Policy: Force-Deny All UWP Apps Microphone Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Machine-level policy force-denying all UWP apps access to the microphone. "
                + "LetAppsAccessMicrophone=2. Overrides user consent for all accounts.",
            Tags = ["microphone", "policy", "app privacy", "uwp", "privacy"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessMicrophone", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessMicrophone")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessMicrophone", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-notifications",
            Label = "Policy: Force-Deny All UWP Apps Notification Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Prevents all UWP apps from reading notification content across devices. "
                + "LetAppsAccessNotifications=2. Toast/lock-screen notifications still fire;  "
                + "cross-device notification mirroring is blocked.",
            Tags = ["notifications", "policy", "app privacy", "uwp", "privacy"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessNotifications", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessNotifications")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessNotifications", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-account-info",
            Label = "Policy: Force-Deny All UWP Apps Account Information Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Blocks all UWP apps from reading account information (name, picture, "
                + "username) at the machine policy level. LetAppsAccessAccountInfo=2.",
            Tags = ["account info", "policy", "app privacy", "uwp", "privacy"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessAccountInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessAccountInfo")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessAccountInfo", 2)],
        },
        new TweakDef
        {
            Id = "appp-block-background-apps",
            Label = "Policy: Block All UWP Apps from Running in Background",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Machine policy preventing UWP apps from running background tasks (live "
                + "tile updates, push notifications, location tracking). "
                + "LetAppsRunInBackground=2.",
            Tags = ["background", "policy", "app privacy", "uwp", "performance"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsRunInBackground", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-device-sync",
            Label = "Policy: Deny UWP Apps Near-Device Sync (Bluetooth/NFC)",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Prevents UWP apps from syncing with nearby devices via Bluetooth, "
                + "NFC, or other proximity technologies. LetAppsSyncWithDevices=2.",
            Tags = ["sync", "bluetooth", "nfc", "policy", "app privacy"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsSyncWithDevices", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsSyncWithDevices")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsSyncWithDevices", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-phone-calls",
            Label = "Policy: Force-Deny All UWP Apps Phone Call Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Blocks all UWP apps from accessing the phone/dialer at the machine level. "
                + "LetAppsAccessPhone=2.",
            Tags = ["phone", "calls", "policy", "app privacy", "uwp"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessPhone", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessPhone")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessPhone", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-tasks",
            Label = "Policy: Force-Deny All UWP Apps Task List Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Prevents all UWP apps from reading or writing to the system task list "
                + "(Cortana reminders, to-do lists). LetAppsAccessTasks=2.",
            Tags = ["tasks", "to-do", "policy", "app privacy", "uwp"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessTasks", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessTasks")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessTasks", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-messaging",
            Label = "Policy: Force-Deny All UWP Apps SMS / Messaging Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Blocks all UWP apps from sending or reading SMS and MMS messages at "
                + "the machine level. LetAppsAccessMessaging=2.",
            Tags = ["sms", "messaging", "policy", "app privacy", "uwp"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessMessaging", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessMessaging")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessMessaging", 2)],
        },
        new TweakDef
        {
            Id = "appp-deny-video-library",
            Label = "Policy: Force-Deny All UWP Apps Video Library Access",
            Category = "App Privacy Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Machine policy denying all UWP apps access to the user's Video library "
                + "folder. LetAppsAccessVideoLibrary=2.",
            Tags = ["video", "library", "files", "policy", "app privacy", "uwp"],
            RegistryKeys = [Policy],
            ApplyOps = [RegOp.SetDword(Policy, "LetAppsAccessVideoLibrary", 2)],
            RemoveOps = [RegOp.DeleteValue(Policy, "LetAppsAccessVideoLibrary")],
            DetectOps = [RegOp.CheckDword(Policy, "LetAppsAccessVideoLibrary", 2)],
        },
    ];
}
