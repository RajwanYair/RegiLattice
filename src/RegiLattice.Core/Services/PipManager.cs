// RegiLattice.Core — Services/PipManager.cs
// pip (Python) package management wrapper.

using System.Text.Json;
using System.Text.RegularExpressions;

namespace RegiLattice.Core;

/// <summary>Wraps pip CLI operations with input validation.</summary>
public static partial class PipManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-\[\],]+$")]
    private static partial Regex SafeNameRegex();

    /// <summary>Find the python executable path.</summary>
    public static string FindPython()
    {
        foreach (var name in new[] { "python", "python3", "py" })
        {
            var (code, stdout, _) = ShellRunner.Run(name, ["--version"]);
            if (code == 0 && stdout.Contains("Python", StringComparison.OrdinalIgnoreCase))
                return name;
        }
        return "python";
    }

    /// <summary>Check if pip is available.</summary>
    public static bool IsPipInstalled(string pythonExe = "python")
    {
        var (code, _, _) = ShellRunner.Run(pythonExe, ["-m", "pip", "--version"]);
        return code == 0;
    }

    /// <summary>List installed pip packages.</summary>
    public static async Task<List<string>> ListInstalledAsync(
        string pythonExe = "python", bool userOnly = false, CancellationToken ct = default)
    {
        var args = new List<string> { "-m", "pip", "list", "--format=json" };
        if (userOnly) args.Add("--user");

        var (code, stdout, _) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0 || string.IsNullOrWhiteSpace(stdout))
            return [];

        var packages = JsonSerializer.Deserialize<List<PipPackage>>(stdout) ?? [];
        return packages
            .Select(p => p.Name)
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Install a pip package.</summary>
    public static async Task InstallAsync(
        string name, string pythonExe = "python", bool userOnly = true, CancellationToken ct = default)
    {
        ValidateName(name);
        var args = new List<string> { "-m", "pip", "install", name };
        if (userOnly) args.Add("--user");

        var (code, _, stderr) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip install failed: {stderr.Trim()}");
    }

    /// <summary>Uninstall a pip package.</summary>
    public static async Task UninstallAsync(
        string name, string pythonExe = "python", CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner.RunAsync(
            pythonExe, ["-m", "pip", "uninstall", "-y", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip uninstall failed: {stderr.Trim()}");
    }

    /// <summary>Upgrade a pip package.</summary>
    public static async Task UpgradeAsync(
        string name, string pythonExe = "python", bool userOnly = true, CancellationToken ct = default)
    {
        ValidateName(name);
        var args = new List<string> { "-m", "pip", "install", "--upgrade", name };
        if (userOnly) args.Add("--user");

        var (code, _, stderr) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip upgrade failed: {stderr.Trim()}");
    }

    /// <summary>Validate a pip package name.</summary>
    public static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}': only letters, digits, '.', '_', '-', '[', ']', ',' allowed.");
        return name;
    }

    /// <summary>Popular Python packages for quick install.</summary>
    public static readonly string[] PopularPackages =
        ["requests", "rich", "click", "pydantic", "httpx", "pytest", "ruff", "mypy", "black", "ipython"];

    private sealed record PipPackage(string Name, string Version);
}
