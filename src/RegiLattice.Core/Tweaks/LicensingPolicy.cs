// RegiLattice.Core — Tweaks/LicensingPolicy.cs
// Sprint 310: Licensing Policy tweaks (10 tweaks)
// Category: "Licensing Policy" | Slug: licpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LicensingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\SoftwareProtectionPlatform";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "licpol-disable-activation-status-report",
            Label = "Disable Windows Activation Status Reporting",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Windows activation status reporting sends information about the device's license activation state to Microsoft telemetry endpoints. Disabling activation status reporting prevents activation state information from being included in Windows telemetry. Activation status data is sensitive in enterprise environments as it reveals licensing arrangements and activation method details. Activation data collected through telemetry could be used to identify endpoints using volume license keys subject to audit. Enterprise licensing should be managed through KMS or Active Directory-Based Activation without telemetry reporting to external endpoints. Disabling this reporting does not affect the activation status or functionality of the Windows installation.",
            Tags = ["licensing", "activation", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoGenTicket", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoGenTicket")],
            DetectOps = [RegOp.CheckDword(Key, "NoGenTicket", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-kms-discovery",
            Label = "Disable KMS Server Auto-Discovery",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "KMS activation uses DNS SRV records to automatically discover the KMS server if a specific server is not configured on the endpoint. Disabling KMS auto-discovery prevents endpoints from finding and using arbitrary KMS servers advertised in DNS. Rogue KMS servers can be used to track endpoint activation attempts or serve as a reconnaissance mechanism in corporate environments. Enterprise endpoints should have a specific KMS server address configured rather than relying on dynamic DNS-based discovery. Explicit KMS server configuration provides IT with control over which server handles activation and enables monitoring of activation attempts. This setting is not applicable on endpoints using ADBA which does not require KMS discovery.",
            Tags = ["licensing", "kms", "activation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoKMSDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoKMSDiscovery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoKMSDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-activation-ui",
            Label = "Disable Activation UI Prompts",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 4,
            Description =
                "When Windows is not properly activated it displays persistent UI notifications including watermarks on the desktop and prompts in settings. Disabling activation UI suppresses these prompts on endpoints that may be in transit between purchasing and full activation. Enterprise environments using ADBA or KMS ensure activation occurs automatically when endpoints connect to the domain and should not require UI prompts. Activation UI prompts can distract users and cause support requests when endpoints are in temporarily non-activated states. Suppressing UI allows IT to manage activation in the background without end-user confusion or unnecessary support escalations. Windows functionality is not impaired during temporary non-activation states in enterprise volume licensing scenarios.",
            Tags = ["licensing", "activation", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoAcquireGT", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoAcquireGT")],
            DetectOps = [RegOp.CheckDword(Key, "NoAcquireGT", 1)],
        },
        new TweakDef
        {
            Id = "licpol-set-renewal-interval",
            Label = "Set License Renewal Check Interval",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Windows periodically contacts KMS servers and Microsoft online services to renew activation tokens and verify license validity. Configuring the renewal interval controls how frequently the endpoint contacts licensing servers for license verification. Default renewal intervals may cause excessive traffic to KMS servers or external Microsoft licensing endpoints. Enterprise environments with large numbers of endpoints benefit from staggered renewal intervals to reduce concentrated KMS load. Properly configured renewal intervals ensure that licensing remains current while avoiding unnecessary network traffic. License function is unaffected by extending the renewal interval as tokens remain valid for extended periods between renewals.",
            Tags = ["licensing", "renewal", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RenewalInterval", 10080)],
            RemoveOps = [RegOp.DeleteValue(Key, "RenewalInterval")],
            DetectOps = [RegOp.CheckDword(Key, "RenewalInterval", 10080)],
        },
        new TweakDef
        {
            Id = "licpol-disable-oem-activation",
            Label = "Disable OEM Activation Key Usage",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "OEM activation uses digital product keys embedded in UEFI firmware to automatically activate Windows without user intervention. Disabling OEM activation enforcement ensures that enterprise volume license keys are used instead of the OEM key present in device firmware. Enterprise management with KMS or ADBA requires that volume license editions and keys be used rather than consumer-oriented OEM activations. OEM keys are tied to specific hardware and do not transfer with re-imaging, causing activation issues in managed environments. Volume license activation provides centralized management and audit capabilities not available with OEM individual key activation. Disabling OEM activation directs the endpoint to use IT-managed volume licensing infrastructure.",
            Tags = ["licensing", "oem", "activation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOEMActivation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOEMActivation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOEMActivation", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-license-backout",
            Label = "Disable License Downgrade",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Windows licensing supports downgrade rights that allow newer edition license holders to install and use older Windows editions. Disabling license downgrade prevents activation of unauthorized earlier Windows editions on enterprise endpoints. Bringing older Windows versions into a managed environment through downgrade rights can introduce endpoints with missing security updates. Enterprise security baselines are written for specific Windows versions and may not address security considerations of older editions. Standardizing on a single Windows edition version simplifies patch management and compliance validation. Downgrade rights are a procurement mechanism and should be evaluated by IT before enabling in managed environments.",
            Tags = ["licensing", "downgrade", "management", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDowngrade", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDowngrade")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDowngrade", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-license-telemetry",
            Label = "Disable License Data Telemetry Uploads",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "The Software Protection Platform service collects and transmits data about installed software licenses and activation states to Microsoft. Disabling license telemetry prevents upload of software inventory and activation state data gathered by the licensing subsystem. Licensing telemetry data can include information about enterprise software portfolio which constitutes sensitive business intelligence. Transmission of software inventory to external parties without explicit consent may conflict with enterprise data governance requirements. Enterprise license management does not require external telemetry and should rely only on internal license management tools. Disabling licensing telemetry has no impact on local activation functionality or KMS-based volume activation.",
            Tags = ["licensing", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "NoDataCollection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "NoDataCollection")],
            DetectOps = [RegOp.CheckDword(Key, "NoDataCollection", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-license-store-access",
            Label = "Restrict License Store User Access",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The Windows license store contains product keys, activation tokens, and licensing metadata used by the Software Protection Platform. Restricting user access to the license store prevents non-administrative users from viewing or manipulating activation and key data. Product key exposure through readable license stores could allow key extraction for use on unauthorized systems. License store manipulation could interfere with activation integrity checks and allow tampered licenses to be installed. Enterprise product keys stored in the license store should be accessible only to privileged administrative processes. Restricting non-administrative access reduces the risk of license data being accessed or modified by malicious software running under user context.",
            Tags = ["licensing", "store", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLicenseStoreAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLicenseStoreAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLicenseStoreAccess", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-online-activation",
            Label = "Disable Online Product Activation",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Online activation connects to Microsoft licensing servers over the internet to validate and activate Windows product keys. Disabling online activation prevents endpoints from contacting Microsoft activation servers and requires offline or KMS-based activation. Enterprise environments with volume licensing should use KMS or ADBA activation methods that do not require internet connectivity. Online activation attempts from endpoints without internet connectivity create unnecessary timeout delays and error conditions. Network monitoring can reveal product key usage patterns when online activation attempts are captured in network logs. Disabling online activation centralizes key management through enterprise infrastructure and avoids direct Microsoft activation server contact from endpoints.",
            Tags = ["licensing", "online", "activation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableOnlineActivation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineActivation")],
            DetectOps = [RegOp.CheckDword(Key, "DisableOnlineActivation", 1)],
        },
        new TweakDef
        {
            Id = "licpol-disable-grace-period-notifications",
            Label = "Disable Activation Grace Period Notifications",
            Category = "Licensing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "During the activation grace period Windows displays notifications reminding users to activate before the grace period expires. Disabling grace period notifications suppresses user-facing activation prompts during managed activation processes. Enterprise endpoints in managed activation workflows should complete activation automatically without requiring user interaction. User activation prompts during device onboarding create unnecessary confusion and support calls when IT is handling activation centrally. Suppressing notifications allows the activation process to proceed transparently in the background without disrupting end users. Activation status and grace period data remains accessible to administrators who need to monitor activation compliance.",
            Tags = ["licensing", "notifications", "ui", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGracePeriodNotifications", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGracePeriodNotifications")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGracePeriodNotifications", 1)],
        },
    ];
}
