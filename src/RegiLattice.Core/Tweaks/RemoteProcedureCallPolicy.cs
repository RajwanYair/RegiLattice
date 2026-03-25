// RegiLattice.Core — Tweaks/RemoteProcedureCallPolicy.cs
// Sprint 309: RPC Policy tweaks (10 tweaks)
// Category: "RPC Policy" | Slug: rpcpol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemoteProcedureCallPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rpcpol-enable-rpc-authentication",
            Label = "Enable RPC Endpoint Mapper Authentication",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "The RPC Endpoint Mapper receives incoming RPC connections and directs them to the appropriate server processes without authentication by default. Enabling authentication on the RPC Endpoint Mapper requires that callers authenticate before the mapper reveals service endpoint locations. Unauthenticated mapper access allows external parties to enumerate available RPC services on the endpoint which aids attack reconnaissance. Authentication requirements prevent unauthenticated sweep attacks that map available RPC services across enterprise networks. Endpoint mapper authentication is part of RPC hardening guidance from Microsoft and security assessors. This setting improves RPC security without degrading functionality for authenticated users and systems.",
            Tags = ["rpc", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableAuthEpResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableAuthEpResolution")],
            DetectOps = [RegOp.CheckDword(Key, "EnableAuthEpResolution", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-unauthenticated-rpc",
            Label = "Restrict Unauthenticated RPC Calls",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 3,
            Description =
                "RPC servers can be configured to reject unauthenticated connections and require that callers provide authentication credentials before being served. Restricting unauthenticated RPC prevents unknown callers from invoking RPC services without establishing an authenticated identity. Unauthenticated RPC calls can be used for reconnaissance and exploitation of services that implement insufficient authorization. Authenticated RPC provides the identity context necessary for access logging and auditing of service invocations. Enterprise hardening requirements typically include restricting unauthenticated access to RPC services on managed endpoints. Restricting unauthenticated RPC may require testing as some legitimate services use unauthenticated RPC for specific operational purposes.",
            Tags = ["rpc", "authentication", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictRemoteClients", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictRemoteClients")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictRemoteClients", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-enable-rpc-message-integrity",
            Label = "Enable RPC Connection Message Integrity",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "RPC message integrity uses message authentication codes to verify that RPC call payloads have not been tampered with in transit. Enabling RPC message integrity protects against man-in-the-middle attacks that modify RPC payloads after authentication is established. Authenticated RPC sessions without integrity protection are vulnerable to post-authentication payload manipulation. Message integrity checking adds minimal overhead while providing strong protection against active attacks on RPC communications. Enterprise security policies should require integrity protection for all network communications including internal RPC services. RPC integrity is supported by all modern Windows authentication mechanisms including Kerberos and NTLM.",
            Tags = ["rpc", "integrity", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinimumConnectionTimeout", 120)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinimumConnectionTimeout")],
            DetectOps = [RegOp.CheckDword(Key, "MinimumConnectionTimeout", 120)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-rpc-over-http",
            Label = "Disable RPC over HTTP Proxy",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "RPC over HTTP allows RPC calls to be tunneled through HTTP connections enabling RPC communication across firewalls that block native RPC ports. Disabling RPC over HTTP prevents the use of HTTP as a transport mechanism for RPC communications from managed endpoints. RPC over HTTP is used primarily for Exchange Outlook Anywhere connectivity and is not needed for standard enterprise operations. HTTP tunnaling of RPC can bypass network security controls designed to monitor and filter RPC protocol communications. Network perimeter security tools are less effective at inspecting and controlling RPC-over-HTTP compared to native RPC traffic. Environments using Exchange Online or modern authentication methods have no need for legacy RPC over HTTP connections.",
            Tags = ["rpc", "http", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRPCOverHTTP", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRPCOverHTTP")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRPCOverHTTP", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-enforce-rpc-security-callback",
            Label = "Enforce RPC Security Callback Verification",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "RPC servers can implement security callback functions that verify caller permissions before processing each RPC call. Requiring security callback enforcement ensures that RPC servers always invoke their security callback functions and cannot be bypassed. Vulnerabilities in RPC callback handling have allowed attackers to bypass authorization and invoke privileged RPC operations. Enforcing the security callback policy requires that all registered RPC permission verification code actually executes and cannot be skipped. This reduces the risk of RPC service implementation flaws that may conditionally skip credential validation. Security callback enforcement is a defense-in-depth measure ensuring RPC access control code is always exercised.",
            Tags = ["rpc", "security", "verification", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceRpcSecurityCallback", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceRpcSecurityCallback")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceRpcSecurityCallback", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-rpc-logging",
            Label = "Enable RPC Connection Logging",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "RPC connection logging records information about incoming RPC connections including caller identity, endpoint invoked, and connection metadata. Enabling RPC logging provides visibility into RPC traffic patterns and supports forensic investigation of RPC-based attacks. Detecting anomalous RPC activity requires that connection records be available for analysis by security teams. RPC logging data is written to the Windows Event Log and can be forwarded to SIEM systems for correlation. Organizations investigating incidents rely on RPC logs to understand attack paths that used Windows RPC-based lateral movement. Enabling logging has minimal performance impact and provides significant security value for detection and forensics.",
            Tags = ["rpc", "logging", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableConnectionLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableConnectionLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableConnectionLogging", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-set-rpc-call-timeout",
            Label = "Set RPC Call Timeout Limit",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "RPC calls that do not complete within a reasonable time can indicate server overload, network problems, or active attacks consuming RPC processing resources. Setting an RPC call timeout ensures that long-running or hung RPC calls are terminated rather than consuming resources indefinitely. Denial of service attacks can exploit unlimited RPC call duration by submitting calls designed to consume server processing resources. Reasonable timeout limits ensure that endpoints remain responsive even when individual RPC calls encounter unexpected delays. RPC timeouts should be set according to legitimate business requirements while remaining low enough to limit malicious resource consumption. Legitimate fast-completing RPC operations are completely unaffected by call timeout settings.",
            Tags = ["rpc", "timeout", "performance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinCallTimeout", 240)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinCallTimeout")],
            DetectOps = [RegOp.CheckDword(Key, "MinCallTimeout", 240)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-rpc-ncalrpc-transport",
            Label = "Restrict RPC NCALRPC Local Transport",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "NCALRPC is the Named Pipe-based local RPC transport used for inter-process communication on the local system without network involvement. Restricting NCALRPC transport prevents certain local RPC escalation techniques that exploit local pipe-based RPC connections. Privilege escalation through local-only RPC transports has been demonstrated in several Windows elevation of privilege vulnerabilities. Limiting NCALRPC availability constrains the attack surface available for local privilege escalation through RPC-based mechanisms. Server applications that legitimately use local RPC communication may need to be assessed before restricting this transport. This setting requires careful evaluation as it may affect legitimate inter-process communication on the endpoint.",
            Tags = ["rpc", "local", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLocalRpc", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalRpc")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLocalRpc", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-rpc-anon-auth",
            Label = "Disable Anonymous RPC Authentication Level",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Anonymous authentication in RPC allows callers to invoke RPC services without presenting any identity credentials. Disabling anonymous RPC authentication prevents services from accepting calls from undisclosed callers with no authentication information. Allowing anonymous RPC creates significant access control risks as authorization decisions cannot be based on caller identity. Authentication-challenged RPC calls can expose sensitive services to any endpoint that can reach the RPC port. Enterprise environments should not allow anonymous access to internal services where authentication is technically feasible. Disabling anonymous RPC does not affect authenticated access which continues to work with proper credentials.",
            Tags = ["rpc", "anonymous", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MinimumAuthenticationLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "MinimumAuthenticationLevel")],
            DetectOps = [RegOp.CheckDword(Key, "MinimumAuthenticationLevel", 1)],
        },
        new TweakDef
        {
            Id = "rpcpol-disable-rpc-portrange-override",
            Label = "Disable RPC Dynamic Port Range Override",
            Category = "RPC Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "RPC uses dynamic port allocation for many services assigning ports from a configurable range for each new connection. Disabling dynamic port range overrides ensures that RPC uses the restricted Windows default port range making firewall rule management consistent. Unrestricted dynamic port ranges make firewall rules for RPC communications impractical requiring either wide port range rules or no firewall protection. The restricted Windows RPC port range provides a reasonable balance between flexibility and firewall manageability. Consistent port ranges enable network security teams to write targeted firewall rules instead of allowing all high-port UDP and TCP traffic. Organizations should align their firewall rules with the configured RPC port range for consistent network security policy.",
            Tags = ["rpc", "ports", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "PortsInternetAvailable", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "PortsInternetAvailable")],
            DetectOps = [RegOp.CheckDword(Key, "PortsInternetAvailable", 0)],
        },
    ];
}
