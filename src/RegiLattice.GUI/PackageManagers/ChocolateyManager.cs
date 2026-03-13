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
            var (code, _, _) = ShellRunner
                .RunAsync("choco", ["--version"], default, timeout: TimeSpan.FromSeconds(10))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return code == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Installs Chocolatey via the official install script (requires admin).</summary>
    internal static async Task InstallChocoAsync(CancellationToken ct = default)
    {
        string script = "[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; " +
            "iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))";
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync(script, ct, TimeSpan.FromMinutes(3)).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"Chocolatey installation failed (may require admin): {stderr.Trim()}");
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (code, stdout, stderr) = await ShellRunner
            .RunAsync("choco", ["list", "--limit-output"], ct, timeout: TimeSpan.FromSeconds(60))
            .ConfigureAwait(false);

        if (code != 0 && stdout.Trim().Length == 0)
            throw new InvalidOperationException($"choco list failed (exit {code}): {stderr.Trim()}");

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

    /// <summary>Returns just the package names (no version) of installed packages.</summary>
    internal static async Task<HashSet<string>> ListInstalledNamesAsync(CancellationToken ct = default)
    {
        var list = await ListInstalledAsync(ct).ConfigureAwait(false);
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var entry in list)
        {
            int paren = entry.IndexOf(" (", StringComparison.Ordinal);
            names.Add(paren > 0 ? entry[..paren] : entry);
        }
        return names;
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("choco", ["install", name, "-y", "--no-progress"], ct, timeout: TimeSpan.FromSeconds(120))
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("choco", ["uninstall", name, "-y"], ct, timeout: TimeSpan.FromSeconds(120))
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"choco uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("choco", ["upgrade", name, "-y", "--no-progress"], ct, timeout: TimeSpan.FromSeconds(120))
            .ConfigureAwait(false);
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
        var (_, stdout, _) = await ShellRunner
            .RunAsync("choco", ["outdated", "--limit-output"], ct, timeout: TimeSpan.FromSeconds(60))
            .ConfigureAwait(false);

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
    [
        "googlechrome",
        "firefox",
        "vscode",
        "7zip",
        "git",
        "notepadplusplus",
        "vlc",
        "python3",
        "nodejs",
        "powershell-core",
    ];
}
