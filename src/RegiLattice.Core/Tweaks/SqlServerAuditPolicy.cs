// RegiLattice.Core — Tweaks/SqlServerAuditPolicy.cs
// SQL Server Security & Audit Configuration — Sprint 433.
// Controls SQL Server instance-level registry settings for login auditing,
// authentication mode enforcement, network protocol hardening, and error reporting.
// Category: "SQL Server Audit Policy" | Slug: sqlaup
// Registry paths:
//   HKLM\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer         — instance configuration
//   HKLM\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\SuperSocketNetLib — network protocols

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class SqlServerAuditPolicy
{
    private const string InstanceKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer";
    private const string NetLibKey   = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\MSSQLServer\MSSQLServer\SuperSocketNetLib";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id          = "sqlaup-enable-full-login-audit",
                Label       = "Enable Full SQL Server Login Audit (Success + Failure)",
                Category    = "SQL Server Audit Policy",
                Description = "Sets AuditLevel=3 in the MSSQLServer instance key. Controls the level of SQL Server login auditing: 0=none, 1=success only, 2=failure only, 3=both success and failure. Full auditing (level 3) records every authentication attempt to the SQL error log, enabling detection of brute-force attacks and unauthorised access. Required by most security compliance frameworks (CIS SQL Server Benchmark, STIG).",
                Tags        = ["sql-server", "audit", "login", "compliance", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 5,
                ImpactNote  = "Logs every SQL login attempt; increases SQL error log size on high-connection-rate servers.",
                ApplyOps    = [RegOp.SetDword(InstanceKey, "AuditLevel", 3)],
                RemoveOps   = [RegOp.DeleteValue(InstanceKey, "AuditLevel")],
                DetectOps   = [RegOp.CheckDword(InstanceKey, "AuditLevel", 3)],
            },
            new TweakDef
            {
                Id          = "sqlaup-enforce-windows-auth-only",
                Label       = "Enforce Windows Authentication Only for SQL Server",
                Category    = "SQL Server Audit Policy",
                Description = "Sets LoginMode=1 in the MSSQLServer instance key. Restricts SQL Server to Windows Authentication (Integrated Security) mode only, disabling SQL Server login accounts (LoginMode=2 enables mixed mode). Windows Authentication uses Kerberos or NTLM, benefits from Active Directory password policies, is audited by Windows Security event logs, and eliminates the risk of weak SQL-only passwords.",
                Tags        = ["sql-server", "authentication", "windows-auth", "security", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote  = "Disables SQL login accounts; applications using SQL usernames/passwords must be migrated to Windows Auth first.",
                ApplyOps    = [RegOp.SetDword(InstanceKey, "LoginMode", 1)],
                RemoveOps   = [RegOp.DeleteValue(InstanceKey, "LoginMode")],
                DetectOps   = [RegOp.CheckDword(InstanceKey, "LoginMode", 1)],
            },
            new TweakDef
            {
                Id          = "sqlaup-disable-named-pipes",
                Label       = "Disable SQL Server Named Pipe Protocol",
                Category    = "SQL Server Audit Policy",
                Description = "Sets NpEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Named Pipes network protocol for SQL Server connections. Named Pipes traverses SMB and can expose the SQL Server service through Windows file-sharing ports (445/TCP). Disabling Named Pipes forces all connections through TCP/IP which can be precisely port-filtered by a firewall.",
                Tags        = ["sql-server", "network", "named-pipes", "protocol", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote  = "Drops Named Pipes support; local applications using np: connection strings must switch to tcp:.",
                ApplyOps    = [RegOp.SetDword(NetLibKey, "NpEnabled", 0)],
                RemoveOps   = [RegOp.DeleteValue(NetLibKey, "NpEnabled")],
                DetectOps   = [RegOp.CheckDword(NetLibKey, "NpEnabled", 0)],
            },
            new TweakDef
            {
                Id          = "sqlaup-disable-shared-memory",
                Label       = "Disable SQL Server Shared Memory Protocol",
                Category    = "SQL Server Audit Policy",
                Description = "Sets SmEnabled=0 in the SQL Server SuperSocketNetLib key. Disables the Shared Memory protocol that allows local processes to connect to SQL Server via memory-mapped communication. While convenient, Shared Memory connections bypass network-layer access controls entirely. Disabling it forces all connections (even local) through explicit TCP/IP, ensuring firewall rules and port-level controls apply uniformly.",
                Tags        = ["sql-server", "network", "shared-memory", "protocol", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote  = "Drops Shared Memory; local automated tools and T-SQL jobs using shared memory connections must use TCP instead.",
                ApplyOps    = [RegOp.SetDword(NetLibKey, "SmEnabled", 0)],
                RemoveOps   = [RegOp.DeleteValue(NetLibKey, "SmEnabled")],
                DetectOps   = [RegOp.CheckDword(NetLibKey, "SmEnabled", 0)],
            },
            new TweakDef
            {
                Id          = "sqlaup-enable-tcp-protocol",
                Label       = "Ensure SQL Server TCP/IP Protocol Is Enabled",
                Category    = "SQL Server Audit Policy",
                Description = "Sets TcpEnabled=1 in the SQL Server SuperSocketNetLib key. Guarantees the TCP/IP network protocol is active for SQL Server, which is the only protocol that can be properly firewalled and port-filtered. Combined with disabling Named Pipes and Shared Memory, this ensures all SQL Server traffic traverses TCP so network access controls are consistently applied.",
                Tags        = ["sql-server", "network", "tcp", "protocol", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote  = "Ensures TCP is enabled; no business impact if TCP was already active (the common default).",
                ApplyOps    = [RegOp.SetDword(NetLibKey, "TcpEnabled", 1)],
                RemoveOps   = [RegOp.DeleteValue(NetLibKey, "TcpEnabled")],
                DetectOps   = [RegOp.CheckDword(NetLibKey, "TcpEnabled", 1)],
            },
            new TweakDef
            {
                Id          = "sqlaup-hide-sql-instance",
                Label       = "Hide SQL Server Instance from Network Browsers",
                Category    = "SQL Server Audit Policy",
                Description = "Sets HideInstance=1 in the MSSQLServer key. Instructs SQL Server Browser to not return the instance name in response to network enumeration requests. When hidden, clients must supply the explicit server name and port; they cannot discover it through SQL Server Browser UDP broadcasts. This reduces the attack surface by preventing automated scanners from locating the SQL instance via port 1434 UDP enumeration.",
                Tags        = ["sql-server", "browser", "discovery", "network", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote  = "Hides instance from SQL Browser; connection strings must specify host\\instance explicitly.",
                ApplyOps    = [RegOp.SetDword(InstanceKey, "HideInstance", 1)],
                RemoveOps   = [RegOp.DeleteValue(InstanceKey, "HideInstance")],
                DetectOps   = [RegOp.CheckDword(InstanceKey, "HideInstance", 1)],
            },
            new TweakDef
            {
                Id          = "sqlaup-disable-xp-cmdshell-flag",
                Label       = "Record xp_cmdshell Disabled State in Registry",
                Category    = "SQL Server Audit Policy",
                Description = "Sets XPCmdShellEnabled=0 in the MSSQLServer key. This registry flag indicates that the xp_cmdshell extended stored procedure (which executes OS shell commands from T-SQL) must remain disabled. While the authoritative control is sp_configure inside SQL Server, recording the intended state in the registry allows compliance scanning tools that audit registry keys to verify xp_cmdshell is disabled without querying the SQL instance directly.",
                Tags        = ["sql-server", "xp-cmdshell", "compliance", "policy", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 4,
                SafetyRating = 5,
                ImpactNote  = "Registry flag only; xp_cmdshell must also be disabled via sp_configure inside SQL Server for full protection.",
                ApplyOps    = [RegOp.SetDword(InstanceKey, "XPCmdShellEnabled", 0)],
                RemoveOps   = [RegOp.DeleteValue(InstanceKey, "XPCmdShellEnabled")],
                DetectOps   = [RegOp.CheckDword(InstanceKey, "XPCmdShellEnabled", 0)],
            },
            new TweakDef
            {
                Id          = "sqlaup-enable-error-reporting",
                Label       = "Enable SQL Server Error Log Verbosity",
                Category    = "SQL Server Audit Policy",
                Description = "Sets NumErrorLogs=10 in the MSSQLServer key. Controls how many SQL Server error log files are retained in rotation. Increasing from the default (6) to 10 prevents aggressive error log cycling that could make forensic investigation of incidents difficult. Retaining more log cycles ensures a longer audit trail is available when a security incident is discovered days or weeks after it occurred.",
                Tags        = ["sql-server", "error-log", "audit", "retention", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote  = "Retains 10 rotated error log files instead of 6; negligible additional disk usage.",
                ApplyOps    = [RegOp.SetDword(InstanceKey, "NumErrorLogs", 10)],
                RemoveOps   = [RegOp.DeleteValue(InstanceKey, "NumErrorLogs")],
                DetectOps   = [RegOp.CheckDword(InstanceKey, "NumErrorLogs", 10)],
            },
            new TweakDef
            {
                Id          = "sqlaup-disable-olap-remote-connect",
                Label       = "Disable SQL Server OLAP Remote Connections Flag",
                Category    = "SQL Server Audit Policy",
                Description = "Sets AllowRemoteConnections=0 in the SQL Server SuperSocketNetLib key. Disables incoming remote connections through the OLAP/Analysis Services network library path. When SQL Server Analysis Services is not deployed or when OLAP connectivity should be restricted to the local machine, disabling remote connections through this protocol handler reduces the network-exposed attack surface of the SQL Server installation.",
                Tags        = ["sql-server", "olap", "remote", "network", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote  = "Blocks OLAP remote connections; Analysis Services remote clients must connect via explicit TCP/IP instead.",
                ApplyOps    = [RegOp.SetDword(NetLibKey, "AllowRemoteConnections", 0)],
                RemoveOps   = [RegOp.DeleteValue(NetLibKey, "AllowRemoteConnections")],
                DetectOps   = [RegOp.CheckDword(NetLibKey, "AllowRemoteConnections", 0)],
            },
            new TweakDef
            {
                Id          = "sqlaup-enable-sql-server-encryption",
                Label       = "Enable SQL Server Force Encryption Flag",
                Category    = "SQL Server Audit Policy",
                Description = "Sets ForceEncryption=1 in the SQL Server SuperSocketNetLib key. Instructs SQL Server to require encrypted connections (TLS/SSL) for all client connections. Without forced encryption, clients may connect without TLS, transmitting queries and data in plaintext across the network. This registry flag mirrors the Force Encryption option in SQL Server Configuration Manager and should be set alongside a valid server certificate.",
                Tags        = ["sql-server", "encryption", "tls", "network", "hardening"],
                NeedsAdmin  = true,
                CorpSafe    = true,
                ImpactScore = 5,
                SafetyRating = 3,
                ImpactNote  = "Forces TLS on all connections; client connection strings must trust the SQL Server certificate or connections will fail.",
                ApplyOps    = [RegOp.SetDword(NetLibKey, "ForceEncryption", 1)],
                RemoveOps   = [RegOp.DeleteValue(NetLibKey, "ForceEncryption")],
                DetectOps   = [RegOp.CheckDword(NetLibKey, "ForceEncryption", 1)],
            },
        ];
}
