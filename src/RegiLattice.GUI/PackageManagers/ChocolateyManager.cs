using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps Chocolatey CLI operations with input validation.</summary>
internal static partial class ChocolateyManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    internal static bool IsChocoInstalled()
    {
        try
        {
            var (code, _, _) = ShellRunner.RunAsync("choco", ["--version"], default)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return code == 0;
        }
        catch { return false; }
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunAsync(
            "choco", ["list", "--limit-output"], ct).ConfigureAwait(false);

        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Contains('|'))
            .Select(l =>
            {
                var parts = l.Split('|', 2);
                return parts.Length == 2 ? $"{parts[0]} ({parts[1]})" : parts[0];
            })
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["install", name, "-y", "--no-progress"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["uninstall", name, "-y"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["upgrade", name, "-y", "--no-progress"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco upgrade failed: {stderr.Trim()}");
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
            "choco", ["outdated", "--limit-output"], ct).ConfigureAwait(false);

        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Contains('|'))
            .Select(l =>
            {
                var parts = l.Split('|');
                return parts.Length >= 3 ? $"{parts[0]} ({parts[1]} → {parts[2]})" : parts[0];
            })
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static readonly string[] PopularPackages =
        ["googlechrome", "firefox", "vscode", "7zip", "git", "notepadplusplus",
         "vlc", "python3", "nodejs", "powershell-core"];
}
