// RegiLattice.Core — Plugins/PackDef.cs
// Immutable model for a Tweak Pack (community plugin).

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Trust level reported by <see cref="PackSignatureVerifier"/> after verifying a pack.
/// </summary>
public enum PackTrustLevel
{
    /// <summary>No SHA-256 hash or signature was checked.</summary>
    None,

    /// <summary>SHA-256 hash was provided and verified against the pack content.</summary>
    HashVerified,

    /// <summary>Both SHA-256 hash and RSA-SHA256 detached signature were verified.</summary>
    Signed,
}

/// <summary>
/// Metadata for a Tweak Pack — a community-authored bundle of declarative tweaks.
/// Packs are JSON-only (no .dll execution) and installed to %LOCALAPPDATA%\RegiLattice\packs\.
/// </summary>
public sealed record PackDef
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Version { get; init; }
    public required string Author { get; init; }
    public string Description { get; init; } = "";
    public int TweakCount { get; init; }
    public IReadOnlyList<string> Categories { get; init; } = [];
    public IReadOnlyList<string> Tags { get; init; } = [];
    public string Sha256 { get; init; } = "";
    public string DownloadUrl { get; init; } = "";
    public string MinRegiLatticeVersion { get; init; } = "3.3.0";
    public int MinWindowsBuild { get; init; }
    public string Changelog { get; init; } = "";

    // ── Signature fields (T7.3) ────────────────────────────────────────

    /// <summary>
    /// URL of the detached RSA-SHA256 signature file (`.rlpack.sig`, base64-encoded).
    /// Empty string means the pack has not been signed.
    /// </summary>
    public string SignatureUrl { get; init; } = "";

    /// <summary>
    /// Trust level assigned by <see cref="PackSignatureVerifier"/> after loading.
    /// Not persisted to JSON — computed at runtime only.
    /// </summary>
    public PackTrustLevel TrustLevel { get; init; } = PackTrustLevel.None;
}

/// <summary>
/// Describes a registry key/value pair that is modified by two or more installed packs.
/// </summary>
public sealed record PackConflict(string RegistryPath, string ValueName, IReadOnlyList<string> ConflictingPacks);
