namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyNetwork
{
    // ── HotspotAuthenticationPolicy ──
    private static class _HotspotAuthenticationPolicy
    {
        private const string HsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HotspotAuthentication";
        private const string WcmLocal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WcmSvc\Local";
        private const string WirelessGpt = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Wireless\GPTWirelessPolicy";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "hotspot-disable-captive-portal",
                Label = "Disable Captive Portal Detection",
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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
                Category = "Network — Dns Secure",
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

    // ── Ieee8021xPolicy ──
    private static class _Ieee8021xPolicy
    {
        private const string WiredKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WiredNetwork";

        private const string WirelessKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork";

        private const string EapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EapHost";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ieee8021x-enable-wired-8021x",
                    Label = "IEEE 802.1x: Enable Wired Network 802.1x Authentication",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets EnableAutoConfig=1 in WiredNetwork policy. Activates the Windows wired 802.1x supplicant (the Wired AutoConfig service) and enables port-based network access control on all wired Ethernet adapters. With 802.1x enabled, the switch's authentication server (RADIUS) validates the endpoint's identity before granting network access. Unauthenticated endpoints are placed in a guest VLAN or denied access. Essential security control for protecting internal network access via physical Ethernet port.",
                    Tags = ["ieee8021x", "wired", "authentication", "network", "nac"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 3,
                    ImpactNote =
                        "Wired 802.1x authentication is enforced. Network switches must support 802.1x and be configured with RADIUS server settings. Machines without valid credentials are denied network access — ensure machine certificates are enrolled first.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "EnableAutoConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "EnableAutoConfig")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "EnableAutoConfig", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-eapol-start-timeout",
                    Label = "IEEE 802.1x: Set EAPOL-Start Timeout to 20 Seconds",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets AuthResponse=20 in WiredNetwork policy. Configures the supplicant's EAPOL-Start transmission timer: how long the supplicant waits for an EAPOL Request-Identity from the switch before sending an explicit EAPOL-Start frame. On access switches that do not send the initial EAP Request/Identity packet (authenticator-initiated), the supplicant must initiate. A longer initial wait delays authentication. Setting to 20 seconds reduces the wait for supplicant-initiated authentication without being so short it floods slow switch responses.",
                    Tags = ["ieee8021x", "eapol", "timeout", "supplicant", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAPOL-Start timer set to 20 seconds. Only affects authentication initiation timing. Network connectivity is unchanged once authentication completes.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "AuthResponse", 20)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "AuthResponse")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "AuthResponse", 20)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-max-eapol-retransmit",
                    Label = "IEEE 802.1x: Limit EAPOL-Start Retransmit Count to 5",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets MaxEapolStartAttempts=5 in WiredNetwork policy. Limits the number of EAPOL-Start retransmission attempts before the supplicant gives up and treats the port as unauthenticated. Each EAPOL-Start is separated by the AuthResponse timer (AuthResponse seconds). With 5 retransmits at 20 seconds each, the supplicant spends 100 seconds attempting to authenticate before failing. This limits the time a machine retries against an unavailable RADIUS server while ensuring legitimate connection delays are handled gracefully.",
                    Tags = ["ieee8021x", "eapol", "retransmit", "retry", "supplicant"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAPOL-Start retransmit limit is 5 attempts. After 5 failures, the supplicant stops trying. Most switches respond to the first EAPOL-Start; retransmits are only relevant for switches with slow RADIUS response.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "MaxEapolStartAttempts", 5)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "MaxEapolStartAttempts")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "MaxEapolStartAttempts", 5)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-enable-single-sign-on",
                    Label = "IEEE 802.1x: Enable Single Sign-On Integration",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets BlockPeriod=0 in WiredNetwork policy (enables SSO) and sets SingleSignOn=1. Configures the wired supplicant to synchronise 802.1x authentication with Windows logon. In SSO mode, machine certificates are used before the Windows logon screen (machine authentication), and user certificates are presented after the user logs in (user authentication). Without SSO, the machine may remain in a partially authenticated state during logon, potentially delaying Group Policy application or network-dependent logon scripts.",
                    Tags = ["ieee8021x", "sso", "logon", "authentication", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SSO integrates 802.1x with Windows logon. Machine authenticates before logon screen; user authenticates after. Requires both machine and user certificates to be enrolled.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "BlockPeriod", 0), RegOp.SetDword(WiredKey, "SingleSignOn", 1)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "BlockPeriod"), RegOp.DeleteValue(WiredKey, "SingleSignOn")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "BlockPeriod", 0), RegOp.CheckDword(WiredKey, "SingleSignOn", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-user-only-mode",
                    Label = "IEEE 802.1x: Disable User-Only 802.1x Mode (Require Machine Auth)",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets UserOnlyMode=0 in WiredNetwork policy. Prevents the supplicant from operating in user-only authentication mode where only user credentials are sent and machine credentials are not used. In user-only mode, the machine VLAN access depends entirely on the currently logged-in user's authentication; when no user is logged in (e.g., at Windows logon screen, or when machine admin scripts run before user logon), the port may be unauthenticated. Machine authentication ensures the port is authenticated regardless of user logon state.",
                    Tags = ["ieee8021x", "machine-auth", "user-auth", "wired", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Machine authentication is required; user-only mode is disabled. Machine certificates must be enrolled before the logon screen appears. Group Policy and SCCM scripts that run at machine startup require machine-authenticated network access.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "UserOnlyMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "UserOnlyMode")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "UserOnlyMode", 0)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-guest-vlan-access",
                    Label = "IEEE 802.1x: Disable Automatic Guest VLAN Fallback on Auth Failure",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets AllowGuestVLAN=0 in WiredNetwork policy. Prevents the Windows 802.1x supplicant from signalling to the switch that it accepts placement in a guest VLAN on authentication failure. Some switch configurations use EAP-Failure with a VLAN assignment to place unauthenticated endpoints in a restricted guest VLAN. Setting AllowGuestVLAN=0 causes the supplicant to treat authentication failure as a complete block rather than accepting reduced-access guest VLAN placement. Prevents accidental network access via the guest VLAN.",
                    Tags = ["ieee8021x", "guest-vlan", "fallback", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Guest VLAN fallback is disabled. Failed authentication results in no network access rather than guest VLAN. Ensure there is an alternate recovery path (console access, out-of-band management) for machines with certificate failures.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "AllowGuestVLAN", 0)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "AllowGuestVLAN")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "AllowGuestVLAN", 0)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-set-eap-type-tls",
                    Label = "IEEE 802.1x: Set Wired EAP Method to EAP-TLS (Certificate-Based)",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets EAPType=13 in WiredNetwork policy (EAP-TLS, EAP type 13 per IANA). Configures the Windows wired 802.1x supplicant to use EAP-TLS as the preferred EAP authentication method. EAP-TLS uses mutual certificate-based authentication: the client presents a certificate (from machine store or smart card) and the RADIUS server presents a server certificate. It is the strongest EAP method available, providing phishing-resistant authentication and protection against credential interception attacks that affect password-based PEAP.",
                    Tags = ["ieee8021x", "eap-tls", "certificate", "authentication", "wired"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "EAP-TLS requires client certificates on all machines. Machine certificates must be enrolled via enterprise CA before deployment. Strongest 802.1x authentication method — recommended for high-security environments.",
                    ApplyOps = [RegOp.SetDword(WiredKey, "EAPType", 13)],
                    RemoveOps = [RegOp.DeleteValue(WiredKey, "EAPType")],
                    DetectOps = [RegOp.CheckDword(WiredKey, "EAPType", 13)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-enable-eap-inner-identity",
                    Label = "IEEE 802.1x: Enable Anonymous Identity (Outer Identity Privacy)",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets EnableAnonymousIdentity=1 in EapHost policy. Enables the use of an anonymous outer identity in tunnelled EAP methods (PEAP, TTLS). The EAP Identity response (outer identity) is transmitted in plaintext in the first EAP exchange. Without anonymous identity, the user's actual username is visible in the network traffic of the outer EAP exchange, revealing who is authenticating before the TLS tunnel is established. An anonymous outer identity (e.g., 'anonymous@contoso.com') hides the actual username until the protected tunnel is established.",
                    Tags = ["ieee8021x", "eap", "identity", "privacy", "peap"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous identity is sent as EAP outer identity. The RADIUS server must be configured to match the anonymous realm. The real user identity is still used inside the TLS tunnel for authentication.",
                    ApplyOps = [RegOp.SetDword(EapKey, "EnableAnonymousIdentity", 1)],
                    RemoveOps = [RegOp.DeleteValue(EapKey, "EnableAnonymousIdentity")],
                    DetectOps = [RegOp.CheckDword(EapKey, "EnableAnonymousIdentity", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-disable-cached-wireless-creds",
                    Label = "IEEE 802.1x: Disable Wireless 802.1x Credential Caching",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets DisableUserCredentialCaching=1 in WirelessNetwork policy. Prevents the Windows wireless supplicant from caching 802.1x user credentials to local storage. Some EAP methods (PEAP-MSCHAPv2) can cache credentials to allow re-authentication without prompting the user. While convenient, cached credentials are a persistence mechanism for the credentials and may be accessible from the credential cache if the machine is compromised. Required credentials are re-fetched on each authentication using the user's interactive session or certificate store.",
                    Tags = ["ieee8021x", "wireless", "credential-cache", "security", "wifi"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Wireless 802.1x credentials are not cached. Users may be re-prompted for credentials on reconnection if using PEAP-MSCHAPv2. Certificate-based EAP-TLS is unaffected.",
                    ApplyOps = [RegOp.SetDword(WirelessKey, "DisableUserCredentialCaching", 1)],
                    RemoveOps = [RegOp.DeleteValue(WirelessKey, "DisableUserCredentialCaching")],
                    DetectOps = [RegOp.CheckDword(WirelessKey, "DisableUserCredentialCaching", 1)],
                },
                new TweakDef
                {
                    Id = "ieee8021x-require-mutual-authentication",
                    Label = "IEEE 802.1x: Require Mutual Authentication in PEAP Handshake",
                    Category = "Network — Dns Secure",
                    Description =
                        "Sets RequireMutualAuth=1 in EapHost policy. Requires that tunnelled EAP methods (PEAP, TTLS) perform full mutual authentication: the server must present a trusted certificate AND the client must authenticate with a credential inside the TLS tunnel. Without mutual authentication, a rogue access point can complete the outer TLS handshake with any certificate while the client still transmits their inner credentials to an attacker-controlled server. Mutual authentication (server cert validation + valid inner credentials) is the minimum security requirement for PEAP deployments.",
                    Tags = ["ieee8021x", "mutual-authentication", "peap", "certificate", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Mutual authentication is required for PEAP/TTLS. RADIUS/NPS servers must present a trusted certificate. Ensures both server and client authenticate — critical for preventing evil twin attacks.",
                    ApplyOps = [RegOp.SetDword(EapKey, "RequireMutualAuth", 1)],
                    RemoveOps = [RegOp.DeleteValue(EapKey, "RequireMutualAuth")],
                    DetectOps = [RegOp.CheckDword(EapKey, "RequireMutualAuth", 1)],
                },
            ];
    }

    // ── InternetCommunicationPolicy ──
    private static class _InternetCommunicationPolicy
    {
        private const string InetMgmtKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\InternetManagement";
        private const string InetRestrictKey =
            @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\CurrentVersion\Software Protection Platform";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inetcomm-restrict-internet-communication",
                Label = "Internet Communication: Restrict All Internet Communication Features",
                Category = "Network — Dns Secure",
                Description =
                    "Enables the master Internet Communication Management policy that restricts or disables all Windows features that communicate with the internet, including Windows Error Reporting, Windows Update, Help and Support Center online search, Microsoft Customer Experience Improvement Program (CEIP), online activation, and other phone-home features. This is the master switch that enables sub-policies defined elsewhere under the InternetManagement key.",
                Tags = ["internet communication", "internet restriction", "privacy", "phone home", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "RestrictInternetCommunication", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "RestrictInternetCommunication")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "RestrictInternetCommunication", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Broad internet communication block; may affect Windows Update, activation, and online Help. Test thoroughly before deploying.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-printing-over-http",
                Label = "Internet Communication: Disable Printing Over HTTP",
                Category = "Network — Dns Secure",
                Description =
                    "Disables the ability for Windows to send print jobs over HTTP, which is used for sending documents to Internet Printing Protocol (IPP) printers outside the corporate network. HTTP printing can bypass proxy controls and DLP systems. On managed networks, all printing should be directed to IT-managed printer queues; direct-to-internet IPP printing is a potential data exfiltration vector.",
                Tags = ["internet communication", "printing", "http", "data loss prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableHTTPPrinting", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableHTTPPrinting")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableHTTPPrinting", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Internet Printing Protocol (IPP) over HTTP; LAN-connected print servers are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-windows-update-access",
                Label = "Internet Communication: Disable Access to Windows Update (Non-WSUS)",
                Category = "Network — Dns Secure",
                Description =
                    "Blocks Windows from accessing Microsoft's Windows Update servers directly, restricting update access to corporate WSUS or Windows Update for Business (WUfB) only. Direct Windows Update connections bypass the organization's patch approval process, potentially installing untested updates or creating tracking data. This policy should be combined with a WSUS or Intune Update Ring configuration to ensure updates are still received through an approved channel.",
                Tags = ["internet communication", "windows update", "wsus", "patch management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableWindowsUpdateAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableWindowsUpdateAccess")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableWindowsUpdateAccess", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Blocks direct access to Windows Update; requires WSUS or WUfB to be configured for updates to be received.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-web-communities",
                Label = "Internet Communication: Disable Windows Web Communities Feature",
                Category = "Network — Dns Secure",
                Description =
                    "Disables the Windows Web Communities feature that allowed Windows Explorer and Help Center to automatically submit queries to Microsoft-hosted community websites (forums, knowledge base, support articles). While useful in consumer scenarios, this feature sends details about the user's system context, open documents, and help searches to external Microsoft servers, which creates a data-leakage concern in regulated business environments.",
                Tags = ["internet communication", "communities", "help center", "telemetry", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableWebCommunities", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableWebCommunities")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableWebCommunities", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables Windows community search in Help Center; standard online help articles are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-event-viewer-links",
                Label = "Internet Communication: Disable Event Viewer Online Links",
                Category = "Network — Dns Secure",
                Description =
                    "Prevents Event Viewer from displaying the 'More Information' link that opens a Microsoft online support page when viewing event log entries. Clicking these links sends event detail data (Event ID, source, parameters) to Microsoft's online event log lookup service. In sensitive environments, event log data may contain internal application identifiers, username fragments, or file path details that should not be transmitted outside the network.",
                Tags = ["internet communication", "event viewer", "online links", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableEventViewer", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableEventViewer")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableEventViewer", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes 'More Information' online links from Event Viewer; local event logs are fully accessible.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-registration-wizard",
                Label = "Internet Communication: Disable Windows Registration Wizard",
                Category = "Network — Dns Secure",
                Description =
                    "Disables the Windows Product Registration Wizard that appears after OS installation and prompts users to register the Windows license with Microsoft. The registration process transmits hardware information (CPU, RAM, disk size), Windows edition, and product key metadata to Microsoft's registration servers. On volume-licensed enterprise deployments, this registration is handled by Microsoft's volume activation infrastructure and the wizard is unnecessary.",
                Tags = ["internet communication", "registration", "product key", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableRegistration")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableRegistration", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables the Windows registration wizard; volume-licensed systems are unaffected — VL activation works separately.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-windows-activation-online",
                Label = "Internet Communication: Disable Windows Online Activation",
                Category = "Network — Dns Secure",
                Description =
                    "Prevents Windows from attempting online activation via Microsoft's activation servers. In enterprise environments using KMS (Key Management Service) or MAK (Multiple Activation Key) volume licensing, Windows activates against the internal KMS server. Attempting simultaneous online activation can cause interference and may expose the KMS key or activation state to Microsoft's telemetry infrastructure.",
                Tags = ["internet communication", "activation", "kms", "volume license", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetRestrictKey],
                ApplyOps = [RegOp.SetDword(InetRestrictKey, "NoGenTicket", 1)],
                RemoveOps = [RegOp.DeleteValue(InetRestrictKey, "NoGenTicket")],
                DetectOps = [RegOp.CheckDword(InetRestrictKey, "NoGenTicket", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Disables online activation; requires KMS or MAK infrastructure to be in place for Windows to remain activated.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-task-scheduler-download",
                Label = "Internet Communication: Disable Task Scheduler Internet Download",
                Category = "Network — Dns Secure",
                Description =
                    "Prevents Windows Task Scheduler from downloading programs, scripts, or task definitions from internet URIs. Task Scheduler tasks can be configured with action items that fetch content from HTTP/HTTPS URLs. Blocking internet downloads from Task Scheduler prevents lateral movement via scheduled tasks that pull malware from C2 servers and prevents administrative tasks from overriding proxy policies by downloading directly.",
                Tags = ["internet communication", "task scheduler", "download", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableTaskSchedulerDownload", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableTaskSchedulerDownload")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableTaskSchedulerDownload", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks internet downloads initiated by Task Scheduler actions; scheduled tasks with local scripts are unaffected.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-online-search-help",
                Label = "Internet Communication: Disable Windows Online Search in Help",
                Category = "Network — Dns Secure",
                Description =
                    "Prevents the Windows Help and Support Center from augmenting local help content with online Microsoft documentation and search results. The online search feature sends the user's help query terms and partial system context to Microsoft's servers. In environments where query terms may contain project names, application names, or technical details classified as business-sensitive, disabling online help search prevents inadvertent disclosure.",
                Tags = ["internet communication", "help center", "online search", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableOnlineSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableOnlineSearch")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableOnlineSearch", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Limits Help Center to local offline articles; online Microsoft documentation is not fetched.",
            },
            new TweakDef
            {
                Id = "inetcomm-disable-driver-update-internet",
                Label = "Internet Communication: Disable Driver Download from Windows Update",
                Category = "Network — Dns Secure",
                Description =
                    "Prevents Windows from automatically downloading device driver updates directly from Windows Update when a new device is plugged in. Driver updates fetched directly from Windows Update bypass the organization's driver qualification and testing process. In managed environments, all driver updates should go through SCCM, Intune, or another MDM platform that can test and stage driver deployments before production rollout.",
                Tags = ["internet communication", "driver update", "windows update", "device management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [InetMgmtKey],
                ApplyOps = [RegOp.SetDword(InetMgmtKey, "DisableDriverUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(InetMgmtKey, "DisableDriverUpdate")],
                DetectOps = [RegOp.CheckDword(InetMgmtKey, "DisableDriverUpdate", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote =
                    "Stops Windows from fetching driver updates via Windows Update; driver management must be handled via MDM or manual deployment.",
            },
        ];
    }

    // ── IpsecRulePolicy ──
    private static class _IpsecRulePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\PolicyAgent\Oakley";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\IPSec\LocalPolicyModule";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ipsecpol-disable-default-exemptions",
                    Label = "Disable IPSec Default Exemptions",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets DisableDefaultExemptions=3 to remove built-in IKE, Kerberos, and multicast exemptions, ensuring all traffic is subject to IPSec filtering rules.",
                    Tags = ["ipsec", "ike", "exemptions", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Removes default exemptions; may disrupt Kerberos until IPSec rules are configured.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableDefaultExemptions", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableDefaultExemptions")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableDefaultExemptions", 3)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-strong-crl-check",
                    Label = "Enable Strong CRL Checking for IPSec",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Enables certificate revocation list (CRL) checking for certificates used in IPSec authentication, preventing revoked certificates from being accepted.",
                    Tags = ["ipsec", "crl", "certificate", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "CRL checked per IKE negotiation; requires CRL availability at connection time.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableCRLCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableCRLCheck")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableCRLCheck", 1)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-ike-key-lifetime",
                    Label = "Set IKE Main Mode Key Lifetime to 8 Hours",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets the IKE main mode key lifetime to 480 minutes (8 hours). Regular renegotiation limits the window of exposure if a key is compromised.",
                    Tags = ["ipsec", "ike", "key-lifetime", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Shorter lifetime improves key hygiene; increases IKE renegotiation frequency.",
                    ApplyOps = [RegOp.SetDword(Key, "IKEKeyExpirationTime", 480)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IKEKeyExpirationTime")],
                    DetectOps = [RegOp.CheckDword(Key, "IKEKeyExpirationTime", 480)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-session-key-lifetime",
                    Label = "Set IPSec Session Key Lifetime to 15 Minutes",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets the IPSec quick mode session key lifetime to 900 seconds (15 minutes), limiting the impact window of a compromised session key.",
                    Tags = ["ipsec", "session-key", "lifetime", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "15-minute session key reduces exposed data per compromise; slight CPU overhead on busy links.",
                    ApplyOps = [RegOp.SetDword(Key, "IKESessionKeyLifetime", 900)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IKESessionKeyLifetime")],
                    DetectOps = [RegOp.CheckDword(Key, "IKESessionKeyLifetime", 900)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-enable-pfs",
                    Label = "Enable Perfect Forward Secrecy for IPSec",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Enables Perfect Forward Secrecy (PFS) so each session key is derived independently, preventing compromise of one key from exposing past or future sessions.",
                    Tags = ["ipsec", "pfs", "encryption", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Adds computational overhead per session key negotiation; essential for high-security environments.",
                    ApplyOps = [RegOp.SetDword(Key, "EnablePFS", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnablePFS")],
                    DetectOps = [RegOp.CheckDword(Key, "EnablePFS", 1)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-require-dh-group2",
                    Label = "Require Diffie-Hellman Group 2 for IKE",
                    Category = "Network — Ipsec Rule",
                    Description = "Sets the minimum DH group to Group 2 (1024-bit MODP) for IKE negotiation, blocking the weaker Group 1 (768-bit).",
                    Tags = ["ipsec", "dh", "diffie-hellman", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Requires DH Group 2+; incompatible with peers using obsolete Group 1.",
                    ApplyOps = [RegOp.SetDword(Key, "DHGroup", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DHGroup")],
                    DetectOps = [RegOp.CheckDword(Key, "DHGroup", 2)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-enable-ah-integrity",
                    Label = "Enable AH Integrity Checking for IPSec",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Enables the AH (Authentication Header) integrity mechanism, ensuring packet headers are cryptographically verified during IPSec communication.",
                    Tags = ["ipsec", "ah", "integrity", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "AH header authentication adds integrity; incompatible with NAT traversal.",
                    ApplyOps = [RegOp.SetDword(Key2, "EnableAHIntegrity", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "EnableAHIntegrity")],
                    DetectOps = [RegOp.CheckDword(Key2, "EnableAHIntegrity", 1)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-block-null-encryption",
                    Label = "Block Null Encryption in IPSec ESP",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables null encryption in ESP (Encapsulating Security Payload), ensuring all IPSec-encrypted traffic uses a real cipher such as AES.",
                    Tags = ["ipsec", "esp", "encryption", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "All ESP tunnels must use a real cipher; null-encryption tunnels are rejected.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableNullEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableNullEncryption")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableNullEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-require-esp-encryption",
                    Label = "Require ESP Encryption for All IPSec Tunnels",
                    Category = "Network — Ipsec Rule",
                    Description = "Requires ESP with encryption for all IPSec connections, preventing integrity-only AH-only or unencrypted tunnels.",
                    Tags = ["ipsec", "esp", "encryption", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces encrypted ESP; AH-only tunnels are disallowed.",
                    ApplyOps = [RegOp.SetDword(Key2, "RequireESPEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RequireESPEncryption")],
                    DetectOps = [RegOp.CheckDword(Key2, "RequireESPEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "ipsecpol-negotiation-poll-interval",
                    Label = "Set IPSec Negotiation Poll Interval to 5 Minutes",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets the IPSec policy negotiation polling interval to 300 seconds (5 minutes), controlling how frequently the service checks for policy changes.",
                    Tags = ["ipsec", "negotiation", "policy", "interval"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Reduces policy-update latency; negligible performance impact.",
                    ApplyOps = [RegOp.SetDword(Key2, "NegotiationPollInterval", 300)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "NegotiationPollInterval")],
                    DetectOps = [RegOp.CheckDword(Key2, "NegotiationPollInterval", 300)],
                },
            ];
    }

    // ── Ipv6Policy ──
    private static class _Ipv6Policy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Tcpip\Parameters";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "ipv6pol-disable-ipv6",
                Label = "Disable IPv6 on All Network Adapters",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                Description =
                    "IPv6 provides the next generation internet protocol addressing and communication capabilities alongside or replacing IPv4. Disabling IPv6 on all interfaces prevents the endpoint from using IPv6 for any network communication and removes associated attack surfaces. IPv6 tunneling protocols including Teredo and 6to4 can bypass IPv4-based network security controls by encapsulating IPv6 within IPv4. Networks not prepared to monitor and filter IPv6 traffic have security gaps when endpoints use IPv6 active alongside IPv4. Enterprise environments that do not operate IPv6 network infrastructure should disable IPv6 to prevent protocol confusion and bypass risks. Organizations actively deploying IPv6 should not apply this tweak and should instead configure IPv6 security controls appropriately.",
                Tags = ["ipv6", "network", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIPv6", 0xFF)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPv6", 0xFF)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-teredo",
                Label = "Disable Teredo IPv6 Tunnel",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Teredo is an IPv6 tunneling protocol that encapsulates IPv6 traffic inside UDP packets enabling IPv6 connectivity through IPv4 NAT devices. Disabling Teredo prevents endpoints from establishing IPv6 tunnels that can bypass IPv4-based network security controls. Teredo tunnels traverse NAT and firewall devices that are configured for IPv4 only creating unmonitored network paths. Malware uses Teredo to establish IPv6-based command and control connections that bypass IPv4-only security monitoring. Enterprise networks should use native IPv6 or approved tunnel mechanisms rather than automatic tunnel adapters like Teredo. Disabling Teredo does not affect native IPv6 connectivity or explicitly configured enterprise IPv6 tunnels.",
                Tags = ["ipv6", "teredo", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTeredo", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTeredo")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTeredo", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-6to4",
                Label = "Disable 6to4 IPv6 Tunnel Adapter",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "6to4 is an automatic IPv6 tunneling mechanism that encapsulates IPv6 packets inside IPv4 packets using protocol 41. Disabling 6to4 prevents automatic IPv6 tunnel creation through the 6to4 relay infrastructure operated by third parties. 6to4 relay servers on the internet are not controlled by enterprise network administrators and represent unmonitored egress paths. Traffic through 6to4 relays bypasses enterprise security controls that operate on the IPv4 network layer. The 6to4 mechanism has known security weaknesses including inability to verify the identity of relay servers used for the tunnel. Disabling 6to4 is recommended for enterprise networks that do not require automatic IPv6 tunnel establishment.",
                Tags = ["ipv6", "6to4", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "Disable6to4", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "Disable6to4")],
                DetectOps = [RegOp.CheckDword(Key, "Disable6to4", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-isatap",
                Label = "Disable ISATAP IPv6 Tunnel",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Intra-Site Automatic Tunnel Addressing Protocol enables IPv6 communication within IPv4 intranets by creating automatic virtual IPv6 adapters. Disabling ISATAP prevents automatic creation of IPv6 tunnels within the enterprise intranet using the ISATAP mechanism. ISATAP tunnels are typically managed by enterprise IT but the automatic discovery and tunnel creation aspects reduce IT control. Legacy ISATAP deployments represent a transition technology that should be replaced by native IPv6 as part of enterprise IPv6 planning. Endpoints with ISATAP enabled have additional network interfaces that must be independently secured and monitored. Disabling ISATAP simplifies network adapter management and reduces the number of active network interfaces requiring security consideration.",
                Tags = ["ipv6", "isatap", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableISATAP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableISATAP")],
                DetectOps = [RegOp.CheckDword(Key, "DisableISATAP", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-prefer-ipv4",
                Label = "Prefer IPv4 over IPv6 by Default",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Windows preferentially uses IPv6 for network connections when both IPv4 and IPv6 addresses are available for a destination. Setting IPv4 preference ensures that endpoint connections use the enterprise-monitored IPv4 network rather than IPv6 when both are available. Enterprise network security monitoring is often more mature for IPv4 than IPv6 and IPv4 preference ensures traffic goes through monitored paths. IPv6-only destinations remain reachable through direct IPv6 connections while dual-stack destinations prefer IPv4 paths. This is a transitional setting appropriate for environments where IPv6 security monitoring lags IPv4 capability. When IPv6 monitoring is fully operational this preference setting can be removed to allow natural protocol selection.",
                Tags = ["ipv6", "preference", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PreferIpv4OverIpv6", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "PreferIpv4OverIpv6")],
                DetectOps = [RegOp.CheckDword(Key, "PreferIpv4OverIpv6", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-dhcpv6",
                Label = "Disable DHCPv6 Client",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "DHCPv6 provides automatic IPv6 address configuration for endpoints on networks with DHCPv6 servers. Disabling DHCPv6 prevents the endpoint from receiving IPv6 address assignments from DHCPv6 servers on the network. Rogue DHCPv6 servers can be set up to respond faster than legitimate servers and provide malicious IPv6 gateway and DNS configurations. DHCP starvation and rogue server attacks can redirect endpoint traffic through attacker-controlled infrastructure. Enterprises that do not deploy DHCPv6 infrastructure have no need for DHCPv6 client functionality on endpoints. Static IPv6 addressing or SLAAC can be used for legitimate IPv6 needs without DHCPv6 client-side exposure.",
                Tags = ["ipv6", "dhcpv6", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDHCPv6", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDHCPv6")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDHCPv6", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-ipv6-multicast",
                Label = "Disable IPv6 Multicast Listener Discovery",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "IPv6 Multicast Listener Discovery allows endpoints to announce multicast group subscriptions to routers and other endpoints. Disabling IPv6 MLD prevents the endpoint from participating in IPv6 multicast groups and announcing multicast subscriptions. IPv6 multicast subscriptions are broadcast to the network segment and can reveal information about applications and services running on the endpoint. Multicast-based discovery protocols expose service information that can aid network reconnaissance. Endpoints that do not require IPv6 multicast functionality have no need for MLD participation and can reduce network protocol exposure. This setting reduces the IPv6 protocol footprint on networks where multicast is not required.",
                Tags = ["ipv6", "multicast", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableIPv6Multicast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableIPv6Multicast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableIPv6Multicast", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-privacy-extensions",
                Label = "Disable IPv6 Privacy Extensions",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "IPv6 privacy extensions generate random IPv6 addresses that change periodically to prevent tracking of endpoints by external observers. Disabling IPv6 privacy extensions causes the endpoint to use its permanent EUI-64 based IPv6 address derived from the MAC address. In enterprise environments endpoint tracking through IPv6 addresses may be required for network security monitoring and incident response. Security tools that map IPv6 addresses to endpoints require consistent addresses that do not rotate to function correctly. Privacy extensions interfere with network access control systems that enforce policy based on endpoint IPv6 address identity. Disabling privacy extensions maintains consistent IPv6 address associations needed for enterprise monitoring and access control.",
                Tags = ["ipv6", "privacy", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePrivacyExtensions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivacyExtensions")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePrivacyExtensions", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-restrict-ipv6-scope",
                Label = "Restrict IPv6 to Site-Local Scope",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "IPv6 addresses can have global scope allowing them to be routed to the internet or site-local scope limiting them to the internal network. Restricting the endpoint to site-local scope IPv6 addresses prevents direct routed internet communication using the IPv6 global prefix. Global IPv6 addresses on enterprise endpoints bypass NAT-based perimeter security that restricts which internal systems have direct internet accessibility. Enterprises that provision globally routable IPv6 addresses on all endpoints provide attackers a direct path to each endpoint from the internet. Site-local IPv6 restricts internet-initiable connections requiring explicit routing policy to allow inbound IPv6 connections. This setting is appropriate for enterprise edges where inbound connection control is a security requirement.",
                Tags = ["ipv6", "scope", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictGlobalIPv6", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictGlobalIPv6")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictGlobalIPv6", 1)],
            },
            new TweakDef
            {
                Id = "ipv6pol-disable-ipv6-transition-mechs",
                Label = "Disable IPv6 Transition Technologies",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "IPv6 transition technologies including Teredo 6to4 ISATAP and other tunneling mechanisms allow IPv6 over IPv4 infrastructure. Disabling all transition technologies with a single setting prevents any automatically configured IPv6 tunnel from activating. Individual disabling of each transition mechanism provides defense-in-depth but this comprehensive setting ensures complete coverage. Transition mechanisms are often used by attackers to bypass firewall controls that only filter IPv4 traffic. Enterprise networks should deploy native IPv6 through controlled infrastructure rather than automatic tunneling mechanisms. A comprehensive disable of transition technologies eliminates the risk of overlooking any specific mechanism while planning IPv6 deployment.",
                Tags = ["ipv6", "transition", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTransitionTechnologies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTransitionTechnologies")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTransitionTechnologies", 1)],
            },
        ];
    }

    // ── LanmanServerPolicy ──
    private static class _LanmanServerPolicy
    {
        private const string SrvKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanServer";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lansrv-disable-auto-share-wks",
                    Label = "Disable Automatic Admin Shares (Workstation)",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents Windows from automatically creating administrative shares (C$, D$) on workstations.",
                    Tags = ["smb", "shares", "lanman", "security", "admin-shares"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Removes hidden admin shares used for remote administration; may break management tools.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareWks", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareWks")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareWks", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-auto-share-server",
                    Label = "Disable Automatic Admin Shares (Server)",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents Windows from automatically creating server-side default network shares (ADMIN$).",
                    Tags = ["smb", "shares", "lanman", "security", "admin-shares"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Removes server-side default shares; may impact domain management and remote admin tools.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoShareServer", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoShareServer")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoShareServer", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-plain-text-password",
                    Label = "Disable Plain-Text Password Authentication (Server)",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents the SMB server from accepting unencrypted password authentication over the network.",
                    Tags = ["smb", "authentication", "lanman", "security", "password"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks legacy clear-text SMB authentication; no impact on modern NTLMv2 or Kerberos clients.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnablePlainTextPassword", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnablePlainTextPassword")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnablePlainTextPassword", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-enable-security-signature",
                    Label = "Enable SMB Server Security Signatures",
                    Category = "Network — Ipsec Rule",
                    Description = "Enables cryptographic packet signing for SMB server connections to detect in-transit tampering.",
                    Tags = ["smb", "signing", "lanman", "security", "integrity"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Enables SMB signing; small CPU overhead; highly recommended for all environments.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-require-security-signature",
                    Label = "Require SMB Server Security Signature",
                    Category = "Network — Ipsec Rule",
                    Description = "Mandates that all SMB connections to this server use packet signing; unsigned clients are rejected.",
                    Tags = ["smb", "signing", "lanman", "security", "integrity", "enforce"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Enforces SMB signing; may break very old clients (Windows XP era) that do not support signing.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-enable-spn-validation",
                    Label = "Enable SMB Server SPN Validation",
                    Category = "Network — Ipsec Rule",
                    Description = "Requires clients to provide a valid Service Principal Name when connecting, preventing relay attacks.",
                    Tags = ["smb", "spn", "kerberos", "lanman", "security", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Hardens against NTLM relay attacks; minimal impact in domain-joined environments.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "SmbServerNameHardeningLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "SmbServerNameHardeningLevel")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "SmbServerNameHardeningLevel", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-restrict-null-session",
                    Label = "Restrict Null Session Access",
                    Category = "Network — Ipsec Rule",
                    Description = "Blocks anonymous null-session connections from enumerating shares, users, and other resources.",
                    Tags = ["smb", "null-session", "anonymous", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents anonymous enumeration; safe for domain environments that use authenticated access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "RestrictNullSessAccess", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "RestrictNullSessAccess")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "RestrictNullSessAccess", 1)],
                },
                new TweakDef
                {
                    Id = "lansrv-auto-disconnect-idle",
                    Label = "Auto-Disconnect Idle SMB Sessions",
                    Category = "Network — Ipsec Rule",
                    Description = "Automatically disconnects idle SMB client sessions after 15 minutes to reduce resource exposure.",
                    Tags = ["smb", "session", "idle", "lanman", "resource"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Idle sessions disconnected after 15 minutes; transparent reconnect on next file access.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AutoDisconnect", 15)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AutoDisconnect")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AutoDisconnect", 15)],
                },
                new TweakDef
                {
                    Id = "lansrv-disable-multicast",
                    Label = "Disable SMB WSD Multicast Discovery",
                    Category = "Network — Ipsec Rule",
                    Description = "Disables WS-Discovery multicast traffic used by SMB to advertise network shares on the local subnet.",
                    Tags = ["smb", "multicast", "discovery", "lanman", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Stops SMB multicast probes; reduces network chatter; shares remain accessible via UNC path.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "EnableMulticast", 0)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "EnableMulticast")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "EnableMulticast", 0)],
                },
                new TweakDef
                {
                    Id = "lansrv-audit-insecure-guest-logon",
                    Label = "Audit Insecure SMB Guest Logon Attempts",
                    Category = "Network — Ipsec Rule",
                    Description = "Logs an event whenever a client attempts an anonymous or guest SMB logon that would be rejected.",
                    Tags = ["smb", "audit", "guest", "lanman", "security", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enables security auditing for rejected guest logins; adds entries to the Security event log.",
                    ApplyOps = [RegOp.SetDword(SrvKey, "AuditInsecureGuestLogon", 1)],
                    RemoveOps = [RegOp.DeleteValue(SrvKey, "AuditInsecureGuestLogon")],
                    DetectOps = [RegOp.CheckDword(SrvKey, "AuditInsecureGuestLogon", 1)],
                },
            ];
    }

    // ── LanmanWorkstationPolicy ──
    private static class _LanmanWorkstationPolicy
    {
        private const string WksKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LanmanWorkstation";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lanwks-block-insecure-guest-auth",
                    Label = "Block Insecure Guest Authentication",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents the SMB client from falling back to insecure guest authentication when a server rejects credentials.",
                    Tags = ["smb", "guest", "authentication", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks unauthenticated SMB guest connections; may prevent access to old NAS using anonymous shares.",
                    ApplyOps = [RegOp.SetDword(WksKey, "AllowInsecureGuestAuth", 0)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "AllowInsecureGuestAuth")],
                    DetectOps = [RegOp.CheckDword(WksKey, "AllowInsecureGuestAuth", 0)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-plain-text-password",
                    Label = "Disable Plain-Text Password Authentication (Client)",
                    Category = "Network — Ipsec Rule",
                    Description = "Stops the SMB workstation client from sending unencrypted passwords to network servers.",
                    Tags = ["smb", "authentication", "plaintext", "password", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Blocks clear-text SMB auth on the client side; no impact on NTLMv2 or Kerberos connections.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnablePlainTextPassword", 0)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnablePlainTextPassword")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnablePlainTextPassword", 0)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-security-signature",
                    Label = "Enable SMB Client Security Signatures",
                    Category = "Network — Ipsec Rule",
                    Description = "Enables cryptographic SMB packet signing on the client for outbound connections when the server supports it.",
                    Tags = ["smb", "signing", "client", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Adds packet integrity verification; slight CPU overhead; compatible with all modern servers.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-require-security-signature",
                    Label = "Require SMB Client Security Signature",
                    Category = "Network — Ipsec Rule",
                    Description = "Mandates that the SMB client use packet signing for all connections; unsigned servers are rejected.",
                    Tags = ["smb", "signing", "client", "lanman", "security", "enforce"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Enforces signing on all SMB connections; breaks access to servers that do not support signing.",
                    ApplyOps = [RegOp.SetDword(WksKey, "RequireSecuritySignature", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "RequireSecuritySignature")],
                    DetectOps = [RegOp.CheckDword(WksKey, "RequireSecuritySignature", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-smb-encryption",
                    Label = "Enable SMB Client Encryption",
                    Category = "Network — Ipsec Rule",
                    Description = "Requests encrypted SMB connections wherever the server supports SMB 3.x encryption.",
                    Tags = ["smb", "encryption", "client", "lanman", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "End-to-end encryption for SMB traffic; requires SMB 3.x on both sides; silently ignored by older servers.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableSMBEncryption", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableSMBEncryption")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableSMBEncryption", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-smb1",
                    Label = "Disable SMBv1 Client Protocol",
                    Category = "Network — Ipsec Rule",
                    Description = "Disables the legacy SMBv1 dialect on the workstation client to prevent WannaCry-class exploits.",
                    Tags = ["smb", "smb1", "client", "lanman", "security", "legacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote = "Eliminates SMBv1 support; may break access to Windows XP/2003 or old NAS that only support SMBv1.",
                    ApplyOps = [RegOp.SetDword(WksKey, "DisableSMB1", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "DisableSMB1")],
                    DetectOps = [RegOp.CheckDword(WksKey, "DisableSMB1", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-require-ntlmv2",
                    Label = "Require NTLMv2 Authentication (Client)",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents the SMB workstation client from using the weak NTLMv1 authentication protocol.",
                    Tags = ["smb", "ntlm", "ntlmv1", "client", "authentication", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Forces NTLMv2 or Kerberos; may affect connections to very old servers that only support NTLMv1.",
                    ApplyOps = [RegOp.SetDword(WksKey, "RequireNTLMv2", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "RequireNTLMv2")],
                    DetectOps = [RegOp.CheckDword(WksKey, "RequireNTLMv2", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-enable-logon-audit",
                    Label = "Enable SMB Workstation Logon Audit",
                    Category = "Network — Ipsec Rule",
                    Description = "Records authentication events for SMB workstation connections in the Security event log.",
                    Tags = ["smb", "audit", "logon", "client", "lanman", "logging"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Adds logon audit entries to the event log; useful for detecting lateral movement attempts.",
                    ApplyOps = [RegOp.SetDword(WksKey, "EnableLogonAuditing", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "EnableLogonAuditing")],
                    DetectOps = [RegOp.CheckDword(WksKey, "EnableLogonAuditing", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-no-inplace-sharing",
                    Label = "Disable In-Place Sharing on Removable Media",
                    Category = "Network — Ipsec Rule",
                    Description = "Prevents in-place file sharing on removable storage media accessed through SMB workstation connections.",
                    Tags = ["smb", "removable", "sharing", "client", "lanman"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks in-place sharing from removable drives; users must copy files to a local path first.",
                    ApplyOps = [RegOp.SetDword(WksKey, "NoInplaceSharingOnRemovableMedia", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "NoInplaceSharingOnRemovableMedia")],
                    DetectOps = [RegOp.CheckDword(WksKey, "NoInplaceSharingOnRemovableMedia", 1)],
                },
                new TweakDef
                {
                    Id = "lanwks-disable-multicast-name-resolution",
                    Label = "Disable SMB Multicast Name Resolution",
                    Category = "Network — Ipsec Rule",
                    Description = "Stops the SMB client from using LLMNR and NetBIOS multicast name resolution, reducing lateral movement risk.",
                    Tags = ["smb", "llmnr", "netbios", "multicast", "client", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks LLMNR/NetBIOS name poisoning attacks; may slow discovery of shares without DNS entries.",
                    ApplyOps = [RegOp.SetDword(WksKey, "DisableMulticastNameResolution", 1)],
                    RemoveOps = [RegOp.DeleteValue(WksKey, "DisableMulticastNameResolution")],
                    DetectOps = [RegOp.CheckDword(WksKey, "DisableMulticastNameResolution", 1)],
                },
            ];
    }

    // ── LdapClientPolicy ──
    private static class _LdapClientPolicy
    {
        private const string LdapPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";

        private const string LdapSvcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ldapclnt-require-ldap-signing",
                    Label = "LDAP Client: Require LDAP Signing (Negotiate/Require)",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets LDAPClientIntegrity=2 in the LDAP policy hive (value 2 = Require signing; value 1 = Negotiate signing; value 0 = None). Requires LDAP clients to request LDAP packet signing on all LDAP connections to domain controllers. Without LDAP signing, the LDAP exchange is susceptible to LDAP relay attacks — an attacker who can perform a man-in-the-middle attack can modify LDAP query results without detection. LDAP signing is part of the security hardening recommended by Microsoft Security Advisory ADV190023 (LDAP channel binding and LDAP signing). Combined with LDAP channel binding, this closes a class of LDAP relay and NTLM relay attacks.",
                    Tags = ["ldap", "signing", "integrity", "adv190023", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP clients require signing for all DC connections. Legacy LDAP applications that use anonymous LDAP binds or simple (plaintext) binds without signing will fail. Audit LDAP usage with DC diagnostic logging (Set 16 LDAP Interface Events to 2) before enforcing. Older UNIX/Linux LDAP clients may need PAM/NSS configuration updates.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LDAPClientIntegrity")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LDAPClientIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-require-ldap-channel-binding",
                    Label = "LDAP Client: Require LDAP Channel Binding Token (CBT)",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets LdapEnforceChannelBinding=2 in the LDAP policy hive (value 2 = Always require channel binding; value 1 = Supported; value 0 = Never). LDAP Channel Binding Tokens (CBT) cryptographically bind an LDAP authentication exchange to the specific TLS channel it is using. This prevents LDAP relay attacks where an attacker intercepts an NTLM authentication for an LDAP-over-TLS connection and replays it over a different TLS connection (cross-channel relay). Combined with LDAP signing enforcement, this closes the NTLM relay to LDAP attack vector used by tools like Responder and ntlmrelayx.",
                    Tags = ["ldap", "channel-binding", "cbt", "ntlm-relay", "tls"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP channel binding enforced on all connections. Applications that use LDAP-over-TLS (LDAPS on port 636) with NTLM authentication must support channel binding tokens. Older LDAP client libraries (OpenLDAP < 2.5, Python-ldap without CBT patches) will fail LDAPS authentication. Survey LDAP client versions before enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapEnforceChannelBinding")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapEnforceChannelBinding", 2)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-set-ldap-client-timeout-120s",
                    Label = "LDAP Client: Set LDAP Search and Connection Timeout to 120 Seconds",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets LdapClientTimeout=120 in the LDAP policy hive (units: seconds). Sets the maximum number of seconds an LDAP client will wait for a search result before terminating the operation. Without a timeout, an LDAP client that connects to a slow or unresponsive domain controller can hold open connections indefinitely — an attacker who controls a fake DC can exploit this by responding very slowly to keep the LDAP connection open and drain client resources. Setting a bounded timeout ensures that LDAP operations fail gracefully when the DC is unresponsive, and the client can fall back to another DC.",
                    Tags = ["ldap", "timeout", "connection-limit", "dc-failover", "dos-mitigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP search and connection timeout is 120 seconds. Operations that require longer LDAP searches (large group enumeration, deep OU subtree searches) may time out in environments with extremely large directories. Monitor LDAP timeout events in application logs on clients running LDAP-intensive applications.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapClientTimeout", 120)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapClientTimeout")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapClientTimeout", 120)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-disable-ldap-anonymous-bind",
                    Label = "LDAP Client: Disable Unauthenticated (Anonymous) LDAP Bind",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets DisableAnonymousBind=1 in the LDAP policy hive. Prevents LDAP clients from performing anonymous (unauthenticated) LDAP binds to Active Directory domain controllers. Anonymous LDAP binds historically allowed any system on the network to query AD for directory information (user accounts, group memberships, computer accounts) without authenticating. While Windows Server 2003 and later restrict anonymous LDAP read access by default at the DC level, client-side enforcement ensures that applications in the environment never attempt anonymous LDAP binds — a pattern that could succeed against non-standard LDAP servers or legacy DCs with weakened configuration.",
                    Tags = ["ldap", "anonymous-bind", "authentication", "directory-enumeration"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Anonymous LDAP binds are blocked at the client level. Applications that use anonymous LDAP to query directory info without credentials will fail. Older monitoring tools and health check scripts that rely on anonymous LDAP must be updated to use service account credentials.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "DisableAnonymousBind")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "DisableAnonymousBind", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enforce-ldaps-port-636",
                    Label = "LDAP Client: Enforce LDAP over TLS (LDAPS) on Port 636",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets UseLdapSsl=1 in the LDAP policy hive. Enforces the use of LDAP over TLS (LDAPS on port 636) for LDAP connections. Standard LDAP on port 389 transmits all directory queries and responses, including credentials, in plaintext. An attacker performing network traffic capture on the corporate network can extract LDAP bind credentials, observe group memberships, and construct detailed maps of Active Directory structure. LDAPS encrypts the entire LDAP session using TLS. Combined with LDAP signing and channel binding, LDAPS provides end-to-end protection for directory communications.",
                    Tags = ["ldap", "ldaps", "tls", "port-636", "plaintext-prevention"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "LDAP connections use LDAPS (port 636) with TLS. Domain controllers must have valid LDAP server certificates installed. Certificate authority chain must be trusted by all LDAP clients. LDAP port 389 connections from this client will be rejected by LDAPS-only DCs. Ensure DCs have DC certificates from an internal PKI before enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "UseLdapSsl", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "UseLdapSsl")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "UseLdapSsl", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-set-max-ldap-connections-500",
                    Label = "LDAP Client: Cap Maximum Concurrent LDAP Connections to 500",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets MaxConnections=500 in the LDAP service key. Limits the number of concurrent LDAP connections the client interface will maintain to 500. Unbounded LDAP connections on an LDAP client can lead to memory exhaustion if an application has a connection leak or if a malicious application attempts to open large numbers of LDAP connections to degrade other directory services consumers on the same host. This setting provides a reasonable upper bound for normal enterprise usage while preventing connection floods.",
                    Tags = ["ldap", "connection-limit", "resource-bound", "dos-mitigation", "memory"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Maximum of 500 concurrent LDAP connections. In typical enterprise environments the actual number of concurrent LDAP connections is under 20. Applications that open many parallel LDAP connection contexts (directory synchronisation tools, identity management systems) should be tested to confirm they stay under 500.",
                    ApplyOps = [RegOp.SetDword(LdapSvcKey, "MaxConnections", 500)],
                    RemoveOps = [RegOp.DeleteValue(LdapSvcKey, "MaxConnections")],
                    DetectOps = [RegOp.CheckDword(LdapSvcKey, "MaxConnections", 500)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enable-referral-following-sasl",
                    Label = "LDAP Client: Require SASL Authentication When Following LDAP Referrals",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets FollowReferralsWithSasl=1 in the LDAP policy hive. Requires that when an LDAP client follows an LDAP referral (a response from one DC that redirects the client to query a different DC or domain), the subsequent connection to the referred server uses SASL (GSSAPI/Kerberos) authentication rather than simple bind. An attacker who can serve a crafted LDAP referral can attempt to redirect the client to a malicious LDAP server — if the client then connects using simple bind (password), the credentials can be captured. SASL with Kerberos prevents this: the referral target must prove its identity via Kerberos before credentials are presented.",
                    Tags = ["ldap", "referral", "sasl", "kerberos", "credential-capture"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "SASL required when following LDAP referrals. Applications that follow referrals using simple bind must switch to SASL/Kerberos for the referred connection. Modern .NET LDAP libraries and Windows LDAP APIs handle this transparently. Custom LDAP implementations may require code changes.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "FollowReferralsWithSasl")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "FollowReferralsWithSasl", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enable-ldap-diagnostic-logging",
                    Label = "LDAP Client: Enable LDAP Client Diagnostic Event Logging",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets LdapDiagnosticsEnabled=1 in the LDAP policy hive. Enables LDAP client diagnostic logging to the Windows Application event log. When enabled, LDAP authentication failures, TLS handshake errors, channel binding failures, and referral-following events are logged with details including the DC hostname, error code, and the identity attempting authentication. This logging is essential for detecting LDAP attacks (repeated anonymous bind attempts, LDAP relay attempts where channel binding fails) and for diagnosing LDAP signing/channel binding compatibility issues during enforcement rollout.",
                    Tags = ["ldap", "diagnostics", "logging", "event-log", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP diagnostic events are logged to the Application event log. Generates event log volume proportional to the number of LDAP operations. In environments with high LDAP query rates, review the log volume impact. Events appear under source 'LDAP Client'. Integrate with SIEM for attack detection use cases.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "LdapDiagnosticsEnabled")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "LdapDiagnosticsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-block-ntlm-ldap-fallback",
                    Label = "LDAP Client: Block NTLM Fallback in LDAP Authentication Negotiation",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets BlockNtlmLdapFallback=1 in the LDAP policy hive. Prevents the LDAP client from falling back to NTLM authentication when Kerberos authentication to a domain controller fails. An attacker who performs a downgrade attack (e.g., interfering with Kerberos SPN resolution) can force an LDAP client to use NTLM instead of Kerberos for authentication. NTLM is weaker and susceptible to relay attacks. Blocking NTLM fallback forces the client to fail visibly when Kerberos is unavailable rather than silently using the weaker protocol — making downgrade attacks immediately visible in logs.",
                    Tags = ["ldap", "ntlm-fallback", "kerberos", "downgrade", "relay"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "NTLM fallback for LDAP is blocked. When Kerberos authentication to a DC fails (e.g., SPN resolution failure, KDC unreachable), the LDAP operation fails rather than retrying with NTLM. This may cause authentication failures during DC failover events. Monitor LDAP authentication failures in event logs after enforcing.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "BlockNtlmLdapFallback")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "BlockNtlmLdapFallback", 1)],
                },
                new TweakDef
                {
                    Id = "ldapclnt-enforce-ldap-query-size-limit-1000",
                    Label = "LDAP Client: Enforce Maximum LDAP Query Result Size of 1000 Objects",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets MaxPageSize=1000 in the LDAP policy hive. Limits LDAP query results to a maximum of 1000 directory objects per page. Unbounded LDAP queries (queries without a size limit) can return tens of thousands of objects, consuming excessive DC memory and CPU, and enabling bulk directory enumeration by an attacker who has obtained LDAP query access. Setting a page size limit of 1000 ensures that applications must use paged results (LDAP paging control) to enumerate large sets of objects — and an attacker attempting to dump the entire directory in one query receives an error and must iterate, increasing the attack duration and detectability.",
                    Tags = ["ldap", "query-size", "paging", "enumeration", "dos-mitigation"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "LDAP query result size limited to 1000 objects per response. Applications that depend on unbounded LDAP queries (returning >1000 objects in one response) must be updated to use LDAP paged results control (LDAP_CONTROL_PAGEDRESULTS). Most modern LDAP libraries support paged results automatically.",
                    ApplyOps = [RegOp.SetDword(LdapPolicyKey, "MaxPageSize", 1000)],
                    RemoveOps = [RegOp.DeleteValue(LdapPolicyKey, "MaxPageSize")],
                    DetectOps = [RegOp.CheckDword(LdapPolicyKey, "MaxPageSize", 1000)],
                },
            ];
    }

    // ── LdapSigningPolicy ──
    private static class _LdapSigningPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\ldap";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LDAP";
        private const string DcKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NTDS\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "ldapsec-require-client-signing",
                    Label = "Require LDAP Client Signing for All Directory Connections",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Configures the LDAP client to always request LDAP signing (integrity protection), preventing man-in-the-middle attacks against LDAP sessions that could be used to modify query results or inject forged LDAP responses.",
                    Tags = ["ldap", "signing", "integrity", "mitm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP client signing required; LDAP MITM response injection attacks mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "LDAPClientIntegrity", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LDAPClientIntegrity")],
                    DetectOps = [RegOp.CheckDword(Key, "LDAPClientIntegrity", 2)],
                },
                new TweakDef
                {
                    Id = "ldapsec-require-channel-binding",
                    Label = "Require LDAP Channel Binding Tokens (CBT Hardening)",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Configures the LDAP client to include EPA Channel Binding Tokens in all LDAP over TLS sessions, preventing NTLM relay attacks that forward LDAP authentication to a different TLS channel.",
                    Tags = ["ldap", "channel-binding", "cbt", "ntlm-relay", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP channel binding required; NTLM relay attacks forwarding LDAP auth to different TLS channel blocked.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LdapClientChannelBinding", 2)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LdapClientChannelBinding")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LdapClientChannelBinding", 2)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-simple-bind",
                    Label = "Disable LDAP Simple Bind Authentication",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Prevents the use of LDAP Simple Bind authentication which sends credentials as plaintext Base64 without integrity protection. NTLM or Kerberos SASL must be used for all LDAP authentication.",
                    Tags = ["ldap", "simple-bind", "plaintext-auth", "sasl", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAP Simple Bind disabled; plaintext credential authentication to LDAP blocked. SASL required.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableSimpleBind", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableSimpleBind")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableSimpleBind", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-require-ldaps-port636",
                    Label = "Require LDAP over SSL/TLS (LDAPS, Port 636) for All AD Connections",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Configures the LDAP client to use LDAPS (LDAP over TLS on port 636) for all Active Directory connections, ensuring the entire LDAP session including SASL auth handshake is TLS-encrypted.",
                    Tags = ["ldap", "ldaps", "tls", "port-636", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "LDAPS required; all AD directory queries and authentications use TLS encryption on port 636.",
                    ApplyOps = [RegOp.SetDword(PolKey, "RequireLDAPS", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "RequireLDAPS")],
                    DetectOps = [RegOp.CheckDword(PolKey, "RequireLDAPS", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-set-max-query-result-size",
                    Label = "Set Maximum LDAP Query Result Set to 1000 Entries",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Limits LDAP query result sets to 1000 entries, preventing oversized LDAP result enumeration attacks that could be used to enumerate all AD objects in a single query exceeding normal LDAP paged result limits.",
                    Tags = ["ldap", "result-size", "enumeration-prevention", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LDAP result set limited to 1000 entries; full AD object enumeration in single query prevented.",
                    ApplyOps = [RegOp.SetDword(DcKey, "MaxPageSize", 1000)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "MaxPageSize")],
                    DetectOps = [RegOp.CheckDword(DcKey, "MaxPageSize", 1000)],
                },
                new TweakDef
                {
                    Id = "ldapsec-set-query-timeout-30s",
                    Label = "Set LDAP Query Timeout to 30 Seconds to Prevent Slow Queries",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Sets the LDAP client query timeout to 30 seconds, ensuring that slow/hanging LDAP queries do not block authentication processes and preventing DoS via crafted slow LDAP response attacks.",
                    Tags = ["ldap", "query-timeout", "dos-prevention", "authentication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LDAP query timeout set to 30 seconds; hanging LDAP queries do not block auth. Slow-response DoS mitigated.",
                    ApplyOps = [RegOp.SetDword(Key, "TimeLimit", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TimeLimit")],
                    DetectOps = [RegOp.CheckDword(Key, "TimeLimit", 30)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-ldap-null-base-queries",
                    Label = "Disable Unauthenticated LDAP Null-Base DNS Queries",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Prevents anonymous LDAP queries with a null base (empty search base DN) that enable unauthenticated discovery of AD domain information, domain naming context, and supported SASL mechanisms.",
                    Tags = ["ldap", "null-base", "anonymous", "enumeration", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LDAP anonymous null-base queries blocked; unauthenticated AD domain enumeration prevented.",
                    ApplyOps = [RegOp.SetDword(DcKey, "DisableNullBaseQuery", 1)],
                    RemoveOps = [RegOp.DeleteValue(DcKey, "DisableNullBaseQuery")],
                    DetectOps = [RegOp.CheckDword(DcKey, "DisableNullBaseQuery", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-log-signing-failures",
                    Label = "Log LDAP Signing and Channel Binding Failure Events",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Enables Security audit log entries for LDAP sessions that fail signing or channel binding requirements, providing visibility into tools and applications sending unsigned LDAP queries.",
                    Tags = ["ldap", "signing-failure", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LDAP signing/channel-binding failures logged; applications sending unsigned LDAP visible for remediation.",
                    ApplyOps = [RegOp.SetDword(PolKey, "LogSigningFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "LogSigningFailures")],
                    DetectOps = [RegOp.CheckDword(PolKey, "LogSigningFailures", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-disable-ldap-telemetry",
                    Label = "Disable LDAP Client Telemetry to Microsoft",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Prevents the Windows LDAP client from sending signing negotiation stats, connection failure rates, and cipher suite telemetry to Microsoft.",
                    Tags = ["ldap", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LDAP telemetry to Microsoft disabled; connection stats and cipher negotiation data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "ldapsec-enable-integrity-check-on-reconnect",
                    Label = "Re-Verify LDAP Integrity on Session Reconnection",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Forces the LDAP client to re-negotiate and verify signing integrity tokens when an LDAP session is reconnected after a network interruption, preventing session hijacking via injection into a reconnected unsigned LDAP stream.",
                    Tags = ["ldap", "reconnect", "integrity", "session-hijacking", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LDAP integrity re-verified on reconnect; injecting bytes into reconnected sessions blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "VerifyServerCertificate", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "VerifyServerCertificate")],
                    DetectOps = [RegOp.CheckDword(Key, "VerifyServerCertificate", 1)],
                },
            ];
    }

    // ── LegacyProtocols ──
    private static class _LegacyProtocols
    {
        private const string DnsClient = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\DNSClient";

        private const string DnsClientSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Dnscache\Parameters";

        private const string NetBtParams = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\NetBT\Parameters";

        private const string LltdSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\lltdsvc";

        private const string TeledoSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

        private const string IpHlpSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\iphlpsvc\Parameters\Teredo";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "legprot-disable-lltd",
                Label = "Disable LLTD (Link-Layer Topology Discovery)",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["lltd", "network discovery", "topology", "legacy"],
                Description =
                    "Disables the Link-Layer Topology Discovery (LLTD) service — used to build "
                    + "the Network Map in Windows Vista/7. Rarely needed on modern networks. "
                    + "Setting Start=4 disables the lltdsvc service.",
                ApplyOps = [RegOp.SetDword(LltdSvc, "Start", 4)],
                RemoveOps = [RegOp.SetDword(LltdSvc, "Start", 3)],
                DetectOps = [RegOp.CheckDword(LltdSvc, "Start", 4)],
            },
            new TweakDef
            {
                Id = "legprot-disable-6to4",
                Label = "Disable IPv6-to-IPv4 Transition (6to4)",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["6to4", "ipv6", "transition", "legacy protocol"],
                Description =
                    "Disables the 6to4 service — an IPv6 transition technology that wraps IPv6 "
                    + "packets in IPv4. Rarely used in modern networks and creates a potential "
                    + "covert channel. DisabledComponents+=2 in TCP/IP v6 parameters.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
                RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 3)],
                DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\6to4", "Start", 4)],
            },
            new TweakDef
            {
                Id = "legprot-disable-wins-client",
                Label = "Disable WINS Client Lookup",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wins", "netbios", "name resolution", "legacy"],
                Description =
                    "Disables WINS (Windows Internet Naming Service) client lookups. "
                    + "EnableWins=0 in NetBT parameters. WINS is a legacy NetBIOS name resolution "
                    + "service superseded by DNS. Disabling it on non-legacy environments "
                    + "reduces name-resolution attack surface.",
                ApplyOps = [RegOp.SetDword(NetBtParams, "EnableWins", 0)],
                RemoveOps = [RegOp.DeleteValue(NetBtParams, "EnableWins")],
                DetectOps = [RegOp.CheckDword(NetBtParams, "EnableWins", 0)],
            },
            new TweakDef
            {
                Id = "legprot-disable-llmnr-fallback",
                Label = "Disable LLMNR Name Resolution Fallback",
                Category = "Network — Ipsec Rule",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["llmnr", "dns fallback", "name resolution", "responder", "security"],
                Description =
                    "Prevents the DNS Client from falling back to LLMNR when DNS resolution fails. "
                    + "QueryAdapterName=0 in Dnscache parameters stops adapter-specific LLMNR "
                    + "queries, closing a secondary Responder attack vector beyond the primary "
                    + "legprot-disable-llmnr policy setting.",
                ApplyOps = [RegOp.SetDword(DnsClientSvc, "QueryAdapterName", 0)],
                RemoveOps = [RegOp.DeleteValue(DnsClientSvc, "QueryAdapterName")],
                DetectOps = [RegOp.CheckDword(DnsClientSvc, "QueryAdapterName", 0)],
            },
        ];
    }

    // ── LltdProtocolPolicy ──
    private static class _LltdProtocolPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\LLTD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io",
                    Label = "Disable LLTD I/O (Network Map Responder on Private Networks)",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD I/O component that allows this machine to respond to Link Layer Topology Discovery queries on private networks (home/work), preventing its network adapters from appearing in the Windows Network Map.",
                    Tags = ["lltd", "network-map", "topology-discovery", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on private networks disabled; machine removed from Windows Network Map on private networks.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableLLTDIO", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableLLTDIO")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableLLTDIO", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io-domain",
                    Label = "Disable LLTD I/O on Domain Networks",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD I/O component on domain-authenticated networks, preventing this machine from exposing its network topology to network discovery tools on corporate domain networks.",
                    Tags = ["lltd", "domain-network", "topology-discovery", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on domain networks disabled; machine does not respond to topology probes on domain networks.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnDomain", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnDomain")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnDomain", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-io-public",
                    Label = "Disable LLTD I/O on Public Networks",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD I/O component on public networks (airports, hotels, coffee shops), preventing network enumeration of this machine by other devices on untrusted public Wi-Fi networks.",
                    Tags = ["lltd", "public-network", "topology-discovery", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LLTD I/O on public networks disabled; machine not discoverable on hotel/airport/coffee-shop Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLLTDIOOnPublicNet", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLLTDIOOnPublicNet")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLLTDIOOnPublicNet", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr",
                    Label = "Disable LLTD Responder Component on Private Networks",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD Responder driver (rspndr) on private networks, preventing this machine from sending LLTD discovery responses that reveal its presence and IP/MAC mapping to network topology collectors.",
                    Tags = ["lltd", "responder", "network-discovery", "mac-address", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on private networks; MAC address and IP not revealed via discovery responses.",
                    ApplyOps = [RegOp.SetDword(Key, "EnableRspndr", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "EnableRspndr")],
                    DetectOps = [RegOp.CheckDword(Key, "EnableRspndr", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr-domain",
                    Label = "Disable LLTD Responder on Domain Networks",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD Responder driver on domain-authenticated networks, preventing topology discovery responses on corporate LANs where network mapping is managed exclusively by centralised network tools.",
                    Tags = ["lltd", "responder", "domain-network", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on domain networks; machine not visible in Windows Network Map on domain.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnDomain", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnDomain")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnDomain", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-rspndr-public",
                    Label = "Disable LLTD Responder on Public Networks",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD Responder driver on public networks, ensuring that this machine does not expose its presence, MAC address, or IP mapping to other hosts on untrusted public network segments.",
                    Tags = ["lltd", "responder", "public-network", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "LLTD Responder disabled on public networks; presence not exposed on untrusted public Wi-Fi.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowRspndrOnPublicNet", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowRspndrOnPublicNet")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowRspndrOnPublicNet", 0)],
                },
                new TweakDef
                {
                    Id = "lltdpol-log-lltd-probe-events",
                    Label = "Enable Logging of LLTD Discovery Probe Events",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Enables event log entries when LLTD discovery probes are received, providing audit trail of which hosts on the network are conducting topology discovery scans against this machine.",
                    Tags = ["lltd", "audit", "discovery-probe", "event-log", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD probe receipt events logged; topology scanning activity against this machine auditable.",
                    ApplyOps = [RegOp.SetDword(Key, "LogLLTDProbeEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogLLTDProbeEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogLLTDProbeEvents", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-managed-network-qos",
                    Label = "Disable LLTD Managed Network QoS Signalling",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables the LLTD managed network Quality of Service signalling extension, preventing this machine from participating in QoS scheduling signals broadcast over LLTD on Windows home network environments.",
                    Tags = ["lltd", "qos", "network-management", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD QoS signalling disabled; machine does not participate in LLTD-based bandwidth scheduling.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLLTDQoSSignaling", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDQoSSignaling")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLLTDQoSSignaling", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-block-lltd-service-admin-change",
                    Label = "Block Admin From Re-Enabling LLTD Without Policy Override",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Prevents local administrators from re-enabling LLTD I/O or Responder components without a Group Policy override, ensuring that the LLTD disable policy cannot be circumvented at the local machine level.",
                    Tags = ["lltd", "admin-lockdown", "gpo", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "LLTD cannot be re-enabled locally without GPO change; admin cannot override the detection disable.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockLocalLLTDOverride", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockLocalLLTDOverride")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockLocalLLTDOverride", 1)],
                },
                new TweakDef
                {
                    Id = "lltdpol-disable-lltd-multicast",
                    Label = "Disable LLTD Multicast Discovery on All Segments",
                    Category = "Network — Ipsec Rule",
                    Description =
                        "Disables LLTD multicast discovery messages sent across all network segments, preventing bandwidth consumption from periodic LLTD multicast discovery packets on busy enterprise networks.",
                    Tags = ["lltd", "multicast", "bandwidth", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "LLTD multicast discovery disabled; no periodic LLTD multicast traffic generated on any segment.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLLTDMulticast", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLLTDMulticast")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLLTDMulticast", 1)],
                },
            ];
    }

    // ── MapsBrowserPolicy ──
    private static class _MapsBrowserPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\MapsBrowser";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mapsbr-disable-auto-download",
                    Label = "Disable Automatic Offline Maps Download",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Windows from automatically downloading offline map data updates in the background. Reduces unnecessary network traffic on metered connections and removes a low-value background data transfer. Default: auto-download enabled. Recommended: 1.",
                    Tags = ["maps", "offline", "download", "background", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Offline map data is not downloaded automatically; maps remain available but may be outdated.",
                    ApplyOps = [RegOp.SetDword(Key, "AutoDownloadAndUpdateMapData", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AutoDownloadAndUpdateMapData")],
                    DetectOps = [RegOp.CheckDword(Key, "AutoDownloadAndUpdateMapData", 0)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-untriggered-network-traffic",
                    Label = "Disable Maps App Untriggered Network Traffic",
                    Category = "Network — Maps Browser",
                    Description =
                        "Stops the Maps application from initiating network requests that are not triggered by explicit user interaction (such as background tile prefetching or POI data sync). Reduces bandwidth consumption and privacy exposure. Default: background network traffic allowed. Recommended: 1.",
                    Tags = ["maps", "network", "background", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maps app stops all unsolicited background network requests; only user-initiated map loads use network.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUntriggeredNetworkTrafficOnSettingsPage", 0)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-location-for-maps",
                    Label = "Disable Location Access for Windows Maps",
                    Category = "Network — Maps Browser",
                    Description =
                        "Blocks the Windows Maps application from using the device's current location (GPS, Wi-Fi triangulation, IP geolocation) to centre the map or suggest nearby places. Prevents continuous location sampling by the app. Default: location allowed. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "location", "gps", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows Maps cannot access device location; map starts at a default location, not the user's current position.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLocationForMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLocationForMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLocationForMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-block-map-traffic-data",
                    Label = "Disable Real-Time Traffic Data in Maps",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Windows Maps from fetching real-time traffic data (congestion, incidents, road closures) from Microsoft's mapping service. Reduces background network calls and location telemetry inferences. Default: traffic data enabled. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "traffic", "realtime", "network", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Maps app does not show live traffic data; routes are calculated without congestion awareness.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTrafficData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTrafficData")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTrafficData", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-map-tile-storage",
                    Label = "Disable Offline Map Tile Storage",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Windows Maps from caching map tiles on local disk for offline use. Removes the map data footprint from managed devices where the maps feature is not used. Default: tiles cached locally. Recommended: 1 on space-constrained or managed endpoints where maps is unused.",
                    Tags = ["maps", "tile", "cache", "storage", "disk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Map tile cache is not maintained on disk; offline map access is unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableOfflineTileStorage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableOfflineTileStorage")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableOfflineTileStorage", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-bing-search-integration",
                    Label = "Disable Bing Search Integration in Maps",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Windows Maps from sending search queries to Bing when a user searches for a place, address, or business. Stops search terms from being transmitted to Microsoft's servers. Default: Bing integration enabled. Recommended: 1 on privacy-hardened endpoints.",
                    Tags = ["maps", "bing", "search", "privacy", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Map searches do not query Bing; only locally cached/offline map data is searched.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableBingSearchIntegration", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableBingSearchIntegration")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableBingSearchIntegration", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-route-sharing",
                    Label = "Disable Route/Directions Sharing from Maps",
                    Category = "Network — Maps Browser",
                    Description =
                        "Removes the 'Share' button and functionality from Windows Maps so users cannot share routes, locations, or directions via mail, SMS, or other apps. Prevents incidental location data leakage through sharing. Default: sharing enabled. Recommended: 1.",
                    Tags = ["maps", "sharing", "route", "privacy", "dlp", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Share functionality is removed from Maps; routes and places cannot be shared externally.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRouteSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRouteSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRouteSharing", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-personalized-maps",
                    Label = "Disable Personalised Map Suggestions",
                    Category = "Network — Maps Browser",
                    Description =
                        "Disables personalised place recommendations and 'frequent locations' features in Windows Maps that are based on past search history and route patterns. Prevents the accumulation of a location history profile. Default: personalisation enabled. Recommended: 1.",
                    Tags = ["maps", "personalisation", "history", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Maps does not build or use a personal history; no frequent-place suggestions or route preferences are stored.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePersonalisedMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePersonalisedMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePersonalisedMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-indoor-maps",
                    Label = "Disable Indoor Maps Feature",
                    Category = "Network — Maps Browser",
                    Description =
                        "Turns off the indoor floor-plan mapping feature in Windows Maps. Indoor maps require additional tile downloads and location data for floor-level positioning. On managed endpoints the feature is rarely needed and adds unnecessary resource usage. Default: indoor maps enabled. Recommended: 1.",
                    Tags = ["maps", "indoor", "floorplan", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Indoor floor-plan maps are disabled; building interior layouts are not shown.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableIndoorMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableIndoorMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableIndoorMaps", 1)],
                },
                new TweakDef
                {
                    Id = "mapsbr-disable-3d-maps",
                    Label = "Disable 3D Maps View (Birds Eye / Air View)",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Windows Maps from loading 3D aerial/birds-eye imagery tiles. 3D tiles are much larger than standard tiles and result in significant bandwidth consumption. On managed endpoints with limited bandwidth or no approved use of Maps, this reduces network overhead. Default: 3D imagery enabled. Recommended: 1.",
                    Tags = ["maps", "3d", "aerial", "network", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "3D birds-eye view tiles are not downloaded; Maps shows only flat 2D cartographic view.",
                    ApplyOps = [RegOp.SetDword(Key, "Disable3DMaps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Disable3DMaps")],
                    DetectOps = [RegOp.CheckDword(Key, "Disable3DMaps", 1)],
                },
            ];
    }

    // ── MobilityPolicy ──
    private static class _MobilityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Mobility";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "mob-disable-cellular-data-roaming",
                Label = "Mobility Policy: Disable Cellular Data Roaming",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents Windows from enabling cellular data roaming, which connects to and uses foreign carrier networks at potentially extreme per-MB charges. "
                    + "On enterprise-managed endpoints with cellular adapters, roaming data costs can accumulate without user awareness. "
                    + "Disabling via policy overrides any SIM-level or carrier profile allowing roaming. "
                    + "Removing this policy reverts cellular data roaming to device/SIM defaults.",
                Tags = ["mobility", "cellular", "roaming", "cost", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCellularDataRoaming", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCellularDataRoaming")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCellularDataRoaming", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks cellular roaming; prevents unexpected international data charges on managed endpoints.",
            },
            new TweakDef
            {
                Id = "mob-disable-mobile-hotspot",
                Label = "Mobility Policy: Disable Mobile Hotspot Sharing",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents the device from being configured as a mobile hotspot that shares its cellular or Wi-Fi connection with other devices. "
                    + "Mobile hotspot sharing bypasses network access controls and can expose the corporate network to unauthorised connected devices. "
                    + "Removing this policy allows mobile hotspot sharing to be configured.",
                Tags = ["mobility", "hotspot", "tethering", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMobileHotspot", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMobileHotspot")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMobileHotspot", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Blocks mobile hotspot; prevents unauthorised network sharing from managed endpoints.",
            },
            new TweakDef
            {
                Id = "mob-disable-usb-tethering",
                Label = "Mobility Policy: Disable USB Tethering",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents the device from being used as a USB tethering gateway, sharing its internet connection via USB to other devices. "
                    + "USB tethering creates a NAT bridge that can leak network traffic around firewall controls. "
                    + "Removing this policy allows USB tethering configuration.",
                Tags = ["mobility", "usb", "tethering", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUsbTethering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUsbTethering")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUsbTethering", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks USB tethering; prevents NAT bridge that could route around network firewalls.",
            },
            new TweakDef
            {
                Id = "mob-disable-automatic-connection-switch",
                Label = "Mobility Policy: Disable Auto WiFi-to-Cellular Switch",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents Windows from automatically switching the active network connection from Wi-Fi to cellular when Wi-Fi signal drops. "
                    + "Automatic switching can result in cellular data consumption and unexpected data charges on limited data plans. "
                    + "On enterprise machines the network handover should be manual or policy-driven. "
                    + "Removing this policy re-enables automatic Wi-Fi to cellular failover.",
                Tags = ["mobility", "wifi", "cellular", "auto-switch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutoConnectionSwitch", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoConnectionSwitch")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutoConnectionSwitch", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents auto Wi-Fi→cellular failover; avoids unintended cellular data consumption.",
            },
            new TweakDef
            {
                Id = "mob-disable-bluetooth-tethering",
                Label = "Mobility Policy: Disable Bluetooth Tethering",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents the device from sharing its internet connection via Bluetooth DUN (Dial-Up Networking) to devices paired over Bluetooth. "
                    + "Bluetooth tethering is a lower-visibility bridging path that can expose sensitive traffic without user awareness. "
                    + "Removing this policy allows Bluetooth tethering to be configured.",
                Tags = ["mobility", "bluetooth", "tethering", "network", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBluetoothTethering", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBluetoothTethering")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBluetoothTethering", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Blocks Bluetooth internet sharing; closes low-visibility network bridging path.",
            },
            new TweakDef
            {
                Id = "mob-disable-data-sense",
                Label = "Mobility Policy: Disable Data Sense Usage Monitoring",
                Category = "Network — Maps Browser",
                Description =
                    "Disables the Data Sense feature that monitors per-app cellular usage and restricts background data on metered connections. "
                    + "On managed endpoints, data usage enforcement should come from network policy rather than per-device Data Sense heuristics. "
                    + "Removing this policy re-enables Data Sense monitoring and throttling.",
                Tags = ["mobility", "data-sense", "metered", "monitoring", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDataSense", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDataSense")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDataSense", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Turns off Data Sense heuristics; network policy controls take over data management.",
            },
            new TweakDef
            {
                Id = "mob-disable-carrier-provisioning",
                Label = "Mobility Policy: Disable Carrier Provisioning Updates",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents mobile carriers from remotely pushing provisioning XML updates to the device that can change network settings, APN configurations, and restrictions. "
                    + "Carrier provisioning is an automated out-of-band configuration channel that can override IT-managed network settings. "
                    + "Removing this policy allows carriers to provision the device with their default network profiles.",
                Tags = ["mobility", "carrier", "provisioning", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCarrierProvisioning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCarrierProvisioning")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCarrierProvisioning", 1)],
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote =
                    "Blocks OTA carrier provisioning; prevents carriers from overriding IT network settings. May break initial cellular setup.",
            },
            new TweakDef
            {
                Id = "mob-disable-wifi-sense",
                Label = "Mobility Policy: Disable Wi-Fi Sense Auto-Connect",
                Category = "Network — Maps Browser",
                Description =
                    "Disables Wi-Fi Sense, which automatically connects to crowdsourced open Wi-Fi hotspots and can share Wi-Fi credentials with contacts. "
                    + "Wi-Fi Sense credential-sharing is a privacy and security risk on enterprise networks — credentials can be propagated to users' personal device contacts. "
                    + "Removing this policy re-enables Wi-Fi Sense auto-connect behaviour.",
                Tags = ["mobility", "wifi-sense", "credentials", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWifiSense", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWifiSense")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWifiSense", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables Wi-Fi Sense; prevents automatic hotspot connection and Wi-Fi credential sharing.",
            },
            new TweakDef
            {
                Id = "mob-disable-network-roaming-policy",
                Label = "Mobility Policy: Disable Network Roaming Profiles Sync",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents user roaming profiles from synchronising over cellular connections when roaming on a foreign network. "
                    + "Syncing large roaming profiles over cellular roaming can incur significant data charges and slow the logon process. "
                    + "Removing this policy allows roaming profile sync over any active connection.",
                Tags = ["mobility", "roaming-profile", "cellular", "logon", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRoamingProfileSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRoamingProfileSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRoamingProfileSync", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks profile sync over cellular roaming; avoids data charges during international travel.",
            },
            new TweakDef
            {
                Id = "mob-disable-wwan-ui",
                Label = "Mobility Policy: Disable WWAN Cellular UI Controls",
                Category = "Network — Maps Browser",
                Description =
                    "Hides the cellular (WWAN) control panel and settings UI from users, preventing manual changes to cellular configuration on managed endpoints. "
                    + "On enterprise endpoints where cellular settings are managed via MDM or IT policy, user-facing cellular UI is redundant and can lead to misconfiguration. "
                    + "Removing this policy restores the WWAN settings UI.",
                Tags = ["mobility", "wwan", "ui", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWwanUI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWwanUI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWwanUI", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Hides WWAN settings UI; prevents users from manually reconfiguring managed cellular connections.",
            },
        ];
    }

    // ── NearbySharingPolicy ──
    private static class _NearbySharingPolicy
    {
        private const string NearbyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NearbySharing";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nshpol-disable-nearby-sharing",
                    Label = "Disable Nearby Sharing (GPO)",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets DisableNearbySharing=1 to disable the Windows Nearby Sharing feature via Group Policy. Prevents file and URL transfers between nearby devices over Bluetooth and Wi-Fi Direct.",
                    Tags = ["nearby", "sharing", "bluetooth", "wifi-direct", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Removes Nearby Sharing from Action Center and context menus; local file moves unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "DisableNearbySharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisableNearbySharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "DisableNearbySharing", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-paired-devices",
                    Label = "Block Paired Device File Sharing",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowPairedDevices=0. Prevents this device from sharing files or URLs with paired Bluetooth devices through the Nearby Sharing subsystem, even when the feature itself is enabled.",
                    Tags = ["nearby", "paired", "bluetooth", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks file transfers to paired Bluetooth devices; standard Bluetooth audio unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowPairedDevices", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowPairedDevices")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowPairedDevices", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-message-sync",
                    Label = "Disable Phone Link Message Sync",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowMessageSync=0 in the Nearby Sharing policy. Prevents SMS and MMS messages from being synced from a paired Android device to this PC through the Phone Link (formerly Your Phone) application.",
                    Tags = ["nearby", "phone", "message", "sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks phone SMS sync in Phone Link; call notifications and other features may still work.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowMessageSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowMessageSync")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowMessageSync", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-contacts-sync",
                    Label = "Block Phone Link Contacts Sync",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowContactsSync=0. Prevents the Phone Link app from synchronising device contacts from a paired phone to the Windows People app or contact suggestions across the OS.",
                    Tags = ["nearby", "phone", "contacts", "sync", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks contact sync from paired phone; standalone Windows contacts unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowContactsSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowContactsSync")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowContactsSync", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-phone-link",
                    Label = "Disable Phone Link Feature (GPO)",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets DisablePhoneLinkFromSettings=1. Removes the Phone Link pairing option from the Windows Settings app, preventing users from linking a mobile phone to this PC through the connected-devices platform.",
                    Tags = ["nearby", "phone-link", "pairing", "policy", "settings"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Hides Phone Link from Settings; existing phone pairings may persist until removed manually.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "DisablePhoneLinkFromSettings")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "DisablePhoneLinkFromSettings", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-restrict-to-my-devices",
                    Label = "Restrict Nearby Sharing to My Devices Only",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets SharingScope=1 to restrict Nearby Sharing to devices signed in with the same Microsoft or Azure AD account. Prevents sharing with unknown nearby devices while preserving same-account file transfers.",
                    Tags = ["nearby", "scope", "trusted", "devices", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Limits sharing to own devices only; removes 'Everyone nearby' option from sharing scope.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "SharingScope", 1)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "SharingScope")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "SharingScope", 1)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-bluetooth-sharing",
                    Label = "Block Bluetooth File Sharing",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowBluetoothSharing=0 in the Nearby Sharing policy. Blocks the Bluetooth Object Push Profile (OPP) used by Nearby Sharing, preventing file reception from any Bluetooth source.",
                    Tags = ["nearby", "bluetooth", "opp", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Disables Bluetooth file receipt via OPP; Bluetooth audio and peripherals unaffected.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowBluetoothSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowBluetoothSharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowBluetoothSharing", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-wifi-direct-sharing",
                    Label = "Block Wi-Fi Direct Nearby Sharing",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowWifiDirectNearbySharing=0. Disables the Wi-Fi Direct transport layer used for Nearby Sharing. Prevents high-speed peer-to-peer file transfers that bypass the corporate network.",
                    Tags = ["nearby", "wifi-direct", "p2p", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Wi-Fi Direct used for large file transfers in Nearby Sharing.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowWifiDirectNearbySharing")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowWifiDirectNearbySharing", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-disable-activity-feed-sharing",
                    Label = "Disable Nearby Activity Feed Sharing",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowActivityFeed=0 in the Nearby Sharing policy scope. Prevents the connected-devices platform from broadcasting recently-used documents and activities to nearby enrolled devices via the activity feed.",
                    Tags = ["nearby", "activity", "feed", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Stops nearby activity broadcasting; Timeline/Activity History within this PC unchanged.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowActivityFeed", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowActivityFeed")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowActivityFeed", 0)],
                },
                new TweakDef
                {
                    Id = "nshpol-block-cross-device-clipboard",
                    Label = "Block Cross-Device Clipboard via Nearby Sharing",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets AllowCrossDeviceClipboard=0 in the Nearby Sharing policy. Prevents clipboard content from being sent or received between nearby Windows devices through the connected-devices platform, blocking near-field clipboard data exfiltration.",
                    Tags = ["nearby", "clipboard", "cross-device", "sharing", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks near-field clipboard transfers; cloud clipboard sync is a separate setting.",
                    ApplyOps = [RegOp.SetDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
                    RemoveOps = [RegOp.DeleteValue(NearbyKey, "AllowCrossDeviceClipboard")],
                    DetectOps = [RegOp.CheckDword(NearbyKey, "AllowCrossDeviceClipboard", 0)],
                },
            ];
    }

    // ── NetBiosPolicy ──
    private static class _NetBiosPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetBIOS";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netbios-disable-netbios-over-tcpip",
                Label = "Disable NetBIOS over TCP/IP on All Network Interfaces",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "NetBIOS over TCP/IP is a legacy name resolution protocol from the 1980s that is rarely required in modern DNS-based networks but provides significant attack surface for lateral movement. Disabling NetBIOS over TCP/IP eliminates LLMNR, NBT-NS, and WINS name resolution attacks that are used by tools like Responder to capture NTLM credentials. NetBIOS poisoning attacks intercept broadcast name resolution requests and respond with attacker-controlled responses causing systems to authenticate to attacker servers. Organizations that have fully migrated to DNS for name resolution have no functional need for NetBIOS and should disable it on all interfaces. Before disabling NetBIOS verify that no legacy applications require NetBIOS name resolution as this can break application connectivity in mixed environments. Disabling NetBIOS is a CIS benchmark recommendation that significantly hardens Windows systems against LLMNR/NBT-NS capture attacks.",
                Tags = ["netbios", "llmnr", "name-resolution", "ntlm-relay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNetBIOS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNetBIOS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNetBIOS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-llmnr-resolution",
                Label = "Disable Link-Local Multicast Name Resolution",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Link-Local Multicast Name Resolution (LLMNR) is a fallback name resolution protocol that broadcasts queries to the local network segment and can be poisoned by attackers. Disabling LLMNR prevents name resolution poisoning attacks where Responder and similar tools intercept LLMNR queries and respond with attacker-controlled IP addresses redirecting authentication traffic. LLMNR-based credential capture is one of the most common techniques used during internal network penetration testing due to its near-universal success in environments that have not disabled LLMNR. Disabling LLMNR has minimal impact on modern Windows environments that use DNS as the primary name resolution mechanism. Organizations should disable LLMNR across all systems in their domain as a standard hardening step that is low-risk and high-reward. LLMNR disabling is controlled through the EnableMulticast registry value under the DNSClient policy key rather than a dedicated LLMNR key.",
                Tags = ["netbios", "llmnr", "multicast", "credential-capture", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableLLMNR", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableLLMNR")],
                DetectOps = [RegOp.CheckDword(Key, "DisableLLMNR", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-wins-client",
                Label = "Disable WINS Client Name Resolution",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Windows Internet Name Service (WINS) client is a legacy NetBIOS name resolution service that maps NetBIOS names to IP addresses and predates DNS as the network name resolution standard. Disabling WINS client name resolution removes a legacy attack surface for name resolution spoofing and simplifies the network stack by removing unused legacy protocols. WINS infrastructure is rarely deployed in modern enterprise environments that have migrated to DNS for all name resolution requirements. Disabling WINS client prevents the system from querying WINS servers that may be attackers spoofing the WINS server to redirect name resolution. Organizations still running WINS for legacy application compatibility should have a migration plan to retire WINS and transition fully to DNS. Windows Server 2012 R2 was the last version to ship with WINS as an installable server role making now the time for WINS infrastructure retirement.",
                Tags = ["netbios", "wins", "name-resolution", "legacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWINSClient", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWINSClient")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWINSClient", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-netbios-name-broadcasts",
                Label = "Disable NetBIOS Name Broadcast Announcements",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "NetBIOS name broadcasts announce the system's NetBIOS name to the local network segment providing attackers with host discovery and naming information. Disabling NetBIOS name broadcasts reduces the information available to attackers performing network reconnaissance and eliminates broadcast-based credential capture opportunities. NetBIOS announcements also consume network bandwidth and CPU resources particularly on large flat networks with many systems broadcasting simultaneously. Modern Windows systems that use DNS-SD or other modern discovery protocols do not require NetBIOS broadcasts for network discovery functionality. Disabling broadcasts prevents tools like NetBIOS scanners from easily discovering systems and their NetBIOS names during reconnaissance. Organizations with large flat Networks should prioritize disabling NetBIOS broadcasts to reduce both security risk and unnecessary broadcast traffic.",
                Tags = ["netbios", "broadcasts", "reconnaissance", "network-discovery", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNameBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNameBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNameBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-nbt-ns-resolution",
                Label = "Disable NetBIOS over TCP/IP Name Service Queries",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "NetBIOS Name Service (NBT-NS) queries are broadcast UDP requests for name resolution that are intercepted by poisoning tools to capture NTLM credentials. Disabling NBT-NS query transmission prevents the system from sending NBT-NS queries that can be captured and responded to by attacker tools. NBT-NS poisoning is one of the most common attack techniques used in Active Directory environment compromises because it is reliable and does not require any vulnerability. Organizations that have disabled DNS fallback to NetBIOS can safely disable NBT-NS without impacting name resolution for modern applications. Firewall rules blocking UDP port 137 at the host level provide an additional layer of protection against NBT-NS exploitation. The combination of disabling LLMNR, NBT-NS, and WINS eliminates all multicast and broadcast name resolution attack vectors from the system.",
                Tags = ["netbios", "nbt-ns", "poisoning", "ntlm-relay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNBTNS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTNS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNBTNS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-computer-browser-service",
                Label = "Disable Computer Browser Service NetBIOS Dependency",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Computer Browser service maintains a list of computers and resources on the network using NetBIOS broadcasts and is rarely needed in modern AD environments. Disabling the Computer Browser service eliminates the NetBIOS dependency it creates and removes the master browser election process that generates unnecessary broadcast traffic. Browser service elections on large networks can cause periodic network storms as systems compete for master browser status. Modern Windows environments use Active Directory for computer discovery and organizational structure making the legacy Computer Browser service redundant. Disabling Computer Browser has no impact on Active Directory domain functionality including group policy, authentication, or shared resource access. The Computer Browser service should be disabled and set to Manual or Disabled startup to prevent automatic startup in future sessions.",
                Tags = ["netbios", "computer-browser", "broadcast", "legacy-service", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableComputerBrowser", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableComputerBrowser")],
                DetectOps = [RegOp.CheckDword(Key, "DisableComputerBrowser", 1)],
            },
            new TweakDef
            {
                Id = "netbios-restrict-smb-netbios-sharing",
                Label = "Restrict SMB NetBIOS File Sharing to Authenticated Users",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "SMB over NetBIOS (ports 139) is a legacy file sharing path that runs alongside the modern SMB direct connection (port 445) and represents additional attack surface. Restricting SMB over NetBIOS to authenticated users prevents anonymous and unauthenticated access attempts that probe legacy SMB services. Port 139 represents an older NetBIOS session service for SMB that is rarely needed in modern networks running purely SMB 2.0 or later. Organizations should configure Windows Firewall to block port 139 inbound traffic in addition to policy-based NetBIOS restrictions. Disabling NetBT in the network adapter settings removes the NetBIOS over TCP/IP stack entirely eliminating port 137, 138, and 139 from the system's network exposure. Legacy applications that require NetBIOS for file sharing should be migrated to use standard SMB over port 445.",
                Tags = ["netbios", "smb", "file-sharing", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictAnonymousNetBIOS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictAnonymousNetBIOS")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictAnonymousNetBIOS", 1)],
            },
            new TweakDef
            {
                Id = "netbios-audit-netbios-name-queries",
                Label = "Enable Audit Logging for NetBIOS Name Query Events",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NetBIOS name query audit logging captures all NetBIOS name lookup events providing visibility into legacy name resolution usage and potential poisoning activity. Enabling NetBIOS query audit logging helps identify systems that still rely on NetBIOS name resolution which should be migrated to DNS before NetBIOS is disabled. Audit data from NetBIOS queries can reveal hidden dependencies on NetBIOS in legacy applications that would break if NetBIOS were disabled without investigation. NetBIOS audit events combined with network monitoring help detect LLMNR and NBT-NS poisoning attacks in progress by correlating unexpected name resolution responses. Organizations should run NetBIOS query auditing for 30 days before disabling NetBIOS to identify all systems and applications that depend on it. Regular review of NetBIOS audit data after deploying other restrictions helps confirm that the restrictions are working and no bypass paths exist.",
                Tags = ["netbios", "audit", "monitoring", "name-resolution", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditNameQueries", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditNameQueries")],
                DetectOps = [RegOp.CheckDword(Key, "AuditNameQueries", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-netbt-registration",
                Label = "Disable NetBIOS Computer Name Registration on Network",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "NetBIOS computer name registration broadcasts the system hostname to all systems on the local network segment providing attacker-friendly network enumeration data. Disabling NetBT name registration prevents the system from advertising its hostname through NetBIOS reducing the information available for network reconnaissance. NetBIOS name registration on modern networks duplicates DNS registration and adds unnecessary broadcast traffic while providing attack surface. Systems that disable NetBIOS name registration will not be discoverable through NetBIOS enumeration tools but remain accessible through DNS-based discovery. Penetration tester tools like nbtscan rely on NetBIOS name registration to enumerate Windows systems making disabling registration a valuable hardening step. Organizations should combine disabling NetBIOS registration with DNS hostname privacy configurations for comprehensive network discovery hardening.",
                Tags = ["netbios", "name-registration", "reconnaissance", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableNBTRegistration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNBTRegistration")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNBTRegistration", 1)],
            },
            new TweakDef
            {
                Id = "netbios-disable-multicast-dns",
                Label = "Disable Multicast DNS to Prevent mDNS-Based Poisoning",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Multicast DNS (mDNS) is a zero-configuration networking protocol that resolves hostnames on local networks without a central DNS server and is exploitable for name poisoning similar to LLMNR. Disabling mDNS prevents mDNS-based credential capture attacks where tools like Responder implement mDNS poisoning to steal NTLM credentials. mDNS shares similar attack characteristics with LLMNR and NBT-NS and should be disabled alongside them for comprehensive broadcast name resolution hardening. Windows uses mDNS implemented through the DNS Client service and disabling it is controlled through policy rather than removing the service. mDNS is more commonly needed for Apple Bonjour-compatible devices and IoT devices than for standard Windows domain environments. Organizations should disable mDNS on domain-joined Windows systems while evaluating whether IoT or Apple devices on the same network segment require it.",
                Tags = ["netbios", "mdns", "multicast-dns", "poisoning", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMulticastDNS", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticastDNS")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMulticastDNS", 1)],
            },
        ];
    }

    // ── NetCfgPolicy ──
    private static class _NetCfgPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NCSI";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "netcfg-disable-ncsi-active-probe",
                Label = "Disable Network Connectivity Status Indicator Active Internet Probe",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "The Network Connectivity Status Indicator performs active probes to Microsoft servers to determine internet connectivity status which generates regular outbound traffic to external Microsoft infrastructure. Disabling NCSI active probes stops the regular HTTP and DNS queries that NCSI sends to connectivity check endpoints preventing this traffic pattern from consuming network resources. Organizations that use network behavior analytics tools should be aware that NCSI traffic represents a baseline of normal traffic that should be filtered from anomaly detection. The NCSI probe traffic can reveal the presence of Windows systems on a network to passive security monitoring tools even in environments where active scanning is prohibited. Private network environments and air-gapped networks may generate security alerts when NCSI fails to connect to Microsoft servers. Organizations should evaluate whether disabling NCSI probes affects any software that relies on the NCSI network status indication for connectivity-dependent behavior.",
                Tags = ["ncsi", "active-probe", "internet-connectivity", "network-privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoActiveProbe", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoActiveProbe")],
                DetectOps = [RegOp.CheckDword(Key, "NoActiveProbe", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-disable-ncsi-passive-polling",
                Label = "Disable NCSI Passive Network Polling for Network Status Detection",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "NCSI passive polling monitors network traffic to infer connectivity state without making active connections to connectivity check endpoints providing a less intrusive connectivity detection mechanism. Disabling passive polling stops NCSI from monitoring network traffic patterns to update its connectivity assessments between active probe cycles. Some network security tools flag the passive monitoring behavior of NCSI as anomalous traffic since it involves monitoring of network flows at the OS level. Organizations that conduct formal network security assessments should include NCSI behavior in their assessment scope to understand its traffic profile. Disabling both active probes and passive polling effectively turns off NCSI connectivity status reporting which may cause system tray indicators to show incorrect connectivity status. The impact of disabling NCSI should be evaluated for applications that use the Windows network connectivity notification API before applying this policy.",
                Tags = ["ncsi", "passive-polling", "network-monitoring", "connectivity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePassivePolling", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePassivePolling")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePassivePolling", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-configure-global-dns-suffix",
                Label = "Configure Global DNS Suffix for Network Name Resolution Policy",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Configuring the global DNS suffix search list through network configuration policy ensures consistent name resolution behavior across all domain members regardless of individual DNS client configuration. A controlled DNS suffix list prevents users from encountering DNS resolution issues caused by missing suffixes and ensures that organizational resource names are resolved through the correct DNS namespace. The global suffix configuration should list all organizational DNS domains in priority order to ensure efficient name resolution. DNS suffix policy should be aligned with the organizational network topology and should be updated when new DNS namespaces are added to the environment. Consistent DNS suffix configuration prevents split-brain DNS issues where resources are accessible by different names depending on client DNS configuration. Organizations should test DNS suffix changes in a test environment before deploying globally to prevent disruption to name resolution for critical services.",
                Tags = ["ncsi", "dns-suffix", "name-resolution", "network-config", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableGlobalDnsSuffixPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableGlobalDnsSuffixPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableGlobalDnsSuffixPolicy", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-restrict-network-location-awareness",
                Label = "Restrict Network Location Awareness Profile Assignment",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Network Location Awareness determines the network profile public private or domain assigned to each network connection which affects firewall rules and network sharing settings. Restricting network profile assignment prevents networks from being incorrectly classified as private or domain which could apply less restrictive firewall rules than appropriate for untrusted networks. Public networks receive more restrictive firewall configuration while domain and private networks receive more permissive configuration. Users operating on untrusted networks like hotel WiFi or coffee shop hotspots may be prompted to change the network profile which if set to private or domain exposes more services through the firewall. Policy-based network location awareness override ensures that specific networks are consistently classified regardless of user choices. Organizations should configure their corporate network identifiers to ensure that corporate networks are consistently classified as domain networks.",
                Tags = ["ncsi", "network-location-awareness", "firewall", "network-profiles", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNetworkLocationAwareness", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNetworkLocationAwareness")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNetworkLocationAwareness", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-disable-network-connectivity-popup",
                Label = "Disable Network Connectivity Status Popup Notifications",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Disabling network connectivity status popups suppresses the Windows notification dialogs that appear when network connectivity status changes such as connecting to a new network or losing internet access. Network connectivity popups in environments where users are not expected to make network configuration decisions can be disruptive to user workflows. In kiosk and restricted desktop environments the connectivity popups may expose information about the network infrastructure that should not be visible to users. Organizations should disable connectivity popups for environments where IT manages all network configuration and users should not be involved in network status decisions. The popup suppression does not prevent connectivity status from being reported through the system tray or through programmatic queries to the network connectivity API. Network administrators should still be notified of connectivity changes through monitoring tools even when user-facing popups are disabled.",
                Tags = ["ncsi", "popup-notifications", "network-status", "user-interface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableConnectivityPopup", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableConnectivityPopup")],
                DetectOps = [RegOp.CheckDword(Key, "DisableConnectivityPopup", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-block-captive-portal-redirect",
                Label = "Block Captive Portal Redirect Detection and Notification",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Blocking captive portal redirect detection prevents Windows from automatically presenting captive portal authentication prompts when connecting to networks that require web-based authentication login. Captive portal detection involves probing external URLs which reveals the presence of enrolled Windows devices to network operators and monitoring systems. In enterprise environments devices should not be connecting to networks that use captive portals and the prompt itself may confuse users or provide a social engineering attack vector. Disabling captive portal redirection prevents the NCSI from following HTTP redirects to captive portal authentication pages which could be used by malicious hotspot operators to redirect to phishing pages. Organizations that legitimately need guest WiFi access through captive portals for visitors should provide this access through separate devices rather than managed enterprise endpoints. Blocking captive portal redirect also prevents managed networks from being accidentally classified as having limited internet access.",
                Tags = ["ncsi", "captive-portal", "network-security", "hotspot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockCaptivePortalRedirect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockCaptivePortalRedirect")],
                DetectOps = [RegOp.CheckDword(Key, "BlockCaptivePortalRedirect", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-enable-interface-metric-policy",
                Label = "Enable Policy-Based Interface Metric Configuration",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Policy-based interface metric configuration allows administrators to define the routing preference of network interfaces through group policy ensuring consistent traffic routing behavior across all managed systems. Interface metrics determine the order in which network interfaces are used for routing with lower metric values preferred for outbound traffic. Controlled interface metrics ensure that traffic goes through the appropriate network interface including security appliances like network DLP and inspection systems when multiple network paths are available. Inconsistent interface metrics across the fleet can cause security traffic to bypass inspection systems when lower-priority interfaces are used instead of the primary managed interface. Organizations should define interface metrics that route traffic through all required security inspection points before reaching external destinations. Interface metric policy should be tested for each network topology including VPN-connected remote workers who have both VPN and direct internet interfaces.",
                Tags = ["ncsi", "interface-metrics", "routing", "network-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableInterfaceMetricPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableInterfaceMetricPolicy")],
                DetectOps = [RegOp.CheckDword(Key, "EnableInterfaceMetricPolicy", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-restrict-apipa-assignment",
                Label = "Restrict Automatic Private IP Address Assignment on Link Failure",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restricting Automatic Private IP Addressing prevents Windows from assigning a 169.254.x.x address when DHCP is unavailable which reduces the risk of systems accidentally joining link-local network segments that bypass normal network routing. APIPA addresses allow systems with failed DHCP to communicate with other APIPA-addressed systems on the same network segment which can create unmonitored peer-to-peer communication channels. In environments with strict network segmentation APIPA assignment can allow systems to communicate on segments they should not be able to reach if DHCP failure places unexpected systems on the same broadcast domain. Disabling APIPA ensures that systems without a valid DHCP lease lose network connectivity rather than falling back to an uncontrolled addressing scheme. Organizations should monitor for DHCP failures that would cause system unavailability when APIPA is disabled and ensure DHCP server high availability matches system uptime requirements. DHCP failure alerting should be configured to ensure that the absence of APIPA fallback does not result in extended system unavailability.",
                Tags = ["ncsi", "apipa", "dhcp", "network-addressing", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAPIPAAssignment", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAPIPAAssignment")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAPIPAAssignment", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-block-wi-fi-sense-auto-connect",
                Label = "Block Wi-Fi Sense Automatic Connection to Shared Networks",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Wi-Fi Sense automatic connection sharing exposes corporate network credentials to contact networks by sharing WiFi credentials through Microsoft account contact sharing which extends beyond organizational control. Enterprise devices should not have their stored WiFi credentials shared with Microsoft contact networks as this distributes corporate network access credentials to uncontrolled parties. Blocking Wi-Fi Sense prevents automatic connection to networks that contacts have shared which can include untrusted consumer WiFi networks that could expose device traffic to unauthorized parties. On corporate networks Wi-Fi Sense could allow external parties who receive shared network credentials to connect to corporate guest networks or networks that should be restricted to managed devices. Organizations should disable Wi-Fi Sense on all managed enterprise devices to prevent inadvertent credential sharing and unexpected connections to external networks. WiFi network credential management in enterprise environments should be handled through 802.1X certificate-based authentication rather than shared pre-shared key credentials.",
                Tags = ["ncsi", "wi-fi-sense", "credential-sharing", "wireless-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockWiFiSenseAutoConnect", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockWiFiSenseAutoConnect")],
                DetectOps = [RegOp.CheckDword(Key, "BlockWiFiSenseAutoConnect", 1)],
            },
            new TweakDef
            {
                Id = "netcfg-enforce-secure-dns-configuration",
                Label = "Enforce Secure DNS Configuration for Network Name Resolution",
                Category = "Network — Maps Browser",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing secure DNS configuration ensures that DNS queries are sent only to organizationally managed DNS servers that provide DNS security extensions and protective filtering. Bypassing organizational DNS servers is a common technique for circumventing DNS-based security controls including malware domain blocking threat intelligence feeds and data loss prevention. Secure DNS configuration policy locks the DNS server configuration to approved organizational servers preventing users and applications from switching to alternative DNS providers. Organizations that have deployed DNS security features like RPZ zones threat feeds and DoH-based filtering rely on all clients using the configured DNS servers. The policy should be combined with network firewall rules that block DNS queries to non-approved DNS servers providing defense-in-depth against DNS bypass attempts. Regular auditing of DNS query patterns for queries to non-organizational DNS servers helps detect attempts to bypass DNS security controls.",
                Tags = ["ncsi", "dns-security", "dns-servers", "name-resolution", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureDnsConfiguration", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureDnsConfiguration")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureDnsConfiguration", 1)],
            },
        ];
    }

    // ── NetIoOffloadPolicy ──
    private static class _NetIoOffloadPolicy
    {
        private const string TcpKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters";

        private const string TcpifKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip6\Parameters";

        private const string AfDKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AFD\Parameters";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "netio-set-tcp-checksum-hardware",
                    Label = "Net IO: Enable TCP/IP Hardware Checksum Offload",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets EnableTCPChimneyOffload=1 and ChecksumOffloadEnabled=1 in TCP/IP settings. Configures the TCP/IP stack to delegate TCP and IP header checksum computation to the network adapter hardware (TCP Offload Engine). Hardware checksum computation removes the CPU overhead of per-packet checksum calculations from the host CPU. On servers handling 10 Gbps+ traffic or high-PPS packet flows, hardware checksum offload can reduce CPU utilization by 5–15% for network processing.",
                    Tags = ["tcp", "checksum", "offload", "hardware", "nic"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Offloads TCP checksum to NIC hardware. Requires checksum offload-capable NIC (all modern enterprise NICs support this). Some virtualization hypervisors may intercept and verify checksums.",
                    ApplyOps = [RegOp.SetDword(TcpKey, "EnableTCPChimneyOffload", 1)],
                    RemoveOps = [RegOp.DeleteValue(TcpKey, "EnableTCPChimneyOffload")],
                    DetectOps = [RegOp.CheckDword(TcpKey, "EnableTCPChimneyOffload", 1)],
                },
                new TweakDef
                {
                    Id = "netio-set-tcp-autotuning-high",
                    Label = "Net IO: Set TCP Window Autotuning to Highly Restricted Mode",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets TcpAutoTuningLevel=4 (highly restricted) in TCP/IP parameters. Sets TCP receive window auto-tuning to a conservative algorithm that grows the receive buffer more cautiously than the default. The highly restricted mode is appropriate for environments with high-speed last-mile but intermediate links with lossy behavior (satellite links, 4G LTE backhaul) where aggressive window growth causes sporadic retransmit storms. On reliable Ethernet networks, 'normal' (0) autotuning provides higher throughput.",
                    Tags = ["tcp", "autotuning", "window", "buffer", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "Conservative TCP window scaling may reduce throughput on high-bandwidth low-latency links. Only recommended for networks with frequent packet loss (satellite, LTE backhaul).",
                    ApplyOps = [RegOp.SetDword(TcpKey, "TcpAutoTuningLevel", 4)],
                    RemoveOps = [RegOp.DeleteValue(TcpKey, "TcpAutoTuningLevel")],
                    DetectOps = [RegOp.CheckDword(TcpKey, "TcpAutoTuningLevel", 4)],
                },
                new TweakDef
                {
                    Id = "netio-enable-rss",
                    Label = "Net IO: Enable Receive-Side Scaling (RSS) for Multi-CPU Load",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets EnableRSS=1 and MaxRssProcessors=4 in network adapter policy. Enables Receive-Side Scaling (RSS) which distributes incoming network packet processing across multiple CPU cores. Without RSS, all incoming traffic for a given NIC is processed on a single CPU core, creating a per-core throughput bottleneck at approximately 3–5 Gbps on modern hardware. With RSS, incoming packets are hashed to multiple CPU queues, scaling receive throughput linearly with CPU cores up to the NIC's hardware RSS queue limit.",
                    Tags = ["rss", "networking", "cpu", "performance", "offload"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Requires RSS-capable NIC (all server-class NICs support RSS). RSS distributes interrupts across CPUs which may change CPU affinity behavior of network-intensive processes.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS", 1)],
                    RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS")],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\QoS", "EnableRSS", 1)],
                },
                new TweakDef
                {
                    Id = "netio-set-afd-fast-send-datagram",
                    Label = "Net IO: Enable AFD Fast Send Datagram Path",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets FastSendDatagramThreshold=1024 in AFD parameters. Configures the Windows Ancillary Function Driver (AFD) to use the fast datagram send path for UDP packets under 1024 bytes. The fast path bypasses several AFD buffer validation steps for trusted-size datagrams, reducing per-packet CPU cost for high-PPS UDP workloads. This benefits applications generating large volumes of small UDP packets: DNS servers processing thousands of queries per second, game servers, or network telemetry agents.",
                    Tags = ["afd", "udp", "datagram", "fast-path", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Fast path bypasses some buffer validation for small UDP datagrams. Testing recommended for high-PPS DNS server workloads before production deployment.",
                    ApplyOps = [RegOp.SetDword(AfDKey, "FastSendDatagramThreshold", 1024)],
                    RemoveOps = [RegOp.DeleteValue(AfDKey, "FastSendDatagramThreshold")],
                    DetectOps = [RegOp.CheckDword(AfDKey, "FastSendDatagramThreshold", 1024)],
                },
                new TweakDef
                {
                    Id = "netio-disable-ipv6-source-routing",
                    Label = "Net IO: Disable IPv6 Source Routing (Anti-Spoofing)",
                    Category = "Network — Maps Browser",
                    Description =
                        "Sets DisableIPv6SourceRouting=1 in TCPv6 parameters. Drops IPv6 packets containing Type 0 Routing Header (RH0) extension headers. IPv6 RH0 was used in major amplified DoS attacks (CVE-2007-2242) where a small packet can be amplified to an enormous amount of traffic by specifying 127 intermediate hops in the routing header. RFC 5095 deprecated RH0; all modern networks should drop RH0-containing packets. This setting enforces RFC 5095 at the host‐stack level.",
                    Tags = ["ipv6", "source-routing", "rh0", "dos", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "No legitimate applications use IPv6 RH0 since RFC 5095 deprecation in 2007. This setting has no impact on normal network operations.",
                    ApplyOps = [RegOp.SetDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
                    RemoveOps = [RegOp.DeleteValue(TcpifKey, "DisableIPv6SourceRouting")],
                    DetectOps = [RegOp.CheckDword(TcpifKey, "DisableIPv6SourceRouting", 1)],
                },
            ];
    }

    // ── NetLocationAwarenessAdvancedPolicy ──
    private static class _NetLocationAwarenessAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList\Signatures";
        private const string NlmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkList";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nlaadv-always-classify-as-public",
                    Label = "Always Classify Unrecognised Networks as Public",
                    Category = "Network — Maps Browser",
                    Description =
                        "Forces Windows to classify all new or unrecognised network connections as Public network profile (most restrictive firewall rules) until explicitly reclassified by an administrator, applying maximum firewall protection to unknown networks.",
                    Tags = ["nla", "network-classification", "public-profile", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Unknown networks classified as Public; most restrictive firewall rules apply to all unrecognised connections.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DefaultClassification", 0)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DefaultClassification")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DefaultClassification", 0)],
                },
                new TweakDef
                {
                    Id = "nlaadv-block-user-profile-reclassification",
                    Label = "Block Standard Users from Reclassifying Network Profiles",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents standard users from changing a network's classification (Private/Public/Domain Work) in Windows, ensuring that firewall profile assignments can only be modified by administrators.",
                    Tags = ["nla", "network-profile", "reclassification", "standard-user", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Network profile reclassification blocked for standard users; firewall profile changes require admin.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "AllowUserSetNetworkLocation")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "AllowUserSetNetworkLocation", 0)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-domain-network-upgrade",
                    Label = "Disable Automatic Domain Network Upgrade from NLA",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents NLA from automatically reclassifying a network from Public/Private to Domain Work profile when domain controllers are reachable, keeping explicit admin-assigned firewall profiles even on domain member machines.",
                    Tags = ["nla", "domain-profile", "auto-detect", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NLA domain auto-upgrade disabled; domain networks stay at assigned profile without auto-promotion.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableDomainNetworkAutoDetect")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableDomainNetworkAutoDetect", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-log-profile-change-events",
                    Label = "Log Network Profile Change Events",
                    Category = "Network — Maps Browser",
                    Description =
                        "Enables System event log entries when a network connection profile is changed (Private to Public, etc.), providing audit visibility into firewall profile transitions that could weaken security posture.",
                    Tags = ["nla", "network-profile", "event-log", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Network profile changes logged; firewall profile transitions recorded in System event log.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "LogNetworkProfileChanges", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "LogNetworkProfileChanges")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "LogNetworkProfileChanges", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-nla-internet-probe",
                    Label = "Disable NLA Internet Connectivity Probe (NCSI Bypass)",
                    Category = "Network — Maps Browser",
                    Description =
                        "Disables the Network Connectivity Status Indicator (NCSI) probe that NLA sends to Microsoft servers (connectivity.microsoft.com) to determine internet connectivity status, preventing outbound probe traffic to cloud hosts.",
                    Tags = ["nla", "ncsi", "connectivity-probe", "microsoft", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NCSI internet probe disabled; no connectivity.microsoft.com probe. System tray may show 'No internet' falsely.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableNCSI", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNCSI")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableNCSI", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-captive-portal-detect",
                    Label = "Disable Captive Portal Detection",
                    Category = "Network — Maps Browser",
                    Description =
                        "Disables Windows captive portal detection that redirects a browser to a hotel/airport landing page, preventing unwanted browser launches in locked-down environments and avoiding false-positive network change alerts.",
                    Tags = ["nla", "captive-portal", "hotspot", "browser", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Captive portal detection disabled; Windows does not auto-open browser when hotspot login required.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableCaptivePortalDetection", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableCaptivePortalDetection")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableCaptivePortalDetection", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-require-auth-for-private-upgrade",
                    Label = "Require User Authentication Before Private Network Upgrade",
                    Category = "Network — Maps Browser",
                    Description =
                        "Requires an administrator confirmation before NLA upgrades a network from Public to Private profile, preventing accidental loosening of firewall rules when a device connects to an unknown but trusted-seeming network.",
                    Tags = ["nla", "private-profile", "authentication", "firewall", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Admin confirmation required before Private profile upgrade; prevents accidental firewall relaxation.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "RequireAuthForPrivateNetworkUpgrade")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "RequireAuthForPrivateNetworkUpgrade", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-block-network-name-ui",
                    Label = "Hide Network Name and Location in System Tray",
                    Category = "Network — Maps Browser",
                    Description =
                        "Removes the network name and location type from the system tray network flyout, preventing casual users from seeing and potentially modifying network profile names or locations.",
                    Tags = ["nla", "system-tray", "network-name", "ui", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network name hidden in system tray; network profile names and types not shown in flyout.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "HideNetworkLocationUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "HideNetworkLocationUI")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "HideNetworkLocationUI", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-disable-network-telemetry",
                    Label = "Disable NLA Network Profile Telemetry to Microsoft",
                    Category = "Network — Maps Browser",
                    Description =
                        "Prevents Network Location Awareness from sending network profile assignment and classification telemetry to Microsoft, protecting information about this machine's network environment from cloud disclosure.",
                    Tags = ["nla", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "NLA telemetry to Microsoft disabled; network profile assignment and connectivity data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "DisableNLATelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "DisableNLATelemetry")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "DisableNLATelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "nlaadv-set-profile-icon-classification",
                    Label = "Set Network Profile Icon to Reflect Classification",
                    Category = "Network — Maps Browser",
                    Description =
                        "Configures the network profile icon in the system tray to visually reflect the current classification (Public/Private/Domain) to ensure users have immediate visual awareness of the active firewall profile strength.",
                    Tags = ["nla", "network-icon", "ui", "firewall-profile", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Network icon reflects profile classification; users see current Public/Private/Domain firewall level.",
                    ApplyOps = [RegOp.SetDword(NlmKey, "ShowProfileClassificationIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(NlmKey, "ShowProfileClassificationIcon")],
                    DetectOps = [RegOp.CheckDword(NlmKey, "ShowProfileClassificationIcon", 1)],
                },
            ];
    }

    // ── NetworkAccessProtectionPolicy ──
    private static class _NetworkAccessProtectionPolicy
    {
        private const string NapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetworkAccessProtection\MSNAPAgent";
        private const string QuarantineKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\NetworkAccessProtection\Quarantine";
        private const string NapAgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\NAPAgent";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "napcomp-enable-nap-client-enforcement",
                Label = "NAP Policy: Enable NAP Client Enforcement Mode",
                Category = "Network — Maps Browser",
                Description =
                    "Enables the Network Access Protection (NAP) client on the machine, allowing the machine to participate in NAP health validation and enforcement workflows. NAP is a policy-based network access control framework that evaluates the health state of a machine (antivirus, patch level, firewall status) before granting full network access. Enabling enforcement mode ensures that a machine reporting an unhealthy health state is placed in a restricted network segment until it is remediated.",
                Tags = ["nap", "network access protection", "compliance", "health validation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnableNAPClient", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableNAPClient")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableNAPClient", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Enables NAP health enforcement; machines that fail health checks may lose full network access — ensure NHPs and remediation servers are configured.",
            },
            new TweakDef
            {
                Id = "napcomp-require-health-validation",
                Label = "NAP Policy: Require Health Certificate for Network Access",
                Category = "Network — Maps Browser",
                Description =
                    "Configures the NAP client to require a valid System Health Certificate (SHC) before being granted unrestricted network access. Without a health certificate, the machine is placed in the quarantine network. Health certificates are issued by the Health Registration Authority (HRA) after the NPS Health Policy Server verifies that all system health validators (SHVs) report a compliant state. This policy forms the core of the 802.1X-based NAP enforcement chain.",
                Tags = ["nap", "health certificate", "802.1x", "hps", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "RequireHealthCertificate", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "RequireHealthCertificate")],
                DetectOps = [RegOp.CheckDword(NapKey, "RequireHealthCertificate", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Requires health certificate for full network access; misconfigured NAP infrastructure will quarantine all machines including healthy ones.",
            },
            new TweakDef
            {
                Id = "napcomp-enforce-vpn-shv",
                Label = "NAP Policy: Enable System Health Validation for VPN Connections",
                Category = "Network — Maps Browser",
                Description =
                    "Activates System Health Validator (SHV) evaluation for VPN-based NAP enforcement, ensuring that remote machines connecting via VPN are subject to the same health compliance checks as internally-connected machines. Without VPN-SHV enforcement, remote workers can connect to the corporate network with out-of-date antivirus signatures or missing security patches while bypassing the health gating that on-premises machines face.",
                Tags = ["nap", "vpn", "system health validator", "remote access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnableVPNSHV", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnableVPNSHV")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnableVPNSHV", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote =
                    "Extends NAP health checks to VPN connections; requires a configured NAP Network Policy Server and VPN server with NAP support.",
            },
            new TweakDef
            {
                Id = "napcomp-disable-auto-remediation",
                Label = "NAP Policy: Disable Automatic Health Remediation",
                Category = "Network — Maps Browser",
                Description =
                    "Prevents the NAP client from automatically remediating detected health deficiencies by making software changes (e.g., enabling Windows Firewall, triggering Windows Update). Automatic remediation can make unexpected changes to a system without user awareness and may conflict with endpoint management tools (such as Intune or SCCM) that manage those settings centrally. Disabling auto-remediation forces the user or help desk to perform explicit remediation steps.",
                Tags = ["nap", "auto remediation", "endpoint management", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "DisableAutoRemediation", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "DisableAutoRemediation")],
                DetectOps = [RegOp.CheckDword(NapKey, "DisableAutoRemediation", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Disables auto-remediation; unhealthy machines stay quarantined until manual intervention — ensure help desk procedures are documented.",
            },
            new TweakDef
            {
                Id = "napcomp-enforce-quarantine-timeout",
                Label = "NAP Policy: Set Quarantine Timeout Period for Unhealthy Machines",
                Category = "Network — Maps Browser",
                Description =
                    "Configures how long (in minutes) a machine can remain in the quarantine network before its health is re-evaluated and upgraded to full access or flagged for intervention. A timeout of 480 minutes (8 hours) reflects a typical business-day remediation window. Without a timeout, a machine placed in quarantine due to a transient health failure stays restricted indefinitely unless an administrator intervenes or the NAP client is restarted.",
                Tags = ["nap", "quarantine", "timeout", "health policy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [QuarantineKey],
                ApplyOps = [RegOp.SetDword(QuarantineKey, "QuarantineTimeoutMinutes", 480)],
                RemoveOps = [RegOp.DeleteValue(QuarantineKey, "QuarantineTimeoutMinutes")],
                DetectOps = [RegOp.CheckDword(QuarantineKey, "QuarantineTimeoutMinutes", 480)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Sets quarantine timeout to 8 hours; machines that remediate earlier will get re-evaluated when they reconnect.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-quarantine-state-pki",
                Label = "NAP Policy: Enable PKI-Based Quarantine State Machine",
                Category = "Network — Maps Browser",
                Description =
                    "Activates the PKI-based state machine within the NAP client that uses X.509 certificates to encode the machine's health attestation state. The PKI state machine is required for the IPSEC enforcement method, which uses health certificates to gate machine-to-machine communication at the network layer. Without this, only 802.1X or DHCP-based NAP enforcement methods are available, which are less granular than IPSEC health-gated policies.",
                Tags = ["nap", "pki", "ipsec", "health attestation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapKey],
                ApplyOps = [RegOp.SetDword(NapKey, "EnablePKIStateMachine", 1)],
                RemoveOps = [RegOp.DeleteValue(NapKey, "EnablePKIStateMachine")],
                DetectOps = [RegOp.CheckDword(NapKey, "EnablePKIStateMachine", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Activates PKI health-state machine; requires a Health Registration Authority (HRA) and certificate infrastructure.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-dhcp-enforcement",
                Label = "NAP Policy: Enable DHCP-Based NAP Enforcement",
                Category = "Network — Maps Browser",
                Description =
                    "Enables the DHCP enforcement client for NAP, allowing the machine to participate in DHCP quarantine workflows. In DHCP enforcement mode, the DHCP server issues different IP address leases (quarantine vs. full-access scope) based on the client's health certificate. This is the simplest NAP enforcement method to deploy and requires no changes to network switches, making it suitable for organisations with legacy switching infrastructure.",
                Tags = ["nap", "dhcp enforcement", "quarantine", "network access", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableDHCPEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableDHCPEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableDHCPEnforcement", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Enables DHCP-based NAP enforcement; requires NAP-capable DHCP server and NPS health policy configuration.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-wired-8021x-enforcement",
                Label = "NAP Policy: Enable Wired 802.1X NAP Enforcement",
                Category = "Network — Maps Browser",
                Description =
                    "Activates the 802.1X enforcement client for wired network connections, enabling switch-level NAP quarantine for machines that fail health evaluations. 802.1X enforcement is the strongest NAP mechanism because it operates at the switch port level — a machine placed in quarantine cannot communicate on the network at all without passing through the enforcement switch, regardless of IP configuration. This method requires 802.1X-capable managed switches.",
                Tags = ["nap", "802.1x", "wired enforcement", "switch", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableWired8021xEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableWired8021xEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableWired8021xEnforcement", 1)],
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote =
                    "Activates 802.1X NAP; requires managed 802.1X switches and NPS RADIUS configuration — test in lab before broad deployment.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-ts-gateway-enforcement",
                Label = "NAP Policy: Enable Terminal Services Gateway NAP Enforcement",
                Category = "Network — Maps Browser",
                Description =
                    "Enables the Terminal Services (Remote Desktop) Gateway NAP enforcement client. When active, the TS Gateway evaluates the client machine's NAP health certificate before establishing an RDP tunnel, ensuring that remote desktop sessions from unhealthy endpoints are blocked at the gateway. This is particularly important for privileged access workstations (PAWs) connecting to administrative systems — an infected admin workstation should not be allowed to initiate RDP sessions.",
                Tags = ["nap", "terminal services", "remote desktop", "gateway", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableTSGatewayEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableTSGatewayEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableTSGatewayEnforcement", 1)],
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Gates RDP gateway access on NAP health; requires a NAP-enabled TS Gateway and NPS policy configuration.",
            },
            new TweakDef
            {
                Id = "napcomp-enable-ipsec-enforcement",
                Label = "NAP Policy: Enable IPsec-Based NAP Enforcement",
                Category = "Network — Maps Browser",
                Description =
                    "Activates the IPsec enforcement client for NAP, which uses short-lived health certificates to enforce IPsec policies between machines on the same network. IPsec NAP enforcement ensures that only machines with current, valid health certificates can communicate over authenticated IPsec channels. This is the most granular NAP enforcement method and enables zero-trust-style east-west traffic control within a corporate network segment.",
                Tags = ["nap", "ipsec", "zero trust", "east-west", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [NapAgentKey],
                ApplyOps = [RegOp.SetDword(NapAgentKey, "EnableIPsecEnforcement", 1)],
                RemoveOps = [RegOp.DeleteValue(NapAgentKey, "EnableIPsecEnforcement")],
                DetectOps = [RegOp.CheckDword(NapAgentKey, "EnableIPsecEnforcement", 1)],
                ImpactScore = 5,
                SafetyRating = 2,
                ImpactNote =
                    "Activates IPsec NAP; requires HRA, NPS, and IPsec policy infrastructure — extremely disruptive if misconfigured. Production testing mandatory.",
            },
        ];
    }

    // ── NetworkAccessProtPolicy ──
    private static class _NetworkAccessProtPolicy
    {
        private const string NapKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NetworkAccessProtection";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "nappol-disable-nap-client",
                    Label = "Disable NAP Client Service",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets Enabled=0 to disable the Network Access Protection client service. NAP was deprecated in Windows 10 but the client components remain; disabling prevents unnecessary service overhead.",
                    Tags = ["nap", "network", "policy", "service"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "NAP client disabled; no impact on modern networks that do not use NAP infrastructure.",
                    ApplyOps = [RegOp.SetDword(NapKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(NapKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-dhcp-quarantine",
                    Label = "Disable NAP DHCP Quarantine Enforcement",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets EnableDhcpQuarantine=0 to disable NAP enforcement through DHCP. Prevents the client from being quarantined to a restricted network when DHCP health checks fail.",
                    Tags = ["nap", "dhcp", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "DHCP NAP quarantine disabled; client bypasses network health checks via DHCP.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableDhcpQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableDhcpQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableDhcpQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-8021x-quarantine",
                    Label = "Disable NAP 802.1X Quarantine Enforcement",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets Enable8021xQuarantine=0 to disable NAP enforcement through 802.1X-authenticated network switches. Prevents 802.1X-based client quarantine on wired/wireless networks.",
                    Tags = ["nap", "802.1x", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "802.1X NAP quarantine disabled; wired/wireless enforcement bypassed for this client.",
                    ApplyOps = [RegOp.SetDword(NapKey, "Enable8021xQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "Enable8021xQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "Enable8021xQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-vpn-quarantine",
                    Label = "Disable NAP VPN Quarantine Enforcement",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets EnableVpnQuarantine=0 to disable VPN-based NAP enforcement. Prevents VPN connections from triggering NAP health evaluations and potential quarantine.",
                    Tags = ["nap", "vpn", "quarantine", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "VPN health checks via NAP bypassed; may reduce VPN connection setup time.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableVpnQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableVpnQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableVpnQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-ipsec-quarantine",
                    Label = "Disable NAP IPSec Quarantine Enforcement",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets EnableIpsecQuarantine=0 to disable IPSec-based NAP health enforcement. Prevents IPSec connections from routing through NAP health validation and quarantine zones.",
                    Tags = ["nap", "ipsec", "quarantine", "network", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 3,
                    ImpactNote = "IPSec NAP health checks disabled; IPSec connections bypass health gating.",
                    ApplyOps = [RegOp.SetDword(NapKey, "EnableIpsecQuarantine", 0)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "EnableIpsecQuarantine")],
                    DetectOps = [RegOp.CheckDword(NapKey, "EnableIpsecQuarantine", 0)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-dhcp-auto-remediation",
                    Label = "Disable NAP DHCP Auto-Remediation",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets DisableDhcpAutoRemediation=1 to prevent the NAP client from automatically attempting to remediate health failures during DHCP-based enforcement. Manual intervention is required.",
                    Tags = ["nap", "dhcp", "remediation", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Automatic DHCP remediation disabled; health policy failures require manual administrator action.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableDhcpAutoRemediation", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableDhcpAutoRemediation")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableDhcpAutoRemediation", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-status-notifications",
                    Label = "Disable NAP Status Notifications",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets DisableStatusNotifications=1 to suppress NAP status change notifications from appearing to users. Network Access Protection events are logged but not displayed.",
                    Tags = ["nap", "notifications", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP status notifications hidden from users; NAP events still logged in Event Viewer.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableStatusNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableStatusNotifications")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableStatusNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-ui",
                    Label = "Disable NAP User Interface",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets DisableUserUi=1 to completely disable the Network Access Protection user interface. NAP-related dialogs and status pages are inaccessible to users.",
                    Tags = ["nap", "ui", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP UI fully disabled; no user-facing NAP dialogs, status screens, or repair wizards.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisableUserUi", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisableUserUi")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisableUserUi", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-hide-nap-tray-icon",
                    Label = "Hide NAP System Tray Icon",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets HideSystemTrayIcon=1 to prevent the NAP system tray notification icon from appearing. Reduces status bar clutter when NAP components are otherwise disabled.",
                    Tags = ["nap", "tray", "ui", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "NAP tray icon hidden; no impact on networking, only removes the visual indicator.",
                    ApplyOps = [RegOp.SetDword(NapKey, "HideSystemTrayIcon", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "HideSystemTrayIcon")],
                    DetectOps = [RegOp.CheckDword(NapKey, "HideSystemTrayIcon", 1)],
                },
                new TweakDef
                {
                    Id = "nappol-disable-nap-policy-autoupdate",
                    Label = "Disable NAP Policy Auto-Update",
                    Category = "Network — Network Access Prot",
                    Description =
                        "Sets DisablePolicyAutoUpdate=1 to prevent the NAP client from automatically downloading updated health requirement policies from the network policy server (NPS).",
                    Tags = ["nap", "policy", "update", "network"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "NAP policy updates disabled; client retains last-known health policy settings.",
                    ApplyOps = [RegOp.SetDword(NapKey, "DisablePolicyAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(NapKey, "DisablePolicyAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(NapKey, "DisablePolicyAutoUpdate", 1)],
                },
            ];
    }
}
