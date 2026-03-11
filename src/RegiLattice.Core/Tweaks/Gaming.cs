namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Gaming
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "game-disable-game-mode",
            Label = "Disable Windows Game Mode",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Game Mode which can cause stutter in some games.",
            Tags = ["gaming", "performance", "game-mode"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AllowAutoGameMode", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AllowAutoGameMode"),
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-xbox-services",
            Label = "Disable Xbox Background Services",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Xbox Auth Manager and Xbox Game Save services. Frees resources if you don't use Xbox Live features. Options: 3=Manual, 4=Disabled. Default: Manual. Recommended: Disabled.",
            Tags = ["gaming", "xbox", "services", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 4),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 3),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-game-input-redirect",
            Label = "Disable Game Input Redirection",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the GameInput service to prevent input redirection overhead.",
            Tags = ["gaming", "input", "services"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-gpu-scheduling",
            Label = "Enable Hardware-Accelerated GPU Scheduling",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables hardware-accelerated GPU scheduling (HAGS) for reduced latency and improved frame scheduling on supported GPUs.",
            Tags = ["gaming", "performance", "gpu", "hags"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "game-disable-xbox-game-monitoring",
            Label = "Disable Xbox Game Monitoring Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Xbox Game Monitoring (xbgm) background service.",
            Tags = ["gaming", "xbox", "services", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm", "Start", 4),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm", "Start", 3),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-nagles-algorithm",
            Label = "Disable Nagle's Algorithm (Low Latency)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Nagle's algorithm via TCPNoDelay for lower network latency in multiplayer games.",
            Tags = ["gaming", "network", "latency", "tcp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
        },
        new TweakDef
        {
            Id = "game-gaming-disable-dvr-background",
            Label = "Disable Game DVR Background Recording",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Game DVR background recording. Frees GPU encoder and disk I/O resources. Default: Enabled. Recommended: Disabled for maximum FPS.",
            Tags = ["gaming", "dvr", "recording", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore", @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1),
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "AppCaptureEnabled", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-game-bar-tips",
            Label = "Disable Game Bar Tips",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Game Bar tips and startup panel notifications. Also disables the Nexus overlay for Game Bar. Default: Enabled. Recommended: Disabled.",
            Tags = ["gaming", "game-bar", "tips", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "game-optimize-gpu-scheduling",
            Label = "Optimize GPU Scheduling",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables hardware-accelerated GPU scheduling (HwSchMode=2). Reduces latency by letting the GPU manage its own scheduling. Default: 1 (off). Recommended: 2 for modern GPUs.",
            Tags = ["gaming", "gpu", "scheduling", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "game-disable-auto-hdr",
            Label = "Disable Auto HDR",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows Auto HDR for games. Prevents automatic tone-mapping that can cause washed-out colors in SDR titles. Default: Enabled. Recommended: Disabled if HDR causes issues.",
            Tags = ["gaming", "hdr", "auto-hdr", "display", "colors"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 0)],
        },
        new TweakDef
        {
            Id = "game-enable-timer-resolution",
            Label = "Enable Global Timer Resolution Requests",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables global timer resolution requests for lower input latency. Allows applications to request higher timer precision (0.5 ms). Default: 0 (disabled). Recommended: 1 for competitive gaming.",
            Tags = ["gaming", "timer", "resolution", "latency", "precision"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 0),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1)],
        },
        new TweakDef
        {
            Id = "game-disable-dvr-policy",
            Label = "Disable Game DVR (Policy)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Game DVR and Game Bar via HKLM group policy. Prevents background game recording system-wide. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["gaming", "dvr", "game-bar", "recording", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-dvr-configstore",
            Label = "Disable Game DVR (ConfigStore)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Game DVR via the user-level GameConfigStore. Complements the policy-level DVR disable. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["gaming", "dvr", "configstore", "recording", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-game-bar-presence-writer",
            Label = "Disable Game Bar Presence Writer",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the Game Bar presence writer process that tracks currently running games. Saves background CPU. Default: enabled.",
            Tags = ["gaming", "game-bar", "presence", "background"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
        },
        new TweakDef
        {
            Id = "game-disable-mouse-acceleration",
            Label = "Disable Mouse Acceleration for Gaming",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows mouse acceleration (enhance pointer precision). Provides 1:1 mouse movement for FPS games. Default: enabled.",
            Tags = ["gaming", "mouse", "acceleration", "precision"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "game-disable-auto-gamemode",
            Label = "Disable Auto Game Mode Detection",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows automatic Game Mode detection. Prevents Windows from changing priorities unexpectedly. Default: enabled.",
            Tags = ["gaming", "game-mode", "auto-detect", "priority"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-variable-refresh-rate",
            Label = "Disable Variable Refresh Rate (VRR)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Windows variable refresh rate for windowed games. Useful if VRR causes flickering. Default: enabled.",
            Tags = ["gaming", "vrr", "refresh-rate", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-game-input-redirection",
            Label = "Disable Game Input Redirection",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Game Input Redirection service that intercepts gamepad input. Reduces input latency. Default: enabled.",
            Tags = ["gaming", "input", "redirection", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-diagtrack-keyword",
            Label = "Disable DiagTrack ETW Session",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the DiagTrack ETW auto-logger that collects telemetry in the background. Frees CPU cycles for gaming. Default: enabled.",
            Tags = ["gaming", "telemetry", "diagtrack", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-fullscreen-optimizations",
            Label = "Disable Fullscreen Optimizations",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables DWM fullscreen optimizations globally. Forces true exclusive fullscreen for better frame pacing. Default: enabled.",
            Tags = ["gaming", "fullscreen", "dwm", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehavior", 2)],
        },
        new TweakDef
        {
            Id = "game-disable-gamedvr",
            Label = "Disable Game DVR (User)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables Game DVR at the user level. Prevents background game recording and screenshot capture. Default: enabled.",
            Tags = ["gaming", "dvr", "recording", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "game-force-exclusive-fullscreen",
            Label = "Force Exclusive Fullscreen Mode",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces applications to use exclusive fullscreen mode instead of borderless windowed. Reduces input latency. Default: automatic.",
            Tags = ["gaming", "fullscreen", "exclusive", "latency"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "game-gaming-mode-priority",
            Label = "Set Gaming Task CPU Priority",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Games multimedia task to CPU priority level 6 (high) and background-only to false. Ensures games get maximum CPU scheduling. Default: priority 2.",
            Tags = ["gaming", "priority", "cpu", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Priority", 6),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Background Only", "False"),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Priority", 2),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Background Only", "True"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Priority", 6)],
        },
        new TweakDef
        {
            Id = "game-honor-fse-compat",
            Label = "Honor Fullscreen Exclusive Compatibility",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables honoring of per-app fullscreen exclusive compatibility settings. Ensures apps that request FSE get it. Default: off.",
            Tags = ["gaming", "fullscreen", "exclusive", "compatibility"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_HonorUserFSEBehaviorMode", 1)],
        },
        new TweakDef
        {
            Id = "game-irq8-realtime",
            Label = "Set IRQ8 to High Priority",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets IRQ8 (real-time clock) to high priority. Improves timer precision for gaming. Default: not set.",
            Tags = ["gaming", "irq", "timer", "realtime", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IRQ8Priority", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IRQ8Priority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "IRQ8Priority", 1)],
        },
        new TweakDef
        {
            Id = "game-network-throttling-off",
            Label = "Disable Multimedia Network Throttling",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the multimedia network throttling that limits non-multimedia traffic during audio/video playback. Prevents bandwidth caps during gaming. Default: 10.",
            Tags = ["gaming", "network", "throttling", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", 10)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
        },
        new TweakDef
        {
            Id = "game-priority-high",
            Label = "Set Foreground Priority Boost (High)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets Win32PrioritySeparation to 38 (short, variable, high foreground boost). Prioritises foreground applications for gaming. Default: 2.",
            Tags = ["gaming", "priority", "foreground", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\PriorityControl", "Win32PrioritySeparation", 38)],
        },
        new TweakDef
        {
            Id = "game-set-system-responsiveness",
            Label = "Set System Responsiveness to 0%",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets SystemResponsiveness to 0, reserving zero percent of CPU for background tasks. Maximises CPU availability for foreground games. Default: 20.",
            Tags = ["gaming", "responsiveness", "cpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "SystemResponsiveness", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "SystemResponsiveness", 20)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "SystemResponsiveness", 0)],
        },
        new TweakDef
        {
            Id = "game-system-profile-games",
            Label = "Optimise GPU Priority for Games",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the Games multimedia system profile to GPU priority 8 (high) and scheduling category High. Ensures games receive priority GPU time. Default: GPU priority 8, scheduling Normal.",
            Tags = ["gaming", "gpu", "priority", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "GPU Priority", 8),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Scheduling Category", "High"),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "GPU Priority", 8),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Scheduling Category", "Medium"),
            ],
            DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games", "Scheduling Category", "High")],
        },
    ];
}
