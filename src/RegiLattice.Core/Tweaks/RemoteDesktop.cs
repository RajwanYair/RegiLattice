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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "UserAuthentication", 1),
            ],
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "MinEncryptionLevel", 3),
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowToGetHelp", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "Shadow", 0)],
        },
        new TweakDef
        {
            Id = "rdp-disable-printer-policy",
            Label = "Disable RDP Printer Redirection (Policy + WinStation)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables printer redirection in RDP via both policy and WinStation config. Blocks client printers from mapping to RDP sessions. Default: allowed. Recommended: disabled.",
            Tags = ["rdp", "printer", "redirect", "policy"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp",
            ],
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
            Description =
                "Disables desktop wallpaper rendering in RDP sessions to reduce bandwidth. Improves performance over slow connections. Default: Enabled. Recommended: Disabled.",
            Tags = ["rdp", "wallpaper", "performance", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableWallpaper", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-font-smoothing",
            Label = "Enable Font Anti-Aliasing in RDP Sessions",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Allows ClearType / font anti-aliasing in Remote Desktop sessions. Improves text readability at cost of slight bandwidth increase. Default: Disabled. Recommended: Enabled for clarity.",
            Tags = ["rdp", "font", "cleartype", "smoothing", "display"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowFontAntiAlias", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-audio-record",
            Label = "Disable Audio Recording Redirection in RDP",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables audio capture/microphone redirection from the client to the RDP session. Reduces attack surface and bandwidth. Default: Enabled. Recommended: Disabled.",
            Tags = ["rdp", "audio", "microphone", "record", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-compression",
            Label = "Enable RDP Data Compression",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables compression of RDP session data to reduce bandwidth usage. Useful for connections over slower networks. Default: Disabled. Recommended: Enabled.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "CompressedData", 1),
            ],
        },
        new TweakDef
        {
            Id = "rdp-single-session",
            Label = "Restrict to Single RDP Session Per User",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits each user to a single concurrent RDP session, reconnecting to an existing session rather than creating a new one. Default: Multiple sessions. Recommended: Single session.",
            Tags = ["rdp", "session", "single", "reconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser")],
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
            Description =
                "Sets RDP encryption to High (128-bit). Ensures maximum encryption for client-server communication. Default: client-compatible.",
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
            Description =
                "Enables Restricted Admin mode for RDP. Prevents credential delegation to remote hosts, mitigating pass-the-hash. Default: disabled.",
            Tags = ["rdp", "restricted-admin", "security", "credentials"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Lsa", "DisableRestrictedAdmin", 0)],
        },
        new TweakDef
        {
            Id = "rdp-bitmap-caching",
            Label = "Enable RDP Bitmap Caching",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables bitmap caching for RDP sessions. Improves rendering performance by caching frequently displayed images locally. Default: enabled.",
            Tags = ["rdp", "bitmap", "caching", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowCacheBitmaps", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowCacheBitmaps")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AllowCacheBitmaps", 1)],
        },
        new TweakDef
        {
            Id = "rdp-change-port-3390",
            Label = "Change RDP Port to 3390",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Changes the RDP listening port from 3389 to 3390. Reduces automated scan hits on the default port. Default: 3389.",
            Tags = ["rdp", "port", "security", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "PortNumber", 3390),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "PortNumber", 3389),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "PortNumber", 3390),
            ],
        },
        new TweakDef
        {
            Id = "rdp-disable-clipboard-redirect",
            Label = "Disable RDP Clipboard Redirect (Policy)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables clipboard redirection in RDP sessions via Group Policy. Prevents clipboard content from being shared between local and remote. Default: allowed.",
            Tags = ["rdp", "clipboard", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableClip", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-drive-redirect",
            Label = "Disable RDP Drive Redirect (Policy)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables drive redirection in RDP sessions via Group Policy. Prevents local drives from being accessible in the remote session. Default: allowed.",
            Tags = ["rdp", "drive", "redirect", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCdm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-printer-redirect",
            Label = "Disable RDP Printer Redirect (Policy)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables printer redirection in RDP sessions via Group Policy. Prevents local printers from being mapped in the remote session. Default: allowed.",
            Tags = ["rdp", "printer", "redirect", "security", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCpm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-keepalive",
            Label = "Enable RDP Keep-Alive (1 min)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables RDP keep-alive packets every 60 seconds. Prevents idle sessions from being disconnected by firewalls or proxies. Default: off.",
            Tags = ["rdp", "keepalive", "timeout", "connection"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "KeepAliveEnable", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "KeepAliveInterval", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "KeepAliveEnable"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "KeepAliveInterval"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "KeepAliveEnable", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enable-remote-desktop",
            Label = "Enable Remote Desktop",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables Remote Desktop connections on this machine. Opens the system for incoming RDP connections. Default: disabled.",
            Tags = ["rdp", "remote-desktop", "access", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fDenyTSConnections", 0)],
        },
        new TweakDef
        {
            Id = "rdp-idle-timeout-15m",
            Label = "Set RDP Idle Timeout to 15 Minutes",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the RDP idle session timeout to 15 minutes (900000ms). Disconnects idle sessions to free resources. Default: no timeout.",
            Tags = ["rdp", "idle", "timeout", "session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime", 900000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxIdleTime", 900000)],
        },
        new TweakDef
        {
            Id = "rdp-security-layer-ssl",
            Label = "Set RDP Security Layer to SSL/TLS",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the RDP security layer to SSL/TLS (2). Ensures encrypted transport for RDP sessions. Default: Negotiate.",
            Tags = ["rdp", "ssl", "tls", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 2),
            ],
        },
        new TweakDef
        {
            Id = "rdp-session-timeout-30m",
            Label = "Set RDP Session Timeout to 30 Minutes",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the RDP disconnected session timeout to 30 minutes (1800000ms). Logs off disconnected sessions after 30 minutes. Default: no timeout.",
            Tags = ["rdp", "session", "timeout", "disconnect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxDisconnectionTime", 1800000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxDisconnectionTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxDisconnectionTime", 1800000),
            ],
        },
        new TweakDef
        {
            Id = "rdp-set-max-connections-unlimited",
            Label = "Allow Unlimited Concurrent RDP Connections",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Removes the default single-session limit for concurrent RDP connections. Requires appropriate Windows Server or RDSH licensing. Useful on multi-user workstations.",
            Tags = ["rdp", "connections", "concurrent", "sessions", "unlimited"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server", "fSingleSessionPerUser", 0)],
        },
        new TweakDef
        {
            Id = "rdp-set-color-depth-32",
            Label = "Set RDP Session Colour Depth to 32-bit",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces Remote Desktop sessions to use 32-bit colour depth for better visual quality. Default: 32-bit in most configurations, but may be lower on constrained connections.",
            Tags = ["rdp", "color", "depth", "quality", "visual"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "ColorDepth", 5)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "ColorDepth")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "ColorDepth", 5)],
        },
        new TweakDef
        {
            Id = "rdp-disable-smart-card-redirection",
            Label = "Disable Smart Card Redirection in RDP",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents physical smart card readers from being redirected into Remote Desktop sessions, reducing the attack surface for credential theft via smart cards.",
            Tags = ["rdp", "smart-card", "redirect", "security", "credentials"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fEnableSmartCard", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fEnableSmartCard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fEnableSmartCard", 0)],
        },
        new TweakDef
        {
            Id = "rdp-set-remote-assistance-off",
            Label = "Disable Windows Remote Assistance (Legacy)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the legacy Windows Remote Assistance feature (msra.exe) via policy, which is separate from the modern Quick Assist. Remote Assistance is rarely needed and can be exploited.",
            Tags = ["rdp", "remote-assistance", "legacy", "security", "msra"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowFullControl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowFullControl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance", "fAllowFullControl", 0)],
        },
        new TweakDef
        {
            Id = "rdp-set-audio-play-on-server",
            Label = "Set RDP Audio Playback to Remote Server",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures RDP sessions to play audio on the remote server rather than redirecting it to the client device. Reduces bandwidth for sessions where local audio is not required.",
            Tags = ["rdp", "audio", "redirect", "bandwidth", "server"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AudioRedirectionMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AudioRedirectionMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "AudioRedirectionMode", 1)],
        },
        new TweakDef
        {
            Id = "rdp-disable-com-port-redirect",
            Label = "Disable RDP COM Port Redirection",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents COM (serial) port hardware from being redirected to Remote Desktop sessions. Reduces attack surface on systems with serial port equipment.",
            Tags = ["rdp", "com", "serial", "port", "redirect", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableCcm", 1)],
        },
        new TweakDef
        {
            Id = "rdp-enforce-tls-security-layer",
            Label = "Enforce TLS Security Layer for RDP",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Forces RDP to use TLS for transport security instead of falling back to RDP Security layer. Prevents downgrade attacks. Value 2 = SSL/TLS required.",
            Tags = ["rdp", "tls", "security", "encryption", "hardening"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 1),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp", "SecurityLayer", 2),
            ],
        },
        new TweakDef
        {
            Id = "rdp-limit-single-monitor",
            Label = "Limit RDP Session to Single Monitor",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Restricts Remote Desktop sessions to a single display, reducing bandwidth usage and preventing multi-monitor spanning.",
            Tags = ["rdp", "monitor", "display", "bandwidth", "restriction"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxMonitors", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxMonitors")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxMonitors", 1)],
        },
        new TweakDef
        {
            Id = "rdp-set-connection-timeout-8h",
            Label = "Set Maximum RDP Session Duration to 8 Hours",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Limits active RDP sessions to 8 hours (28800000ms) to enforce session rotation and prevent forgotten sessions from consuming server resources indefinitely.",
            Tags = ["rdp", "session", "timeout", "duration", "limit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxConnectionTime", 28800000),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxConnectionTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "MaxConnectionTime", 28800000),
            ],
        },
        new TweakDef
        {
            Id = "rdp-disable-lpt-port-redirect",
            Label = "Disable RDP LPT Port Redirection",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents parallel (LPT) printer ports from being redirected to Remote Desktop sessions, commonly used by legacy printers and industrial equipment.",
            Tags = ["rdp", "lpt", "parallel", "printer", "redirect", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableLPT", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableLPT")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services", "fDisableLPT", 1)],
        },
    ];
}

// ── Remote Management ─────────────────────────────────────────────────────────
// Merged from RemoteManagement.cs (WinRM policy hardening and RPC restriction tweaks)

internal static class RemoteManagement
{
    private const string WinRmSvcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
    private const string WinRmCliPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
    private const string RpcPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";
    private const string WinRmSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rmt-disable-winrm-service",
            Label = "Disable WinRM Service",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the WinRM (Windows Remote Management) service start type to Disabled. "
                + "Prevents PowerShell Remoting and remote WMI sessions when no remote management is needed. "
                + "Default: Manual. Recommended: Disabled on workstations not managed via WinRM.",
            Tags = ["winrm", "remote", "powershell", "security", "hardening"],
            RegistryKeys = [WinRmSvc],
            ApplyOps = [RegOp.SetDword(WinRmSvc, "Start", 4)],
            RemoveOps = [RegOp.SetDword(WinRmSvc, "Start", 3)],
            DetectOps = [RegOp.CheckDword(WinRmSvc, "Start", 4)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-unencrypted-server",
            Label = "Block Unencrypted WinRM Traffic (Server)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowUnencryptedTraffic=0 for the WinRM service (server-side). "
                + "Forces all incoming WinRM sessions to use HTTPS/encrypted transport. "
                + "Prevents credential interception on WinRM connections. Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "encryption", "security", "policy", "hardening"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "AllowUnencryptedTraffic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "AllowUnencryptedTraffic")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "AllowUnencryptedTraffic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-basic-auth-server",
            Label = "Block Basic Authentication in WinRM Server",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowBasic=0 for the WinRM service. Prevents clients from authenticating using "
                + "Basic HTTP authentication (plaintext Base64 credentials). Kerberos/Negotiate remain available. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "security", "policy", "hardening"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-unencrypted-client",
            Label = "Block Unencrypted WinRM Traffic (Client)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowUnencryptedTraffic=0 for the WinRM client. "
                + "Prevents this machine from initiating unencrypted WinRM sessions to remote hosts. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "encryption", "security", "policy", "client"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowUnencryptedTraffic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowUnencryptedTraffic")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowUnencryptedTraffic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-basic-auth-client",
            Label = "Block Basic Authentication in WinRM Client",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowBasic=0 for the WinRM client. Prevents using Basic authentication "
                + "when connecting to WinRM servers. Closes NTLM relay paths via Basic credentials. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "security", "policy", "client"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowBasic", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowBasic")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowBasic", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-digest-auth",
            Label = "Block Digest Authentication in WinRM Client",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowDigest=0 for the WinRM client. Digest authentication uses MD5 hashing "
                + "and is considered weak. Blocking it forces Kerberos/Negotiate. "
                + "Default: allowed. Recommended: blocked.",
            Tags = ["winrm", "authentication", "digest", "security", "policy"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowDigest", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowDigest")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowDigest", 0)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-block-credssp",
            Label = "Block CredSSP in WinRM Client",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets policy AllowCredSSP=0 for the WinRM client. CredSSP delegates credentials to the "
                + "remote host, creating credential theft risk on compromised endpoints. "
                + "Default: allowed. Recommended: blocked unless explicitly needed.",
            Tags = ["winrm", "credssp", "credential", "security", "policy"],
            RegistryKeys = [WinRmCliPol],
            ApplyOps = [RegOp.SetDword(WinRmCliPol, "AllowCredSSP", 0)],
            RemoveOps = [RegOp.DeleteValue(WinRmCliPol, "AllowCredSSP")],
            DetectOps = [RegOp.CheckDword(WinRmCliPol, "AllowCredSSP", 0)],
        },
        new TweakDef
        {
            Id = "rmt-restrict-rpc-clients",
            Label = "Restrict Unauthenticated RPC Clients",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RPC policy RestrictRemoteClients=1 to block unauthenticated remote RPC connections. "
                + "Authenticated connections and loopback are still permitted. "
                + "Default: 0 (no restriction). Recommended: 1 on servers/workstations.",
            Tags = ["rpc", "remote", "authentication", "security", "policy"],
            RegistryKeys = [RpcPol],
            ApplyOps = [RegOp.SetDword(RpcPol, "RestrictRemoteClients", 1)],
            RemoveOps = [RegOp.DeleteValue(RpcPol, "RestrictRemoteClients")],
            DetectOps = [RegOp.CheckDword(RpcPol, "RestrictRemoteClients", 1)],
        },
        new TweakDef
        {
            Id = "rmt-require-rpc-auth-ep",
            Label = "Require Authentication for RPC Endpoint Resolution",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets RPC policy EnableAuthEpResolution=1. The RPC endpoint mapper requires "
                + "clients to authenticate when resolving remote endpoints. "
                + "Prevents unauthenticated enumeration of RPC services. Default: 0.",
            Tags = ["rpc", "endpoint", "authentication", "security", "policy"],
            RegistryKeys = [RpcPol],
            ApplyOps = [RegOp.SetDword(RpcPol, "EnableAuthEpResolution", 1)],
            RemoveOps = [RegOp.DeleteValue(RpcPol, "EnableAuthEpResolution")],
            DetectOps = [RegOp.CheckDword(RpcPol, "EnableAuthEpResolution", 1)],
        },
        new TweakDef
        {
            Id = "rmt-winrm-limit-shell-memory",
            Label = "Limit WinRM Shell Memory Per Session",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets MaxMemoryPerShellMB=150 for the WinRM service. Caps the amount of memory "
                + "each remote shell session can consume, preventing WinRM from being used as a "
                + "DoS vector on low-memory machines. Default: unlimited. Recommended: 150 MB.",
            Tags = ["winrm", "memory", "dos", "security", "policy"],
            RegistryKeys = [WinRmSvcPol],
            ApplyOps = [RegOp.SetDword(WinRmSvcPol, "MaxMemoryPerShellMB", 150)],
            RemoveOps = [RegOp.DeleteValue(WinRmSvcPol, "MaxMemoryPerShellMB")],
            DetectOps = [RegOp.CheckDword(WinRmSvcPol, "MaxMemoryPerShellMB", 150)],
        },
    ];
}

// ── Merged from RealVnc.cs ──────────────────────────────────────────────────

internal static class RealVnc
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vnc-enforce-encryption",
            Label = "VNC: Enforce Encryption",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces VNC Server to use 'AlwaysOn' encryption for all connections.",
            Tags = ["vnc", "security", "encryption"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "Encryption", "AlwaysOn"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "PreferOn"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "Encryption"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn")],
        },
        new TweakDef
        {
            Id = "vnc-strong-auth",
            Label = "VNC: Strong Authentication",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets VNC authentication to VncAuth + System authentication.",
            Tags = ["vnc", "security", "authentication"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SingleSignOn")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth")],
        },
        new TweakDef
        {
            Id = "vnc-idle-timeout",
            Label = "VNC: 1-Hour Idle Timeout",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disconnects idle VNC sessions after 1 hour (3600s).",
            Tags = ["vnc", "security", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600)],
        },
        new TweakDef
        {
            Id = "vnc-blank-screen",
            Label = "VNC: Blank Screen When Connected",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blanks the local monitor during an active VNC session for privacy.",
            Tags = ["vnc", "security", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "WhenConnected")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "Never")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "WhenConnected")],
        },
        new TweakDef
        {
            Id = "vnc-no-clipboard",
            Label = "VNC: Disable Clipboard Sharing",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables clipboard sharing between VNC server and viewer (DLP).",
            Tags = ["vnc", "security", "clipboard", "dlp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AcceptCutText", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "SendCutText", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "AcceptCutText", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "SendCutText", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AcceptCutText", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "SendCutText", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "AcceptCutText"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "SendCutText"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AcceptCutText", 0)],
        },
        new TweakDef
        {
            Id = "vnc-realvnc-disable-auto-update",
            Label = "RealVNC Disable Auto-Update",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables RealVNC automatic update checks. Updates must be applied manually. Default: Enabled. Recommended: Disabled for managed deployments.",
            Tags = ["realvnc", "vnc", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "vnc-realvnc-optimize-encoding",
            Label = "RealVNC Optimize Encoding",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets VNC encoding to ZRLE for best compression ratio over slow networks. Reduces bandwidth usage. Default: Auto. Recommended: ZRLE for WAN connections.",
            Tags = ["realvnc", "vnc", "encoding", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding", "ZRLE")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding", "ZRLE")],
        },
        new TweakDef
        {
            Id = "vnc-session-timeout",
            Label = "VNC: Set Idle Session Timeout (30 min)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets VNC idle session timeout to 30 minutes (1800 seconds). Automatically disconnects idle VNC sessions for security. Default: no timeout. Recommended: 1800.",
            Tags = ["vnc", "timeout", "idle", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 1800)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 1800)],
        },
        new TweakDef
        {
            Id = "vnc-disable-clipboard",
            Label = "VNC: Disable Clipboard Sharing (DWord)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables clipboard sharing between VNC server and clients via DWORD value. Prevents data leakage through clipboard transfer. Default: enabled. Recommended: disabled for DLP.",
            Tags = ["vnc", "clipboard", "dlp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard", 1)],
        },
        new TweakDef
        {
            Id = "vnc-encryption-always",
            Label = "VNC: Enforce Encryption Always On (Policy)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Forces VNC encryption to AlwaysOn via group policy key and sets EncryptionForced flag. Ensures connections are always encrypted regardless of server config. Default: PreferOn. Recommended: AlwaysOn.",
            Tags = ["vnc", "encryption", "policy", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn"),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "Encryption", "AlwaysOn"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "PreferOn"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "Encryption"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn")],
        },
        new TweakDef
        {
            Id = "vnc-disable-file-transfer",
            Label = "VNC: Disable File Transfer",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables file transfer capability in VNC sessions. Prevents users from transferring files via the VNC connection. Default: enabled. Recommended: disabled for DLP.",
            Tags = ["vnc", "file-transfer", "dlp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "EnableFileTransfer", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "EnableFileTransfer", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "EnableFileTransfer", 1),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "EnableFileTransfer"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "EnableFileTransfer", 0)],
        },
        new TweakDef
        {
            Id = "vnc-auth-system",
            Label = "VNC: Set Authentication to SystemAuth",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets VNC authentication to SystemAuth (Windows credentials). Uses OS-level authentication instead of VNC-specific password. Default: VncAuth. Recommended: SystemAuth for enterprise.",
            Tags = ["vnc", "auth", "system", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SingleSignOn")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SystemAuth")],
        },
        new TweakDef
        {
            Id = "vnc-query-on-connect",
            Label = "Prompt User on Incoming VNC Connection",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables QueryConnect — shows a dialog on the physical machine asking the logged-in user to accept or reject each incoming VNC connection. Default: disabled. Recommended: enabled for attended machines.",
            Tags = ["vnc", "security", "query", "connect", "prompt"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect", 1),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "QueryConnect", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver", "QueryConnect"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect", 1)],
        },
        new TweakDef
        {
            Id = "vnc-lock-on-disconnect",
            Label = "Lock Screen When VNC Session Ends",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets DisconnectAction=Lock so the screen locks when a VNC session cleanly disconnects. Prevents leaving an unlocked desktop after remote access. Default: Nothing. Recommended: Lock.",
            Tags = ["vnc", "security", "disconnect", "lock", "session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Lock")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Nothing")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Lock")],
        },
        new TweakDef
        {
            Id = "vnc-lost-conn-lock",
            Label = "Lock Screen When VNC Connection Drops",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets LostConnAction=Lock so the screen locks when a VNC connection is unexpectedly terminated (network drop, client crash). Default: Nothing. Recommended: Lock.",
            Tags = ["vnc", "security", "lost-connection", "lock", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Lock")],
            RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Nothing")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Lock")],
        },
        new TweakDef
        {
            Id = "vnc-viewer-fullscreen",
            Label = "VNC Viewer: Open in Fullscreen by Default",
            Category = "Remote Desktop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures RealVNC Viewer to open remote sessions in fullscreen mode automatically, maximising the workspace for remote control. Default: windowed. Recommended: fullscreen for power users.",
            Tags = ["vnc", "viewer", "fullscreen", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen", 1)],
        },
        new TweakDef
        {
            Id = "vnc-disable-share-desktop",
            Label = "VNC: Disable Desktop Sharing (Exclusive Access)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables desktop sharing so only one VNC viewer can connect at a time (exclusive access). Prevents multiple simultaneous viewers from watching a session. Default: shared. Recommended: exclusive.",
            Tags = ["vnc", "security", "share", "exclusive", "access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop", 0)],
        },
        new TweakDef
        {
            Id = "vnc-set-idle-timeout-300",
            Label = "Set VNC Idle Timeout to 5 Minutes",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disconnects VNC sessions after 5 minutes of inactivity. Default: no timeout.",
            Tags = ["vnc", "idle", "timeout", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 300)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 300)],
        },
        new TweakDef
        {
            Id = "vnc-enable-query-connect",
            Label = "Enable VNC Query Connect Prompt",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Requires the local user to accept each connection before granting access. Default: disabled.",
            Tags = ["vnc", "query", "connect", "prompt", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "QueryConnect", 1)],
        },
        new TweakDef
        {
            Id = "vnc-disable-clipboard-sync",
            Label = "Disable VNC Clipboard Synchronization",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables clipboard sharing between VNC client and server. Prevents copy-paste data leakage. Default: enabled.",
            Tags = ["vnc", "clipboard", "sync", "disable", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "SendCutText", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "SendCutText")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "SendCutText", 0)],
        },
        new TweakDef
        {
            Id = "vnc-enable-encryption-always",
            Label = "Enforce VNC Encryption (Always On)",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Forces all VNC connections to use encryption. Connections without encryption support are rejected. Default: prefer on.",
            Tags = ["vnc", "encryption", "security", "enforce"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption")],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Encryption", "AlwaysOn")],
        },
        new TweakDef
        {
            Id = "vnc-set-idle-timeout-3600",
            Label = "Set VNC Idle Timeout to 1 Hour",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disconnects idle VNC sessions after 1 hour. Frees resources and improves security. Default: 0 (no timeout).",
            Tags = ["vnc", "idle", "timeout", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600)],
        },
        new TweakDef
        {
            Id = "vnc-hide-tray",
            Label = "Hide VNC Server Tray Icon",
            Category = "Remote Desktop",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Hides the VNC Server system tray icon. Keeps VNC running without a visible indicator. Useful for kiosk or embedded scenarios. Default: shown.",
            Tags = ["vnc", "tray", "icon", "hide"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShowTrayIcon", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShowTrayIcon", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShowTrayIcon", 0)],
        },
        new TweakDef
        {
            Id = "vnc-viewer-fitwindow",
            Label = "VNC Viewer Fit to Window",
            Category = "Remote Desktop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Configures VNC Viewer to automatically scale the remote desktop to fit the client window. Default: native resolution.",
            Tags = ["vnc", "viewer", "scaling", "fitwindow"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "Scaling", "FitWindow")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "Scaling")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "Scaling", "FitWindow")],
        },
        new TweakDef
        {
            Id = "vnc-viewer-recent",
            Label = "Disable VNC Viewer Recent Connections",
            Category = "Remote Desktop",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables VNC Viewer from storing recent connection history. Enhances privacy by not recording server addresses. Default: stored.",
            Tags = ["vnc", "viewer", "recent", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections", 0)],
        },
    ];
}

// ── merged from PolicyRemoteAccess.cs ──
// RegiLattice.Core — Tweaks/PolicyRemoteAccess.cs
// RDP, WinRM, remote assistance, terminal services, WDAG, remote procedure call, and PowerShell JEA policies
// Category: "Remote Access Policy"
// Consolidated from 14 modules.

internal static class PolicyRemoteAccess
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _ClipboardRdpRedirectionPolicy.Data,
            .. _RdpClientPolicy.Data,
            .. _RemoteAssistancePolicy.Data,
            .. _RemoteCredentialGuardPolicy.Data,
            .. _RemoteProcedureCallPolicy.Data,
            .. _RemotePsJeaPolicy.Data,
            .. _TerminalServicesAdvPolicy.Data,
            .. _TerminalServicesPolicy.Data,
            .. _WdagFileCachePolicy.Data,
            .. _WdagPolicy.Data,
            .. _WinRmHardening.Data,
            .. _WinRmPolicy.Data,
            .. _WinRmRemoteShellPolicy.Data,
            .. _WinRmSecurityPolicy.Data,
        ];

    // ── ClipboardRdpRedirectionPolicy ──
    private static class _ClipboardRdpRedirectionPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "cliprdp-disable-clipboard-redirection",
                    Label = "Disable Clipboard Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fDisableClip=1 to prevent clipboard contents from being shared between the RDP client and the remote session, blocking data exfiltration via clipboard.",
                    Tags = ["rdp", "clipboard", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard copy-paste between local and remote session is blocked. Copy-paste within the session still works.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableClip", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableClip")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableClip", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-drive-redirection",
                    Label = "Disable Drive Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fDisableCdm=1 to prevent local drives from being mapped into the remote session, blocking file transfer via mapped drives.",
                    Tags = ["rdp", "drive", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Local drives are not accessible inside RDP sessions; file sharing via drive mapping is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableCdm", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableCdm")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableCdm", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-printer-redirection",
                    Label = "Disable Printer Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fDisableCpm=1 to prevent local printers from being redirected into RDP sessions, blocking potentially sensitive print jobs from reaching the remote host.",
                    Tags = ["rdp", "printer", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Local printers not available in RDP session; use remote printers instead.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableCpm", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableCpm")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableCpm", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-com-port-redirection",
                    Label = "Disable COM Port Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description = "Sets fDisableCcm=1 to prevent local COM (serial) ports from being redirected into RDP sessions.",
                    Tags = ["rdp", "com-port", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Local COM ports not accessible in RDP; serial device data cannot be exfiltrated.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableCcm", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableCcm")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableCcm", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-lpt-redirection",
                    Label = "Disable LPT Port Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description = "Sets fDisableLPT=1 to prevent local parallel (LPT) ports from being redirected into the remote session.",
                    Tags = ["rdp", "lpt", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Local LPT ports not available in RDP session.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableLPT", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableLPT")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableLPT", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-smart-card-redirection",
                    Label = "Disable Smart Card Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fDisableSCard=1 to prevent local smart cards from being redirected into the remote session, mitigating remote authentication using locally inserted smart cards.",
                    Tags = ["rdp", "smart-card", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Smart cards not forwarded into remote session; use remote card readers instead.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableSCard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableSCard")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableSCard", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-audio-recording-redirection",
                    Label = "Disable Audio Recording Redirection in RDP",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fDisableAudioCapture=1 to prevent the remote session from recording audio through the local client's microphone.",
                    Tags = ["rdp", "audio", "microphone", "redirection", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Remote session cannot access local microphone; audio recording in session is disabled.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableAudioCapture", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableAudioCapture")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableAudioCapture", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-clipboard-file-copy",
                    Label = "Disable Clipboard File Copy from RDP Session",
                    Category = "Remote Desktop",
                    Description =
                        "Disables the ability to copy files via clipboard between the remote session and the local desktop, supplementing fDisableClip for file-drag exfiltration prevention.",
                    Tags = ["rdp", "clipboard", "file-copy", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "File drag-and-drop and clipboard file copy blocked between sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableClipboardFileTransfer", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableClipboardFileTransfer")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableClipboardFileTransfer", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-usb-redirection",
                    Label = "Disable USB Device Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description = "Disables USB device redirection so that locally connected USB devices are not forwarded into the remote session.",
                    Tags = ["rdp", "usb", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "USB devices not available in remote session; prevents USB-based data exfiltration.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisableUSBRedir", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisableUSBRedir")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisableUSBRedir", 1)],
                },
                new TweakDef
                {
                    Id = "cliprdp-disable-pnp-redirection",
                    Label = "Disable PnP Device Redirection in RDP Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Disables Plug-and-Play device redirection so locally connected PnP devices (cameras, scanners, etc.) are not accessible from the remote session.",
                    Tags = ["rdp", "pnp", "redirection", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "PnP devices not forwarded into remote session; cameras and scanners inaccessible.",
                    ApplyOps = [RegOp.SetDword(Key, "fDisablePNPRedir", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fDisablePNPRedir")],
                    DetectOps = [RegOp.CheckDword(Key, "fDisablePNPRedir", 1)],
                },
            ];
    }

    // ── RdpClientPolicy ──
    private static class _RdpClientPolicy
    {
        private const string RdpClientPolicyKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\Client";
        private const string RdpClientUserKey = @"HKEY_CURRENT_USER\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
        private const string RdpServerPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rdpclt-require-nla",
                Label = "RDP Client: Require Network Level Authentication (NLA)",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 5,
                SafetyRating = 5,
                RegistryKeys = [RdpClientPolicyKey],
                Tags = ["rdp", "remote-desktop", "nla", "authentication", "security", "hardening"],
                Description =
                    "Sets fRequireNLA=1 in Terminal Services\\Client policy. "
                    + "Enforces Network Level Authentication before the RDP connection is established. "
                    + "NLA authenticates the user before the full desktop session is created, preventing "
                    + "pre-authentication credential-theft attacks and reducing the attack surface of the "
                    + "Remote Desktop logon screen.",
                ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "fRequireNLA", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "fRequireNLA")],
                DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "fRequireNLA", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-warn-on-auth-fail",
                Label = "RDP Client: Warn When Server Authentication Fails",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpClientPolicyKey],
                Tags = ["rdp", "remote-desktop", "server-authentication", "certificate", "security", "mitm"],
                Description =
                    "Sets AuthenticationLevel=1 in Terminal Services\\Client policy. "
                    + "Prompts the user with a warning before connecting when the server's identity "
                    + "certificate cannot be verified. 0=always connect, 1=warn (default), 2=never connect "
                    + "if auth fails. Setting 1 ensures users see MITM warnings instead of connecting silently.",
                ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "AuthenticationLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "AuthenticationLevel")],
                DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "AuthenticationLevel", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-deny-on-auth-fail",
                Label = "RDP Client: Block Connection When Server Authentication Fails",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 4,
                RegistryKeys = [RdpClientPolicyKey],
                Tags = ["rdp", "remote-desktop", "server-authentication", "certificate", "security", "hardening", "mitm"],
                Description =
                    "Sets AuthenticationLevel=2 in Terminal Services\\Client policy. "
                    + "Blocks the RDP connection entirely if the server's identity certificate cannot be "
                    + "verified. Provides the strongest protection against MITM interception of RDP sessions. "
                    + "Requires valid certificates on all RDP servers.",
                ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "AuthenticationLevel", 2)],
                RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "AuthenticationLevel")],
                DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "AuthenticationLevel", 2)],
            },
            new TweakDef
            {
                Id = "rdpclt-disable-clipboard-redirection",
                Label = "RDP Client: Disable Clipboard Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "clipboard", "data-exfiltration", "security", "dlp"],
                Description =
                    "Sets fDisableClip=1 in Terminal Services server policy. "
                    + "Prevents clipboard content from being shared between the RDP client and the remote "
                    + "server session. Blocks a common data-exfiltration vector where users copy sensitive "
                    + "data between the corporate remote desktop and their local machine.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableClip", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableClip")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableClip", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-disable-drive-redirection",
                Label = "RDP Client: Disable Local Drive Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "drive-redirection", "data-exfiltration", "security", "dlp"],
                Description =
                    "Sets fDisableCdm=1 in Terminal Services server policy. "
                    + "Blocks access to local client drives (C:, D:, USB, etc.) from within the RDP "
                    + "session. Prevents data exfiltration via drag-and-drop or file copy between the "
                    + "remote session and local storage.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableCdm", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableCdm")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableCdm", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-disable-printer-redirection",
                Label = "RDP Client: Disable Printer Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 2,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "printer", "redirection", "security"],
                Description =
                    "Sets fDisableCpm=1 in Terminal Services server policy. "
                    + "Blocks local printers from being redirected into the RDP session. "
                    + "Reduces the attack surface by preventing spooler access from the remote session "
                    + "and blocking potential PrintNightmare-style printer driver exploitation via RDP.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fDisableCpm", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fDisableCpm")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fDisableCpm", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-force-encryption-high",
                Label = "RDP Client: Enforce High (128-Bit) RDP Encryption",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 4,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "encryption", "tls", "security", "hardening"],
                Description =
                    "Sets MinEncryptionLevel=3 in Terminal Services policy (3=High/128-bit). "
                    + "Enforces 128-bit RC4 or TLS encryption for all RDP session data. "
                    + "Level 1=low, 2=medium (legacy 56-bit), 3=high, 4=FIPS-compliant. "
                    + "Modern RDP (TLS mode) supersedes this, but the policy prevents fallback to weak ciphers.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "MinEncryptionLevel", 3)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "MinEncryptionLevel")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "MinEncryptionLevel", 3)],
            },
            new TweakDef
            {
                Id = "rdpclt-set-session-timeout-30min",
                Label = "RDP Client: Disconnect Idle Sessions After 30 Minutes",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "idle-timeout", "security"],
                Description =
                    "Sets MaxIdleTime=1800000 (30 minutes in ms) in Terminal Services policy. "
                    + "Automatically disconnects RDP sessions that have been idle for more than 30 minutes. "
                    + "Reduces the risk of unattended remote sessions being hijacked and prevents license "
                    + "consumption by orphaned sessions.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "MaxIdleTime", 1800000)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "MaxIdleTime")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "MaxIdleTime", 1800000)],
            },
            new TweakDef
            {
                Id = "rdpclt-disable-password-save",
                Label = "RDP Client: Prevent Saving Passwords in Remote Desktop Client",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpClientPolicyKey],
                Tags = ["rdp", "remote-desktop", "credentials", "password", "security", "credential-manager"],
                Description =
                    "Sets DisablePasswordSaving=1 in Terminal Services\\Client policy. "
                    + "Disables the 'Save Password' option in the Remote Desktop Connection client (mstsc). "
                    + "Prevents RDP credentials from being stored in Windows Credential Manager where they "
                    + "can be extracted by credential-dumping tools.",
                ApplyOps = [RegOp.SetDword(RdpClientPolicyKey, "DisablePasswordSaving", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpClientPolicyKey, "DisablePasswordSaving")],
                DetectOps = [RegOp.CheckDword(RdpClientPolicyKey, "DisablePasswordSaving", 1)],
            },
            new TweakDef
            {
                Id = "rdpclt-enable-audit-logging",
                Label = "RDP Client: Enable RDP Session Audit Logging",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                MinBuild = 17763,
                ImpactScore = 3,
                SafetyRating = 5,
                RegistryKeys = [RdpServerPolicy],
                Tags = ["rdp", "remote-desktop", "audit", "logging", "security", "compliance", "event-log"],
                Description =
                    "Sets fLogonDisabled=0 and EnableLogonWatermarking=1 in Terminal Services policy. "
                    + "Ensures that RDP logon events are audited and that the watermarking feature "
                    + "(which embeds session metadata for forensics) is enabled. "
                    + "Supports SOC investigation of lateral movement via RDP.",
                ApplyOps = [RegOp.SetDword(RdpServerPolicy, "fLogonDisabled", 0), RegOp.SetDword(RdpServerPolicy, "EnableLogonWatermarking", 1)],
                RemoveOps = [RegOp.DeleteValue(RdpServerPolicy, "fLogonDisabled"), RegOp.DeleteValue(RdpServerPolicy, "EnableLogonWatermarking")],
                DetectOps = [RegOp.CheckDword(RdpServerPolicy, "fLogonDisabled", 0), RegOp.CheckDword(RdpServerPolicy, "EnableLogonWatermarking", 1)],
            },
        ];
    }

    // ── RemoteAssistancePolicy ──
    private static class _RemoteAssistancePolicy
    {
        private const string RaPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";
        private const string RaOffered = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services\RAUnsolicit";
        private const string RaRuntime = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Remote Assistance";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rast-disable-solicited-gpo",
                Label = "Remote Assistance: Block Solicited RA via GPO",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaPolicy],
                Tags = ["remote-assistance", "gpo", "policy", "security", "network"],
                Description =
                    "Sets fAllowToGetHelp=0 in Terminal Services policy. Prevents users from sending Remote Assistance "
                    + "invitations. Enforced as a Group Policy that overrides the per-user Windows Security setting. "
                    + "Default: enabled. Recommended: disable on hardened systems.",
                ApplyOps = [RegOp.SetDword(RaPolicy, "fAllowToGetHelp", 0)],
                RemoveOps = [RegOp.DeleteValue(RaPolicy, "fAllowToGetHelp")],
                DetectOps = [RegOp.CheckDword(RaPolicy, "fAllowToGetHelp", 0)],
            },
            new TweakDef
            {
                Id = "rast-view-only-gpo",
                Label = "Remote Assistance: Restrict to View-Only via GPO",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaPolicy],
                Tags = ["remote-assistance", "gpo", "view-only", "policy", "security"],
                Description =
                    "Sets fAllowFullControl=0 in Terminal Services policy. Helpers can watch the screen but cannot "
                    + "operate the keyboard or mouse during a Remote Assistance session. "
                    + "Default: full control allowed. Recommended: view-only for minimum-privilege RA sessions.",
                ApplyOps = [RegOp.SetDword(RaPolicy, "fAllowFullControl", 0)],
                RemoveOps = [RegOp.DeleteValue(RaPolicy, "fAllowFullControl")],
                DetectOps = [RegOp.CheckDword(RaPolicy, "fAllowFullControl", 0)],
            },
            new TweakDef
            {
                Id = "rast-limit-ticket-6hr-gpo",
                Label = "Remote Assistance: Limit Invitation Lifetime to 6 Hours (GPO)",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaPolicy],
                Tags = ["remote-assistance", "gpo", "ticket", "expiry", "policy"],
                Description =
                    "Sets MaxTicketExpiry=6 and MaxTicketExpiryUnits=1 (hours) in Terminal Services policy. "
                    + "Limits the window during which a stolen or forwarded RA invitation file can be used. "
                    + "Default: no strict policy limit. Recommended: 1–6 hours.",
                ApplyOps = [RegOp.SetDword(RaPolicy, "MaxTicketExpiry", 6), RegOp.SetDword(RaPolicy, "MaxTicketExpiryUnits", 1)],
                RemoveOps = [RegOp.DeleteValue(RaPolicy, "MaxTicketExpiry"), RegOp.DeleteValue(RaPolicy, "MaxTicketExpiryUnits")],
                DetectOps = [RegOp.CheckDword(RaPolicy, "MaxTicketExpiry", 6)],
            },
            new TweakDef
            {
                Id = "rast-disable-offer-gpo",
                Label = "Remote Assistance: Block Unsolicited (Offer) RA via GPO",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaOffered],
                Tags = ["remote-assistance", "gpo", "offer", "unsolicited", "security"],
                Description =
                    "Sets fAllowUnsolicited=0 in the RAUnsolicit policy key. Prevents IT admins from initiating "
                    + "Remote Assistance connections without an end-user invitation (Offer RA / DCOM RA). "
                    + "Default: Offer RA allowed. Recommended: disable unless actively used by IT.",
                ApplyOps = [RegOp.SetDword(RaOffered, "fAllowUnsolicited", 0)],
                RemoveOps = [RegOp.DeleteValue(RaOffered, "fAllowUnsolicited")],
                DetectOps = [RegOp.CheckDword(RaOffered, "fAllowUnsolicited", 0)],
            },
            new TweakDef
            {
                Id = "rast-limit-offer-ticket-1hr",
                Label = "Remote Assistance: Limit Offer RA Token Expiry to 1 Hour",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaOffered],
                Tags = ["remote-assistance", "offer", "ticket", "expiry", "gpo"],
                Description =
                    "Sets MaxTicketExpiry=1 and MaxTicketExpiryUnits=1 in the RAUnsolicit key. "
                    + "Offer RA tokens granted by IT expire after 1 hour. Default: no strict limit. "
                    + "Reduces risk of stale credentials being replayed for extended access.",
                ApplyOps = [RegOp.SetDword(RaOffered, "MaxTicketExpiry", 1), RegOp.SetDword(RaOffered, "MaxTicketExpiryUnits", 1)],
                RemoveOps = [RegOp.DeleteValue(RaOffered, "MaxTicketExpiry"), RegOp.DeleteValue(RaOffered, "MaxTicketExpiryUnits")],
                DetectOps = [RegOp.CheckDword(RaOffered, "MaxTicketExpiry", 1)],
            },
            new TweakDef
            {
                Id = "rast-disable-runtime",
                Label = "Remote Assistance: Disable via Runtime Control Key",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [RaRuntime],
                Tags = ["remote-assistance", "disable", "runtime", "security"],
                Description =
                    "Sets fAllowToGetHelp=0 in the runtime Remote Assistance control key. "
                    + "Disables Remote Assistance at the OS level for non-domain / standalone machines. "
                    + "Default: enabled. complements the GPO setting. Effective without domain membership.",
                ApplyOps = [RegOp.SetDword(RaRuntime, "fAllowToGetHelp", 0)],
                RemoveOps = [RegOp.SetDword(RaRuntime, "fAllowToGetHelp", 1)],
                DetectOps = [RegOp.CheckDword(RaRuntime, "fAllowToGetHelp", 0)],
            },
            new TweakDef
            {
                Id = "rast-view-only-runtime",
                Label = "Remote Assistance: Restrict to View-Only (Runtime)",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [RaRuntime],
                Tags = ["remote-assistance", "view-only", "runtime", "privacy"],
                Description =
                    "Sets fAllowFullControl=0 in the runtime Remote Assistance key. "
                    + "Helpers can observe the session but cannot control the keyboard or mouse. "
                    + "Default: full control. Recommended: view-only to limit surface during RA.",
                ApplyOps = [RegOp.SetDword(RaRuntime, "fAllowFullControl", 0)],
                RemoveOps = [RegOp.SetDword(RaRuntime, "fAllowFullControl", 1)],
                DetectOps = [RegOp.CheckDword(RaRuntime, "fAllowFullControl", 0)],
            },
            new TweakDef
            {
                Id = "rast-limit-ticket-1hr-runtime",
                Label = "Remote Assistance: Limit Invitation Lifetime to 1 Hour (Runtime)",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [RaRuntime],
                Tags = ["remote-assistance", "ticket", "expiry", "runtime", "security"],
                Description =
                    "Sets MaxTicketExpiry=1 and MaxTicketExpiryUnits=1 in the runtime key. "
                    + "RA invitations expire after 1 hour. Default: 6 hours (Windows default). "
                    + "Reduces the time window during which a leaked invitation can be exploited.",
                ApplyOps = [RegOp.SetDword(RaRuntime, "MaxTicketExpiry", 1), RegOp.SetDword(RaRuntime, "MaxTicketExpiryUnits", 1)],
                RemoveOps = [RegOp.SetDword(RaRuntime, "MaxTicketExpiry", 6), RegOp.SetDword(RaRuntime, "MaxTicketExpiryUnits", 1)],
                DetectOps = [RegOp.CheckDword(RaRuntime, "MaxTicketExpiry", 1)],
            },
            new TweakDef
            {
                Id = "rast-disable-chat",
                Label = "Remote Assistance: Disable Chat Window",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = false,
                RegistryKeys = [RaRuntime],
                Tags = ["remote-assistance", "chat", "privacy", "runtime"],
                Description =
                    "Sets fEnableChatControl=0. Disables the text chat panel during Remote Assistance sessions. "
                    + "Reduces communication surface and prevents helpers from exfiltrating data via chat. "
                    + "Default: chat enabled. Recommended: disable on sensitive systems.",
                ApplyOps = [RegOp.SetDword(RaRuntime, "fEnableChatControl", 0)],
                RemoveOps = [RegOp.SetDword(RaRuntime, "fEnableChatControl", 1)],
                DetectOps = [RegOp.CheckDword(RaRuntime, "fEnableChatControl", 0)],
            },
            new TweakDef
            {
                Id = "rast-deny-ts-connections-gpo",
                Label = "Remote Assistance: Block All Remote Desktop/RA Connections via GPO",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [RaPolicy],
                Tags = ["remote-assistance", "rdp", "gpo", "lockdown", "security"],
                Description =
                    "Sets fDenyTSConnections=1 in Terminal Services policy. Blocks all incoming Remote Desktop "
                    + "and Remote Assistance connections at the policy level. "
                    + "Default: connections allowed. Use on high-security kiosk or server systems without RDP needs.",
                ApplyOps = [RegOp.SetDword(RaPolicy, "fDenyTSConnections", 1)],
                RemoveOps = [RegOp.DeleteValue(RaPolicy, "fDenyTSConnections")],
                DetectOps = [RegOp.CheckDword(RaPolicy, "fDenyTSConnections", 1)],
            },
        ];
    }

    // ── RemoteCredentialGuardPolicy ──
    private static class _RemoteCredentialGuardPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RemoteCredentialGuard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rcgrd-enable-remote-credential-guard",
                Label = "Enable Remote Credential Guard for Remote Desktop Connections",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Remote Credential Guard protects credentials used for Remote Desktop connections by keeping credentials on the originating device rather than sending them to the remote host. Without Remote Credential Guard credentials are sent to the remote host where they are vulnerable to credential-dumping attacks by malware or attackers with elevated privileges on the remote system. Enabling Remote Credential Guard is one of the most effective mitigations against pass-the-hash and pass-the-ticket attacks that use credentials harvested from remote desktop target systems. The feature requires that the client device supports Credential Guard and that the remote host is running Windows 10 1607 or later or Windows Server 2016 or later. Remote Credential Guard is particularly important for privileged administrator accounts that use remote desktop to manage sensitive systems. Organizations should combine Remote Credential Guard with Restricted Admin mode to provide two complementary credential theft prevention approaches.",
                Tags = ["remote-credential-guard", "rdp", "credential-theft", "pass-the-hash", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableRemoteCredentialGuard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableRemoteCredentialGuard")],
                DetectOps = [RegOp.CheckDword(Key, "EnableRemoteCredentialGuard", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-enforce-strict-kerberos-delegation",
                Label = "Enforce Strict Kerberos Delegation Constraints for RCG Sessions",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Strict Kerberos delegation constraints in Remote Credential Guard sessions limit the ability of remote hosts to use delegated credentials for additional authentication to downstream services. Without delegation constraints an attacker who compromises a remote desktop target system can use the delegated credentials to access additional systems on behalf of the authenticated user. Constrained delegation limits the services that delegated credentials can be used to access to a pre-configured allowlist of validated services. Remote Credential Guard with strict delegation prevents credential abuse through delegation chains that could otherwise allow attackers to move laterally using the initial credential. Organizations should review and restrict delegation settings for accounts that use remote desktop to high-value systems. Strict delegation constraints combined with Remote Credential Guard provide layered protection against credential abuse in remote access scenarios.",
                Tags = ["remote-credential-guard", "kerberos-delegation", "constrained-delegation", "lateral-movement", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceStrictKerberosDelegation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrictKerberosDelegation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceStrictKerberosDelegation", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-restrict-ntlm-in-rcg-sessions",
                Label = "Restrict NTLM Authentication in Remote Credential Guard Sessions",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Restricting NTLM authentication in Remote Credential Guard sessions prevents fallback to NTLM when Kerberos fails which would expose credentials to NTLM relay attacks. Remote Credential Guard relies on Kerberos for its credential protection guarantees and NTLM fallback undermines these protections by allowing credential exposure through NTLM. NTLM relay attacks are a common post-exploitation technique that can be used when NTLM authentication is available even in environments that have prohibited NTLM for general use. Restricting NTLM in RCG sessions enforces Kerberos-only authentication ensuring that the full credential protection benefits of Remote Credential Guard apply. Organizations should verify that Kerberos authentication infrastructure is correctly configured and accessible from all systems where Remote Credential Guard is in use before restricting NTLM. Monitoring for NTLM authentication attempts during RCG sessions helps identify cases where Kerberos is failing and NTLM restriction is causing authentication failures.",
                Tags = ["remote-credential-guard", "ntlm", "kerberos", "credential-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictNTLMInRCGSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictNTLMInRCGSessions")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictNTLMInRCGSessions", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-audit-rcg-session-events",
                Label = "Enable Audit Logging for Remote Credential Guard Session Events",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Remote Credential Guard session audit logging captures connection establishment authentication events and session termination data for security monitoring and incident response. Audit data from RCG sessions enables detection of unauthorized remote access attempts failed authentication that may indicate brute force and unusual connection patterns. RCG audit events should be forwarded to a central logging system for correlation with other security events from the remote hosts and authentication infrastructure. Connection timing geographical patterns and frequency anomalies in RCG sessions may indicate compromised account credentials attempting to use legitimate remote access channels. Audit logs from RCG sessions provide accountability for administrative actions performed on sensitive systems accessed through remote desktop. Organizations should retain RCG session audit logs for a period appropriate to their regulatory requirements and incident investigation needs.",
                Tags = ["remote-credential-guard", "audit", "remote-desktop", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditRCGSessionEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditRCGSessionEvents")],
                DetectOps = [RegOp.CheckDword(Key, "AuditRCGSessionEvents", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-enable-rcg-for-admin-sessions",
                Label = "Enforce Remote Credential Guard for All Administrator Remote Desktop Sessions",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Enforcing Remote Credential Guard for administrator remote desktop sessions ensures that privileged credentials are never exposed to the remote hosts that administrators manage. Administrator accounts are high-value targets for credential theft and administrator remote desktop sessions to servers are a common source of credential compromise in enterprise intrusions. Enforcing RCG for admin sessions prevents the most common attack pattern where attackers compromise a single server and then dump administrator credentials from RDP sessions connected to it. Policy enforcement ensures that administrators cannot bypass Remote Credential Guard even when connecting from tools that do not use it by default. Organizations should deploy this policy to all systems that administrator accounts use to launch remote desktop sessions rather than to the servers being managed. Combining RCG enforcement with privileged access workstations creates a comprehensive approach to protecting administrator credentials.",
                Tags = ["remote-credential-guard", "admin-accounts", "rdp", "privileged-access", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceRCGForAdminSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceRCGForAdminSessions")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceRCGForAdminSessions", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-block-credential-delegation-to-untrusted",
                Label = "Block Credential Delegation to Untrusted Remote Desktop Hosts",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Description =
                    "Blocking credential delegation to untrusted remote desktop hosts prevents credentials from being sent to systems that are not explicitly authorized to receive them through Windows Remote Management policy. Untrusted remote hosts that receive delegated credentials pose a significant risk as credentials can be extracted from them by attackers who have already compromised those systems. The trust model for credential delegation must be carefully maintained with only known-good systems in the trusted hosts list. Hosts should be considered trusted only when they have current security baselines applied Endpoint Protection coverage and are monitored for security events. Blocking delegation to untrusted hosts reduces the blast radius of a single system compromise by preventing credential propagation to that compromised system from subsequent remote desktop connections. Organizations should maintain an accurate and current list of trusted hosts and regularly audit which systems are in the trusted list.",
                Tags = ["remote-credential-guard", "credential-delegation", "trusted-hosts", "blast-radius", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockDelegationToUntrustedHosts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockDelegationToUntrustedHosts")],
                DetectOps = [RegOp.CheckDword(Key, "BlockDelegationToUntrustedHosts", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-require-nla-for-rcg-sessions",
                Label = "Require Network Level Authentication for Remote Credential Guard Sessions",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Requiring Network Level Authentication for Remote Credential Guard sessions ensures that authentication must be completed before a remote desktop session is established rather than after the login screen is presented. NLA authentication pre-authenticates the user before session establishment preventing scenarios where the remote host is exposed to the network before the user proves their identity. Without NLA the remote desktop login screen itself is exposed to network attackers who can attempt exploitation before any credentials are involved. Combining NLA with Remote Credential Guard provides defense-in-depth where authentication occurs before session establishment and credentials are protected during the session. NLA is also significantly more efficient for server resources as sessions that fail NLA authentication do not consume remote desktop server session resources. Organizations should verify that all clients that will use Remote Credential Guard support NLA to avoid compatibility issues.",
                Tags = ["remote-credential-guard", "nla", "pre-authentication", "rdp-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireNLAForRCGSessions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireNLAForRCGSessions")],
                DetectOps = [RegOp.CheckDword(Key, "RequireNLAForRCGSessions", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-restrict-rcg-to-domain-joined-hosts",
                Label = "Restrict Remote Credential Guard to Domain-Joined Remote Hosts",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting Remote Credential Guard connections to domain-joined remote hosts ensures that RCG is only used to connect to systems that are subject to organizational security policy and management. Non-domain-joined systems do not have the same security baseline expectations and may not have the protections expected for systems that will receive remote desktop connections. Domain membership provides verification that the remote host has been configured according to organizational security standards and has Group Policy applied. Systems outside the domain like personal computers contractor systems or guest network hosts should not receive domain credentials through remote desktop even with Remote Credential Guard protections. The domain restriction aligns remote desktop credential usage with the formal trust model of the domain ensuring credentials are only used within the authenticated infrastructure. Organizations should enforce this restriction through Group Policy to prevent users from using Remote Credential Guard to access non-domain targets.",
                Tags = ["remote-credential-guard", "domain-joined", "trusted-hosts", "rdp", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictToDomainJoinedHosts", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictToDomainJoinedHosts")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictToDomainJoinedHosts", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-enable-rcg-session-encryption",
                Label = "Enforce Strong Encryption for Remote Credential Guard Session Traffic",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing strong encryption for Remote Credential Guard session traffic ensures that all remote desktop session data is protected against network interception and man-in-the-middle attacks. Weak encryption configurations for RDP sessions allow attackers with network access to decrypt and access session content including screen data keystrokes and clipboard data. Modern RDP encryption uses TLS with strong cipher suites and Remote Credential Guard sessions should require TLS 1.2 or higher with strong cipher suites. Legacy encryption modes that provide weaker protection should be explicitly disabled to prevent negotiation of weak encryption even when the client or server supports legacy modes. Organizations should verify the TLS version and cipher suite negotiations used for Remote Credential Guard sessions using network capture analysis. Regular rotation of the RDP server certificate and verification of certificate validity helps maintain the security of the encryption channel.",
                Tags = ["remote-credential-guard", "encryption", "tls", "rdp-encryption", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceStrongEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceStrongEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceStrongEncryption", 1)],
            },
            new TweakDef
            {
                Id = "rcgrd-monitor-rcg-connection-anomalies",
                Label = "Enable Monitoring for Remote Credential Guard Connection Anomalies",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Monitoring for Remote Credential Guard connection anomalies detects unusual access patterns that may indicate account compromise or unauthorized use of legitimate remote access credentials. Anomaly monitoring tracks connection frequency timing geographic origin and destination patterns to identify sessions that deviate from established baselines. Compromised accounts used for legitimate remote desktop access may be detected when they are used outside normal working hours from unusual source addresses or to access systems they do not normally connect to. Connection anomaly detection for RCG sessions provides an additional detection layer beyond simple authentication success monitoring. Organizations should establish behavioral baselines for each privileged account's remote desktop usage and configure alerting thresholds appropriate for their environment. Anomaly alerts should be integrated with the incident response process to enable rapid investigation and containment of potentially compromised accounts.",
                Tags = ["remote-credential-guard", "anomaly-detection", "monitoring", "behavioral-analysis", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MonitorConnectionAnomalies", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "MonitorConnectionAnomalies")],
                DetectOps = [RegOp.CheckDword(Key, "MonitorConnectionAnomalies", 1)],
            },
        ];
    }

    // ── RemoteProcedureCallPolicy ──
    private static class _RemoteProcedureCallPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Rpc";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "rpcpol-enable-rpc-authentication",
                Label = "Enable RPC Endpoint Mapper Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "The RPC Endpoint Mapper receives incoming RPC connections and directs them to the appropriate server processes without authentication by default. Enabling authentication on the RPC Endpoint Mapper requires that callers authenticate before the mapper reveals service endpoint locations. Unauthenticated mapper access allows external parties to enumerate available RPC services on the endpoint which aids attack reconnaissance. Authentication requirements prevent unauthenticated sweep attacks that map available RPC services across enterprise networks. Endpoint mapper authentication is part of RPC hardening guidance from Microsoft and security assessors. This setting improves RPC security without degrading functionality for authenticated users and systems.",
                Tags = ["rpc", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableAuthEpResolution", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableAuthEpResolution")],
                DetectOps = [RegOp.CheckDword(Key, "EnableAuthEpResolution", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-unauthenticated-rpc",
                Label = "Restrict Unauthenticated RPC Calls",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 3,
                Description =
                    "RPC servers can be configured to reject unauthenticated connections and require that callers provide authentication credentials before being served. Restricting unauthenticated RPC prevents unknown callers from invoking RPC services without establishing an authenticated identity. Unauthenticated RPC calls can be used for reconnaissance and exploitation of services that implement insufficient authorization. Authenticated RPC provides the identity context necessary for access logging and auditing of service invocations. Enterprise hardening requirements typically include restricting unauthenticated access to RPC services on managed endpoints. Restricting unauthenticated RPC may require testing as some legitimate services use unauthenticated RPC for specific operational purposes.",
                Tags = ["rpc", "authentication", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictRemoteClients", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictRemoteClients")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictRemoteClients", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-enable-rpc-message-integrity",
                Label = "Enable RPC Connection Message Integrity",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "RPC message integrity uses message authentication codes to verify that RPC call payloads have not been tampered with in transit. Enabling RPC message integrity protects against man-in-the-middle attacks that modify RPC payloads after authentication is established. Authenticated RPC sessions without integrity protection are vulnerable to post-authentication payload manipulation. Message integrity checking adds minimal overhead while providing strong protection against active attacks on RPC communications. Enterprise security policies should require integrity protection for all network communications including internal RPC services. RPC integrity is supported by all modern Windows authentication mechanisms including Kerberos and NTLM.",
                Tags = ["rpc", "integrity", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinimumConnectionTimeout", 120)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumConnectionTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumConnectionTimeout", 120)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-rpc-over-http",
                Label = "Disable RPC over HTTP Proxy",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "RPC over HTTP allows RPC calls to be tunneled through HTTP connections enabling RPC communication across firewalls that block native RPC ports. Disabling RPC over HTTP prevents the use of HTTP as a transport mechanism for RPC communications from managed endpoints. RPC over HTTP is used primarily for Exchange Outlook Anywhere connectivity and is not needed for standard enterprise operations. HTTP tunnaling of RPC can bypass network security controls designed to monitor and filter RPC protocol communications. Network perimeter security tools are less effective at inspecting and controlling RPC-over-HTTP compared to native RPC traffic. Environments using Exchange Online or modern authentication methods have no need for legacy RPC over HTTP connections.",
                Tags = ["rpc", "http", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableRPCOverHTTP", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableRPCOverHTTP")],
                DetectOps = [RegOp.CheckDword(Key, "DisableRPCOverHTTP", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-enforce-rpc-security-callback",
                Label = "Enforce RPC Security Callback Verification",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "RPC servers can implement security callback functions that verify caller permissions before processing each RPC call. Requiring security callback enforcement ensures that RPC servers always invoke their security callback functions and cannot be bypassed. Vulnerabilities in RPC callback handling have allowed attackers to bypass authorization and invoke privileged RPC operations. Enforcing the security callback policy requires that all registered RPC permission verification code actually executes and cannot be skipped. This reduces the risk of RPC service implementation flaws that may conditionally skip credential validation. Security callback enforcement is a defense-in-depth measure ensuring RPC access control code is always exercised.",
                Tags = ["rpc", "security", "verification", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceRpcSecurityCallback", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceRpcSecurityCallback")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceRpcSecurityCallback", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-rpc-logging",
                Label = "Enable RPC Connection Logging",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "RPC connection logging records information about incoming RPC connections including caller identity, endpoint invoked, and connection metadata. Enabling RPC logging provides visibility into RPC traffic patterns and supports forensic investigation of RPC-based attacks. Detecting anomalous RPC activity requires that connection records be available for analysis by security teams. RPC logging data is written to the Windows Event Log and can be forwarded to SIEM systems for correlation. Organizations investigating incidents rely on RPC logs to understand attack paths that used Windows RPC-based lateral movement. Enabling logging has minimal performance impact and provides significant security value for detection and forensics.",
                Tags = ["rpc", "logging", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableConnectionLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableConnectionLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableConnectionLogging", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-set-rpc-call-timeout",
                Label = "Set RPC Call Timeout Limit",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "RPC calls that do not complete within a reasonable time can indicate server overload, network problems, or active attacks consuming RPC processing resources. Setting an RPC call timeout ensures that long-running or hung RPC calls are terminated rather than consuming resources indefinitely. Denial of service attacks can exploit unlimited RPC call duration by submitting calls designed to consume server processing resources. Reasonable timeout limits ensure that endpoints remain responsive even when individual RPC calls encounter unexpected delays. RPC timeouts should be set according to legitimate business requirements while remaining low enough to limit malicious resource consumption. Legitimate fast-completing RPC operations are completely unaffected by call timeout settings.",
                Tags = ["rpc", "timeout", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinCallTimeout", 240)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinCallTimeout")],
                DetectOps = [RegOp.CheckDword(Key, "MinCallTimeout", 240)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-rpc-ncalrpc-transport",
                Label = "Restrict RPC NCALRPC Local Transport",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 3,
                Description =
                    "NCALRPC is the Named Pipe-based local RPC transport used for inter-process communication on the local system without network involvement. Restricting NCALRPC transport prevents certain local RPC escalation techniques that exploit local pipe-based RPC connections. Privilege escalation through local-only RPC transports has been demonstrated in several Windows elevation of privilege vulnerabilities. Limiting NCALRPC availability constrains the attack surface available for local privilege escalation through RPC-based mechanisms. Server applications that legitimately use local RPC communication may need to be assessed before restricting this transport. This setting requires careful evaluation as it may affect legitimate inter-process communication on the endpoint.",
                Tags = ["rpc", "local", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictLocalRpc", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictLocalRpc")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictLocalRpc", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-rpc-anon-auth",
                Label = "Disable Anonymous RPC Authentication Level",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Anonymous authentication in RPC allows callers to invoke RPC services without presenting any identity credentials. Disabling anonymous RPC authentication prevents services from accepting calls from undisclosed callers with no authentication information. Allowing anonymous RPC creates significant access control risks as authorization decisions cannot be based on caller identity. Authentication-challenged RPC calls can expose sensitive services to any endpoint that can reach the RPC port. Enterprise environments should not allow anonymous access to internal services where authentication is technically feasible. Disabling anonymous RPC does not affect authenticated access which continues to work with proper credentials.",
                Tags = ["rpc", "anonymous", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MinimumAuthenticationLevel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "MinimumAuthenticationLevel")],
                DetectOps = [RegOp.CheckDword(Key, "MinimumAuthenticationLevel", 1)],
            },
            new TweakDef
            {
                Id = "rpcpol-disable-rpc-portrange-override",
                Label = "Disable RPC Dynamic Port Range Override",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "RPC uses dynamic port allocation for many services assigning ports from a configurable range for each new connection. Disabling dynamic port range overrides ensures that RPC uses the restricted Windows default port range making firewall rule management consistent. Unrestricted dynamic port ranges make firewall rules for RPC communications impractical requiring either wide port range rules or no firewall protection. The restricted Windows RPC port range provides a reasonable balance between flexibility and firewall manageability. Consistent port ranges enable network security teams to write targeted firewall rules instead of allowing all high-port UDP and TCP traffic. Organizations should align their firewall rules with the configured RPC port range for consistent network security policy.",
                Tags = ["rpc", "ports", "network", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PortsInternetAvailable", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PortsInternetAvailable")],
                DetectOps = [RegOp.CheckDword(Key, "PortsInternetAvailable", 0)],
            },
        ];
    }

    // ── RemotePsJeaPolicy ──
    private static class _RemotePsJeaPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "psjea-disable-winrm-service",
                    Label = "Disable WinRM Service Auto-Start via Policy",
                    Category = "Remote Desktop",
                    Description =
                        "Disables the WinRM service from starting automatically via Group Policy, preventing incoming PowerShell remoting and WMI-over-WinRM connections unless explicitly activated.",
                    Tags = ["winrm", "remoting", "jea", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Incoming WinRM connections blocked; PS remoting sessions cannot be established.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableWinRM", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableWinRM")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableWinRM", 1)],
                },
                new TweakDef
                {
                    Id = "psjea-block-unencrypted-winrm",
                    Label = "Block Unencrypted WinRM Traffic",
                    Category = "Remote Desktop",
                    Description =
                        "Disallows unencrypted WinRM communication, requiring all WinRM traffic to use HTTPS or Kerberos/TLS encryption to protect credentials and data in transit.",
                    Tags = ["winrm", "encryption", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Plain-text WinRM rejected; all remote PS connections must use HTTPS or Kerberos.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "psjea-disable-basic-auth-server",
                    Label = "Disable WinRM Basic Authentication (Server Side)",
                    Category = "Remote Desktop",
                    Description =
                        "Disables Basic authentication on the WinRM server side, preventing password transmission in clear text (Base64) over WinRM connections.",
                    Tags = ["winrm", "basic-auth", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM Basic auth disabled server-side; Kerberos/NTLM/Certificates required.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "psjea-disable-basic-auth-client",
                    Label = "Disable WinRM Basic Authentication (Client Side)",
                    Category = "Remote Desktop",
                    Description =
                        "Disables Basic authentication for the WinRM client, preventing the client from offering or accepting Basic auth credentials when connecting to remote endpoints.",
                    Tags = ["winrm", "basic-auth", "client", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client will not use Basic auth for connections.",
                    ApplyOps = [RegOp.SetDword(Key2, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(Key2, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "psjea-disable-digest-auth",
                    Label = "Disable WinRM Digest Authentication",
                    Category = "Remote Desktop",
                    Description =
                        "Disables the Digest authentication scheme on the WinRM client, preventing weak credential hashing schemes from being used in remote management connections.",
                    Tags = ["winrm", "digest-auth", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Digest auth blocked; modern Kerberos or certificate auth required.",
                    ApplyOps = [RegOp.SetDword(Key2, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(Key2, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "psjea-require-kerberos",
                    Label = "Require Kerberos for WinRM Authentication",
                    Category = "Remote Desktop",
                    Description =
                        "Configures the WinRM client to require Kerberos-based authentication for remote management connections, ensuring only domain-authenticated sessions are established.",
                    Tags = ["winrm", "kerberos", "authentication", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Kerberos only for WinRM; workgroup machines cannot use PS remoting.",
                    ApplyOps = [RegOp.SetDword(Key2, "AllowKerberos", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "AllowKerberos")],
                    DetectOps = [RegOp.CheckDword(Key2, "AllowKerberos", 1)],
                },
                new TweakDef
                {
                    Id = "psjea-set-idle-timeout",
                    Label = "Set WinRM Session Idle Timeout to 900 Seconds",
                    Category = "Remote Desktop",
                    Description =
                        "Sets the WinRM service idle timeout to 900 seconds (15 minutes) to automatically terminate abandoned PowerShell remoting sessions, reducing attack window for session hijacking.",
                    Tags = ["winrm", "timeout", "jea", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Idle remote sessions disconnected after 15 minutes; long-running automation scripts should keep alive.",
                    ApplyOps = [RegOp.SetDword(Key, "IdleTimeoutms", 900000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "IdleTimeoutms")],
                    DetectOps = [RegOp.CheckDword(Key, "IdleTimeoutms", 900000)],
                },
                new TweakDef
                {
                    Id = "psjea-disable-runasinteractive",
                    Label = "Disable RunAs Interactive in WinRM Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents users on WinRM sessions from using RunAs to elevate to interactive logon tokens, limiting privilege escalation paths within remote management sessions.",
                    Tags = ["winrm", "runas", "jea", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Interactive RunAs blocked in remote sessions; all JEA sessions operate under delegated role accounts.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableRunAsInteractive", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableRunAsInteractive")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableRunAsInteractive", 1)],
                },
                new TweakDef
                {
                    Id = "psjea-block-client-unencrypted",
                    Label = "Block Unencrypted WinRM on Client Side",
                    Category = "Remote Desktop",
                    Description =
                        "Disallows the WinRM client from sending or accepting unencrypted messages, ensuring all outgoing remote management traffic is protected.",
                    Tags = ["winrm", "encryption", "client", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client refuses unencrypted connections; HTTPS listeners required.",
                    ApplyOps = [RegOp.SetDword(Key2, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(Key2, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "psjea-disable-credssp",
                    Label = "Disable CredSSP Authentication for WinRM",
                    Category = "Remote Desktop",
                    Description =
                        "Disables CredSSP (Credential Security Support Provider) in WinRM, preventing credential delegation attacks where full network credentials are passed through to remote hosts.",
                    Tags = ["winrm", "credssp", "delegation", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "CredSSP blocked; no double-hop credential delegation via PS remoting. Use Kerberos constrained delegation instead.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCredSSP", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCredSSP")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCredSSP", 1)],
                },
            ];
    }

    // ── TerminalServicesAdvPolicy ──
    private static class _TerminalServicesAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "tssvcadv-require-network-level-authentication",
                    Label = "TS Adv: Require Network Level Authentication for RDP Connections",
                    Category = "Remote Desktop",
                    Description =
                        "Sets UserAuthentication=1 in Terminal Services policy. Requires Network Level Authentication (NLA) for all Remote Desktop connections to this computer. NLA forces the connecting client to authenticate using their domain credentials before the full Windows logon screen is presented — if NLA fails, no graphical session or memory buffering occurs. "
                        + "Without NLA, Remote Desktop presents a full Windows logon screen to any network client that completes the TLS handshake. This graphical pre-authentication screen consumes significant server-side memory and CPU for rendering, is vulnerable to credential brute-force attacks against the visible login prompt, and is exploitable via session pre-authentication logic flaws (BlueKeep/DejaBlue were pre-NLA vulnerabilities). With NLA, unauthenticated clients never reach the graphical login stage — the server rejects unauthenticated connections at the NTLMSSP or Kerberos layer before any session rendering occurs.",
                    Tags = ["ts-adv", "rdp", "nla", "network-level-authentication", "bluekeep", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "NLA required for all RDP; unauthenticated clients rejected before graphical session. Requires NLA-capable clients (Windows Vista+, all modern RDP clients).",
                    ApplyOps = [RegOp.SetDword(Key, "UserAuthentication", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UserAuthentication")],
                    DetectOps = [RegOp.CheckDword(Key, "UserAuthentication", 1)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-require-tls-security-layer",
                    Label = "TS Adv: Enforce TLS Security Layer for RDP Session Encryption",
                    Category = "Remote Desktop",
                    Description =
                        "Sets SecurityLayer=2 in Terminal Services policy. Forces RDP to use TLS 1.2+ for the transport security layer of all Remote Desktop connections, replacing the legacy RDP Security Layer (SecurityLayer=0) and Negotiate mode (SecurityLayer=1). TLS authentication requires the server to present a valid certificate, providing mutual authentication and preventing connection to a man-in-the-middle rogue RDP server. "
                        + "The legacy RDP Security Layer uses RC4-128 encryption proprietary to the RDP protocol. It provides no server identity verification — a client connecting to a rogue RDP endpoint (via DNS poisoning or BGP hijack) has no mechanism to verify they are connected to the legitimate server. With SecurityLayer=2 (SSL/TLS), the server presents an X.509 certificate that the client validates against a trusted CA. An RDP certificate pinned to the domain CA ensures that a man-in-the-middle attack requires forging a domain-trusted certificate — a significantly harder attack than spoofing the legacy RDP protocol layer.",
                    Tags = ["ts-adv", "rdp", "tls", "security-layer", "certificate", "mitm"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "TLS required for all RDP transport. Legacy RDP Security Layer disabled. Requires valid server certificate — auto-generated self-signed cert is accepted but domain cert preferred.",
                    ApplyOps = [RegOp.SetDword(Key, "SecurityLayer", 2)],
                    RemoveOps = [RegOp.DeleteValue(Key, "SecurityLayer")],
                    DetectOps = [RegOp.CheckDword(Key, "SecurityLayer", 2)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-enforce-high-encryption",
                    Label = "TS Adv: Enforce High (128-bit) RDP Session Encryption Minimum",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MinEncryptionLevel=3 in Terminal Services policy (3 = High — 128-bit minimum). Requires all Remote Desktop sessions to use 128-bit or higher AES encryption. Setting 1 (Low) allows 56-bit DES; setting 2 (Client compatible) negotiates down to whatever the client supports. Setting 3 (High) rejects any client that cannot negotiate at least 128-bit AES encryption. "
                        + "Low or medium encryption settings allow legacy RDP clients to negotiate DES-56 or RC4-40 encryption — both of which are trivially breakable with modern hardware within hours. Any network capture of a DES-56 encrypted RDP session can be decrypted offline. All modern RDP clients (Windows Vista+, FreeRDP 2.0+, macOS Microsoft Remote Desktop 10+, iOS/Android Microsoft Remote Desktop 10+) support 128-bit AES. Enforcing MinEncryptionLevel=3 rejects only RDP 4.0-era legacy clients created before 2001.",
                    Tags = ["ts-adv", "rdp", "encryption", "128-bit", "aes", "minimum-encryption"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "128-bit minimum encryption enforced. DES-56 and RC4-40 clients rejected. All clients from 2002 onward are unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "MinEncryptionLevel", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinEncryptionLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "MinEncryptionLevel", 3)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-limit-max-idle-time",
                    Label = "TS Adv: Enforce 30-Minute Maximum RDP Session Idle Timeout",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxIdleTime=1800000 in Terminal Services policy (milliseconds — 30 minutes). Disconnects idle RDP sessions after 30 minutes of inactivity. An idle session that remains connected indefinitely is a persistent attack surface: an attacker who gains network access can hijack an idle session without re-authenticating (using RDP session shadowing or session token replay if credentials are weak). Disconnecting idle sessions forces re-authentication and clears the session's memory state. "
                        + "Session idle time limits are a defence-in-depth control against insider threat and unauthorised physical access scenarios. A developer who leaves an RDP session connected overnight to a production server creates an unmonitored privileged session on that server. If the developer's endpoint is compromised (via malware or physical access), the attacker can traverse the existing RDP session to reach the server without any new authentication event in the Security log. MaxIdleTime forces session termination, requiring fresh authentication and generating a new logon event.",
                    Tags = ["ts-adv", "rdp", "idle-timeout", "session-management", "disconnect"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Sessions disconnecting after 30 minutes idle. Users should save work before stepping away from long RDP sessions.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxIdleTime", 1800000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxIdleTime")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxIdleTime", 1800000)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-limit-max-connection-time",
                    Label = "TS Adv: Enforce 8-Hour Maximum RDP Active Session Duration",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxConnectionTime=28800000 in Terminal Services policy (milliseconds — 8 hours). Terminates active RDP sessions after 8 hours of continuous connection, requiring re-authentication. Without a maximum connection time, a session established on Monday morning could remain continuously active through the weekend. Long-lived sessions accumulate open handles, privileged process tokens, and memory that should be periodically refreshed. "
                        + "Maximum connection time is distinct from idle timeout — an active session that the user is continuously using can still run indefinitely without this limit. On shared terminal servers (RDSH — Remote Desktop Session Host), long-lived sessions consume CALs (RDS Client Access Licences) and server resources. From a security perspective, a 72-hour active session may have been left running after the initiating user departed (e.g., automated script session that outlived human oversight), creating an orphaned privileged session that is no longer monitored.",
                    Tags = ["ts-adv", "rdp", "max-session-time", "session-management", "rdsh"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Active sessions disconnected after 8 hours. Users running long overnight jobs via RDP should use scheduled tasks instead.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxConnectionTime", 28800000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxConnectionTime")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxConnectionTime", 28800000)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-limit-reconnection-time",
                    Label = "TS Adv: Enforce 15-Minute Disconnected Session Reconnection Limit",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxDisconnectionTime=900000 in Terminal Services policy (milliseconds — 15 minutes). Terminates disconnected (not active) RDP sessions after 15 minutes. A disconnected session maintains the user's running processes, network connections, and open files on the RDS host, consuming server-side resources. More importantly, a disconnected session can be reconnected from any client with the corresponding user credentials — if a user's credentials are stolen, an attacker can reconnect to an existing privileged session in a disconnected state. "
                        + "The distinction between disconnected and logged-off is critical: when a user closes the RDP window without logging off, the session becomes disconnected — processes continue running, network connections remain active, and the session is available for reconnection. From an attacker's perspective, a disconnected session can be reconnected via standard RDP credentials. Terminating disconnected sessions after 15 minutes ensures that sessions cannot be indefinitely parked in disconnected state waiting for credential theft.",
                    Tags = ["ts-adv", "rdp", "disconnection-timeout", "session-management"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disconnected sessions terminated after 15 minutes. Users must save work before disconnecting rather than leaving sessions in background.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxDisconnectionTime", 900000)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxDisconnectionTime")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxDisconnectionTime", 900000)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-disable-session-shadowing",
                    Label = "TS Adv: Disable RDP Session Shadowing to Prevent Covert Observation",
                    Category = "Remote Desktop",
                    Description =
                        "Sets Shadow=4 in Terminal Services policy (4 = No remote control — shadowing disabled). Disables the RDP session shadowing feature that allows administrators to view or interact with another user's Remote Desktop session. While useful for helpdesk support, session shadowing creates a backdoor for privileged observation without the target user's knowledge on systems configured with Shadow=2 (Full control without user permission). "
                        + "Session shadowing (Remote Control in older RDP documentation) with Shadow=2 allows domain administrators to take full control of any active user session without generating a visible prompt to the session user. From a data privacy perspective, this means an administrator can silently observe and control everything a user types, views, or sends — including personal passwords entered in non-SSO login forms, confidential documents open in the session, or personal communications. Setting Shadow=4 eliminates shadowing capability even for administrators; helpdesk support must use alternative methods (Teams screenshare, Intune remote assist).",
                    Tags = ["ts-adv", "rdp", "session-shadowing", "remote-control", "privacy", "disable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "RDP session shadowing disabled. Admins cannot view or control other users' sessions via RDP shadow. Helpdesk support requires alternative tools.",
                    ApplyOps = [RegOp.SetDword(Key, "Shadow", 4)],
                    RemoveOps = [RegOp.DeleteValue(Key, "Shadow")],
                    DetectOps = [RegOp.CheckDword(Key, "Shadow", 4)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-enable-rpc-traffic-encryption",
                    Label = "TS Adv: Enable RPC Traffic Encryption for Terminal Services Channel",
                    Category = "Remote Desktop",
                    Description =
                        "Sets fEncryptRPCTraffic=1 in Terminal Services policy. Enables encryption of all RPC (Remote Procedure Call) traffic on the Terminal Services management channel — the control plane channel used for session brokering, licensing, and management operations separate from the RDP data channel. "
                        + "The Terminal Services RPC management channel is used for session reconnection brokering, RD Gateway authentication, RD Connection Broker negotiation, and licensing validation between RDS components. Without RPC traffic encryption on this channel, an attacker with network access to internal RDS infrastructure can intercept session broker negotiations, connection authorisation data, and licensing state exchanges. These are lower-frequency channels than the RDP data stream but contain session routing and authentication decisions that could be manipulated to redirect sessions or suppress licensing enforcement.",
                    Tags = ["ts-adv", "rdp", "rpc-encryption", "session-broker", "control-plane"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "RDS control channel RPC traffic encrypted. No user-visible impact; applies to RDS infrastructure communication.",
                    ApplyOps = [RegOp.SetDword(Key, "fEncryptRPCTraffic", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "fEncryptRPCTraffic")],
                    DetectOps = [RegOp.CheckDword(Key, "fEncryptRPCTraffic", 1)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-enable-keepalive-connection",
                    Label = "TS Adv: Enable RDP Keep-Alive to Detect and Clean Up Stale Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets KeepAliveEnable=1 and KeepAliveInterval=1 in Terminal Services policy. Enables the RDP keep-alive mechanism to send periodic keep-alive probes (every 1 minute) to detect stale or dead RDP sessions. When a client disappears abruptly without a graceful disconnect (network failure, power outage, crash), the server-side session remains in a connected/active state consuming resources until the TCP timeout expires — which can be hours on some network stacks. "
                        + "Stale sessions occupying Connected status are undetectable via casual inspection (they appear active) but are abandoned by their client. On RDSH (Remote Desktop Session Host) deployments that license per-concurrent-session, stale sessions consume RDS CALs. More critically, a stale session with an open command prompt or elevated shell is an exploitable privileged session — if an attacker can send RST packets to the genuine client's TCP stream and then replay the session token, they may be able to inherit the session state.",
                    Tags = ["ts-adv", "rdp", "keep-alive", "stale-session", "session-cleanup"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Keep-alive probes every 1 minute; stale/dead sessions cleaned up promptly instead of lingering for hours consuming resources.",
                    ApplyOps = [RegOp.SetDword(Key, "KeepAliveEnable", 1), RegOp.SetDword(Key, "KeepAliveInterval", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "KeepAliveEnable"), RegOp.DeleteValue(Key, "KeepAliveInterval")],
                    DetectOps = [RegOp.CheckDword(Key, "KeepAliveEnable", 1)],
                },
                new TweakDef
                {
                    Id = "tssvcadv-disable-clipboard-redirection",
                    Label = "TS Adv: Disable RDP Clipboard Redirection to Prevent Data Exfiltration",
                    Category = "Remote Desktop",
                    Description =
                        "Sets DisableClipboardRedirection=1 in Terminal Services policy. Disables the clipboard channel in RDP sessions, preventing clients from copying text or files from their local clipboard into the RDP session (or vice versa). Clipboard redirection is a primary data exfiltration vector: an attacker with a compromised client endpoint can paste data from a server-side RDP session directly to a local application, bypassing DLP controls that monitor local clipboard activity. "
                        + "In high-security environments (PCI DSS, HIPAA, financial trading floors), users operating on sensitive servers via RDP must not be able to copy sensitive data from the server to their client workstation. Clipboard redirection creates a bidirectional unmonitored channel between the server (where sensitive data resides) and the client workstation (which may be less hardened and have internet access). Eliminating clipboard redirection forces users to use proper documented data transfer mechanisms (shared drives with audit logging, email with DLP scanning) rather than direct clipboard copy.",
                    Tags = ["ts-adv", "rdp", "clipboard", "data-exfiltration", "dlp", "disable-redirection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Clipboard copy-paste between RDP client and server blocked. Users cannot paste passwords or copy data across the RDP session boundary.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableClipboardRedirection", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableClipboardRedirection")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableClipboardRedirection", 1)],
                },
            ];
    }

    // ── TerminalServicesPolicy ──
    private static class _TerminalServicesPolicy
    {
        private const string TsPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Terminal Services";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tspol-require-nla",
                Label = "RDS: Require Network Level Authentication (NLA)",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "nla", "authentication", "security", "group policy", "terminal services"],
                Description =
                    "Requires Network Level Authentication (NLA/CredSSP) before establishing an RDP session. "
                    + "UserAuthentication = 1. Prevents brute-force attacks against the Windows login screen "
                    + "by requiring valid credentials before the remote session is created. "
                    + "Default: NLA not enforced. This is the single most impactful RDS security hardening step.",
                ApplyOps = [RegOp.SetDword(TsPol, "UserAuthentication", 1)],
                RemoveOps = [RegOp.SetDword(TsPol, "UserAuthentication", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "UserAuthentication", 1)],
            },
            new TweakDef
            {
                Id = "tspol-set-encryption-high",
                Label = "RDS: Enforce High (128-bit) Encryption Level",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "encryption", "security", "group policy", "terminal services"],
                Description =
                    "Forces all RDP/RDS session traffic to use 128-bit RC4 encryption (High level). "
                    + "MinEncryptionLevel = 3 (1=Low, 2=Client-Compatible, 3=High, 4=FIPS). "
                    + "Prevents session eavesdropping on 40-bit or client-negotiated weaker ciphers. "
                    + "Default: client-compatible (2). Recommended: High (3) or FIPS (4) for sensitive data.",
                ApplyOps = [RegOp.SetDword(TsPol, "MinEncryptionLevel", 3)],
                RemoveOps = [RegOp.SetDword(TsPol, "MinEncryptionLevel", 2)],
                DetectOps = [RegOp.CheckDword(TsPol, "MinEncryptionLevel", 3)],
            },
            new TweakDef
            {
                Id = "tspol-session-timeout-active",
                Label = "RDS: Set Active Session Timeout to 4 Hours",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "timeout", "session", "security", "group policy", "terminal services"],
                Description =
                    "Limits active Remote Desktop sessions to a maximum of 4 hours (240 minutes). "
                    + "MaxConnectionTime = 14400000 ms. "
                    + "Prevents orphaned or hijacked sessions from remaining active indefinitely. "
                    + "Default: no maximum connection time limit. Recommended for multi-user RDS servers.",
                ApplyOps = [RegOp.SetDword(TsPol, "MaxConnectionTime", 14400000)],
                RemoveOps = [RegOp.SetDword(TsPol, "MaxConnectionTime", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "MaxConnectionTime", 14400000)],
            },
            new TweakDef
            {
                Id = "tspol-session-timeout-idle",
                Label = "RDS: Disconnect Idle Sessions After 15 Minutes",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "idle", "timeout", "security", "group policy", "terminal services"],
                Description =
                    "Disconnects Remote Desktop sessions that have been idle for 15 minutes. "
                    + "MaxIdleTime = 900000 ms. "
                    + "Frees server resources and closes unattended sessions that could be hijacked "
                    + "from an unlocked workstation. Default: no idle timeout. CIS benchmark recommended: 15 min.",
                ApplyOps = [RegOp.SetDword(TsPol, "MaxIdleTime", 900000)],
                RemoveOps = [RegOp.SetDword(TsPol, "MaxIdleTime", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "MaxIdleTime", 900000)],
            },
            new TweakDef
            {
                Id = "tspol-session-timeout-disconnect",
                Label = "RDS: Terminate Disconnected Sessions After 1 Hour",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "reconnect", "timeout", "security", "group policy", "terminal services"],
                Description =
                    "Ends disconnected RDS sessions after they have been in the disconnected state "
                    + "for more than 1 hour. MaxDisconnectionTime = 3600000 ms. "
                    + "Reclaims memory and CPU from abandoned sessions and limits re-attach window "
                    + "for stolen session tokens. Default: disconnected sessions kept indefinitely.",
                ApplyOps = [RegOp.SetDword(TsPol, "MaxDisconnectionTime", 3600000)],
                RemoveOps = [RegOp.SetDword(TsPol, "MaxDisconnectionTime", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "MaxDisconnectionTime", 3600000)],
            },
            new TweakDef
            {
                Id = "tspol-disable-drive-redirection",
                Label = "RDS: Disable Client Drive Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "drive redirection", "data loss prevention", "security", "group policy", "terminal services"],
                Description =
                    "Prevents RDP clients from mapping local drives into the remote session. "
                    + "fDisableCdm = 1. Stops users from copying files between the local machine "
                    + "and the RDS server via drive mapping. Key DLP control for preventing data exfiltration "
                    + "from terminal servers through redirected drives. Default: drive redirection allowed.",
                ApplyOps = [RegOp.SetDword(TsPol, "fDisableCdm", 1)],
                RemoveOps = [RegOp.SetDword(TsPol, "fDisableCdm", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "fDisableCdm", 1)],
            },
            new TweakDef
            {
                Id = "tspol-disable-clipboard-redirection",
                Label = "RDS: Disable Client Clipboard Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "clipboard", "data loss prevention", "security", "group policy", "terminal services"],
                Description =
                    "Blocks clipboard synchronisation between the RDP client and the remote session. "
                    + "fDisableClip = 1. Prevents copy-paste of sensitive data from the RDS server "
                    + "to the local machine and vice versa. Critical DLP control. "
                    + "Default: clipboard redirection allowed. CIS recommends disabling on shared RDS servers.",
                ApplyOps = [RegOp.SetDword(TsPol, "fDisableClip", 1)],
                RemoveOps = [RegOp.SetDword(TsPol, "fDisableClip", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "fDisableClip", 1)],
            },
            new TweakDef
            {
                Id = "tspol-disable-printer-redirection",
                Label = "RDS: Disable Client Printer Redirection",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "printing", "data loss prevention", "security", "group policy", "terminal services"],
                Description =
                    "Prevents local printers from appearing inside Remote Desktop sessions. "
                    + "fDisableCpm = 1. Stops users from printing sensitive server-side documents to "
                    + "local or home printers via the RDP session. "
                    + "Default: printer redirection enabled. Recommended for HIPAA/regulated environments.",
                ApplyOps = [RegOp.SetDword(TsPol, "fDisableCpm", 1)],
                RemoveOps = [RegOp.SetDword(TsPol, "fDisableCpm", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "fDisableCpm", 1)],
            },
            new TweakDef
            {
                Id = "tspol-single-session-per-user",
                Label = "RDS: Limit Users to a Single Remote Session",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "session limit", "licensing", "group policy", "terminal services"],
                Description =
                    "Restricts each user account to a maximum of one concurrent Remote Desktop session. "
                    + "fSingleSessionPerUser = 1. Prevents the same account from being used across "
                    + "multiple simultaneous sessions, reducing session hijack risk and managing RDS CAL consumption. "
                    + "Default: multiple concurrent sessions per user allowed.",
                ApplyOps = [RegOp.SetDword(TsPol, "fSingleSessionPerUser", 1)],
                RemoveOps = [RegOp.SetDword(TsPol, "fSingleSessionPerUser", 0)],
                DetectOps = [RegOp.CheckDword(TsPol, "fSingleSessionPerUser", 1)],
            },
            new TweakDef
            {
                Id = "tspol-enable-automatic-reconnect",
                Label = "RDS: Enable Session Reconnect on Network Drop",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["rdp", "rds", "reconnect", "reliability", "network", "group policy", "terminal services"],
                Description =
                    "Allows RDP clients to automatically reconnect to a disconnected session after "
                    + "a transient network interruption. "
                    + "fDisableAutoReconnect = 0. "
                    + "Improves user experience on unstable network links (Wi-Fi, VPN) by resuming the "
                    + "session without requiring a full new logon. "
                    + "Default: auto-reconnect enabled. This tweak explicitly enforces that policy.",
                ApplyOps = [RegOp.SetDword(TsPol, "fDisableAutoReconnect", 0)],
                RemoveOps = [RegOp.DeleteValue(TsPol, "fDisableAutoReconnect")],
                DetectOps = [RegOp.CheckDword(TsPol, "fDisableAutoReconnect", 0)],
            },
        ];
    }

    // ── WdagFileCachePolicy ──
    private static class _WdagFileCachePolicy
    {
        private const string WdagKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wdagfc-enable-wdag-for-edge",
                    Label = "WDAG: Enable Application Guard for Microsoft Edge",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowAppHVSI_ProviderSet=1 in AppHVSI policy. Enables Windows Defender Application Guard (WDAG) for Microsoft Edge at the enterprise policy level. When WDAG is enabled, Edge opens untrusted websites (those not in the enterprise trusted zone) inside a hardware-isolated Hyper-V container. If a malicious website exploits a browser vulnerability within the WDAG container, the exploit is contained within the isolated environment and cannot access the host OS, its files, credentials, or the clipboard. The container is discarded after the session. This is the highest-grade browser isolation available on Windows.",
                    Tags = ["wdag", "application-guard", "browser-isolation", "hyperv", "container"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 4,
                    ImpactNote =
                        "Edge opens untrusted websites in a Hyper-V container. Requires enterprise-grade hardware (Hyper-V support, SLAT, 8GB+ RAM). Container startup adds 5–15 second latency for the first WDAG session. Copy-paste between container and host is blocked by default unless explicitly enabled by policy. Printing and camera access are also restricted by default.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AllowAppHVSI_ProviderSet", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowAppHVSI_ProviderSet")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AllowAppHVSI_ProviderSet", 1)],
                },
                new TweakDef
                {
                    Id = "wdagfc-block-clipboard-from-container",
                    Label = "WDAG: Block Clipboard from WDAG Container to Host",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSIClipboardSettings=1 in AppHVSI policy (host-to-container only). Configures clipboard sharing so that content can be pasted from the host into the WDAG container (needed to enter URLs or credentials) but content from the container cannot be pasted to the host. This prevents an attack where a compromised WDAG session tries to exfiltrate data by copying it to the clipboard and the user then pastes it outside the container. The asymmetric clipboard policy allows productive use of WDAG without creating an exfiltration channel.",
                    Tags = ["wdag", "clipboard", "exfiltration-prevention", "asymmetric", "container"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Clipboard is one-way: host → container only. Users cannot copy content from WDAG container sessions to the host. Legitimate workflows that require copying content from a WDAG session (e.g., copying a URL from an isolated browser to share) are blocked. Users must retype or use an approved sharing method.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIClipboardSettings", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIClipboardSettings")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIClipboardSettings", 1)],
                },
                new TweakDef
                {
                    Id = "wdagfc-block-print-from-container",
                    Label = "WDAG: Block Printing from the WDAG Container",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSIPrintBlockSettings=0 in AppHVSI policy (all printing blocked). Disables printing from within the WDAG container. WDAG printing is a potential exfiltration vector: a compromised website within the container could automatically trigger printing sensitive data from the host-provided print queue. While this is a low-probability attack (requiring significant user interaction), blocking printing from the container eliminates the risk while having minimal impact — users who need to print document in an isolated session can download the document to an approved location first.",
                    Tags = ["wdag", "print-block", "exfiltration", "container", "network-printer"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Printing from within the WDAG Edge container is disabled. Content accessed in the WDAG container cannot be directly printed. Users who need to print a WDAG page must save and transfer through an approved workflow. Negligible productivity impact for typical web browsing.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIPrintBlockSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIPrintBlockSettings")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIPrintBlockSettings", 0)],
                },
                new TweakDef
                {
                    Id = "wdagfc-disable-container-persistence",
                    Label = "WDAG: Disable Container Persistence (Discard Container on Close)",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowPersistence=0 in AppHVSI policy. Configures WDAG to discard the container state when the WDAG session is closed — the container does not persist browser history, cookies, downloads, or any state between sessions. Each WDAG session starts from a clean container image. Persistence, if enabled, allows attack artefacts (malware files, poisoned cookies, modified registry state) to survive across WDAG sessions and potentially be leveraged in future sessions. Discarding the container eliminates the possibility of session-to-session attack propagation within WDAG.",
                    Tags = ["wdag", "persistence", "container", "discard", "clean-slate"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG container is discarded on session close. No browsing history, cookies, or downloads persist in WDAG. Users who need to return to a WDAG session must log in again to websites. Clean container on each session eliminates attack artefact accumulation.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AllowPersistence", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AllowPersistence")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AllowPersistence", 0)],
                },
                new TweakDef
                {
                    Id = "wdagfc-disable-camera-mic-in-container",
                    Label = "WDAG: Disable Camera and Microphone Access in WDAG Container",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSICameraAndMicrophoneSettings=0 in AppHVSI policy (both disabled). Prevents websites running inside the WDAG container from accessing the host's camera and microphone. Camera and microphone access from an isolated container is a potential privacy and exfiltration risk — a malicious site in the container could silently activate the camera or microphone to capture the user's environment. Since WDAG is intended for untrusted websites, granting media device access undermines the isolation model. The default is blocked; this policy explicitly enforces it.",
                    Tags = ["wdag", "camera", "microphone", "privacy", "media-access"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Websites in the WDAG container cannot access camera or microphone. Video conferencing and voice features in WDAG browser sessions are blocked. This is appropriate for untrusted sites. Trusted sites should not be opened in WDAG — they should be in the enterprise trusted zone.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSICameraAndMicrophoneSettings", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSICameraAndMicrophoneSettings")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSICameraAndMicrophoneSettings", 0)],
                },
                new TweakDef
                {
                    Id = "wdagfc-block-file-download-from-container",
                    Label = "WDAG: Block File Downloads from the WDAG Container to Host",
                    Category = "Remote Desktop",
                    Description =
                        "Sets SaveFilesToHost=0 in AppHVSI policy. Prevents files downloaded within the WDAG container from being saved directly to the host file system. Without this restriction, a malicious site could prompt the user to download and save a file — the file lands on the host's file system outside the container isolation, potentially infecting the host. Blocking file save to host means downloads within WDAG are contained within the isolated environment. Users who need a downloaded file from a WDAG session must go through an approved file transfer workflow.",
                    Tags = ["wdag", "file-download", "exfiltration", "container-escape", "host-protection"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "Files downloaded in the WDAG container cannot be saved to the host. Attempts to save downloads land in the container's temporary storage (discarded on close if persistence is disabled). Users cannot retrieve downloads from WDAG sessions without an explicit transfer mechanism defined by IT.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "SaveFilesToHost", 0)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "SaveFilesToHost")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "SaveFilesToHost", 0)],
                },
                new TweakDef
                {
                    Id = "wdagfc-define-enterprise-network-domain-list",
                    Label = "WDAG: Enable Enterprise Network Isolation (Route Untrusted Sites to Container)",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSIAllowedDomains policy enabled flag=1. Activates the WDAG enterprise network domain isolation feature. Enterprise domains (configured via the Network Isolation policy) are considered trusted and open in the standard Edge host browser. All other domains are considered untrusted and are automatically redirected to the WDAG container. This network isolation approach ensures that users are protected by default without needing to consciously open WDAG — the browser automatically routes untrusted traffic to the container.",
                    Tags = ["wdag", "network-isolation", "domain-routing", "automatic", "enterprise-trusted"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "All non-enterprise-domain sites automatically open in the WDAG container. Requires NetworkIsolation policy to define the trusted enterprise domains. First-time WDAG container startup adds latency. Enterprise trusted zone domains (intranet, SharePoint, corporate portals) must be correctly enumerated for a smooth user experience.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAllowedDomainsEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAllowedDomainsEnabled")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAllowedDomainsEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "wdagfc-enable-wdag-audit-logging",
                    Label = "WDAG: Enable WDAG Container Event Logging",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSIAuditMode=1 in AppHVSI policy. Enables event logging for WDAG container lifecycle and security events: container start, container stop, isolation boundary violations, clipboard policy enforcement, network domain routing decisions, and container crashes. WDAG events are logged to the Microsoft-Windows-Windows-Defender-ApplicationGuard/Operational channel. These events enable IT to track WDAG usage patterns, detect frequent container crashes (indicating exploit attempts), and audit clipboard and print policy enforcement.",
                    Tags = ["wdag", "audit-logging", "container-events", "event-log", "security-monitoring"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG container lifecycle and security events are logged. Logs include container start/stop, boundary violations, and policy enforcement decisions. Low log volume — events are per-container-session, not per-page-load. Enables SIEM alerting on anomalous WDAG behaviour.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIAuditMode", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIAuditMode")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIAuditMode", 1)],
                },
                new TweakDef
                {
                    Id = "wdagfc-restrict-wdag-to-edge-only",
                    Label = "WDAG: Restrict WDAG Isolation to Microsoft Edge Only",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSIBrowserOptions=1 in AppHVSI policy. Restricts WDAG container usage to Microsoft Edge. Prevents other application types from launching their own WDAG containers. If enabled for standalone WDAG (value 2), arbitrary applications can request WDAG isolation. Restricting to Edge-only (value 1) ensures the WDAG container surface is limited to the browser scenario, reducing the attack surface of the WDAG subsystem itself.",
                    Tags = ["wdag", "edge-only", "app-isolation", "restriction", "container-surface"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG containers can only be started by Microsoft Edge. Applications that attempt to use the standalone WDAG API are denied. Reduces WDAG attack surface to the browser scenario only. Standalone WDAG for Office documents (Word, Excel) will not function.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSIBrowserOptions", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSIBrowserOptions")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSIBrowserOptions", 1)],
                },
                new TweakDef
                {
                    Id = "wdagfc-enable-wdag-telemetry",
                    Label = "WDAG: Enable WDAG Diagnostic Telemetry for Threat Intelligence",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AppHVSITelemetry=1 in AppHVSI policy. Enables WDAG to send diagnostic telemetry data to Microsoft when a threat is detected or a container security boundary violation is attempted. WDAG telemetry covers container anomaly detection: unexpected kernel calls from the container, attempts to access host memory, and container process crashes consistent with exploit activity. This telemetry feeds Microsoft's Windows Defender threat intelligence, improving detection of novel browser exploits. In regulated environments, telemetry policy should be reviewed against data handling requirements.",
                    Tags = ["wdag", "telemetry", "threat-intelligence", "diagnostics", "microsoft"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "WDAG sends diagnostic data to Microsoft on detected anomalies. Telemetry includes container explosion attempts and security boundary violations — not browsing content. Review data handling obligations before enabling in regulated industries (HIPAA, PCI-DSS). Disabled by default in some enterprise configurations.",
                    ApplyOps = [RegOp.SetDword(WdagKey, "AppHVSITelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(WdagKey, "AppHVSITelemetry")],
                    DetectOps = [RegOp.CheckDword(WdagKey, "AppHVSITelemetry", 1)],
                },
            ];
    }

    // ── WdagPolicy ──
    private static class _WdagPolicy
    {
        private const string AppHvsi = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\AppHVSI";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "wdagpol-enable-application-guard",
                Label = "Enable Windows Defender Application Guard for Edge",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["wdag", "application guard", "edge", "security", "isolation", "enterprise"],
                Description =
                    "Enables Windows Defender Application Guard (WDAG) for Microsoft Edge via Group Policy. "
                    + "Untrusted websites open in a Hyper-V isolated container, isolating the host from "
                    + "browser-based exploits. AllowAppHVSI_ProviderSet = 1. "
                    + "Default: disabled. Requires Virtualization-Based Security (VBS) and Hyper-V.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AllowAppHVSI_ProviderSet", 1)],
                RemoveOps = [RegOp.SetDword(AppHvsi, "AllowAppHVSI_ProviderSet", 0)],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AllowAppHVSI_ProviderSet", 1)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-clipboard-to-container",
                Label = "WDAG: Block Clipboard from Host to Container",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["wdag", "clipboard", "isolation", "security", "enterprise"],
                Description =
                    "Restricts clipboard operations so content cannot be pasted from the host into the "
                    + "Application Guard container. AppHVSIClipboardSettings = 1 (copy from container only). "
                    + "Prevents credential theft and data exfiltration via clipboard paste into untrusted sites. "
                    + "Default: bidirectional clipboard (0). Hardened value: 1.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 1)],
                RemoveOps = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 0)],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AppHVSIClipboardSettings", 1)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-clipboard-to-host",
                Label = "WDAG: Block Clipboard from Container to Host",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Tags = ["wdag", "clipboard", "isolation", "security", "enterprise"],
                Description =
                    "Restricts clipboard so content inside the Application Guard container cannot be "
                    + "pasted to the host. AppHVSIClipboardSettings = 2 (copy to container only). "
                    + "Prevents malicious container content from reaching host applications. "
                    + "Combine with wdagpol-disable-clipboard-to-container for full isolation (value 3).",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 2)],
                RemoveOps = [RegOp.SetDword(AppHvsi, "AppHVSIClipboardSettings", 0)],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AppHVSIClipboardSettings", 2)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-printing",
                Label = "WDAG: Disable Printing from Application Guard Container",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wdag", "printing", "isolation", "security", "enterprise"],
                Description =
                    "Disables all printing from within the Application Guard container. "
                    + "AppHVSIPrintingSettings = 0 (no printing). "
                    + "Prevents document exfiltration via printing from untrusted container sessions. "
                    + "Default: printing enabled (network, PDF, XPS, local printers all allowed).",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AppHVSIPrintingSettings", 0)],
                RemoveOps = [RegOp.DeleteValue(AppHvsi, "AppHVSIPrintingSettings")],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AppHVSIPrintingSettings", 0)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-data-persistence",
                Label = "WDAG: Disable Container Data Persistence",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["wdag", "persistence", "isolation", "security", "privacy", "enterprise"],
                Description =
                    "Disables data persistence in the Application Guard container. "
                    + "AllowPersistence = 0. Browser data (cookies, history, passwords, downloads) is "
                    + "deleted when the container session ends. "
                    + "Default: persistence disabled. Some orgs enable it for usability — hardened value is 0.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AllowPersistence", 0)],
                RemoveOps = [RegOp.DeleteValue(AppHvsi, "AllowPersistence")],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AllowPersistence", 0)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-camera-microphone",
                Label = "WDAG: Disable Camera and Microphone in Container",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["wdag", "camera", "microphone", "privacy", "isolation", "security", "enterprise"],
                Description =
                    "Prevents the Application Guard container from accessing the camera and microphone. "
                    + "AllowCameraMicrophoneRedirection = 0. "
                    + "Stops untrusted browser sessions from recording the user without consent. "
                    + "Default: access disabled. Must be explicitly enabled if required.",
                MinBuild = 18362,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AllowCameraMicrophoneRedirection", 0)],
                RemoveOps = [RegOp.DeleteValue(AppHvsi, "AllowCameraMicrophoneRedirection")],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AllowCameraMicrophoneRedirection", 0)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-virtual-gpu",
                Label = "WDAG: Disable Virtual GPU in Container",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["wdag", "gpu", "isolation", "security", "enterprise"],
                Description =
                    "Disables hardware-accelerated rendering in the Application Guard container. "
                    + "AllowVirtualGPU = 0. Reduces GPU attack surface — a compromised vGPU driver "
                    + "could potentially escape the container. Rendering falls back to software. "
                    + "Default: hardware GPU disabled for maximum isolation.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AllowVirtualGPU", 0)],
                RemoveOps = [RegOp.DeleteValue(AppHvsi, "AllowVirtualGPU")],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AllowVirtualGPU", 0)],
            },
            new TweakDef
            {
                Id = "wdagpol-enable-audit-mode",
                Label = "WDAG: Enable Audit Mode for Application Guard",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["wdag", "audit", "logging", "security", "enterprise", "siem"],
                Description =
                    "Enables audit logging for Application Guard container events. "
                    + "AuditApplicationGuard = 1. Events are logged to the Windows event log "
                    + "(Microsoft-Windows-Windows Defender Application Guard/Operational). "
                    + "Useful for SIEM integration and security monitoring of container activity.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "AuditApplicationGuard", 1)],
                RemoveOps = [RegOp.SetDword(AppHvsi, "AuditApplicationGuard", 0)],
                DetectOps = [RegOp.CheckDword(AppHvsi, "AuditApplicationGuard", 1)],
            },
            new TweakDef
            {
                Id = "wdagpol-disable-download-to-host",
                Label = "WDAG: Block Saving Container Downloads to Host",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["wdag", "downloads", "isolation", "security", "enterprise"],
                Description =
                    "Prevents files downloaded in the Application Guard container from being saved "
                    + "to the host filesystem. SaveFilesToHost = 0. "
                    + "Stops container-side malware payloads from escaping isolation via the Downloads folder. "
                    + "Default: download-to-host disabled for maximum isolation.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "SaveFilesToHost", 0)],
                RemoveOps = [RegOp.DeleteValue(AppHvsi, "SaveFilesToHost")],
                DetectOps = [RegOp.CheckDword(AppHvsi, "SaveFilesToHost", 0)],
            },
            new TweakDef
            {
                Id = "wdagpol-configure-network-isolation",
                Label = "WDAG: Enable Network Isolation for Container",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 5,
                SafetyRating = 5,
                Tags = ["wdag", "network", "isolation", "security", "enterprise"],
                Description =
                    "Sets WDAG to use an isolated network namespace for the container. "
                    + "NetworkIsolation = 1. The container gets a separate virtual NIC that cannot "
                    + "reach internal corporate resources, only the public internet. "
                    + "Prevents lateral movement from a compromised browser session to internal servers.",
                MinBuild = 16299,
                ApplyOps = [RegOp.SetDword(AppHvsi, "NetworkIsolation", 1)],
                RemoveOps = [RegOp.SetDword(AppHvsi, "NetworkIsolation", 0)],
                DetectOps = [RegOp.CheckDword(AppHvsi, "NetworkIsolation", 1)],
            },
        ];
    }

    // ── WinRmHardening ──
    private static class _WinRmHardening
    {
        private const string Client = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
        private const string Service = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "winrm-client-no-basic",
                Label = "Disable WinRM Client Basic Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Prevents WinRM clients from using Basic authentication, which transmits "
                    + "credentials in Base64-encoded plaintext over the network. AllowBasic=0.",
                Tags = ["winrm", "authentication", "basic auth", "security", "remote management"],
                RegistryKeys = [Client],
                ApplyOps = [RegOp.SetDword(Client, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Client, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrm-client-no-unencrypted",
                Label = "Disable Unencrypted WinRM Client Traffic",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Forces WinRM client connections to always use encrypted channels. "
                    + "Prevents plaintext remote management sessions over the network. "
                    + "AllowUnencrypted=0.",
                Tags = ["winrm", "encryption", "plaintext", "security", "remote management"],
                RegistryKeys = [Client],
                ApplyOps = [RegOp.SetDword(Client, "AllowUnencrypted", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowUnencrypted")],
                DetectOps = [RegOp.CheckDword(Client, "AllowUnencrypted", 0)],
            },
            new TweakDef
            {
                Id = "winrm-client-no-digest",
                Label = "Disable WinRM Client Digest Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disables Digest authentication for WinRM clients. Digest is vulnerable "
                    + "to offline dictionary and replay attacks. AllowDigest=0.",
                Tags = ["winrm", "digest", "authentication", "security"],
                RegistryKeys = [Client],
                ApplyOps = [RegOp.SetDword(Client, "AllowDigest", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowDigest")],
                DetectOps = [RegOp.CheckDword(Client, "AllowDigest", 0)],
            },
            new TweakDef
            {
                Id = "winrm-client-no-negotiate",
                Label = "Disable WinRM Client NTLM (Negotiate) Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Prevents WinRM clients from using NTLM (Negotiate) authentication. "
                    + "In Kerberos-capable environments, this forces Kerberos-only remote "
                    + "management. AllowNegotiate=0. May break cross-forest management.",
                Tags = ["winrm", "ntlm", "negotiate", "kerberos", "security"],
                RegistryKeys = [Client],
                ApplyOps = [RegOp.SetDword(Client, "AllowNegotiate", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowNegotiate")],
                DetectOps = [RegOp.CheckDword(Client, "AllowNegotiate", 0)],
            },
            new TweakDef
            {
                Id = "winrm-client-no-credssp",
                Label = "Disable WinRM Client CredSSP Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Disables CredSSP (Credential Security Support Provider) in WinRM clients. "
                    + "CredSSP delegates user credentials to the remote host, enabling "
                    + "credential theft if the remote host is compromised. AllowCredSSP=0.",
                Tags = ["winrm", "credssp", "credential delegation", "security"],
                RegistryKeys = [Client],
                ApplyOps = [RegOp.SetDword(Client, "AllowCredSSP", 0)],
                RemoveOps = [RegOp.DeleteValue(Client, "AllowCredSSP")],
                DetectOps = [RegOp.CheckDword(Client, "AllowCredSSP", 0)],
            },
            new TweakDef
            {
                Id = "winrm-service-no-basic",
                Label = "Disable WinRM Service Basic Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Prevents the WinRM service (listener) from accepting Basic authentication "
                    + "requests from remote clients. AllowBasic=0 on the service side.",
                Tags = ["winrm", "service", "basic auth", "security", "listener"],
                RegistryKeys = [Service],
                ApplyOps = [RegOp.SetDword(Service, "AllowBasic", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowBasic")],
                DetectOps = [RegOp.CheckDword(Service, "AllowBasic", 0)],
            },
            new TweakDef
            {
                Id = "winrm-service-no-unencrypted",
                Label = "Disable Unencrypted WinRM Service Traffic",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Forces the WinRM service listener to require encrypted sessions for "
                    + "all incoming remote management connections. AllowUnencrypted=0.",
                Tags = ["winrm", "service", "encryption", "security"],
                RegistryKeys = [Service],
                ApplyOps = [RegOp.SetDword(Service, "AllowUnencrypted", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowUnencrypted")],
                DetectOps = [RegOp.CheckDword(Service, "AllowUnencrypted", 0)],
            },
            new TweakDef
            {
                Id = "winrm-service-no-negotiate",
                Label = "Disable WinRM Service NTLM (Negotiate) Authentication",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Prevents the WinRM service from accepting NTLM-based authentication. "
                    + "In a fully Kerberos domain, this enforces Kerberos-only remote access. "
                    + "AllowNegotiate=0. May break workgroup/non-domain management.",
                Tags = ["winrm", "service", "ntlm", "kerberos", "security"],
                RegistryKeys = [Service],
                ApplyOps = [RegOp.SetDword(Service, "AllowNegotiate", 0)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowNegotiate")],
                DetectOps = [RegOp.CheckDword(Service, "AllowNegotiate", 0)],
            },
            new TweakDef
            {
                Id = "winrm-service-disable-runas",
                Label = "Disable WinRM Service RunAs Feature",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disables the ability for WinRM service plug-ins to run as a different " + "user account via the RunAs feature. DisableRunAs=1.",
                Tags = ["winrm", "service", "runas", "privilege", "security"],
                RegistryKeys = [Service],
                ApplyOps = [RegOp.SetDword(Service, "DisableRunAs", 1)],
                RemoveOps = [RegOp.DeleteValue(Service, "DisableRunAs")],
                DetectOps = [RegOp.CheckDword(Service, "DisableRunAs", 1)],
            },
            new TweakDef
            {
                Id = "winrm-service-allow-kerberos",
                Label = "Explicitly Allow Kerberos for WinRM Service",
                Category = "Remote Desktop",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Explicitly enables Kerberos authentication on the WinRM service. "
                    + "Combine with disabling Basic/NTLM to enforce Kerberos-only remote "
                    + "management. AllowKerberos=1.",
                Tags = ["winrm", "service", "kerberos", "authentication", "security"],
                RegistryKeys = [Service],
                ApplyOps = [RegOp.SetDword(Service, "AllowKerberos", 1)],
                RemoveOps = [RegOp.DeleteValue(Service, "AllowKerberos")],
                DetectOps = [RegOp.CheckDword(Service, "AllowKerberos", 1)],
            },
        ];
    }

    // ── WinRmPolicy ──
    private static class _WinRmPolicy
    {
        private const string Client = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
        private const string Service = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "winrmpol-disable-basic-auth-client",
                    Label = "Disable Basic Authentication on WinRM Client",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM client from using HTTP Basic authentication, which transmits credentials in a reversibly encoded form. Forces use of Kerberos, NTLM, or certificate-based authentication instead. Default: Basic auth allowed. Recommended: 1 to prevent credential interception.",
                    Tags = ["winrm", "remoting", "authentication", "basic-auth", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client refuses Basic authentication; credentials cannot be sent in cleartext over HTTP.",
                    ApplyOps = [RegOp.SetDword(Client, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Client, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(Client, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-disable-basic-auth-service",
                    Label = "Disable Basic Authentication on WinRM Service",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM service (listener) from accepting Basic authentication requests. Complementary to the client-side setting — both must be set to fully eliminate Basic auth from the WinRM channel. Default: Basic auth accepted by service. Recommended: 1.",
                    Tags = ["winrm", "remoting", "authentication", "basic-auth", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM service rejects Basic authentication; only Kerberos / NTLM / certificate connections succeed.",
                    ApplyOps = [RegOp.SetDword(Service, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Service, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(Service, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-require-encrypted-traffic-client",
                    Label = "Require Encrypted Traffic on WinRM Client",
                    Category = "Remote Desktop",
                    Description =
                        "Forces the WinRM client to use encrypted transport (HTTPS or Kerberos message-level encryption) for all remote management sessions. Plaintext HTTP-based sessions are refused. Default: unencrypted HTTP sessions permitted. Recommended: 1 on all managed endpoints.",
                    Tags = ["winrm", "remoting", "encryption", "https", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client only uses encrypted channels; cleartext remote management sessions are blocked.",
                    ApplyOps = [RegOp.SetDword(Client, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Client, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(Client, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-require-encrypted-traffic-service",
                    Label = "Require Encrypted Traffic on WinRM Service",
                    Category = "Remote Desktop",
                    Description =
                        "Forces the WinRM listener to reject any inbound session that does not use transport-level or message-level encryption. Prevents man-in-the-middle interception of remote management traffic. Default: unencrypted connections accepted. Recommended: 1.",
                    Tags = ["winrm", "remoting", "encryption", "https", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote =
                        "WinRM service rejects non-encrypted inbound connections; remote PowerShell/WMI sessions must use HTTPS or Kerberos.",
                    ApplyOps = [RegOp.SetDword(Service, "AllowUnencryptedTraffic", 0)],
                    RemoveOps = [RegOp.DeleteValue(Service, "AllowUnencryptedTraffic")],
                    DetectOps = [RegOp.CheckDword(Service, "AllowUnencryptedTraffic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-disable-digest-auth-client",
                    Label = "Disable Digest Authentication on WinRM Client",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM client from offering Digest authentication. Digest auth sends a hash of the credentials that can be cracked offline. Kerberos or certificate auth should be used instead. Default: Digest allowed on client. Recommended: 1.",
                    Tags = ["winrm", "remoting", "digest-auth", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client does not offer Digest auth; offline hash-cracking attacks against captured sessions are prevented.",
                    ApplyOps = [RegOp.SetDword(Client, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(Client, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(Client, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-disable-credential-delegation",
                    Label = "Disable Credential Delegation in WinRM",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents WinRM sessions from delegating the user's credentials to the remote machine (CredSSP / AllowCredSSP=0). Credential delegation is the basis of pass-the-credentials attacks — the remote host receives usable Kerberos tickets. Disable unless explicitly required for multi-hop scenarios. Default: CredSSP delegation permitted. Recommended: 1.",
                    Tags = ["winrm", "remoting", "credssp", "delegation", "credentials", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM CredSSP delegation is blocked; multi-hop credential forwarding attacks via remote management are prevented.",
                    ApplyOps = [RegOp.SetDword(Client, "AllowCredSSP", 0)],
                    RemoveOps = [RegOp.DeleteValue(Client, "AllowCredSSP")],
                    DetectOps = [RegOp.CheckDword(Client, "AllowCredSSP", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-disable-credssp-service",
                    Label = "Disable CredSSP on WinRM Service",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM service from accepting CredSSP-authenticated inbound connections. Blocking CredSSP at the service prevents the remote endpoint from collecting forwarded credentials even if an attacker manipulates the client configuration. Default: service accepts CredSSP. Recommended: 1.",
                    Tags = ["winrm", "remoting", "credssp", "service", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM service refuses CredSSP logons; remote credential harvesting via CredSSP is blocked.",
                    ApplyOps = [RegOp.SetDword(Service, "AllowCredSSP", 0)],
                    RemoveOps = [RegOp.DeleteValue(Service, "AllowCredSSP")],
                    DetectOps = [RegOp.CheckDword(Service, "AllowCredSSP", 0)],
                },
                new TweakDef
                {
                    Id = "winrmpol-restrict-trusted-hosts",
                    Label = "Restrict WinRM Trusted Hosts to Empty List",
                    Category = "Remote Desktop",
                    Description =
                        "Sets the WinRM TrustedHosts list to empty, preventing the client from connecting to non-domain machines using NTLM. Trusted hosts bypass server certificate validation; an empty list forces certificate or Kerberos authentication. Default: TrustedHosts may be set by users. Recommended: 1 (empty list) in domain environments.",
                    Tags = ["winrm", "remoting", "trusted-hosts", "ntlm", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "TrustedHosts is locked to empty; WinRM cannot connect to non-domain hosts via NTLM without explicit admin configuration.",
                    ApplyOps = [RegOp.SetString(Client, "TrustedHosts", "")],
                    RemoveOps = [RegOp.DeleteValue(Client, "TrustedHosts")],
                    DetectOps = [RegOp.CheckString(Client, "TrustedHosts", "")],
                },
                new TweakDef
                {
                    Id = "winrmpol-disable-winrm-service",
                    Label = "Disable WinRM Service Autostart",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the Windows Remote Management service from starting automatically. On endpoints that do not require remote management (most workstations), disabling WinRM removes the remote PowerShell attack surface entirely. Default: WinRM may be enabled on domain machines via Group Policy. Recommended: 1 on non-managed or non-admin workstations.",
                    Tags = ["winrm", "remoting", "service", "disabled", "attack-surface", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "WinRM service is disabled; remote PowerShell, remote WMI, and DSC push configurations do not work until re-enabled.",
                    ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM", "Start", 4)],
                    RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM", "Start", 3)],
                    DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WinRM", "Start", 4)],
                },
                new TweakDef
                {
                    Id = "winrmpol-enable-audit-logging",
                    Label = "Enable WinRM Session Audit Logging",
                    Category = "Remote Desktop",
                    Description =
                        "Records successful and failed WinRM authentication attempts, session creation, and session teardown events in the Windows event log (Microsoft-Windows-WinRM/Operational). Provides forensic visibility into remote management activity. Default: WinRM operational log not always enabled. Recommended: 1 on all endpoints.",
                    Tags = ["winrm", "remoting", "audit", "logging", "forensics", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WinRM session events (connect, authenticate, disconnect) are written to the Operational event channel.",
                    ApplyOps = [RegOp.SetDword(Service, "EnableVerboseLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Service, "EnableVerboseLogging")],
                    DetectOps = [RegOp.CheckDword(Service, "EnableVerboseLogging", 1)],
                },
            ];
    }

    // ── WinRmRemoteShellPolicy ──
    private static class _WinRmRemoteShellPolicy
    {
        private const string ShellKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service\RemoteShell";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "rshpol-disable-remote-shell",
                    Label = "Disable WinRM Remote Shell Access",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowRemoteShellAccess=0 to disable remote shell access over WinRM entirely. Blocks winrs.exe connections and PowerShell remoting sessions initiated from remote machines, limiting interactive shell exposure.",
                    Tags = ["winrm", "remote-shell", "access", "policy", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote = "Blocks winrs and PowerShell remoting shells; WSMAN API and Invoke-Command may also fail.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "AllowRemoteShellAccess", 0)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowRemoteShellAccess")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "AllowRemoteShellAccess", 0)],
                },
                new TweakDef
                {
                    Id = "rshpol-limit-shells-per-user",
                    Label = "Limit WinRM Shells per User",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxShellsPerUser=2 in the RemoteShell policy. Caps the number of concurrent WinRM remote shells a single user can open, mitigating resource exhaustion from shell flooding attacks.",
                    Tags = ["winrm", "shell", "quota", "limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Limits remote shells to 2 per user; legitimate automation may need a higher value.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "MaxShellsPerUser", 2)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxShellsPerUser")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "MaxShellsPerUser", 2)],
                },
                new TweakDef
                {
                    Id = "rshpol-limit-concurrent-users",
                    Label = "Limit Concurrent WinRM Shell Users",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxConcurrentUsers=5 in the RemoteShell policy. Restricts the total number of users who can run simultaneous WinRM remote shells on this endpoint, bounding server load from large-scale remoting campaigns.",
                    Tags = ["winrm", "concurrent", "users", "quota", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Limits to 5 concurrent remote shell users; increase for servers used by larger teams.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "MaxConcurrentUsers", 5)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxConcurrentUsers")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "MaxConcurrentUsers", 5)],
                },
                new TweakDef
                {
                    Id = "rshpol-set-idle-timeout",
                    Label = "Set WinRM Shell Idle Timeout (1 min)",
                    Category = "Remote Desktop",
                    Description =
                        "Sets IdleTimeoutms=60000 (1 minute) in the RemoteShell policy. Disconnects remote shell sessions that have been idle beyond the threshold, reclaiming server resources and reducing the attack window of orphaned sessions.",
                    Tags = ["winrm", "timeout", "idle", "session", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Terminates shells idle for >1 min; increase to 300000ms (5 min) if admins need longer pauses.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "IdleTimeoutms", 60000)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "IdleTimeoutms")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "IdleTimeoutms", 60000)],
                },
                new TweakDef
                {
                    Id = "rshpol-set-shell-run-time",
                    Label = "Limit WinRM Shell Maximum Run Time",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxShellRunTime=900000 (15 minutes) in the RemoteShell policy. Forces termination of remote shells running longer than the threshold, preventing long-running background processes from persisting through WinRM sessions.",
                    Tags = ["winrm", "runtime", "timeout", "shell", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Kills shells after 15 min; long-running admin scripts may be interrupted.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "MaxShellRunTime", 900000)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxShellRunTime")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "MaxShellRunTime", 900000)],
                },
                new TweakDef
                {
                    Id = "rshpol-limit-processes-per-shell",
                    Label = "Limit WinRM Processes per Shell",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxProcessesPerShell=5 in the RemoteShell policy. Caps the number of child processes a single WinRM shell session can spawn, limiting post-exploitation process trees from remote shell access.",
                    Tags = ["winrm", "processes", "quota", "limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Limits remote shell to 5 child processes; complex scripts spawning many processes may fail.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "MaxProcessesPerShell", 5)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxProcessesPerShell")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "MaxProcessesPerShell", 5)],
                },
                new TweakDef
                {
                    Id = "rshpol-limit-memory-per-shell",
                    Label = "Limit WinRM Shell Memory (150 MB)",
                    Category = "Remote Desktop",
                    Description =
                        "Sets MaxMemoryPerShellMB=150 in the RemoteShell policy. Restricts total RAM available to a single WinRM remote shell session, preventing memory exhaustion attacks from intensive remote operations.",
                    Tags = ["winrm", "memory", "quota", "limit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Shell terminated when it uses >150 MB; increase for data-intensive remote admin tasks.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "MaxMemoryPerShellMB", 150)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "MaxMemoryPerShellMB")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "MaxMemoryPerShellMB", 150)],
                },
                new TweakDef
                {
                    Id = "rshpol-block-env-variables",
                    Label = "Block Environment Variable Modification in WinRM Shells",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowEnvironmentVariables=0 in the RemoteShell policy. Prevents remote WinRM shells from setting or overriding environment variables, reducing the risk of PATH hijacking or credential injection via env variable manipulation.",
                    Tags = ["winrm", "environment", "variables", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks env variable changes in remote shells; scripts relying on custom env vars may fail.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "AllowEnvironmentVariables", 0)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowEnvironmentVariables")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "AllowEnvironmentVariables", 0)],
                },
                new TweakDef
                {
                    Id = "rshpol-block-interactive-shell",
                    Label = "Block Interactive WinRM Shell Sessions",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowInteractiveShell=0 in the RemoteShell policy. Blocks the creation of interactive WinRM shell sessions (winrs -r:<server> cmd) while still allowing non-interactive command execution, limiting attacker-controlled shell access.",
                    Tags = ["winrm", "interactive", "shell", "access", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks interactive winrs shells; non-interactive Invoke-Command sessions are unaffected.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "AllowInteractiveShell", 0)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowInteractiveShell")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "AllowInteractiveShell", 0)],
                },
                new TweakDef
                {
                    Id = "rshpol-disable-remote-shell-inbound",
                    Label = "Disable WinRM Inbound Remote Shell",
                    Category = "Remote Desktop",
                    Description =
                        "Sets AllowRemoteShellInbound=0 in the RemoteShell policy. Prevents this machine from accepting inbound WinRM remote shell connections while still permitting outbound WinRM sessions to remote targets.",
                    Tags = ["winrm", "inbound", "shell", "hardening", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Blocks this machine as a WinRM target; it can still initiate outbound WinRM connections.",
                    ApplyOps = [RegOp.SetDword(ShellKey, "AllowRemoteShellInbound", 0)],
                    RemoveOps = [RegOp.DeleteValue(ShellKey, "AllowRemoteShellInbound")],
                    DetectOps = [RegOp.CheckDword(ShellKey, "AllowRemoteShellInbound", 0)],
                },
            ];
    }

    // ── WinRmSecurityPolicy ──
    private static class _WinRmSecurityPolicy
    {
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Service";
        private const string CliKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM\Client";
        private const string WsmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WinRM";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "winrmadv-disable-basic-auth-service",
                    Label = "Disable WinRM Basic Authentication on Service (Listener)",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM service from accepting Basic authentication credentials, which transmit usernames and passwords in Base64 without encryption. Forces use of Kerberos or CredSSP authenticated sessions.",
                    Tags = ["winrm", "basic-auth", "authentication", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM Basic authentication disabled on service; Kerberos or CredSSP required for remote management.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmadv-disable-basic-auth-client",
                    Label = "Disable WinRM Basic Authentication on Client",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM client from sending Basic authentication credentials to remote hosts, ensuring WinRM connections from this machine always use authenticated and encrypted transport.",
                    Tags = ["winrm", "basic-auth", "client", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM client Basic authentication disabled; remote management connections require Kerberos/CredSSP.",
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowBasic", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowBasic")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowBasic", 0)],
                },
                new TweakDef
                {
                    Id = "winrmadv-require-https-transport",
                    Label = "Require HTTPS Transport for All WinRM Connections",
                    Category = "Remote Desktop",
                    Description =
                        "Configures WinRM to only accept management sessions over HTTPS (port 5986), blocking unencrypted HTTP WinRM connections (port 5985) that transmit management traffic in plaintext.",
                    Tags = ["winrm", "https", "transport", "encryption", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "WinRM HTTP (5985) blocked; only HTTPS (5986) remote management sessions accepted.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowUnencrypted", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowUnencrypted")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowUnencrypted", 0)],
                },
                new TweakDef
                {
                    Id = "winrmadv-disable-client-digest-auth",
                    Label = "Disable WinRM Client Digest Authentication",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM client from using Digest authentication to remote hosts, as Digest sends password hashes that are susceptible to pass-the-hash attacks in non-Kerberos environments.",
                    Tags = ["winrm", "digest-auth", "client", "pass-the-hash", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM Digest authentication disabled on client; hash-based auth not sent to remote endpoints.",
                    ApplyOps = [RegOp.SetDword(CliKey, "AllowDigest", 0)],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "AllowDigest")],
                    DetectOps = [RegOp.CheckDword(CliKey, "AllowDigest", 0)],
                },
                new TweakDef
                {
                    Id = "winrmadv-set-idle-timeout",
                    Label = "Set WinRM Session Idle Timeout to 7200 Seconds (2 Hours)",
                    Category = "Remote Desktop",
                    Description =
                        "Sets the WinRM service idle session timeout to 7200 seconds, ensuring that management sessions that have been idle for more than 2 hours are automatically terminated, preventing stale privileged sessions.",
                    Tags = ["winrm", "session-timeout", "idle", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinRM idle timeout set to 2 hours; stale privileged remote management sessions auto-terminated.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "IdleTimeoutms", 7200000)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "IdleTimeoutms")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "IdleTimeoutms", 7200000)],
                },
                new TweakDef
                {
                    Id = "winrmadv-set-max-connections",
                    Label = "Limit Maximum Concurrent WinRM Management Connections",
                    Category = "Remote Desktop",
                    Description =
                        "Sets the maximum number of concurrent WinRM management sessions to 25, preventing resource exhaustion from session flooding attacks against the WinRM listener.",
                    Tags = ["winrm", "max-connections", "dos-prevention", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WinRM concurrent session limit set to 25; session flooding against management endpoint prevented.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "MaxConnections", 25)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "MaxConnections")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "MaxConnections", 25)],
                },
                new TweakDef
                {
                    Id = "winrmadv-restrict-trusted-hosts-empty",
                    Label = "Clear WinRM Trusted Hosts (Require Kerberos Domain Auth)",
                    Category = "Remote Desktop",
                    Description =
                        "Clears the WinRM TrustedHosts list, preventing workgroup/NTLM authentication to arbitrary hosts and requiring all WinRM connections to use Kerberos domain authentication.",
                    Tags = ["winrm", "trusted-hosts", "kerberos", "ntlm", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "TrustedHosts cleared; WinRM connections require Kerberos domain auth. Workgroup NTLM auth disabled.",
                    ApplyOps = [RegOp.SetString(CliKey, "TrustedHosts", "")],
                    RemoveOps = [RegOp.DeleteValue(CliKey, "TrustedHosts")],
                    DetectOps = [RegOp.CheckString(CliKey, "TrustedHosts", "")],
                },
                new TweakDef
                {
                    Id = "winrmadv-disable-credssp-service",
                    Label = "Disable CredSSP Authentication on WinRM Service",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents the WinRM service from accepting CredSSP (Credential Security Support Provider) authentication, which delegates full NTLM/Kerberos credentials to the remote host and is the credential delegation method most exploited in pass-the-credential attacks.",
                    Tags = ["winrm", "credssp", "credential-delegation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "CredSSP authentication on WinRM service disabled; credential delegation to remote hosts blocked.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "AllowCredSSP", 0)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "AllowCredSSP")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "AllowCredSSP", 0)],
                },
                new TweakDef
                {
                    Id = "winrmadv-disable-winrm-telemetry",
                    Label = "Disable WinRM / WSMAN Telemetry to Microsoft",
                    Category = "Remote Desktop",
                    Description =
                        "Prevents WinRM / WSMAN from sending authentication event, session usage, and protocol negotiation telemetry to Microsoft.",
                    Tags = ["winrm", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "WinRM telemetry to Microsoft disabled; remote management session data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(WsmKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsmKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(WsmKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "winrmadv-log-authentication-failures",
                    Label = "Log WinRM Authentication Failure Events in Security Log",
                    Category = "Remote Desktop",
                    Description =
                        "Enables Security event log entries for all failed WinRM authentication attempts, providing visibility into brute-force and credential stuffing attacks against the remote management endpoint.",
                    Tags = ["winrm", "auth-failure", "event-log", "security-audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "WinRM auth failure events logged in Security log; brute-force attempts visible for SIEM alerting.",
                    ApplyOps = [RegOp.SetDword(WsmKey, "LogAuthFailures", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsmKey, "LogAuthFailures")],
                    DetectOps = [RegOp.CheckDword(WsmKey, "LogAuthFailures", 1)],
                },
            ];
    }
}
