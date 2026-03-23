// RegiLattice.Core — Tweaks/SensorPolicy.cs
// Sensor & capability access policy hardening (Sprint 129, T8.2).
// Slug "sensor" — LocationAndSensors policy key + CapabilityAccessManager ConsentStore
// entries not covered by Privacy.cs (which covers location/webcam/microphone/appDiagnostics).
// Adds: scripting-level location block, sensor kill switch, radios/activity/gazeInput/
// contacts/calendar/email/userDataTasks/bluetoothSync capability deny.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SensorPolicy
{
    private const string LocSensors = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LocationAndSensors";

    // CapabilityAccessManager ConsentStore — machine-wide capability deny
    private const string CamBase = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sensor-block-location-scripting",
            Label = "Block Script Access to Location Services",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "location", "scripting", "privacy", "policy"],
            Description =
                "Prevents scripts (Windows Script Host, PowerShell, browser scripts via MSHTML) from "
                + "accessing the Windows Location Platform API (DisableLocationScripting=1). Scripts "
                + "in browser controls and automation tools cannot query the device's geographic position. "
                + "Distinct from the full location disable in Privacy.cs.",
            ApplyOps = [RegOp.SetDword(LocSensors, "DisableLocationScripting", 1)],
            RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableLocationScripting")],
            DetectOps = [RegOp.CheckDword(LocSensors, "DisableLocationScripting", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-all-sensors",
            Label = "Disable All Sensor Devices via Policy",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "hardware", "disable", "privacy", "policy"],
            Description =
                "Disables all sensor hardware (ambient light sensors, accelerometers, compasses, "
                + "barometers, proximity sensors) via policy (DisableSensors=1). Applications cannot "
                + "query sensor data. Does not affect GPS/location which is controlled separately. "
                + "Useful for kiosk and high-security deployments.",
            ApplyOps = [RegOp.SetDword(LocSensors, "DisableSensors", 1)],
            RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableSensors")],
            DetectOps = [RegOp.CheckDword(LocSensors, "DisableSensors", 1)],
        },
        new TweakDef
        {
            Id = "sensor-disable-windows-location-provider",
            Label = "Disable Windows Location Platform Provider",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "location", "provider", "windows", "privacy"],
            Description =
                "Disables the Windows Location Platform, which aggregates GPS, Wi-Fi triangulation, "
                + "and IP-based positioning (DisableWindowsLocationProvider=1). Applications requesting "
                + "location data receive no position fix even when GPS hardware is present. "
                + "Works alongside Privacy.cs DisableLocation for defence-in-depth.",
            ApplyOps = [RegOp.SetDword(LocSensors, "DisableWindowsLocationProvider", 1)],
            RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableWindowsLocationProvider")],
            DetectOps = [RegOp.CheckDword(LocSensors, "DisableWindowsLocationProvider", 1)],
        },
        new TweakDef
        {
            Id = "sensor-block-location-user-override",
            Label = "Prevent Users Re-Enabling Location Services",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "location", "user-override", "policy", "lock"],
            Description =
                "Prevents users from enabling Location Services in the Settings app after an admin "
                + "policy has disabled them (DisableLocationSettingUserOverride=1). Locks the location "
                + "setting to the machine policy value and hides the toggle in Privacy & Security → "
                + "Location settings.",
            ApplyOps = [RegOp.SetDword(LocSensors, "DisableLocationSettingUserOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(LocSensors, "DisableLocationSettingUserOverride")],
            DetectOps = [RegOp.CheckDword(LocSensors, "DisableLocationSettingUserOverride", 1)],
        },
        new TweakDef
        {
            Id = "sensor-deny-radios-capability",
            Label = "Deny App Access to Radio (Bluetooth/Wi-Fi) Controls",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "radios", "bluetooth", "wifi", "privacy"],
            Description =
                "Revokes app access to radio-control capability (turn Bluetooth/Wi-Fi on or off) "
                + "for all applications via the CapabilityAccessManager ConsentStore. Apps cannot "
                + "programmatically toggle wireless radios. Prevents rogue apps from disabling Wi-Fi "
                + "to force expensive cellular data usage.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\radios", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\radios", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\radios", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "sensor-deny-activity-capability",
            Label = "Deny App Access to User Activity / Fitness Data",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "activity", "fitness", "privacy", "capability"],
            Description =
                "Denies all apps access to activity and fitness sensor data (step counts, movement "
                + "patterns) via ConsentStore/activity. Prevents UWP and packaged apps from reading "
                + "physical activity data without explicit user consent enforcement.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\activity", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\activity", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\activity", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "sensor-deny-gaze-input-capability",
            Label = "Deny App Access to Gaze / Eye-Tracking Input",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "gaze", "eye-tracking", "input", "privacy"],
            Description =
                "Denies all apps access to gaze input / eye-tracking hardware "
                + "(ConsentStore/gazeInput Value=Deny). Prevents applications from tracking where a "
                + "user is looking on screen — a high-value data source for profiling and surveillance.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\gazeInput", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\gazeInput", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\gazeInput", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "sensor-deny-contacts-capability",
            Label = "Deny App Access to Contacts",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "contacts", "privacy", "capability", "data"],
            Description =
                "Denies all apps machine-wide access to the user's Contacts/People data "
                + "(ConsentStore/contacts Value=Deny). Prevents packaged apps from reading the "
                + "address book. Protect personally identifiable information and corporate directory "
                + "data from apps that should not need contact access.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\contacts", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\contacts", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\contacts", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "sensor-deny-email-capability",
            Label = "Deny App Access to Email",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "email", "privacy", "capability", "data"],
            Description =
                "Denies all apps machine-wide access to the user's email data "
                + "(ConsentStore/email Value=Deny). Prevents packaged apps from reading mailbox content "
                + "or sending email on the user's behalf. Important in environments where only "
                + "approved email clients should have inbox access.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\email", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\email", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\email", "Value", "Deny")],
        },
        new TweakDef
        {
            Id = "sensor-deny-bluetooth-sync-capability",
            Label = "Deny App Access to Bluetooth Sync",
            Category = "Sensor Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["sensor", "bluetooth", "sync", "privacy", "capability"],
            Description =
                "Denies all apps access to the Bluetooth synchronisation capability "
                + "(ConsentStore/bluetoothSync Value=Deny). Prevents apps from syncing data to/from "
                + "paired Bluetooth devices without explicit authorisation. Reduces the Bluetooth "
                + "data-exfiltration attack surface on shared workstations.",
            ApplyOps = [RegOp.SetString($@"{CamBase}\bluetoothSync", "Value", "Deny")],
            RemoveOps = [RegOp.DeleteValue($@"{CamBase}\bluetoothSync", "Value")],
            DetectOps = [RegOp.CheckString($@"{CamBase}\bluetoothSync", "Value", "Deny")],
        },
    ];
}
