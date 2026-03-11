// RegiLattice.Core — Services/Elevation.cs
// Native UAC elevation helpers — replaces Python elevation.py.

using System.Diagnostics;
using System.Security.Principal;

namespace RegiLattice.Core;

/// <summary>UAC privilege management using native .NET APIs.</summary>
public static class Elevation
{
    /// <summary>Returns true if the current process has administrator privileges.</summary>
    public static bool IsAdmin()
    {
        using var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>Re-launches the current executable elevated (UAC prompt).</summary>
    /// <returns>Exit code of the elevated process, or -1 if user cancelled.</returns>
    public static int RequestElevation(string[]? args = null)
    {
        var exePath = Environment.ProcessPath
            ?? throw new InvalidOperationException("Cannot determine current process path.");

        var psi = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = args is not null ? string.Join(' ', args.Select(QuoteArg)) : "",
            Verb = "runas",
            UseShellExecute = true,
        };

        try
        {
            using var proc = Process.Start(psi);
            proc?.WaitForExit();
            return proc?.ExitCode ?? -1;
        }
        catch (System.ComponentModel.Win32Exception)
        {
            // User cancelled UAC prompt
            return -1;
        }
    }

    /// <summary>
    /// Runs an allowed system command elevated.
    /// Only whitelisted executables are permitted.
    /// </summary>
    public static (int ExitCode, string StdOut, string StdErr) RunElevated(
        string executable, string[] args, int timeoutMs = 120_000)
    {
        var exeName = Path.GetFileNameWithoutExtension(executable).ToLowerInvariant();
        if (!AllowedCommands.Contains(exeName))
            throw new UnauthorizedAccessException($"Command '{executable}' is not in the elevation allowlist.");

        var psi = new ProcessStartInfo
        {
            FileName = executable,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };
        foreach (var arg in args) psi.ArgumentList.Add(arg);

        using var proc = Process.Start(psi)
            ?? throw new InvalidOperationException($"Failed to start {executable}");

        var stdout = proc.StandardOutput.ReadToEnd();
        var stderr = proc.StandardError.ReadToEnd();
        proc.WaitForExit(timeoutMs);
        return (proc.ExitCode, stdout, stderr);
    }

    /// <summary>Throws if not running as admin and requireAdmin is true.</summary>
    public static void AssertAdmin(bool requireAdmin = true)
    {
        if (requireAdmin && !IsAdmin())
            throw new UnauthorizedAccessException("This operation requires administrator privileges.");
    }

    private static string QuoteArg(string arg)
        => arg.Contains(' ') || arg.Contains('"') ? $"\"{arg.Replace("\"", "\\\"")}\"" : arg;

    private static readonly HashSet<string> AllowedCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "reg", "dism", "icacls", "takeown", "netsh", "sc", "schtasks",
        "taskkill", "powershell", "pwsh", "cmd", "bcdedit", "fsutil",
        "net", "netstat", "wmic", "reagentc", "verifier", "wusa",
    };
}
