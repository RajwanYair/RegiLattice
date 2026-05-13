// RegiLattice.Core — ITweakExecutor.cs
// Interface for applying, removing, and updating tweaks.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides apply, remove, update, and batch execution of tweaks.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in code that
/// needs to make registry changes (e.g. MainForm apply buttons, CLI apply/remove).
/// </summary>
public interface ITweakExecutor
{
    /// <summary>
    /// Apply a single tweak. Respects <see cref="TweakDef.NeedsAdmin"/>, CorporateGuard,
    /// MinBuild guard, and hardware applicability.
    /// </summary>
    TweakResult Apply(TweakDef td, bool requireAdmin = true, bool forceCorp = false);

    /// <summary>
    /// Remove / revert a single tweak.
    /// </summary>
    TweakResult Remove(TweakDef td, bool requireAdmin = true, bool forceCorp = false);

    /// <summary>
    /// Update a tweak (runs <see cref="TweakDef.UpdateAction"/> or falls back to Apply).
    /// </summary>
    TweakResult Update(TweakDef td, bool requireAdmin = true, bool forceCorp = false);

    /// <summary>
    /// Apply a batch of tweaks. Returns a per-ID result map.
    /// </summary>
    Dictionary<string, TweakResult> ApplyBatch(IEnumerable<TweakDef> tweaks, bool forceCorp = false, bool parallel = false);

    /// <summary>
    /// Remove a batch of tweaks. Returns a per-ID result map.
    /// </summary>
    Dictionary<string, TweakResult> RemoveBatch(IEnumerable<TweakDef> tweaks, bool forceCorp = false, bool parallel = false);
}
