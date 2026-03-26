// RegiLattice.Core — Tweaks/HotpatchUpdatePolicy.cs
// Windows Hotpatch update policy controls — Sprint 375.
// Category: "Hotpatch Update Policy" | Slug: hotpatch
// Registry paths: HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\Hotpatch
//                 HKLM\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate
// MinBuild: 26100 (Windows 11 24H2+ — Hotpatch available on Copilot+ / Azure Edition)
namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HotpatchUpdatePolicy
{
    private const string HotpatchKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate\Hotpatch";
    private const string WuKey       = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WindowsUpdate";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id = "hotpatch-enable-hotpatch-updates",
            Label = "Enable Windows Hotpatch (Live Kernel Patching)",
            Category = "Hotpatch Update Policy",
            Description = "Enables Windows Hotpatch, which installs security patches directly into running kernel and system process memory without requiring a reboot. Dramatically reduces downtime for critical servers and VMs while keeping them current.",
            Tags = ["hotpatch", "live-patching", "kernel", "windows-update", "reboot-less"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 5,
            SafetyRating = 4,
            ImpactNote = "Enables in-memory patching for monthly quality updates without reboots. Requires Windows 11 24H2+ or Azure Edition VMs. Baseline updates still require occasional reboots quarterly.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotPatch", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotPatch")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotPatch", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-disable-hotpatch-updates",
            Label = "Disable Windows Hotpatch Updates",
            Category = "Hotpatch Update Policy",
            Description = "Administratively disables the Hotpatch update channel, reverting the device to the traditional monthly Update Tuesday update cycle that installs patches via a reboot. Suitable for environments that require deterministic full-restart update cycles.",
            Tags = ["hotpatch", "disable", "windows-update", "patching", "control"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Reverts to standard reboot-required patching; ensures full restart cycle occurs each month. No security risk from disabling Hotpatch as long as devices are patched via regular WU channel.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotPatch", 0)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotPatch")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotPatch", 0)],
        },
        new TweakDef
        {
            Id = "hotpatch-require-code-integrity",
            Label = "Require Code Integrity Validation for Hotpatch Modules",
            Category = "Hotpatch Update Policy",
            Description = "Enforces Authenticode signature verification for every Hotpatch module before it is loaded into kernel memory. Prevents unsigned or tampered patches from being applied even if a threat actor gains WU delivery access.",
            Tags = ["hotpatch", "code-integrity", "signature", "authenticode", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Strong defence-in-depth: only Microsoft-signed hotpatch binaries can be applied. Has no impact on legitimate Microsoft patches; all Microsoft hotpatches are signed.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "RequireCodeIntegrity", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "RequireCodeIntegrity")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "RequireCodeIntegrity", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-block-rollback",
            Label = "Block Hotpatch Rollback to Unpatched State",
            Category = "Hotpatch Update Policy",
            Description = "Prevents administrators and automated tools from rolling back applied hotpatch modules to a pre-patched kernel state. Ensures regulatory compliance environments maintain a continuous patched state.",
            Tags = ["hotpatch", "rollback", "compliance", "integrity", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Blocking rollback ensures continuous kernel-level protection but may complicate incident response if a hotpatch introduces a regression. Test thoroughly before enforcing.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "BlockHotpatchRollback", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "BlockHotpatchRollback")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "BlockHotpatchRollback", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-audit-patch-events",
            Label = "Enable Hotpatch Apply and Fail Event Auditing",
            Category = "Hotpatch Update Policy",
            Description = "Enables detailed event logging for every Hotpatch application attempt, whether successful or failed. Events include the patch identifier, timestamp, module hash, and failure reason code for SIEM ingestion.",
            Tags = ["hotpatch", "audit", "event-log", "siem", "monitoring"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Non-disruptive; only adds event log entries. Essential for organisations with change-management and patch-tracking compliance requirements.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "EnableHotpatchAudit", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "EnableHotpatchAudit")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "EnableHotpatchAudit", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-limit-max-deferred-reboots",
            Label = "Limit Maximum Reboots Deferred by Hotpatch to 2 Baseline Periods",
            Category = "Hotpatch Update Policy",
            Description = "Caps the number of consecutive Update Tuesday cycles that Hotpatch can defer a baseline (reboot-required) update. After the configured number of hotpatch-only cycles, a baseline restart is mandated to consolidate all patches.",
            Tags = ["hotpatch", "baseline-reboot", "deferred-restart", "patch-cycle", "control"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 4,
            ImpactNote = "Prevents indefinite deferral of reboot-required baselines. Allows 2 hotpatch months before a mandatory restart, balancing uptime and update discipline.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "MaxDeferredBaselineRestarts")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "MaxDeferredBaselineRestarts", 2)],
        },
        new TweakDef
        {
            Id = "hotpatch-schedule-baseline-restart",
            Label = "Schedule Mandatory Baseline Restart Outside Business Hours",
            Category = "Hotpatch Update Policy",
            Description = "Configures hotpatch baseline restarts to occur outside defined active hours (default: 2:00 AM), avoiding interruption of user sessions. When a baseline reboot is required, it is deferred to the next maintenance window.",
            Tags = ["hotpatch", "baseline-reboot", "active-hours", "maintenance-window", "scheduling"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Configures restart timing to 2 AM UTC; pairs with the WU active hours policy to keep machines updated without disrupting users.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ScheduleBaselineRestartHour")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "ScheduleBaselineRestartHour", 2)],
        },
        new TweakDef
        {
            Id = "hotpatch-disable-telemetry-upload",
            Label = "Disable Hotpatch Telemetry Upload to Microsoft",
            Category = "Hotpatch Update Policy",
            Description = "Prevents the Hotpatch subsystem from uploading patch application telemetry, timing data, and failure diagnostics to Microsoft. Retains telemetry locally in the event log only for internal analysis.",
            Tags = ["hotpatch", "telemetry", "privacy", "diagnostic-data", "cloud"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Reduces data shared with Microsoft about kernel patching events. Does not affect hotpatch functionality or reliability; purely a data outflow control.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "DisableHotpatchTelemetry")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "DisableHotpatchTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-exclude-driver-updates",
            Label = "Exclude Driver Updates from Hotpatch Delivery Channel",
            Category = "Hotpatch Update Policy",
            Description = "Restricts the Hotpatch delivery channel to security patches only, excluding optional and driver updates. Driver changes often require a full reboot for hardware initialisation; delivering them via Hotpatch risks incomplete initialisation.",
            Tags = ["hotpatch", "driver-updates", "exclusion", "windows-update", "stability"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Prevents unexpected driver-level changes during a hotpatch cycle; driver updates fall through to the next reboot-requiring WU pass.",
            RegistryKeys = [HotpatchKey],
            ApplyOps  = [RegOp.SetDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
            RemoveOps = [RegOp.DeleteValue(HotpatchKey, "ExcludeDriversFromHotpatch")],
            DetectOps = [RegOp.CheckDword(HotpatchKey, "ExcludeDriversFromHotpatch", 1)],
        },
        new TweakDef
        {
            Id = "hotpatch-require-managed-device-enrollment",
            Label = "Require Managed Device Enrollment for Hotpatch Activation",
            Category = "Hotpatch Update Policy",
            Description = "Permits Hotpatch activation only on devices enrolled in a compatible MDM solution (Intune, MEM). Unmanaged devices fall back to the standard WU reboot channel. Ensures compliance-tracking for reboot-free patch deployments.",
            Tags = ["hotpatch", "mdm", "intune", "device-enrollment", "compliance"],
            NeedsAdmin = true,
            CorpSafe = true,
            MinBuild = 26100,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Ties Hotpatch enrollment to MDM compliance posture; non-enrolled devices are not eligible. Useful for enterprise environments tracking update compliance via Intune.",
            RegistryKeys = [WuKey],
            ApplyOps  = [RegOp.SetDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
            RemoveOps = [RegOp.DeleteValue(WuKey, "RequireManagedDeviceForHotpatch")],
            DetectOps = [RegOp.CheckDword(WuKey, "RequireManagedDeviceForHotpatch", 1)],
        },
    ];
}
