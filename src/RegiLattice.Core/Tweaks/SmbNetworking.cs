// RegiLattice.Core — Tweaks/SmbNetworking.cs
// SMB/Workstation service and LanmanServer advanced tuning (Sprint 79).
// Slug "smb" — LanmanWorkstation + LanmanServer parameters distinct from
// NetworkOptimization.cs (which only sets DisableBandwidthThrottling).
// Uses HKLM\SYSTEM\CurrentControlSet\Services\Lanman{Workstation,Server}\Parameters.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SmbNetworking
{
    private const string LmWks = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanWorkstation\Parameters";

    private const string LmSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

    private const string MrxSmb = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MrxSmb\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smb-increase-max-cmds",
            Label = "Increase SMB Outstanding Command Limit",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["smb", "network", "performance", "workstation"],
            Description =
                "Increases the SMB client outstanding command limit from 50 to 256. "
                + "Improves file-server throughput when accessing many files concurrently.",
            ApplyOps = [RegOp.SetDword(LmWks, "MaxCmds", 256)],
            RemoveOps = [RegOp.DeleteValue(LmWks, "MaxCmds")],
            DetectOps = [RegOp.CheckDword(LmWks, "MaxCmds", 256)],
        },
        new TweakDef
        {
            Id = "smb-enable-large-mtu",
            Label = "Enable SMB Large MTU Support",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["smb", "network", "mtu", "performance"],
            Description =
                "Enables SMB large MTU (Maximum Transmission Unit) negotiation. "
                + "Improves transfer speed on jumbo-frame-capable networks (MTU ≥ 9000).",
            ApplyOps = [RegOp.SetDword(LmWks, "EnableLargeMTU", 1)],
            RemoveOps = [RegOp.DeleteValue(LmWks, "EnableLargeMTU")],
            DetectOps = [RegOp.CheckDword(LmWks, "EnableLargeMTU", 1)],
        },
        new TweakDef
        {
            Id = "smb-reduce-dormant-file-limit",
            Label = "Reduce SMB Dormant File Connection Limit",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Tags = ["smb", "network", "connections", "memory"],
            Description =
                "Reduces the number of dormant SMB connections kept open from 1023 to 64. "
                + "Frees memory on workstations that connect to many different file servers.",
            ApplyOps = [RegOp.SetDword(LmWks, "DormantFileLimit", 64)],
            RemoveOps = [RegOp.DeleteValue(LmWks, "DormantFileLimit")],
            DetectOps = [RegOp.CheckDword(LmWks, "DormantFileLimit", 64)],
        },
        new TweakDef
        {
            Id = "smb-disable-oplocks-server",
            Label = "Disable Opportunistic Locks on SMB Server",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Tags = ["smb", "server", "oplocks", "stability"],
            Description =
                "Disables opportunistic locks (oplocks) on the LanMan Server service. "
                + "Recommended when SMB shares serve legacy applications prone to "
                + "oplock-related file corruption.",
            ApplyOps = [RegOp.SetDword(LmSrv, "EnableOplocks", 0)],
            RemoveOps = [RegOp.SetDword(LmSrv, "EnableOplocks", 1)],
            DetectOps = [RegOp.CheckDword(LmSrv, "EnableOplocks", 0)],
        },
        new TweakDef
        {
            Id = "smb-increase-server-max-work-items",
            Label = "Increase SMB Server Work Items",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["smb", "server", "performance", "workitems"],
            Description =
                "Increases the maximum number of queued work items the SMB server "
                + "processes to 2048 (from the default 128). Helps high-concurrency file servers.",
            ApplyOps = [RegOp.SetDword(LmSrv, "MaxWorkItems", 2048)],
            RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxWorkItems")],
            DetectOps = [RegOp.CheckDword(LmSrv, "MaxWorkItems", 2048)],
        },
        new TweakDef
        {
            Id = "smb-increase-server-max-raw-work-items",
            Label = "Increase SMB Server Raw Work Buffer",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["smb", "server", "performance", "buffer"],
            Description =
                "Increases the raw-mode SMB server buffer count to 512 (default 4). "
                + "Improves large sequential read/write throughput on file servers.",
            ApplyOps = [RegOp.SetDword(LmSrv, "MaxRawWorkItems", 512)],
            RemoveOps = [RegOp.DeleteValue(LmSrv, "MaxRawWorkItems")],
            DetectOps = [RegOp.CheckDword(LmSrv, "MaxRawWorkItems", 512)],
        },
        new TweakDef
        {
            Id = "smb-disable-smb1",
            Label = "Disable SMB 1.0 Protocol (Server-Side)",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["smb", "smb1", "security", "legacy", "hardening"],
            Description =
                "Disables the legacy SMB 1.0 protocol on the SMB server component. "
                + "SMBv1 is vulnerable to EternalBlue/WannaCry. Modern Windows (10/11) "
                + "does not require SMB1 for normal file sharing.",
            ApplyOps = [RegOp.SetDword(LmSrv, "SMB1", 0)],
            RemoveOps = [RegOp.SetDword(LmSrv, "SMB1", 1)],
            DetectOps = [RegOp.CheckDword(LmSrv, "SMB1", 0)],
        },
        new TweakDef
        {
            Id = "smb-enforce-smb-signing-server",
            Label = "Enforce SMB Signing on Server",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["smb", "signing", "security", "hardening", "mitm"],
            Description =
                "Requires SMB packet signing for all server connections. Prevents "
                + "man-in-the-middle attacks against SMB sessions. May reduce throughput "
                + "by ~10–15% due to signing overhead.",
            ApplyOps = [RegOp.SetDword(LmSrv, "RequireSecuritySignature", 1)],
            RemoveOps = [RegOp.SetDword(LmSrv, "RequireSecuritySignature", 0)],
            DetectOps = [RegOp.CheckDword(LmSrv, "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "smb-enforce-smb-signing-client",
            Label = "Enforce SMB Signing on Client",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Tags = ["smb", "signing", "security", "hardening", "client"],
            Description =
                "Requires the SMB client to sign all outgoing SMB connections. "
                + "Pairs with the server-side enforcement tweak for full MITM protection. "
                + "Reboot or network reconnect required for effect.",
            ApplyOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 1)],
            RemoveOps = [RegOp.SetDword(LmWks, "RequireSecuritySignature", 0)],
            DetectOps = [RegOp.CheckDword(LmWks, "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "smb-increase-collection-count",
            Label = "Increase SMB Write-Ahead Collection Count",
            Category = "SMB Networking",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Tags = ["smb", "write", "buffer", "performance"],
            Description =
                "Increases the SMB client write-ahead collection buffer count to 32 "
                + "(from default 16). Improves sequential write performance to file servers "
                + "by batching more data before flushing.",
            ApplyOps = [RegOp.SetDword(LmWks, "MaxCollectionCount", 32)],
            RemoveOps = [RegOp.DeleteValue(LmWks, "MaxCollectionCount")],
            DetectOps = [RegOp.CheckDword(LmWks, "MaxCollectionCount", 32)],
        },
    ];
}
