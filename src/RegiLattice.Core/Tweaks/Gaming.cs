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
        new TweakDef
        {
            Id = "game-disable-xbox-auth-manager",
            Label = "Disable Xbox Live Authentication Manager Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Xbox Live Authentication Manager (XblAuthManager) service. Reduces Xbox-related background overhead on PCs that do not use Xbox Live. Default: manual start.",
            Tags = ["game", "xbox", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblAuthManager", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-xbox-live-game-save",
            Label = "Disable Xbox Live Game Save Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Xbox Live Game Save (XblGameSave) service which syncs game saves to the Xbox cloud. Removes background upload overhead for non-Xbox users. Default: manual start.",
            Tags = ["game", "xbox", "cloud", "save", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\XblGameSave", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-disable-game-dvr-shadow",
            Label = "Disable Game DVR Shadow Recording",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables the GameDVR shadow recording (background continuous capture) which records the last N seconds of gameplay. Removes persistent capture overhead. Default: enabled when Game Bar is active.",
            Tags = ["game", "dvr", "shadow", "record", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "ShadowRecord", 0)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "ShadowRecord", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR", "ShadowRecord", 0)],
        },
        new TweakDef
        {
            Id = "game-disable-background-app-access",
            Label = "Disable Global Background App Access",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = false,
            Description =
                "Globally disables background app access for all UWP applications. Prevents background apps from consuming CPU/RAM/network while gaming. Default: individual app settings.",
            Tags = ["game", "background", "uwp", "performance"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 1),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications", "GlobalUserDisabled", 0),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications",
                    "GlobalUserDisabled",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-disable-bcast-dvr-svc",
            Label = "Disable Broadcast DVR User Service",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the BcastDVRUserService which handles Game Bar broadcasting/recording per-session. Eliminates session-level DVR overhead when Game Bar broadcasting is not used. Default: automatic.",
            Tags = ["game", "bcast", "dvr", "service", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BcastDVRUserService"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BcastDVRUserService", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BcastDVRUserService", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\BcastDVRUserService", "Start", 4)],
        },
        new TweakDef
        {
            Id = "game-set-mmcss-scheduling-high",
            Label = "Set MMCSS Games Scheduling Category to High",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the MMCSS (Multimedia Class Scheduler) Games task scheduling category to High. Ensures game threads receive the highest available CPU scheduling class. Default: Medium.",
            Tags = ["game", "mmcss", "scheduling", "cpu", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
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
            Id = "game-set-mmcss-latency-sensitive",
            Label = "Set MMCSS Games Task as Latency Sensitive",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Marks the MMCSS Games multimedia task as Latency Sensitive. This hint causes the scheduler to reduce interrupt coalescing and batch delays for game threads. Default: False.",
            Tags = ["game", "mmcss", "latency", "scheduler"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitive",
                    "True"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitive",
                    "False"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Latency Sensitive",
                    "True"
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-set-mmcss-clock-rate",
            Label = "Set MMCSS Games Clock Rate to 5000 (0.5ms)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the MMCSS Games task minimum scheduling clock rate to 5000 (in 100-ns units = 0.5 ms). Reduces the minimum scheduler quantum for game threads to sub-millisecond intervals. Default: 10000 (1ms).",
            Tags = ["game", "mmcss", "clock", "latency", "scheduler"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Clock Rate",
                    5000
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Clock Rate",
                    10000
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games",
                    "Clock Rate",
                    5000
                ),
            ],
        },
        new TweakDef
        {
            Id = "game-disable-uwp-bg-access",
            Label = "Block All UWP Apps from Running in Background (GPO)",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Enforces that all UWP apps are denied background execution via Group Policy (LetAppsRunInBackground=2). Frees CPU/RAM/network for game workloads by preventing any UWP app from running while not in focus. Default: user-controlled.",
            Tags = ["game", "uwp", "background", "gpo", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppPrivacy", "LetAppsRunInBackground", 2)],
        },
    ];
}

// ── Merged from XboxGameBar.cs ──────────────────────────────────────────────────

internal static class XboxGameBar
{
    private const string GameBar = @"HKEY_CURRENT_USER\Software\Microsoft\GameBar";
    private const string GameCfg = @"HKEY_CURRENT_USER\System\GameConfigStore";
    private const string AppCapturePol = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\GameDVR";
    private const string XNetProv = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PolicyManager\default\Gaming\AllowGameDVR";
    private const string XSvc = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services";
    private const string GamebarPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
    private const string CapturesPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppX";
    private const string XboxGlobal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\AppCompatFlags\Layers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "xbgb-disable-captures-folder-indexing",
            Label = "Disable Game Bar Captures Folder Indexing",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "performance"],
            Description = "Prevents Windows Search from indexing the Game Bar captures folder, " + "reducing background I/O during gaming sessions.",
            ApplyOps = [RegOp.SetDword(GameBar, "GamebarCaptureFolderIndexingEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GamebarCaptureFolderIndexingEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "GamebarCaptureFolderIndexingEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-controller-activation",
            Label = "Disable Game Bar Activation via Controller",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "controller"],
            Description =
                "Prevents pressing the Xbox button on an XInput controller from opening "
                + "the Game Bar overlay, avoiding accidental pop-ups mid-game.",
            ApplyOps = [RegOp.SetDword(GameBar, "ShowStartupPanel", 0), RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "ShowStartupPanel"), RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 1)],
            DetectOps = [RegOp.CheckDword(GameBar, "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-capture-audio",
            Label = "Disable Game Bar Audio Capture",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "audio", "performance"],
            Description = "Disables audio capture in Game Bar recordings to reduce CPU and I/O " + "overhead when recording gameplay.",
            ApplyOps = [RegOp.SetDword(GameBar, "AudioCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "AudioCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "AudioCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-achievement-notifications",
            Label = "Disable Xbox Achievement Notifications",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "notifications"],
            Description = "Suppresses Xbox achievement toast notifications which can break " + "full-screen exclusive games and cause stutter.",
            ApplyOps = [RegOp.SetDword(GameBar, "NotificationsEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "NotificationsEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "NotificationsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-game-bar-tips",
            Label = "Disable Game Bar First-Run Tips",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "notifications", "tips"],
            Description = "Hides the first-run tutorial tips pane inside the Game Bar overlay " + "to keep the overlay uncluttered.",
            ApplyOps = [RegOp.SetDword(GameBar, "GameBarTipsEnabled", 0), RegOp.SetDword(GameBar, "FirstTimeExperienceCompleted", 1)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GameBarTipsEnabled"), RegOp.DeleteValue(GameBar, "FirstTimeExperienceCompleted")],
            DetectOps = [RegOp.CheckDword(GameBar, "GameBarTipsEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-game-dvr-policy",
            Label = "Disable Game DVR via Group Policy",
            Category = "Gaming",
            NeedsAdmin = true,
            CorpSafe = false,
            Tags = ["gaming", "xbox", "game-dvr", "policy"],
            Description =
                "Applies the Group Policy that disables Game DVR recording at the "
                + "system level, complementing the per-user GameConfigStore setting.",
            ApplyOps = [RegOp.SetDword(GamebarPol, "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(GamebarPol, "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(GamebarPol, "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-full-scene-optimizations",
            Label = "Disable Full-Scene Optimizations Globally",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "performance", "fps"],
            Description = "Sets the global Full-Scene Optimization flag off, which can improve " + "frame-time consistency in some DX11/DX12 titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameConfigStoreEnable", 1), RegOp.SetDword(GameCfg, "Win32GameDVR_FSEBehaviorMode", 2)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "Win32GameDVR_FSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(GameCfg, "Win32GameDVR_FSEBehaviorMode", 2)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-social-features",
            Label = "Disable Xbox Social Integration",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "privacy", "social"],
            Description = "Disables the Xbox social panel and friends list integration in the " + "Game Bar, removing online presence tracking.",
            ApplyOps = [RegOp.SetDword(GameBar, "SocialEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "SocialEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "SocialEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-premium-badges",
            Label = "Disable Game Bar Premium Feature Badges",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar"],
            Description = "Removes the premium/sponsor badge promotions displayed inside the " + "Game Bar overlay.",
            ApplyOps = [RegOp.SetDword(GameBar, "GameBarPremiumFeaturesEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "GameBarPremiumFeaturesEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "GameBarPremiumFeaturesEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-enable-exclusive-fullscreen",
            Label = "Force Exclusive Fullscreen (Disable Optimised FSE Override)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "performance", "fps", "fullscreen"],
            Description =
                "Restores true exclusive fullscreen for games that have been "
                + "quietly switched to FSO (Fullscreen Optimisation) mode by Windows, "
                + "which can lower latency in some GPU + monitor combinations.",
            ApplyOps = [RegOp.SetDword(GameCfg, "Win32GameDVR_EFSEBehavior", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "Win32GameDVR_EFSEBehavior")],
            DetectOps = [RegOp.CheckDword(GameCfg, "Win32GameDVR_EFSEBehavior", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-app-capture",
            Label = "Disable Game DVR App Capture",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "privacy", "performance"],
            Description =
                "Disables the background application capture that the Game DVR driver uses "
                + "to record gameplay footage on demand. Reduces CPU/GPU overhead even when "
                + "Game Bar itself is disabled at the policy level.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "AppCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "AppCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "AppCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-cursor-in-capture",
            Label = "Exclude Cursor from Game Captures",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "cursor"],
            Description =
                "Prevents the mouse cursor from being included in Game DVR recordings " + "and screenshots, producing cleaner gameplay footage.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "CursorCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "CursorCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "CursorCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-mic-in-capture",
            Label = "Disable Microphone in Game DVR Captures",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "microphone", "audio", "privacy"],
            Description =
                "Mutes microphone input from Game DVR recordings so voice chat or " + "background noise is not embedded into saved clips by default.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "MicrophoneCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "MicrophoneCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "MicrophoneCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-audio-capture",
            Label = "Disable Audio Capture in Game DVR Recordings",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "capture", "audio"],
            Description =
                "Turns off background audio capture in Game DVR recordings, including "
                + "system audio. Useful when you record silently or handle audio in post.",
            ApplyOps = [RegOp.SetDword(AppCapturePol, "AudioCaptureEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(AppCapturePol, "AudioCaptureEnabled")],
            DetectOps = [RegOp.CheckDword(AppCapturePol, "AudioCaptureEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-fse-hook",
            Label = "Disable Game DVR Fullscreen Exclusivity Hook",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fullscreen", "fse", "performance"],
            Description =
                "Prevents Game DVR from hooking into the fullscreen exclusive "
                + "(FSE) path used by legacy DirectX games, eliminating a source "
                + "of frame-rate stutters on older titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_HonorUserFSEBehaviorMode", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_HonorUserFSEBehaviorMode")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_HonorUserFSEBehaviorMode", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-dxgi-fse-compat",
            Label = "Disable Game DVR DXGI Fullscreen Compatibility Mode",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fullscreen", "dxgi", "performance"],
            Description =
                "Stops Game DVR from forcing DXGI-based games into its FSE "
                + "compatibility wrapper, which can cap frame rates and introduce "
                + "latency on DX11/DX12 titles.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_DXGIHonorFSEWindowsCompatible", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-startup-panel",
            Label = "Disable Game Bar Startup Tip Panel",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "startup"],
            Description =
                "Hides the introductory tip panel that appears the first time " + "Game Bar is opened after an OS update or new installation.",
            ApplyOps = [RegOp.SetDword(GameBar, "ShowStartupPanel", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "ShowStartupPanel")],
            DetectOps = [RegOp.CheckDword(GameBar, "ShowStartupPanel", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-nexus-bar",
            Label = "Disable Game Bar Nexus Pop-up Panel",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game bar", "nexus", "ui"],
            Description =
                "Disables the Nexus (circular game-bar widget) pop-up panel that "
                + "appears at the edge of the screen when a game is detected, "
                + "removing a persistent UI intrusion during gameplay.",
            ApplyOps = [RegOp.SetDword(GameBar, "UseNexusForGameBarEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "UseNexusForGameBarEnabled")],
            DetectOps = [RegOp.CheckDword(GameBar, "UseNexusForGameBarEnabled", 0)],
        },
        new TweakDef
        {
            Id = "xbgb-enable-game-mode-all-games",
            Label = "Allow Game Mode for All Games (Enable AllowAutoGameMode)",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "game mode", "performance"],
            Description =
                "Sets the Game Bar flag that allows Windows Game Mode to "
                + "auto-activate for any process that registers as a game, "
                + "giving it CPU/GPU scheduling priority.",
            ApplyOps = [RegOp.SetDword(GameBar, "AllowAutoGameMode", 1)],
            RemoveOps = [RegOp.DeleteValue(GameBar, "AllowAutoGameMode")],
            DetectOps = [RegOp.CheckDword(GameBar, "AllowAutoGameMode", 1)],
        },
        new TweakDef
        {
            Id = "xbgb-disable-efs-feature-hooks",
            Label = "Disable Game DVR Extended FSE Feature Flags",
            Category = "Gaming",
            NeedsAdmin = false,
            CorpSafe = true,
            Tags = ["gaming", "xbox", "fse", "fullscreen", "performance"],
            Description =
                "Zeroes the extended fullscreen-exclusive feature flags "
                + "(EFSEFeatureFlags) used by Game DVR hooks, preventing "
                + "unwanted interference with exclusive-fullscreen rendering paths.",
            ApplyOps = [RegOp.SetDword(GameCfg, "GameDVR_EFSEFeatureFlags", 0)],
            RemoveOps = [RegOp.DeleteValue(GameCfg, "GameDVR_EFSEFeatureFlags")],
            DetectOps = [RegOp.CheckDword(GameCfg, "GameDVR_EFSEFeatureFlags", 0)],
        },
    ];
}

// ── merged from PolicyGaming.cs ──
// RegiLattice.Core — Tweaks/PolicyGaming.cs
// Xbox Game Bar, Game DVR, game explorer, cloud gaming, and gaming performance policies
// Category: "Gaming Policy"
// Consolidated from 8 modules.

internal static class PolicyGaming
{
    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            .. _GameBarOverlayPolicy.Data,
            .. _GameBarPolicy.Data,
            .. _GameDvrPolicy.Data,
            .. _GameExplorerPolicy.Data,
            .. _GameStreamingPolicy.Data,
            .. _GamingPerformancePolicy.Data,
            .. _XboxCloudGamingPolicy.Data,
            .. _XboxNetworkingPolicy.Data,
        ];

    // ── GameBarOverlayPolicy ──
    private static class _GameBarOverlayPolicy
    {
        private const string GbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\GameBar";
        private const string GmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameMode";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gamebarpol-disable-game-bar",
                Label = "Game Bar Policy: Disable Game Bar Overlay",
                Category = "Gaming",
                Description =
                    "Disables the Game Bar overlay (Win+G shortcut) via Group Policy. Game Bar can cause focus issues in enterprise applications when activated unexpectedly by keyboard shortcuts.",
                Tags = ["gamebar", "overlay", "disable", "gaming", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables Win+G overlay; prevents accidental activation in enterprise full-screen apps.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "AllowGameBar", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "AllowGameBar")],
                DetectOps = [RegOp.CheckDword(GbKey, "AllowGameBar", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-auto-game-mode",
                Label = "Game Bar Policy: Disable Automatic Game Mode",
                Category = "Gaming",
                Description =
                    "Prevents Windows from automatically switching to Game Mode for detected full-screen applications. Game Mode reprioritizes GPU and CPU resources, which can cause performance degradation in non-game enterprise workloads.",
                Tags = ["gamebar", "game-mode", "auto", "cpu", "gpu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents Game Mode from reprioritizing CPU/GPU for detected full-screen processes.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "AllowAutoGameMode", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "AllowAutoGameMode")],
                DetectOps = [RegOp.CheckDword(GbKey, "AllowAutoGameMode", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-nexus",
                Label = "Game Bar Policy: Disable Nova Game Bar Experience (Nexus)",
                Category = "Gaming",
                Description =
                    "Disables the Nexus (Nova) Game Bar experience via GPO. The Nexus interface provides an enhanced overlay for gaming stats, Xbox integration, and achievement notifications which are unnecessary on managed enterprise devices.",
                Tags = ["gamebar", "nexus", "nova", "xbox", "overlay", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables the enhanced Xbox overlay; unnecessary on managed enterprise endpoints.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "UseNexusForGameBarEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "UseNexusForGameBarEnabled")],
                DetectOps = [RegOp.CheckDword(GbKey, "UseNexusForGameBarEnabled", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-presence-writer",
                Label = "Game Bar Policy: Disable Game Bar Presence Writer",
                Category = "Gaming",
                Description =
                    "Disables the GameBarPresenceWriter process which monitors running games and reports activity to Xbox services. Reduces background telemetry and removes the Xbox social presence feature on managed devices.",
                Tags = ["gamebar", "presence", "telemetry", "xbox", "background", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes GameBarPresenceWriter background process and associated Xbox telemetry.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "EnableGameBarPresenceWriter", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "EnableGameBarPresenceWriter")],
                DetectOps = [RegOp.CheckDword(GbKey, "EnableGameBarPresenceWriter", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-broadcast",
                Label = "Game Bar Policy: Disable Game Broadcasting",
                Category = "Gaming",
                Description =
                    "Disables the Game Bar broadcasting feature (Mixer/Beam/Xbox Live streaming) via Group Policy. Prevents users from live-streaming their screen via the overlay, eliminating a potential data leakage channel.",
                Tags = ["gamebar", "broadcast", "streaming", "xbox", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Prevents screen streaming via Game Bar; eliminates a potential data leakage channel.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "AllowGameBarBroadcast", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "AllowGameBarBroadcast")],
                DetectOps = [RegOp.CheckDword(GbKey, "AllowGameBarBroadcast", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-startup-panel",
                Label = "Game Bar Policy: Hide Game Bar Startup Panel",
                Category = "Gaming",
                Description =
                    "Prevents the Game Bar startup tips panel from appearing when a game or full-screen application is launched. Eliminates the 'Did you know the GameBar exists?' tip that appears on first game launch.",
                Tags = ["gamebar", "startup", "tips", "panel", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses Game Bar tip banners on first full-screen application launch.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "ShowStartupPanel", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "ShowStartupPanel")],
                DetectOps = [RegOp.CheckDword(GbKey, "ShowStartupPanel", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-game-mode-global",
                Label = "Game Bar Policy: Disable Game Mode System-Wide",
                Category = "Gaming",
                Description =
                    "Disables Game Mode globally via the GameMode policy key. Game Mode allocates additional GPU and CPU resources to games, which can reduce determinism and throughput for productivity and CAD workloads on high-end machines.",
                Tags = ["gamebar", "game-mode", "global", "cpu", "gpu", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Globally disables GPU/CPU resource reallocation for gaming workloads.",
                RegistryKeys = [GmKey],
                ApplyOps = [RegOp.SetDword(GmKey, "GameModeEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(GmKey, "GameModeEnabled")],
                DetectOps = [RegOp.CheckDword(GmKey, "GameModeEnabled", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-clip-cursor",
                Label = "Game Bar Policy: Disable Cursor Clip in Game Bar",
                Category = "Gaming",
                Description =
                    "Prevents the Game Bar from clipping the cursor to the game window while the overlay is active. This avoids mouse pointer getting trapped inside a full-screen application when toggling the overlay.",
                Tags = ["gamebar", "cursor", "clip", "overlay", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents mouse pointer getting trapped inside full-screen apps during overlay toggle.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "AllowGameClipCursor", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "AllowGameClipCursor")],
                DetectOps = [RegOp.CheckDword(GbKey, "AllowGameClipCursor", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-achievements-overlay",
                Label = "Game Bar Policy: Disable Achievement Notifications Overlay",
                Category = "Gaming",
                Description =
                    "Disables the in-game achievement / award notification overlay that appears via Xbox Game Bar when unlocking Steam, Epic Games, or Xbox achievements. Prevents distraction during enterprise workloads running in fullscreen mode.",
                Tags = ["gamebar", "achievements", "notifications", "xbox", "overlay", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes Xbox achievement pop-up overlays in full-screen enterprise applications.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "ShowAchievements", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "ShowAchievements")],
                DetectOps = [RegOp.CheckDword(GbKey, "ShowAchievements", 0)],
            },
            new TweakDef
            {
                Id = "gamebarpol-disable-xbox-integration",
                Label = "Game Bar Policy: Disable Xbox Network Integration in Game Bar",
                Category = "Gaming",
                Description =
                    "Prevents Game Bar from connecting to Xbox Network services (friend status, party chat, activity feed). Disables the social layer that reports gaming activity to Microsoft and Xbox friends.",
                Tags = ["gamebar", "xbox", "network", "social", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents Game Bar from connecting to Xbox Network; disables social activity reporting.",
                RegistryKeys = [GbKey],
                ApplyOps = [RegOp.SetDword(GbKey, "AllowXboxNetworkIntegration", 0)],
                RemoveOps = [RegOp.DeleteValue(GbKey, "AllowXboxNetworkIntegration")],
                DetectOps = [RegOp.CheckDword(GbKey, "AllowXboxNetworkIntegration", 0)],
            },
        ];
    }

    // ── GameBarPolicy ──
    private static class _GameBarPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
        private const string GbKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameBar";
        private const string GmKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameMode";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gamebar-disable-gamedvr",
                    Label = "Disable Game DVR Background Recording",
                    Category = "Gaming",
                    Description =
                        "Disables the Game DVR background recording feature that continuously records game footage to disk, freeing GPU encoder time and eliminating the performance overhead of continuous H.264/H.265 encoding in the background.",
                    Tags = ["gamedvr", "recording", "background", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Game DVR background recording disabled; GPU encoder freed, disk writes stopped, game perf overhead removed.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowGameDVR", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowGameDVR")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowGameDVR", 0)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-gamebar-tips",
                    Label = "Disable Game Bar First-Run Tips and Overlay Prompts",
                    Category = "Gaming",
                    Description =
                        "Prevents the Game Bar from displaying first-run tips and overlay prompt notifications in full-screen applications, eliminating interruptions during gaming or full-screen media playback.",
                    Tags = ["gamebar", "tips", "overlay", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Game Bar tips and overlay prompts disabled; no interruptions during full-screen app or game sessions.",
                    ApplyOps = [RegOp.SetDword(GbKey, "DisableGameBarTips", 1)],
                    RemoveOps = [RegOp.DeleteValue(GbKey, "DisableGameBarTips")],
                    DetectOps = [RegOp.CheckDword(GbKey, "DisableGameBarTips", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-gamemode",
                    Label = "Disable Windows Game Mode Globally",
                    Category = "Gaming",
                    Description =
                        "Disables Windows Game Mode which dynamically adjusts CPU/GPU scheduling when a game is in focus. On systems running background services or VMs, Game Mode can disrupt non-game workloads; disabling it provides more predictable scheduling.",
                    Tags = ["gamemode", "scheduler", "cpu", "gpu", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Game Mode disabled; CPU/GPU scheduler not dynamically adjusted when game is focused. More predictable perf.",
                    ApplyOps = [RegOp.SetDword(GmKey, "AutoGameModeEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(GmKey, "AutoGameModeEnabled")],
                    DetectOps = [RegOp.CheckDword(GmKey, "AutoGameModeEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-xbox-network-check",
                    Label = "Disable Xbox Network Connectivity Check at Game Launch",
                    Category = "Gaming",
                    Description =
                        "Prevents the Game Bar from performing Xbox Live / Microsoft network connectivity checks at game launch, eliminating network latency at game start and avoiding telemetry associated with Xbox network status probes.",
                    Tags = ["gamebar", "xbox", "network-check", "telemetry", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox Live network check at game launch disabled; no network probe or telemetry on game start.",
                    ApplyOps = [RegOp.SetDword(GbKey, "DisableXboxNetworkCheck", 1)],
                    RemoveOps = [RegOp.DeleteValue(GbKey, "DisableXboxNetworkCheck")],
                    DetectOps = [RegOp.CheckDword(GbKey, "DisableXboxNetworkCheck", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-broadcast-streaming",
                    Label = "Disable Game Bar Broadcast Streaming to Twitch/Mixer",
                    Category = "Gaming",
                    Description =
                        "Prevents the Game Bar from offering game broadcast streaming functionality to third-party streaming services, disabling the broadcast API and ensuring game streams cannot be initiated without explicit user action outside the Game Bar.",
                    Tags = ["gamebar", "broadcast", "streaming", "twitch", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Game Bar broadcast streaming disabled; game sessions cannot be streamed via Game Bar broadcast UI.",
                    ApplyOps = [RegOp.SetDword(GbKey, "DisableBroadcasting", 1)],
                    RemoveOps = [RegOp.DeleteValue(GbKey, "DisableBroadcasting")],
                    DetectOps = [RegOp.CheckDword(GbKey, "DisableBroadcasting", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-screenshot-shortcut",
                    Label = "Disable Game Bar Screenshot Keyboard Shortcut",
                    Category = "Gaming",
                    Description =
                        "Disables the Win+Alt+PrtSc keyboard shortcut that captures game screenshots via Game Bar, preventing accidental screenshot capture and avoiding screenshots being stored in the GameCapture screenshots folder.",
                    Tags = ["gamebar", "screenshot", "keyboard-shortcut", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Game Bar screenshot hotkey disabled; Win+Alt+PrtSc no longer captures game screenshots.",
                    ApplyOps = [RegOp.SetDword(GbKey, "DisableScreenshotShortcut", 1)],
                    RemoveOps = [RegOp.DeleteValue(GbKey, "DisableScreenshotShortcut")],
                    DetectOps = [RegOp.CheckDword(GbKey, "DisableScreenshotShortcut", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-gameclips-upload",
                    Label = "Disable Automatic Game Clip Upload to Xbox Cloud",
                    Category = "Gaming",
                    Description =
                        "Prevents automatically uploading captured game clips and screenshots to Xbox cloud storage, ensuring game captures remain local and are not synchronized to Microsoft cloud accounts without explicit user action.",
                    Tags = ["gamebar", "clips", "cloud-upload", "xbox", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Game clip auto-upload to Xbox cloud disabled; captures stored locally only, not synced to Microsoft cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCloudUpload", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudUpload")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCloudUpload", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-disable-gamebar-telemetry",
                    Label = "Disable Game Bar and GameDVR Telemetry to Microsoft",
                    Category = "Gaming",
                    Description =
                        "Prevents Game Bar and GameDVR from sending gaming session duration, game title names, capture statistics, and hardware performance data to Microsoft.",
                    Tags = ["gamebar", "gamedvr", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Game Bar and GameDVR telemetry to Microsoft disabled; game session and capture data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableGameBarTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBarTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableGameBarTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "gamebar-set-capture-folder-policy",
                    Label = "Set Game Capture Storage Folder via Policy",
                    Category = "Gaming",
                    Description =
                        "Configures the Game Bar capture storage location to the local Videos\\GameCaptures path via policy, overriding per-user settings to ensure game recordings are stored to a known, auditable location and not redirected elsewhere.",
                    Tags = ["gamebar", "capture-folder", "storage", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Game capture folder fixed to Videos\\GameCaptures via policy; per-user folder change overridden.",
                    ApplyOps = [RegOp.SetString(Key, "CaptureFolder", @"%USERPROFILE%\Videos\GameCaptures")],
                    RemoveOps = [RegOp.DeleteValue(Key, "CaptureFolder")],
                    DetectOps = [RegOp.CheckString(Key, "CaptureFolder", @"%USERPROFILE%\Videos\GameCaptures")],
                },
                new TweakDef
                {
                    Id = "gamebar-log-capture-events",
                    Label = "Log Game Bar Capture Start/Stop Events",
                    Category = "Gaming",
                    Description =
                        "Enables event log entries when Game Bar starts or stops a recording or screenshot capture session, providing visibility into screen capture activity for compliance auditing.",
                    Tags = ["gamebar", "event-log", "capture", "audit", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Game Bar capture start/stop events logged; recording sessions visible in System event log for auditing.",
                    ApplyOps = [RegOp.SetDword(Key, "LogCaptureEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LogCaptureEvents")],
                    DetectOps = [RegOp.CheckDword(Key, "LogCaptureEvents", 1)],
                },
            ];
    }

    // ── GameDvrPolicy ──
    private static class _GameDvrPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gamedvr-disable-all",
                Label = "Game DVR Policy: Disable Game DVR Recording",
                Category = "Gaming",
                Description =
                    "Disables Windows Game DVR (Game Digital Video Recording) via Group Policy. "
                    + "Game DVR continuously captures gameplay footage in the background, consuming CPU, GPU, RAM, and disk resources even when the user is not actively recording. "
                    + "On non-gaming workstations this is pure overhead with no benefit. "
                    + "Removing this policy re-enables Game DVR recording capability.",
                Tags = ["gamedvr", "recording", "game-bar", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowGameDVR", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowGameDVR")],
                DetectOps = [RegOp.CheckDword(Key, "AllowGameDVR", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Disables Game DVR background recording; recovers continuous background resource overhead.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-game-bar",
                Label = "Game DVR Policy: Disable Xbox Game Bar",
                Category = "Gaming",
                Description =
                    "Disables the Xbox Game Bar overlay via Group Policy, preventing it from launching via Win+G or game launch hooks. "
                    + "Game Bar is a WinRT overlay that injects into render pipelines and can cause micro-stutters and frame drops in some titles. "
                    + "On enterprise workstations its recording and social features are inappropriate and add unnecessary attack surface. "
                    + "Removing this policy re-enables the Game Bar overlay.",
                Tags = ["gamedvr", "game-bar", "overlay", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameBar", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBar")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameBar", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removes Win+G Game Bar; eliminates overlay injection that can cause frame pacing issues.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-background-recording",
                Label = "Game DVR Policy: Disable Background Video Recording",
                Category = "Gaming",
                Description =
                    "Disables Game DVR's background clip recording feature that captures the last N minutes of gameplay continuously. "
                    + "This feature allocates a rolling video buffer on disk and in RAM at all times, degrading performance of high-load applications. "
                    + "Disabling it via policy prevents users or MSI installers from re-enabling it. "
                    + "Removing this policy allows background recording to be re-enabled.",
                Tags = ["gamedvr", "background-recording", "background", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBackgroundRecording", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBackgroundRecording")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBackgroundRecording", 0)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Removes rolling background clip buffer; reclaims GPU encoder cycles and disk I/O bandwidth.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-broadcast",
                Label = "Game DVR Policy: Disable Game Broadcasting",
                Category = "Gaming",
                Description =
                    "Prohibits live broadcasting of gameplay via services such as Mixer (retired) or any future broadcasting back-end. "
                    + "Broadcasting continually encodes and streams video, consuming significant bandwidth and GPU encoder capacity. "
                    + "On enterprise networks, live broadcasting is a data exfiltration risk and a bandwidth hog. "
                    + "Removing this policy re-enables broadcasting capability.",
                Tags = ["gamedvr", "broadcast", "streaming", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowBroadcast", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "AllowBroadcast", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks live game broadcasting; removes bandwidth and GPU encoder overhead.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-game-mode",
                Label = "Game DVR Policy: Disable Automatic Game Mode",
                Category = "Gaming",
                Description =
                    "Prevents Windows from automatically activating Game Mode when a game is detected. "
                    + "Game Mode reprioritises threads and may cause stutter in other processes sharing the CPU, including audio and network services. "
                    + "On workstations running mixed workloads, fixed CPU scheduling policy is more predictable than automatic game detection. "
                    + "Removing this policy allows Windows to auto-enable Game Mode for detected games.",
                Tags = ["gamedvr", "game-mode", "scheduling", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowAutoGameMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowAutoGameMode")],
                DetectOps = [RegOp.CheckDword(Key, "AllowAutoGameMode", 0)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents auto Game Mode; avoids CPU scheduling disruption on mixed-workload machines.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-feedback-button",
                Label = "Game DVR Policy: Disable Game Bar Feedback Button",
                Category = "Gaming",
                Description =
                    "Removes the feedback button from the Game Bar overlay that prompts users to submit feedback about gaming performance to Microsoft. "
                    + "On managed systems, direct feedback telemetry paths should be controlled centrally rather than through individual user submissions. "
                    + "This also removes one entry point for initiating corporate network connections from game sessions. "
                    + "Removing this policy restores the feedback button in the Game Bar.",
                Tags = ["gamedvr", "feedback", "telemetry", "game-bar", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameBarFeedbackButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBarFeedbackButton")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameBarFeedbackButton", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Removes feedback button from Game Bar; blocks one Microsoft telemetry submission path.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-coach-tips",
                Label = "Game DVR Policy: Disable Game Bar Coach Tips",
                Category = "Gaming",
                Description =
                    "Suppresses the 'coach' overlay tips that appear over games when Game Bar is active, prompting users to use recording and features. "
                    + "Coach tips are intrusive and distract from productive workflows when applications are mistakenly classified as games. "
                    + "Removing this policy restores the Game Bar coach tip overlays.",
                Tags = ["gamedvr", "coach", "tips", "ux", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameBarCoach", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameBarCoach")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameBarCoach", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides Game Bar coach tips; no more intrusive recording prompts over full-screen apps.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-game-overlay",
                Label = "Game DVR Policy: Disable Game Bar On-Screen Overlay",
                Category = "Gaming",
                Description =
                    "Disables the Game Bar on-screen performance and stats overlay that can appear in full-screen applications. "
                    + "The overlay periodically injects into the application's render surface and can introduce frame time spikes. "
                    + "On non-gaming workstations the overlay provides no value and adds rendering overhead. "
                    + "Removing this policy re-enables the Game Bar overlay capability.",
                Tags = ["gamedvr", "overlay", "rendering", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameOverlay", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameOverlay")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameOverlay", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes GPU overlay rendering; eliminates potential frame-time spikes from overlay injection.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-handheld",
                Label = "Game DVR Policy: Disable Handheld Console DVR Support",
                Category = "Gaming",
                Description =
                    "Disables Game DVR recording support for Windows handheld gaming devices via the AllowHandheld policy flag. "
                    + "On desktop and laptop enterprise machines this capability is irrelevant and its associated services consume memory at startup. "
                    + "Disabling via policy prevents peripheral detection code from loading the DVR subsystem on non-handheld hardware. "
                    + "Removing this policy re-enables handheld DVR support.",
                Tags = ["gamedvr", "handheld", "gaming-device", "service", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowHandheld", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowHandheld")],
                DetectOps = [RegOp.CheckDword(Key, "AllowHandheld", 0)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks handheld DVR subsystem loading on desktop/laptop hardware; minor memory saving.",
            },
            new TweakDef
            {
                Id = "gamedvr-disable-social-features",
                Label = "Game DVR Policy: Disable Game Social Features",
                Category = "Gaming",
                Description =
                    "Disables Xbox social features integration in the Game Bar that show friends, activity feeds, and achievements. "
                    + "Social features require persistent background network connections to Xbox Live services, adding ongoing network traffic and CPU wake events. "
                    + "On enterprise systems, Xbox Live social connectivity is inappropriate and a data governance concern. "
                    + "Removing this policy re-enables Xbox social integration in the Game Bar.",
                Tags = ["gamedvr", "social", "xbox-live", "enterprise", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowSocialFeatures", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSocialFeatures")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSocialFeatures", 0)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disconnects Xbox Live social in Game Bar; removes background network wake events.",
            },
        ];
    }

    // ── GameExplorerPolicy ──
    private static class _GameExplorerPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameExplorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "gex-disable-all-games",
                Label = "Disable Game Explorer for All Users",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableAll=1 in the GameExplorer policy key. Prevents users from "
                    + "launching, installing, or accessing Game Explorer entirely. Removes the "
                    + "Games folder from Windows Explorer and Start Menu. Default: Game Explorer "
                    + "is enabled. Recommended: 1 on managed corporate or kiosk machines "
                    + "where gaming is prohibited by policy.",
                Tags = ["games", "game-explorer", "group-policy", "lockdown"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAll", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAll")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAll", 1)],
            },
            new TweakDef
            {
                Id = "gex-block-game-downloads",
                Label = "Block Downloading Game Ratings",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets AllowDownloadingRatings=0 in the GameExplorer policy key. Prevents "
                    + "Windows from downloading updated game ratings (ESRB/PEGI data) over the "
                    + "internet. Eliminates background network traffic generated by the Games "
                    + "folder ratings update service. Default: ratings downloaded automatically. "
                    + "Recommended: 0 to suppress telemetry-adjacent outbound connections.",
                Tags = ["games", "network", "ratings", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowDownloadingRatings", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowDownloadingRatings")],
                DetectOps = [RegOp.CheckDword(Key, "AllowDownloadingRatings", 0)],
            },
            new TweakDef
            {
                Id = "gex-hide-recommended-games",
                Label = "Hide Recommended Games in Game Explorer",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets ListRecommendedGames=0 in the GameExplorer policy key. Removes the "
                    + "'Recommended Games' section from the Games folder that normally shows "
                    + "games available to purchase or download from Microsoft. Eliminates "
                    + "advertising content from the Windows shell. Default: recommended games "
                    + "are shown. Recommended: 0 to suppress advertising.",
                Tags = ["games", "ads", "privacy", "game-explorer", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ListRecommendedGames", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ListRecommendedGames")],
                DetectOps = [RegOp.CheckDword(Key, "ListRecommendedGames", 0)],
            },
            new TweakDef
            {
                Id = "gex-block-game-launching",
                Label = "Prevent Launching Games from Game Explorer",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets AllowLaunchingGames=0 in the GameExplorer policy key. Prevents users "
                    + "from launching games directly through the Games Explorer interface. Games "
                    + "remain installed but cannot be started via the Games folder. Default: "
                    + "launching games is permitted. Recommended: 0 on education or kiosk "
                    + "environments where gameplay must be blocked during work hours.",
                Tags = ["games", "lockdown", "group-policy", "game-explorer"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowLaunchingGames", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowLaunchingGames")],
                DetectOps = [RegOp.CheckDword(Key, "AllowLaunchingGames", 0)],
            },
            new TweakDef
            {
                Id = "gex-disable-online-games",
                Label = "Hide Online Games Section",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets ShowOnlineGames=0 in the GameExplorer policy key. Removes the Online "
                    + "Games section that links to Windows Live Games and Microsoft game services "
                    + "from within Game Explorer. Reduces outbound traffic and eliminates "
                    + "Microsoft storefront exposure in managed environments. Default: online "
                    + "games section is visible.",
                Tags = ["games", "online", "privacy", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ShowOnlineGames", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShowOnlineGames")],
                DetectOps = [RegOp.CheckDword(Key, "ShowOnlineGames", 0)],
            },
            new TweakDef
            {
                Id = "gex-disable-parental-controls",
                Label = "Disable Game Parental Controls",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Sets AllowParentalControls=0 in the GameExplorer policy key. Disables the "
                    + "Windows parental control enforcement layer that restricts game access "
                    + "based on ratings. On managed machines where all user accounts are "
                    + "supervised by IT, the parental controls subsystem can be disabled to "
                    + "reduce background service overhead. Default: parental controls available "
                    + "but not enforced unless configured.",
                Tags = ["games", "parental-controls", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowParentalControls", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowParentalControls")],
                DetectOps = [RegOp.CheckDword(Key, "AllowParentalControls", 0)],
            },
            new TweakDef
            {
                Id = "gex-block-game-updates",
                Label = "Block Automatic Game Updates",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets AllowGameUpdates=0 in the GameExplorer policy key. Prevents Game "
                    + "Explorer from automatically downloading and applying updates to games "
                    + "managed through the Windows Games infrastructure. Gives IT control over "
                    + "when game updates are deployed. Default: auto updates are permitted. "
                    + "Recommended: 0 on machines where bandwidth and installed software "
                    + "must be tightly managed.",
                Tags = ["games", "updates", "group-policy", "bandwidth"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowGameUpdates", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowGameUpdates")],
                DetectOps = [RegOp.CheckDword(Key, "AllowGameUpdates", 0)],
            },
            new TweakDef
            {
                Id = "gex-disable-game-notifications",
                Label = "Disable Game Explorer Notifications",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets AllowGameNotifications=0 in the GameExplorer policy key. Suppresses "
                    + "balloon tips and toast notifications generated by Game Explorer, such as "
                    + "news about newly available games or update alerts. Reduces notification "
                    + "noise on managed workstations. Default: game notifications are shown. "
                    + "Recommended: 0 for distraction-free work environments.",
                Tags = ["games", "notifications", "group-policy", "focus"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowGameNotifications", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowGameNotifications")],
                DetectOps = [RegOp.CheckDword(Key, "AllowGameNotifications", 0)],
            },
            new TweakDef
            {
                Id = "gex-block-game-install",
                Label = "Block Game Installation via Game Explorer",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets AllowInstallGames=0 in the GameExplorer policy key. Prevents users "
                    + "from installing new games through the Games folder or Game Explorer "
                    + "interface, including games obtained from Microsoft. Games already "
                    + "installed remain accessible unless further restricted. Default: "
                    + "game installation is allowed. Recommended: 0 on locked-down machines.",
                Tags = ["games", "installation", "lockdown", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowInstallGames", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowInstallGames")],
                DetectOps = [RegOp.CheckDword(Key, "AllowInstallGames", 0)],
            },
            new TweakDef
            {
                Id = "gex-disable-game-activity-log",
                Label = "Disable Game Activity Logging",
                Category = "Gaming",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets LogGameActivity=0 in the GameExplorer policy key. Stops Windows from "
                    + "logging game session activity (launch times, play duration, achievements) "
                    + "in the Games Explorer database. Reduces disk I/O and eliminates locally "
                    + "stored telemetry about gaming habits. Default: activity is logged. "
                    + "Recommended: 0 for privacy-focused deployments.",
                Tags = ["games", "privacy", "logging", "group-policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogGameActivity", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogGameActivity")],
                DetectOps = [RegOp.CheckDword(Key, "LogGameActivity", 0)],
            },
        ];
    }

    // ── GameStreamingPolicy ──
    private static class _GameStreamingPolicy
    {
        private const string DvrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR";
        private const string InputKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameInput";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gstream-block-xbox-remote-play",
                    Label = "Game Stream: Block Xbox Remote Play From This Device",
                    Category = "Gaming",
                    Description =
                        "Sets AllowXboxRemotePlay=0 in GameDVR policy. Prevents users from streaming their Xbox console games to this Windows PC via the Xbox Remote Play feature. "
                        + "Xbox Remote Play creates a persistent streaming session between the PC and an Xbox console over the internet. On enterprise devices, this introduces an unmanaged high-bandwidth consumer service into the corporate network and may allow the Xbox console (outside the corporate network perimeter) to stream audio/video captured by the device's microphone and camera back to it.",
                    Tags = ["gaming", "streaming", "remote-play", "xbox", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Xbox Remote Play streaming sessions; prevents unsanctioned remote device connectivity.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowXboxRemotePlay", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowXboxRemotePlay")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowXboxRemotePlay", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-block-game-streaming-from-pc",
                    Label = "Game Stream: Block Streaming PC Games to Other Devices",
                    Category = "Gaming",
                    Description =
                        "Sets AllowGameStreamingFromPC=0 in GameDVR policy. Prevents games running on this PC from being streamed to another device (e.g., to a browser, mobile device, or other PC) via Xbox-based or Miracast streaming. "
                        + "PC game streaming renders GPU frames and encodes video of potentially sensitive content visible on the desktop, which is then transmitted across the network. Blocking streaming prevents screen content from being sent to uncontrolled endpoint devices.",
                    Tags = ["gaming", "streaming", "screen-capture", "pc", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks PC game streaming; desktop video not transmitted to other devices via Xbox streaming.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowGameStreamingFromPC", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowGameStreamingFromPC")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowGameStreamingFromPC", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-block-game-streaming-upload",
                    Label = "Game Stream: Block Streaming Video Upload to Xbox Network",
                    Category = "Gaming",
                    Description =
                        "Sets AllowGameStreamingUpload=0 in GameDVR policy. Prevents captured game footage and streaming session recordings from being uploaded to Xbox Live's video hosting service. "
                        + "Captured game clips that are uploaded to Xbox Live become publicly accessible (or accessible to the user's Xbox friends list). On managed devices, preventing video content capture and upload stops potential accidental disclosure of sensitive information visible on the screen (corporate applications, documents, chat windows) that appear in the game capture frame.",
                    Tags = ["gaming", "streaming", "upload", "video", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks game video upload to Xbox Live; screen content not sent to external cloud.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowGameStreamingUpload", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowGameStreamingUpload")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowGameStreamingUpload", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-block-game-input-service-telemetry",
                    Label = "Game Stream: Block GameInput Service Telemetry Upload",
                    Category = "Gaming",
                    Description =
                        "Sets DisableTelemetry=1 in GameInput machine policy. Prevents the GameInput API service from sending telemetry about controller, keyboard, and mouse input events to Microsoft. "
                        + "The GameInput service collects keystroke timing, button press frequency, and peripheral usage patterns from connected game input devices. On corporate workstations, this input telemetry includes potentially sensitive information about productivity application usage patterns that happen to be captured via the same keyboard or mouse.",
                    Tags = ["gaming", "input", "telemetry", "keyboard", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks GameInput API telemetry; controller/keyboard usage data not sent to Microsoft.",
                    ApplyOps = [RegOp.SetDword(InputKey, "DisableTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(InputKey, "DisableTelemetry")],
                    DetectOps = [RegOp.CheckDword(InputKey, "DisableTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "gstream-block-game-input-cloud-config",
                    Label = "Game Stream: Block GameInput Cloud Configuration Sync",
                    Category = "Gaming",
                    Description =
                        "Sets DisableCloudConfig=1 in GameInput machine policy. Prevents the GameInput service from downloading controller mapping configurations, button remapping profiles, and vibration settings from Microsoft's cloud game-input configuration service. "
                        + "Cloud config sync for GameInput connects to an external endpoint at startup to check for updated controller profiles. This is an outbound network call to a Microsoft-controlled cloud endpoint that occurs automatically. On locked-down or air-gapped environments, any automatic cloud-sync mechanism should be disabled.",
                    Tags = ["gaming", "input", "cloud-sync", "config", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks GameInput cloud config sync; controller profiles not auto-updated from Microsoft cloud.",
                    ApplyOps = [RegOp.SetDword(InputKey, "DisableCloudConfig", 1)],
                    RemoveOps = [RegOp.DeleteValue(InputKey, "DisableCloudConfig")],
                    DetectOps = [RegOp.CheckDword(InputKey, "DisableCloudConfig", 1)],
                },
                new TweakDef
                {
                    Id = "gstream-disable-streaming-microphone-access",
                    Label = "Game Stream: Disable Microphone Access for Game Streaming",
                    Category = "Gaming",
                    Description =
                        "Sets AllowMicrophoneAccess=0 in GameDVR policy. Prevents game streaming sessions and game capture from accessing the device's microphone. "
                        + "Game streaming with microphone access enables audio capture of the room environment, which is a significant privacy and security concern on corporate devices. Disabling mic access for game streaming ensures that voice communication via Xbox streaming channels cannot intercept ambient conversations regardless of whether the user intentionally activates party chat.",
                    Tags = ["gaming", "microphone", "privacy", "audio", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks microphone access for game streaming; ambient audio cannot be captured via Xbox streaming.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowMicrophoneAccess", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowMicrophoneAccess")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowMicrophoneAccess", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-disable-streaming-camera-access",
                    Label = "Game Stream: Disable Camera Access for Game Streaming",
                    Category = "Gaming",
                    Description =
                        "Sets AllowCameraAccess=0 in GameDVR policy. Prevents game streaming sessions from accessing the device's webcam or front-facing camera. "
                        + "Game streaming with camera access enables live video capture of the user's physical environment. This creates a serious privacy risk on enterprise devices, where the room visible to the camera may contain whiteboards with sensitive information, other screens, or colleagues who have not consented to being recorded.",
                    Tags = ["gaming", "camera", "webcam", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks webcam access for game streaming; no live video capture via Xbox streaming channels.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowCameraAccess", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowCameraAccess")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowCameraAccess", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-limit-streaming-resolution-1080p",
                    Label = "Game Stream: Limit Outbound Game Streaming Resolution to 1080p",
                    Category = "Gaming",
                    Description =
                        "Sets MaxStreamingResolution=2 in GameDVR policy. Caps outbound game streaming resolution at 1080p (1920×1080), preventing 4K or higher resolution game stream encoding. "
                        + "4K game streaming at 60 fps can consume 40–80 Mbps of bandwidth, which severely degrades other users on shared corporate network links. Capping at 1080p limits peak streaming bandwidth to ~15–20 Mbps, reducing network impact for the rare scenario where streaming is permitted within a managed environment's acceptable-use policy.",
                    Tags = ["gaming", "streaming", "resolution", "bandwidth", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "1080p streaming cap; reduces peak streaming bandwidth from ~80 Mbps (4K) to ~20 Mbps.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "MaxStreamingResolution", 2)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "MaxStreamingResolution")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "MaxStreamingResolution", 2)],
                },
                new TweakDef
                {
                    Id = "gstream-disable-stream-session-auto-reconnect",
                    Label = "Game Stream: Disable Automatic Reconnection for Streaming Sessions",
                    Category = "Gaming",
                    Description =
                        "Sets AllowAutoReconnect=0 in GameDVR policy. Prevents game streaming sessions from automatically and silently reconnecting after a network interruption. "
                        + "Auto-reconnect for streaming sessions can cause the device to establish a new streaming connection in the background (potentially after hours, if a session was left active) without the user's knowledge. This background reconnect may re-activate microphone and camera capture unexpectedly. Requiring manual reconnection ensures the user is aware of and actively initiating each streaming session.",
                    Tags = ["gaming", "streaming", "reconnect", "background", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Prevents background streaming reconnect; each session requires explicit user initiation.",
                    ApplyOps = [RegOp.SetDword(DvrKey, "AllowAutoReconnect", 0)],
                    RemoveOps = [RegOp.DeleteValue(DvrKey, "AllowAutoReconnect")],
                    DetectOps = [RegOp.CheckDword(DvrKey, "AllowAutoReconnect", 0)],
                },
                new TweakDef
                {
                    Id = "gstream-disable-game-input-accessibility-overlay",
                    Label = "Game Stream: Disable GameInput Accessibility Overlay in Streaming",
                    Category = "Gaming",
                    Description =
                        "Sets DisableAccessibilityOverlay=1 in GameInput machine policy. Prevents the GameInput accessibility overlay (on-screen virtual controller, input visualiser) from appearing during game streaming sessions. "
                        + "The GameInput accessibility overlay injects screen content via a system-level overlay that is captured by game recording and streaming functions. On streaming sessions, this overlay appears in the video feed received by all viewers. On corporate devices, ensuring that no system-level overlays inject unexpected UI into game-streamed frames is part of content control.",
                    Tags = ["gaming", "input", "overlay", "accessibility", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Hides GameInput accessibility overlay in streamed video; clean streaming output without injected overlays.",
                    ApplyOps = [RegOp.SetDword(InputKey, "DisableAccessibilityOverlay", 1)],
                    RemoveOps = [RegOp.DeleteValue(InputKey, "DisableAccessibilityOverlay")],
                    DetectOps = [RegOp.CheckDword(InputKey, "DisableAccessibilityOverlay", 1)],
                },
            ];
    }

    // ── GamingPerformancePolicy ──
    private static class _GamingPerformancePolicy
    {
        private const string GamesKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Games";
        private const string SysKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "gamperf-set-games-scheduling-gpu100",
                    Label = "Gaming Perf: Set Game Scheduler GPU Priority to Maximum (100%)",
                    Category = "Gaming",
                    Description =
                        "Sets GPU Priority=8 in Multimedia SystemProfile Games task. Configures the Windows Multimedia Class Scheduler Service (MMCSS) to assign the highest GPU execution priority to processes registered under the Games scheduling category. "
                        + "MMCSS Games profile governs time-critical GPU resource allocation for games and real-time rendering applications. Setting GPU Priority=8 (the maximum value) ensures game rendering passes are given priority access to the GPU command queue over background tasks such as desktop composition, codec decode, or system monitoring overlays.",
                    Tags = ["gaming", "gpu", "scheduler", "mmcss", "priority"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote = "Max GPU scheduling priority for games; other GPU workloads de-prioritised while game is running.",
                    ApplyOps = [RegOp.SetDword(GamesKey, "GPU Priority", 8)],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "GPU Priority")],
                    DetectOps = [RegOp.CheckDword(GamesKey, "GPU Priority", 8)],
                },
                new TweakDef
                {
                    Id = "gamperf-set-games-scheduling-priority-high",
                    Label = "Gaming Perf: Set Game Thread Priority to High",
                    Category = "Gaming",
                    Description =
                        "Sets Priority=6 in Multimedia SystemProfile Games task. Sets the Windows MMCSS thread priority for game processes to 6, which maps to the 'High' thread scheduling priority. "
                        + "MMCSS priority 6 (High) gives game threads elevated scheduling preference over normal threads (priority 2) and background threads (priority 1), without reaching real-time priority levels that could cause system instability. This ensures game simulation and rendering threads are not preempted by background telemetry and maintenance tasks during time-sensitive frame computation.",
                    Tags = ["gaming", "cpu", "thread-priority", "mmcss", "scheduler"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Game threads at High priority; reduced preemption from background tasks during frame computation.",
                    ApplyOps = [RegOp.SetDword(GamesKey, "Priority", 6)],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "Priority")],
                    DetectOps = [RegOp.CheckDword(GamesKey, "Priority", 6)],
                },
                new TweakDef
                {
                    Id = "gamperf-set-mmcss-scheduling-category-high",
                    Label = "Gaming Perf: Set MMCSS Games Scheduling Category to High",
                    Category = "Gaming",
                    Description =
                        "Sets Scheduling Category=High in Multimedia SystemProfile Games task. Configures the MMCSS scheduling category for the Games profile, determining how aggressively the scheduler boosts game thread quantum lengths. "
                        + "The High scheduling category allows game threads to receive longer CPU quantum slices per scheduling interval compared to Medium or Low, reducing the frequency of context switches during active rendering loops. Fewer context switches mean less OS scheduling overhead per frame and more deterministic frame timing.",
                    Tags = ["gaming", "cpu", "quantum", "mmcss", "frame-time"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "High scheduling category for game threads; longer CPU quanta reduce context-switch overhead per frame.",
                    ApplyOps = [RegOp.SetString(GamesKey, "Scheduling Category", "High")],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "Scheduling Category")],
                    DetectOps = [RegOp.CheckString(GamesKey, "Scheduling Category", "High")],
                },
                new TweakDef
                {
                    Id = "gamperf-set-mmcss-sfio-priority-high",
                    Label = "Gaming Perf: Set MMCSS SFIO Priority to High for Asset Streaming",
                    Category = "Gaming",
                    Description =
                        "Sets SFIO Priority=High in Multimedia SystemProfile Games task. Sets the Scheduled File I/O (SFIO) priority for game processes within MMCSS to High. "
                        + "Modern games heavily rely on background asset streaming — loading textures, audio, and map data from disk while the game is running. SFIO priority determines how quickly the OS services I/O requests from game processes relative to other I/O consumers. High SFIO priority ensures disk operations for game asset streaming are serviced before background indexer, antivirus scanner, or cloud sync I/O.",
                    Tags = ["gaming", "io", "sfio", "asset-streaming", "disk"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "High SFIO priority for game I/O; asset streaming served ahead of background disk consumers.",
                    ApplyOps = [RegOp.SetString(GamesKey, "SFIO Priority", "High")],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "SFIO Priority")],
                    DetectOps = [RegOp.CheckString(GamesKey, "SFIO Priority", "High")],
                },
                new TweakDef
                {
                    Id = "gamperf-set-games-affinity-all-cores",
                    Label = "Gaming Perf: Set MMCSS Games Affinity to All CPU Cores",
                    Category = "Gaming",
                    Description =
                        "Sets Affinity=0 in Multimedia SystemProfile Games task. Sets the CPU affinity mask for the MMCSS Games scheduling category to 0, which instructs MMCSS to allow game threads to run on all available CPU cores rather than a constrained subset. "
                        + "A value of 0 in the Affinity field means no affinity restriction — game threads can migrate to any core. This is optimal for modern games that use job-graph and worker-thread models to parallelise simulation, physics, and audio processing across all available cores. Pinning to a subset of cores (non-zero affinity) reduces parallelism and can cause load imbalance on multi-core CPUs.",
                    Tags = ["gaming", "cpu", "affinity", "cores", "parallel"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Unrestricted CPU core affinity for game threads; game parallelism spans all CCD/cores.",
                    ApplyOps = [RegOp.SetDword(GamesKey, "Affinity", 0)],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "Affinity")],
                    DetectOps = [RegOp.CheckDword(GamesKey, "Affinity", 0)],
                },
                new TweakDef
                {
                    Id = "gamperf-disable-background-only-affinity",
                    Label = "Gaming Perf: Disable Background-Only MMCSS Affinity Restriction",
                    Category = "Gaming",
                    Description =
                        "Sets Background Only=False in Multimedia SystemProfile Games task. Clears the background-only restriction flag for the MMCSS Games profile, ensuring game processes are not treated as background-priority workloads by the scheduler. "
                        + "When Background Only is True, MMCSS treats threads in that category as background work — deprioritising their scheduling and reducing their CPU time allocation when a foreground process is active. For games that must be in the foreground to run, setting this False is a no-op in practice, but explicitly clearing it in MMCSS configuration prevents any edge-case scenario where the game process is momentarily backgrounded (e.g., during Alt+Tab) from permanently degrading its scheduling.",
                    Tags = ["gaming", "cpu", "background", "mmcss", "foreground"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Game threads not restricted to background-only affinity; normal scheduling during brief window focus changes.",
                    ApplyOps = [RegOp.SetString(GamesKey, "Background Only", "False")],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "Background Only")],
                    DetectOps = [RegOp.CheckString(GamesKey, "Background Only", "False")],
                },
                new TweakDef
                {
                    Id = "gamperf-set-system-responsiveness-20pct",
                    Label = "Gaming Perf: Set SystemResponsiveness to Reserve 20% CPU for Background Tasks",
                    Category = "Gaming",
                    Description =
                        "Sets SystemResponsiveness=20 in Multimedia SystemProfile (parent key). Controls what percentage of CPU time MMCSS reserves for background tasks when a high-priority multimedia or gaming application is running. "
                        + "The default value is 20 (20% reserved for background). Setting this to 20 (or lower) maximises the CPU time available to the gaming/multimedia process. Values above 20 make the system more responsive to background tasks at the cost of game frame rates. Windows audio and game processes compete for the 80% non-reserved pool; a 20% reservation ensures the system remains stable (audio does not glitch) even under full game load.",
                    Tags = ["gaming", "cpu", "mmcss", "responsiveness", "background"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "20% CPU reserved for background tasks; 80% available to games — matches Windows MMCSS default but made explicit.",
                    ApplyOps = [RegOp.SetDword(SysKey, "SystemResponsiveness", 20)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "SystemResponsiveness")],
                    DetectOps = [RegOp.CheckDword(SysKey, "SystemResponsiveness", 20)],
                },
                new TweakDef
                {
                    Id = "gamperf-enable-network-throttling-index-bypass",
                    Label = "Gaming Perf: Disable MMCSS Network Throttling for Low-Latency Gaming",
                    Category = "Gaming",
                    Description =
                        "Sets NetworkThrottlingIndex=0xFFFFFFFF in Multimedia SystemProfile. Disables MMCSS network throttling for multimedia applications. "
                        + "By default, MMCSS throttles network activity for multimedia processes to prevent network I/O from interrupting the CPU scheduler's time allocations for audio/video threads. While beneficial for video playback, this throttling adds latency to outbound network packets from game processes. Setting NetworkThrottlingIndex to 0xFFFFFFFF disables the throttle, allowing game networking threads to send packets at their full rate without artificial scheduling delays.",
                    Tags = ["gaming", "network", "latency", "mmcss", "throttling"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote =
                        "Network throttling disabled for multimedia tasks; game packets sent without MMCSS-imposed delay. May affect audio in edge cases.",
                    ApplyOps = [RegOp.SetDword(SysKey, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "NetworkThrottlingIndex")],
                    DetectOps = [RegOp.CheckDword(SysKey, "NetworkThrottlingIndex", unchecked((int)0xFFFFFFFF))],
                },
                new TweakDef
                {
                    Id = "gamperf-set-games-clock-rate-10000hz",
                    Label = "Gaming Perf: Set MMCSS Games Clock Rate to 10,000 Hz (0.1 ms Precision)",
                    Category = "Gaming",
                    Description =
                        "Sets Clock Rate=10000 in Multimedia SystemProfile Games task. Sets the Windows multimedia timer resolution for game processes to 10,000 units (100 microseconds / 0.1 ms). "
                        + "The Clock Rate value in MMCSS controls the minimum timer resolution requested by game processes. A higher clock rate (lower number of 100ns units) results in more frequent clock interrupts, enabling finer-grained sleep/wait precision for game loops. "
                        + "10,000 (0.1ms) represents the finest practical clock granularity achievable on x86 hardware. This benefits games with precision sleep-based frame limiters and reduces the floor on observable frame timing jitter.",
                    Tags = ["gaming", "timer", "clock", "mmcss", "precision"],
                    NeedsAdmin = true,
                    CorpSafe = false,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "0.1ms timer clock for game tasks; tighter frame timing precision at cost of slightly higher interrupt rate.",
                    ApplyOps = [RegOp.SetDword(GamesKey, "Clock Rate", 10000)],
                    RemoveOps = [RegOp.DeleteValue(GamesKey, "Clock Rate")],
                    DetectOps = [RegOp.CheckDword(GamesKey, "Clock Rate", 10000)],
                },
                new TweakDef
                {
                    Id = "gamperf-enable-multimedia-gaming-class-scheduler",
                    Label = "Gaming Perf: Enable Multimedia Class Scheduler for Gaming Tasks",
                    Category = "Gaming",
                    Description =
                        "Sets Enabled=1 in Multimedia SystemProfile. Ensures the Windows Multimedia Class Scheduler Service (MMCSS) is active and applies scheduling boosts for registered audio, gaming, and multimedia tasks. "
                        + "MMCSS is the system service that applies all Games and Audio scheduling profiles described in the SystemProfile Tasks registry. If MMCSS is disabled or its settings are cleared during OS hardening, the GPU priority, CPU thread priority, and timer clock rate settings have no effect. Explicitly enabling MMCSS as a policy ensures all the other gaming performance settings in this module are active.",
                    Tags = ["gaming", "mmcss", "scheduler", "enable", "performance"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Ensures MMCSS is active; prerequisite for all other MMCSS gaming performance settings to take effect.",
                    ApplyOps = [RegOp.SetDword(SysKey, "Enabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(SysKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(SysKey, "Enabled", 1)],
                },
            ];
    }

    // ── XboxCloudGamingPolicy ──
    private static class _XboxCloudGamingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\XboxLive";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "xbcloud-disable-cloud-gaming-access",
                    Label = "Xbox Cloud: Block Xbox Cloud Gaming (xCloud) Access",
                    Category = "Gaming",
                    Description =
                        "Sets AllowCloudGaming=0 in XboxLive machine policy. Prevents users on this device from accessing Xbox Cloud Gaming (Project xCloud). "
                        + "Xbox Cloud Gaming streams game compute from Azure data-centres and requires persistent high-bandwidth internet. On corporate or managed devices, cloud gaming represents an unsanctioned cloud service, a bandwidth-exhaustion risk during business hours, and a potential data-exfiltration vector if game streaming sessions can capture screen content. "
                        + "Blocking cloud gaming ensures the device is used only for sanctioned workloads.",
                    Tags = ["xbox", "cloud-gaming", "xcloud", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks xCloud streaming; prevents bandwidth consumption and unsanctioned cloud service access on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCloudGaming", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudGaming")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCloudGaming", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-disable-xbox-cloud-save-sync",
                    Label = "Xbox Cloud: Disable Xbox Live Cloud Save Synchronisation",
                    Category = "Gaming",
                    Description =
                        "Sets AllowCloudSaveSync=0 in XboxLive machine policy. Prevents Xbox game save data from being synchronised to and from Xbox Live cloud storage. "
                        + "Cloud save sync transfers game state, progress, and personal gaming data to Microsoft's Xbox Live servers. In regulated industries or GDPR-compliant environments, this data transfer requires legal basis and may conflict with data residency requirements. "
                        + "Disabling cloud save sync keeps game data local and prevents any XboxLive-originated outbound data flow from the device.",
                    Tags = ["xbox", "cloud-save", "data-sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Xbox save data sync to cloud; game progress stays local only.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCloudSaveSync", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCloudSaveSync")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCloudSaveSync", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-block-xbox-multiplayer-social-features",
                    Label = "Xbox Cloud: Block Xbox Live Multiplayer and Social Features",
                    Category = "Gaming",
                    Description =
                        "Sets AllowXboxLiveMultiplayer=0 in XboxLive machine policy. Disables Xbox Live multiplayer matchmaking, party chat, and social gaming features. "
                        + "Xbox Live multiplayer requires open communication channels to Xbox Live services and to other players, which can be misused for social engineering, off-channel communication in corporate environments, or unmonitored voice chat. "
                        + "Blocking multiplayer social features ensures the device does not participate in any Xbox Live social graph activity.",
                    Tags = ["xbox", "multiplayer", "social", "enterprise", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Xbox multiplayer matchmaking and social features; games can still be played locally without Xbox Live.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowXboxLiveMultiplayer", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowXboxLiveMultiplayer")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowXboxLiveMultiplayer", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-block-xbox-in-game-purchases",
                    Label = "Xbox Cloud: Block Xbox Live In-Game Purchases",
                    Category = "Gaming",
                    Description =
                        "Sets AllowInGamePurchases=0 in XboxLive machine policy. Prevents users from making in-game or in-app purchases through the Xbox Live storefront. "
                        + "In-game purchases (microtransactions) using corporate-provisioned accounts or linked payment methods create financial exposure. Blocking this setting on managed devices prevents unauthorised financial transactions and ensures the device cannot be used as a purchase gateway for Xbox content.",
                    Tags = ["xbox", "purchases", "microtransactions", "financial", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Xbox in-game purchases; eliminates financial transaction risk on managed devices.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowInGamePurchases", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowInGamePurchases")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowInGamePurchases", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-block-xbox-achievement-sharing",
                    Label = "Xbox Cloud: Block Xbox Achievement and Activity Sharing",
                    Category = "Gaming",
                    Description =
                        "Sets AllowAchievementSharing=0 in XboxLive machine policy. Blocks Xbox achievement notifications and activity sharing from being posted to the Xbox Live social feed. "
                        + "Achievement sharing publishes play activity to the public Xbox social graph, which may disclose information about the user's presence, games played, and gaming schedule. On managed devices, any outbound social activity not authorised by IT policy should be suppressed.",
                    Tags = ["xbox", "achievements", "sharing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses achievement sharing; no social feed posts from this device.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowAchievementSharing", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowAchievementSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowAchievementSharing", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-restrict-xbox-content-rating-e",
                    Label = "Xbox Cloud: Restrict Xbox Content to Everyone (E) Rated Only",
                    Category = "Gaming",
                    Description =
                        "Sets ContentRatingMaxLevel=1 in XboxLive machine policy. Restricts the maximum content rating that can be played or purchased through Xbox to Everyone (E) — suitable for all ages. "
                        + "Content rating enforcement is critical on shared devices, lab workstations, kiosk PCs, or any device where unexpected mature content could appear on-screen in a professional or educational setting. An E-rating cap prevents the Xbox service from displaying or launching Mature/M-rated, Teen/T-rated, or Adults Only content.",
                    Tags = ["xbox", "content-rating", "parental-control", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Enforces E-rating ceiling for Xbox content; only Everyone-rated titles accessible.",
                    ApplyOps = [RegOp.SetDword(Key, "ContentRatingMaxLevel", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ContentRatingMaxLevel")],
                    DetectOps = [RegOp.CheckDword(Key, "ContentRatingMaxLevel", 1)],
                },
                new TweakDef
                {
                    Id = "xbcloud-disable-xbox-live-friend-requests",
                    Label = "Xbox Cloud: Disable Xbox Live Friend Requests",
                    Category = "Gaming",
                    Description =
                        "Sets AllowFriendRequests=0 in XboxLive machine policy. Prevents users from sending or receiving Xbox Live friend requests from this device. "
                        + "Friends on Xbox Live gain access to the user's gaming status, presence information, and can initiate voice/party invitations. On corporate or supervised devices, building a social gaming network using company credentials or contact details is an oversharing risk. Blocking friend requests prevents social graph expansion from managed endpoints.",
                    Tags = ["xbox", "friends", "social-graph", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Blocks friend requests; prevents Xbox social graph expansion from managed endpoints.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowFriendRequests", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowFriendRequests")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowFriendRequests", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-block-user-generated-content-access",
                    Label = "Xbox Cloud: Block User-Generated Content Access",
                    Category = "Gaming",
                    Description =
                        "Sets AllowUserGeneratedContent=0 in XboxLive machine policy. Prevents browsing and downloading of user-generated content (UGC) from Xbox Live — including user-created game maps, mods, character skins, and downloadable content created by the community. "
                        + "UGC from Xbox Live is unvetted third-party content that could contain inappropriate material, modified game executables, or content that violates organisational acceptable-use policies. Blocking UGC access reduces the attack surface for content-moderation bypass exploits in Xbox-connected games.",
                    Tags = ["xbox", "ugc", "user-content", "content-filter", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks Xbox UGC access; prevents community-created content downloads from unvetted sources.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowUserGeneratedContent", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowUserGeneratedContent")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowUserGeneratedContent", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-disable-xbox-live-voice-messaging",
                    Label = "Xbox Cloud: Disable Xbox Live Voice and Text Messaging",
                    Category = "Gaming",
                    Description =
                        "Sets AllowVoiceMessaging=0 in XboxLive machine policy. Disables the Xbox Live voice message and text message features that allow users to send audio clips and text messages to other Xbox Live users. "
                        + "Xbox Live messaging creates a communication channel that bypasses corporate email/instant-messaging policies and monitoring. On managed devices, any off-channel communication tool should be blocked to ensure all communications go through monitored, policy-compliant channels.",
                    Tags = ["xbox", "messaging", "voice", "communication", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Disables Xbox Live voice/text messaging; eliminates off-channel communication vector.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowVoiceMessaging", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowVoiceMessaging")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowVoiceMessaging", 0)],
                },
                new TweakDef
                {
                    Id = "xbcloud-block-xbox-cross-play-with-consoles",
                    Label = "Xbox Cloud: Block Xbox Cross-Play with Console Players",
                    Category = "Gaming",
                    Description =
                        "Sets AllowCrossPlay=0 in XboxLive machine policy. Prevents games running on this Windows PC from engaging in crossplay sessions with Xbox console players via Xbox Live. "
                        + "Cross-play requires the Windows machine to be reachable by console-based connection requests through Xbox Live's relay infrastructure. Blocking cross-play reduces the device's exposure to Xbox Live's multi-platform multiplayer network and prevents unexpected inbound connection attempts from console players who may have different content settings.",
                    Tags = ["xbox", "cross-play", "multiplayer", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks cross-play with console users; PC gaming sessions isolated from Xbox console multiplayer.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowCrossPlay", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowCrossPlay")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowCrossPlay", 0)],
                },
            ];
    }

    // ── XboxNetworkingPolicy ──
    private static class _XboxNetworkingPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\XboxLive";
        private const string SvcKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Gaming";
        private const string GipKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameInput";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-live-access",
                    Label = "Disable Xbox Live Network Access for Win32 Apps",
                    Category = "Gaming",
                    Description =
                        "Prevents Win32 (non-Store) applications from accessing Xbox Live services and the Xbox Identity Provider API, blocking non-Store games from connecting to Xbox Live authentication and social services.",
                    Tags = ["xbox", "xbox-live", "win32", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox Live access blocked for Win32 apps; non-Store games cannot authenticate or use Xbox services.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockXboxLiveForWin32Apps", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockXboxLiveForWin32Apps")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockXboxLiveForWin32Apps", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-live-signin",
                    Label = "Disable Xbox Live Automatic Sign-In at Windows Logon",
                    Category = "Gaming",
                    Description =
                        "Prevents the Xbox Identity Provider service from automatically signing in the user to Xbox Live when they log on to Windows, reducing outbound authentication traffic and Xbox identity disclosure.",
                    Tags = ["xbox", "xbox-live", "auto-signin", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox Live auto-sign-in at logon disabled; user not automatically authenticated to Xbox on Windows startup.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoSignIn", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoSignIn")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoSignIn", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-gameinput-service",
                    Label = "Disable GameInput Service Auto-Start",
                    Category = "Gaming",
                    Description =
                        "Prevents the GameInput host service from starting automatically at boot, reducing background service overhead on non-gaming devices. GameInput is required only for Xbox controller input handling in games.",
                    Tags = ["gameinput", "service", "auto-start", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GameInput service auto-start disabled; Xbox controller input may not work in some games until service is started.",
                    ApplyOps = [RegOp.SetDword(GipKey, "DisableAutoStart", 1)],
                    RemoveOps = [RegOp.DeleteValue(GipKey, "DisableAutoStart")],
                    DetectOps = [RegOp.CheckDword(GipKey, "DisableAutoStart", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-block-xbox-identity-provider",
                    Label = "Block Xbox Identity Provider from Network Access",
                    Category = "Gaming",
                    Description =
                        "Restricts the Xbox Identity Provider (XIP) from making outbound network requests, effectively preventing Xbox authentication tokens from being obtained via the network and Xbox Live connectivity.",
                    Tags = ["xbox", "identity-provider", "network-block", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox Identity Provider blocked from network; Xbox Live authentication tokens cannot be obtained.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockXboxIdentityProviderNetwork", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockXboxIdentityProviderNetwork")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockXboxIdentityProviderNetwork", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-presence",
                    Label = "Disable Xbox Presence and Social Notifications",
                    Category = "Gaming",
                    Description =
                        "Disables Xbox presence reporting and social notifications (friends online, friend activity, achievement alerts) from appearing on the Windows desktop, preventing Xbox social platform notifications from interrupting work.",
                    Tags = ["xbox", "presence", "social", "notifications", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox social presence and achievement notifications disabled; Xbox friend alerts not shown on desktop.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePresenceAndSocialNotifications", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePresenceAndSocialNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePresenceAndSocialNotifications", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-tcui",
                    Label = "Disable Xbox Title-Callable UI Overlay",
                    Category = "Gaming",
                    Description =
                        "Prevents Xbox Title-Callable UI (TCUI) — the Xbox social overlay triggered by games — from rendering on top of applications, eliminating the in-game Xbox social layer that shows friend lists and achievement progress.",
                    Tags = ["xbox", "tcui", "overlay", "social", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox TCUI in-game social overlay disabled; Xbox friend list and achievement overlay no longer rendered.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTCUI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTCUI")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTCUI", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-gaming-services-update",
                    Label = "Disable Automatic Gaming Services Component Updates",
                    Category = "Gaming",
                    Description =
                        "Prevents the Gaming Services package (GamingServices.exe) and Xbox Gaming Overlay from automatically updating via the Microsoft Store in the background, ensuring controlled update cycles for gaming components.",
                    Tags = ["gaming-services", "auto-update", "store", "gaming", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Gaming Services auto-update disabled; Xbox Gaming Overlay not updated automatically via Store.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "DisableGamingServicesAutoUpdate", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "DisableGamingServicesAutoUpdate")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "DisableGamingServicesAutoUpdate", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-app-launch-on-connect",
                    Label = "Disable Xbox App Auto-Launch on Xbox Controller Connect",
                    Category = "Gaming",
                    Description =
                        "Prevents the Xbox application from automatically launching when an Xbox One or Series controller is connected via USB or Bluetooth, eliminating unwanted UI interruptions when using controllers with non-Xbox applications.",
                    Tags = ["xbox-app", "controller", "auto-launch", "usb", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox app auto-launch on controller connect disabled; plugging an Xbox controller does not open the Xbox app.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableXboxAppAutoLaunch", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableXboxAppAutoLaunch")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableXboxAppAutoLaunch", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-disable-xbox-networking-telemetry",
                    Label = "Disable Xbox Networking and Gaming Service Telemetry",
                    Category = "Gaming",
                    Description =
                        "Prevents Xbox networking services, Gaming Services, and GameInput from sending usage, connectivity, and diagnostic telemetry to Microsoft.",
                    Tags = ["xbox", "gaming-services", "telemetry", "privacy", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Xbox networking and gaming service telemetry to Microsoft disabled; gaming usage data not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableXboxTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableXboxTelemetry")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableXboxTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "xboxnet-log-gaming-service-events",
                    Label = "Log Gaming Services Start/Stop Events in System Log",
                    Category = "Gaming",
                    Description =
                        "Enables System event log entries for Gaming Services (GamingServices.exe) start, stop, crash, and recovery events, providing audit visibility into Xbox/gaming component lifecycle on corporate endpoints.",
                    Tags = ["gaming-services", "event-log", "audit", "xbox", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Gaming Services start/stop events logged in System log; Xbox component lifecycle visible for auditing.",
                    ApplyOps = [RegOp.SetDword(SvcKey, "LogGamingServiceEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(SvcKey, "LogGamingServiceEvents")],
                    DetectOps = [RegOp.CheckDword(SvcKey, "LogGamingServiceEvents", 1)],
                },
            ];
    }
}
