// RegiLattice.Core — Plugins/PackSignatureVerifier.cs
// RSA-SHA256 detached signature verification for Tweak Packs (T7.3).
//
// Design:
//   - Pack authors generate a 2048-bit (minimum) RSA key pair.
//   - The private key is kept by the author; the public key PEM is submitted to
//     the RajwanYair/RegiLattice-Packs index alongside the pack.
//   - When a pack is distributed, a detached signature file (<pack>.rlpack.sig)
//     is produced: base64(RSA-SHA256-sign(SHA256(pack.json UTF-8))).
//   - PackSignatureVerifier.Verify() re-hashes the pack JSON content, performs
//     RSA PKCS#1 v1.5 verification against the signature, and returns a
//     PackTrustLevel to indicate whether the pack is Signed, HashVerified, or None.
//
// Security guarantees:
//   - RsaKeyMinBits = 2048  (rejects insecure keys at load-time)
//   - Hash algorithm: SHA-256 (no MD5/SHA-1 fallback)
//   - Signature format: RSA PKCS#1 v1.5 (RSASSA-PKCS1-v1_5, not PSS) for
//     broadest tooling compatibility (openssl, gpg --sign --digest-algo SHA256).
//   - Public key input: PEM-encoded SubjectPublicKeyInfo (SPKI) — the standard
//     format from `openssl rsa -pubout`.

using System.Security.Cryptography;
using System.Text;

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Verifies RSA-SHA256 detached signatures for Tweak Packs.
/// All methods are stateless and thread-safe.
/// </summary>
public static class PackSignatureVerifier
{
    private const int RsaKeyMinBits = 2048;

    // Explicitly name the hash algorithm used for signatures.
    private static readonly HashAlgorithmName s_hashAlgorithm = HashAlgorithmName.SHA256;

    /// <summary>
    /// Verify a detached RSA-SHA256 signature against the pack JSON content.
    /// </summary>
    /// <param name="packJsonUtf8">Raw pack JSON content as UTF-8 bytes.</param>
    /// <param name="signatureBase64">Base64-encoded RSA signature (PKCS#1 v1.5).</param>
    /// <param name="publicKeyPem">PEM-encoded RSA public key (SPKI format).</param>
    /// <returns>
    /// <see langword="true"/> if the signature is valid; <see langword="false"/> otherwise.
    /// Throws <see cref="CryptographicException"/> only if the key PEM is malformed.
    /// </returns>
    public static bool Verify(ReadOnlySpan<byte> packJsonUtf8, string signatureBase64, string publicKeyPem)
    {
        if (string.IsNullOrWhiteSpace(signatureBase64)) return false;
        if (string.IsNullOrWhiteSpace(publicKeyPem)) return false;

        byte[] signature;
        try
        {
            signature = Convert.FromBase64String(signatureBase64);
        }
        catch (FormatException)
        {
            return false;
        }

        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);

        if (rsa.KeySize < RsaKeyMinBits)
            throw new CryptographicException($"RSA key size {rsa.KeySize} bits is below the minimum of {RsaKeyMinBits} bits.");

        return rsa.VerifyData(packJsonUtf8, signature, s_hashAlgorithm, RSASignaturePadding.Pkcs1);
    }

    /// <summary>Convenience overload that accepts the pack JSON as a string.</summary>
    public static bool Verify(string packJson, string signatureBase64, string publicKeyPem)
    {
        return Verify(Encoding.UTF8.GetBytes(packJson), signatureBase64, publicKeyPem);
    }

    /// <summary>
    /// Determine the <see cref="PackTrustLevel"/> for a pack after loading.
    /// </summary>
    /// <param name="packJson">Raw pack JSON.</param>
    /// <param name="pack">The loaded <see cref="PackDef"/> (provides Sha256 and SignatureUrl).</param>
    /// <param name="signatureBase64">
    /// Base64 signature string fetched from <paramref name="pack"/>.<see cref="PackDef.SignatureUrl"/>
    /// by the caller. <see langword="null"/> or empty means the signature was not fetched / not available.
    /// </param>
    /// <param name="publicKeyPem">
    /// Author public key, looked up from the marketplace index. Null means unknown.
    /// </param>
    public static PackTrustLevel DetermineTrustLevel(
        string packJson,
        PackDef pack,
        string? signatureBase64,
        string? publicKeyPem)
    {
        // If SHA-256 was provided, verify it first.
        if (!string.IsNullOrWhiteSpace(pack.Sha256))
        {
            string actualHash = PackLoader.ComputeSha256(packJson);
            if (!string.Equals(actualHash, pack.Sha256, StringComparison.OrdinalIgnoreCase))
                return PackTrustLevel.None; // hash mismatch → untrusted
        }

        // If a detached signature and public key were both supplied, verify the signature.
        if (!string.IsNullOrWhiteSpace(signatureBase64) && !string.IsNullOrWhiteSpace(publicKeyPem))
        {
            bool sigOk;
            try
            {
                sigOk = Verify(packJson, signatureBase64, publicKeyPem);
            }
            catch (CryptographicException)
            {
                sigOk = false;
            }

            return sigOk ? PackTrustLevel.Signed : PackTrustLevel.None;
        }

        // Signature not available but hash was present and matched.
        if (!string.IsNullOrWhiteSpace(pack.Sha256))
            return PackTrustLevel.HashVerified;

        return PackTrustLevel.None;
    }

    /// <summary>
    /// Generate a new RSA-2048 key pair and return the PEM strings.
    /// Intended for use in tooling/tests; not called at runtime.
    /// </summary>
    /// <returns>(publicKeyPem, privateKeyPem)</returns>
    public static (string PublicKeyPem, string PrivateKeyPem) GenerateKeyPair(int keySize = 2048)
    {
        if (keySize < RsaKeyMinBits)
            throw new ArgumentException($"Key size must be at least {RsaKeyMinBits} bits.", nameof(keySize));

        using RSA rsa = RSA.Create(keySize);
        return (rsa.ExportSubjectPublicKeyInfoPem(), rsa.ExportPkcs8PrivateKeyPem());
    }

    /// <summary>
    /// Sign pack JSON content with the given RSA private key PEM and return a base64 signature.
    /// Intended for pack author tooling; not used during normal install/verification.
    /// </summary>
    public static string Sign(string packJson, string privateKeyPem)
    {
        using RSA rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);
        byte[] sig = rsa.SignData(Encoding.UTF8.GetBytes(packJson), s_hashAlgorithm, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(sig);
    }
}
