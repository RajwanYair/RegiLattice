// RegiLattice.Core — Tweaks/AppContainerSandboxPolicy.cs
// UWP AppContainer capability restrictions, package isolation, and sandbox hardening — Sprint 516.
// Category: "AppContainer Sandbox Policy" | Slug: appcon
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppContainerSandboxPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";
    private const string IsoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppIsolation";
    private const string AppKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Appx";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "appcon-deny-broadfileaccess",
            Label        = "Deny Broad File System Access Capability to UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks UWP apps from exercising the broadFileSystemAccess capability that allows reading files outside the app's sandbox, preventing apps from reading arbitrary user files even if they declare the capability in their manifest.",
            Tags         = ["appcontainer", "broad-file-access", "capability", "sandbox", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "UWP broadFileSystemAccess capability denied; Store apps cannot read files outside their package sandbox.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessFileSystem", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessFileSystem")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessFileSystem", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-deny-uwp-microphone",
            Label        = "Deny Microphone Access to All UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks all UWP applications from accessing the microphone device regardless of user-level privacy settings, ensuring that even if a user grants permission, the policy override prevents audio capture by Store apps.",
            Tags         = ["appcontainer", "microphone", "privacy", "capability", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Microphone access denied to all UWP apps via policy; user-level grants overridden. Voice apps will not work.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessMicrophone", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessMicrophone")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessMicrophone", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-deny-uwp-camera",
            Label        = "Deny Camera Access to All UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks all UWP applications from accessing the camera device via the AppContainer capability policy, preventing webcam capture by Store apps even if user-level permission has been granted.",
            Tags         = ["appcontainer", "camera", "webcam", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Camera access denied to all UWP apps via policy; webcam capture by Store apps blocked system-wide.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessCamera", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessCamera")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessCamera", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-deny-uwp-location",
            Label        = "Deny Location Access to All UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks UWP applications from accessing GPS and Windows Location Services, preventing Store apps from determining the user's physical location regardless of user-level location permission grants.",
            Tags         = ["appcontainer", "location", "gps", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Location access denied to all UWP apps via policy; GPS/location not available to Store apps.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessLocation", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessLocation")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessLocation", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-deny-uwp-contacts",
            Label        = "Deny Contacts Access to All UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks all UWP applications from accessing the Windows Contacts / People app contacts store, preventing Store apps from reading or exporting the user's contact list.",
            Tags         = ["appcontainer", "contacts", "people", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Contacts access denied to all UWP apps via policy; contact list not accessible to Store apps.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessContacts", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessContacts")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-deny-uwp-calendar",
            Label        = "Deny Calendar Access to All UWP Apps",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Blocks UWP applications from reading or modifying Windows Calendar data, preventing Store apps from accessing the user's appointment and scheduling information.",
            Tags         = ["appcontainer", "calendar", "privacy", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Calendar access denied to all UWP apps via policy; appointment data not accessible to Store apps.",
            ApplyOps     = [RegOp.SetDword(Key, "LetAppsAccessCalendar", 2)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LetAppsAccessCalendar")],
            DetectOps    = [RegOp.CheckDword(Key, "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id           = "appcon-enable-appcontainer-network-isolation",
            Label        = "Enable Network Isolation for AppContainer Processes",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Enforces strict network isolation for AppContainer processes, ensuring that UWP apps can only make network connections to endpoints declared in their manifest capabilities, blocking undeclared outbound connections.",
            Tags         = ["appcontainer", "network-isolation", "sandbox", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "AppContainer network isolation enforced; UWP apps can only connect to declared network endpoints.",
            ApplyOps     = [RegOp.SetDword(IsoKey, "EnforceNetworkIsolation", 1)],
            RemoveOps    = [RegOp.DeleteValue(IsoKey, "EnforceNetworkIsolation")],
            DetectOps    = [RegOp.CheckDword(IsoKey, "EnforceNetworkIsolation", 1)],
        },
        new TweakDef
        {
            Id           = "appcon-block-appcontainer-loopback",
            Label        = "Block AppContainer Loopback Exemption by Default",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Prevents UWP/AppContainer apps from being granted loopback network access exemptions that bypass AppContainer network isolation, ensuring all sandbox processes respect network isolation boundaries.",
            Tags         = ["appcontainer", "loopback", "network-isolation", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Loopback exemptions blocked for AppContainer; sandbox apps cannot bypass network isolation via localhost.",
            ApplyOps     = [RegOp.SetDword(IsoKey, "BlockLoopbackExemption", 1)],
            RemoveOps    = [RegOp.DeleteValue(IsoKey, "BlockLoopbackExemption")],
            DetectOps    = [RegOp.CheckDword(IsoKey, "BlockLoopbackExemption", 1)],
        },
        new TweakDef
        {
            Id           = "appcon-disable-appcontainer-telemetry",
            Label        = "Disable AppContainer and AppPrivacy Telemetry to Microsoft",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Prevents AppContainer isolation components and app privacy capability grant telemetry from being sent to Microsoft, protecting information about app capability usage patterns from cloud disclosure.",
            Tags         = ["appcontainer", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "AppContainer and app privacy telemetry to Microsoft disabled; capability grant stats not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(IsoKey, "DisableIsolationTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(IsoKey, "DisableIsolationTelemetry")],
            DetectOps    = [RegOp.CheckDword(IsoKey, "DisableIsolationTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "appcon-audit-capability-grants",
            Label        = "Audit AppContainer Capability Grant Events in Security Log",
            Category     = "AppContainer Sandbox Policy",
            Description  = "Enables Security event log entries when a UWP application is granted access to a sensitive capability (location, microphone, camera, contacts, calendar), providing an audit trail of capability access grants.",
            Tags         = ["appcontainer", "capability", "audit", "event-log", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "AppContainer capability grant events logged in Security log; sensitive access grants auditable for compliance.",
            ApplyOps     = [RegOp.SetDword(IsoKey, "AuditCapabilityGrantEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(IsoKey, "AuditCapabilityGrantEvents")],
            DetectOps    = [RegOp.CheckDword(IsoKey, "AuditCapabilityGrantEvents", 1)],
        },
    ];
}
