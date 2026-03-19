// RegiLattice.Core — Services/NetworkManager.cs
// Sprint 27: DNS quick-switch and network repair utilities.

using System.Net.NetworkInformation;

namespace RegiLattice.Core;

/// <summary>Well-known DNS server presets available in the quick-switch UI.</summary>
public sealed record DnsPreset(string Name, string Primary, string Secondary, string? Primary6 = null, string? Secondary6 = null)
{
    public static readonly IReadOnlyList<DnsPreset> BuiltIn =
    [
        new("Automatic (DHCP)", "", "", null, null),
        new("Cloudflare", "1.1.1.1", "1.0.0.1", "2606:4700:4700::1111", "2606:4700:4700::1001"),
        new("Google", "8.8.8.8", "8.8.4.4", "2001:4860:4860::8888", "2001:4860:4860::8844"),
        new("Quad9 (filtered)", "9.9.9.9", "149.112.112.112", "2620:fe::fe", "2620:fe::9"),
        new("OpenDNS", "208.67.222.222", "208.67.220.220", "2620:119:35::35", "2620:119:53::53"),
        new("NextDNS", "45.90.28.0", "45.90.30.0", null, null),
    ];
}

/// <summary>Result of a network repair or DNS operation.</summary>
public sealed record NetworkOperationResult(bool Success, string Message);

/// <summary>Summary of a <see cref="NetworkManager.PingAsync"/> call.</summary>
public sealed record PingResult(string Host, int Sent, int Received, int Lost, double AverageMs, double MinMs, double MaxMs)
{
    /// <summary>Packet loss percentage (0–100).</summary>
    public double LossPercent => Sent == 0 ? 100.0 : (double)Lost / Sent * 100.0;

    /// <summary>Parses the stdout of a Windows <c>ping -n N host</c> command.</summary>
    internal static PingResult Parse(string host, string stdout)
    {
        int sent = 0,
            received = 0,
            lost = 0;
        double avg = 0,
            min = 0,
            max = 0;

        foreach (string line in stdout.Split('\n'))
        {
            string l = line.Trim();
            if (l.StartsWith("Packets:", StringComparison.OrdinalIgnoreCase))
            {
                // "Packets: Sent = 4, Received = 4, Lost = 0 (0% loss)"
                var m = System.Text.RegularExpressions.Regex.Matches(l, @"\d+");
                if (m.Count >= 3)
                {
                    int.TryParse(m[0].Value, out sent);
                    int.TryParse(m[1].Value, out received);
                    int.TryParse(m[2].Value, out lost);
                }
            }
            else if (
                l.StartsWith("Minimum", StringComparison.OrdinalIgnoreCase) || l.StartsWith("Minimum".ToLower(), StringComparison.OrdinalIgnoreCase)
            )
            {
                // "Minimum = 1ms, Maximum = 4ms, Average = 2ms"
                var m = System.Text.RegularExpressions.Regex.Matches(l, @"(\d+)ms");
                if (m.Count >= 3)
                {
                    double.TryParse(m[0].Value, out min);
                    double.TryParse(m[1].Value, out max);
                    double.TryParse(m[2].Value, out avg);
                }
            }
        }

        return new PingResult(host, sent, received, lost, avg, min, max);
    }
}

/// <summary>Per-adapter network I/O statistics snapshot.</summary>
public sealed record NetworkInterfaceStats(string Name, long BytesSent, long BytesReceived, long PacketsSent, long PacketsReceived, long Errors);

/// <summary>
/// Sprint 27 service: DNS quick-switch and network repair.
/// All mutating operations require Administrator privileges.
/// </summary>
public static class NetworkManager
{
    // ── DNS quick-switch ──────────────────────────────────────────────────

    /// <summary>Returns the names of all active, non-loopback network adapters.</summary>
    public static IReadOnlyList<string> GetActiveAdapterNames()
    {
        var adapters = new List<string>();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;
            if (ni.NetworkInterfaceType is NetworkInterfaceType.Loopback or NetworkInterfaceType.Tunnel)
                continue;
            adapters.Add(ni.Name);
        }
        return adapters;
    }

    /// <summary>Gets the current DNS servers for the named adapter.</summary>
    public static (string Primary, string Secondary) GetCurrentDns(string adapterName)
    {
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (!string.Equals(ni.Name, adapterName, StringComparison.OrdinalIgnoreCase))
                continue;

            var dns = ni.GetIPProperties().DnsAddresses;
            string primary = dns.Count > 0 ? dns[0].ToString() : "";
            string secondary = dns.Count > 1 ? dns[1].ToString() : "";
            return (primary, secondary);
        }
        return ("", "");
    }

    /// <summary>
    /// Sets IPv4 DNS servers on the named adapter using <c>netsh</c>.
    /// Pass empty strings for <paramref name="primary"/> to revert to DHCP.
    /// Requires Administrator.
    /// </summary>
    public static async Task<NetworkOperationResult> SetDnsAsync(string adapterName, string primary, string secondary, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(primary))
            return await ResetDnsToDhcpAsync(adapterName, ct).ConfigureAwait(false);

        // Set primary
        var (rc1, _, err1) = await ShellRunner
            .RunAsync("netsh", ["interface", "ip", "set", "dns", $"name={adapterName}", "source=static", $"address={primary}", "validate=no"], ct)
            .ConfigureAwait(false);

        if (rc1 != 0)
            return new NetworkOperationResult(false, $"Failed to set primary DNS: {err1.Trim()}");

        if (!string.IsNullOrWhiteSpace(secondary))
        {
            var (rc2, _, err2) = await ShellRunner
                .RunAsync("netsh", ["interface", "ip", "add", "dns", $"name={adapterName}", $"address={secondary}", "index=2", "validate=no"], ct)
                .ConfigureAwait(false);

            if (rc2 != 0)
                return new NetworkOperationResult(false, $"Warning: primary DNS set but secondary failed: {err2.Trim()}");
        }

        return new NetworkOperationResult(true, $"DNS set to {primary} / {secondary} on '{adapterName}'.");
    }

    /// <summary>Resets the named adapter's DNS to DHCP automatic mode.</summary>
    public static async Task<NetworkOperationResult> ResetDnsToDhcpAsync(string adapterName, CancellationToken ct = default)
    {
        var (rc, _, err) = await ShellRunner
            .RunAsync("netsh", ["interface", "ip", "set", "dns", $"name={adapterName}", "source=dhcp"], ct)
            .ConfigureAwait(false);

        return rc == 0
            ? new NetworkOperationResult(true, $"DNS reset to DHCP (automatic) on '{adapterName}'.")
            : new NetworkOperationResult(false, $"Failed to reset DNS: {err.Trim()}");
    }

    // ── Network repair ─────────────────────────────────────────────────────

    /// <summary>Flush the DNS resolver cache (<c>ipconfig /flushdns</c>). Does NOT require admin.</summary>
    public static async Task<NetworkOperationResult> FlushDnsCacheAsync(CancellationToken ct = default)
    {
        var (rc, out_, _) = await ShellRunner.RunAsync("ipconfig", ["/flushdns"], ct).ConfigureAwait(false);
        return rc == 0
            ? new NetworkOperationResult(true, out_.Trim())
            : new NetworkOperationResult(false, "ipconfig /flushdns returned non-zero exit code.");
    }

    /// <summary>Reset TCP/IP stack via <c>netsh int ip reset</c>. Requires admin; reboot recommended.</summary>
    public static async Task<NetworkOperationResult> ResetTcpIpAsync(CancellationToken ct = default)
    {
        var logPath = Path.Combine(Path.GetTempPath(), "regilattice-tcpip-reset.log");
        var (rc, _, err) = await ShellRunner.RunAsync("netsh", ["int", "ip", "reset", logPath], ct).ConfigureAwait(false);
        return rc == 0
            ? new NetworkOperationResult(true, "TCP/IP stack reset. A reboot is recommended.")
            : new NetworkOperationResult(false, $"TCP/IP reset failed: {err.Trim()}");
    }

    /// <summary>Reset Winsock via <c>netsh winsock reset</c>. Requires admin; reboot recommended.</summary>
    public static async Task<NetworkOperationResult> ResetWinsockAsync(CancellationToken ct = default)
    {
        var (rc, _, err) = await ShellRunner.RunAsync("netsh", ["winsock", "reset"], ct).ConfigureAwait(false);
        return rc == 0
            ? new NetworkOperationResult(true, "Winsock reset successfully. A reboot is recommended.")
            : new NetworkOperationResult(false, $"Winsock reset failed: {err.Trim()}");
    }

    /// <summary>Release and renew the DHCP lease on the named adapter (<c>ipconfig /release</c> + <c>/renew</c>).</summary>
    public static async Task<NetworkOperationResult> RenewDhcpLeaseAsync(string adapterName, CancellationToken ct = default)
    {
        await ShellRunner.RunAsync("ipconfig", ["/release", adapterName], ct).ConfigureAwait(false);
        var (rc, _, err) = await ShellRunner.RunAsync("ipconfig", ["/renew", adapterName], ct).ConfigureAwait(false);
        return rc == 0
            ? new NetworkOperationResult(true, $"DHCP lease renewed on '{adapterName}'.")
            : new NetworkOperationResult(false, $"Renew failed: {err.Trim()}");
    }

    /// <summary>
    /// Run a full network repair sequence: flush DNS → reset TCP/IP → reset Winsock.
    /// Reports progress via <paramref name="progress"/>. Requires admin; reboot recommended.
    /// </summary>
    public static async IAsyncEnumerable<NetworkOperationResult> RepairAllAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default
    )
    {
        yield return await FlushDnsCacheAsync(ct).ConfigureAwait(false);
        yield return await ResetTcpIpAsync(ct).ConfigureAwait(false);
        yield return await ResetWinsockAsync(ct).ConfigureAwait(false);
    }

    // ── Sprint 47 enhancements ─────────────────────────────────────────────

    /// <summary>
    /// Pings the specified <paramref name="host"/> up to <paramref name="count"/> times
    /// and returns a <see cref="PingResult"/> summarising latency and packet loss.
    /// </summary>
    public static async Task<PingResult> PingAsync(string host, int count = 4, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(host);
        if (count < 1 || count > 100)
            throw new ArgumentOutOfRangeException(nameof(count), "count must be 1–100.");

        var (_, stdout, _) = await ShellRunner.RunAsync("ping", ["-n", count.ToString(), host], ct).ConfigureAwait(false);

        return PingResult.Parse(host, stdout);
    }

    /// <summary>
    /// Returns per-adapter byte/packet statistics for all active non-loopback interfaces.
    /// </summary>
    public static IReadOnlyList<NetworkInterfaceStats> GetNetworkInterfaceStats()
    {
        var result = new List<NetworkInterfaceStats>();
        foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus != OperationalStatus.Up)
                continue;
            if (ni.NetworkInterfaceType is NetworkInterfaceType.Loopback or NetworkInterfaceType.Tunnel)
                continue;

            var stats = ni.GetIPStatistics();
            result.Add(
                new NetworkInterfaceStats(
                    Name: ni.Name,
                    BytesSent: stats.BytesSent,
                    BytesReceived: stats.BytesReceived,
                    PacketsSent: stats.UnicastPacketsSent,
                    PacketsReceived: stats.UnicastPacketsReceived,
                    Errors: stats.IncomingPacketsWithErrors + stats.OutgoingPacketsWithErrors
                )
            );
        }
        return result;
    }
}
