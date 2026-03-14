namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Firewall
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fw-enable-all-profiles",
            Label = "Enable Firewall on All Profiles",
            Category = "Firewall",
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
            Category = "Firewall",
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
            Id = "fw-enable-logging-dropped",
            Label = "Enable Firewall Logging (Dropped Packets)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of dropped packets on the domain profile. Helps troubleshoot firewall rules and detect intrusion attempts. Default: not logged.",
            Tags = ["firewall", "logging", "security", "audit", "dropped"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile\Logging",
                    "LogDroppedPackets",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogDroppedPackets",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogDroppedPackets",
                    0
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
            Id = "fw-enable-logging-success",
            Label = "Enable Firewall Logging (Successful Connections)",
            Category = "Firewall",
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
            Category = "Firewall",
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
            Category = "Firewall",
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
            Category = "Firewall",
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
            Id = "fw-default-outbound-allow",
            Label = "Default Outbound Action: Allow",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the default outbound action to Allow on all profiles. Standard configuration that doesn't break apps. Default: Allow.",
            Tags = ["firewall", "security", "outbound", "allow", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultOutboundAction",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultOutboundAction",
                    0
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultOutboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DefaultOutboundAction"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile",
                    "DefaultOutboundAction"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\DomainProfile",
                    "DefaultOutboundAction",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-stealth-mode",
            Label = "Enable Stealth Mode (Ignore Unsolicited)",
            Category = "Firewall",
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
            Category = "Firewall",
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
            Category = "Firewall",
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
            Category = "Firewall",
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
            Id = "fw-enable-logging-success-private",
            Label = "Enable Success Logging (Private Profile)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Logs successful connections on the private profile. Useful for auditing which programs communicate on your home network. Default: not logged.",
            Tags = ["firewall", "logging", "security", "audit", "private"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile\Logging",
                    "LogSuccessfulConnections",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-enable-logging-dropped-public",
            Label = "Enable Dropped Packet Logging (Public)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of dropped packets specifically on the public profile. Critical for detecting attacks on untrusted networks. Default: not logged.",
            Tags = ["firewall", "logging", "security", "dropped", "public"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogDroppedPackets",
                    0
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogDroppedPackets",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-increase-log-size-public",
            Label = "Increase Firewall Log Size (Public)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the maximum firewall log file size on the public profile to 32 MB. Default: 4 MB, which can roll over quickly on busy networks.",
            Tags = ["firewall", "logging", "public", "log-size"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    4096
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\PublicProfile\Logging",
                    "LogFileSize",
                    32768
                ),
            ],
        },
        new TweakDef
        {
            Id = "fw-disable-notifications-domain",
            Label = "Disable Firewall Notifications (Domain)",
            Category = "Firewall",
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
            Id = "fw-disable-notifications-private",
            Label = "Disable Firewall Notifications (Private)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables firewall notification popups on the private/home profile. Stops popups when apps try to listen on the network. Default: enabled.",
            Tags = ["firewall", "notifications", "private", "quiet"],
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
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\SharedAccess\Parameters\FirewallPolicy\StandardProfile",
                    "DisableNotifications",
                    0
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
            Id = "fw-default-outbound-block-public",
            Label = "Default Block Outbound (Public Profile)",
            Category = "Firewall",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets default outbound action to block on the public profile. Only explicitly allowed apps can send data. Prevents data exfiltration but may break many apps. Default: allow.",
            Tags = ["firewall", "security", "outbound", "public", "block"],
            SideEffects = "May break internet access for apps without explicit outbound rules.",
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
            Id = "fw-disable-stealth-mode-private",
            Label = "Disable Stealth Mode (Private Profile)",
            Category = "Firewall",
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
            Category = "Firewall",
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
    ];
}
