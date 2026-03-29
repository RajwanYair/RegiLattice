// RegiLattice.Core — Tweaks/DefenderNetworkProtectionPolicy.cs
// Defender Network Protection Policy — Sprint 534.
// Controls Windows Defender Exploit Guard Network Protection, which blocks connections
// to low-reputation domains, phishing sites, and exploit kit infrastructure.
// Category: "Defender Network Protection Policy" | Slug: defnet
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DefenderNetworkProtectionPolicy
{
    private const string NetProtKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\Windows Defender Exploit Guard\Network Protection";

    private const string SmartScreenKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows Defender\SmartScreen";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "defnet-enable-block-mode",
                Label = "Network Protection: Enable Block Mode",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableNetworkProtection=1 (block mode). Activates Defender Network Protection, which uses Microsoft's cloud-based threat intelligence to block outbound connections to known-malicious IP addresses and domains. NP operates at the kernel level via the Windows Filtering Platform, intercepting connections before they leave the machine. Covers all applications (not just browsers), including LOLBins and malware command-and-control beaconing.",
                Tags = ["network-protection", "defender", "c2", "malware", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 4,
                ImpactNote = "Blocks connections to Microsoft-rated malicious hosts. Rare false positives for uncommon legitimate domains.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtection")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtection", 1)],
            },
            new TweakDef
            {
                Id = "defnet-enable-audit-mode",
                Label = "Network Protection: Enable Audit Mode",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableNetworkProtection=2 (audit mode). Logs all Network Protection block events to Event ID 1125 without actually blocking the connection. Use audit mode to understand which outbound connections would be blocked before switching to block mode. Useful for identifying legitimate business applications that connect to hosts that NP would flag. Requires Microsoft Defender Antivirus to be the active AV.",
                Tags = ["network-protection", "defender", "audit", "monitoring"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Audit-only; no connections blocked. Safe to deploy first.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtection", 2)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtection")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtection", 2)],
            },
            new TweakDef
            {
                Id = "defnet-block-low-reputation",
                Label = "Network Protection: Block Low-Reputation Cloud Downloads",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets BlockLowReputationCode=1. Instructs Network Protection to block downloads from URLs where the destination file has a low cloud reputation score in Microsoft's SmartScreen service. Files with no reputation or insufficient prevalence among Microsoft's telemetry pool are blocked before they are fully downloaded. Complements SmartScreen-at-launch protection with pre-download reputation-based blocking.",
                Tags = ["network-protection", "smartscreen", "download", "reputation", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "May block uncommon but legitimate files with low cloud prevalence. Users may see block alerts on novel tools.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "BlockLowReputationCode", 1)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "BlockLowReputationCode")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "BlockLowReputationCode", 1)],
            },
            new TweakDef
            {
                Id = "defnet-disable-dns-over-udp",
                Label = "Network Protection: Enforce DNS Inspection (Block DNS Tunneling)",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableDnsOverHttps=0. Prevents applications from bypassing Network Protection DNS inspection by forcing DNS queries through encrypted channels (DoH) that NP cannot inspect. DNS tunneling is used by C2 frameworks (Cobalt Strike, Metasploit DNS shells) to exfiltrate data and receive commands via DNS TXT/CNAME records. Keeping NP's DNS inspection path active ensures malicious DNS traffic is visible to Defender.",
                Tags = ["network-protection", "dns", "tunneling", "c2", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Disables app-level DoH within NP scope; system DoH policy is separate. Some apps may fall back to plain DNS.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "EnableDnsOverHttps", 0)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableDnsOverHttps")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "EnableDnsOverHttps", 0)],
            },
            new TweakDef
            {
                Id = "defnet-enable-smartscreen-app",
                Label = "Network Protection: Enable SmartScreen for Applications",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableSmartScreenInShell=1.  Forces SmartScreen reputation checks for all executables and scripts launched from within applications (not just from Explorer). Without this setting, processes launched by LOLBins or injected threads bypass the Explorer SmartScreen path. Enabling SmartScreen in-shell ensures reputation checks happen regardless of the launch context.",
                Tags = ["network-protection", "smartscreen", "lolbin", "reputation", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Adds SmartScreen latency to process launches. Infrequent but non-zero delay for unknown binaries.",
                ApplyOps = [RegOp.SetDword(SmartScreenKey, "EnableSmartScreenInShell", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartScreenKey, "EnableSmartScreenInShell")],
                DetectOps = [RegOp.CheckDword(SmartScreenKey, "EnableSmartScreenInShell", 1)],
            },
            new TweakDef
            {
                Id = "defnet-disable-bypass-smartscreen",
                Label = "Network Protection: Prevent Users from Bypassing SmartScreen Blocks",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets PreventOverrideForFilesInShell=1. Removes the 'Run Anyway' button from SmartScreen block dialogs for file launches. Without this setting, determined users can bypass SmartScreen warnings by clicking through. In enterprise environments, users should not be able to override network protection policy decisions. Setting this to 1 makes SmartScreen blocks final — users must contact IT administration.",
                Tags = ["network-protection", "smartscreen", "override", "enterprise", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Users cannot bypass SmartScreen. Ensure an IT process exists for requesting allow-list exceptions.",
                ApplyOps = [RegOp.SetDword(SmartScreenKey, "PreventOverrideForFilesInShell", 1)],
                RemoveOps = [RegOp.DeleteValue(SmartScreenKey, "PreventOverrideForFilesInShell")],
                DetectOps = [RegOp.CheckDword(SmartScreenKey, "PreventOverrideForFilesInShell", 1)],
            },
            new TweakDef
            {
                Id = "defnet-block-suspicious-behaviors",
                Label = "Network Protection: Enable Behavioral Monitoring of Network Traffic",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableBehavioralNetworkBlocks=1. Activates Defender's behavioral engine for network connection analysis. Unlike reputation-only blocks, behavioral monitoring detects C2 patterns (beaconing intervals, jitter, domain generation algorithms) that signature-only defenses cannot catch. Behavioral blocks are complementary to reputation blocks — a novel C2 domain with no reputation history will still be detected via behavioral patterns.",
                Tags = ["network-protection", "behavioral", "c2", "detection", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Behavioral analysis adds minor network latency. Rare false positives on legitimate beaconing apps (monitoring agents).",
                ApplyOps = [RegOp.SetDword(NetProtKey, "EnableBehavioralNetworkBlocks", 1)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableBehavioralNetworkBlocks")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "EnableBehavioralNetworkBlocks", 1)],
            },
            new TweakDef
            {
                Id = "defnet-block-potentially-unwanted",
                Label = "Network Protection: Block Connections to PUA/PUP Infrastructure",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets EnableNetworkProtectionPua=1. Extends Network Protection to block outbound connections to infrastructure associated with Potentially Unwanted Applications and bundlers. PUA families (adware, browser hijackers, crypto miners) frequently use dedicated C2 networks distinct from malware. Blocking PUA network traffic prevents tracking pixel calls, update beaconing, and telemetry uploads from unwanted applications.",
                Tags = ["network-protection", "pua", "adware", "privacy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Targets PUA-associated network infrastructure. Some dual-use analytic tools may be affected.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "EnableNetworkProtectionPua", 1)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "EnableNetworkProtectionPua")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "EnableNetworkProtectionPua", 1)],
            },
            new TweakDef
            {
                Id = "defnet-enable-cloud-check",
                Label = "Network Protection: Enable Real-Time Cloud-Based URL Lookup",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets CloudExtendedTimeout=50. Sets the maximum time (50 × 100 ms = 5 s) that Network Protection will wait for a cloud reputation response before allowing a connection. A longer timeout allows the cloud protection service to consult the NP telemetry database fully before deciding whether to block a connection to a novel domain. Balances cloud check completeness against connection latency.",
                Tags = ["network-protection", "cloud", "url-check", "latency"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Adds up to 5 s latency on first connections to novel domains. Subsequent connections to the same domain are cached.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "CloudExtendedTimeout", 50)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "CloudExtendedTimeout")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "CloudExtendedTimeout", 50)],
            },
            new TweakDef
            {
                Id = "defnet-enable-loopback-block",
                Label = "Network Protection: Block Loopback Bypass Attempts",
                Category = "Defender Network Protection Policy",
                Description =
                    "Sets DisableLoopbackExemption=1. Removes the automatic exemption that Network Protection grants to loopback (127.0.0.1) connections. Some malware proxies its C2 traffic through a local port listener on loopback to bypass per-process network monitoring. While most NP rules already apply to all network destinations, this setting ensures that loopback-aliased proxy traffic is also subject to behavioral analysis.",
                Tags = ["network-protection", "loopback", "proxy", "evasion", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Applies NP to loopback traffic. Development tools that use local proxies (Fiddler, mitmproxy) may be affected.",
                ApplyOps = [RegOp.SetDword(NetProtKey, "DisableLoopbackExemption", 1)],
                RemoveOps = [RegOp.DeleteValue(NetProtKey, "DisableLoopbackExemption")],
                DetectOps = [RegOp.CheckDword(NetProtKey, "DisableLoopbackExemption", 1)],
            },
        ];
}
