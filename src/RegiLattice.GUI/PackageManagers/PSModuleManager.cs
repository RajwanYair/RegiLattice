using System.Text.RegularExpressions;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps PowerShell module management via PSGet.</summary>
internal static partial class PSModuleManager
{
    [GeneratedRegex(@"^[A-Za-z0-9._\-]+$")]
    private static partial Regex SafeNameRegex();

    internal static async Task<List<string>> ListInstalledAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        string scopeFilter = scope is "CurrentUser" or "AllUsers"
            ? $"-Scope {scope}"
            : string.Empty;
        string script = $"Get-InstalledModule {scopeFilter} | Select-Object -ExpandProperty Name 2>&1";
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);

        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0)
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static async Task InstallAsync(string name, string scope = "CurrentUser", CancellationToken ct = default)
    {
        ValidateName(name);
        ValidateScope(scope);
        string script = $"Install-Module -Name {name} -Scope {scope} -Force -AllowClobber 2>&1";
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"Install-Module failed: {stderr.Trim()}");
    }

    internal static async Task RemoveAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        string script = $"Uninstall-Module -Name {name} -AllVersions -Force 2>&1";
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"Uninstall-Module failed: {stderr.Trim()}");
    }

    internal static async Task UpdateAsync(string name, string scope = "CurrentUser", CancellationToken ct = default)
    {
        ValidateName(name);
        ValidateScope(scope);
        string script = $"Update-Module -Name {name} -Scope {scope} -Force 2>&1";
        var (code, _, stderr) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"Update-Module failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || !SafeNameRegex().IsMatch(name))
            throw new ArgumentException($"Invalid module name '{name}'.");
        return name;
    }

    internal static string ValidateScope(string scope)
    {
        if (scope is not ("CurrentUser" or "AllUsers"))
            throw new ArgumentException($"Invalid scope '{scope}': must be CurrentUser or AllUsers.");
        return scope;
    }

    internal static async Task<List<string>> ListOutdatedAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        string scopeFilter = scope is "CurrentUser" or "AllUsers"
            ? $"-Scope {scope}"
            : string.Empty;
        string script = $$"""
            Get-InstalledModule {{scopeFilter}} | ForEach-Object {
                $online = Find-Module -Name $_.Name -ErrorAction SilentlyContinue
                if ($online -and $online.Version -gt $_.Version) {
                    "$($_.Name) ($($_.Version) -> $($online.Version))"
                }
            }
            """;
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);

        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0)
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    internal static readonly string[] PopularModules =
        ["PSReadLine", "posh-git", "PowerShellGet", "Az", "Microsoft.Graph",
         "Terminal-Icons", "oh-my-posh", "PSScriptAnalyzer", "Pester", "SqlServer"];
}
