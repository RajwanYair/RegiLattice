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
            Id = "netopt-set-tcp-timestamps",
            Label = "Enable TCP Timestamps (RFC 1323)",
            Category = "Network Optimization",
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
        new TweakDef
        {
            Id = "netopt-enable-tcp-fast-open",
            Label = "Enable TCP Fast Open (TFO)",
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
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
            Category = "Network Optimization",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Increases the ephemeral port range to 65534. Prevents port exhaustion under high connection loads.",
            Tags = ["network", "ports", "ephemeral", "connections"],
            RegistryKeys = [TcpParams],
            ApplyOps = [RegOp.SetDword(TcpParams, "MaxUserPort", 65534)],
            RemoveOps = [RegOp.DeleteValue(TcpParams, "MaxUserPort")],
            DetectOps = [RegOp.CheckDword(TcpParams, "MaxUserPort", 65534)],
        },
    ];
}
