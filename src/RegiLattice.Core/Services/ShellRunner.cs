// RegiLattice.Core — Services/ShellRunner.cs
// Safe process runner for command-based tweaks.

using System.Diagnostics;
using System.Text;

namespace RegiLattice.Core;

/// <summary>
/// Low-level process runner used by command-based tweaks.
/// Uses <see cref="ProcessStartInfo.ArgumentList"/> (no shell injection risk).
/// </summary>
public static class ShellRunner
{
    /// <summary>Run a process with explicit argument list. Kills the process if <paramref name="ct"/> is cancelled.</summary>
    public static async Task<(int ExitCode, string StdOut, string StdErr)> RunAsync(
        string fileName,
        IEnumerable<string> args,
        CancellationToken ct = default
    )
    {
        using var proc = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = fileName,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding = Encoding.UTF8,
            },
        };
        foreach (var arg in args)
            proc.StartInfo.ArgumentList.Add(arg);

        proc.Start();
        var stdoutTask = proc.StandardOutput.ReadToEndAsync(ct);
        var stderrTask = proc.StandardError.ReadToEndAsync(ct);
        try
        {
            await proc.WaitForExitAsync(ct).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            try { proc.Kill(entireProcessTree: true); } catch (InvalidOperationException) { }
            return (-1, string.Empty, "timeout");
        }
        return (proc.ExitCode, await stdoutTask.ConfigureAwait(false), await stderrTask.ConfigureAwait(false));
    }

    /// <summary>Run a PowerShell -Command script.</summary>
    public static Task<(int ExitCode, string StdOut, string StdErr)> RunPowerShellAsync(string script, CancellationToken ct = default) =>
        RunAsync("powershell.exe", ["-NoProfile", "-NonInteractive", "-Command", script], ct);

    /// <summary>
    /// Synchronous wrapper for RunAsync (used by TweakDef delegates).
    /// Kills the process after <paramref name="timeoutMs"/> milliseconds (default 10 s).
    /// </summary>
    public static (int ExitCode, string StdOut, string StdErr) Run(string fileName, IEnumerable<string> args, int timeoutMs = 10_000)
    {
        using var cts = new CancellationTokenSource(timeoutMs);
        try
        {
            return RunAsync(fileName, args, cts.Token).GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
            return (-1, string.Empty, "timeout");
        }
    }

    /// <summary>Synchronous wrapper for RunPowerShellAsync (default 30 s timeout).</summary>
    public static (int ExitCode, string StdOut, string StdErr) RunPowerShell(string script, int timeoutMs = 30_000)
    {
        using var cts = new CancellationTokenSource(timeoutMs);
        try
        {
            return RunPowerShellAsync(script, cts.Token).GetAwaiter().GetResult();
        }
        catch (OperationCanceledException)
        {
            return (-1, string.Empty, "timeout");
        }
    }
}
