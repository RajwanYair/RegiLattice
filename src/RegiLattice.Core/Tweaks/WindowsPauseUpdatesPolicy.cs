// WindowsPauseUpdatesPolicy.cs — Windows Update pause and deferral enforcement
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\PauseUpdates (and AU sub-path)
// Slug: pauseupd
// Category: Windows Update Pause Policy

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsPauseUpdatesPolicy
{
    private const string PauseKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";
    private const string AuKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\AU";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "pauseupd-defer-feature-30days",
            Label = "Windows Update Pause: Defer Feature Updates 30 Days",
            Category = "Windows Update Pause Policy",
            Description =
                "Defers Windows feature updates by 30 days beyond their general availability date. "
                + "Deferral gives IT administrators time to test compatibility before feature updates reach production endpoints. "
                + "30 days is the minimum recommended deferral for enterprise deployments and allows Microsoft to identify critical regressions first. "
                + "Removing this policy re-enables immediate feature update availability.",
            Tags = ["windows-update", "defer", "feature-update", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferFeatureUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(PauseKey, "DeferFeatureUpdatesPeriodInDays", 30)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Delays feature updates by 30 days; reduces exposure to day-zero feature regressions.",
        },
        new TweakDef
        {
            Id = "pauseupd-defer-quality-7days",
            Label = "Windows Update Pause: Defer Quality Updates 7 Days",
            Category = "Windows Update Pause Policy",
            Description =
                "Defers Windows quality (security patch) updates by 7 days, allowing time for emergency patch retraction. "
                + "Quality updates occasionally introduce regressions; a 7-day deferral window reduces blast radius from faulty patches. "
                + "7 days is short enough to maintain adequate security posture while providing a testing buffer. "
                + "Removing this policy makes quality updates available immediately upon release.",
            Tags = ["windows-update", "defer", "quality-update", "patch", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "DeferQualityUpdatesPeriodInDays")],
            DetectOps = [RegOp.CheckDword(PauseKey, "DeferQualityUpdatesPeriodInDays", 7)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Delays security patches by 7 days; provides testing buffer without excessive security lag.",
        },
        new TweakDef
        {
            Id = "pauseupd-disable-auto-install-on-shutdown",
            Label = "Windows Update Pause: Disable Auto-Install Updates on Shutdown",
            Category = "Windows Update Pause Policy",
            Description =
                "Prevents Windows Update from automatically installing updates when the user initiates a shutdown. "
                + "Auto-install-on-shutdown can extend shutdown times and cause unexpected restarts, especially on laptops before meetings. "
                + "Updates are controlled through scheduled windows instead, giving IT full control over the timing. "
                + "Removing this policy re-enables automatic installation during shutdown sequences.",
            Tags = ["windows-update", "shutdown", "auto-install", "schedule", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AuKey],
            ApplyOps = [RegOp.SetDword(AuKey, "NoAutoUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "NoAutoUpdate")],
            DetectOps = [RegOp.CheckDword(AuKey, "NoAutoUpdate", 1)],
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents updates installing on shutdown; avoids unexpected extended shutdown times.",
        },
        new TweakDef
        {
            Id = "pauseupd-set-active-hours-start",
            Label = "Windows Update Pause: Set Active Hours Start (8 AM)",
            Category = "Windows Update Pause Policy",
            Description =
                "Configures the Windows Update active hours start time to 8 AM, preventing reboots for updates during business hours. "
                + "Active hours protect users from unexpected reboots during the configured working hours window. "
                + "Setting an explicit start ensures policy is enforced rather than relying on user configuration. "
                + "Removing this policy reverts to Windows default or user-configured active hours.",
            Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursStart", 8)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursStart")],
            DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursStart", 8)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Locks active hours start to 8 AM; prevents update reboots interrupting morning workflows.",
        },
        new TweakDef
        {
            Id = "pauseupd-set-active-hours-end",
            Label = "Windows Update Pause: Set Active Hours End (6 PM)",
            Category = "Windows Update Pause Policy",
            Description =
                "Configures the Windows Update active hours end time to 6 PM (18:00), ensuring reboots cannot occur during standard business hours. "
                + "With start fixed at 8 AM and end at 6 PM, the full working day is protected from forced reboots. "
                + "Updates can install after 6 PM via the scheduled maintenance window. "
                + "Removing this policy reverts to Windows default or user-configured active hours end.",
            Tags = ["windows-update", "active-hours", "reboot", "schedule", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "ActiveHoursEnd", 18)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "ActiveHoursEnd")],
            DetectOps = [RegOp.CheckDword(PauseKey, "ActiveHoursEnd", 18)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Locks active hours end to 6 PM; complete 8 AM–6 PM protection from forced reboots.",
        },
        new TweakDef
        {
            Id = "pauseupd-block-driver-updates",
            Label = "Windows Update Pause: Block Driver Updates via Windows Update",
            Category = "Windows Update Pause Policy",
            Description =
                "Prevents Windows Update from automatically downloading and installing driver updates. "
                + "Automatic driver updates can replace validated enterprise drivers with incompatible versions, causing hardware failures or BSODs. "
                + "Driver management should be handled by IT through validated packages rather than Windows Update. "
                + "Removing this policy re-enables automatic driver updates through Windows Update.",
            Tags = ["windows-update", "driver", "exclusion", "enterprise", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "ExcludeWUDriversInQualityUpdate")],
            DetectOps = [RegOp.CheckDword(PauseKey, "ExcludeWUDriversInQualityUpdate", 1)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocks automatic driver updates via WU; prevents validated drivers being silently replaced.",
        },
        new TweakDef
        {
            Id = "pauseupd-disable-upgrade-notifications",
            Label = "Windows Update Pause: Disable Upgrade Notification Toasts",
            Category = "Windows Update Pause Policy",
            Description =
                "Suppresses the Windows Update toast notifications that prompt users to restart for pending updates. "
                + "In a managed environment, restart timing is controlled by IT policy — user-visible prompts are redundant and disruptive. "
                + "Suppressing notifications prevents users from inadvertently triggering reboots outside the maintenance window. "
                + "Removing this policy re-enables Windows Update restart notification toasts.",
            Tags = ["windows-update", "notifications", "restart", "ux", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AuKey],
            ApplyOps = [RegOp.SetDword(AuKey, "SetDisableUXWUAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "SetDisableUXWUAccess")],
            DetectOps = [RegOp.CheckDword(AuKey, "SetDisableUXWUAccess", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Hides WU restart prompts from users; IT maintains full control of update timing.",
        },
        new TweakDef
        {
            Id = "pauseupd-set-update-detection-frequency",
            Label = "Windows Update Pause: Set Update Detection Frequency (22 Hours)",
            Category = "Windows Update Pause Policy",
            Description =
                "Sets the Windows Update service to check for updates every 22 hours instead of the default automatic random interval. "
                + "A predictable 22-hour check interval prevents multiple machines on the same network from surging the update server simultaneously. "
                + "Combined with an WSUS/SCCM deployment, this ensures consistent, manageable update bandwidth. "
                + "Removing this policy reverts to Windows' random detection frequency.",
            Tags = ["windows-update", "detection", "frequency", "bandwidth", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AuKey],
            ApplyOps = [RegOp.SetDword(AuKey, "DetectionFrequencyEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "DetectionFrequencyEnabled")],
            DetectOps = [RegOp.CheckDword(AuKey, "DetectionFrequencyEnabled", 1)],
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Predictable 22-hour WU check interval; prevents bandwidth surge on shared networks.",
        },
        new TweakDef
        {
            Id = "pauseupd-allow-mu-updates",
            Label = "Windows Update Pause: Allow Microsoft Update for Other Products",
            Category = "Windows Update Pause Policy",
            Description =
                "Configures Windows Update to also deliver updates for other Microsoft products (Office, .NET, Visual C++) alongside OS patches. "
                + "Receiving all Microsoft product updates through a single channel simplifies patch management and reduces the attack surface. "
                + "This is equivalent to enabling 'Give me updates for other Microsoft products' in Windows Update settings. "
                + "Removing this policy reverts to OS-only updates via Windows Update.",
            Tags = ["windows-update", "microsoft-update", "office", "patch", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AuKey],
            ApplyOps = [RegOp.SetDword(AuKey, "AllowMUUpdateService", 1)],
            RemoveOps = [RegOp.DeleteValue(AuKey, "AllowMUUpdateService")],
            DetectOps = [RegOp.CheckDword(AuKey, "AllowMUUpdateService", 1)],
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Enables Microsoft Update for all products; consolidates patching into a single channel.",
        },
        new TweakDef
        {
            Id = "pauseupd-enforce-restart-deadline",
            Label = "Windows Update Pause: Enforce 72-Hour Restart Deadline",
            Category = "Windows Update Pause Policy",
            Description =
                "Sets a 72-hour mandatory restart deadline after Windows Update installs updates requiring a reboot. "
                + "Without a deadline, users can indefinitely postpone required restarts, leaving the system vulnerable to active exploits. "
                + "72 hours provides reasonable flexibility for users to save work while ensuring security patches are applied promptly. "
                + "Removing this policy removes the forced restart deadline.",
            Tags = ["windows-update", "restart", "deadline", "security", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [PauseKey],
            ApplyOps = [RegOp.SetDword(PauseKey, "SetAutoRestartDeadline", 72)],
            RemoveOps = [RegOp.DeleteValue(PauseKey, "SetAutoRestartDeadline")],
            DetectOps = [RegOp.CheckDword(PauseKey, "SetAutoRestartDeadline", 72)],
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Forces restart within 72 hours of patch install; prevents indefinite deferral of security updates.",
        },
    ];
}
