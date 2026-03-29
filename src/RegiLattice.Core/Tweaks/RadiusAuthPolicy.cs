// RegiLattice.Core — Tweaks/RadiusAuthPolicy.cs
// RADIUS Authentication Policy — Sprint 550.
// Configures Group Policy for Windows Network Policy Server (NPS / RADIUS):
// EAP-TLS enforcements, server certificate validation, accounting logging,
// retry and timeout settings, and connection request policy.
// Category: "RADIUS Auth Policy" | Slug: radius
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NPS
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\NetworkAccess

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RadiusAuthPolicy
{
    private const string NpsKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NPS";

    private const string NetworkAccessKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccess";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "radius-require-server-cert-validation",
                Label = "RADIUS: Require Server Certificate Validation for EAP-TLS/PEAP",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets ValidateServerCert=1 in NPS policy. Requires client supplicants to validate the NPS/RADIUS server's TLS certificate before completing the EAP-TLS or PEAP authentication handshake. Without server certificate validation, a rogue RADIUS server can impersonate the legitimate NPS server (evil twin attack) and harvest EAP credentials or perform man-in-the-middle authentication. Server certificate validation prevents this by verifying the RADIUS server's identity using the trusted PKI before committing credentials.",
                Tags = ["radius", "nps", "eap", "certificate", "authentication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "RADIUS server certificate must be trusted by client machines. Deploy NPS server certificate from an enterprise CA that clients trust. Without this setup, 802.1x authentication fails.",
                ApplyOps = [RegOp.SetDword(NpsKey, "ValidateServerCert", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "ValidateServerCert")],
                DetectOps = [RegOp.CheckDword(NpsKey, "ValidateServerCert", 1)],
            },
            new TweakDef
            {
                Id = "radius-disable-legacy-eap-md5",
                Label = "RADIUS: Disable Legacy EAP-MD5 Authentication Method",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets DisableEapMD5=1 in NPS policy. Removes EAP-MD5 from the list of accepted EAP authentication methods in the Windows Network Policy Server. EAP-MD5 is the oldest EAP method and is fundamentally insecure: it is vulnerable to dictionary attacks and offline brute force because the MD5 challenge-response is transmitted in the clear. RFC 9190 has deprecated EAP-MD5. Modern deployments should use EAP-TLS (certificate-based) or PEAP-MSCHAPv2 (password-based with TLS tunnel).",
                Tags = ["radius", "nps", "eap", "md5", "authentication"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "EAP-MD5 clients cannot authenticate. Legacy devices that support only EAP-MD5 must be replaced or given alternative access. Most enterprise clients support EAP-TLS or PEAP.",
                ApplyOps = [RegOp.SetDword(NpsKey, "DisableEapMD5", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "DisableEapMD5")],
                DetectOps = [RegOp.CheckDword(NpsKey, "DisableEapMD5", 1)],
            },
            new TweakDef
            {
                Id = "radius-enable-accounting-logging",
                Label = "RADIUS: Enable NPS Accounting Log to Windows Event Log",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets AccountingLogging=1 in NPS policy. Enables the NPS to log RADIUS accounting records (Start, Stop, Interim-Update, Accounting-On/Off) to the Windows Security Event Log. Accounting logs record all network access sessions: who connected, for how long, from what endpoint, with what access policy applied. These logs are essential for security investigations (who was connected when an incident occurred?) and compliance (demonstrating network access is tracked and audited).",
                Tags = ["radius", "nps", "accounting", "logging", "audit"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "RADIUS accounting events are written to the Security Event Log. Ensure sufficient log size retention is configured. Windows Event Log defaults may fill quickly in large environments.",
                ApplyOps = [RegOp.SetDword(NpsKey, "AccountingLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "AccountingLogging")],
                DetectOps = [RegOp.CheckDword(NpsKey, "AccountingLogging", 1)],
            },
            new TweakDef
            {
                Id = "radius-set-auth-retry-limit",
                Label = "RADIUS: Limit Authentication Retry Attempts to 3 per Session",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets MaxAuthRetries=3 in NetworkAccess policy. Limits the number of consecutive EAP authentication retry attempts per network access session before the access request is rejected. Without a retry limit, an attacker can enumerate EAP authentication attempts indefinitely (automated brute-force). Setting the limit to 3 matches best practice from 802.1x implementations: a user who mistyped their PIN gets two retries, and the third failure terminates the connection, requiring physical re-insertion of the token or reconnection.",
                Tags = ["radius", "authentication", "retry", "brute-force", "protection"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "After 3 failed authentication attempts the session is terminated. Users with multiple errors (e.g., wrong smart card PIN) must disconnect and reconnect. Adjust to 5 for environments with frequent PIN entry errors.",
                ApplyOps = [RegOp.SetDword(NetworkAccessKey, "MaxAuthRetries", 3)],
                RemoveOps = [RegOp.DeleteValue(NetworkAccessKey, "MaxAuthRetries")],
                DetectOps = [RegOp.CheckDword(NetworkAccessKey, "MaxAuthRetries", 3)],
            },
            new TweakDef
            {
                Id = "radius-set-eap-timeout-30s",
                Label = "RADIUS: Set EAP Authentication Timeout to 30 Seconds",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets EapTimeout=30 in NPS policy. Configures the maximum duration the NPS allows for a single EAP authentication exchange. If the EAP conversation (from initial EAP Identity request to EAP Success/Failure) takes longer than 30 seconds, the NPS terminates the access request with an Access-Reject. Short timeouts prevent slow-response attacks and stale session accumulation from half-open EAP conversations. 30 seconds is sufficient for all current EAP methods including EAP-TLS on slow links.",
                Tags = ["radius", "nps", "eap", "timeout", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Authentication exceeding 30 seconds is terminated. Smart card logon with OCSP/CRL check offline might briefly exceed this. Increase to 60 seconds if authentication latency is observed in slower environments.",
                ApplyOps = [RegOp.SetDword(NpsKey, "EapTimeout", 30)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "EapTimeout")],
                DetectOps = [RegOp.CheckDword(NpsKey, "EapTimeout", 30)],
            },
            new TweakDef
            {
                Id = "radius-enable-nps-audit-success",
                Label = "RADIUS: Enable NPS Success Audit Events",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets AuditSuccessAuthentications=1 in NPS policy. Enables the logging of successful RADIUS Access-Accept events to the Windows Security Event Log (Event 6272: NPS granted access to a user). Success logging allows security operations teams to establish a baseline of acceptable network access patterns and detect anomalies (a user authenticating from an unknown location or at an unusual time). Without success logging, only failures are recorded, making it impossible to detect horizontal movement via legitimate credentials.",
                Tags = ["radius", "nps", "audit", "success", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "RADIUS success events are written to Security Event Log. Event 6272 is generated for each successful 802.1x or VPN authentication. Volume will be high in large environments — ensure SIEM can handle the ingestion rate.",
                ApplyOps = [RegOp.SetDword(NpsKey, "AuditSuccessAuthentications", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "AuditSuccessAuthentications")],
                DetectOps = [RegOp.CheckDword(NpsKey, "AuditSuccessAuthentications", 1)],
            },
            new TweakDef
            {
                Id = "radius-enable-nps-audit-failure",
                Label = "RADIUS: Enable NPS Failure Audit Events",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets AuditFailedAuthentications=1 in NPS policy. Enables the logging of failed RADIUS Access-Reject events to the Windows Security Event Log (Event 6273: NPS denied access to a user, with the specific rejection reason code). Failure audit logging is essential for: detecting brute-force or credential stuffing attacks against network access, diagnosing 802.1x EAP failure reasons (certificate issues, policy mismatches, account disabled), and regulatory compliance that requires failed access to be logged.",
                Tags = ["radius", "nps", "audit", "failure", "logging"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "RADIUS failure events are written to Security Event Log. Event 6273 is generated for each rejected 802.1x/VPN request. Essential for network access security monitoring.",
                ApplyOps = [RegOp.SetDword(NpsKey, "AuditFailedAuthentications", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "AuditFailedAuthentications")],
                DetectOps = [RegOp.CheckDword(NpsKey, "AuditFailedAuthentications", 1)],
            },
            new TweakDef
            {
                Id = "radius-disable-pap-authentication",
                Label = "RADIUS: Disable PAP (Password Authentication Protocol) on NPS",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets DisablePAP=1 in NPS policy. Removes PAP from the allowed RADIUS authentication protocols. PAP transmits the user password as cleartext (obfuscated only by MD5 XOR with the RADIUS shared secret) in the RADIUS Access-Request attribute (User-Password). An attacker with access to RADIUS traffic and knowledge of the shared secret can trivially recover user passwords from PAP requests. PAP is explicitly prohibited by PCI-DSS and NIST SP 800-162 for network authentication.",
                Tags = ["radius", "pap", "authentication", "cleartext", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PAP authentication is disabled. Clients that support only PAP (rare legacy devices) cannot authenticate. Verify all network clients support at least CHAP or EAP.",
                ApplyOps = [RegOp.SetDword(NpsKey, "DisablePAP", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "DisablePAP")],
                DetectOps = [RegOp.CheckDword(NpsKey, "DisablePAP", 1)],
            },
            new TweakDef
            {
                Id = "radius-restrict-shared-secret-length",
                Label = "RADIUS: Enforce Minimum 22-Character RADIUS Shared Secret",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets MinSharedSecretLength=22 in NPS policy. Enforces a minimum length for the RADIUS shared secret (the password shared between the NPS server and authenticating access points/NAS devices). The RADIUS shared secret is used as a key in the User-Password MD5 obfuscation and in the Message-Authenticator HMAC-MD5. Short shared secrets are vulnerable to offline dictionary and brute-force attacks on captured RADIUS traffic. NIST SP 800-162 recommends at least 22 random characters for RADIUS shared secrets.",
                Tags = ["radius", "shared-secret", "password", "length", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Shared secrets shorter than 22 characters are rejected at NPS configuration. Existing access points with short shared secrets must be reconfigured. Minimum change required for existing deployments.",
                ApplyOps = [RegOp.SetDword(NpsKey, "MinSharedSecretLength", 22)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "MinSharedSecretLength")],
                DetectOps = [RegOp.CheckDword(NpsKey, "MinSharedSecretLength", 22)],
            },
            new TweakDef
            {
                Id = "radius-enable-proxy-state-attribute",
                Label = "RADIUS: Enable Proxy-State Attribute Forwarding",
                Category = "RADIUS Auth Policy",
                Description =
                    "Sets EnableProxyState=1 in NPS policy. Enables the preservation and forwarding of the RADIUS 'Proxy-State' attribute (attribute 33) in proxied RADIUS requests. When an NPS server forwards authentication requests to another RADIUS server in a tiered proxy topology (e.g., NPS proxy → corporate NPS → Active Directory), the Proxy-State attribute allows the proxy chain to correlate responses to their originating requests. Without Proxy-State, high-volume RADIUS proxy deployments suffer mismatched request-response correlation.",
                Tags = ["radius", "proxy", "nps", "forwarding", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Proxy-State forwarding is enabled. Only relevant in proxied RADIUS deployments. Required for correct NPS proxy chain configuration.",
                ApplyOps = [RegOp.SetDword(NpsKey, "EnableProxyState", 1)],
                RemoveOps = [RegOp.DeleteValue(NpsKey, "EnableProxyState")],
                DetectOps = [RegOp.CheckDword(NpsKey, "EnableProxyState", 1)],
            },
        ];
}
