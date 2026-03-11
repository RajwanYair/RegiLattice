namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Chrome
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "chrome-disable-chrome-bg",
            Label = "Disable Chrome Background Apps",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Chrome from running in the background after the browser window is closed, saving memory and CPU.",
            Tags = ["chrome", "browser", "background"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-telemetry",
            Label = "Disable Chrome Telemetry",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Chrome metrics, spell-check cloud, translate, and extended safe-browsing reporting.",
            Tags = ["chrome", "browser", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-update",
            Label = "Disable Chrome Auto-Update",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Prevents Chrome from checking for or installing updates.",
            Tags = ["chrome", "browser", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Update"],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-hwaccel",
            Label = "Disable Chrome Hardware Acceleration",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Forces Chrome to use software rendering instead of GPU, useful for troubleshooting display issues.",
            Tags = ["chrome", "browser", "gpu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-chrome-signin",
            Label = "Disable Chrome Sign-In & Sync",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome browser sign-in and sync via policy.",
            Tags = ["chrome", "browser", "privacy", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-secure-dns",
            Label = "Enable Chrome Secure DNS (DoH)",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables DNS-over-HTTPS (automatic mode) in Chrome.",
            Tags = ["chrome", "browser", "dns", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-signin",
            Label = "Disable Chrome Browser Sign-In Prompt",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Chrome browser sign-in prompt via policy.",
            Tags = ["chrome", "browser", "signin", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-sync",
            Label = "Disable Chrome Sync",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome profile sync via policy.",
            Tags = ["chrome", "browser", "sync", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-spell-check-service",
            Label = "Disable Chrome Cloud Spell Check",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the cloud-based spell-check service in Chrome, keeping only the local spell checker.",
            Tags = ["chrome", "browser", "spellcheck", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-block-third-party-cookies",
            Label = "Block Third-Party Cookies",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blocks third-party cookies in Chrome via policy.",
            Tags = ["chrome", "browser", "cookies", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-autofill-passwords",
            Label = "Disable Chrome Password Autofill",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the built-in Chrome password manager and autofill for passwords via policy.",
            Tags = ["chrome", "browser", "passwords", "autofill", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-reporter",
            Label = "Disable Chrome Software Reporter",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome Software Reporter Tool and cleanup reporting. Prevents high CPU usage from background scanning. Default: Enabled. Recommended: Disabled.",
            Tags = ["chrome", "reporter", "cleanup", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-background",
            Label = "Disable Chrome Background Apps",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from running in the background after closing. Frees memory and CPU. Default: Enabled. Recommended: Disabled.",
            Tags = ["chrome", "background", "performance", "memory"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-metrics-reporting",
            Label = "Disable Chrome Metrics Reporting",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Chrome metrics and usage reporting via enterprise policy. Prevents Chrome from sending usage statistics. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["chrome", "metrics", "telemetry", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-default-browser-check",
            Label = "Disable Chrome Default Browser Check",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Chrome from prompting to set itself as the default browser on startup. Default: Enabled. Recommended: Disabled for managed environments.",
            Tags = ["chrome", "default-browser", "prompt", "ux"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-hardware-accel-policy",
            Label = "Disable Chrome Hardware Acceleration (Policy)",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Chrome hardware acceleration via enterprise policy. Useful for troubleshooting GPU-related rendering issues. Default: Enabled. Recommended: Disabled if GPU issues occur.",
            Tags = ["chrome", "hardware", "acceleration", "gpu", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-enforce-3p-cookie-block",
            Label = "Block Chrome Third-Party Cookies (Policy)",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Blocks third-party cookies in Chrome via enterprise policy. Enhances privacy by preventing cross-site tracking. Default: Allowed. Recommended: Blocked for privacy.",
            Tags = ["chrome", "cookies", "third-party", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-translate",
            Label = "Disable Chrome Translate",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Chrome built-in page translation feature via enterprise policy. Prevents translate bar prompts. Default: Enabled. Recommended: Disabled if not needed.",
            Tags = ["chrome", "translate", "language", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-media-recommendations",
            Label = "Disable Chrome Media Recommendations",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables personalized media recommendations on the Chrome New Tab page. Reduces data collection and distractions. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["chrome", "media", "recommendations", "privacy", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
        new TweakDef
        {
            Id = "chrome-disable-leak-detection",
            Label = "Disable Chrome Password Leak Detection",
            Category = "Chrome",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Chrome password leak detection that checks saved passwords against known data breaches. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["chrome", "password", "leak", "detection", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Google\Chrome"],
        },
    ];
}
