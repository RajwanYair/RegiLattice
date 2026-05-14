// RegiLattice.Core — Services/Elevation.cs
// UAC elevation helpers.

using System.Diagnostics;
using System.Security.Principal;
using RegiLattice.Core.Models;

namespace RegiLattice.Core;

/// <summary>UAC privilege management using .NET APIs.</summary>
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
        var exePath = Environment.ProcessPath ?? throw new InvalidOperationException("Cannot determine current process path.");

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
    public static (int ExitCode, string StdOut, string StdErr) RunElevated(string executable, string[] args, int timeoutMs = 120_000)
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
        foreach (var arg in args)
            psi.ArgumentList.Add(arg);

        using var proc = Process.Start(psi) ?? throw new InvalidOperationException($"Failed to start {executable}");

        // Read stdout/stderr asynchronously to prevent deadlock when output buffers fill up
        var stdoutTask = proc.StandardOutput.ReadToEndAsync();
        var stderrTask = proc.StandardError.ReadToEndAsync();

        if (!proc.WaitForExit(timeoutMs))
        {
            try
            {
                proc.Kill(entireProcessTree: true);
            }
            catch
            { /* best effort */
            }
            throw new TimeoutException($"Process '{executable}' did not exit within {timeoutMs}ms.");
        }

        return (proc.ExitCode, stdoutTask.GetAwaiter().GetResult(), stderrTask.GetAwaiter().GetResult());
    }

    /// <summary>Throws if not running as admin and requireAdmin is true.</summary>
    public static void AssertAdmin(bool requireAdmin = true)
    {
        if (requireAdmin && !IsAdmin())
            throw new UnauthorizedAccessException("This operation requires administrator privileges.");
    }

    /// <summary>
    /// Returns <c>true</c> when a tweak can be executed without administrator
    /// privileges.  A tweak is elevation-free when ALL of the following hold:
    /// <list type="bullet">
    ///   <item><description><see cref="TweakDef.NeedsAdmin"/> is <c>false</c>, OR every registry
    ///   operation targets HKEY_CURRENT_USER exclusively.</description></item>
    ///   <item><description>The tweak has no <see cref="TweakDef.ApplyAction"/> delegate
    ///   (custom actions may call system tools that require elevation).</description></item>
    /// </list>
    /// </summary>
    public static bool CanRunWithoutElevation(TweakDef td)
    {
        // Custom-action tweaks may run arbitrary code — conservatively require elevation.
        if (td.ApplyAction is not null)
            return false;

        // If the tweak itself declares it doesn't need admin, honour that.
        if (!td.NeedsAdmin)
            return true;

        // If every registry op is under HKCU, Windows will allow it without elevation.
        if (td.ApplyOps.Count == 0)
            return false;

        return td.ApplyOps.All(op =>
            op.Path.StartsWith("HKEY_CURRENT_USER", StringComparison.OrdinalIgnoreCase) ||
            op.Path.StartsWith("HKCU", StringComparison.OrdinalIgnoreCase));
    }

    private static string QuoteArg(string arg) => arg.Contains(' ') || arg.Contains('"') ? $"\"{arg.Replace("\"", "\\\"")}\"" : arg;

    private static readonly HashSet<string> AllowedCommands = new(StringComparer.OrdinalIgnoreCase)
    {
        "reg",
        "dism",
        "icacls",
        "takeown",
        "netsh",
        "sc",
        "schtasks",
        "taskkill",
        "powershell",
        "pwsh",
        "cmd",
        "bcdedit",
        "fsutil",
        "net",
        "netstat",
        "wmic",
        "reagentc",
        "verifier",
        "wusa",
    };
}
