// RegiLattice.Core — Services/IRatingsRepository.cs
// Abstraction over ratings persistence (B.3 — decouple Ratings from file I/O).

namespace RegiLattice.Core.Services;

/// <summary>
/// Provides read/write access to user-submitted tweak star-ratings.
/// Decouple storage from consumers so the GUI and CLI can receive this via DI
/// and tests can use an in-memory implementation without touching the filesystem.
/// </summary>
public interface IRatingsRepository
{
    /// <summary>
    /// Record a star rating (1–5) for a tweak.
    /// Replaces any existing rating for the same <paramref name="tweakId"/>.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="stars"/> is outside the 1–5 range.
    /// </exception>
    void Rate(string tweakId, int stars, string note = "");

    /// <summary>
    /// Returns the rating for <paramref name="tweakId"/>, or <c>null</c> if none has been recorded.
    /// </summary>
    TweakRating? GetRating(string tweakId);

    /// <summary>Returns all recorded ratings keyed by tweak ID.</summary>
    IReadOnlyDictionary<string, TweakRating> AllRatings();

    /// <summary>Remove the rating for <paramref name="tweakId"/> (no-op if not present).</summary>
    void RemoveRating(string tweakId);

    /// <summary>Returns up to <paramref name="n"/> tweaks ordered by highest rating descending.</summary>
    IReadOnlyList<(string Id, TweakRating Rating)> TopRated(int n = 10);

    /// <summary>
    /// Returns the mean star value across all rated tweaks,
    /// or <c>null</c> when no tweaks have been rated yet.
    /// </summary>
    double? AverageRating();

    /// <summary>Flush any pending in-memory changes to the backing store.</summary>
    void Flush();
}
