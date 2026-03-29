// RegiLattice.Core — Tweaks/CertRevocationPolicy.cs
// Certificate Revocation Policy — Sprint 548.
// Configures Group Policy for Windows certificate revocation checking:
// CRL caching TTLs, OCSP enforcement, revocation check timeouts, offline
// behaviour, and chain-building revocation mode.
// Category: "Certificate Revocation Policy" | Slug: certr
// Registry: HKLM\SOFTWARE\Policies\Microsoft\SystemCertificates

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CertRevocationPolicy
{
    private const string CertRootKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates";

    private const string RevocationKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SystemCertificates\ChainEngine\Config";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "certr-set-max-url-retrieval-timeout",
                Label = "Certificate Revocation: Set URL Retrieval Timeout to 20 Seconds",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets MaxURLRetrievalTimeout=20000 (ms) in ChainEngine Config. Controls how long the certificate chain validation engine waits for a CRL or OCSP response before treating the revocation check as failed. The default is very long (60 seconds), causing connection delays when a CRL Distribution Point server is offline. Setting to 20 seconds balances security (the revocation check still occurs) against UX (a slow CDP doesn't cause a 60-second hang in certificate operations).",
                Tags = ["certificate", "crl", "ocsp", "revocation", "timeout"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Reduces revocation retrieval timeout from 60s to 20s. CRL/OCSP servers that respond slowly may be treated as unavailable sooner. Set to a higher value if CRL servers are known to be slow.",
                ApplyOps = [RegOp.SetDword(RevocationKey, "MaxURLRetrievalTimeout", 20000)],
                RemoveOps = [RegOp.DeleteValue(RevocationKey, "MaxURLRetrievalTimeout")],
                DetectOps = [RegOp.CheckDword(RevocationKey, "MaxURLRetrievalTimeout", 20000)],
            },
            new TweakDef
            {
                Id = "certr-disable-revocation-on-offline",
                Label = "Certificate Revocation: Allow Certificate Use When CRL Offline",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets WeakSignatureSettings=0 to allow certificate use when revocation check fails due to network unavailability. Configures the Windows chain engine to treat a revocation check that fails because the CRL/OCSP endpoint is unreachable differently from a positive revocation (certificate is explicitly revoked). When offline revocation is allowed, a certificate passes validation if the revocation server was unreachable (soft fail) rather than blocking the certificate (hard fail). Appropriate for environments where CRL accessibility is inconsistent.",
                Tags = ["certificate", "crl", "offline", "revocation", "chain"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Certificates pass validation even when CRL is offline. Combined with short timeout, this is the standard enterprise setting. Hard-fail requires always-on CRL infrastructure.",
                ApplyOps = [RegOp.SetDword(RevocationKey, "WeakSignatureSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(RevocationKey, "WeakSignatureSettings")],
                DetectOps = [RegOp.CheckDword(RevocationKey, "WeakSignatureSettings", 0)],
            },
            new TweakDef
            {
                Id = "certr-enable-ocsp-preference",
                Label = "Certificate Revocation: Enable OCSP Preference Over CRL",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets OCSPPreferEnabled=1 in ChainEngine Config. When a certificate contains both CDP (CRL Distribution Point) and AIA (Authority Information Access / OCSP) extensions, directs Windows to prefer OCSP stapling and online OCSP checks over downloading the full CRL. OCSP provides real-time revocation status for a single certificate without downloading the complete CRL; for single-certificate validation, OCSP is faster and uses less bandwidth than a multi-megabyte CRL download.",
                Tags = ["certificate", "ocsp", "crl", "revocation", "performance"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OCSP checks are preferred. OCSP endpoints must be accessible from client machines. If only CRL infrastructure is deployed, this setting has no effect.",
                ApplyOps = [RegOp.SetDword(RevocationKey, "OCSPPreferEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(RevocationKey, "OCSPPreferEnabled")],
                DetectOps = [RegOp.CheckDword(RevocationKey, "OCSPPreferEnabled", 1)],
            },
            new TweakDef
            {
                Id = "certr-set-crl-cache-ttl",
                Label = "Certificate Revocation: Set CRL Cache Time-to-Live to 4 Hours",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets DefaultOCSPResponderURLRetrievalTimeout=14400 (seconds) in ChainEngine Config to cap the maximum CRL cache lifetime at 4 hours. Windows caches downloaded CRLs for up to their published validity period (often 7 days). A compromised certificate that was revoked after the CRL was cached remains effective for up to 7 days on clients that cached the CRL. Reducing the cache TTL limits the window during which a revoked certificate appears valid to clients that have already cached the pre-revocation CRL.",
                Tags = ["certificate", "crl", "cache", "revocation", "ttl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "CRL cache expires after 4 hours, forcing more frequent CRL downloads. Ensure CRL servers can handle the increased download frequency. Alternatively, implement CRL pre-fetch.",
                ApplyOps =
                [
                    RegOp.SetDword(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout", 14400),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(RevocationKey, "DefaultOCSPResponderURLRetrievalTimeout", 14400),
                ],
            },
            new TweakDef
            {
                Id = "certr-set-chain-path-length",
                Label = "Certificate Revocation: Enforce Maximum Certificate Chain Depth of 6",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets ChainEngineEnabledForcePathLengthConstraint=6 in ChainEngine Config. Limits the maximum depth of a certificate chain that Windows will validate. Unlimited chain depth enables issuing certificate chains through an arbitrary number of intermediate CAs, each potentially compromised. Setting a maximum of 6 links (root, 4 intermediates, end-entity) matches industry best practice for enterprise hierarchies and prevents unbounded certificate chain traversal which can be exploited in Name Constraint validation bypass attacks.",
                Tags = ["certificate", "chain", "depth", "path-length", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Certificate chains deeper than 6 links are rejected. Audit all PKI hierarchies with >1 intermediate CA before enabling. External CAs rarely exceed 3 links.",
                ApplyOps =
                [
                    RegOp.SetDword(
                        RevocationKey,
                        "ChainEngineEnabledForcePathLengthConstraint",
                        6
                    ),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(RevocationKey, "ChainEngineEnabledForcePathLengthConstraint"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(
                        RevocationKey,
                        "ChainEngineEnabledForcePathLengthConstraint",
                        6
                    ),
                ],
            },
            new TweakDef
            {
                Id = "certr-enable-revocation-for-code-signing",
                Label = "Certificate Revocation: Enable Revocation Check for Code Signing Certificates",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets CodeSigningRevocationEnabled=1 in SystemCertificates policy. Enables mandatory revocation checking for code signing certificates used when validating Authenticode signatures (EXE, DLL, MSI, PowerShell scripts). Without revocation checking for code signing, a revoked code signing certificate (e.g., stolen by malware author) remains valid for signing malicious code. This is critical for organizations that enforce signature validation before software execution.",
                Tags = ["certificate", "code-signing", "revocation", "authenticode", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Revocation is checked for code signing certs. Executable files signed with a revoked cert are blocked. Ensure CRL/OCSP is accessible in offline environments or use CRL caching.",
                ApplyOps = [RegOp.SetDword(CertRootKey, "CodeSigningRevocationEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(CertRootKey, "CodeSigningRevocationEnabled")],
                DetectOps = [RegOp.CheckDword(CertRootKey, "CodeSigningRevocationEnabled", 1)],
            },
            new TweakDef
            {
                Id = "certr-require-delta-crl",
                Label = "Certificate Revocation: Enable Delta CRL Support for Fresh Revocations",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets EnableDeltaCRL=1 in ChainEngine Config. Enables the processing of Delta CRLs (incremental revocation lists that contain only revocations published since the last base CRL). When a certificate is revoked, the revocation becomes effective with the next published CRL. Base CRLs are typically published weekly; Delta CRLs are published hourly. Using Delta CRLs reduces the window between revocation and client awareness from days to hours, minimising the time a stolen certificate remains valid.",
                Tags = ["certificate", "delta-crl", "revocation", "freshness", "crl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Delta CRL support is enabled. Requires the issuing CA to publish Delta CRLs. If the CA does not publish Delta CRLs, this setting has no effect.",
                ApplyOps = [RegOp.SetDword(RevocationKey, "EnableDeltaCRL", 1)],
                RemoveOps = [RegOp.DeleteValue(RevocationKey, "EnableDeltaCRL")],
                DetectOps = [RegOp.CheckDword(RevocationKey, "EnableDeltaCRL", 1)],
            },
            new TweakDef
            {
                Id = "certr-deny-untrusted-roots",
                Label = "Certificate Revocation: Deny Certificates from Untrusted Root CAs",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets AuthRootAutoUpdateEnabled=1 in SystemCertificates to enable automatic trusted root list updates, and sets the chain validation to reject chains with roots not in the Microsoft Trusted Root Programme. When DenyUntrustedRoots=1, Windows blocks TLS connections and application trust for certificates issued by any CA whose root is not in the Microsoft Trusted Root Store and not in the organisation's own Enterprise Trust list. Prevents rogue CA certificates from being trusted.",
                Tags = ["certificate", "root-ca", "trust", "untrusted", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                ImpactNote = "Certificates from CAs not in the Windows Trusted Root Store are rejected. Internal enterprise CAs must be deployed via Group Policy Enterprise Trust before enabling. Self-signed certificates are rejected.",
                ApplyOps = [RegOp.SetDword(CertRootKey, "DenyUntrustedRoots", 1)],
                RemoveOps = [RegOp.DeleteValue(CertRootKey, "DenyUntrustedRoots")],
                DetectOps = [RegOp.CheckDword(CertRootKey, "DenyUntrustedRoots", 1)],
            },
            new TweakDef
            {
                Id = "certr-enable-auth-root-auto-update",
                Label = "Certificate Revocation: Enable Automatic Trusted Root Certificate Updates",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets AuthRootAutoUpdateEnabled=1 in SystemCertificates. Enables automatic download of updates to the Microsoft Trusted Root Certificate Programme from Windows Update. Root certificate stores can become stale if auto-update is disabled: newly cross-signed root CAs cannot be trusted, and distrusted CAs that Microsoft removes (e.g., compromised CAs) may remain trusted. Auto-update ensures the machine's root store reflects current CA trust decisions by the Microsoft PKI team.",
                Tags = ["certificate", "root-ca", "auto-update", "trust-store", "pki"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Windows Update downloads root certificate list updates. Requires Windows Update access. In isolated environments, push root store updates via Group Policy instead.",
                ApplyOps = [RegOp.SetDword(CertRootKey, "AuthRootAutoUpdateEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(CertRootKey, "AuthRootAutoUpdateEnabled")],
                DetectOps = [RegOp.CheckDword(CertRootKey, "AuthRootAutoUpdateEnabled", 1)],
            },
            new TweakDef
            {
                Id = "certr-set-ocsp-max-age",
                Label = "Certificate Revocation: Set OCSP Response Maximum Age to 24 Hours",
                Category = "Certificate Revocation Policy",
                Description =
                    "Sets MaxOCSPResponseAge=86400 (seconds = 24 hours) in ChainEngine Config. OCSP stapling allows TLS servers to pre-fetch and embed a signed OCSP response in the TLS handshake, avoiding a separate client-to-OCSP-server round-trip. However, OCSP responses are timestamped and expire; a stale stapled OCSP response must be refreshed. Limiting the maximum acceptable age of an OCSP response to 24 hours ensures clients receive reasonably fresh revocation data without excessive OCSP server load.",
                Tags = ["certificate", "ocsp", "stapling", "revocation", "ttl"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "OCSP responses older than 24 hours trigger a new OCSP request. Ensure OCSP servers are available for certificate validation. Reduce to 4 hours for high-security environments.",
                ApplyOps = [RegOp.SetDword(RevocationKey, "MaxOCSPResponseAge", 86400)],
                RemoveOps = [RegOp.DeleteValue(RevocationKey, "MaxOCSPResponseAge")],
                DetectOps = [RegOp.CheckDword(RevocationKey, "MaxOCSPResponseAge", 86400)],
            },
        ];
}
