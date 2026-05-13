// RegiLattice.Core — ITweakSearch.cs
// Interface for tweak search and filtering.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides full-text search and multi-criteria filtering over registered tweaks.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in code that
/// only needs to query tweaks (e.g. search bars, CLI --search).
/// </summary>
public interface ITweakSearch
{
    /// <summary>
    /// Full-text search with synonym expansion over all tweaks.
    /// Returns tweaks ordered by relevance score (highest first).
    /// </summary>
    IReadOnlyList<TweakDef> Search(string query, CancellationToken ct = default);

    /// <summary>
    /// Full-text search returning tweaks with their relevance score, ordered descending.
    /// </summary>
    IReadOnlyList<(TweakDef Tweak, int Score)> SearchRanked(string query, CancellationToken ct = default);

    /// <summary>
    /// Multi-criteria filter. All non-null parameters are ANDed together.
    /// </summary>
    IReadOnlyList<TweakDef> Filter(
        bool? corpSafe = null,
        bool? needsAdmin = null,
        TweakScope? scope = null,
        string? category = null,
        int? minBuild = null,
        string? query = null
    );
}
