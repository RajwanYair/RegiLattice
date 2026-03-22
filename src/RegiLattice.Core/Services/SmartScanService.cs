// RegiLattice.Core — Services/SmartScanService.cs
// Smart Scan: analyses the current TweakEngine status map and generates a prioritised
// list of recommended tweaks ranked by ImpactScore × SafetyRating.

namespace RegiLattice.Core;

using RegiLattice.Core.Models;

/// <summary>A single tweak recommendation from the Smart Scan.</summary>
public sealed class ScanRecommendation
{
    /// <summary>The tweak being recommended.</summary>
    public required TweakDef Tweak { get; init; }

    /// <summary>Human-readable reason for the recommendation.</summary>
    public required string Reason { get; init; }

    /// <summary>Combined priority score: ImpactScore × SafetyRating (max 25).</summary>
    public required int PriorityScore { get; init; }

    /// <summary>True when ImpactScore ≥ 4 AND SafetyRating ≥ 4 — considered a "quick win".</summary>
    public bool IsQuickWin => Tweak.ImpactScore >= 4 && Tweak.SafetyRating >= 4;
}

/// <summary>Result returned by <see cref="SmartScanService.Scan"/>.</summary>
public sealed class ScanResult
{
    /// <summary>All recommendations, ordered by <see cref="ScanRecommendation.PriorityScore"/> descending.</summary>
    public required IReadOnlyList<ScanRecommendation> Recommendations { get; init; }

    /// <summary>Number of tweaks that qualify as "Quick Wins" (impact ≥4, safety ≥4, unapplied).</summary>
    public int QuickWinsCount => Recommendations.Count(r => r.IsQuickWin);

    /// <summary>Number of tweaks scanned for recommendations.</summary>
    public int ScannedCount { get; init; }

    /// <summary>UTC timestamp of when the scan was performed.</summary>
    public DateTime ScannedAt { get; init; } = DateTime.UtcNow;
}

/// <summary>
/// Smart Scan service — evaluates the current tweak status map and returns
/// prioritised recommendations for unapplied tweaks.
/// </summary>
public static class SmartScanService
{
    /// <summary>Maximum number of recommendations to return.</summary>
    public const int MaxRecommendations = 25;

    /// <summary>
    /// Generates a <see cref="ScanResult"/> based on the supplied engine and status map.
    /// Thread-safe — reads only; never modifies registry state.
    /// </summary>
    /// <param name="engine">A populated <see cref="TweakEngine"/>.</param>
    /// <param name="statusMap">
    /// A dictionary of tweak ID → <see cref="TweakResult"/>, typically from
    /// <c>engine.StatusMap(parallel: true)</c>.  Pass <c>null</c> to skip status filtering
    /// (all unapplied tweaks are candidates).
    /// </param>
    /// <param name="forceCorpSafe">
    /// When <c>true</c>, only <see cref="TweakDef.CorpSafe"/> tweaks are included.
    /// </param>
    public static ScanResult Scan(TweakEngine engine, IReadOnlyDictionary<string, TweakResult>? statusMap = null, bool forceCorpSafe = false)
    {
        ArgumentNullException.ThrowIfNull(engine);

        var all = engine.AllTweaks();
        int scanned = 0;

        var candidates = new List<ScanRecommendation>();

        foreach (TweakDef td in all)
        {
            // Skip tweaks with no operations (informational-only).
            if (!td.HasOperations)
                continue;

            // Skip corp-unsafe tweaks when corp guard is active.
            if (forceCorpSafe && !td.CorpSafe)
                continue;

            // Skip tweaks that are not applicable on this hardware.
            Func<bool>? isApplicable = td.IsApplicable;
            if (isApplicable is not null && !isApplicable.Invoke())
                continue;

            scanned++;

            // Only recommend tweaks that are currently NOT applied.
            if (statusMap is not null && statusMap.ContainsKey(td.Id) && statusMap[td.Id] == TweakResult.Applied)
                continue; // already applied — skip

            int priority = td.ImpactScore * td.SafetyRating;
            string reason = BuildReason(td);

            candidates.Add(
                new ScanRecommendation
                {
                    Tweak = td,
                    Reason = reason,
                    PriorityScore = priority,
                }
            );
        }

        // Sort descending by priority, then alphabetically as a tiebreaker.
        candidates.Sort(
            (a, b) =>
                b.PriorityScore != a.PriorityScore
                    ? b.PriorityScore.CompareTo(a.PriorityScore)
                    : string.Compare(a.Tweak.Label, b.Tweak.Label, StringComparison.Ordinal)
        );

        IReadOnlyList<ScanRecommendation> top = candidates.Take(MaxRecommendations).ToList();

        return new ScanResult { Recommendations = top, ScannedCount = scanned };
    }

    /// <summary>
    /// Async wrapper for <see cref="Scan"/> — runs the scan on a background thread.
    /// </summary>
    public static Task<ScanResult> ScanAsync(
        TweakEngine engine,
        IReadOnlyDictionary<string, TweakResult>? statusMap = null,
        bool forceCorpSafe = false
    ) => Task.Run(() => Scan(engine, statusMap, forceCorpSafe));

    // ── Private helpers ──────────────────────────────────────────────────────

    private static string BuildReason(TweakDef td)
    {
        // Prefer the tweak's own description if it's concise enough.
        if (!string.IsNullOrWhiteSpace(td.Description) && td.Description.Length <= 120)
            return td.Description;

        // Generate a short summary from scores and category.
        string impact = td.ImpactScore switch
        {
            5 => "High impact",
            4 => "Good impact",
            3 => "Moderate impact",
            _ => "Low impact",
        };
        string safety = td.SafetyRating switch
        {
            5 => "completely safe",
            4 => "very safe",
            3 => "generally safe",
            _ => "use with caution",
        };
        return $"{impact}, {safety} — {td.Category} tweak.";
    }
}
