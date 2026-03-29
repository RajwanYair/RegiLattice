// RegiLattice.Core — Tweaks/ProxyBypassPolicy.cs
// Proxy settings enforcement and bypass-prevention via Group Policy — Sprint 441.
// Disables auto-detect, WPAD, direct internet access, and proxy-settings change by users;
// enforces authenticated proxy, locks settings, and disables VPN split tunneling.
// Category: "Proxy Bypass Policy" | Slug: proxbyp
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings
//           HKLM\SOFTWARE\Policies\Microsoft\Internet Explorer\Control Panel

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ProxyBypassPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";
    private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Internet Explorer\Control Panel";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "proxbyp-disable-autodetect",
                Label = "Disable Auto-Detect Proxy Settings",
                Category = "Proxy Bypass Policy",
                Description =
                    "Disables automatic proxy detection (AutoDetect=0), preventing browsers and WinINET from trying to discover a proxy server automatically via WPAD or DHCP.",
                Tags = ["proxy", "autodetect", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Stops auto-detection; clients must use explicitly configured proxy settings.",
                ApplyOps = [RegOp.SetDword(Key, "AutoDetect", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AutoDetect")],
                DetectOps = [RegOp.CheckDword(Key, "AutoDetect", 0)],
            },
            new TweakDef
            {
                Id = "proxbyp-disable-autoconfig-url",
                Label = "Disable Proxy Auto-Config URL",
                Category = "Proxy Bypass Policy",
                Description =
                    "Disables policy-driven auto-configuration URL (PAC file) processing by forcing ProxyAutoConfigUrl to an empty value, preventing unauthorized PAC file adoption.",
                Tags = ["proxy", "pac", "autoconfig", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables PAC file; proxy must be explicitly set or controlled by separate policy.",
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoConfigUrl", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConfigUrl")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoConfigUrl", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-block-settings-change",
                Label = "Block Proxy Settings Change by Users",
                Category = "Proxy Bypass Policy",
                Description =
                    "Locks the Connections settings page in Internet Properties so standard users cannot modify proxy settings (Connections=1 in IE Control Panel policy).",
                Tags = ["proxy", "settings", "lockdown", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents user-side proxy bypass; admin privileges required to change proxy settings.",
                ApplyOps = [RegOp.SetDword(Key2, "Connections", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "Connections")],
                DetectOps = [RegOp.CheckDword(Key2, "Connections", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-disable-wpad",
                Label = "Disable WPAD (Web Proxy Auto-Discovery)",
                Category = "Proxy Bypass Policy",
                Description =
                    "Disables WPAD by setting DisableWpad=1, preventing the client from broadcasting WS-Discovery or DNS queries for a WPAD host, which can be spoofed by attackers.",
                Tags = ["proxy", "wpad", "network", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Eliminates WPAD attack surface; explicit proxy configuration required.",
                ApplyOps = [RegOp.SetDword(Key, "DisableWpad", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWpad")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWpad", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-require-authenticated-proxy",
                Label = "Require Authenticated Proxy",
                Category = "Proxy Bypass Policy",
                Description =
                    "Enforces proxy server authentication requirement, ensuring all WinINET proxy connections use valid enterprise credentials.",
                Tags = ["proxy", "authentication", "network", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Authenticated proxy prevents anonymous internet access via the corporate proxy.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAuthenticatedProxy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAuthenticatedProxy")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAuthenticatedProxy", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-block-direct-internet",
                Label = "Block Direct Internet Access",
                Category = "Proxy Bypass Policy",
                Description =
                    "Blocks direct internet connections by policy, forcing all external traffic through the configured enterprise proxy server.",
                Tags = ["proxy", "internet", "network", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Enforces proxy-only outbound; direct connections to internet IPs are blocked.",
                ApplyOps = [RegOp.SetDword(Key, "BlockDirectInternetAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDirectInternetAccess")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDirectInternetAccess", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-disable-bypass-for-local",
                Label = "Disable Proxy Bypass for Local Addresses",
                Category = "Proxy Bypass Policy",
                Description =
                    "Disables the default proxy bypass for local (intranet) addresses so that all traffic — including intranet — routes through the proxy for inspection.",
                Tags = ["proxy", "local", "bypass", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Routes intranet traffic through proxy; may increase latency on local resources.",
                ApplyOps = [RegOp.SetDword(Key, "DisableBypassForLocal", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBypassForLocal")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBypassForLocal", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-lock-proxy-settings",
                Label = "Lock Proxy Settings from Changes",
                Category = "Proxy Bypass Policy",
                Description =
                    "Applies the proxy-settings lockdown policy so that users cannot view or change proxy configuration through the Internet Options dialog.",
                Tags = ["proxy", "lockdown", "settings", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Internet Options Connections tab locked; admin-only access to proxy config.",
                ApplyOps = [RegOp.SetDword(Key2, "Advanced", 1)],
                RemoveOps = [RegOp.DeleteValue(Key2, "Advanced")],
                DetectOps = [RegOp.CheckDword(Key2, "Advanced", 1)],
            },
            new TweakDef
            {
                Id = "proxbyp-enforce-proxy-server",
                Label = "Enforce Proxy Server Policy Setting",
                Category = "Proxy Bypass Policy",
                Description =
                    "Enables the ProxySettingsPerUser=0 policy to enforce machine-wide proxy settings rather than per-user, preventing individual users from substituting their own proxy.",
                Tags = ["proxy", "server", "machine", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Machine-wide proxy enforced; per-user proxy overrides are blocked.",
                ApplyOps = [RegOp.SetDword(Key, "ProxySettingsPerUser", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ProxySettingsPerUser")],
                DetectOps = [RegOp.CheckDword(Key, "ProxySettingsPerUser", 0)],
            },
            new TweakDef
            {
                Id = "proxbyp-disable-vpn-split-tunneling",
                Label = "Disable VPN Split Tunneling via Proxy Policy",
                Category = "Proxy Bypass Policy",
                Description =
                    "Disables VPN split tunneling enforcement bypass by requiring all traffic to route through the proxy, eliminating the split-tunnel proxy-bypass vector.",
                Tags = ["proxy", "vpn", "split-tunneling", "hardening", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Forces all traffic through proxy; VPN split-tunnel internet bypass is eliminated.",
                ApplyOps = [RegOp.SetDword(Key, "DisableSplitTunnelingBypass", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSplitTunnelingBypass")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSplitTunnelingBypass", 1)],
            },
        ];
}
