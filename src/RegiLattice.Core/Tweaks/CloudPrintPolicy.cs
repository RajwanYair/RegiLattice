// RegiLattice.Core — Tweaks/CloudPrintPolicy.cs
// Sprint 360: Cloud Print Policy tweaks (10 tweaks)
// Category: "Cloud Print Policy" | Slug: cldprt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudPrint

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CloudPrintPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudPrint";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cldprt-disable-cloud-print-service",
            Label = "Disable Windows Cloud Print Discovery and Universal Print Services",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disabling Windows Cloud Print discovery prevents client computers from connecting to cloud-hosted print services that could transmit document content to external cloud infrastructure outside organizational control. Cloud print services route document data through external servers and the privacy and security controls of those cloud services may not meet organizational compliance requirements. Organizations that manage print infrastructure with on-premises print servers should disable cloud print discovery to ensure all printing flows through audited enterprise print infrastructure. Accidental use of cloud print services can result in sensitive documents being transmitted to and stored by external cloud providers without appropriate data handling controls. Disabling cloud print discovery does not prevent users from manually configuring printers but does remove automatic discovery of cloud print endpoints from the print experience. Organizations that have legitimate cloud print requirements through approved enterprise services like Universal Print should configure those services centrally rather than enabling broad cloud print discovery.",
            Tags = ["cloud-print", "print-services", "data-protection", "discovery", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCloudPrintService", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudPrintService")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCloudPrintService", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-restrict-cloud-printer-installation",
            Label = "Restrict User-Initiated Cloud Printer Installation Without Administrator Approval",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting user-initiated cloud printer installation prevents standard users from adding cloud print destinations that route documents through external services not approved or managed by IT operations. User-installed cloud printers can exfiltrate sensitive document content to personal or unauthorized business cloud print accounts outside organizational visibility. Print driver installation associated with cloud printers can introduce software components that have not been vetted by endpoint security teams. Organizations should centrally manage all printer deployments including cloud printers through Group Policy or device management platforms to ensure only approved print destinations are available. Restricting printer installation to administrators allows IT to control the complete list of print destinations available to users including auditing which cloud print services are in use. Users who have legitimate business requirements for cloud printing should submit requests through the IT service catalog for evaluation and approved deployment.",
            Tags = ["cloud-print", "printer-installation", "user-restriction", "data-handling", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCloudPrinterInstallation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCloudPrinterInstallation")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCloudPrinterInstallation", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-enforce-enterprise-cloud-print-only",
            Label = "Enforce Use of Enterprise-Only Cloud Print Services for All Organization Devices",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enforcing enterprise-only cloud print restricts cloud print operations to organizationally approved cloud print services preventing use of personal or unauthorized third-party cloud print providers. Many employees use consumer cloud print services for convenience when enterprise print alternatives are inconvenient but this creates uncontrolled data flows of potentially sensitive documents. Enterprise cloud print services like Microsoft Universal Print integrate with Azure Active Directory and provide administrative visibility into print jobs including audit logging relevant to compliance requirements. Organizations should deploy approved enterprise cloud print services that provide administrative oversight before enforcing the enterprise-only cloud print restriction. Transitioning from unrestricted cloud print to enterprise-only requires communication to users about what print services are approved and how to access them for remote and mobile printing scenarios. Audit logging of cloud print operations through enterprise print services provides visibility into printing volumes and patterns that may indicate data exfiltration attempts.",
            Tags = ["cloud-print", "enterprise-only", "approved-services", "universal-print", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceEnterpriseCloudPrintOnly")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceEnterpriseCloudPrintOnly", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-disable-mobile-print-discovery",
            Label = "Disable Automatic Mobile Print Service Discovery on Corporate Network",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Automatic mobile print discovery broadcasts the availability of print services to mobile devices on the corporate network creating potential data exfiltration pathways through mobile device printing that bypasses desktop endpoint security controls. Mobile devices connecting to corporate printers through automatic discovery may not have the same document handling policies applied that are enforced on managed desktop systems. Disabling automatic mobile print discovery reduces the attack surface from rogue mobile devices that join the corporate wireless network and attempt to access print infrastructure. Organizations that support mobile printing should implement this through Mobile Device Management policies that configure approved wireless print access rather than through open network service discovery. Print infrastructure security should include network access controls to restrict which devices can communicate with print servers. Corporate print servers should be segmented from general user VLAN segments to limit direct print protocol access to devices with legitimate printing needs.",
            Tags = ["mobile-print", "print-discovery", "network-security", "attack-surface", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMobilePrintDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMobilePrintDiscovery")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMobilePrintDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-audit-cloud-print-job-submissions",
            Label = "Enable Audit Logging for All Cloud Print Job Submissions and Printer Access",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Audit logging for cloud print job submissions records every print job sent to cloud print services including the user identity document metadata and destination printer providing an audit trail for potential data exfiltration investigations. Large document printing events or printing to unusual destinations outside normal work hours may indicate data exfiltration using print as a covert data transfer channel. Print audit logs should be integrated with SIEM alerting to detect anomalous print activity such as printing significantly more pages than baseline or printing sensitive documents to non-standard destinations. Cloud print audit logging from enterprise services provides more complete visibility than local print spooler logging because it captures the complete print workflow across cloud infrastructure. Organizations subject to data protection regulations should retain print audit logs for periods that satisfy retention requirements for regulatory investigations. User privacy considerations should be balanced with security monitoring needs when designing print audit programs to ensure appropriate oversight without unnecessary surveillance.",
            Tags = ["cloud-print", "print-audit", "monitoring", "data-exfiltration", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditCloudPrintJobSubmissions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditCloudPrintJobSubmissions")],
            DetectOps = [RegOp.CheckDword(Key, "AuditCloudPrintJobSubmissions", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-require-mfa-for-cloud-print-auth",
            Label = "Require Multi-Factor Authentication for Cloud Print Service Authentication",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Requiring multi-factor authentication for cloud print service operations ensures that cloud print access cannot be used to authenticate as a user using only stolen credentials which could expose sensitive documents in the print queue. Cloud print services that authenticate with Azure Active Directory credentials should inherit the conditional access policies that require MFA for cloud service authentication. Print jobs queued in cloud print infrastructure are protected by authentication requirements at the time of retrieval preventing unauthorized release of documents from cloud print queues. MFA for cloud print authentication prevents attackers who compromise user credentials from submitting or retrieving print jobs that might reveal organizational information. Organizations should configure conditional access policies that apply MFA requirements to cloud print services as part of the general cloud service MFA rollout. Service accounts used for print infrastructure management should use certificate-based authentication or managed identity approaches rather than password-based MFA.",
            Tags = ["cloud-print", "mfa", "authentication", "cloud-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireMFAForCloudPrintAuth", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireMFAForCloudPrintAuth")],
            DetectOps = [RegOp.CheckDword(Key, "RequireMFAForCloudPrintAuth", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-disable-print-to-pdf-cloud-storage",
            Label = "Disable Automatic Saving of Print to PDF Output to Cloud Storage Locations",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The Print to PDF feature when configured to automatically save output to cloud-connected storage locations including OneDrive can result in sensitive document content being transmitted to cloud storage without explicit user intent. Disabling automatic cloud save for Print to PDF output ensures that users must explicitly choose to save PDF output to cloud storage rather than having documents automatically uploaded. Documents printed to PDF during the course of normal workday operations may include sensitive contracts financial documents HR information or other content that should not be automatically uploaded to personal cloud storage. Organizations should configure Print to PDF default save locations to point to local or networked storage under organizational control. Users who have business requirements to share PDF output via cloud storage should explicitly save documents to approved business cloud storage rather than through automatic upload from the print subsystem. Document classification and DLP policies should be applied to all cloud storage upload paths to prevent accidental upload of sensitive content.",
            Tags = ["print-to-pdf", "cloud-storage", "data-protection", "automatic-upload", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrintToPdfCloudStorage", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrintToPdfCloudStorage")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrintToPdfCloudStorage", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-restrict-personal-cloud-print-accounts",
            Label = "Restrict Use of Personal Cloud Print Accounts on Domain-Joined Devices",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting personal cloud print account use on domain-joined devices prevents sensitive corporate documents from being transmitted to personal cloud print queues associated with non-corporate accounts that lack organizational data security controls. Employees using personal Google Cloud Print HP ePrint or other consumer cloud print services may not understand that their documents are being stored by the print service provider under consumer terms of service rather than enterprise security agreements. Personal cloud print accounts are not subject to corporate data retention deletion and security audit requirements creating compliance gaps for regulated organizations. Domain-joined devices should be configured to allow only enterprise cloud print accounts authenticated with organizational credentials. Unified endpoint management platforms can enforce cloud print account restrictions for both domain-joined and modern device-enrolled endpoints providing consistent control across device management approaches. User education about the importance of using only approved organizational print services for corporate documents should accompany technical controls.",
            Tags = ["personal-accounts", "cloud-print", "data-governance", "domain-joined", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictPersonalCloudPrintAccounts")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictPersonalCloudPrintAccounts", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-block-unencrypted-cloud-print-transmission",
            Label = "Block Unencrypted Data Transmission to Cloud Print Service Endpoints",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Blocking unencrypted cloud print transmission ensures that all document data sent to cloud print services uses TLS-encrypted connections preventing interception of document content during transmission to the cloud print infrastructure. Legacy print protocols and some consumer cloud print integrations may use unencrypted HTTP connections for print job submission that expose document content to network interception. Organizations should verify that approved cloud print services use TLS 1.2 or higher for all print data transmission and certificate validation is enforced to prevent man-in-the-middle attacks. Encrypted cloud print transmission protects document content from passive network monitoring by adversaries with access to enterprise or internet network segments. DLP monitoring at the network layer can inspect print traffic transmitted over unencrypted channels making encryption enforcement a complementary control to DLP. Cloud print service vendor security documentation should be reviewed to understand the encryption and data protection measures in place for print data at rest in cloud infrastructure.",
            Tags = ["cloud-print", "encryption", "tls", "data-in-transit", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockUnencryptedCloudPrintTransmission")],
            DetectOps = [RegOp.CheckDword(Key, "BlockUnencryptedCloudPrintTransmission", 1)],
        },
        new TweakDef
        {
            Id = "cldprt-enforce-print-data-retention-policy",
            Label = "Enforce Organizational Data Retention Policy for Cloud Print Job Metadata",
            Category = "Cloud Print Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Cloud print service metadata including print job history user identities document names timestamps and printer destinations is retained by cloud print providers for configuration, support and billing purposes which may conflict with organizational data retention and deletion policies. Enforcing an organizational print data retention policy ensures that print job metadata stored in cloud print infrastructure is deleted on a schedule consistent with organizational data governance requirements. Regulatory requirements in some jurisdictions limit retention of personal data associated with user activity including print job metadata which requires coordination with cloud print service providers regarding their data retention practices. Organizations should review the data processing and retention terms of cloud print service agreements as part of the vendor management and data privacy compliance process. Cloud print audit data should be distinguished from cloud print service operational metadata with audit data retained based on the organization's security audit requirements. Data subject access requests for personal data may need to include print metadata managed by cloud print services requiring notification of the cloud print service vendor as part of the request fulfillment process.",
            Tags = ["data-retention", "cloud-print", "compliance", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrintDataRetentionPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnforcePrintDataRetentionPolicy", 1)],
        },
    ];
}
