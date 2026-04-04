using System.Text.Json;

namespace RegiLattice.GUI.PackageManagers;

/// <summary>Wraps pip CLI operations with input validation.</summary>
internal static class PipManager
{
    /// <summary>
    /// Finds the best available Python executable by searching well-known installation
    /// locations before falling back to PATH resolution.
    /// Search order (first working pip wins):
    ///   1. py launcher enumeration (PEP 514 — covers all registered Windows installs)
    ///   2. User-scope installs  — %LOCALAPPDATA%\Programs\Python\Python*\python.exe
    ///   3. System-scope installs — %ProgramFiles%\Python*\python.exe
    ///   4. System-scope (x86)   — %ProgramFiles(x86)%\Python*\python.exe
    ///   5. Conda/Miniconda base — %USERPROFILE%\miniconda3, ~\anaconda3, %ProgramData%\miniconda3|anaconda3
    ///   6. PATH fallback        — python, python3, py
    /// </summary>
    internal static string? FindPython() => FindAllPythons().FirstOrDefault();

    /// <summary>
    /// Returns all Python executables that have pip available, ordered by version descending.
    /// Each entry is an absolute path (or a bare name for PATH-resolved executables).
    /// </summary>
    internal static IReadOnlyList<string> FindAllPythons()
    {
        var candidates = new List<string>();

        // 1. py launcher: 'py -0p' lists all registered installs with their full paths
        try
        {
            var (code, stdout, _) = ShellRunner
                .RunAsync("py", ["-0p"], default, TimeSpan.FromSeconds(5))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
            if (code == 0)
            {
                foreach (string raw in stdout.Split('\n', StringSplitOptions.RemoveEmptyEntries))
                {
                    // Format: " -3.12-64       C:\Users\...\Python312\python.exe"
                    string line = raw.Trim();
                    int pathStart = line.IndexOf('\\');
                    if (pathStart <= 0)
                        pathStart = line.IndexOf("python.exe", StringComparison.OrdinalIgnoreCase);
                    if (pathStart > 0)
                    {
                        string exePath = line[pathStart..].Trim();
                        if (File.Exists(exePath))
                            candidates.Add(exePath);
                    }
                }
            }
        }
        catch
        { /* py launcher not available */
        }

        // 2. User-scope: %LOCALAPPDATA%\Programs\Python\Python*\python.exe
        string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        string userPythonRoot = Path.Combine(localAppData, "Programs", "Python");
        candidates.AddRange(GlobPythonExes(userPythonRoot));

        // 3. System-scope: %ProgramFiles%\Python*\python.exe
        string progFiles = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
        candidates.AddRange(GlobPythonExes(progFiles));

        // 4. System-scope x86: %ProgramFiles(x86)%\Python*\python.exe
        string? progFilesX86 = Environment.GetEnvironmentVariable("ProgramFiles(x86)");
        if (!string.IsNullOrEmpty(progFilesX86))
            candidates.AddRange(GlobPythonExes(progFilesX86));

        // 5. Conda/Miniconda/Anaconda base environments
        string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        string? programData = Environment.GetEnvironmentVariable("ProgramData");
        foreach (
            string condaBase in new[]
            {
                Path.Combine(userProfile, "miniconda3"),
                Path.Combine(userProfile, "anaconda3"),
                Path.Combine(userProfile, "miniforge3"),
                programData is not null ? Path.Combine(programData, "miniconda3") : null,
                programData is not null ? Path.Combine(programData, "anaconda3") : null,
            }
                .Where(p => p is not null)
                .Cast<string>()
        )
        {
            string condaPy = Path.Combine(condaBase, "python.exe");
            if (File.Exists(condaPy))
                candidates.Add(condaPy);
        }

        // 6. PATH fallback (bare executable names)
        foreach (string exe in new[] { "python", "python3", "py" })
        {
            try
            {
                var (code, _, _) = ShellRunner
                    .RunAsync(exe, ["--version"], default, TimeSpan.FromSeconds(5))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                if (code == 0)
                    candidates.Add(exe);
            }
            catch
            { /* not on PATH */
            }
        }

        // Deduplicate (resolve canonical paths), then keep only those that have pip
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var working = new List<string>();
        foreach (string cand in candidates)
        {
            // Normalise to canonical path for dedup; keep original string for invocation
            string key = cand.Contains('\\') || cand.Contains('/') ? Path.GetFullPath(cand) : cand;
            if (!seen.Add(key))
                continue;
            try
            {
                var (code, _, _) = ShellRunner
                    .RunAsync(cand, ["-m", "pip", "--version"], default, TimeSpan.FromSeconds(8))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
                if (code == 0)
                    working.Add(cand);
            }
            catch
            { /* no pip on this interpreter */
            }
        }
        return working;
    }

    /// <summary>Scans <paramref name="root"/> for subdirectories matching Python*\ and returns python.exe paths.</summary>
    private static IEnumerable<string> GlobPythonExes(string root)
    {
        if (!Directory.Exists(root))
            yield break;
        foreach (
            string dir in Directory
                .GetDirectories(root, "Python*", SearchOption.TopDirectoryOnly)
                .OrderByDescending(d => d, StringComparer.OrdinalIgnoreCase)
        )
        {
            string exe = Path.Combine(dir, "python.exe");
            if (File.Exists(exe))
                yield return exe;
        }
    }

    internal static bool IsPipInstalled()
    {
        string? python = FindPython();
        if (python is null)
            return false;
        try
        {
            var (code, _, _) = ShellRunner.RunAsync(python, ["-m", "pip", "--version"], default).ConfigureAwait(false).GetAwaiter().GetResult();
            return code == 0;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Attempts to install Python via winget (if available).</summary>
    internal static async Task InstallPythonAsync(CancellationToken ct = default)
    {
        if (WinGetManager.IsWinGetInstalled())
        {
            var (code, _, stderr) = await ShellRunner
                .RunAsync(
                    "winget",
                    ["install", "Python.Python.3.12", "--accept-package-agreements", "--accept-source-agreements", "--disable-interactivity"],
                    ct,
                    TimeSpan.FromMinutes(5)
                )
                .ConfigureAwait(false);
            if (code == 0)
                return;
            throw new InvalidOperationException($"Python installation via winget failed: {stderr.Trim()}");
        }
        throw new InvalidOperationException("winget is not available. Please install Python manually from python.org.");
    }

    internal static async Task<List<string>> ListInstalledAsync(CancellationToken ct = default) =>
        await ListInstalledAsync(userOnly: false, python: null, ct).ConfigureAwait(false);

    /// <summary>Lists installed pip packages. If userOnly is true, lists only --user packages.</summary>
    internal static async Task<List<string>> ListInstalledAsync(bool userOnly, CancellationToken ct = default) =>
        await ListInstalledAsync(userOnly, python: null, ct).ConfigureAwait(false);

    /// <summary>Lists installed pip packages using a specific <paramref name="python"/> executable.</summary>
    internal static async Task<List<string>> ListInstalledAsync(bool userOnly, string? python, CancellationToken ct = default)
    {
        string pyExe = python ?? FindPython() ?? "python";
        var args = userOnly ? new[] { "-m", "pip", "list", "--user", "--format=json" } : new[] { "-m", "pip", "list", "--format=json" };
        var (_, stdout, _) = await ShellRunner.RunAsync(pyExe, args, ct, TimeSpan.FromSeconds(30)).ConfigureAwait(false);

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
        catch
        { /* parse failed — return empty */
        }

        return packages.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    internal static async Task<HashSet<string>> ListInstalledNamesAsync(CancellationToken ct = default) =>
        await ListInstalledNamesAsync(python: null, ct).ConfigureAwait(false);

    /// <summary>Returns just the package names (no version) using a specific interpreter.</summary>
    internal static async Task<HashSet<string>> ListInstalledNamesAsync(string? python, CancellationToken ct = default)
    {
        var list = await ListInstalledAsync(userOnly: false, python, ct).ConfigureAwait(false);
        return PackageNameValidator.ExtractNames(list);
    }

    internal static async Task InstallAsync(string name, CancellationToken ct = default) =>
        await InstallAsync(name, python: null, ct).ConfigureAwait(false);

    internal static async Task InstallAsync(string name, string? python, CancellationToken ct = default)
    {
        ValidateName(name);
        string pyExe = python ?? FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(pyExe, ["-m", "pip", "install", "--user", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip install failed: {stderr.Trim()}");
    }

    internal static async Task UninstallAsync(string name, CancellationToken ct = default) =>
        await UninstallAsync(name, python: null, ct).ConfigureAwait(false);

    internal static async Task UninstallAsync(string name, string? python, CancellationToken ct = default)
    {
        ValidateName(name);
        string pyExe = python ?? FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(pyExe, ["-m", "pip", "uninstall", "-y", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip uninstall failed: {stderr.Trim()}");
    }

    internal static async Task UpgradeAsync(string name, CancellationToken ct = default) =>
        await UpgradeAsync(name, python: null, ct).ConfigureAwait(false);

    internal static async Task UpgradeAsync(string name, string? python, CancellationToken ct = default)
    {
        ValidateName(name);
        string pyExe = python ?? FindPython() ?? "python";
        var (code, _, stderr) = await ShellRunner.RunAsync(pyExe, ["-m", "pip", "install", "--user", "--upgrade", name], ct).ConfigureAwait(false);
        if (code != 0)
            throw new InvalidOperationException($"pip upgrade failed: {stderr.Trim()}");
    }

    internal static string ValidateName(string name) => PackageNameValidator.Validate(name, "package", allowBrackets: true);

    internal static async Task<List<string>> ListOutdatedAsync(CancellationToken ct = default) =>
        await ListOutdatedAsync(python: null, ct).ConfigureAwait(false);

    internal static async Task<List<string>> ListOutdatedAsync(string? python, CancellationToken ct = default)
    {
        string pyExe = python ?? FindPython() ?? "python";
        var (_, stdout, _) = await ShellRunner.RunAsync(pyExe, ["-m", "pip", "list", "--outdated", "--format=json"], ct).ConfigureAwait(false);

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
        catch
        { /* parse failed — return empty */
        }

        return packages.OrderBy(n => n, StringComparer.OrdinalIgnoreCase).ToList();
    }

    internal static readonly string[] PopularPackages =
    [
        "requests",
        "rich",
        "click",
        "pydantic",
        "httpx",
        "pytest",
        "ruff",
        "mypy",
        "black",
        "ipython",
    ];
}
