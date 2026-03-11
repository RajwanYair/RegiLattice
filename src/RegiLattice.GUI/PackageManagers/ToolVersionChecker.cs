namespace RegiLattice.GUI.PackageManagers;

/// <summary>
/// Checks installed versions of external tools used by package managers and tweaks.
/// Returns the installed version string for each tool (or null if not found).
/// </summary>
internal static class ToolVersionChecker
{
    internal sealed record ToolInfo(string Name, string? InstalledVersion, bool IsInstalled);

    /// <summary>Check all known external tools and return their status.</summary>
    internal static async Task<IReadOnlyList<ToolInfo>> CheckAllAsync(CancellationToken ct = default)
    {
        var tasks = new List<Task<ToolInfo>>
        {
            CheckToolAsync("PowerShell", "pwsh", ["-NoProfile", "-Command", "$PSVersionTable.PSVersion.ToString()"], ct),
            CheckToolAsync("Python", FindPythonExe(), ["-c", "import sys; print(f'{sys.version_info.major}.{sys.version_info.minor}.{sys.version_info.micro}')"], ct),
            CheckToolAsync("winget", "winget", ["--version"], ct),
            CheckToolAsync("Scoop", "powershell", ["-NoProfile", "-Command", "scoop --version | Select-Object -First 1"], ct),
            CheckToolAsync("Chocolatey", "choco", ["--version"], ct),
            CheckToolAsync("Git", "git", ["--version"], ct),
            CheckToolAsync("Node.js", "node", ["--version"], ct),
        };

        return await Task.WhenAll(tasks).ConfigureAwait(false);
    }

    /// <summary>Check a single tool's version.</summary>
    internal static async Task<ToolInfo> CheckToolAsync(
        string displayName,
        string fileName,
        IEnumerable<string> args,
        CancellationToken ct = default)
    {
        try
        {
            var (code, stdout, _) = await ShellRunner.RunAsync(fileName, args, ct).ConfigureAwait(false);
            if (code == 0)
            {
                string version = stdout.Trim().Split('\n')[0].Trim();
                // Strip common prefixes like "v" from version strings
                if (version.StartsWith('v') || version.StartsWith('V'))
                    version = version[1..];
                // Strip "git version " prefix
                if (version.StartsWith("git version ", StringComparison.OrdinalIgnoreCase))
                    version = version["git version ".Length..].Trim();
                return new ToolInfo(displayName, version, true);
            }
        }
        catch
        {
            // Tool not installed or not on PATH
        }
        return new ToolInfo(displayName, null, false);
    }

    private static string FindPythonExe()
    {
        foreach (string exe in new[] { "python", "python3", "py" })
        {
            try
            {
                var (code, _, _) = ShellRunner.RunAsync(exe, ["--version"], default)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
                if (code == 0) return exe;
            }
            catch { /* not found */ }
        }
        return "python";
    }
}
