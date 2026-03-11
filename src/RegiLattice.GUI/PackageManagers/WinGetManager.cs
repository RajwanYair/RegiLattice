using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps winget CLI operations with input validation.</summary>
internal static partial class WinGetManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    internal static bool IsWinGetInstalled()
    {
        try
        {
            var (code, _, _) = ShellRunner.RunAsync("winget", ["--version"], default)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return code == 0;
        }
        catch { return false; }
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunAsync(
            "winget", ["list", "--disable-interactivity", "--accept-source-agreements"], ct)
            .ConfigureAwait(false);

        return ParseWinGetTable(stdout);
    }

    internal static async Task<List<string>> SearchAsync(string query, CancellationToken ct = default)
    {
        ValidateName(query);
        var (_, stdout, _) = await ShellRunner.RunAsync(
            "winget", ["search", query, "--disable-interactivity", "--accept-source-agreements"], ct)
            .ConfigureAwait(false);

        return ParseWinGetTable(stdout);
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget",
            ["install", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget", ["uninstall", "--id", name, "--disable-interactivity"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "winget",
            ["upgrade", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"],
            ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget upgrade failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-' allowed.");
        return name;
    }

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunAsync(
            "winget", ["upgrade", "--disable-interactivity", "--accept-source-agreements"], ct)
            .ConfigureAwait(false);

        return ParseWinGetTable(stdout);
    }

    internal static readonly string[] PopularPackages =
        ["7zip.7zip", "Git.Git", "Microsoft.VisualStudioCode", "Google.Chrome",
         "Mozilla.Firefox", "Notepad++.Notepad++", "VideoLAN.VLC", "WinSCP.WinSCP",
         "Python.Python.3.12", "Microsoft.PowerToys"];

    private static List<string> ParseWinGetTable(string output)
    {
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var results = new List<string>();
        bool pastHeader = false;

        foreach (var rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.StartsWith('-') && line.Length > 10)
            {
                pastHeader = true;
                continue;
            }
            if (!pastHeader || line.Length == 0) continue;

            // Take the first "column" — name or id up to double-space
            int dblSpace = line.IndexOf("  ", StringComparison.Ordinal);
            string entry = dblSpace > 0 ? line[..dblSpace].Trim() : line;
            if (entry.Length > 0)
                results.Add(entry);
        }

        return results.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
