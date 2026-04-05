namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicySecurityHardening
{
    // ── SecurityCenterPolicy ──
    private static class _SecurityCenterPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\SecurityCenter";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "secctr-disable-security-center",
                Label = "Disable Windows Security Center",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "Sets NoSecurityCenter=1 in the SecurityCenter policy key. Disables the "
                    + "Windows Security Center (WSC) service and its tray icon. WSC provides "
                    + "unified visibility into antivirus, firewall, updates, and UAC status. "
                    + "Disabling it removes the notification hub but does not disable the "
                    + "underlying security features. Suitable for environments with third-party "
                    + "endpoint management tools. Default: 0. Recommended: enterprise use only.",
                Tags = ["security-center", "wsc", "tray", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoSecurityCenter", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoSecurityCenter")],
                DetectOps = [RegOp.CheckDword(Key, "NoSecurityCenter", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-spyware-monitoring",
                Label = "Disable Security Center Spyware Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableAntiSpywareMonitoring=1 in the SecurityCenter policy key. "
                    + "Prevents Windows Security Center from monitoring and reporting the status "
                    + "of installed anti-spyware solutions. Useful when a third-party endpoint "
                    + "protection platform manages spyware detection and WSC reports a false "
                    + "warning about missing coverage. Default: 0. Recommended: when using a "
                    + "non-WSC-integrated security suite.",
                Tags = ["security-center", "antispyware", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAntiSpywareMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAntiSpywareMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAntiSpywareMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-antivirus-monitoring",
                Label = "Disable Security Center Antivirus Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableAntiVirusMonitoring=1 in the SecurityCenter policy key. Stops "
                    + "Windows Security Center from monitoring and alerting on antivirus product "
                    + "status. Enterprise environments using CrowdStrike, Sentinel One, or other "
                    + "third-party AV tools that do not fully integrate with WSC may see constant "
                    + "warning notifications. This policy suppresses those. "
                    + "Default: 0. Recommended: where third-party AV is managed separately.",
                Tags = ["security-center", "antivirus", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAntiVirusMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAntiVirusMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAntiVirusMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-firewall-monitoring",
                Label = "Disable Security Center Firewall Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableFirewallMonitoring=1 in the SecurityCenter policy key. Prevents "
                    + "Windows Security Center from reporting the status of the Windows firewall or "
                    + "third-party firewall products. When a hardware firewall or enterprise network "
                    + "appliance provides perimeter protection, WSC firewall alerts are noise. "
                    + "Default: 0. Recommended: network-locked enterprise LAN environments.",
                Tags = ["security-center", "firewall", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFirewallMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFirewallMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFirewallMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-update-monitoring",
                Label = "Disable Security Center Windows Update Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableWindowsUpdateMonitoring=1 in the SecurityCenter policy key. "
                    + "Stops Security Center from surfacing Windows Update status alerts. In "
                    + "managed environments where WSUS, MECM, or Intune controls the update "
                    + "schedule, WSC update prompts are redundant and confusing. "
                    + "Default: 0. Recommended: managed update pipelines only.",
                Tags = ["security-center", "windows-update", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWindowsUpdateMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWindowsUpdateMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWindowsUpdateMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-uac-monitoring",
                Label = "Disable Security Center UAC Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableUACMonitoring=1 in the SecurityCenter policy key. Prevents "
                    + "Security Center from monitoring User Account Control state and alerting "
                    + "when UAC is disabled or misconfigured. Useful when UAC policy is managed "
                    + "via separate GP objects and WSC warnings are contradictory to intent. "
                    + "Default: 0. Recommended: only when UAC is governed by dedicated policy.",
                Tags = ["security-center", "uac", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUACMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUACMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUACMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-internet-monitoring",
                Label = "Disable Security Center Internet Security Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableInternetSecurityMonitoring=1 in the SecurityCenter policy key. "
                    + "Stops Security Center from checking and alerting on Internet Explorer / "
                    + "Microsoft Edge security zone settings. In environments where browser "
                    + "security is locked down via separate GP templates, duplicate WSC reports "
                    + "add noise. Default: 0. Recommended: enterprise-managed browser fleets.",
                Tags = ["security-center", "internet", "browser", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInternetSecurityMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInternetSecurityMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInternetSecurityMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-hide-systray-icon",
                Label = "Hide Windows Security Center Tray Icon",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets HideSystrayIcon=1 in the SecurityCenter policy key. Removes the "
                    + "Windows Security tray icon from the notification area, including the "
                    + "shield badge that appears on Start. The underlying Security Center "
                    + "service continues to run; only the visual indicator is suppressed. "
                    + "Useful for kiosk or locked-down desktop configurations. "
                    + "Default: 0 (icon visible). Recommended: kiosk environments.",
                Tags = ["security-center", "systray", "tray", "icon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideSystrayIcon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideSystrayIcon")],
                DetectOps = [RegOp.CheckDword(Key, "HideSystrayIcon", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-account-monitoring",
                Label = "Disable Security Center Account Protection Monitoring",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableAccountMonitoring=1 in the SecurityCenter policy key. Prevents "
                    + "Security Center from surfacing Account Protection recommendations (Windows "
                    + "Hello, lock screen PIN, dynamic lock). In environments using smart-card "
                    + "or certificate-based authentication, these prompts encourage conflicting "
                    + "credential configurations. Default: 0. Recommended: smart-card auth.",
                Tags = ["security-center", "account", "windows-hello", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAccountMonitoring", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAccountMonitoring")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAccountMonitoring", 1)],
            },
            new TweakDef
            {
                Id = "secctr-disable-notifications",
                Label = "Disable Security Center Notification Toasts",
                Category = "Security — Ntlm Authentication",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableNotifications=1 in the SecurityCenter policy key. Suppresses "
                    + "all Security Center action-required and informational notification toasts. "
                    + "WSC notifications for status changes (protection off, update pending) will "
                    + "not appear as toast banners; the Security app itself still shows status. "
                    + "Default: 0. Recommended: managed estates where alerts go to SIEM instead.",
                Tags = ["security-center", "notifications", "toasts", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNotifications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNotifications", 1)],
            },
        ];
    }

    // ── ServiceAccountPolicy ──
    private static class _ServiceAccountPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ServiceAccounts";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "svcact-enable-managed-service-accounts",
                Label = "Enable Managed Service Accounts for Windows Services",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Managed Service Accounts (MSAs) are Active Directory accounts specifically designed for services with automatic password management eliminating manually managed service account passwords. Enabling managed service accounts ensures that service authentication uses automatically rotated passwords that cannot be guessed through credential stuffing attacks. Traditional service accounts use static passwords that are often set to never expire creating long-term credential material vulnerabilities if captured. MSAs integrate with Active Directory to automatically change their passwords on a regular schedule without requiring service restarts. Group Managed Service Accounts (gMSAs) extend MSAs to support multiple servers using the same service account reducing administrative overhead for clustered services. Organizations should migrate all services from domain accounts with static passwords to MSAs or gMSAs during their next infrastructure refresh.",
                Tags = ["service-accounts", "msa", "password-management", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableManagedServiceAccounts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableManagedServiceAccounts")],
                DetectOps = [RegOp.CheckDword(Key, "EnableManagedServiceAccounts", 1)],
            },
            new TweakDef
            {
                Id = "svcact-restrict-service-account-interactive",
                Label = "Prevent Service Accounts from Interactive Logon",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Service accounts should only authenticate to services they run and should never be used for interactive desktop logon on any system. Preventing service account interactive logon ensures that service credentials cannot be used by attackers for lateral movement through terminal services or direct console access. Service accounts used for interactive logon are stronger attack targets as operators may set easily remembered passwords rather than complex automated passwords. Disabling interactive logon for service accounts is enforced through Deny logon locally and Deny logon through Remote Desktop Services user rights. Service account logon restrictions should be enforced across all systems in the domain not just the servers running the specific services. Monitoring for attempted interactive logon with service account credentials is a detection pattern for credential theft and misuse.",
                Tags = ["service-accounts", "interactive-logon", "lateral-movement", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DenyInteractiveLogon", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyInteractiveLogon")],
                DetectOps = [RegOp.CheckDword(Key, "DenyInteractiveLogon", 1)],
            },
            new TweakDef
            {
                Id = "svcact-enforce-service-account-password-complexity",
                Label = "Enforce Strong Password Complexity for Service Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Service account password complexity requirements ensure that manually managed service credentials meet minimum entropy requirements to resist brute-force and dictionary attacks. Enforcing strong passwords for service accounts applies password complexity rules specifically to accounts designated for service use. Service account passwords should be at least 25 characters with mixed character classes to resist offline cracking if the hash is compromised. Password complexity enforcement for service accounts should be higher than standard user requirements due to the elevated privileges service accounts typically hold. Organizations should automate service account password management through vaults like Azure Key Vault or CyberArk PAM rather than relying on manually managed static passwords. Service account password rotation policies should ensure rotation at least quarterly or more frequently for accounts with access to sensitive systems.",
                Tags = ["service-accounts", "password-complexity", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforcePasswordComplexity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePasswordComplexity")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePasswordComplexity", 1)],
            },
            new TweakDef
            {
                Id = "svcact-audit-service-account-usage",
                Label = "Enable Audit Logging for Service Account Authentication",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Service account authentication auditing generates events for all authentication operations performed by service accounts for security monitoring and anomaly detection. Enabling service account audit logging creates visibility into normal service communication patterns that baseline detectors use to identify anomalies. Abnormal service account activity such as authentication to hosts not normally accessed by the service indicates potential credential theft and lateral movement. Service account credential stuffing attacks where the same service credentials are tried against multiple systems are detectable through authentication audit events. Service account audit data should be forwarded to SIEM with behavioral analytics to detect deviations from established normal authentication patterns. Regular review of service account audit data reduces the time to detect service account compromise which is a common attacker technique.",
                Tags = ["service-accounts", "audit", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditServiceAccountUsage", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditServiceAccountUsage")],
                DetectOps = [RegOp.CheckDword(Key, "AuditServiceAccountUsage", 1)],
            },
            new TweakDef
            {
                Id = "svcact-restrict-service-account-delegation",
                Label = "Restrict Unconstrained Kerberos Delegation for Service Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "Unconstrained Kerberos delegation allows a service to request Kerberos tickets on behalf of any principal and present them to any service creating a powerful privilege escalation path. Restricting unconstrained delegation forces service accounts to use constrained delegation specifying only the services they are authorized to access on behalf of users. Unconstrained delegation accounts are commonly targeted in Active Directory attacks because compromising them provides lateral movement to any service authenticated users access through them. Constrained delegation limits the blast radius of a service account compromise to only the specific services defined in its delegation configuration. Resource-Based Constrained Delegation is the most modern form and should be preferred over traditional constrained delegation for new service deployments. Organizations should audit all accounts with unconstrained delegation enabled and restrict them to constrained delegation as a priority remediation.",
                Tags = ["service-accounts", "kerberos", "delegation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictUnconstrainedDelegation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictUnconstrainedDelegation")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictUnconstrainedDelegation", 1)],
            },
            new TweakDef
            {
                Id = "svcact-enable-tiered-service-access",
                Label = "Enable Tiered Access for Service Accounts by Privilege Level",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Tiered access control for service accounts restricts service account usage to specific privilege tiers preventing tier-0 service accounts from being used on tier-1 or tier-2 systems. Enabling tiered service account access implements Active Directory tiered administrative model principles for non-administrative service accounts. Service accounts with access to tier-0 systems like domain controllers should be different from service accounts used on tier-1 member servers. Tier-based separation prevents lateral movement from lower-tier compromised systems from directly reaching tier-0 infrastructure through service account credentials. Tiered service accounts should be enforced through security group policies that deny logon rights at inappropriate tiers. The Microsoft Enterprise Access Model documentation provides implementation guidance for tiered service account architecture.",
                Tags = ["service-accounts", "tiered-access", "privilege-isolation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTieredServiceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTieredServiceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTieredServiceAccess", 1)],
            },
            new TweakDef
            {
                Id = "svcact-prevent-spn-enumeration",
                Label = "Restrict Unauthenticated Service Principal Name Enumeration",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Service Principal Name (SPN) enumeration allows attackers to discover service accounts with SPNs for Kerberoasting attacks requesting and offline cracking Kerberos service tickets. Restricting SPN enumeration to authenticated domain members reduces the ease of discovering Kerberoastable service accounts. While all authenticated users can query SPN data by default additional restrictions can limit SPN data visibility to authorized groups. Service accounts with weak passwords are vulnerable to Kerberoasting even with SPN visibility restrictions so password strength is complementary to this control. Organizations should implement long random passwords for all service accounts with SPNs as the primary Kerberoasting defense. Alerting on high-volume SPN queries from a single source helps detect Kerberoasting enumeration activities in the domain.",
                Tags = ["service-accounts", "spn", "kerberoasting", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSPNEnumeration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSPNEnumeration")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSPNEnumeration", 1)],
            },
            new TweakDef
            {
                Id = "svcact-disable-service-account-email-usage",
                Label = "Prevent Service Accounts from Accessing Email Mailboxes",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Service accounts should not be associated with email mailboxes as phishing attacks against service accounts via email are a common compromise vector. Disabling email access for service accounts prevents attackers from using phishing or credential stuffing to access corporate email through service account credentials. Service accounts with email access have been used to exfiltrate sensitive data by attackers who compromise them through shared password reuse. Service accounts by definition are not human users and do not need email communication capabilities for their service functions. Organizations should disable Exchange Online licensing for service accounts and configure mailbox restrictions to prevent service account email access. Service accounts found accessing email in audit logs should be investigated immediately as this indicates potential credential compromise.",
                Tags = ["service-accounts", "email", "phishing-resistance", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableServiceAccountEmail", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableServiceAccountEmail")],
                DetectOps = [RegOp.CheckDword(Key, "DisableServiceAccountEmail", 1)],
            },
            new TweakDef
            {
                Id = "svcact-set-service-account-logon-hours",
                Label = "Restrict Service Account Logon to Business Hours Windows",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Service account logon hour restrictions limit when interactive authentication with service credentials can occur reducing the window available for credential abuse. Restricting service account logon to operational hours creates anomaly detection opportunities when service accounts are used outside their normal hours. Service accounts running scheduled tasks may need logon hours that cover their operational schedule rather than full 24-hour access. Attackers who capture service credentials prefer to use them during off-hours when security monitoring coverage may be lower making logon hour restrictions an effective detective control. Service accounts running continuous services like databases require logon access at all hours but batch processing accounts can be restricted to business hours. Logon attempts outside the defined window generate authentication failure events that should be alerted on immediately.",
                Tags = ["service-accounts", "logon-hours", "access-control", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceLogonHours", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceLogonHours")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceLogonHours", 1)],
            },
            new TweakDef
            {
                Id = "svcact-enable-just-in-time-service-access",
                Label = "Enable Just-in-Time Access Elevation for Service Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Just-in-time access elevation for service accounts grants elevated service permissions only when needed and revokes them after the task completes reducing standing privilege. Enabling JIT access for service privileges ensures that service accounts do not continuously hold elevated rights that could be abused if the account is compromised. Standing elevated privileges are the primary enabler of lateral movement where a single compromised account can immediately access critical resources. JIT access implementation requires a Privileged Access Management system like Azure AD PIM or CyberArk PAM to issue time-limited permission grants. Service account JIT access is more complex to implement than user JIT but provides the same significant security benefits for automated workloads. Organizations implementing JIT for service accounts should prioritize accounts with access to sensitive data or critical infrastructure.",
                Tags = ["service-accounts", "jit", "privileged-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableJITServiceAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableJITServiceAccess")],
                DetectOps = [RegOp.CheckDword(Key, "EnableJITServiceAccess", 1)],
            },
        ];
    }

    // ── SystemGuardRuntimePolicy ──
    private static class _SystemGuardRuntimePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\SystemGuardRuntime";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sgrm-enable-system-guard-runtime",
                    Label = "Enable System Guard Runtime Monitor",
                    Category = "Security — Service Account",
                    Description =
                        "Enables System Guard Runtime Monitor (SGRM) which continuously monitors the OS security state during runtime using VBS, detecting live kernel patching, rootkits, and hypervisor attacks.",
                    Tags = ["system-guard", "runtime-monitor", "vbs", "attestation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "SGRM active; runtime kernel integrity monitored; rootkits and hypervisor attacks detected.",
                    ApplyOps = [RegOp.SetDword(Key, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Enabled")],
                    DetectOps = [RegOp.CheckDword(Key, "Enabled", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-require-firmware-attestation",
                    Label = "Require Firmware Attestation in System Guard",
                    Category = "Security — Service Account",
                    Description =
                        "Requires System Guard to include firmware measurement (SMBIOS, ACPI tables, UEFI variables) in its attestation report, detecting firmware-level tampering via Microsoft AAP cloud verification.",
                    Tags = ["system-guard", "firmware", "attestation", "uefi", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Firmware attestation included; unauthorized UEFI/BIOS changes detectable via cloud attestation.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireFirmwareAttestation", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireFirmwareAttestation")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireFirmwareAttestation", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-enable-kernel-dma-runtime-check",
                    Label = "Enable Runtime DMA Remapping Check in System Guard",
                    Category = "Security — Service Account",
                    Description =
                        "Enables System Guard to continuously verify that the Intel VT-d / AMD-Vi DMA remapping tables are not tampered with at runtime, detecting DMA remap attacks from existing PCIe devices.",
                    Tags = ["system-guard", "dma", "vtd", "pcie", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "DMA remap table integrity checked at runtime; live DMA table tamper attacks detected.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableKernelDMARuntimeCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableKernelDMARuntimeCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableKernelDMARuntimeCheck", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-enable-memory-scan",
                    Label = "Enable System Guard Memory Scan for Rootkits",
                    Category = "Security — Service Account",
                    Description =
                        "Enables System Guard runtime memory scanning to detect kernel rootkits and bootkits that modify kernel data structures, leveraging VBS isolation to inspect kernel memory safely.",
                    Tags = ["system-guard", "memory-scan", "rootkit", "vbs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Kernel memory scanned at runtime; rootkits targeting SSDT/DKOM/EPROCESS detected.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRuntimeMemoryScan", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRuntimeMemoryScan")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRuntimeMemoryScan", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-set-attestation-interval",
                    Label = "Set System Guard Attestation Report Interval to 60 Minutes",
                    Category = "Security — Service Account",
                    Description =
                        "Configures System Guard to generate and upload attestation reports every 60 minutes, ensuring near-real-time security state visibility in Microsoft Defender for Endpoint.",
                    Tags = ["system-guard", "attestation", "interval", "mde", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Attestation reports every 60 minutes; security state visible in MDE within 1 hour of a compromise.",
                    ApplyOps = [RegOp.SetDword(Key, "AttestationIntervalMinutes", 60)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AttestationIntervalMinutes")],
                    DetectOps = [RegOp.CheckDword(Key, "AttestationIntervalMinutes", 60)],
                },
                new TweakDef
                {
                    Id = "sgrm-block-anti-cheat-bypass",
                    Label = "Block Anti-Cheat Driver from Bypassing System Guard",
                    Category = "Security — Service Account",
                    Description =
                        "Blocks kernel-mode anti-cheat and DRM drivers from acquiring write access to kernel memory regions protected by System Guard, preventing anti-cheat tools from weakening system integrity.",
                    Tags = ["system-guard", "anti-cheat", "drm", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Anti-cheat drivers with kernel write access blocked; some games with invasive anti-cheat may not run.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockAntiCheatDriverBypass", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockAntiCheatDriverBypass")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockAntiCheatDriverBypass", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-enable-hypervisor-integrity-check",
                    Label = "Enable Hypervisor Page Table Integrity Check",
                    Category = "Security — Service Account",
                    Description =
                        "Enables System Guard to verify the integrity of the Hyper-V hypervisor page table entries at runtime, detecting attacks that modify the hypervisor's own memory mappings.",
                    Tags = ["system-guard", "hypervisor", "page-table", "integrity", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Hypervisor page table verified; attacks targeting VBS isolation layers detected.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableHypervisorIntegrityCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableHypervisorIntegrityCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableHypervisorIntegrityCheck", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-secure-world-crash-dump-policy",
                    Label = "Restrict Crash Dump Access from Outside Secure World",
                    Category = "Security — Service Account",
                    Description =
                        "Configures System Guard to prevent the normal OS kernel dump process from reading VBS Secure World memory, ensuring credential material and kernel secrets are not exposed in crash dumps.",
                    Tags = ["system-guard", "crash-dump", "secure-world", "credential-protection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Secure World memory excluded from crash dumps; credentials not leaked in BSOD minidumps.",
                    ApplyOps = [RegOp.SetDword(Key, "RestrictCrashDumpFromSecureWorld", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrashDumpFromSecureWorld")],
                    DetectOps = [RegOp.CheckDword(Key, "RestrictCrashDumpFromSecureWorld", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-require-modern-standby-isolation",
                    Label = "Require Hardware Isolation During Modern Standby",
                    Category = "Security — Service Account",
                    Description =
                        "Requires System Guard to maintain hardware VBS isolation during Modern Standby (DRIPS) low-power states, preventing attacks that exploit relaxed memory permissions during sleep transitions.",
                    Tags = ["system-guard", "modern-standby", "sleepstate", "vbs", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "VBS isolation maintained in Modern Standby; sleep-state memory attacks mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireIsolationDuringModernStandby", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireIsolationDuringModernStandby")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireIsolationDuringModernStandby", 1)],
                },
                new TweakDef
                {
                    Id = "sgrm-enable-post-boot-runtime-check",
                    Label = "Enable Post-Boot Runtime Integrity Check",
                    Category = "Security — Service Account",
                    Description =
                        "Enables a periodic post-boot integrity check by System Guard that verifies critical kernel structures have not been modified since boot, catching late-stage kernel tampering.",
                    Tags = ["system-guard", "post-boot", "integrity", "kernel", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Kernel structures periodically re-checked post-boot; late-stage kernel rootkits detected.",
                    ApplyOps = [RegOp.SetDword(Key, "EnablePostBootRuntimeCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnablePostBootRuntimeCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "EnablePostBootRuntimeCheck", 1)],
                },
            ];
    }

    // ── TaskSchedulerSecurityPolicy ──
    private static class _TaskSchedulerSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0";
        private const string CompatKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0\Compatibility";
        private const string MaintKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Task Scheduler5.0\Maintenance";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "schtasksec-disable-task-creation",
                    Label = "Prevent Non-Admin Task Creation",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents standard users from creating new scheduled tasks. Only administrators can create, modify, or delete tasks. Default: allowed. Recommended for hardened systems.",
                    Tags = ["scheduled-tasks", "security", "hardening", "user-restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Standard users cannot create scheduled tasks; admin tasks unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "TaskCreation", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TaskCreation")],
                    DetectOps = [RegOp.CheckDword(Key, "TaskCreation", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-task-deletion",
                    Label = "Prevent Non-Admin Task Deletion",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents standard users from deleting existing scheduled tasks. Protects system maintenance and backup tasks from accidental removal. Default: allowed.",
                    Tags = ["scheduled-tasks", "security", "hardening", "user-restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Standard users cannot remove any scheduled tasks.",
                    ApplyOps = [RegOp.SetDword(Key, "TaskDeletion", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TaskDeletion")],
                    DetectOps = [RegOp.CheckDword(Key, "TaskDeletion", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-execution-control",
                    Label = "Prevent Non-Admin Manual Task Execution",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents standard users from running existing tasks on-demand (right-click → Run). Tasks still execute on their configured schedule. Default: allowed.",
                    Tags = ["scheduled-tasks", "security", "hardening", "execution", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Standard users can't manually trigger tasks; scheduled execution continues normally.",
                    ApplyOps = [RegOp.SetDword(Key, "Execution", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Execution")],
                    DetectOps = [RegOp.CheckDword(Key, "Execution", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-drag-drop-run",
                    Label = "Disable Drag-and-Drop Task Execution",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents running a scheduled task by dragging and dropping a file onto its entry. Reduces accidental or social-engineering-based task execution. Default: allowed.",
                    Tags = ["scheduled-tasks", "security", "hardening", "drag-drop", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Drag-and-drop execution blocked; no impact on normal scheduled execution.",
                    ApplyOps = [RegOp.SetDword(Key, "DragAndDrop", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DragAndDrop")],
                    DetectOps = [RegOp.CheckDword(Key, "DragAndDrop", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-property-pages",
                    Label = "Hide Task Scheduler Property Pages",
                    Category = "Security — Service Account",
                    Description =
                        "Hides the property pages (settings, triggers, conditions, history) for existing scheduled tasks from standard users. Prevents information disclosure of task details. Default: visible.",
                    Tags = ["scheduled-tasks", "security", "information-disclosure", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Standard users cannot view task configuration details; admin view unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "PropertyPages", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PropertyPages")],
                    DetectOps = [RegOp.CheckDword(Key, "PropertyPages", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-browse-network",
                    Label = "Disable Network Browse in Task Scheduler",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents the Task Scheduler from browsing the network for remote task targets. Limits attack surface when managing scheduled tasks. Default: allowed.",
                    Tags = ["scheduled-tasks", "security", "network", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Cannot browse remote computers from Task Scheduler; local management unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowBrowse", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowBrowse")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowBrowse", 0)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-at-compatibility",
                    Label = "Disable AT Command Compatibility in Task Scheduler",
                    Category = "Security — Service Account",
                    Description =
                        "Disables the legacy AT.exe command compatibility layer. AT-scheduled tasks bypass modern security controls. Default: compatible.",
                    Tags = ["scheduled-tasks", "security", "legacy", "at-command", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Legacy AT.exe tasks no longer execute; modern schtasks/Task Scheduler UI unaffected.",
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisableATCompatibility", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisableATCompatibility")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisableATCompatibility", 1)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-v1-api",
                    Label = "Disable Task Scheduler 1.0 API Compatibility",
                    Category = "Security — Service Account",
                    Description =
                        "Disables the legacy Task Scheduler 1.0 COM API. Prevents applications using the old interface from creating or modifying tasks. Default: enabled.",
                    Tags = ["scheduled-tasks", "security", "legacy", "api", "com", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Legacy COM-based task management disabled; may break old automation scripts.",
                    ApplyOps = [RegOp.SetDword(CompatKey, "DisableV1Api", 1)],
                    RemoveOps = [RegOp.DeleteValue(CompatKey, "DisableV1Api")],
                    DetectOps = [RegOp.CheckDword(CompatKey, "DisableV1Api", 1)],
                },
                new TweakDef
                {
                    Id = "schtasksec-set-maint-boundary-days",
                    Label = "Set Maintenance Task Deadline to 7 Days",
                    Category = "Security — Service Account",
                    Description =
                        "Sets the automatic maintenance activation boundary to 7 days. If maintenance has not run in 7 days, the system forces it on next idle. Default: 2 days.",
                    Tags = ["scheduled-tasks", "maintenance", "deadline", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maintenance forced after 7 days of missed windows; more lenient than default 2 days.",
                    ApplyOps = [RegOp.SetDword(MaintKey, "DeadlineDays", 7)],
                    RemoveOps = [RegOp.DeleteValue(MaintKey, "DeadlineDays")],
                    DetectOps = [RegOp.CheckDword(MaintKey, "DeadlineDays", 7)],
                },
                new TweakDef
                {
                    Id = "schtasksec-disable-maint-wakeup",
                    Label = "Disable Maintenance Task Wake-Up Timer",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents the automatic maintenance task from waking the computer from sleep. Maintenance only runs when the device is already awake. Default: may wake.",
                    Tags = ["scheduled-tasks", "maintenance", "wake-timer", "power", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Computer not woken from sleep for maintenance; maintenance runs on next idle wake.",
                    ApplyOps = [RegOp.SetDword(MaintKey, "WakeUp", 0)],
                    RemoveOps = [RegOp.DeleteValue(MaintKey, "WakeUp")],
                    DetectOps = [RegOp.CheckDword(MaintKey, "WakeUp", 0)],
                },
            ];
    }

    // ── TokenBrokerPolicy ──
    private static class _TokenBrokerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TokenBroker";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tokbrk-disable-token-broker",
                Label = "Disable Web Account Token Broker",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets DisableTokenBroker=1 in the TokenBroker policy key. Prevents the "
                    + "Web Account Manager (WAM) token broker from brokering OAuth access "
                    + "tokens between UWP and Win32 applications and Microsoft identity "
                    + "endpoints. Token brokering silently reuses cached Microsoft account "
                    + "credentials across applications without per-request user consent. "
                    + "Disabling may break SSO workflows. Default: 0. Recommended: 1.",
                Tags = ["token", "broker", "wam", "oauth", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTokenBroker", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTokenBroker")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTokenBroker", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-persistent-token-cache",
                Label = "Disable Persistent Token Cache",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisablePersistentTokenCache=1 in the TokenBroker policy key. "
                    + "Prevents Web Account Manager from persisting OAuth refresh tokens to "
                    + "the Windows Credential Locker across reboots. Without persistence "
                    + "tokens expire on sign-out and cannot be reused after a cold start, "
                    + "limiting the window for token theft via offline credential-store attacks. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["token", "cache", "credential", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePersistentTokenCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePersistentTokenCache")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePersistentTokenCache", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-background-token-refresh",
                Label = "Disable Background Token Refresh",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableBackgroundTokenRefresh=1 in the TokenBroker policy key. "
                    + "Prevents WAM from silently refreshing expiring access tokens in the "
                    + "background before the calling application requests them. Background "
                    + "refresh tasks contact Azure AD or MSA endpoints at unpredictable "
                    + "intervals, creating covert network egress that is invisible to manual "
                    + "connection-monitoring tools. Default: 0. Recommended: 1.",
                Tags = ["token", "refresh", "background", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundTokenRefresh", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundTokenRefresh")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundTokenRefresh", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-aad-token-sharing",
                Label = "Disable Azure AD Token Sharing Between Apps",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAadTokenSharing=1 in the TokenBroker policy key. Blocks WAM "
                    + "from sharing a single Azure AD access token issued to one application "
                    + "with other requesting applications on the same machine. Token sharing "
                    + "enables unexpected cross-application impersonation; each application "
                    + "should acquire its own token with its own consent scope. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["aad", "token", "sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAadTokenSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAadTokenSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAadTokenSharing", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-msa-token-sharing",
                Label = "Disable Microsoft Account Token Sharing",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableMsaTokenSharing=1 in the TokenBroker policy key. Prevents WAM "
                    + "from sharing MSA (personal Microsoft account) access tokens between "
                    + "installed applications. This stops first-party Microsoft apps with broad "
                    + "scopes from silently delegating access to third-party applications that "
                    + "have registered as WAM token consumers. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["msa", "token", "sharing", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMsaTokenSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMsaTokenSharing")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMsaTokenSharing", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-require-user-consent",
                Label = "Require Explicit User Consent for Token Grants",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets RequireUserConsentForTokenGrant=1 in the TokenBroker policy key. "
                    + "Forces WAM to display a consent dialog each time an application requests "
                    + "an access token, instead of silently granting from cache. Explicit "
                    + "consent makes token acquisition visible to the user and prevents "
                    + "applications from quietly accumulating access to cloud resources without "
                    + "awareness. Default: 0. Recommended: 1.",
                Tags = ["token", "consent", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireUserConsentForTokenGrant", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireUserConsentForTokenGrant")],
                DetectOps = [RegOp.CheckDword(Key, "RequireUserConsentForTokenGrant", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-token-telemetry",
                Label = "Disable Token Broker Telemetry",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableTokenBrokerTelemetry=1 in the TokenBroker policy key. Stops "
                    + "WAM from emitting diagnostic events that log token-request attempts, "
                    + "grant results, application identifiers, and API surface usage to Windows "
                    + "telemetry ingestion. Token-request metadata can reveal which cloud "
                    + "services applications are accessing without consent from the user. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["token", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTokenBrokerTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTokenBrokerTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTokenBrokerTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-implicit-account-discovery",
                Label = "Disable Implicit Account Discovery",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableImplicitAccountDiscovery=1 in the TokenBroker policy key. "
                    + "Prevents WAM from enumerating Microsoft accounts and Azure AD accounts "
                    + "registered on the device to pre-populate sign-in flows in UWP apps. "
                    + "Implicit discovery leaks the list of associated accounts to requesting "
                    + "applications before any explicit user action. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["token", "account", "discovery", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableImplicitAccountDiscovery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableImplicitAccountDiscovery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableImplicitAccountDiscovery", 1)],
            },
            new TweakDef
            {
                Id = "tokbrk-limit-token-lifetime",
                Label = "Limit OAuth Token Lifetime",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets MaxTokenLifetimeMinutes=60 in the TokenBroker policy key. Caps the "
                    + "maximum lifetime of access tokens cached by WAM to 60 minutes. Short "
                    + "token lifetimes reduce the window during which a stolen token remains "
                    + "usable and force more frequent re-authentication events that validate "
                    + "the principal's current session state against the identity provider. "
                    + "Default: not set (provider default, often 60–90 mins). Recommended: 60.",
                Tags = ["token", "lifetime", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxTokenLifetimeMinutes", 60)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxTokenLifetimeMinutes")],
                DetectOps = [RegOp.CheckDword(Key, "MaxTokenLifetimeMinutes", 60)],
            },
            new TweakDef
            {
                Id = "tokbrk-disable-enterprise-sso",
                Label = "Disable Enterprise SSO Token Propagation",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DisableEnterpriseSso=1 in the TokenBroker policy key. Prevents WAM "
                    + "from propagating a Kerberos or NTLM enterprise SSO token to non-enrolled "
                    + "applications requesting Windows-integrated authentication. SSO propagation "
                    + "to arbitrary applications can enable CSRF-style attacks where a malicious "
                    + "application abuses enterprise credentials obtained via token forwarding. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["sso", "enterprise", "token", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEnterpriseSso", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEnterpriseSso")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEnterpriseSso", 1)],
            },
        ];
    }

    // ── TokenPrivilegePolicy ──
    private static class _TokenPrivilegePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Privileges";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tokpriv-restrict-debug-privilege-assignment",
                Label = "Restrict Assignment of Debug Programs Privilege to Authorized Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "The SeDebugPrivilege token privilege allows processes that hold it to debug any other process on the system by attaching a debugger granting full read and write access to the target process's memory including LSASS where credentials are stored. Restricting assignment of debug privilege is critical because it is the primary privilege exploited by credential dumping tools such as Mimikatz that read LSASS memory to extract plaintext passwords and Kerberos tickets. By default only members of the local Administrators group hold debug privilege but privilege escalation vulnerabilities or misconfigured services can grant this privilege to unauthorized accounts. Organizations should audit and minimize the number of accounts that are assigned debug privilege reviewing each account's business justification. Developer accounts that require debug privilege for legitimate software development work should use separate privileged accounts rather than their standard user accounts. Changes to debug privilege assignments should trigger security alerts for immediate investigation.",
                Tags = ["debug-privilege", "se-debug", "credential-protection", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictDebugPrivilegeAssignment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictDebugPrivilegeAssignment")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictDebugPrivilegeAssignment", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-audit-privilege-use-events",
                Label = "Enable Audit Logging for Sensitive Privilege Use Security Events",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Auditing sensitive privilege use events records use of security-sensitive Windows privileges to the security event log providing visibility into high-risk operations that could indicate privilege abuse or compromise. Sensitive privileges include SeDebugPrivilege SeTcbPrivilege SeLoadDriverPrivilege SeTakeOwnershipPrivilege and SeSecurityPrivilege each of which can be used to escalate control over system resources. Security operations teams should baseline normal patterns of privilege use and alert on anomalous privilege use that deviates from established patterns. Windows generates Event ID 4672 for special logon with sensitive privileges and Event ID 4673 for privilege use operations that should both be monitored. Over-monitoring of non-sensitive privilege use can generate excessive event volume so organizations should focus auditing on the specific high-risk privileges most likely to be exploited. Privilege use audit events should be correlated with identity events to identify privilege abuse patterns across multiple systems.",
                Tags = ["privilege-audit", "security-events", "monitoring", "privilege-use", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditPrivilegeUseEvents", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditPrivilegeUseEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditPrivilegeUseEvents", 3)],
            },
            new TweakDef
            {
                Id = "tokpriv-restrict-take-ownership-privilege",
                Label = "Restrict SeTakeOwnershipPrivilege Assignment on Domain and Local Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SeTakeOwnershipPrivilege allows accounts that hold it to take ownership of any file object directory registry key or other securable object on the system regardless of the object's current discretionary access control list. Misuse of take ownership privilege can bypass file and registry ACL protections and allow unauthorized modification of system files security configuration files or credential stores. Ransomware and destructive malware can use take ownership privilege to override ACLs protecting backup data preventing recovery. Standard user accounts and service accounts should never be assigned take ownership privilege without written justification and security review. IT administrator accounts that have legitimate requirements to use take ownership in specific operational scenarios should be required to use separate privileged accounts and just-in-time access. Audit events for take ownership privilege use should be reviewed regularly to ensure that ownership changes are operation-appropriate.",
                Tags = ["take-ownership", "acl-bypass", "privilege-restriction", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictTakeOwnershipPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictTakeOwnershipPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictTakeOwnershipPrivilege", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-block-load-driver-privilege-expansion",
                Label = "Block Unauthorized Expansion of Load Driver Privilege to Standard Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "SeLoadDriverPrivilege enables accounts that hold it to load kernel mode device drivers which execute with the highest privilege level on Windows and can fully control the operating system with no security boundary blocking their access. Malicious drivers loaded by compromised accounts with load driver privilege can disable security software including antivirus and endpoint detection and response agents at the kernel level. Blocking expansion of load driver privilege prevents privilege escalation vectors that involve convincing applications with load driver privilege to load attacker-controlled drivers. Only the most privileged system accounts should hold load driver privilege and mechanisms like Windows Driver Signature Enforcement and Kernel Mode Code Signing Policy should complement privilege restriction. Organizations should verify that driver loading is controlled through enterprise driver deployment mechanisms rather than individual user-initiated loading operations. Driver load events should generate security alerts for security operations investigation.",
                Tags = ["driver-loading", "kernel-privilege", "privilege-escalation", "driver-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockLoadDriverPrivilegeExpansion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockLoadDriverPrivilegeExpansion")],
                DetectOps = [RegOp.CheckDword(Key, "BlockLoadDriverPrivilegeExpansion", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-enforce-privilege-token-filtering",
                Label = "Enforce Privilege Token Filtering for Administrative Access on Domain Systems",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Windows User Account Control token filtering splits administrator tokens into a limited user token and a full administrator token requiring elevation for operations that need the full administrator privileges. Enforcing privilege token filtering ensures that remote administrative access to systems uses split tokens by default limiting the damage that can be done if an administrator session is compromised. The UAC token filtering for remote access is controlled by the LocalAccountTokenFilterPolicy setting and should be configured to filter remote access tokens for all accounts. Local administrator accounts used for remote administration are particularly vulnerable to pass-the-hash attacks when they hold full administrative tokens for remote sessions. Token filtering for remote access should be complemented by restricting which accounts can perform remote administrative access using restricted admin mode or Windows Defender Remote Credential Guard. Domain accounts with local administrative rights should have token filtering applied consistently across all domain systems.",
                Tags = ["token-filtering", "uac", "remote-access", "pass-the-hash", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforcePrivilegeTokenFiltering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforcePrivilegeTokenFiltering")],
                DetectOps = [RegOp.CheckDword(Key, "EnforcePrivilegeTokenFiltering", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-restrict-impersonate-client-privilege",
                Label = "Restrict SeImpersonatePrivilege to Service Accounts That Require Impersonation",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "SeImpersonatePrivilege allows a service account to impersonate a client that has authenticated to the service presenting the client's security identity for access checks on other resources during the service operation. Misuse of impersonation privilege through named pipe impersonation attacks or token kidnapping can allow a low-privileged service to escalate to SYSTEM by impersonating a higher-privileged process that connects to a service endpoint. Impersonation privilege is required by many legitimate Windows services including IIS RPC services and database servers so blanket removal is not feasible but the list of accounts holding impersonation privilege should be minimized. Service accounts that do not service remote clients should not hold impersonation privilege and should be configured with SeImpersonatePrivilege explicitly removed. Named pipe impersonation attacks are mitigated by network access policies that restrict which accounts can create named pipes and network logon type restrictions. Security review of accounts holding impersonation privilege should verify each account's impersonation use case is valid and necessary.",
                Tags = ["impersonate-privilege", "privilege-escalation", "service-accounts", "impersonation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictImpersonateClientPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictImpersonateClientPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictImpersonateClientPrivilege", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-block-privilege-abuse-alerts",
                Label = "Enable Real-Time Alerts for Privilege Abuse and Excessive Privilege Operations",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Real-time privilege abuse alerts notify security operations teams immediately when sensitive privileges are exercised in ways that deviate from established operational baselines providing early attack detection capability. Privilege abuse alerts should be configured for all operations involving debug privilege take ownership privilege and load driver privilege regardless of the identity of the account performing the operation. Time-of-day baseline analysis can identify privilege use that occurs outside normal business hours which may indicate account compromise or insider threat activity. Volume-based analysis should detect privilege escalation attacks where an attacker attempts many privilege operations in rapid succession. Privilege abuse alerts should be integrated with the security incident response process to ensure that alerts are triaged and investigated within defined service level objectives. False positive tuning for privilege abuse alerts is important to avoid alert fatigue which reduces the effectiveness of security monitoring programs.",
                Tags = ["privilege-abuse", "real-time-alerts", "security-monitoring", "incident-detection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnablePrivilegeAbuseAlerts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnablePrivilegeAbuseAlerts")],
                DetectOps = [RegOp.CheckDword(Key, "EnablePrivilegeAbuseAlerts", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-enforce-time-limited-privilege-grants",
                Label = "Enforce Time-Limited Just-In-Time Privilege Elevation for Administrative Tasks",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Time-limited privilege grants require that elevated privileges are explicitly requested for specific administrative tasks and automatically revoke the elevated privileges when the administrative session ends rather than having privileges permanently assigned to accounts. Just-in-time privilege access reduces the window of opportunity for attackers to exploit administrator accounts by limiting the duration during which elevated privileges are available. Permanent privilege assignments create long-lived attack opportunities while time-limited grants ensure that account compromise during non-administrative periods does not provide attacker access to administrative capabilities. JIT privilege systems should require multi-factor authentication for all privilege elevation requests to ensure that privilege is granted only to authenticated users with legitimate needs. Activity performed under JIT privilege sessions should be recorded for audit and investigation purposes. JIT privilege grant workflows should integrate with change management systems to ensure that privilege elevation requests are tied to approved work orders.",
                Tags = ["jit-privilege", "time-limited", "privilege-grants", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceTimeLimitedPrivilegeGrants", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceTimeLimitedPrivilegeGrants")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceTimeLimitedPrivilegeGrants", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-restrict-backup-restore-privileges",
                Label = "Restrict SeBackupPrivilege and SeRestorePrivilege to Authorized Backup Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SeBackupPrivilege and SeRestorePrivilege allow accounts to bypass file and directory ACLs when reading or writing files as part of backup and restore operations making them powerful privileges that can be used to read any file regardless of access controls. SeBackupPrivilege is exploited by attackers to read the NTDS.dit Active Directory database file and the SYSTEM registry hive enabling full domain credential extraction even without direct access to LSASS memory. These privileges are assigned to backup service accounts and the Backup Operators group by default and the membership of Backup Operators should be reviewed to ensure only authorized backup service accounts are included. Backup accounts should be monitored for use of backup and restore privileges outside of scheduled backup windows as out-of-window use may indicate compromise. File access under backup and restore privileges should be logged with sufficient detail to reconstruct which files were accessed by the backup process. Segregation of backup privileges should be implemented where separate accounts perform backup and restore operations to limit the capabilities available to any single compromised account.",
                Tags = ["backup-privilege", "restore-privilege", "acl-bypass", "ntds-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictBackupRestorePrivileges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictBackupRestorePrivileges")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictBackupRestorePrivileges", 1)],
            },
            new TweakDef
            {
                Id = "tokpriv-block-assign-primary-token-privilege",
                Label = "Block Unauthorized Assignment of Primary Token Privilege to Service Accounts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The SeAssignPrimaryTokenPrivilege allows a service to create child processes that run under a different security token enabling the calling process to run new processes with the token of a different user account without going through standard logon procedures. Unauthorized use of assign primary token privilege can be used to start processes with stolen or forged tokens allowing privilege escalation beyond the account level of the service that holds the privilege. This privilege is required by the Windows service account infrastructure and specific system services but should not be held by application service accounts. Service accounts that do not spawn child processes under different security contexts should have assign primary token privilege removed from their effective privilege set. New service deployments should be reviewed to verify that assign primary token privilege is not included in the account rights unless there is documented justification for needing this capability. Security testing of service accounts should verify that assign primary token privilege cannot be exploited for privilege escalation through token swapping techniques.",
                Tags = ["primary-token", "privilege-assignment", "service-security", "token-escalation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockAssignPrimaryTokenPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockAssignPrimaryTokenPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "BlockAssignPrimaryTokenPrivilege", 1)],
            },
        ];
    }

    // ── TpmAdvancedPolicy ──
    private static class _TpmAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tpmadv-configure-tpm-lockout-duration",
                Label = "Configure TPM Lockout Duration for Failed Authorization Attempts",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "TPM lockout duration controls how long the TPM enters a lockout state after excessive failed authorization attempts preventing brute-force attacks against TPM-protected credentials. Configuring an appropriate lockout duration ensures that TPM-based authentication cannot be brute-forced while minimizing disruption from accidental lockout. TPM 2.0 devices implement anti-hammering protection that progressively increases lockout duration after failed attempts up to a maximum duration defined by this policy. A standard lockout duration of 2 hours provides reasonable brute-force protection while limiting the operational impact of accidental lockouts. Organizations with strict security requirements may extend the lockout duration to 24 hours or more to extend the time required for physical or software attacks. TPM lockout events should be monitored as they may indicate attempted credential attacks against BitLocker recovery or other TPM-protected data.",
                Tags = ["tpm", "lockout", "anti-hammering", "brute-force", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "OSManagedAuthLevel", 4)],
                RemoveOps = [RegOp.DeleteValue(Key, "OSManagedAuthLevel")],
                DetectOps = [RegOp.CheckDword(Key, "OSManagedAuthLevel", 4)],
            },
            new TweakDef
            {
                Id = "tpmadv-require-tpm-for-bitlocker",
                Label = "Require TPM for BitLocker Drive Encryption Operations",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "BitLocker with a TPM provides hardware-based key protection ensuring the encryption key is only released when the measured boot state matches the expected configuration. Requiring TPM for BitLocker prevents use of the software-only BitLocker mode that stores the key on a USB drive and lacks hardware binding. TPM-protected BitLocker keys are released based on platform configuration register (PCR) measurements that detect changes to the boot sequence that may indicate tampering. BitLocker without TPM using only a USB key or password provides encryption at rest but without the hardware-based tamper detection of TPM-bound keys. Organizations should require TPM for all BitLocker deployments and ensure TPM presence is verified as part of standard hardware procurement. BitLocker with TPM plus PIN or TPM plus USB key provides multi-factor authentication for disk encryption addressing scenarios where physical theft is a primary concern.",
                Tags = ["tpm", "bitlocker", "encryption", "hardware-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireTpmForBitLocker", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTpmForBitLocker")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTpmForBitLocker", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-enable-tpm-platform-attestation",
                Label = "Enable TPM Platform Attestation for Device Health Verification",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "TPM Platform Attestation uses the TPM to cryptographically prove the device's boot state and security configuration to remote verification services. Enabling TPM platform attestation allows Microsoft Device Health Attestation Service or organizational attestation servers to verify device security posture before granting access to protected resources. Attestation provides verifiable evidence that Secure Boot was enabled and that no unauthorized components were present during the boot process. Conditional access policies that require device health attestation ensure that only compliant devices with verified boot states can access sensitive corporate resources. TPM attestation is the highest assurance form of device compliance verification compared to software-only compliance reporting. Organizations implementing Zero Trust architectures should require TPM attestation as part of the device identity verification process.",
                Tags = ["tpm", "attestation", "platform-health", "zero-trust", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTPMAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTPMAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTPMAttestation", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-configure-tpm-pcr-banks",
                Label = "Configure TPM PCR Bank Selection for Maximum Security Coverage",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "TPM Platform Configuration Registers (PCRs) store measurement values from the measured boot sequence that are used to validate the integrity of the boot process. Configuring which PCR banks are used for measurement ensures comprehensive coverage of boot components that could be modified for bootkit or rootkit attacks. PCR banks using SHA-256 provide stronger measurement integrity compared to SHA-1 banks and should be preferred for all deployments. The specific PCRs included in the TPM sealing policy for BitLocker determine which boot components are verified before releasing the encryption key. More extensive PCR coverage increases tamper detection but reduces flexibility for legitimate firmware and software changes that would trigger BitLocker recovery. Organizations should configure PCR banks to include UEFI firmware measurements driver loading and boot loader measurements for comprehensive boot integrity.",
                Tags = ["tpm", "pcr-banks", "measured-boot", "integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSHA256Bank", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSHA256Bank")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSHA256Bank", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-disable-tpm-clear-without-pin",
                Label = "Require Physical Presence PIN for TPM Clear Operations",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "TPM clear operations reset the TPM to factory state erasing all keys and credentials which is a destructive operation that should require physical presence verification. Requiring a physical presence PIN for TPM clear prevents remote attackers who gain administrator access from destroying TPM-protected credentials and BitLocker keys. Remote TPM clear could be used by an attacker to force BitLocker recovery on all systems simultaneously creating a denial of service across the organization. Physical presence requirements for TPM clear ensure that an authorized person must be at the physical console to approve the destructive operation. Organizations should document and protect the physical presence PIN used for TPM management and ensure it is stored securely through the PAM process. TPM clear operations should be logged and require change management approval before execution even when the physical presence requirement is satisfied.",
                Tags = ["tpm", "clear-protection", "physical-presence", "key-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockedCommandsList", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockedCommandsList")],
                DetectOps = [RegOp.CheckDword(Key, "BlockedCommandsList", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-restrict-tpm-commandlist",
                Label = "Restrict TPM Command List to Approved Operations Only",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "TPM command lists define which TPM commands can be sent to the hardware restricting the operations that software can request from the TPM. Restricting the TPM command list to approved operations prevents exploitation of obscure or rarely-used TPM commands that may have implementation vulnerabilities. TPM command filtering blocks commands that are not needed for normal Windows security operations reducing the attack surface for TPM firmware vulnerabilities. The Windows TPM command list policy defines both blocked and allowed command lists that are enforced by the TPM driver before forwarding to the hardware. Organizations should configure the minimum required TPM command set for their workload and test thoroughly before deploying restrictions. TPM command restriction policies should be reviewed after TPM firmware updates that may add or modify command handling.",
                Tags = ["tpm", "command-list", "attack-surface", "firmware", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UseAdvancedStartup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "UseAdvancedStartup")],
                DetectOps = [RegOp.CheckDword(Key, "UseAdvancedStartup", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-enable-tpm-srk-policy",
                Label = "Configure TPM Storage Root Key Policy for Key Protection",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "The TPM Storage Root Key (SRK) is the master key that protects all other keys created under the TPM's storage hierarchy and is critical to the security of all TPM-protected data. Configuring the SRK policy ensures that the storage hierarchy is initialized with the appropriate authorization requirements for organizational security needs. SRK policy configuration includes whether the SRK requires authorization for key creation and how hierarchy keys are sealed to PCR measurements. Proper SRK configuration is essential for TPM 1.2 devices where the SRK is established during TPM ownership which differs from TPM 2.0 storage hierarchy management. Organizations should verify SRK policy alignment with their BitLocker and Credential Guard deployment requirements. SRK policy management for TPM 1.2 differs significantly from TPM 2.0 storage hierarchy management and organizations should consult the appropriate hardware documentation.",
                Tags = ["tpm", "srk", "storage-root-key", "key-hierarchy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSrkPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSrkPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSrkPolicy", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-audit-tpm-operations",
                Label = "Enable Comprehensive Audit Logging for TPM Operations",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "TPM operation audit logging captures successful and failed TPM operations providing visibility into TPM usage patterns and potential exploitation attempts. Enabling comprehensive TPM audit logging creates a forensic record of BitLocker key operations credential creations and platform measurements. Anomalous TPM operation patterns such as repeated failed TPM authorization attempts may indicate a BitLocker brute-force attack or TPM exploitation attempt. TPM audit events are available in the System log under the Microsoft-Windows-Security-Tpm category and should be forwarded to SIEM. Organizations should baseline normal TPM operation patterns including BitLocker key release frequency and Windows Hello authentication to detect deviations. Correlation of TPM audit events with other security signals provides context for determining whether anomalous patterns represent attacks.",
                Tags = ["tpm", "audit", "monitoring", "key-operations", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditTPMOperations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditTPMOperations")],
                DetectOps = [RegOp.CheckDword(Key, "AuditTPMOperations", 1)],
            },
            new TweakDef
            {
                Id = "tpmadv-configure-tpm-firmware-update",
                Label = "Configure TPM Firmware Update Policy for Security Patches",
                Category = "Security — Service Account",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "TPM firmware vulnerabilities such as the Infineon TPM weak key generation vulnerability require firmware updates that can only be applied through a TPM firmware update process. Configuring TPM firmware update policy ensures that critical TPM security patches can be applied through the organizational update infrastructure. TPM firmware updates may require BitLocker recovery or TPM re-enrollment of Windows Hello credentials depending on how the update changes the TPM state. Organizations should test TPM firmware updates in a lab environment to understand the user impact before broad deployment. The TPM firmware update approval process should be tracked in the vulnerability management system to ensure timely remediation of critical TPM security issues. TPM firmware update events should be logged for compliance reporting on vulnerability remediation timelines.",
                Tags = ["tpm", "firmware-update", "vulnerability-management", "patching", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowFirmwareUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowFirmwareUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "AllowFirmwareUpdate", 1)],
            },
        ];
    }

    // ── TpmAttestationPolicy ──
    private static class _TpmAttestationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";
        private const string MbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceHealthAttestation";
        private const string HaKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HealthAttestation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tpmpol-enable-device-health-attestation",
                    Label = "Enable Device Health Attestation (DHA) Service",
                    Category = "Security — Service Account",
                    Description =
                        "Enables the Windows Device Health Attestation service which uses TPM measurements to produce a signed boot health certificate, allowing MDM policies to verify device security posture before granting access.",
                    Tags = ["tpm", "dha", "health-attestation", "mdm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Device Health Attestation enabled; TPM boot measurements sent to DHA service for MDM compliance checking.",
                    ApplyOps = [RegOp.SetDword(HaKey, "EnableDeviceHealthAttestationService", 1)],
                    RemoveOps = [RegOp.DeleteValue(HaKey, "EnableDeviceHealthAttestationService")],
                    DetectOps = [RegOp.CheckDword(HaKey, "EnableDeviceHealthAttestationService", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-set-lockout-threshold-10",
                    Label = "Set TPM Anti-Hammering Lockout Threshold to 10 Failures",
                    Category = "Security — Service Account",
                    Description =
                        "Configures the TPM anti-hammering lockout threshold to 10 failed authorisation attempts, after which the TPM enters lockout mode requiring administrative reset, protecting against brute-force TPM dictionary attacks.",
                    Tags = ["tpm", "lockout", "anti-hammering", "brute-force", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TPM lockout after 10 failed auth attempts; brute-force dictionary attacks against TPM mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "StandardUserAuthorizationFailureDuration_Name", 10)],
                    RemoveOps = [RegOp.DeleteValue(Key, "StandardUserAuthorizationFailureDuration_Name")],
                    DetectOps = [RegOp.CheckDword(Key, "StandardUserAuthorizationFailureDuration_Name", 10)],
                },
                new TweakDef
                {
                    Id = "tpmpol-block-tpm-clear-by-standard-user",
                    Label = "Block Standard Users from Clearing the TPM",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents standard (non-administrator) users from clearing the TPM chip, which would destroy all TPM-protected keys and could be used to defeat BitLocker and Credential Guard protections.",
                    Tags = ["tpm", "clear-tpm", "standard-user", "bitlocker", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "TPM clear blocked for standard users; only admins can clear TPM. Prevents BitLocker key destruction.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockStandardUserClearTPM", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockStandardUserClearTPM")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockStandardUserClearTPM", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-require-tpm2-minimum",
                    Label = "Require TPM 2.0 Minimum for Secure Device Operations",
                    Category = "Security — Service Account",
                    Description =
                        "Enforces that all security operations requiring TPM attestation (BitLocker, Credential Guard, Device Guard, Windows Hello) use TPM 2.0, blocking fallback to the weaker TPM 1.2 specification.",
                    Tags = ["tpm", "tpm-2.0", "version-requirement", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TPM 2.0 required minimum; security features that fall back to TPM 1.2 are blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "RequireTPM20", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "RequireTPM20")],
                    DetectOps = [RegOp.CheckDword(Key, "RequireTPM20", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-enable-measured-boot",
                    Label = "Enable Windows Measured Boot with TPM PCR Logging",
                    Category = "Security — Service Account",
                    Description =
                        "Enables Measured Boot on the Windows bootloader, ensuring that each boot component hash is recorded in TPM PCR registers, creating an immutable tamper-evident boot measurement log.",
                    Tags = ["tpm", "measured-boot", "pcr", "boot-security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Measured Boot with TPM PCR logging enabled; boot chain tamper evident and verifiable via attestation.",
                    ApplyOps = [RegOp.SetDword(MbKey, "EnableMeasuredBoot", 1)],
                    RemoveOps = [RegOp.DeleteValue(MbKey, "EnableMeasuredBoot")],
                    DetectOps = [RegOp.CheckDword(MbKey, "EnableMeasuredBoot", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-disable-tpm-auto-provisioning",
                    Label = "Disable Automatic TPM Provisioning by Windows",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents Windows from automatically taking ownership of the TPM during first boot provisioning, requiring explicit administrative TPM initialisation and ensuring TPM ownership is a deliberate IT action.",
                    Tags = ["tpm", "provisioning", "ownership", "admin", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Automatic TPM provisioning disabled; TPM ownership requires explicit admin initialisation.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoProvisioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProvisioning")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoProvisioning", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-log-tpm-events",
                    Label = "Log TPM Attestation and Lockout Events in Security Log",
                    Category = "Security — Service Account",
                    Description =
                        "Enables Security event log entries for TPM lockout events, attestation failures, and TPM provisioning changes, providing audit visibility into hardware security chip state changes.",
                    Tags = ["tpm", "event-log", "audit", "lockout", "attestation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "TPM lockout and attestation events logged in Security log; hardware security chip state visible for auditing.",
                    ApplyOps = [RegOp.SetDword(Key, "LogTPMEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogTPMEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogTPMEvents", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-disable-tpm-remote-management",
                    Label = "Disable Remote TPM Management via DCOM",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents remote administration of the TPM chip via the DCOM TPM management interface, ensuring all TPM configuration changes require local administrator access to the physical or virtual machine.",
                    Tags = ["tpm", "remote-management", "dcom", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Remote TPM management via DCOM disabled; TPM configuration requires local admin access only.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRemoteTPMManagement", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRemoteTPMManagement")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRemoteTPMManagement", 1)],
                },
                new TweakDef
                {
                    Id = "tpmpol-disable-tpm-telemetry",
                    Label = "Disable TPM Telemetry Reporting to Microsoft",
                    Category = "Security — Service Account",
                    Description =
                        "Prevents Windows from sending TPM chip model, firmware version, PCR configuration, and health attestation result telemetry to Microsoft.",
                    Tags = ["tpm", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "TPM telemetry to Microsoft disabled; chip model, firmware, and attestation data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTPMTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTPMTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTPMTelemetry", 1)],
                },
            ];
    }

    // ── TpmRecoveryPolicy ──
    private static class _TpmRecoveryPolicy
    {
        private const string BitLockerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker";

        private const string TpmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tpmrec-backup-tpm-owner-info-to-ad",
                    Label = "TPM Recovery: Backup TPM Owner Information to Active Directory",
                    Category = "Security — Service Account",
                    Description =
                        "Sets ActiveDirectoryBackupEnabled=1 in the TPM policy hive. Enables automatic backup of the TPM owner authorization value (TPM owner password hash) to Active Directory when the TPM is initialised or reset. The TPM owner password is needed for certain TPM management operations (clearing the TPM, resetting TPM lockout after dictionary attack). Without Active Directory backup, losing the owner password means the TPM cannot be cleared without a firmware-level reset, which can prevent BitLocker recovery in certain scenarios. Backing up to AD ensures the TPM owner information is recoverable by enterprise admins.",
                    Tags = ["tpm", "backup", "active-directory", "owner-password", "recovery"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM owner information backed up to Active Directory. Requires AD schema extension for TPM backup (ms-TPM-OwnerInformation attribute — part of Windows Server 2012 R2 AD schema). Backup occurs when TPM is first initialised or when ownership is taken. Verify the DC has the ms-TPM-OwnerInformation attribute in schema before enforcing.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "ActiveDirectoryBackupEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "ActiveDirectoryBackupEnabled")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "ActiveDirectoryBackupEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "tpmrec-block-tpm-clear-by-non-admin",
                    Label = "TPM Recovery: Block TPM Clear Operation by Non-Administrator Users",
                    Category = "Security — Service Account",
                    Description =
                        "Sets BlockTPMClear=1 in the TPM policy hive. Prevents non-administrator users from clearing the TPM. Clearing the TPM destroys all TPM-protected keys — all BitLocker encryption keys bound to the TPM, Windows Hello for Business keys, and any application TPM keys. A non-admin user who can clear the TPM on a shared workstation can force a BitLocker recovery event (requiring the recovery key) and potentially create confusion that could be exploited during the recovery process. Restricting TPM clear operations to administrators ensures only authorised personnel can perform this destructive operation.",
                    Tags = ["tpm", "clear-prevention", "admin-only", "bitlocker", "key-destruction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM clear blocked for non-administrators. Administrators can still clear the TPM using TPM.msc or PowerShell (Get-Tpm | Clear-Tpm). Standard users clicking 'Clear TPM' in the Security Center get an access denied error. Reduces risk of accidental or malicious TPM clear on shared workstations.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "BlockTPMClear", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "BlockTPMClear")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "BlockTPMClear", 1)],
                },
                new TweakDef
                {
                    Id = "tpmrec-enable-tpm-auto-provisioning",
                    Label = "TPM Recovery: Enable Automatic TPM Provisioning by Windows",
                    Category = "Security — Service Account",
                    Description =
                        "Sets EnableAutoTPMProvisioning=1 in the TPM policy hive. Enables automatic TPM provisioning by Windows during first use. When a device ships with an unprovisioned TPM (factory default), Windows can automatically provision the TPM, set the owner password, and back up the owner info to AD. Without auto-provisioning, administrators must manually provision each device's TPM before BitLocker can be deployed. Auto-provisioning ensures that all devices in the enterprise have their TPMs properly initialised during the Windows Setup or subsequent first-login process, enabling enterprise-wide BitLocker deployment without per-device manual TPM steps.",
                    Tags = ["tpm", "provisioning", "auto", "bitlocker-readiness", "deployment"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM auto-provisioning enabled. On first login, Windows provisions the TPM and backs up owner info to AD. No user-visible impact. TPM provisioning occurs in the background. If the device cannot contact AD (e.g., not domain-joined yet), provisioning is deferred until next contact.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "EnableAutoTPMProvisioning", 1)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "EnableAutoTPMProvisioning")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "EnableAutoTPMProvisioning", 1)],
                },
                new TweakDef
                {
                    Id = "tpmrec-set-tpm-lockout-duration-30min",
                    Label = "TPM Recovery: Set TPM Dictionary Attack Lockout Duration to 30 Minutes",
                    Category = "Security — Service Account",
                    Description =
                        "Sets DictionaryAttackLockoutDuration=30 in the TPM policy hive (units: minutes). Sets the TPM dictionary attack lockout duration to 30 minutes. The TPM tracks repeated failed authorisation attempts (dictionary attack mitigation). After exceeding the threshold, the TPM enters a lockout mode and refuses further authorisation attempts. The lockout duration determines how long the TPM remains locked before resetting its counter. A 30-minute lockout provides strong protection against automated PIN/password brute-force attacks against BitLocker TPM+PIN while being reasonable for legitimate pin-entry mistakes.",
                    Tags = ["tpm", "dictionary-attack", "lockout", "bitlocker", "brute-force"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM dictionary attack lockout is 30 minutes. After threshold failed PIN entries, TPM locks out for 30 minutes. Users who repeatedly enter wrong BitLocker PINs will be locked out for 30 minutes. The lockout counter resets after a full 30-minute wait. Adjust the lockout threshold (DictionaryAttackLockoutThreshold) and duration to match your user population's PIN error rate.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "DictionaryAttackLockoutDuration", 30)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "DictionaryAttackLockoutDuration")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "DictionaryAttackLockoutDuration", 30)],
                },
                new TweakDef
                {
                    Id = "tpmrec-set-tpm-lockout-threshold-5",
                    Label = "TPM Recovery: Set TPM Dictionary Attack Lockout Threshold to 5 Attempts",
                    Category = "Security — Service Account",
                    Description =
                        "Sets DictionaryAttackLockoutThreshold=5 in the TPM policy hive. Sets the number of failed TPM authorisation attempts before the TPM enters lockout mode to 5. Five attempts is consistent with enterprise account lockout policies (typically 5–10 attempts) — it provides a reasonable number of legitimate re-entry attempts while blocking automated brute-force attacks (which attempt thousands of PINs per minute). Combined with the 30-minute lockout duration, this means an attacker can test at most 5 PINs every 30 minutes — making a full 6-digit PIN space (1,000,000 values) take over 100,000 hours to exhaust.",
                    Tags = ["tpm", "lockout-threshold", "brute-force", "bitlocker", "dictionary-attack"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TPM locks after 5 failed PIN authorisations. Users who enter the wrong BitLocker PIN more than 5 times trigger a 30-minute lockout. Helpdesk should be prepared for lockout-related support calls. The TPM lockout counter resets after the lockout duration expires or after a successful authorisation.",
                    ApplyOps = [RegOp.SetDword(TpmKey, "DictionaryAttackLockoutThreshold", 5)],
                    RemoveOps = [RegOp.DeleteValue(TpmKey, "DictionaryAttackLockoutThreshold")],
                    DetectOps = [RegOp.CheckDword(TpmKey, "DictionaryAttackLockoutThreshold", 5)],
                },
                new TweakDef
                {
                    Id = "tpmrec-backup-bitlocker-recovery-key-to-ad",
                    Label = "TPM Recovery: Require BitLocker Recovery Key Backup to Active Directory/AAD",
                    Category = "Security — Service Account",
                    Description =
                        "Sets RequireDeviceLockout=1 in the BitLocker policy hive (OSRecoveryInformationBackup flag). Requires that BitLocker recovery keys be backed up to Active Directory or Azure AD before BitLocker encryption can be enabled. Without this requirement, users or automated deployment systems can enable BitLocker and generate a recovery key that is never stored in the enterprise directory — resulting in encrypted devices with no enterprise-retrievable recovery key. If the device then undergoes a TPM change, firmware update, or Secure Boot configuration change, recovery requires that local key which may be lost.",
                    Tags = ["bitlocker", "recovery-key", "ad-backup", "aad-backup", "encryption"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "BitLocker recovery key backup to AD or AAD is required before encryption activation. BitLocker setup will not complete until the backup succeeds. Device must be domain-joined or AAD-joined and reachable. Ensures all enterprise devices have retrievable recovery keys in the directory. Required for self-service BitLocker recovery capabilities.",
                    ApplyOps =
                    [
                        RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout", 1),
                    ],
                    RemoveOps =
                    [
                        RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout"),
                    ],
                    DetectOps =
                    [
                        RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "RequireDeviceLockout", 1),
                    ],
                },
                new TweakDef
                {
                    Id = "tpmrec-enable-bitlocker-preboot-pin",
                    Label = "TPM Recovery: Require BitLocker Pre-Boot PIN for OS Drive",
                    Category = "Security — Service Account",
                    Description =
                        "Sets EnableBDEWithNoTPM=0 and TPM-based protection with PIN by setting UseACBitLockerPIN=1 in the BitLocker policy. Requires that the BitLocker OS drive uses TPM+PIN authentication, not TPM-only. TPM-only BitLocker can be bypassed by a cold-boot attack (freezing RAM to preserve encryption keys) or by DMA attacks against the boot process. Requiring a PIN in addition to the TPM ensures that even if hardware-level memory extraction is performed, the attacker must also know the PIN. The PIN is never transmitted over the network and is not stored in AD — it is the 'something you know' factor in the BitLocker two-factor authentication.",
                    Tags = ["bitlocker", "pre-boot-pin", "tpm-plus-pin", "cold-boot", "dma-attack"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "BitLocker requires a PIN every time the device boots. Users must remember and enter the BitLocker PIN at each startup. This is an additional step that prevents fast boot in enterprise thin-client and kiosk deployments. For devices in remote locations or used by users who frequently forget PINs, consider Network Unlock as an alternative.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\BitLocker\OSRecovery", "UseTPMPIN", 1)],
                },
                new TweakDef
                {
                    Id = "tpmrec-enable-enhanced-pin",
                    Label = "TPM Recovery: Enable Enhanced BitLocker PIN (Allows Full Keyboard Chars)",
                    Category = "Security — Service Account",
                    Description =
                        "Sets UseEnhancedPin=1 in the BitLocker policy hive. Enables Enhanced PINs for BitLocker pre-boot authentication on supported firmware. By default, BitLocker PINs only accept numeric digits (0–9) in pre-boot. Enhanced PINs allow letters, symbols, and spaces — enabling passphrases and mixed PINs that are significantly harder to brute-force. A 6-digit numeric PIN has 1,000,000 combinations; an 8-character alphanumeric+symbol passphrase has over 6 quadrillion combinations. Enabling Enhanced PINs dramatically increases the effective entropy of BitLocker pre-boot authentication without changing the TPM+PIN hardware requirement.",
                    Tags = ["bitlocker", "enhanced-pin", "passphrase", "entropy", "pre-boot"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Enhanced PIN (alphanumeric + symbols) enabled for BitLocker. Users can use a full passphrase instead of a numeric-only PIN. International keyboard layouts should be tested — some special characters may not be available in UEFI pre-boot environments. Existing numeric-only PINs are automatically migrated to Enhanced PIN format on next PIN change.",
                    ApplyOps = [RegOp.SetDword(BitLockerKey, "UseEnhancedPin", 1)],
                    RemoveOps = [RegOp.DeleteValue(BitLockerKey, "UseEnhancedPin")],
                    DetectOps = [RegOp.CheckDword(BitLockerKey, "UseEnhancedPin", 1)],
                },
            ];
    }

    // ── TpmSecurityPolicy ──
    private static class _TpmSecurityPolicy
    {
        private const string Tpm = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TPM";
        private const string TpmDg = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\DeviceGuard";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tpmgpo-require-active-directory-backup",
                Label = "Require TPM Owner Info Backup to Active Directory",
                Category = "Security — Tpm Security",
                Description =
                    "Requires TPM owner authorization information to be backed up to Active Directory before TPM operations are allowed. Prevents TPM ownership from being set on machines where AD backup fails, ensuring recoverability. Default: 0. Recommended: 1 for AD-joined enterprise machines.",
                Tags = ["tpm", "active-directory", "backup", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Tpm],
                ApplyOps = [RegOp.SetDword(Tpm, "RequireActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(Tpm, "RequireActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(Tpm, "RequireActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "tpmgpo-enable-active-directory-backup",
                Label = "Enable TPM Owner Info Active Directory Backup",
                Category = "Security — Tpm Security",
                Description =
                    "Enables automatic backup of TPM owner authorization to Active Directory. When combined with RequireActiveDirectoryBackup, ensures all TPM-protected machines have recoverable owner keys in AD. Default: 0. Recommended: 1.",
                Tags = ["tpm", "active-directory", "backup", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Tpm],
                ApplyOps = [RegOp.SetDword(Tpm, "ActiveDirectoryBackup", 1)],
                RemoveOps = [RegOp.DeleteValue(Tpm, "ActiveDirectoryBackup")],
                DetectOps = [RegOp.CheckDword(Tpm, "ActiveDirectoryBackup", 1)],
            },
            new TweakDef
            {
                Id = "tpmgpo-standard-user-lockout-threshold",
                Label = "Set TPM Standard-User Authorization Failure Threshold",
                Category = "Security — Tpm Security",
                Description =
                    "Sets the TPM lockout threshold for standard users to 32 failed authorization attempts before the TPM enters lockout mode. Balances brute-force protection with usability. Default: 32. Recommended: 9 for stricter environments.",
                Tags = ["tpm", "lockout", "brute-force", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Tpm],
                ApplyOps = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureTotalThreshold", 9)],
                RemoveOps = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureTotalThreshold")],
                DetectOps = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureTotalThreshold", 9)],
            },
            new TweakDef
            {
                Id = "tpmgpo-standard-user-lockout-duration",
                Label = "Set TPM Standard-User Lockout Duration to 1 Hour",
                Category = "Security — Tpm Security",
                Description =
                    "Sets the TPM lockout observation window to 3 600 seconds (1 hour). Failed authorization attempts within this window count toward the lockout threshold. After the window expires, failed counts reset. Default: 7200. Recommended: 3600.",
                Tags = ["tpm", "lockout", "duration", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Tpm],
                ApplyOps = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureDuration", 3600)],
                RemoveOps = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureDuration")],
                DetectOps = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureDuration", 3600)],
            },
            new TweakDef
            {
                Id = "tpmgpo-standard-user-individual-lockout",
                Label = "Set TPM Standard-User Individual Auth Failure Threshold",
                Category = "Security — Tpm Security",
                Description =
                    "Sets the TPM individual authorization failure threshold for standard users to 4. A single TPM authorization can fail at most 4 times within the observation window before triggering lockout for that key. Default: 4. Recommended: 4.",
                Tags = ["tpm", "lockout", "individual", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Tpm],
                ApplyOps = [RegOp.SetDword(Tpm, "StandardUserAuthorizationFailureIndividualThreshold", 4)],
                RemoveOps = [RegOp.DeleteValue(Tpm, "StandardUserAuthorizationFailureIndividualThreshold")],
                DetectOps = [RegOp.CheckDword(Tpm, "StandardUserAuthorizationFailureIndividualThreshold", 4)],
            },
        ];
    }

    // ── TrustProviderPolicy ──
    private static class _TrustProviderPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TrustProvider";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "trustprov-require-trust-chain",
                Label = "Require Complete Certificate Trust Chain",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Complete certificate trust chain validation ensures that every certificate in a chain traces back to a trusted root CA without gaps. Requiring complete trust chains prevents acceptance of certificates with missing intermediate CAs or broken trust paths. Incomplete certificate chains are a common misconfiguration that can allow man-in-the-middle attacks when clients accept partial chains. Windows Trust Provider APIs are used by Authenticode, WinHTTP, Schannel, and other Windows security components for certificate validation. Trust chain requirements help ensure that all security components validate certificates consistently through a configurable policy. Enterprise deployments should ensure their internal PKI certificates deploy complete chains to avoid legitimate certificate acceptance failures.",
                Tags = ["trust", "certificates", "pki", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireCompleteTrustChain", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireCompleteTrustChain")],
                DetectOps = [RegOp.CheckDword(Key, "RequireCompleteTrustChain", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-enable-revocation-check",
                Label = "Enable Certificate Revocation Checking",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Certificate revocation checking validates that a certificate has not been revoked by the CA before trusting it for signature verification. Enabling revocation checking through OCSP or CRL prevents accepting certificates that have been revoked due to compromise or policy violation. Revocation checking is critical for catching certificates that were stolen or issued to unauthorized parties after the CA discovered the issue. Windows Trust Provider supports both Online Certificate Status Protocol and Certificate Revocation List distribution point revocation checks. Revocation checking requires network connectivity to CRL distribution points or OCSP responders which should be accessible from enterprise endpoints. Failing to check revocation allows system compromise even after a certificate has been reported as revoked.",
                Tags = ["trust", "revocation", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRevocationChecking", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRevocationChecking")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRevocationChecking", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-block-expired-certificates",
                Label = "Block Expired Authenticode Certificates",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Expired Authenticode certificates should not be trusted for code signing as the CA can no longer guarantee the integrity of the certificate holder. Blocking expired certificates prevents software signed with certificates past their validity period from being accepted without timestamp countersignatures. Windows by default accepts PE files signed with expired certificates if signed before expiry and timestamped but policy can enforce stricter requirements. Many malware families use expired or revoked certificates to avoid detection while appearing to be legitimately signed. Expired certificate blocking forces vendors to keep certificates current and reduces the window of malicious exploitation of certificate compromise. Organizations should inventory software signed with near-expiry certificates before enabling expired certificate blocking to prevent deployment disruption.",
                Tags = ["trust", "expired", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockExpiredCertificates", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockExpiredCertificates")],
                DetectOps = [RegOp.CheckDword(Key, "BlockExpiredCertificates", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-require-timestamping",
                Label = "Require Timestamp Countersignature for Code Signing",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Timestamp countersignatures provide a trusted time binding for code signatures ensuring they remain valid even after the signing certificate expires. Requiring timestamping prevents distribution of signed code without a verifiable trust anchor to authentication events before certificate expiry. Authenticode timestamps from trusted TSAs embed the signing time in the signature and allow validation of signatures made before certificate revocation. Code signing without timestamps becomes invalid when the signing certificate expires which can trigger false security alerts on legitimate software. RFC 3161 timestamp authorities from trusted providers should be used and are required for Extended Validation code signing compliance. Timestamp requirements reduce the practical window for attacks using stolen certificates by limiting their usefulness to the TSA-recorded window.",
                Tags = ["trust", "timestamp", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireTimestampCountersig", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireTimestampCountersig")],
                DetectOps = [RegOp.CheckDword(Key, "RequireTimestampCountersig", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-restrict-trust-to-enterprise-ca",
                Label = "Restrict Code Trust to Enterprise CA",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Restricting code trust to enterprise CAs limits which certificate authorities can issue certificates accepted for code signing on managed endpoints. Enterprise CA restriction ensures that internally deployed software must be signed by the enterprise PKI rather than arbitrary commercial CAs. This policy supports zero-trust models for application execution where only IT-managed code signing authorities are recognized. Restricting to enterprise CAs prevents attackers who obtain certificates from public CAs from deploying signed malware on restricted endpoints. PKI pinning through enterprise certificate stores is the technical mechanism for implementing enterprise CA code signing restrictions. Enterprise CA restriction may require re-signing vendor software with enterprise certificates which adds PKI management overhead.",
                Tags = ["trust", "enterprise-ca", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictTrustToEnterpriseCA", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictTrustToEnterpriseCA")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictTrustToEnterpriseCA", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-block-md5-signatures",
                Label = "Block MD5-Based Certificate Signatures",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "MD5-based certificate signatures are cryptographically broken and should not be trusted for any security-sensitive operation. Blocking MD5 signatures prevents certificates signed with the MD5 hash algorithm from being accepted by Windows Trust Provider. MD5 collisions are computationally feasible and have been demonstrated in attacks against certificate authorities that resulted in fraudulent CA certificates. Windows deprecated MD5 in certificate chains in 2009 but policy enforcement ensures no legacy applications re-enable MD5 trust. Any certificates in the enterprise environment still using MD5 should be identified and replaced immediately. MD5 blocking is defense-in-depth ensuring that even if a deprecated component attempts to accept MD5 certificates trust provider policy prevents acceptance.",
                Tags = ["trust", "md5", "certificates", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockMD5Signatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMD5Signatures")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMD5Signatures", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-block-sha1-code-signing",
                Label = "Block SHA-1 Authenticode Code Signatures",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "SHA-1 is cryptographically weak and has demonstrated practical collision vulnerabilities that can be exploited to forge signatures. Blocking SHA-1 Authenticode signatures ensures that all code signing certificates use SHA-256 or stronger algorithms. Microsoft announced deprecation of SHA-1 certificate chain validation in Authenticode starting in January 2016 but policy enforcement provides explicit control. Legacy software signed with SHA-1 certificates should be re-signed with SHA-256 before this policy is enforced. SHA-1 blocking for Authenticode is separate from SHA-1 blocking for TLS certificates and must be configured specifically for code signing trust. Enterprise environments with legacy signed software must audit and replace SHA-1 signed executables before enabling blocking to prevent application disruption.",
                Tags = ["trust", "sha1", "authenticode", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockSHA1CodeSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockSHA1CodeSigning")],
                DetectOps = [RegOp.CheckDword(Key, "BlockSHA1CodeSigning", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-audit-revocation-failures",
                Label = "Audit Certificate Revocation Check Failures",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Certificate revocation check failures occur when revocation status cannot be determined due to network issues or unavailable CRL distribution points. Auditing revocation failures provides visibility into potential certificate validation problems that could indicate network blocking or misconfiguration. Persistent revocation check failures may indicate that CRL distribution point URLs are inaccessible from the endpoint or OCSP responders are down. Logging revocation failures to the security event log enables SIEM correlation to identify compromised endpoints blocking revocation checks. An attacker who gains network-level access might attempt to block revocation checking to maintain access via revoked certificates. Revocation failure auditing should be combined with a policy decision about whether to fail-open or fail-closed when revocation checking is unavailable.",
                Tags = ["trust", "revocation", "audit", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditRevocationFailures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRevocationFailures")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRevocationFailures", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-enable-ev-code-signing",
                Label = "Prefer Extended Validation Code Signing Certificates",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Extended Validation code signing certificates require rigorous identity verification by the CA providing higher assurance than standard Organization Validation certificates. Preferring EV code signing provides higher trust signals in SmartScreen and Windows security UIs for EV-signed software. EV certificates immediately unlock SmartScreen reputation whereas OV certificates require building reputation over time through downloads. Malware operators rarely obtain EV certificates due to the identity verification requirements making EV an effective higher-trust signal. Enterprise software distribution should prefer EV signing for critical infrastructure components and software distributed externally. EV certificate preference policy provides behavioral guidance to users and security tooling that can differentiate between EV and non-EV signatures.",
                Tags = ["trust", "ev-certificate", "authenticode", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreferEVCodeSigning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreferEVCodeSigning")],
                DetectOps = [RegOp.CheckDword(Key, "PreferEVCodeSigning", 1)],
            },
            new TweakDef
            {
                Id = "trustprov-log-trust-decisions",
                Label = "Enable Trust Decision Audit Logging",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Trust decision audit logging records all certificate verification decisions made by Windows Trust Provider components during normal system operation. Enabling trust decision logging provides a comprehensive audit trail of all code signing verifications, TLS validations, and certificate trust decisions. Trust decision logs help identify attempts to run untrusted software, access systems with invalid certificates, and trust policy violations. Security teams can use trust decision events to detect lateral movement techniques that involve running unsigned tools across the network. Trust logging events can be correlated with other security events to understand the full scope of an intrusion or attack campaign. Trust decision audit logs should be collected and retained according to enterprise log retention policies for post-incident forensic analysis.",
                Tags = ["trust", "audit", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableTrustDecisionLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTrustDecisionLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTrustDecisionLogging", 1)],
            },
        ];
    }

    // ── UserAccountControlAdvPolicy ──
    private static class _UserAccountControlAdvPolicy
    {
        private const string UacAdv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        private const string Winlogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "uacadv-disable-arso",
                Label = "Logon: Disable automatic restart sign-on (ARSO)",
                Category = "Security — Tpm Security",
                Description =
                    "Sets DisableAutomaticRestartSignOn=1 in Policies\\System. Prevents Windows from "
                    + "automatically signing in and locking the last interactive user after a reboot "
                    + "(e.g., triggered by a Windows Update restart).",
                Tags = ["uac", "logon", "arso", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "DisableAutomaticRestartSignOn", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableAutomaticRestartSignOn")],
                DetectOps = [RegOp.CheckDword(UacAdv, "DisableAutomaticRestartSignOn", 1)],
            },
            new TweakDef
            {
                Id = "uacadv-hide-network-selection-ui",
                Label = "Logon: Hide the network selection UI on the sign-in screen",
                Category = "Security — Tpm Security",
                Description =
                    "Sets DontDisplayNetworkSelectionUI=1 in Policies\\System. Removes the Wi-Fi/network "
                    + "chooser button from the Windows logon screen, preventing unauthenticated network changes.",
                Tags = ["uac", "logon", "network", "lock-screen", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "DontDisplayNetworkSelectionUI", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "DontDisplayNetworkSelectionUI")],
                DetectOps = [RegOp.CheckDword(UacAdv, "DontDisplayNetworkSelectionUI", 1)],
            },
            new TweakDef
            {
                Id = "uacadv-hide-failed-unlock-text",
                Label = "Logon: Hide failed-unlock notification text on lock screen",
                Category = "Security — Tpm Security",
                Description =
                    "Sets DontDisplayFailedUnlock=1 in Policies\\System. Suppresses the 'Your account has "
                    + "been locked' / 'too many attempts' banner shown on the lock screen after failed logins.",
                Tags = ["uac", "logon", "lock-screen", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "DontDisplayFailedUnlock", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "DontDisplayFailedUnlock")],
                DetectOps = [RegOp.CheckDword(UacAdv, "DontDisplayFailedUnlock", 1)],
            },
            new TweakDef
            {
                Id = "uacadv-require-msa-optional",
                Label = "Logon: Make Microsoft Account sign-in optional (allow local accounts)",
                Category = "Security — Tpm Security",
                Description =
                    "Sets MSAOptional=1 in Policies\\System. Allows users on apps and services to proceed "
                    + "without a Microsoft Account when the service offers a local-account alternative.",
                Tags = ["uac", "msa", "microsoft-account", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "MSAOptional", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "MSAOptional")],
                DetectOps = [RegOp.CheckDword(UacAdv, "MSAOptional", 1)],
            },
            new TweakDef
            {
                Id = "uacadv-disable-lock-workstation",
                Label = "Logon: Prevent users from locking the workstation via keyboard shortcut",
                Category = "Security — Tpm Security",
                Description =
                    "Sets DisableLockWorkstation=1 in Policies\\System. Disables the Win+L and "
                    + "Ctrl+Alt+Del > Lock option, preventing interactive users from manually locking the PC.",
                Tags = ["uac", "logon", "lock-workstation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "DisableLockWorkstation", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableLockWorkstation")],
                DetectOps = [RegOp.CheckDword(UacAdv, "DisableLockWorkstation", 1)],
            },
            new TweakDef
            {
                Id = "uacadv-disable-change-password",
                Label = "Logon: Prevent users from changing their password via Ctrl+Alt+Del",
                Category = "Security — Tpm Security",
                Description =
                    "Sets DisableChangePassword=1 in Policies\\System. Removes the 'Change password' "
                    + "option from the Ctrl+Alt+Del security screen (useful in kiosk/shared-PC scenarios).",
                Tags = ["uac", "logon", "password", "kiosk", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(UacAdv, "DisableChangePassword", 1)],
                RemoveOps = [RegOp.DeleteValue(UacAdv, "DisableChangePassword")],
                DetectOps = [RegOp.CheckDword(UacAdv, "DisableChangePassword", 1)],
            },
        ];
    }

    // ── UserProfilePolicy ──
    private static class _UserProfilePolicy
    {
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "upprof-restrict-cmd-for-users",
                    Label = "Restrict Command Prompt Access for Standard Users",
                    Category = "Security — Tpm Security",
                    Description = "Prevents standard (non-admin) users from launching Command Prompt (cmd.exe) directly.",
                    Tags = ["cmd", "command-prompt", "restriction", "users", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Value 2 = disable cmd for users but allow cmd scripts; value 1 = disable both. Admins are unaffected.",
                    ApplyOps = [RegOp.SetDword(SysKey, "DisableCMD", 2)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "DisableCMD")],
                    DetectOps = [RegOp.CheckDword(SysKey, "DisableCMD", 2)],
                },
                new TweakDef
                {
                    Id = "upprof-enable-gp-refresh",
                    Label = "Enable Background Group Policy Refresh",
                    Category = "Security — Tpm Security",
                    Description = "Ensures Group Policy is refreshed in the background on a schedule, even when the user is logged in.",
                    Tags = ["group-policy", "gpo-refresh", "background", "user-profile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Value 0 = allow background GP refresh (default-on but explicitly set via policy for enforcement).",
                    ApplyOps = [RegOp.SetDword(SysKey, "DisableBkGndGroupPolicy", 0)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "DisableBkGndGroupPolicy")],
                    DetectOps = [RegOp.CheckDword(SysKey, "DisableBkGndGroupPolicy", 0)],
                },
                new TweakDef
                {
                    Id = "upprof-block-roaming-profile-changes",
                    Label = "Prevent Local Changes from Syncing Back to Roaming Profile",
                    Category = "Security — Tpm Security",
                    Description = "Prevents any changes made to a roaming user profile during a session from syncing back to the network share.",
                    Tags = ["roaming-profile", "user-profile", "sync", "restriction"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Discards local profile changes at logoff; changes made in the session are lost unless saved explicitly.",
                    ApplyOps = [RegOp.SetDword(SysKey, "SlowLinkDefaultForDirectAccess", 0)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "SlowLinkDefaultForDirectAccess")],
                    DetectOps = [RegOp.CheckDword(SysKey, "SlowLinkDefaultForDirectAccess", 0)],
                },
                new TweakDef
                {
                    Id = "upprof-verbose-logon-status",
                    Label = "Disable Verbose Logon Status Messages",
                    Category = "Security — Tpm Security",
                    Description = "Suppresses the verbose 'Please wait...' and 'Applying computer settings...' status messages during logon.",
                    Tags = ["logon", "status-messages", "ui", "boot", "user-profile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes verbose status text at logon/logoff; cleaner experience in enterprise and kiosk deployments.",
                    ApplyOps = [RegOp.SetDword(SysKey, "VerboseStatus", 0)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "VerboseStatus")],
                    DetectOps = [RegOp.CheckDword(SysKey, "VerboseStatus", 0)],
                },
                new TweakDef
                {
                    Id = "upprof-block-profile-list-enumeration",
                    Label = "Block Switching to Another User's Profile",
                    Category = "Security — Tpm Security",
                    Description =
                        "Restricts the ability to download or load another user's roaming profile on this machine during interactive logon.",
                    Tags = ["user-profile", "roaming-profile", "multi-user", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Prevents profile hijacking and cross-user profile leakage on shared workstations.",
                    ApplyOps = [RegOp.SetDword(SysKey, "LocalProfile", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "LocalProfile")],
                    DetectOps = [RegOp.CheckDword(SysKey, "LocalProfile", 1)],
                },
                new TweakDef
                {
                    Id = "upprof-enable-logon-scripts-for-admins",
                    Label = "Run Logon Scripts for Administrator Accounts",
                    Category = "Security — Tpm Security",
                    Description = "Ensures Group Policy logon scripts execute even when an administrator account is used to log on.",
                    Tags = ["logon-scripts", "admin", "gpo", "user-profile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "By default, some admin configs skip scripts; this enforces scripts run for admins too.",
                    ApplyOps = [RegOp.SetDword(SysKey, "UserPoliciesAreMachinePolicies", 0)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "UserPoliciesAreMachinePolicies")],
                    DetectOps = [RegOp.CheckDword(SysKey, "UserPoliciesAreMachinePolicies", 0)],
                },
                new TweakDef
                {
                    Id = "upprof-clear-recent-docs-on-logoff",
                    Label = "Clear Recent Document Lists on Logoff",
                    Category = "Security — Tpm Security",
                    Description = "Deletes the list of recently accessed documents and applications from the user profile when they log off.",
                    Tags = ["recent-docs", "logoff", "privacy", "user-profile"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Recent file list cleared at each logoff; improves privacy on shared workstations and kiosk machines.",
                    ApplyOps = [RegOp.SetDword(SysKey, "ClearRecentDocsOnExit", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "ClearRecentDocsOnExit")],
                    DetectOps = [RegOp.CheckDword(SysKey, "ClearRecentDocsOnExit", 1)],
                },
            ];
    }

    // ── UserProfilesPolicy ──
    private static class _UserProfilesPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "uprof-disable-slow-link-detection",
                Label = "Disable Slow Network Link Detection for User Profiles",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets SlowLinkDetect=0 in the System policy key. Prevents Windows from "
                    + "detecting a slow network link and switching to local profile mode. Avoids "
                    + "unexpected profile behaviour on VPN or high-latency connections. Default: 1 "
                    + "(detection enabled). Recommended: 0 when local profiles are always preferred.",
                Tags = ["user-profiles", "slow-link", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SlowLinkDetect", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "SlowLinkDetect")],
                DetectOps = [RegOp.CheckDword(Key, "SlowLinkDetect", 0)],
            },
            new TweakDef
            {
                Id = "uprof-delete-cached-copies",
                Label = "Delete Cached Copies of Roaming Profiles at Logoff",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets DeleteRoamingCache=1 in the System policy key. Removes the locally "
                    + "cached copy of each roaming profile when the user logs off. Prevents profile "
                    + "data accumulation on shared/kiosk machines and ensures each logon fetches a "
                    + "fresh copy from the server. Default: 0. Recommended: 1 on multi-user machines.",
                Tags = ["user-profiles", "roaming", "cache", "policy", "cleanup"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DeleteRoamingCache", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DeleteRoamingCache")],
                DetectOps = [RegOp.CheckDword(Key, "DeleteRoamingCache", 1)],
            },
            new TweakDef
            {
                Id = "uprof-prevent-profile-size-limit",
                Label = "Disable User Profile Size Limit Warning Dialog",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets ProfileDlgTimeOut=0 in the System policy key. Sets the timeout for the "
                    + "profile size warning dialog to zero, preventing it from appearing. Removes a "
                    + "source of user interruption on managed machines where profile disk quotas are "
                    + "enforced by other means. Default: 15 seconds. Recommended: 0 on managed desktops.",
                Tags = ["user-profiles", "dialog", "quota", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ProfileDlgTimeOut", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ProfileDlgTimeOut")],
                DetectOps = [RegOp.CheckDword(Key, "ProfileDlgTimeOut", 0)],
            },
            new TweakDef
            {
                Id = "uprof-wait-on-logoff",
                Label = "Wait for Remote Profile Upload at Logoff",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets WaitForNetwork=0 in the System policy key. Prevents Windows from waiting "
                    + "for a network connection to upload a roaming profile on logoff. Speeds up "
                    + "logoff on machines with intermittent or no network connectivity. Default: 1. "
                    + "Recommended: 0 when roaming profiles are not used.",
                Tags = ["user-profiles", "logoff", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "WaitForNetwork", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "WaitForNetwork")],
                DetectOps = [RegOp.CheckDword(Key, "WaitForNetwork", 0)],
            },
            new TweakDef
            {
                Id = "uprof-disable-profile-error-notify",
                Label = "Disable User Profile Load Error Notification",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoProfileErrorNotification=1 in the System policy key. Suppresses the "
                    + "desktop notification shown when a user profile fails to load and a temporary "
                    + "profile is created instead. Prevents confusing pop-ups on kiosk machines where "
                    + "temporary profiles are expected. Default: 0 (notification shown). "
                    + "Recommended: 1 for kiosk/shared machines.",
                Tags = ["user-profiles", "error", "notification", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoProfileErrorNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoProfileErrorNotification")],
                DetectOps = [RegOp.CheckDword(Key, "NoProfileErrorNotification", 1)],
            },
            new TweakDef
            {
                Id = "uprof-disable-guest-logon",
                Label = "Disable Guest Account Profile Creation",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoGuestAccount=1 in the System policy key. Prevents the built-in Guest "
                    + "account from creating a user profile on the machine. Closes an attack surface "
                    + "where temporary guest sessions accumulate profile data or are used for lateral "
                    + "movement. Default: 0. Recommended: 1 on domain-joined and managed devices.",
                Tags = ["user-profiles", "guest", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoGuestAccount", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoGuestAccount")],
                DetectOps = [RegOp.CheckDword(Key, "NoGuestAccount", 1)],
            },
            new TweakDef
            {
                Id = "uprof-apply-gpo-at-logon",
                Label = "Force Synchronous Group Policy Processing at Logon",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets EnableSlowLinkUI=0 in the System policy key. Disables the slow-link UI "
                    + "that defers Group Policy processing to the background, ensuring all GPOs are "
                    + "fully applied before the desktop appears. Guarantees policies are in effect "
                    + "from the first moment of user access. Default: 1. Recommended: 0 on secure "
                    + "managed machines.",
                Tags = ["user-profiles", "group-policy", "gpo", "logon", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSlowLinkUI", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSlowLinkUI")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSlowLinkUI", 0)],
            },
            new TweakDef
            {
                Id = "uprof-disable-user-tracking",
                Label = "Disable User Profile Tracking for Shell Namespace",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoUserFolderRedirection=1 in the System policy key. Prevents the shell "
                    + "from tracking redirected user folders in the namespace extension. Reduces "
                    + "overhead from folder-tracking checks during shell operations. Default: 0. "
                    + "Recommended: 1 on machines without folder redirection configured.",
                Tags = ["user-profiles", "tracking", "shell", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoUserFolderRedirection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoUserFolderRedirection")],
                DetectOps = [RegOp.CheckDword(Key, "NoUserFolderRedirection", 1)],
            },
            new TweakDef
            {
                Id = "uprof-limit-profile-size",
                Label = "Disable User Profile Disk Quota Enforcement",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets EnableProfileQuota=0 in the System policy key. Turns off the built-in "
                    + "profile size quota that can force logoff or prevent profile sync when the "
                    + "disk quota is exceeded. Avoids unexpected user disruption on machines where "
                    + "storage is managed by other means (disk quotas, FSRM). Default: 1 (quota "
                    + "enforcement active if configured). Recommended: 0 when not using profile quotas.",
                Tags = ["user-profiles", "quota", "disk", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableProfileQuota", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableProfileQuota")],
                DetectOps = [RegOp.CheckDword(Key, "EnableProfileQuota", 0)],
            },
        ];
    }

    // ── UserRightsPolicy ──
    private static class _UserRightsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PrivilegeRights";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "usrrts-restrict-debug-privilege",
                Label = "Restrict SeDebugPrivilege to Administrators Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "SeDebugPrivilege allows a process to read and write to any other process's memory regardless of the target process's security descriptor. Restricting debug privilege to only administrators prevents standard users and service accounts from accessing kernel and other privileged process memory. Credential-stealing malware including Mimikatz requires SeDebugPrivilege to read LSASS memory and extract authentication credentials. Standard user accounts should never have debug privilege as there is no legitimate operational reason for non-administrators to debug system processes. Service accounts used for application workloads should be audited to ensure SeDebugPrivilege has not been inadvertently granted to them. Restricting debug privilege is one of the most effective controls against credential theft from LSASS complementing Credential Guard.",
                Tags = ["privilege", "debug", "credentials", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeDebugPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeDebugPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeDebugPrivilege", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-backup-privilege",
                Label = "Restrict SeBackupPrivilege to Backup Operators Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SeBackupPrivilege allows bypassing file and directory permissions to read any file on the system for backup purposes including the SAM database and NTDS.dit. Restricting backup privilege to designated backup operator accounts prevents standard users from using backup APIs to extract protected files. Unauthorized use of SeBackupPrivilege is a documented technique for reading the NTDS.dit Active Directory database without triggering normal access control auditing. Service accounts and applications that require file backup capabilities should be granted backup privilege through membership in the Backup Operators group rather than individually. Monitoring backup privilege use events in the security event log helps identify unauthorized access to protected files through backup APIs. Backup privilege restriction is especially important for domain controllers where NTDS.dit contains all domain password hashes.",
                Tags = ["privilege", "backup", "file-access", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeBackupPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeBackupPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeBackupPrivilege", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-restore-privilege",
                Label = "Restrict SeRestorePrivilege to Backup Operators Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SeRestorePrivilege allows overwriting any file or directory on the system bypassing normal access controls which could be used to replace critical system files. Restricting restore privilege to designated accounts prevents unauthorized users from using restore APIs to overwrite system binaries or security configuration files. Malicious use of restore privilege can replace legitimate Windows components with trojanized versions achieving persistent system compromise. System file replacement through restore privilege does not require disabling Windows File Protection if done through the backup APIs directly. Restore privilege is necessary for legitimate backup solutions and disaster recovery but should be restricted to dedicated service accounts. Monitoring restore privilege use events helps detect unauthorized file replacement operations that may indicate an active attack.",
                Tags = ["privilege", "restore", "file-integrity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeRestorePrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeRestorePrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeRestorePrivilege", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-load-driver-privilege",
                Label = "Restrict SeLoadDriverPrivilege to Administrators Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SeLoadDriverPrivilege allows loading and unloading device drivers into kernel memory providing complete system control to the account possessing it. Restricting driver load privilege to administrators ensures that only authorized accounts can introduce new code into kernel address space. Malicious drivers loaded through SeLoadDriverPrivilege can bypass antivirus, hide processes and files, and provide undetectable persistent access. Vulnerability exploitation has historically sometimes escalated from user to kernel via SeLoadDriverPrivilege held by non-admin accounts. Non-administrative accounts in Windows generally should not have driver load privilege unless there is a specific documented requirement. Driver load privilege audit events should be monitored with alerting for any driver loading by unexpected accounts or outside of authorized change windows.",
                Tags = ["privilege", "drivers", "kernel", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeLoadDriverPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeLoadDriverPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeLoadDriverPrivilege", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-take-ownership",
                Label = "Restrict SeTakeOwnershipPrivilege to Administrators Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SeTakeOwnershipPrivilege allows taking ownership of any file, directory, or other object overriding access control lists regardless of the current owner's settings. Restricting take ownership privilege prevents unauthorized users from claiming ownership of files they do not have permission to access. Take ownership attacks allow gaining access to protected files like ntds.dit or private keys by claiming ownership and then modifying ACLs to grant access. Administrative accounts legitimately need this privilege for maintenance operations but standard and service accounts generally should not. Take ownership events should be audited as legitimate use is rare and unauthorized use is a strong indicator of attempted privilege abuse. Restricting take ownership privilege reduces the risk of data access through ACL manipulation attacks.",
                Tags = ["privilege", "ownership", "acl", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeTakeOwnership", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeTakeOwnership")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeTakeOwnership", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-impersonate-privilege",
                Label = "Restrict SeImpersonatePrivilege to Service Accounts Only",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                Description =
                    "SeImpersonatePrivilege allows a process to impersonate another user's security token which can be used for token theft attacks to escalate privileges. Restricting impersonate privilege to service accounts and administrative groups prevents standard users from using impersonation for privilege escalation. Juicy Potato and similar token impersonation attacks exploit SeImpersonatePrivilege to escalate from service account to SYSTEM level access. Web application service accounts that hold impersonate privilege are common attack targets for token impersonation after exploiting web application vulnerabilities. Service accounts that require impersonation should be granted it explicitly through security group membership rather than directly and usage should be monitored. Restricting impersonate privilege is particularly important for IIS application pool accounts and other internet-facing service accounts.",
                Tags = ["privilege", "impersonation", "token-theft", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeImpersonatePrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeImpersonatePrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeImpersonatePrivilege", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-remove-network-logon-guests",
                Label = "Deny Network Logon Rights to Guest Accounts",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Network logon rights for guest accounts allow unauthenticated or guest-authenticated users to access network resources on systems where guests are allowed. Explicitly denying network logon to guest accounts prevents null-session attacks and anonymous network access to shared system resources. Guest account network access has been used to enumerate system information without authentication providing attackers a foothold for further attacks. Denying network logon rights to guests should be combined with disabling the Guest account itself for defense-in-depth. Even disabled guest accounts should have network logon rights denied to prevent accidental re-enabling from creating an access pathway. Legacy Windows networks sometimes relied on guest access for file sharing compatibility but modern environments should not have guest network access enabled.",
                Tags = ["privilege", "guest", "network-logon", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DenyNetworkLogonGuests", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DenyNetworkLogonGuests")],
                DetectOps = [RegOp.CheckDword(Key, "DenyNetworkLogonGuests", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-shutdown-privilege",
                Label = "Restrict Remote Shutdown Privilege to Administrators",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Remote shutdown privilege allows a user to shut down or restart a system remotely which can be used as a denial-of-service attack against production systems. Restricting remote shutdown privilege to administrators prevents standard users and service accounts from remotely restarting critical systems. Unauthorized remote shutdown capability could be exploited to disrupt services interrupt availability and trigger incidents that distract security teams from the primary attack. Production servers and domain controllers should only allow authorized IT administrators to perform remote shutdown operations. Remote shutdown events should be audited and alerts generated for shutdowns outside of authorized maintenance windows. Restricting shutdown privilege is a defense-in-depth control that limits the impact an attacker can cause with hijacked user credentials.",
                Tags = ["privilege", "shutdown", "availability", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictRemoteShutdown", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictRemoteShutdown")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictRemoteShutdown", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-create-global-objects",
                Label = "Restrict SeCreateGlobalPrivilege to Trusted Service Accounts",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "SeCreateGlobalPrivilege allows creating global Windows objects accessible from any user session which can be used for privilege escalation through global object manipulation. Restricting global object creation privilege to trusted service accounts prevents standard users from creating objects that can interfere with other user sessions. Global object namespace attacks can be used by malware to create objects with predictable names that privileged processes will open allowing exploitation. Applications that require creating global objects should be specifically evaluated and granted this privilege through dedicated service accounts. Global object creation restriction is particularly relevant for applications that accept data from untrusted sources and have privileged service components. Auditing global object creation events helps identify unauthorized use of this privilege that may indicate an active attack.",
                Tags = ["privilege", "global-objects", "privilege-escalation", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeCreateGlobal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeCreateGlobal")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeCreateGlobal", 1)],
            },
            new TweakDef
            {
                Id = "usrrts-restrict-act-as-os-privilege",
                Label = "Restrict SeTcbPrivilege (Act as Part of OS) to System Accounts",
                Category = "Security — Tpm Security",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "SeTcbPrivilege (Trusted Computing Base privilege) grants a process the ability to act as part of the operating system itself with the highest possible level of system access. Restricting SeTcbPrivilege to only SYSTEM and LocalService accounts prevents any user or service from obtaining OS-level capabilities that bypass all security controls. Accounts holding SeTcbPrivilege can create security tokens for any user and impersonate any principal including SYSTEM without restriction. Any process running under an account with SeTcbPrivilege has complete and unrestricted access to the entire system making credential theft of such accounts catastrophic. No administrative user account should hold SeTcbPrivilege as even administrators should operate without full OS-level access. Monitoring for accounts added to the holders of SeTcbPrivilege should generate immediate security alerts.",
                Tags = ["privilege", "tcb", "act-as-os", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictSeTcbPrivilege", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictSeTcbPrivilege")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictSeTcbPrivilege", 1)],
            },
        ];
    }

    // ── WindowsAttachmentsPolicy ──
    private static class _WindowsAttachmentsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Attachments";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "attach-dont-save-zone-info",
                Label = "Attachments: Do Not Preserve Zone ID on Downloads",
                Category = "Security — Tpm Security",
                Description =
                    "Prevents Windows from saving the Zone Identifier (Zone.Identifier ADS stream) on files downloaded from the internet. When set, users will not receive SmartScreen or Open File security warnings for downloaded files. Use only if zone information is enforced by a separate security layer.",
                Tags = ["attachments", "zone-id", "download", "smartscreen", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "SaveZoneInformation", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "SaveZoneInformation")],
                DetectOps = [RegOp.CheckDword(Key, "SaveZoneInformation", 2)],
                ImpactScore = 3,
                SafetyRating = 2,
                ImpactNote = "Removes Zone.Identifier protection; apply only if a separate DLP layer enforces download security.",
            },
            new TweakDef
            {
                Id = "attach-always-scan-with-av",
                Label = "Attachments: Require Antivirus Scan on File Open",
                Category = "Security — Tpm Security",
                Description =
                    "Forces Windows Attachment Manager to invoke the registered antivirus product before allowing the user to open any file attachment. Ensures executables and archives received via email or browser downloads are scanned prior to execution.",
                Tags = ["attachments", "antivirus", "scan", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ScanWithAntiVirus", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "ScanWithAntiVirus")],
                DetectOps = [RegOp.CheckDword(Key, "ScanWithAntiVirus", 3)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Ensures every attachment is antivirus-scanned before execution.",
            },
            new TweakDef
            {
                Id = "attach-default-high-risk",
                Label = "Attachments: Set Default File Type Risk to High",
                Category = "Security — Tpm Security",
                Description =
                    "Sets the default file type risk level for Attachment Manager to High (3). Files with unknown extension risk mappings are treated as high-risk and trigger a security prompt before execution. Protects against novel filetype exploit vectors.",
                Tags = ["attachments", "file-type", "risk", "high", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DefaultFileTypeRisk", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultFileTypeRisk")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultFileTypeRisk", 3)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Unknown file extensions treated as high-risk; reduces novel-filetype exploit exposure.",
            },
            new TweakDef
            {
                Id = "attach-show-zone-info-properties",
                Label = "Attachments: Show Zone ID in File Properties (Zone Tab Visible)",
                Category = "Security — Tpm Security",
                Description =
                    "Ensures the Zone Information tab is visible in file properties for downloaded files. When HideZoneInfoOnProperties=0, users can inspect a file's zone origin, supporting security awareness and incident investigation.",
                Tags = ["attachments", "zone-id", "properties", "transparency", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "HideZoneInfoOnProperties", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "HideZoneInfoOnProperties")],
                DetectOps = [RegOp.CheckDword(Key, "HideZoneInfoOnProperties", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Zone tab visible in file properties; aids security incident investigation.",
            },
            new TweakDef
            {
                Id = "attach-notify-blocked-executables",
                Label = "Attachments: Notify User When Executable Attachment Is Blocked",
                Category = "Security — Tpm Security",
                Description =
                    "Enables user notification when Attachment Manager blocks a potentially unsafe file from being opened. Alerts are displayed when a download is prevented by file-type risk classification, improving user awareness that a file was quarantined.",
                Tags = ["attachments", "notify", "block", "executable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NotifyOnRunBlockedFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NotifyOnRunBlockedFiles")],
                DetectOps = [RegOp.CheckDword(Key, "NotifyOnRunBlockedFiles", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "User notified when an attachment is blocked rather than silently dropped.",
            },
            new TweakDef
            {
                Id = "attach-block-remote-file-open",
                Label = "Attachments: Block Direct Open of Remote Files Without Save",
                Category = "Security — Tpm Security",
                Description =
                    "Prevents users from opening downloaded files without first saving them locally (where zone information and AV scanning are applied). Helps enforce the attachment scan pipeline for files opened directly from browser 'Open' prompts.",
                Tags = ["attachments", "remote", "open", "block", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowInternetAccess", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowInternetAccess")],
                DetectOps = [RegOp.CheckDword(Key, "AllowInternetAccess", 0)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Forces save-before-open workflow; ensures AV scan and zone tagging are applied to downloads.",
            },
            new TweakDef
            {
                Id = "attach-disable-file-unblock",
                Label = "Attachments: Prevent Users from Unblocking File Attachments",
                Category = "Security — Tpm Security",
                Description =
                    "Disables the 'Unblock' option on file properties for internet-zone downloads. Prevents users from bypassing attachment security by removing the zone identifier (Zone.Identifier stream) via the file's Properties → Security tab.",
                Tags = ["attachments", "unblock", "zone-id", "bypass", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoUnblockAttachments", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoUnblockAttachments")],
                DetectOps = [RegOp.CheckDword(Key, "NoUnblockAttachments", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Prevents users removing Zone.Identifier via file Properties Unblock button.",
            },
            new TweakDef
            {
                Id = "attach-force-zone-id-prompt",
                Label = "Attachments: Force Security Warning Prompt for Internet-Zone Files",
                Category = "Security — Tpm Security",
                Description =
                    "Ensures the security warning dialog is always displayed when a user attempts to open files tagged with the Internet zone identifier. Prevents security-zone bypass for files copied into local folders that might otherwise strip zone data.",
                Tags = ["attachments", "zone-id", "prompt", "internet-zone", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceZoneIDPrompt", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceZoneIDPrompt")],
                DetectOps = [RegOp.CheckDword(Key, "ForceZoneIDPrompt", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Security warning always shown for Internet-zone files regardless of folder location.",
            },
            new TweakDef
            {
                Id = "attach-disable-inheritance-bypass",
                Label = "Attachments: Block Zone Inheritance Bypass for Attachments",
                Category = "Security — Tpm Security",
                Description =
                    "Prevents attachment processing from inheriting a lower-risk zone classification from parent application contexts. Ensures that all attachments opened from email clients or Office files are evaluated at the attachment's own zone level.",
                Tags = ["attachments", "zone-inheritance", "bypass", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInheritanceZoneMap", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInheritanceZoneMap")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInheritanceZoneMap", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Attachments from Office or email evaluated at their own zone level, not the calling app's.",
            },
            new TweakDef
            {
                Id = "attach-block-mime-sniff-override",
                Label = "Attachments: Block MIME-Type Sniffing as Risk Classification Override",
                Category = "Security — Tpm Security",
                Description =
                    "Prevents Attachment Manager from using MIME content-type sniffing to downgrade the risk classification of a file beyond its registered extension risk level. A JPEG served with an executable MIME type remains high-risk.",
                Tags = ["attachments", "mime", "sniff", "risk-override", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockMimeTypeChange", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMimeTypeChange")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMimeTypeChange", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "MIME content-type sniffing cannot downgrade executable risk classification.",
            },
        ];
    }

    // ── WindowsEventLogAccessPolicy ──
    private static class _WindowsEventLogAccessPolicy
    {
        private const string AppLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Application";
        private const string SecLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Security";
        private const string SysLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\System";
        private const string PsLog = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EventLog\Windows PowerShell";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "evtacc-set-powershell-log-size-50mb",
                Label = "Event Log Access: Set PowerShell Log Maximum Size to 50 MB",
                Category = "Security — Tpm Security",
                Description =
                    "Sets the maximum size of the Windows PowerShell event log to 50 MB (51,200 KB). "
                    + "PowerShell logs are critical for detecting malicious script execution, living-off-the-land attacks, and lateral movement. "
                    + "The default PowerShell log size is too small to retain a meaningful window of script block and operational events. "
                    + "Removing this policy reverts the PowerShell log to its configured or default maximum size.",
                Tags = ["event-log", "powershell-log", "log-size", "threat-detection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [PsLog],
                ApplyOps = [RegOp.SetDword(PsLog, "MaxSize", 51200)],
                RemoveOps = [RegOp.DeleteValue(PsLog, "MaxSize")],
                DetectOps = [RegOp.CheckDword(PsLog, "MaxSize", 51200)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Expands PowerShell log to 50 MB; retains more script event history for threat detection.",
            },
            new TweakDef
            {
                Id = "evtacc-system-log-autobackup",
                Label = "Event Log Access: Auto-Backup System Log When Full",
                Category = "Security — Tpm Security",
                Description =
                    "Enables automatic backup archiving of the system event log when it reaches capacity. "
                    + "System events relating to hardware failures, driver crashes, or service terminations should be preserved. "
                    + "Auto-backup ensures system events are not lost when the log fills during a high-activity period such as a malware incident or hardware degradation. "
                    + "Removing this policy disables automatic backup of the system log on overflow.",
                Tags = ["event-log", "system-log", "backup", "archive", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [SysLog],
                ApplyOps = [RegOp.SetDword(SysLog, "AutoBackupLogFiles", 1)],
                RemoveOps = [RegOp.DeleteValue(SysLog, "AutoBackupLogFiles")],
                DetectOps = [RegOp.CheckDword(SysLog, "AutoBackupLogFiles", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Archives system log on overflow; ensures hardware/service events are not lost.",
            },
        ];
    }
}
