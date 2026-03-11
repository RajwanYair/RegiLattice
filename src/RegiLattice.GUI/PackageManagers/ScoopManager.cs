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

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop status 2>&1", ct).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0
                        && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("Updates", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("Scoop", StringComparison.OrdinalIgnoreCase))
            .Select(l =>
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return parts.Length >= 3 ? $"{parts[0]} ({parts[1]} → {parts[2]})" : parts[0];
            })
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync($"scoop update {name} 2>&1", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop update failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAllAsync(CancellationToken ct = default)
    {
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync("scoop update *", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop update * failed: {stderr.Trim()}");
    }

    internal static async Task<List<string>> SearchAsync(string query, CancellationToken ct = default)
    {
        ValidateName(query);
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync($"scoop search {query} 2>&1", ct).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0
                        && !l.StartsWith("Results", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("'", StringComparison.Ordinal))
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static async Task<List<string>> ListBucketsAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop bucket list 2>&1", ct).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0
                        && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                        && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase))
            .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0])
            .Where(n => !string.IsNullOrEmpty(n))
            .ToList();
    }

    internal static async Task AddBucketAsync(string bucketName, CancellationToken ct = default)
    {
        ValidateName(bucketName);
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync($"scoop bucket add {bucketName} 2>&1", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop bucket add failed: {stderr.Trim()}");
    }

    internal static async Task<string> GetInfoAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync($"scoop info {name} 2>&1", ct).ConfigureAwait(false);
        return stdout.Trim();
    }

    internal static async Task<string> ExportAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop export 2>&1", ct).ConfigureAwait(false);
        return stdout.Trim();
    }

    internal static async Task CleanupAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync($"scoop cleanup {name} 2>&1", ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"scoop cleanup failed: {stderr.Trim()}");
    }

    internal static readonly string[] PopularTools =
        ["7zip", "git", "ripgrep", "fd", "bat", "fzf", "jq", "gsudo", "neovim", "starship",
         "delta", "cmake", "ninja", "nuget", "tokei", "cloc", "doxygen", "graphviz"];
}
