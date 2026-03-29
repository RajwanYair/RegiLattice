// RegiLattice.Core — Tweaks/NicTeamingPolicy.cs
// NIC Teaming (LBFO), adapter bonding, and network team management policy — Sprint 500.
// Category: "NIC Teaming Policy" | Slug: nicteam
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\NICTeaming

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class NicTeamingPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\NICTeaming";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "nicteam-block-team-creation",
                Label = "Block NIC Team Creation by Standard Users",
                Category = "NIC Teaming Policy",
                Description =
                    "Prevents standard (non-administrator) users from creating new NIC teams (LBFO load-balancing / failover adapters), ensuring that network adapter bonding configurations are controlled exclusively by administrators.",
                Tags = ["nic-teaming", "lbfo", "adapter", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team creation blocked for standard users; LBFO adapter bonding requires admin rights.",
                ApplyOps = [RegOp.SetDword(Key, "AllowTeamCreationByNonAdmin", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "AllowTeamCreationByNonAdmin")],
                DetectOps = [RegOp.CheckDword(Key, "AllowTeamCreationByNonAdmin", 0)],
            },
            new TweakDef
            {
                Id = "nicteam-require-admin-for-deletion",
                Label = "Require Admin to Delete NIC Teams",
                Category = "NIC Teaming Policy",
                Description =
                    "Requires administrator privileges to delete NIC teams, preventing accidental or malicious destruction of load-balancing or failover network configurations by standard users or malicious scripts.",
                Tags = ["nic-teaming", "lbfo", "deletion", "admin", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team deletion requires admin rights; standard users cannot destroy bonded adapter configurations.",
                ApplyOps = [RegOp.SetDword(Key, "RequireAdminForTeamDeletion", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "RequireAdminForTeamDeletion")],
                DetectOps = [RegOp.CheckDword(Key, "RequireAdminForTeamDeletion", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-set-teaming-mode-static",
                Label = "Set Default NIC Teaming Mode to Static (Switch Independent)",
                Category = "NIC Teaming Policy",
                Description =
                    "Sets the default NIC teaming mode to Switch Independent (no LACP negotiation), which does not require switch-side port aggregation configuration and works with any managed switch that allows multiple ports to the same host.",
                Tags = ["nic-teaming", "lbfo", "teaming-mode", "switch-independent", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Default NIC teaming mode set to Switch Independent; LACP negotiation not required on the switch.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultTeamingMode", 0)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultTeamingMode")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultTeamingMode", 0)],
            },
            new TweakDef
            {
                Id = "nicteam-enable-team-health-logging",
                Label = "Enable NIC Team Health Change Event Logging",
                Category = "NIC Teaming Policy",
                Description =
                    "Enables event log entries for NIC team health state changes including member adapter failures, additions, and team-wide operational state changes for proactive failover monitoring.",
                Tags = ["nic-teaming", "lbfo", "health", "event-log", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team health state changes logged; adapter failures and team state changes recorded in event log.",
                ApplyOps = [RegOp.SetDword(Key, "EnableTeamHealthEventLogging", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableTeamHealthEventLogging")],
                DetectOps = [RegOp.CheckDword(Key, "EnableTeamHealthEventLogging", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-disable-team-ui",
                Label = "Disable NIC Teaming Configuration UI for Standard Users",
                Category = "NIC Teaming Policy",
                Description =
                    "Removes the NIC Teaming page from Server Manager and Network Connections for non-administrator users, preventing accidental or unauthorised modification of NIC team configurations.",
                Tags = ["nic-teaming", "lbfo", "ui", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NIC Teaming UI hidden from standard users; only admins can access teaming configuration.",
                ApplyOps = [RegOp.SetDword(Key, "DisableTeamingUIForNonAdmin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableTeamingUIForNonAdmin")],
                DetectOps = [RegOp.CheckDword(Key, "DisableTeamingUIForNonAdmin", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-set-load-balance-dynamic",
                Label = "Set Default NIC Team Load Balancing Mode to Dynamic",
                Category = "NIC Teaming Policy",
                Description =
                    "Sets the default NIC team load balancing algorithm to Dynamic mode, which distributes outbound traffic based on TCP/UDP flow measurements and periodically rebalances to prevent hot-spotting on a single team member.",
                Tags = ["nic-teaming", "lbfo", "load-balancing", "dynamic", "performance", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team default load balancing set to Dynamic; flow-aware rebalancing across team members.",
                ApplyOps = [RegOp.SetDword(Key, "DefaultLBAlgorithm", 2)],
                RemoveOps = [RegOp.DeleteValue(Key, "DefaultLBAlgorithm")],
                DetectOps = [RegOp.CheckDword(Key, "DefaultLBAlgorithm", 2)],
            },
            new TweakDef
            {
                Id = "nicteam-set-standby-adapter-failover",
                Label = "Set Default NIC Team Standby Adapter Mode for Failover",
                Category = "NIC Teaming Policy",
                Description =
                    "Configures the default NIC team to use an active-standby topology where one adapter is always idle as a hot standby, ensuring seamless failover with no traffic disruption when the primary adapter fails.",
                Tags = ["nic-teaming", "lbfo", "failover", "standby", "resilience", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team standby mode enabled; one adapter held as hot standby for seamless failover on primary failure.",
                ApplyOps = [RegOp.SetDword(Key, "EnableStandbyAdapterFailover", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "EnableStandbyAdapterFailover")],
                DetectOps = [RegOp.CheckDword(Key, "EnableStandbyAdapterFailover", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-block-team-membership-change",
                Label = "Block Standard Users from Modifying NIC Team Membership",
                Category = "NIC Teaming Policy",
                Description =
                    "Prevents standard users from adding adapters to or removing adapters from existing NIC teams, ensuring team membership changes can only be made by administrators.",
                Tags = ["nic-teaming", "lbfo", "membership", "standard-user", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team membership modification blocked for standard users; only admins add/remove team members.",
                ApplyOps = [RegOp.SetDword(Key, "BlockMembershipChangeByNonAdmin", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "BlockMembershipChangeByNonAdmin")],
                DetectOps = [RegOp.CheckDword(Key, "BlockMembershipChangeByNonAdmin", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-audit-team-cfg-changes",
                Label = "Audit NIC Team Configuration Changes",
                Category = "NIC Teaming Policy",
                Description =
                    "Enables Security event log entries for all NIC team configuration changes (create, delete, member add/remove, mode change), providing a change-management audit trail for network team availability configurations.",
                Tags = ["nic-teaming", "lbfo", "audit", "configuration-change", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "NIC team config changes audited; create/delete/membership events logged for change management.",
                ApplyOps = [RegOp.SetDword(Key, "AuditTeamConfigChanges", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "AuditTeamConfigChanges")],
                DetectOps = [RegOp.CheckDword(Key, "AuditTeamConfigChanges", 1)],
            },
            new TweakDef
            {
                Id = "nicteam-disable-nicteam-telemetry",
                Label = "Disable NIC Teaming Telemetry Reporting to Microsoft",
                Category = "NIC Teaming Policy",
                Description =
                    "Prevents the NIC Teaming subsystem from sending adapter bonding performance and health telemetry to Microsoft, protecting internal network adapter topology from cloud disclosure.",
                Tags = ["nic-teaming", "telemetry", "privacy", "microsoft", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "NIC Teaming telemetry to Microsoft disabled; adapter bonding topology not sent to cloud.",
                ApplyOps = [RegOp.SetDword(Key, "DisableNICTeamingTelemetry", 1)],
                RemoveOps = [RegOp.DeleteValue(Key, "DisableNICTeamingTelemetry")],
                DetectOps = [RegOp.CheckDword(Key, "DisableNICTeamingTelemetry", 1)],
            },
        ];
}
