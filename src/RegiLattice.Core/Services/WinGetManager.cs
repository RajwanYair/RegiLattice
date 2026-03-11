// RegiLattice.Core — Services/WinGetManager.cs
// WinGet (Windows Package Manager) wrapper.

using System.Text.RegularExpressions;

namespace RegiLattice.Core;

/// <summary>Wraps winget CLI operations with input validation.</summary>
public static partial class WinGetManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    /// <summary>Check if winget is available.</summary>
    public static bool IsWinGetInstalled()
    {
        var (code, _, _) = ShellRunner.Run("winget", ["--version"]);
        return code == 0;
    }

    /// <summary>List installed winget packages.</summary>
    public static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (code, stdout, _) = await ShellRunner.RunAsync(
            "winget", ["list", "--disable-interactivity", "--accept-source-agreements"], ct).ConfigureAwait(false);
        if (code != 0 || string.IsNullOrWhiteSpace(stdout))
            return [];

        // Parse winget list table output: skip header lines, extract first column
        var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var packages = new List<string>();
        bool pastHeader = false;
        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.StartsWith("---", StringComparison.Ordinal))
            {
                pastHeader = true;
                continue;
            }
            if (!pastHeader || line.Length == 0) continue;

            // First column is the Name, split on double-space (table formatting)
            var parts = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 1 && parts[0].Trim().Length > 0)
                packages.Add(parts[0].Trim());
        }
        return packages.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>Install a winget package by ID or name.</summary>
    public static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget", ["install", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget install failed: {stderr.Trim()}");
    }

    /// <summary>Uninstall a winget package.</summary>
    public static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget", ["uninstall", "--id", name, "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget uninstall failed: {stderr.Trim()}");
    }

    /// <summary>Upgrade a winget package.</summary>
    public static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget", ["upgrade", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget upgrade failed: {stderr.Trim()}");
    }

    /// <summary>Search for packages.</summary>
    public static async Task<List<string>> SearchAsync(string query, CancellationToken ct = default)
    {
        var (code, stdout, _) = await ShellRunner.RunAsync(
            "winget", ["search", query, "--accept-source-agreements", "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0 || string.IsNullOrWhiteSpace(stdout))
            return [];

        var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var results = new List<string>();
        bool pastHeader = false;
        foreach (var rawLine in lines)
        {
            var line = rawLine.Trim();
            if (line.StartsWith("---", StringComparison.Ordinal)) { pastHeader = true; continue; }
            if (!pastHeader || line.Length == 0) continue;
            var parts = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                results.Add($"{parts[0].Trim()} ({parts[1].Trim()})");
        }
        return results;
    }

    /// <summary>Validate a winget package name/ID.</summary>
    public static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-' allowed.");
        return name;
    }

    /// <summary>Popular winget packages for quick install.</summary>
    public static readonly string[] PopularPackages =
        ["7zip.7zip", "Git.Git", "Microsoft.VisualStudioCode", "Google.Chrome",
         "Mozilla.Firefox", "Notepad++.Notepad++", "VideoLAN.VLC",
         "Python.Python.3.12", "WinSCP.WinSCP", "PuTTY.PuTTY"];
}
