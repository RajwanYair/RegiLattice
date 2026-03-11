namespace RegiLattice.Core.Tweaks;

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
