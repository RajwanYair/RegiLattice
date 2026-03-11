namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Firefox
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
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
            Id = "firefox-disable-content-analysis",
            Label = "Disable Firefox Content Analysis",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Firefox content analysis features that scan downloads and form data. Default: enabled.",
            Tags = ["firefox", "content", "analysis", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox", "ContentAnalysisEnabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-normandy",
            Label = "Disable Firefox Normandy/Shield",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Mozilla Normandy/Shield system used for remote experiments and preference rollouts. Default: enabled.",
            Tags = ["firefox", "normandy", "shield", "experiments"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "app.normandy.enabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-beacon-api",
            Label = "Disable Firefox Beacon API",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Beacon API in Firefox. Prevents sites from sending analytics data asynchronously on page unload. Default: enabled.",
            Tags = ["firefox", "beacon", "api", "tracking"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "beacon.enabled", 0)],
        },
        new TweakDef
        {
            Id = "firefox-disable-webrtc-leak",
            Label = "Disable Firefox WebRTC IP Leak",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents WebRTC from leaking local IP addresses. Enhances VPN privacy. Default: leaks IP.",
            Tags = ["firefox", "webrtc", "ip", "leak", "vpn"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "media.peerconnection.ice.no_host", 1)],
        },
        new TweakDef
        {
            Id = "firefox-disable-speculative-connections",
            Label = "Disable Firefox Speculative Connections",
            Category = "Firefox",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Firefox from pre-connecting to sites when hovering over links. Reduces network requests. Default: enabled.",
            Tags = ["firefox", "speculative", "connections", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Mozilla\Firefox\Preferences", "network.http.speculative-parallel-limit", 0)],
        },
    ];
}
