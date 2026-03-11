namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ScoopTools
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "scoop-7zip",
            Label = "Scoop: 7-Zip",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "7-Zip file archiver — fast compression/decompression supporting 7z, ZIP, RAR, TAR, GZIP, and more. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "archiver", "compression"],
        },
        new TweakDef
        {
            Id = "scoop-git",
            Label = "Scoop: Git",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Git distributed version control system. Required by many scoop buckets and developer workflows. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "git", "vcs", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-curl",
            Label = "Scoop: cURL",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "cURL command-line tool for transferring data with URLs. Supports HTTP, HTTPS, FTP, and many other protocols. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "networking", "download"],
        },
        new TweakDef
        {
            Id = "scoop-wget",
            Label = "Scoop: wget",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "GNU Wget — non-interactive network downloader. Supports HTTP, HTTPS, and FTP with recursive download. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "networking", "download"],
        },
        new TweakDef
        {
            Id = "scoop-fzf",
            Label = "Scoop: fzf (fuzzy finder)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "fzf is a general-purpose command-line fuzzy finder. Blazing fast, works with any list — files, history, processes. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "search", "productivity"],
        },
        new TweakDef
        {
            Id = "scoop-ripgrep",
            Label = "Scoop: ripgrep (rg)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "ripgrep (rg) — lightning-fast regex search tool. Recursively searches directories, respects .gitignore. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "search", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-fd",
            Label = "Scoop: fd (find alternative)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "fd is a fast, user-friendly alternative to 'find'. Colorized output, regex support, respects .gitignore. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "search", "files", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-bat",
            Label = "Scoop: bat (cat with syntax)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "bat — a cat clone with syntax highlighting, git integration, and automatic paging. Drop-in replacement for cat. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "terminal", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-jq",
            Label = "Scoop: jq (JSON processor)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "jq — lightweight command-line JSON processor. Slice, filter, map, and transform structured data. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "json", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-neovim",
            Label = "Scoop: Neovim",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Neovim — hyperextensible Vim-based text editor. Async plugins, built-in LSP, Lua scripting. Default: Not installed.",
            Tags = ["scoop", "tools", "editor", "vim", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-starship",
            Label = "Scoop: Starship Prompt",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Starship — blazing-fast, cross-shell prompt in Rust. Shows git status, language versions, battery, and more. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "terminal", "prompt", "shell"],
        },
        new TweakDef
        {
            Id = "scoop-delta",
            Label = "Scoop: delta (git diff)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "delta — syntax-highlighting pager for git, diff, and grep. Modern, beautiful git diffs. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "git", "diff", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-everything",
            Label = "Scoop: Everything Search",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Everything — instant file search for Windows by name. Indexes NTFS drives in seconds — near-zero memory usage. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "search", "files", "productivity"],
        },
        new TweakDef
        {
            Id = "scoop-gsudo",
            Label = "Scoop: gsudo (sudo for Windows)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "gsudo — a sudo equivalent for Windows. Run elevated commands from your current console. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "admin", "terminal", "elevation"],
        },
        new TweakDef
        {
            Id = "scoop-python",
            Label = "Scoop: Python",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Python interpreter managed via Scoop. Easy version switching with scoop reset. Default: Not installed.",
            Tags = ["scoop", "tools", "python", "developer", "language"],
        },
        new TweakDef
        {
            Id = "scoop-nodejs",
            Label = "Scoop: Node.js",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "Node.js JavaScript runtime managed via Scoop. Includes npm package manager. Default: Not installed.",
            Tags = ["scoop", "tools", "nodejs", "javascript", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-btop",
            Label = "Scoop: btop++ (Resource Monitor)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "btop++ — modern resource monitor with battery, CPU, memory, disk, network stats. Beautiful TUI. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "monitor", "resource", "terminal"],
        },
        new TweakDef
        {
            Id = "scoop-lazygit",
            Label = "Scoop: lazygit (Git TUI)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "lazygit — simple terminal UI for git commands. Stage, commit, rebase interactively. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "git", "tui", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-duf",
            Label = "Scoop: duf (Disk Usage)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "duf — disk usage/free utility with colorful output. Modern alternative to df. Default: Not installed.",
            Tags = ["scoop", "tools", "disk", "utility", "terminal"],
        },
        new TweakDef
        {
            Id = "scoop-tldr",
            Label = "Scoop: tldr (Simplified Man Pages)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "tldr — simplified, community-driven man pages. Quick command reference. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "documentation", "terminal", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-dust",
            Label = "Scoop: dust (Disk Usage TUI)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "dust -- a more intuitive version of du written in Rust. Visualizes disk usage with a bar chart in the terminal. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "disk", "utility", "terminal", "rust"],
        },
        new TweakDef
        {
            Id = "scoop-hyperfine",
            Label = "Scoop: hyperfine (Benchmarking)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = false,
            Description = "hyperfine -- command-line benchmarking tool. Statistical analysis, warmup runs, export to CSV/JSON. Default: Not installed. Recommended: Install.",
            Tags = ["scoop", "tools", "benchmark", "performance", "developer"],
        },
        new TweakDef
        {
            Id = "scoop-set-global-path",
            Label = "Set Scoop Global Install Path",
            Category = "Scoop Tools",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = @"Sets the SCOOP_GLOBAL environment variable to C:\ScoopGlobal for system-wide Scoop installations. Default: not set. Recommended: set for multi-user machines.",
            Tags = ["scoop", "global", "path", "environment"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Environment"],
        },
        new TweakDef
        {
            Id = "scoop-disable-autoupdate",
            Label = "Disable Scoop Auto-Update on Install",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SCOOP_NO_AUTO_UPDATE=1 to prevent Scoop from auto-updating itself before every app install. Default: auto-update. Recommended: disabled for speed.",
            Tags = ["scoop", "autoupdate", "speed", "environment"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_NO_AUTO_UPDATE", "1")],
        },
        new TweakDef
        {
            Id = "scoop-parallel-downloads",
            Label = "Enable Scoop Parallel Downloads (aria2)",
            Category = "Scoop Tools",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets SCOOP_ARIA2_ENABLED=true to enable parallel downloads via aria2 for faster Scoop package installs. Default: disabled. Recommended: enabled.",
            Tags = ["scoop", "parallel", "downloads", "aria2", "speed"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED", "true"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_ARIA2_ENABLED"),
            ],
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
            Description = "Sets the Scoop global apps install directory to C:\\ScoopGlobal. Keeps system programs organised. Default: %ProgramData%\\scoop.",
            Tags = ["scoop", "global", "install-path", "directory"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Environment"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Environment", "SCOOP_GLOBAL", @"C:\ScoopGlobal")],
        },
    ];
}
