namespace RegiLattice.Core.Tweaks;

using System.IO;
using RegiLattice.Core.Models;

internal static class ScoopTools
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "scoop-disable-autoupdate",
            Label = "Disable Scoop Auto-Update on Install",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_NO_AUTO_UPDATE=1 to prevent Scoop from auto-updating itself before every app install. Default: auto-update. Recommended: disabled for speed.",
            Tags = ["scoop", "autoupdate", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
        },
        new TweakDef
        {
            Id = "scoop-parallel-downloads",
            Label = "Enable Scoop Parallel Downloads (aria2)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets SCOOP_ARIA2_ENABLED=true to enable parallel downloads via aria2 for faster Scoop package installs. Default: disabled. Recommended: enabled.",
            Tags = ["scoop", "parallel", "downloads", "aria2", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-dir",
            Label = "Set Scoop Global Install Directory",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the global Scoop install directory to C:\\Scoop via environment variable. Default: C:\\ProgramData\\scoop.",
            Tags = ["scoop", "global", "install", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\Scoop")],
        },
        new TweakDef
        {
            Id = "scoop-set-cache-dir",
            Label = "Set Scoop Cache Directory",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop download cache to C:\\ScoopCache. Keeps downloads separate from installs. Default: ~\\scoop\\cache.",
            Tags = ["scoop", "cache", "directory", "download"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_CACHE", @"C:\ScoopCache")],
        },
        new TweakDef
        {
            Id = "scoop-enable-debug-mode",
            Label = "Enable Scoop Debug Mode",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables Scoop debug output for troubleshooting install failures. Default: disabled.",
            Tags = ["scoop", "debug", "verbose", "troubleshooting"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_DEBUG", "true")],
        },
        new TweakDef
        {
            Id = "scoop-set-aria2-max-connections",
            Label = "Set Scoop Aria2 Max Connections to 16",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets Scoop Aria2 max connections per server to 16. Speeds up downloads. Default: not set (Aria2 default is 1).",
            Tags = ["scoop", "aria2", "connections", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_OPTIONS", "-x 16 -s 16")],
        },
        new TweakDef
        {
            Id = "scoop-set-global-install-path",
            Label = "Set Scoop Global Install Path",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Scoop global apps install directory to C:\\ScoopGlobal. Keeps system programs organised. Default: %ProgramData%\\scoop.",
            Tags = ["scoop", "global", "install-path", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
        },
        new TweakDef
        {
            Id = "scoop-set-virustotal-api-key",
            Label = "Set Scoop VirusTotal API Key Variable",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the SCOOP_VIRUSTOTAL_API_KEY environment variable placeholder. Replace with your actual key for automatic malware scanning. Default: not set.",
            Tags = ["scoop", "virustotal", "security", "scanning"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_VIRUSTOTAL_API_KEY", "YOUR_API_KEY_HERE")],
        },
        new TweakDef
        {
            Id = "scoop-disable-checkver",
            Label = "Disable Scoop Auto-Version Check",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SCOOP_NO_CHECKVER=1 to skip automatic version checks. Speeds up 'scoop status'. Default: checks versions.",
            Tags = ["scoop", "checkver", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_CHECKVER", "1")],
        },
        new TweakDef
        {
            Id = "scoop-add-extras-bucket",
            Label = "Add Scoop Extras Bucket",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Adds the Scoop 'extras' bucket which contains popular GUI apps. Default: not added.",
            Tags = ["scoop", "extras", "bucket", "apps"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop bucket add extras"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop bucket rm extras"),
            DetectAction = () =>
            {
                var (exit, _, _) = ShellRunner.Run("scoop", ["bucket", "list"]);
                return exit == 0;
            },
        },
        new TweakDef
        {
            Id = "scoop-cleanup-all",
            Label = "Clean Up All Scoop Caches",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PowerShell,
            Description = "Runs scoop cleanup * and scoop cache rm * to free disk space from old versions and downloads.",
            Tags = ["scoop", "cleanup", "cache", "disk-space"],
            ApplyAction = _ =>
            {
                ShellRunner.RunPowerShell("scoop cleanup *");
                ShellRunner.RunPowerShell("scoop cache rm *");
            },
            DetectAction = () => false,
        },
        new TweakDef
        {
            Id = "scoop-install-aria2",
            Label = "Install Aria2 for Scoop Downloads",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs aria2 download manager for faster parallel Scoop downloads. Default: not installed.",
            Tags = ["scoop", "aria2", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install aria2"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall aria2"),
            DetectAction = () =>
            {
                var scoopDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "aria2");
                return Directory.Exists(scoopDir);
            },
        },
        new TweakDef
        {
            Id = "scoop-7zip",
            Label = "Install 7-Zip via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs 7-Zip file archiver via Scoop. Supports 7z, ZIP, RAR, and many other formats.",
            Tags = ["scoop", "7zip", "archive", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install 7zip"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall 7zip"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "7zip")),
        },
        new TweakDef
        {
            Id = "scoop-bat",
            Label = "Install bat via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bat — a cat clone with syntax highlighting and Git integration.",
            Tags = ["scoop", "bat", "cat", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bat"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bat"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bat")),
        },
        new TweakDef
        {
            Id = "scoop-btop",
            Label = "Install btop via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs btop — a resource monitor with CPU, memory, disk, network, and process stats.",
            Tags = ["scoop", "btop", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install btop"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall btop"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "btop")),
        },
        new TweakDef
        {
            Id = "scoop-curl",
            Label = "Install curl via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs curl — command-line tool for transferring data with URLs.",
            Tags = ["scoop", "curl", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install curl"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall curl"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "curl")),
        },
        new TweakDef
        {
            Id = "scoop-delta",
            Label = "Install delta via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs delta — a syntax-highlighting pager for git, diff, and grep output.",
            Tags = ["scoop", "delta", "diff", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install delta"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall delta"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "delta")),
        },
        new TweakDef
        {
            Id = "scoop-duf",
            Label = "Install duf via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs duf — a better df alternative for viewing disk usage with a modern UI.",
            Tags = ["scoop", "duf", "disk", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install duf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall duf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "duf")),
        },
        new TweakDef
        {
            Id = "scoop-dust",
            Label = "Install dust via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs dust — a more intuitive version of du showing disk usage as a tree.",
            Tags = ["scoop", "dust", "disk-usage", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install dust"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall dust"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "dust")),
        },
        new TweakDef
        {
            Id = "scoop-everything",
            Label = "Install Everything via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Everything — instant file search engine for Windows with real-time indexing.",
            Tags = ["scoop", "everything", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install everything"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall everything"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "everything")),
        },
        new TweakDef
        {
            Id = "scoop-fd",
            Label = "Install fd via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fd — a fast and user-friendly alternative to find.",
            Tags = ["scoop", "fd", "find", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fd")),
        },
        new TweakDef
        {
            Id = "scoop-fzf",
            Label = "Install fzf via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs fzf — a general-purpose command-line fuzzy finder.",
            Tags = ["scoop", "fzf", "fuzzy", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install fzf"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall fzf"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "fzf")),
        },
        new TweakDef
        {
            Id = "scoop-git",
            Label = "Install Git via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Git distributed version control system via Scoop.",
            Tags = ["scoop", "git", "vcs", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install git"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall git"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "git")),
        },
        new TweakDef
        {
            Id = "scoop-gsudo",
            Label = "Install gsudo via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gsudo — a sudo equivalent for Windows that elevates commands inline.",
            Tags = ["scoop", "gsudo", "sudo", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gsudo"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gsudo"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gsudo")),
        },
        new TweakDef
        {
            Id = "scoop-hyperfine",
            Label = "Install hyperfine via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs hyperfine — a command-line benchmarking tool with statistical analysis.",
            Tags = ["scoop", "hyperfine", "benchmark", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install hyperfine"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall hyperfine"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "hyperfine")),
        },
        new TweakDef
        {
            Id = "scoop-jq",
            Label = "Install jq via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs jq — a lightweight command-line JSON processor.",
            Tags = ["scoop", "jq", "json", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install jq"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall jq"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "jq")),
        },
        new TweakDef
        {
            Id = "scoop-lazygit",
            Label = "Install lazygit via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lazygit — a simple terminal UI for Git commands.",
            Tags = ["scoop", "lazygit", "git", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lazygit"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lazygit"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lazygit")),
        },
        new TweakDef
        {
            Id = "scoop-neovim",
            Label = "Install Neovim via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Neovim — a hyperextensible Vim-based text editor.",
            Tags = ["scoop", "neovim", "editor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install neovim"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall neovim"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "neovim")),
        },
        new TweakDef
        {
            Id = "scoop-nodejs",
            Label = "Install Node.js via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Node.js JavaScript runtime via Scoop.",
            Tags = ["scoop", "nodejs", "javascript", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nodejs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nodejs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nodejs")),
        },
        new TweakDef
        {
            Id = "scoop-python",
            Label = "Install Python via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Python interpreter via Scoop.",
            Tags = ["scoop", "python", "interpreter", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install python"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall python"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "python")),
        },
        new TweakDef
        {
            Id = "scoop-ripgrep",
            Label = "Install ripgrep via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs ripgrep — a line-oriented search tool that recursively searches directories.",
            Tags = ["scoop", "ripgrep", "search", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install ripgrep"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall ripgrep"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "ripgrep")),
        },
        new TweakDef
        {
            Id = "scoop-set-global-path",
            Label = "Add Scoop Global Apps to PATH",
            Category = "Scoop Tools",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Adds the Scoop global apps directory to the system PATH. Allows globally installed Scoop apps to be available to all users. Default: not in PATH.",
            Tags = ["scoop", "global", "path", "environment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
            ApplyOps =
            [
                RegOp.SetExpandString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment", "SCOOP_GLOBAL")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment",
                    "SCOOP_GLOBAL",
                    @"%ProgramData%\scoop"
                ),
            ],
        },
        new TweakDef
        {
            Id = "scoop-starship",
            Label = "Install Starship via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Starship — a minimal, blazing-fast cross-shell prompt.",
            Tags = ["scoop", "starship", "prompt", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install starship"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall starship"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "starship")),
        },
        new TweakDef
        {
            Id = "scoop-tldr",
            Label = "Install tldr via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tldr — simplified and community-driven man pages.",
            Tags = ["scoop", "tldr", "man", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tldr"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tldr"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tldr")),
        },
        new TweakDef
        {
            Id = "scoop-wget",
            Label = "Install wget via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs wget — a non-interactive network downloader for HTTP, HTTPS, and FTP.",
            Tags = ["scoop", "wget", "download", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install wget"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall wget"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "wget")),
        },
        new TweakDef
        {
            Id = "scoop-zoxide",
            Label = "Install zoxide via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs zoxide — a smarter cd command that learns your most-used directories and jumps to them intelligently.",
            Tags = ["scoop", "zoxide", "navigation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zoxide")),
        },
        new TweakDef
        {
            Id = "scoop-lsd",
            Label = "Install lsd via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs lsd — a modern ls replacement with icons, colours, and tree view.",
            Tags = ["scoop", "lsd", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install lsd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall lsd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "lsd")),
        },
        new TweakDef
        {
            Id = "scoop-sd",
            Label = "Install sd via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs sd — a modern sed/awk replacement for intuitive find-and-replace operations.",
            Tags = ["scoop", "sd", "text", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install sd"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall sd"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "sd")),
        },
        new TweakDef
        {
            Id = "scoop-procs",
            Label = "Install procs via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs procs — a modern replacement for ps that shows processes with colour-coded resource usage.",
            Tags = ["scoop", "procs", "processes", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install procs"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall procs"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "procs")),
        },
        new TweakDef
        {
            Id = "scoop-bottom",
            Label = "Install bottom via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs bottom (btm) — a graphical system monitor for CPU, RAM, disk, and network.",
            Tags = ["scoop", "bottom", "monitor", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install bottom"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall bottom"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "bottom")),
        },
        new TweakDef
        {
            Id = "scoop-xh",
            Label = "Install xh via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs xh — a friendly and fast HTTP client similar to HTTPie.",
            Tags = ["scoop", "xh", "http", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install xh"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall xh"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "xh")),
        },
        new TweakDef
        {
            Id = "scoop-gping",
            Label = "Install gping via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gping — a graphical ping tool that displays network latency as a real-time graph.",
            Tags = ["scoop", "gping", "network", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gping"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gping"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gping")),
        },
        new TweakDef
        {
            Id = "scoop-tokei",
            Label = "Install tokei via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tokei — a fast code statistics tool that counts lines of code by language.",
            Tags = ["scoop", "tokei", "code", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tokei"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tokei"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tokei")),
        },
        new TweakDef
        {
            Id = "scoop-tealdeer",
            Label = "Install tealdeer via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs tealdeer — a fast Rust-based tldr pages client for quick command summaries.",
            Tags = ["scoop", "tealdeer", "tldr", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install tealdeer"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall tealdeer"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "tealdeer")),
        },
        new TweakDef
        {
            Id = "scoop-carapace-bin",
            Label = "Install carapace-bin via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs carapace-bin — a multi-shell completion engine supporting PowerShell, bash, zsh, and more.",
            Tags = ["scoop", "carapace", "completion", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install carapace-bin"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall carapace-bin"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "carapace-bin")),
        },
        new TweakDef
        {
            Id = "scoop-eza",
            Label = "Install eza via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs eza — a modern, maintained replacement for ls with colour output, icons, and git integration. Replaces the deprecated exa.",
            Tags = ["scoop", "eza", "ls", "files", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install eza"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall eza"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "eza")),
        },
        new TweakDef
        {
            Id = "scoop-yazi",
            Label = "Install yazi via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs yazi — a blazing-fast terminal file manager written in Rust with async I/O and vim-style navigation.",
            Tags = ["scoop", "yazi", "file-manager", "terminal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install yazi"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall yazi"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "yazi")),
        },
        new TweakDef
        {
            Id = "scoop-helix",
            Label = "Install Helix editor via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Helix — a post-modern modal text editor inspired by Kakoune and Neovim, with built-in LSP and tree-sitter support.",
            Tags = ["scoop", "helix", "editor", "modal", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install helix"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall helix"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "helix")),
        },
        new TweakDef
        {
            Id = "scoop-nushell",
            Label = "Install Nushell via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Nushell (nu) — a modern shell with structured, type-aware pipelines. Treats output as tables, not plain text.",
            Tags = ["scoop", "nushell", "shell", "nu", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install nu"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall nu"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "nu")),
        },
        new TweakDef
        {
            Id = "scoop-zellij",
            Label = "Install Zellij via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs Zellij — a terminal workspace with panes, tabs, and a plugin system. Modern Rust-based tmux alternative.",
            Tags = ["scoop", "zellij", "terminal", "multiplexer", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install zellij"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall zellij"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "zellij")),
        },
        new TweakDef
        {
            Id = "scoop-gitoxide",
            Label = "Install gitoxide (gix) via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs gitoxide — a pure Rust git implementation providing the gix CLI tool for fast, safe git operations.",
            Tags = ["scoop", "gitoxide", "git", "rust", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install gitoxide"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall gitoxide"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "gitoxide")),
        },
        new TweakDef
        {
            Id = "scoop-watchexec",
            Label = "Install watchexec via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs watchexec — a tool that watches files for changes and re-runs a command automatically. Ideal for dev and build workflows.",
            Tags = ["scoop", "watchexec", "watch", "automation", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install watchexec"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall watchexec"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "watchexec")),
        },
        new TweakDef
        {
            Id = "scoop-topgrade",
            Label = "Install topgrade via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs topgrade — a one-command upgrade tool for all package managers, shells, plugins, and tools on the system.",
            Tags = ["scoop", "topgrade", "upgrade", "maintenance", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install topgrade"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall topgrade"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "topgrade")),
        },
        new TweakDef
        {
            Id = "scoop-pueue",
            Label = "Install pueue via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs pueue — a background task queue manager for long-running shell commands with pause, abort, and log features.",
            Tags = ["scoop", "pueue", "task-queue", "background", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install pueue"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall pueue"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "pueue")),
        },
        new TweakDef
        {
            Id = "scoop-oha",
            Label = "Install oha via Scoop",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            KindHint = TweakKind.PackageManager,
            Description = "Installs oha — a fast HTTP load generator written in Rust for benchmarking web endpoints from the command line.",
            Tags = ["scoop", "oha", "http", "load-test", "install"],
            ApplyAction = _ => ShellRunner.RunPowerShell("scoop install oha"),
            RemoveAction = _ => ShellRunner.RunPowerShell("scoop uninstall oha"),
            DetectAction = () =>
                Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "scoop", "apps", "oha")),
        },
    ];
}
