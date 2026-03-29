// RegiLattice.Core — Tweaks/CryptographicOperationsPolicy.cs
// Cryptographic Operations Policy — Sprint 547.
// Configures Group Policy for Windows cryptographic service provider (CSP)
// settings, algorithm restrictions, CNG (Cryptography Next Generation) policy,
// TLS/DTLS cipher suite ordering, and hash algorithm enforcement.
// Category: "Cryptographic Operations Policy" | Slug: cryptops
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Cryptography

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CryptographicOperationsPolicy
{
    private const string CryptoKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography";

    private const string CngKey =
        @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Cryptography\Configuration\SSL\00010002";

    private const string FipsKey =
        @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa\FipsAlgorithmPolicy";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "cryptops-enable-fips-mode",
                Label = "Cryptographic: Enable FIPS 140-2 Compliant Algorithm Mode",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets Enabled=1 in FipsAlgorithmPolicy. Activates Windows FIPS 140-2 compliant algorithm mode. FIPS mode restricts all cryptographic operations to NIST-validated algorithms: AES (128/192/256 bit), 3DES (112-bit effective), SHA-1/SHA-256/SHA-384/SHA-512, RSA, and ECDH. It disables non-FIPS algorithms including RC4, MD5, DES, and any non-validated implementations. Required by US Federal Government agencies, DoD, HIPAA-compliant healthcare, and certain financial institutions.",
                Tags = ["fips", "cryptography", "compliance", "federal", "algorithm"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote = "FIPS mode breaks some applications using non-FIPS algorithms (RC4, MD5, non-validated TLS). Test all business applications before enabling in production. Known to break: some Java apps, older .NET apps, certain VPN clients that use RC4.",
                ApplyOps = [RegOp.SetDword(FipsKey, "Enabled", 1)],
                RemoveOps = [RegOp.DeleteValue(FipsKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(FipsKey, "Enabled", 1)],
            },
            new TweakDef
            {
                Id = "cryptops-disable-rc4-cipher",
                Label = "Cryptographic: Disable RC4 Cipher in TLS/DTLS Negotiation",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets RC4Enabled=0 in Cryptography policy. Removes RC4 from the TLS cipher suite negotiation list. RC4 is broken: statistical biases in its keystream have been exploited in the RC4 NOMORE attack (86 hours to decrypt a cookie in RC4-protected TLS). IETF prohibited RC4 in TLS in RFC 7465 (2015). Despite the RFC prohibition, RC4 remains enabled on Windows by default for backwards compatibility. Explicitly disabling RC4 enforces the RFC 7465 prohibition in the Windows Schannel TLS provider.",
                Tags = ["cryptography", "rc4", "tls", "cipher", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "RC4 is removed from TLS negotiation. Very old clients that offer only RC4 cannot connect. All TLS 1.2+ clients support AES-based cipher suites.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "RC4Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "RC4Enabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "RC4Enabled", 0)],
            },
            new TweakDef
            {
                Id = "cryptops-disable-md5-signature",
                Label = "Cryptographic: Disable MD5 for Digital Signature Verification",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets MD5SignatureEnabled=0 in Cryptography policy. Disables MD5 signatures in the Windows cryptographic infrastructure's digital signature verification pipeline. MD5 is cryptographically broken (collision attacks are practical): forged X.509 certificates signed with MD5 have been demonstrated in academic research (the Rogue CA attack in 2008). Any certificate in the chain using MD5 is treated as invalid. Windows already rejects MD5 certificates in many contexts; this policy extends the restriction.",
                Tags = ["cryptography", "md5", "signature", "certificate", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "MD5-signed certificates are rejected. Verify that no internal CA certs or code-signing certs in the environment use MD5. The public PKI has been using SHA-256 since 2017.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "MD5SignatureEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "MD5SignatureEnabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "MD5SignatureEnabled", 0)],
            },
            new TweakDef
            {
                Id = "cryptops-set-min-rsa-key-length",
                Label = "Cryptographic: Enforce Minimum 2048-bit RSA Key Length",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets MinRsaKeyLength=2048 in Cryptography policy. Rejects any RSA cryptographic operation (key generation, signature verification, key exchange) using keys shorter than 2048 bits. NIST deprecated 1024-bit RSA in 2010 (Special Publication 800-131A); keys of this length are attackable by well-resourced adversaries using GNFS factoring. 2048-bit RSA provides approximately 112 bits of security and is the current minimum for new deployments through 2030.",
                Tags = ["cryptography", "rsa", "key-length", "pki", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Rejects operations with RSA keys <2048 bits. Verify that no legacy certificates in the environment use 1024-bit RSA. Old code signing, S/MIME, or VPN certs may need renewal.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "MinRsaKeyLength", 2048)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "MinRsaKeyLength")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "MinRsaKeyLength", 2048)],
            },
            new TweakDef
            {
                Id = "cryptops-enable-crl-check",
                Label = "Cryptographic: Enable CRL Check on All Certificate Verification",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets CertificateRevocationEnabled=1 in Cryptography policy. Enables CRL (Certificate Revocation List) checking for all certificate verification operations. When a certificate is presented for authentication or TLS, Windows checks the issuing CA's CRL distribution point to verify the certificate has not been revoked. Without CRL checking, revoked certificates (e.g., from a compromised private key) remain functional. This is a mandatory control in PKI-secured environments.",
                Tags = ["cryptography", "crl", "certificate", "revocation", "pki"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "CRL download failures (network unavailable during cert verification) cause authentication failure. Ensure CDP/OCSP endpoints are accessible or implement OCSP caching.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "CertificateRevocationEnabled", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "CertificateRevocationEnabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "CertificateRevocationEnabled", 1)],
            },
            new TweakDef
            {
                Id = "cryptops-disable-null-ciphers",
                Label = "Cryptographic: Disable NULL Cipher Suite in TLS",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets NullCipherEnabled=0 in Cryptography policy. Removes all NULL encryption cipher suites from TLS/SSL negotiation. NULL cipher suites (TLS_RSA_WITH_NULL_SHA, etc.) perform authentication and integrity checking but transmit payload data in plaintext. While uncommon in practice, NULL ciphers in the cipher suite list represent a denial-of-confidentiality risk: a man-in-the-middle that can manipulate TLS negotiation could force both parties to select a NULL cipher, establishing an authenticated but unencrypted channel.",
                Tags = ["cryptography", "null-cipher", "tls", "plaintext", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "NULL ciphers are removed. No legitimate application should use NULL ciphers; this setting should have no operational impact.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "NullCipherEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "NullCipherEnabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "NullCipherEnabled", 0)],
            },
            new TweakDef
            {
                Id = "cryptops-require-strong-key-protection",
                Label = "Cryptographic: Require Strong Key Protection UI for Private Keys",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets ForceKeyProtection=2 in Cryptography policy. Configures the CSP to require explicit user interaction (a password prompt) every time a private key is accessed from the Windows certificate store. With strong protection, private keys cannot be silently accessed by malware running in the user context without triggering a UI prompt that requires button click or password entry. This detects and prevents silent RSA private key use by credential-theft tools (e.g., mimikatz crypto module).",
                Tags = ["cryptography", "private-key", "protection", "key-guard", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Private key use prompts appear at every signature or decryption operation (email signing, VPN auth). Users with S/MIME-signed email or certificate-based auth will see frequent prompts.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "ForceKeyProtection", 2)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "ForceKeyProtection")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "ForceKeyProtection", 2)],
            },
            new TweakDef
            {
                Id = "cryptops-disable-sha1-server-auth",
                Label = "Cryptographic: Disable SHA-1 for Server Authentication Certificates",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets SHA1ServerAuthEnabled=0 in Cryptography policy. Rejects SHA-1 signed server authentication certificates from TLS handshakes. SHA-1 has been practically broken since 2017 (Google's SHAttered attack demonstrated a SHA-1 collision for $75,000 in cloud compute). Major CAs stopped issuing SHA-1 certs in 2016; public trust anchors no longer accept SHA-1. Internal PKI CAs that still issue SHA-1 server certs should be upgraded to SHA-256.",
                Tags = ["cryptography", "sha1", "tls", "certificate", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "SHA-1 server certs are rejected. Internal web applications with SHA-1 certs will fail TLS. Audit internal CA to identify SHA-1 certs before enabling.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "SHA1ServerAuthEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "SHA1ServerAuthEnabled")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "SHA1ServerAuthEnabled", 0)],
            },
            new TweakDef
            {
                Id = "cryptops-enable-pkcs11-interface",
                Label = "Cryptographic: Enable PKCS#11 Hardware Token Interface",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets EnablePkcs11=1 in Cryptography policy. Registers the Windows PKCS#11 bridge layer, enabling applications that use the PKCS#11 (Cryptoki) standard hardware security module API to use Windows-managed smart cards and Trusted Platform Module (TPM) key storage via a unified interface. This is required in environments deploying hardware security tokens for code signing, SSH key storage, or network authentication (e.g., PIV smart cards, YubiKey HSM).",
                Tags = ["cryptography", "pkcs11", "smart-card", "hsm", "token"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Enables PKCS#11 bridge. Required for hardware token integration. No impact if no PKCS#11 applications are deployed.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "EnablePkcs11", 1)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "EnablePkcs11")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "EnablePkcs11", 1)],
            },
            new TweakDef
            {
                Id = "cryptops-disable-export-of-user-keys",
                Label = "Cryptographic: Prevent Export of User Private Keys from Key Store",
                Category = "Cryptographic Operations Policy",
                Description =
                    "Sets AllowKeyExport=0 in Cryptography policy. Prevents the export of user private keys from the Windows certificate store to PFX files. Private key export is a credential theft vector: an attacker with user-level access can export the user's private email signing key, code signing key, or authentication certificate to a PFX file and exfiltrate it. Keys stored in non-exportable containers provide in-place security; the private key cannot be removed from the machine's key storage even by the key owner.",
                Tags = ["cryptography", "key-export", "private-key", "dlp", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Private keys cannot be exported. Users cannot back up their private keys or move them to another device. Ensure key archival is handled by the enterprise CA (key recovery) before enabling.",
                ApplyOps = [RegOp.SetDword(CryptoKey, "AllowKeyExport", 0)],
                RemoveOps = [RegOp.DeleteValue(CryptoKey, "AllowKeyExport")],
                DetectOps = [RegOp.CheckDword(CryptoKey, "AllowKeyExport", 0)],
            },
        ];
}
