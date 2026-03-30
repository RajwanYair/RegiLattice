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
