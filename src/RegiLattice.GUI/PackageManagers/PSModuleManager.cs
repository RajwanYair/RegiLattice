namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps PowerShell module management via PSGet.</summary>
internal static class PSModuleManager
{
    internal static bool IsPowerShellGetAvailable()
    {
        try
        {
            var (code, stdout, _) = ShellRunner
                .RunPowerShellAsync("Get-Module PowerShellGet -ListAvailable | Select-Object -First 1 -ExpandProperty Version", default)
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            return code == 0 && stdout.Trim().Length > 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Installs or updates PowerShellGet module.</summary>
    internal static async Task InstallPowerShellGetAsync(CancellationToken ct = default)
    {
        var (code, _, stderr) = await ShellRunner
            .RunPowerShellAsync("Install-Module PowerShellGet -Force -AllowClobber -Scope CurrentUser", ct, TimeSpan.FromMinutes(2))
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"PowerShellGet installation failed: {stderr.Trim()}");
    }

    internal static async Task<List<string>> ListInstalledAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        // Get-Module -ListAvailable shows all installed modules (not just PSGallery ones)
        string script =
            scope == "AllUsers"
                ? "Get-Module -ListAvailable -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Name -Unique | Sort-Object"
                : "Get-Module -ListAvailable -ErrorAction SilentlyContinue | Where-Object { $_.ModuleBase -like \"$($env:USERPROFILE)*\" -or $_.ModuleBase -like \"$($env:HOME)*\" } | Select-Object -ExpandProperty Name -Unique | Sort-Object";
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync(script, ct, TimeSpan.FromSeconds(30)).ConfigureAwait(false);

        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l => l.Length > 0 && !l.StartsWith("WARNING", StringComparison.OrdinalIgnoreCase))
            .Distinct(StringComparer.OrdinalIgnoreCase)
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

    internal static string ValidateName(string name) => PackageNameValidator.Validate(name, "module");

    internal static string ValidateScope(string scope)
    {
        if (scope is not ("CurrentUser" or "AllUsers"))
            throw new ArgumentException($"Invalid scope '{scope}': must be CurrentUser or AllUsers.");
        return scope;
    }

    /// <summary>Returns just the module names of installed modules in the given scope.</summary>
    internal static async Task<HashSet<string>> ListInstalledNamesAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        var list = await ListInstalledAsync(scope, ct).ConfigureAwait(false);
        return new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);
    }

    internal static async Task<List<string>> ListOutdatedAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        string scopeFilter = scope is "CurrentUser" or "AllUsers" ? $"-Scope {scope}" : string.Empty;
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
    [
        "PSReadLine",
        "posh-git",
        "PowerShellGet",
        "Az",
        "Microsoft.Graph",
        "Terminal-Icons",
        "oh-my-posh",
        "PSScriptAnalyzer",
        "Pester",
        "SqlServer",
    ];

    /// <summary>
    /// Returns a mapping of module name → module base directory path (highest version wins).
    /// </summary>
    internal static async Task<Dictionary<string, string>> GetModuleBasesAsync(string scope = "CurrentUser", CancellationToken ct = default)
    {
        string script =
            scope == "AllUsers"
                ? "Get-Module -ListAvailable -EA SilentlyContinue | Group-Object Name | ForEach-Object { $_.Group | Sort-Object Version -Desc | Select-Object -First 1 } | ForEach-Object { \"$($_.Name)|$($_.ModuleBase)\" }"
                : "Get-Module -ListAvailable -EA SilentlyContinue | Where-Object { $_.ModuleBase -like \"$($env:USERPROFILE)*\" } | Group-Object Name | ForEach-Object { $_.Group | Sort-Object Version -Desc | Select-Object -First 1 } | ForEach-Object { \"$($_.Name)|$($_.ModuleBase)\" }";
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync(script, ct).ConfigureAwait(false);
        var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (string line in stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            int pipe = line.IndexOf('|');
            if (pipe > 0)
                result[line[..pipe].Trim()] = line[(pipe + 1)..].Trim();
        }
        return result;
    }

    /// <summary>
    /// Returns estimated install sizes per module by measuring each module's base directory.
    /// Runs on the thread pool to avoid blocking the UI.
    /// </summary>
    internal static Task<Dictionary<string, string>> GetInstalledSizesAsync(Dictionary<string, string> moduleBases, CancellationToken ct = default) =>
        Task.Run(
            () =>
            {
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (var (name, basePath) in moduleBases)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    result[name] = Directory.Exists(basePath) ? FormatBytes(GetDirSize(basePath)) : "—";
                }
                return result;
            },
            ct
        );

    private static long GetDirSize(string path)
    {
        long size = 0;
        try
        {
            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
            {
                try
                {
                    size += new FileInfo(file).Length;
                }
                catch
                { /* skip locked or inaccessible files */
                }
            }
        }
        catch
        { /* skip inaccessible root directory */
        }
        return size;
    }

    private static string FormatBytes(long bytes) =>
        bytes switch
        {
            >= 1_073_741_824 => $"{bytes / 1_073_741_824.0:F1} GB",
            >= 1_048_576 => $"{bytes / 1_048_576.0:F1} MB",
            >= 1_024 => $"{bytes / 1_024.0:F0} KB",
            _ => $"{bytes} B",
        };
}
