namespace RegiLattice.GUI.PackageManagers;

/// <summary>
/// Checks installed versions of external tools used by package managers and tweaks.
/// Returns the installed version string for each tool (or null if not found).
/// Optionally checks if newer versions are available via winget.
/// </summary>
internal static class ToolVersionChecker
{
    internal sealed record ToolInfo(
        string Name,
        string? InstalledVersion,
        bool IsInstalled,
        string? LatestVersion = null,
        bool UpdateAvailable = false
    );

    /// <summary>Check all known external tools and return their status.</summary>
    internal static async Task<IReadOnlyList<ToolInfo>> CheckAllAsync(CancellationToken ct = default)
    {
        string pythonExe = await FindPythonExeAsync(ct).ConfigureAwait(false);

        var tasks = new List<Task<ToolInfo>>
        {
            CheckToolAsync("PowerShell", "pwsh", ["-NoProfile", "-Command", "$PSVersionTable.PSVersion.ToString()"], ct),
            CheckToolAsync(
                "Python",
                pythonExe,
                ["-c", "import sys; print(f'{sys.version_info.major}.{sys.version_info.minor}.{sys.version_info.micro}')"],
                ct
            ),
            CheckToolAsync("winget", "winget", ["--version"], ct),
            CheckToolAsync("Scoop", "powershell", ["-NoProfile", "-Command", "scoop --version | Select-Object -First 1"], ct),
            CheckToolAsync("Chocolatey", "choco", ["--version"], ct),
            CheckToolAsync("Git", "git", ["--version"], ct),
            CheckToolAsync("Node.js", "node", ["--version"], ct),
            CheckToolAsync(".NET SDK", "dotnet", ["--version"], ct),
            CheckToolAsync("GitHub CLI", "gh", ["--version"], ct),
            CheckToolAsync("CMake", "cmake", ["--version"], ct),
            CheckToolAsync("Ninja", "ninja", ["--version"], ct),
            CheckToolAsync("Perl", "perl", ["-v"], ct),
            CheckToolAsync("Docker", "docker", ["--version"], ct),
            CheckToolAsync("Rust", "rustc", ["--version"], ct),
            CheckToolAsync("Go", "go", ["version"], ct),
            CheckToolAsync("Java", "java", ["-version"], ct),
        };

        var results = await Task.WhenAll(tasks).ConfigureAwait(false);
        return results;
    }

    /// <summary>Check all tools and also query for available updates via winget.</summary>
    internal static async Task<IReadOnlyList<ToolInfo>> CheckAllWithUpdatesAsync(CancellationToken ct = default)
    {
        var tools = await CheckAllAsync(ct).ConfigureAwait(false);

        // Build a set of available updates from winget
        Dictionary<string, string> wingetUpdates = [];
        try
        {
            wingetUpdates = await GetWingetAvailableUpdatesAsync(ct).ConfigureAwait(false);
        }
        catch
        { /* winget not available or failed */
        }

        // Map tool names to their winget package IDs for matching
        var wingetMapping = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            ["PowerShell"] = ["Microsoft.PowerShell"],
            ["Python"] = ["Python.Python.3"],
            ["Git"] = ["Git.Git"],
            ["Node.js"] = ["OpenJS.NodeJS"],
            [".NET SDK"] = ["Microsoft.DotNet.SDK"],
            ["GitHub CLI"] = ["GitHub.cli"],
            ["CMake"] = ["Kitware.CMake"],
            ["Docker"] = ["Docker.DockerDesktop"],
            ["Rust"] = ["Rustlang.Rustup"],
            ["Go"] = ["GoLang.Go"],
            ["Java"] = ["Oracle.JDK", "EclipseAdoptium.Temurin"],
        };

        var enriched = new List<ToolInfo>(tools.Count);
        foreach (var tool in tools)
        {
            if (!tool.IsInstalled || !wingetMapping.TryGetValue(tool.Name, out var packageIds))
            {
                enriched.Add(tool);
                continue;
            }

            string? latestVersion = null;
            foreach (var pkgId in packageIds)
            {
                foreach (var (key, ver) in wingetUpdates)
                {
                    if (key.Contains(pkgId, StringComparison.OrdinalIgnoreCase))
                    {
                        latestVersion = ver;
                        break;
                    }
                }
                if (latestVersion is not null)
                    break;
            }

            bool hasUpdate = latestVersion is not null;
            enriched.Add(tool with { LatestVersion = latestVersion, UpdateAvailable = hasUpdate });
        }

        return enriched;
    }

    /// <summary>Check a single tool's version.</summary>
    internal static async Task<ToolInfo> CheckToolAsync(string displayName, string fileName, IEnumerable<string> args, CancellationToken ct = default)
    {
        try
        {
            var (code, stdout, stderr) = await ShellRunner.RunAsync(fileName, args, ct, TimeSpan.FromSeconds(3)).ConfigureAwait(false);
            // Java outputs version to stderr
            string output = !string.IsNullOrWhiteSpace(stdout) ? stdout : stderr;
            if (code == 0 && !string.IsNullOrWhiteSpace(output))
            {
                string version = ExtractVersion(displayName, output);
                return new ToolInfo(displayName, version, true);
            }
        }
        catch
        {
            // Tool not installed or not on PATH
        }
        return new ToolInfo(displayName, null, false);
    }

    /// <summary>Extract a clean version string from raw tool output.</summary>
    private static string ExtractVersion(string tool, string raw)
    {
        string line = raw.Trim().Split('\n')[0].Trim();

        // Strip leading "v" / "V"
        if (line.StartsWith('v') || line.StartsWith('V'))
            line = line[1..];

        // Tool-specific prefixes
        if (line.StartsWith("git version ", StringComparison.OrdinalIgnoreCase))
            return line["git version ".Length..].Split(' ')[0].Trim();
        if (line.StartsWith("cmake version ", StringComparison.OrdinalIgnoreCase))
            return line["cmake version ".Length..].Trim();
        if (line.StartsWith("docker version ", StringComparison.OrdinalIgnoreCase))
            return line["docker version ".Length..].Trim().TrimEnd(',');
        if (line.StartsWith("Docker version ", StringComparison.OrdinalIgnoreCase))
            return line["Docker version ".Length..].Trim().TrimEnd(',');
        if (line.StartsWith("rustc ", StringComparison.OrdinalIgnoreCase))
            return line["rustc ".Length..].Split(' ')[0].Trim();
        if (line.StartsWith("go version ", StringComparison.OrdinalIgnoreCase))
        {
            // "go version go1.22.1 windows/amd64"
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length >= 3 ? parts[2].TrimStart('g', 'o') : line;
        }
        if (line.Contains("gh version ", StringComparison.OrdinalIgnoreCase))
            return line.Split(' ', StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(2) ?? line;

        // Java: "java version \"21.0.1\"" or "openjdk version \"21.0.1\""
        if (line.Contains("version \"", StringComparison.OrdinalIgnoreCase))
        {
            int start = line.IndexOf('"') + 1;
            int end = line.IndexOf('"', start);
            if (start > 0 && end > start)
                return line[start..end];
        }

        // Perl: "This is perl 5, version 42, subversion 0 (v5.42.0) ..."
        if (line.Contains("(v", StringComparison.Ordinal))
        {
            int start = line.IndexOf("(v", StringComparison.Ordinal) + 1;
            int end = line.IndexOf(')', start);
            if (start > 0 && end > start)
                return line[start..end];
        }

        return line;
    }

    /// <summary>Parse winget upgrade output to find available updates.</summary>
    private static async Task<Dictionary<string, string>> GetWingetAvailableUpdatesAsync(CancellationToken ct)
    {
        var (code, stdout, _) = await ShellRunner
            .RunAsync("winget", ["upgrade", "--disable-interactivity", "--accept-source-agreements"], ct)
            .ConfigureAwait(false);

        var updates = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        if (code != 0)
            return updates;

        var lines = stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries);
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
            if (line.Contains("upgrades available", StringComparison.OrdinalIgnoreCase))
                break;

            // Parse: Name  Id  Version  Available  Source
            var parts = line.Split("  ", StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 4)
            {
                string id = parts.Length >= 2 ? parts[1].Trim() : "";
                string available = parts.Length >= 4 ? parts[3].Trim() : "";
                if (id.Length > 0 && available.Length > 0)
                    updates[id] = available;
            }
        }
        return updates;
    }

    private static async Task<string> FindPythonExeAsync(CancellationToken ct)
    {
        foreach (string exe in new[] { "python", "python3", "py" })
        {
            try
            {
                var (code, _, _) = await ShellRunner.RunAsync(exe, ["--version"], ct, TimeSpan.FromSeconds(2)).ConfigureAwait(false);
                if (code == 0)
                    return exe;
            }
            catch
            { /* not found */
            }
        }
        return "python";
    }

    /// <summary>
    /// Returns a recommended install command and documentation URL for a given tool name.
    /// Returns (null, null) for unknown tools.
    /// </summary>
    internal static (string? Command, string? Url) GetInstallGuide(string toolName) =>
        toolName switch
        {
            "PowerShell" => ("winget install Microsoft.PowerShell --accept-source-agreements", "https://aka.ms/install-powershell"),
            "Python" => (
                "winget install Python.Python.3.12 --accept-source-agreements --accept-package-agreements",
                "https://www.python.org/downloads/"
            ),
            "winget" => (
                "Add-AppxPackage -RegisterByFamilyName -MainPackage Microsoft.DesktopAppInstaller_8wekyb3d8bbwe",
                "https://aka.ms/getwinget"
            ),
            "Scoop" => ("Set-ExecutionPolicy RemoteSigned -Scope CurrentUser -Force; irm get.scoop.sh | iex", "https://scoop.sh"),
            "Chocolatey" => (
                "Set-ExecutionPolicy Bypass -Scope Process -Force; iex ((New-Object System.Net.WebClient).DownloadString('https://community.chocolatey.org/install.ps1'))",
                "https://chocolatey.org/install"
            ),
            "Git" => ("winget install Git.Git --accept-source-agreements", "https://git-scm.com/download/win"),
            "Node.js" => ("winget install OpenJS.NodeJS.LTS --accept-source-agreements", "https://nodejs.org/en/download/"),
            ".NET SDK" => ("winget install Microsoft.DotNet.SDK.9 --accept-source-agreements", "https://dotnet.microsoft.com/download"),
            "GitHub CLI" => ("winget install GitHub.cli --accept-source-agreements", "https://cli.github.com"),
            "CMake" => ("winget install Kitware.CMake --accept-source-agreements", "https://cmake.org/download/"),
            "Ninja" => ("scoop install ninja", "https://ninja-build.org"),
            "Perl" => ("winget install StrawberryPerl.StrawberryPerl --accept-source-agreements", "https://strawberryperl.com"),
            "Docker" => (
                "winget install Docker.DockerDesktop --accept-source-agreements",
                "https://docs.docker.com/desktop/install/windows-install/"
            ),
            "Rust" => ("winget install Rustlang.Rustup --accept-source-agreements", "https://www.rust-lang.org/tools/install"),
            "Go" => ("winget install GoLang.Go --accept-source-agreements", "https://go.dev/dl/"),
            "Java" => ("winget install EclipseAdoptium.Temurin.21.JDK --accept-source-agreements", "https://adoptium.net"),
            _ => (null, null),
        };
}
