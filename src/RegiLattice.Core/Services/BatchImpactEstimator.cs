// RegiLattice.Core — Services/BatchImpactEstimator.cs
// Estimates the combined impact of applying a set of tweaks, including
// dependency-chain resolution and per-category breakdown.

namespace RegiLattice.Core.Services;

using System.Collections.Generic;
using RegiLattice.Core.Models;

/// <summary>
/// Tiered overall impact label for a batch of tweaks.
/// Derived from the weighted average of <see cref="TweakDef.ImpactScore"/> values.
/// </summary>
public enum ImpactTier
{
    /// <summary>Average ImpactScore &lt; 1.5 — negligible benefit.</summary>
    Minimal,

    /// <summary>Average ImpactScore 1.5–2.5 — minor improvements.</summary>
    Low,

    /// <summary>Average ImpactScore 2.5–3.5 — noticeable improvements.</summary>
    Moderate,

    /// <summary>Average ImpactScore 3.5–4.5 — significant improvements.</summary>
    High,

    /// <summary>Average ImpactScore ≥ 4.5 — transformative improvements.</summary>
    Significant,
}

/// <summary>
/// Immutable summary produced by <see cref="BatchImpactEstimator.Estimate"/>.
/// </summary>
public sealed record BatchImpactSummary
{
    /// <summary>Total number of tweaks in the batch (after dependency expansion).</summary>
    public required int TweakCount { get; init; }

    /// <summary>Number of tweaks that qualify as Quick Wins (ImpactScore ≥4, SafetyRating ≥4).</summary>
    public required int QuickWinCount { get; init; }

    /// <summary>Weighted average <see cref="TweakDef.ImpactScore"/> across all tweaks (1–5 scale).</summary>
    public required double AverageImpact { get; init; }

    /// <summary>Weighted average <see cref="TweakDef.SafetyRating"/> across all tweaks (1–5 scale).</summary>
    public required double AverageSafety { get; init; }

    /// <summary>Overall impact tier derived from <see cref="AverageImpact"/>.</summary>
    public required ImpactTier OverallTier { get; init; }

    /// <summary>
    /// Top user-facing benefit notes, drawn from the highest-scoring tweaks that have
    /// a non-empty <see cref="TweakDef.ImpactNote"/>. At most 5 entries.
    /// </summary>
    public required IReadOnlyList<string> TopBenefits { get; init; }

    /// <summary>
    /// Number of distinct registry key+value operations across all tweaks.
    /// Gives a concrete sense of "how many registry changes this batch makes".
    /// </summary>
    public required int UniqueRegistryOps { get; init; }

    /// <summary>
    /// Count of tweaks broken down by their primary category.
    /// Key = category name, Value = tweak count.
    /// </summary>
    public required IReadOnlyDictionary<string, int> ByCategory { get; init; }

    /// <summary>Human-readable one-liner summary of the overall tier and count.</summary>
    public string OneLiner =>
        $"{TweakCount} tweak{(TweakCount == 1 ? "" : "s")} — {OverallTier} impact "
        + $"(avg {AverageImpact:F1}/5, safety {AverageSafety:F1}/5, {QuickWinCount} quick win{(QuickWinCount == 1 ? "" : "s")})";
}

/// <summary>
/// Computes a combined impact estimate for a set of tweaks.
/// Optionally expands dependency chains so that dependent tweaks are included in the count.
/// Thread-safe: all public methods are read-only and side-effect free.
/// </summary>
public static class BatchImpactEstimator
{
    /// <summary>
    /// Estimates the combined impact of applying <paramref name="tweaks"/>.
    /// </summary>
    /// <param name="tweaks">Tweaks to analyse. Duplicates are collapsed by ID.</param>
    /// <param name="engine">
    /// Optional engine used to expand <see cref="TweakDef.DependsOn"/> chains.
    /// Pass <c>null</c> to skip dependency resolution.
    /// </param>
    /// <returns>An immutable <see cref="BatchImpactSummary"/>.</returns>
    public static BatchImpactSummary Estimate(IEnumerable<TweakDef> tweaks, TweakEngine? engine = null)
    {
        ArgumentNullException.ThrowIfNull(tweaks);

        // Deduplicate by ID and optionally expand dependency chains.
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var resolved = new List<TweakDef>();

        foreach (TweakDef td in tweaks)
        {
            if (!seen.Add(td.Id))
                continue;
            resolved.Add(td);

            if (engine is not null && td.DependsOn.Count > 0)
                ExpandDependencies(td, engine, seen, resolved);
        }

        if (resolved.Count == 0)
        {
            return new BatchImpactSummary
            {
                TweakCount = 0,
                QuickWinCount = 0,
                AverageImpact = 0,
                AverageSafety = 0,
                OverallTier = ImpactTier.Minimal,
                TopBenefits = [],
                UniqueRegistryOps = 0,
                ByCategory = new Dictionary<string, int>(),
            };
        }

        double totalImpact = 0;
        double totalSafety = 0;
        int quickWins = 0;
        var categoryCount = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        var registryOps = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        // Collect high-scoring tweaks with non-empty ImpactNotes for TopBenefits.
        var benefitCandidates = new List<(int Score, string Note)>();

        foreach (TweakDef td in resolved)
        {
            totalImpact += td.ImpactScore;
            totalSafety += td.SafetyRating;

            if (td.ImpactScore >= 4 && td.SafetyRating >= 4)
                quickWins++;

            categoryCount[td.Category] = categoryCount.GetValueOrDefault(td.Category) + 1;

            // Collect distinct registry key+name pairs.
            foreach (RegOp op in td.ApplyOps)
            {
                if (op.Kind == RegOpKind.SetValue)
                    registryOps.Add($"{op.Path}\\{op.Name}");
            }

            if (!string.IsNullOrWhiteSpace(td.ImpactNote))
                benefitCandidates.Add((td.ImpactScore, td.ImpactNote));
        }

        int count = resolved.Count;
        double avgImpact = totalImpact / count;
        double avgSafety = totalSafety / count;

        ImpactTier tier = avgImpact switch
        {
            < 1.5 => ImpactTier.Minimal,
            < 2.5 => ImpactTier.Low,
            < 3.5 => ImpactTier.Moderate,
            < 4.5 => ImpactTier.High,
            _ => ImpactTier.Significant,
        };

        // Top 5 benefits from highest-scoring tweaks.
        var topBenefits = benefitCandidates
            .OrderByDescending(b => b.Score)
            .Select(b => b.Note)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(5)
            .ToList();

        return new BatchImpactSummary
        {
            TweakCount = count,
            QuickWinCount = quickWins,
            AverageImpact = Math.Round(avgImpact, 2),
            AverageSafety = Math.Round(avgSafety, 2),
            OverallTier = tier,
            TopBenefits = topBenefits,
            UniqueRegistryOps = registryOps.Count,
            ByCategory = categoryCount,
        };
    }

    /// <summary>
    /// Renders a concise multi-line impact report suitable for CLI or log output.
    /// </summary>
    public static string FormatReport(BatchImpactSummary summary)
    {
        ArgumentNullException.ThrowIfNull(summary);

        var sb = new System.Text.StringBuilder();
        sb.AppendLine($"Impact Estimate: {summary.TweakCount} tweak{(summary.TweakCount == 1 ? "" : "s")}");
        sb.AppendLine($"  Overall tier : {summary.OverallTier}");
        sb.AppendLine($"  Avg impact   : {summary.AverageImpact:F1} / 5  {ImpactStars(summary.AverageImpact)}");
        sb.AppendLine($"  Avg safety   : {summary.AverageSafety:F1} / 5  {SafetyStars(summary.AverageSafety)}");
        sb.AppendLine($"  Quick wins   : {summary.QuickWinCount}");
        sb.AppendLine($"  Registry ops : {summary.UniqueRegistryOps} unique key+value changes");

        if (summary.ByCategory.Count > 0)
        {
            sb.AppendLine("  Categories   :");
            foreach (var (cat, cnt) in summary.ByCategory.OrderByDescending(kv => kv.Value))
                sb.AppendLine($"    {cat,-28} {cnt}");
        }

        if (summary.TopBenefits.Count > 0)
        {
            sb.AppendLine("  Top benefits :");
            foreach (string note in summary.TopBenefits)
                sb.AppendLine($"    \u2022 {note}");
        }

        return sb.ToString().TrimEnd();
    }

    // ── Helpers ─────────────────────────────────────────────────────────

    private static void ExpandDependencies(TweakDef root, TweakEngine engine, HashSet<string> seen, List<TweakDef> resolved)
    {
        foreach (string depId in root.DependsOn)
        {
            if (!seen.Add(depId))
                continue;
            TweakDef? dep = engine.GetTweak(depId);
            if (dep is null)
                continue;
            resolved.Add(dep);
            if (dep.DependsOn.Count > 0)
                ExpandDependencies(dep, engine, seen, resolved);
        }
    }

    private static string ImpactStars(double score)
    {
        int filled = (int)Math.Round(score);
        return new string('\u2605', Math.Clamp(filled, 0, 5)) + new string('\u2606', Math.Clamp(5 - filled, 0, 5));
    }

    private static string SafetyStars(double score)
    {
        int filled = (int)Math.Round(score);
        return new string('\u2605', Math.Clamp(filled, 0, 5)) + new string('\u2606', Math.Clamp(5 - filled, 0, 5));
    }
}
