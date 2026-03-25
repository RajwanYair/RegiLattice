// RegiLattice.Core — Tweaks/RestartManagerPolicy.cs
// Sprint 291: Restart Manager Policy tweaks (10 tweaks)
// Category: "Restart Manager Policy" | Slug: rstmgr
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RestartManager

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class RestartManagerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\RestartManager";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "rstmgr-disable-restart-manager",
            Label = "Disable Restart Manager",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Restart Manager coordinates with applications during software installation to minimize system restarts by restarting only affected processes. Disabling Restart Manager forces software installers to require full system reboots rather than selective process restarts. This policy is appropriate for environments where installers cannot be trusted to coordinate correctly through the Restart Manager API. Some security tools prefer full reboots to partial process restarts to ensure complete state refresh after security updates. Installers in enterprise environments are typically tested and certified to work correctly with or without Restart Manager. Administrators should evaluate installer behavior before broadly disabling this feature across a fleet.",
            Tags = ["restart-manager", "installation", "reboot", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRestartManager", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRestartManager")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRestartManager", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-disable-app-relaunch",
            Label = "Disable Application Relaunch After Restart",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Restart Manager can relaunch applications that were running before a system restart, attempting to restore the pre-restart state. Disabling app relaunch prevents applications from being automatically started by Restart Manager after the system returns from a reboot. Enterprise environments with managed startup sequences controlled through logon scripts and Group Policy prefer deterministic application startup. Unexpected application relaunch can interfere with security tools, time-sensitive workflows, and session initialization scripts. This policy ensures that the post-restart application state is controlled exclusively by the configured startup infrastructure. Users can still manually restart their applications after reboot without any functional limitation.",
            Tags = ["restart-manager", "relaunch", "startup", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAppRelaunch", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAppRelaunch")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAppRelaunch", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-allow-mitigations",
            Label = "Allow Restart Manager Mitigations",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Restart Manager mitigations provide fallback behaviors when applications cannot be restarted gracefully during software installation. Keeping mitigations enabled at their default safe state ensures Restart Manager can handle edge cases in application shutdown sequences. This policy sets DisableMitigations to zero, preserving the protective fallback behaviors built into the Restart Manager framework. Applications that do not respond to shutdown requests during installation benefit from these mitigation strategies. Disabling mitigations can cause installer hangs when applications refuse to terminate during the shutdown phase. The default mitigation behavior represents the most resilient configuration for diverse application environments.",
            Tags = ["restart-manager", "mitigations", "installation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMitigations", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMitigations")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMitigations", 0)],
        },
        new TweakDef
        {
            Id = "rstmgr-disable-telemetry",
            Label = "Disable Restart Manager Telemetry",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "Restart Manager telemetry collects data about shutdown sequences, application restart success rates, and installation coordination outcomes. This information is transmitted to Microsoft to improve installer compatibility and Restart Manager behavior in future Windows versions. Disabling this telemetry prevents installation process metadata from leaving the enterprise environment. Organizations with data egress monitoring policies benefit from eliminating telemetry streams from system components. Restart Manager continues to function identically regardless of whether telemetry is enabled or disabled. Administrative audit requirements can be met through event log monitoring rather than external telemetry.",
            Tags = ["restart-manager", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRestartManagerTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRestartManagerTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRestartManagerTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-disable-session-registration",
            Label = "Disable Restart Manager Session Registration",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Restart Manager session registration allows applications and services to register themselves for coordination during installation shutdowns. Disabling session registration prevents applications from enrolling in the Restart Manager coordination protocol. This is appropriate for environments where all software is deployed through enterprise deployment tools that manage their own process termination. Applications running in containerized or isolated environments do not benefit from cross-process Restart Manager coordination. Disabling registration simplifies the installation pipeline by removing dependencies on the Restart Manager inter-process communication channel. Standard application functionality is not affected since session registration is only relevant during software installation.",
            Tags = ["restart-manager", "registration", "installation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableSessionRegistration", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableSessionRegistration")],
            DetectOps = [RegOp.CheckDword(Key, "DisableSessionRegistration", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-set-shutdown-timeout",
            Label = "Set Service Shutdown Timeout 30 Seconds",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "The shutdown timeout determines how long Restart Manager waits for services to respond to shutdown requests during software installation. Configuring a 30-second timeout ensures that the installation process does not wait indefinitely for unresponsive services. Services that fail to shut down within 30 seconds are forcibly terminated, allowing the installation to proceed without hanging. This timeout value balances giving legitimate services adequate time to shut down gracefully against preventing indefinite installation stalls. Environments with large enterprise services that require extended shutdown sequences may need to increase this timeout. The value should be coordinated with observed shut-down times for the largest services running on the target systems.",
            Tags = ["restart-manager", "timeout", "installation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "ShutdownTimeout", 30)],
            RemoveOps = [RegOp.DeleteValue(Key, "ShutdownTimeout")],
            DetectOps = [RegOp.CheckDword(Key, "ShutdownTimeout", 30)],
        },
        new TweakDef
        {
            Id = "rstmgr-disable-reboot-notification",
            Label = "Disable Reboot Notification",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 4,
            Description =
                "Restart Manager generates user-facing notifications when a pending reboot is required after software installation. Disabling reboot notifications suppresses these prompts, delegating reboot scheduling to enterprise patch management tools. Environments using SCCM, Intune, or similar platforms manage reboot windows through maintenance windows and compliance policies. Unsuppressed reboot notifications can prompt users to restart at inopportune times, interrupting active work sessions. Enterprise maintenance window policies ensure reboots occur during off-hours without disrupting productivity. Disabling these notifications requires that the enterprise patch management platform reliably handles all reboot coordination.",
            Tags = ["restart-manager", "notifications", "reboot", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableRebootNotification", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableRebootNotification")],
            DetectOps = [RegOp.CheckDword(Key, "DisableRebootNotification", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-disable-service-restart",
            Label = "Disable Service Restart by Restart Manager",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 3,
            Description =
                "Restart Manager can automatically restart Windows services after shutting them down to facilitate software installation without a full system reboot. Disabling service restart by Restart Manager prevents services from being bounced automatically during installation sequences. Enterprise services such as security agents, database services, and monitoring agents may have complex restart dependencies that Restart Manager cannot honor. Forcibly restarting services can cause data loss or transaction failures if the service has in-flight operations at shutdown time. Organizations with strict service availability requirements prefer to control all service restarts through documented runbook procedures. This setting is appropriate for environments where unplanned service restarts pose operational or data integrity risks.",
            Tags = ["restart-manager", "services", "installation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableServiceRestart", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableServiceRestart")],
            DetectOps = [RegOp.CheckDword(Key, "DisableServiceRestart", 1)],
        },
        new TweakDef
        {
            Id = "rstmgr-zero-max-service-shutdown-wait",
            Label = "Set Max Service Shutdown Wait to Zero",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 3,
            Description =
                "The maximum service shutdown wait time specifies how long Restart Manager waits for each individual service to complete its shutdown before force-terminating it. Setting this to zero removes a policy-defined maximum wait, reverting to operating system default timeout behavior for service shutdown. This prevents a policy that forces overly aggressive service termination from conflicting with services requiring longer graceful shutdown sequences. Services with extensive state serialization or active network connections may require several seconds to shut down cleanly without data corruption. Removing the policy-imposed maximum ensures the default operating system timeout governs service shutdown behavior. This setting is appropriate when the default Windows service control manager timeout is preferred over a policy override.",
            Tags = ["restart-manager", "services", "timeout", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "MaxServiceShutdownWaitSeconds", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "MaxServiceShutdownWaitSeconds")],
            DetectOps = [RegOp.CheckDword(Key, "MaxServiceShutdownWaitSeconds", 0)],
        },
        new TweakDef
        {
            Id = "rstmgr-allow-graceful-shutdown",
            Label = "Allow Graceful Shutdown Behavior",
            Category = "Restart Manager Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Graceful shutdown behavior in Restart Manager allows services and applications to receive proper shutdown notifications and complete in-progress operations before termination. Keeping this behavior enabled at its default value ensures applications can safely persist state and close open file handles during installation sequences. This policy sets DisableGracefulShutdown to zero, preserving the safe default of allowing orderly application and service termination. Ungraceful termination can leave files in inconsistent states, corrupt databases, and cause post-installation errors that require manual cleanup. Graceful shutdown is particularly important for applications maintaining write transactions at the time of shutdown. This critical safety mechanism should remain enabled in all enterprise configurations.",
            Tags = ["restart-manager", "shutdown", "stability", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableGracefulShutdown", 0)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableGracefulShutdown")],
            DetectOps = [RegOp.CheckDword(Key, "DisableGracefulShutdown", 0)],
        },
    ];
}
