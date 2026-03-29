// RegiLattice.Core — Tweaks/LogonEventsAuditPolicy.cs
// Logon, logoff, and session-based advanced audit policy controls (Sprint 625).
// Category: "Logon Events Audit Policy" | Slug: logonaudit
// Key: HKLM\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies\Logon Logoff

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LogonEventsAuditPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AdvancedAuditPolicyConfiguration\System Audit Policies - Local Group Policy\Logon-Logoff";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "logonaudit-audit-logon-success-failure",
            Label = "Logon Audit: Enable Success+Failure Auditing for All Interactive and Network Logon Events",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditLogon=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4624 (successful logon) with logon type, source IP, and authentication protocol, and 4625 (failed logon) with error code, source IP, and account name for every interactive (Type 2), network (Type 3), service (Type 5), batch (Type 4), and remote desktop (Type 10) logon and logon failure. " +
                "Event 4624 and 4625 are the most fundamental SOC monitoring events — all lateral movement paths (SMB, RDP, WinRM, PsExec, WMI) generate logon events on the destination endpoint. Without logon auditing, there is no on-endpoint record of who authenticated, from where, and using what mechanism. The combination of 4624 (successful network logon) from an unexpected IP with 4648 (explicit credential use) from the same timeframe is a high-fidelity indicator for pass-the-hash lateral movement.",
            Tags = ["logon-audit", "event-4624", "event-4625", "lateral-movement", "rdp", "smb"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "All logon success and failure events generated; lateral movement via SMB/RDP/WMI leaves on-endpoint Event 4624 traces.",
            ApplyOps = [RegOp.SetDword(Key, "AuditLogon", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditLogon")],
            DetectOps = [RegOp.CheckDword(Key, "AuditLogon", 3)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-logoff-events",
            Label = "Logon Audit: Enable Logoff Event Auditing to Calculate Session Duration",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditLogoff=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4634 (account logoff) when an interactive or network session ends, enabling SIEM correlation to calculate session duration by pairing each 4624 logon event with its 4634 logoff counterpart. Session duration is an important context signal for anomalous access detection. " +
                "Session duration analysis enables detection of anomalous access patterns. A network logon (4624 Type 3) that lasts 0.3 seconds followed by a logoff (4634) is consistent with automated tool access (PsExec command execution, SMB enumeration). A session from an external IP lasting 4 hours at 2 AM is anomalous for a finance analyst's account. Without logoff events, session duration calculations are impossible and the analyst must infer session end from other activity gaps in the log.",
            Tags = ["logon-audit", "event-4634", "session-duration", "anomaly-detection", "siem"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Logoff events generated (4634); session duration calculable; anomalous session patterns detectable via logon/logoff correlation.",
            ApplyOps = [RegOp.SetDword(Key, "AuditLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditLogoff")],
            DetectOps = [RegOp.CheckDword(Key, "AuditLogoff", 1)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-account-lockout-logon",
            Label = "Logon Audit: Enable Account Lockout Event Auditing at Logon (4740 on Destination)",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditAccountLockout=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management lockout setting). Generates Security event 4625 subtype failure events on the endpoint where a locked-out account attempts logon in addition to the domain controller-generated 4740. Provides per-endpoint lockout event rather than only DC-centric events. " +
                "Domain controller-generated lockout events (4740) identify that an account locked out but report only the last DC that processed the lockout, not all the individual endpoints generating failed logon attempts that accumulated to the lockout threshold. Endpoint-generated 4625 Failure / Sub-status 0xC0000234 (account locked out at logon time) events pinpoint exactly which endpoints are producing the lockout-triggering authentication failures, enabling source system identification for spray attack forensics.",
            Tags = ["logon-audit", "account-lockout", "4740", "4625", "spray-attack", "source-identification"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Per-endpoint lockout attempt events generated; spray attack source endpoints identifiable without relying only on DC events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditAccountLockout", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditAccountLockout")],
            DetectOps = [RegOp.CheckDword(Key, "AuditAccountLockout", 1)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-network-policy-server",
            Label = "Logon Audit: Enable Network Policy Server Radius/NPS Authentication Auditing",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditNetworkPolicyServer=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 6272 (NPS granted access), 6273 (NPS denied access), 6274 (NPS discarded request), 6275 (NPS discarded accounting request), 6276 (NPS quarantined client), 6277/6278 (NPS granted probation/revoked access) for RADIUS network access control decisions made by the local NPS role. " +
                "Network Policy Server (NPS/RADIUS) is the authentication gateway for 802.1X network access control (wired and wireless NAC), VPN authentication, and DirectAccess. NPS audit events record every network access authentication decision — including which machine certificates or user credentials were validated, which NPS policy matched, and whether access was granted or denied. A compromised certificate used to authenticate to the corporate wireless network generates NPS event 6272 with the certificate thumbprint, enabling certificate abuse detection.",
            Tags = ["logon-audit", "nps", "radius", "802.1x", "vpn", "nac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "NPS/RADIUS authentication decisions audited; network access control events provide NAC bypass and certificate abuse detection.",
            ApplyOps = [RegOp.SetDword(Key, "AuditNetworkPolicyServer", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditNetworkPolicyServer")],
            DetectOps = [RegOp.CheckDword(Key, "AuditNetworkPolicyServer", 3)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-other-logon-logoff-events",
            Label = "Logon Audit: Enable 'Other Logon/Logoff Events' for Session Reconnection Tracking",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditOtherLogonLogoffEvents=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events 4649 (replay attack detected), 4778 (session reconnected to Window Station), 4779 (session disconnected from Window Station), 4800 (workstation locked), 4801 (workstation unlocked), 4802/4803 (screensaver invoked/dismissed), 5378 (credential delegation requested), 5632/5633 (wireless/wired 802.1X authentication). " +
                "Events 4778/4779 (RDP/Terminal Services session reconnect and disconnect) are critical for RDP lateral movement forensics. Each reconnect event records the source IP, session ID, and account name separately from the initial logon event. Without other logon/logoff events, an attacker who uses RDP shadowing or session hijacking (connecting to an existing session without creating a new logon event) may not generate additional 4624 events. The 4778 reconnect event captures this post-logon session reuse.",
            Tags = ["logon-audit", "other-logon", "4778", "4779", "rdp-session", "session-hijacking"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "RDP session reconnect/disconnect events (4778/4779) audited; RDP session hijacking and shadowing generate detectable events.",
            ApplyOps = [RegOp.SetDword(Key, "AuditOtherLogonLogoffEvents", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditOtherLogonLogoffEvents")],
            DetectOps = [RegOp.CheckDword(Key, "AuditOtherLogonLogoffEvents", 3)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-explicit-credential-use",
            Label = "Logon Audit: Enable Explicit Credential Use Auditing (RunAs, Over-Pass-the-Hash, WinRM)",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditExplicitCredentialUse=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4648 (logon using explicit credentials) when a process uses a different set of credentials to create a new logon session — covering RunAs executions, WMI remote command execution using explicit credentials, WinRM with credential parameters, and Over-Pass-the-Hash (explicit logon using an injected NTLM hash). " +
                "Event 4648 is a direct detection signal for Over-Pass-the-Hash and Overpass-the-Hash attacks. When Mimikatz performs an OverPTH (inject NTLM hash into a new logon session using explicit credential logon), Windows generates a 4648 event on the source machine. The combination of 4648 from Machine-A with 4624 Type 3 from Machine-B to Machine-A within the same second is a high-fidelity indicator of pass-the-hash lateral movement initiation from Machine-A.",
            Tags = ["logon-audit", "explicit-credentials", "event-4648", "overpass-the-hash", "winrm", "runas"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Explicit credential use events (4648) generated; Over-Pass-the-Hash and RunAs credential abuse directly detectable.",
            ApplyOps = [RegOp.SetDword(Key, "AuditExplicitCredentialUse", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditExplicitCredentialUse")],
            DetectOps = [RegOp.CheckDword(Key, "AuditExplicitCredentialUse", 3)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-special-logon-sensitive-groups",
            Label = "Logon Audit: Enable Special Logon Auditing for Privileged Group Member Authentication",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditSpecialLogon=1 (Success) in Advanced Audit Policy Logon/Logoff category (logon-side complement to the Account Management special logon setting). Generates Security event 4964 whenever a user whose account is a member of the Special Groups list (typically Domain Admins, Enterprise Admins) authenticates interactively or via the network, providing privileged account authentication monitoring without the noise of universal 4624 auditing. " +
                "Privileged account authentication monitoring serves as a low-effort approximation of Privileged Access Workstation (PAW) compliance enforcement. If Domain Admins should only authenticate from designated admin workstations, Event 4964 events where the source computer name is not in the approved PAW list indicate a policy violation — an admin authenticated from a regular user workstation. This SIEM rule requires only two data sources: the 4964 event and the approved PAW machine list.",
            Tags = ["logon-audit", "event-4964", "domain-admins", "paw", "privileged-access", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Privileged group member logons generate Event 4964; admin authentication from non-PAW workstations detectable by SIEM.",
            ApplyOps = [RegOp.SetDword(Key, "AuditSpecialLogon", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditSpecialLogon")],
            DetectOps = [RegOp.CheckDword(Key, "AuditSpecialLogon", 1)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-group-membership-at-logon",
            Label = "Logon Audit: Enable Group Membership Enumeration at Logon for Privilege Visibility",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditGroupMembership=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4627 which lists the full group membership of the logon token at logon time, complementing Event 4624 with the list of all security groups the logging-on user is a member of at the moment of logon. Enables detection of SID injection and Kerberos golden ticket attacks using extra group SIDs. " +
                "Kerberos golden tickets can be crafted with extra group SIDs added to the PAC (Privileged Account Certificate) that were not in the account's actual group membership. When such a ticket is used for authentication, Windows generates a 4627 event showing the effective group membership of the logon token. By comparing 4627 group membership against the account's actual AD group membership, anomalous extra SIDs (e.g., Domain Admins SID for a non-admin account) are immediately visible as golden ticket indicators.",
            Tags = ["logon-audit", "group-membership", "event-4627", "golden-ticket", "pac", "kerberos"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Logon-time group membership logged (4627); Kerberos golden ticket with extra group SIDs detectable via 4627/AD membership comparison.",
            ApplyOps = [RegOp.SetDword(Key, "AuditGroupMembership", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditGroupMembership")],
            DetectOps = [RegOp.CheckDword(Key, "AuditGroupMembership", 1)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-ipsec-extended-mode",
            Label = "Logon Audit: Enable IPSec Extended Mode Auditing for Network Authentication Failures",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditIPSecExtendedMode=3 (Success+Failure) in Advanced Audit Policy Logon/Logoff category. Generates Security events for IPSec IKEv2 extended mode negotiation (4978/4979/4980/4983/4984), recording Kerberos, certificate, or preshared-key authentication exchanges, useful in environments using IPSec machine authentication for network segmentation enforcement via Windows Firewall with Advanced Security rules. " +
                "IPSec extended mode authentication provides machine-level authentication for encrypted connections between Windows endpoints in isolated network segments. Failure events from IPSec extended mode indicate endpoints attempting cross-segment communication that is blocked by IPSec policy — a potential indicator of lateral movement attempts that a compromised endpoint's attacker is trying to reach an isolated server segment. Extended mode failures highlight network segmentation policy violations in real time.",
            Tags = ["logon-audit", "ipsec", "ike", "extended-mode", "network-segmentation", "firewall"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "IPSec extended mode authentication events audited; cross-segment communication failures generate events indicating lateral movement.",
            ApplyOps = [RegOp.SetDword(Key, "AuditIPSecExtendedMode", 3)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditIPSecExtendedMode")],
            DetectOps = [RegOp.CheckDword(Key, "AuditIPSecExtendedMode", 3)],
        },
        new TweakDef
        {
            Id = "logonaudit-audit-user-device-claims",
            Label = "Logon Audit: Enable User and Device Claims Auditing for Dynamic Access Control",
            Category = "Logon Events Audit Policy",
            Description = "Sets AuditUserDeviceClaims=1 (Success) in Advanced Audit Policy Logon/Logoff category. Generates Security event 4626 at logon time, which records the user and device claims embedded in the Kerberos authentication token when Dynamic Access Control (DAC) is used — providing visibility into the claims used for conditional access decisions in DAC-protected file server and classification label systems. " +
                "Dynamic Access Control uses Kerberos claims (user department, device compliance state, classification clearance level) to make file access decisions on Windows Server file shares. A user whose Kerberos token contains an incorrect department claim (e.g., claim was modified at token issue time by a Kerberos token forgery attack) could gain access to files classified for a different department. Event 4626 records the actual claims present at logon time, enabling post-incident review of whether inappropriate access was gated on correct claim values.",
            Tags = ["logon-audit", "claims", "dynamic-access-control", "event-4626", "kerberos", "dac"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "User/device Kerberos claims logged at logon (4626); Dynamic Access Control claim-based access decisions auditable.",
            ApplyOps = [RegOp.SetDword(Key, "AuditUserDeviceClaims", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditUserDeviceClaims")],
            DetectOps = [RegOp.CheckDword(Key, "AuditUserDeviceClaims", 1)],
        },
    ];
}
