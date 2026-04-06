// RegiLattice.Core — Services/ConfirmApplyThreshold.cs
// Phase 2.3: Pure-logic helper that decides whether a tweak requires a confirmation
// dialog before it is applied.  Lives in Core so that it can be tested without any
// WinForms dependency.

#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// Determines whether a tweak requires an explicit user confirmation before it is applied.
/// Confirmation is triggered when the safety rating is low or the effective risk flags
/// include destructive or high-impact operations.
/// </summary>
public static class ConfirmApplyThreshold
{
    /// <summary>
    /// Tweaks with a safety rating at or below this value always require confirmation.
    /// </summary>
    public const int SafetyRatingThreshold = 3;

    /// <summary>
    /// Any of these risk flags in <see cref="TweakDef.EffectiveRiskFlags"/> trigger confirmation
    /// regardless of <see cref="TweakDef.SafetyRating"/>.
    /// </summary>
    public const TweakRisk ConfirmationFlags =
        TweakRisk.DeletesKey
        | TweakRisk.RequiresReboot
        | TweakRisk.AffectsSecurity
        | TweakRisk.PotentialDataLoss;

    /// <summary>
    /// Returns <c>true</c> when the tweak should be shown to the user in a risk-confirmation
    /// dialog before applying.
    /// </summary>
    /// <param name="td">The tweak to evaluate.</param>
    /// <returns>
    /// <c>true</c> if <paramref name="td"/>'s <see cref="TweakDef.SafetyRating"/> is ≤ 3,
    /// or if <see cref="TweakDef.EffectiveRiskFlags"/> includes any of
    /// <see cref="ConfirmationFlags"/>; <c>false</c> otherwise.
    /// </returns>
    public static bool ShouldConfirm(TweakDef td)
    {
        if (td.SafetyRating <= SafetyRatingThreshold)
            return true;
        return (td.EffectiveRiskFlags & ConfirmationFlags) != TweakRisk.None;
    }
}
