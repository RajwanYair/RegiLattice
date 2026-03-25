// RegiLattice.Core — Tweaks/KioskBrowserPolicy.cs
// Sprint 333: Kiosk Browser Policy tweaks (10 tweaks)
// Category: "Kiosk Browser Policy" | Slug: kiosk
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\KioskBrowser

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class KioskBrowserPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\KioskBrowser";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "kiosk-enable-kiosk-mode",
            Label = "Enable Kiosk Browser Lockdown Mode",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Kiosk browser lockdown mode restricts the browser to a limited interface preventing navigation to unauthorized sites and blocking browser customization. Enabling kiosk mode creates a purpose-specific browsing experience suitable for public terminals, self-service kiosks, and restricted endpoint deployments. Kiosk mode removes the browser address bar, navigation history, developer tools, and other features that could be used to browse unauthorized content. Lockdown mode prevents end users of kiosk terminals from accessing sensitive information or performing unauthorized actions through the browser. Kiosk browser policies integrate with the Windows assigned access feature for single-app kiosk deployments. Organizations deploying kiosk endpoints should combine kiosk browser mode with OS-level kiosk configuration to prevent breakouts.",
            Tags = ["kiosk", "browser", "lockdown", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableKioskMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableKioskMode")],
            DetectOps = [RegOp.CheckDword(Key, "EnableKioskMode", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-restrict-allowed-urls",
            Label = "Restrict Kiosk Browser to Allowed URLs Only",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "URL allow-listing restricts kiosk browser navigation to only the websites explicitly approved for kiosk use preventing browsing to unauthorized content. Restricting to allowed URLs creates a whitelist model ensuring kiosk users can only access the specific web applications intended for the deployment. URL restrictions prevent kiosk users from navigating away from authorized applications to social media, external services, or malicious sites. Allowed URL patterns support wildcard matching to accommodate web applications with multiple pages and dynamic URLs. URL restriction enforcement prevents social engineering attacks against kiosk users who might be directed to malicious sites. Allowed URL lists should be maintained and reviewed regularly to ensure only currently needed URLs are included.",
            Tags = ["kiosk", "url-allowlist", "browsing-restriction", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetString(Key, "AllowedUrls", "about:blank")],
            RemoveOps = [RegOp.DeleteValue(Key, "AllowedUrls")],
            DetectOps = [RegOp.CheckString(Key, "AllowedUrls", "about:blank")],
        },
        new TweakDef
        {
            Id = "kiosk-disable-browser-extensions",
            Label = "Disable Browser Extensions in Kiosk Mode",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Browser extensions in kiosk mode could be used to bypass URL restrictions, capture user input, or perform unauthorized actions outside the kiosk purpose. Disabling extensions in kiosk mode ensures that the browser operates in a clean state without third-party code that could compromise kiosk security. Extensions have access to browser internals and could intercept communications between the kiosk user and the target web application. Kiosk deployments should never have user-installed extensions as these are not validated for the kiosk security model. Organizations that require functionality typically provided by extensions should integrate it into the target web application instead. Extension disablement in kiosk mode is a fundamental security requirement for any public-facing terminal deployment.",
            Tags = ["kiosk", "extensions", "browser-security", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableExtensions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableExtensions")],
            DetectOps = [RegOp.CheckDword(Key, "DisableExtensions", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-enable-end-session-button",
            Label = "Enable End Session Button in Kiosk Browser",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "The end session button allows kiosk users to reset the browser session and clear any data from their interaction before the next user accesses the terminal. Enabling the end session button provides a privacy protection mechanism for kiosk users who can explicitly terminate their session after use. Session data including form inputs, cookies, and browsing history from one kiosk user should not be accessible to the next user. The end session button resets the browser to its initial state clearing all user-specific data from the current session. Kiosk terminals without session reset capability risk exposing previous user data to subsequent users creating privacy and security risks. Session reset should also be triggered automatically after a configurable idle period to handle cases where users forget to end their session.",
            Tags = ["kiosk", "session-management", "privacy", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ShowEndSessionButton", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ShowEndSessionButton")],
            DetectOps = [RegOp.CheckDword(Key, "ShowEndSessionButton", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-configure-idle-timeout-reset",
            Label = "Configure Kiosk Browser Idle Timeout Session Reset",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Idle timeout session reset automatically clears kiosk browser state when the terminal has been inactive for a specified period. Configuring idle timeout ensures that abandoned kiosk sessions are automatically cleared preventing the next user from accessing previous user data. Public kiosks require automatic session reset because many users will walk away without explicitly ending their session. Idle reset removes form inputs, session cookies, authentication tokens, and browsing history from abandoned sessions. The idle timeout period should be set to balance user experience with security — too short disrupts slow users while too long exposes session data. Idle reset notifications can warn users before reset occurs giving them time to complete their task or explicitly end the session.",
            Tags = ["kiosk", "idle-timeout", "session-reset", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "IdleTimeInMinutes", 5)],
            RemoveOps = [RegOp.DeleteValue(Key, "IdleTimeInMinutes")],
            DetectOps = [RegOp.CheckDword(Key, "IdleTimeInMinutes", 5)],
        },
        new TweakDef
        {
            Id = "kiosk-block-popups",
            Label = "Block Pop-up Windows in Kiosk Browser",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Pop-up windows in kiosk mode can be used to open unauthorized websites, display misleading content, or trick kiosk users into entering information in malicious forms. Blocking pop-ups in kiosk mode ensures that web applications cannot open secondary browser windows that circumvent URL allow-listing restrictions. Pop-up blocking prevents JavaScript-based navigation attacks where a website attempts to open a new window pointing to a restricted or malicious URL. Web-based popups could also be used to confuse kiosk users and potentially capture sensitive information they believe is part of the legitimate application. Kiosk web applications should be designed to function without pop-ups using in-page modal dialogs instead of spawning separate windows. Pop-up blocking is a fundamental defensive control for any kiosk deployment alongside URL allowlisting.",
            Tags = ["kiosk", "popups", "browser-security", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockPopups", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockPopups")],
            DetectOps = [RegOp.CheckDword(Key, "BlockPopups", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-developer-tools",
            Label = "Disable Developer Tools in Kiosk Browser",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Browser developer tools provide JavaScript console access, DOM inspection, network traffic monitoring, and storage inspection capabilities in kiosk browsers. Disabling developer tools prevents sophisticated kiosk users from manipulating the kiosk web application through JavaScript injection or DOM modification. Developer tool access in kiosk mode could allow users to extract authentication tokens, modify application behavior, or access restricted application functionality. The console in developer tools provides full JavaScript execution capability against the current page context creating serious security risks in kiosk environments. Developer tools access in kiosk mode could be used to exfiltrate data that the kiosk application handles on behalf of users. Developers should have access to developer tools only in isolated kiosk development environments, never in production kiosk deployments.",
            Tags = ["kiosk", "developer-tools", "browser-security", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperTools", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperTools")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperTools", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-download-manager",
            Label = "Disable Browser Download Manager in Kiosk Mode",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The browser download manager in kiosk mode allows users to save files from web applications to the local kiosk terminal file system. Disabling downloads in kiosk mode prevents data exfiltration from kiosk applications and blocks malware delivery through drive-by downloads. Downloaded files on kiosk terminals persist between sessions potentially storing sensitive user data or malicious executables. Kiosk applications that need to provide documents to users should deliver them through a controlled print workflow rather than file downloads. Blocking download manager access prevents kiosk users from using the terminal as a personal storage device for external files. Download blocking should be enforced at both the browser policy level and the file system permission level for defense-in-depth.",
            Tags = ["kiosk", "downloads", "data-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDownloadManager", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDownloadManager")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDownloadManager", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-clear-session-data-on-exit",
            Label = "Clear All Session Data When Kiosk Browser Restarts",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Clearing session data on kiosk browser restart removes all cookies, cache, browsing history, and session storage accumulated during the previous usage session. Clearing data on restart ensures that kiosk terminals always start in a clean state regardless of what the previous user did. Residual session data including authentication cookies could allow the next kiosk user to inherit an authenticated session from the previous user. Session data clearing prevents fingerprinting data accumulated during previous sessions from being used to track users or correlate sessions. Kiosk terminals in high-traffic environments may see hundreds of users per day making automatic data clearing essential for user privacy. Session clearing should be verified by testing that login credentials from previous sessions are not auto-filled for subsequent kiosk users.",
            Tags = ["kiosk", "session-data", "privacy", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DeleteBrowsingDataOnClose", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DeleteBrowsingDataOnClose")],
            DetectOps = [RegOp.CheckDword(Key, "DeleteBrowsingDataOnClose", 1)],
        },
        new TweakDef
        {
            Id = "kiosk-disable-address-bar",
            Label = "Hide Address Bar in Kiosk Browser",
            Category = "Kiosk Browser Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "The browser address bar allows kiosk users to type arbitrary URLs and navigate to sites that may not be included in the approved URL allowlist. Hiding the address bar removes the primary user interface element for unauthorized navigation preventing kiosk users from typing arbitrary destination URLs. Address bar restriction combined with URL allowlisting provides a defense-in-depth approach since even if one control is bypassed the other provides protection. Hiding the address bar also reduces the kiosk terminal interface to only the essential web application elements improving the user experience for intended kiosk functions. URL allowlisting through registry policy should also be enforced independently from the address bar visibility as the bar can still be accessed through keyboard shortcuts. Address bar hiding is a usability and defense-in-depth control that works best as part of a comprehensive kiosk security configuration.",
            Tags = ["kiosk", "address-bar", "navigation-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "HideAddressBar", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "HideAddressBar")],
            DetectOps = [RegOp.CheckDword(Key, "HideAddressBar", 1)],
        },
    ];
}
