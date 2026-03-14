namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

/// <summary>
/// Network security and optimisation tweaks — TCP/IP stack tuning, WiFi, DNS hardening,
/// NIC offloading, bandwidth management, and network adapter power settings.
/// </summary>
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Id = "netopt-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense (Auto-Connect)",
            Category = "Network Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Wi-Fi Sense which auto-connects to open hotspots and shares WiFi credentials.",
            Tags = ["network", "wifi", "privacy", "security"],
            RegistryKeys = [$@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config"],
            ApplyOps = [RegOp.SetDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
            RemoveOps = [RegOp.DeleteValue($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM")],
            DetectOps = [RegOp.CheckDword($@"{LmKey}\SOFTWARE\Microsoft\WcmSvc\wifinetworkmanager\config", "AutoConnectAllowedOEM", 0)],
        },
        new TweakDef
        {
            Id = "netopt-set-dns-cloudflare",
            Label = "Set DNS to Cloudflare (1.1.1.1 + DoH)",
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
    ];
}
