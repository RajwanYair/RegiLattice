// RegiLattice.Core — Plugins/PackIndex.cs
// Model for the remote marketplace index (index.json hosted on GitHub).

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core.Plugins;

/// <summary>
/// Per-author signing key registered in the marketplace index.
/// The public key is stored as a PEM-encoded SubjectPublicKeyInfo string.
/// </summary>
public sealed record AuthorKey
{
    [JsonPropertyName("author")]
    public string Author { get; init; } = "";

    [JsonPropertyName("publicKeyPem")]
    public string PublicKeyPem { get; init; } = "";
}

/// <summary>
/// Represents the marketplace index fetched from the GitHub repository.
/// Contains metadata for all available packs without their full tweak definitions.
/// </summary>
public sealed class PackIndex
{
    [JsonPropertyName("version")]
    public int Version { get; init; } = 1;

    [JsonPropertyName("lastUpdated")]
    public DateTime LastUpdated { get; init; }

    [JsonPropertyName("packs")]
    public IReadOnlyList<PackDef> Packs { get; init; } = [];

    /// <summary>
    /// RSA-SHA256 public keys for pack authors who have registered signing credentials.
    /// Keyed by author name (case-insensitive). Used by <see cref="PackSignatureVerifier"/>.
    /// </summary>
    [JsonPropertyName("authorKeys")]
    public IReadOnlyList<AuthorKey> AuthorKeys { get; init; } = [];

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private static readonly JsonSerializerOptions s_jsonWriteOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    /// <summary>Deserialise an index.json string into a PackIndex.</summary>
    public static PackIndex? FromJson(string json)
    {
        return JsonSerializer.Deserialize<PackIndex>(json, s_jsonOptions);
    }

    /// <summary>Serialise this index to a JSON string.</summary>
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, s_jsonWriteOptions);
    }

    /// <summary>
    /// Look up the registered public key PEM for the given author name.
    /// Returns <see langword="null"/> if no key is registered.
    /// </summary>
    public string? GetAuthorPublicKey(string author)
    {
        foreach (AuthorKey ak in AuthorKeys)
        {
            if (string.Equals(ak.Author, author, StringComparison.OrdinalIgnoreCase))
                return ak.PublicKeyPem;
        }

        return null;
    }
}
