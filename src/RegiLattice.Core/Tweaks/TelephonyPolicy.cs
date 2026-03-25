// RegiLattice.Core — Tweaks/TelephonyPolicy.cs
// Sprint 295: Telephony Policy tweaks (10 tweaks)
// Category: "Telephony Policy" | Slug: telpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Telephony

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class TelephonyPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Telephony";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "telpol-disable-call-telemetry",
            Label = "Disable Phone Call Telemetry",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows phone call telemetry collects data about calling application usage, call durations, and telephony API invocations. This telemetry is transmitted to Microsoft for improving the Windows telephony stack and Teams integration. Disabling it prevents call metadata from being transmitted outside the enterprise network. Organizations with strict communication privacy requirements benefit from eliminating call-related telemetry streams. Telephony functionality including PSTN, VoIP, and integrated calling applications is unaffected by disabling telemetry. Enterprise communication analytics are better gathered through centralized UCaaS platform reporting rather than OS-level telemetry.",
            Tags = ["telephony", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhoneCallTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneCallTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhoneCallTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "telpol-block-phone-app",
            Label = "Block Phone App",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "The Windows Phone app integrates Android and iPhone devices with Windows for call mirroring and cross-device notifications. Blocking the Phone app prevents the Phone Link service from linking personal mobile devices to corporate workstations. Device linking can create data exfiltration channels by mirroring corporate notifications to personal smartphones. Enterprises managing data loss prevention need to prevent uncontrolled device pairing with corporate endpoints. The Phone app is primarily a consumer convenience feature with limited enterprise value and significant security implications. Blocking it enforces a corporate data boundary between managed endpoints and employee personal devices.",
            Tags = ["telephony", "phone-app", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockPhoneApp", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockPhoneApp")],
            DetectOps = [RegOp.CheckDword(Key, "BlockPhoneApp", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-voice-capture",
            Label = "Disable Voice Capture in Telephony",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Voice capture in the telephony framework allows applications to access microphone input through telephony API calls for recording or analysis. Disabling voice capture prevents telephony-framework applications from accessing recorded audio through the telephony stack. Corporate telephony calls can contain sensitive negotiations, strategic discussions, and confidential information. Preventing unauthorized voice capture reduces the risk of covert recording by applications with telephony access. Enterprise communication recording should be managed through compliant UCaaS platforms with proper consent mechanisms. Disabling this capability ensures only explicitly authorized recording tools can access microphone audio.",
            Tags = ["telephony", "microphone", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableVoiceCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceCapture")],
            DetectOps = [RegOp.CheckDword(Key, "DisableVoiceCapture", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-phone-integration",
            Label = "Disable Phone Integration",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Phone integration enables the Windows telephony subsystem to bridge desktop applications with mobile phone calling capabilities through the Phone Link service. Disabling phone integration removes the bridge between desktop workflows and mobile telephony. Enterprise environments with managed VoIP solutions covering desktop calling needs have no requirement for mobile device telephony integration. The integration creates implicit dependencies on personal mobile devices that are outside enterprise IT management scope. Disabling phone integration enforces the separation between personal and corporate communication channels. Standard VOIP and SIP-based enterprise telephony solutions remain fully functional when phone integration is disabled.",
            Tags = ["telephony", "integration", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhoneIntegration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneIntegration")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhoneIntegration", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-callerid-uploads",
            Label = "Disable Caller ID Upload",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Caller ID upload functionality transmits phone number identifiers from incoming calls to Microsoft or third-party caller identification services. This transmission allows the service to return caller name information but exposes phone numbers from corporate calls to external services. Disabling caller ID uploads prevents corporate contact phone numbers from being disclosed to external lookup services. Financial institutions, law firms, and healthcare organizations have strict obligations to prevent disclosure of client contact information. The caller identification feature provides convenience but creates compliance risks when corporate contacts' numbers are transmitted externally. Disabling uploads eliminates this data exfiltration risk while preserving all local telephony functionality.",
            Tags = ["telephony", "caller-id", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCallerIdUploads", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCallerIdUploads")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCallerIdUploads", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-phone-number",
            Label = "Disable Phone Number Access",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Windows phone number access allows applications to read the device's cellular or VoIP telephone numbers through the telephony API. Disabling phone number access prevents applications from reading the device's associated telephone numbers without explicit user consent. Phone numbers are unique identifiers that can be used for device fingerprinting and identity correlation across services. Enterprise devices with assigned phone numbers should not expose these identifiers to arbitrary applications through the telephony API. Only explicitly authorized telephony applications with business justification should access device phone number information. Disabling this access aligns with privacy-by-default principles for enterprise endpoint configuration.",
            Tags = ["telephony", "phone-number", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhoneNumber", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneNumber")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhoneNumber", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-dialer-app",
            Label = "Disable Dialer Application",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The Windows phone dialer application provides an interface for making calls through connected mobile devices or VoIP applications from the desktop. Disabling the dialer app removes this consumer calling experience from corporate workstations. Enterprise users making calls rely on unified communications platforms such as Microsoft Teams, Cisco Webex, or Zoom which provide superior enterprise calling features. The dialer app has limited utility in enterprise environments with managed UCaaS solutions and can create confusion about which calling application to use. Disabling it streamlines the calling experience by directing all calls through the approved enterprise communication platform. The dialer app removal does not affect any enterprise VoIP or PSTN gateway functionality.",
            Tags = ["telephony", "dialer", "applications", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDialerApp", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDialerApp")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDialerApp", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-phone-sync-history",
            Label = "Disable Phone Sync History",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Phone sync history stores records of calls, messages, and notifications synced between mobile devices and Windows through Phone Link. This history creates a persistent log of mobile communications on the corporate workstation that can be accessed by the operating system. Disabling phone sync history prevents call and message records from a personal mobile device from being stored on the corporate endpoint. Cross-device synchronization logs should not be created on corporate endpoints as they represent personal data outside enterprise management scope. HR and legal considerations around employee privacy require that personal communication history not be stored on corporate systems. Disabling this feature maintains a clear boundary between personal mobile data and corporate endpoint storage.",
            Tags = ["telephony", "sync", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhoneSyncHistory", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneSyncHistory")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhoneSyncHistory", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-phone-book-access",
            Label = "Disable Phone Book Access",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Phone book access through the telephony framework allows applications to read contact lists from paired mobile devices through the Phone Link connection. Disabling phone book access prevents applications from reading personal contact information stored on mobile devices through the desktop telephony bridge. Contact lists can contain sensitive personal and professional relationship information that should not be accessible through untrusted applications. Enterprise DLP policies should prevent personal device contact lists from being read by any application on the corporate endpoint. Personal contacts on mobile devices fall outside corporate data governance and must not be merged with or accessible from corporate systems. Disabling phone book access enforces the boundary between personal mobile data and corporate endpoint application access.",
            Tags = ["telephony", "contacts", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePhoneBookAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneBookAccess")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePhoneBookAccess", 1)],
        },
        new TweakDef
        {
            Id = "telpol-disable-incoming-call-notif",
            Label = "Disable Incoming Call Notification",
            Category = "Telephony Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Incoming call notifications from paired mobile devices display caller information on the corporate desktop through the Phone Link notification system. Disabling incoming call notifications prevents personal mobile phone calls from being displayed on the corporate workstation screen. Personal call notifications can reveal personal contact names from the employee's mobile device to colleagues who can see the screen. Suppressing phone call notifications from personal devices on corporate endpoints respects employee privacy around personal communications. Enterprise unified communications notifications for business calls should be managed exclusively through the corporate UCaaS platform. Disabling mobile-sourced call notifications does not affect any corporate telephony or UCaaS notification channels.",
            Tags = ["telephony", "notifications", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableIncomingCallNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableIncomingCallNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableIncomingCallNotification", 1)],
        },
    ];
}
