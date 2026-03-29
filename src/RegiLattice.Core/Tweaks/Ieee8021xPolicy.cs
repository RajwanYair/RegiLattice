// RegiLattice.Core — Tweaks/Ieee8021xPolicy.cs
// IEEE 802.1x Port-Based Network Access Control Policy — Sprint 551.
// Configures Group Policy for wired/wireless 802.1x EAP authentication,
// supplicant timeouts, EAP method enforcement, fallback behaviour, and
// network access control integration.
// Category: "IEEE 802.1x Policy" | Slug: ieee8021x
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WiredNetwork
//           HKLM\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Ieee8021xPolicy
{
    private const string WiredKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WiredNetwork";

    private const string WirelessKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WirelessNetwork";

    private const string EapKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EapHost";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "ieee8021x-enable-wired-8021x",
                Label = "IEEE 802.1x: Enable Wired Network 802.1x Authentication",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets EnableAutoConfig=1 in WiredNetwork policy. Activates the Windows wired 802.1x supplicant (the Wired AutoConfig service) and enables port-based network access control on all wired Ethernet adapters. With 802.1x enabled, the switch's authentication server (RADIUS) validates the endpoint's identity before granting network access. Unauthenticated endpoints are placed in a guest VLAN or denied access. Essential security control for protecting internal network access via physical Ethernet port.",
                Tags = ["ieee8021x", "wired", "authentication", "network", "nac"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "Wired 802.1x authentication is enforced. Network switches must support 802.1x and be configured with RADIUS server settings. Machines without valid credentials are denied network access — ensure machine certificates are enrolled first.",
                ApplyOps = [RegOp.SetDword(WiredKey, "EnableAutoConfig", 1)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "EnableAutoConfig")],
                DetectOps = [RegOp.CheckDword(WiredKey, "EnableAutoConfig", 1)],
            },
            new TweakDef
            {
                Id = "ieee8021x-set-eapol-start-timeout",
                Label = "IEEE 802.1x: Set EAPOL-Start Timeout to 20 Seconds",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets AuthResponse=20 in WiredNetwork policy. Configures the supplicant's EAPOL-Start transmission timer: how long the supplicant waits for an EAPOL Request-Identity from the switch before sending an explicit EAPOL-Start frame. On access switches that do not send the initial EAP Request/Identity packet (authenticator-initiated), the supplicant must initiate. A longer initial wait delays authentication. Setting to 20 seconds reduces the wait for supplicant-initiated authentication without being so short it floods slow switch responses.",
                Tags = ["ieee8021x", "eapol", "timeout", "supplicant", "wired"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "EAPOL-Start timer set to 20 seconds. Only affects authentication initiation timing. Network connectivity is unchanged once authentication completes.",
                ApplyOps = [RegOp.SetDword(WiredKey, "AuthResponse", 20)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "AuthResponse")],
                DetectOps = [RegOp.CheckDword(WiredKey, "AuthResponse", 20)],
            },
            new TweakDef
            {
                Id = "ieee8021x-set-max-eapol-retransmit",
                Label = "IEEE 802.1x: Limit EAPOL-Start Retransmit Count to 5",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets MaxEapolStartAttempts=5 in WiredNetwork policy. Limits the number of EAPOL-Start retransmission attempts before the supplicant gives up and treats the port as unauthenticated. Each EAPOL-Start is separated by the AuthResponse timer (AuthResponse seconds). With 5 retransmits at 20 seconds each, the supplicant spends 100 seconds attempting to authenticate before failing. This limits the time a machine retries against an unavailable RADIUS server while ensuring legitimate connection delays are handled gracefully.",
                Tags = ["ieee8021x", "eapol", "retransmit", "retry", "supplicant"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "EAPOL-Start retransmit limit is 5 attempts. After 5 failures, the supplicant stops trying. Most switches respond to the first EAPOL-Start; retransmits are only relevant for switches with slow RADIUS response.",
                ApplyOps = [RegOp.SetDword(WiredKey, "MaxEapolStartAttempts", 5)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "MaxEapolStartAttempts")],
                DetectOps = [RegOp.CheckDword(WiredKey, "MaxEapolStartAttempts", 5)],
            },
            new TweakDef
            {
                Id = "ieee8021x-enable-single-sign-on",
                Label = "IEEE 802.1x: Enable Single Sign-On Integration",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets BlockPeriod=0 in WiredNetwork policy (enables SSO) and sets SingleSignOn=1. Configures the wired supplicant to synchronise 802.1x authentication with Windows logon. In SSO mode, machine certificates are used before the Windows logon screen (machine authentication), and user certificates are presented after the user logs in (user authentication). Without SSO, the machine may remain in a partially authenticated state during logon, potentially delaying Group Policy application or network-dependent logon scripts.",
                Tags = ["ieee8021x", "sso", "logon", "authentication", "wired"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "SSO integrates 802.1x with Windows logon. Machine authenticates before logon screen; user authenticates after. Requires both machine and user certificates to be enrolled.",
                ApplyOps =
                [
                    RegOp.SetDword(WiredKey, "BlockPeriod", 0),
                    RegOp.SetDword(WiredKey, "SingleSignOn", 1),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(WiredKey, "BlockPeriod"),
                    RegOp.DeleteValue(WiredKey, "SingleSignOn"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(WiredKey, "BlockPeriod", 0),
                    RegOp.CheckDword(WiredKey, "SingleSignOn", 1),
                ],
            },
            new TweakDef
            {
                Id = "ieee8021x-disable-user-only-mode",
                Label = "IEEE 802.1x: Disable User-Only 802.1x Mode (Require Machine Auth)",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets UserOnlyMode=0 in WiredNetwork policy. Prevents the supplicant from operating in user-only authentication mode where only user credentials are sent and machine credentials are not used. In user-only mode, the machine VLAN access depends entirely on the currently logged-in user's authentication; when no user is logged in (e.g., at Windows logon screen, or when machine admin scripts run before user logon), the port may be unauthenticated. Machine authentication ensures the port is authenticated regardless of user logon state.",
                Tags = ["ieee8021x", "machine-auth", "user-auth", "wired", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Machine authentication is required; user-only mode is disabled. Machine certificates must be enrolled before the logon screen appears. Group Policy and SCCM scripts that run at machine startup require machine-authenticated network access.",
                ApplyOps = [RegOp.SetDword(WiredKey, "UserOnlyMode", 0)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "UserOnlyMode")],
                DetectOps = [RegOp.CheckDword(WiredKey, "UserOnlyMode", 0)],
            },
            new TweakDef
            {
                Id = "ieee8021x-disable-guest-vlan-access",
                Label = "IEEE 802.1x: Disable Automatic Guest VLAN Fallback on Auth Failure",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets AllowGuestVLAN=0 in WiredNetwork policy. Prevents the Windows 802.1x supplicant from signalling to the switch that it accepts placement in a guest VLAN on authentication failure. Some switch configurations use EAP-Failure with a VLAN assignment to place unauthenticated endpoints in a restricted guest VLAN. Setting AllowGuestVLAN=0 causes the supplicant to treat authentication failure as a complete block rather than accepting reduced-access guest VLAN placement. Prevents accidental network access via the guest VLAN.",
                Tags = ["ieee8021x", "guest-vlan", "fallback", "authentication", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Guest VLAN fallback is disabled. Failed authentication results in no network access rather than guest VLAN. Ensure there is an alternate recovery path (console access, out-of-band management) for machines with certificate failures.",
                ApplyOps = [RegOp.SetDword(WiredKey, "AllowGuestVLAN", 0)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "AllowGuestVLAN")],
                DetectOps = [RegOp.CheckDword(WiredKey, "AllowGuestVLAN", 0)],
            },
            new TweakDef
            {
                Id = "ieee8021x-set-eap-type-tls",
                Label = "IEEE 802.1x: Set Wired EAP Method to EAP-TLS (Certificate-Based)",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets EAPType=13 in WiredNetwork policy (EAP-TLS, EAP type 13 per IANA). Configures the Windows wired 802.1x supplicant to use EAP-TLS as the preferred EAP authentication method. EAP-TLS uses mutual certificate-based authentication: the client presents a certificate (from machine store or smart card) and the RADIUS server presents a server certificate. It is the strongest EAP method available, providing phishing-resistant authentication and protection against credential interception attacks that affect password-based PEAP.",
                Tags = ["ieee8021x", "eap-tls", "certificate", "authentication", "wired"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "EAP-TLS requires client certificates on all machines. Machine certificates must be enrolled via enterprise CA before deployment. Strongest 802.1x authentication method — recommended for high-security environments.",
                ApplyOps = [RegOp.SetDword(WiredKey, "EAPType", 13)],
                RemoveOps = [RegOp.DeleteValue(WiredKey, "EAPType")],
                DetectOps = [RegOp.CheckDword(WiredKey, "EAPType", 13)],
            },
            new TweakDef
            {
                Id = "ieee8021x-enable-eap-inner-identity",
                Label = "IEEE 802.1x: Enable Anonymous Identity (Outer Identity Privacy)",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets EnableAnonymousIdentity=1 in EapHost policy. Enables the use of an anonymous outer identity in tunnelled EAP methods (PEAP, TTLS). The EAP Identity response (outer identity) is transmitted in plaintext in the first EAP exchange. Without anonymous identity, the user's actual username is visible in the network traffic of the outer EAP exchange, revealing who is authenticating before the TLS tunnel is established. An anonymous outer identity (e.g., 'anonymous@contoso.com') hides the actual username until the protected tunnel is established.",
                Tags = ["ieee8021x", "eap", "identity", "privacy", "peap"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Anonymous identity is sent as EAP outer identity. The RADIUS server must be configured to match the anonymous realm. The real user identity is still used inside the TLS tunnel for authentication.",
                ApplyOps = [RegOp.SetDword(EapKey, "EnableAnonymousIdentity", 1)],
                RemoveOps = [RegOp.DeleteValue(EapKey, "EnableAnonymousIdentity")],
                DetectOps = [RegOp.CheckDword(EapKey, "EnableAnonymousIdentity", 1)],
            },
            new TweakDef
            {
                Id = "ieee8021x-disable-cached-wireless-creds",
                Label = "IEEE 802.1x: Disable Wireless 802.1x Credential Caching",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets DisableUserCredentialCaching=1 in WirelessNetwork policy. Prevents the Windows wireless supplicant from caching 802.1x user credentials to local storage. Some EAP methods (PEAP-MSCHAPv2) can cache credentials to allow re-authentication without prompting the user. While convenient, cached credentials are a persistence mechanism for the credentials and may be accessible from the credential cache if the machine is compromised. Required credentials are re-fetched on each authentication using the user's interactive session or certificate store.",
                Tags = ["ieee8021x", "wireless", "credential-cache", "security", "wifi"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Wireless 802.1x credentials are not cached. Users may be re-prompted for credentials on reconnection if using PEAP-MSCHAPv2. Certificate-based EAP-TLS is unaffected.",
                ApplyOps = [RegOp.SetDword(WirelessKey, "DisableUserCredentialCaching", 1)],
                RemoveOps = [RegOp.DeleteValue(WirelessKey, "DisableUserCredentialCaching")],
                DetectOps = [RegOp.CheckDword(WirelessKey, "DisableUserCredentialCaching", 1)],
            },
            new TweakDef
            {
                Id = "ieee8021x-require-mutual-authentication",
                Label = "IEEE 802.1x: Require Mutual Authentication in PEAP Handshake",
                Category = "IEEE 802.1x Policy",
                Description =
                    "Sets RequireMutualAuth=1 in EapHost policy. Requires that tunnelled EAP methods (PEAP, TTLS) perform full mutual authentication: the server must present a trusted certificate AND the client must authenticate with a credential inside the TLS tunnel. Without mutual authentication, a rogue access point can complete the outer TLS handshake with any certificate while the client still transmits their inner credentials to an attacker-controlled server. Mutual authentication (server cert validation + valid inner credentials) is the minimum security requirement for PEAP deployments.",
                Tags = ["ieee8021x", "mutual-authentication", "peap", "certificate", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Mutual authentication is required for PEAP/TTLS. RADIUS/NPS servers must present a trusted certificate. Ensures both server and client authenticate — critical for preventing evil twin attacks.",
                ApplyOps = [RegOp.SetDword(EapKey, "RequireMutualAuth", 1)],
                RemoveOps = [RegOp.DeleteValue(EapKey, "RequireMutualAuth")],
                DetectOps = [RegOp.CheckDword(EapKey, "RequireMutualAuth", 1)],
            },
        ];
}
