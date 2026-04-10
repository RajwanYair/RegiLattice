// RegiLattice.Core — Services/ScheduledTweakService.cs
// Sprint 50: Persists tweak schedules and provides CRUD for scheduled tweak execution.
// Phase 6.4: Extended with ScheduleAction, additional triggers, and query helpers.

#nullable enable

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RegiLattice.Core.Services;

/// <summary>Action to perform when a scheduled trigger fires.</summary>
public enum ScheduleAction
{
    /// <summary>Apply the tweak (default).</summary>
    Apply,

    /// <summary>Remove/revert the tweak.</summary>
    Remove,

    /// <summary>Detect-only: check current status without writing.</summary>
    Detect,
}

/// <summary>Trigger types for scheduled tweaks.</summary>
public enum ScheduleTrigger
{
    /// <summary>Apply on every system boot (via startup registry key).</summary>
    OnBoot,

    /// <summary>Apply on user login (HKCU Run key).</summary>
    OnLogin,

    /// <summary>Apply on a repeating timer interval.</summary>
    Timer,

    /// <summary>Apply on system startup (Windows Task Scheduler OnStartup; functionally equivalent to <see cref="OnBoot"/>).</summary>
    OnStartup,

    /// <summary>Apply when the user logs out of the session.</summary>
    OnLogout,

    /// <summary>Apply on system shutdown.</summary>
    OnShutdown,

    /// <summary>Apply once every 24 hours (set <see cref="TweakSchedule.IntervalMinutes"/> = 1440).</summary>
    Daily,

    /// <summary>Apply once every 7 days (set <see cref="TweakSchedule.IntervalMinutes"/> = 10080).</summary>
    Weekly,

    /// <summary>Apply when a network adapter changes state (connect/disconnect).</summary>
    OnNetworkChange,

    /// <summary>Apply when the system switches between AC power and battery.</summary>
    OnPowerChange,
}

/// <summary>Immutable record representing one scheduled tweak entry.</summary>
public sealed record TweakSchedule
{
    [JsonPropertyName("tweakId")]
    public required string TweakId { get; init; }

    [JsonPropertyName("trigger")]
    public required ScheduleTrigger Trigger { get; init; }

    [JsonPropertyName("intervalMinutes")]
    public int IntervalMinutes { get; init; }

    [JsonPropertyName("lastRun")]
    public DateTime? LastRun { get; init; }

    [JsonPropertyName("enabled")]
    public bool Enabled { get; init; } = true;

    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>Action to perform when this trigger fires. Defaults to <see cref="ScheduleAction.Apply"/>.</summary>
    [JsonPropertyName("action")]
    public ScheduleAction Action { get; init; } = ScheduleAction.Apply;
}

/// <summary>
/// Manages a persistent collection of scheduled tweak entries stored in
/// <c>%LOCALAPPDATA%\RegiLattice\scheduled-tweaks.json</c>.
/// Thread-safety: single-threaded (call from UI/CLI thread only).
/// </summary>
public sealed class ScheduledTweakService
{
    private static readonly string SchedulesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "RegiLattice",
        "scheduled-tweaks.json"
    );

    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true, Converters = { new JsonStringEnumConverter() } };

    private List<TweakSchedule> _schedules = [];

    /// <summary>All currently loaded schedules (read-only view).</summary>
    public IReadOnlyList<TweakSchedule> Schedules => _schedules;

    /// <summary>Load schedules from disk. Creates an empty file if absent.</summary>
    public void Load()
    {
        if (!File.Exists(SchedulesPath))
        {
            _schedules = [];
            return;
        }

        try
        {
            string json = File.ReadAllText(SchedulesPath);
            _schedules = JsonSerializer.Deserialize<List<TweakSchedule>>(json, JsonOptions) ?? [];
        }
        catch (JsonException)
        {
            _schedules = [];
        }
    }

    /// <summary>Save all schedules to disk.</summary>
    public void Save()
    {
        string dir = Path.GetDirectoryName(SchedulesPath)!;
        Directory.CreateDirectory(dir);
        string json = JsonSerializer.Serialize(_schedules, JsonOptions);
        File.WriteAllText(SchedulesPath, json);
    }

    /// <summary>
    /// Add a new schedule. Replaces any existing schedule for the same tweak ID.
    /// </summary>
    public void AddSchedule(TweakSchedule schedule)
    {
        ArgumentNullException.ThrowIfNull(schedule);
        _schedules.RemoveAll(s => string.Equals(s.TweakId, schedule.TweakId, StringComparison.OrdinalIgnoreCase));
        _schedules.Add(schedule);
    }

    /// <summary>Remove the schedule for <paramref name="tweakId"/> if it exists.</summary>
    public bool RemoveSchedule(string tweakId)
    {
        int removed = _schedules.RemoveAll(s => string.Equals(s.TweakId, tweakId, StringComparison.OrdinalIgnoreCase));
        return removed > 0;
    }

    /// <summary>Returns the schedule for <paramref name="tweakId"/>, or <c>null</c> if not found.</summary>
    public TweakSchedule? GetSchedule(string tweakId) =>
        _schedules.FirstOrDefault(s => string.Equals(s.TweakId, tweakId, StringComparison.OrdinalIgnoreCase));

    /// <summary>
    /// Update the LastRun timestamp for an existing schedule without changing other fields.
    /// </summary>
    public void RecordLastRun(string tweakId, DateTime runTime)
    {
        int idx = _schedules.FindIndex(s => string.Equals(s.TweakId, tweakId, StringComparison.OrdinalIgnoreCase));
        if (idx < 0)
            return;

        _schedules[idx] = _schedules[idx] with { LastRun = runTime };
    }

    /// <summary>
    /// Toggle the <see cref="TweakSchedule.Enabled"/> flag for <paramref name="tweakId"/>.
    /// </summary>
    public void SetEnabled(string tweakId, bool enabled)
    {
        int idx = _schedules.FindIndex(s => string.Equals(s.TweakId, tweakId, StringComparison.OrdinalIgnoreCase));
        if (idx >= 0)
            _schedules[idx] = _schedules[idx] with { Enabled = enabled };
    }

    /// <summary>Returns all schedules that are due to run based on their interval (Timer, Daily, or Weekly).</summary>
    public IReadOnlyList<TweakSchedule> GetDueTimerSchedules()
    {
        var now = DateTime.UtcNow;
        return _schedules
            .Where(s =>
                s.Enabled
                && (s.Trigger == ScheduleTrigger.Timer
                    || s.Trigger == ScheduleTrigger.Daily
                    || s.Trigger == ScheduleTrigger.Weekly)
                && s.IntervalMinutes > 0
                && (s.LastRun is null || (now - s.LastRun.Value).TotalMinutes >= s.IntervalMinutes)
            )
            .ToList();
    }

    /// <summary>Returns all schedules whose trigger matches <paramref name="trigger"/>.</summary>
    public IReadOnlyList<TweakSchedule> GetSchedulesByTrigger(ScheduleTrigger trigger) =>
        _schedules.Where(s => s.Trigger == trigger).ToList();

    /// <summary>Returns all schedules whose action matches <paramref name="action"/>.</summary>
    public IReadOnlyList<TweakSchedule> GetSchedulesByAction(ScheduleAction action) =>
        _schedules.Where(s => s.Action == action).ToList();
}
