// RegiLattice.Core — Tweaks/ModernStandbyPolicy.cs
// Modern Standby (S0 Low Power Idle) Group Policy controls — Sprint 376.
// Category: "Modern Standby Policy" | Slug: mstandby
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\ModernStandby
//                 HKLM\SOFTWARE\Policies\Microsoft\Power
// MinBuild: 18362 (Windows 10 1903+ — Modern Standby widely available from this build)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class ModernStandbyPolicy
{
    private const string MsKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\ModernStandby";
    private const string PwrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power\PowerSettings";
    private const string PwrSleepKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Power";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "mstandby-disable-connected-standby",
            Label = "Disable Modern Standby (S0 Low-Power Idle) — Use S3 Sleep",
            Category = "Modern Standby Policy",
            Description = "Disables Modern Standby (S0ix) and falls back to the traditional S3 sleep state. S0 keeps the network and background apps active during sleep, which can interfere with security tools, drain battery unexpectedly, and create wake-on-network attack surfaces.",
            Tags = ["modern-standby", "s0", "s3-sleep", "power", "disable"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 4,
            SafetyRating = 3,
            ImpactNote = "Forces S3 sleep where hardware supports it. Network and background activity cease during sleep — improves battery life on older HW but disables instant-on and wake-on-LAN in S0. Some OEM hardware only supports S0 and cannot fall back.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "AllowStandby", 0)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "AllowStandby")],
            DetectOps = [RegOp.CheckDword(MsKey, "AllowStandby", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-block-network-during-standby",
            Label = "Block Network Activity During Modern Standby",
            Category = "Modern Standby Policy",
            Description = "Prevents the NIC from remaining active and processing network packets while the device is in Modern Standby. Reduces the attack surface from wake-on-LAN exploitation, rogue DHCP offers, and directed broadcast attacks arriving while the user is away.",
            Tags = ["modern-standby", "network", "wifi", "attack-surface", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Push notifications, live tiles, and scheduled background sync will not occur while the device is in standby. Recommended for shared, high-security environments.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "NetworkActivityAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "NetworkActivityAllowed")],
            DetectOps = [RegOp.CheckDword(MsKey, "NetworkActivityAllowed", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-disable-smart-standby",
            Label = "Disable Adaptive Smart Standby Adjustments",
            Category = "Modern Standby Policy",
            Description = "Disables the intelligent standby system that dynamically adjusts deep-sleep exit rates based on historical usage patterns. When disabled, the system uses fixed configured timeouts rather than ML-driven adaptive transitions.",
            Tags = ["modern-standby", "adaptive", "smart-standby", "power", "predictable"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Produces deterministic standby behaviour at the cost of optimal power efficiency. Useful for kiosk and fixed-use devices where predictable power cycling is preferred.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "DisableSmartStandby", 1)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "DisableSmartStandby")],
            DetectOps = [RegOp.CheckDword(MsKey, "DisableSmartStandby", 1)],
        },
        new TweakDef
        {
            Id = "mstandby-disable-background-tasks-in-standby",
            Label = "Disable Background Task Execution During Modern Standby",
            Category = "Modern Standby Policy",
            Description = "Prevents application background tasks from running while the system is in Modern Standby. Background tasks in S0 consume battery, can trigger wake-locks that prevent deep sleep, and may leak user data via cloud sync while the device appears powered off.",
            Tags = ["modern-standby", "background-tasks", "battery", "privacy", "s0"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Suppresses background app refresh during standby; notifications and cloud sync resume on user wake. Significantly improves battery life on devices with aggressive background app models.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "AllowBackgroundTasksInStandby")],
            DetectOps = [RegOp.CheckDword(MsKey, "AllowBackgroundTasksInStandby", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-disable-maintenance-in-standby",
            Label = "Disable Automatic Maintenance Execution During Standby",
            Category = "Modern Standby Policy",
            Description = "Prevents the Windows Automatic Maintenance scheduler from running maintenance tasks (Disk Defrag, Windows Defender scans, app updates) while the device is in Modern Standby. Avoids unexpected disk I/O, CPU wake, and battery drain during standby periods.",
            Tags = ["modern-standby", "maintenance", "automatic-maintenance", "battery", "scheduling"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Maintenance tasks (including Defender scans) will defer to the next active session. Track that maintenance completes during awake sessions to avoid indefinite deferral.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "AllowMaintenanceDuringStandby")],
            DetectOps = [RegOp.CheckDword(MsKey, "AllowMaintenanceDuringStandby", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-require-fast-startup-disabled",
            Label = "Disable Hybrid Shutdown / Fast Startup (Hiberboot)",
            Category = "Modern Standby Policy",
            Description = "Disables Hybrid Shutdown (Fast Startup / Hiberboot) which persists kernel session to the hibernate file across reboots. Hiberboot bypasses full driver reinitialisation and can leave security tools in stale state; full cold boot is safer and more predictable.",
            Tags = ["modern-standby", "fast-startup", "hiberboot", "hibernate", "cold-boot"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Shutdown/startup will be slightly slower but every boot is a clean cold boot. Recommended for compliance-sensitive environments and shared machines.",
            RegistryKeys = [PwrKey],
            ApplyOps  = [RegOp.SetDword(PwrKey, "HiberbootEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(PwrKey, "HiberbootEnabled")],
            DetectOps = [RegOp.CheckDword(PwrKey, "HiberbootEnabled", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-set-idle-standby-timeout",
            Label = "Set Plugged-In Idle-to-Standby Timeout to 30 Minutes",
            Category = "Modern Standby Policy",
            Description = "Configures the AC (plugged-in) idle timeout before the system enters Modern Standby or sleep to 30 minutes (1800 seconds). Reduces the window in which an unattended unlocked workstation is physically accessible before it locks and suspends.",
            Tags = ["modern-standby", "idle-timeout", "screen-lock", "power", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "30-minute AC idle timeout before sleep is a reasonable physical-security baseline for workstations. Pairs with screen lock and credential timeout policies.",
            RegistryKeys = [PwrSleepKey],
            ApplyOps  = [RegOp.SetDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
            RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "IdleTimeoutAC")],
            DetectOps = [RegOp.CheckDword(PwrSleepKey, "IdleTimeoutAC", 1800)],
        },
        new TweakDef
        {
            Id = "mstandby-block-wake-timers-in-standby",
            Label = "Block Programmatic Wake Timers During Modern Standby",
            Category = "Modern Standby Policy",
            Description = "Prevents applications and scheduled tasks from setting wake timers that force the system out of Modern Standby. Rogue or poorly coded applications can use wake timers to keep the device powered on continuously; blocking timers enforces true standby.",
            Tags = ["modern-standby", "wake-timer", "power", "battery", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Blocks all application-set wake timers during standby. Windows Update maintenance wake timers are a notable exception — it may still wake for critical updates depending on policy.",
            RegistryKeys = [PwrSleepKey],
            ApplyOps  = [RegOp.SetDword(PwrSleepKey, "AllowWakeTimers", 0)],
            RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "AllowWakeTimers")],
            DetectOps = [RegOp.CheckDword(PwrSleepKey, "AllowWakeTimers", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-disable-wol-in-standby",
            Label = "Disable Wake-on-LAN During Modern Standby",
            Category = "Modern Standby Policy",
            Description = "Prevents the NIC from processing Wake-on-LAN (WoL) magic packets while the device is in Modern Standby. Eliminates the network-based remote-wake attack surface; an attacker with network access cannot remotely wake and attack the device.",
            Tags = ["modern-standby", "wake-on-lan", "wol", "network", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Disables WoL in standby; remote power-on via network magic packet will not work while in S0. BIOS/UEFI WoL may override this — also disable WoL there for full protection.",
            RegistryKeys = [MsKey],
            ApplyOps  = [RegOp.SetDword(MsKey, "WakeOnLanAllowed", 0)],
            RemoveOps = [RegOp.DeleteValue(MsKey, "WakeOnLanAllowed")],
            DetectOps = [RegOp.CheckDword(MsKey, "WakeOnLanAllowed", 0)],
        },
        new TweakDef
        {
            Id = "mstandby-require-password-on-resume",
            Label = "Require Password When Resuming from Modern Standby",
            Category = "Modern Standby Policy",
            Description = "Forces credential re-entry when the device resumes from Modern Standby or sleep. Without this policy the screen may stay unlocked after resume, exposing the session to physical access attacks on shared or temporarily unattended machines.",
            Tags = ["modern-standby", "password-resume", "screen-lock", "credential", "physical-security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 18362,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Ensures the screen is locked on every standby resume, requiring Windows Hello or password to re-enter the session. This is a standard physical-security baseline.",
            RegistryKeys = [PwrSleepKey],
            ApplyOps  = [RegOp.SetDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
            RemoveOps = [RegOp.DeleteValue(PwrSleepKey, "PromptPasswordOnWakeup")],
            DetectOps = [RegOp.CheckDword(PwrSleepKey, "PromptPasswordOnWakeup", 1)],
        },
    ];
}
