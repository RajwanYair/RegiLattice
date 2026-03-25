// RegiLattice.Core — Tweaks/LockdownBrowsingPolicy.cs
// Sprint 350: Lockdown Browsing Policy tweaks (10 tweaks)
// Category: "Lockdown Browsing Policy" | Slug: lkdwnbr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LockdownBrowser

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class LockdownBrowsingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LockdownBrowser";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "lkdwnbr-enable-lockdown-mode",
            Label = "Enable Lockdown Browsing Mode for Restricted Kiosk Environments",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Lockdown browsing mode restricts users to a limited set of web-based interactions preventing access to operating system features through the browser interface. Enabling lockdown mode is appropriate for kiosk deployments and environments where users should be able to access specific web content without access to general system functionality. The lockdown browser prevents users from opening new windows or tabs outside the allowed navigation context. Restrictions include disabling the address bar context menus browser history access and configuration menus that could provide OS access. Organizations should configure lockdown browsing mode alongside allowlisted URL policies to define the specific web content accessible to users. Users in lockdown environments should be informed about the restrictions and provided alternative access paths for tasks outside the kiosk scope.",
            Tags = ["lockdown-browser", "kiosk", "restricted-browsing", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableLockdownMode", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableLockdownMode")],
            DetectOps = [RegOp.CheckDword(Key, "EnableLockdownMode", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-disable-developer-tools",
            Label = "Disable Web Browser Developer Tools in Lockdown Environments",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Disabling browser developer tools prevents users in lockdown environments from using the developer console JavaScript console and DOM inspector to bypass browsing restrictions. Developer tools provide powerful capabilities including the ability to execute arbitrary JavaScript modify DOM elements and intercept network requests that could be used to circumvent lockdown controls. Users with knowledge of web technologies could use the developer console to access restricted features or data visible in the page source. Disabling developer tools also prevents exposure of sensitive page payload data in the network tab that might be visible to unauthorized users. Organizations deploying lockdown browsers for public access or restricted access scenarios should disable developer tools as part of the baseline configuration. The restriction specifically prevents keyboard shortcuts that activate developer tools like F12 or Ctrl+Shift+I.",
            Tags = ["lockdown-browser", "developer-tools", "kiosk-security", "bypass-prevention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeveloperTools", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeveloperTools")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeveloperTools", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-block-file-protocol",
            Label = "Block file:// Protocol Access in Lockdown Browser Environments",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Blocking the file:// URL protocol prevents users in lockdown browser environments from browsing local file system contents through the browser interface. The file:// protocol allows browsers to render local HTML files and navigate the file system hierarchy which circumvents lockdown browsing restrictions intended to prevent access to the underlying OS. Users who discover the file:// protocol can use it to browse sensitive configuration files log files and data files stored on the local system. Blocking this protocol is a minimum requirement for lockdown browsing environments where users are not authorized to access the local file system. Organizations should also block other potentially dangerous protocol handlers like res:// and data:// that can render local resources or inline malicious content. Browser protocol restrictions should be tested after browser updates to verify that the restrictions are re-applied when browser settings are reset.",
            Tags = ["lockdown-browser", "file-protocol", "protocol-blocking", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockFileProtocol", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockFileProtocol")],
            DetectOps = [RegOp.CheckDword(Key, "BlockFileProtocol", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-restrict-context-menus",
            Label = "Restrict Right-Click Context Menus to Prevent Navigation Bypass",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Context menu restriction in lockdown browser environments prevents users from accessing browser functionality like opening links in new windows or viewing page source through the right-click menu. Unrestricted context menus in lockdown environments can allow users to navigate outside the allowed URL list by opening links in new windows that may not have the same restrictions applied. The page source view accessible through context menus exposes the HTML JavaScript and CSS code which may contain sensitive business logic or configuration information. Context menu restrictions limit the navigation options available to users to forward and back navigation within the allowed URL space. Organizations should configure context menus to only show options relevant to user's tasks in the kiosk application such as copy paste and text selection. Testing context menu restrictions with each new browser version is important as browser updates may introduce new context menu entries.",
            Tags = ["lockdown-browser", "context-menus", "kiosk", "bypass-prevention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictContextMenus", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictContextMenus")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictContextMenus", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-disable-printing",
            Label = "Disable Printing Functionality in Lockdown Browser Sessions",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Disabling print functionality in lockdown browser environments prevents users from printing sensitive content accessed through the restricted browser session. Print functionality in browsers provides access to the system print dialog which may expose available printers network printers and printer configuration that is outside the scope of the kiosk session. Printing prevents the application of data loss prevention controls that might otherwise limit what data can leave the environment. Organizations that deploy lockdown browsers for scenarios where users should not extract data should disable printing as part of the data control baseline. Print-to-PDF functionality in browsers is a particular concern as it allows saving digital copies of all page content to the local file system or network locations. Disabling printing is appropriate when the kiosk use case is reference access to content without a requirement to extract information.",
            Tags = ["lockdown-browser", "printing", "data-loss-prevention", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrinting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrinting")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrinting", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-enforce-popup-blocking",
            Label = "Enforce Popup Blocking in Lockdown Browser Browsing Policy",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enforcing popup blocking in lockdown browser environments prevents web content from opening additional browser windows that may operate outside the lockdown restrictions. Popup windows created by web applications may not inherit the same lockdown policy as the parent window allowing users to navigate to unrestricted content through the popup. Web-based attacks use popups to launch phishing or social engineering attacks that direct users to malicious sites outside the intended kiosk scope. Policy-enforced popup blocking ensures that allowed applications can still use programmatic popups if they are in the allowlisted URL space while blocking popups from untrusted sources. Popup blocking logs can provide insight into web content that is attempting to open additional windows which may indicate malicious content in allowed sites. Organizations should test that authorized web applications function correctly with popup blocking enabled.",
            Tags = ["lockdown-browser", "popup-blocking", "window-restrictions", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforcePopupBlocking", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforcePopupBlocking")],
            DetectOps = [RegOp.CheckDword(Key, "EnforcePopupBlocking", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-disable-browser-extensions",
            Label = "Disable Browser Extension Installation in Lockdown Environments",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Disabling browser extension installation in lockdown browser environments prevents users from installing extensions that could bypass lockdown restrictions or harvest data. Browser extensions currently have access to all page content the ability to intercept and modify network requests and can communicate with external services. Malicious browser extensions or extensions used by unauthorized users can completely circumvent lockdown browsing restrictions by injecting JavaScript or intercepting navigation. Extension restrictions ensure that the browser operates in a controlled state without third-party code that can modify its behavior. Organizations should also review any pre-installed extensions to verify they are required for the kiosk application and do not provide capabilities that conflict with lockdown goals. Extension policy restrictions should be combined with management of the browser profile directory to prevent users from copying extensions into the profile manually.",
            Tags = ["lockdown-browser", "extensions", "browser-security", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableBrowserExtensions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableBrowserExtensions")],
            DetectOps = [RegOp.CheckDword(Key, "DisableBrowserExtensions", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-force-clear-session-data",
            Label = "Force Clearing Session Data on Lockdown Browser Session End",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Forcing session data clearing at lockdown browser session end removes cookies stored credentials autocomplete data browsing history and cached content between user sessions. Session data retention between kiosk users is a privacy and security risk as subsequent users can access the previous user's session state or credentials. Automatic session clearing between users is critical in shared kiosk deployments where multiple untrusted users use the same browser session for government services banking or healthcare access. Browser session data that persists between users can include authentication session cookies that allow the next user to impersonate the previous user's authenticated session. Organizations operating public-facing kiosks should configure browser session clearing to run before each new session starts rather than only at system startup. Testing session clearing completeness with browser developer tools after each implementation helps verify that all sensitive data is removed.",
            Tags = ["lockdown-browser", "session-clearing", "privacy", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ForceClearSessionData", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "ForceClearSessionData")],
            DetectOps = [RegOp.CheckDword(Key, "ForceClearSessionData", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-restrict-keyboard-shortcuts",
            Label = "Restrict Browser Keyboard Shortcuts in Lockdown Browser Policy",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting browser keyboard shortcuts prevents users from activating browser features through key combinations that are not appropriate for the lockdown environment. Keyboard shortcuts like Ctrl+L to focus the address bar Ctrl+T for new tabs and F11 for fullscreen exit can provide paths around lockdown restrictions even when toolbar elements are hidden. Users with technical knowledge use keyboard shortcuts to access browser configuration menus navigate outside the allowed URL list and open developer tools. Keyboard shortcut restrictions require that silo keyboard input is filtered by the lockdown browser before reaching the browser engine. Some keyboard shortcuts may be required for accessibility purposes and organizations should evaluate which restrictions to apply based on user requirements. Organizations should test that all keyboard shortcut restrictions work correctly including after users discover and attempt to use them.",
            Tags = ["lockdown-browser", "keyboard-shortcuts", "bypass-prevention", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictKeyboardShortcuts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictKeyboardShortcuts")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictKeyboardShortcuts", 1)],
        },
        new TweakDef
        {
            Id = "lkdwnbr-enable-idle-session-reset",
            Label = "Enable Automatic Reset of Lockdown Browser on Session Idle Timeout",
            Category = "Lockdown Browsing Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Idle session reset automatically resets the lockdown browser to its initial state after a configured period of user inactivity ensuring clean starting conditions for the next user. Without idle reset a user who leaves a kiosk with an active session may leave sensitive data visible or maintain an authenticated application session that the next user can exploit. Idle session reset returns the browser to the default configured start page clears session data and removes any personal information entered during the previous session. The idle timeout period should be configured based on the typical session duration and the sensitivity of data accessible through the kiosk application. Idle reset is particularly important for kiosks in public areas like libraries hospitals and government offices where user turnover is high. Organizations should configure idle reset to include a brief countdown warning that allows users to continue their session if they return before the reset completes.",
            Tags = ["lockdown-browser", "idle-reset", "session-management", "kiosk", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIdleSessionReset", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIdleSessionReset")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIdleSessionReset", 1)],
        },
    ];
}
