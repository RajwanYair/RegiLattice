// RegiLattice.Core — Tweaks/WinInetPolicy.cs
// Sprint 364: WinInet Policy tweaks (10 tweaks)
// Category: "WinInet Policy" | Slug: wininet
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings
// ⚠️ Special: ProxyOverride uses SetString+CheckString; EnabledProtocols uses SetDword with value 2688

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WinInetPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CurrentVersion\Internet Settings";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wininet-enable-enhanced-protected-mode",
            Label = "Enable Enhanced Protected Mode for Internet Explorer and WinInet Clients",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enhanced Protected Mode extends the standard Protected Mode sandbox by running browser tab processes in a 64-bit AppContainer with additional restrictions on access to sensitive user files network resources and system components. Enhanced Protected Mode significantly increases the effort required for malicious web content to escape the browser sandbox and access system resources or user data. WinInet applications that use the Internet Explorer rendering engine or MSHTML benefit from Enhanced Protected Mode when it is enabled through policy. The 64-bit AppContainer used by Enhanced Protected Mode prevents many exploit techniques that rely on 32-bit process assumptions and low-integrity level bypass methods. Organizations should test web application compatibility with Enhanced Protected Mode before deploying it as some ActiveX controls and legacy add-ins may not function in the AppContainer sandbox. Security vendors analyze Enhanced Protected Mode bypass techniques as high-severity findings recognizing the importance of the protection it provides against web-based exploitation.",
            Tags = ["enhanced-protected-mode", "wininet", "sandbox", "ie-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedProtectedMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedProtectedMode")],
            DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedProtectedMode", 1)],
        },
        new TweakDef
        {
            Id = "wininet-enforce-tls-protocol-restriction",
            Label = "Enforce TLS Protocol Version Restrictions to Disable Legacy SSL and TLS Versions",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Restricting TLS protocol versions through WinInet policy disables SSL 3.0 TLS 1.0 and TLS 1.1 while enabling TLS 1.2 and TLS 1.3 preventing connections to servers that use cryptographically weak protocols vulnerable to known attacks. The EnabledProtocols policy value uses a bitmask where bit 2048 (0x800) enables TLS 1.2 and bit 640 (0x280) combined gives TLS 1.2 only; the value 2688 (0xA80) enables both TLS 1.2 and TLS 1.3 exclusively. SSL 3.0 is vulnerable to POODLE attacks and all TLS 1.0 implementations are vulnerable to BEAST attacks that allow active network interception to decrypt session traffic. Disabling TLS 1.0 and 1.1 through WinInet policy affects all applications that use the WinInet or WinHTTP API stack ensuring consistent protocol restrictions across the user mode networking layer. Organizations must verify that all internal web services and application dependencies support TLS 1.2 before deploying TLS 1.0 and 1.1 restrictions to avoid breaking legitimate service connectivity. External service dependencies should also be audited for TLS protocol support with a migration plan for services that do not yet support TLS 1.2.",
            Tags = ["tls", "protocol-restriction", "ssl-disable", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnabledProtocols", 2688)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnabledProtocols")],
            DetectOps = [RegOp.CheckDword(Key, "EnabledProtocols", 2688)],
        },
        new TweakDef
        {
            Id = "wininet-configure-proxy-bypass-for-local",
            Label = "Configure Proxy Bypass List to Allow Direct Access to Local Intranet Resources",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Configuring the proxy bypass list to include the local intranet bypass designator ensures that connections to local intranet hosts do not traverse the external proxy server preserving local network performance and avoiding round-trip latency for intranet resources. The ProxyOverride value with the string value of `<local>` instructs WinInet to bypass the configured proxy server for all hosts resolved to private IP address ranges and single-label DNS names. Without the local bypass designation all intranet traffic is routed through the proxy server which can cause failures for intranet applications that are not designed for proxy traversal and increases proxy server load unnecessarily. The proxy bypass configuration should be consistent with the organization's network topology ensuring that intranet resources are correctly identified as local. Custom bypass entries for specific intranet domain names or IP subnets can be added to the ProxyOverride value in addition to the `<local>` designation for more granular control. Proxy configuration testing should verify that internal resource access is not routed through external proxy infrastructure to prevent inadvertent data exposure to proxy service providers.",
            Tags = ["proxy-bypass", "local-intranet", "wininet", "network-configuration", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "ProxyOverride", "<local>")],
            RemoveOps = [RegOp.DeleteValue(Key, "ProxyOverride")],
            DetectOps = [RegOp.CheckString(Key, "ProxyOverride", "<local>")],
        },
        new TweakDef
        {
            Id = "wininet-disable-certificate-revocation-soft-fail",
            Label = "Disable Soft-Fail Certificate Revocation Checking to Enforce Hard Revocation Policy",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Soft-fail certificate revocation checking allows TLS connections to proceed even when the certificate revocation check cannot be completed due to an unavailable OCSP responder or CRL distribution point creating a vulnerability window where connections using revoked certificates are not blocked. Enforcing hard revocation checking ensures that TLS connections are refused when certificate revocation status cannot be verified giving attackers no benefit from making the revocation infrastructure unavailable. Hard revocation checking may cause connectivity failures when revocation infrastructure is unreachable so organizations should ensure OCSP responders and CRL distribution points are highly available before deploying hard revocation policy. Certificate pinning and OCSP stapling provide alternatives to traditional revocation checking that are not subject to soft-fail vulnerabilities and should be preferred for high-security applications. The combination of soft-fail revocation and short-lived certificates is a common approach to managing high-availability requirements while maintaining security guarantees. Organizations should evaluate whether OCSP stapling support in their web infrastructure allows hard revocation checking without availability impacts.",
            Tags = ["certificate-revocation", "hard-fail", "tls-security", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "CertificateRevocationHardFail", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "CertificateRevocationHardFail")],
            DetectOps = [RegOp.CheckDword(Key, "CertificateRevocationHardFail", 1)],
        },
        new TweakDef
        {
            Id = "wininet-enable-https-strict-transport-security",
            Label = "Enable HTTP Strict Transport Security Enforcement in WinInet Stack",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "HTTP Strict Transport Security enforcement ensures that HSTS headers received from web servers are respected by WinInet-based applications preventing downgrade attacks that redirect HTTPS connections to HTTP before the server-side redirect can enforce HTTPS. HSTS protection eliminates a class of SSL stripping attacks in which a network adversary intercepts the initial HTTP request before the server-side 301 redirect upgrades the connection to HTTPS. WinInet HSTS enforcement builds the transport security list from HSTS headers and preloaded HSTS site lists to provide protection for sites whether or not the browser has previously visited them. Applications that use WinInet for HTTPS communication benefit from HSTS enforcement consistently across all sites that have deployed HSTS including sites on the HSTS preload list. Organizations running internal HTTPS services should deploy HSTS headers on those services to benefit from HSTS caching on client devices. HSTS should be combined with HTTPS-only policies on internal web servers to create a comprehensive transport security posture.",
            Tags = ["hsts", "https-enforcement", "tls-downgrade", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableHSTS", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableHSTS")],
            DetectOps = [RegOp.CheckDword(Key, "EnableHSTS", 1)],
        },
        new TweakDef
        {
            Id = "wininet-block-mixed-content-navigation",
            Label = "Block Navigation to Mixed HTTP and HTTPS Content in WinInet Applications",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Mixed content navigation occurs when HTTPS pages include or link to HTTP content creating security weaknesses where the unencrypted HTTP content can be intercepted or tampered with by network adversaries even though the page is nominally loaded over HTTPS. Blocking mixed content prevents HTTPS pages from loading insecure sub-resources including scripts stylesheets and images over HTTP which could enable cross-site scripting via content injection. Mixed active content including scripts and iframes is strictly blocked by default in modern browsers but mixed passive content including images and audio is often allowed with warnings. Enforcing strict mixed content blocking through WinInet policy ensures that all WinInet-based applications not just modern browsers refuse to load HTTP sub-resources on HTTPS pages. Organizations running internal HTTPS web applications should ensure all referenced assets are served over HTTPS to avoid compatibility issues when mixed content blocking is enforced. Mixed content blocking policy should be tested against all critical web applications before enforcement to identify applications that need to be updated to serve all content over HTTPS.",
            Tags = ["mixed-content", "https-enforcement", "wininet", "content-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockMixedContent", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockMixedContent")],
            DetectOps = [RegOp.CheckDword(Key, "BlockMixedContent", 1)],
        },
        new TweakDef
        {
            Id = "wininet-disable-automatic-proxy-detection",
            Label = "Disable Automatic Proxy Detection and WPAD Protocol in WinInet Stack",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Automatic proxy detection using Web Proxy Auto-Discovery Protocol allows network infrastructure to automatically configure proxy settings for client systems by broadcasting proxy configuration file locations through DNS and DHCP which can be exploited by attackers. WPAD attacks allow an attacker on the local network to provide a malicious proxy auto-configuration script that redirects traffic through a controlled proxy for interception and credential harvesting. Disabling automatic proxy detection prevents WPAD-based proxy configuration attacks while requiring that proxy settings be explicitly configured through Group Policy ensuring that proxy configuration is under organizational control. WPAD is particularly dangerous on untrusted networks such as conference WiFi or hotel networks where attackers can respond to WPAD DNS queries with malicious proxy configurations. Organizations should configure proxy settings through Group Policy using explicit proxy server addresses or PAC file URLs rather than relying on WPAD auto-detection. Systems that connect to guest WiFi or external networks should be specifically evaluated for WPAD attack exposure if automatic proxy detection is not disabled.",
            Tags = ["wpad", "proxy-detection", "proxy-auto-config", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAutoProxyDetection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoProxyDetection")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAutoProxyDetection", 1)],
        },
        new TweakDef
        {
            Id = "wininet-enforce-certificate-error-handling",
            Label = "Enforce Strict Certificate Error Handling to Prevent User Override of TLS Errors",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Strict certificate error handling prevents users from clicking through TLS certificate errors including expired certificates invalid hostnames and untrusted certificate authority errors which are common indicators of man-in-the-middle attacks. Allowing user override of certificate errors creates a human-factor vulnerability where social engineering can convince users to accept illegitimate certificates by training them that certificate errors are acceptable to bypass. Organizations should enforce certificate error handling through policy to ensure that TLS certificate validation failures result in connection refusal rather than user prompts. Certificate transparency monitoring and certificate pinning are complementary controls that detect certificate authority misissuance which would not be caught by standard certificate validation. Internal applications should use certificates issued by the organization's trusted PKI infrastructure to avoid generating false-positive certificate errors on endpoints configured with enterprise certificate authority trust. The strictness of certificate error handling should be calibrated to the organization's risk tolerance with stricter enforcement appropriate for high-security environments.",
            Tags = ["certificate-errors", "tls-enforcement", "user-bypass", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceCertificateErrorHandling", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceCertificateErrorHandling")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceCertificateErrorHandling", 1)],
        },
        new TweakDef
        {
            Id = "wininet-restrict-third-party-cookie-access",
            Label = "Restrict Third-Party Cookie Access to Reduce Cross-Site Tracking in WinInet",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Restricting third-party cookie access through WinInet policy prevents advertising networks and tracking services from setting and reading cookies across different websites significantly reducing cross-site tracking of user browsing behavior. Third-party cookies are used to build user profiles based on browsing behavior across multiple sites and the data collected may include sensitive information about user interests health conditions or financial situations. Blocking third-party cookies is consistent with increasing regulatory requirements under GDPR and similar privacy regulations that require consent for tracking technologies. Browser vendors have already begun restrictive third-party cookie policies and WinInet policy enforcement ensures consistent behavior across applications that use the WinInet API stack. Organizations should evaluate third-party cookie restrictions against web application single sign-on functionality as some authentication flows use third-party cookies for session management. The impact on SAML and OAuth authentication flows should be tested specifically when implementing third-party cookie restrictions.",
            Tags = ["third-party-cookies", "cross-site-tracking", "privacy", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictThirdPartyCookies", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictThirdPartyCookies")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictThirdPartyCookies", 1)],
        },
        new TweakDef
        {
            Id = "wininet-disable-legacy-security-zones-modification",
            Label = "Prevent User Modification of WinInet Security Zone Configuration",
            Category = "WinInet Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Preventing user modification of security zone configuration ensures that the organization's WinInet security zone policies remain in effect and cannot be weakened by users who move sites to less restrictive zones to make blocked content accessible. Security zones control what capabilities web content has when executing in WinInet-based applications and user-added trusted sites with low security settings create exploitation opportunities for malicious websites. Malware and phishing attacks sometimes instruct victims to add malicious sites to the Trusted Sites zone to enable ActiveX installation or bypass security prompts. Locking security zone configuration through policy ensures that only administrators can add sites to the Trusted Sites zone and modify zone security settings. Organizations should audit the zone configuration regularly to ensure that the sites in the Trusted Sites zone are legitimate and still require trusted access. User requests to add sites to the Trusted Sites zone should go through a security review process before being added through centrally managed Group Policy settings.",
            Tags = ["security-zones", "trusted-sites", "user-restriction", "wininet", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableZoneModification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableZoneModification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableZoneModification", 1)],
        },
    ];
}
