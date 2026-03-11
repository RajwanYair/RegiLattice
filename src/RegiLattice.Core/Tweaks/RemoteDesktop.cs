namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RemoteDesktop
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rdp-require-nla",
            Label = "Require NLA for RDP",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Require Network Level Authentication before RDP session. Default: enabled. Recommended: enabled for security.",
            Tags = ["rdp", "nla", "authentication", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1)],
        },
        new TweakDef
        {
            Id = "rdp-high-encryption",
            Label = "RDP High Encryption Level",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Set RDP minimum encryption to High (128-bit). Default: client-compatible. Recommended: high.",
            Tags = ["rdp", "encryption", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "MinEncryptionLevel", 3),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "MinEncryptionLevel", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "MinEncryptionLevel", 3)],
        },
        new TweakDef
        {
            Id = "rdp-disable-remote-assistance",
            Label = "Disable Remote Assistance",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disable offering and requesting Remote Assistance. Default: enabled. Recommended: disabled if unused.",
            Tags = ["remote", "assistance", "help", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0)],
        },
        new TweakDef
        {
            Id = "rdp-disable-rdp",
            Label = "Disable Remote Desktop",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Remote Desktop connections entirely. Default: Depends on edition. Recommended: Disabled if unused.",
            Tags = ["rdp", "remote", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-shadow",
            Label = "Disable RDP Session Shadowing",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables remote session shadowing/observation via RDP. Default: Allowed. Recommended: Disabled for privacy.",
            Tags = ["rdp", "shadow", "observation", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 0)],
        },
        new TweakDef
        {
            Id = "rdp-disable-printer-policy",
            Label = "Disable RDP Printer Redirection (Policy + WinStation)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables printer redirection in RDP via both policy and WinStation config. Blocks client printers from mapping to RDP sessions. Default: allowed. Recommended: disabled.",
            Tags = ["rdp", "printer", "redirect", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "fDisableCpm", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm"),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "fDisableCpm", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-wallpaper",
            Label = "Disable Desktop Wallpaper in RDP Sessions",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables desktop wallpaper rendering in RDP sessions to reduce bandwidth. Improves performance over slow connections. Default: Enabled. Recommended: Disabled.",
            Tags = ["rdp", "wallpaper", "performance", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-font-smoothing",
            Label = "Enable Font Anti-Aliasing in RDP Sessions",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Allows ClearType / font anti-aliasing in Remote Desktop sessions. Improves text readability at cost of slight bandwidth increase. Default: Disabled. Recommended: Enabled for clarity.",
            Tags = ["rdp", "font", "cleartype", "smoothing", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-audio-record",
            Label = "Disable Audio Recording Redirection in RDP",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables audio capture/microphone redirection from the client to the RDP session. Reduces attack surface and bandwidth. Default: Enabled. Recommended: Disabled.",
            Tags = ["rdp", "audio", "microphone", "record", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-compression",
            Label = "Enable RDP Data Compression",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables compression of RDP session data to reduce bandwidth usage. Useful for connections over slower networks. Default: Disabled. Recommended: Enabled.",
            Tags = ["rdp", "compression", "bandwidth", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "CompressedData", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "CompressedData"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "CompressedData", 1)],
        },
        new TweakDef
        {
            Id = "rdp-single-session",
            Label = "Restrict to Single RDP Session Per User",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Limits each user to a single concurrent RDP session, reconnecting to an existing session rather than creating a new one. Default: Multiple sessions. Recommended: Single session.",
            Tags = ["rdp", "session", "single", "reconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 1)],
        },
        new TweakDef
        {
            Id = "rdp-set-max-idle-time-15min",
            Label = "Set RDP Max Idle Time to 15 Minutes",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disconnects idle RDP sessions after 15 minutes (900000 ms). Frees resources and improves security. Default: no limit.",
            Tags = ["rdp", "idle", "timeout", "session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime", 900000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime", 900000)],
        },
        new TweakDef
        {
            Id = "rdp-disable-clipboard-redirection",
            Label = "Disable RDP Clipboard Redirection",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables clipboard sharing between RDP client and server. Prevents data leakage. Default: enabled.",
            Tags = ["rdp", "clipboard", "redirection", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-drive-redirection",
            Label = "Disable RDP Drive Redirection",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables mapping local drives in RDP sessions. Prevents file access from remote to local. Default: enabled.",
            Tags = ["rdp", "drive", "redirection", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-set-encryption-high",
            Label = "Set RDP Encryption Level to High",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets RDP encryption to High (128-bit). Ensures maximum encryption for client-server communication. Default: client-compatible.",
            Tags = ["rdp", "encryption", "security", "high"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MinEncryptionLevel", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MinEncryptionLevel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MinEncryptionLevel", 3)],
        },
        new TweakDef
        {
            Id = "rdp-enable-restricted-admin-mode",
            Label = "Enable RDP Restricted Admin Mode",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables Restricted Admin mode for RDP. Prevents credential delegation to remote hosts, mitigating pass-the-hash. Default: disabled.",
            Tags = ["rdp", "restricted-admin", "security", "credentials"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin", 0)],
        },
    ];
}
