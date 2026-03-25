// RegiLattice.Core — Tweaks/NetBiosPolicy.cs
// Sprint 343: NetBIOS Policy tweaks (10 tweaks)
// Category: "NetBIOS Policy" | Slug: netbios
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetBIOS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NetBiosPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetBIOS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "netbios-disable-netbios-over-tcpip",
            Label = "Disable NetBIOS over TCP/IP on All Network Interfaces",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "NetBIOS over TCP/IP is a legacy name resolution protocol from the 1980s that is rarely required in modern DNS-based networks but provides significant attack surface for lateral movement. Disabling NetBIOS over TCP/IP eliminates LLMNR, NBT-NS, and WINS name resolution attacks that are used by tools like Responder to capture NTLM credentials. NetBIOS poisoning attacks intercept broadcast name resolution requests and respond with attacker-controlled responses causing systems to authenticate to attacker servers. Organizations that have fully migrated to DNS for name resolution have no functional need for NetBIOS and should disable it on all interfaces. Before disabling NetBIOS verify that no legacy applications require NetBIOS name resolution as this can break application connectivity in mixed environments. Disabling NetBIOS is a CIS benchmark recommendation that significantly hardens Windows systems against LLMNR/NBT-NS capture attacks.",
            Tags = ["netbios", "llmnr", "name-resolution", "ntlm-relay", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNetBIOS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNetBIOS")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNetBIOS", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-llmnr-resolution",
            Label = "Disable Link-Local Multicast Name Resolution",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Link-Local Multicast Name Resolution (LLMNR) is a fallback name resolution protocol that broadcasts queries to the local network segment and can be poisoned by attackers. Disabling LLMNR prevents name resolution poisoning attacks where Responder and similar tools intercept LLMNR queries and respond with attacker-controlled IP addresses redirecting authentication traffic. LLMNR-based credential capture is one of the most common techniques used during internal network penetration testing due to its near-universal success in environments that have not disabled LLMNR. Disabling LLMNR has minimal impact on modern Windows environments that use DNS as the primary name resolution mechanism. Organizations should disable LLMNR across all systems in their domain as a standard hardening step that is low-risk and high-reward. LLMNR disabling is controlled through the EnableMulticast registry value under the DNSClient policy key rather than a dedicated LLMNR key.",
            Tags = ["netbios", "llmnr", "multicast", "credential-capture", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableLLMNR", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableLLMNR")],
            DetectOps = [RegOp.CheckDword(Key, "DisableLLMNR", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-wins-client",
            Label = "Disable WINS Client Name Resolution",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Windows Internet Name Service (WINS) client is a legacy NetBIOS name resolution service that maps NetBIOS names to IP addresses and predates DNS as the network name resolution standard. Disabling WINS client name resolution removes a legacy attack surface for name resolution spoofing and simplifies the network stack by removing unused legacy protocols. WINS infrastructure is rarely deployed in modern enterprise environments that have migrated to DNS for all name resolution requirements. Disabling WINS client prevents the system from querying WINS servers that may be attackers spoofing the WINS server to redirect name resolution. Organizations still running WINS for legacy application compatibility should have a migration plan to retire WINS and transition fully to DNS. Windows Server 2012 R2 was the last version to ship with WINS as an installable server role making now the time for WINS infrastructure retirement.",
            Tags = ["netbios", "wins", "name-resolution", "legacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableWINSClient", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableWINSClient")],
            DetectOps = [RegOp.CheckDword(Key, "DisableWINSClient", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-netbios-name-broadcasts",
            Label = "Disable NetBIOS Name Broadcast Announcements",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "NetBIOS name broadcasts announce the system's NetBIOS name to the local network segment providing attackers with host discovery and naming information. Disabling NetBIOS name broadcasts reduces the information available to attackers performing network reconnaissance and eliminates broadcast-based credential capture opportunities. NetBIOS announcements also consume network bandwidth and CPU resources particularly on large flat networks with many systems broadcasting simultaneously. Modern Windows systems that use DNS-SD or other modern discovery protocols do not require NetBIOS broadcasts for network discovery functionality. Disabling broadcasts prevents tools like NetBIOS scanners from easily discovering systems and their NetBIOS names during reconnaissance. Organizations with large flat Networks should prioritize disabling NetBIOS broadcasts to reduce both security risk and unnecessary broadcast traffic.",
            Tags = ["netbios", "broadcasts", "reconnaissance", "network-discovery", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNameBroadcast", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNameBroadcast")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNameBroadcast", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-nbt-ns-resolution",
            Label = "Disable NetBIOS over TCP/IP Name Service Queries",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "NetBIOS Name Service (NBT-NS) queries are broadcast UDP requests for name resolution that are intercepted by poisoning tools to capture NTLM credentials. Disabling NBT-NS query transmission prevents the system from sending NBT-NS queries that can be captured and responded to by attacker tools. NBT-NS poisoning is one of the most common attack techniques used in Active Directory environment compromises because it is reliable and does not require any vulnerability. Organizations that have disabled DNS fallback to NetBIOS can safely disable NBT-NS without impacting name resolution for modern applications. Firewall rules blocking UDP port 137 at the host level provide an additional layer of protection against NBT-NS exploitation. The combination of disabling LLMNR, NBT-NS, and WINS eliminates all multicast and broadcast name resolution attack vectors from the system.",
            Tags = ["netbios", "nbt-ns", "poisoning", "ntlm-relay", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNBTNS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTNS")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNBTNS", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-computer-browser-service",
            Label = "Disable Computer Browser Service NetBIOS Dependency",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The Computer Browser service maintains a list of computers and resources on the network using NetBIOS broadcasts and is rarely needed in modern AD environments. Disabling the Computer Browser service eliminates the NetBIOS dependency it creates and removes the master browser election process that generates unnecessary broadcast traffic. Browser service elections on large networks can cause periodic network storms as systems compete for master browser status. Modern Windows environments use Active Directory for computer discovery and organizational structure making the legacy Computer Browser service redundant. Disabling Computer Browser has no impact on Active Directory domain functionality including group policy, authentication, or shared resource access. The Computer Browser service should be disabled and set to Manual or Disabled startup to prevent automatic startup in future sessions.",
            Tags = ["netbios", "computer-browser", "broadcast", "legacy-service", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableComputerBrowser", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableComputerBrowser")],
            DetectOps = [RegOp.CheckDword(Key, "DisableComputerBrowser", 1)],
        },
        new TweakDef
        {
            Id = "netbios-restrict-smb-netbios-sharing",
            Label = "Restrict SMB NetBIOS File Sharing to Authenticated Users",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "SMB over NetBIOS (ports 139) is a legacy file sharing path that runs alongside the modern SMB direct connection (port 445) and represents additional attack surface. Restricting SMB over NetBIOS to authenticated users prevents anonymous and unauthenticated access attempts that probe legacy SMB services. Port 139 represents an older NetBIOS session service for SMB that is rarely needed in modern networks running purely SMB 2.0 or later. Organizations should configure Windows Firewall to block port 139 inbound traffic in addition to policy-based NetBIOS restrictions. Disabling NetBT in the network adapter settings removes the NetBIOS over TCP/IP stack entirely eliminating port 137, 138, and 139 from the system's network exposure. Legacy applications that require NetBIOS for file sharing should be migrated to use standard SMB over port 445.",
            Tags = ["netbios", "smb", "file-sharing", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousNetBIOS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousNetBIOS")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousNetBIOS", 1)],
        },
        new TweakDef
        {
            Id = "netbios-audit-netbios-name-queries",
            Label = "Enable Audit Logging for NetBIOS Name Query Events",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "NetBIOS name query audit logging captures all NetBIOS name lookup events providing visibility into legacy name resolution usage and potential poisoning activity. Enabling NetBIOS query audit logging helps identify systems that still rely on NetBIOS name resolution which should be migrated to DNS before NetBIOS is disabled. Audit data from NetBIOS queries can reveal hidden dependencies on NetBIOS in legacy applications that would break if NetBIOS were disabled without investigation. NetBIOS audit events combined with network monitoring help detect LLMNR and NBT-NS poisoning attacks in progress by correlating unexpected name resolution responses. Organizations should run NetBIOS query auditing for 30 days before disabling NetBIOS to identify all systems and applications that depend on it. Regular review of NetBIOS audit data after deploying other restrictions helps confirm that the restrictions are working and no bypass paths exist.",
            Tags = ["netbios", "audit", "monitoring", "name-resolution", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditNameQueries", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditNameQueries")],
            DetectOps = [RegOp.CheckDword(Key, "AuditNameQueries", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-netbt-registration",
            Label = "Disable NetBIOS Computer Name Registration on Network",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "NetBIOS computer name registration broadcasts the system hostname to all systems on the local network segment providing attacker-friendly network enumeration data. Disabling NetBT name registration prevents the system from advertising its hostname through NetBIOS reducing the information available for network reconnaissance. NetBIOS name registration on modern networks duplicates DNS registration and adds unnecessary broadcast traffic while providing attack surface. Systems that disable NetBIOS name registration will not be discoverable through NetBIOS enumeration tools but remain accessible through DNS-based discovery. Penetration tester tools like nbtscan rely on NetBIOS name registration to enumerate Windows systems making disabling registration a valuable hardening step. Organizations should combine disabling NetBIOS registration with DNS hostname privacy configurations for comprehensive network discovery hardening.",
            Tags = ["netbios", "name-registration", "reconnaissance", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableNBTRegistration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTRegistration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableNBTRegistration", 1)],
        },
        new TweakDef
        {
            Id = "netbios-disable-multicast-dns",
            Label = "Disable Multicast DNS to Prevent mDNS-Based Poisoning",
            Category = "NetBIOS Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Multicast DNS (mDNS) is a zero-configuration networking protocol that resolves hostnames on local networks without a central DNS server and is exploitable for name poisoning similar to LLMNR. Disabling mDNS prevents mDNS-based credential capture attacks where tools like Responder implement mDNS poisoning to steal NTLM credentials. mDNS shares similar attack characteristics with LLMNR and NBT-NS and should be disabled alongside them for comprehensive broadcast name resolution hardening. Windows uses mDNS implemented through the DNS Client service and disabling it is controlled through policy rather than removing the service. mDNS is more commonly needed for Apple Bonjour-compatible devices and IoT devices than for standard Windows domain environments. Organizations should disable mDNS on domain-joined Windows systems while evaluating whether IoT or Apple devices on the same network segment require it.",
            Tags = ["netbios", "mdns", "multicast-dns", "poisoning", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMulticastDNS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticastDNS")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMulticastDNS", 1)],
        },
    ];
}
