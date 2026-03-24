// RegiLattice.Core — Tweaks/EapNetworkPolicy.cs
// Extensible Authentication Protocol (EAP) network GPO controls — Sprint 221.
// Hardens EAP/802.1X authentication for wired and wireless networks.
// Category: "EAP Network Policy" | Slug: eappol
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class EapNetworkPolicy
{
    private const string Key =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP";
    private const string PeapKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP\PEAP";
    private const string ClientKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\EAP\Client";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "eappol-require-server-cert-validation",
                Label = "Require Server Certificate Validation for EAP",
                Category = "EAP Network Policy",
                Description =
                    "Forces EAP authentication to validate the RADIUS/NPS server certificate before accepting the authentication challenge. Without this, a rogue access point with a fake RADIUS server can capture credentials. Default: validation may be skipped depending on supplicant configuration. Recommended: 1.",
                Tags = ["eap", "802.1x", "certificate", "radius", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "EAP refuses to complete authentication if the RADIUS server certificate fails validation; rogue AP attacks are blocked.",
                ApplyOps = [RegOp.SetDword(PeapKey, "RequireServerCertValidation", 1)],
                RemoveOps = [RegOp.DeleteValue(PeapKey, "RequireServerCertValidation")],
                DetectOps = [RegOp.CheckDword(PeapKey, "RequireServerCertValidation", 1)],
            },
            new TweakDef
            {
                Id = "eappol-disable-simple-certificate-selection",
                Label = "Disable Simple Certificate Selection for EAP-TLS",
                Category = "EAP Network Policy",
                Description =
                    "Disables the automatic (heuristic) certificate selection for EAP-TLS / PEAP authentication. When enabled, Windows auto-selects a certificate without user confirmation — which can choose an expired or unintended certificate. Forcing explicit selection ensures the correct certificate is always used. Default: automatic selection enabled. Recommended: 1.",
                Tags = ["eap", "eap-tls", "certificate", "selection", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Certificate selection is not automatic; users/supplicants must explicitly select the correct certificate for EAP-TLS.",
                ApplyOps = [RegOp.SetDword(ClientKey, "DisableSimpleCertSelection", 1)],
                RemoveOps = [RegOp.DeleteValue(ClientKey, "DisableSimpleCertSelection")],
                DetectOps = [RegOp.CheckDword(ClientKey, "DisableSimpleCertSelection", 1)],
            },
            new TweakDef
            {
                Id = "eappol-enable-fast-reconnect",
                Label = "Enable EAP Fast Reconnect (PEAP Session Resumption)",
                Category = "EAP Network Policy",
                Description =
                    "Enables PEAP fast reconnect which allows a client to resume an authenticated session without a full re-authentication when roaming between access points or reconnecting after a brief disconnection. Reduces authentication latency on Wi-Fi roaming without weakening security. Default: fast reconnect disabled in strict environments. Recommended: 1.",
                Tags = ["eap", "peap", "fast-reconnect", "wifi", "roaming", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "PEAP sessions resume quickly on reconnect/roam without full RADIUS re-authentication; Wi-Fi hand-off is seamless.",
                ApplyOps = [RegOp.SetDword(PeapKey, "FastReconnect", 1)],
                RemoveOps = [RegOp.DeleteValue(PeapKey, "FastReconnect")],
                DetectOps = [RegOp.CheckDword(PeapKey, "FastReconnect", 1)],
            },
            new TweakDef
            {
                Id = "eappol-disable-identity-privacy",
                Label = "Disable EAP Identity Privacy (Anonymous Outer Identity)",
                Category = "EAP Network Policy",
                Description =
                    "By default PEAP sends an outer anonymous identity (e.g., 'anonymous') to keep the real username hidden from unauthenticated observers. On tightly controlled networks where the RADIUS server already knows all identities, the anonymous outer identity adds unnecessary overhead. Setting this to 0 reveals the actual username in the outer EAP exchange. Recommended: leave as default (1) unless anonymous identity causes RADIUS matching issues.",
                Tags = ["eap", "peap", "identity", "privacy", "radius", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 3,
                ImpactNote = "Real username is exposed in outer EAP exchange; RADIUS server sees the actual identity at the network layer.",
                ApplyOps = [RegOp.SetDword(PeapKey, "DisableIdentityPrivacy", 0)],
                RemoveOps = [RegOp.DeleteValue(PeapKey, "DisableIdentityPrivacy")],
                DetectOps = [RegOp.CheckDword(PeapKey, "DisableIdentityPrivacy", 0)],
            },
            new TweakDef
            {
                Id = "eappol-require-cryptobinding",
                Label = "Require Cryptobinding for PEAP",
                Category = "EAP Network Policy",
                Description =
                    "Requires cryptobinding TLV in PEAP Type-Length-Value exchanges to bind the inner and outer authentication channels. Cryptobinding prevents channel-binding attacks where an attacker relays an inner authentication from a different outer TLS tunnel. Default: not required by all RADIUS implementations. Recommended: 1 where RADIUS supports it.",
                Tags = ["eap", "peap", "cryptobinding", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "PEAP requires cryptobinding; channel-binding relay attacks between different TLS sessions are prevented.",
                ApplyOps = [RegOp.SetDword(PeapKey, "RequireCryptobinding", 1)],
                RemoveOps = [RegOp.DeleteValue(PeapKey, "RequireCryptobinding")],
                DetectOps = [RegOp.CheckDword(PeapKey, "RequireCryptobinding", 1)],
            },
            new TweakDef
            {
                Id = "eappol-disable-eap-md5",
                Label = "Disable EAP-MD5 Authentication Method",
                Category = "EAP Network Policy",
                Description =
                    "Removes EAP-MD5 from the list of permitted EAP methods. MD5 uses a challenge-response scheme with a one-way hash that is vulnerable to dictionary and offline brute-force attacks. Operators should use EAP-TLS or PEAP instead. Default: MD5 may be offered. Recommended: 1.",
                Tags = ["eap", "md5", "authentication", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "EAP-MD5 authentication is blocked; clients must use a stronger method such as EAP-TLS or PEAP.",
                ApplyOps = [RegOp.SetDword(Key, "DisableEAPMD5", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEAPMD5")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEAPMD5", 1)],
            },
            new TweakDef
            {
                Id = "eappol-log-authentication-events",
                Label = "Enable EAP Authentication Event Logging",
                Category = "EAP Network Policy",
                Description =
                    "Records successful and failed EAP / 802.1X authentication attempts to the Security event log. Provides visibility into network authentication activity including unexpected failures that may indicate a rogue access point or credential attack. Default: EAP events not always forwarded to Security log. Recommended: 1.",
                Tags = ["eap", "802.1x", "audit", "logging", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "EAP authentication successes and failures are written to the Security event log.",
                ApplyOps = [RegOp.SetDword(Key, "LogSuccessfulAuthentications", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogSuccessfulAuthentications")],
                DetectOps = [RegOp.CheckDword(Key, "LogSuccessfulAuthentications", 1)],
            },
            new TweakDef
            {
                Id = "eappol-set-max-auth-failures",
                Label = "Set Maximum EAP Authentication Failures to 3",
                Category = "EAP Network Policy",
                Description =
                    "Limits the number of consecutive EAP authentication failures before the supplicant stops retrying. Limits brute-force attempts against the RADIUS server from a single endpoint and reduces event log noise from misconfigured supplicants. Default: may retry indefinitely. Recommended: 3.",
                Tags = ["eap", "authentication", "retry", "brute-force", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "After 3 consecutive authentication failures the supplicant stops retrying; manual intervention is needed to reconnect.",
                ApplyOps = [RegOp.SetDword(Key, "MaxAuthFailures", 3)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxAuthFailures")],
                DetectOps = [RegOp.CheckDword(Key, "MaxAuthFailures", 3)],
            },
            new TweakDef
            {
                Id = "eappol-require-mutual-auth",
                Label = "Require Mutual Authentication for EAP",
                Category = "EAP Network Policy",
                Description =
                    "Enforces bidirectional (mutual) authentication in EAP exchanges — both the client and the RADIUS server must authenticate to each other. Without mutual auth, a client may authenticate to a rogue server without the server proving its own identity. Default: some EAP types do not enforce mutual auth. Recommended: 1.",
                Tags = ["eap", "mutual-auth", "radius", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote = "Both client and server must authenticate; one-sided authentication and rogue-RADIUS attacks are prevented.",
                ApplyOps = [RegOp.SetDword(Key, "RequireMutualAuthentication", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireMutualAuthentication")],
                DetectOps = [RegOp.CheckDword(Key, "RequireMutualAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "eappol-block-nontls-methods",
                Label = "Block Non-TLS EAP Methods on Corporate Networks",
                Category = "EAP Network Policy",
                Description =
                    "Restricts 802.1X authentication to TLS-based EAP methods (EAP-TLS, PEAP-TLS) only. Legacy password-based EAP types (LEAP, EAP-PAP) that transmit or derive credentials without TLS protection are blocked. Default: legacy methods permitted. Recommended: 1 in enterprise 802.1X deployments.",
                Tags = ["eap", "tls", "legacy", "authentication", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Only TLS-protected EAP methods are accepted; password-based EAP types without TLS are rejected.",
                ApplyOps = [RegOp.SetDword(Key, "AllowOnlyTLSBasedMethods", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowOnlyTLSBasedMethods")],
                DetectOps = [RegOp.CheckDword(Key, "AllowOnlyTLSBasedMethods", 1)],
            },
        ];
}
