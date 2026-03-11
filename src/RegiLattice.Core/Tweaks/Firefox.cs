namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Firefox
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "firefox-disable-firefox-telemetry",
            Label = "Disable Firefox Telemetry & Studies",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox telemetry, Shield studies, and the Default Browser Agent background task.",
            Tags = ["firefox", "browser", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-pocket",
            Label = "Disable Firefox Pocket",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Pocket integration in Firefox new tab page.",
            Tags = ["firefox", "browser", "pocket"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-update",
            Label = "Disable Firefox Auto-Update",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Firefox from auto-updating. Use in controlled environments.",
            Tags = ["firefox", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-crash-reporter",
            Label = "Disable Firefox Crash Reporter",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Firefox crash reporter dialog after crashes.",
            Tags = ["firefox", "browser", "crash"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-firefox-default-check",
            Label = "Disable Firefox Default Browser Check",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops Firefox from asking to be the default browser on startup.",
            Tags = ["firefox", "browser", "default"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-studies",
            Label = "Disable Firefox Shield Studies",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox Shield studies that deploy experimental features.",
            Tags = ["firefox", "browser", "studies", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-feedback",
            Label = "Disable Firefox Feedback Prompts",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables feedback prompts and commands in Firefox.",
            Tags = ["firefox", "browser", "feedback"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-captive-portal",
            Label = "Disable Firefox Captive Portal Detection",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox captive portal detection network requests.",
            Tags = ["firefox", "browser", "captive-portal", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-dns-over-https",
            Label = "Enable DNS-over-HTTPS",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables DNS-over-HTTPS in Firefox using the Cloudflare resolver.",
            Tags = ["firefox", "browser", "dns", "privacy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 1),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "ProviderURL", "https://mozilla.cloudflare-dns.com/dns-query"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "ProviderURL"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\DNSOverHTTPS", "Enabled", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-extension-recommendations",
            Label = "Disable Extension Recommendations",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables extension recommendations in Firefox.",
            Tags = ["firefox", "browser", "extensions", "recommendations"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-password-reveal",
            Label = "Disable Password Reveal Button",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the password reveal button in Firefox login fields.",
            Tags = ["firefox", "browser", "password", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-telemetry",
            Label = "Disable Firefox Telemetry",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox telemetry data collection via enterprise policy. Reduces background network traffic. Default: Enabled. Recommended: Disabled.",
            Tags = ["firefox", "telemetry", "privacy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-default-check",
            Label = "Disable Firefox Default Browser Check",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from checking if it's the default browser on startup. Removes the nag dialog. Default: Check enabled. Recommended: Disabled.",
            Tags = ["firefox", "default-browser", "nag"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-form-history",
            Label = "Disable Firefox Form History",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox form auto-fill history via enterprise policy. Prevents saving of form data and search history. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["firefox", "form", "history", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableFormHistory", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-profile-import",
            Label = "Disable Firefox Profile Import",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the profile import wizard in Firefox. Prevents importing data from other browsers. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["firefox", "import", "profile", "managed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "DisableProfileImport", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-pocket",
            Label = "Disable Firefox Pocket",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox Pocket integration via enterprise policy. Removes the Pocket button and save-to-Pocket feature. Default: Enabled. Recommended: Disabled if not used.",
            Tags = ["firefox", "pocket", "policy", "feature"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-disable-auto-update",
            Label = "Disable Firefox Auto-Update",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Firefox automatic updates via enterprise policy. Allows manual update control for managed environments. Default: Auto-update. Recommended: Disabled for managed setups.",
            Tags = ["firefox", "update", "auto-update", "policy", "managed"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-password-autosave",
            Label = "Disable Firefox Password Auto-Save",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the offer-to-save-logins prompt in Firefox via enterprise policy. Use when an external password manager is preferred. Default: Enabled. Recommended: Disabled with external manager.",
            Tags = ["firefox", "passwords", "autosave", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-screenshots",
            Label = "Disable Firefox Screenshots",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the built-in Firefox Screenshots feature via enterprise policy. Removes the screenshot button from the toolbar. Default: Enabled. Recommended: Disabled in managed environments.",
            Tags = ["firefox", "screenshots", "policy", "feature"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
        new TweakDef
        {
            Id = "firefox-ff-disable-safe-mode",
            Label = "Disable Firefox Safe Mode",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables access to Firefox Safe Mode via enterprise policy. Prevents users from bypassing managed extensions and settings. Default: Enabled. Recommended: Disabled for locked-down deployments.",
            Tags = ["firefox", "safe-mode", "policy", "managed", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
        },
    ];
}
