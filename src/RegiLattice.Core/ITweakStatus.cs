// RegiLattice.Core — ITweakStatus.cs
// Interface for tweak status detection.
// Part of the ITweakEngine interface suite (B.1 — interface segregation).

using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>
/// Provides tweak status detection and hardware applicability checks.
/// Implemented by <see cref="TweakEngine"/>; consume this interface in code that
/// only needs to read current state without making changes (e.g. status badges, CLI status).
/// </summary>
public interface ITweakStatus
{
    /// <summary>
    /// Detect whether <paramref name="td"/> is currently applied, not applied, or unknown.
    /// </summary>
    TweakResult DetectStatus(TweakDef td);

    /// <summary>
    /// Detect status for a set of tweaks. Returns a per-ID result map.
    /// </summary>
    Dictionary<string, TweakResult> StatusMap(bool parallel = false, IEnumerable<string>? ids = null, CancellationToken ct = default);
}
