// RegiLattice.Core — Tweaks/SmbServerPolicy.cs
// Category: "SMB Server Policy" — Slug "smbsrv"
// HKLM\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters
// SMB server configuration — admin hidden shares, performance tuning, and session limits.
// Does NOT duplicate RequireSecuritySignature (FileSystem.cs) or EnableSecuritySignature (Network.cs).

#nullable enable

using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class SmbServerPolicy
{
    private const string SmbSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smbsrv-disable-admin-share-server",
            Label = "Disable Hidden Admin Shares (Server Mode)",
            Category = "SMB Server Policy",
            Description =
                "Sets AutoShareServer=0 in LanmanServer parameters. Prevents Windows from automatically creating the hidden administrative shares (C$, D$, ADMIN$, IPC$) on server-class installations when the computer starts. Reduces the exposed SMB attack surface on file server roles.",
            Tags = ["smb", "admin-share", "server", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(SmbSrv, "AutoShareServer", 0)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "AutoShareServer")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "AutoShareServer", 0)],
        },
        new TweakDef
        {
            Id = "smbsrv-disable-admin-share-workstation",
            Label = "Disable Hidden Admin Shares (Workstation Mode)",
            Category = "SMB Server Policy",
            Description =
                "Sets AutoShareWks=0 in LanmanServer parameters. Prevents Windows from automatically creating the hidden workstation administrative shares (C$, D$, ADMIN$) when the computer starts. Best practice for standalone workstations where remote admin shares are not required.",
            Tags = ["smb", "admin-share", "workstation", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(SmbSrv, "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "smbsrv-enable-oplocks",
            Label = "Enable SMB Opportunistic Locking",
            Category = "SMB Server Policy",
            Description =
                "Sets EnableOpLocks=1 in LanmanServer parameters. Ensures opportunistic locking (oplocks) is enabled for SMB shares. Oplocks allow the SMB client to cache file data locally without re-reading from the server on every access, significantly improving throughput for frequently accessed files.",
            Tags = ["smb", "oplocks", "performance", "server"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "EnableOpLocks", 1)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "EnableOpLocks")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "EnableOpLocks", 1)],
        },
        new TweakDef
        {
            Id = "smbsrv-enable-forced-logoff",
            Label = "Disconnect SMB Sessions When Logon Hours Expire",
            Category = "SMB Server Policy",
            Description =
                "Sets EnableForcedLogoff=1 in LanmanServer parameters. Instructs the SMB server to forcibly disconnect client sessions whose account logon hours have expired instead of allowing the session to continue indefinitely. Enforces time-based access policies set in Active Directory.",
            Tags = ["smb", "session", "logoff", "policy", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "EnableForcedLogoff", 1)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "EnableForcedLogoff")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "EnableForcedLogoff", 1)],
        },
        new TweakDef
        {
            Id = "smbsrv-set-irp-stack-size",
            Label = "Increase SMB IRP Stack Size to 15",
            Category = "SMB Server Policy",
            Description =
                "Sets IRPStackSize=15 in LanmanServer parameters. Increases the Windows I/O Request Packet stack depth for the SMB server. The default of 11 can be insufficient for deeply nested filter drivers (antivirus, DLP, encryption). Value 15 is Microsoft's recommended increase when SMB errors occur with third-party filters.",
            Tags = ["smb", "irp", "performance", "server", "tuning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "IRPStackSize", 15)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "IRPStackSize")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "IRPStackSize", 15)],
        },
        new TweakDef
        {
            Id = "smbsrv-max-mpx-count",
            Label = "Set SMB Max Multiplex Count to 2048",
            Category = "SMB Server Policy",
            Description =
                "Sets MaxMpxCt=2048 in LanmanServer parameters. Controls the maximum number of simultaneous outstanding SMB requests the server will accept per client connection. Default is 50 which can be a bottleneck for workloads that use many concurrent file operations (e.g. software build servers or archive tools).",
            Tags = ["smb", "mpx", "performance", "server", "tuning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "MaxMpxCt", 2048)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "MaxMpxCt")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "MaxMpxCt", 2048)],
        },
        new TweakDef
        {
            Id = "smbsrv-max-work-items",
            Label = "Set SMB Server Work Item Pool to 8192",
            Category = "SMB Server Policy",
            Description =
                "Sets MaxWorkItems=8192 in LanmanServer parameters. Increases the SMB server work-item thread pool from the default 4096 to 8192 entries. Work items handle incoming client SMB requests; a larger pool reduces the chance of request queuing delays under high simultaneous connection counts.",
            Tags = ["smb", "workitems", "performance", "server", "tuning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "MaxWorkItems", 8192)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "MaxWorkItems")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "MaxWorkItems", 8192)],
        },
        new TweakDef
        {
            Id = "smbsrv-enable-raw-mode",
            Label = "Enable SMB Raw Read/Write Mode",
            Category = "SMB Server Policy",
            Description =
                "Sets EnableRaw=1 in LanmanServer parameters. Ensures the SMB server permits raw-mode transfers (large single-command reads and writes without the overhead of a separate setup packet). Raw mode is the default; restoring it if previously disabled improves LAN performance for large file copies.",
            Tags = ["smb", "raw", "performance", "server", "tuning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "EnableRaw", 1)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "EnableRaw")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "EnableRaw", 1)],
        },
        new TweakDef
        {
            Id = "smbsrv-set-size-req-buf",
            Label = "Set SMB Server Request Buffer Size to 4356",
            Category = "SMB Server Policy",
            Description =
                "Sets SizReqBuf=4356 in LanmanServer parameters. Configures the raw-mode read buffer size for the SMB server. 4356 bytes aligns the buffer to a common Ethernet MTU boundary (4 KB + SMB header overhead), which can reduce fragmented TCP segments for raw SMB operations on Gigabit networks.",
            Tags = ["smb", "buffer", "performance", "server", "tuning"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "SizReqBuf", 4356)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "SizReqBuf")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "SizReqBuf", 4356)],
        },
        new TweakDef
        {
            Id = "smbsrv-disk-space-threshold",
            Label = "Require 10% Free Disk Before SMB Writes",
            Category = "SMB Server Policy",
            Description =
                "Sets DiskSpaceThreshold=10 in LanmanServer parameters. Instructs the SMB server to return a disk-full error to clients when the volume hosting a share has less than 10% free space remaining, rather than waiting until the volume is completely full. Prevents total disk exhaustion which can corrupt open files.",
            Tags = ["smb", "disk", "threshold", "server", "reliability"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(SmbSrv, "DiskSpaceThreshold", 10)],
            RemoveOps = [RegOp.DeleteValue(SmbSrv, "DiskSpaceThreshold")],
            DetectOps = [RegOp.CheckDword(SmbSrv, "DiskSpaceThreshold", 10)],
        },
    ];
}
