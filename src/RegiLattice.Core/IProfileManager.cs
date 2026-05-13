// RegiLattice.Core — IProfileManager.cs
// Interface for profile management.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides access to built-in profiles and profile-driven batch apply.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in code that
/// needs profile selection (e.g. profile dropdown, CLI --profile).
/// </summary>
public interface IProfileManager
{
    /// <summary>All 5 built-in profiles.</summary>
    IReadOnlyList<ProfileDef> Profiles { get; }

    /// <summary>Look up a profile by name (case-insensitive). Returns <c>null</c> if not found.</summary>
    ProfileDef? GetProfile(string name);

    /// <summary>Returns tweaks that belong to the named profile's categories.</summary>
    IReadOnlyList<TweakDef> TweaksForProfile(string name);

    /// <summary>Apply all tweaks for the named profile. Returns a per-ID result map.</summary>
    Dictionary<string, TweakResult> ApplyProfile(string name, bool forceCorp = false, bool parallel = false);
}
