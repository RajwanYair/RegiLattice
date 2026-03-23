#nullable enable
using RegiLattice.Core.Models;
using System.Collections.Generic;

namespace RegiLattice.Core.Tweaks;

// Slug "fwlog" — Windows Firewall GPO logging policy (dropped packets and successful connections).
// SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging
// SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging
// SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging
internal static class FirewallLogPolicy
{
    private const string DomainLog =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\DomainProfile\Logging";

    private const string PrivateLog =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PrivateProfile\Logging";

    private const string PublicLog =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsFirewall\PublicProfile\Logging";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "fwlog-domain-log-dropped",
            Label = "Log dropped packets — Domain firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of dropped packets in the Domain firewall profile via GPO policy. "
                + "LogDroppedPackets=1. Default: not logged. Helps detect blocked connection attempts.",
            Tags = ["firewall", "logging", "dropped", "domain", "policy"],
            ApplyOps = [RegOp.SetDword(DomainLog, "LogDroppedPackets", 1)],
            RemoveOps = [RegOp.DeleteValue(DomainLog, "LogDroppedPackets")],
            DetectOps = [RegOp.CheckDword(DomainLog, "LogDroppedPackets", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-domain-log-success",
            Label = "Log successful connections — Domain firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of allowed connections in the Domain firewall profile via GPO policy. "
                + "LogSuccessfulConnections=1. Useful for network access auditing.",
            Tags = ["firewall", "logging", "success", "domain", "policy"],
            ApplyOps = [RegOp.SetDword(DomainLog, "LogSuccessfulConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(DomainLog, "LogSuccessfulConnections")],
            DetectOps = [RegOp.CheckDword(DomainLog, "LogSuccessfulConnections", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-domain-log-size",
            Label = "Set Domain firewall log size to 16 MB (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Domain profile firewall log file size to 16 MB via GPO policy. "
                + "LogFileSize=16384 (KB). Default: 4096 KB (4 MB). Larger logs retain more history.",
            Tags = ["firewall", "logging", "size", "domain", "policy"],
            ApplyOps = [RegOp.SetDword(DomainLog, "LogFileSize", 16384)],
            RemoveOps = [RegOp.DeleteValue(DomainLog, "LogFileSize")],
            DetectOps = [RegOp.CheckDword(DomainLog, "LogFileSize", 16384)],
        },
        new TweakDef
        {
            Id = "fwlog-private-log-dropped",
            Label = "Log dropped packets — Private firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of dropped packets in the Private (home/work) firewall profile via GPO policy. "
                + "LogDroppedPackets=1. Helps detect unsolicited connection attempts on private networks.",
            Tags = ["firewall", "logging", "dropped", "private", "policy"],
            ApplyOps = [RegOp.SetDword(PrivateLog, "LogDroppedPackets", 1)],
            RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogDroppedPackets")],
            DetectOps = [RegOp.CheckDword(PrivateLog, "LogDroppedPackets", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-private-log-success",
            Label = "Log successful connections — Private firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of allowed connections in the Private firewall profile via GPO policy. "
                + "LogSuccessfulConnections=1. Useful for home/work network access auditing.",
            Tags = ["firewall", "logging", "success", "private", "policy"],
            ApplyOps = [RegOp.SetDword(PrivateLog, "LogSuccessfulConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogSuccessfulConnections")],
            DetectOps = [RegOp.CheckDword(PrivateLog, "LogSuccessfulConnections", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-private-log-size",
            Label = "Set Private firewall log size to 16 MB (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Private profile firewall log file size to 16 MB via GPO policy. "
                + "LogFileSize=16384 (KB). Default: 4096 KB.",
            Tags = ["firewall", "logging", "size", "private", "policy"],
            ApplyOps = [RegOp.SetDword(PrivateLog, "LogFileSize", 16384)],
            RemoveOps = [RegOp.DeleteValue(PrivateLog, "LogFileSize")],
            DetectOps = [RegOp.CheckDword(PrivateLog, "LogFileSize", 16384)],
        },
        new TweakDef
        {
            Id = "fwlog-public-log-dropped",
            Label = "Log dropped packets — Public firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of dropped packets in the Public firewall profile via GPO policy. "
                + "LogDroppedPackets=1. Critical for monitoring untrusted network environments.",
            Tags = ["firewall", "logging", "dropped", "public", "policy"],
            ApplyOps = [RegOp.SetDword(PublicLog, "LogDroppedPackets", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicLog, "LogDroppedPackets")],
            DetectOps = [RegOp.CheckDword(PublicLog, "LogDroppedPackets", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-public-log-success",
            Label = "Log successful connections — Public firewall profile (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables logging of allowed connections in the Public (untrusted) firewall profile via GPO policy. "
                + "LogSuccessfulConnections=1. Reveals network access on public Wi-Fi/untrusted networks.",
            Tags = ["firewall", "logging", "success", "public", "policy"],
            ApplyOps = [RegOp.SetDword(PublicLog, "LogSuccessfulConnections", 1)],
            RemoveOps = [RegOp.DeleteValue(PublicLog, "LogSuccessfulConnections")],
            DetectOps = [RegOp.CheckDword(PublicLog, "LogSuccessfulConnections", 1)],
        },
        new TweakDef
        {
            Id = "fwlog-public-log-size",
            Label = "Set Public firewall log size to 16 MB (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the maximum Public profile firewall log file size to 16 MB via GPO policy. "
                + "LogFileSize=16384 (KB). Default: 4096 KB. Larger logs help with incident investigation.",
            Tags = ["firewall", "logging", "size", "public", "policy"],
            ApplyOps = [RegOp.SetDword(PublicLog, "LogFileSize", 16384)],
            RemoveOps = [RegOp.DeleteValue(PublicLog, "LogFileSize")],
            DetectOps = [RegOp.CheckDword(PublicLog, "LogFileSize", 16384)],
        },
        new TweakDef
        {
            Id = "fwlog-domain-log-file-path",
            Label = "Set Domain firewall log file to pfirewall-domain.log (policy)",
            Category = "Firewall Log Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures a distinct log file path for the Domain firewall profile via GPO policy. "
                + "LogFilePath=%systemroot%\\system32\\LogFiles\\Firewall\\pfirewall-domain.log. "
                + "Default: pfirewall.log (shared with all profiles).",
            Tags = ["firewall", "logging", "path", "domain", "policy"],
            ApplyOps =
            [
                RegOp.SetExpandString(
                    DomainLog,
                    "LogFilePath",
                    @"%systemroot%\system32\LogFiles\Firewall\pfirewall-domain.log"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(DomainLog, "LogFilePath")],
            DetectOps =
            [
                RegOp.CheckString(
                    DomainLog,
                    "LogFilePath",
                    @"%systemroot%\system32\LogFiles\Firewall\pfirewall-domain.log"
                ),
            ],
        },
    ];
}
