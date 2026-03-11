using System.Diagnostics;
using System.Text;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Low-level process runner used by all package managers.</summary>
internal static class ShellRunner
{
    /// <summary>Run a process with explicit argument list (no shell injection risk).</summary>
    internal static async Task<(int ExitCode, string StdOut, string StdErr)> RunAsync(
        string fileName,
        IEnumerable<string> args,
        CancellationToken ct = default)
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
        await proc.WaitForExitAsync(ct).ConfigureAwait(false);
        return (proc.ExitCode, await stdoutTask.ConfigureAwait(false), await stderrTask.ConfigureAwait(false));
    }

    /// <summary>Run a PowerShell -Command script.</summary>
    internal static Task<(int ExitCode, string StdOut, string StdErr)> RunPowerShellAsync(
        string script,
        CancellationToken ct = default)
        => RunAsync("powershell.exe",
            ["-NoProfile", "-NonInteractive", "-Command", script],
            ct);
}
