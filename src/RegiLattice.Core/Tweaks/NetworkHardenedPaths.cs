#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Network Hardened Paths — UNC path authentication requirements and WebDAV client security.
// HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider\HardenedPaths
// HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient\Parameters
internal static class NetworkHardenedPaths
{
    private const string HardenedPaths = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkProvider\HardenedPaths";
    private const string WebClient = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WebClient\Parameters";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "nethpth-harden-sysvol",
            Label = "Network: Require Mutual Auth + Integrity for SYSVOL Shares",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HardenedPaths],
            Tags = ["network", "unc", "sysvol", "smb", "hardening", "mitm"],
            Description =
                @"Sets \\*\SYSVOL = ""RequireMutualAuthentication=1, RequireIntegrity=1"" in UNC Hardened Paths. "
                + "Prevents SYSVOL share access over unauthenticated or integrity-unprotected channels, "
                + "blocking pass-the-hash and relay attacks on domain share access. Default: not hardened.",
            ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\SYSVOL", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\SYSVOL")],
            DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\SYSVOL", "RequireMutualAuthentication=1, RequireIntegrity=1")],
        },
        new TweakDef
        {
            Id = "nethpth-harden-netlogon",
            Label = "Network: Require Mutual Auth + Integrity for NETLOGON Shares",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HardenedPaths],
            Tags = ["network", "unc", "netlogon", "smb", "hardening", "mitm"],
            Description =
                @"Sets \\*\NETLOGON = ""RequireMutualAuthentication=1, RequireIntegrity=1"". "
                + "Ensures NETLOGON share authentication against MITM attacks and relay attacks. "
                + "Critical for domain-joined machines. Default: not hardened.",
            ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\NETLOGON", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\NETLOGON")],
            DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\NETLOGON", "RequireMutualAuthentication=1, RequireIntegrity=1")],
        },
        new TweakDef
        {
            Id = "nethpth-harden-admin-shares",
            Label = "Network: Require Authentication for All Admin UNC Shares",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HardenedPaths],
            Tags = ["network", "unc", "admin-share", "hardening", "lateral-movement"],
            Description =
                @"Sets \\*\* (wildcard) = ""RequireAuthentication=1"" in UNC Hardened Paths. "
                + "Requires SMB authentication on all UNC paths system-wide. "
                + "Broad protection against unauthenticated lateral movement. Default: not enforced.",
            ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\*", "RequireAuthentication=1")],
            RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\*")],
            DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\*", "RequireAuthentication=1")],
        },
        new TweakDef
        {
            Id = "nethpth-harden-ipc-share",
            Label = "Network: Require Mutual Authentication for IPC$ Shares",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HardenedPaths],
            Tags = ["network", "unc", "ipc", "smb", "hardening", "security"],
            Description =
                @"Sets \\*\IPC$ = ""RequireMutualAuthentication=1"" in UNC Hardened Paths. "
                + "Hardens connections to the IPC$ null session share used for named pipes and RPCs. "
                + "Default: unauthenticated access allowed to IPC$ from some tools.",
            ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1")],
            RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\IPC$")],
            DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1")],
        },
        new TweakDef
        {
            Id = "nethpth-harden-ipc-integrity",
            Label = "Network: Require Integrity for IPC$ Shares",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [HardenedPaths],
            Tags = ["network", "unc", "ipc", "integrity", "smb-signing", "hardening"],
            Description =
                @"Sets \\*\IPC$ = ""RequireMutualAuthentication=1, RequireIntegrity=1"" in UNC Hardened Paths. "
                + "Full mutual authentication plus packet integrity (SMB signing) on IPC$ connections. "
                + "Strongest IPC$ hardening. Supersedes nethpth-harden-ipc-share.",
            ApplyOps = [RegOp.SetString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1, RequireIntegrity=1")],
            RemoveOps = [RegOp.DeleteValue(HardenedPaths, @"\\*\IPC$")],
            DetectOps = [RegOp.CheckString(HardenedPaths, @"\\*\IPC$", "RequireMutualAuthentication=1, RequireIntegrity=1")],
        },
        new TweakDef
        {
            Id = "nethpth-disable-webdav-basic-auth",
            Label = "Network: Disable WebDAV Client Basic Authentication",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [WebClient],
            Tags = ["webdav", "webclient", "basic-auth", "network", "security"],
            Description =
                "Sets BasicAuthLevel=0 in WebClient service parameters. Prevents the WebDAV client from "
                + "sending credentials as plaintext in HTTP Basic Authentication headers. "
                + "Default: BasicAuthLevel=1 (disabled over HTTP, allowed over HTTPS). Set 0 to block entirely.",
            ApplyOps = [RegOp.SetDword(WebClient, "BasicAuthLevel", 0)],
            RemoveOps = [RegOp.DeleteValue(WebClient, "BasicAuthLevel")],
            DetectOps = [RegOp.CheckDword(WebClient, "BasicAuthLevel", 0)],
        },
        new TweakDef
        {
            Id = "nethpth-limit-webdav-file-size",
            Label = "Network: Cap WebDAV File Transfer Size at 50 MB",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [WebClient],
            Tags = ["webdav", "webclient", "file-size", "network", "data-loss"],
            Description =
                "Sets FileSizeLimitInBytes=52428800 (50 MB) in WebClient parameters. Limits the maximum file "
                + "size the WebDAV client can upload or download in a single operation. "
                + "Default: 47 MB (Windows default). Reduces risk of bulk data exfiltration via WebDAV.",
            ApplyOps = [RegOp.SetDword(WebClient, "FileSizeLimitInBytes", 52428800)],
            RemoveOps = [RegOp.DeleteValue(WebClient, "FileSizeLimitInBytes")],
            DetectOps = [RegOp.CheckDword(WebClient, "FileSizeLimitInBytes", 52428800)],
        },
        new TweakDef
        {
            Id = "nethpth-webdav-connection-timeout",
            Label = "Network: Reduce WebDAV Connection Timeout to 60 Seconds",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [WebClient],
            Tags = ["webdav", "webclient", "timeout", "network", "performance"],
            Description =
                "Sets ConnectionTimeout=60000 ms in WebClient parameters. Reduces blocking time when "
                + "WebDAV connections to unavailable servers are attempted. "
                + "Default: 60 000 ms. Lower values improve responsiveness when remote shares are offline.",
            ApplyOps = [RegOp.SetDword(WebClient, "ConnectionTimeout", 60000)],
            RemoveOps = [RegOp.DeleteValue(WebClient, "ConnectionTimeout")],
            DetectOps = [RegOp.CheckDword(WebClient, "ConnectionTimeout", 60000)],
        },
        new TweakDef
        {
            Id = "nethpth-webdav-send-timeout",
            Label = "Network: Reduce WebDAV Send Timeout to 30 Seconds",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [WebClient],
            Tags = ["webdav", "webclient", "timeout", "network", "performance"],
            Description =
                "Sets SendTimeout=30000 ms in WebClient parameters. Cuts time the client blocks waiting "
                + "for a WebDAV server to confirm reception of a request. "
                + "Default: 30 000 ms. Helps detect stalled upload sessions faster.",
            ApplyOps = [RegOp.SetDword(WebClient, "SendTimeout", 30000)],
            RemoveOps = [RegOp.DeleteValue(WebClient, "SendTimeout")],
            DetectOps = [RegOp.CheckDword(WebClient, "SendTimeout", 30000)],
        },
        new TweakDef
        {
            Id = "nethpth-webdav-receive-timeout",
            Label = "Network: Reduce WebDAV Receive Timeout to 60 Seconds",
            Category = "Network Hardened Paths",
            NeedsAdmin = true,
            CorpSafe = false,
            RegistryKeys = [WebClient],
            Tags = ["webdav", "webclient", "timeout", "network", "performance"],
            Description =
                "Sets ReceiveTimeout=60000 ms in WebClient parameters. Limits how long the client waits "
                + "for a server response after sending a WebDAV request. "
                + "Default: 60 000 ms. Conservative value for typical enterprise network latency.",
            ApplyOps = [RegOp.SetDword(WebClient, "ReceiveTimeout", 60000)],
            RemoveOps = [RegOp.DeleteValue(WebClient, "ReceiveTimeout")],
            DetectOps = [RegOp.CheckDword(WebClient, "ReceiveTimeout", 60000)],
        },
    ];
}
