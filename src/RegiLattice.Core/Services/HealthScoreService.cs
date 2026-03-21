// RegiLattice.Core — Services/HealthScoreService.cs
// Computes a 4-dimension system-health score from the current tweak state.
// Scores are cached per engine instance; call Recalculate() after applying tweaks.

#nullable enable

using RegiLattice.Core.Models;

namespace RegiLattice.Core.Services;

/// <summary>
/// Immutable snapshot of the four health sub-scores and an overall composite.
/// All values are 0–100 (percentage).
/// </summary>
/// <param name="Privacy">
/// Fraction of privacy-oriented tweaks that are applied, weighted by
/// <see cref="TweakDef.ImpactScore"/>.
/// </param>
/// <param name="Performance">
/// Fraction of performance-oriented tweaks that are applied.
/// </param>
/// <param name="Security">
/// Fraction of security / hardening tweaks that are applied.
/// </param>
/// <param name="Stability">
/// Fraction of stability / crash-prevention tweaks that are applied.
/// </param>
/// <param name="Overall">
/// Weighted average of all four sub-scores.
/// </param>
public sealed record HealthScore(int Privacy, int Performance, int Security, int Stability, int Overall)
{
    /// <summary>Returns a short human-readable label for the overall score.</summary>
    public string OverallLabel =>
        Overall switch
        {
            >= 85 => "Excellent",
            >= 65 => "Good",
            >= 40 => "Fair",
            >= 20 => "Needs Work",
            _ => "Poor",
        };
}

/// <summary>
/// Computes <see cref="HealthScore"/> for a given <see cref="TweakEngine"/>
/// by bucketing tweaks into four categories and measuring what percentage of
/// each bucket is currently applied.
/// </summary>
public sealed class HealthScoreService
{
    private readonly TweakEngine _engine;

    public HealthScoreService(TweakEngine engine) => _engine = engine ?? throw new ArgumentNullException(nameof(engine));

    // ── Bucket definitions ───────────────────────────────────────────────

    // Tag or category keywords that route a tweak into each bucket.
    // Tags are checked first (exact token match, case-insensitive);
    // then the category name substring is checked.
    private static readonly string[] _privacyTokens =
    [
        "privacy",
        "priv",
        "telemetry",
        "telem",
        "tracking",
        "diagnostics",
        "advertising",
        "cortana",
        "recall",
        "windows recall",
        "ai copilot",
    ];

    private static readonly string[] _perfTokens =
    [
        "performance",
        "perf",
        "gaming",
        "game",
        "gpu",
        "fps",
        "memory",
        "mem",
        "ram",
        "storage",
        "ssd",
        "boot",
        "startup",
        "network optimization",
        "netopt",
        "speed",
    ];

    private static readonly string[] _securityTokens =
    [
        "security",
        "sec",
        "hardening",
        "harden",
        "firewall",
        "fw",
        "encryption",
        "enc",
        "uac",
        "smart app control",
        "sac",
        "group policy",
        "gpo",
        "bitlocker",
        "defender",
    ];

    private static readonly string[] _stabilityTokens =
    [
        "crash",
        "recovery",
        "backup",
        "restore",
        "maintenance",
        "maint",
        "stability",
        "startup",
        "boot",
        "event log",
        "evtlog",
        "scheduled tasks",
        "schtask",
    ];

    // ── Public API ───────────────────────────────────────────────────────

    /// <summary>
    /// Computes the <see cref="HealthScore"/> synchronously using the provided
    /// pre-computed status map (to avoid blocking calls to the registry).
    /// </summary>
    /// <param name="statusMap">
    /// The output of <see cref="TweakEngine.StatusMap"/>.
    /// Pass an empty dictionary for an instant preview with all tweaks assumed
    /// <see cref="TweakResult.Unknown"/>.
    /// </param>
    public HealthScore Compute(IReadOnlyDictionary<string, TweakResult> statusMap)
    {
        var all = _engine.AllTweaks();

        int privacy = ScoreBucket(all, statusMap, _privacyTokens);
        int performance = ScoreBucket(all, statusMap, _perfTokens);
        int security = ScoreBucket(all, statusMap, _securityTokens);
        int stability = ScoreBucket(all, statusMap, _stabilityTokens);

        // Overall is the unweighted average of the four sub-scores.
        int overall = (privacy + performance + security + stability) / 4;

        return new HealthScore(privacy, performance, security, stability, overall);
    }

    /// <summary>
    /// Async convenience: calls <see cref="TweakEngine.StatusMap"/> in a
    /// background thread, then computes the health score.
    /// </summary>
    public async Task<HealthScore> ComputeAsync()
    {
        var statusMap = await Task.Run(() => _engine.StatusMap(parallel: true)).ConfigureAwait(false);
        return Compute(statusMap);
    }

    // ── Internal helpers ─────────────────────────────────────────────────

    /// <summary>
    /// Scores a single health dimension bucket.
    /// Returns 0–100.
    /// </summary>
    private static int ScoreBucket(IReadOnlyList<TweakDef> all, IReadOnlyDictionary<string, TweakResult> statusMap, string[] bucketTokens)
    {
        int totalWeight = 0;
        int appliedWeight = 0;

        foreach (var td in all)
        {
            if (!IsInBucket(td, bucketTokens))
                continue;

            int weight = td.ImpactScore; // 1–5
            totalWeight += weight;

            if (statusMap.TryGetValue(td.Id, out var result) && result == TweakResult.Applied)
                appliedWeight += weight;
        }

        if (totalWeight == 0)
            return 0;

        return (int)Math.Round(appliedWeight * 100.0 / totalWeight);
    }

    /// <summary>
    /// Returns <see langword="true"/> when the tweak belongs to the given bucket,
    /// determined by matching any of the bucket tokens against the tweak's tags
    /// or its category name (case-insensitive substring).
    /// </summary>
    private static bool IsInBucket(TweakDef td, string[] bucketTokens)
    {
        foreach (var token in bucketTokens)
        {
            // Check tags first (exact token match)
            foreach (var tag in td.Tags)
            {
                if (tag.Contains(token, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            // Check category substring
            if (td.Category.Contains(token, StringComparison.OrdinalIgnoreCase))
                return true;
        }
        return false;
    }

    // ── Bucket inspection helpers (for UI / test diagnostics) ────────────

    /// <summary>Returns all Privacy-bucket tweaks.</summary>
    public IReadOnlyList<TweakDef> PrivacyTweaks() => Filter(_privacyTokens);

    /// <summary>Returns all Performance-bucket tweaks.</summary>
    public IReadOnlyList<TweakDef> PerformanceTweaks() => Filter(_perfTokens);

    /// <summary>Returns all Security-bucket tweaks.</summary>
    public IReadOnlyList<TweakDef> SecurityTweaks() => Filter(_securityTokens);

    /// <summary>Returns all Stability-bucket tweaks.</summary>
    public IReadOnlyList<TweakDef> StabilityTweaks() => Filter(_stabilityTokens);

    private IReadOnlyList<TweakDef> Filter(string[] tokens) => _engine.AllTweaks().Where(td => IsInBucket(td, tokens)).ToList();

    // ── Score preview ────────────────────────────────────────────────────

    /// <summary>
    /// Computes the score delta that would result from applying ALL currently
    /// unapplied tweaks in <paramref name="category"/>.
    /// Returns <c>(Before, After)</c> so callers can compute deltas per dimension.
    /// If the category is empty or all its tweaks are already applied, both
    /// scores will be identical.
    /// </summary>
    public (HealthScore Before, HealthScore After) PreviewCategoryImpact(string category, IReadOnlyDictionary<string, TweakResult> currentStatusMap)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentNullException.ThrowIfNull(currentStatusMap);

        var before = Compute(currentStatusMap);

        // Build a simulated map where all tweaks in the category are Applied.
        var simulated = new Dictionary<string, TweakResult>(currentStatusMap, StringComparer.OrdinalIgnoreCase);
        if (_engine.TweaksByCategory().TryGetValue(category, out var tweaksInCat))
            foreach (var td in tweaksInCat)
                simulated[td.Id] = TweakResult.Applied;

        var after = Compute(simulated);
        return (before, after);
    }
}
