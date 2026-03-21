// RegiLattice.Core — Tweaks/RemoteManagement.cs
// WinRM policy hardening and RPC restriction tweaks.
// Slug: "rmt" — targeted at all Windows 10/11 editions; most are CorpSafe.

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemoteManagement
{
    private const string WinRmSvcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
    private const string WinRmCliPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
    private const string RpcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";
    private const string WinRmSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rmt-disable-winrm-service",
            Label = "Disable WinRM Service",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the WinRM (Windows Remote Management) service start type to Disabled. "
                + "Prevents PowerShell Remoting and remote WMI sessions when no remote management is needed. "
                + "Default: Manual. Recommended: Disabled on workstations not managed via WinRM.",
            Tags = ["winrm", "remote", "powershell", "security", "hardening"],
            RegistryKeys = [WinRmSvc],
            ApplyOps = [RegOp.SetDword(WinRmSvc, "Start", 4)],
            RemoveOps = [RegOp.SetDword(WinRmSvc, "Start", 3)],
            DetectOps = [RegOp.CheckDword(WinRmSvc, "Start", 4)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-unencrypted-server",
            Label = "Block Unencrypted WinRM Traffic (Server)",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowUnencryptedTraffic=0 for the WinRM service (server-side). "
                + "Forces all incoming WinRM sessions to use HTTPS/encrypted transport. "
                + "Prevents credential interception on WinRM connections. Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "encryption", "security", "policy", "hardening"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "AllowUnencryptedTraffic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "AllowUnencryptedTraffic")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "AllowUnencryptedTraffic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-basic-auth-server",
            Label = "Block Basic Authentication in WinRM Server",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowBasic=0 for the WinRM service. Prevents clients from authenticating using "
                + "Basic HTTP authentication (plaintext Base64 credentials). Kerberos/Negotiate remain available. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "security", "policy", "hardening"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-unencrypted-client",
            Label = "Block Unencrypted WinRM Traffic (Client)",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowUnencryptedTraffic=0 for the WinRM client. "
                + "Prevents this machine from initiating unencrypted WinRM sessions to remote hosts. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "encryption", "security", "policy", "client"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowUnencryptedTraffic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowUnencryptedTraffic")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowUnencryptedTraffic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-basic-auth-client",
            Label = "Block Basic Authentication in WinRM Client",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowBasic=0 for the WinRM client. Prevents using Basic authentication "
                + "when connecting to WinRM servers. Closes NTLM relay paths via Basic credentials. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "security", "policy", "client"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-digest-auth",
            Label = "Block Digest Authentication in WinRM Client",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowDigest=0 for the WinRM client. Digest authentication uses MD5 hashing "
                + "and is considered weak. Blocking it forces Kerberos/Negotiate. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "digest", "security", "policy"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowDigest", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowDigest")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowDigest", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-credssp",
            Label = "Block CredSSP in WinRM Client",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowCredSSP=0 for the WinRM client. CredSSP delegates credentials to the "
                + "remote host, creating credential theft risk on compromised endpoints. "
                + "Default: allowed. Recommended: blocked unless explicitly needed.",
            Tags = ["winrm", "credssp", "credential", "security", "policy"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowCredSSP", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowCredSSP")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowCredSSP", 0)],
        },
        new TweakDef
        {
            Id = "rmt-restrict-rpc-clients",
            Label = "Restrict Unauthenticated RPC Clients",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RPC policy RestrictRemoteClients=1 to block unauthenticated remote RPC connections. "
                + "Authenticated connections and loopback are still permitted. "
                + "Default: 0 (no restriction). Recommended: 1 on servers/workstations.",
            Tags = ["rpc", "remote", "authentication", "security", "policy"],
            RegistryKeys = [RpcPol],
            ApplyOps = [RegOp.SetDword(RpcPol, "RestrictRemoteClients", 1)],
            RemoveOps = [RegOp.DeleteValue(RpcPol, "RestrictRemoteClients")],
            DetectOps = [RegOp.CheckDword(RpcPol, "RestrictRemoteClients", 1)],
        },
        new TweakDef
        {
            Id = "rmt-require-rpc-auth-ep",
            Label = "Require Authentication for RPC Endpoint Resolution",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RPC policy EnableAuthEpResolution=1. The RPC endpoint mapper requires "
                + "clients to authenticate when resolving remote endpoints. "
                + "Prevents unauthenticated enumeration of RPC services. Default: 0.",
            Tags = ["rpc", "endpoint", "authentication", "security", "policy"],
            RegistryKeys = [RpcPol],
            ApplyOps = [RegOp.SetDword(RpcPol, "EnableAuthEpResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(RpcPol, "EnableAuthEpResolution")],
            DetectOps = [RegOp.CheckDword(RpcPol, "EnableAuthEpResolution", 1)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-limit-shell-memory",
            Label = "Limit WinRM Shell Memory Per Session",
            Category = "Remote Management",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxMemoryPerShellMB=150 for the WinRM service. Caps the amount of memory "
                + "each remote shell session can consume, preventing WinRM from being used as a "
                + "DoS vector on low-memory machines. Default: unlimited. Recommended: 150 MB.",
            Tags = ["winrm", "memory", "dos", "security", "policy"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "MaxMemoryPerShellMB", 150)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "MaxMemoryPerShellMB")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "MaxMemoryPerShellMB", 150)],
        },
    ];
}
