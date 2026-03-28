namespace RegiLattice.Core.Tweaks;

using System.Collections.Generic;
using RegiLattice.Core.Models;

internal static class EdgeNotificationsAndPopupPolicy
{
    private const string EdgeKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Edge";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "edgenotif-block-notifications",
            Label = "Edge Notifications & Popup Policy: Block All Web Push Notifications",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Configures Microsoft Edge to block all websites from sending push notification requests to the user. Setting DefaultNotificationsSetting to 2 means no site can prompt the user for notification permission and no notifications are delivered via the Web Notifications API. Web push notifications are a significant phishing and social engineering vector, used by malicious sites to display alarming security messages, fake prize alerts, or to maintain persistent contact with the browser even when the site tab is closed.",
            Tags = ["edge", "notifications", "push notifications", "web push", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultNotificationsSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultNotificationsSetting")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultNotificationsSetting", 2)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All websites are blocked from requesting notification permission; zero push notifications delivered by the browser.",
        },
        new TweakDef
        {
            Id = "edgenotif-block-popups",
            Label = "Edge Notifications & Popup Policy: Block All Website Popup Windows",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Configures Microsoft Edge to block all popup windows opened by website JavaScript. Setting DefaultPopupsSetting to 2 prevents any site from opening new browser windows or tabs via window.open() or similar JavaScript calls. Unrequested popups are used by advertising networks for ad injection, by tech-support-scam pages to open fullscreen lock screens, and by malicious sites to spawn additional attack browser windows that are difficult to close without a Task Manager.",
            Tags = ["edge", "popups", "popup blocking", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultPopupsSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultPopupsSetting")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultPopupsSetting", 2)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All JavaScript popup window creation is blocked site-wide; legitimate OAuth popups may require allowlist exceptions.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-password-reveal",
            Label = "Edge Notifications & Popup Policy: Disable Password Reveal Button in Input Fields",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Removes the eye icon that Microsoft Edge renders in password input fields letting users toggle between hidden and plaintext password views. Setting PasswordRevealEnabled to 0 hides this button from all password fields. In open-office environments and shared-screen presentations, the reveal button is a risk because it allows a bystander to see a plaintext password in a single click. Removing the button reduces the risk of shoulder-surfing credential exposure and is aligned with common enterprise browser hardening guidance.",
            Tags = ["edge", "password", "credential", "reveal", "shoulder surfing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "PasswordRevealEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "PasswordRevealEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "PasswordRevealEnabled", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Eye/reveal icon removed from password fields; users must rely on password manager for password visibility.",
        },
        new TweakDef
        {
            Id = "edgenotif-block-geolocation",
            Label = "Edge Notifications & Popup Policy: Block Websites from Accessing Geolocation",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Prevents any website from requesting or obtaining the user's physical location through the browser Geolocation API. Setting DefaultGeolocationSetting to 2 globally blocks geolocation permission so no site can query GPS/Wi-Fi/cell tower location. Geolocation can be combined with other telemetry to build precise user-movement profiles. Blocking it at the browser policy level prevents accidental or silent location sharing through map pages, check-in apps, and advertising networks.",
            Tags = ["edge", "geolocation", "location", "privacy", "gps", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultGeolocationSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultGeolocationSetting")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultGeolocationSetting", 2)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "All websites are denied geolocation access; map and location-based web apps may require manual site exceptions.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-shopping-list",
            Label = "Edge Notifications & Popup Policy: Disable Shopping List Feature",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Disables the Shopping List sidebar feature in Microsoft Edge that allows users to save products to a personal wish list and track price changes. Setting ShoppingListEnabled to 0 removes the Shopping List button from the Edge toolbar and disables the price-tracking infrastructure. The shopping list submits product page URLs and track-point data to Microsoft cloud services for price monitoring. In enterprise environments this creates an unsanctioned data channel for product page URLs visited by employees.",
            Tags = ["edge", "shopping list", "commerce", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "ShoppingListEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "ShoppingListEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "ShoppingListEnabled", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Shopping List toolbar button is removed; no product price tracking data is sent to Microsoft.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-related-website-sets",
            Label = "Edge Notifications & Popup Policy: Disable Related Website Sets Cookie Access",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Prevents Microsoft Edge from applying the Related Website Sets feature (formerly First-Party Sets) to grant cross-site cookie access across groups of domains submitted by a first party. Setting RelatedWebsiteSetsEnabled to 0 means domains that have declared themselves in a Related Website Set cannot automatically share storage with each other. This prevents first-party declared groups from being used to circumvent third-party cookie blocking, maintaining a stricter cross-site data isolation boundary.",
            Tags = ["edge", "related website sets", "first party sets", "cookies", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "RelatedWebsiteSetsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "RelatedWebsiteSetsEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "RelatedWebsiteSetsEnabled", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Related Website Sets cross-site cookie access is blocked; stricter cookie partitioning in effect.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-inapp-support",
            Label = "Edge Notifications & Popup Policy: Disable In-App Microsoft Support Chat",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Disables the in-app Microsoft support mechanism in Microsoft Edge that allows users to contact Microsoft Customer Support directly from within the browser via a chat window or feedback form. Setting InAppSupportEnabled to 0 removes this support channel from the Edge interface. In enterprise environments, user support is handled through internal IT helpdesk processes. The presence of a direct-to-Microsoft support channel may confuse end users and create shadow-IT support workflows that circumvent IT governance.",
            Tags = ["edge", "support", "help", "in-app support", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "InAppSupportEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "InAppSupportEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "InAppSupportEnabled", 0)],
            ImpactScore = 1,
            SafetyRating = 5,
            ImpactNote = "In-app Microsoft support entry point removed from Edge; users must contact IT helpdesk through standard channels.",
        },
        new TweakDef
        {
            Id = "edgenotif-block-sensors",
            Label = "Edge Notifications & Popup Policy: Block Websites from Accessing Device Sensors",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Prevents websites from accessing device motion, orientation, and ambient light sensors through the Generic Sensor API. Setting DefaultSensorsSetting to 2 blocks all sensor access so no site can read accelerometer, gyroscope, magnetometer, or ambient light data without user consent. Sensor data can be used for device fingerprinting, covert user motion profiling, and side-channel attacks like PIN inference from phone-hold orientation patterns. Blocking sensors at policy level removes this attack surface entirely.",
            Tags = ["edge", "sensors", "accelerometer", "gyroscope", "fingerprinting", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "DefaultSensorsSetting", 2)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "DefaultSensorsSetting")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "DefaultSensorsSetting", 2)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Generic Sensor API is blocked site-wide; motion and orientation data are not available to web pages.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-labs",
            Label = "Edge Notifications & Popup Policy: Disable Edge Experimental Labs Features",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Prevents users from accessing Edge Experimental Labs where pre-release and experimental browser features can be enabled. Setting BrowserLabsEnabled to 0 removes the labs button from the Edge toolbar and blocks access to about://flags-style experimental toggles. Experimental features may be unstable, have unreviewed security properties, or create unexpected data flows. Disabling labs in managed environments ensures only stable, IT-approved browser features are active.",
            Tags = ["edge", "labs", "experimental features", "enterprise", "stability", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "BrowserLabsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "BrowserLabsEnabled")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "BrowserLabsEnabled", 0)],
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Labs / experimental features button is hidden from Edge; only stable features are available.",
        },
        new TweakDef
        {
            Id = "edgenotif-disable-fullscreen",
            Label = "Edge Notifications & Popup Policy: Block Websites from Entering Fullscreen Mode",
            Category = "Edge Notifications & Popup Policy",
            Description =
                "Prevents websites from programmatically entering fullscreen mode via the Fullscreen API. Setting FullscreenAllowed to 0 blocks all sites from calling requestFullscreen() on any element or the document. Fullscreen mode is routinely abused by phishing pages and tech-support scam sites to fake operating-system lock screens, panic screens, or Windows Defender alerts that fill the entire display. Users on kiosk hardware also benefit since fullscreen prevents inadvertent accidental keystrokes from covering the screen frame.",
            Tags = ["edge", "fullscreen", "phishing", "tech support scam", "kiosk", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [EdgeKey],
            ApplyOps = [RegOp.SetDword(EdgeKey, "FullscreenAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(EdgeKey, "FullscreenAllowed")],
            DetectOps = [RegOp.CheckDword(EdgeKey, "FullscreenAllowed", 0)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Websites cannot enter fullscreen mode; video players and presentation sites will not be able to go fullscreen.",
        },
    ];
}
