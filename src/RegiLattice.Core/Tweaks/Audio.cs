namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class Audio
{
    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "audio-disable-system-sounds",
            Label = "Disable System Sounds",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the Windows sound scheme to .None, silencing all system event sounds (alerts, notifications, asterisks, etc.).",
            Tags = ["audio", "sounds", "scheme", "silence"],
            RegistryKeys = [@"HKEY_CURRENT_USER\AppEvents\Schemes"],
        },
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
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Authentication\LogonUI\BootAnimation", "DisableStartupSound", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-comm-ducking",
            Label = "Disable Communication Auto-Reduction",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Prevents Windows from automatically reducing the volume of other sounds during voice/video calls (UserDuckingPreference=3).",
            Tags = ["audio", "ducking", "communication", "volume"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Audio", "EnableSpatialAudio", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-notification-sounds",
            Label = "Disable Notification Sounds",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Silences all notification sounds while keeping toast notifications visible.",
            Tags = ["audio", "notifications", "sounds", "toast"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Notifications\Settings"],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultFormat", 24),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultFormat"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveMode", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveMode"),
            ],
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
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "DisableLowBatterySound", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-speech-recognition",
            Label = "Disable Online Speech Recognition",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Opts out of online speech recognition, preventing voice data from being sent to Microsoft cloud services.",
            Tags = ["audio", "speech", "recognition", "privacy"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 1),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Speech_OneCore\Settings\OnlineSpeechPrivacy", "HasAccepted", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-enhancements-global",
            Label = "Disable Audio Enhancements Globally",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables all audio processing enhancements system-wide. Reduces audio latency and prevents driver conflicts. Default: Enabled. Recommended: Disabled for music production.",
            Tags = ["audio", "enhancements", "latency", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DisableAllEnhancements", 1)],
        },
        new TweakDef
        {
            Id = "audio-exclusive-priority",
            Label = "Set Audio Exclusive Mode Priority",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Sets audio thread scheduling to highest priority. Reduces audio glitches during heavy CPU load. Default: Medium. Recommended: High for audio workstations.",
            Tags = ["audio", "priority", "performance", "latency"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", "Priority", 6),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", "Scheduling Category", "High"),
            ],
            RemoveOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", "Priority", 2),
                RegOp.SetString(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", "Scheduling Category", "Medium"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio", "Priority", 6)],
        },
        new TweakDef
        {
            Id = "audio-disable-effects-global",
            Label = "Disable Audio Effects Globally",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables all audio effects system-wide via DisableEffects flag. Eliminates post-processing for cleaner audio output. Default: Enabled. Recommended: Disabled for studio use.",
            Tags = ["audio", "effects", "performance", "studio"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableEffects", 1)],
        },
        new TweakDef
        {
            Id = "audio-exclusive-mode-priority",
            Label = "Set Exclusive Mode Audio Priority",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Enables exclusive mode audio priority for applications that request dedicated audio device access. Default: Disabled. Recommended: Enabled for DAWs.",
            Tags = ["audio", "exclusive", "priority", "daw"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "ExclusiveModePriority", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-absolute-volume",
            Label = "Disable Bluetooth Absolute Volume",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Disables Bluetooth absolute volume control. Allows independent volume adjustment for BT and system. Default: Enabled. Recommended: Disabled for BT headphones.",
            Tags = ["audio", "bluetooth", "absolute-volume", "headphones"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Control\Bluetooth\Audio\AVRCP\CT"],
        },
        new TweakDef
        {
            Id = "audio-reduce-audio-latency",
            Label = "Reduce Audio Latency",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Enables low-latency audio mode by reducing the audio buffer size. Improves real-time audio responsiveness. Default: Disabled. Recommended: Enabled for music production.",
            Tags = ["audio", "latency", "buffer", "performance"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
        },
        new TweakDef
        {
            Id = "audio-disable-loudness-eq",
            Label = "Disable Loudness Equalization",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables loudness equalization which normalizes audio levels. Preserves original dynamic range of audio output. Default: Enabled. Recommended: Disabled for audiophiles.",
            Tags = ["audio", "loudness", "equalization", "dynamic-range"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "LoudnessEqualization", 0)],
        },
        new TweakDef
        {
            Id = "audio-48khz-sample-rate",
            Label = "Set Default Sample Rate to 48 kHz",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Sets the default audio sample rate to 48000 Hz (DVD quality). Matches standard for video and professional audio. Default: 44100 Hz. Recommended: 48000 Hz.",
            Tags = ["audio", "sample-rate", "48khz", "quality"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate", 48000),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "DefaultSampleRate", 48000)],
        },
        new TweakDef
        {
            Id = "audio-disable-auto-gain",
            Label = "Disable Automatic Gain Control",
            Category = "Audio",
            NeedsAdmin = false,
            CorpSafe = true,
            Description = "Disables automatic gain control that adjusts microphone input levels. Prevents unwanted volume fluctuations. Default: Enabled. Recommended: Disabled for streaming.",
            Tags = ["audio", "gain", "microphone", "agc", "streaming"],
            RegistryKeys = [@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_CURRENT_USER\Software\Microsoft\Multimedia\Audio", "AutomaticGainControl", 0)],
        },
        new TweakDef
        {
            Id = "audio-set-sfio-high-priority",
            Label = "Set Audio SFIO Priority to Normal",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets SFIO (Synchronous File I/O) priority for the Audio task to Normal. Prevents audio dropouts caused by competing I/O operations. Default: Not set. Recommended: Normal for audio production.",
            Tags = ["audio", "sfio", "priority", "latency", "production"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
        },
        new TweakDef
        {
            Id = "audio-set-pro-audio-scheduling",
            Label = "Set Audio Scheduling to Pro Audio",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets the Audio multimedia system profile scheduling category to 'Pro Audio'. Allocates higher CPU time slices to audio processing threads. Default: Medium. Recommended: Pro Audio for DAW use.",
            Tags = ["audio", "scheduling", "pro-audio", "daw", "priority"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
        },
        new TweakDef
        {
            Id = "audio-set-gpu-priority",
            Label = "Raise GPU Priority for Audio Task",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = false,
            Description = "Sets GPU Priority to 8 for the Audio multimedia system profile task. Reduces audio glitches when GPU is under heavy load. Default: Not set. Recommended: 8 for content creation.",
            Tags = ["audio", "gpu", "priority", "glitch", "production"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Multimedia\SystemProfile\Tasks\Audio"],
        },
        new TweakDef
        {
            Id = "audio-disable-dynamic-silence",
            Label = "Disable Dynamic Silence Detection",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables dynamic silence detection that may cut audio during quiet passages. Prevents unintended audio cutoff during low-volume sections. Default: Enabled. Recommended: Disabled for musicians.",
            Tags = ["audio", "silence", "detection", "cutoff", "production"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection", 0),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DynamicSilenceDetection", 0)],
        },
        new TweakDef
        {
            Id = "audio-disable-app-capture",
            Label = "Disable Audio Application Capture Loopback",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables audio application loopback capture. Prevents applications from recording system audio output without permission. Default: Enabled. Recommended: Disabled for privacy.",
            Tags = ["audio", "capture", "loopback", "privacy", "recording"],
            RegistryKeys = [@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio"],
            ApplyOps =
            [
                RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture", 1),
            ],
            RemoveOps =
            [
                RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture"),
            ],
            DetectOps = [RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Audio", "DisableAudioApplicationCapture", 1)],
        },
        new TweakDef
        {
            Id = "audio-disable-spatial-sound",
            Label = "Disable Spatial Sound",
            Category = "Audio",
            NeedsAdmin = true,
            CorpSafe = true,
            Description = "Disables Windows Sonic and other spatial sound processing. Reduces audio latency for competitive gaming. Default: off (per-device).",
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
    ];
}
