namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Audio
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "audio-disable-startup-sound",
            Label = "Disable Startup Sound",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows boot/startup sound played when logging in.",
            Tags = ["audio", "startup", "boot", "logon"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation",
                    "DisableStartupSound",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation",
                    "DisableStartupSound"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation",
                    "DisableStartupSound",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-enhancements",
            Label = "Disable Audio Enhancements",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all audio enhancements (equalizer, bass boost, virtual surround, loudness equalization).",
            Tags = ["audio", "enhancements", "equalizer", "processing"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-spatial-audio",
            Label = "Disable Spatial Audio Auto-Activation",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic spatial audio (Windows Sonic / Dolby Atmos) activation by the OS.",
            Tags = ["audio", "spatial", "sonic", "atmos"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio", 0)],
        },
        new TweakDef
        {
            Id = "audio-set-24bit-quality",
            Label = "Set Default Audio to 24-bit Quality",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default audio format to 24-bit quality for improved audio fidelity.",
            Tags = ["audio", "quality", "24-bit", "format"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultFormat", 24)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultFormat")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultFormat", 24)],
        },
        new TweakDef
        {
            Id = "audio-disable-exclusive-mode",
            Label = "Disable Audio Exclusive Mode",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents applications from taking exclusive control of audio devices, avoiding sound conflicts.",
            Tags = ["audio", "exclusive", "sharing", "device"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveMode", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveMode", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-low-battery-sound",
            Label = "Disable Low Battery Warning Sound",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Silences the audible alert played when the battery level drops below the warning threshold.",
            Tags = ["audio", "battery", "warning", "power"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound", 1),
            ],
        },
        new TweakDef
        {
            Id = "audio-exclusive-priority",
            Label = "Set Audio Exclusive Mode Priority",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets audio thread scheduling to highest priority. Reduces audio glitches during heavy CPU load. Default: Medium. Recommended: High for audio workstations.",
            Tags = ["audio", "priority", "performance", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    6
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    2
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "Medium"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-effects-global",
            Label = "Disable Audio Effects Globally",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables all audio effects system-wide via DisableEffects flag. Eliminates post-processing for cleaner audio output. Default: Enabled. Recommended: Disabled for studio use.",
            Tags = ["audio", "effects", "performance", "studio"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects", 1)],
        },
        new TweakDef
        {
            Id = "audio-exclusive-mode-priority",
            Label = "Set Exclusive Mode Audio Priority",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Enables exclusive mode audio priority for applications that request dedicated audio device access. Default: Disabled. Recommended: Enabled for DAWs.",
            Tags = ["audio", "exclusive", "priority", "daw"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-loudness-eq",
            Label = "Disable Loudness Equalization",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables loudness equalization which normalizes audio levels. Preserves original dynamic range of audio output. Default: Enabled. Recommended: Disabled for audiophiles.",
            Tags = ["audio", "loudness", "equalization", "dynamic-range"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization", 0)],
        },
        new TweakDef
        {
            Id = "audio-48khz-sample-rate",
            Label = "Set Default Sample Rate to 48 kHz",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the default audio sample rate to 48000 Hz (DVD quality). Matches standard for video and professional audio. Default: 44100 Hz. Recommended: 48000 Hz.",
            Tags = ["audio", "sample-rate", "48khz", "quality"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate", 48000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate", 48000)],
        },
        new TweakDef
        {
            Id = "audio-disable-auto-gain",
            Label = "Disable Automatic Gain Control",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic gain control that adjusts microphone input levels. Prevents unwanted volume fluctuations. Default: Enabled. Recommended: Disabled for streaming.",
            Tags = ["audio", "gain", "microphone", "agc", "streaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-dynamic-silence",
            Label = "Disable Dynamic Silence Detection",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables dynamic silence detection that may cut audio during quiet passages. Prevents unintended audio cutoff during low-volume sections. Default: Enabled. Recommended: Disabled for musicians.",
            Tags = ["audio", "silence", "detection", "cutoff", "production"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-app-capture",
            Label = "Disable Audio Application Capture Loopback",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables audio application loopback capture. Prevents applications from recording system audio output without permission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["audio", "capture", "loopback", "privacy", "recording"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture", 1),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-spatial-sound",
            Label = "Disable Spatial Sound",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Sonic and other spatial sound processing. Reduces audio latency for competitive gaming. Default: off (per-device).",
            Tags = ["audio", "spatial", "sonic", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableSpatialAudio", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableSpatialAudio")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableSpatialAudio", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-audio-ducking",
            Label = "Disable Audio Ducking",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic volume reduction of other sounds during voice calls. Default: reduce other sounds by 80%.",
            Tags = ["audio", "ducking", "volume", "communications"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
        },
        new TweakDef
        {
            Id = "audio-disable-sound-scheme",
            Label = "Set Sound Scheme to No Sounds",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the Windows sound scheme to No Sounds. Silences all system event sounds. Default: Windows Default scheme.",
            Tags = ["audio", "sound-scheme", "silent", "events"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".Default")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
        },
        new TweakDef
        {
            Id = "audio-disable-audio-enhancements-global",
            Label = "Disable Audio Enhancements Globally",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables all audio signal enhancements system-wide. Ensures bit-perfect output. Default: per-device.",
            Tags = ["audio", "enhancements", "disable", "bit-perfect"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 1)],
        },
        new TweakDef
        {
            Id = "audio-set-default-format-48khz",
            Label = "Set Default Audio Format to 48 kHz",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets the default audio sample rate to 48000 Hz via the audio subsystem policy. Default: device-dependent.",
            Tags = ["audio", "sample-rate", "48khz", "quality"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DefaultSampleRate", 48000)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DefaultSampleRate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DefaultSampleRate", 48000)],
        },
        new TweakDef
        {
            Id = "audio-disable-absolute-volume",
            Label = "Disable Bluetooth Absolute Volume",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Bluetooth absolute volume control. Separates Bluetooth device volume from system volume. Default: enabled.",
            Tags = ["audio", "bluetooth", "volume", "absolute"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "DisableAbsoluteVolume", 1),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-comm-ducking",
            Label = "Disable Communication Ducking",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Disables automatic audio ducking when communications are detected (e.g., calls). Prevents volume reduction during voice calls. Default: reduce 80%.",
            Tags = ["audio", "ducking", "communication", "volume"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference", 0)],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Multimedia\Audio", "UserDuckingPreference", 3)],
        },
        new TweakDef
        {
            Id = "audio-disable-notification-sounds",
            Label = "Disable Notification Sounds",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables all notification sounds. Toast and system notifications appear silently. Default: enabled.",
            Tags = ["audio", "notifications", "sounds", "quiet"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings",
                    "NOC_GLOBAL_SETTING_ALLOW_NOTIFICATION_SOUND",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-system-sounds",
            Label = "Disable System Sounds Scheme",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the sound scheme to 'No Sounds'. Disables all Windows event sounds (startup, navigation, errors). Default: Windows Default.",
            Tags = ["audio", "sounds", "scheme", "quiet"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".Default")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
        },
        new TweakDef
        {
            Id = "audio-reduce-audio-latency",
            Label = "Reduce Audio Latency (MMCSS)",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the MMCSS Audio task to high priority and low latency scheduling. Reduces audio glitches and pops. Default: Normal scheduling.",
            Tags = ["audio", "latency", "mmcss", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    6
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    2
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "Medium"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-gpu-priority",
            Label = "Set Audio GPU Priority to 8",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the GPU priority for the Audio MMCSS task to 8 (high). Ensures audio-related GPU operations are not delayed. Default: 8.",
            Tags = ["audio", "gpu", "priority", "mmcss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "GPU Priority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "GPU Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "GPU Priority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-pro-audio-scheduling",
            Label = "Set Pro Audio Scheduling Category",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Pro Audio MMCSS task scheduling category to High. Optimises CPU scheduling for pro audio workloads. Default: High.",
            Tags = ["audio", "pro-audio", "scheduling", "mmcss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority",
                    6
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority",
                    1
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority",
                    6
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-sfio-high-priority",
            Label = "Set SFIO Priority to High",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Scheduled File I/O (SFIO) priority for audio playback to High. Ensures audio stream buffers are filled with minimal delay. Default: Normal.",
            Tags = ["audio", "sfio", "priority", "io"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "SFIO Priority",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "SFIO Priority",
                    "Normal"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "SFIO Priority",
                    "High"
                ),
            ],
        },
        // ── Sprint 19 additions ────────────────────────────────────────────
        new TweakDef
        {
            Id = "audio-disable-recording-quality-limit",
            Label = "Remove Recording Quality Limit",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Removes the default recording quality limit. Allows recording at the maximum quality supported by the hardware. Default: limited.",
            Tags = ["audio", "recording", "quality", "limit"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\MMDevices\Audio\Capture"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableRecordingQualityLimit", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableRecordingQualityLimit")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableRecordingQualityLimit", 1)],
        },
        new TweakDef
        {
            Id = "audio-enable-stereo-mix",
            Label = "Enable Stereo Mix Device",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables the Stereo Mix virtual recording device for capturing system audio output. Default: disabled/hidden.",
            Tags = ["audio", "stereo", "mix", "recording"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableStereoMix", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableStereoMix")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableStereoMix", 1)],
        },
        new TweakDef
        {
            Id = "audio-set-mmcss-scheduling",
            Label = "Optimise MMCSS Audio Scheduling",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the Multimedia Class Scheduler Service (MMCSS) to prioritise audio threads. Reduces audio glitches. Default: default scheduling.",
            Tags = ["audio", "mmcss", "scheduling", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "SystemResponsiveness",
                    10
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
                    10
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-network-throttling",
            Label = "Disable Network Throttling for Audio",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables network throttling during multimedia playback. Prevents network-related audio dropouts. Default: throttled.",
            Tags = ["audio", "network", "throttling", "streaming"],
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
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile",
                    "NetworkThrottlingIndex"
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
            Id = "audio-disable-audio-graph-isolation",
            Label = "Disable Audio Graph Isolation",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Windows Audio Graph isolation process (audiodg.exe enhancements). Can reduce latency for DAW users. Default: enabled.",
            Tags = ["audio", "graph", "isolation", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 1)],
        },
        new TweakDef
        {
            Id = "audio-set-device-priority-high",
            Label = "Set Audio Device Priority to High",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets audio device thread priority to high. Improves audio playback reliability under CPU load. Default: normal.",
            Tags = ["audio", "priority", "device", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "Medium"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Scheduling Category",
                    "High"
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-dpc-latency-low",
            Label = "Reduce Audio DPC Latency",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Reduces Deferred Procedure Call (DPC) latency for audio processing. Minimises audio stuttering. Default: 10ms.",
            Tags = ["audio", "dpc", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Latency Sensitive",
                    1
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Latency Sensitive"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Latency Sensitive",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-beep-sounds",
            Label = "Disable System Beep",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the system beep (PC speaker). Stops the annoying beep on errors and backspace in CMD. Default: enabled.",
            Tags = ["audio", "beep", "speaker", "disable"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Beep"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Beep", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Beep", "Start", 1)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Beep", "Start", 4)],
        },
        new TweakDef
        {
            Id = "audio-disable-critical-battery-sound",
            Label = "Disable Critical Battery Sound",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Silences the critical battery level alert sound. The visual notification remains. Default: enabled.",
            Tags = ["audio", "battery", "critical", "alert"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current", "", "")],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current", "")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\CriticalBatteryAlarm\.Current", "", "")],
        },
        new TweakDef
        {
            Id = "audio-set-headphone-auto-detect",
            Label = "Enable Headphone Auto-Detection",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Enables automatic audio device switching when headphones are plugged in or removed. Default: varies by driver.",
            Tags = ["audio", "headphone", "auto-detect", "switching"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableAutoDeviceSwitch", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableAutoDeviceSwitch")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "EnableAutoDeviceSwitch", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-comms-ducking",
            Label = "Disable Communication Audio Ducking",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Prevents Windows from automatically lowering the volume of other sounds during VoIP calls. Default: reduce by 80%. Recommended: Do nothing.",
            Tags = ["audio", "ducking", "voip", "volume", "calls"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\UserPreference"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\UserPreference", "UserPreferenceDucking", 3)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\UserPreference", "UserPreferenceDucking")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio\UserPreference", "UserPreferenceDucking", 3)],
        },
        new TweakDef
        {
            Id = "audio-set-pro-audio-priority",
            Label = "Set Pro Audio Task to Maximum Priority",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Assigns the highest scheduling priority and GPU priority to the Pro Audio task profile for minimal latency in DAW/recording software.",
            Tags = ["audio", "pro-audio", "daw", "latency", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority",
                    1
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority",
                    8
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Scheduling Category",
                    "High"
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "SFIO Priority",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Scheduling Category"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "SFIO Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "Priority",
                    1
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-disable-audio-idle-powerdown",
            Label = "Disable Audio Device Power-Down on Idle",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Prevents the audio subsystem from entering low-power idle state, eliminating the pop/click that occurs when audio wakes up after silence.",
            Tags = ["audio", "power", "idle", "pop", "click", "crackling"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableProtectedAudioDG", 0)],
        },
        new TweakDef
        {
            Id = "audio-set-avrcp-volume-sync",
            Label = "Disable Bluetooth AVRCP Volume Sync",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables automatic volume synchronisation between the Bluetooth AVRCP controller and Windows media volume. Useful when Bluetooth devices control their own volume independently.",
            Tags = ["audio", "bluetooth", "avrcp", "volume", "sync"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "IsAbsoluteVolumeSupported", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "IsAbsoluteVolumeSupported"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Bluetooth\Audio\AVRCP\CT", "IsAbsoluteVolumeSupported", 0),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-audio-latency-mode",
            Label = "Set Audio Engine to Low-Latency Mode",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Configures the Windows Audio Engine to use the lowest achievable latency period. Reduces audio buffering at the cost of slightly higher CPU usage.",
            Tags = ["audio", "latency", "engine", "buffer", "wasapi"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyMode", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyMode"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile", "NoLazyMode", 1),
            ],
        },
        new TweakDef
        {
            Id = "audio-enable-audio-log-off",
            Label = "Mute Audio Service Log Event Verbosity",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Enables the Windows Audio service to run with reduced EventLog verbosity. Suppresses diagnostic spew to the System event log from audiodg.exe.",
            Tags = ["audio", "logging", "events", "diagnostics", "audiodg"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Audiosrv\Parameters"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Audiosrv\Parameters", "ServiceDllUnloadOnStop", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Audiosrv\Parameters", "ServiceDllUnloadOnStop")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Audiosrv\Parameters", "ServiceDllUnloadOnStop", 1)],
        },
        new TweakDef
        {
            Id = "audio-set-endpoint-builder-manual",
            Label = "Set Audio Endpoint Builder to Manual Start",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Sets the Windows Audio Endpoint Builder service to manual start. This reduces idle startup time when audio hardware is not actively in use.",
            Tags = ["audio", "endpoint-builder", "service", "startup", "ram"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AudioEndpointBuilder"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AudioEndpointBuilder", "Start", 3)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AudioEndpointBuilder", "Start", 2)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\AudioEndpointBuilder", "Start", 3)],
        },
        new TweakDef
        {
            Id = "audio-disable-voice-typing-toast",
            Label = "Disable Voice Typing Microphone Toast",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Suppresses the toast notification prompting users to try Voice Typing when a microphone is connected.",
            Tags = ["audio", "voice", "typing", "toast", "notification"],
            RegistryKeys = [@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.VoiceTyping"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.VoiceTyping",
                    "Enabled",
                    0
                ),
            ],
            RemoveOps =
            [
                RegOp.SetDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.VoiceTyping",
                    "Enabled",
                    1
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Notifications\Settings\Windows.SystemToast.VoiceTyping",
                    "Enabled",
                    0
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-render-clock-rate",
            Label = "Set Audio Render Clock Rate to 10000ns",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the audio rendering clock period to 10000 (100ns units = 1ms), the minimum supported by Windows Audio. Reduces audio callback jitter.",
            Tags = ["audio", "clock", "render", "latency", "jitter"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Clock Rate",
                    10000
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Clock Rate"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio",
                    "Clock Rate",
                    10000
                ),
            ],
        },
        new TweakDef
        {
            Id = "audio-set-capture-clock-rate",
            Label = "Set Audio Capture Clock Rate to 10000ns",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Sets the audio capture (recording) clock period to 10000 (100ns units = 1ms), matching the render clock for consistent low-latency recording.",
            Tags = ["audio", "clock", "capture", "recording", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Clock Rate",
                    10000
                ),
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Priority",
                    1
                ),
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Scheduling Category",
                    "High"
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Clock Rate"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Priority"
                ),
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Scheduling Category"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Capture",
                    "Clock Rate",
                    10000
                ),
            ],
        },
    ];
}

// === Merged from: Multimedia.cs ===


internal static class Multimedia
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [

        new TweakDef
        {
            Id = "media-disable-autorun",
            Label = "Disable AutoRun for All Drives",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables AutoRun for all drive types, preventing automatic execution of autorun.inf files. Mitigates USB-based malware. Default: Enabled. Recommended: Disabled.",
            Tags = ["multimedia", "autorun", "security", "usb"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoAutorun", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-media-sharing",
            Label = "Disable Media Streaming/Sharing Service",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables the Windows Media Player Network Sharing Service (WMPNetworkSvc). Prevents DLNA media streaming to other devices. Default: Manual. Recommended: Disabled.",
            Tags = ["multimedia", "sharing", "dlna", "service"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "media-set-wallpaper-quality",
            Label = "Set Wallpaper JPEG Quality to Maximum",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets desktop wallpaper JPEG import quality to 100 percent. Prevents Windows from recompressing wallpaper images. Default: ~85. Recommended: 100.",
            Tags = ["multimedia", "wallpaper", "quality", "display"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Control Panel\Desktop"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Control Panel\Desktop", "JPEGImportQuality", 100)],
        },


        new TweakDef
        {
            Id = "media-disable-media-streaming",
            Label = "Disable Windows Media Streaming (Policy)",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows Media Player library sharing via policy. Prevents media streaming to other devices on the network. Default: allowed. Recommended: disabled for security.",
            Tags = ["multimedia", "streaming", "media", "sharing", "policy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
        new TweakDef
        {
            Id = "media-set-default-player-assoc",
            Label = "Set Default Media Player Associations",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Suppresses WMP first-run setup and player prompt via policy. Prevents Windows Media Player from claiming file associations. Default: enabled. Recommended: disabled.",
            Tags = ["multimedia", "player", "associations", "default", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun", 0),
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PlayerPrompt", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun"),
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PlayerPrompt"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "SetupFirstRun", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-wm-drm",
            Label = "Disable Windows Media DRM",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description =
                "Disables Windows Media DRM online license acquisition via policy. Prevents DRM phone-home for protected media content. Default: enabled. Recommended: disabled for privacy.",
            Tags = ["multimedia", "drm", "wmdrm", "license", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-gamebar-policy",
            Label = "Disable Xbox Game Bar (Policy)",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables the Xbox Game Bar via Group Policy (AllowGameDVR=0). Prevents Win+G from opening and removes the overlay entirely. Default: Allowed. Recommended: Disabled for non-gaming workstations.",
            Tags = ["multimedia", "gamebar", "xbox", "policy", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\GameDVR", "AllowGameDVR", 0)],
        },
        new TweakDef
        {
            Id = "media-reduce-tooltip-delay",
            Label = "Instant Tooltip Display (0 ms)",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets extended UI hover time to 0, making Explorer tooltips appear immediately. Default: system default. Recommended: 0 for fast typists.",
            Tags = ["multimedia", "tooltip", "delay", "explorer", "ux"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime")],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "ExtendedUIHoverTime", 0),
            ],
        },

        new TweakDef
        {
            Id = "media-set-wmf-no-telemetry",
            Label = "Disable Windows Media Foundation Telemetry",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables telemetry/DRM phone-home in Windows Media Foundation components. Default: enabled.",
            Tags = ["media", "wmf", "telemetry", "drm"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WMDRM", "DisableOnline", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-casting",
            Label = "Disable Media Casting to Device",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Cast to Device feature for media streaming. Default: enabled.",
            Tags = ["media", "cast", "device", "streaming"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PlayToReceiver", "AutoEnabled", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-media-player-sharing",
            Label = "Disable Windows Media Player Network Sharing",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables the Windows Media Player Network Sharing Service. Prevents DLNA media streaming. Default: enabled.",
            Tags = ["multimedia", "media-player", "sharing", "dlna"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
            RemoveOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 3)],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\WMPNetworkSvc", "Start", 4)],
        },
        new TweakDef
        {
            Id = "media-disable-disc-burning",
            Label = "Disable CD/DVD Burning in Explorer",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables the built-in CD/DVD burning capability in Windows Explorer. Default: enabled.",
            Tags = ["multimedia", "disc", "burning", "cdrom"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer", "NoCDBurning", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-sound-scheme",
            Label = "Disable System Sound Scheme",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description =
                "Sets the Windows sound scheme to None, disabling all system sounds. Creates a silent desktop experience. Default: Windows Default.",
            Tags = ["multimedia", "sound", "scheme", "silent"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes"],
            ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
            RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".Default")],
            DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\AppEvents\Schemes", "", ".None")],
        },
        new TweakDef
        {
            Id = "media-disable-wmp-network-sharing",
            Label = "Disable WMP Network Sharing",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description =
                "Disables Windows Media Player network sharing service. Prevents media library from being shared across the network. Default: enabled.",
            Tags = ["multimedia", "wmp", "sharing", "network"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventLibrarySharing", 1)],
        },
        // ── Sprint 47 additions ───────────────────────────────────────────────
        new TweakDef
        {
            Id = "media-disable-wmp-autoplay",
            Label = "Disable WMP AutoPlay",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows Media Player from automatically playing media when inserted.",
            Tags = ["multimedia", "wmp", "autoplay"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart", 0)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\MediaPlayer\Player\Settings", "AutoStart", 0)],
        },
        new TweakDef
        {
            Id = "media-disable-wmp-codec-download",
            Label = "Disable WMP Automatic Codec Download",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Windows Media Player from automatically downloading codecs from the internet.",
            Tags = ["multimedia", "wmp", "codecs", "privacy"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "DisableAutoUpdate", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-video-thumbnail-cache",
            Label = "Disable Video Thumbnail Cache",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Stops Windows from caching video thumbnail images in hidden system folders.",
            Tags = ["multimedia", "thumbnails", "privacy", "disk"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer", "DisableThumbnailCache", 1)],
        },
        new TweakDef
        {
            Id = "media-set-system-responsiveness-media",
            Label = "Optimise System Responsiveness for Media",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MMCSS SystemResponsiveness to 0 so multimedia threads get maximum CPU time.",
            Tags = ["multimedia", "performance", "audio", "mmcss"],
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
            Id = "media-enable-hardware-video-decode",
            Label = "Enable Hardware-Accelerated Video Decode",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Forces Windows apps to use GPU hardware decoding for video playback to reduce CPU load.",
            Tags = ["multimedia", "gpu", "performance", "video"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings"],
            ApplyOps = [RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode", 2)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode")],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\VideoSettings", "VideoQualityMode", 2)],
        },
        new TweakDef
        {
            Id = "media-set-pro-audio-latency",
            Label = "Set Pro Audio Scheduling Latency",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets MMCSS Pro Audio task scheduling key for lower latency; optimises for audio production.",
            Tags = ["multimedia", "audio", "latency", "pro-audio", "mmcss"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio"],
            ApplyOps =
            [
                RegOp.SetDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority",
                    8
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckDword(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Pro Audio",
                    "GPU Priority",
                    8
                ),
            ],
        },
        new TweakDef
        {
            Id = "media-disable-casting-extension",
            Label = "Disable Cast to Device Extension",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Removes the 'Cast to device' option from Explorer context menus.",
            Tags = ["multimedia", "casting", "context-menu"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked"],
            ApplyOps =
            [
                RegOp.SetString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}"
                ),
            ],
            DetectOps =
            [
                RegOp.CheckString(
                    @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Shell Extensions\Blocked",
                    "{7AD84985-87B4-4a16-BE58-8B72A5B390F7}",
                    ""
                ),
            ],
        },
        new TweakDef
        {
            Id = "media-disable-media-metadata-streaming",
            Label = "Disable Media Metadata Internet Lookup",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Prevents Media Player from downloading album art and track metadata from the internet.",
            Tags = ["multimedia", "privacy", "metadata", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval", 1)],
            RemoveOps = [RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval")],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventCDDVDMetadataRetrieval", 1)],
        },
        new TweakDef
        {
            Id = "media-disable-media-usage-reporting",
            Label = "Disable Media Usage Reporting",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables anonymous usage reporting sent from Windows Media Player to Microsoft.",
            Tags = ["multimedia", "privacy", "telemetry", "wmp"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer"],
            ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval", 1)],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval"),
            ],
            DetectOps =
            [
                RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsMediaPlayer", "PreventMusicFileMetadataRetrieval", 1),
            ],
        },
    ];
}
