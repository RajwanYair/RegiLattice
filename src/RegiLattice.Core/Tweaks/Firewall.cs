namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

// ── merged from Firewall.cs ──
internal static class Firewall
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-enable-all-profiles",
            Label = "Enable Firewall on All Profiles",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables Windows Defender Firewall on Domain, Private, and Public profiles. Default: enabled. Ensures firewall is not quietly disabled.",
            Tags = ["firewall", "security", "enable", "profiles"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
            ],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "EnableFirewall",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "EnableFirewall",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "EnableFirewall"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "EnableFirewall"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "EnableFirewall",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-public",
            Label = "Block All Inbound on Public Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks all inbound connections on the public network profile (including those in the allowed apps list). Maximum security on untrusted networks. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "public", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-logging-success",
            Label = "Enable Firewall Logging (Successful Connections)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of successful connections across all profiles. Useful for security auditing and forensics. Default: not logged.",
            Tags = ["firewall", "logging", "security", "audit", "connections"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-increase-log-size",
            Label = "Increase Firewall Log Size to 32 MB",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the firewall log file maximum size to 32768 KB (32 MB) across all profiles. Default: 4096 KB (4 MB).",
            Tags = ["firewall", "logging", "size", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    32768
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileSize",
                    32768
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    4096
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileSize",
                    4096
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    4096
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-notifications-public",
            Label = "Disable Firewall Notifications on Public",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses firewall notification popups when apps are blocked on the public profile. Reduces noise on servers and kiosks. Default: notifications shown.",
            Tags = ["firewall", "notifications", "public", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-default-inbound-block",
            Label = "Default Inbound Action: Block",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the default inbound action to Block on all profiles. Only explicitly allowed connections pass through. Default: Block (ensures it stays that way).",
            Tags = ["firewall", "security", "inbound", "block", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultInboundAction",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultInboundAction",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultInboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultInboundAction"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultInboundAction",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-stealth-mode",
            Label = "Enable Stealth Mode (Ignore Unsolicited)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables stealth mode on the public profile — the computer does not respond to unsolicited requests and is invisible to port scans. Default: varies.",
            Tags = ["firewall", "security", "stealth", "port-scan", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DisableStealthMode", 0)],
        },
        new TweakDef
        {
            Id = "fw-disable-unicast-response",
            Label = "Disable Unicast Response to Multicast",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the computer from sending unicast responses to multicast or broadcast messages. Reduces attack surface. Default: allowed.",
            Tags = ["firewall", "security", "multicast", "broadcast", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-domain",
            Label = "Block All Inbound on Domain Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks all inbound connections on the domain profile. Provides maximum protection but may break domain services. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "domain", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-inbound-private",
            Label = "Block All Inbound on Private Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks all inbound connections on the private/home profile. Stricter than default which allows exceptions. Default: not blocked.",
            Tags = ["firewall", "security", "inbound", "private", "block"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DoNotAllowExceptions",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-notifications-domain",
            Label = "Disable Firewall Notifications (Domain)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables firewall notification popups on the domain profile. Reduces distractions in managed environments. Default: enabled.",
            Tags = ["firewall", "notifications", "domain", "quiet"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-stealth-mode-private",
            Label = "Disable Stealth Mode (Private Profile)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables stealth mode on the private profile. Stealth mode prevents the computer from responding to PING and other probing requests. Disabling makes the machine discoverable on home networks. Default: off.",
            Tags = ["firewall", "stealth", "private", "discovery", "ping"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableStealthMode",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-stealth-mode-public",
            Label = "Enable Stealth Mode (Public Profile)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables stealth mode on the public profile. Makes the computer invisible to port scanners and PING sweeps on untrusted networks. Default: varies.",
            Tags = ["firewall", "stealth", "public", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DisableStealthMode",
                    0
                ),
            ],
        },
        // ── Sprint 18 — 10 new Firewall tweaks ────────────────────────────
        new TweakDef
        {
            Id = "fw-disable-multicast-broadcast-response",
            Label = "Disable Multicast/Broadcast Response (All Profiles)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the firewall from responding to multicast and broadcast traffic on all profiles. Reduces network visibility. Default: respond.",
            Tags = ["firewall", "multicast", "broadcast", "stealth", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableUnicastResponsesToMulticastBroadcast",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-domain-profile-logging",
            Label = "Enable Domain Profile Firewall Logging",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables dropped packet and successful connection logging for the domain firewall profile. Default: logging off.",
            Tags = ["firewall", "logging", "domain", "audit", "monitoring"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-outbound-default-public",
            Label = "Default-Block Outbound on Public Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets default outbound action to block on the public firewall profile. Only explicitly allowed apps can send traffic. Default: allow.",
            Tags = ["firewall", "outbound", "public", "block", "restrictive"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-netbios-inbound",
            Label = "Block NetBIOS Inbound (All Profiles)",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks inbound NetBIOS name service (UDP 137-138) via firewall policy. Reduces SMB relay and NBNS poisoning risk. Default: allowed.",
            Tags = ["firewall", "netbios", "inbound", "smb", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=17|LPort=137-138|Name=RegiLattice Block NetBIOS Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockNetBIOS-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=17|LPort=137-138|Name=RegiLattice Block NetBIOS Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-smb-inbound-public",
            Label = "Block SMB Inbound on Public Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks inbound SMB (TCP 445) on the public firewall profile. Prevents remote file sharing exploits on untrusted networks. Default: allowed.",
            Tags = ["firewall", "smb", "inbound", "public", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=445|Profile=Public|Name=RegiLattice Block SMB Public Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockSMB-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=445|Profile=Public|Name=RegiLattice Block SMB Public Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-firewall-notifications",
            Label = "Disable Firewall Notification Pop-ups",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses Windows Firewall notification pop-ups when a program is blocked. Default: notifications shown.",
            Tags = ["firewall", "notifications", "popup", "quiet", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-set-log-max-size",
            Label = "Increase Firewall Log Max Size to 32 MB",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum firewall log file size from 4 MB (default) to 32 MB for better audit trail retention.",
            Tags = ["firewall", "logging", "log-size", "audit", "retention"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    32768
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    4096
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogFileMaxSize",
                    32768
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-block-rpc-inbound-public",
            Label = "Block RPC Inbound on Public Profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Blocks inbound RPC (TCP 135) on the public firewall profile. Prevents DCOM/RPC exploits on untrusted networks. Default: allowed.",
            Tags = ["firewall", "rpc", "dcom", "inbound", "public", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=135|Profile=Public|Name=RegiLattice Block RPC Public Inbound|"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\FirewallRules",
                    "RegiLattice-BlockRPC-Public-In",
                    "v2.30|Action=Block|Active=TRUE|Dir=In|Protocol=6|LPort=135|Profile=Public|Name=RegiLattice Block RPC Public Inbound|"
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-domain",
            Label = "Prevent local firewall policy override in Domain profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "domain", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-private",
            Label = "Prevent local firewall policy override in Private profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "private", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-local-policy-merge-public",
            Label = "Prevent local firewall policy override in Public profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "public", "policy-merge"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-inbound-block-domain",
            Label = "Block all inbound connections in Domain profile via GPO",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "inbound", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "DefaultInboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-inbound-block-private",
            Label = "Block all inbound connections in Private profile via GPO",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "inbound", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultInboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-size-domain",
            Label = "Maximize firewall log size for Domain profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize", 32767),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "LogFileSize", 32767),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-size-private",
            Label = "Maximize firewall log size for Private profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "private"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize", 32767),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging", "LogFileSize", 32767),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-dropped-domain",
            Label = "Enable logging of dropped packets in Domain profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "dropped", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "EnableLogDroppedPackets", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging", "EnableLogDroppedPackets"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-dropped-private",
            Label = "Enable logging of dropped packets in Private profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "dropped", "private"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging",
                    "EnableLogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-success-domain",
            Label = "Enable logging of successful connections in Domain profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "success", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-log-success-public",
            Label = "Enable logging of successful connections in Public profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "logging", "success", "public"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging",
                    "EnableLogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-outbound-block-private",
            Label = "Block outbound connections by default in Private profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["firewall", "gpo", "outbound", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DefaultOutboundAction", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-exceptions-public",
            Label = "Do not allow firewall exceptions in Public profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "exceptions", "public"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "DoNotAllowExceptions", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-exceptions-private",
            Label = "Do not allow firewall exceptions in Private profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "exceptions", "private"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile", "DoNotAllowExceptions", 1),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-ipsec-merge-domain",
            Label = "Prevent local IPsec policy override in Domain profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "ipsec", "domain"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-no-ipsec-merge-public",
            Label = "Prevent local IPsec policy override in Public profile",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "ipsec", "public"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile", "AllowLocalIPsecPolicyMerge", 0),
            ],
        },
        new TweakDef
        {
            Id = "fw-gpo-enable-domain-profile",
            Label = "Enable Windows Firewall for Domain profile via GPO",
            Category = "Security — Windows Adcs",
            NeedsAdmin = true,
            CorpSafe = true,
            Tags = ["firewall", "gpo", "enable", "domain"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile", "EnableFirewall", 1)],
        },
    ];
}
