namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps scoop CLI operations with input validation.</summary>
internal static class ScoopManager
{
    internal static bool IsScoopInstalled()
    {
        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "shims", "scoop.ps1");
        if (File.Exists(path))
            return true;

        // Fallback: check if scoop is in PATH
        try
        {
            var (code, _, _) = ShellRunner.RunAsync("scoop", ["--version"], default, TimeSpan.FromSeconds(5)).GetAwaiter().GetResult();
            return code == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Installs Scoop via the official installer script (user-level, no admin needed).</summary>
    internal static async Task InstallScoopAsync(CancellationToken ct = default)
    {
        var (code, _, stderr) = await ShellRunner
            .RunPowerShellAsync(
                "Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; irm get.scoop.sh | iex",
                ct,
                TimeSpan.FromMinutes(3)
            )
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"Scoop installation failed: {stderr.Trim()}");
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop list 2>&1", ct, TimeSpan.FromSeconds(30)).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l =>
                l.Length > 0
                && !l.StartsWith("Installed", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
            )
            .Select(l =>
            {
                var parts = l.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                return parts.Length >= 2 ? $"{parts[0]} ({parts[1]})" : parts[0];
            })
            .Where(n => !string.IsNullOrEmpty(n))
            .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    /// <summary>Returns just the package names (no version) of installed packages.</summary>
    internal static async Task<HashSet<string>> ListInstalledNamesAsync(CancellationToken ct = default)
    {
        var list = await ListInstalledAsync(ct).ConfigureAwait(false);
        return PackageNameValidator.ExtractNames(list);
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

    internal static string ValidateName(string name) => PackageNameValidator.Validate(name, "package");

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner.RunPowerShellAsync("scoop status 2>&1", ct, TimeSpan.FromSeconds(30)).ConfigureAwait(false);
        return stdout
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(l => l.Trim())
            .Where(l =>
                l.Length > 0
                && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("Updates", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("Scoop", StringComparison.OrdinalIgnoreCase)
            )
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
            .Where(l =>
                l.Length > 0
                && !l.StartsWith("Results", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
                && !l.StartsWith("'", StringComparison.Ordinal)
            )
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
            .Where(l =>
                l.Length > 0 && !l.StartsWith("Name", StringComparison.OrdinalIgnoreCase) && !l.StartsWith("---", StringComparison.OrdinalIgnoreCase)
            )
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
    [
        "7zip",
        "git",
        "ripgrep",
        "fd",
        "bat",
        "fzf",
        "jq",
        "gsudo",
        "neovim",
        "starship",
        "delta",
        "cmake",
        "ninja",
        "nuget",
        "tokei",
        "cloc",
        "doxygen",
        "graphviz",
    ];

    /// <summary>
    /// Returns estimated install sizes per package by measuring each package's directory under
    /// <c>~\scoop\apps\&lt;name&gt;</c>. Runs on the thread pool to avoid blocking the UI.
    /// </summary>
    internal static Task<Dictionary<string, string>> GetInstalledSizesAsync(
        HashSet<string> installedNames,
        CancellationToken ct = default
    ) =>
        Task.Run(
            () =>
            {
                string scoopApps = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "scoop",
                    "apps"
                );
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (string name in installedNames)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    string dir = Path.Combine(scoopApps, name);
                    result[name] = Directory.Exists(dir) ? FormatBytes(GetDirSize(dir)) : "—";
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
                catch { /* skip locked or inaccessible files */ }
            }
        }
        catch { /* skip inaccessible root directory */ }
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
