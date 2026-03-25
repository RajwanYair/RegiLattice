// RegiLattice.Core — Tweaks/AppGuardPolicy.cs
// Sprint 332: Application Guard Policy tweaks (10 tweaks)
// Category: "Application Guard Policy" | Slug: appgrd
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppGuardPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appgrd-enable-managed-mode",
            Label = "Enable Microsoft Defender Application Guard Managed Mode",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Microsoft Defender Application Guard (MDAG) creates a hardware-isolated container for browsing untrusted websites preventing host system compromise. Enabling MDAG managed mode activates the Hyper-V container isolation for Microsoft Edge and protects the host from browser-based attacks. Managed mode uses enterprise-defined site lists to determine which sites are considered trusted and which must be opened in the isolated container. MDAG provides defense-in-depth for users who browse unknown external sites as container compromise does not affect the host operating system. The isolated container is discarded after each MDAG session preventing persistent malware from surviving browser session boundaries. MDAG requires hardware virtualization support and appropriate hardware which should be verified before enabling this policy.",
            Tags = ["app-guard", "edge", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowAppHVSI_ProviderSet", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowAppHVSI_ProviderSet")],
            DetectOps = [RegOp.CheckDword(Key, "AllowAppHVSI_ProviderSet", 1)],
        },
        new TweakDef
        {
            Id = "appgrd-disable-clipboard-host-to-container",
            Label = "Disable Clipboard from Host to Application Guard Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Clipboard sharing between the host and the Application Guard container allows data to flow between the trusted and untrusted environments. Disabling clipboard from host to container prevents accidental or intentional data leakage from the trusted host environment into isolated browsing sessions. Clipboard data copied in the host may contain sensitive credentials, personally identifiable information, or confidential documents. Disabling host-to-container clipboard sharing reduces the risk of data being captured by malicious content in the isolated container. Container-to-host clipboard may be separately controlled allowing users to copy content from isolated sessions back to the host. Organizations must balance user productivity with security when configuring clipboard sharing in Application Guard.",
            Tags = ["app-guard", "clipboard", "data-leakage", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardSettings", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardSettings")],
            DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardSettings", 1)],
        },
        new TweakDef
        {
            Id = "appgrd-disable-clipboard-container-to-host",
            Label = "Disable Clipboard from Application Guard Container to Host",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Container-to-host clipboard sharing allows malicious content in the Application Guard container to inject data into the trusted host clipboard. Disabling container-to-host clipboard sharing prevents malicious code running in the isolated environment from passing data to the host system. Container-to-host clipboard attacks could inject malicious commands into the host clipboard that are executed when pasted into applications like PowerShell or terminals. Completely blocking container-to-host clipboard providing a complete bidirectional isolation between the browsing container and the host environment. Users who need to copy content from Application Guard sessions should use the approved content sharing mechanisms defined by IT policy. Setting clipboard to completely disabled for both directions provides maximum isolation at the cost of some user convenience.",
            Tags = ["app-guard", "clipboard", "container-escape", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppHVSIClipboardFileType", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIClipboardFileType")],
            DetectOps = [RegOp.CheckDword(Key, "AppHVSIClipboardFileType", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-disable-print-from-container",
            Label = "Disable Printing from Application Guard Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Printing from the Application Guard container sends document content from the isolated environment to physical printers potentially enabling data exfiltration. Disabling printing from Application Guard prevents untrusted web content from being printed to physical or virtual printers. Malicious websites in the container could initiate print jobs to capture screenshots or generate large volumes of print output. Disabling container printing ensures that print operations can only be initiated from trusted host applications preventing lateral data movement. Users who need to print from content viewed in Application Guard should be directed to copy content to the host environment through approved channels first. Print restrictions are particularly important in regulated environments where print activity must be controlled and audited.",
            Tags = ["app-guard", "printing", "data-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppHVSIPrintingSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIPrintingSettings")],
            DetectOps = [RegOp.CheckDword(Key, "AppHVSIPrintingSettings", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-disable-container-persistence",
            Label = "Disable Application Guard Container Data Persistence",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Application Guard container persistence allows browser data like cookies, history, and cached content to be saved between sessions providing malicious code with persistence mechanisms. Disabling container persistence ensures that each Application Guard session starts fresh with no data from previous browsing sessions. Persistent container data including tracking cookies, session tokens, and malicious extensions would survive container restarts defeating isolation guarantees. Fresh container sessions on each launch ensure that any malicious code that executed in a previous session cannot influence subsequent sessions. Disabling persistence increases the security isolation guarantee at the cost of user convenience for sites requiring repeated authentication. Session persistence should only be enabled when users have a documented business need and appropriate security compensating controls are in place.",
            Tags = ["app-guard", "persistence", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppHVSIContainerPersistence", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppHVSIContainerPersistence")],
            DetectOps = [RegOp.CheckDword(Key, "AppHVSIContainerPersistence", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-enable-audit-logging",
            Label = "Enable Application Guard Usage Audit Logging",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Application Guard audit logging records session starts, container operations, and security policy violations related to MDAG usage. Enabling MDAG audit logging provides visibility into Application Guard usage patterns and potential security events within containers. Audit logs capture information about blocked network requests, copy-paste attempts, and other security-relevant container events. MDAG audit data helps identify users who are routinely browsing high-risk sites that trigger container usage indicating potential behavioral risks. Audit logs should be forwarded to SIEM and correlated with endpoint security events to detect potential container escape attempts. Regular review of MDAG audit data helps tune the trusted site lists and identify when container protections are most heavily used.",
            Tags = ["app-guard", "audit", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditApplicationGuard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditApplicationGuard")],
            DetectOps = [RegOp.CheckDword(Key, "AuditApplicationGuard", 1)],
        },
        new TweakDef
        {
            Id = "appgrd-block-enterprise-content-in-container",
            Label = "Block Access to Enterprise Sites inside Application Guard",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Blocking access to enterprise sites from within the Application Guard container prevents malicious code in the container from accessing internal corporate resources. Enterprise site access from within the container allows drive-by download attacks to potentially pivot to internal corporate applications using the user's session context. Blocking enterprise domains from container access ensures that untrusted browsing sessions cannot reach internal applications even with the user's credentials. Container-to-intranet blocking prevents Business Email Compromise attacks where users are tricked into clicking links that would access internal systems from an untrusted context. Enterprise site blocking rules should align with the Windows Information Protection trusted site list for consistent data protection. Blocking enterprise sites from the container does not affect normal host Edge browsing to trusted corporate resources.",
            Tags = ["app-guard", "enterprise-sites", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNonEnterpriseContent", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNonEnterpriseContent")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNonEnterpriseContent", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-disable-camera-microphone-in-container",
            Label = "Disable Camera and Microphone in Application Guard Container",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Camera and microphone access from the Application Guard container allows websites in the isolated environment to capture audio and video from the device. Disabling camera and microphone access in the Application Guard container prevents malicious websites from conducting surveillance of users. Rogue websites in the container could use camera or microphone to capture sensitive conversations or visual information present near the device. The isolation benefit of Application Guard is partially undermined if container content can access real hardware sensors. Camera and microphone should only be enabled in containers for specific documented use cases where untrusted browsing with media access is required. Container media restrictions protect against social engineering attacks that prompt users to allow camera access to untrusted websites.",
            Tags = ["app-guard", "camera", "microphone", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AllowCameraMicrophoneRedirection", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowCameraMicrophoneRedirection")],
            DetectOps = [RegOp.CheckDword(Key, "AllowCameraMicrophoneRedirection", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-block-download-saving",
            Label = "Block Saving Downloaded Files from Application Guard",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Saving downloaded files from the Application Guard container to the host file system can introduce malware from untrusted sites to the trusted environment. Blocking downloads from Application Guard prevents malicious files downloaded in the isolated container from accessing the host file system. Drive-by download attacks in the container that deliver malware payloads are rendered ineffective when container downloads cannot reach the host. Files downloaded in the container remain contained within the isolated environment and are discarded when the container session ends. Users who need to save content from Application Guard browsing should be directed to approved transfer mechanisms like enterprise file sharing. Download blocking should be combined with container persistence disabled to ensure that malicious downloads cannot persist across container sessions.",
            Tags = ["app-guard", "downloads", "file-transfer", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "SaveFilesToHost", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "SaveFilesToHost")],
            DetectOps = [RegOp.CheckDword(Key, "SaveFilesToHost", 0)],
        },
        new TweakDef
        {
            Id = "appgrd-prevent-certificate-sharing",
            Label = "Prevent Certificate Sharing from Host to Application Guard",
            Category = "Application Guard Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Certificate sharing from the host to the Application Guard container allows the container to use enterprise certificates for TLS authentication. Preventing certificate sharing ensures that enterprise identity certificates are not available to content running in the isolated container. Malicious content in the container with access to enterprise certificates could authenticate to enterprise services using the user's corporate identity. Certificate forwarding to the container effectively bridges the trust boundary between the untrusted browsing environment and enterprise identity. Enterprise certificates should remain scoped to the trusted host environment where access controls and monitoring provide appropriate oversight. Organizations that need enterprise certificate access in Application Guard should evaluate whether the use case justifies the reduced isolation boundary.",
            Tags = ["app-guard", "certificates", "identity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AppHVSICertificateSharing", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "AppHVSICertificateSharing")],
            DetectOps = [RegOp.CheckDword(Key, "AppHVSICertificateSharing", 0)],
        },
    ];
}
