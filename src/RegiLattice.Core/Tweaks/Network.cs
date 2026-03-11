namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Network
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "net-increase-irpstack",
            Label = "Increase IRPStackSize",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the I/O Request Packet stack size to 32 for better network/file-sharing throughput.",
            Tags = ["network", "performance", "smb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 15),
            ],
        },
        new TweakDef
        {
            Id = "net-enable-rdp",
            Label = "Enable Remote Desktop",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Remote Desktop with Network Level Authentication (NLA).",
            Tags = ["network", "remote", "rdp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Wi-Fi Sense auto-connect to suggested open hotspots.",
            Tags = ["network", "wifi", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-llmnr",
            Label = "Disable LLMNR",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution. Mitigates LLMNR poisoning attacks on enterprise networks.",
            Tags = ["network", "security", "llmnr", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-wpad",
            Label = "Disable WPAD Auto-Proxy",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Web Proxy Auto-Discovery (WPAD). Prevents rogue WPAD attacks on untrusted networks.",
            Tags = ["network", "security", "proxy", "wpad"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad", "WpadOverride", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad", "WpadOverride"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad", "WpadOverride", 1)],
        },
        new TweakDef
        {
            Id = "net-enable-ecn",
            Label = "Enable TCP ECN",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Explicit Congestion Notification for smarter TCP congestion control without packet loss.",
            Tags = ["network", "performance", "ecn", "tcp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableECN", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableECN"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableECN", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-smbv1",
            Label = "Disable SMBv1 Client",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the legacy and insecure SMBv1 protocol. Protects against EternalBlue and similar exploits.",
            Tags = ["network", "security", "smb", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "EnableSecuritySignature", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "Start", 4)],
        },
        new TweakDef
        {
            Id = "net-increase-dns-cache",
            Label = "Increase DNS Cache TTL (24h)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the DNS cache TTL to 24 hours and reduces negative cache to 5 seconds for faster repeat lookups.",
            Tags = ["network", "performance", "dns", "cache"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "net-increase-arp-cache",
            Label = "Increase ARP Cache Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the ARP cache lifetime to 3600 seconds (1 hour). Reduces ARP broadcast traffic on busy networks and speeds up repeated connections to known hosts. Default: 120s. Recommended: 3600s.",
            Tags = ["network", "arp", "cache", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheLife", 3600),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheMinReferencedLife", 3600),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheLife"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheMinReferencedLife"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheLife", 3600)],
        },
        new TweakDef
        {
            Id = "net-rss-enable",
            Label = "Enable Receive Side Scaling (RSS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Receive Side Scaling (RSS) to distribute network receive processing across multiple CPU cores. Improves throughput on multi-core systems with supported NICs. Default: OS-managed. Recommended: enabled.",
            Tags = ["network", "rss", "performance", "throughput", "multicore"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters", "RssBaseCpu", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableRSS", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters", "RssBaseCpu"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableRSS"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableRSS", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-nagle",
            Label = "Disable Nagle's Algorithm",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Nagle's algorithm via TcpNoDelay for lower network latency. Sends TCP packets immediately without buffering. Default: Enabled. Recommended: Disabled for gaming/real-time.",
            Tags = ["network", "nagle", "tcp", "latency", "gaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpAckFrequency", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPNoDelay", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpAckFrequency"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPNoDelay"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPNoDelay", 1)],
        },
        new TweakDef
        {
            Id = "net-increase-max-connections",
            Label = "Increase Max User Port Range",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Increases maximum ephemeral port range to 65534. Allows more concurrent outbound connections for high-throughput workloads. Default: 5000. Recommended: 65534 for servers.",
            Tags = ["network", "port", "connections", "throughput", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpTimedWaitDelay", 30),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 65535),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow", 65535),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpTimedWaitDelay"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534)],
        },
        new TweakDef
        {
            Id = "net-disable-teredo",
            Label = "Disable Teredo Tunneling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Teredo IPv6 tunneling which is rarely used and can be a security risk. Default: enabled. Recommended: disabled.",
            Tags = ["network", "teredo", "ipv6", "tunneling", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 4)],
        },
        new TweakDef
        {
            Id = "net-disable-isatap",
            Label = "Disable ISATAP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the ISATAP IPv6 transition adapter. Removes an unnecessary virtual adapter. Default: enabled. Recommended: disabled.",
            Tags = ["network", "isatap", "ipv6", "adapter", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Disabled"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Default"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Disabled")],
        },
        new TweakDef
        {
            Id = "net-tcp-autotune-restricted",
            Label = "Set TCP Auto-Tuning to Restricted",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets TCP receive window auto-tuning to restricted mode. Can improve compatibility with older routers/firewalls. Default: normal. Recommended: restricted for problematic networks.",
            Tags = ["network", "tcp", "autotune", "window", "restricted"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning", 0)],
        },
        new TweakDef
        {
            Id = "net-enable-smb-signing",
            Label = "Require SMB Packet Signing",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enforces SMB packet signing on all server connections. Protects against NTLM relay and man-in-the-middle attacks on file shares. Default: Not required. Recommended: Enabled on corp networks.",
            Tags = ["network", "smb", "signing", "security", "ntlm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableSecuritySignature", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "EnableSecuritySignature", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-network-wizard",
            Label = "Suppress Network Location Wizard",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Suppresses the 'Set Network Location' dialog when connecting to a new network. Useful on headless/server machines. Default: Enabled (dialog appears).",
            Tags = ["network", "wizard", "dialog", "location", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "(Default)", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "(Default)"),
            ],
        },
        new TweakDef
        {
            Id = "net-tcp-timestamps",
            Label = "Enable TCP Timestamps and Window Scaling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables TCP timestamps (RFC 1323) for more accurate RTT calculations. Improves TCP performance on high-bandwidth connections. Default: Not set. Recommended: Enabled.",
            Tags = ["network", "tcp", "timestamps", "rfc1323", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-ipv6",
            Label = "Disable All IPv6 Tunneling Components",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets DisabledComponents=255 to disable all IPv6 tunnel adapters (6to4, ISATAP, Teredo, etc.) at once. Reduces attack surface on IPv4-only networks. Default: Enabled. Recommended: Disabled on pure IPv4 networks.",
            Tags = ["network", "ipv6", "tunnel", "6to4", "isatap", "teredo", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            SideEffects = "IPv6 connectivity is fully disabled.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
        },
        new TweakDef
        {
            Id = "net-disable-pnrp",
            Label = "Disable Peer Name Resolution Protocol",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the PNRP service used for peer-to-peer name resolution. Eliminates an infrequently used network service. Default: Manual. Recommended: Disabled.",
            Tags = ["network", "pnrp", "p2p", "service", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "net-disable-wcn",
            Label = "Disable Windows Connect Now (WCN)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Connect Now which broadcasts Wi-Fi credentials over USB and NFC. Policy-level control to prevent accidental credential sharing. Default: Enabled. Recommended: Disabled.",
            Tags = ["network", "wcn", "wifi", "credentials", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars", "DisableWPDRegistrar", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars", "EnableRegistrars", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars", "DisableWPDRegistrar"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars", "EnableRegistrars"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WCN\Registrars", "EnableRegistrars", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-task-offload",
            Label = "Disable TCP/IP Task Offload",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TCP/IP task offloading to the NIC. Resolves connectivity issues caused by buggy NIC firmware or driver offload bugs. Default: Enabled. Recommended: Disabled for troubleshooting network issues.",
            Tags = ["network", "tcp", "offload", "nic", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters"],
            SideEffects = "May slightly increase CPU usage for network processing.",
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "DisableTaskOffload", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "DisableTaskOffload"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "DisableTaskOffload", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-network-location-wizard",
            Label = "Disable Network Location Wizard",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 'Set Network Location' wizard when connecting to new networks. Default: enabled.",
            Tags = ["network", "location", "wizard", "dialog"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network", "NewNetworkWindowOff", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network", "NewNetworkWindowOff")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network", "NewNetworkWindowOff", 1)],
        },
        new TweakDef
        {
            Id = "net-enable-rss",
            Label = "Enable Receive Side Scaling (RSS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Receive Side Scaling to distribute network receive processing across multiple CPUs. Default: enabled.",
            Tags = ["network", "rss", "performance", "multi-core"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "EnableRSS", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "EnableRSS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "EnableRSS", 1)],
        },
        new TweakDef
        {
            Id = "net-increase-tcp-connections",
            Label = "Increase Max TCP Connections per Server",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum number of concurrent TCP connections per server from 2 to 16. Improves download speeds. Default: 2.",
            Tags = ["network", "tcp", "connections", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER", "explorer.exe", 16)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER", "explorer.exe")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER", "explorer.exe", 16)],
        },
        new TweakDef
        {
            Id = "net-disable-link-local-multicast",
            Label = "Disable Link-Local Multicast Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LLMNR which can be exploited for credential relay attacks. DNS should be used instead. Default: enabled.",
            Tags = ["network", "llmnr", "multicast", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NetBIOS over TCP/IP. Reduces attack surface from NBT-NS poisoning. May break legacy SMB. Default: enabled.",
            Tags = ["network", "netbios", "tcpip", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
    ];
}
