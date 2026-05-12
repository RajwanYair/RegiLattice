namespace RegiLattice.Core.Tweaks;

// Sprint B.2: attribute-based module discovery sample

using RegiLattice.Core.Models;

[TweakModule]
internal static class Gaming
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "game-disable-xbox-services",
            Label = "Disable Xbox Background Services",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-disable-game-bar-tips",
            Label = "Disable Game Bar Tips",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-disable-game-bar-presence-writer",
            Label = "Disable Game Bar Presence Writer",
            Category = "Gaming 1",
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
            Id = "game-disable-game-input-redirection",
            Label = "Disable Game Input Redirection",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-gaming-mode-priority",
            Label = "Set Gaming Task CPU Priority",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-disable-ndu-service",
            Label = "Disable Network Data Usage Monitor (NDU)",
            Category = "Gaming 1",
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
            Id = "game-set-gpu-priority-8",
            Label = "Set High GPU Priority for Games",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-increase-max-user-port",
            Label = "Increase Max UDP/TCP Port Range for Gaming",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-set-dxgi-flip-model",
            Label = "Prefer Flip Presentation Model for DirectX",
            Category = "Gaming 1",
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
            Id = "game-set-games-sfio-priority-high",
            Label = "Set Games SFIO Priority to High",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-input-hooks-fast",
            Label = "Reduce LowLevel Input Hook Timeout",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-audio-latency-1ms",
            Label = "Set Multimedia Audio Latency to 1ms",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-disable-game-dvr-shadow",
            Label = "Disable Game DVR Shadow Recording",
            Category = "Gaming 1",
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
            Id = "game-disable-bcast-dvr-svc",
            Label = "Disable Broadcast DVR User Service",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "game-disable-uwp-bg-access",
            Label = "Block All UWP Apps from Running in Background (GPO)",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "xbgb-disable-capture-audio",
            Label = "Disable Game Bar Audio Capture",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "xbgb-disable-full-scene-optimizations",
            Label = "Disable Full-Scene Optimizations Globally",
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Category = "Gaming 1",
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
            Id = "xbgb-disable-dxgi-fse-compat",
            Label = "Disable Game DVR DXGI Fullscreen Compatibility Mode",
            Category = "Gaming 1",
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
            Id = "xbgb-disable-efs-feature-hooks",
            Label = "Disable Game DVR Extended FSE Feature Flags",
            Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                    Id = "gamebar-disable-gamebar-tips",
                    Label = "Disable Game Bar First-Run Tips and Overlay Prompts",
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                    Category = "Gaming 1",
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
                Id = "gamedvr-disable-game-bar",
                Label = "Game DVR Policy: Disable Xbox Game Bar",
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 1",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Id = "gamperf-enable-multimedia-gaming-class-scheduler",
                    Label = "Gaming Perf: Enable Multimedia Class Scheduler for Gaming Tasks",
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
                    Category = "Gaming 2",
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
