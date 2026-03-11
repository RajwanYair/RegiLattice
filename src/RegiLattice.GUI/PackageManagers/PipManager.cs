using System.Text.Json;
using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps pip CLI operations with input validation.</summary>
internal static partial class PipManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-\[\]]+$")]
    private static partial Regex SafeNameRegex();

    /// <summary>Finds python executable (python, python3, py).</summary>
    internal static string? FindPython()
    {
        foreach (string exe in new[] { "python", "python3", "py" })
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "AppData", "Local", "Programs", "Python");
            // Quick check: try running the executable
            try
            {
                var (code, _, _) = ShellRunner.RunAsync(exe, ["--version"], default)
                    .ConfigureAwait(false).GetAwaiter().GetResult();
                if (code == 0) return exe;
            }
            catch { /* not found */ }
        }
        return null;
    }

    internal static bool IsPipInstalled()
    {
        string? python = FindPython();
        if (python is null) return false;
        try
        {
            var (code, _, _) = ShellRunner.RunAsync(python, ["-m", "pip", "--version"], default)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return code == 0;
        }
        catch { return false; }
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        string python = FindPython() ?? "python";
        var (_, stdout, _) = await ShellRunner.RunAsync(
            python, ["-m", "pip", "list", "--format=json"], ct).ConfigureAwait(false);

        var packages = new List<string>();
        try
        {
            using var doc = JsonDocument.Parse(stdout);
            foreach (var pkg in doc.RootElement.EnumerateArray())
            {
                string? name = pkg.GetProperty("name").GetString();
                string? ver = pkg.GetProperty("version").GetString();
                if (name is not null)
                    packages.Add(ver is not null ? $"{name} ({ver})" : name);
            }
        }
        catch { /* parse failed — return empty */ }

        return packages.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        string python = FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(
            python, ["-m", "pip", "install", "--user", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        string python = FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(
            python, ["-m", "pip", "uninstall", "-y", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        string python = FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(
            python, ["-m", "pip", "install", "--user", "--upgrade", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip upgrade failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-', '[]' allowed.");
        return name;
    }

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default)
    {
        string python = FindPython() ?? "python";
        var (_, stdout, _) = await ShellRunner.RunAsync(
            python, ["-m", "pip", "list", "--outdated", "--format=json"], ct).ConfigureAwait(false);

        var packages = new List<string>();
        try
        {
            using var doc = JsonDocument.Parse(stdout);
            foreach (var pkg in doc.RootElement.EnumerateArray())
            {
                string? name = pkg.GetProperty("name").GetString();
                string? ver = pkg.GetProperty("version").GetString();
                string? latest = pkg.GetProperty("latest_version").GetString();
                if (name is not null)
                    packages.Add($"{name} ({ver} → {latest})");
            }
        }
        catch { /* parse failed — return empty */ }

        return packages.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    internal static readonly string[] PopularPackages =
        ["requests", "rich", "click", "pydantic", "httpx", "pytest", "ruff", "mypy", "black", "ipython"];
}
