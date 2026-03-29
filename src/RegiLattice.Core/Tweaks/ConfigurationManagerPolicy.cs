// RegiLattice.Core — Tweaks/ConfigurationManagerPolicy.cs
// System Center Configuration Manager (SCCM / ConfigMgr) client agent Group Policy controls (Sprint 618).
// Category: "Configuration Manager Policy" | Slug: confmgr
// Key: HKLM\SOFTWARE\Policies\Microsoft\ConfigurationManager

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ConfigurationManagerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\ConfigurationManager";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "confmgr-require-code-signing-for-scripts",
            Label = "ConfigMgr: Require Script Code Signing for All Client-Side Script Execution",
            Category = "Configuration Manager Policy",
            Description = "Sets RequireScriptCodeSigning=1 in ConfigurationManager policy. Requires that any script (PowerShell, VBScript, JScript) deployed through the Configuration Manager client for task sequences or application deployment must be digitally signed by a certificate trusted by the client's root store before execution. " +
                "Configuration Manager script execution is a primary lateral movement vector in enterprise environments. A compromised management server or a rogue admin with deployment rights can push arbitrary scripts to all managed clients. Without code signing enforcement, any script pushed through ConfigMgr is executed verbatim. Requiring script code signing ensures only scripts signed by the enterprise PKI certificate authority are executed.",
            Tags = ["configmgr", "sccm", "scripts", "code-signing", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Unsigned ConfigMgr scripts blocked; all deployment scripts must be signed by enterprise PKI before execution.",
            ApplyOps = [RegOp.SetDword(Key, "RequireScriptCodeSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireScriptCodeSigning")],
            DetectOps = [RegOp.CheckDword(Key, "RequireScriptCodeSigning", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-enable-client-audit-logging",
            Label = "ConfigMgr: Enable Comprehensive Audit Logging for Client Agent Operations",
            Category = "Configuration Manager Policy",
            Description = "Sets EnableClientAuditLogging=1 in ConfigurationManager policy. Enables detailed audit logging in the Configuration Manager client agent, causing all deployment operations (software installs, uninstalls, state machine transitions, inventory collection, policy downloads) to be recorded in the Security event log in addition to the standard ccmsetup.log files. " +
                "The default ConfigMgr client logging writes verbose detail to log files under C:\\Windows\\CCM\\Logs\\ but does not generate Security event log entries auditable by a SIEM. With audit logging enabled, Security events are generated for every ConfigMgr operation, enabling correlation with Active Directory logon events, PowerShell execution events, and process creation events during incident investigations. This enables detection of ConfigMgr-based lateral movement.",
            Tags = ["configmgr", "sccm", "audit-log", "security", "siem"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "ConfigMgr operations generate Security event log entries; SIEM can correlate ConfigMgr deployments with suspicious activities.",
            ApplyOps = [RegOp.SetDword(Key, "EnableClientAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableClientAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableClientAuditLogging", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-require-ssl-for-management-point",
            Label = "ConfigMgr: Require HTTPS/PKI for All Client-to–Management Point Communication",
            Category = "Configuration Manager Policy",
            Description = "Sets RequireSSLForManagementPoint=1 in ConfigurationManager policy. Enforces that the ConfigMgr client uses HTTPS with PKI client certificates for all communication with the Management Point, Distribution Point, and other site roles, blocking fallback to HTTP. " +
                "Configuration Manager in HTTP mode transmits deployment data, credentials used for network access accounts, and package download URLs in plaintext. A network attacker on the same segment as a ConfigMgr client can intercept policy downloads and inject malicious package locations. Enforcing HTTPS-only communication requires PKI infrastructure but prevents man-in-the-middle interception of ConfigMgr policy and deployment content.",
            Tags = ["configmgr", "sccm", "https", "pki", "ssl", "management-point"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "ConfigMgr client requires HTTPS; HTTP communication with management point blocked. Requires PKI client certificates to be enrolled.",
            ApplyOps = [RegOp.SetDword(Key, "RequireSSLForManagementPoint", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireSSLForManagementPoint")],
            DetectOps = [RegOp.CheckDword(Key, "RequireSSLForManagementPoint", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-disable-software-center-user-portal",
            Label = "ConfigMgr: Disable Software Center User-Initiated Install Portal",
            Category = "Configuration Manager Policy",
            Description = "Sets DisableSoftwareCenterPortal=1 in ConfigurationManager policy. Disables the Software Center user-facing portal through which end users can browse 'Available' software and initiate their own optional application installs. Only 'Required' deployments that are pushed and mandatory remain active; the Software Center self-service catalog is removed from the user's Start menu. " +
                "The Software Center self-service portal is appropriate for general enterprise endpoints where end users should be able to install productivity tools. In high-security or locked-down environments (healthcare workstations, kiosk terminals, PCI-scope machines), allowing users to install any software from the catalog — even admin-approved software — introduces unnecessary attack surface expansion. Application installs should be exclusively IT-admin-driven deployments.",
            Tags = ["configmgr", "sccm", "software-center", "lockdown", "user-install"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Software Center self-service portal disabled; only mandatory/required ConfigMgr deployments are presented to users.",
            ApplyOps = [RegOp.SetDword(Key, "DisableSoftwareCenterPortal", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSoftwareCenterPortal")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSoftwareCenterPortal", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-disable-client-auto-upgrade",
            Label = "ConfigMgr: Disable Automatic ConfigMgr Client Agent Auto-Upgrade",
            Category = "Configuration Manager Policy",
            Description = "Sets DisableAutoUpgrade=1 in ConfigurationManager policy. Prevents the ConfigMgr client agent from automatically upgrading itself when the site server is running a newer version of the ConfigMgr client, requiring IT to explicitly push client upgrades through a managed deployment. " +
                "The ConfigMgr client auto-upgrade mechanism upgrades the client agent on all managed endpoints automatically when the Primary Site server is upgraded. While convenient, this means that upgrading the site server triggers an automatic, uncontrolled rollout to thousands of endpoints simultaneously, with no staging, no pilot group, and no rollback capability. A buggy client version pushed by auto-upgrade to all endpoints can simultaneously disrupt the management channel for the entire estate.",
            Tags = ["configmgr", "sccm", "client-upgrade", "rollout", "change-control"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "ConfigMgr client auto-upgrade disabled; client upgrades require explicit IT-managed deployment packages.",
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoUpgrade", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoUpgrade")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoUpgrade", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-require-admin-for-user-policy-execution",
            Label = "ConfigMgr: Require Administrative Approval Before User-Targeted Policy Execution",
            Category = "Configuration Manager Policy",
            Description = "Sets RequireAdminApprovalForUserPolicy=1 in ConfigurationManager policy. Requires that user-targeted configuration baseline deployments (policies applied to users, not computers) receive explicit IT admin approval in the ConfigMgr console before the client agent executes them on the endpoint. " +
                "In some ConfigMgr configurations, user-targeted configuration baselines can be deployed to security groups by less-privileged admins (Help Desk, Application Deployment staff) without requiring full ConfigMgr infrastructure admin privileges. If those baselines include scripts or registry modifications, a Help Desk operator with deployment rights could push policy changes to all users in their management scope. Requiring admin approval creates a second-factor approval gate for user-targeted policy execution.",
            Tags = ["configmgr", "sccm", "user-policy", "admin-approval", "separation-of-duties"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "User-targeted ConfigMgr configuration baselines require admin approval before execution; prevents unauthorised user-policy deployment.",
            ApplyOps = [RegOp.SetDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminApprovalForUserPolicy")],
            DetectOps = [RegOp.CheckDword(Key, "RequireAdminApprovalForUserPolicy", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-cap-content-cache-size-5gb",
            Label = "ConfigMgr: Cap Client Content Cache Size at 5 GB",
            Category = "Configuration Manager Policy",
            Description = "Sets MaxContentCacheSizeGB=5 in ConfigurationManager policy. Limits the ConfigMgr client content cache (the local disk cache where the client pre-downloads content from Distribution Points before installation) to a maximum of 5 GB, preventing the cache from consuming disk space beyond this limit. " +
                "By default, the ConfigMgr client content cache can grow to 10% of total disk size. On large-disk endpoints (1 TB drives), this allows a 100 GB cache. In environments with thin-provisioned storage (VDI, laptop SSDs) or low-disk-space scenarios, an unbounded cache can fill available disk space, causing operating system failures or application performance issues. A 5 GB cap is sufficient for most enterprise software deployments while protecting disk space.",
            Tags = ["configmgr", "sccm", "cache", "disk-space", "storage"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "ConfigMgr client content cache capped at 5 GB; disk consumption controlled for thin-provisioned storage environments.",
            ApplyOps = [RegOp.SetDword(Key, "MaxContentCacheSizeGB", 5)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxContentCacheSizeGB")],
            DetectOps = [RegOp.CheckDword(Key, "MaxContentCacheSizeGB", 5)],
        },
        new TweakDef
        {
            Id = "confmgr-disable-client-notification-feature",
            Label = "ConfigMgr: Disable ConfigMgr Client Notification Channel",
            Category = "Configuration Manager Policy",
            Description = "Sets DisableClientNotification=1 in ConfigurationManager policy. Disables the ConfigMgr client notification channel — a push mechanism that allows the site server to send fast-path notifications to clients to immediately trigger a policy evaluation or initiate re-inventory without waiting for the standard polling interval. " +
                "The client notification channel uses a persistent TCP connection from the ConfigMgr client to the Management Point. While this enables near-real-time policy deployment, it also means a compromised Management Point has an active connection to every managed client and can trigger immediate policy execution on all clients simultaneously. In environments where the threat model includes Management Point compromise, disabling the notification channel forces deployments to use the standard polling schedule which is easier to audit and rate-limit.",
            Tags = ["configmgr", "sccm", "client-notification", "tcp", "management-point"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "ConfigMgr push notifications disabled; policy deployment uses scheduled polling intervals instead of near-real-time push.",
            ApplyOps = [RegOp.SetDword(Key, "DisableClientNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClientNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClientNotification", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-enable-tamper-protection",
            Label = "ConfigMgr: Enable Tamper Protection for ConfigMgr Client Agent",
            Category = "Configuration Manager Policy",
            Description = "Sets EnableClientTamperProtection=1 in ConfigurationManager policy. Enables the ConfigMgr client tamper protection mechanism, which prevents standard users and non-admin processes from stopping or disabling the CCMExec service, deleting the CCM client registry keys, or uninstalling the ConfigMgr client agent. " +
                "Attackers that gain code execution on an endpoint as a standard user or as a low-privilege process will attempt to disable security tools and management agents before proceeding with lateral movement or data exfiltration. The ConfigMgr client agent is a high-value target for disablement because it delivers security baselines, patches, and malware detection policies. Tamper protection prevents the CCMExec service from being stopped by non-admin processes.",
            Tags = ["configmgr", "sccm", "tamper-protection", "service-protection", "ccmexec"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "ConfigMgr client tamper protection active; CCMExec service cannot be stopped by non-admin processes or scripts.",
            ApplyOps = [RegOp.SetDword(Key, "EnableClientTamperProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableClientTamperProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableClientTamperProtection", 1)],
        },
        new TweakDef
        {
            Id = "confmgr-block-network-access-account-caching",
            Label = "ConfigMgr: Block Caching of Network Access Account Credentials on Client Disk",
            Category = "Configuration Manager Policy",
            Description = "Sets DisableNAACredentialCaching=1 in ConfigurationManager policy. Prevents the ConfigMgr client from caching the Network Access Account (NAA) credentials — the service account used to authenticate with Distribution Points — in the local DPAPI credential store on the client disk. " +
                "The ConfigMgr Network Access Account is a domain service account whose credentials are distributed to all ConfigMgr-managed clients to allow content download from Distribution Points. By default, these credentials are cached on disk using DPAPI. On a compromised endpoint, an attacker can extract the NAA credentials using tools that decrypt DPAPI-protected data (accessible to SYSTEM-level processes) and then use those credentials to authenticate to internal servers as the NAA service account, often a domain user with broad read access.",
            Tags = ["configmgr", "sccm", "naa", "credentials", "dpapi", "credential-theft"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "NAA credential caching disabled; ConfigMgr service account credentials are not stored on client disk after each policy download.",
            ApplyOps = [RegOp.SetDword(Key, "DisableNAACredentialCaching", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNAACredentialCaching")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNAACredentialCaching", 1)],
        },
    ];
}
