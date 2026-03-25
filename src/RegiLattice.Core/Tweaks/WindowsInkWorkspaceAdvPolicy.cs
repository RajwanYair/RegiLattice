// RegiLattice.Core — Tweaks/WindowsInkWorkspaceAdvPolicy.cs
// Sprint 366: Windows Ink Workspace Advanced Policy tweaks (10 tweaks)
// Category: "Windows Ink Workspace Advanced Policy" | Slug: inkwsadv
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsInkWorkspaceAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "inkwsadv-restrict-ink-workspace-on-lockscreen",
            Label = "Restrict Windows Ink Workspace Access from the Lock Screen",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting Windows Ink Workspace access from the lock screen prevents users who are not authenticated to the device from launching ink workspace features that may expose note content or access note-taking functionality with user data. Lock screen accessible features bypass authentication requirements and any data that can be accessed from the lock screen represents a risk of unauthorized access to user information. The default Windows configuration allows Ink Workspace notes to be accessed from the lock screen through the taskbar which could expose previously created notes to physical access attackers. Restricting lock screen access to Ink Workspace is particularly important for shared devices and devices in physical locations accessible to people outside the organization. Users who frequently take notes using the stylus workflow should be informed that lock screen notes will not be accessible and alternative note-taking approaches should be available. Lock screen feature restrictions should be tested for consistency on all device types including Surface devices that have stylus-focused features.",
            Tags = ["ink-workspace", "lock-screen", "access-restriction", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceOnLockScreen")],
            DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
        },
        new TweakDef
        {
            Id = "inkwsadv-disable-suggested-ink-apps",
            Label = "Disable Suggested App Recommendations in Windows Ink Workspace",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Disabling suggested app recommendations in Windows Ink Workspace prevents Microsoft Store app recommendations from appearing in the Ink Workspace panel which may install unauthorized software or display advertising content within the workspace. Suggested apps in Ink Workspace can prompt users to install pen-input optimized applications from the Microsoft Store which may not have been vetted by the organization's software approval process. Enterprise devices should display only organization-approved applications and features rather than consumer-oriented app recommendations that are not relevant to business workflows. Disabling Ink Workspace suggestions also removes a potential data collection vector where suggestion telemetry tracks which ink-related applications users show interest in. Software deployment in enterprise environments should be managed through IT-controlled channels rather than direct user-initiated app store downloads regardless of whether they are promoted through Ink Workspace. Organizations that use Windows Ink Workspace features should configure it to display only relevant line-of-business applications rather than general consumer app suggestions.",
            Tags = ["ink-workspace", "app-suggestions", "store-apps", "enterprise-management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowSuggestedAppsInWindowsInkWorkspace")],
            DetectOps = [RegOp.CheckDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
        },
        new TweakDef
        {
            Id = "inkwsadv-restrict-ink-workspace-to-approved-users",
            Label = "Restrict Windows Ink Workspace Feature Access to Authorized User Groups",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Restricting Windows Ink Workspace to authorized user groups ensures that the feature is available only to employees who have a legitimate business need for pen input capabilities aligned with their role such as design healthcare or field operations functions. Unrestricted access to Ink Workspace on all endpoints makes the feature available to users who do not use pen input and creates an unnecessary attack surface for potential vulnerabilities in the ink processing stack. Role-based feature availability reduces the endpoint attack surface by disabling features that are not required for specific job functions while maintaining availability for users with legitimate use cases. Ink Workspace capability restrictions align with application allowlisting principles extending feature allowlisting beyond application execution to complex OS features. User group membership for ink workspace access authorization should be maintained as part of the identity management and access governance process. Devices that are not configured with digitizer hardware should have ink workspace disabled by default to avoid providing no-op features that consume resources.",
            Tags = ["ink-workspace", "user-restriction", "role-based-access", "attack-surface", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceToApprovedUsers")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-disable-ink-personalization-data-collection",
            Label = "Disable Ink and Typing Personalization Data Collection for Windows Ink",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Disabling ink and typing personalization data collection prevents Windows from sending handwriting samples stylus gesture patterns and ink input data to Microsoft services for handwriting recognition improvement and personalization. Handwriting recognition data transmitted to cloud services for personalization may include sensitive information written with a stylus such as signatures handwritten notes and sketches containing confidential content. Ink personalization data represents a category of behavioral biometric data that is sensitive from both privacy and security perspectives as handwriting patterns can uniquely identify individuals. Organizations with strict data minimization requirements should disable ink personalization collection to prevent unnecessary transmission of user behavioral data to cloud services. Users who rely on accurate handwriting recognition for productivity should be informed that disabling personalization may reduce recognition accuracy over time as the local model will not improve through cloud training. Disabling ink personalization collection is consistent with broader Windows telemetry and data collection minimization policies in enterprise environments.",
            Tags = ["ink-personalization", "data-collection", "privacy", "handwriting", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInkPersonalizationData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInkPersonalizationData")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInkPersonalizationData", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-enforce-ink-clipboard-restrictions",
            Label = "Enforce Clipboard Restrictions for Ink Content Sharing Between Applications",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Enforcing clipboard restrictions for ink content prevents handwritten ink from being copied and pasted between applications in ways that could bypass data loss prevention controls configured to monitor or restrict text data transfer. Ink content transferred through the clipboard may bypass DLP policies that inspect text-format clipboard data because ink is represented as a different data format that may not be inspected by the same DLP mechanisms. Organizations with strict DLP requirements should evaluate whether ink clipboard sharing creates a bypass channel for sensitive data transfer that circumvents text-based DLP controls. Clipboard restrictions for ink content should be evaluated in the context of the organization's broader clipboard management policy including cloud clipboard synchronization controls. Users who use ink input for legitimate workflow reasons including annotations and sketching should be made aware of any restrictions on ink clipboard sharing that affect their productivity. Ink data format controls should be reviewed periodically as Windows updates may introduce new ink data formats that affect the applicability of existing clipboard restriction policies.",
            Tags = ["ink-clipboard", "dlp", "data-transfer", "clipboard-restriction", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceInkClipboardRestrictions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceInkClipboardRestrictions")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceInkClipboardRestrictions", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-block-ink-workspace-third-party-apps",
            Label = "Block Third-Party Applications from Integrating with Windows Ink Workspace",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Blocking third-party application integration with Windows Ink Workspace prevents unapproved applications from registering themselves as ink workspace providers or intercepting ink input streams through the workspace integration API. Third-party ink workspace integrations that have not been vetted by organizational security review may collect ink input data including handwritten sensitive content through their integration with the ink pipeline. Malicious applications posing as legitimate ink workspace tools can use the integration API to capture all ink input data as a form of keylogging for stylus-entered text. Organizations should evaluate legitimate third-party ink application requirements through the normal software approval process rather than allowing unrestricted third-party ink workspace integration. Approved pen-input applications should be deployed through device management platforms with appropriate application version controls to ensure only vetted application versions can access ink workspace integration points. Security testing of the ink workspace third-party integration API should be included in application security assessments for applications that request ink workspace integration capabilities.",
            Tags = ["ink-workspace", "third-party-apps", "integration-security", "input-protection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyInkWorkspaceApps")],
            DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-disable-cloud-ink-sync",
            Label = "Disable Synchronization of Windows Ink Notes to Cloud Storage Services",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disabling cloud synchronization of Windows Ink notes prevents handwritten digital ink sketches and annotations from being automatically uploaded to cloud storage services where they are subject to third-party data handling and may be accessible from devices not managed by the organization. Cloud sync of ink notes can transmit sensitive handwritten content outside the organizational security boundary including annotated documents contract documents and handwritten notes taken during confidential meetings. Ink note synchronization disabled through policy ensures that notes remain on the device until the user explicitly chooses to share them through approved channels. Organizations that have approved OneDrive or other enterprise cloud storage for document synchronization should configure ink sync to use only approved enterprise storage rather than disabling all sync. Data sovereignty requirements may prohibit automatic cloud sync of data including handwritten notes to cloud regions or providers that do not meet organizational compliance requirements. Users should be informed about the cloud sync policy for ink notes and provided with guidance on how to share ink content through approved methods when collaboration requires information sharing.",
            Tags = ["ink-sync", "cloud-storage", "data-protection", "note-taking", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudInkSync", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudInkSync")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudInkSync", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-enforce-stylus-firmware-attestation",
            Label = "Enforce Firmware Attestation Checks for Connected Stylus and Pen Devices",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Stylus and pen device firmware attestation ensures that pen devices connected to enterprise systems have firmware that has been validated as authentic and unmodified preventing compromised pen firmware from injecting false input events or exploiting pen driver vulnerabilities. Pen and stylus devices connect through USB or Bluetooth interfaces and the driver surface that processes pen input represents an attack vector for specially crafted devices with malicious firmware. Supply chain attacks targeting peripheral firmware have demonstrated that hardware devices can be compromised to execute code in the context of the operating system's hardware input processing. Firmware attestation requirements for pen devices should be part of the organization's broader peripheral device security policy that includes hardware device approval and firmware management. Organizations should define a list of approved pen device models that have been security-evaluated and use device management controls to restrict use of pen devices to approved models. Security monitoring for pen device connection events should be implemented to detect use of unauthorized or unusual pen devices.",
            Tags = ["stylus-firmware", "attestation", "peripheral-security", "supply-chain", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceStylusFirmwareAttestation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceStylusFirmwareAttestation")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceStylusFirmwareAttestation", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-audit-ink-workspace-activity",
            Label = "Enable Audit Logging for Windows Ink Workspace Activation and Feature Use",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Enabling audit logging for Windows Ink Workspace activity records when the Ink Workspace is activated which features are used and what applications receive ink input providing a behavioral baseline for ink workspace usage patterns. Anomalous ink workspace activity including activation at unusual times screen-capturing ink operations or large volumes of ink note creation may indicate data harvesting or unauthorized use of pen input features. Audit logs for Ink Workspace should include user identity device identifier timestamp and feature used to support investigation of security incidents involving ink input workflows. Organizations can use ink workspace audit data to identify users who would benefit from additional training on ink security policies or who may be using ink features in ways that create security risks. Ink workspace activity monitoring should be proportionate to the sensitivity of data that users with ink access handle with more intensive monitoring for users with access to highly sensitive information. Audit events from ink workspace usage should be integrated with user and entity behavior analytics platforms to contextualize ink activity within the broader user behavioral profile.",
            Tags = ["ink-workspace", "audit-logging", "behavioral-analytics", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditInkWorkspaceActivity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditInkWorkspaceActivity")],
            DetectOps = [RegOp.CheckDword(Key, "AuditInkWorkspaceActivity", 1)],
        },
        new TweakDef
        {
            Id = "inkwsadv-restrict-ink-workspace-on-shared-devices",
            Label = "Apply Strict Ink Workspace Restrictions on Shared and Kiosk Devices",
            Category = "Windows Ink Workspace Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Applying strict Ink Workspace restrictions on shared devices and kiosk configurations prevents previous users' ink notes and sketches from being accessible to subsequent users of the same device minimizing data leakage between user sessions on shared infrastructure. Shared devices that do not clear Ink Workspace content between user sessions can expose handwritten notes taken by previous authenticated users to users who authenticate later. Kiosk configurations should have Ink Workspace disabled entirely unless pen input is an integral part of the kiosk application's intended function. Session isolation for ink workspace data on shared devices should ensure that ink content from one user session is not accessible in another user's session context. Organizations deploying hot-desking or shared workstation environments should configure ink workspace policies appropriate for the shared use context including automatic note clearing on session end. Ink workspace configuration for shared devices should be validated as part of the device provisioning process to ensure that shared use policies are applied correctly.",
            Tags = ["ink-workspace", "shared-devices", "kiosk", "session-isolation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceOnSharedDevices")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
        },
    ];
}
