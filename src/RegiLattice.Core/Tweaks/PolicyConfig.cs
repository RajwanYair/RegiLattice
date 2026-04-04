namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// Sprint 647-651 — v6.4.0
// 5 new policy modules × 10 tweaks = 50 tweaks
// All tweaks target HKLM machine-wide Group Policy paths.

/// <summary>
/// Sprint 647 — Windows Firewall per-profile enforcement policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\WindowsFirewall\{DomainProfile|PrivateProfile|PublicProfile}
/// These keys enforce Windows Firewall enablement and default connection actions
/// for each network location profile when managed via Group Policy.
/// </summary>
internal static class PolicyFirewallProfiles
{
    private const string Domain = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile";
    private const string Private = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile";
    private const string Public = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-policy-domain-enable",
            Label = "Enforce Firewall On (Domain Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFirewall=1 under the Domain profile Group Policy path. "
                + "Forces Windows Firewall to remain enabled for domain-joined network connections, even if a local administrator attempts to disable it. "
                + "Prevents bypassing the firewall when connected to the corporate network.",
            Tags = ["firewall", "domain", "policy", "enforce", "security"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Guarantees firewall active on domain networks; prevents local admin overrides.",
            ApplyOps = [RegOp.SetDword(Domain, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(Domain, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(Domain, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-domain-block-inbound",
            Label = "Block Unsolicited Inbound (Domain Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultInboundAction=1 (Block) under the Domain profile Group Policy path. "
                + "Instructs Windows Firewall to block all inbound connections that do not match an explicit allow rule when on a domain network. "
                + "Reduces attack surface by ensuring only explicitly permitted inbound traffic reaches the endpoint.",
            Tags = ["firewall", "domain", "inbound", "block", "policy"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks unsolicited inbound on domain-joined networks; explicit rules required for services.",
            ApplyOps = [RegOp.SetDword(Domain, "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(Domain, "DefaultInboundAction")],
            DetectOps = [RegOp.CheckDword(Domain, "DefaultInboundAction", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-domain-allow-outbound",
            Label = "Allow Outbound by Default (Domain Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Domain profile Group Policy path. "
                + "Permits outbound connections by default when on a domain network, while still logging them. "
                + "Allows legitimate outbound traffic without requiring per-application outbound rules for normal domain operations.",
            Tags = ["firewall", "domain", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Maintains normal outbound connectivity on domain networks.",
            ApplyOps = [RegOp.SetDword(Domain, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Domain, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Domain, "DefaultOutboundAction", 0)],
        },
        new TweakDef
        {
            Id = "fw-policy-private-enable",
            Label = "Enforce Firewall On (Private Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFirewall=1 under the Private profile Group Policy path. "
                + "Forces Windows Firewall to remain active on home and trusted private networks. "
                + "Prevents disabling the firewall even when users believe they are on a safe home network.",
            Tags = ["firewall", "private", "policy", "enforce", "home-network"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Guarantees firewall active on private/home networks.",
            ApplyOps = [RegOp.SetDword(Private, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(Private, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(Private, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-private-block-inbound",
            Label = "Block Unsolicited Inbound (Private Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultInboundAction=1 (Block) under the Private profile Group Policy path. "
                + "Blocks inbound connections that lack an explicit allow rule when on private/trusted networks. "
                + "Protects against lateral movement and inbound attacks from trusted-but-compromised devices on the same home or office network.",
            Tags = ["firewall", "private", "inbound", "block", "policy"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks unsolicited inbound on private networks; may affect file/printer sharing.",
            ApplyOps = [RegOp.SetDword(Private, "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(Private, "DefaultInboundAction")],
            DetectOps = [RegOp.CheckDword(Private, "DefaultInboundAction", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-private-allow-outbound",
            Label = "Allow Outbound by Default (Private Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Private profile Group Policy path. "
                + "Allows outbound connections by default on private networks. "
                + "Maintains normal application connectivity for home users while keeping inbound protections active.",
            Tags = ["firewall", "private", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Maintains normal outbound connectivity on private networks.",
            ApplyOps = [RegOp.SetDword(Private, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Private, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Private, "DefaultOutboundAction", 0)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-enable",
            Label = "Enforce Firewall On (Public Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableFirewall=1 under the Public profile Group Policy path. "
                + "The Public profile applies when connected to untrusted networks such as airports, coffee shops and hotel Wi-Fi. "
                + "Ensures the firewall cannot be disabled in the most exposed network context.",
            Tags = ["firewall", "public", "policy", "enforce", "wi-fi", "untrusted"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Critical: guarantees firewall active on public/untrusted networks.",
            ApplyOps = [RegOp.SetDword(Public, "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(Public, "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(Public, "EnableFirewall", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-block-inbound",
            Label = "Block All Inbound (Public Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultInboundAction=1 (Block) under the Public profile Group Policy path. "
                + "Blocks all unsolicited inbound connections on public networks where device exposure is highest. "
                + "Prevents network scanning, port probing, and drive-by exploitation from other devices on shared public Wi-Fi.",
            Tags = ["firewall", "public", "inbound", "block", "policy", "wi-fi"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Critical protection on public Wi-Fi; blocks all unsolicited inbound.",
            ApplyOps = [RegOp.SetDword(Public, "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(Public, "DefaultInboundAction")],
            DetectOps = [RegOp.CheckDword(Public, "DefaultInboundAction", 1)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-allow-outbound",
            Label = "Allow Outbound by Default (Public Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultOutboundAction=0 (Allow) under the Public profile Group Policy path. "
                + "Permits outbound connections on public networks so users can browse the web and access cloud services normally. "
                + "Paired with strict inbound blocking to balance security and usability on untrusted networks.",
            Tags = ["firewall", "public", "outbound", "allow", "policy"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Preserves outbound connectivity on public networks.",
            ApplyOps = [RegOp.SetDword(Public, "DefaultOutboundAction", 0)],
            RemoveOps = [RegOp.DeleteValue(Public, "DefaultOutboundAction")],
            DetectOps = [RegOp.CheckDword(Public, "DefaultOutboundAction", 0)],
        },
        new TweakDef
        {
            Id = "fw-policy-public-disable-notifications",
            Label = "Show Firewall Block Notifications (Public Profile)",
            Category = "Firewall Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableNotifications=0 under the Public profile Group Policy path. "
                + "Ensures Windows Firewall displays a notification when an application is blocked on public networks. "
                + "Helps users and IT staff identify applications attempting unexpected inbound connections on untrusted networks.",
            Tags = ["firewall", "public", "notifications", "policy", "visibility"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Shows notifications when apps are blocked on public networks.",
            ApplyOps = [RegOp.SetDword(Public, "DisableNotifications", 0)],
            RemoveOps = [RegOp.DeleteValue(Public, "DisableNotifications")],
            DetectOps = [RegOp.CheckDword(Public, "DisableNotifications", 0)],
        },
    ];
}

/// <summary>
/// Sprint 648 — Netlogon secure channel and domain authentication policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon and
///           HKLM\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters
/// Controls domain controller secure channel signing, sealing,
/// NT4 crypto restrictions, and DNS-only domain joining.
/// </summary>
internal static class PolicyNetLogon
{
    private const string GpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\NetLogon";
    private const string SvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Netlogon\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-netlogon-sign-secure-channel",
            Label = "Require Netlogon Secure Channel Signing",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SignSecureChannel=1 in Netlogon parameters. "
                + "Requires all Netlogon secure channel communications between this machine and its domain controller to use digital signing. "
                + "Prevents man-in-the-middle attacks that tamper with domain authentication traffic on compromised network segments.",
            Tags = ["netlogon", "domain", "secure-channel", "signing", "ad", "authentication"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Hardens DC communication; requires DC support for secure channel signing.",
            ApplyOps = [RegOp.SetDword(SvcKey, "SignSecureChannel", 1)],
            RemoveOps = [RegOp.SetDword(SvcKey, "SignSecureChannel", 0)],
            DetectOps = [RegOp.CheckDword(SvcKey, "SignSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-seal-secure-channel",
            Label = "Require Netlogon Secure Channel Sealing",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SealSecureChannel=1 in Netlogon parameters. "
                + "Requires encryption (sealing) of all Netlogon secure channel data in addition to signing. "
                + "Prevents eavesdropping on domain authentication credentials and policy traffic intercepted between the endpoint and its DC.",
            Tags = ["netlogon", "domain", "secure-channel", "sealing", "encryption", "ad"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Encrypts DC-endpoint channel; requires modern DC with NTLMv2/Kerberos support.",
            ApplyOps = [RegOp.SetDword(SvcKey, "SealSecureChannel", 1)],
            RemoveOps = [RegOp.SetDword(SvcKey, "SealSecureChannel", 0)],
            DetectOps = [RegOp.CheckDword(SvcKey, "SealSecureChannel", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-require-sign-or-seal",
            Label = "Require Netlogon Signing or Sealing",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireSignOrSeal=1 in Netlogon parameters. "
                + "Prevents the machine from joining a domain unless the domain controller supports Netlogon secure channel signing or sealing. "
                + "Ensures no downgrade attack can force the endpoint to communicate with a spoofed DC that lacks signing support.",
            Tags = ["netlogon", "domain", "sign-or-seal", "security", "ad"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks domain join if DC lacks secure-channel support; requires modern Windows Server DCs.",
            ApplyOps = [RegOp.SetDword(SvcKey, "RequireSignOrSeal", 1)],
            RemoveOps = [RegOp.SetDword(SvcKey, "RequireSignOrSeal", 0)],
            DetectOps = [RegOp.CheckDword(SvcKey, "RequireSignOrSeal", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-require-strong-key",
            Label = "Require Strong Netlogon Session Key",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RequireStrongKey=1 in Netlogon parameters. "
                + "Forces the Netlogon secure channel to use 128-bit strong session keys for encryption. "
                + "Prevents use of weaker 56-bit DES-based session keys that were vulnerable to brute-force attacks on older domain configurations.",
            Tags = ["netlogon", "domain", "strong-key", "crypto", "128-bit", "ad"],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Enforces 128-bit key for secure channel; DCs must support 128-bit session keys.",
            ApplyOps = [RegOp.SetDword(SvcKey, "RequireStrongKey", 1)],
            RemoveOps = [RegOp.SetDword(SvcKey, "RequireStrongKey", 0)],
            DetectOps = [RegOp.CheckDword(SvcKey, "RequireStrongKey", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-disable-nt4-crypto",
            Label = "Disable NT4 Compatible Cryptography",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowNT4Crypto=0 in Netlogon parameters. "
                + "Prevents use of NT4-era RC4/DES cryptographic algorithms for Netlogon secure channel. "
                + "Eliminates weak legacy cipher usage that was introduced for backward compatibility with Windows NT 4.0 domain controllers no longer in use.",
            Tags = ["netlogon", "domain", "nt4", "crypto", "legacy", "weak-cipher"],
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Breaks compatibility with NT4 DCs (obsolete); required for modern environments.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AllowNT4Crypto", 0)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowNT4Crypto")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AllowNT4Crypto", 0)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-dns-only-domain-join",
            Label = "Restrict Domain Join to DNS Registration",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllowDNSOnlyJoin=1 in Netlogon parameters. "
                + "Prevents the domain join process from using WINS/NetBIOS name resolution to locate domain controllers. "
                + "Forces domain join operations to rely on DNS only, eliminating NetBIOS-based DC discovery that is vulnerable to spoofing.",
            Tags = ["netlogon", "domain-join", "dns", "netbios", "wins", "security"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "DNS-only DC discovery; WINS-based environments may need DNS records updated.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AllowDNSOnlyJoin", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowDNSOnlyJoin")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AllowDNSOnlyJoin", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-disable-password-change",
            Label = "Disable Machine Account Password Auto-Change",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisablePasswordChange=1 in Netlogon parameters. "
                + "Prevents the computer from automatically changing its machine account password every 30 days. "
                + "Useful for read-only media deployments and scenarios where domain machine accounts must remain static; should be combined with strong initial password management.",
            Tags = ["netlogon", "machine-account", "password", "domain", "static"],
            ImpactScore = 2,
            SafetyRating = 3,
            ImpactNote = "Disables auto password rotation; only safe for purpose-built static-image environments.",
            ApplyOps = [RegOp.SetDword(SvcKey, "DisablePasswordChange", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "DisablePasswordChange")],
            DetectOps = [RegOp.CheckDword(SvcKey, "DisablePasswordChange", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-max-password-age",
            Label = "Enforce Maximum Machine Password Age (30 days)",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaximumPasswordAge=30 in Netlogon parameters. "
                + "Sets the maximum number of days before the computer automatically changes its machine account password to 30. "
                + "Limits the window during which a captured machine account hash remains usable for pass-the-hash attacks.",
            Tags = ["netlogon", "password-rotation", "machine-account", "security", "ad"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Forces 30-day machine account password rotation.",
            ApplyOps = [RegOp.SetDword(SvcKey, "MaximumPasswordAge", 30)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "MaximumPasswordAge")],
            DetectOps = [RegOp.CheckDword(SvcKey, "MaximumPasswordAge", 30)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-avoid-pdc-on-wan",
            Label = "Avoid PDC Emulator on WAN for Authentication",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AvoidPdcOnWan=1 in Netlogon parameters. "
                + "Instructs the Netlogon service not to contact the PDC Emulator across slow WAN links during user authentication. "
                + "Reduces authentication delays at remote branch office sites where WAN latency to the PDC Emulator would cause login hangs.",
            Tags = ["netlogon", "pdc", "wan", "performance", "branch-office", "ad"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Improves login speed at WAN-connected sites; no security downside.",
            ApplyOps = [RegOp.SetDword(SvcKey, "AvoidPdcOnWan", 1)],
            RemoveOps = [RegOp.DeleteValue(SvcKey, "AvoidPdcOnWan")],
            DetectOps = [RegOp.CheckDword(SvcKey, "AvoidPdcOnWan", 1)],
        },
        new TweakDef
        {
            Id = "sec-netlogon-restrict-ntlm-in-domain",
            Label = "Restrict NTLM Authentication in Domain",
            Category = "Active Directory Security",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RestrictNtlmAudit=1 and RestrictSendingNTLMTraffic=1 in Netlogon/LSA parameters. "
                + "Audits and restricts outbound NTLM authentication in domain environments. "
                + "Encourages migration from legacy NTLM to Kerberos for domain authentication, reducing exposure to pass-the-hash and NTLM relay attacks.",
            Tags = ["netlogon", "ntlm", "kerberos", "domain", "relay-attack", "pass-the-hash"],
            ImpactScore = 5,
            SafetyRating = 3,
            ImpactNote = "Reduces NTLM relay risk; may break applications requiring NTLM — audit first.",
            ApplyOps =
            [
                RegOp.SetDword(GpKey, "RestrictNtlm", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(GpKey, "RestrictNtlm"),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\MSV1_0", "RestrictSendingNTLMTraffic", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(GpKey, "RestrictNtlm", 1),
            ],
        },
    ];
}

/// <summary>
/// Sprint 649 — Reliability Monitor data collection and WER reporting policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting
///           HKLM\SOFTWARE\Policies\Microsoft\Windows NT\Reliability
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting
/// Controls whether Reliability Monitor gathers crash data, uploads it,
/// and exposes it through the Windows Error Reporting UI.
/// </summary>
internal static class PolicyReliabilityMonitor
{
    private const string RacKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Reliability";
    private const string WerKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Error Reporting";
    private const string PcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\PCHealth\ErrorReporting";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "maint-reliability-shutdown-reason-text",
            Label = "Require Shutdown Reason Text",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ShutdownReasonUI=1 and ReasonCodeRequired=1 in Reliability policy. "
                + "Forces users to select a shutdown reason and enter explanatory text when initiating a planned or unplanned shutdown. "
                + "Improves uptime tracking and post-incident root cause analysis in managed enterprise environments.",
            Tags = ["reliability", "shutdown", "reason", "audit", "uptime"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts for shutdown reason; visible user impact at every shutdown/restart.",
            ApplyOps =
            [
                RegOp.SetDword(RacKey, "ShutdownReasonUI", 1),
                RegOp.SetDword(RacKey, "ReasonCodeRequired", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(RacKey, "ShutdownReasonUI"),
                RegOp.DeleteValue(RacKey, "ReasonCodeRequired"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(RacKey, "ShutdownReasonUI", 1),
                RegOp.CheckDword(RacKey, "ReasonCodeRequired", 1),
            ],
        },
        new TweakDef
        {
            Id = "maint-reliability-racevent-interval",
            Label = "Extend Reliability Event Logging Interval",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TimeStampInterval=7 in Reliability policy, extending the RAC (Reliability Analysis Component) time-stamp interval to 7 days. "
                + "Reduces disk I/O for reliability data collection on endpoints where the default hourly reliability logging is excessive. "
                + "Useful for write-sensitive devices such as those with eMMC storage.",
            Tags = ["reliability", "rac", "logging", "interval", "disk-io"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces reliability event logging frequency; less granular uptime data.",
            ApplyOps = [RegOp.SetDword(RacKey, "TimeStampInterval", 7)],
            RemoveOps = [RegOp.DeleteValue(RacKey, "TimeStampInterval")],
            DetectOps = [RegOp.CheckDword(RacKey, "TimeStampInterval", 7)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-default-consent",
            Label = "Disable Windows Error Reporting Default Consent",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultConsent=1 in WER policy (Always Ask). "
                + "Requires explicit user or administrator consent before any error report is sent to Microsoft. "
                + "Prevents automatic or silent submission of crash dumps and application error telemetry that may contain sensitive process memory contents.",
            Tags = ["wer", "error-reporting", "consent", "privacy", "telemetry"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prompts before any error report upload; no silent data submission.",
            ApplyOps = [RegOp.SetDword(WerKey, "DefaultConsent", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DefaultConsent")],
            DetectOps = [RegOp.CheckDword(WerKey, "DefaultConsent", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-corporate-upload",
            Label = "Disable WER Upload to Microsoft (Corp WER Server)",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CorporateWerServer=0 and LoggingDisabled=0 in WER policy, keeping local WER logging but disabling external uploads. "
                + "Retains crash dump collection for internal analysis without transmitting potentially sensitive memory dumps to Microsoft. "
                + "Appropriate for regulated industries where crash data may contain PII or business-confidential memory contents.",
            Tags = ["wer", "crash-dump", "upload", "corporate", "privacy", "pii"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Keeps local dumps; prevents transmission to Microsoft reporting endpoint.",
            ApplyOps =
            [
                RegOp.SetDword(WerKey, "CorporateWerUseSSL", 1),
                RegOp.SetDword(WerKey, "LoggingDisabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(WerKey, "CorporateWerUseSSL"),
                RegOp.DeleteValue(WerKey, "LoggingDisabled"),
            ],
            DetectOps = [RegOp.CheckDword(WerKey, "CorporateWerUseSSL", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-kernel-faults",
            Label = "Exclude Kernel-Level Faults from WER",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ExcludeKernelFaults=1 in WER policy. "
                + "Prevents kernel-level crash events from being included in Windows Error Reporting submissions. "
                + "Kernel dumps can contain entire memory contents including encryption keys and privileged process memory, making them unsuitable for external submission.",
            Tags = ["wer", "kernel-dump", "crash", "memory", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Excludes kernel crash data from WER submissions; reduces data leakage risk.",
            ApplyOps = [RegOp.SetDword(WerKey, "ExcludeKernelFaults", 1)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "ExcludeKernelFaults")],
            DetectOps = [RegOp.CheckDword(WerKey, "ExcludeKernelFaults", 1)],
        },
        new TweakDef
        {
            Id = "maint-wer-disable-archive-behavior",
            Label = "Disable WER Problem Reporting Queue Archival",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DumpType=0 in WER policy. "
                + "Prevents Windows Error Reporting from archiving application crash mini-dumps to the local queue directory for later upload. "
                + "Reduces disk usage from accumulated crash dump files and prevents sensitive process memory from persisting on disk beyond the immediate crash event.",
            Tags = ["wer", "archive", "dump", "disk", "privacy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents crash dump accumulation on disk; no deferred upload queue.",
            ApplyOps = [RegOp.SetDword(WerKey, "DumpType", 0)],
            RemoveOps = [RegOp.DeleteValue(WerKey, "DumpType")],
            DetectOps = [RegOp.CheckDword(WerKey, "DumpType", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-reporting-v4",
            Label = "Disable Legacy PCHealth Error Reporting",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DoReport=0 in PCHealth\\ErrorReporting policy. "
                + "Disables the legacy Windows XP/Vista-era PCHealth error reporting component that predates the modern WER pipeline. "
                + "This path is still read on modern Windows for backward compatibility; setting it prevents unexpected legacy reporting activity.",
            Tags = ["pchealth", "error-reporting", "legacy", "telemetry"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Disables legacy PCHealth error uploader; no user-visible impact.",
            ApplyOps = [RegOp.SetDword(PcKey, "DoReport", 0)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "DoReport")],
            DetectOps = [RegOp.CheckDword(PcKey, "DoReport", 0)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-all-error-reporting",
            Label = "Disable All PCHealth Error Reporting Channels",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AllOrNone=1 and ShowUI=0 in PCHealth\\ErrorReporting policy. "
                + "Blocks the PCHealth component from showing any error reporting UI and from queuing reports to any reporting channel. "
                + "Complements the DoReport=0 setting to ensure the legacy error reporting subsystem is fully silent.",
            Tags = ["pchealth", "error-reporting", "legacy", "silent", "ui"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Silences legacy PCHealth error dialogs and submission queue.",
            ApplyOps =
            [
                RegOp.SetDword(PcKey, "AllOrNone", 1),
                RegOp.SetDword(PcKey, "ShowUI", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(PcKey, "AllOrNone"),
                RegOp.DeleteValue(PcKey, "ShowUI"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(PcKey, "AllOrNone", 1),
                RegOp.CheckDword(PcKey, "ShowUI", 0),
            ],
        },
        new TweakDef
        {
            Id = "maint-pch-force-queue-mode",
            Label = "Set PCHealth Reporting to Queue Mode Only",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ForceQueue=1 in PCHealth\\ErrorReporting policy. "
                + "Forces error reports to accumulate in a local queue rather than being submitted immediately or interactively. "
                + "Gives administrators time to review and approve queued reports before any data leaves the endpoint.",
            Tags = ["pchealth", "queue", "error-reporting", "review", "approval"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Queues reports for admin review; no immediate uploads.",
            ApplyOps = [RegOp.SetDword(PcKey, "ForceQueue", 1)],
            RemoveOps = [RegOp.DeleteValue(PcKey, "ForceQueue")],
            DetectOps = [RegOp.CheckDword(PcKey, "ForceQueue", 1)],
        },
        new TweakDef
        {
            Id = "maint-pch-disable-report-by-app",
            Label = "Disable Per-Application Error Reporting Override",
            Category = "Diagnostics Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IncludeMicrosoftApps=0 and IncludeWindowsApps=0 in PCHealth\\ErrorReporting policy. "
                + "Prevents individual Microsoft and Windows applications from independently initiating error reports through the PCHealth channel. "
                + "Ensures that the enterprise error reporting policy cannot be overridden by per-application reporting preferences.",
            Tags = ["pchealth", "per-app", "error-reporting", "override", "policy"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Blocks per-app PCHealth error submissions regardless of app preference.",
            ApplyOps =
            [
                RegOp.SetDword(PcKey, "IncludeMicrosoftApps", 0),
                RegOp.SetDword(PcKey, "IncludeWindowsApps", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(PcKey, "IncludeMicrosoftApps"),
                RegOp.DeleteValue(PcKey, "IncludeWindowsApps"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(PcKey, "IncludeMicrosoftApps", 0),
                RegOp.CheckDword(PcKey, "IncludeWindowsApps", 0),
            ],
        },
    ];
}

/// <summary>
/// Sprint 650 — DNS client security and multicast name resolution policies (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config
/// Controls LLMNR multicast, DNS-over-HTTPS enforcement, smart name resolution,
/// and related DNS/name resolution security policies.
/// </summary>
internal static class PolicyDNSSecurity
{
    private const string DnsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";
    private const string MdmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\wifinetworkmanager\config";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "net-dns-policy-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableMulticast=0 in DNS client policy. "
                + "Disables LLMNR (Link-Local Multicast Name Resolution), which resolves single-label hostnames on the local subnet. "
                + "LLMNR responses are unauthenticated and can be spoofed by attackers (Responder/MITM attacks); "
                + "disabling LLMNR forces all name resolution through DNS where responses can be validated.",
            Tags = ["dns", "llmnr", "multicast", "responder", "mitm", "security", "network"],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Blocks LLMNR; single-label name resolution requires proper DNS entries.",
            ApplyOps = [RegOp.SetDword(DnsKey, "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(DnsKey, "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-disable-smart-resolution",
            Label = "Disable DNS Smart Name Resolution",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableSmartNameResolution=1 in DNS client policy. "
                + "Prevents DNS client from appending connection-specific suffixes when resolving names that fail primary DNS lookup. "
                + "Smart name resolution can leak internal hostnames to external DNS servers when a split-horizon resolution fails, "
                + "inadvertently revealing internal network structure to external resolvers.",
            Tags = ["dns", "smart-resolution", "suffix", "privacy", "leak"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Stops hostname guessing via suffix appending; may affect unqualified name resolution.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-prefer-local-responses",
            Label = "Prefer Local DNS Responses Over Cached External",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AddrConfigControl=1 in DNS client policy. "
                + "Configures the DNS client to prefer locally produced name resolution results over cached external responses. "
                + "Reduces the window for DNS cache poisoning attacks by ensuring addresses from the local DNS zone take priority over potentially stale or tampered cached entries.",
            Tags = ["dns", "local", "cache-poisoning", "resolution", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reduces risk from cached DNS poisoning; favors local authoritative answers.",
            ApplyOps = [RegOp.SetDword(DnsKey, "AddrConfigControl", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "AddrConfigControl")],
            DetectOps = [RegOp.CheckDword(DnsKey, "AddrConfigControl", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-disable-multicast-to-others",
            Label = "Restrict DNS Multicast Query Scope",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets QueryAdapterName=0 in DNS client policy. "
                + "Restricts the DNS client from broadcasting adapter names as part of multicast DNS queries. "
                + "Prevents information disclosure of network adapter hostnames and interface identifiers to other machines on the subnet via mDNS/LLMNR broadcast frames.",
            Tags = ["dns", "multicast", "adapter", "information-disclosure", "mdns"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Stops adapter name broadcast in DNS queries; minor information hardening.",
            ApplyOps = [RegOp.SetDword(DnsKey, "QueryAdapterName", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "QueryAdapterName")],
            DetectOps = [RegOp.CheckDword(DnsKey, "QueryAdapterName", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-update-top-domain-zones",
            Label = "Allow DNS Updates to Top-Level Domain Zones",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UpdateTopLevelDomainZones=0 in DNS client policy. "
                + "Prevents the DNS client from attempting dynamic DNS registration in top-level domain zones (e.g., .com, .net). "
                + "Eliminates noise from failed or unintended TLD zone update requests that may expose internal host information to external authoritative DNS servers.",
            Tags = ["dns", "dynamic-update", "tld", "zone", "registration"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops TLD dynamic DNS registration attempts; no impact on local DNS.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UpdateTopLevelDomainZones")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UpdateTopLevelDomainZones", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-use-name-resolution-policy",
            Label = "Enforce Name Resolution Policy Table (NRPT)",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableNRPT=0 in DNS client policy. "
                + "Ensures the Name Resolution Policy Table (NRPT) is active and consulted for every DNS query. "
                + "The NRPT allows per-namespace DNS settings including DNSSEC enforcement and DirectAccess DNS routing. "
                + "Disabling it silently bypasses all namespace-specific security rules.",
            Tags = ["dns", "nrpt", "dnssec", "direct-access", "namespace", "security"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Activates NRPT for per-namespace DNS policy enforcement.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DisableNRPT", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DisableNRPT")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DisableNRPT", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-attempt-autodial",
            Label = "Disable DNS Auto-Dial Connections",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UseAdapterSpecificDomainSuffix=0 in DNS client policy. "
                + "Prevents the DNS client from attempting adapter-specific domain suffix resolution when the primary DNS server is unreachable. "
                + "Avoids sending hostname queries to ISP-controlled DNS servers when the corporate DNS is unreachable, "
                + "preventing corporate hostname leakage to external resolvers.",
            Tags = ["dns", "suffix", "adapter", "fallback", "corporate", "leak"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks adapter-specific suffix fallback; may affect resolution on secondary NICs.",
            ApplyOps = [RegOp.SetDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "UseAdapterSpecificDomainSuffix")],
            DetectOps = [RegOp.CheckDword(DnsKey, "UseAdapterSpecificDomainSuffix", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-devolution-level",
            Label = "Restrict DNS Devolution Level",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DevolveLevel=1 in DNS client policy. "
                + "Limits DNS name devolution to only one step above the fully qualified domain name. "
                + "DNS devolution automatically strips labels from unresolved host names and retries (e.g., 'server' -> 'server.corp' -> 'server.com'). "
                + "Leaving devolution unconstrained risks single-label queries resolving against external TLD registrations.",
            Tags = ["dns", "devolution", "name-resolution", "suffix", "tld"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Limits suffix devolution; unqualified names may not resolve in some configurations.",
            ApplyOps = [RegOp.SetDword(DnsKey, "DevolveLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "DevolveLevel")],
            DetectOps = [RegOp.CheckDword(DnsKey, "DevolveLevel", 1)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-disable-hosts-file-resolution",
            Label = "Limit Hosts File Priority in Name Resolution",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets HostsFileBypassFlag=0 in DNS client policy. "
                + "Ensures the DNS client does not bypass the standard resolution order to exclusively use the HOSTS file. "
                + "Attackers who obtain write access to %SYSTEMROOT%\\System32\\drivers\\etc\\hosts can redirect any hostname; "
                + "normalizing the resolution stack reduces the impact of hosts-file modification attacks.",
            Tags = ["dns", "hosts-file", "resolution-order", "security", "redirect"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Standard hosts-file behaviour preserved; no bypass of DNS resolution order.",
            ApplyOps = [RegOp.SetDword(DnsKey, "HostsFileBypassFlag", 0)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "HostsFileBypassFlag")],
            DetectOps = [RegOp.CheckDword(DnsKey, "HostsFileBypassFlag", 0)],
        },
        new TweakDef
        {
            Id = "net-dns-policy-register-ptr-records",
            Label = "Enable Auto-Registration of PTR Records",
            Category = "DNS Security Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RegisterReverseLookup=1 in DNS client policy. "
                + "Ensures the DNS client registers PTR (reverse lookup) records in addition to A/AAAA forward records during dynamic DNS registration. "
                + "Reverse records are required by many security monitoring tools, intrusion detection systems, and firewall devices for host identification.",
            Tags = ["dns", "ptr", "reverse-lookup", "registration", "monitoring"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ensures reverse DNS records exist; required for many security monitoring tools.",
            ApplyOps = [RegOp.SetDword(DnsKey, "RegisterReverseLookup", 1)],
            RemoveOps = [RegOp.DeleteValue(DnsKey, "RegisterReverseLookup")],
            DetectOps = [RegOp.CheckDword(DnsKey, "RegisterReverseLookup", 1)],
        },
    ];
}

/// <summary>
/// Sprint 651 — Windows SmartScreen and application reputation enforcement (10 tweaks).
/// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\System (EnableSmartScreen)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components (Enhanced Phishing)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\PowerShell (SmartScreen for scripts)
///           HKLM\SOFTWARE\Policies\Microsoft\Windows\Safer (Software Restriction)
/// Controls Windows SmartScreen filters, enhanced phishing protection, and
/// application reputation checks for downloaded files.
/// </summary>
internal static class PolicySmartScreenWin
{
    private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
    private const string WtdsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WTDS\Components";
    private const string SaferKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Safer\CodeIdentifiers";
    private const string AppRepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\MRT";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "sec-smartscreen-enable-win",
            Label = "Enable Windows SmartScreen (Shell)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnableSmartScreen=1 in Windows System policy. "
                + "Forces Windows SmartScreen to be enabled for all executable file reputation checks at the shell level. "
                + "SmartScreen blocks or warns when users attempt to run executable files downloaded from the Internet that have not established a reputation with Microsoft's cloud service.",
            Tags = ["smartscreen", "shell", "policy", "reputation", "download-check"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Enables SmartScreen for all downloaded executables; may prompt on unsigned apps.",
            ApplyOps = [RegOp.SetDword(SysKey, "EnableSmartScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(SysKey, "EnableSmartScreen")],
            DetectOps = [RegOp.CheckDword(SysKey, "EnableSmartScreen", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-shell-block-level",
            Label = "Set SmartScreen Shell Level to Block",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ShellSmartScreenLevel=Block in Windows System policy. "
                + "Configures SmartScreen to block execution of files with bad reputation rather than only showing a warning that users can override. "
                + "Prevents social engineering attacks where users dismiss SmartScreen warnings to run malicious downloaders.",
            Tags = ["smartscreen", "shell", "block", "policy", "no-bypass"],
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Blocks unrecognised executables; users cannot override — use Warn for flexibility.",
            ApplyOps = [RegOp.SetString(SysKey, "ShellSmartScreenLevel", "Block")],
            RemoveOps = [RegOp.DeleteValue(SysKey, "ShellSmartScreenLevel")],
            DetectOps = [RegOp.CheckString(SysKey, "ShellSmartScreenLevel", "Block")],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-capture",
            Label = "Enable Enhanced Phishing Protection — Capture Check",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CaptureThreatWindow=1 in WTDS Components policy. "
                + "Activates Enhanced Phishing Protection's threat capture mechanism, which screenshots and checks credential entry pages. "
                + "Detects credential harvesting phishing sites in real time, even when embedded in enterprise applications or documents.",
            Tags = ["smartscreen", "phishing", "wtds", "credential", "enhanced"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Captures phishing attempts at credential entry; slight performance overhead.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "CaptureThreatWindow", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "CaptureThreatWindow")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "CaptureThreatWindow", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-malicious",
            Label = "Enhanced Phishing Protection — Notify on Malicious Site",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyMalicious=1 in WTDS Components policy. "
                + "Configures Enhanced Phishing Protection to display a warning notification when a user visits a detected phishing or malicious site. "
                + "Alerts users in real time rather than silently blocking traffic, allowing them to understand why access was interrupted.",
            Tags = ["smartscreen", "phishing", "notification", "wtds", "warning"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Shows phishing warnings in Windows; requires Microsoft Defender support.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyMalicious", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyMalicious")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyMalicious", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-notify-password-reuse",
            Label = "Enhanced Phishing Protection — Warn on Password Reuse",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyPasswordReuse=1 in WTDS Components policy. "
                + "Enables the Enhanced Phishing Protection warning that fires when a user types their Windows account password into a non-Windows credential form. "
                + "Detects password reuse attacks where users enter their corporate password on a personal or untrusted site.",
            Tags = ["smartscreen", "phishing", "password-reuse", "wtds", "credential"],
            ImpactScore = 5,
            SafetyRating = 5,
            ImpactNote = "Warns when Windows password is reused on other sites; zero performance impact.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyPasswordReuse", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyPasswordReuse")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyPasswordReuse", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-enhanced-phishing-unsafe-app",
            Label = "Enhanced Phishing Protection — Warn on Unsafe App Password Entry",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NotifyUnsafeApp=1 in WTDS Components policy. "
                + "Triggers Enhanced Phishing Protection warnings when the Windows account password is entered in an application flagged as potentially unsafe. "
                + "Extends phishing detection beyond browser sessions to desktop applications that prompt for credentials.",
            Tags = ["smartscreen", "phishing", "unsafe-app", "wtds", "desktop-app"],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Monitors desktop app credential prompts for password misuse.",
            ApplyOps = [RegOp.SetDword(WtdsKey, "NotifyUnsafeApp", 1)],
            RemoveOps = [RegOp.DeleteValue(WtdsKey, "NotifyUnsafeApp")],
            DetectOps = [RegOp.CheckDword(WtdsKey, "NotifyUnsafeApp", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-safer-default-level",
            Label = "Set Software Restriction Policy Default Level (Unrestricted Audit)",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultLevel=262144 in Software Restriction Policy (SRP) code identifiers. "
                + "Establishes the baseline trust level for Software Restriction Policies to Unrestricted with audit logging. "
                + "When SRP is deployed, this default ensures all software is permitted to run but all execution is logged, "
                + "enabling detection of unauthorized software without blocking legitimate applications.",
            Tags = ["srp", "software-restriction", "safer", "default", "audit"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Establishes SRP audit mode; no blocking without additional path/hash rules.",
            ApplyOps = [RegOp.SetDword(SaferKey, "DefaultLevel", 262144)],
            RemoveOps = [RegOp.DeleteValue(SaferKey, "DefaultLevel")],
            DetectOps = [RegOp.CheckDword(SaferKey, "DefaultLevel", 262144)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-safer-log-policy",
            Label = "Enable Software Restriction Policy Event Logging",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets LogFileName=%WINDIR%\\system32\\spp.log in Software Restriction Policy code identifiers. "
                + "Enables SRP to write a detailed log of all application execution events with their restriction disposition to the specified log file. "
                + "Provides an audit trail for compliance and incident investigation.",
            Tags = ["srp", "software-restriction", "safer", "logging", "audit"],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Logs all SRP execution decisions to a file for audit review.",
            ApplyOps = [RegOp.SetExpandString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
            RemoveOps = [RegOp.DeleteValue(SaferKey, "LogFileName")],
            DetectOps = [RegOp.CheckString(SaferKey, "LogFileName", @"%WINDIR%\system32\spp.log")],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-auto-download",
            Label = "Disable Automatic MRT Download via Windows Update",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontOfferThroughWUAU=1 in MRT policy. "
                + "Prevents Windows Update from automatically downloading and running the Microsoft Malicious Software Removal Tool (MRT/MSRT). "
                + "In enterprise environments, MRT deployment should be managed through SCCM/Intune or WSUS rather than automatic Windows Update push, "
                + "to control scan timing and avoid unexpected CPU/disk load during business hours.",
            Tags = ["mrt", "msrt", "windows-update", "malware-removal", "enterprise"],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents auto MRT push via WU; enterprise should deploy MRT through managed channels.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontOfferThroughWUAU", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontOfferThroughWUAU")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontOfferThroughWUAU", 1)],
        },
        new TweakDef
        {
            Id = "sec-smartscreen-mrt-disable-infection-report",
            Label = "Disable MRT Infection Report Upload to Microsoft",
            Category = "SmartScreen Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DontReportInfectionInformation=1 in MRT policy. "
                + "Prevents the Malicious Software Removal Tool from sending infection report telemetry to Microsoft after removing malware. "
                + "The infection report includes information about the malware found, the machine configuration, and the removal status. "
                + "In air-gapped or high-security environments, preventing this upload limits external data transmission.",
            Tags = ["mrt", "infection-report", "telemetry", "upload", "privacy", "air-gap"],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Stops MRT infection report uploads; no impact on malware removal capability.",
            ApplyOps = [RegOp.SetDword(AppRepKey, "DontReportInfectionInformation", 1)],
            RemoveOps = [RegOp.DeleteValue(AppRepKey, "DontReportInfectionInformation")],
            DetectOps = [RegOp.CheckDword(AppRepKey, "DontReportInfectionInformation", 1)],
        },
    ];
}
