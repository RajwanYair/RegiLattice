namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RealVnc
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "vnc-enforce-encryption",
            Label = "VNC: Enforce Encryption",
            Category = "RealVNC",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets VNC authentication to VncAuth + System authentication.",
            Tags = ["vnc", "security", "authentication"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SingleSignOn"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth")],
        },
        new TweakDef
        {
            Id = "vnc-idle-timeout",
            Label = "VNC: 1-Hour Idle Timeout",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disconnects idle VNC sessions after 1 hour (3600s).",
            Tags = ["vnc", "security", "timeout"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 3600)],
        },
        new TweakDef
        {
            Id = "vnc-blank-screen",
            Label = "VNC: Blank Screen When Connected",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Blanks the local monitor during an active VNC session for privacy.",
            Tags = ["vnc", "security", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "WhenConnected"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "Never"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "BlankScreen", "WhenConnected")],
        },
        new TweakDef
        {
            Id = "vnc-no-clipboard",
            Label = "VNC: Disable Clipboard Sharing",
            Category = "RealVNC",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables RealVNC automatic update checks. Updates must be applied manually. Default: Enabled. Recommended: Disabled for managed deployments.",
            Tags = ["realvnc", "vnc", "update"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "AutoUpdate", 0)],
        },
        new TweakDef
        {
            Id = "vnc-realvnc-optimize-encoding",
            Label = "RealVNC Optimize Encoding",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets VNC encoding to ZRLE for best compression ratio over slow networks. Reduces bandwidth usage. Default: Auto. Recommended: ZRLE for WAN connections.",
            Tags = ["realvnc", "vnc", "encoding", "performance", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding", "ZRLE"),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "PreferredEncoding", "ZRLE")],
        },
        new TweakDef
        {
            Id = "vnc-session-timeout",
            Label = "VNC: Set Idle Session Timeout (30 min)",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets VNC idle session timeout to 30 minutes (1800 seconds). Automatically disconnects idle VNC sessions for security. Default: no timeout. Recommended: 1800.",
            Tags = ["vnc", "timeout", "idle", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 1800),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "IdleTimeout", 1800)],
        },
        new TweakDef
        {
            Id = "vnc-disable-clipboard",
            Label = "VNC: Disable Clipboard Sharing (DWord)",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables clipboard sharing between VNC server and clients via DWORD value. Prevents data leakage through clipboard transfer. Default: enabled. Recommended: disabled for DLP.",
            Tags = ["vnc", "clipboard", "dlp", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisableClipboard", 1)],
        },
        new TweakDef
        {
            Id = "vnc-encryption-always",
            Label = "VNC: Enforce Encryption Always On (Policy)",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Forces VNC encryption to AlwaysOn via group policy key and sets EncryptionForced flag. Ensures connections are always encrypted regardless of server config. Default: PreferOn. Recommended: AlwaysOn.",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables file transfer capability in VNC sessions. Prevents users from transferring files via the VNC connection. Default: enabled. Recommended: disabled for DLP.",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets VNC authentication to SystemAuth (Windows credentials). Uses OS-level authentication instead of VNC-specific password. Default: VncAuth. Recommended: SystemAuth for enterprise.",
            Tags = ["vnc", "auth", "system", "security"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "VncAuth+SystemAuth"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SingleSignOn"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "Authentication", "SystemAuth")],
        },
        new TweakDef
        {
            Id = "vnc-query-on-connect",
            Label = "Prompt User on Incoming VNC Connection",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables QueryConnect — shows a dialog on the physical machine asking the logged-in user to accept or reject each incoming VNC connection. Default: disabled. Recommended: enabled for attended machines.",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets DisconnectAction=Lock so the screen locks when a VNC session cleanly disconnects. Prevents leaving an unlocked desktop after remote access. Default: Nothing. Recommended: Lock.",
            Tags = ["vnc", "security", "disconnect", "lock", "session"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Lock"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Nothing"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "DisconnectAction", "Lock")],
        },
        new TweakDef
        {
            Id = "vnc-lost-conn-lock",
            Label = "Lock Screen When VNC Connection Drops",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets LostConnAction=Lock so the screen locks when a VNC connection is unexpectedly terminated (network drop, client crash). Default: Nothing. Recommended: Lock.",
            Tags = ["vnc", "security", "lost-connection", "lock", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Lock"),
            ],
            RemoveOps =
            [
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Nothing"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "LostConnAction", "Lock")],
        },
        new TweakDef
        {
            Id = "vnc-viewer-fullscreen",
            Label = "VNC Viewer: Open in Fullscreen by Default",
            Category = "RealVNC",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Configures RealVNC Viewer to open remote sessions in fullscreen mode automatically, maximising the workspace for remote control. Default: windowed. Recommended: fullscreen for power users.",
            Tags = ["vnc", "viewer", "fullscreen", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\RealVNC\vncviewer", "FullScreen", 1)],
        },
        new TweakDef
        {
            Id = "vnc-disable-share-desktop",
            Label = "VNC: Disable Desktop Sharing (Exclusive Access)",
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables desktop sharing so only one VNC viewer can connect at a time (exclusive access). Prevents multiple simultaneous viewers from watching a session. Default: shared. Recommended: exclusive.",
            Tags = ["vnc", "security", "share", "exclusive", "access"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\RealVNC\vncserver", "ShareDesktop", 0)],
        },
        new TweakDef
        {
            Id = "vnc-set-idle-timeout-300",
            Label = "Set VNC Idle Timeout to 5 Minutes",
            Category = "RealVNC",
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
            Category = "RealVNC",
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
            Category = "RealVNC",
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
            Category = "RealVNC",
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
            Category = "RealVNC",
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
            Category = "RealVNC",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Hides the VNC Server system tray icon. Keeps VNC running without a visible indicator. Useful for kiosk or embedded scenarios. Default: shown.",
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
            Category = "RealVNC",
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
            Category = "RealVNC",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables VNC Viewer from storing recent connection history. Enhances privacy by not recording server addresses. Default: stored.",
            Tags = ["vnc", "viewer", "recent", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\RealVNC\vncviewer", "RememberConnections", 0)],
        },
    ];
}
