// RegiLattice.Core — ITweakValidator.cs
// Interface for tweak validation and dependency resolution.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides tweak integrity validation and dependency resolution.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in code that
/// needs to validate tweaks (e.g. CLI --validate, test fixtures).
/// </summary>
public interface ITweakValidator
{
    /// <summary>
    /// Validate all registered tweaks. Returns a list of error strings;
    /// empty list means no issues detected.
    /// </summary>
    IReadOnlyList<string> ValidateTweaks();

    /// <summary>
    /// Detect duplicate registry operations across all tweaks.
    /// Returns a list of warning strings describing path+name collisions.
    /// </summary>
    IReadOnlyList<string> DetectDuplicateRegistryOps();

    /// <summary>
    /// Returns the topologically-sorted dependency chain for <paramref name="tweakId"/>
    /// (prerequisites first).
    /// </summary>
    IReadOnlyList<TweakDef> ResolveDependencies(string tweakId);

    /// <summary>
    /// Returns all tweaks that depend (directly or transitively) on <paramref name="tweakId"/>.
    /// </summary>
    IReadOnlyList<TweakDef> Dependents(string tweakId);
}
