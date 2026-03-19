// RegiLattice.Core — Services/ScheduledTaskManager.cs
// Sprint 30: Scheduled task enumeration and control via schtasks.exe.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RegiLattice.Core.Services;

/// <summary>Status of a scheduled task.</summary>
public enum ScheduledTaskStatus
{
    Unknown,
    Ready,
    Running,
    Disabled,
    Queued,
}

/// <summary>Immutable snapshot of a Windows scheduled task.</summary>
[SupportedOSPlatform("windows")]
public sealed record ScheduledTaskEntry(
    string TaskName,
    string TaskPath,
    ScheduledTaskStatus Status,
    string NextRunTime,
    string LastRunTime,
    string Author
)
{
    /// <summary>Display-friendly name (without folder path prefix).</summary>
    public string DisplayName => TaskName.Contains('\\') ? TaskName[(TaskName.LastIndexOf('\\') + 1)..] : TaskName;
}

/// <summary>
/// Enumerates and controls Windows scheduled tasks via <c>schtasks.exe</c>.
/// All mutating operations require Administrator privileges.
/// </summary>
[SupportedOSPlatform("windows")]
public static class ScheduledTaskManager
{
    // ── Query ────────────────────────────────────────────────────────────────

    /// <summary>
    /// Returns all scheduled tasks in CSV format from schtasks.exe.
    /// Filters out empty / header rows.
    /// </summary>
    public static IReadOnlyList<ScheduledTaskEntry> GetAllTasks()
    {
        string csv = RunSchtasks("/query /fo CSV /nh 2>&1");
        return ParseCsvOutput(csv);
    }

    // ── Control ──────────────────────────────────────────────────────────────

    /// <summary>Enables a disabled scheduled task.</summary>
    public static async Task EnableAsync(string taskName, CancellationToken ct = default) =>
        await RunSchtasksAsync($"/change /tn \"{EscapeTaskName(taskName)}\" /enable", ct).ConfigureAwait(false);

    /// <summary>Disables a scheduled task.</summary>
    public static async Task DisableAsync(string taskName, CancellationToken ct = default) =>
        await RunSchtasksAsync($"/change /tn \"{EscapeTaskName(taskName)}\" /disable", ct).ConfigureAwait(false);

    /// <summary>Deletes a scheduled task (irreversible — requires admin).</summary>
    public static async Task DeleteAsync(string taskName, CancellationToken ct = default) =>
        await RunSchtasksAsync($"/delete /tn \"{EscapeTaskName(taskName)}\" /f", ct).ConfigureAwait(false);

    /// <summary>Runs a scheduled task immediately.</summary>
    public static async Task RunNowAsync(string taskName, CancellationToken ct = default) =>
        await RunSchtasksAsync($"/run /tn \"{EscapeTaskName(taskName)}\"", ct).ConfigureAwait(false);

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static string EscapeTaskName(string name) => name.Replace("\"", "\\\"");

    private static string RunSchtasks(string args)
    {
        try
        {
            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "schtasks.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8,
                },
            };
            proc.Start();
            string output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit(30_000);
            return output;
        }
        catch
        {
            return string.Empty;
        }
    }

    private static async Task RunSchtasksAsync(string args, CancellationToken ct)
    {
        using var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "schtasks.exe",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            },
        };
        proc.Start();
        await proc.WaitForExitAsync(ct).ConfigureAwait(false);
    }

    private static IReadOnlyList<ScheduledTaskEntry> ParseCsvOutput(string csv)
    {
        var result = new List<ScheduledTaskEntry>();
        foreach (string line in csv.Split('\n'))
        {
            string trimmed = line.Trim();
            if (string.IsNullOrEmpty(trimmed))
                continue;

            // Parse CSV fields (handles quoted commas)
            string[] fields = SplitCsvLine(trimmed);
            if (fields.Length < 3)
                continue;

            string taskName = fields[0].Trim('"').Trim();
            if (string.IsNullOrEmpty(taskName) || taskName.StartsWith("TaskName", StringComparison.OrdinalIgnoreCase))
                continue;

            string nextRun = fields.Length > 1 ? fields[1].Trim('"').Trim() : "";
            string status = fields.Length > 2 ? fields[2].Trim('"').Trim() : "";

            // schtasks /fo CSV /nh schema: TaskName, NextRunTime, Status, LogonMode, LastRunTime, LastResult, Author, ...
            string lastRun = fields.Length > 4 ? fields[4].Trim('"').Trim() : "";
            string author = fields.Length > 6 ? fields[6].Trim('"').Trim() : "";

            result.Add(
                new ScheduledTaskEntry(
                    TaskName: taskName,
                    TaskPath: taskName,
                    Status: ParseStatus(status),
                    NextRunTime: nextRun,
                    LastRunTime: lastRun,
                    Author: author
                )
            );
        }

        result.Sort((a, b) => string.Compare(a.TaskName, b.TaskName, StringComparison.OrdinalIgnoreCase));
        return result.AsReadOnly();
    }

    private static ScheduledTaskStatus ParseStatus(string s) =>
        s.ToLowerInvariant() switch
        {
            "ready" => ScheduledTaskStatus.Ready,
            "running" => ScheduledTaskStatus.Running,
            "disabled" => ScheduledTaskStatus.Disabled,
            "queued" => ScheduledTaskStatus.Queued,
            _ => ScheduledTaskStatus.Unknown,
        };

    private static string[] SplitCsvLine(string line)
    {
        var fields = new List<string>();
        bool inQuote = false;
        var current = new StringBuilder();

        foreach (char c in line)
        {
            if (c == '"')
            {
                inQuote = !inQuote;
            }
            else if (c == ',' && !inQuote)
            {
                fields.Add(current.ToString());
                current.Clear();
            }
            else
            {
                current.Append(c);
            }
        }
        fields.Add(current.ToString());
        return fields.ToArray();
    }
}
