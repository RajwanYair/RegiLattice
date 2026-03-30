namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps winget CLI operations with input validation.</summary>
internal static class WinGetManager
{
    internal static bool IsWinGetInstalled()
    {
        try
        {
            var (code, _, _) = ShellRunner.RunAsync("winget", ["--version"], default).ConfigureAwait(false).GetAwaiter().GetResult();
            return code == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Attempts to register/install WinGet via the App Installer package.</summary>
    internal static async Task InstallWinGetAsync(CancellationToken ct = default)
    {
        var (code, _, stderr) = await ShellRunner
            .RunPowerShellAsync(
                "Add-AppxPackage -RegisterByFamilyName -MainPackage Microsoft.DesktopAppInstaller_8wekyb3d8bbwe",
                ct,
                TimeSpan.FromMinutes(2)
            )
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException(
                $"Could not install WinGet automatically. Please install 'App Installer' from the Microsoft Store. {stderr.Trim()}"
            );
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner
            .RunAsync("winget", ["list", "--disable-interactivity", "--accept-source-agreements"], ct, TimeSpan.FromSeconds(60))
            .ConfigureAwait(false);

        return ParseWinGetTable(stdout);
    }

    internal static async Task<List<string>> SearchAsync(string query, CancellationToken ct = default)
    {
        ValidateName(query);
        var (_, stdout, _) = await ShellRunner
            .RunAsync("winget", ["search", query, "--disable-interactivity", "--accept-source-agreements"], ct)
            .ConfigureAwait(false);

        return ParseWinGetTable(stdout);
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("winget", ["install", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"], ct)
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("winget", ["uninstall", "--id", name, "--disable-interactivity"], ct)
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default)
    {
        ValidateName(name);
        var (code, _, stderr) = await ShellRunner
            .RunAsync("winget", ["upgrade", "--id", name, "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"], ct)
            .ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"winget upgrade failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name) => PackageNameValidator.Validate(name, "package");

    /// <summary>Returns just the package names/IDs of installed packages.</summary>
    internal static async Task<HashSet<string>> ListInstalledNamesAsync(CancellationToken ct = default)
    {
        var list = await ListInstalledAsync(ct).ConfigureAwait(false);
        return new HashSet<string>(list, StringComparer.OrdinalIgnoreCase);
    }

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default)
    {
        var (_, stdout, _) = await ShellRunner
            .RunAsync("winget", ["upgrade", "--disable-interactivity", "--accept-source-agreements"], ct, TimeSpan.FromSeconds(60))
            .ConfigureAwait(false);

        return ParseWinGetOutdatedTable(stdout);
    }

    internal static readonly string[] PopularPackages =
    [
        "7zip.7zip",
        "Git.Git",
        "Microsoft.VisualStudioCode",
        "Google.Chrome",
        "Mozilla.Firefox",
        "Notepad++.Notepad++",
        "VideoLAN.VLC",
        "WinSCP.WinSCP",
        "Python.Python.3.12",
        "Microsoft.PowerToys",
    ];

    private static List<string> ParseWinGetTable(string output)
    {
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var results = new List<string>();
        bool pastHeader = false;

        foreach (var rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.StartsWith('-') && line.Length > 10)
            {
                pastHeader = true;
                continue;
            }
            if (!pastHeader || line.Length == 0)
                continue;

            // Take the first "column" — name or id up to double-space
            int dblSpace = line.IndexOf("  ", StringComparison.Ordinal);
            string entry = dblSpace > 0 ? line[..dblSpace].Trim() : line;
            if (entry.Length > 0)
                results.Add(entry);
        }

        return results.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    /// <summary>
    /// Parses `winget upgrade` fixed-width output, extracting "name (currentVer → availableVer)" entries.
    /// Falls back to name-only when column positions cannot be determined.
    /// </summary>
    private static List<string> ParseWinGetOutdatedTable(string output)
    {
        var lines = output.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var results = new List<string>();
        bool pastHeader = false;
        int versionStart = -1;
        int availableStart = -1;

        foreach (var rawLine in lines)
        {
            string line = rawLine.TrimEnd();

            if (!pastHeader)
            {
                // Identify the header line by presence of both "Version" and "Available" columns
                if (versionStart < 0
                    && line.IndexOf("Version", StringComparison.OrdinalIgnoreCase) >= 0
                    && line.IndexOf("Available", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    versionStart = line.IndexOf("Version", StringComparison.OrdinalIgnoreCase);
                    availableStart = line.IndexOf("Available", StringComparison.OrdinalIgnoreCase);
                }
                if (line.StartsWith('-') && line.Length > 10)
                    pastHeader = true;
                continue;
            }

            if (line.Length == 0)
                continue;

            // First column: package name up to first double-space
            int dblSpace = line.IndexOf("  ", StringComparison.Ordinal);
            string name = dblSpace > 0 ? line[..dblSpace].Trim() : line.Trim();
            if (string.IsNullOrEmpty(name))
                continue;

            // Extract version columns when positions are known
            if (versionStart > 0 && availableStart > versionStart && line.Length > versionStart)
            {
                string curVer = line[versionStart..Math.Min(availableStart, line.Length)].Trim();
                int endAvail = line.IndexOf("  ", availableStart, StringComparison.Ordinal);
                string newVer = line.Length > availableStart
                    ? (endAvail > availableStart ? line[availableStart..endAvail].Trim() : line[availableStart..].Trim())
                    : string.Empty;

                if (!string.IsNullOrEmpty(curVer) && !string.IsNullOrEmpty(newVer))
                {
                    results.Add($"{name} ({curVer} \u2192 {newVer})");
                    continue;
                }
            }

            results.Add(name);
        }

        return results.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }
}
