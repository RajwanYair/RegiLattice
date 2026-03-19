// RegiLattice.Core — Services/PowerPlanManager.cs
// Sprint 31: Power plan enumeration and switching via powercfg.exe.

#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Versioning;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace RegiLattice.Core.Services;

/// <summary>Immutable snapshot of a Windows power plan.</summary>
[SupportedOSPlatform("windows")]
public sealed record PowerPlanEntry(Guid Guid, string Name, bool IsActive);

/// <summary>
/// Enumerates and switches Windows power plans via <c>powercfg.exe</c>.
/// Switching the active plan requires Administrator privileges.
/// </summary>
[SupportedOSPlatform("windows")]
public static class PowerPlanManager
{
    // ── Well-known GUIDs (informational) ────────────────────────────────────
    public static readonly Guid HighPerformance = new("8c5e7fda-e8bf-4a96-9a85-a6e23a8c635c");
    public static readonly Guid Balanced = new("381b4222-f694-41f0-9685-ff5bb260df2e");
    public static readonly Guid PowerSaver = new("a1841308-3541-4fab-bc81-f71556f20b4a");
    public static readonly Guid UltimatePerfMode = new("e9a42b02-d5df-448d-aa00-03f14749eb61");

    // ── Query ────────────────────────────────────────────────────────────────

    /// <summary>Returns all available power plans from <c>powercfg /list</c>.</summary>
    public static IReadOnlyList<PowerPlanEntry> GetAllPlans()
    {
        string output = RunPowercfg("/list");
        return ParseListOutput(output);
    }

    /// <summary>Returns the GUID of the currently active plan, or <see langword="null"/>.</summary>
    public static Guid? GetActivePlanGuid()
    {
        foreach (var plan in GetAllPlans())
            if (plan.IsActive)
                return plan.Guid;
        return null;
    }

    // ── Control ──────────────────────────────────────────────────────────────

    /// <summary>Activates the specified power plan (requires admin).</summary>
    public static async Task SetActivePlanAsync(Guid planGuid, CancellationToken ct = default) =>
        await RunPowercfgAsync($"/setactive {planGuid:D}", ct).ConfigureAwait(false);

    /// <summary>Creates the Ultimate Performance plan (may already exist on some SKUs).</summary>
    public static async Task DuplicateSchemeAsync(Guid sourceGuid, CancellationToken ct = default) =>
        await RunPowercfgAsync($"/duplicatescheme {sourceGuid:D}", ct).ConfigureAwait(false);

    /// <summary>Deletes a power plan (cannot delete the active plan).</summary>
    public static async Task DeleteSchemeAsync(Guid planGuid, CancellationToken ct = default) =>
        await RunPowercfgAsync($"/delete {planGuid:D}", ct).ConfigureAwait(false);

    /// <summary>Exports a power plan to a file.</summary>
    public static async Task ExportSchemeAsync(Guid planGuid, string filePath, CancellationToken ct = default) =>
        await RunPowercfgAsync($"/export \"{filePath}\" {planGuid:D}", ct).ConfigureAwait(false);

    /// <summary>Imports a power plan from a file.</summary>
    public static async Task ImportSchemeAsync(string filePath, CancellationToken ct = default) =>
        await RunPowercfgAsync($"/import \"{filePath}\"", ct).ConfigureAwait(false);

    // ── Helpers ──────────────────────────────────────────────────────────────

    private static readonly Regex _planLineRegex = new(
        @"Power Scheme GUID:\s*([0-9a-fA-F-]{36})\s*\(([^)]+)\)(\s*\*)?",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    private static IReadOnlyList<PowerPlanEntry> ParseListOutput(string output)
    {
        var result = new List<PowerPlanEntry>();
        foreach (string line in output.Split('\n'))
        {
            var m = _planLineRegex.Match(line);
            if (!m.Success)
                continue;

            if (!Guid.TryParse(m.Groups[1].Value, out Guid guid))
                continue;
            string name = m.Groups[2].Value.Trim();
            bool isActive = m.Groups[3].Success;
            result.Add(new PowerPlanEntry(guid, name, isActive));
        }
        return result.AsReadOnly();
    }

    private static string RunPowercfg(string args)
    {
        try
        {
            using var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powercfg.exe",
                    Arguments = args,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
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

    private static async Task RunPowercfgAsync(string args, CancellationToken ct)
    {
        using var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "powercfg.exe",
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
}
