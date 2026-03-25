// RegiLattice.Core — Tweaks/AppSiloPolicy.cs
// Sprint 349: App Silo Policy tweaks (10 tweaks)
// Category: "App Silo Policy" | Slug: appsiloa
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSilo

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppSiloPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSilo";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appsiloa-enable-silo-isolation",
            Label = "Enable App Silo Process Isolation for Privileged Process Containers",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "App Silo provides process isolation through Windows container primitives creating secure execution environments that limit the blast radius of application vulnerabilities. Enabling App Silo isolation restricts processes to their assigned containers preventing cross-process access to sensitive system resources and other application data. Application silos use Windows isolated namespaces and ACLs to prevent silo processes from accessing objects outside their container boundary. Process containment through silos limits the effectiveness of memory-based attacks like heap sprays and use-after-free vulnerabilities by preventing access to objects in other containers. Organizations should evaluate App Silo isolation for high-risk applications like browser components email clients and document processors that commonly execute untrusted content. Silo-isolated processes have reduced access to the system providing defense-in-depth against application exploitation.",
            Tags = ["app-silo", "isolation", "container", "process-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSiloIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSiloIsolation", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-restrict-silo-network-access",
            Label = "Restrict Silo Network Access to Allowlisted Endpoints",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "App Silo network access restriction limits the network endpoints that isolated processes can communicate with preventing data exfiltration through unauthorized network connections. Restricting silo network access to allowlisted endpoints contains the damage from application compromise by preventing the compromised process from reaching attacker-controlled infrastructure. Network isolation for application silos is implemented through Windows Filtering Platform rules that block silo processes from accessing unauthorized network destinations. Allowlist-based network access provides more precise control than broad network restrictions ensuring that legitimate application functionality is not impaired. Organizations should define network access allowlists based on the specific network endpoints each application requires for its legitimate business function. Monitoring for silo processes attempting to access blocked network destinations provides detection for application exploitation attempts.",
            Tags = ["app-silo", "network-isolation", "allowlist", "exfiltration-prevention", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSiloNetworkAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloNetworkAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSiloNetworkAccess", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-block-silo-registry-writes",
            Label = "Block Silo Processes from Writing to System Registry Hives",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Silo registry isolation prevents processes inside application silos from writing to system registry hives outside their container virtual registry namespace. Blocking silo registry writes prevents application exploits from achieving persistence through registry modifications or compromising system configuration. Registry virtualization within silos allows applications to read system registry keys while writes are redirected to a per-silo virtual registry that does not affect the system. Applications that legitimately need to write to system registry locations must have those specific requirements accommodated through policy exceptions. Silo registry isolation is particularly effective at preventing malware injected into silo processes from establishing persistence through standard registry-based persistence mechanisms. Monitoring for attempt to bypass silo registry restrictions provides detection for advanced attackers who attempt to escape the container.",
            Tags = ["app-silo", "registry-isolation", "persistence-prevention", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockSystemRegistryWrites", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockSystemRegistryWrites")],
            DetectOps = [RegOp.CheckDword(Key, "BlockSystemRegistryWrites", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-restrict-silo-filesystem-access",
            Label = "Restrict Silo Processes to Designated File System Namespaces",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "File system namespace isolation for application silos restricts silo processes to only accessing file system locations within their designated namespace preventing access to sensitive system files. Restricting file system access contains the impact of application compromise by preventing exploited processes from reading sensitive system data or user files outside their container. File system isolation uses Windows NTFS ACLs and namespace virtualization to provide silo processes with a view of only the file system locations they need. Applications that access user documents will have appropriate user namespace access while being blocked from accessing system directories and other users' files. File system isolation is particularly effective against path traversal and directory traversal attacks that attempt to access files outside the application's intended scope. Organizations should test file system restrictions with each silo-enabled application to verify that legitimate file access requirements are met.",
            Tags = ["app-silo", "filesystem-isolation", "namespace", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictFileSystemNamespace", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictFileSystemNamespace")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictFileSystemNamespace", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-enable-silo-audit-logging",
            Label = "Enable Audit Logging for App Silo Isolation Events",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "App Silo audit logging captures events related to silo creation termination and security boundary violations providing forensic data for incident response. Enabling silo audit logging creates visibility into application behavior within containers that can be analyzed to detect exploitation and containment violations. Silo boundary violation events indicate that a process attempted to access resources outside its container which is a strong indicator of application exploitation followed by sandbox escape attempts. Audit events from silo operations should be forwarded to SIEM with alerting on boundary violation events. Regular review of silo audit data helps identify applications that frequently attempt boundary violations which may indicate poorly configured silo policies or active attacks. Silo audit data combined with process execution monitoring provides comprehensive coverage for detecting application exploitation.",
            Tags = ["app-silo", "audit", "boundary-violations", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSiloAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSiloAuditLogging", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-restrict-silo-token-privileges",
            Label = "Restrict Security Token Privileges for Silo Process Execution",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Token privilege restriction for silo processes removes elevated Windows privileges from application execution tokens limiting the capabilities available to exploit code running within the silo. Restricting token privileges reduces the severity of exploitation by preventing compromise code from leveraging privileges like SeDebugPrivilege or SeImpersonatePrivilege that enable lateral movement. Privilege reduction through token stripping is a well-established security technique that limits escalation paths even after an application vulnerability has been exploited. Organizations should remove all privileges not required for the specific application's legitimate function from the silo execution token. Token privilege restriction is complementary to process integrity levels and AppContainer restrictions providing multiple layers of privilege limitation. Applications requiring specific privileges for legitimate functions should have only those specific privileges allowed rather than running with full default token privileges.",
            Tags = ["app-silo", "token-privileges", "privilege-reduction", "exploitation-mitigation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictTokenPrivileges", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictTokenPrivileges")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictTokenPrivileges", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-enable-silo-integrity-monitoring",
            Label = "Enable File Integrity Monitoring for App Silo Container Contents",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "App Silo file integrity monitoring tracks changes to files within the silo container namespace detecting unauthorized modifications that may indicate persistence establishment by exploit code. Enabling silo integrity monitoring creates alerts when silo container files are modified outside the normal application update path. Exploit code that achieves code execution within a silo may attempt to persist by modifying application files or configuration within the container namespace. Integrity monitoring establishes a baseline of expected container file states and reports deviations that require investigation. Organizations should configure integrity monitoring baselines after initial application deployment and after each authorized application update. Integrity alerts for silo containers should have high priority in security monitoring as they indicate potential exploitation success requiring investigation.",
            Tags = ["app-silo", "integrity-monitoring", "persistence-detection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableIntegrityMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableIntegrityMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "EnableIntegrityMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-restrict-silo-object-access",
            Label = "Restrict Silo Access to Named Objects and Synchronization Primitives",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Named object restriction prevents silo processes from accessing Windows named objects like mutexes events and shared memory sections that belong to other processes outside the silo. Restricting silo named object access prevents inter-process communication channels that could be used for silo escape or cross-process exploitation. Named objects are commonly used for IPC between applications and malicious use of these channels is a container escape technique. Silo isolation of named objects requires that silo processes use their own namespace for named objects rather than the global namespace shared by all processes. Applications that use inter-process communication with components outside the silo need policy exceptions that allow the specific named objects required. Monitoring for silo access to named objects in the global namespace helps detect container escape attempts that use IPC channels.",
            Tags = ["app-silo", "named-objects", "ipc", "container-escape", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictNamedObjectAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictNamedObjectAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictNamedObjectAccess", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-set-silo-memory-limit",
            Label = "Set Memory Usage Limits for App Silo Process Containers",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Memory limits for application silos prevent individual containers from consuming excessive system memory which could cause denial of service by starving other processes or the OS kernel. Setting silo memory limits to appropriate values for the application's expected workload contains memory-based denial of service attacks within the container. Applications that contain memory leaks or run untrusted content that causes memory inflation can be contained to their limit preventing system-wide impact. Memory limit enforcement through Windows job objects is the mechanism used by silos to enforce per-container memory consumption. Organizations should set memory limits based on observed peak memory usage of each silo application with a reasonable headroom above normal peak. Memory limit violations that trigger container termination should be alerted on as they may indicate exploitation or memory-based denial of service within the container.",
            Tags = ["app-silo", "memory-limits", "denial-of-service", "resource-control", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableMemoryLimits", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableMemoryLimits")],
            DetectOps = [RegOp.CheckDword(Key, "EnableMemoryLimits", 1)],
        },
        new TweakDef
        {
            Id = "appsiloa-enable-silo-crash-reporting",
            Label = "Enable Crash Reporting for App Silo Security Boundary Failure Analysis",
            Category = "App Silo Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Silo crash reporting captures crash dumps and failure information when silo processes terminate abnormally enabling investigation of potential exploitation attempts. Enabling crash reporting for silos provides forensic data about the state of the process at the time of termination for analysis of exploitation attempts. Crash dumps from silo processes may contain exploitation artifacts like return-oriented programming chains or heap spray patterns that confirm an exploitation attempt occurred. Organizations should configure silo crash reporting to generate mini-dumps or full dumps depending on the sensitivity of the data processed in the silo. Crash dumps should not be transmitted to external services for silos that process sensitive data as the dump may contain the sensitive data. Regular analysis of silo crash patterns helps identify applications under active exploitation attack.",
            Tags = ["app-silo", "crash-reporting", "forensics", "exploitation-detection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSiloCrashReporting", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloCrashReporting")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSiloCrashReporting", 1)],
        },
    ];
}
