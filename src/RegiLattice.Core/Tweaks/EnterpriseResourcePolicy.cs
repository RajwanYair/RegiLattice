// RegiLattice.Core — Tweaks/EnterpriseResourcePolicy.cs
// Sprint 353: Enterprise Resource Policy tweaks (10 tweaks)
// Category: "Enterprise Resource Policy" | Slug: entres
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EnterpriseResourcePolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EnterpriseResourceManager";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "entres-enable-enterprise-resource-audit",
            Label = "Enable Audit Logging for Enterprise Resource Access Events",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enterprise resource access auditing creates detailed logs of access to managed enterprise resources enabling compliance reporting and security monitoring for sensitive business assets. Audit events for enterprise resources include successful and failed access attempts the identity of the requester the time of access and the resources accessed. Comprehensive resource access logging is a requirement for many regulatory frameworks including PCI-DSS HIPAA and SOX that mandate audit trails for access to regulated data and systems. Organizations should forward enterprise resource audit events to a central security information and event management system for correlation and long-term retention. Audit logs should be protected from modification and deletion and retained for a period consistent with regulatory requirements for the organization. Regular review of enterprise resource audit data helps identify access patterns that deviate from expected behavior.",
            Tags = ["enterprise-resources", "audit", "compliance", "access-control", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableResourceAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceAudit")],
            DetectOps = [RegOp.CheckDword(Key, "EnableResourceAudit", 1)],
        },
        new TweakDef
        {
            Id = "entres-enforce-resource-access-policies",
            Label = "Enforce Centralized Access Policies for Enterprise Resource Management",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Centralized access policy enforcement ensures that enterprise resource access decisions are made by the central policy engine rather than individual system ACLs which can become inconsistent over time. Centralized access policies allow organizations to define access rules based on user attributes resource sensitivity and context ensuring consistent enforcement across all relevant systems. Policy-based access control allows rapid updating of access rules during security incidents such as revoking access for compromised accounts across all resources simultaneously. Dynamic access control policies can incorporate contextual factors like device health network location and time of day into access decisions providing more nuanced and secure access control. Organizations should test centralized access policies in audit mode before enforcing them to identify access configurations that require adjustment. The policy engine should be highly available and integrated with directory services to ensure that access decisions can be made reliably.",
            Tags = ["enterprise-resources", "centralized-access", "dynamic-access-control", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceCentralizedAccessPolicies", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceCentralizedAccessPolicies")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceCentralizedAccessPolicies", 1)],
        },
        new TweakDef
        {
            Id = "entres-restrict-resource-sharing-to-domain",
            Label = "Restrict Enterprise Resource Sharing to Domain-Authenticated Sessions",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting enterprise resource sharing to domain-authenticated sessions prevents non-domain accounts from accessing enterprise resources shared through the enterprise resource manager. Non-domain accounts include local accounts workgroup accounts and external accounts that have not been validated through the organizational authentication infrastructure. Domain authentication requirement ensures that resource access is tied to organizational identity management which controls account lifecycle and credentials. Resources shared through enterprise resource manager can include network shares printers work resources and device access that should be limited to known and managed identities. The domain restriction applies the organizational security policy and access controls to resource sharing preventing circumvention through local account access. Organizations should audit resource sharing configurations to verify that domain authentication is required for all sensitive enterprise resources.",
            Tags = ["enterprise-resources", "domain-restriction", "authentication", "access-control", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSharingToDomain", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSharingToDomain")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSharingToDomain", 1)],
        },
        new TweakDef
        {
            Id = "entres-enable-data-classification-integration",
            Label = "Enable Data Classification Integration with Enterprise Resource Policy",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Data classification integration allows enterprise resource policy to make access decisions based on the sensitivity classification of the data resource ensuring that highly classified resources have appropriately strict access controls. Resource access policies that incorporate data classification labels can automatically apply stricter controls to sensitive or regulated data without requiring manual policy configuration for each resource. Classification-aware access control ensures that as data sensitivity increases the access controls around that data automatically tighten to apply appropriate protections. Integration with data loss prevention systems allows classification labels to also drive DLP policy enforcement decisions for enterprise resources. Organizations should establish a consistent data classification taxonomy that aligns with their regulatory obligations and risk management requirements. Classification labels should be applied consistently and the automated policy enforcement should be tested to verify that classification-based access decisions function as expected.",
            Tags = ["enterprise-resources", "data-classification", "access-control", "compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableDataClassificationIntegration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableDataClassificationIntegration")],
            DetectOps = [RegOp.CheckDword(Key, "EnableDataClassificationIntegration", 1)],
        },
        new TweakDef
        {
            Id = "entres-configure-resource-manager-logging",
            Label = "Configure Verbose Logging for Enterprise Resource Manager Operations",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Verbose logging for enterprise resource manager captures detailed operational events including policy evaluation access decisions and configuration changes providing comprehensive visibility into resource management operations. Detailed logs enable troubleshooting of access issues where users are denied access to resources they should have access to or granted access to resources they should not. Resource manager operation logs should include policy evaluation results that explain why access was granted or denied based on the attributes evaluated. Organizations should forward resource manager logs to centralized log management for correlation with other security and operational telemetry. Log retention for resource manager operations should be at least 90 days to support incident investigation and compliance auditing. Verbose logging has a minor performance impact on resource access operations and the verbosity level should be validated against performance requirements.",
            Tags = ["enterprise-resources", "logging", "operational-visibility", "troubleshooting", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableVerboseLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableVerboseLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableVerboseLogging", 1)],
        },
        new TweakDef
        {
            Id = "entres-enforce-resource-expiration-policy",
            Label = "Enforce Expiration Policy for Temporary Enterprise Resource Access Grants",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Resource access expiration policy ensures that temporary access grants automatically expire after their configured lifetime preventing indefinite accumulation of access permissions that are no longer required. Just-in-time access grants should expire automatically after the authorized time period without requiring manual intervention to remove the access. Organizations that grant temporary elevated access for specific tasks or time-limited projects benefit from automatic expiration to prevent those grants from remaining active after the justification expires. Expiration of temporary access is a key principle of zero standing privilege architectures where no accounts maintain persistent access to sensitive resources. The policy should configure appropriate default expiration periods for different types of resource access and should alert administrators when access is about to expire for review. Organizations should regularly audit active access grants to identify any that have excessive lifetimes or that should be expired.",
            Tags = ["enterprise-resources", "access-expiration", "just-in-time", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceAccessExpirationPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceAccessExpirationPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceAccessExpirationPolicy", 1)],
        },
        new TweakDef
        {
            Id = "entres-block-cross-tenant-resource-access",
            Label = "Block Enterprise Resource Access from External Tenant Identities",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Blocking cross-tenant resource access prevents external organizational identities from accessing enterprise resources without explicit authorization through configured cross-tenant access policies. External tenant identities from partner organizations guest accounts and contractor identities from different tenants should only access enterprise resources when explicitly granted through formal access review processes. Unrestricted cross-tenant access can allow resources to be reached by compromised accounts from partner organizations that have not implemented equivalent security controls. The policy enforces that any cross-tenant access must be explicitly configured rather than allowed by default based on federation trust relationships. Organizations that have legitimate cross-tenant collaboration requirements should configure specific cross-tenant access policies that grant the minimum required access to known and trusted partner identities. Regular review of cross-tenant access grants ensures that access is revoked when business relationships and collaboration requirements change.",
            Tags = ["enterprise-resources", "cross-tenant", "external-access", "identity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockCrossTenantResourceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossTenantResourceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "BlockCrossTenantResourceAccess", 1)],
        },
        new TweakDef
        {
            Id = "entres-enforce-resource-location-restriction",
            Label = "Restrict Enterprise Resource Access to Organizational Network Locations",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Network location restrictions for enterprise resource access limit access to authorized network locations such as corporate network subnets and VPN connections preventing access from arbitrary internet locations. Location-based access restrictions add a context-aware control layer that complements identity-based controls by requiring that access come from known trusted infrastructure. Compromised credentials used from external locations are blocked by network location restrictions providing detection and blocking of credential theft that is used away from corporate infrastructure. Organizations should configure location-based restrictions to include both physical office networks and VPN connections that remote workers and administrators use. The policy should be implemented alongside conditional access policies to provide a consistent location-aware access control framework. Regular review of allowed network locations ensures that the list remains current as network infrastructure changes.",
            Tags = ["enterprise-resources", "network-location", "conditional-access", "credential-theft", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceNetworkLocationRestriction", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceNetworkLocationRestriction")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceNetworkLocationRestriction", 1)],
        },
        new TweakDef
        {
            Id = "entres-enable-resource-health-monitoring",
            Label = "Enable Health Monitoring for Enterprise Resource Availability and Integrity",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enterprise resource health monitoring tracks the availability integrity and performance of managed resources providing early warning of resource degradation ahead of user-impacting failures. Resource health data helps distinguish between security incidents that cause resource degradation and operational causes allowing appropriate response procedures to be initiated quickly. Monitoring for unexpected resource configuration changes provides detection for insider threat and attacker activity that modifies resource settings to expand access or disrupt operations. Automated health checks should verify that resource access controls are intact that resources are accessible to authorized users and that resource configurations match their expected baselines. Health monitoring alerts should be integrated with incident management and change management processes to ensure timely and appropriate responses. Regular health monitoring reviews help identify systemic issues in resource management that require architectural changes.",
            Tags = ["enterprise-resources", "health-monitoring", "availability", "integrity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableResourceHealthMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableResourceHealthMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "EnableResourceHealthMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "entres-enforce-resource-naming-standards",
            Label = "Enforce Naming Standards for Enterprise Resource Registration and Discovery",
            Category = "Enterprise Resource Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Resource naming standards enforcement ensures that enterprise resources conform to organizational naming conventions that encode ownership classification and lifecycle information in the resource name. Consistent naming conventions enable automated policy application based on resource names including applying different security policies based on naming conventions that indicate the resource's environment or sensitivity. Naming standard enforcement prevents the creation of resources with ambiguous or misleading names that could cause misapplication of security policies. The naming convention should include indicators for environment production vs. test sensitivity classification owning team or department and creation date. Automated validation of resource names against the naming standard at registration time prevents non-compliant resources from being added to the enterprise resource inventory. Regular scanning of existing resources for naming standard compliance helps identify legacy resources that require remediation.",
            Tags = ["enterprise-resources", "naming-standards", "governance", "policy-automation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceResourceNamingStandards", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceResourceNamingStandards")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceResourceNamingStandards", 1)],
        },
    ];
}
