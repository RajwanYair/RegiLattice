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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 15)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "IRPStackSize", 32)],
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
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp",
            ],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad", "WpadOverride", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad", "WpadOverride")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableECN", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableECN")],
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
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "Start", 3)],
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
            Description =
                "Increases the ARP cache lifetime to 3600 seconds (1 hour). Reduces ARP broadcast traffic on busy networks and speeds up repeated connections to known hosts. Default: 120s. Recommended: 3600s.",
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
            Description =
                "Enables Receive Side Scaling (RSS) to distribute network receive processing across multiple CPU cores. Improves throughput on multi-core systems with supported NICs. Default: OS-managed. Recommended: enabled.",
            Tags = ["network", "rss", "performance", "throughput", "multicore"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NDIS\Parameters",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters",
            ],
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
            Description =
                "Disables Nagle's algorithm via TcpNoDelay for lower network latency. Sends TCP packets immediately without buffering. Default: Enabled. Recommended: Disabled for gaming/real-time.",
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
            Description =
                "Increases maximum ephemeral port range to 65534. Allows more concurrent outbound connections for high-throughput workloads. Default: 5000. Recommended: 65534 for servers.",
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Teredo\Parameters", "Type", 4)],
        },
        new TweakDef
        {
            Id = "net-disable-isatap",
            Label = "Disable ISATAP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the ISATAP IPv6 transition adapter. Removes an unnecessary virtual adapter. Default: enabled. Recommended: disabled.",
            Tags = ["network", "isatap", "ipv6", "adapter", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Disabled")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Default")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ISATAP\Parameters", "State", "Disabled")],
        },
        new TweakDef
        {
            Id = "net-tcp-autotune-restricted",
            Label = "Set TCP Auto-Tuning to Restricted",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets TCP receive window auto-tuning to restricted mode. Can improve compatibility with older routers/firewalls. Default: normal. Recommended: restricted for problematic networks.",
            Tags = ["network", "tcp", "autotune", "window", "restricted"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableAutoTuning", 0)],
        },
        new TweakDef
        {
            Id = "net-enable-smb-signing",
            Label = "Require SMB Packet Signing",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enforces SMB packet signing on all server connections. Protects against NTLM relay and man-in-the-middle attacks on file shares. Default: Not required. Recommended: Enabled on corp networks.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters", "RequireSecuritySignature", 1),
            ],
        },
        new TweakDef
        {
            Id = "net-disable-network-wizard",
            Label = "Suppress Network Location Wizard",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Suppresses the 'Set Network Location' dialog when connecting to a new network. Useful on headless/server machines. Default: Enabled (dialog appears).",
            Tags = ["network", "wizard", "dialog", "location", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "(Default)", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "(Default)")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "(Default)", 0)],
        },
        new TweakDef
        {
            Id = "net-tcp-timestamps",
            Label = "Enable TCP Timestamps and Window Scaling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables TCP timestamps (RFC 1323) for more accurate RTT calculations. Improves TCP performance on high-bandwidth connections. Default: Not set. Recommended: Enabled.",
            Tags = ["network", "tcp", "timestamps", "rfc1323", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-ipv6",
            Label = "Disable All IPv6 Tunneling Components",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DisabledComponents=255 to disable all IPv6 tunnel adapters (6to4, ISATAP, Teredo, etc.) at once. Reduces attack surface on IPv4-only networks. Default: Enabled. Recommended: Disabled on pure IPv4 networks.",
            Tags = ["network", "ipv6", "tunnel", "6to4", "isatap", "teredo", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            SideEffects = "IPv6 connectivity is fully disabled.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
        },
        new TweakDef
        {
            Id = "net-disable-pnrp",
            Label = "Disable Peer Name Resolution Protocol",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the PNRP service used for peer-to-peer name resolution. Eliminates an infrequently used network service. Default: Manual. Recommended: Disabled.",
            Tags = ["network", "pnrp", "p2p", "service", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PNRPsvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "net-disable-wcn",
            Label = "Disable Windows Connect Now (WCN)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Connect Now which broadcasts Wi-Fi credentials over USB and NFC. Policy-level control to prevent accidental credential sharing. Default: Enabled. Recommended: Disabled.",
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
            Description =
                "Disables TCP/IP task offloading to the NIC. Resolves connectivity issues caused by buggy NIC firmware or driver offload bugs. Default: Enabled. Recommended: Disabled for troubleshooting network issues.",
            Tags = ["network", "tcp", "offload", "nic", "compatibility"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters"],
            SideEffects = "May slightly increase CPU usage for network processing.",
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "DisableTaskOffload", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\TCPIP\Parameters", "DisableTaskOffload")],
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
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "explorer.exe",
                    16
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "explorer.exe"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "explorer.exe",
                    16
                ),
            ],
        },
        new TweakDef
        {
            Id = "net-block-non-domain-wifi",
            Label = "Block Non-Domain WiFi Networks",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Blocks connections to WiFi networks that are not on the domain. Prevents connecting to untrusted wireless networks. Default: allowed.",
            Tags = ["network", "wifi", "domain", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy", "fBlockNonDomain", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy", "fBlockNonDomain")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\GroupPolicy", "fBlockNonDomain", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-network-throttle",
            Label = "Disable QoS Bandwidth Throttle",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the QoS reservable bandwidth limit to 0%. Prevents Windows from reserving bandwidth for QoS traffic. Default: 20%.",
            Tags = ["network", "qos", "bandwidth", "throttle"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "net-enable-dns-over-https",
            Label = "Enable DNS over HTTPS (DoH)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables automatic DNS over HTTPS for the DNS client. Encrypts DNS queries for privacy. Requires a DoH-capable DNS server. Default: disabled.",
            Tags = ["network", "dns", "doh", "https", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
        },
        new TweakDef
        {
            Id = "net-increase-max-tcp",
            Label = "Increase Maximum User Ports",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum ephemeral TCP port range to 65534. Allows more simultaneous outbound connections. Default: 5000.",
            Tags = ["network", "tcp", "ports", "connections"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534)],
        },
        new TweakDef
        {
            Id = "net-prefer-ipv4",
            Label = "Prefer IPv4 over IPv6",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the system to prefer IPv4 over IPv6 without fully disabling IPv6. Sets DisabledComponents to 0x20. Default: IPv6 preferred.",
            Tags = ["network", "ipv4", "ipv6", "preference"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 0x20)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 0x20)],
        },
        new TweakDef
        {
            Id = "net-restrict-anonymous",
            Label = "Restrict Anonymous Network Access",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Restricts anonymous enumeration of SAM accounts and shares. Prevents unauthenticated users from listing network resources. Default: allowed.",
            Tags = ["network", "security", "anonymous", "enumeration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 1)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "RestrictAnonymous", 1)],
        },
        new TweakDef
        {
            Id = "net-tcp-initial-rtt",
            Label = "Set TCP Initial RTT to 300ms",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the initial TCP retransmission timeout estimate to 300ms. Faster initial connection establishment on fast networks. Default: 3000ms.",
            Tags = ["network", "tcp", "rtt", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "InitialRtt", 300)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "InitialRtt")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "InitialRtt", 300)],
        },
        new TweakDef
        {
            Id = "net-tcp-keepalive-5min",
            Label = "Set TCP Keepalive to 5 Minutes",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TCP keepalive interval to 300000ms (5 minutes). Detects dead connections faster than the 2-hour default. Default: 7200000ms (2 hours).",
            Tags = ["network", "tcp", "keepalive", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000)],
        },
        new TweakDef
        {
            Id = "net-tcp-syn-attack-protection",
            Label = "Enable TCP SYN Attack Protection",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables TCP SYN flood attack protection. Reduces resource consumption from SYN flood attacks by enabling SYN-ACK retransmission control. Default: off.",
            Tags = ["network", "tcp", "syn", "security", "ddos"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SynAttackProtect", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SynAttackProtect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SynAttackProtect", 1)],
        },
        new TweakDef
        {
            Id = "net-throttling-index",
            Label = "Disable Multimedia Network Throttling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the multimedia system profile NetworkThrottlingIndex to 0xFFFFFFFF, disabling network traffic throttling during multimedia playback. Default: 10.",
            Tags = ["network", "throttling", "multimedia", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    10
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "net-reduce-timed-wait-delay",
            Label = "Reduce TCP TIME_WAIT Delay",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the TCP TIME_WAIT socket recycling delay from 240 s to 30 s. Frees ports faster under high connection churn.",
            Tags = ["network", "tcp", "time-wait", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpTimedWaitDelay", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpTimedWaitDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpTimedWaitDelay", 30)],
        },
        new TweakDef
        {
            Id = "net-disable-icmp-redirects",
            Label = "Disable ICMP Redirect Acceptance",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from accepting ICMP redirect packets that could silently reroute traffic. Hardens workstations against MITM-style route injection.",
            Tags = ["network", "icmp", "security", "routing"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirects", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirects")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableICMPRedirects", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-router-discovery",
            Label = "Disable ICMP Router Discovery",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the IPv4 Router Discovery (IRDP) mechanism. Prevents the host from updating its default gateway via ICMP Router Advertisements.",
            Tags = ["network", "icmp", "router", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "PerformRouterDiscovery", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "PerformRouterDiscovery")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "PerformRouterDiscovery", 0)],
        },
        new TweakDef
        {
            Id = "net-set-tcp-max-data-retransmit",
            Label = "Reduce TCP Data Retransmission Limit",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Lowers TcpMaxDataRetransmissions from 5 to 3. Dead connections are detected and closed more quickly.",
            Tags = ["network", "tcp", "retransmit", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxDataRetransmissions", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxDataRetransmissions")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxDataRetransmissions", 3)],
        },
        new TweakDef
        {
            Id = "net-disable-dead-gateway-detect",
            Label = "Disable Dead Gateway Detection",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically switching to a backup gateway when it detects the primary might be unreachable. Avoids unintended gateway changes on multi-homed systems.",
            Tags = ["network", "gateway", "routing", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDeadGWDetect", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDeadGWDetect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDeadGWDetect", 0)],
        },
        new TweakDef
        {
            Id = "net-disable-ip-source-routing",
            Label = "Disable IP Source Routing",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableIPSourceRouting=2 to fully block source-routed packets. Prevents attackers from specifying packet routing paths to bypass firewalls.",
            Tags = ["network", "ip", "routing", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableIPSourceRouting", 2)],
        },
        new TweakDef
        {
            Id = "net-set-tcp-fin-wait-delay",
            Label = "Reduce TCP FIN_WAIT_2 Delay",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the FIN_WAIT_2 timeout from 240 s to 30 s. Sockets waiting for a remote FIN are recycled faster.",
            Tags = ["network", "tcp", "fin-wait", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPFinWait2Delay", 30)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPFinWait2Delay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TCPFinWait2Delay", 30)],
        },
        new TweakDef
        {
            Id = "net-afd-receive-window",
            Label = "Increase AFD Default Receive Buffer",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the AFD driver default receive window to 128 KB. Reduces buffer pressure on high-throughput socket applications.",
            Tags = ["network", "afd", "socket", "buffer", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 131072)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 131072)],
        },
        new TweakDef
        {
            Id = "net-afd-send-window",
            Label = "Increase AFD Default Send Buffer",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the AFD driver default send window to 128 KB. Improves throughput for high-volume socket senders.",
            Tags = ["network", "afd", "socket", "buffer", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow", 131072)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow", 131072)],
        },
        new TweakDef
        {
            Id = "net-enable-selective-ack",
            Label = "Enable TCP Selective Acknowledgements (SACK)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables TCP SACK (Selective Acknowledgements). Allows the receiver to report non-contiguous received blocks so the sender retransmits only missing segments, improving throughput on lossy links.",
            Tags = ["network", "tcp", "sack", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SackOpts", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SackOpts")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "SackOpts", 1)],
        },
        new TweakDef
        {
            Id = "net-disable-dhcp-media-sense",
            Label = "Disable DHCP Media Sense",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DHCP Media Sense so Windows does not tear down the IP stack when a link-down event is detected. Prevents spurious IP address releases on flapping links.",
            Tags = ["network", "dhcp", "media-sense", "stability"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableDHCPMediaSense", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableDHCPMediaSense")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "DisableDHCPMediaSense", 1)],
        },
        new TweakDef
        {
            Id = "net-set-tcp-max-connect-retransmit",
            Label = "Reduce TCP Connect Retransmission Limit",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Lowers TcpMaxConnectRetransmissions from the default to 2. Unreachable hosts are timed out and reported more quickly during connection establishment.",
            Tags = ["network", "tcp", "retransmit", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxConnectRetransmissions", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxConnectRetransmissions")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxConnectRetransmissions", 2),
            ],
        },
        new TweakDef
        {
            Id = "net-disable-ip-forwarding",
            Label = "Disable IP Packet Forwarding",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets IPEnableRouter=0 to ensure this workstation does not forward IP packets between interfaces. Prevents the machine from inadvertently acting as a router.",
            Tags = ["network", "ip", "routing", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "IPEnableRouter", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "IPEnableRouter")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "IPEnableRouter", 0)],
        },
    ];
}

// ── Merged from NetworkOptimization.cs ──────────────────────────────────────────────────

internal static class NetworkOptimization
{
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string TcpParams = $@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netopt-disable-nagle-algorithm",
            Label = "Disable Nagle's Algorithm (Low Latency)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TCP Nagle's algorithm which buffers small packets. Improves latency for gaming and real-time apps.",
            Tags = ["network", "latency", "gaming", "tcp"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpNoDelay", 1), RegOp.SetDword(TcpParams, "TcpAckFrequency", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpNoDelay"), RegOp.DeleteValue(TcpParams, "TcpAckFrequency")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpNoDelay", 1)],
        },
        new TweakDef
        {
            Id = "netopt-increase-tcp-window-size",
            Label = "Increase TCP Window Size (High Throughput)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets TCP global receive window to 16 MB. Improves throughput on high-bandwidth connections.",
            Tags = ["network", "throughput", "tcp", "bandwidth"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpWindowSize", 16776960), RegOp.SetDword(TcpParams, "GlobalMaxTcpWindowSize", 16776960)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpWindowSize"), RegOp.DeleteValue(TcpParams, "GlobalMaxTcpWindowSize")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpWindowSize", 16776960)],
        },
        new TweakDef
        {
            Id = "netopt-increase-max-connections",
            Label = "Increase Max TCP Connections per Server",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases max simultaneous TCP connections beyond the default. Improves download managers and web scraping.",
            Tags = ["network", "tcp", "connections", "throughput"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe",
                    10
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\MAIN\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe",
                    10
                ),
            ],
        },
        new TweakDef
        {
            Id = "netopt-disable-network-throttling",
            Label = "Disable Network Throttling Index",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the Windows network throttling that limits non-multimedia traffic to 10 packets/ms.",
            Tags = ["network", "throughput", "throttling", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "netopt-set-tcp-timestamps",
            Label = "Enable TCP Timestamps (RFC 1323)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables TCP timestamps for improved RTT calculation and protection against wrapped sequence numbers. Better performance on high-bandwidth connections.",
            Tags = ["network", "tcp", "timestamps", "rfc1323"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "Tcp1323Opts", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "Tcp1323Opts")],
            DetectOps = [RegOp.CheckDword(TcpParams, "Tcp1323Opts", 1)],
        },
        new TweakDef
        {
            Id = "netopt-set-dns-cloudflare",
            Label = "Set DNS to Cloudflare (1.1.1.1 + DoH)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Sets all active network adapters to use Cloudflare DNS (1.1.1.1, 1.0.0.1) with DNS-over-HTTPS enabled.",
            Tags = ["network", "dns", "cloudflare", "privacy", "doh"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter | Where-Object Status -eq 'Up' | ForEach-Object { "
                        + "Set-DnsClientServerAddress -InterfaceIndex $_.ifIndex -ServerAddresses @('1.1.1.1','1.0.0.1','2606:4700:4700::1111','2606:4700:4700::1001') }; "
                        + "Set-DnsClientDohServerAddress -ServerAddress 1.1.1.1 -DohTemplate 'https://cloudflare-dns.com/dns-query' -AllowFallbackToUdp $true -AutoUpgrade $true -ErrorAction SilentlyContinue; "
                        + "Set-DnsClientDohServerAddress -ServerAddress 1.0.0.1 -DohTemplate 'https://cloudflare-dns.com/dns-query' -AllowFallbackToUdp $true -AutoUpgrade $true -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter | Where-Object Status -eq 'Up' | ForEach-Object { "
                        + "Set-DnsClientServerAddress -InterfaceIndex $_.ifIndex -ResetServerAddresses }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "$dns = (Get-DnsClientServerAddress -AddressFamily IPv4 | Where-Object ServerAddresses -Contains '1.1.1.1'); $null -ne $dns"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "netopt-set-dns-google",
            Label = "Set DNS to Google (8.8.8.8 + DoH)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Sets all active network adapters to use Google DNS (8.8.8.8, 8.8.4.4).",
            Tags = ["network", "dns", "google", "doh"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter | Where-Object Status -eq 'Up' | ForEach-Object { "
                        + "Set-DnsClientServerAddress -InterfaceIndex $_.ifIndex -ServerAddresses @('8.8.8.8','8.8.4.4','2001:4860:4860::8888','2001:4860:4860::8844') }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter | Where-Object Status -eq 'Up' | ForEach-Object { "
                        + "Set-DnsClientServerAddress -InterfaceIndex $_.ifIndex -ResetServerAddresses }"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "$dns = (Get-DnsClientServerAddress -AddressFamily IPv4 | Where-Object ServerAddresses -Contains '8.8.8.8'); $null -ne $dns"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "netopt-disable-ipv6",
            Label = "Disable IPv6 on All Adapters",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            KindHint = TweakKind.PowerShell,
            Description = "Disables IPv6 on all network adapters. Some ISPs don't support IPv6, causing latency from failed lookups.",
            Tags = ["network", "ipv6", "latency", "performance"],
            SideEffects = "IPv6-only services will be unreachable.",
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapterBinding -ComponentID ms_tcpip6 -ErrorAction SilentlyContinue | "
                        + "Disable-NetAdapterBinding -ComponentID ms_tcpip6 -ErrorAction SilentlyContinue"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapterBinding -ComponentID ms_tcpip6 -ErrorAction SilentlyContinue | "
                        + "Enable-NetAdapterBinding -ComponentID ms_tcpip6 -ErrorAction SilentlyContinue"
                ),
            DetectAction = () =>
            {
                var (_, stdout, _) = ShellRunner.RunPowerShell(
                    "(Get-NetAdapterBinding -ComponentID ms_tcpip6 -ErrorAction SilentlyContinue | Where-Object Enabled -eq $true).Count -eq 0"
                );
                return stdout.Trim().Equals("True", StringComparison.OrdinalIgnoreCase);
            },
        },
        new TweakDef
        {
            Id = "netopt-disable-lmhosts-lookup",
            Label = "Disable LMHOSTS Lookup",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables LMHOSTS file lookup for NetBIOS name resolution. Reduces unnecessary DNS overhead.",
            Tags = ["network", "netbios", "performance", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 1)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "netopt-disable-qos-packet-scheduler",
            Label = "Remove QoS Bandwidth Reservation",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets QoS reserved bandwidth to 0% instead of the default 20% reservation.",
            Tags = ["network", "bandwidth", "qos", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Psched"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "netopt-disable-adapter-power-save",
            Label = "Disable Network Adapter Power Saving",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Prevents Windows from turning off network adapters to save power. Fixes connection drops after sleep.",
            Tags = ["network", "power", "adapter", "stability"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Disabled -WakeOnPattern Disabled -ErrorAction SilentlyContinue; "
                        + "$dev = Get-PnpDevice -FriendlyName $_.InterfaceDescription -ErrorAction SilentlyContinue; "
                        + "if ($dev) { "
                        + "$instance = $dev.InstanceId; "
                        + "$path = \"HKLM:\\SYSTEM\\CurrentControlSet\\Enum\\$instance\\Device Parameters\\Power\"; "
                        + "if (Test-Path $path) { Set-ItemProperty $path -Name 'AllowIdleIrpInD3' -Value 0 -ErrorAction SilentlyContinue } } }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Enabled -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false, // Complex detection across adapters
        },
        new TweakDef
        {
            Id = "netopt-flush-arp-cache",
            Label = "Flush ARP Cache",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Clears the ARP cache table. Resolves stale IP-to-MAC address mappings after network changes.",
            Tags = ["network", "arp", "cache", "maintenance"],
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["interface", "ip", "delete", "arpcache"]),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-reset-winsock",
            Label = "Reset Winsock Catalog (Network Fix)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.SystemCommand,
            Description = "Resets the Winsock catalog to a clean state. Fixes network connectivity issues caused by malware or broken LSPs.",
            Tags = ["network", "winsock", "repair", "maintenance"],
            SideEffects = "Requires reboot. Some VPN/proxy software may need reconfiguration.",
            ApplyAction = _ => ShellRunner.Run("netsh.exe", ["winsock", "reset"]),
            RemoveAction = _ => { },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-enable-large-send-offload",
            Label = "Optimise NIC Offload Settings",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Enables TCP/UDP checksum offload and large send offload on all physical adapters for reduced CPU usage.",
            Tags = ["network", "offload", "performance", "nic"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { "
                        + "Enable-NetAdapterChecksumOffload -Name $_.Name -ErrorAction SilentlyContinue; "
                        + "Enable-NetAdapterLso -Name $_.Name -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { " + "Disable-NetAdapterLso -Name $_.Name -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-enable-dns-cache-boost",
            Label = "Increase DNS Client Cache Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the DNS client cache limits for faster repeated lookups.",
            Tags = ["network", "dns", "cache", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 5),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit"),
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
        },
        new TweakDef
        {
            Id = "netopt-enable-tcp-fast-open",
            Label = "Enable TCP Fast Open (TFO)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables TCP Fast Open to reduce connection latency by sending data in the SYN packet.",
            Tags = ["network", "tcp", "latency", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableTFO", 3)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableTFO")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableTFO", 3)],
        },
        new TweakDef
        {
            Id = "netopt-disable-tcp-slow-start",
            Label = "Disable TCP Slow Start After Idle",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Prevents TCP from resetting congestion window after idle, keeping throughput high on persistent connections.",
            Tags = ["network", "tcp", "throughput", "performance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Set-NetTCPSetting -SettingName InternetCustom -CongestionProvider CTCP; netsh int tcp set supplemental Template=InternetCustom"
                ),
            RemoveAction = _ => ShellRunner.RunPowerShell("Set-NetTCPSetting -SettingName InternetCustom -CongestionProvider Default"),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-increase-arp-cache",
            Label = "Increase ARP Cache Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the ARP cache limit from default 256 to 4096 entries for networks with many peers.",
            Tags = ["network", "arp", "cache", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheSize", 4096),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheLife", 300),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheSize"),
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheLife"),
            ],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "ArpCacheSize", 4096)],
        },
        new TweakDef
        {
            Id = "netopt-enable-rsc",
            Label = "Enable Receive Segment Coalescing (RSC)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Enables Receive Segment Coalescing on all physical adaptors to reduce CPU overhead for high-throughput scenarios.",
            Tags = ["network", "rsc", "nic", "performance"],
            ApplyAction = _ => ShellRunner.RunPowerShell("Get-NetAdapter -Physical | Enable-NetAdapterRsc -IPv4 -ErrorAction SilentlyContinue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("Get-NetAdapter -Physical | Disable-NetAdapterRsc -IPv4 -ErrorAction SilentlyContinue"),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-enable-direct-cache-access",
            Label = "Enable Direct Cache Access (DCA)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Direct Cache Access so NIC data is placed directly into CPU cache, reducing memory latency.",
            Tags = ["network", "nic", "dca", "performance"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDCA", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDCA")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "EnableDCA", 1)],
        },
        new TweakDef
        {
            Id = "netopt-increase-tcp-max-connections",
            Label = "Increase Maximum TCP Connections Per Server",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the maximum allowed half-open TCP connections from default 10 to 65534 for high-throughput workloads.",
            Tags = ["network", "tcp", "connections", "throughput"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxHalfOpen", 65534)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxHalfOpen")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "TcpMaxHalfOpen", 65534)],
        },
        new TweakDef
        {
            Id = "netopt-increase-tcp-keepalive",
            Label = "Reduce TCP Keep-Alive Interval",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the TCP keep-alive interval from 2 hours to 30 minutes, detecting dead connections faster.",
            Tags = ["network", "tcp", "keepalive", "reliability"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 1800000)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 1800000)],
        },
        new TweakDef
        {
            Id = "netopt-enable-flow-control",
            Label = "Enable NIC Flow Control",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Enables IEEE 802.3x flow control on physical adapters for buffer overflow prevention.",
            Tags = ["network", "nic", "flow-control", "performance"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { Set-NetAdapterAdvancedProperty -Name $_.Name -RegistryKeyword '*FlowControl' -RegistryValue 3 -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { Set-NetAdapterAdvancedProperty -Name $_.Name -RegistryKeyword '*FlowControl' -RegistryValue 0 -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-disable-power-management-nic",
            Label = "Disable NIC Power Management (Prevent Sleep)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Disables power management on all physical network adapters so they don't disconnect during sleep transitions.",
            Tags = ["network", "nic", "power", "reliability"],
            ApplyAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Disabled -WakeOnPattern Disabled -ErrorAction SilentlyContinue }"
                ),
            RemoveAction = _ =>
                ShellRunner.RunPowerShell(
                    "Get-NetAdapter -Physical | ForEach-Object { Set-NetAdapterPowerManagement -Name $_.Name -WakeOnMagicPacket Enabled -WakeOnPattern Enabled -ErrorAction SilentlyContinue }"
                ),
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "netopt-set-dns-priority-ipv4",
            Label = "Prioritise IPv4 DNS over IPv6",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the prefix policy to prefer IPv4 DNS resolution, reducing lookup latency on networks without native IPv6.",
            Tags = ["network", "dns", "ipv4", "ipv6"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
        },
        new TweakDef
        {
            Id = "netopt-increase-arp-cache-size",
            Label = "Increase ARP Cache Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the ARP table cache from default 256 to 4096 entries. Reduces ARP lookup latency on busy networks.",
            Tags = ["network", "arp", "cache", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "ArpCacheSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "ArpCacheSize")],
            DetectOps = [RegOp.CheckDword(TcpParams, "ArpCacheSize", 4096)],
        },
        new TweakDef
        {
            Id = "netopt-set-max-connections-per-server",
            Label = "Increase Max Connections Per Server",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets maximum concurrent connections per server from default 2 to 16. Improves parallel download speed.",
            Tags = ["network", "connections", "http", "speed"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER"],
            ApplyOps =
            [
                RegOp.SetDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe",
                    16
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    $@"{LmKey}\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_MAXCONNECTIONSPERSERVER",
                    "iexplore.exe",
                    16
                ),
            ],
        },
        new TweakDef
        {
            Id = "netopt-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NetBIOS over TCP/IP. Reduces network attack surface and broadcast traffic. May break legacy file sharing.",
            Tags = ["network", "netbios", "security", "disable"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NetbiosOptions", 2)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NetbiosOptions", 0)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NetbiosOptions", 2)],
        },
        new TweakDef
        {
            Id = "netopt-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense Auto-Connect",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Wi-Fi Sense that auto-connects to suggested open hotspots and shared networks. Improves security and prevents unwanted connections.",
            Tags = ["network", "wifi", "sense", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "netopt-set-dns-cache-max-ttl",
            Label = "Increase DNS Cache Max TTL to 1 Day",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the maximum DNS cache TTL to 86400 seconds (1 day). Reduces DNS queries for frequently visited sites.",
            Tags = ["network", "dns", "cache", "ttl"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "netopt-set-dns-negative-cache-ttl",
            Label = "Reduce DNS Negative Cache TTL",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the negative DNS cache TTL from 900s to 60s. Failed DNS lookups are retried sooner.",
            Tags = ["network", "dns", "cache", "negative"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 60)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 60)],
        },
        new TweakDef
        {
            Id = "netopt-disable-wpad",
            Label = "Disable Web Proxy Auto-Discovery (WPAD)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables WPAD protocol used for automatic proxy configuration. Removes a known security risk. Not recommended on corporate networks.",
            Tags = ["network", "wpad", "proxy", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "netopt-enable-rss",
            Label = "Enable Receive Side Scaling (RSS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables receive-side scaling to distribute network processing across multiple CPU cores. Improves throughput on multi-core systems.",
            Tags = ["network", "rss", "multicore", "throughput"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "EnableRSS", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "EnableRSS")],
            DetectOps = [RegOp.CheckDword(TcpParams, "EnableRSS", 1)],
        },
        new TweakDef
        {
            Id = "netopt-disable-smb-bandwidth-throttling",
            Label = "Disable SMB Bandwidth Throttling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes large-file SMB bandwidth throttling. Allows file copies over network shares to use full bandwidth.",
            Tags = ["network", "smb", "bandwidth", "fileserver"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "DisableBandwidthThrottling", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "DisableBandwidthThrottling")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters", "DisableBandwidthThrottling", 1),
            ],
        },
        new TweakDef
        {
            Id = "netopt-set-max-user-port",
            Label = "Increase Max Ephemeral Port Range",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the ephemeral port range to 65534. Prevents port exhaustion under high connection loads.",
            Tags = ["network", "ports", "ephemeral", "connections"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "MaxUserPort", 65534)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "MaxUserPort")],
            DetectOps = [RegOp.CheckDword(TcpParams, "MaxUserPort", 65534)],
        },
        new TweakDef
        {
            Id = "netopt-set-default-ttl",
            Label = "Set Default IPv4 TTL to 64",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Explicitly sets DefaultTTL to 64, matching Linux/macOS defaults. Reduces unnecessary router hops and normalises TTL fingerprinting signatures.",
            Tags = ["network", "tcp", "ttl", "performance", "privacy"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "DefaultTTL", 64)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "DefaultTTL")],
            DetectOps = [RegOp.CheckDword(TcpParams, "DefaultTTL", 64)],
        },
        new TweakDef
        {
            Id = "netopt-enable-tcp-1323-opts",
            Label = "Enable TCP Window Scaling and Timestamps",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Tcp1323Opts=3 to enable both RFC 1323 large window scaling and TCP timestamps. Allows gigabit-scale receive windows and improves round-trip measurements.",
            Tags = ["network", "tcp", "window-scaling", "timestamps", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "Tcp1323Opts", 3)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "Tcp1323Opts")],
            DetectOps = [RegOp.CheckDword(TcpParams, "Tcp1323Opts", 3)],
        },
        new TweakDef
        {
            Id = "netopt-set-global-max-tcp-window",
            Label = "Set Global Maximum TCP Window Size to 8 MB",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets GlobalMaxTcpWindowSize to 8 388 608 bytes (8 MB). Caps the global TCP receive window ceiling, allowing the auto-tuning algorithm room to grow on high-bandwidth connections.",
            Tags = ["network", "tcp", "window", "bandwidth", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "GlobalMaxTcpWindowSize", 8388608)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "GlobalMaxTcpWindowSize")],
            DetectOps = [RegOp.CheckDword(TcpParams, "GlobalMaxTcpWindowSize", 8388608)],
        },
        new TweakDef
        {
            Id = "netopt-enable-pmtu-discovery",
            Label = "Enable Path MTU Discovery",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures EnablePMTUDiscovery=1 is set. Path MTU Discovery allows TCP to negotiate the largest possible packet size along a route, reducing fragmentation overhead.",
            Tags = ["network", "tcp", "mtu", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "EnablePMTUDiscovery", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "EnablePMTUDiscovery")],
            DetectOps = [RegOp.CheckDword(TcpParams, "EnablePMTUDiscovery", 1)],
        },
        new TweakDef
        {
            Id = "netopt-disable-pmtu-blackhole-detect",
            Label = "Disable Path MTU Blackhole Detection",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets EnablePMTUBHDetect=0. The blackhole-detection workaround can reduce the window size unnecessarily on modern networks; disabling it keeps PMTU working efficiently.",
            Tags = ["network", "tcp", "mtu", "blackhole", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "EnablePMTUBHDetect", 0)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "EnablePMTUBHDetect")],
            DetectOps = [RegOp.CheckDword(TcpParams, "EnablePMTUBHDetect", 0)],
        },
        new TweakDef
        {
            Id = "netopt-set-max-half-open",
            Label = "Limit Maximum Half-Open TCP Connections",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TcpMaxHalfOpen to 100. Caps the number of simultaneous half-open (SYN-sent) connections, complementing SYN-flood attack protection.",
            Tags = ["network", "tcp", "syn", "security", "connections"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpMaxHalfOpen", 100)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpMaxHalfOpen")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpMaxHalfOpen", 100)],
        },
        new TweakDef
        {
            Id = "netopt-set-max-half-open-retried",
            Label = "Limit Maximum Retried Half-Open TCP Connections",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TcpMaxHalfOpenRetried to 80. Once the half-open count exceeds this, the TCP stack starts dropping the oldest half-open connections.",
            Tags = ["network", "tcp", "syn", "security", "connections"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpMaxHalfOpenRetried", 80)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpMaxHalfOpenRetried")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpMaxHalfOpenRetried", 80)],
        },
        new TweakDef
        {
            Id = "netopt-set-tcp-max-send-free",
            Label = "Increase TCP Send Window Free Space",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TcpMaxSendFree to 65 536 bytes. Provides extra headroom in the TCP send buffer pool, reducing stalls on burst-sending applications.",
            Tags = ["network", "tcp", "send-buffer", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpMaxSendFree", 65536)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpMaxSendFree")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpMaxSendFree", 65536)],
        },
        new TweakDef
        {
            Id = "netopt-set-delayed-ack-ticks",
            Label = "Reduce TCP Delayed-ACK Tick Count",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TcpDelAckTicks to 1. Lowers the number of clock ticks before a delayed ACK is sent. Reduces latency for interactive protocols such as SSH and RDP.",
            Tags = ["network", "tcp", "ack", "latency", "gaming"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpDelAckTicks", 1)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpDelAckTicks")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpDelAckTicks", 1)],
        },
        new TweakDef
        {
            Id = "netopt-set-dynamic-port-start",
            Label = "Set Dynamic Port Allocation Start to 49152",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MinUserPort to 49152, aligning with the IANA-recommended ephemeral port range (49152–65535). Reserves the range 1024–49151 for server listen ports.",
            Tags = ["network", "ports", "ephemeral", "iana", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "MinUserPort", 49152)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "MinUserPort")],
            DetectOps = [RegOp.CheckDword(TcpParams, "MinUserPort", 49152)],
        },
        new TweakDef
        {
            Id = "netopt-set-tcp-listen-backlog",
            Label = "Increase TCP Listen Backlog",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the AFD ListenBacklog to 200. Increases the queue depth for incoming connection requests before the application accept()s them, reducing connection refusals under burst load.",
            Tags = ["network", "tcp", "listen", "connections", "server"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\AFD\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "ListenBacklog", 200)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "ListenBacklog")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "ListenBacklog", 200)],
        },
        new TweakDef
        {
            Id = "netopt-set-default-mss",
            Label = "Set Default TCP Maximum Segment Size to 1460",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultMSS to 1 460 bytes, the standard Ethernet MSS (MTU 1500 − 40 bytes). Prevents unnecessary TCP fragmentation on Ethernet-based networks.",
            Tags = ["network", "tcp", "mss", "mtu", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "DefaultMSS", 1460)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "DefaultMSS")],
            DetectOps = [RegOp.CheckDword(TcpParams, "DefaultMSS", 1460)],
        },
        new TweakDef
        {
            Id = "netopt-set-max-syn-retransmissions",
            Label = "Reduce TCP SYN Retransmission Limit",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TcpMaxConnectRetransmissions to 2. Unreachable hosts are declared unavailable after 2 SYN retries instead of the default, reducing connection hang time.",
            Tags = ["network", "tcp", "syn", "retransmit", "performance"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "TcpMaxConnectRetransmissions", 2)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "TcpMaxConnectRetransmissions")],
            DetectOps = [RegOp.CheckDword(TcpParams, "TcpMaxConnectRetransmissions", 2)],
        },
    ];
}

// ── Merged from DnsNetworking.cs ──────────────────────────────────────────────────

internal static class DnsNetworking
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "dns-force-doh-policy",
            Label = "Force DNS-over-HTTPS (Policy)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DoHPolicy=3 via Group Policy to require DNS-over-HTTPS. DNS queries that cannot use DoH will fail. Default: not set. Recommended: 3 (require DoH).",
            Tags = ["dns", "doh", "privacy", "encryption", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DoHPolicy", 3)],
        },
        new TweakDef
        {
            Id = "dns-disable-mdns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables mDNS responder (EnableMDNS=0). Reduces network chatter and attack surface on enterprise networks. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "mdns", "security", "network", "multicast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-name-resolution",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from sending DNS queries to all adapters simultaneously. Stops DNS leaks on VPN split-tunnel setups. Default: not configured. Recommended: 1 (disabled).",
            Tags = ["dns", "privacy", "vpn", "network", "leak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-devolution",
            Label = "Disable DNS Devolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables DNS suffix devolution (stripping sub-domain labels). Prevents unintended DNS queries to parent domains. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "security", "network", "enterprise"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-lmhosts",
            Label = "Disable LMHOSTS Lookup",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables LMHOSTS file lookup for NetBIOS name resolution. Reduces attack surface and legacy protocol overhead. Default: 1 (enabled). Recommended: 0 (disabled).",
            Tags = ["dns", "netbios", "lmhosts", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-qos-reserve",
            Label = "Remove QoS Reserved Bandwidth",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets QoS non-best-effort bandwidth limit to 0%, reclaiming the 20% Windows reserves by default. Default: 20. Recommended: 0.",
            Tags = ["dns", "qos", "bandwidth", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Psched", "NonBestEffortLimit", 0)],
        },
        new TweakDef
        {
            Id = "dns-increase-socket-buffers",
            Label = "Increase Socket Buffer Sizes (256 KB)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases default receive/send socket buffers from 64 KB to 256 KB for better throughput on fast connections. Default: 65535. Recommended: 262144.",
            Tags = ["dns", "socket", "tcp", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 262144),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow", 262144),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultSendWindow"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters", "DefaultReceiveWindow", 262144)],
        },
        new TweakDef
        {
            Id = "dns-tcp-keepalive-tuning",
            Label = "Tune TCP Keep-Alive Intervals",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets TCP KeepAliveTime to 5 minutes and KeepAliveInterval to 1 second for faster detection of dead connections. Default: KeepAliveTime=7200000 (2h). Recommended: 300000 (5m).",
            Tags = ["dns", "tcp", "keepalive", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveInterval", 1000),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveInterval"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "KeepAliveTime", 300000)],
        },
        new TweakDef
        {
            Id = "dns-disable-ncsi-probes",
            Label = "Disable NCSI Active Probing",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Network Connectivity Status Indicator probes to Microsoft servers. Improves privacy but may affect captive portal detection. Default: not set. Recommended: 1 (disabled).",
            Tags = ["dns", "ncsi", "privacy", "network", "probe"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1),
            ],
        },
        new TweakDef
        {
            Id = "dns-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast Name Resolution)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables LLMNR to prevent local name-resolution poisoning attacks. LLMNR responds to multicast queries on the local subnet and can be exploited for credential relay. Default: enabled. Recommended: disabled.",
            Tags = ["dns", "llmnr", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets NetBT NodeType to 2 (P-node, point-to-point only) to disable broadcast-based NetBIOS name resolution. Mitigates NBNS spoofing. Default: 0 (B-node broadcast). Recommended: 2 (P-node).",
            Tags = ["dns", "netbios", "security", "network", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-disable-wpad",
            Label = "Disable WPAD Auto-Discovery",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Web Proxy Auto-Discovery (WPAD) protocol to prevent automatic proxy configuration and WPAD poisoning attacks. Default: Enabled. Recommended: Disabled for security.",
            Tags = ["dns", "wpad", "proxy", "security", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableWpad", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-doh",
            Label = "Enable DNS-over-HTTPS",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNS-over-HTTPS (DoH) for encrypted DNS resolution. Requires a DoH-capable DNS server. Default: plaintext DNS.",
            Tags = ["dns", "doh", "encrypted", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
        },
        new TweakDef
        {
            Id = "dns-set-cache-size-512",
            Label = "Increase DNS Cache Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the DNS resolver cache maximum entry count from default to 512. Reduces repeat DNS lookups. Default: system-managed.",
            Tags = ["dns", "cache", "size", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheEntryTtlLimit", 86400),
            ],
        },
        new TweakDef
        {
            Id = "dns-disable-multicast-dns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables mDNS (Bonjour-style) name resolution. Reduces network chatter on enterprise networks. Default: enabled.",
            Tags = ["dns", "mdns", "multicast", "bonjour"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-negative-cache-ttl",
            Label = "Reduce Negative DNS Cache TTL",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces the time negative DNS responses (NXDOMAIN) are cached to 5 seconds. Useful for DNS failover. Default: 5 minutes.",
            Tags = ["dns", "cache", "negative", "ttl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios-over-tcp",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables NetBIOS over TCP/IP. Eliminates NBT broadcasts and reduces attack surface. May break legacy apps. Default: enabled.",
            Tags = ["dns", "netbios", "tcp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-disable-ipv6-transition",
            Label = "Disable IPv6 Transition Technologies",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables IPv6 transition technologies (6to4, Teredo, ISATAP). Reduces attack surface and simplifies networking. Default: enabled.",
            Tags = ["dns", "ipv6", "transition", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-negative-cache",
            Label = "Disable DNS Negative Cache",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DNS negative response caching. Failed lookups are retried immediately instead of being cached. Useful for development. Default: cached.",
            Tags = ["dns", "negative", "cache", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxNegativeCacheTtl", 0)],
        },
        new TweakDef
        {
            Id = "dns-increase-cache-entry-ttl",
            Label = "Increase DNS Cache TTL",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the DNS cache entry TTL to 86400 seconds (24 hours). Reduces DNS lookup frequency. Improves browsing speed. Default: server-defined.",
            Tags = ["dns", "cache", "ttl", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "dns-reduce-query-timeout",
            Label = "Reduce DNS Query Timeout",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces DNS query timeout to 2 seconds. Faster fallback to alternate DNS servers on poor connectivity. Default: varies.",
            Tags = ["dns", "query", "timeout", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval", 2000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "QueryRetryInterval", 2000)],
        },
        new TweakDef
        {
            Id = "dns-disable-multicast-name-resolution",
            Label = "Disable Multicast Name Resolution (LLMNR)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution (LLMNR). Prevents LLMNR poisoning attacks. Default: enabled.",
            Tags = ["dns", "llmnr", "multicast", "security", "poisoning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-enable-doh-require",
            Label = "Require DNS over HTTPS (DoH)",
            Category = "Network",
            NeedsAdmin = true,
            Description = "Requires all DNS queries to use DNS over HTTPS. Falls back to no resolution if DoH is unavailable. Default: disabled.",
            Tags = ["dns", "doh", "https", "privacy", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 3)],
        },
        new TweakDef
        {
            Id = "dns-disable-name-devolution-policy",
            Label = "Disable DNS Devolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables DNS name devolution (searching parent domains). Prevents unintended DNS queries to parent zones. Default: enabled.",
            Tags = ["dns", "devolution", "security", "domain"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables NetBIOS name resolution over TCP/IP. Mitigates NBT-NS poisoning attacks. Default: enabled.",
            Tags = ["dns", "netbios", "tcp", "security", "poisoning"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "dns-set-negative-cache-ttl-0",
            Label = "Disable Negative DNS Cache",
            Category = "Network",
            NeedsAdmin = true,
            Description =
                "Sets negative DNS cache TTL to 0, so failed lookups are not cached. Useful for dynamic environments. Default: 900 seconds.",
            Tags = ["dns", "cache", "negative", "ttl"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 0)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-multihomed-policy",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            Description =
                "Disables Smart Multi-Homed Name Resolution which sends DNS queries to all adapters simultaneously. Security risk on VPN. Default: enabled.",
            Tags = ["dns", "smart", "multi-homed", "vpn", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-max-cache-ttl-86400",
            Label = "Limit DNS Cache TTL to 24 Hours",
            Category = "Network",
            NeedsAdmin = true,
            Description = "Caps the DNS cache entry TTL at 86400 seconds (24 hours). Prevents stale DNS entries. Default: unlimited.",
            Tags = ["dns", "cache", "ttl", "staleness"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtl", 86400)],
        },
        new TweakDef
        {
            Id = "dns-disable-parallel-queries",
            Label = "Disable Parallel DNS Queries",
            Category = "Network",
            NeedsAdmin = true,
            Description =
                "Prevents DNS resolver from sending parallel queries across all network adapters. Reduces DNS leakage on VPN. Default: enabled.",
            Tags = ["dns", "parallel", "vpn", "leak", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableParallelAandAAAA", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-dns-cache-locking",
            Label = "Enable DNS Cache Locking at 100%",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Locks cached DNS records for 100% of their TTL, preventing cache poisoning via premature overwrites. Default: 100.",
            Tags = ["dns", "cache", "locking", "poisoning", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent", 100),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxCacheTtlEntryLockingPercent", 100),
            ],
        },
        new TweakDef
        {
            Id = "dns-enable-dnssec-validation",
            Label = "Enable DNSSEC Validation",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNSSEC validation in the Windows DNS client. Verifies DNS response authenticity. Default: disabled.",
            Tags = ["dns", "dnssec", "validation", "security", "integrity"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableDnsSec", 1)],
        },
        // ── Sprint 18 — 10 new DNS & Networking Advanced tweaks ────────────
        new TweakDef
        {
            Id = "dns-disable-llmnr-via-policy",
            Label = "Disable LLMNR via Group Policy",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution (LLMNR) to prevent name resolution poisoning attacks. Default: enabled.",
            Tags = ["dns", "llmnr", "security", "poisoning", "multicast"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-dns-priority-v4",
            Label = "Prioritise IPv4 DNS over IPv6",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures prefix policy to prefer IPv4 name resolution over IPv6, reducing latency on networks without native v6. Default: IPv6 preferred.",
            Tags = ["dns", "ipv4", "ipv6", "priority", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 32)],
        },
        new TweakDef
        {
            Id = "dns-disable-smart-protocol-reorder",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents Windows from sending DNS queries to all adapters simultaneously. Reduces DNS leaks on VPN connections. Default: enabled.",
            Tags = ["dns", "multi-homed", "vpn", "leak", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-dns-filtering-platform",
            Label = "Enable DNS Client Diagnostic Logging",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables analytic logging in the DNS client for troubleshooting resolution issues. Default: disabled.",
            Tags = ["dns", "logging", "diagnostics", "troubleshooting"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableLogging", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-wins-resolution",
            Label = "Disable WINS Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables legacy WINS/NBNS name resolution in the TCP/IP stack. Reduces NetBIOS attack surface. Default: enabled.",
            Tags = ["dns", "wins", "netbios", "legacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "EnableLMHOSTS", 0)],
        },
        new TweakDef
        {
            Id = "dns-set-max-negative-cache-ttl",
            Label = "Limit Negative DNS Cache TTL to 5 Seconds",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits how long failed DNS lookups are cached. Faster recovery after DNS changes or outages. Default: 900s (15 min).",
            Tags = ["dns", "cache", "negative-cache", "ttl", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "NegativeCacheTime", 5)],
        },
        new TweakDef
        {
            Id = "dns-disable-devolution-fallback",
            Label = "Disable DNS Devolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Prevents Windows from walking up the domain suffix hierarchy when resolving names. Reduces information leakage. Default: enabled.",
            Tags = ["dns", "devolution", "suffix", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "UseDomainNameDevolution", 0)],
        },
        new TweakDef
        {
            Id = "dns-force-fqdn-only",
            Label = "Require Fully Qualified Domain Names",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Forces the DNS client to only resolve fully qualified domain names. Prevents unqualified name lookups. Default: not required.",
            Tags = ["dns", "fqdn", "security", "resolution", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "QueryAdapterName", 1)],
        },
        new TweakDef
        {
            Id = "dns-enable-query-logging",
            Label = "Enable DNS Query ETW Logging",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables ETW-based DNS query logging for security monitoring and forensics. Default: disabled.",
            Tags = ["dns", "logging", "etw", "monitoring", "forensics"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableDnsQueryLogging", 1)],
        },
        new TweakDef
        {
            Id = "dns-disable-parallel-adapter-queries",
            Label = "Disable Parallel DNS Queries Across Adapters",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents sending the same DNS query across multiple network adapters simultaneously. Reduces traffic and VPN DNS leaks. Default: enabled.",
            Tags = ["dns", "parallel", "adapters", "vpn", "leak"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableParallelAandAAAA", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-socket-pool-size",
            Label = "Increase DNS Socket Pool Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the DNS resolver socket pool to 2 500 sockets (CERT-recommended). A larger pool randomises source ports, mitigating DNS cache-poisoning attacks.",
            Tags = ["dns", "security", "cache-poisoning", "socket", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize", 2500)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "SocketPoolSize", 2500)],
        },
        new TweakDef
        {
            Id = "dns-disable-ptr-registration",
            Label = "Disable Reverse-Address (PTR) Registration",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the DNS client from dynamically registering PTR (reverse lookup) records. Reduces DNS noise and avoids exposing the hostname via reverse lookups.",
            Tags = ["dns", "ptr", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableReverseAddressRegistrations", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "DisableReverseAddressRegistrations"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters",
                    "DisableReverseAddressRegistrations",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "dns-gpo-disable-dynamic-registration",
            Label = "Disable DNS Dynamic Update Registration (Policy)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Applies the DNSClient GPO policy to disable dynamic DNS registration. The workstation will not automatically update its A or AAAA records in DNS.",
            Tags = ["dns", "dynamic-update", "gpo", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableDynamicUpdate", 1)],
        },
        new TweakDef
        {
            Id = "dns-set-max-udp-datagram",
            Label = "Increase Max DNS UDP Datagram Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Raises MaximumUdpDatagramSize to 4 096 bytes. Required to receive full DNSSEC-signed responses over UDP without falling back to TCP.",
            Tags = ["dns", "dnssec", "udp", "performance", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumUdpDatagramSize", 4096),
            ],
        },
        new TweakDef
        {
            Id = "dns-set-server-priority-limit",
            Label = "Reduce DNS Server Priority Timeout",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets ServerPriorityTimeLimit to 400 ms. The resolver will try the next server in its list sooner when the current preferred server is slow.",
            Tags = ["dns", "timeout", "performance", "server-priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit", 400)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "ServerPriorityTimeLimit", 400),
            ],
        },
        new TweakDef
        {
            Id = "dns-set-cache-hash-table-size",
            Label = "Increase DNS Cache Hash Table Size",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets CacheHashTableSize to 4 096 buckets. A larger hash table reduces collision chains in the DNS cache, improving cache lookup speed under heavy load.",
            Tags = ["dns", "cache", "performance", "hash-table"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize", 4096)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "CacheHashTableSize", 4096)],
        },
        new TweakDef
        {
            Id = "dns-set-max-hostname-ttl",
            Label = "Cap Hostname Cache TTL at 1 Hour",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxHostnameTtl to 3 600 seconds (1 hour). Prevents the DNS cache from holding stale hostname entries for excessively long periods.",
            Tags = ["dns", "cache", "ttl", "hostname"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl", 3600)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxHostnameTtl", 3600)],
        },
        new TweakDef
        {
            Id = "dns-set-address-query-timeout",
            Label = "Cap DNS Address Query Timeout",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MaxAddressQueryTimeout to 30 000 ms. Prevents the resolver from waiting indefinitely for an address record response.",
            Tags = ["dns", "timeout", "query", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaxAddressQueryTimeout", 30000),
            ],
        },
        new TweakDef
        {
            Id = "dns-disable-adapter-name-reg",
            Label = "Disable Per-Adapter Name DNS Registration",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RegisterAdapterName=0 to stop the DNS client from registering individual adapter names. Reduces DNS record clutter on multi-homed hosts.",
            Tags = ["dns", "registration", "adapter", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "RegisterAdapterName", 0)],
        },
        new TweakDef
        {
            Id = "dns-gpo-disable-ptr-update",
            Label = "Disable PTR Record Registration (Policy)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Applies the DNSClient GPO policy to disable reverse-address (PTR) record registration. Provides a policy-enforced version of the Dnscache parameter equivalent.",
            Tags = ["dns", "ptr", "gpo", "privacy", "registration"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableReverseAddressRegistrations", 1),
            ],
        },
        new TweakDef
        {
            Id = "dns-set-update-security-level",
            Label = "Require Secure DNS Dynamic Updates",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets UpdateSecurityLevel=256 so the DNS client only attempts GSS-API authenticated (secure) dynamic updates. Prevents unauthenticated update attempts on Active Directory-integrated zones.",
            Tags = ["dns", "security", "update", "gss", "ad"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel", 256)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "UpdateSecurityLevel", 256)],
        },
        new TweakDef
        {
            Id = "dns-set-max-dynamic-backoff",
            Label = "Cap DNS Dynamic Update Backoff Interval",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaximumDynamicBackoff to 20 000 ms. Stops the DNS client from waiting more than 20 s between retry attempts for failed dynamic updates.",
            Tags = ["dns", "dynamic-update", "backoff", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff", 20000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "MaximumDynamicBackoff", 20000),
            ],
        },
    ];
}

// === Merged from: ProxyVpn.cs ===

/// <summary>
/// Proxy, VPN, and network tunneling tweaks — configures system proxy settings,
/// VPN auto-connect behaviour, and WinHTTP/WinINet proxy policies.
/// </summary>
internal static class ProxyVpn
{
    private const string CuKey = @"HKEY_CURRENT_USER";
    private const string LmKey = @"HKEY_LOCAL_MACHINE";
    private const string InetKey = $@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings";
    private const string VpnKey = $@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "proxy-disable-auto-detect",
            Label = "Disable Proxy Auto-Detect (WPAD)",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables WPAD (Web Proxy Auto-Discovery Protocol). Speeds up network connections but may break corporate proxy.",
            Tags = ["proxy", "network", "wpad", "performance"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "AutoDetect", 0)],
            RemoveOps = [RegOp.SetDword(InetKey, "AutoDetect", 1)],
            DetectOps = [RegOp.CheckDword(InetKey, "AutoDetect", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-proxy-server",
            Label = "Disable Manual Proxy Server",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Disables the manual proxy server setting, ensuring direct internet connections.",
            Tags = ["proxy", "network", "direct"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "ProxyEnable", 0)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "ProxyEnable")],
            DetectOps = [RegOp.CheckDword(InetKey, "ProxyEnable", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ncsi-active-probing",
            Label = "Disable NCSI Active Probing",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Network Connectivity Status Indicator active probing to Microsoft servers. May show 'No Internet' warning.",
            Tags = ["proxy", "network", "ncsi", "privacy", "telemetry"],
            RegistryKeys = [VpnKey],
            ApplyOps = [RegOp.SetDword(VpnKey, "NoActiveProbe", 1)],
            RemoveOps = [RegOp.DeleteValue(VpnKey, "NoActiveProbe")],
            DetectOps = [RegOp.CheckDword(VpnKey, "NoActiveProbe", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ipv6-transition",
            Label = "Disable IPv6 Transition Technologies",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Teredo, ISATAP, and 6to4 IPv6 tunneling protocols. Reduces attack surface and fixes VPN leaks.",
            Tags = ["proxy", "network", "ipv6", "tunnel", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 255)],
        },
        new TweakDef
        {
            Id = "proxy-disable-smart-multi-homed",
            Label = "Disable Smart Multi-Homed Name Resolution",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables DNS queries across all network adapters simultaneously. Prevents DNS leak over VPN connections.",
            Tags = ["proxy", "vpn", "dns", "leak", "privacy", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "DisableSmartNameResolution", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-llmnr",
            Label = "Disable LLMNR (Link-Local Multicast)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Link-Local Multicast Name Resolution. Mitigates LLMNR poisoning attacks on local networks.",
            Tags = ["proxy", "network", "llmnr", "security", "poisoning"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient", "EnableMulticast", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables NetBIOS over TCP/IP globally. Reduces attack surface and prevents NetBIOS name resolution over VPN.",
            Tags = ["proxy", "network", "netbios", "security", "vpn"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\NetBT\Parameters", "NodeType", 2)],
        },
        new TweakDef
        {
            Id = "proxy-set-winhttp-timeout",
            Label = "Set WinHTTP Connection Timeout (30s)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets WinHTTP connection timeout to 30 seconds. Reduces delay when proxy is unavailable.",
            Tags = ["proxy", "network", "timeout", "winhttp", "performance"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\WinHttp"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\WinHttp", "ConnectTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\WinHttp", "ConnectTimeout")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\Windows\CurrentVersion\Internet Settings\WinHttp", "ConnectTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "proxy-disable-insecure-fallback",
            Label = "Disable Insecure TLS Fallback",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables fallback to insecure SSL 2.0/3.0 and TLS 1.0/1.1 protocols. Forces modern TLS 1.2+.",
            Tags = ["proxy", "security", "tls", "ssl", "encryption"],
            RegistryKeys =
            [
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client",
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client",
            ],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client", "Enabled", 0),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client", "Enabled"),
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 3.0\Client", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\SSL 2.0\Client", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "proxy-disable-web-proxy-auto-config",
            Label = "Disable PAC File Auto-Configuration",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Removes automatic proxy configuration script (PAC) URL. Prevents proxy hijacking.",
            Tags = ["proxy", "network", "pac", "security"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetString(InetKey, "AutoConfigURL", "")],
            RemoveOps = [RegOp.DeleteValue(InetKey, "AutoConfigURL")],
            DetectOps = [RegOp.CheckString(InetKey, "AutoConfigURL", "")],
        },
        new TweakDef
        {
            Id = "proxy-enable-dns-over-https",
            Label = "Enable DNS over HTTPS (System)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 22000,
            Description = "Enables DNS over HTTPS (DoH) at the system level for encrypted DNS resolution.",
            Tags = ["proxy", "dns", "doh", "encryption", "privacy", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableAutoDoh", 2)],
        },
        new TweakDef
        {
            Id = "proxy-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense (Auto-Connect)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Wi-Fi Sense which automatically connects to open hotspots and shared networks.",
            Tags = ["proxy", "wifi", "network", "security", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-hotspot-2",
            Label = "Disable Hotspot 2.0 / Passpoint",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Hotspot 2.0 (Passpoint) which auto-connects to carrier hotspots.",
            Tags = ["proxy", "wifi", "hotspot", "passpoint", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\WlanSvc\AnqpCache"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\WlanSvc\AnqpCache", "OsuRegistrationStatus", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\WlanSvc\AnqpCache", "OsuRegistrationStatus")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\WlanSvc\AnqpCache", "OsuRegistrationStatus", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-wpad",
            Label = "Disable WPAD Protocol",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Web Proxy Auto-Discovery (WPAD) to prevent automatic proxy detection attacks.",
            Tags = ["proxy", "wpad", "security", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "proxy-enforce-tls12-minimum",
            Label = "Enforce TLS 1.2 as Minimum",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TLS 1.0 and 1.1, requiring TLS 1.2 or higher for all HTTPS connections.",
            Tags = ["proxy", "tls", "security", "encryption", "https"],
            RegistryKeys =
            [
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client",
                $@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client",
            ],
            ApplyOps =
            [
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0),
                RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled"),
                RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.1\Client", "Enabled"),
            ],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.0\Client", "Enabled", 0),
            ],
        },
        new TweakDef
        {
            Id = "proxy-disable-multicast-dns",
            Label = "Disable Multicast DNS (mDNS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables mDNS to prevent local network service discovery and reduce attack surface.",
            Tags = ["proxy", "mdns", "network", "security", "dns"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters", "EnableMDNS", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-network-location-wizard",
            Label = "Disable Network Location Wizard",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents the network location identification wizard from appearing on new networks.",
            Tags = ["proxy", "network", "wizard", "firewall"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "NewNetworkWindowOff", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "NewNetworkWindowOff")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Control\Network\NewNetworkWindowOff", "NewNetworkWindowOff", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-winhttp-autoproxy",
            Label = "Disable WinHTTP Auto-Proxy Discovery",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables automatic proxy server discovery via WPAD/WinHTTP. Prevents malicious WPAD responses from hijacking network traffic.",
            Tags = ["proxy", "wpad", "security", "network"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", "DisableProxyPAC", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", "DisableProxyPAC")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings", "DisableProxyPAC", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ie-proxy-bypass",
            Label = "Disable IE/WinINet Proxy Bypass for Local Addresses",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Removes the default bypass list that skips the proxy for local intranet addresses. Enforces all traffic through the configured proxy.",
            Tags = ["proxy", "wininet", "internet-explorer", "network"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyOverride", string.Empty)],
            RemoveOps = [RegOp.SetString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyOverride", "<local>")],
            DetectOps = [RegOp.CheckString($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "ProxyOverride", string.Empty)],
        },
        new TweakDef
        {
            Id = "proxy-disable-vpn-split-tunneling",
            Label = "Disable VPN Split Tunneling (RAS)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Forces all traffic through the VPN interface by disabling split tunneling in RAS default settings.",
            Tags = ["proxy", "vpn", "ras", "security", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasMan\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasMan\Parameters", "DisableSplitTunneling", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasMan\Parameters", "DisableSplitTunneling")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasMan\Parameters", "DisableSplitTunneling", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ras-autodial",
            Label = "Disable RAS AutoDial Manager",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the RasAuto service which automatically re-dials dropped connections. Reduces background network activity on desktops.",
            Tags = ["proxy", "vpn", "ras", "autodial", "svc"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasAuto"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasAuto", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasAuto", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\RasAuto", "Start", 4)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ipv6-teredo",
            Label = "Disable IPv6 Teredo Tunneling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Teredo IPv4-to-IPv6 tunneling mechanism. Prevents unwanted inbound IPv6 connections.",
            Tags = ["proxy", "ipv6", "teredo", "network", "security"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 0x8E)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters", "DisabledComponents", 0x8E)],
        },
        new TweakDef
        {
            Id = "proxy-disable-connection-auto-tuning",
            Label = "Disable HTTP Auto-Proxy Auto-Configuration",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic proxy configuration (PAC) for HKCU WinINet, preventing accidental proxy configuration changes.",
            Tags = ["proxy", "wininet", "http", "network"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "AutoDetect", 0)],
            RemoveOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "AutoDetect", 1)],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "AutoDetect", 0)],
        },
        new TweakDef
        {
            Id = "proxy-disable-6to4-tunneling",
            Label = "Disable IPv6 6to4 Tunneling",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the 6to4 automatic tunneling adapter which bypasses network filtering and may expose internal resources.",
            Tags = ["proxy", "ipv6", "6to4", "tunnel", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\6to4"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\6to4", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ip-tunnel-adapter",
            Label = "Disable IP-HTTPS Tunneling Adapter",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the IPHTTPS tunneling adapter (ISATAP successor). Not needed on most consumer networks.",
            Tags = ["proxy", "ipv6", "iphttps", "tunnel", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\IpHttps"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\IpHttps", "Start", 4)],
            RemoveOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\IpHttps", "Start", 3)],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\IpHttps", "Start", 4)],
        },
        new TweakDef
        {
            Id = "proxy-disable-network-connectivity-test",
            Label = "Disable Network Connectivity Status Indicator (NCSI)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the NCSI probe that sends HTTP requests to microsoft.com to detect internet connectivity. Reduces background telemetry.",
            Tags = ["proxy", "ncsi", "network", "telemetry", "privacy"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Policies\Microsoft\Windows\NetworkConnectivityStatusIndicator", "NoActiveProbe", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-tcp-timestamps",
            Label = "Disable TCP Timestamps",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TCP timestamp option (RFC 1323). Prevents system uptime fingerprinting via network scanners.",
            Tags = ["proxy", "tcp", "timestamps", "security", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 0)],
        },
        new TweakDef
        {
            Id = "proxy-set-max-conn-per-server",
            Label = "Increase Max Connections Per Server (WinINet)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases MaxConnectionsPerServer to 128 in the HKLM WinINet settings. Allows services using WinINet to open more simultaneous connections to the same origin.",
            Tags = ["proxy", "wininet", "connections", "performance"],
            RegistryKeys = [$@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPerServer", 128)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPerServer")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPerServer", 128)],
        },
        new TweakDef
        {
            Id = "proxy-set-max-conn-per-1-0",
            Label = "Increase Max Connections Per HTTP/1.0 Server (WinINet)",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases MaxConnectionsPer1_0Server to 128. Applies to legacy HTTP/1.0 servers accessed via WinINet.",
            Tags = ["proxy", "wininet", "connections", "http", "performance"],
            RegistryKeys = [$@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPer1_0Server", 128)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPer1_0Server")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "MaxConnectionsPer1_0Server", 128),
            ],
        },
        new TweakDef
        {
            Id = "proxy-enable-https-downgrade-warn",
            Label = "Warn on HTTPS-to-HTTP Downgrade",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables a warning dialog when a redirect downgrades from HTTPS to HTTP. Alerts users to potential MitM or stripping attacks.",
            Tags = ["proxy", "https", "security", "warning", "downgrade"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "WarnOnHTTPSToHTTPRedirect", 1)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "WarnOnHTTPSToHTTPRedirect")],
            DetectOps = [RegOp.CheckDword(InetKey, "WarnOnHTTPSToHTTPRedirect", 1)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ie-first-run",
            Label = "Disable Internet Explorer First-Run Wizard",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableFirstRunCustomize=1 (HKLM) to suppress the IE first-run customisation wizard. Prevents unwanted home-page changes on managed systems.",
            Tags = ["proxy", "ie", "first-run", "debloat"],
            RegistryKeys = [$@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DisableFirstRunCustomize", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DisableFirstRunCustomize")],
            DetectOps =
            [
                RegOp.CheckDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DisableFirstRunCustomize", 1),
            ],
        },
        new TweakDef
        {
            Id = "proxy-enable-ie-dnt",
            Label = "Enable Do-Not-Track Header in Internet Explorer",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets DoNotTrack=1 (HKCU) in IE/Main so Internet Explorer sends the DNT: 1 request header.",
            Tags = ["proxy", "ie", "dnt", "privacy", "tracking"],
            RegistryKeys = [$@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main"],
            ApplyOps = [RegOp.SetDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DoNotTrack", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DoNotTrack")],
            DetectOps = [RegOp.CheckDword($@"{CuKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "DoNotTrack", 1)],
        },
        new TweakDef
        {
            Id = "proxy-enable-ie-zone-change-warning",
            Label = "Warn When Changing Internet Security Zones",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables a warning when a page moves between Internet Explorer security zones (e.g., Internet → Intranet). Surfaces unexpected zone transitions to the user.",
            Tags = ["proxy", "ie", "security-zone", "warning"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "WarnOnChangingZone", 1)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "WarnOnChangingZone")],
            DetectOps = [RegOp.CheckDword(InetKey, "WarnOnChangingZone", 1)],
        },
        new TweakDef
        {
            Id = "proxy-set-ie-receive-timeout",
            Label = "Set Internet Explorer Receive Timeout to 30 s",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets ReceiveTimeout to 30 000 ms in WinINet. Drops stalled HTTP receives after 30 seconds instead of the 5-minute default.",
            Tags = ["proxy", "ie", "wininet", "timeout", "performance"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "ReceiveTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "ReceiveTimeout")],
            DetectOps = [RegOp.CheckDword(InetKey, "ReceiveTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "proxy-set-ie-send-timeout",
            Label = "Set Internet Explorer Send Timeout to 30 s",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SendTimeout to 30 000 ms in WinINet. HTTP request sends that take longer than 30 seconds are aborted.",
            Tags = ["proxy", "ie", "wininet", "timeout", "performance"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "SendTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "SendTimeout")],
            DetectOps = [RegOp.CheckDword(InetKey, "SendTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "proxy-set-ie-connect-timeout",
            Label = "Set Internet Explorer Connect Timeout to 30 s",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets ConnectTimeout to 30 000 ms in WinINet. Connection attempts to unreachable servers are abandoned after 30 seconds.",
            Tags = ["proxy", "ie", "wininet", "timeout", "performance"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "ConnectTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "ConnectTimeout")],
            DetectOps = [RegOp.CheckDword(InetKey, "ConnectTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "proxy-set-ie-keepalive-timeout",
            Label = "Set Internet Explorer Keep-Alive Timeout to 30 s",
            Category = "Network",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets KeepAliveTimeout to 30 000 ms in WinINet. Idle HTTP keep-alive connections are recycled after 30 seconds.",
            Tags = ["proxy", "ie", "wininet", "keepalive", "timeout"],
            RegistryKeys = [InetKey],
            ApplyOps = [RegOp.SetDword(InetKey, "KeepAliveTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(InetKey, "KeepAliveTimeout")],
            DetectOps = [RegOp.CheckDword(InetKey, "KeepAliveTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "proxy-disable-ie-run-once",
            Label = "Disable Internet Explorer Run-Once First-Use Prompt",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RunOnceComplete=1 in HKLM IE\\Main to silently mark the IE run-once flow as complete. Suppresses the first-launch settings prompt on managed deployments.",
            Tags = ["proxy", "ie", "run-once", "debloat"],
            RegistryKeys = [$@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "RunOnceComplete", 1)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "RunOnceComplete")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Explorer\Main", "RunOnceComplete", 1)],
        },
        new TweakDef
        {
            Id = "proxy-set-winhttp-receive-timeout",
            Label = "Set HKLM WinINet Default Receive Timeout to 30 s",
            Category = "Network",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DefaultReceiveTimeout to 30 000 ms in the HKLM Internet Settings hive. Applies a machine-wide receive timeout for WinHTTP/WinINet components that read from the HKLM store.",
            Tags = ["proxy", "wininet", "winhttp", "timeout", "performance"],
            RegistryKeys = [$@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "DefaultReceiveTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "DefaultReceiveTimeout")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\Software\Microsoft\Windows\CurrentVersion\Internet Settings", "DefaultReceiveTimeout", 30000)],
        },
    ];
}

