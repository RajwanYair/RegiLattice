// RegiLattice.Core — Services/IFavoritesRepository.cs
// Abstraction over favorites persistence (B.3 — decouple Favorites from file I/O).

namespace RegiLattice.Core.Services;

/// <summary>
/// Provides read/write access to the user's favorite tweak IDs.
/// Decouple storage from consumers so the GUI and CLI can receive this via DI
/// and tests can use an in-memory implementation without touching the filesystem.
/// </summary>
public interface IFavoritesRepository
{
    /// <summary>Add a tweak ID to favorites (idempotent).</summary>
    void Add(string tweakId);

    /// <summary>Remove a tweak ID from favorites (idempotent).</summary>
    void Remove(string tweakId);

    /// <summary>
    /// Toggle a tweak — add if not present, remove if present.
    /// Returns <c>true</c> if the tweak is now favorited.
    /// </summary>
    bool Toggle(string tweakId);

    /// <summary>Returns <c>true</c> if <paramref name="tweakId"/> is in the favorites list.</summary>
    bool IsFavorite(string tweakId);

    /// <summary>Returns all favorite tweak IDs sorted alphabetically.</summary>
    IReadOnlyList<string> All();

    /// <summary>Total number of favorites.</summary>
    int Count { get; }

    /// <summary>Flush any pending in-memory changes to the backing store.</summary>
    void Flush();
}
