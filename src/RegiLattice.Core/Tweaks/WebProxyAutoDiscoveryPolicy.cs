// RegiLattice.Core — Tweaks/WebProxyAutoDiscoveryPolicy.cs
// Web Proxy Auto-Discovery (WPAD) Policy — Sprint 545.
// Configures Group Policy for WPAD (Web Proxy Auto-Discovery Protocol),
// WinHTTP proxy settings, proxy server detection, PAC file security, and
// internet proxy configuration for corporate environments.
// Category: "Web Proxy Auto-Discovery Policy" | Slug: wpad
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WebProxyAutoDiscoveryPolicy
{
    private const string InetKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

    private const string WpadKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings\Wpad";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wpad-disable-auto-detect",
                Label = "WPAD: Disable Automatic Proxy Detection (WPAD Protocol)",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets AutoDetect=0 in WPAD policy. Disables Web Proxy Auto-Discovery Protocol (WPAD) which broadcasts DHCP/DNS queries to discover proxy configuration servers on the local network. WPAD is exploited in PoisonTap and similar attacks where an attacker's rogue DHCP or DNS server responds to WPAD queries, redirecting all HTTP/HTTPS traffic through an attacker-controlled proxy. Disabling WPAD and using explicit PAC file URLs or manual proxy configuration eliminates this attack surface.",
                Tags = ["wpad", "proxy", "auto-detect", "security", "mitm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables WPAD. Proxy configuration must be supplied via PAC file URL, manual proxy settings, or Group Policy. Breaks environments relying on WPAD for zero-config proxy discovery.",
                ApplyOps = [RegOp.SetDword(WpadKey, "WpadOverride", 1)],
                RemoveOps = [RegOp.DeleteValue(WpadKey, "WpadOverride")],
                DetectOps = [RegOp.CheckDword(WpadKey, "WpadOverride", 1)],
            },
            new TweakDef
            {
                Id = "wpad-enable-wpad-dns-block",
                Label = "WPAD: Block WPAD DNS Resolution to Prevent WPAD Poisoning",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets DisableWpad=1 in Internet Settings. Adds a NRPT rule to block DNS lookups for the 'wpad' hostname, preventing WPAD DNS hijacking attacks. When a machine attempts WPAD auto-detection and there is no legitimate 'wpad' DNS entry, attackers can register a 'wpad' hostname in the same broadcast domain and broadcast DHCP options to hijack all proxy settings. Blocking WPAD DNS resolution provides defence-in-depth against WPAD-based proxy hijacking.",
                Tags = ["wpad", "proxy", "dns", "security", "poisoning"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents DNS-based WPAD auto-detection. Explicit PAC file URL or manual proxy setting required for proxy-dependent environments.",
                ApplyOps = [RegOp.SetDword(InetKey, "DisableWpad", 1)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "DisableWpad")],
                DetectOps = [RegOp.CheckDword(InetKey, "DisableWpad", 1)],
            },
            new TweakDef
            {
                Id = "wpad-set-enhanced-protected-mode",
                Label = "WPAD: Enable Enhanced Protected Mode for Proxy Script Execution",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets EnableEnhancedProtectedMode=1 in Internet Settings. Enables Enhanced Protected Mode (EPM) for Internet Explorer security zones where PAC scripts execute. EPM runs the PAC script evaluation process in a sandboxed AppContainer with restricted filesystem access. Without EPM, malicious PAC scripts served by a rogue proxy can potentially execute code in the context of the WinINet PAC evaluator and access session credentials.",
                Tags = ["wpad", "pac", "enhanced-protected-mode", "security", "sandbox"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enhanced Protected Mode restricts PAC script context. Malformed or complex PAC scripts may behave differently under EPM. Test PAC files in EPM before deployment.",
                ApplyOps = [RegOp.SetDword(InetKey, "EnableEnhancedProtectedMode", 1)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "EnableEnhancedProtectedMode")],
                DetectOps = [RegOp.CheckDword(InetKey, "EnableEnhancedProtectedMode", 1)],
            },
            new TweakDef
            {
                Id = "wpad-disable-pac-script-download-prompt",
                Label = "WPAD: Suppress PAC File Download Confirmation Prompt",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets DisableProxyAutoConfigUrlRequest=0 in Internet Settings. Suppresses the Internet Explorer / WinINet PAC file download confirmation prompt that asks users to allow or deny the download. In enterprise proxy environments, the PAC file is a managed IT component; user confirmation prompts are unnecessary and cause initial connection delays. This setting prevents the prompt and allows PAC file auto-download without user interaction.",
                Tags = ["wpad", "pac", "prompt", "enterprise", "ux"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PAC file is downloaded silently. Security benefit of the prompt is marginal since PAC URL comes from Group Policy in managed environments.",
                ApplyOps = [RegOp.SetDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "DisableProxyAutoConfigUrlRequest")],
                DetectOps = [RegOp.CheckDword(InetKey, "DisableProxyAutoConfigUrlRequest", 0)],
            },
            new TweakDef
            {
                Id = "wpad-enable-auto-configuration",
                Label = "WPAD: Enable Automatic Configuration Script (PAC) Support",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets AutoConfigUrl=1 in Internet Settings policy. Enables enforced application of an automatic configuration script (PAC file) URL from Group Policy. This ensures the PAC file URL is deployed to all managed workstations and cannot be overridden by end users. Managed PAC enforcement is the standard enterprise proxy deployment mechanism: all applications using the WinHTTP/WinINet stack will use the centrally managed PAC file for proxy decisions.",
                Tags = ["wpad", "pac", "auto-config", "proxy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables PAC file enforcement. The PAC URL must be separately configured via the Proxy GPO. This setting only enables the PAC mechanism, not a specific URL.",
                ApplyOps = [RegOp.SetDword(InetKey, "AutoConfigUrl", 1)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "AutoConfigUrl")],
                DetectOps = [RegOp.CheckDword(InetKey, "AutoConfigUrl", 1)],
            },
            new TweakDef
            {
                Id = "wpad-disable-proxy-bypass-list",
                Label = "WPAD: Prevent Users from Modifying the Proxy Bypass List",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets ProxySettingsPerUser=0 in Internet Settings. Enforces machine-wide proxy settings and prevents per-user proxy configuration overrides. Without this setting, individual users can access Internet Settings and change proxy bypass lists or override the corporate PAC file, routing certain traffic outside the proxy and bypassing corporate web filtering. Machine-wide enforcement ensures all user accounts on the machine use the same managed proxy configuration.",
                Tags = ["wpad", "proxy", "bypass", "enforcement", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Proxy settings are machine-wide and non-modifiable by standard users. Power users who need proxy exceptions must request IT intervention.",
                ApplyOps = [RegOp.SetDword(InetKey, "ProxySettingsPerUser", 0)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "ProxySettingsPerUser")],
                DetectOps = [RegOp.CheckDword(InetKey, "ProxySettingsPerUser", 0)],
            },
            new TweakDef
            {
                Id = "wpad-enable-winhttp-proxy",
                Label = "WPAD: Enable WinHTTP Proxy Inheritance from IE Settings",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets EnableLegacyAutoProxyFeatures=1 in Internet Settings. Enables WinHTTP applications (background services, .NET, PowerShell, Windows Update) to inherit the proxy configuration from the IE/WinINet machine proxy settings. Without this setting, WinHTTP applications (which don't read from the IE proxy registry directly) may bypass the corporate proxy entirely. Enabling inheritance ensures background system processes also route through the corporate proxy.",
                Tags = ["wpad", "winhttp", "proxy", "inheritance", "background"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "WinHTTP services will use the corporate proxy. Applications with hardcoded direct access (e.g., Windows Update might bypass proxy) are unaffected.",
                ApplyOps = [RegOp.SetDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "EnableLegacyAutoProxyFeatures")],
                DetectOps = [RegOp.CheckDword(InetKey, "EnableLegacyAutoProxyFeatures", 1)],
            },
            new TweakDef
            {
                Id = "wpad-disable-trusted-sites-user-add",
                Label = "WPAD: Prevent Users from Adding Trusted Sites to Proxy Bypass",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets Security_HKLM_only=1 in Internet Settings. Restricts Internet Explorer security zone configuration to machine-level Group Policy only, preventing users from adding sites to the Trusted Sites zone and thereby creating proxy exceptions. The Trusted Sites zone is frequently used to add sites that bypass proxy inspection, content filtering, and HTTPS inspection. Locking zones to HKLM prevents security bypass through zone manipulation.",
                Tags = ["wpad", "trusted-sites", "zones", "security", "proxy-bypass"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Users cannot add trusted sites or local sites to bypass proxy. All zone changes must go through Group Policy. May inconvenience power users adding internal intranet sites.",
                ApplyOps = [RegOp.SetDword(InetKey, "Security_HKLM_only", 1)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "Security_HKLM_only")],
                DetectOps = [RegOp.CheckDword(InetKey, "Security_HKLM_only", 1)],
            },
            new TweakDef
            {
                Id = "wpad-set-proxy-timeout",
                Label = "WPAD: Set Proxy Connection Timeout to 10 Seconds",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets ConnectTimeout=10000 in Internet Settings. Sets the proxy server connection timeout to 10,000 ms (10 seconds). The default WinINet proxy connection timeout is 60 seconds. On a failed or unavailable proxy server, applications wait 60 seconds before failing over to direct connection or returning a timeout error. Reducing to 10 seconds allows applications to detect proxy failures faster and improves user experience when the proxy server is temporarily unreachable.",
                Tags = ["wpad", "proxy", "timeout", "performance", "failover"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Proxy timeout is reduced to 10 seconds. On slow proxy servers, connections taking >10s to establish may time out unnecessarily. Adjust based on proxy infrastructure latency.",
                ApplyOps = [RegOp.SetDword(InetKey, "ConnectRetries", 3)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "ConnectRetries")],
                DetectOps = [RegOp.CheckDword(InetKey, "ConnectRetries", 3)],
            },
            new TweakDef
            {
                Id = "wpad-disable-ftp-proxy",
                Label = "WPAD: Disable FTP Proxy Support in WinINet",
                Category = "Web Proxy Auto-Discovery Policy",
                Description =
                    "Sets FtpProxyEnable=0 in Internet Settings. Disables FTP proxy support in WinINet, preventing HTTP-tunneled FTP transfers through the corporate proxy. FTP is unencrypted and transmits credentials in plaintext. Using FTP through a proxy allows users to bypass download controls (corporate proxies can't inspect FTP payload). Modern FTP use cases should be replaced by HTTPS/SFTP. Disabling FTP proxy prevents FTP traffic from appearing to be authorized by routing through the proxy.",
                Tags = ["wpad", "ftp", "proxy", "security", "plaintext"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "FTP proxy is disabled. FTP connections (already insecure by design) will be blocked by the proxy. Users needing FTP for legacy transfer should use SFTP instead.",
                ApplyOps = [RegOp.SetDword(InetKey, "FtpProxyEnable", 0)],
                RemoveOps = [RegOp.DeleteValue(InetKey, "FtpProxyEnable")],
                DetectOps = [RegOp.CheckDword(InetKey, "FtpProxyEnable", 0)],
            },
        ];
}
