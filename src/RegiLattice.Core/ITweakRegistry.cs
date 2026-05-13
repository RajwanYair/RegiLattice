// RegiLattice.Core — ITweakRegistry.cs
// Interface for tweak registration and enumeration.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides tweak registration, enumeration, and lookup operations.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in
/// code that only needs to read or register tweaks.
/// </summary>
public interface ITweakRegistry
{
    /// <summary>Total number of registered tweaks.</summary>
    int TweakCount { get; }

    /// <summary>Total number of distinct categories.</summary>
    int CategoryCount { get; }

    /// <summary>Register a batch of tweaks from a module.</summary>
    void Register(IEnumerable<TweakDef> tweaks);

    /// <summary>Register all built-in tweak modules discovered via <c>[TweakModule]</c> or namespace scan.</summary>
    void RegisterBuiltins();

    /// <summary>All registered tweaks in registration order.</summary>
    IReadOnlyList<TweakDef> AllTweaks();

    /// <summary>Look up a tweak by its unique ID (case-insensitive). Returns <c>null</c> if not found.</summary>
    TweakDef? GetTweak(string id);

    /// <summary>Sorted list of distinct category names.</summary>
    IReadOnlyList<string> Categories();

    /// <summary>Map of category name → tweaks in that category.</summary>
    IReadOnlyDictionary<string, IReadOnlyList<TweakDef>> TweaksByCategory();

    /// <summary>Returns tweaks for the given IDs (skips missing IDs).</summary>
    IReadOnlyList<TweakDef> TweaksByIds(IEnumerable<string> ids);

    /// <summary>Returns all tweaks tagged with <paramref name="tag"/>.</summary>
    IReadOnlyList<TweakDef> TweaksByTag(string tag);

    /// <summary>Returns all tweaks in <paramref name="scope"/>.</summary>
    IReadOnlyList<TweakDef> TweaksByScope(TweakScope scope);

    /// <summary>Returns the resolved <see cref="TweakScope"/> for a tweak definition.</summary>
    TweakScope GetScope(TweakDef td);

    /// <summary>Per-category tweak counts.</summary>
    Dictionary<string, int> CategoryCounts();

    /// <summary>Per-scope tweak counts.</summary>
    Dictionary<TweakScope, int> ScopeCounts();
}
