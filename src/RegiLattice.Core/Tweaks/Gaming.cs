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
            Description =
                "Disables Xbox Auth Manager and Xbox Game Save services. Frees resources if you don't use Xbox Live features. Options: 3=Manual, 4=Disabled. Default: Manual. Recommended: Disabled.",
            Tags = ["gaming", "xbox", "services", "performance"],
            RegistryKeys =
            [
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager",
                @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave",
            ],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 3)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\xbgm", "Start", 3)],
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
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters", "TCPNoDelay", 1)],
        },
        new TweakDef
        {
            Id = "game-gaming-disable-dvr-background",
            Label = "Disable Game DVR Background Recording",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Game DVR background recording. Frees GPU encoder and disk I/O resources. Default: Enabled. Recommended: Disabled for maximum FPS.",
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
            Description =
                "Disables Game Bar tips and startup panel notifications. Also disables the Nexus overlay for Game Bar. Default: Enabled. Recommended: Disabled.",
            Tags = ["gaming", "game-bar", "tips", "notifications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "game-optimize-gpu-scheduling",
            Label = "Optimize GPU Scheduling",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables hardware-accelerated GPU scheduling (HwSchMode=2). Reduces latency by letting the GPU manage its own scheduling. Default: 1 (off). Recommended: 2 for modern GPUs.",
            Tags = ["gaming", "gpu", "scheduling", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers", "HwSchMode", 2)],
        },
        new TweakDef
        {
            Id = "game-disable-auto-hdr",
            Label = "Disable Auto HDR",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Windows Auto HDR for games. Prevents automatic tone-mapping that can cause washed-out colors in SDR titles. Default: Enabled. Recommended: Disabled if HDR causes issues.",
            Tags = ["gaming", "hdr", "auto-hdr", "display", "colors"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AutoHDREnable", 0)],
        },
        new TweakDef
        {
            Id = "game-enable-timer-resolution",
            Label = "Enable Global Timer Resolution Requests",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enables global timer resolution requests for lower input latency. Allows applications to request higher timer precision (0.5 ms). Default: 0 (disabled). Recommended: 1 for competitive gaming.",
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
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "GlobalTimerResolutionRequests", 1),
            ],
        },
        new TweakDef
        {
            Id = "game-disable-dvr-policy",
            Label = "Disable Game DVR (Policy)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Game DVR and Game Bar via HKLM group policy. Prevents background game recording system-wide. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["gaming", "dvr", "game-bar", "recording", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-dvr-configstore",
            Label = "Disable Game DVR (ConfigStore)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables Game DVR via the user-level GameConfigStore. Complements the policy-level DVR disable. Default: Enabled. Recommended: Disabled for performance.",
            Tags = ["gaming", "dvr", "configstore", "recording", "user"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_Enabled", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-game-bar-presence-writer",
            Label = "Disable Game Bar Presence Writer",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the Game Bar presence writer process that tracks currently running games. Saves background CPU. Default: enabled.",
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
            Description =
                "Disables Windows mouse acceleration (enhance pointer precision). Provides 1:1 mouse movement for FPS games. Default: enabled.",
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
            Description =
                "Disables the DiagTrack ETW auto-logger that collects telemetry in the background. Frees CPU cycles for gaming. Default: enabled.",
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
            Description =
                "Disables DWM fullscreen optimizations globally. Forces true exclusive fullscreen for better frame pacing. Default: enabled.",
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
            Description =
                "Forces applications to use exclusive fullscreen mode instead of borderless windowed. Reduces input latency. Default: automatic.",
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
            Description =
                "Sets the Games multimedia task to CPU priority level 6 (high) and background-only to false. Ensures games get maximum CPU scheduling. Default: priority 2.",
            Tags = ["gaming", "priority", "cpu", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    "False"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    2
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    "True"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-honor-fse-compat",
            Label = "Honor Fullscreen Exclusive Compatibility",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables honoring of per-app fullscreen exclusive compatibility settings. Ensures apps that request FSE get it. Default: off.",
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
            Description =
                "Disables the multimedia network throttling that limits non-multimedia traffic during audio/video playback. Prevents bandwidth caps during gaming. Default: 10.",
            Tags = ["gaming", "network", "throttling", "bandwidth"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    10
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-priority-high",
            Label = "Set Foreground Priority Boost (High)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Win32PrioritySeparation to 38 (short, variable, high foreground boost). Prioritises foreground applications for gaming. Default: 2.",
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
            Description =
                "Sets SystemResponsiveness to 0, reserving zero percent of CPU for background tasks. Maximises CPU availability for foreground games. Default: 20.",
            Tags = ["gaming", "responsiveness", "cpu", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    20
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-system-profile-games",
            Label = "Optimise GPU Priority for Games",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Games multimedia system profile to GPU priority 8 (high) and scheduling category High. Ensures games receive priority GPU time. Default: GPU priority 8, scheduling Normal.",
            Tags = ["gaming", "gpu", "priority", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Scheduling Category",
                    "Medium"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Scheduling Category",
                    "High"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-sfio-priority-high",
            Label = "Set Game SFIO Priority to High",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Raises the Synchronous File I/O priority for the Games multimedia profile. Reduces stuttering caused by disk I/O during gaming.",
            Tags = ["gaming", "sfio", "io", "performance", "multimedia-profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority",
                    "Normal"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority",
                    "High"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-disable-ndu-service",
            Label = "Disable Network Data Usage Monitor (NDU)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Network Data Usage (NDU) monitoring service, which can cause latency spikes in network-intensive games.",
            Tags = ["gaming", "network", "ndu", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-set-system-responsiveness-zero",
            Label = "Maximise CPU Time for Games (SystemResponsiveness=0)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets SystemResponsiveness to 0 in the multimedia system profile, giving games up to 100% of CPU scheduling time instead of the default 20%.",
            Tags = ["gaming", "cpu", "performance", "multimedia-profile"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    20
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-network-throttling-off",
            Label = "Disable Network Throttling Index for Games",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NetworkThrottlingIndex to max (0xFFFFFFFF) removing Windows network throttling during multimedia/gaming to improve throughput.",
            Tags = ["gaming", "network", "throttling", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    10
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex",
                    unchecked((int)0xFFFFFFFF)
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-gpu-priority-8",
            Label = "Set High GPU Priority for Games",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Raises GPU scheduling priority to 8 for the Games multimedia profile, giving game rendering tasks preferential GPU access.",
            Tags = ["gaming", "gpu", "priority", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    2
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "GPU Priority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-latency-sensitivity-high",
            Label = "Set High Latency Sensitivity for Games",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Marks the Games multimedia task as Latency Sensitive=High, allowing Windows scheduler to handle it with lowest possible scheduling latency.",
            Tags = ["gaming", "latency", "scheduler", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitivity",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitivity",
                    "Medium"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitivity",
                    "High"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-background-only-false",
            Label = "Prevent Games from Running as Background Only",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets Background Only=False for the Games multimedia profile task, ensuring games receive full foreground priority scheduling.",
            Tags = ["gaming", "background", "priority", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    "False"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    "True"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Background Only",
                    "False"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-priority-6",
            Label = "Set Game Task CPU Priority to 6",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Raises the CPU priority weight for the Games multimedia profile task from 2 to 6, giving game threads more scheduling time.",
            Tags = ["gaming", "cpu", "priority", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    2
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-disable-xbox-accessory-svc",
            Label = "Disable Xbox Accessory Management Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables the Xbox Accessory Management service (XboxGipSvc). Saves resources when no Xbox controllers are used.",
            Tags = ["gaming", "xbox", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-increase-max-user-port",
            Label = "Increase Max UDP/TCP Port Range for Gaming",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Increases the ephemeral TCP/UDP port ceiling to 65534, allowing game servers to open more simultaneous connections without port exhaustion.",
            Tags = ["gaming", "network", "ports", "tcp", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Tcpip\Parameters", "MaxUserPort", 65534)],
        },
        new TweakDef
        {
            Id = "game-disable-msmq-service",
            Label = "Disable MSMQ Service for Gaming",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Message Queuing (MSMQ) service to manual start. This service is not needed for most games and can be disabled to free resources.",
            Tags = ["gaming", "msmq", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSMQ\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MSMQ", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MSMQ", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\MSMQ", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-gameinput-service",
            Label = "Disable GameInput Service (Non-Xbox Controllers)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Microsoft GameInput service when Xbox controllers or GameInput-compatible devices are not in use. Frees background threads.",
            Tags = ["gaming", "gameinput", "controller", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\GameInput", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-set-dxgi-flip-model",
            Label = "Prefer Flip Presentation Model for DirectX",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Configures per-user DirectX GPU preferences to prefer the Flip sequential swap chain presentation model, enabling tearing and reducing input latency.",
            Tags = ["gaming", "directx", "flip", "tearing", "latency", "dxgi"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=0;"
                ),
            ],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences", "DirectXUserGlobalSettings")],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_CURRENT_USER\Software\Microsoft\DirectX\UserGpuPreferences",
                    "DirectXUserGlobalSettings",
                    "SwapEffectUpgradeEnable=0;"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-enable-game-bar-perf-counter",
            Label = "Enable Game Bar Performance Counter Overlay",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables the performance overlay showing FPS, CPU, and GPU usage within the Xbox Game Bar. Requires Game Bar to be enabled.",
            Tags = ["gaming", "game-bar", "overlay", "fps", "counter"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-diagtrack-autologger",
            Label = "Disable DiagTrack Autologger During Gaming",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the DiagTrack autologger session, which periodically collects telemetry data and causes micro-stutters during gaming.",
            Tags = ["gaming", "diagtrack", "telemetry", "stutter", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WMI\Autologger\DiagTrack", "Start", 0)],
        },
        new TweakDef
        {
            Id = "game-set-xgip-service-manual",
            Label = "Set Xbox Accessories Service to Manual Start",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Xbox Accessories Manager service (XboxGipSvc) to manual start if you don't use Xbox controllers/accessories, freeing startup resources.",
            Tags = ["gaming", "xbox", "accessories", "service", "startup"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XboxGipSvc", "Start", 3)],
        },
        new TweakDef
        {
            Id = "game-disable-ndu-adapter",
            Label = "Disable Network Data Usage Monitor for Gaming",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Network Data Usage (Ndu) monitoring driver that polls network adapters every second and can cause micro-stutters in CPU-bound games.",
            Tags = ["gaming", "ndu", "network", "cpu", "stutter"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Ndu", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-set-games-sfio-priority-high",
            Label = "Set Games SFIO Priority to High",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the scheduled I/O (SFIO) priority of the Games multimedia task to High, reducing storage access latency for game asset streaming.",
            Tags = ["gaming", "sfio", "io", "priority", "storage"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "SFIO Priority",
                    "High"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-mouse-fix-off",
            Label = "Disable Mouse Pointer Precision Enhancement",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the 'Enhance Pointer Precision' mouse acceleration feature. Raw mouse input without acceleration is preferred by most FPS/competitive gamers for consistent aim.",
            Tags = ["gaming", "mouse", "acceleration", "pointer", "precision", "fps"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Mouse"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "1")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Mouse", "MouseSpeed", "0")],
        },
        new TweakDef
        {
            Id = "game-set-games-affinity-all-cpus",
            Label = "Set Game Scheduler to Use All CPUs",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Ensures the Games multimedia task profile does not restrict thread affinity, allowing games to use all available CPU cores and logical processors.",
            Tags = ["gaming", "cpu", "affinity", "cores", "scheduling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Affinity",
                    "0"
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Clock Rate",
                    "10000"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Affinity"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Clock Rate"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Affinity",
                    "0"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-dvr-allow-capture-off",
            Label = "Disable DVR App Capture AllowAppCapture",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets AllowAppCapture=0 in GameConfigStore to disable background app DVR capture. Reduces background overhead during gaming sessions. Default: enabled.",
            Tags = ["gaming", "dvr", "capture", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowAppCapture", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowAppCapture")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowAppCapture", 0)],
        },
        new TweakDef
        {
            Id = "game-bar-gamepad-hotkey-off",
            Label = "Disable Xbox Game Bar Gamepad Hotkey",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents the Xbox gamepad button from opening Game Bar overlay. Avoids accidental overlay popups during gameplay. Default: enabled.",
            Tags = ["gaming", "gamebar", "gamepad", "hotkey"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowOpenByGamepad", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowOpenByGamepad")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "AllowOpenByGamepad", 0)],
        },
        new TweakDef
        {
            Id = "game-mode-auto-enable",
            Label = "Enable Auto Game Mode Activation",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Forces AutoGameModeEnabled=1 so Windows 11 automatically activates Game Mode when a full-screen game is detected. Default: enabled on Win11.",
            Tags = ["gaming", "game-mode", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\GameBar"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\GameBar", "AutoGameModeEnabled", 1)],
        },
        new TweakDef
        {
            Id = "game-input-hooks-fast",
            Label = "Reduce LowLevel Input Hook Timeout",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets LowLevelHooksTimeout=50ms so unresponsive keyboard/mouse hooks are bypassed faster. Reduces input latency in games. Default: 5000ms.",
            Tags = ["gaming", "input", "latency", "hooks"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LowLevelHooksTimeout", "50")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LowLevelHooksTimeout")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "LowLevelHooksTimeout", "50")],
        },
        new TweakDef
        {
            Id = "game-hpet-disable-reg",
            Label = "Disable HPET via Registry Kernel Flag",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets DisableHighPrecisionEventTimer=1 in the kernel config to disable HPET. Can improve frame consistency on some hardware by forcing use of TSC/LAPIC timers. Default: HPET enabled.",
            Tags = ["gaming", "hpet", "timer", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableHighPrecisionEventTimer", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableHighPrecisionEventTimer"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\kernel", "DisableHighPrecisionEventTimer", 1),
            ],
        },
        new TweakDef
        {
            Id = "game-no-lazy-mode-timeout",
            Label = "Disable Multimedia SystemProfile Lazy Mode Timeout",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets NoLazyModeTimeout=1 in the Multimedia SystemProfile to prevent the scheduler from entering lazy mode between frames. Reduces micro-stutters. Default: off.",
            Tags = ["gaming", "multimedia", "scheduler", "stutter"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyModeTimeout", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyModeTimeout"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyModeTimeout", 1),
            ],
        },
        new TweakDef
        {
            Id = "game-psched-svc-disable",
            Label = "Disable Packet Scheduler (Psched) Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Psched (QoS Packet Scheduler) service to Disabled. Removes QoS scheduling overhead on gaming machines that don't need traffic shaping. Default: Manual.",
            Tags = ["gaming", "network", "qos", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Psched"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Psched", "Start", 4)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Psched", "Start")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Psched", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-fse-behavior-optimize",
            Label = "Set FSE Behavior Mode for Fullscreen Optimization",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets GameDVR_FSEBehaviorMode=2 which forces exclusive fullscreen mode for games running through GameConfigStore. Reduces DWM overhead. Default: 0.",
            Tags = ["gaming", "fullscreen", "fse", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\System\GameConfigStore"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\System\GameConfigStore", "GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "game-audio-latency-1ms",
            Label = "Set Multimedia Audio Latency to 1ms",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets AudioLatencyMs=1 in Multimedia SystemProfile. Requests minimum audio buffer latency from the audio engine. May cause crackling on weak CPUs. Default: 10ms.",
            Tags = ["gaming", "audio", "latency", "multimedia"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "AudioLatencyMs", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "AudioLatencyMs"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "AudioLatencyMs", 1),
            ],
        },
        new TweakDef
        {
            Id = "game-profile-tasks-bg-off",
            Label = "Set Games Profile BackgroundOnly to False",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets BackgroundOnly=False in the Multimedia SystemProfile Tasks\\Games key. Ensures games are not treated as background-only processes by MMCSS. Default: False.",
            Tags = ["gaming", "mmcss", "background", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "BackgroundOnly",
                    "False"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "BackgroundOnly"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "BackgroundOnly",
                    "False"
                ),
            ],
        },
    ];
}
