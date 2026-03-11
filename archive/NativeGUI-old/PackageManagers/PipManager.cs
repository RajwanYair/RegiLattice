using System.Text.Json;
using System.Text.RegularExpressions;

namespace RegiLattice.Native.PackageManagers;

/// <summary>Wraps pip package management.</summary>
internal static class PipManager
{
    private static readonly Regex SafeNameRe = new(@"^[A-Za-z0-9._\-\[\],]+$", RegexOptions.Compiled);

    internal static async Task<List<string>> ListInstalledAsync(
        string pythonExe, bool userOnly = false, CancellationToken ct = default)
    {
        var args = new List<string> { "-m", "pip", "list", "--format=json" };
        if (userOnly) args.Add("--user");
        var (_, stdout, _) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);

        try
        {
            var items = JsonSerializer.Deserialize<JsonElement[]>(stdout);
            if (items is null) return [];
            return items
                .Select(e => e.TryGetProperty("name", out var n) ? n.GetString() ?? "" : "")
                .Where(n => n.Length > 0)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }
        catch
        {
            return [];
        }
    }

    internal static async Task InstallAsync(
        string name, string pythonExe, bool userOnly = true, CancellationToken ct = default)
    {
        ValidateName(name);
        var args = new List<string> { "-m", "pip", "install", name };
        if (userOnly) args.Add("--user");
        var (code, _, stderr) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(
        string name, string pythonExe, CancellationToken ct = default)
    {
        ValidateName(name);
        var args = new List<string> { "-m", "pip", "uninstall", "-y", name };
        var (code, _, stderr) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpdateAsync(
        string name, string pythonExe, bool userOnly = true, CancellationToken ct = default)
    {
        ValidateName(name);
        var args = new List<string> { "-m", "pip", "install", "--upgrade", name };
        if (userOnly) args.Add("--user");
        var (code, _, stderr) = await ShellRunner.RunAsync(pythonExe, args, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip upgrade failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRe.IsMatch(name))
            throw new ArgumentException($"Invalid package name '{name}'.");
        return name;
    }

    internal static readonly string[] PopularPackages =
        ["requests", "rich", "click", "pydantic", "httpx", "pytest", "ruff", "mypy", "black", "ipython"];
}
