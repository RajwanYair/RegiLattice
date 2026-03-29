// RegiLattice.Core — Tweaks/SharepointOnlinePolicy.cs
// SharePoint Online and Microsoft 365 Data Governance Policy — Sprint 589.
// Configures SharePoint Online access controls, sharing restrictions,
// external collaboration limits, and sensitivity label enforcement.
// Category: "SharePoint Online Policy" | Slug: spol
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Office\16.0\SharePoint
//           HKLM\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Privacy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SharepointOnlinePolicy
{
    private const string SharepointKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\SharePoint";

    private const string OfficePrivacyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Office\16.0\Common\Privacy";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "spol-disable-external-sharing",
                Label = "SharePoint Online: Prohibit External Sharing from SharePoint Sites",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets AllowExternalSharing=0 in the SharePoint policy key. Sets the client-side policy assertion that external sharing from SharePoint Online sites is not permitted. While the authoritative SharePoint sharing setting is managed in the SharePoint Admin Center, this registry policy works with Office client apps to enforce the restriction locally — Office add-ins and co-authoring flows check this policy to determine whether to offer 'share with external users' options. Combined with SharePoint Admin Center's external sharing settings, this provides defence-in-depth.",
                Tags = ["sharepoint", "external-sharing", "dlp", "data-exfiltration", "collaboration"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "External sharing prohibited in Office client SharePoint integration. Users cannot share items from Office apps to external email addresses via SharePoint sharing. External collaboration requires admin-authorised guest access configuration in the SharePoint Admin Center. Web-based sharing via SharePoint.com may still allow sharing depending on SharePoint Admin Center tenant-level settings.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "AllowExternalSharing", 0)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "AllowExternalSharing")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "AllowExternalSharing", 0)],
            },
            new TweakDef
            {
                Id = "spol-enable-sensitivity-label-enforcement",
                Label = "SharePoint Online: Enable Microsoft Information Protection Sensitivity Labels in Office",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets EnableMIPIntegration=1 in the SharePoint policy key. Enables the Microsoft Information Protection (MIP) AIP unified labelling integration in Office apps connecting to SharePoint Online. When enabled, Office apps (Word, Excel, PowerPoint, Outlook) display the sensitivity label bar and enforce label-based policies (encryption, access control, DRM) defined in the Microsoft Purview Compliance Center. Users are prompted to label documents before saving to SharePoint, and unlabelled uploads to labelled libraries are rejected.",
                Tags = ["sharepoint", "sensitivity-labels", "mip", "dlp", "information-protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "Sensitivity labelling integrated in Office apps. Users see the sensitivity label bar in Word, Excel, PowerPoint, and Outlook. Requires Microsoft Purview Information Protection (MIP) licensing (M365 E3/E5 or Azure Information Protection P1/P2). Labels configured in Purview Compliance Center are deployed to Office apps. Unlabelled existing documents are not automatically labelled — only new documents are prompted.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "EnableMIPIntegration", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableMIPIntegration")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "EnableMIPIntegration", 1)],
            },
            new TweakDef
            {
                Id = "spol-disable-co-authoring-with-external-users",
                Label = "SharePoint Online: Disable Real-Time Co-Authoring with External (Guest) Users",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets DisableExternalCoAuthoring=1 in the SharePoint policy key. Prevents Office real-time co-authoring sessions with external/guest users via SharePoint Online. Co-authoring with external users transmits document content character-by-character in real time — in strict DLP scenarios, even the act of collaborating with an external user on a sensitive document may constitute a data disclosure event. Disabling external co-authoring while retaining internal co-authoring preserves team collaboration while blocking external data flows.",
                Tags = ["sharepoint", "co-authoring", "external-guest", "dlp", "real-time"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "External guest co-authoring sessions blocked. Internal team co-authoring is unaffected. Guests in SharePoint sites can still view and download documents but cannot participate in real-time co-authoring sessions. Impact is primarily on M365 guest collaboration workflows.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableExternalCoAuthoring")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "DisableExternalCoAuthoring", 1)],
            },
            new TweakDef
            {
                Id = "spol-set-download-permissions-block-unmanaged",
                Label = "SharePoint Online: Block Downloads from SharePoint for Unmanaged Devices",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets BlockDownloadOnUnmanagedDevice=1 in the SharePoint policy key. Prevents file downloads from SharePoint Online to unmanaged (non-Azure-AD-joined) devices. This is the client-side policy flag — the enforcement is primarily in SharePoint Online Conditional Access policies configured for unmanaged devices. When this flag is set, Office apps enforce the restriction by checking the device management state before initiating downloads. Users on unmanaged devices can view documents in the browser (web-only mode) but cannot download files for local storage.",
                Tags = ["sharepoint", "unmanaged-device", "download-block", "byod", "conditional-access"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "SharePoint downloads blocked on unmanaged devices. Users on personal devices can only view content in the browser in read-only web view — they cannot download files, open in desktop Office apps, or print. Managed (Azure AD joined) devices are not affected.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "BlockDownloadOnUnmanagedDevice")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "BlockDownloadOnUnmanagedDevice", 1)],
            },
            new TweakDef
            {
                Id = "spol-disable-sharepoint-addins",
                Label = "SharePoint Online: Disable SharePoint Store Add-ins (Prevent Marketplace Apps)",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets DisableSharePointStoreAddins=1 in the SharePoint policy key. Prevents users from acquiring and installing SharePoint Add-ins from the SharePoint App Marketplace. Unvetted SharePoint add-ins can request high-privilege API permissions (full site read/write, full tenant admin on some legacy add-ins), access sensitive SharePoint data, and exfiltrate content to external services. IT should pre-approve and deploy authorised SharePoint add-ins via the corporate app catalogue rather than allowing open marketplace installs.",
                Tags = ["sharepoint", "add-ins", "app-marketplace", "shadow-it", "permissions"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "SharePoint marketplace add-in installs blocked. Users cannot install new add-ins from the SharePoint store. IT-approved add-ins deployed via the corporate App Catalogue are not affected. Existing installed marketplace add-ins may continue to function depending on SharePoint tenant configuration.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableSharePointStoreAddins")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "DisableSharePointStoreAddins", 1)],
            },
            new TweakDef
            {
                Id = "spol-enable-connected-experiences",
                Label = "SharePoint Online: Enable Required Connected Experiences (Disable Optional Diagnostic Data Opt-out)",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets DisconnectedState=0 in the Office Privacy policy key. Ensures that 'required connected experiences' in Office (spell check, grammar check, co-authoring, document recovery) remain enabled and cannot be disabled by users. The Office 'Disconnected Experiences' setting allows users to disable all cloud-connected features, which prevents essential collaboration features like SharePoint co-authoring, OneDrive sync status, and Exchange mail flow from functioning. In enterprise deployments, connected experiences should be enforced to ensure Office functionality meets business requirements.",
                Tags = ["office", "connected-experiences", "sharepoint", "coauthoring", "privacy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Office connected experiences enforced — users cannot disable cloud-connected Office features. Some optional connected experiences (LinkedIn integration, translation, proofing tool cloud lookups) may still be controllable via separate policies. Required connected experiences (co-authoring, OneDrive, spell check) remain active.",
                ApplyOps = [RegOp.SetDword(OfficePrivacyKey, "DisconnectedState", 0)],
                RemoveOps = [RegOp.DeleteValue(OfficePrivacyKey, "DisconnectedState")],
                DetectOps = [RegOp.CheckDword(OfficePrivacyKey, "DisconnectedState", 0)],
            },
            new TweakDef
            {
                Id = "spol-disable-optional-connected-experiences",
                Label = "SharePoint Online: Disable Optional Connected Experiences (Third-Party Add-ons in Office)",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets UserContentDisabled=1 in the Office Privacy policy key. Disables optional connected experiences in Office that access user content and connect to third-party services — for example, the Office Intelligent Services panel that submits document sections to third-party APIs for translation, AI writing assistance, or research suggestions. These optional experiences transmit document content to external (non-Microsoft) services, which may violate data residency requirements or expose confidential content. Disabling optional connected experiences reduces the Office external data transmission footprint.",
                Tags = ["office", "optional-experiences", "third-party", "privacy", "data-residency"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote =
                    "Optional connected experiences disabled. Third-party Office add-in services that submit document content to external APIs will not activate. Built-in Microsoft services (SharePoint, OneDrive, Exchange connected experiences, Microsoft Translator) are not classified as optional third-party experiences and remain functional.",
                ApplyOps = [RegOp.SetDword(OfficePrivacyKey, "UserContentDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(OfficePrivacyKey, "UserContentDisabled")],
                DetectOps = [RegOp.CheckDword(OfficePrivacyKey, "UserContentDisabled", 1)],
            },
            new TweakDef
            {
                Id = "spol-set-sync-client-tenant-restriction",
                Label = "SharePoint Online: Restrict OneDrive/SharePoint Sync to Authorised Tenant Only",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets AllowTenantList enforcement flag TenantRestrictionEnabled=1 in the SharePoint policy key. Enables the tenant restriction for OneDrive and SharePoint sync — the OneDrive client only allows sync connections to the authorised corporate tenant. Without this restriction, users can sign into any Microsoft 365 tenant from the OneDrive client (including a free personal tenant they created to receive data) and sync corporate SharePoint libraries to the non-corporate tenant. This is a data exfiltration vector for malicious insiders.",
                Tags = ["sharepoint", "tenant-restriction", "onedrive", "data-exfiltration", "insider"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote =
                    "OneDrive and SharePoint sync restricted to authorised tenant. Users cannot sync data to or from a non-corporate Microsoft 365 tenant. Requires the authorised tenant GUID to be configured in the policy (set via Group Policy ADMX template AllowTenantList setting). This registry flag enables the enforcement mechanism but requires the tenant GUID to be fully enforced.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "TenantRestrictionEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "TenantRestrictionEnabled")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "TenantRestrictionEnabled", 1)],
            },
            new TweakDef
            {
                Id = "spol-disable-sharepoint-meeting-recordings-personal",
                Label = "SharePoint Online: Disable Personal Meeting Recording Storage in OneDrive",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets DisableMeetingRecordingToPersonalOneDrive=1 in the SharePoint policy key. Prevents Teams meeting recordings from being saved to the organiser's personal OneDrive for Business. Instead, recordings are directed to the meeting's SharePoint channel (group OneDrive). Personal OneDrive storage for meeting recordings is uncontrolled from an IT governance perspective — recordings stored personally may be retained beyond the organisation's retention policy, shared with external recipients without oversight, or lost when an employee departures. Channel-based recording storage is covered by the SharePoint retention and eDiscovery policies.",
                Tags = ["sharepoint", "meeting-recordings", "teams", "onedrive", "retention"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote =
                    "Meeting recordings saved to SharePoint channel storage, not personal OneDrive. Teams recordings still available to meeting participants via the SharePoint channel. Compliance with meeting recording retention policies is simplified as recordings are under SharePoint retention policy scope.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "DisableMeetingRecordingToPersonalOneDrive", 1)],
            },
            new TweakDef
            {
                Id = "spol-enable-access-log-audit",
                Label = "SharePoint Online: Enable SharePoint Access and File Activity Audit Logging",
                Category = "SharePoint Online Policy",
                Description =
                    "Sets EnableAccessAudit=1 in the SharePoint policy key. Enables detailed file access and activity auditing in SharePoint Online — records who accessed, downloaded, modified, or shared each file, and from which device. SharePoint access audit logs are used for insider threat detection, eDiscovery, data breach investigation, and regulatory compliance (HIPAA, SOX, GDPR). Without audit logging, it is impossible to reconstruct who accessed sensitive files during a data breach investigation window.",
                Tags = ["sharepoint", "audit-log", "access-log", "insider-threat", "ediscovery"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote =
                    "SharePoint file access, modification, sharing, and download events logged. Full audit log available in Microsoft Purview Compliance Center and via Microsoft Graph API. High-volume event environments (large file libraries with frequent access) generate significant audit trail data. Audit log retention depends on Microsoft 365/Purview licensing.",
                ApplyOps = [RegOp.SetDword(SharepointKey, "EnableAccessAudit", 1)],
                RemoveOps = [RegOp.DeleteValue(SharepointKey, "EnableAccessAudit")],
                DetectOps = [RegOp.CheckDword(SharepointKey, "EnableAccessAudit", 1)],
            },
        ];
}
