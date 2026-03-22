// RegiLattice.Core — Services/ComplianceHistory.cs
// Persists compliance check results to a JSON log so administrators can track
// registry drift over time and compare compliance across days/weeks.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core;

/// <summary>A single entry in the compliance history log.</summary>
public sealed record ComplianceHistoryEntry
{
    public required DateTime CheckedAt { get; init; }
    public required int TotalChecked { get; init; }
    public required int ViolationCount { get; init; }
    public required bool IsCompliant { get; init; }
    public required IReadOnlyList<string> DriftedTweakIds { get; init; }
    public string? SnapshotPath { get; init; }
}

/// <summary>
/// Persists <see cref="ComplianceReport"/> results to a rolling JSON log file.
/// The log is stored at <c>%LOCALAPPDATA%\RegiLattice\compliance-history.json</c>
/// (or the portable equivalent) and keeps the 90 most recent entries.
/// </summary>
public static class ComplianceHistory
{
    /// <summary>Maximum number of entries retained in the history file.</summary>
    public const int MaxEntries = 90;

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    };

    // ── Path resolution ─────────────────────────────────────────────────────

    /// <summary>Returns the path to the compliance history JSON file.</summary>
    public static string HistoryPath => Path.Combine(AppConfig.ConfigDir, "compliance-history.json");

    // ── Public API ──────────────────────────────────────────────────────────

    /// <summary>
    /// Loads all entries from the history log.
    /// Returns an empty list if the file does not exist or cannot be read.
    /// </summary>
    public static IReadOnlyList<ComplianceHistoryEntry> GetHistory()
    {
        string path = HistoryPath;
        if (!File.Exists(path))
            return [];

        try
        {
            string json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<ComplianceHistoryEntry>>(json, JsonOpts)?.AsReadOnly() ?? [];
        }
        catch (Exception ex) when (ex is IOException or JsonException or UnauthorizedAccessException)
        {
            return [];
        }
    }

    /// <summary>
    /// Returns the most recent compliance history entry, or <c>null</c> if history is empty.
    /// </summary>
    public static ComplianceHistoryEntry? GetLatest()
    {
        var history = GetHistory();
        return history.Count > 0 ? history[^1] : null;
    }

    /// <summary>
    /// Appends a <see cref="ComplianceReport"/> to the history log.
    /// The log is capped at <see cref="MaxEntries"/> — the oldest entries are trimmed.
    /// </summary>
    public static void AddEntry(ComplianceReport report, string? snapshotPath = null)
    {
        ArgumentNullException.ThrowIfNull(report);

        var entry = new ComplianceHistoryEntry
        {
            CheckedAt = report.CheckedAt,
            TotalChecked = report.TotalChecked,
            ViolationCount = report.ViolationCount,
            IsCompliant = report.IsCompliant,
            DriftedTweakIds = report.Drifted.Select(d => d.TweakId).ToList().AsReadOnly(),
            SnapshotPath = snapshotPath,
        };

        // Load existing, append, trim, save
        var existing = GetHistory().ToList();
        existing.Add(entry);
        if (existing.Count > MaxEntries)
            existing = existing.Skip(existing.Count - MaxEntries).ToList();

        Flush(existing);
    }

    /// <summary>Clears all entries from the history log.</summary>
    public static void Clear()
    {
        string path = HistoryPath;
        if (File.Exists(path))
        {
            try
            {
                File.Delete(path);
            }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
            {
                // Best-effort: overwrite with empty array
                Flush([]);
            }
        }
    }

    /// <summary>Returns how many violations occurred in the last N checks.</summary>
    public static int TotalViolationsInLast(int count)
    {
        var history = GetHistory();
        return history.Skip(Math.Max(0, history.Count - count)).Sum(e => e.ViolationCount);
    }

    // ── Internal helpers ────────────────────────────────────────────────────

    private static void Flush(IReadOnlyList<ComplianceHistoryEntry> entries)
    {
        string path = HistoryPath;
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, JsonSerializer.Serialize(entries, JsonOpts));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            // Non-critical — silently skip if the log cannot be written
            _ = ex;
        }
    }
}
