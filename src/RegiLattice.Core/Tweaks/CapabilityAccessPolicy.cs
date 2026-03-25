// RegiLattice.Core — Tweaks/CapabilityAccessPolicy.cs
// Sprint 313: Capability Access Policy tweaks (10 tweaks)
// Category: "Capability Access Policy" | Slug: capacs
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CapabilityAccessPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "capacs-deny-microphone-access",
            Label = "Force Deny App Microphone Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Apps on Windows can request permission to access the system microphone for voice recording and audio input capabilities. Force denying microphone access prevents all apps from using the microphone regardless of individual app permission settings. Unauthorized microphone access by applications can result in recording of confidential business conversations in sensitive locations. Enterprise endpoints in secure facilities such as sensitive compartmented information facilities must prevent all application audio capture. Microphone policy denial does not affect legacy desktop applications running outside the UWP permission model. High-security environments should combine this policy with physical hardware controls for complete assurance.",
            Tags = ["privacy", "microphone", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessMicrophone", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessMicrophone")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessMicrophone", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-camera-access",
            Label = "Force Deny App Camera Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Camera access permissions control whether apps are authorized to capture images and video using the device camera hardware. Force denying camera access prevents applications from accessing camera hardware for image capture and video recording. Unauthorized camera use by applications can result in visual surveillance of personnel and facilities through infected or rogue applications. Enterprise endpoints in secure environments should restrict camera usage to approved applications with specific operational justification. This policy prevents unauthorized video recording even when users may have accepted prompts for camera permission in specific applications. Camera policy controls should be combined with physical camera covers for complete protection in high-security environments.",
            Tags = ["privacy", "camera", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCamera", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCamera")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCamera", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-location-access",
            Label = "Force Deny App Location Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Location services allow applications to determine the physical location of the device using GPS, Wi-Fi triangulation, and cell tower data. Force denying location access prevents applications from determining the physical location of enterprise endpoints. Location data reveals employee movement patterns, facility layouts, and physical security perimeter information. Enterprise employees in sensitive roles or handling classified information should not have location accessible to applications. Location telemetry from enterprise endpoints can build a detailed picture of organizational activities and physical security arrangements. Denying location access also reduces battery consumption on laptop and tablet devices by preventing continuous location polling.",
            Tags = ["privacy", "location", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessLocation", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessLocation")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessLocation", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-contacts-access",
            Label = "Force Deny App Contacts Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Contacts access permissions allow applications to read the user's contact list from the Windows People repository or connected account contact stores. Force denying contacts access prevents applications from reading business contacts, organizational directory information, and personal contact data. Contacts often contain email addresses, phone numbers, and organizational affiliation data representing sensitive corporate intelligence. Applications with contacts access can exfiltrate the entire corporate contact database to external endpoints. Contact data exfiltration enables targeted phishing campaigns using authentic-looking sender addresses from harvested contacts. Denying contacts access prevents application-level access regardless of user permissions granted to individual applications.",
            Tags = ["privacy", "contacts", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessContacts", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessContacts")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessContacts", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-calendar-access",
            Label = "Force Deny App Calendar Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Calendar access permissions allow applications to read, write, and monitor calendar events from connected calendar accounts. Force denying calendar access prevents applications from accessing meeting schedules, attendee information, and event descriptions. Calendar data provides detailed intelligence about organizational activities, business relationships, and sensitive scheduled events. Meeting titles and attendee lists can reveal confidential projects, customer relationships, and strategic plans. Calendar harvesting through malicious applications has been used in targeted corporate espionage operations. Denying calendar access removes a significant source of organizational intelligence accessible through the permission system.",
            Tags = ["privacy", "calendar", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessCalendar", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessCalendar")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessCalendar", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-messaging-access",
            Label = "Force Deny App Messaging Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Messaging access permissions allow applications to read and send SMS messages through mobile broadband device interfaces on Windows. Force denying messaging access prevents applications from accessing SMS messages or sending messages through local cellular radio hardware. SMS messages can contain two-factor authentication codes, sensitive communications, and password reset information. Applications with messaging access can harvest authentication codes in real time to defeat SMS-based multi-factor authentication. Access to messaging interfaces also enables premium SMS fraud through unauthorized message sending. Denying messaging access removes this attack vector on endpoints with cellular connectivity.",
            Tags = ["privacy", "messaging", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessMessaging", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessMessaging")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessMessaging", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-radios-access",
            Label = "Force Deny App Radio Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Radio access permissions allow applications to control wireless radio hardware including Bluetooth, cellular, and Wi-Fi interface management. Force denying radio access prevents applications from managing wireless radio hardware including enabling or disabling radios. Applications with radio control can enable disabled interfaces, create unauthorized ad-hoc connections, or manipulate radio hardware. Unauthorized radio management can bypass network access controls that depend on specific radio hardware states. Security controls that disable Bluetooth or other radios on corporate endpoints could be undermined by applications with radio access. Denying radio access ensures that wireless interface management remains under IT control through Group Policy and MDM rather than individual application permissions.",
            Tags = ["privacy", "radios", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessRadios", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessRadios")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessRadios", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-calls-access",
            Label = "Force Deny App Phone Call Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Phone call access permissions allow applications to access call history and initiate phone calls through mobile broadband interfaces on Windows. Force denying call access prevents applications from accessing call logs or making unauthorized phone calls through device cellular hardware. Call logs contain sensitive metadata including business contact numbers, call frequency, and call timing information. Applications with call access on devices with cellular capability can initiate unauthorized calls and access call history. Call metadata represents sensitive corporate intelligence that could reveal business relationships and communication patterns. Denying call access eliminates this data exposure risk on cellular-capable enterprise endpoints.",
            Tags = ["privacy", "calls", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessPhone", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessPhone")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessPhone", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-account-info-access",
            Label = "Force Deny App Account Information Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Account information access permissions allow applications to access Windows account information including user name, display picture, and account tier. Force denying account information access prevents applications from accessing user account data from the Windows account store. Account information combined with organizational affiliation enables targeted social engineering and identity theft attacks. Applications that collect account information can build profiles of enterprise users for malicious exploitation. User identity data harvested through account access permissions can be used for credential stuffing and account takeover attacks. Denying account information access protects user identity data from application-level harvesting through permission grants.",
            Tags = ["privacy", "account", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsAccessAccountInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsAccessAccountInfo")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsAccessAccountInfo", 2)],
        },
        new TweakDef
        {
            Id = "capacs-deny-diagnostics-access",
            Label = "Force Deny App Diagnostic Information Access",
            Category = "Capability Access Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "App diagnostic access permissions allow applications to access running process information, application data, and diagnostic information from other apps. Force denying diagnostic access prevents applications from reading information about other running applications and their data. Cross-application diagnostic access can be used to enumerate running processes, read application state, and identify sensitive applications. Security software and enterprise applications running on the endpoint could be identified and targeted by malicious apps with diagnostic access. Application inventory information gathered through diagnostic access can aid malware in selecting appropriate payloads and evasion techniques. Denying diagnostic access enforces application isolation and prevents cross-app intelligence gathering.",
            Tags = ["privacy", "diagnostics", "capability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LetAppsGetDiagnosticInfo", 2)],
            RemoveOps = [RegOp.DeleteValue(Key, "LetAppsGetDiagnosticInfo")],
            DetectOps = [RegOp.CheckDword(Key, "LetAppsGetDiagnosticInfo", 2)],
        },
    ];
}
