#nullable enable
using System.Collections.Generic;
using RegiLattice.Core.Models;

namespace RegiLattice.Core.Tweaks;

// Sprint 254 — Wi-Fi Hotspot Authentication Policy (10 tweaks)
// Keys: HKLM\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\Local
//       HKLM\SOFTWARE\Policies\Microsoft\Windows\Wireless\GPTWirelessPolicy
internal static class HotspotAuthenticationPolicy
{
    private const string HsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication";
    private const string WcmLocal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\Local";
    private const string WirelessGpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Wireless\GPTWirelessPolicy";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "hotspot-disable-captive-portal",
            Label = "Disable Captive Portal Detection",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets Enabled=0 in the HotspotAuthentication policy key. "
                + "Prevents Windows from detecting captive portal Wi-Fi hotspots and launching the "
                + "browser-based authentication dialog. Reduces network probing and location privacy leakage "
                + "on public or untrusted networks. "
                + "Default: absent (captive portal detection on). Recommended: 0 on corporate or locked-down devices.",
            Tags = ["hotspot", "captive-portal", "wifi", "authentication", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Captive portal auto-detect and browser pop-up disabled; user must manually navigate to the portal.",
            ApplyOps = [RegOp.SetDword(HsKey, "Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(HsKey, "Enabled")],
            DetectOps = [RegOp.CheckDword(HsKey, "Enabled", 0)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-auto-connect-new",
            Label = "Disable Auto-Connect to New Wi-Fi Networks",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fBlockNonDomain=1 in the WcmSvc Local policy key. "
                + "Prevents Windows from automatically connecting to new or unknown Wi-Fi networks, "
                + "including open hotspots. Only pre-configured domain or saved networks are allowed. "
                + "Default: absent (auto-connect allowed). Recommended: 1 on corporate domain machines.",
            Tags = ["hotspot", "wifi", "auto-connect", "domain", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Windows will not automatically connect to new Wi-Fi hotspots; only saved networks are used.",
            ApplyOps = [RegOp.SetDword(WcmLocal, "fBlockNonDomain", 1)],
            RemoveOps = [RegOp.DeleteValue(WcmLocal, "fBlockNonDomain")],
            DetectOps = [RegOp.CheckDword(WcmLocal, "fBlockNonDomain", 1)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-internet-sharing",
            Label = "Disable Wi-Fi Internet Connection Sharing",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets NC_ShowSharedAccessUI=0 in the WcmSvc Local policy key. "
                + "Hides and disables the Internet Connection Sharing (ICS) functionality in Windows "
                + "network connection properties, preventing the device from acting as a Wi-Fi hotspot. "
                + "Default: absent (ICS UI shown). Recommended: 0 on corporate devices.",
            Tags = ["hotspot", "ics", "sharing", "wifi", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Internet Connection Sharing (ICS) hotspot functionality hidden and disabled from the UI.",
            ApplyOps = [RegOp.SetDword(WcmLocal, "NC_ShowSharedAccessUI", 0)],
            RemoveOps = [RegOp.DeleteValue(WcmLocal, "NC_ShowSharedAccessUI")],
            DetectOps = [RegOp.CheckDword(WcmLocal, "NC_ShowSharedAccessUI", 0)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-wifi-sense",
            Label = "Disable Wi-Fi Sense Contact Sharing",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fScanConnectIntervalNearby=0 in the WcmSvc Local policy key. "
                + "Disables the Wi-Fi Sense feature that automatically shares Wi-Fi passwords with "
                + "contacts via Microsoft account. Prevents credential sharing across devices and accounts. "
                + "Default: absent (Wi-Fi Sense on). Recommended: 0 for credential hygiene.",
            Tags = ["hotspot", "wifi-sense", "sharing", "contacts", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Wi-Fi Sense nearby network sharing and credential auto-exchange disabled.",
            ApplyOps = [RegOp.SetDword(WcmLocal, "fScanConnectIntervalNearby", 0)],
            RemoveOps = [RegOp.DeleteValue(WcmLocal, "fScanConnectIntervalNearby")],
            DetectOps = [RegOp.CheckDword(WcmLocal, "fScanConnectIntervalNearby", 0)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-manual-hotspot",
            Label = "Disable Mobile Hotspot Feature",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets AllowHotspot=0 in the WcmSvc Local policy key. "
                + "Prevents users from enabling the Windows Mobile Hotspot feature, which turns the "
                + "device into a Wi-Fi hotspot sharing its internet connection. "
                + "Default: absent (hotspot allowed). Recommended: 0 on corporate devices to prevent unauthorized internet sharing.",
            Tags = ["hotspot", "mobile-hotspot", "sharing", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Windows Mobile Hotspot feature disabled; users cannot share internet via Wi-Fi.",
            ApplyOps = [RegOp.SetDword(WcmLocal, "AllowHotspot", 0)],
            RemoveOps = [RegOp.DeleteValue(WcmLocal, "AllowHotspot")],
            DetectOps = [RegOp.CheckDword(WcmLocal, "AllowHotspot", 0)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-hotspot2",
            Label = "Disable Wi-Fi Hotspot 2.0 / Passpoint",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fDisablePassport=1 in the HotspotAuthentication policy key. "
                + "Disables the Wi-Fi Hotspot 2.0 (Passpoint / 802.11u) automatic authentication protocol. "
                + "Prevents Windows from automatically authenticating to Hotspot 2.0-capable public networks "
                + "using stored service credentials. "
                + "Default: absent. Recommended: 1 to prevent auto-auth to unknown carrier Wi-Fi networks.",
            Tags = ["hotspot", "hotspot2", "passpoint", "802.11u", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hotspot 2.0 / Passpoint auto-authentication disabled; public carrier Wi-Fi not auto-joined.",
            ApplyOps = [RegOp.SetDword(HsKey, "fDisablePassport", 1)],
            RemoveOps = [RegOp.DeleteValue(HsKey, "fDisablePassport")],
            DetectOps = [RegOp.CheckDword(HsKey, "fDisablePassport", 1)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-network-roaming",
            Label = "Disable Wi-Fi Network Roaming",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets DisableRoaming=1 in the WcmSvc Local policy key. "
                + "Prevents the Windows wireless service from automatically roaming between Wi-Fi access points, "
                + "including between networks with the same SSID at different locations. "
                + "Locks the device to its current network association until manually disconnected. "
                + "Default: absent (roaming enabled). Recommended: 1 on fixed workstations.",
            Tags = ["hotspot", "roaming", "wifi", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "Wi-Fi automatic roaming between access points/SSIDs disabled.",
            ApplyOps = [RegOp.SetDword(WcmLocal, "DisableRoaming", 1)],
            RemoveOps = [RegOp.DeleteValue(WcmLocal, "DisableRoaming")],
            DetectOps = [RegOp.CheckDword(WcmLocal, "DisableRoaming", 1)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-wlan-autoconfig",
            Label = "Block WLAN AutoConfig Profile Changes",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fPreventAutoConnectToWiFiSenseHotspots=1 in the HotspotAuthentication policy key. "
                + "Prevents WLAN AutoConfig from applying automatic Wi-Fi profile changes from the Hotspot "
                + "authentication service. Ensures only IT-provisioned wireless profiles are used. "
                + "Default: absent. Recommended: 1 on corporate devices.",
            Tags = ["hotspot", "wlan", "autoconfig", "profile", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "WLAN AutoConfig cannot apply hotspot-originated wireless profile changes.",
            ApplyOps = [RegOp.SetDword(HsKey, "fPreventAutoConnectToWiFiSenseHotspots", 1)],
            RemoveOps = [RegOp.DeleteValue(HsKey, "fPreventAutoConnectToWiFiSenseHotspots")],
            DetectOps = [RegOp.CheckDword(HsKey, "fPreventAutoConnectToWiFiSenseHotspots", 1)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-wireless-gpt-policy",
            Label = "Block GPT Wireless Policy Push",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fEnableGPTWirelessPolicy=0 in the GPTWirelessPolicy key. "
                + "Prevents Windows Wireless Group Policy from applying Group Policy Template (GPT) wireless "
                + "profiles pushed from an Active Directory GPO. Useful when wireless profiles are managed "
                + "by a third-party MDM or RADIUS and AD wireless GPOs are not used. "
                + "Default: absent (GPT policy applied). Recommended: 0 in non-AD wireless deployments.",
            Tags = ["hotspot", "gpt", "wireless", "group-policy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            ImpactNote = "GPT wireless profile push from AD Group Policy Objects is disabled.",
            ApplyOps = [RegOp.SetDword(WirelessGpt, "fEnableGPTWirelessPolicy", 0)],
            RemoveOps = [RegOp.DeleteValue(WirelessGpt, "fEnableGPTWirelessPolicy")],
            DetectOps = [RegOp.CheckDword(WirelessGpt, "fEnableGPTWirelessPolicy", 0)],
        },
        new TweakDef
        {
            Id = "hotspot-disable-credential-caching",
            Label = "Disable Hotspot Credential Caching",
            Category = "Wi-Fi Hotspot Authentication Policy",
            Description =
                "Sets fCacheCredentials=0 in the HotspotAuthentication policy key. "
                + "Prevents the Hotspot 2.0 authentication service from caching Wi-Fi network "
                + "credentials (username/password) for previously authenticated public networks. "
                + "Improves security by forcing re-authentication on each new connection. "
                + "Default: absent (credentials cached). Recommended: 0 on privacy-conscious devices.",
            Tags = ["hotspot", "credentials", "caching", "privacy", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Hotspot authentication credentials not cached; re-authentication required on every connection.",
            ApplyOps = [RegOp.SetDword(HsKey, "fCacheCredentials", 0)],
            RemoveOps = [RegOp.DeleteValue(HsKey, "fCacheCredentials")],
            DetectOps = [RegOp.CheckDword(HsKey, "fCacheCredentials", 0)],
        },
    ];
}
