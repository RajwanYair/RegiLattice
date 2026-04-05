namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static partial class PolicyDesktop
{
    // ── ModernStandbyPolicy ──
    private static class _ModernStandbyPolicy
    {
        private const string MsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ModernStandby";
        private const string PwrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings";
        private const string PwrSleepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "mstandby-disable-connected-standby",
                    Label = "Disable Modern Standby (S0 Low-Power Idle) — Use S3 Sleep",
                    Category = "Display — Input Method",
                    Description =
                        "Disables Modern Standby (S0ix) and falls back to the traditional S3 sleep state. S0 keeps the network and background apps active during sleep, which can interfere with security tools, drain battery unexpectedly, and create wake-on-network attack surfaces.",
                    Tags = ["modern-standby", "s0", "s3-sleep", "power", "disable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 3,
                    ImpactNote =
                        "Forces S3 sleep where hardware supports it. Network and background activity cease during sleep — improves battery life on older HW but disables instant-on and wake-on-LAN in S0. Some OEM hardware only supports S0 and cannot fall back.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-block-network-during-standby",
                    Label = "Block Network Activity During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the NIC from remaining active and processing network packets while the device is in Modern Standby. Reduces the attack surface from wake-on-LAN exploitation, rogue DHCP offers, and directed broadcast attacks arriving while the user is away.",
                    Tags = ["modern-standby", "network", "wifi", "attack-surface", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Push notifications, live tiles, and scheduled background sync will not occur while the device is in standby. Recommended for shared, high-security environments.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "NetworkActivityAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "NetworkActivityAllowed")],
                    DetectOps = [RegOp.CheckDword(MsKey, "NetworkActivityAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-smart-standby",
                    Label = "Disable Adaptive Smart Standby Adjustments",
                    Category = "Display — Input Method",
                    Description =
                        "Disables the intelligent standby system that dynamically adjusts deep-sleep exit rates based on historical usage patterns. When disabled, the system uses fixed configured timeouts rather than ML-driven adaptive transitions.",
                    Tags = ["modern-standby", "adaptive", "smart-standby", "power", "predictable"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Produces deterministic standby behaviour at the cost of optimal power efficiency. Useful for kiosk and fixed-use devices where predictable power cycling is preferred.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "DisableSmartStandby", 1)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "DisableSmartStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "DisableSmartStandby", 1)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-background-tasks-in-standby",
                    Label = "Disable Background Task Execution During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents application background tasks from running while the system is in Modern Standby. Background tasks in S0 consume battery, can trigger wake-locks that prevent deep sleep, and may leak user data via cloud sync while the device appears powered off.",
                    Tags = ["modern-standby", "background-tasks", "battery", "privacy", "s0"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Suppresses background app refresh during standby; notifications and cloud sync resume on user wake. Significantly improves battery life on devices with aggressive background app models.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowBackgroundTasksInStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-maintenance-in-standby",
                    Label = "Disable Automatic Maintenance Execution During Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the Windows Automatic Maintenance scheduler from running maintenance tasks (Disk Defrag, Windows Defender scans, app updates) while the device is in Modern Standby. Avoids unexpected disk I/O, CPU wake, and battery drain during standby periods.",
                    Tags = ["modern-standby", "maintenance", "automatic-maintenance", "battery", "scheduling"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Maintenance tasks (including Defender scans) will defer to the next active session. Track that maintenance completes during awake sessions to avoid indefinite deferral.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "AllowMaintenanceDuringStandby")],
                    DetectOps = [RegOp.CheckDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-require-fast-startup-disabled",
                    Label = "Disable Hybrid Shutdown / Fast Startup (Hiberboot)",
                    Category = "Display — Input Method",
                    Description =
                        "Disables Hybrid Shutdown (Fast Startup / Hiberboot) which persists kernel session to the hibernate file across reboots. Hiberboot bypasses full driver reinitialisation and can leave security tools in stale state; full cold boot is safer and more predictable.",
                    Tags = ["modern-standby", "fast-startup", "hiberboot", "hibernate", "cold-boot"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Shutdown/startup will be slightly slower but every boot is a clean cold boot. Recommended for compliance-sensitive environments and shared machines.",
                    RegistryKeys = [PwrKey],
                    ApplyOps = [RegOp.SetDword(PwrKey, "HiberbootEnabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(PwrKey, "HiberbootEnabled")],
                    DetectOps = [RegOp.CheckDword(PwrKey, "HiberbootEnabled", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-set-idle-standby-timeout",
                    Label = "Set Plugged-In Idle-to-Standby Timeout to 30 Minutes",
                    Category = "Display — Input Method",
                    Description =
                        "Configures the AC (plugged-in) idle timeout before the system enters Modern Standby or sleep to 30 minutes (1800 seconds). Reduces the window in which an unattended unlocked workstation is physically accessible before it locks and suspends.",
                    Tags = ["modern-standby", "idle-timeout", "screen-lock", "power", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "30-minute AC idle timeout before sleep is a reasonable physical-security baseline for workstations. Pairs with screen lock and credential timeout policies.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "IdleTimeoutAC")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
                },
                new TweakDef
                {
                    Id = "mstandby-block-wake-timers-in-standby",
                    Label = "Block Programmatic Wake Timers During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents applications and scheduled tasks from setting wake timers that force the system out of Modern Standby. Rogue or poorly coded applications can use wake timers to keep the device powered on continuously; blocking timers enforces true standby.",
                    Tags = ["modern-standby", "wake-timer", "power", "battery", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocks all application-set wake timers during standby. Windows Update maintenance wake timers are a notable exception — it may still wake for critical updates depending on policy.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "AllowWakeTimers", 0)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "AllowWakeTimers")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "AllowWakeTimers", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-disable-wol-in-standby",
                    Label = "Disable Wake-on-LAN During Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Prevents the NIC from processing Wake-on-LAN (WoL) magic packets while the device is in Modern Standby. Eliminates the network-based remote-wake attack surface; an attacker with network access cannot remotely wake and attack the device.",
                    Tags = ["modern-standby", "wake-on-lan", "wol", "network", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Disables WoL in standby; remote power-on via network magic packet will not work while in S0. BIOS/UEFI WoL may override this — also disable WoL there for full protection.",
                    RegistryKeys = [MsKey],
                    ApplyOps = [RegOp.SetDword(MsKey, "WakeOnLanAllowed", 0)],
                    RemoveOps = [RegOp.DeleteValue(MsKey, "WakeOnLanAllowed")],
                    DetectOps = [RegOp.CheckDword(MsKey, "WakeOnLanAllowed", 0)],
                },
                new TweakDef
                {
                    Id = "mstandby-require-password-on-resume",
                    Label = "Require Password When Resuming from Modern Standby",
                    Category = "Display — Input Method",
                    Description =
                        "Forces credential re-entry when the device resumes from Modern Standby or sleep. Without this policy the screen may stay unlocked after resume, exposing the session to physical access attacks on shared or temporarily unattended machines.",
                    Tags = ["modern-standby", "password-resume", "screen-lock", "credential", "physical-security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 18362,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote =
                        "Ensures the screen is locked on every standby resume, requiring Windows Hello or password to re-enter the session. This is a standard physical-security baseline.",
                    RegistryKeys = [PwrSleepKey],
                    ApplyOps = [RegOp.SetDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
                    RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "PromptPasswordOnWakeup")],
                    DetectOps = [RegOp.CheckDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
                },
            ];
    }

    // ── PenWorkspaceGpoPolicy ──
    private static class _PenWorkspaceGpoPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PenWorkspace";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "penws-disable-pen-workspace",
                Label = "Disable Pen Workspace",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceButtonDesiredVisibility=0 in the PenWorkspace policy key. "
                    + "Hides the Pen Workspace button from the taskbar and prevents the floating "
                    + "Pen Workspace panel from launching. Pen Workspace aggregates Windows Ink, "
                    + "Sticky Notes, and Screen Sketch into a sidebar. On devices without a "
                    + "pen or stylus this button serves no purpose. "
                    + "Default: not set (shown on pen-equipped devices). Recommended: 0.",
                Tags = ["pen", "workspace", "taskbar", "ink", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceButtonDesiredVisibility")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceButtonDesiredVisibility", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-above-lock",
                Label = "Disable Pen Workspace Above Lock Screen",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets UserEducationInAboveLockAllowed=0 in the PenWorkspace policy key. "
                    + "Prevents the Windows Ink Workspace and associated onboarding prompts "
                    + "from appearing on the lock screen. Applications shown above the lock "
                    + "screen are accessible without authentication, creating a potential "
                    + "information-disclosure or bypass surface. "
                    + "Default: 1 (allowed). Recommended: 0 on security-hardened systems.",
                Tags = ["pen", "workspace", "lockscreen", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "UserEducationInAboveLockAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "UserEducationInAboveLockAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "UserEducationInAboveLockAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-touch-keyboard-onboarding",
                Label = "Disable Touch Keyboard Onboarding",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets TouchKeyboardOnboardingAllowed=0 in the PenWorkspace policy key. "
                    + "Suppresses the promotional 'Try the new touch keyboard' onboarding banner "
                    + "that appears in the touch keyboard session. The banner interrupts "
                    + "workflow on tablet form-factor devices and is purely marketing-oriented. "
                    + "Default: 1 (shown). Recommended: 0.",
                Tags = ["pen", "touch", "keyboard", "onboarding", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "TouchKeyboardOnboardingAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "TouchKeyboardOnboardingAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-handwriting-panel",
                Label = "Disable Handwriting Panel",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceHandwritingEnabled=0 in the PenWorkspace policy key. "
                    + "Disables the floating handwriting input panel that appears near text "
                    + "fields when a stylus approaches the screen. This panel intercepts "
                    + "stylus input before the active application and translates strokes to "
                    + "text via Windows Ink. Disabling it may improve stylus performance in "
                    + "drawing or annotation applications. Default: 1. Recommended: 0.",
                Tags = ["pen", "handwriting", "ink", "input", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceHandwritingEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceHandwritingEnabled", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-workspace-telemetry",
                Label = "Disable Pen Workspace Telemetry",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets PenWorkspaceTelemetryAllowed=0 in the PenWorkspace policy key. "
                    + "Stops Windows Ink Workspace from transmitting usage analytics covering "
                    + "which Ink apps were launched, pen interaction rates, stylus hardware "
                    + "model, and session durations to Microsoft's telemetry pipeline. "
                    + "These signals accumulate a detailed device-usage profile. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "workspace", "telemetry", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenWorkspaceTelemetryAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "PenWorkspaceTelemetryAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-ink-replay",
                Label = "Disable Ink Replay Logging",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets InkReplayEnabled=0 in the PenWorkspace policy key. Disables the "
                    + "Windows Ink replay feature that records the full sequence of pen strokes "
                    + "so they can be animated back at playback speed. Stroke replay data is "
                    + "stored as a journal that fully reconstructs handwritten content and can "
                    + "expose sensitive notes or signatures if the device is compromised. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "ink", "replay", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "InkReplayEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "InkReplayEnabled")],
                DetectOps = [RegOp.CheckDword(Key, "InkReplayEnabled", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-pen-promo",
                Label = "Disable Pen Workspace Hardware Promo",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets AllowSuggestedAppsInWindowsInkWorkspace=0 in the PenWorkspace policy "
                    + "key. Removes the 'Suggested Apps' section from Windows Ink Workspace "
                    + "that promotes pen-optimised Store apps. Suggested apps load metadata "
                    + "from the Microsoft Store CDN at every Workspace open, adding network "
                    + "latency and transmitting device pen-hardware telemetry. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "workspace", "promo", "store", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowSuggestedAppsInWindowsInkWorkspace")],
                DetectOps = [RegOp.CheckDword(Key, "AllowSuggestedAppsInWindowsInkWorkspace", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-dictation",
                Label = "Disable Ink Dictation Button",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets AllowWindowsInkWorkspace=0 via the AllowWindowsInkWorkspaceValue "
                    + "policy in the PenWorkspace policy key. Removes the microphone-dictation "
                    + "shortcut button from the touch keyboard and handwriting panel, preventing "
                    + "accidental activation of speech input that streams audio to the Windows "
                    + "speech recognition service. "
                    + "Default: 2 (only above lock). Recommended: 0.",
                Tags = ["pen", "dictation", "voice", "speech", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspace", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspace")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspace", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-sticky-notes-lock",
                Label = "Disable Sticky Notes on Lock Screen",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets StickyNotesOnLockScreenAllowed=0 in the PenWorkspace policy key. "
                    + "Prevents Sticky Notes from appearing on the lock screen, which would "
                    + "allow anyone near the device to view note content without authentication. "
                    + "Users who store passwords, addresses, or meeting details in Sticky Notes "
                    + "are particularly exposed by lock-screen visibility. "
                    + "Default: 1. Recommended: 0.",
                Tags = ["pen", "stickynotes", "lockscreen", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "StickyNotesOnLockScreenAllowed")],
                DetectOps = [RegOp.CheckDword(Key, "StickyNotesOnLockScreenAllowed", 0)],
            },
            new TweakDef
            {
                Id = "penws-disable-pencil-button-shortcut",
                Label = "Disable Pen Button Shortcut to Workspace",
                Category = "Display — Input Method",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets PenButtonDesiredAction=2 in the PenWorkspace policy key. Changes the "
                    + "pen barrel-button shortcut from launching Windows Ink Workspace (default) "
                    + "to a no-op action, preventing accidental Workspace activations while "
                    + "drawing in design and annotation applications. Setting value 2 disables "
                    + "the button's system action entirely, leaving it for application-defined "
                    + "handling. Default: not set. Recommended: 2 on professional artist workstations.",
                Tags = ["pen", "button", "shortcut", "workspace", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "PenButtonDesiredAction", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "PenButtonDesiredAction")],
                DetectOps = [RegOp.CheckDword(Key, "PenButtonDesiredAction", 2)],
            },
        ];
    }

    // ── PersonalizationLockPolicy ──
    private static class _PersonalizationLockPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "plock-disable-lock-screen",
                    Label = "Disable Interactive Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Removes the interactive lock screen and bypasses Cortana, search, and media controls on the lock screen. Users must enter credentials immediately. Default: enabled. Recommended: 1 (disabled) in kiosk or high-security environments.",
                    Tags = ["lock-screen", "personalization", "kiosk", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Lock screen removed; sign-in prompt shown immediately. Cortana and media controls on lock screen are unavailable.",
                    ApplyOps = [RegOp.SetDword(Key, "NoLockScreen", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoLockScreen")],
                    DetectOps = [RegOp.CheckDword(Key, "NoLockScreen", 1)],
                },
                new TweakDef
                {
                    Id = "plock-enforce-lock-screen-image",
                    Label = "Enforce Corporate Lock Screen Image",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces a specific corporate lock screen image path and prevents users from changing it. Ensures brand-consistent or security-warning lock screens. Default: not enforced. Recommended: set path in LockScreenImage value.",
                    Tags = ["lock-screen", "image", "corporate", "branding", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "All users see the same lock screen image; individual customisation is blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "LockScreenImageEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "LockScreenImageEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "LockScreenImageEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "plock-block-user-lock-screen-change",
                    Label = "Block User From Changing Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents non-admin users from changing the lock screen image or slide show. Enforces IT-managed lock screen content. Default: not controlled. Recommended: 1 in managed environments.",
                    Tags = ["lock-screen", "user-restriction", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot change lock screen via Settings; the IT-set lock screen image persists.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventChangingLockScreen", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventChangingLockScreen")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventChangingLockScreen", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-lock-screen-camera",
                    Label = "Disable Camera on Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents the camera from being activated from the lock screen without unlocking. Closes the camera-access-without-authentication attack surface. Default: 0. Recommended: 1 (disabled).",
                    Tags = ["lock-screen", "camera", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Camera cannot be accessed from lock screen; must unlock first.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenCamera", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenCamera")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenCamera", 0)],
                },
                new TweakDef
                {
                    Id = "plock-disable-lock-screen-toast",
                    Label = "Disable Toast Notifications on Lock Screen",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents toast notifications from displaying on the lock screen, hiding message previews and alert content from unauthenticated view. Default: enabled. Recommended: 1 (disabled) for data protection.",
                    Tags = ["lock-screen", "notifications", "toast", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Notification content not visible from lock screen; users must log in to see notifications.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowLockScreenToastNotifications", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowLockScreenToastNotifications")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowLockScreenToastNotifications", 0)],
                },
                new TweakDef
                {
                    Id = "plock-set-auto-slideshow",
                    Label = "Disable Lock Screen Slideshow",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the lock screen slideshow feature that cycles through user-selected photos. Enforces a static lock screen image and prevents unintended photo disclosure on unattended PCs. Default: enabled. Recommended: 1 (disabled).",
                    Tags = ["lock-screen", "slideshow", "photos", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Lock screen does not rotate photos from the user's pictures library.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableLockScreenSlideshow", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableLockScreenSlideshow")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableLockScreenSlideshow", 1)],
                },
                new TweakDef
                {
                    Id = "plock-block-desktop-theme-change",
                    Label = "Block Users From Changing Desktop Theme",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents standard users from applying custom desktop themes, wallpapers, or colour schemes. Enforces consistent corporate visual identity. Default: not controlled. Recommended: 1 in kiosk/call-centre environments.",
                    Tags = ["desktop", "theme", "personalization", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Users cannot customise wallpaper or theme via Settings; admin-set theme is enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "NoChangingTheme", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoChangingTheme")],
                    DetectOps = [RegOp.CheckDword(Key, "NoChangingTheme", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-color-change",
                    Label = "Disable User Windows Accent Colour Change",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from changing the Windows accent colour used in title bars, taskbar, and UI highlights. Enforces brand-consistent UI colouring set by IT policy. Default: not controlled. Recommended: 1 in corporate environments.",
                    Tags = ["accent-color", "personalization", "ui", "corporate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Accent colour picker in Settings is disabled; IT-defined colour is enforced.",
                    ApplyOps = [RegOp.SetDword(Key, "NoColorChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoColorChange")],
                    DetectOps = [RegOp.CheckDword(Key, "NoColorChange", 1)],
                },
                new TweakDef
                {
                    Id = "plock-disable-transparency-effects",
                    Label = "Disable Windows Transparency Effects via Policy",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the acrylic transparency effects in Windows title bars and taskbar via Group Policy. Reduces GPU compositing overhead on resource-constrained hardware. Default: enabled. Recommended: 1 (disabled) on VMs and thin clients.",
                    Tags = ["transparency", "acrylic", "performance", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Windows UI renders without blur/transparency; minor performance improvement on low-end hardware.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableTransparencyEffects", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableTransparencyEffects")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableTransparencyEffects", 1)],
                },
            ];
    }

    // ── PersonalizationPolicy ──
    private static class _PersonalizationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Personalization";
        private const string SysPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "prsnlz-disable-lock-screen-overlays",
                Label = "Disable Lock Screen App Notification Overlays",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes application notification badges (email count, calendar events, alarms) from the lock screen. Reduces information leakage to unauthenticated observers.",
                Tags = ["lock-screen", "notifications", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents notification badges from leaking information to unauthenticated observers.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LockScreenOverlaysDisabled", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LockScreenOverlaysDisabled")],
                DetectOps = [RegOp.CheckDword(Key, "LockScreenOverlaysDisabled", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-force-default-lock-screen",
                Label = "Force Default Lock Screen Image",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from customising the lock screen image. Forces the Windows default lock screen, blocking user-selected photos or Windows Spotlight images.",
                Tags = ["lock-screen", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Enforces a uniform lock screen image organisation-wide.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ForceDefaultLockScreen", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ForceDefaultLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "ForceDefaultLockScreen", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-prevent-wallpaper-change",
                Label = "Prevent Desktop Wallpaper Changes",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from changing the desktop wallpaper via Settings or Control Panel. Enforces a consistent corporate desktop appearance.",
                Tags = ["wallpaper", "personalization", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Locks desktop wallpaper to a corporate standard.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoDesktopBackground", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoDesktopBackground")],
                DetectOps = [RegOp.CheckDword(Key, "NoDesktopBackground", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-background-settings",
                Label = "Hide Background/Wallpaper Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the background/wallpaper tab from Display Properties in Control Panel, preventing users from accessing wallpaper settings.",
                Tags = ["wallpaper", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the wallpaper tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispBackgroundPage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispBackgroundPage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispBackgroundPage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-screensaver-settings",
                Label = "Hide Screensaver Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the screensaver tab from Display Properties in Control Panel, preventing users from changing screensaver settings.",
                Tags = ["screensaver", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the screensaver tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispScrSavPage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispScrSavPage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispScrSavPage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-hide-appearance-settings",
                Label = "Hide Appearance Settings Page",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes the Appearance tab from Display Properties in Control Panel, preventing users from changing colour scheme and system visual style.",
                Tags = ["appearance", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Hides the Appearance tab from Control Panel Display Properties.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoDispAppearancePage", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoDispAppearancePage")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoDispAppearancePage", 1)],
            },
            new TweakDef
            {
                Id = "prsnlz-prevent-color-change",
                Label = "Prevent System Colour Scheme Changes",
                Category = "Display — Personalization Lock",
                Description =
                    "Blocks users from changing the Windows colour scheme (accent colours, dark/light theme selection) via Settings or Control Panel.",
                Tags = ["theme", "colors", "control-panel", "policy", "personalization"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Locks the Windows colour scheme to the admin-set value.",
                RegistryKeys = [SysPolicy],
                ApplyOps = [RegOp.SetDword(SysPolicy, "NoColorChoice", 1)],
                RemoveOps = [RegOp.DeleteValue(SysPolicy, "NoColorChoice")],
                DetectOps = [RegOp.CheckDword(SysPolicy, "NoColorChoice", 1)],
            },
        ];
    }

    // ── PlayToDevicePolicy ──
    private static class _PlayToDevicePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\PlayToReceiver";
        private const string WsdKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WSD";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-receiver",
                    Label = "Play To: Disable Windows Play To Receiver Feature",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets NotAllowPlayToReceiver=1 in PlayToReceiver machine policy. Disables the Windows 'Play To' receiver capability that allows other DLNA-compatible devices on the same network to push media content to this PC for playback. "
                        + "'Play To' opens this device as a DLNA media renderer, listening for UPnP/DLNA control point commands from any device on the local network. On a corporate network, this allows any DLNA-capable device (including personal mobile phones) to push multimedia content to corporate workstations without authentication. Disabling the receiver prevents the device from accepting unsolicited media content pushes.",
                    Tags = ["dlna", "play-to", "receiver", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Disables DLNA Play To receiver; workstation cannot accept media pushes from devices on local network.",
                    ApplyOps = [RegOp.SetDword(Key, "NotAllowPlayToReceiver", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NotAllowPlayToReceiver")],
                    DetectOps = [RegOp.CheckDword(Key, "NotAllowPlayToReceiver", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-sender",
                    Label = "Play To: Disable Windows Play To Media Source Sending",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisablePlayTo=1 in PlayToReceiver machine policy. Disables the ability for users to use this PC as a 'Play To' source — sending media from Windows Media Player, Photos, or other DLNA-compatible applications to an external DLNA renderer. "
                        + "Using this PC as a 'Play To' sender connects to UPnP devices on the network and pushes streaming data to them. On a corporate network, this could be used to stream sensitive video content from a corporate machine to a personal smart TV, Chromecast, or other unmanaged renderer. Disabling 'Play To' sender functionality prevents this data exfiltration vector.",
                    Tags = ["dlna", "play-to", "sender", "media", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks DLNA media sending; screen/media content not streamable from this PC to external renderers.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePlayTo", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePlayTo")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePlayTo", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-device-discovery",
                    Label = "Play To: Block WSD (Web Services on Devices) Network Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDDiscovery=1 in WSD machine policy. Prevents the Windows WSD (Web Services on Devices) stack from announcing this device to the local network and from probing for WSD-compatible peripherals — including networked printers, scanners, and media renderers. "
                        + "WSD uses multicast UDP probes (WS-Discovery) that announce the device's presence to all LAN segments. These broadcasts leak device identity, OS version, and service capabilities to all network listeners. Disabling WSD discovery reduces the device's network footprint.",
                    Tags = ["wsd", "discovery", "network", "multicast", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote =
                        "Blocks WSD/WS-Discovery multicast probes; device not discoverable via WSD protocol. May affect network printer discovery.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDDiscovery", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDDiscovery")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDDiscovery", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-printer-discovery",
                    Label = "Play To: Block WSD-Based Printer Auto-Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDPrinting=1 in WSD machine policy. Prevents auto-discovery and installation of WSD-connected network printers. "
                        + "WSD printer discovery installs printers from the local network automatically without user approval by default on domain-joined machines. On enterprise networks with centralised print server management, rogue WSD printers could intercept print jobs if employees accidentally redirect documents to an unauthorised WSD printer device near them. Disabling WSD printer discovery enforces exclusively server-managed print queue access.",
                    Tags = ["wsd", "printer", "discovery", "print", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks WSD printer auto-install; print queues must be added manually or via GPO print server.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDPrinting", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDPrinting")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDPrinting", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-block-wsd-scanner-discovery",
                    Label = "Play To: Block WSD-Based Scanner Auto-Discovery",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableWSDScanning=1 in WSD machine policy. Prevents automatic discovery and installation of network scanners that advertise themselves via WSD/WIA (Windows Image Acquisition). "
                        + "WSD scanner discovery installs scanning drivers and opens WIA sessions to any WSD-compatible scanner found on the network. Unauthorised scanners on the network could be configured to receive forwarded scan-to-email jobs from misconfigured endpoints. Disabling WSD scanner discovery prevents unsolicited scanner driver installation and ensures scanning hardware is explicitly approved by IT.",
                    Tags = ["wsd", "scanner", "discovery", "wia", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 4,
                    ImpactNote = "Blocks WSD scanner auto-discovery; scanners require manual or GPO-managed WIA configuration.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableWSDScanning", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableWSDScanning")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableWSDScanning", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-play-to-dmr-advertisement",
                    Label = "Play To: Disable DLNA Digital Media Renderer Advertisement",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets NotAdvertisePlayToDevice=1 in PlayToReceiver machine policy. Prevents this Windows PC from advertising itself as a DLNA Digital Media Renderer (DMR) on the local network. "
                        + "DMR advertisement broadcasts multicast UPnP SSDP announcements that include the device's name, model, IP address, and capabilities to all devices on the network. This advertisement allows any DLNA control point (phone, tablet, smart TV) to discover and push media to the PC. Suppressing the advertisement hides the device from DLNA discovery without fully disabling the network stack services.",
                    Tags = ["dlna", "dmr", "advertisement", "ssdp", "privacy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses DLNA DMR advertisement; PC not visible to DLNA control points on the network.",
                    ApplyOps = [RegOp.SetDword(Key, "NotAdvertisePlayToDevice", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NotAdvertisePlayToDevice")],
                    DetectOps = [RegOp.CheckDword(Key, "NotAdvertisePlayToDevice", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-media-sharing-network-access",
                    Label = "Play To: Disable Media Library Network Sharing",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableMediaSharing=1 in PlayToReceiver machine policy. Prevents this PC from sharing its media library (pictures, videos, music) with other devices on the network via the Windows Media Player network sharing service. "
                        + "Media library sharing exposes the contents of the user's Documents, Pictures, and Music folders to any UPnP/DLNA media renderer on the local network without per-file access controls. On corporate networks, user Documents folders frequently contain sensitive files that the file-sharing component includes in its media index.",
                    Tags = ["media", "sharing", "library", "network", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Blocks media library network sharing; document folders not exposed via DLNA/UPnP media server.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableMediaSharing", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaSharing")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableMediaSharing", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-restrict-play-to-local-subnet-only",
                    Label = "Play To: Restrict Play To and DLNA to Local Subnet Only",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets AllowedNetworkScopes=1 in PlayToReceiver machine policy. Restricts the DLNA/Play To feature to the local subnet only, preventing cross-subnet media streaming and rendering. "
                        + "Limiting Play To to the local subnet ensures that media streaming sessions cannot traverse network routers into other VLANs or the wide internet. This is the least-restrictive enterprise hardening option for organisations where DLNA is permitted for AV room systems on a dedicated VLAN but must not be accessible from corporate workstation VLANs.",
                    Tags = ["dlna", "subnet", "scope", "network-segmentation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "DLNA scoped to local subnet only; cross-VLAN and internet-routed media streaming blocked.",
                    ApplyOps = [RegOp.SetDword(Key, "AllowedNetworkScopes", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "AllowedNetworkScopes")],
                    DetectOps = [RegOp.CheckDword(Key, "AllowedNetworkScopes", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-device-play-auto-start",
                    Label = "Play To: Disable Auto-Start of Play To Service at Logon",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableAutoStart=1 in PlayToReceiver machine policy. Prevents the Windows Play To Receiver service from starting automatically at user logon. "
                        + "The Play To receiver service starts in the background and maintains a network listener even when neither party has initiated a media session. This background process consumes memory, CPU, and network port capacity. Disabling auto-start ensures the service only runs when explicitly invoked, reducing the device's idle network service footprint.",
                    Tags = ["dlna", "service", "auto-start", "startup", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Play To service not auto-started; listener not running unless explicitly launched.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableAutoStart", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableAutoStart")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableAutoStart", 1)],
                },
                new TweakDef
                {
                    Id = "playtodev-disable-wsd-host-discovery",
                    Label = "Play To: Disable WSD Function Discovery Host (FDHOST) Network Broadcast",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets DisableFunctionDiscoveryHostBroadcast=1 in WSD machine policy. Prevents the Function Discovery Host service from broadcasting WSD host announcements that advertise this machine's services (web services, UPnP capabilities) to other devices on the network. "
                        + "Function Discovery is the mechanism Windows uses to populate the Network window in Explorer. Broadcasting the host's function discovery metadata exposes its installed services and capabilities to all LAN listeners. On hardened workstations, eliminating unnecessary network announcements reduces the device's identifiable surface in passive network reconnaissance.",
                    Tags = ["wsd", "fdhost", "broadcast", "discovery", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Suppresses WSD function discovery host broadcasts; device less identifiable via passive LAN scanning.",
                    ApplyOps = [RegOp.SetDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
                    RemoveOps = [RegOp.DeleteValue(WsdKey, "DisableFunctionDiscoveryHostBroadcast")],
                    DetectOps = [RegOp.CheckDword(WsdKey, "DisableFunctionDiscoveryHostBroadcast", 1)],
                },
            ];
    }

    // ── ScreenSaverSecurityPolicy ──
    private static class _ScreenSaverSecurityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ScreenSaver";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "scrsvr-enable-screensaver",
                    Label = "Enforce Screen Saver Activation",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Mandates the screen saver is enabled for all users, ensuring the screen locks after inactivity. Prevents unattended desktop access. Default: not enforced. Recommended: 1 in all managed environments.",
                    Tags = ["screensaver", "lock", "session", "timeout", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver activates after the configured timeout; users cannot disable via Settings.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaverEnabled", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaverEnabled")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaverEnabled", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-require-password-resume",
                    Label = "Require Password on Screen Saver Resume",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces Windows to prompt for a password when resuming from the screen saver. Prevents access to an unattended unlocked session. Default: disabled. Recommended: 1 (enabled) for compliance.",
                    Tags = ["screensaver", "password", "security", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 5,
                    SafetyRating = 5,
                    ImpactNote = "Critical access control: an unattended screen always requires re-authentication.",
                    ApplyOps = [RegOp.SetDword(Key, "PasswordProtect", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PasswordProtect")],
                    DetectOps = [RegOp.CheckDword(Key, "PasswordProtect", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-set-timeout-seconds",
                    Label = "Set Screen Saver Inactivity Timeout (600s)",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets the screen saver activation delay to 600 seconds (10 minutes) of inactivity. Balances user productivity against security for typical office environments. Default: not set. Recommended: 600.",
                    Tags = ["screensaver", "timeout", "inactivity", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver activates after 10 minutes of inactivity; adjust timeout per risk posture.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaveTimeOut", 600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaveTimeOut", 600)],
                },
                new TweakDef
                {
                    Id = "scrsvr-block-user-timeout-change",
                    Label = "Block Users From Changing Screen Saver Timeout",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from modifying the screen saver wait time in Display Properties. Ensures the IT-mandated timeout is respected. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "timeout", "user-restriction", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver timeout control is greyed out in user Settings.",
                    ApplyOps = [RegOp.SetDword(Key, "NoDispScrSavPage", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoDispScrSavPage")],
                    DetectOps = [RegOp.CheckDword(Key, "NoDispScrSavPage", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-block-user-ss-change",
                    Label = "Block Users From Changing Screen Saver",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from selecting a different screen saver. The IT-assigned screen saver (blank or corporate-branded) remains fixed. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "user-restriction", "personalization", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver selection list is hidden; the configured screen saver is fixed.",
                    ApplyOps = [RegOp.SetDword(Key, "NoScreenSaverChange", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoScreenSaverChange")],
                    DetectOps = [RegOp.CheckDword(Key, "NoScreenSaverChange", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-use-blank-ss",
                    Label = "Force Blank (Black) Screen Saver",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Forces the blank/black screen saver as the mandatory screen saver. Avoids animation CPU overhead and prevents screen burn-in from animated screensavers. Default: user-selected. Recommended: scrnsave.scr (blank).",
                    Tags = ["screensaver", "blank", "performance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver is blank (black); no CPU/GPU cycles used animating screensaver graphics.",
                    ApplyOps = [RegOp.SetDword(Key, "UseBlankScreenSaver", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "UseBlankScreenSaver")],
                    DetectOps = [RegOp.CheckDword(Key, "UseBlankScreenSaver", 1)],
                },
                new TweakDef
                {
                    Id = "scrsvr-disable-user-password-change",
                    Label = "Block Users From Disabling SS Password Requirement",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Prevents users from unchecking the 'On resume, display logon screen' option in screen saver settings. Ensures password-on-resume cannot be silently disabled. Default: not controlled. Recommended: 1.",
                    Tags = ["screensaver", "password", "user-restriction", "compliance", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "The password-on-resume checkbox in Screen Saver Settings is greyed out and locked enabled.",
                    ApplyOps = [RegOp.SetDword(Key, "NoPasswordOnResume", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "NoPasswordOnResume")],
                    DetectOps = [RegOp.CheckDword(Key, "NoPasswordOnResume", 0)],
                },
                new TweakDef
                {
                    Id = "scrsvr-min-screen-timeout-30s",
                    Label = "Enforce Minimum 30-Second Screen Saver Wait",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets a minimum screen saver activation delay of 30 seconds, preventing users from setting it too low (causing frequent screen lock during active use). Default: not set. Recommended: 30.",
                    Tags = ["screensaver", "timeout", "minimum", "usability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 1,
                    SafetyRating = 5,
                    ImpactNote = "Screen saver will not activate in less than 30 seconds; prevents productivity-breaking too-aggressive locking.",
                    ApplyOps = [RegOp.SetDword(Key, "MinScreenSaveTimeOut", 30)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MinScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "MinScreenSaveTimeOut", 30)],
                },
                new TweakDef
                {
                    Id = "scrsvr-max-inactivity-3600s",
                    Label = "Enforce Maximum 3600-Second Inactivity Limit",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Caps the maximum screen saver inactivity wait to 3600 seconds (1 hour). Prevents users from setting an excessively long timeout that would leave unattended sessions unlocked. Default: not set. Recommended: 3600.",
                    Tags = ["screensaver", "timeout", "maximum", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Unattended sessions cannot remain unlocked for more than 1 hour regardless of user setting.",
                    ApplyOps = [RegOp.SetDword(Key, "MaxScreenSaveTimeOut", 3600)],
                    RemoveOps = [RegOp.DeleteValue(Key, "MaxScreenSaveTimeOut")],
                    DetectOps = [RegOp.CheckDword(Key, "MaxScreenSaveTimeOut", 3600)],
                },
                new TweakDef
                {
                    Id = "scrsvr-grace-period-zero",
                    Label = "Set Screen Saver Lock Grace Period to Zero",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Sets the grace period (seconds after screen saver starts before lock is enforced) to 0. Ensures immediate lock without a bypass window. Default: 5. Recommended: 0.",
                    Tags = ["screensaver", "grace-period", "lock", "immediate", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Moving the mouse immediately after screen saver starts will require re-authentication; no grace bypass window.",
                    ApplyOps = [RegOp.SetDword(Key, "ScreenSaverGracePeriod", 0)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ScreenSaverGracePeriod")],
                    DetectOps = [RegOp.CheckDword(Key, "ScreenSaverGracePeriod", 0)],
                },
            ];
    }

    // ── SharedClipboardControlPolicy ──
    private static class _SharedClipboardControlPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\System";
        private const string Key2 = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\CloudContent";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "shrdclip-disable-phone-link",
                    Label = "Disable Phone Link Clipboard Share",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables clipboard sharing between Windows and a linked Android/iOS phone via the Phone Link (Your Phone) app, preventing cross-device clipboard leakage.",
                    Tags = ["clipboard", "phone-link", "sharing", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Phone Link clipboard sync disabled; clipboard data stays on the PC.",
                    ApplyOps = [RegOp.SetDword(Key, "DisablePhoneLinkClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisablePhoneLinkClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "DisablePhoneLinkClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-sync-across-devices",
                    Label = "Disable Clipboard Sync Across Devices",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables device-to-device clipboard synchronization at the policy level, complementing AllowCrossDeviceClipboard by blocking back-end sync service.",
                    Tags = ["clipboard", "sync", "devices", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cross-device clipboard sync blocked at policy level.",
                    ApplyOps = [RegOp.SetDword(Key, "ClipboardSyncBlock", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ClipboardSyncBlock")],
                    DetectOps = [RegOp.CheckDword(Key, "ClipboardSyncBlock", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-cloud-clipboard",
                    Label = "Disable Cloud Clipboard Sync",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables cloud clipboard synchronization feature that uploads clipboard contents to Microsoft's cloud for cross-device access.",
                    Tags = ["clipboard", "cloud", "sync", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Cloud clipboard disabled; clipboard items not uploaded to Microsoft cloud.",
                    ApplyOps = [RegOp.SetDword(Key, "DisableCloudClipboardContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudClipboardContent")],
                    DetectOps = [RegOp.CheckDword(Key, "DisableCloudClipboardContent", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-tooltip-ads",
                    Label = "Disable Clipboard History Tooltip Ads",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Hides promotional tooltips and advertisements shown in the clipboard history panel (Win+V) that encourage enabling cloud clipboard or other Microsoft services.",
                    Tags = ["clipboard", "tooltip", "ads", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Removes clipboard promotional tooltips; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key, "HideClipboardTooltips", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "HideClipboardTooltips")],
                    DetectOps = [RegOp.CheckDword(Key, "HideClipboardTooltips", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-block-microsoft-apps",
                    Label = "Block Clipboard Access by Microsoft Apps",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Blocks Microsoft first-party applications from accessing the clipboard history API, reducing telemetry and data collection from clipboard contents.",
                    Tags = ["clipboard", "microsoft-apps", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Microsoft apps clipboard access restricted; some features may degrade.",
                    ApplyOps = [RegOp.SetDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "BlockMicrosoftApplicationsClipboard")],
                    DetectOps = [RegOp.CheckDword(Key, "BlockMicrosoftApplicationsClipboard", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-contextual-suggestions",
                    Label = "Disable Contextual Suggestions in Clipboard",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables contextual content suggestions (e.g., smart replies, URL previews) that appear in the clipboard history panel based on clipboard content analysis.",
                    Tags = ["clipboard", "suggestions", "privacy", "cloud-content", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard content not analysed for suggestions; fully private.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardContextSuggestions", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardContextSuggestions")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardContextSuggestions", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-telemetry",
                    Label = "Disable Clipboard Telemetry",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables telemetry data collection about clipboard usage patterns, preventing clipboard interaction metadata from being sent to Microsoft.",
                    Tags = ["clipboard", "telemetry", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard usage telemetry stopped; no functional impact.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableClipboardDiagnosticData", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableClipboardDiagnosticData")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableClipboardDiagnosticData", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-block-sensitive-content-detection",
                    Label = "Block Sensitive Clipboard Content Detection",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables content scanning of clipboard items for sensitive data classification (DLP-style detection) by cloud-connected services.",
                    Tags = ["clipboard", "sensitive", "dlp", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Clipboard content not scanned for sensitive data by cloud services.",
                    ApplyOps = [RegOp.SetDword(Key2, "BlockClipboardSensitiveContent", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "BlockClipboardSensitiveContent")],
                    DetectOps = [RegOp.CheckDword(Key2, "BlockClipboardSensitiveContent", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-disable-uwp-clipboard-api",
                    Label = "Disable Clipboard API for UWP Apps",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Disables the WinRT clipboard API access for Universal Windows Platform (UWP) apps, preventing packaged apps from silently reading or writing the clipboard.",
                    Tags = ["clipboard", "uwp", "api", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "UWP apps cannot access clipboard API; clipboard-dependent UWP features break.",
                    ApplyOps = [RegOp.SetDword(Key2, "DisableUwpClipboardAPI", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "DisableUwpClipboardAPI")],
                    DetectOps = [RegOp.CheckDword(Key2, "DisableUwpClipboardAPI", 1)],
                },
                new TweakDef
                {
                    Id = "shrdclip-restrict-same-process-paste",
                    Label = "Restrict Clipboard Paste to Same Process",
                    Category = "Display — Personalization Lock",
                    Description =
                        "Restricts the clipboard paste operation so that clipboard data can only be pasted within the same process that originally wrote it, preventing cross-process data leakage via clipboard.",
                    Tags = ["clipboard", "paste", "isolation", "security", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 3,
                    ImpactNote = "Cross-process paste restricted; applications that rely on clipboard sharing between processes will break.",
                    ApplyOps = [RegOp.SetDword(Key2, "RestrictSameProcessClipboard", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key2, "RestrictSameProcessClipboard")],
                    DetectOps = [RegOp.CheckDword(Key2, "RestrictSameProcessClipboard", 1)],
                },
            ];
    }

    // ── ShellRestrictionsPolicy ──
    private static class _ShellRestrictionsPolicy
    {
        private const string Pol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shellrst-no-find-command",
                Label = "Remove Find/Search from Start Menu",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoFind=1 in Policies\\Explorer. Removes the Find/Search shortcut and menu item from the Start menu. Prevents quick enumeration of file system contents via the built-in search dialog. Default: Find is visible.",
                Tags = ["shell", "restriction", "search", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoFind", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoFind")],
                DetectOps = [RegOp.CheckDword(Pol, "NoFind", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-logoff-menu",
                Label = "Remove Log Off from Start Menu",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoLogoff=1 in Policies\\Explorer. Removes the Log Off entry from the Start menu, preventing users from signing out via the Start menu shortcut. Session management must be performed through other means (Task Manager, CTRL+ALT+DEL).",
                Tags = ["shell", "restriction", "logoff", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoLogoff", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoLogoff")],
                DetectOps = [RegOp.CheckDword(Pol, "NoLogoff", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-desktop-icons",
                Label = "Hide All Desktop Icons",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDesktop=1 in Policies\\Explorer. Removes all icons from the desktop surface including This PC, Recycle Bin, and user-placed shortcuts. Desktop background is still visible. Used to create clean-slate kiosk or thin-client desktops.",
                Tags = ["shell", "restriction", "desktop", "policy", "kiosk"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDesktop", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDesktop")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDesktop", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-drives-page",
                Label = "Remove Drives Tab from Computer Properties",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDrivesPage=1 in Policies\\Explorer. Removes the Drives tab from the Hardware and Storage area in System Properties, preventing detailed enumeration of physical drive properties. Reduces information leakage on shared systems.",
                Tags = ["shell", "restriction", "drives", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDrivesPage", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDrivesPage")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDrivesPage", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-control-panel-applets",
                Label = "Block All Control Panel Applets",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoCplApplets=1 in Policies\\Explorer. Prevents all Control Panel .cpl applets from launching, including Display, Sound, Network, System, etc. Combined with the GPO applet allow-list this creates a restricted-access Control Panel for standard users.",
                Tags = ["shell", "restriction", "controlpanel", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoCplApplets", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoCplApplets")],
                DetectOps = [RegOp.CheckDword(Pol, "NoCplApplets", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-display-cpl",
                Label = "Hide Display Control Panel Applet",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoDispCPL=1 in Policies\\Explorer. Prevents users from opening the Display applet from Control Panel or the desktop right-click menu, blocking wallpaper, resolution, and colour depth changes. Used to enforce corporate visual standards.",
                Tags = ["shell", "restriction", "display", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoDispCPL", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoDispCPL")],
                DetectOps = [RegOp.CheckDword(Pol, "NoDispCPL", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-restrict-run-list",
                Label = "Enable DisallowRun Application Restriction",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets DisallowRun=1 in Policies\\Explorer. Activates the DisallowRun enforcement mode, which blocks execution of any application names listed under the adjacent DisallowRun sub-key. Enables per-application deny-listing without requiring AppLocker or WDAC.",
                Tags = ["shell", "restriction", "allowlist", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "DisallowRun", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "DisallowRun")],
                DetectOps = [RegOp.CheckDword(Pol, "DisallowRun", 1)],
            },
            new TweakDef
            {
                Id = "shellrst-no-network-neighborhood",
                Label = "Hide Network Neighborhood from Explorer",
                Category = "Display — Personalization Lock",
                Description =
                    "Sets NoNetHood=1 in Policies\\Explorer. Removes the Network Neighborhood (Network Places) icon from Explorer and the desktop. Users can still access UNC paths directly; this only removes the browsable discovery pane that enumerates visible network shares.",
                Tags = ["shell", "restriction", "network", "policy", "gpo"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(Pol, "NoNetHood", 1)],
                RemoveOps = [RegOp.DeleteValue(Pol, "NoNetHood")],
                DetectOps = [RegOp.CheckDword(Pol, "NoNetHood", 1)],
            },
        ];
    }

    // ── ShutdownOptionsPolicy ──
    private static class _ShutdownOptionsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ShutdownOptions";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shtdwn-disable-shutdown-on-ctrl-alt-del",
                Label = "Disable Shutdown from Ctrl+Alt+Del Screen",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets NoShutdownOnCtrlAltDel=1 in the ShutdownOptions policy key. "
                    + "Removes the Shut down button from the Ctrl+Alt+Del secure attention "
                    + "sequence screen, preventing non-admin users from shutting down or "
                    + "restarting the machine without appropriate privilege. On shared "
                    + "workstations and call-centre desktops accidental shutdown of "
                    + "production machines causes service disruption. Default: 0.",
                Tags = ["shutdown", "ctrl-alt-del", "security", "lockdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoShutdownOnCtrlAltDel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoShutdownOnCtrlAltDel")],
                DetectOps = [RegOp.CheckDword(Key, "NoShutdownOnCtrlAltDel", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-require-shutdown-reason",
                Label = "Require Shutdown Reason and Comment",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets ShutdownReasonOn=1 in the ShutdownOptions policy key. Forces the "
                    + "shutdown dialog to display a reason code drop-down and optional "
                    + "comment field before accepting a restart or shutdown command. "
                    + "Mandatory shutdown reason codes create an audit trail of who shut "
                    + "down a system and why, which is valuable in production server "
                    + "environments and shared workstations. Default: 0.",
                Tags = ["shutdown", "reason", "audit", "compliance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "ShutdownReasonOn", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "ShutdownReasonOn")],
                DetectOps = [RegOp.CheckDword(Key, "ShutdownReasonOn", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-restart-apps",
                Label = "Disable App Restart After Reboot",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAppRestart=1 in the ShutdownOptions policy key. Prevents "
                    + "Windows from re-launching applications registered in the RunOnce "
                    + "restart list after a reboot. Some application installers and updaters "
                    + "use the restart application list to auto-launch their product after "
                    + "a reboot; disabling this keeps the post-reboot session clean and "
                    + "consistent in enterprise imaging workflows. Default: 0.",
                Tags = ["shutdown", "restart", "apps", "runonce", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAppRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRestart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAppRestart", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-automatic-restart",
                Label = "Disable Automatic Restart After BSOD",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Sets DisableAutomaticRestart=1 in the ShutdownOptions policy key. "
                    + "Prevents the system from automatically rebooting after a fatal Stop "
                    + "error (Blue Screen of Death). Automatic restart hides the bugcheck "
                    + "code and error details from the user before they can note the stop "
                    + "code; disabling it allows engineers to read and photograph the BSOD "
                    + "screen and correlate it with kernel dump analysis. Default: 0.",
                Tags = ["shutdown", "bsod", "crash", "restart", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAutomaticRestart", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAutomaticRestart")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAutomaticRestart", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-legacy-logoff-script",
                Label = "Disable Legacy Logoff Script Delay",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets MaxWaitForScriptDelay=0 in the ShutdownOptions policy key. Sets "
                    + "the maximum time the system will wait for legacy Group Policy logoff "
                    + "or shutdown scripts to complete before forcing termination to 0 "
                    + "seconds. Long-running logoff scripts delay shutdown chains in VDI "
                    + "environments and can prevent clean hyper-visor-level snapshotting "
                    + "during overnight maintenance windows. Default: 600.",
                Tags = ["shutdown", "script", "logoff", "delay", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "MaxWaitForScriptDelay", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "MaxWaitForScriptDelay")],
                DetectOps = [RegOp.CheckDword(Key, "MaxWaitForScriptDelay", 0)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-forced-reboot-notification",
                Label = "Disable Forced Reboot Notification Banner",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableForcedRebootNotification=1 in the ShutdownOptions policy "
                    + "key. Suppresses the notification banner that warns users of an "
                    + "imminent forced restart scheduled by Windows Update or administrator "
                    + "policy. While intended to inform users, in unattended VDI and "
                    + "server contexts the notification triggers user-interactive dialogs "
                    + "that block automated shutdown orchestration scripts. Default: 0.",
                Tags = ["shutdown", "reboot", "notification", "wus", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableForcedRebootNotification", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableForcedRebootNotification")],
                DetectOps = [RegOp.CheckDword(Key, "DisableForcedRebootNotification", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-power-button-action",
                Label = "Disable Power Button Shutdown",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisablePowerButton=1 in the ShutdownOptions policy key. Prevents "
                    + "the physical power button from triggering a shutdown or hibernate "
                    + "action, regardless of the power plan's button-press setting. On "
                    + "point-of-sale terminals, kiosk devices, and embedded panels the "
                    + "power button is often inadvertently pressed during normal operation "
                    + "causing unexpected downtime. Default: 0.",
                Tags = ["shutdown", "power-button", "kiosk", "hardware", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisablePowerButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisablePowerButton")],
                DetectOps = [RegOp.CheckDword(Key, "DisablePowerButton", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-restart-button-start",
                Label = "Disable Restart Option in Start Menu",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets NoRestartFromStartMenu=1 in the ShutdownOptions policy key. "
                    + "Removes the Restart option from the Start Menu power button flyout, "
                    + "preventing standard users from restarting the system from the desktop. "
                    + "On thin-client terminals and locked-down workstations, restart should "
                    + "only be initiated by IT administrators through remote management "
                    + "tools or scheduled maintenance windows. Default: 0.",
                Tags = ["shutdown", "start-menu", "restart", "lockdown", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "NoRestartFromStartMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "NoRestartFromStartMenu")],
                DetectOps = [RegOp.CheckDword(Key, "NoRestartFromStartMenu", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-disable-hibernate-option",
                Label = "Disable Hibernate Option in Shutdown Menu",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableHibernate=1 in the ShutdownOptions policy key. Removes "
                    + "the Hibernate entry from the shutdown and power flyout menus. "
                    + "Hibernate writes the full memory contents to the hiberfil.sys "
                    + "pagefile, which may contain credentials, encryption keys, and "
                    + "sensitive process data as unencrypted pages unless UEFI provides "
                    + "a sealed hibernation key. Default: 0. Recommended: 1 on shared machines.",
                Tags = ["shutdown", "hibernate", "security", "power", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHibernate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHibernate")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHibernate", 1)],
            },
            new TweakDef
            {
                Id = "shtdwn-log-shutdown-events",
                Label = "Enable Shutdown Event Logging",
                Category = "Display — Personalization Lock",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets LogShutdownEvents=1 in the ShutdownOptions policy key. Enables "
                    + "recording of shutdown, restart, and logoff events with user identity, "
                    + "timestamp, reason code, and any administrator comment to the "
                    + "Security event log. Event log evidence of shutdown sequences is "
                    + "critical for forensic timelines in incident response and for "
                    + "demonstrating change-control compliance in audits. Default: 0.",
                Tags = ["shutdown", "logging", "audit", "event-log", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "LogShutdownEvents", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "LogShutdownEvents")],
                DetectOps = [RegOp.CheckDword(Key, "LogShutdownEvents", 1)],
            },
        ];
    }

    // ── SidebarGadgetsPolicy ──
    private static class _SidebarGadgetsPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sidebar";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "sidebar-turn-off-sidebar",
                Label = "Sidebar Policy: Turn Off Windows Sidebar",
                Category = "Display — Personalization Lock",
                Description =
                    "Disables the Windows Sidebar and all desktop gadgets via Group Policy. Prevents users from running the sidebar process (Sidebar.exe) on Windows Vista/7/8 and legacy gadgets on Windows 10/11.",
                Tags = ["sidebar", "gadgets", "legacy", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Disables legacy sidebar process; removes gadget execution surface.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffSidebar", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffSidebar")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffSidebar", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-block-unsupported-packages",
                Label = "Sidebar Policy: Block Unsupported Gadget Packages",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents execution of gadget packages that are not explicitly supported by the installed Windows version. Unsupported gadget packages can contain vulnerabilities or unsigned code.",
                Tags = ["sidebar", "gadgets", "packages", "block", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents unsigned/unsupported gadget packages from running.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffUnsupportedPackages", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffUnsupportedPackages")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffUnsupportedPackages", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-user-gadgets",
                Label = "Sidebar Policy: Disable Per-User Gadget Execution",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents individual users from installing and running desktop gadgets. Removes gadgets from the right-click desktop context menu and disables the gadget installation dialog.",
                Tags = ["sidebar", "gadgets", "user", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes per-user gadget installation from the desktop context menu.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableUserGadgets", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableUserGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "DisableUserGadgets", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-auto-update",
                Label = "Sidebar Policy: Disable Gadget Metadata Auto-Update",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents Windows gadgets from automatically downloading updated metadata from the Windows Live Gallery or third-party gadget feeds. Reduces network activity and potential data exfiltration.",
                Tags = ["sidebar", "gadgets", "auto-update", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents gadgets from downloading live content updates.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "TurnOffAutoUpdate", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "TurnOffAutoUpdate")],
                DetectOps = [RegOp.CheckDword(Key, "TurnOffAutoUpdate", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-block-from-running",
                Label = "Sidebar Policy: Block Sidebar Process from Running",
                Category = "Display — Personalization Lock",
                Description =
                    "Blocks the Windows Sidebar process (sidebar.exe) from launching. CVE-2013-0088 and other CVEs affect Windows gadgets — disabling the process is a defence-in-depth mitigation.",
                Tags = ["sidebar", "gadgets", "block", "process", "cve", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Mitigates CVE-2013-0088 and related gadget CVEs by blocking sidebar.exe execution.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockFromRunning", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockFromRunning")],
                DetectOps = [RegOp.CheckDword(Key, "BlockFromRunning", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-require-signed-packages",
                Label = "Sidebar Policy: Require Signed Gadget Packages",
                Category = "Display — Personalization Lock",
                Description =
                    "Enforces digital signature verification for all gadget packages before execution. Prevents loading of unsigned or tampered gadgets that could execute arbitrary code.",
                Tags = ["sidebar", "gadgets", "signatures", "signing", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Enforces digital signature verification before any gadget executes.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireGadgetSignatures", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireGadgetSignatures")],
                DetectOps = [RegOp.CheckDword(Key, "RequireGadgetSignatures", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-web-gadgets",
                Label = "Sidebar Policy: Disable Internet-Connected Gadgets",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents gadgets from connecting to the internet to fetch live content (weather, news, finance widgets). Eliminates a data exfiltration channel and mitigates web content injection risks.",
                Tags = ["sidebar", "gadgets", "web", "internet", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Blocks gadgets from fetching live internet content; eliminates exfiltration channel.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableOnlineGadgetContent", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableOnlineGadgetContent")],
                DetectOps = [RegOp.CheckDword(Key, "DisableOnlineGadgetContent", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-desktop-gadgets",
                Label = "Sidebar Policy: Disable Desktop Gadgets",
                Category = "Display — Personalization Lock",
                Description =
                    "Removes desktop gadget functionality entirely, including the right-click 'Gadgets' menu entry on the desktop. Enforces a clean desktop policy on managed enterprise endpoints.",
                Tags = ["sidebar", "gadgets", "desktop", "disable", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes desktop gadget functionality and context menu entry.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableDesktopGadgets", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableDesktopGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "DisableDesktopGadgets", 1)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-third-party-gadgets",
                Label = "Sidebar Policy: Disable Third-Party Gadget Installation",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents users from installing gadgets from third-party sources or URLs. Restricts gadget sources to the built-in Windows Gallery only, reducing the attack surface via malicious gadget packages.",
                Tags = ["sidebar", "gadgets", "third-party", "installation", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Restricts gadget sources to Windows Gallery only; blocks third-party malicious packages.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowThirdPartyGadgets", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowThirdPartyGadgets")],
                DetectOps = [RegOp.CheckDword(Key, "AllowThirdPartyGadgets", 0)],
            },
            new TweakDef
            {
                Id = "sidebar-disable-gadget-gallery",
                Label = "Sidebar Policy: Disable Gadget Gallery Access",
                Category = "Display — Personalization Lock",
                Description =
                    "Prevents access to the Windows Gadget Gallery (the built-in gadget browser). Removes the ability to browse and install new gadgets from both the OS gallery and online sources.",
                Tags = ["sidebar", "gadgets", "gallery", "lockdown", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents browsing and installing new gadgets from OS gallery and online sources.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGadgetGallery", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGadgetGallery")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGadgetGallery", 1)],
            },
        ];
    }

    // ── StartMenuModernPolicy ──
    private static class _StartMenuModernPolicy
    {
        private const string ExplPol = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Explorer";

        private const string SmExp = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\StartMenuExperience";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "smmod-disable-recent-apps-in-start",
                Label = "Start Menu: Disable recently added apps list in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecentAppsInStart=1 in StartMenuExperience policy. Hides the 'Recently "
                    + "added' section at the top of the Start menu application list.",
                Tags = ["start-menu", "recent-apps", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecentAppsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecentAppsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecentAppsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-frequently-used-programs",
                Label = "Start Menu: Disable frequently used programs list",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoFrequentUsedPrograms=1 in Explorer policy. Prevents Windows from tracking and "
                    + "displaying the most frequently launched applications in the Start menu.",
                Tags = ["start-menu", "frequent-apps", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoFrequentUsedPrograms", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoFrequentUsedPrograms")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoFrequentUsedPrograms", 1)],
            },
            new TweakDef
            {
                Id = "smmod-hide-people-bar",
                Label = "Taskbar: Hide the People bar (contacts flyout)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets HidePeopleBar=1 in StartMenuExperience policy. Removes the People button from "
                    + "the taskbar, hiding the contacts/people flyout feature.",
                Tags = ["taskbar", "people", "contacts", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "HidePeopleBar", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "HidePeopleBar")],
                DetectOps = [RegOp.CheckDword(SmExp, "HidePeopleBar", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-recent-docs",
                Label = "Start Menu: Disable recent documents history in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoRecentDocsMenu=1 in Explorer policy. Stops Windows from tracking and showing "
                    + "a recent-documents shortcut list in the Start menu and jump lists.",
                Tags = ["start-menu", "recent-docs", "privacy", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoRecentDocsMenu", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoRecentDocsMenu")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoRecentDocsMenu", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-app-suggestions",
                Label = "Start Menu: Disable app suggestions / promoted apps in Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecommendedAppsInStart=1 in StartMenuExperience policy. Removes "
                    + "Microsoft-promoted app suggestions and advertisements from the Start menu.",
                Tags = ["start-menu", "suggestions", "ads", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedAppsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedAppsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedAppsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-start-recommended-section",
                Label = "Start Menu: Disable the Recommended section in Windows 11 Start",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableRecommendedItemsInStart=1 in StartMenuExperience policy. Hides the "
                    + "'Recommended' tile area at the bottom of the Windows 11 Start menu.",
                Tags = ["start-menu", "recommended", "w11", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableRecommendedItemsInStart", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableRecommendedItemsInStart")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableRecommendedItemsInStart", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-preview-pane",
                Label = "Explorer: Disable the Preview Pane in File Explorer",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoPreviewPane=1 in Explorer policy. Disables the Preview Pane panel that "
                    + "renders a file preview on the right side of File Explorer windows.",
                Tags = ["explorer", "preview-pane", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoPreviewPane", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoPreviewPane")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoPreviewPane", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-details-pane",
                Label = "Explorer: Disable the Details Pane in File Explorer",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoDetailsPane=1 in Explorer policy. Removes the Details Pane that displays "
                    + "file metadata (size, dates, author) on the right side of File Explorer.",
                Tags = ["explorer", "details-pane", "ui", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoDetailsPane", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoDetailsPane")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoDetailsPane", 1)],
            },
            new TweakDef
            {
                Id = "smmod-disable-taskbar-msa-notification",
                Label = "Taskbar: Disable MSA sign-in notification badge on taskbar",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets DisableMSANotification=1 in StartMenuExperience policy. Suppresses the "
                    + "notification badge that prompts users to sign in with a Microsoft Account.",
                Tags = ["taskbar", "msa", "notification", "debloat", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(SmExp, "DisableMSANotification", 1)],
                RemoveOps = [RegOp.DeleteValue(SmExp, "DisableMSANotification")],
                DetectOps = [RegOp.CheckDword(SmExp, "DisableMSANotification", 1)],
            },
            new TweakDef
            {
                Id = "smmod-no-machine-boot-uninstall",
                Label = "Start Menu: Preserve pinned items across machine boot (no uninstall prompt)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Sets NoMachineBootUninstall=1 in Explorer policy. Prevents Windows from prompting to "
                    + "remove pinned Start menu items for apps that were uninstalled on another user profile.",
                Tags = ["start-menu", "pin", "uninstall", "boot", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(ExplPol, "NoMachineBootUninstall", 1)],
                RemoveOps = [RegOp.DeleteValue(ExplPol, "NoMachineBootUninstall")],
                DetectOps = [RegOp.CheckDword(ExplPol, "NoMachineBootUninstall", 1)],
            },
        ];
    }

    // ── SudoWindowsPolicy ──
    private static class _SudoWindowsPolicy
    {
        private const string SudoKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sudo";
        private const string ElevationConfigKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ElevationConfig";
        private const string UacPoliciesKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\UAC";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "sudopol-disable-sudo",
                    Label = "Disable Sudo for Windows",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents users from using the 'sudo' command in Windows to run programs with elevated privileges from a standard terminal. Enforces traditional UAC elevation only.",
                    Tags = ["sudo", "elevation", "uac", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents privilege escalation via sudo from standard terminals; users must use dedicated elevated sessions.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "Enabled", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "Enabled")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "Enabled", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-force-new-window",
                    Label = "Force Sudo to Open New Window",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "When sudo is permitted, forces elevated processes to launch in a new, separate console window rather than running inline in the calling shell. Increases visibility of elevated sessions.",
                    Tags = ["sudo", "elevation", "new-window", "uac", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Elevated process runs in a clearly separate window, reducing confusion about which shell context is privileged.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "SudoMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "SudoMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "SudoMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-disable-inline-mode",
                    Label = "Disable Sudo Inline Execution Mode",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the 'inline' execution mode of sudo where the elevated process shares the calling terminal session. Inline mode can mask privilege escalation; this policy requires isolated execution.",
                    Tags = ["sudo", "elevation", "inline-mode", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Inline sudo blurs the boundary between privileged and non-privileged sessions; disabling it is recommended for corporate environments.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "AllowInlineMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInlineMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "AllowInlineMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-disable-input-disabled-mode",
                    Label = "Disable Sudo Input-Disabled Mode",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the 'input disabled' mode of sudo, which runs an elevated process with stdin closed. This mode is useful for non-interactive elevated scripts but may bypass certain security controls.",
                    Tags = ["sudo", "elevation", "input-disabled", "security", "windows-11"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote =
                        "Prevents automated elevated scripts from running silently via sudo in environments where operator oversight is required.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "AllowInputDisabledMode", 0)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "AllowInputDisabledMode")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "AllowInputDisabledMode", 0)],
                },
                new TweakDef
                {
                    Id = "sudopol-require-admin-group-membership",
                    Label = "Restrict Sudo to Local Administrators Group",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Ensures that only users who are members of the local Administrators group can use sudo for Windows, preventing standard users from attempting elevation via sudo.",
                    Tags = ["sudo", "elevation", "admin-group", "access-control", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Provides an explicit access gate: even if sudo is enabled on the device, non-admin users receive a denial.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "RequireAdminGroupMembership", 1)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "RequireAdminGroupMembership")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "RequireAdminGroupMembership", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-enable-audit-events",
                    Label = "Enable Sudo Elevation Audit Events",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Configures Windows to write an audit log entry whenever a process is elevated via sudo. Audit entries include the calling user, the target executable, and the elevation timestamp.",
                    Tags = ["sudo", "elevation", "audit", "compliance", "event-log"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Produces an accountable record of every sudo elevation, supporting incident response and SOC monitoring.",
                    RegistryKeys = [SudoKey],
                    ApplyOps = [RegOp.SetDword(SudoKey, "EnableAuditEvents", 1)],
                    RemoveOps = [RegOp.DeleteValue(SudoKey, "EnableAuditEvents")],
                    DetectOps = [RegOp.CheckDword(SudoKey, "EnableAuditEvents", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-block-network-elevated-processes",
                    Label = "Block Elevated Processes from Accessing Network",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Restricts network access for processes elevated via sudo, preventing elevated shells from making outbound network connections. Limits lateral movement potential from elevated contexts.",
                    Tags = ["sudo", "elevation", "network", "security", "hardening"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 4,
                    SafetyRating = 4,
                    ImpactNote =
                        "An elevated process with network access can pivot to other systems; this policy reduces the blast radius of a compromised sudo-elevated session.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "BlockNetworkFromElevatedProcesses")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "BlockNetworkFromElevatedProcesses", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-enforce-credential-prompt",
                    Label = "Always Prompt for Credentials on Sudo Elevation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Requires the user to enter explicit credentials (password or Windows Hello) before each sudo elevation, even within an existing authenticated session. Prevents silent re-elevation.",
                    Tags = ["sudo", "elevation", "credential-prompt", "uac", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Satisfies 'explicit approval' audit requirements by ensuring the user actively authenticates for each elevated action.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "AlwaysPromptCredentialsOnElevation", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-log-elevated-command-line",
                    Label = "Log Command-Line Arguments for Sudo-Elevated Processes",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables command-line logging for all processes elevated through sudo, recording the full command line in the Windows event log. Aids forensic investigation of elevation abuse.",
                    Tags = ["sudo", "elevation", "command-line", "audit", "forensics"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote =
                        "Command-line data in event logs reveals what was actually run with elevated privileges, not just that elevation occurred.",
                    RegistryKeys = [ElevationConfigKey],
                    ApplyOps = [RegOp.SetDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
                    RemoveOps = [RegOp.DeleteValue(ElevationConfigKey, "LogElevatedCommandLine")],
                    DetectOps = [RegOp.CheckDword(ElevationConfigKey, "LogElevatedCommandLine", 1)],
                },
                new TweakDef
                {
                    Id = "sudopol-block-unapproved-shells",
                    Label = "Block Sudo Elevation from Unapproved Shell Hosts",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Restricts sudo elevation to approved shell host executables (Windows Terminal, PowerShell 7, cmd.exe). Prevents elevation from unusual hosts such as scripting engines or third-party terminals.",
                    Tags = ["sudo", "elevation", "shell", "allowlist", "security"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    MinBuild = 22631,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "Reduces attack surface by ensuring only known-good terminal applications can initiate sudo elevation requests.",
                    RegistryKeys = [UacPoliciesKey],
                    ApplyOps = [RegOp.SetDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
                    RemoveOps = [RegOp.DeleteValue(UacPoliciesKey, "RestrictSudoToApprovedHosts")],
                    DetectOps = [RegOp.CheckDword(UacPoliciesKey, "RestrictSudoToApprovedHosts", 1)],
                },
            ];
    }

    // ── SystemShutdown ──
    private static class _SystemShutdown
    {
        private const string WinLogon = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Winlogon";

        private const string CurrentVersion = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion";

        private const string PoliciesSystem = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

        private const string PoliciesExplorer = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        private const string PowerSettings = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power";

        private const string SessionManager = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "shdn-reduce-wait-to-kill-timeout",
                Label = "Reduce WaitToKillServiceTimeout to 5 Seconds",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["shutdown", "service", "kill timeout", "speed"],
                Description =
                    "Reduces the time Windows waits for services to stop during shutdown "
                    + "from the default 20,000 ms to 5,000 ms. Speeds up shutdown at the "
                    + "cost of slightly less graceful service termination.",
                ApplyOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
                RemoveOps = [RegOp.SetString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "20000")],
                DetectOps = [RegOp.CheckString(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control", "WaitToKillServiceTimeout", "5000")],
            },
            new TweakDef
            {
                Id = "shdn-reduce-hung-app-timeout",
                Label = "Reduce HungAppTimeout to 4 Seconds",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Tags = ["shutdown", "hung app", "timeout", "speed"],
                Description =
                    "Reduces the time Windows waits before showing the 'This application is "
                    + "not responding' prompt during shutdown. HungAppTimeout=4000 ms "
                    + "(default is 5000 ms). Slightly quicker dialog trigger.",
                ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
                RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "5000")],
                DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "HungAppTimeout", "4000")],
            },
            new TweakDef
            {
                Id = "shdn-enable-auto-end-tasks",
                Label = "Enable AutoEndTasks (Kill Hung Apps on Logout)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Tags = ["shutdown", "auto end tasks", "hung app", "logout"],
                Description =
                    "Enables AutoEndTasks=1 so Windows automatically terminates applications "
                    + "that are hanging during logout or shutdown without waiting for user "
                    + "confirmation. Speeds up shutdown considerably.",
                ApplyOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
                RemoveOps = [RegOp.SetString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "0")],
                DetectOps = [RegOp.CheckString(@"HKEY_CURRENT_USER\Control Panel\Desktop", "AutoEndTasks", "1")],
            },
            new TweakDef
            {
                Id = "shdn-disable-shutdown-event-tracker",
                Label = "Disable Shutdown Event Tracker (No Reason Required)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = false,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["shutdown", "event tracker", "policy", "server"],
                Description =
                    "Disables the Shutdown Event Tracker that asks administrators why they "
                    + "are shutting down or restarting. Normally enabled only on Windows Server. "
                    + "ShutdownReasonUI=0.",
                ApplyOps = [RegOp.SetDword(PoliciesSystem, "ShutdownReasonUI", 0)],
                RemoveOps = [RegOp.DeleteValue(PoliciesSystem, "ShutdownReasonUI")],
                DetectOps = [RegOp.CheckDword(PoliciesSystem, "ShutdownReasonUI", 0)],
            },
            new TweakDef
            {
                Id = "shdn-suppress-logoff-scripts-run-at-shutdown",
                Label = "Run Logoff Scripts Simultaneously with Shutdown Scripts",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["shutdown", "logoff", "scripts", "gpo"],
                Description =
                    "Configures logoff and shutdown scripts to run simultaneously rather "
                    + "than sequentially. Reduces total script execution time during logout. "
                    + "RunLogonScriptSync=0.",
                ApplyOps = [RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0)],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "RunLogonScriptSync", 0),
                ],
            },
            new TweakDef
            {
                Id = "shdn-suppress-logoff-slow-scripts-ui",
                Label = "Disable 'Slow Script' Warning at Shutdown",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["shutdown", "gpo", "slow script", "ui"],
                Description =
                    "Hides the 'Please wait for the <script> to finish' message shown when "
                    + "GPO logoff/shutdown scripts exceed MaxGPOScriptWait. Prevents the UI "
                    + "from blocking shutdown on domain machines with slow scripts.",
                ApplyOps =
                [
                    RegOp.SetDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0),
                ],
                RemoveOps =
                [
                    RegOp.DeleteValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts"),
                ],
                DetectOps =
                [
                    RegOp.CheckDword(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System", "HideShutdownScripts", 0),
                ],
            },
        ];
    }

    // ── TabletPcInputPolicy ──
    private static class _TabletPcInputPolicy
    {
        private const string TabPC = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\TabletPC";
        private const string TabWin = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\TabletPC";

        public static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-data-sharing",
                Label = "Prevent Handwriting Data Sharing with Microsoft",
                Category = "Display — Start Menu Modern",
                Description = "Prevents Windows from sharing handwriting recognition data with Microsoft to improve handwriting recognition.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingDataSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingDataSharing")],
                DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingDataSharing", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-error-reports",
                Label = "Prevent Handwriting Error Reporting",
                Category = "Display — Start Menu Modern",
                Description = "Stops Windows from sending handwriting recognition error reports to Microsoft.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy", "telemetry"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "PreventHandwritingErrorReports", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "PreventHandwritingErrorReports")],
                DetectOps = [RegOp.CheckDword(TabPC, "PreventHandwritingErrorReports", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-ink-ball-game",
                Label = "Disable InkBall Game",
                Category = "Display — Start Menu Modern",
                Description = "Removes the InkBall game from the Start menu and blocks access via Group Policy.",
                Tags = ["tablet", "debloat", "group-policy", "games"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "DisableInkBall", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "DisableInkBall")],
                DetectOps = [RegOp.CheckDword(TabPC, "DisableInkBall", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-turn-off-passwordless",
                Label = "Turn Off Tablet PC Passwordless Experience",
                Category = "Display — Start Menu Modern",
                Description = "Disables the passwordless login experience on Tablet PC, requiring a password for sign-in.",
                Tags = ["tablet", "security", "group-policy", "login"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabPC, "TurnOffPwdlessExperience", 1)],
                RemoveOps = [RegOp.DeleteValue(TabPC, "TurnOffPwdlessExperience")],
                DetectOps = [RegOp.CheckDword(TabPC, "TurnOffPwdlessExperience", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-prevent-handwriting-personalization",
                Label = "Prevent Handwriting Personalization Collection",
                Category = "Display — Start Menu Modern",
                Description =
                    "Blocks Windows from collecting typed and handwriting data to build a personalized dictionary for handwriting recognition.",
                Tags = ["tablet", "privacy", "handwriting", "group-policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "PreventHandwritingPersonalization", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "PreventHandwritingPersonalization")],
                DetectOps = [RegOp.CheckDword(TabWin, "PreventHandwritingPersonalization", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-pen-training-support",
                Label = "Disable Pen Training and Support",
                Category = "Display — Start Menu Modern",
                Description = "Turns off the Tablet PC pen training and pen support documentation from the Tablet PC optional components.",
                Tags = ["tablet", "debloat", "group-policy", "pen"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisablePenTrainingAndSupport", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePenTrainingAndSupport")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisablePenTrainingAndSupport", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-turn-off-pen-feedback",
                Label = "Turn Off Pen Haptic Feedback",
                Category = "Display — Start Menu Modern",
                Description = "Disables haptic and visual ink feedback when using a digital pen on a touch display.",
                Tags = ["tablet", "pen", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "TurnOffPenFeedback", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "TurnOffPenFeedback")],
                DetectOps = [RegOp.CheckDword(TabWin, "TurnOffPenFeedback", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-touch-input",
                Label = "Disable Touch Input (Tablet PC Policy)",
                Category = "Display — Start Menu Modern",
                Description =
                    "Disables all touch-based input processing via Group Policy. Useful for kiosk or hardened deployments without touch screens.",
                Tags = ["tablet", "touch", "group-policy", "input", "kiosk"],
                NeedsAdmin = true,
                CorpSafe = false,
                ApplyOps = [RegOp.SetDword(TabWin, "DisableTouchInput", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisableTouchInput")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisableTouchInput", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-touchscreen-scroll",
                Label = "Disable Touchscreen Panning and Scrolling Inertia",
                Category = "Display — Start Menu Modern",
                Description =
                    "Disables momentum scrolling (inertia) and panning on touchscreens to reduce accidental scrolling in productivity apps.",
                Tags = ["tablet", "touch", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisablePanningFeedback", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisablePanningFeedback")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisablePanningFeedback", 1)],
            },
            new TweakDef
            {
                Id = "tabpol-disable-flick-gestures",
                Label = "Disable Pen and Touch Flick Gestures",
                Category = "Display — Start Menu Modern",
                Description = "Disables pen and touch flick gestures (quick swipe shortcuts) system-wide via Group Policy.",
                Tags = ["tablet", "touch", "pen", "group-policy", "input"],
                NeedsAdmin = true,
                CorpSafe = true,
                ApplyOps = [RegOp.SetDword(TabWin, "DisableFlicksFeature", 1)],
                RemoveOps = [RegOp.DeleteValue(TabWin, "DisableFlicksFeature")],
                DetectOps = [RegOp.CheckDword(TabWin, "DisableFlicksFeature", 1)],
            },
        ];
    }

    // ── TouchpadGestures ──
    private static class _TouchpadGestures
    {
        private const string Ptp = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad";

        private const string PtpSettings = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Settings";

        private const string EaseTouchpad = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\PrecisionTouchPad\Status";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "tpad-disable-three-finger-slide-task-view",
                Label = "Disable Three-Finger Slide (Task View / Switch Apps)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "gestures", "three-finger", "task view"],
                Description =
                    "Disables the three-finger swipe gesture that triggers Task View on "
                    + "upward swipe or switches apps on horizontal swipe. "
                    + "Value 0 = disabled for three-finger swipe actions.",
                ApplyOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "ThreeFingerSlideEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "ThreeFingerSlideEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-disable-four-finger-slide",
                Label = "Disable Four-Finger Slide (Virtual Desktop Navigation)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "gestures", "four-finger", "virtual desktops"],
                Description =
                    "Disables the four-finger horizontal swipe that switches between virtual "
                    + "desktops. Useful on compact touchpads where four-finger gestures are "
                    + "easily triggered accidentally.",
                ApplyOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "FourFingerSlideEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "FourFingerSlideEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-reverse-scroll-direction",
                Label = "Enable Reverse (Natural) Scroll Direction",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Tags = ["touchpad", "scroll", "natural scroll", "direction"],
                Description =
                    "Reverses the touchpad scroll direction to match natural/trackpad-style "
                    + "scrolling (content follows finger direction, like macOS). "
                    + "ReverseScrollingEnabled=1.",
                ApplyOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 1)],
                RemoveOps = [RegOp.SetDword(Ptp, "ReverseScrollingEnabled", 0)],
                DetectOps = [RegOp.CheckDword(Ptp, "ReverseScrollingEnabled", 1)],
            },
            new TweakDef
            {
                Id = "tpad-disable-pinch-to-zoom",
                Label = "Disable Pinch-to-Zoom Gesture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "pinch", "zoom", "gesture"],
                Description =
                    "Disables the pinch-to-zoom gesture on Precision Touchpad. Useful for "
                    + "users who accidentally trigger zoom while scrolling or using two-finger "
                    + "gestures. ZoomEnabled=0.",
                ApplyOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "ZoomEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "ZoomEnabled", 0)],
            },
            new TweakDef
            {
                Id = "tpad-set-sensitivity-most-sensitive",
                Label = "Set Touchpad Sensitivity to Most Sensitive",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "sensitivity", "cursor", "speed"],
                Description =
                    "Sets touchpad cursor speed/sensitivity to maximum (10) for fast, "
                    + "responsive cursor movement across large displays. "
                    + "CursorSpeed=10.",
                ApplyOps = [RegOp.SetDword(Ptp, "CursorSpeed", 10)],
                RemoveOps = [RegOp.SetDword(Ptp, "CursorSpeed", 5)],
                DetectOps = [RegOp.CheckDword(Ptp, "CursorSpeed", 10)],
            },
            new TweakDef
            {
                Id = "tpad-disable-edge-swipe",
                Label = "Disable Edge Swipe for Action Center / Widgets",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["touchpad", "edge swipe", "action center", "widgets"],
                Description =
                    "Disables the right-edge swipe gesture that opens the Action Center "
                    + "and swipe from left that shows widgets. Prevents accidental panel "
                    + "openings on touch-sensitive laptop bezels. "
                    + "EdgeEnabled=0.",
                ApplyOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 0)],
                RemoveOps = [RegOp.SetDword(Ptp, "EdgeEnabled", 1)],
                DetectOps = [RegOp.CheckDword(Ptp, "EdgeEnabled", 0)],
            },
        ];
    }

    // ── VideoCapturePolicy ──
    private static class _VideoCapturePolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VideoCapture";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vcap-disable-video-capture",
                Label = "Disable Video Capture Device Access",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets DisableVideoCapture=1 in the VideoCapture policy key. Blocks all "
                    + "application-level access to video capture hardware (webcams, capture cards, "
                    + "virtual cameras). Applications requesting the Camera device class are denied "
                    + "at the policy layer before the privacy permission prompt. Stronger than "
                    + "per-app toggles; applies universally. Default: 0. Recommended: 1 on "
                    + "kiosk, conference room, or regulated-data machines.",
                Tags = ["video-capture", "camera", "webcam", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVideoCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVideoCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVideoCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-screen-capture",
                Label = "Disable Screen Capture via Policy",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                Description =
                    "Sets DisableScreenCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications from using screen-capture APIs (Desktop Duplication API, "
                    + "Graphics.CaptureItem) to record the screen contents. Blocks tools such "
                    + "as OBS, Teams recording, and screenshot utilities at the policy layer. "
                    + "Default: 0. Recommended: 1 on machines handling sensitive classified "
                    + "or commercially confidential data.",
                Tags = ["video-capture", "screen-capture", "screenshot", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableScreenCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableScreenCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableScreenCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-broadcast",
                Label = "Disable Live Broadcast Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBroadcast=1 in the VideoCapture policy key. Blocks applications "
                    + "from using Windows broadcast APIs to stream game or desktop content to "
                    + "external platforms (Twitch, YouTube, Beam). These broadcast sessions can "
                    + "inadvertently expose corporate data if a game running on a managed device "
                    + "captures an adjacent application window. "
                    + "Default: 0. Recommended: 1 on corporate gaming or shared workstations.",
                Tags = ["video-capture", "broadcast", "streaming", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBroadcast", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBroadcast")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBroadcast", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-game-capture",
                Label = "Disable Game DVR-style Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableGameCapture=1 in the VideoCapture policy key. Prevents the "
                    + "GameDVR / Xbox Game Bar capture subsystem from recording gameplay video "
                    + "clips and screenshots via the VideoCapture pipeline. Frees GPU encoder "
                    + "headroom reserved for background capture. This policy targets the capture "
                    + "backend, supplementing the GameDVR GP setting which only hides the UI. "
                    + "Default: 0. Recommended: 1 on non-gaming workstations.",
                Tags = ["video-capture", "game-dvr", "xbox", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableGameCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableGameCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableGameCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-audio-capture",
                Label = "Disable Audio Capture with Video",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableAudioCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications from pairing microphone or system-audio capture with video "
                    + "capture sessions. Without audio capture, recording tools can still "
                    + "capture video but produce silent clips. Reduces the exposure surface "
                    + "for audio-based eavesdropping via legitimate recording applications. "
                    + "Default: 0. Recommended: 1 on open-plan or regulated office seats.",
                Tags = ["video-capture", "audio", "microphone", "recording", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableAudioCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableAudioCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableAudioCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-require-admin-for-capture",
                Label = "Require Admin Rights for Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets RequireAdminForCapture=1 in the VideoCapture policy key. Elevates "
                    + "the required privilege level for video capture operations so that only "
                    + "processes running with administrative rights can activate capture devices. "
                    + "Standard user applications, including browser-based conferencing tools, "
                    + "cannot access capture without elevation. Effective on shared machines. "
                    + "Default: 0. Recommended: 1 on high-security shared workstations.",
                Tags = ["video-capture", "admin", "privilege", "security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForCapture")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForCapture", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-camera-telemetry",
                Label = "Disable Camera Capture Telemetry",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableCaptureTelemetry=1 in the VideoCapture policy key. Stops "
                    + "Windows from sending camera and capture device usage events to Microsoft. "
                    + "These events include which applications activated the camera, session "
                    + "duration, and device identifiers. The data informs Windows quality "
                    + "improvements but may be unwanted on privacy-sensitive deployments. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["video-capture", "telemetry", "camera", "privacy", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCaptureTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCaptureTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCaptureTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-virtual-camera",
                Label = "Disable Virtual Camera Device Access",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                Description =
                    "Sets DisableVirtualCamera=1 in the VideoCapture policy key. Blocks "
                    + "applications from accessing virtual camera devices installed by software "
                    + "such as OBS Virtual Camera, NDI Tools, or ManyCam. Virtual cameras can "
                    + "function as a transparency layer that bypasses physical camera policies "
                    + "by injecting pre-recorded or manipulated video into conferencing tools. "
                    + "Default: 0. Recommended: 1 on compliance-requiring conferencing setups.",
                Tags = ["video-capture", "virtual-camera", "obs", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVirtualCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVirtualCamera")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVirtualCamera", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-media-capture-api",
                Label = "Disable MediaCapture API for UWP Apps",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableMediaCaptureAPI=1 in the VideoCapture policy key. Prevents "
                    + "UWP applications from using the Windows.Media.Capture.MediaCapture API "
                    + "to access camera and microphone hardware. Most modern Microsoft Store "
                    + "conferencing and imaging apps use this API. Blocking it forces those apps "
                    + "to request fallback devices or fail gracefully. "
                    + "Default: 0. Recommended: 1 on locked-down app environments.",
                Tags = ["video-capture", "uwp", "media-capture", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMediaCaptureAPI", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMediaCaptureAPI")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMediaCaptureAPI", 1)],
            },
            new TweakDef
            {
                Id = "vcap-disable-background-capture",
                Label = "Disable Background Video Capture",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableBackgroundCapture=1 in the VideoCapture policy key. Prevents "
                    + "applications that have been sent to the background from continuing to "
                    + "hold an active video capture session. Normally a minimised app retains "
                    + "the camera even when the user switches away. This policy releases the "
                    + "device when the capturing application loses focus, ensuring the camera "
                    + "indicator light extinguishes. Default: 0. Recommended: 1.",
                Tags = ["video-capture", "background", "camera", "performance", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBackgroundCapture", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBackgroundCapture")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBackgroundCapture", 1)],
            },
        ];
    }

    // ── VirtualKeyboardPolicy ──
    private static class _VirtualKeyboardPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualKeyboard";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "vkbd-disable-touch-keyboard",
                Label = "Disable Automatic Touch Keyboard Pop-Up",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Sets DisableTouchKeyboard=1 in the VirtualKeyboard policy key. Prevents "
                    + "the Windows touch keyboard from appearing automatically when the user "
                    + "taps on a text input field in tablet mode or when no physical keyboard "
                    + "is detected. On hybrid devices used in docked/keyboard mode the "
                    + "automatic pop-up interrupts workflows and requires manual dismissal. "
                    + "Default: 0. Recommended: 1 on non-tablet machines.",
                Tags = ["touch", "keyboard", "virtual", "tablet", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableTouchKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTouchKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTouchKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-emoji-panel",
                Label = "Disable Emoji Panel (Win+.)",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableEmojiPanel=1 in the VirtualKeyboard policy key. Removes the "
                    + "emoji and special-characters picker that opens via Windows + period (.) "
                    + "or Windows + semicolon (;). On production workstations the emoji panel "
                    + "is an unnecessary distraction; the keyboard shortcut is easily triggered "
                    + "accidentally during fast typing. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["emoji", "panel", "keyboard", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableEmojiPanel", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableEmojiPanel")],
                DetectOps = [RegOp.CheckDword(Key, "DisableEmojiPanel", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-sound",
                Label = "Disable Virtual Keyboard Key Click Sound",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardSound=1 in the VirtualKeyboard policy key. Mutes the "
                    + "click sound effect played each time a key on the on-screen touch keyboard "
                    + "is pressed. In quiet office or conference environments the click sounds "
                    + "are disruptive; the system-wide policy prevents users from re-enabling "
                    + "them. Default: 0. Recommended: 1.",
                Tags = ["keyboard", "sound", "click", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardSound", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardSound")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardSound", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-handwriting-button",
                Label = "Disable Touch Keyboard Handwriting Button",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableHandwritingButton=1 in the VirtualKeyboard policy key. Removes "
                    + "the stylus/pen button from the touch keyboard toolbar that switches "
                    + "from the key grid to the free-form handwriting input mode. On devices "
                    + "without a digitiser pen, the button serves no purpose and confuses "
                    + "users who activate it by mistake. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "handwriting", "button", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHandwritingButton", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHandwritingButton")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHandwritingButton", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-telemetry",
                Label = "Disable Virtual Keyboard Telemetry",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardTelemetry=1 in the VirtualKeyboard policy key. Stops "
                    + "the touch keyboard from reporting usage statistics including layout "
                    + "preference, session duration, and interaction rates to Microsoft's "
                    + "telemetry pipeline. Keyboard telemetry is collected continuously and "
                    + "contributes to the same diagnostic data pipeline as other Windows "
                    + "telemetry even when the user has opted out. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "telemetry", "privacy", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardTelemetry", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-fullscreen-keyboard",
                Label = "Disable Full-Screen Keyboard in Desktop Apps",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableFullScreenKeyboard=1 in the VirtualKeyboard policy key. "
                    + "Prevents the touch keyboard from expanding to a full-screen mode when "
                    + "a text field gains focus in a desktop (Win32) application. Full-screen "
                    + "keyboard mode obscures the application window entirely and requires "
                    + "manual collapse, disrupting productivity on hybrid devices. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "fullscreen", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFullScreenKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFullScreenKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFullScreenKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-keyboard-animations",
                Label = "Disable Touch Keyboard Animations",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableKeyboardAnimations=1 in the VirtualKeyboard policy key. "
                    + "Removes the slide and fade animations for touch keyboard show/hide "
                    + "transitions. On lower-end hardware or at high refresh rates the "
                    + "animation frame budget competes with foreground application rendering. "
                    + "Removing animations also improves perceived keyboard responsiveness. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "animation", "performance", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableKeyboardAnimations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableKeyboardAnimations")],
                DetectOps = [RegOp.CheckDword(Key, "DisableKeyboardAnimations", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-voice-dictation-key",
                Label = "Disable Voice Dictation Key on Touch Keyboard",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Sets DisableVoiceDictationKey=1 in the VirtualKeyboard policy key. Removes "
                    + "the microphone button from the touch keyboard that activates the Windows "
                    + "voice dictation mode. Voice dictation streams audio to the Windows "
                    + "speech recognition service; disabling the toolbar button prevents "
                    + "unintentional activation in environments where microphone use is "
                    + "restricted. Default: 0. Recommended: 1.",
                Tags = ["keyboard", "voice", "dictation", "microphone", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableVoiceDictationKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableVoiceDictationKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableVoiceDictationKey", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-split-keyboard",
                Label = "Disable Split Touch Keyboard Mode",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableSplitKeyboard=1 in the VirtualKeyboard policy key. Disables "
                    + "the split-keyboard layout that separates the keyboard into two thumb-"
                    + "typing halves at the screen edges. On non-tablet devices the split "
                    + "keyboard is an unneeded variant that users may accidentally activate via "
                    + "the keyboard settings menu, requiring manual restoration. "
                    + "Default: 0. Recommended: 1 on non-tablet form factors.",
                Tags = ["keyboard", "split", "tablet", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSplitKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSplitKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSplitKeyboard", 1)],
            },
            new TweakDef
            {
                Id = "vkbd-disable-wide-keyboard",
                Label = "Disable Wide Touch Keyboard Layout",
                Category = "Display — Start Menu Modern",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 1,
                SafetyRating = 5,
                Description =
                    "Sets DisableWideKeyboard=1 in the VirtualKeyboard policy key. Removes "
                    + "the wide (full-width undocked) touch keyboard variant from the layout "
                    + "picker. The wide layout is designed for Surface-style devices lying flat; "
                    + "on conventional desktops it covers most of the screen without a "
                    + "productivity benefit. Removing the option simplifies the layout menu. "
                    + "Default: 0. Recommended: 1.",
                Tags = ["keyboard", "wide", "layout", "virtual", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableWideKeyboard", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableWideKeyboard")],
                DetectOps = [RegOp.CheckDword(Key, "DisableWideKeyboard", 1)],
            },
        ];
    }

    // ── WddmDriverPolicy ──
    private static class _WddmDriverPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers";
        private const string ScKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\GraphicsDrivers\Scheduler";
        private const string PolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Display";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wddmpol-block-display-driver-fallback",
                    Label = "Block Fallback to Microsoft Basic Display Adapter",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents Windows from falling back to the Microsoft Basic Display Adapter (2048×1152 VESA-only) when the GPU driver crashes, maintaining the last known working display driver and attempting recovery instead.",
                    Tags = ["wddm", "basic-display", "driver-fallback", "recovery", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 4,
                    ImpactNote = "VGA-mode fallback blocked; driver crash triggers recovery, not basic display. May yield blank screen.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableBasicDisplayDriverFallback")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableBasicDisplayDriverFallback", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-disable-dxgi-flip-discard",
                    Label = "Disable Presentation Model Flip-Discard Optimisation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Disables the DXGI flip-discard presentation model that reuses swap chain surfaces, falling back to flip-sequential for maximum frame ordering correctness in trading and video production environments where tearing prevention is critical.",
                    Tags = ["wddm", "dxgi", "flip-discard", "presentation", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Flip-discard presentation disabled; flip-sequential used. Maximum frame ordering correctness at slight perf cost.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableFlipDiscard", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableFlipDiscard")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableFlipDiscard", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-log-tdr-events",
                    Label = "Log GPU TDR Recovery Events to System Event Log",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables System event log entries (EventID 4101, Display driver stopped responding and has recovered) for GPU TDR events, providing a history of GPU hangs and recovery cycles for diagnostics.",
                    Tags = ["wddm", "tdr", "event-log", "audit", "gpu-stability", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "GPU TDR events logged in System log; driver hang frequency visible for GPU stability diagnostics.",
                    ApplyOps = [RegOp.SetDword(Key, "TdrLogging", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "TdrLogging")],
                    DetectOps = [RegOp.CheckDword(Key, "TdrLogging", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-disable-gpu-driver-telemetry",
                    Label = "Disable WDDM GPU Driver Telemetry to Microsoft",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Prevents the Windows Display Driver Model from sending GPU driver crash reports, TDR telemetry, and hardware capability telemetry to Microsoft, protecting GPU model/driver version information from cloud disclosure.",
                    Tags = ["wddm", "telemetry", "privacy", "gpu", "microsoft", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "WDDM GPU telemetry to Microsoft disabled; GPU model, driver version, and TDR events not sent to cloud.",
                    ApplyOps = [RegOp.SetDword(PolKey, "DisableGPUTelemetry", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "DisableGPUTelemetry")],
                    DetectOps = [RegOp.CheckDword(PolKey, "DisableGPUTelemetry", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-enable-virtual-display",
                    Label = "Enable Virtual Display Adapter for Headless Operation",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Enables the Windows virtual display adapter (IndirectDisplay) for headless server scenarios, providing a software display output that supports RDP and remote management tools without a physical GPU or monitor.",
                    Tags = ["wddm", "virtual-display", "headless", "rdp", "server", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "Virtual display adapter enabled for headless RDP; servers without physical GPU get a software display.",
                    ApplyOps = [RegOp.SetDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
                    RemoveOps = [RegOp.DeleteValue(PolKey, "EnableVirtualDisplayAdapter")],
                    DetectOps = [RegOp.CheckDword(PolKey, "EnableVirtualDisplayAdapter", 1)],
                },
                new TweakDef
                {
                    Id = "wddmpol-set-gpu-priority-realtime",
                    Label = "Set GPU Work Item Priority to Normal for System Processes",
                    Category = "Display — Start Menu Modern",
                    Description =
                        "Configures the WDDM scheduler to run system and background GPU work items at Normal priority, preventing GPU starvation of foreground applications by long-running background ML or compute workloads.",
                    Tags = ["wddm", "gpu-priority", "scheduler", "background", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 3,
                    SafetyRating = 5,
                    ImpactNote = "Background GPU work items set to Normal priority; foreground app rendering not starved by compute jobs.",
                    ApplyOps = [RegOp.SetDword(ScKey, "BackgroundGPUPriority", 1)],
                    RemoveOps = [RegOp.DeleteValue(ScKey, "BackgroundGPUPriority")],
                    DetectOps = [RegOp.CheckDword(ScKey, "BackgroundGPUPriority", 1)],
                },
            ];
    }

    // ── WiaImageAcquisitionPolicy ──
    private static class _WiaImageAcquisitionPolicy
    {
        private const string ScanKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows NT\Scanner";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "imgacquire-disable-wia-service",
                Label = "Image Acquisition: Disable WIA (Windows Image Acquisition) Service",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Windows Image Acquisition (WIA) service via Group Policy, preventing scanners, cameras, and other WIA-compatible imaging devices from automatically launching the scanning wizard when connected. WIA devices can auto-trigger Windows Explorer and photo import dialogs. On managed workstations where scanning occurs through dedicated document management software, disabling WIA eliminates uncontrolled ad-hoc scanning to unmanaged locations.",
                Tags = ["image acquisition", "wia", "scanner", "camera", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "STINoInteractiveMode", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "STINoInteractiveMode")],
                DetectOps = [RegOp.CheckDword(ScanKey, "STINoInteractiveMode", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables WIA interactive mode; connected scanners will not auto-launch import dialogs.",
            },
            new TweakDef
            {
                Id = "imgacquire-restrict-user-device-install",
                Label = "Image Acquisition: Restrict User-Initiated Device Installation for Cameras",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents standard users from installing WIA-compatible cameras and imaging devices without administrator approval. Without this policy, plugging in a consumer camera can trigger a Plug-and-Play installation that adds imaging device drivers and WIA entries to the system. On managed environments, device drivers should only be installed through approved software deployment channels.",
                Tags = ["image acquisition", "wia", "camera", "device install", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RestrictUserDeviceInstall", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RestrictUserDeviceInstall")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RestrictUserDeviceInstall", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents user-triggered camera/scanner driver installation; admin elevation required for new imaging devices.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-transferring-without-policy",
                Label = "Image Acquisition: Disable Image Transfer Without Policy Approval",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Blocks WIA from transferring images from connected cameras, scanners, or memory cards to the local filesystem without an approved destination policy being applied. Without this control, users can freely dump images from connected devices to any local folder, bypassing document management systems and creating unmanaged data. Enabling this policy ensures all image transfer operations occur through sanctioned software.",
                Tags = ["image acquisition", "wia", "image transfer", "data control", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableTransferWithoutPolicy", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableTransferWithoutPolicy")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableTransferWithoutPolicy", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote =
                    "Blocks ad-hoc image transfers from cameras/scanners; images must be transferred through approved document management software.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scan-to-fax",
                Label = "Image Acquisition: Disable Scan-to-Fax WIA Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the WIA Scan-to-Fax destination that allows users to scan a document directly to a fax number through a Windows Fax and Scan workflow. Scan-to-fax functionality can be exploited to exfiltrate documents outside the organisation's content inspection boundary — fax transmissions often bypass DLP controls that monitor email and file-share uploads. Disabling this destination ensures all document workflows go through monitored channels.",
                Tags = ["image acquisition", "scan to fax", "wia", "data loss prevention", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToFax", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToFax")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToFax", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Scan-to-Fax from WIA destinations; Windows Fax and Scan direct-to-scanner functionality is unaffected.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-autoplay-camera",
                Label = "Image Acquisition: Disable AutoPlay for Camera Devices",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows AutoPlay from automatically launching when a camera or memory card is inserted, suppressing the dialog that asks what action to take (view photos, import as a folder, etc.). AutoPlay-triggered actions can automatically copy images from connected devices to default Photos or OneDrive locations without user awareness. Disabling AutoPlay ensures device connections require deliberate user action.",
                Tags = ["image acquisition", "autoplay", "camera", "memory card", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableAutoPlayCamera", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableAutoPlayCamera")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableAutoPlayCamera", 1)],
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Suppresses AutoPlay dialog for cameras and memory cards; no automatic photo import.",
            },
            new TweakDef
            {
                Id = "imgacquire-require-driver-signing",
                Label = "Image Acquisition: Require Signed Drivers for WIA Devices",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Enforces that only digitally-signed drivers can be loaded for WIA imaging devices. Unsigned WIA device drivers are a known malware vector — adversaries have used crafted WIA drivers to establish persistent kernel-mode access. Requiring driver signing ensures that all imaging device drivers are verifiable against Microsoft's WHQL certificate chain or a trusted enterprise root CA.",
                Tags = ["image acquisition", "wia", "driver signing", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RequireSignedDrivers", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RequireSignedDrivers")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RequireSignedDrivers", 1)],
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote = "Requires WHQL or enterprise-signed WIA drivers; unsigned legacy scanner drivers will fail to load.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scan-to-sharepoint",
                Label = "Image Acquisition: Disable Scan-to-SharePoint WIA Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the WIA Scan-to-SharePoint destination that allows users to scan documents and automatically upload them to a SharePoint document library via the Windows Scan app. Scan-to-SharePoint can bypass normal document governance workflows by depositing files directly into collaboration sites without metadata tagging, classification, or legal-hold review. Disabling this destination ensures all scanned documents go through the organisation's records management system.",
                Tags = ["image acquisition", "scan to sharepoint", "wia", "document governance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToSharePoint", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToSharePoint")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToSharePoint", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Removes Scan-to-SharePoint from WIA destination list; manual upload to SharePoint via browser is unaffected.",
            },
            new TweakDef
            {
                Id = "imgacquire-disable-scanner-to-network",
                Label = "Image Acquisition: Disable Scan-to-Network Share Feature",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the built-in scan-to-network-share facility in the WIA STI (Still Image) architecture that allows scanners with FTP/SMB push capability to send files directly to a Windows shared folder. Scan-to-network-share bypasses normal document management channels and can be used to exfiltrate documents to unauthorized UNC paths. Managed scanning environments should use dedicated secure document capture software instead.",
                Tags = ["image acquisition", "scanner", "network share", "data transfer", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToNetworkShare", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToNetworkShare")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToNetworkShare", 1)],
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Disables scan-to-SMB/FTP from the WIA stack; dedicated scanning software configured by IT continues to work.",
            },
            new TweakDef
            {
                Id = "imgacquire-block-scan-to-email",
                Label = "Image Acquisition: Block Scan-to-Email Functionality",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents the Windows Fax and Scan application and WIA-connected scanners from using the scan-to-email feature, which attaches scanned documents directly to email drafts. Scan-to-email can bypass DLP (Data Loss Prevention) policies by sending scanned documents through the default email client without content inspection. In regulated environments, document distribution must be controlled through DLP-aware channels.",
                Tags = ["image acquisition", "scanner", "email", "dlp", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "DisableScanToEmail", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "DisableScanToEmail")],
                DetectOps = [RegOp.CheckDword(ScanKey, "DisableScanToEmail", 1)],
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks scan-to-email; users must save scanned documents to approved locations and attach them manually.",
            },
            new TweakDef
            {
                Id = "imgacquire-restrict-scan-destination",
                Label = "Image Acquisition: Restrict Scan Destination to Approved Paths Only",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Enforces that WIA scan operations can only save documents to pre-approved local paths or managed network shares defined in Group Policy. Without this restriction, users can direct scanned content to removable drives, personal cloud sync folders (OneDrive, Dropbox), or mapped drives outside the corporate network perimeter. Restricting destinations ensures all scanned documents are stored in auditable, managed locations.",
                Tags = ["image acquisition", "scanner", "destination", "restriction", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                RegistryKeys = [ScanKey],
                ApplyOps = [RegOp.SetDword(ScanKey, "RestrictScanDestinations", 1)],
                RemoveOps = [RegOp.DeleteValue(ScanKey, "RestrictScanDestinations")],
                DetectOps = [RegOp.CheckDword(ScanKey, "RestrictScanDestinations", 1)],
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Limits scan destinations to IT-approved paths; scans to arbitrary local or cloud paths are blocked.",
            },
        ];
    }

    // ── WindowsAccessibilityPolicy ──
    private static class _WindowsAccessibilityPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility";
        private const string MagnKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Magnifier";
        private const string NarratorKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Accessibility\Narrator";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "a11ypol-disable-serial-keys",
                Label = "Accessibility Policy: Disable Serial Keys Support",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables Serial Keys accessibility support, which allows alternative input devices (joysticks, switches) connected to the serial port. Disabling reduces the attack surface on managed endpoints without physical accessibility hardware.",
                Tags = ["accessibility", "serial-keys", "input", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Reduces attack surface on managed endpoints without accessibility hardware.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSerialKeysSupport", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSerialKeysSupport")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSerialKeysSupport", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-sound-sentry",
                Label = "Accessibility Policy: Disable SoundSentry Visual Flash",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables SoundSentry, which flashes the screen or a window when a critical sound plays. On enterprise environments with active CAD/3D rendering, unexpected screen flashes can interfere with rendering workflows.",
                Tags = ["accessibility", "soundsentry", "flash", "audio", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Prevents unexpected screen flashes interfering with CAD/3D rendering workflows.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableSoundSentryFunctionality", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableSoundSentryFunctionality")],
                DetectOps = [RegOp.CheckDword(Key, "DisableSoundSentryFunctionality", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-high-contrast-hotkey",
                Label = "Accessibility Policy: Disable High Contrast Mode Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents users from accidentally enabling High Contrast mode via the Left Alt+Left Shift+Print Screen keyboard shortcut. Avoids unexpected UI colour inversions that can disrupt productivity applications.",
                Tags = ["accessibility", "high-contrast", "hotkey", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents accidental Alt+Shift+PrtScr activation of high contrast mode.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHighContrastHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHighContrastHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHighContrastHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-toggle-keys",
                Label = "Accessibility Policy: Disable Toggle Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables activation of Toggle Keys (a beep when pressing Caps Lock, Num Lock, or Scroll Lock) via the Num Lock hotkey shortcut. Prevents unexpected beeping on endpoints with shared keyboards.",
                Tags = ["accessibility", "toggle-keys", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents accidental beeping when Num Lock key is pressed on shared keyboards.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableToggleKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableToggleKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableToggleKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-sticky-keys-hotkey",
                Label = "Accessibility Policy: Disable Sticky Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Sticky Keys prompt when Shift is pressed 5 times. Sticky Keys can interrupt gaming and productivity workflows when activated accidentally, and is better enabled via Settings if needed.",
                Tags = ["accessibility", "sticky-keys", "keyboard", "hotkey", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents 5\u00d7Shift shortcut from interrupting gaming and productivity workflows.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableStickyKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableStickyKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableStickyKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-filter-keys-hotkey",
                Label = "Accessibility Policy: Disable Filter Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables the Filter Keys shortcut activated by holding the Right Shift key for 8 seconds. Filter Keys can cause significant input delay if triggered accidentally.",
                Tags = ["accessibility", "filter-keys", "keyboard", "hotkey", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents 8-second Shift hold from triggering keyboard input delay mode.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableFilterKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableFilterKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableFilterKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-bounce-keys",
                Label = "Accessibility Policy: Disable Bounce Keys for Keyboard Repeat",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables Bounce Keys (Filter Keys variant) that ignores brief multiple key presses. While useful for accessibility, this setting can reduce keyboard responsiveness when not needed.",
                Tags = ["accessibility", "bounce-keys", "filter-keys", "keyboard", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Restores normal keyboard repeat; disables brief multiple-keypress filtering.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableBounceKeyboardSettings", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableBounceKeyboardSettings")],
                DetectOps = [RegOp.CheckDword(Key, "DisableBounceKeyboardSettings", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-mouse-keys-hotkey",
                Label = "Accessibility Policy: Disable Mouse Keys Hotkey",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Disables activation of Mouse Keys via the Left Alt+Left Shift+Num Lock shortcut. Mouse Keys redirects numpad input to pointer movement, which is a common source of unexpected mouse behaviour on laptops.",
                Tags = ["accessibility", "mouse-keys", "hotkey", "numpad", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Prevents numpad-to-mouse redirect being accidentally activated on laptops.",
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableMouseKeysHotKey", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableMouseKeysHotKey")],
                DetectOps = [RegOp.CheckDword(Key, "DisableMouseKeysHotKey", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-magnifier-startup",
                Label = "Accessibility Policy: Disable Magnifier Auto-Start",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows Magnifier from starting automatically when a user signs in. Magnifier auto-start is sometimes triggered by a registry artefact on downgraded or re-imaged systems.",
                Tags = ["accessibility", "magnifier", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents Magnifier from auto-starting on re-imaged systems.",
                RegistryKeys = [MagnKey],
                ApplyOps = [RegOp.SetDword(MagnKey, "StartMinimized", 1)],
                RemoveOps = [RegOp.DeleteValue(MagnKey, "StartMinimized")],
                DetectOps = [RegOp.CheckDword(MagnKey, "StartMinimized", 1)],
            },
            new TweakDef
            {
                Id = "a11ypol-disable-narrator-startup",
                Label = "Accessibility Policy: Disable Narrator Auto-Start on Sign-In",
                Category = "Display — Wia Image Acquisition",
                Description =
                    "Prevents Windows Narrator (screen reader) from starting automatically at sign-in. Narrator auto-activation can be triggered by accessibility registry artefacts on shared or re-used endpoints.",
                Tags = ["accessibility", "narrator", "screen-reader", "startup", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Prevents Narrator screen reader from auto-activating on shared or re-used endpoints.",
                RegistryKeys = [NarratorKey],
                ApplyOps = [RegOp.SetDword(NarratorKey, "DisableNarratorAutoStart", 1)],
                RemoveOps = [RegOp.DeleteValue(NarratorKey, "DisableNarratorAutoStart")],
                DetectOps = [RegOp.CheckDword(NarratorKey, "DisableNarratorAutoStart", 1)],
            },
        ];
    }

    // ── WindowsInkWorkspaceAdvPolicy ──
    private static class _WindowsInkWorkspaceAdvPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\WindowsInkWorkspace";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-on-lockscreen",
                Label = "Restrict Windows Ink Workspace Access from the Lock Screen",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting Windows Ink Workspace access from the lock screen prevents users who are not authenticated to the device from launching ink workspace features that may expose note content or access note-taking functionality with user data. Lock screen accessible features bypass authentication requirements and any data that can be accessed from the lock screen represents a risk of unauthorized access to user information. The default Windows configuration allows Ink Workspace notes to be accessed from the lock screen through the taskbar which could expose previously created notes to physical access attackers. Restricting lock screen access to Ink Workspace is particularly important for shared devices and devices in physical locations accessible to people outside the organization. Users who frequently take notes using the stylus workflow should be informed that lock screen notes will not be accessible and alternative note-taking approaches should be available. Lock screen feature restrictions should be tested for consistency on all device types including Surface devices that have stylus-focused features.",
                Tags = ["ink-workspace", "lock-screen", "access-restriction", "authentication", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowWindowsInkWorkspaceOnLockScreen")],
                DetectOps = [RegOp.CheckDword(Key, "AllowWindowsInkWorkspaceOnLockScreen", 0)],
            },
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-to-approved-users",
                Label = "Restrict Windows Ink Workspace Feature Access to Authorized User Groups",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Restricting Windows Ink Workspace to authorized user groups ensures that the feature is available only to employees who have a legitimate business need for pen input capabilities aligned with their role such as design healthcare or field operations functions. Unrestricted access to Ink Workspace on all endpoints makes the feature available to users who do not use pen input and creates an unnecessary attack surface for potential vulnerabilities in the ink processing stack. Role-based feature availability reduces the endpoint attack surface by disabling features that are not required for specific job functions while maintaining availability for users with legitimate use cases. Ink Workspace capability restrictions align with application allowlisting principles extending feature allowlisting beyond application execution to complex OS features. User group membership for ink workspace access authorization should be maintained as part of the identity management and access governance process. Devices that are not configured with digitizer hardware should have ink workspace disabled by default to avoid providing no-op features that consume resources.",
                Tags = ["ink-workspace", "user-restriction", "role-based-access", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceToApprovedUsers")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceToApprovedUsers", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-disable-ink-personalization-data-collection",
                Label = "Disable Ink and Typing Personalization Data Collection for Windows Ink",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Disabling ink and typing personalization data collection prevents Windows from sending handwriting samples stylus gesture patterns and ink input data to Microsoft services for handwriting recognition improvement and personalization. Handwriting recognition data transmitted to cloud services for personalization may include sensitive information written with a stylus such as signatures handwritten notes and sketches containing confidential content. Ink personalization data represents a category of behavioral biometric data that is sensitive from both privacy and security perspectives as handwriting patterns can uniquely identify individuals. Organizations with strict data minimization requirements should disable ink personalization collection to prevent unnecessary transmission of user behavioral data to cloud services. Users who rely on accurate handwriting recognition for productivity should be informed that disabling personalization may reduce recognition accuracy over time as the local model will not improve through cloud training. Disabling ink personalization collection is consistent with broader Windows telemetry and data collection minimization policies in enterprise environments.",
                Tags = ["ink-personalization", "data-collection", "privacy", "handwriting", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableInkPersonalizationData", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableInkPersonalizationData")],
                DetectOps = [RegOp.CheckDword(Key, "DisableInkPersonalizationData", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-enforce-ink-clipboard-restrictions",
                Label = "Enforce Clipboard Restrictions for Ink Content Sharing Between Applications",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enforcing clipboard restrictions for ink content prevents handwritten ink from being copied and pasted between applications in ways that could bypass data loss prevention controls configured to monitor or restrict text data transfer. Ink content transferred through the clipboard may bypass DLP policies that inspect text-format clipboard data because ink is represented as a different data format that may not be inspected by the same DLP mechanisms. Organizations with strict DLP requirements should evaluate whether ink clipboard sharing creates a bypass channel for sensitive data transfer that circumvents text-based DLP controls. Clipboard restrictions for ink content should be evaluated in the context of the organization's broader clipboard management policy including cloud clipboard synchronization controls. Users who use ink input for legitimate workflow reasons including annotations and sketching should be made aware of any restrictions on ink clipboard sharing that affect their productivity. Ink data format controls should be reviewed periodically as Windows updates may introduce new ink data formats that affect the applicability of existing clipboard restriction policies.",
                Tags = ["ink-clipboard", "dlp", "data-transfer", "clipboard-restriction", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceInkClipboardRestrictions", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceInkClipboardRestrictions")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceInkClipboardRestrictions", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-block-ink-workspace-third-party-apps",
                Label = "Block Third-Party Applications from Integrating with Windows Ink Workspace",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Blocking third-party application integration with Windows Ink Workspace prevents unapproved applications from registering themselves as ink workspace providers or intercepting ink input streams through the workspace integration API. Third-party ink workspace integrations that have not been vetted by organizational security review may collect ink input data including handwritten sensitive content through their integration with the ink pipeline. Malicious applications posing as legitimate ink workspace tools can use the integration API to capture all ink input data as a form of keylogging for stylus-entered text. Organizations should evaluate legitimate third-party ink application requirements through the normal software approval process rather than allowing unrestricted third-party ink workspace integration. Approved pen-input applications should be deployed through device management platforms with appropriate application version controls to ensure only vetted application versions can access ink workspace integration points. Security testing of the ink workspace third-party integration API should be included in application security assessments for applications that request ink workspace integration capabilities.",
                Tags = ["ink-workspace", "third-party-apps", "integration-security", "input-protection", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockThirdPartyInkWorkspaceApps")],
                DetectOps = [RegOp.CheckDword(Key, "BlockThirdPartyInkWorkspaceApps", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-disable-cloud-ink-sync",
                Label = "Disable Synchronization of Windows Ink Notes to Cloud Storage Services",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Disabling cloud synchronization of Windows Ink notes prevents handwritten digital ink sketches and annotations from being automatically uploaded to cloud storage services where they are subject to third-party data handling and may be accessible from devices not managed by the organization. Cloud sync of ink notes can transmit sensitive handwritten content outside the organizational security boundary including annotated documents contract documents and handwritten notes taken during confidential meetings. Ink note synchronization disabled through policy ensures that notes remain on the device until the user explicitly chooses to share them through approved channels. Organizations that have approved OneDrive or other enterprise cloud storage for document synchronization should configure ink sync to use only approved enterprise storage rather than disabling all sync. Data sovereignty requirements may prohibit automatic cloud sync of data including handwritten notes to cloud regions or providers that do not meet organizational compliance requirements. Users should be informed about the cloud sync policy for ink notes and provided with guidance on how to share ink content through approved methods when collaboration requires information sharing.",
                Tags = ["ink-sync", "cloud-storage", "data-protection", "note-taking", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableCloudInkSync", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableCloudInkSync")],
                DetectOps = [RegOp.CheckDword(Key, "DisableCloudInkSync", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-enforce-stylus-firmware-attestation",
                Label = "Enforce Firmware Attestation Checks for Connected Stylus and Pen Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Stylus and pen device firmware attestation ensures that pen devices connected to enterprise systems have firmware that has been validated as authentic and unmodified preventing compromised pen firmware from injecting false input events or exploiting pen driver vulnerabilities. Pen and stylus devices connect through USB or Bluetooth interfaces and the driver surface that processes pen input represents an attack vector for specially crafted devices with malicious firmware. Supply chain attacks targeting peripheral firmware have demonstrated that hardware devices can be compromised to execute code in the context of the operating system's hardware input processing. Firmware attestation requirements for pen devices should be part of the organization's broader peripheral device security policy that includes hardware device approval and firmware management. Organizations should define a list of approved pen device models that have been security-evaluated and use device management controls to restrict use of pen devices to approved models. Security monitoring for pen device connection events should be implemented to detect use of unauthorized or unusual pen devices.",
                Tags = ["stylus-firmware", "attestation", "peripheral-security", "supply-chain", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceStylusFirmwareAttestation", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceStylusFirmwareAttestation")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceStylusFirmwareAttestation", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-audit-ink-workspace-activity",
                Label = "Enable Audit Logging for Windows Ink Workspace Activation and Feature Use",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Enabling audit logging for Windows Ink Workspace activity records when the Ink Workspace is activated which features are used and what applications receive ink input providing a behavioral baseline for ink workspace usage patterns. Anomalous ink workspace activity including activation at unusual times screen-capturing ink operations or large volumes of ink note creation may indicate data harvesting or unauthorized use of pen input features. Audit logs for Ink Workspace should include user identity device identifier timestamp and feature used to support investigation of security incidents involving ink input workflows. Organizations can use ink workspace audit data to identify users who would benefit from additional training on ink security policies or who may be using ink features in ways that create security risks. Ink workspace activity monitoring should be proportionate to the sensitivity of data that users with ink access handle with more intensive monitoring for users with access to highly sensitive information. Audit events from ink workspace usage should be integrated with user and entity behavior analytics platforms to contextualize ink activity within the broader user behavioral profile.",
                Tags = ["ink-workspace", "audit-logging", "behavioral-analytics", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditInkWorkspaceActivity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditInkWorkspaceActivity")],
                DetectOps = [RegOp.CheckDword(Key, "AuditInkWorkspaceActivity", 1)],
            },
            new TweakDef
            {
                Id = "inkwsadv-restrict-ink-workspace-on-shared-devices",
                Label = "Apply Strict Ink Workspace Restrictions on Shared and Kiosk Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Description =
                    "Applying strict Ink Workspace restrictions on shared devices and kiosk configurations prevents previous users' ink notes and sketches from being accessible to subsequent users of the same device minimizing data leakage between user sessions on shared infrastructure. Shared devices that do not clear Ink Workspace content between user sessions can expose handwritten notes taken by previous authenticated users to users who authenticate later. Kiosk configurations should have Ink Workspace disabled entirely unless pen input is an integral part of the kiosk application's intended function. Session isolation for ink workspace data on shared devices should ensure that ink content from one user session is not accessible in another user's session context. Organizations deploying hot-desking or shared workstation environments should configure ink workspace policies appropriate for the shared use context including automatic note clearing on session end. Ink workspace configuration for shared devices should be validated as part of the device provisioning process to ensure that shared use policies are applied correctly.",
                Tags = ["ink-workspace", "shared-devices", "kiosk", "session-isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictInkWorkspaceOnSharedDevices")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictInkWorkspaceOnSharedDevices", 1)],
            },
        ];
    }

    // ── WindowsSearchAdv ──
    private static class _WindowsSearchAdv
    {
        private const string SearchPolicy = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

        private const string SearchUser = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Search";

        private const string SearchInternal = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Search";

        private const string SearchResults = @"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Policies\Explorer";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "search-disable-web-results-policy",
                Label = "Disable Web Results in Search via Policy",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Tags = ["search", "web", "policy", "bing", "privacy"],
                Description =
                    "Enforces disabling of internet search results in Windows Search via "
                    + "Group Policy. Persists across user profile changes and applies to all "
                    + "users on the system.",
                ApplyOps = [RegOp.SetDword(SearchPolicy, "DisableWebSearch", 1)],
                RemoveOps = [RegOp.DeleteValue(SearchPolicy, "DisableWebSearch")],
                DetectOps = [RegOp.CheckDword(SearchPolicy, "DisableWebSearch", 1)],
            },
            new TweakDef
            {
                Id = "search-disable-safe-search",
                Label = "Disable SafeSearch Filter in Windows Search",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["search", "safesearch", "web", "bing"],
                Description =
                    "Disables the SafeSearch filter (adult content filtering) from Windows "
                    + "Search web results. Value 0 = Off. Requires web search to be enabled. "
                    + "Only affects Windows Search Bing integration.",
                ApplyOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 0)],
                RemoveOps = [RegOp.SetDword(SearchUser, "SafeSearchMode", 1)],
                DetectOps = [RegOp.CheckDword(SearchUser, "SafeSearchMode", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-find-my-files",
                Label = "Disable Enhanced 'Find My Files' Deep Search",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                Tags = ["search", "find my files", "enhanced", "indexing"],
                Description =
                    "Disables the 'Find My Files' enhanced indexing mode that deeply indexes "
                    + "all files including non-indexed locations. Reduces background disk I/O "
                    + "from extensive indexing sweeps.",
                ApplyOps = [RegOp.SetDword(SearchUser, "DeviceHistoryEnabled", 0)],
                RemoveOps = [RegOp.DeleteValue(SearchUser, "DeviceHistoryEnabled")],
                DetectOps = [RegOp.CheckDword(SearchUser, "DeviceHistoryEnabled", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-recent-activities-search",
                Label = "Disable Recent Activities in Search Results",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = false,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["search", "recent", "activity", "privacy"],
                Description =
                    "Disables recently opened files and apps from appearing in Windows Search "
                    + "results. Prevents search from surfacing your recent activity to other "
                    + "users on shared machines.",
                ApplyOps = [RegOp.SetDword(SearchUser, "History", 0)],
                RemoveOps = [RegOp.SetDword(SearchUser, "History", 1)],
                DetectOps = [RegOp.CheckDword(SearchUser, "History", 0)],
            },
            new TweakDef
            {
                Id = "search-disable-device-sync-search",
                Label = "Disable Cross-Device Search Sync",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Tags = ["search", "sync", "device", "privacy", "cloud"],
                Description =
                    "Disables Windows Search syncing query history and results across "
                    + "devices connected to the same Microsoft account. Keeps search history "
                    + "local to this machine only.",
                ApplyOps = [RegOp.SetDword(SearchPolicy, "AllowCortana", 0)],
                RemoveOps = [RegOp.DeleteValue(SearchPolicy, "AllowCortana")],
                DetectOps = [RegOp.CheckDword(SearchPolicy, "AllowCortana", 0)],
            },
        ];
    }

    // ── WindowsSearchIndexingAdvancedPolicy ──
    private static class _WindowsSearchIndexingAdvancedPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Windows Search";

        public static IReadOnlyList<TweakDef> Data =>
            [
                new TweakDef
                {
                    Id = "wsidx-prevent-remote-queries",
                    Label = "Prevent Remote Search Queries via Windows Search",
                    Category = "Display — Wia Image Acquisition",
                    Description =
                        "Blocks remote clients from querying the local Windows Search index over the network. Default: allowed. Recommended: disabled for workstations.",
                    Tags = ["search", "indexing", "remote", "privacy", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 4,
                    SafetyRating = 5,
                    ImpactNote = "Prevents network-based search queries against local index; local search unaffected.",
                    ApplyOps = [RegOp.SetDword(Key, "PreventRemoteQueries", 1)],
                    RemoveOps = [RegOp.DeleteValue(Key, "PreventRemoteQueries")],
                    DetectOps = [RegOp.CheckDword(Key, "PreventRemoteQueries", 1)],
                },
                new TweakDef
                {
                    Id = "wsidx-disable-safe-search",
                    Label = "Set Search SafeSearch to Strict via Policy",
                    Category = "Display — Wia Image Acquisition",
                    Description = "Enforces SafeSearch strict mode for web results in Windows Search. Applies via Group Policy. Default: moderate.",
                    Tags = ["search", "safe-search", "content-filter", "policy"],
                    NeedsAdmin = true,
                    CorpSafe = true,
                    ImpactScore = 2,
                    SafetyRating = 5,
                    ImpactNote = "SafeSearch forced to strict; only affects web result filtering.",
                    ApplyOps = [RegOp.SetDword(Key, "ConnectedSearchSafeSearch", 3)],
                    RemoveOps = [RegOp.DeleteValue(Key, "ConnectedSearchSafeSearch")],
                    DetectOps = [RegOp.CheckDword(Key, "ConnectedSearchSafeSearch", 3)],
                },
            ];
    }

    // ── VirtualizationPolicy ──
    private static class _VirtualizationPolicy
    {
        private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";

        internal static IReadOnlyList<TweakDef> Data { get; } =
        [
            new TweakDef
            {
                Id = "virtz-restrict-hyper-v-management-to-admins",
                Label = "Restrict Hyper-V Management Operations to Administrator Accounts",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Restricting Hyper-V management to administrator accounts prevents standard users from creating modifying or deleting virtual machines that could be used to run unauthorized software within a virtualized environment. Standard users with Hyper-V management access could create virtual machines that bypass organizational security controls applied to host systems. Unauthorized virtual machines are difficult to monitor and may not have security software installed creating blind spots in endpoint protection coverage. Hyper-V management access should be limited to IT administrators who have a documented business need to create and manage virtual machines. Organizations should implement least-privilege principles for Hyper-V management using delegated administration where possible to grant only the specific capabilities required for each administrative role. Audit logging for Hyper-V management operations should track all VM creation deletion and configuration changes by administrator.",
                Tags = ["hyper-v", "virtualization", "admin-restriction", "least-privilege", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictManagementToAdmins", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictManagementToAdmins")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictManagementToAdmins", 1)],
            },
            new TweakDef
            {
                Id = "virtz-disable-hyper-v-on-workstations",
                Label = "Disable Hyper-V Virtualization Platform on Standard Enterprise Workstations",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                Description =
                    "Disabling Hyper-V on standard workstations that do not have a documented business requirement for local virtualization reduces attack surface by removing the virtualization infrastructure that could be used for malicious purposes. Hyper-V enabled workstations can be used by attackers to run virtual machines that bypass host-based security controls and operate as isolated systems on the corporate network. Virtualization-based security features like Credential Guard and Device Guard require Hyper-V to be present so disabling Hyper-V must be weighed against the security benefits those features provide. Organizations should evaluate whether disabling Hyper-V is appropriate for their security model or whether keeping it enabled primarily for VBS security features is the better configuration. Developer workstations and IT administration systems that have legitimate virtualization requirements should be exempted from the general workstation policy. Disabling Hyper-V may affect WSL 2 which uses Hyper-V technology; alternatives using WSLg or WSL 1 should be evaluated if WSL is required.",
                Tags = ["hyper-v", "workstation", "virtualization", "attack-surface", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "DisableHyperVOnWorkstations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperVOnWorkstations")],
                DetectOps = [RegOp.CheckDword(Key, "DisableHyperVOnWorkstations", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enforce-synthetic-device-security",
                Label = "Enforce Security Configuration for Hyper-V Synthetic Devices",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Hyper-V synthetic devices provide paravirtualized device interfaces between guest virtual machines and the hypervisor that can be exploited to break guest isolation if not configured securely. Enforcing security configuration for synthetic devices ensures that guest VMs cannot exploit vulnerabilities in device emulation to gain access to host resources or hypervisor memory. Synthetic network adapters storage controllers and video adapters each have configurable security parameters that should be set to the most restrictive values appropriate for the VM workload. Organizations should apply the principle of least capability to Hyper-V VMs granting only the synthetic devices needed for the VM's function. Security configuration for synthetic devices should be audited as part of the VM provisioning process to ensure that all new VMs are configured with appropriate device security settings. Guest VM security configurations should be periodically reviewed to identify VMs that have accumulated unnecessary synthetic device capabilities.",
                Tags = ["hyper-v", "synthetic-devices", "vm-security", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSyntheticDeviceSecurity")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enable-vm-snapshot-encryption",
                Label = "Enable Encryption of Virtual Machine Snapshots and Saved States",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Virtual machine snapshots and saved states contain complete memory images of running VMs that may include encryption keys credentials and sensitive application data. Encrypting VM snapshots and saved states ensures that this sensitive data is protected if snapshot files are accessed outside the Hyper-V management context. VM snapshot files stored on shared storage or backup media are particularly vulnerable to unauthorized access if they are not encrypted at the VM level. Shielded VMs in Hyper-V provide the highest level of protection including encryption of VM configuration snapshots and saved states using Host Guardian Service key management. Organizations should implement VM encryption for any virtual machine that processes sensitive regulated data including financial HR and healthcare applications. Key management for VM encryption should be integrated with the organizational key management infrastructure to ensure proper key lifecycle management.",
                Tags = ["hyper-v", "vm-encryption", "snapshots", "saved-states", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableSnapshotEncryption", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableSnapshotEncryption")],
                DetectOps = [RegOp.CheckDword(Key, "EnableSnapshotEncryption", 1)],
            },
            new TweakDef
            {
                Id = "virtz-restrict-vm-clipboard-sharing",
                Label = "Restrict Clipboard Sharing Between Hyper-V Guest and Host",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Clipboard sharing between Hyper-V guests and the host system creates a data transfer channel that can leak sensitive data between VMs and the host or allow malicious code injection through the clipboard. Restricting clipboard sharing prevents accidental data leakage between isolated VMs running different workloads or between VM environments and the host system. VMs running untrusted content or isolated high-security workloads should have clipboard sharing disabled to prevent data from crossing VM isolation boundaries. Malicious applications running in a VM can use clipboard injection to execute code on the host if the user pastes clipboard content from the VM into host applications. Organizations should evaluate clipboard sharing requirements for each VM type and allow it only where legitimate workflow requirements justify the associated risk. Monitoring clipboard sharing events can help detect attempts to use clipboard as a data exfiltration channel between isolated environments.",
                Tags = ["hyper-v", "clipboard", "data-isolation", "vm-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardSharing", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardSharing")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardSharing", 1)],
            },
            new TweakDef
            {
                Id = "virtz-audit-vm-management-operations",
                Label = "Enable Audit Logging for All Hyper-V Virtual Machine Management Operations",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Hyper-V management audit logging records all VM lifecycle events including VM creation startup shutdown deletion and configuration changes providing accountability for VM management operations. Audit trails for VM management are important for detecting unauthorized VM creation that could be used for shadow IT or malicious purposes. VM deletion events should be monitored closely as deletion of VMs may indicate evidence destruction in the context of security incidents. Audit events for Hyper-V management should include the identity of the administrator performing the operation the timestamp and the details of the changed configuration. Organizations should forward Hyper-V audit events to centralized SIEM for correlation with other administrative activity and identity events. VM management audit logs should be retained for a period appropriate to the organization's compliance requirements.",
                Tags = ["hyper-v", "audit", "vm-management", "monitoring", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "AuditVMManagementOperations", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditVMManagementOperations")],
                DetectOps = [RegOp.CheckDword(Key, "AuditVMManagementOperations", 1)],
            },
            new TweakDef
            {
                Id = "virtz-enforce-secure-boot-for-vms",
                Label = "Enforce Secure Boot Configuration for Hyper-V Generation 2 VMs",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enforcing Secure Boot for Hyper-V Generation 2 virtual machines ensures that VMs only boot operating system images that are signed with trusted certificates preventing rootkit-level malware from persisting in VM boot configurations. Guest VM Secure Boot uses Hyper-V virtual firmware to validate the boot chain signature exactly as physical Secure Boot does on bare metal systems. Malicious modifications to VM boot sectors or boot loaders are prevented by Secure Boot enforcement which is particularly important for VMs that are created from templates or imported from external sources. Guest Secure Boot should be configured with appropriate templates for the VM's operating system type. Organizations should define and enforce VM templates that include Secure Boot configuration to ensure all provisioned VMs have this protection enabled from creation. VMs that fail Secure Boot validation should not be allowed to start and alerts should be generated for investigation.",
                Tags = ["hyper-v", "secure-boot", "vm-security", "boot-integrity", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootForVMs", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootForVMs")],
                DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootForVMs", 1)],
            },
            new TweakDef
            {
                Id = "virtz-restrict-vm-network-access",
                Label = "Restrict Hyper-V Virtual Machine Network Access Configuration",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Restricting VM network access through Hyper-V policy limits which network segments virtual machines can be connected to preventing VMs from accessing sensitive network segments that are not appropriate for their function. Virtual machine network placement should be deliberately configured to provide access only to the network segments required for the VM's workload. Production VMs should be isolated from development and test VMs at the network layer to prevent cross-contamination between environments. VMs that process sensitive data should be on network segments with monitoring and DLP capabilities to detect unauthorized data access. Unrestricted VM network configuration allows administrators to connect VMs to any available virtual switch without network policy review which can lead to inadvertent bypassing of network segmentation controls. Network access configuration for Hyper-V VMs should be part of the VM provisioning review process including approval of new network connections.",
                Tags = ["hyper-v", "vm-network", "segmentation", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "RestrictVMNetworkAccess", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RestrictVMNetworkAccess")],
                DetectOps = [RegOp.CheckDword(Key, "RestrictVMNetworkAccess", 1)],
            },
            new TweakDef
            {
                Id = "virtz-block-nested-virtualization",
                Label = "Block Nested Virtualization in Hyper-V Guest Virtual Machines",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                Description =
                    "Nested virtualization allows virtual machines to run their own hypervisor and create nested VMs which can be used to create isolated execution environments that bypass host security monitoring. Blocking nested virtualization prevents the use of VM-within-VM configurations that increase complexity and make security monitoring and policy enforcement more difficult. Nested virtualization is exploited in some containerization attacks where attackers use nested Hyper-V to create isolated containers that bypass host security controls. The reduced visibility into nested VM operations makes incident investigation significantly more difficult when security events originate from within nested environments. Organizations that have specific legitimate requirements for nested virtualization should isolate those systems and apply additional monitoring rather than broadly enabling nested virtualization. Testing and development environments that require nested virtualization for specific use cases should be treated as high-risk systems with compensating controls.",
                Tags = ["hyper-v", "nested-virtualization", "vm-security", "isolation", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "BlockNestedVirtualization", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockNestedVirtualization")],
                DetectOps = [RegOp.CheckDword(Key, "BlockNestedVirtualization", 1)],
            },
            new TweakDef
            {
                Id = "virtz-configure-vm-memory-protection",
                Label = "Configure Enhanced Memory Protection for Hyper-V Virtual Machines",
                Category = "Display — Wia Image Acquisition",
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 5,
                Description =
                    "Enhanced memory protection for Hyper-V VMs ensures that virtual machine memory is isolated from unauthorized access by other VMs and the host management OS in ways that go beyond standard hypervisor isolation. Hypervisor-protected code integrity uses the hypervisor security boundary to protect kernel memory from modification by malicious code running in the VM. Memory protection features ensure that VM memory cannot be directly accessed by processes on the host even when the host has Hyper-V management privileges without going through the hypervisor management API. Shielded VMs provide the highest level of memory protection by encrypting VM memory and preventing host administrators from directly inspecting VM memory contents. Organizations that run VMs with sensitive workloads including regulated data or privileged authentication components should evaluate the appropriate level of memory protection for each VM type. Regular security reviews of VM memory protection configuration ensure that protection levels remain appropriate as VM workloads and threat models evolve.",
                Tags = ["hyper-v", "memory-protection", "vm-isolation", "hypervisor-security", "policy"],
                RegistryKeys = [Key],
                ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedMemoryProtection", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedMemoryProtection")],
                DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedMemoryProtection", 1)],
            },
        ];
    }
}
