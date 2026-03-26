// RegiLattice.Core — Tweaks/AttentionSensingPolicy.cs
// Presence / Attention Sensing Group Policy controls — Sprint 370.
// Category: "Presence Sensing Policy" | Slug: attsens
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\AttentionSensing
//                 HKLM\SOFTWARE\Policies\Microsoft\Windows\PresenceSensing
// MinBuild: 22621 (Windows 11 22H2+)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AttentionSensingPolicy
{
    private const string AttKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AttentionSensing";
    private const string PresKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PresenceSensing";
    private const string LockKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PresenceSensing\Lock";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "attsens-disable-attention-sensing",
            Label = "Disable Attention Sensing (Gaze Detection)",
            Category = "Presence Sensing Policy",
            Description = "Disables the Windows Attention Sensing feature, which uses the device camera to detect whether the user is looking at the screen. When disabled, screen-dimming and adaptive brightness based on gaze are turned off.",
            Tags = ["attention-sensing", "presence", "camera", "privacy", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents the front camera from being used to monitor user gaze; improves privacy and reduces background camera processing.",
            RegistryKeys = [AttKey],
            ApplyOps  = [RegOp.SetDword(AttKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AttKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(AttKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "attsens-disable-presence-sensing",
            Label = "Disable Presence Sensing (Human Proximity Detection)",
            Category = "Presence Sensing Policy",
            Description = "Disables the Windows Presence Sensing feature, which uses proximity sensors and camera to detect whether a person is near the device. Prevents wake-on-approach and lock-on-leave behaviours.",
            Tags = ["presence-sensing", "proximity", "sensor", "privacy", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents the device from continuously monitoring for human proximity, reducing background sensor and camera activity.",
            RegistryKeys = [PresKey],
            ApplyOps  = [RegOp.SetDword(PresKey, "UserNotPresent", 1)],
            RemoveOps = [RegOp.DeleteValue(PresKey, "UserNotPresent")],
            DetectOps = [RegOp.CheckDword(PresKey, "UserNotPresent", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-wake-on-approach",
            Label = "Disable Wake-on-Approach (Screen Wakes When User Approaches)",
            Category = "Presence Sensing Policy",
            Description = "Disables Wake-on-Approach in Windows 11, which powers on the display when a presence-capable sensor detects a user walking near the device. Screen wake is controlled by normal power management instead.",
            Tags = ["presence-sensing", "wake-on-approach", "sleep", "power", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents unexpected screen activation in offices or public spaces when someone walks past the device.",
            RegistryKeys = [PresKey],
            ApplyOps  = [RegOp.SetDword(PresKey, "DisableWakeOnApproach", 1)],
            RemoveOps = [RegOp.DeleteValue(PresKey, "DisableWakeOnApproach")],
            DetectOps = [RegOp.CheckDword(PresKey, "DisableWakeOnApproach", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-lock-on-leave",
            Label = "Disable Lock-on-Leave (Device Locks When User Departs)",
            Category = "Presence Sensing Policy",
            Description = "Prevents Windows from automatically locking the screen based on presence-sensor detection of the user leaving the area. Screen lock reverts to standard timeout or manual lock controls.",
            Tags = ["presence-sensing", "lock-on-leave", "screen-lock", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Useful in environments where sensor false-positives cause disruptive mid-task lock events; standard timeout-based lock remains active.",
            RegistryKeys = [LockKey],
            ApplyOps  = [RegOp.SetDword(LockKey, "DisableLockOnLeave", 1)],
            RemoveOps = [RegOp.DeleteValue(LockKey, "DisableLockOnLeave")],
            DetectOps = [RegOp.CheckDword(LockKey, "DisableLockOnLeave", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-dim-on-look-away",
            Label = "Disable Screen Dim When User Looks Away",
            Category = "Presence Sensing Policy",
            Description = "Prevents Windows from dimming the screen when attention sensing detects the user is no longer looking at the display. Maintains consistent screen brightness independent of gaze direction.",
            Tags = ["attention-sensing", "screen-dim", "display", "brightness", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents distracting screen dimming in presentations, meetings, or side-glance scenarios.",
            RegistryKeys = [AttKey],
            ApplyOps  = [RegOp.SetDword(AttKey, "DimOnLookAway", 0)],
            RemoveOps = [RegOp.DeleteValue(AttKey, "DimOnLookAway")],
            DetectOps = [RegOp.CheckDword(AttKey, "DimOnLookAway", 0)],
        },
        new TweakDef
        {
            Id = "attsens-block-user-override",
            Label = "Prevent Users from Changing Presence Sensing Settings",
            Category = "Presence Sensing Policy",
            Description = "Locks presence-sensing and attention-sensing settings so users cannot enable or adjust them through Windows Settings, even on devices that have the required sensor hardware.",
            Tags = ["presence-sensing", "user-lock", "policy", "settings", "privacy"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Enforces a consistent presence-sensing posture across all managed devices, regardless of per-user preference.",
            RegistryKeys = [PresKey],
            ApplyOps  = [RegOp.SetDword(PresKey, "BlockUserOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(PresKey, "BlockUserOverride")],
            DetectOps = [RegOp.CheckDword(PresKey, "BlockUserOverride", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-adaptive-dimming",
            Label = "Disable Adaptive Dimming Based on Presence",
            Category = "Presence Sensing Policy",
            Description = "Disables adaptive display-dimming that uses presence sensor data to adjust screen brightness. Ensures display behaviour is governed by the configured power plan rather than sensor inference.",
            Tags = ["presence-sensing", "adaptive-dimming", "display", "power", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Provides consistent display behaviour in professional environments where sensor-based adaptive brightness is unpredictable.",
            RegistryKeys = [AttKey],
            ApplyOps  = [RegOp.SetDword(AttKey, "AdaptiveDimmingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AttKey, "AdaptiveDimmingEnabled")],
            DetectOps = [RegOp.CheckDword(AttKey, "AdaptiveDimmingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "attsens-require-sensor-consent",
            Label = "Require User Consent Before Enabling Presence Sensor",
            Category = "Presence Sensing Policy",
            Description = "Requires explicit user consent before Windows activates the presence sensor hardware for proximity and attention detection. Consent must be re-obtained if settings are reset.",
            Tags = ["presence-sensing", "consent", "privacy", "user-rights", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures users are aware that their movement and gaze are being monitored before the feature activates.",
            RegistryKeys = [PresKey],
            ApplyOps  = [RegOp.SetDword(PresKey, "RequireUserConsentForSensor", 1)],
            RemoveOps = [RegOp.DeleteValue(PresKey, "RequireUserConsentForSensor")],
            DetectOps = [RegOp.CheckDword(PresKey, "RequireUserConsentForSensor", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-presence-data-upload",
            Label = "Block Presence Sensing Data Telemetry Upload",
            Category = "Presence Sensing Policy",
            Description = "Prevents the Windows presence and attention sensing subsystem from sending usage telemetry, sensor performance data, and detection accuracy metrics to Microsoft.",
            Tags = ["presence-sensing", "telemetry", "data-upload", "privacy", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Stops proximity and attention sensing interaction data from being transmitted to Microsoft's cloud services.",
            RegistryKeys = [PresKey],
            ApplyOps  = [RegOp.SetDword(PresKey, "DisableSensingTelemetryUpload", 1)],
            RemoveOps = [RegOp.DeleteValue(PresKey, "DisableSensingTelemetryUpload")],
            DetectOps = [RegOp.CheckDword(PresKey, "DisableSensingTelemetryUpload", 1)],
        },
        new TweakDef
        {
            Id = "attsens-disable-camera-in-lock-screen",
            Label = "Disable Presence Detection on Lock Screen",
            Category = "Presence Sensing Policy",
            Description = "Prevents the Windows lock screen from activating the presence or attention sensor. The camera and proximity hardware remain inactive until the user manually inputs credentials to begin unlocking.",
            Tags = ["presence-sensing", "lock-screen", "camera", "security", "windows-11"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22621,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents the lock screen from briefly activating the camera to detect approaching users, removing an ambient-monitoring concern.",
            RegistryKeys = [LockKey],
            ApplyOps  = [RegOp.SetDword(LockKey, "DisablePresenceOnLockScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(LockKey, "DisablePresenceOnLockScreen")],
            DetectOps = [RegOp.CheckDword(LockKey, "DisablePresenceOnLockScreen", 1)],
        },
    ];
}
