// RegiLattice.Core — Services/ComplianceService.cs
// Detects registry drift: compares current tweak status against a saved baseline (Phase 9 #90).

namespace RegiLattice.Core;

using RegiLattice.Core.Models;

/// <summary>Represents a single tweak that has drifted from its expected state.</summary>
public sealed record ComplianceDrift
{
    public required string TweakId { get; init; }
    public required string Label { get; init; }
    public required string Category { get; init; }
    public required TweakResult BaselineStatus { get; init; }
    public required TweakResult CurrentStatus { get; init; }

    public bool IsViolation => BaselineStatus == TweakResult.Applied && CurrentStatus != TweakResult.Applied;
}

/// <summary>Summary of a compliance check run.</summary>
public sealed record ComplianceReport
{
    public required IReadOnlyList<ComplianceDrift> Drifted { get; init; }
    public required int TotalChecked { get; init; }
    public required DateTime CheckedAt { get; init; }
    public bool IsCompliant => Drifted.Count == 0;

    public int ViolationCount => Drifted.Count(d => d.IsViolation);
}

/// <summary>
/// Compares the current registry state of tweaks against a previously saved snapshot baseline.
/// The snapshot is a <see cref="Dictionary{String,String}"/> as produced by <see cref="SnapshotManager"/>.
/// </summary>
public static class ComplianceService
{
    /// <summary>
    /// Runs a compliance check using <paramref name="engine"/> against the provided
    /// <paramref name="baseline"/> snapshot dictionary (id → status string).
    /// Returns a report of any tweaks that were Applied in baseline but are no longer Applied.
    /// </summary>
    public static ComplianceReport Check(TweakEngine engine, Dictionary<string, string> baseline, bool parallelDetect = true)
    {
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentNullException.ThrowIfNull(baseline);

        // Only care about tweaks that were Applied in the baseline
        var baselineApplied = baseline
            .Where(kv => string.Equals(kv.Value, "applied", StringComparison.OrdinalIgnoreCase))
            .Select(kv => kv.Key)
            .ToList();

        if (baselineApplied.Count == 0)
            return new ComplianceReport
            {
                Drifted = [],
                TotalChecked = 0,
                CheckedAt = DateTime.UtcNow,
            };

        // Detect current status for each baseline-applied tweak
        var currentMap = engine.StatusMap(parallel: parallelDetect, ids: baselineApplied);

        var drifted = new List<ComplianceDrift>();
        foreach (var id in baselineApplied)
        {
            if (!currentMap.TryGetValue(id, out var current))
                continue;

            if (current != TweakResult.Applied)
            {
                var td = engine.GetTweak(id);
                if (td is null)
                    continue;

                drifted.Add(
                    new ComplianceDrift
                    {
                        TweakId = id,
                        Label = td.Label,
                        Category = td.Category,
                        BaselineStatus = TweakResult.Applied,
                        CurrentStatus = current,
                    }
                );
            }
        }

        return new ComplianceReport
        {
            Drifted = drifted.AsReadOnly(),
            TotalChecked = baselineApplied.Count,
            CheckedAt = DateTime.UtcNow,
        };
    }

    /// <summary>
    /// Loads the snapshot from <paramref name="snapshotPath"/> and runs a compliance check.
    /// Returns a report with <c>TotalChecked = -1</c> when the file cannot be loaded.
    /// </summary>
    public static ComplianceReport CheckFromFile(TweakEngine engine, string snapshotPath, bool parallelDetect = true)
    {
        ArgumentNullException.ThrowIfNull(engine);
        ArgumentException.ThrowIfNullOrWhiteSpace(snapshotPath);

        try
        {
            var snap = SnapshotManager.Load(snapshotPath);
            return Check(engine, snap, parallelDetect);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
        {
            return new ComplianceReport
            {
                Drifted = [],
                TotalChecked = -1,
                CheckedAt = DateTime.UtcNow,
            };
        }
    }
}
