namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
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
            Category = "Proxy & VPN",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables TCP timestamp option (RFC 1323). Prevents system uptime fingerprinting via network scanners.",
            Tags = ["proxy", "tcp", "timestamps", "security", "network"],
            RegistryKeys = [$@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "Tcp1323Opts", 0)],
        },
    ];
}
