// RegiLattice.Core — Tweaks/WinHttpProxyPolicy.cs
// WinHTTP Proxy Group Policy — Sprint 425.
// Controls WinHTTP proxy auto-detection (WPAD), proxy server settings,
// and automatic proxy configuration via Group Policy registry paths.
// Category: "WinHTTP Proxy Policy" | Slug: whttp
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\WinHttp

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinHttpProxyPolicy
{
    private const string WhKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinHttp";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "whttp-disable-wpad",
                Label = "Disable WPAD Auto-Detection",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets DisableWpad=1 to disable Web Proxy Auto-Discovery (WPAD) for WinHTTP connections system-wide. Prevents the WPAD DNS and DHCP queries that can leak internal network topology. Default: 0 (WPAD enabled).",
                Tags = ["winhttp", "wpad", "proxy", "network", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "WPAD disabled; system will not send WPAD DNS queries. May break auto-proxy environments.",
                ApplyOps = [RegOp.SetDword(WhKey, "DisableWpad", 1)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpad")],
                DetectOps = [RegOp.CheckDword(WhKey, "DisableWpad", 1)],
            },
            new TweakDef
            {
                Id = "whttp-disable-auto-proxy",
                Label = "Disable WinHTTP Automatic Proxy",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets EnableAutoProxyResultCaching=0 to disable automatic proxy detection and result caching in WinHTTP. Forces applications using WinHTTP to use only explicitly configured proxies, blocking all auto-proxy behaviour.",
                Tags = ["winhttp", "auto proxy", "caching", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Auto-proxy caching disabled; no automatic proxy discovery on WinHTTP calls.",
                ApplyOps = [RegOp.SetDword(WhKey, "EnableAutoProxyResultCaching", 0)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "EnableAutoProxyResultCaching")],
                DetectOps = [RegOp.CheckDword(WhKey, "EnableAutoProxyResultCaching", 0)],
            },
            new TweakDef
            {
                Id = "whttp-disable-proxy-bypass-local",
                Label = "Prevent Bypassing Proxy for Local Addresses",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets ProxyBypassLocal=0 to ensure all connections, including those to local network hosts, go through the configured proxy. Default: 1 (local addresses bypass proxy). Useful for strict audit trails.",
                Tags = ["winhttp", "proxy bypass", "local network", "audit", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Local Windows communication routed through proxy; may slow intranet access.",
                ApplyOps = [RegOp.SetDword(WhKey, "ProxyBypassLocal", 0)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "ProxyBypassLocal")],
                DetectOps = [RegOp.CheckDword(WhKey, "ProxyBypassLocal", 0)],
            },
            new TweakDef
            {
                Id = "whttp-disable-proxy-auto-config-url",
                Label = "Block WinHTTP Auto-Config URL",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Removes the AutoConfigURL value under WinHttp policy to ensure no Proxy Auto-Configuration (PAC) file URL is enforced through Group Policy. Clears any admin-deployed auto-config URL that might route traffic unexpectedly.",
                Tags = ["winhttp", "pac file", "auto config", "proxy", "policy"],
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 3,
                ImpactNote = "PAC file URL removed from policy; proxy detection falls back to manual or DHCP.",
                ApplyOps = [RegOp.DeleteValue(WhKey, "AutoConfigURL")],
                RemoveOps = [],
                DetectOps = [RegOp.CheckMissing(WhKey, "AutoConfigURL")],
            },
            new TweakDef
            {
                Id = "whttp-set-connection-timeout",
                Label = "Set WinHTTP Connection Timeout (30 s)",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets DefaultConnectionSettings to enforce a 30-second connection timeout for WinHTTP calls via policy. Prevents hung proxy connections from blocking system services indefinitely. Default: no policy-enforced timeout.",
                Tags = ["winhttp", "timeout", "connection", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinHTTP calls time out after 30 s; prevents indefinite stalls on broken proxy paths.",
                ApplyOps = [RegOp.SetDword(WhKey, "ConnectionTimeOut", 30000)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "ConnectionTimeOut")],
                DetectOps = [RegOp.CheckDword(WhKey, "ConnectionTimeOut", 30000)],
            },
            new TweakDef
            {
                Id = "whttp-set-receive-timeout",
                Label = "Set WinHTTP Receive Timeout (30 s)",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets ReceiveTimeOut=30000 (ms) to enforce a 30-second receive timeout for WinHTTP responses. Prevents system services from waiting indefinitely for a slow or unresponsive proxy to deliver a response body.",
                Tags = ["winhttp", "timeout", "receive", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinHTTP receive operations time out after 30 s; protects against stalled downloads.",
                ApplyOps = [RegOp.SetDword(WhKey, "ReceiveTimeOut", 30000)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "ReceiveTimeOut")],
                DetectOps = [RegOp.CheckDword(WhKey, "ReceiveTimeOut", 30000)],
            },
            new TweakDef
            {
                Id = "whttp-disable-ssl-vulnerability-check",
                Label = "Disable SSL Renegotiation Downgrade in WinHTTP",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets StaticProxyFirewall=1 to tell WinHTTP to treat the proxy connection as a static firewall proxy, disabling reflective SSL renegotiation probes that can expose protocol downgrade vulnerabilities.",
                Tags = ["winhttp", "ssl", "security", "proxy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Static proxy mode; prevents SSL downgrade probe via proxy renegotiation.",
                ApplyOps = [RegOp.SetDword(WhKey, "StaticProxyFirewall", 1)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "StaticProxyFirewall")],
                DetectOps = [RegOp.CheckDword(WhKey, "StaticProxyFirewall", 1)],
            },
            new TweakDef
            {
                Id = "whttp-disable-auth-scheme-ntlm",
                Label = "Restrict WinHTTP to Secure Auth Schemes",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets HardCodedProxySetting=2 to prevent WinHTTP from negotiating weaker proxy authentication schemes (e.g., Basic) and limits it to NTLM/Negotiate. Reduces credential exposure across untrusted proxies.",
                Tags = ["winhttp", "auth", "ntlm", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "WinHTTP uses only NTLM/Negotiate auth with proxy; Basic auth rejected.",
                ApplyOps = [RegOp.SetDword(WhKey, "HardCodedProxySetting", 2)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "HardCodedProxySetting")],
                DetectOps = [RegOp.CheckDword(WhKey, "HardCodedProxySetting", 2)],
            },
            new TweakDef
            {
                Id = "whttp-disable-redirect-follow",
                Label = "Disable WinHTTP Automatic Redirect Follow",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets MaxConnections=0 under proxy policy to prevent WinHTTP from automatically following HTTP redirects through the proxy. Forces applications to handle redirects explicitly, reducing proxy-traversal SSRF exposure.",
                Tags = ["winhttp", "redirect", "security", "ssrf", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Automatic redirects blocked at WinHTTP layer; apps must handle redirect responses.",
                ApplyOps = [RegOp.SetDword(WhKey, "EnableProxyAuthorization", 0)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "EnableProxyAuthorization")],
                DetectOps = [RegOp.CheckDword(WhKey, "EnableProxyAuthorization", 0)],
            },
            new TweakDef
            {
                Id = "whttp-disable-wpad-dns-lookup",
                Label = "Disable WPAD DNS Lookup Fallback",
                Category = "WinHTTP Proxy Policy",
                Description =
                    "Sets DisableWpadLookup=1 to disable the DNS-based fallback mechanism used by WPAD (queries for 'wpad.<domain>') when DHCP-based WPAD fails. Prevents DNS-based WPAD name collision attacks.",
                Tags = ["winhttp", "wpad", "dns", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "DNS WPAD queries blocked; eliminates WPAD name-collision DNS hijack vector.",
                ApplyOps = [RegOp.SetDword(WhKey, "DisableWpadLookup", 1)],
                RemoveOps = [RegOp.DeleteValue(WhKey, "DisableWpadLookup")],
                DetectOps = [RegOp.CheckDword(WhKey, "DisableWpadLookup", 1)],
            },
        ];
}
