using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps scoop CLI operations with input validation.</summary>
internal static partial class ScoopManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    internal static bool IsScoopInstalled()
    {
        string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "scoop", "shims", "scoop.ps1");
        return File.Exists(path);
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop list 2>&1", ct).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0
                        && !l.StartsWith("Installed", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase))
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync($"scoop install {name} 2>&1", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync($"scoop uninstall {name} 2>&1", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop uninstall failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-' allowed.");
        return name;
    }

    internal static readonly string[] PopularTools =
        ["7zip", "git", "ripgrep", "fd", "bat", "fzf", "jq", "gsudo", "neovim", "starship"];
}
