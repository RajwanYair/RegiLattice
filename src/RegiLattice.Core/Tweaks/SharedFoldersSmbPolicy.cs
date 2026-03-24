#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

internal static class SharedFoldersSmbPolicy
{
    private const string LanWs = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";
    private const string LanSrv = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\LanmanServer\Parameters";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "smbshare-disable-insecure-guest-logons",
            Label = "Disable Insecure Guest Logons to SMB Servers",
            Category = "SMB Shared Folders Policy",
            Description = "Prevents the SMB client from allowing insecure guest logons to SMB file servers, protecting against credential theft.",
            Tags = ["smb", "network", "group-policy", "hardening", "guest"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanWs, "AllowInsecureGuestAuth", 0)],
            RemoveOps = [RegOp.DeleteValue(LanWs, "AllowInsecureGuestAuth")],
            DetectOps = [RegOp.CheckDword(LanWs, "AllowInsecureGuestAuth", 0)],
        },
        new TweakDef
        {
            Id = "smbshare-require-smb-signing-client",
            Label = "Require SMB Packet Signing (Client)",
            Category = "SMB Shared Folders Policy",
            Description = "Requires the SMB client to sign all packets, preventing man-in-the-middle and NTLM relay attacks.",
            Tags = ["smb", "network", "security", "hardening", "signing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "RequireSecuritySignature", 1)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "RequireSecuritySignature")],
            DetectOps = [RegOp.CheckDword(LanSrv, "RequireSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "smbshare-enable-smb-signing-server",
            Label = "Enable SMB Packet Signing (Server)",
            Category = "SMB Shared Folders Policy",
            Description = "Enables SMB packet signing on the server side, allowing signed connections from clients that request it.",
            Tags = ["smb", "network", "security", "hardening", "signing"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "EnableSecuritySignature", 1)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "EnableSecuritySignature")],
            DetectOps = [RegOp.CheckDword(LanSrv, "EnableSecuritySignature", 1)],
        },
        new TweakDef
        {
            Id = "smbshare-restrict-null-session-access",
            Label = "Restrict Null Session Access to Named Pipes and Shares",
            Category = "SMB Shared Folders Policy",
            Description =
                "Prevents anonymous (null session) connections from accessing named pipes and shares, blocking unauthenticated SMB enumeration.",
            Tags = ["smb", "network", "security", "hardening", "anonymous"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "RestrictNullSessAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "RestrictNullSessAccess")],
            DetectOps = [RegOp.CheckDword(LanSrv, "RestrictNullSessAccess", 1)],
        },
        new TweakDef
        {
            Id = "smbshare-clear-null-session-pipes",
            Label = "Clear Null Session Named Pipes List",
            Category = "SMB Shared Folders Policy",
            Description = "Removes all named pipes accessible via anonymous null sessions, reducing SMB attack surface.",
            Tags = ["smb", "network", "security", "hardening", "anonymous"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionPipes", [])],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionPipes")],
            DetectOps = [RegOp.CheckString(LanSrv, "NullSessionPipes", "")],
        },
        new TweakDef
        {
            Id = "smbshare-clear-null-session-shares",
            Label = "Clear Null Session Shares List",
            Category = "SMB Shared Folders Policy",
            Description = "Removes all shares accessible via anonymous null sessions, preventing unauthenticated share enumeration.",
            Tags = ["smb", "network", "security", "hardening", "anonymous"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetMultiSz(LanSrv, "NullSessionShares", [])],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "NullSessionShares")],
            DetectOps = [RegOp.CheckString(LanSrv, "NullSessionShares", "")],
        },
        new TweakDef
        {
            Id = "smbshare-auto-disconnect-idle-sessions",
            Label = "Auto-Disconnect Idle SMB Sessions After 15 Minutes",
            Category = "SMB Shared Folders Policy",
            Description =
                "Automatically disconnects idle SMB sessions after 15 minutes, freeing resources and reducing exposure of open connections.",
            Tags = ["smb", "network", "performance", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "AutoDisconnect", 15)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "AutoDisconnect")],
            DetectOps = [RegOp.CheckDword(LanSrv, "AutoDisconnect", 15)],
        },
        new TweakDef
        {
            Id = "smbshare-enable-forced-logoff",
            Label = "Enable Forced Logoff When Logon Hours Expire",
            Category = "SMB Shared Folders Policy",
            Description = "Forces a logoff when a user's permitted logon hours expire, ensuring access control policies are enforced.",
            Tags = ["smb", "network", "security", "group-policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "EnableForcedLogOff", 1)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "EnableForcedLogOff")],
            DetectOps = [RegOp.CheckDword(LanSrv, "EnableForcedLogOff", 1)],
        },
        new TweakDef
        {
            Id = "smbshare-disable-admin-shares",
            Label = "Disable Default Administrative SMB Shares",
            Category = "SMB Shared Folders Policy",
            Description = "Disables automatic creation of administrative shares (C$, ADMIN$, IPC$), reducing remote administrative access surface.",
            Tags = ["smb", "network", "security", "hardening"],
            NeedsAdmin = true,
            CorpSafe = false,
            ApplyOps = [RegOp.SetDword(LanSrv, "AutoShareWks", 0)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "AutoShareWks")],
            DetectOps = [RegOp.CheckDword(LanSrv, "AutoShareWks", 0)],
        },
        new TweakDef
        {
            Id = "smbshare-set-smb-max-connections",
            Label = "Set Maximum Concurrent SMB Connections",
            Category = "SMB Shared Folders Policy",
            Description = "Limits the number of concurrent SMB connections to 16,777,216 (MaxMpxCt), preventing resource exhaustion from SMB floods.",
            Tags = ["smb", "network", "performance", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ApplyOps = [RegOp.SetDword(LanSrv, "MaxMpxCt", 16777216)],
            RemoveOps = [RegOp.DeleteValue(LanSrv, "MaxMpxCt")],
            DetectOps = [RegOp.CheckDword(LanSrv, "MaxMpxCt", 16777216)],
        },
    ];
}
