using System.Diagnostics;
using System.Text;
using System.Text.Json;
using RegiLattice.Native.Models;

namespace RegiLattice.Native;

/// <summary>
/// Bridges the C# GUI to the RegiLattice Python back-end via subprocess calls.
/// All communication uses the existing <c>python -m regilattice</c> CLI.
/// </summary>
internal sealed class PythonBridge : IDisposable
{
    private readonly string _python;
    private bool _disposed;

    // ── Constructor ────────────────────────────────────────────────────────

    /// <param name="pythonExe">Full path to the Python executable to use.</param>
    public PythonBridge(string pythonExe)
    {
        _python = pythonExe ?? throw new ArgumentNullException(nameof(pythonExe));
    }

    // ── Public API ─────────────────────────────────────────────────────────

    /// <summary>
    /// Exports all tweaks to a temp JSON file and deserialises them.
    /// Calls: <c>python -m regilattice --export-json &lt;tempfile&gt;</c>
    /// </summary>
    public async Task<IReadOnlyList<TweakInfo>> LoadTweaksAsync(CancellationToken ct = default)
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            await RunAsync(new[] { "-m", "regilattice", "--export-json", tempFile }, ct).ConfigureAwait(false);

            await using FileStream fs = File.OpenRead(tempFile);
            var tweaks = await JsonSerializer.DeserializeAsync<List<TweakInfo>>(
                fs,
                TweakInfoSerializerContext.Default.ListTweakInfo,
                ct).ConfigureAwait(false);

            return tweaks ?? [];
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    /// <summary>
    /// Applies a single tweak, bypassing any interactive confirmation.
    /// Calls: <c>python -m regilattice apply &lt;id&gt; -y [--force]</c>
    /// </summary>
    /// <returns>Tuple of (exitCode, stderr).</returns>
    public Task<(int ExitCode, string StdErr)> ApplyTweakAsync(string tweakId, CancellationToken ct = default, bool force = false)
    {
        var args = new List<string> { "-m", "regilattice", "apply", tweakId, "-y" };
        if (force) args.Add("--force");
        return RunCapturedAsync(args.ToArray(), ct);
    }

    /// <summary>
    /// Removes a single tweak, bypassing any interactive confirmation.
    /// Calls: <c>python -m regilattice remove &lt;id&gt; -y [--force]</c>
    /// </summary>
    public Task<(int ExitCode, string StdErr)> RemoveTweakAsync(string tweakId, CancellationToken ct = default, bool force = false)
    {
        var args = new List<string> { "-m", "regilattice", "remove", tweakId, "-y" };
        if (force) args.Add("--force");
        return RunCapturedAsync(args.ToArray(), ct);
    }

    /// <summary>
    /// Queries the live status of a single tweak.
    /// Calls: <c>python -m regilattice status &lt;id&gt;</c>
    /// Returns <c>"applied"</c>, <c>"not_applied"</c>, or <c>"unknown"</c>.
    /// </summary>
    public async Task<string> GetStatusAsync(string tweakId, CancellationToken ct = default)
    {
        var (_, stdout, _) = await RunFullAsync(new[] { "-m", "regilattice", "status", tweakId }, ct).ConfigureAwait(false);
        string lower = stdout.Trim().ToLowerInvariant();
        if (lower.Contains("applied") && !lower.Contains("not"))  return "applied";
        if (lower.Contains("not_applied") || lower.Contains("not applied"))  return "not_applied";
        return "unknown";
    }

    // ── Discovery ──────────────────────────────────────────────────────────

    /// <summary>
    /// Finds a usable Python 3.10+ executable on the current machine.
    /// Priority: LOCALAPPDATA\Python\bin → common install paths → PATH (skipping WindowsApps stubs).
    /// Throws <see cref="InvalidOperationException"/> if Python cannot be found.
    /// </summary>
    public static string FindPython()
    {
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        string[] candidates =
        [
            Path.Combine(localAppData, @"Python\bin\python.exe"),
            Path.Combine(localAppData, @"Python\pythoncore-3.14-64\python.exe"),
            Path.Combine(localAppData, @"Programs\Python\Python314\python.exe"),
            Path.Combine(localAppData, @"Programs\Python\Python313\python.exe"),
            Path.Combine(localAppData, @"Programs\Python\Python312\python.exe"),
            Path.Combine(localAppData, @"Programs\Python\Python311\python.exe"),
            Path.Combine(localAppData, @"Programs\Python\Python310\python.exe"),
            @"C:\Python314\python.exe",
            @"C:\Python313\python.exe",
            @"C:\Python312\python.exe",
        ];

        foreach (string candidate in candidates)
        {
            if (File.Exists(candidate))
                return candidate;
        }

        // Search PATH, skipping the WindowsApps stub
        string? pathEnv = Environment.GetEnvironmentVariable("PATH");
        if (pathEnv is not null)
        {
            foreach (string dir in pathEnv.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dir.Contains("WindowsApps", StringComparison.OrdinalIgnoreCase))
                    continue;
                string full = Path.Combine(dir, "python.exe");
                if (File.Exists(full))
                    return full;
            }
        }

        throw new InvalidOperationException(
            "Python 3.10+ not found. Install Python from https://python.org and add it to PATH.");
    }

    // ── Private helpers ────────────────────────────────────────────────────

    private async Task RunAsync(string[] args, CancellationToken ct)
    {
        var (exitCode, _, stderr) = await RunFullAsync(args, ct).ConfigureAwait(false);
        if (exitCode != 0)
            throw new InvalidOperationException($"RegiLattice exited {exitCode}: {stderr}");
    }

    private async Task<(int ExitCode, string StdErr)> RunCapturedAsync(string[] args, CancellationToken ct)
    {
        var (exitCode, _, stderr) = await RunFullAsync(args, ct).ConfigureAwait(false);
        return (exitCode, stderr);
    }

    private async Task<(int ExitCode, string StdOut, string StdErr)> RunFullAsync(string[] args, CancellationToken ct)
    {
        // Build argument string safely — no shell interpolation
        string argString = string.Join(" ", args.Select(QuoteArg));

        using Process proc = new()
        {
            StartInfo = new ProcessStartInfo
            {
                FileName               = _python,
                Arguments              = argString,
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                CreateNoWindow         = true,
                StandardOutputEncoding = Encoding.UTF8,
                StandardErrorEncoding  = Encoding.UTF8,
            },
            EnableRaisingEvents = true,
        };

        proc.Start();

        Task<string> stdoutTask = proc.StandardOutput.ReadToEndAsync(ct);
        Task<string> stderrTask = proc.StandardError.ReadToEndAsync(ct);

        await proc.WaitForExitAsync(ct).ConfigureAwait(false);
        string stdout = await stdoutTask.ConfigureAwait(false);
        string stderr = await stderrTask.ConfigureAwait(false);

        return (proc.ExitCode, stdout, stderr);
    }

    /// <summary>Quote a single argument for command-line use; does NOT invoke a shell.</summary>
    private static string QuoteArg(string arg)
    {
        // If the arg is a safe file path or simple word, no quoting needed.
        // Otherwise wrap in double-quotes, escaping embedded quotes.
        if (!arg.Contains(' ') && !arg.Contains('"') && !arg.Contains('\\'))
            return arg;
        return "\"" + arg.Replace("\\", "\\\\").Replace("\"", "\\\"") + "\"";
    }

    // ── IDisposable ────────────────────────────────────────────────────────

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
    }
}
