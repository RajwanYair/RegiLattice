namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SnmpPolicy
{
    private const string SnmpKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters";
    private const string AgentKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\ValidCommunities";
    private const string MgrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\SNMP\Parameters\PermittedManagers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "snmppol-enable-auth-traps",
            Label = "SNMP Policy: Enable Authentication Failure Traps",
            Category = "SNMP Policy",
            Description = "Sends SNMP authentication failure traps when unauthorized community string requests are received. Enables monitoring of unauthorized SNMP access attempts.",
            Tags = ["snmp", "auth", "traps", "monitoring", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "EnableAuthenticationTraps", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnableAuthenticationTraps")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "EnableAuthenticationTraps", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-restrict-permitted-managers",
            Label = "SNMP Policy: Restrict Permitted Management Hosts",
            Category = "SNMP Policy",
            Description = "Enforces GPO-defined list of permitted SNMP management hosts. The SNMP service only responds to requests from the hosts listed under PermittedManagers registry key.",
            Tags = ["snmp", "access-control", "managers", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [MgrKey],
            ApplyOps = [RegOp.SetString(MgrKey, "1", "localhost")],
            RemoveOps = [RegOp.DeleteValue(MgrKey, "1")],
            DetectOps = [RegOp.CheckString(MgrKey, "1", "localhost")],
        },
        new TweakDef
        {
            Id = "snmppol-disable-community-readonly",
            Label = "SNMP Policy: Remove Default Public Read-Only Community",
            Category = "SNMP Policy",
            Description = "Removes the 'public' SNMP community string from the valid communities list. The default public community string is a well-known attack vector that enables SNMP enumeration.",
            Tags = ["snmp", "community", "public", "hardening", "network", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AgentKey],
            ApplyOps = [RegOp.SetDword(AgentKey, "public", 0)],
            RemoveOps = [RegOp.DeleteValue(AgentKey, "public")],
            DetectOps = [RegOp.CheckDword(AgentKey, "public", 0)],
        },
        new TweakDef
        {
            Id = "snmppol-set-community-read-only",
            Label = "SNMP Policy: Restrict Community String Permissions (Read-Only)",
            Category = "SNMP Policy",
            Description = "Sets the SNMP community string type to Read Only (4) and removes Write/Create/Delete rights. Prevents SNMP-based configuration changes from network management stations.",
            Tags = ["snmp", "community", "read-only", "permissions", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AgentKey],
            ApplyOps = [RegOp.SetDword(AgentKey, "private", 4)],
            RemoveOps = [RegOp.DeleteValue(AgentKey, "private")],
            DetectOps = [RegOp.CheckDword(AgentKey, "private", 4)],
        },
        new TweakDef
        {
            Id = "snmppol-disable-snmp-writeable",
            Label = "SNMP Policy: Disable SNMP Write Community Access",
            Category = "SNMP Policy",
            Description = "Sets the community permissions to None (1) for the write community, disabling any SNMP SET operations. SNMP write access allows remote configuration changes to network devices.",
            Tags = ["snmp", "write", "set-operations", "hardening", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [AgentKey],
            ApplyOps = [RegOp.SetDword(AgentKey, "write", 1)],
            RemoveOps = [RegOp.DeleteValue(AgentKey, "write")],
            DetectOps = [RegOp.CheckDword(AgentKey, "write", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-enable-snmp-service-policy",
            Label = "SNMP Policy: Enable SNMP Service Policy Enforcement",
            Category = "SNMP Policy",
            Description = "Enables GPO-based enforcement of SNMP service settings. When enabled, all SNMP service configuration is governed by Group Policy, overriding local service settings.",
            Tags = ["snmp", "gpo", "enforcement", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "EnforceSNMPPolicy", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "EnforceSNMPPolicy")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "EnforceSNMPPolicy", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-disable-snmp-v1",
            Label = "SNMP Policy: Disable SNMPv1 Protocol",
            Category = "SNMP Policy",
            Description = "Disables SNMPv1 through GPO policy. SNMPv1 transmits community strings in plain text and lacks encryption or authentication. Disabling it forces use of SNMPv2c or SNMPv3.",
            Tags = ["snmp", "v1", "legacy", "protocol", "hardening", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "DisableSNMPv1", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "DisableSNMPv1")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "DisableSNMPv1", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-log-auth-failures",
            Label = "SNMP Policy: Log Authentication Failures to Event Log",
            Category = "SNMP Policy",
            Description = "Configures the SNMP service to write authentication failure events to the Windows Security event log. Supports Security Information and Event Management (SIEM) integration.",
            Tags = ["snmp", "logging", "event-log", "auth", "siem", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "LogAuthFailures", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "LogAuthFailures")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "LogAuthFailures", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-block-snmp-from-internet",
            Label = "SNMP Policy: Block SNMP from Public Network Access",
            Category = "SNMP Policy",
            Description = "Restricts SNMP to internal network connections only through GPO. Forces the SNMP service to discard any requests arriving from non-private network interfaces.",
            Tags = ["snmp", "firewall", "network", "internet", "restriction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "BlockPublicNetworkAccess")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "BlockPublicNetworkAccess", 1)],
        },
        new TweakDef
        {
            Id = "snmppol-restrict-trap-receivers",
            Label = "SNMP Policy: Restrict SNMP Trap Receivers to Known Hosts",
            Category = "SNMP Policy",
            Description = "Applies GPO-enforced filtering to SNMP trap destinations, limiting trap broadcasts to administrator-approved management systems. Reduces SNMP trap amplification risk.",
            Tags = ["snmp", "traps", "trap-receivers", "network", "restriction", "policy"],
            NeedsAdmin = true,
            CorpSafe = true,
            RegistryKeys = [SnmpKey],
            ApplyOps = [RegOp.SetDword(SnmpKey, "RestrictTrapReceivers", 1)],
            RemoveOps = [RegOp.DeleteValue(SnmpKey, "RestrictTrapReceivers")],
            DetectOps = [RegOp.CheckDword(SnmpKey, "RestrictTrapReceivers", 1)],
        },
    ];
}
