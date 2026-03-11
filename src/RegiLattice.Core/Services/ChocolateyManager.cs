// RegiLattice.Core — Services/ChocolateyManager.cs
// Chocolatey package manager wrapper.

using System.Text.RegularExpressions;

namespace RegiLattice.Core;

/// <summary>Wraps choco CLI operations with input validation.</summary>
public static partial class ChocolateyManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    /// <summary>Check if Chocolatey is installed.</summary>
    public static bool IsChocoInstalled()
    {
        var (code, _, _) = ShellRunner.Run("choco", ["--version"]);
        return code == 0;
    }

    /// <summary>List installed Chocolatey packages.</summary>
    public static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (code, stdout, _) = await ShellRunner.RunAsync(
            "choco", ["list", "--local-only", "--limit-output"], ct).ConfigureAwait(false);
        if (code != 0 || string.IsNullOrWhiteSpace(stdout))
            return [];

        // choco list --limit-output format: name|version
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Split('|', 2)[0].Trim())
            .Where(n => n.Length > 0 && !n.StartsWith("Chocolatey", StringComparison.OrdinalIgnoreCase))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Install a Chocolatey package.</summary>
    public static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["install", name, "-y", "--no-progress"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco install failed: {stderr.Trim()}");
    }

    /// <summary>Uninstall a Chocolatey package.</summary>
    public static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["uninstall", name, "-y", "--no-progress"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco uninstall failed: {stderr.Trim()}");
    }

    /// <summary>Upgrade a Chocolatey package.</summary>
    public static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            "choco", ["upgrade", name, "-y", "--no-progress"], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco upgrade failed: {stderr.Trim()}");
    }

    /// <summary>Validate a Chocolatey package name.</summary>
    public static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-' allowed.");
        return name;
    }

    /// <summary>Popular Chocolatey packages for quick install.</summary>
    public static readonly string[] PopularPackages =
        ["googlechrome", "firefox", "vscode", "7zip", "git",
         "notepadplusplus", "vlc", "python3", "nodejs", "sysinternals"];
}
