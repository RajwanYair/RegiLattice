// RegiLattice.Core — Tweaks/AppSiloAdvPolicy.cs
// Sprint 362: App Silo Advanced Policy tweaks (10 tweaks)
// Category: "App Silo Advanced Policy" | Slug: appsilob
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSiloAdvanced

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class AppSiloAdvPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\AppSiloAdvanced";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "appsilob-enforce-ipc-isolation-between-silos",
            Label = "Enforce Inter-Process Communication Isolation Between Application Silos",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing IPC isolation between application silos prevents applications running in separate silos from communicating through unnamed pipes shared memory and other inter-process communication channels that could be used to exchange unauthorized data or coordinate cross-silo attacks. App silos are designed to create isolation boundaries between applications and IPC channels that cross silo boundaries undermine the isolation guarantee. Applications with legitimate requirements to communicate across silo boundaries should use explicitly declared and audited IPC channels that are subject to appropriate authorization checks. Cross-silo IPC attempts that are blocked by isolation enforcement should generate audit events for security analysis. Silo-aware applications should be designed to use silo-isolated IPC mechanisms rather than system-wide named objects that are visible across all silos. Organizations should review application compatibility testing for IPC isolation policies before deployment to ensure that approved cross-silo communication patterns are correctly exempted.",
            Tags = ["app-silo", "ipc-isolation", "inter-process", "application-isolation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceIPCIsolationBetweenSilos", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceIPCIsolationBetweenSilos")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceIPCIsolationBetweenSilos", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-restrict-silo-exit-control-flow",
            Label = "Restrict Control Flow Exits from Application Silo to Host Environment",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting control flow exits from application silos prevents applications running in silos from executing code injection or DLL injection attacks that target processes running in the host environment outside the silo boundary. Control flow exit restrictions ensure that execution cannot jump from silo-isolated application code into host system code or other silo code through return-oriented programming or code injection techniques. Kernel-enforced control flow restrictions complement user-mode silo enforcement to prevent privilege escalation through exploiting the boundary between silo and host code execution contexts. Applications that require tight integration with host OS features should use approved well-defined API channels rather than direct code execution in host contexts. Control flow integrity enforcement within silos should be combined with host-to-silo boundary checks to create a bidirectional enforcement model. Security testing for silo control flow restrictions should include red team testing of known control flow substitution techniques to validate the effectiveness of enforcement.",
            Tags = ["app-silo", "control-flow", "code-injection", "silo-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSiloExitControlFlow", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloExitControlFlow")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSiloExitControlFlow", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-enable-silo-resource-monitoring",
            Label = "Enable Advanced Resource Usage Monitoring for Application Silo Environments",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Advanced resource monitoring for application silos tracks CPU memory disk and network resource consumption at the silo level providing visibility into anomalous resource use that may indicate malicious activity within a silo. Silos that consume abnormally high resources may be running cryptomining workloads or performing intensive data processing operations not consistent with the silo's declared workload. Resource monitoring data for silos should be integrated with behavioral analytics to establish baselines and detect deviations that warrant investigation. Memory consumption anomalies within silos can indicate buffer overflow exploitation or excessive memory mapping that suggests vulnerability exploitation is in progress. CPU spike analysis within silo contexts can identify cryptomining or computation-intensive malware execution that falls below whole-system thresholds but exceeds per-silo baselines. Silo resource monitoring should be collected with sufficient granularity and retention to support incident investigation when anomalies are detected.",
            Tags = ["app-silo", "resource-monitoring", "behavioral-analytics", "anomaly-detection", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSiloResourceMonitoring", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSiloResourceMonitoring")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSiloResourceMonitoring", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-enforce-silo-network-namespace",
            Label = "Enforce Dedicated Network Namespace Isolation for Application Silos",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing dedicated network namespace isolation for application silos prevents applications within silos from directly accessing network resources visible to the host system or other silos without explicit authorization through defined network paths. Network namespace isolation ensures that silo applications cannot enumerate or access network resources outside their defined scope preventing lateral movement from a compromised silo to other network hosts. Silos with internet-facing components should be isolated in network namespaces that have controlled internet access policies preventing them from accessing internal network resources if compromised. Network traffic entering and exiting silo network namespaces should be inspected by network security controls implemented at the namespace boundary. Silo-specific firewall rules should ensure that network connections are limited to the endpoints required for the silo application's function. Monitoring of cross-namespace network traffic allows detection of silo breakout attempts that try to establish connections to unauthorized network destinations.",
            Tags = ["app-silo", "network-namespace", "network-isolation", "lateral-movement", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSiloNetworkNamespace", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloNetworkNamespace")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSiloNetworkNamespace", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-block-silo-kernel-object-access",
            Label = "Block Application Silo Access to Unauthorized Kernel Objects",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 4,
            Description =
                "Blocking silo access to kernel objects outside the silo's authorized namespace prevents applications from reading or modifying kernel objects owned by the host or other silos which could be used to manipulate system behavior or extract sensitive information. Kernel objects including events mutexes semaphores and named pipes that cross the silo boundary create potential channels for silo breakout if their security descriptors are not appropriately restrictive. Access to kernel objects in the global namespace should be blocked for silo applications with only silo-local namespace objects accessible by default. Vulnerability classes like kernel object abuse become significantly more difficult to exploit when applications within silos cannot access the kernel objects that would normally be targets. Kernel object access control should be audited to detect silo applications attempting to access kernel objects outside their authorized scope. Exceptions for approved cross-silo kernel object access should be explicitly defined and reviewed as part of the silo security architecture.",
            Tags = ["app-silo", "kernel-objects", "namespace-isolation", "object-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockSiloKernelObjectAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockSiloKernelObjectAccess")],
            DetectOps = [RegOp.CheckDword(Key, "BlockSiloKernelObjectAccess", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-restrict-silo-registry-scope",
            Label = "Restrict Application Silo Registry Access to Silo-Scoped Registry Namespace",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting silo registry access to the silo-scoped registry namespace prevents applications running in silos from reading or modifying host registry keys that control system configuration security settings and credentials. Registry key access from silos to host registry paths including HKLM system configuration keys and HKCU user preference keys should require explicit authorization. Malicious applications running in silos can use unrestricted registry access to read sensitive configuration data including stored credentials encryption keys and security policy settings. Silo registry namespace restrictions ensure that each silo has an isolated view of registry state preventing cross-silo information disclosure through shared registry key access. Changes to registry keys in the silo registry should not persist to the host registry unless an explicit registry redirection policy allows the specific key path. Audit events for silo registry access attempts outside the authorized scope should be reviewed to detect applications attempting to read host configuration data.",
            Tags = ["app-silo", "registry-isolation", "silo-namespace", "configuration-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSiloRegistryScope", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloRegistryScope")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSiloRegistryScope", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-enforce-silo-security-event-logging",
            Label = "Enforce Security Event Logging for Application Silo Policy Violations",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Enforcing security event logging for silo policy violations ensures that all attempts to violate silo isolation boundaries including unauthorized IPC attempts registry access requests and network namespace violations are captured in the security event log. Silo violation events provide intelligence about activities within silos that may indicate malware execution attempting to break out of containment or explore the host environment. Security event logging for silo violations should use a consistent event schema that includes silo identifier violating process identity violation type and targeted resource to support effective investigation. Silo security events should be forwarded to centralized SIEM alongside endpoint detection and response telemetry for correlation with broader security event patterns. High-volume silo violation events may indicate active exploitation attempts and should trigger elevated security monitoring responses. Retention of silo security events should match the organizational security audit log retention policy rather than defaulting to shorter operational log retention periods.",
            Tags = ["app-silo", "security-events", "audit-logging", "compliance", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSiloSecurityEventLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloSecurityEventLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSiloSecurityEventLogging", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-block-cross-silo-token-inheritance",
            Label = "Block Security Token Inheritance Across Application Silo Boundaries",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Blocking security token inheritance across silo boundaries prevents applications that spawn processes across silo boundaries from passing elevated or privileged security tokens to processes running in other silos that should have different security contexts. Cross-silo token inheritance creates privilege escalation paths where a process running in a restricted silo could gain the privileges of a token inherited from a higher-privileged process in another silo. Token inheritance blocking ensures that processes created in a silo always have tokens appropriate to that silo's security context rather than inheriting tokens from callers outside the silo. Process creation across silo boundaries should require explicit privilege assignment through the silo security policy rather than automatic token inheritance. Applications that legitimately need to create processes in other silos with specific security contexts should use the approved silo process creation API with explicit token specification. Security testing for cross-silo token inheritance should verify that token stripping is enforced consistently across all process creation mechanisms.",
            Tags = ["app-silo", "token-inheritance", "privilege-control", "cross-silo", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockCrossSiloTokenInheritance", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockCrossSiloTokenInheritance")],
            DetectOps = [RegOp.CheckDword(Key, "BlockCrossSiloTokenInheritance", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-restrict-silo-debug-capabilities",
            Label = "Restrict Debugger Attachment Capabilities Within Application Silo Environments",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting debugger attachment within silo environments prevents applications running inside silos from attaching debuggers to other processes within the same silo which could be used for unauthorized memory inspection and code injection. Debugger access to silo processes should require explicit authorization through a defined diagnostic access policy rather than being available to all processes within the silo by default. Developer workflows that require debugging silo-hosted applications should use approved debugging infrastructure that authenticates the developer identity and logs debugging sessions. Attaching debuggers to silo processes without restriction allows any process within the silo to inspect and modify the memory of other silo processes which could extract sensitive data processed by those processes. Debug access restrictions should be enforced at the system call level to prevent circumvention through low-level debugging APIs. Production silo environments should have stricter debug access restrictions than development environments to protect sensitive workloads from unauthorized inspection.",
            Tags = ["app-silo", "debug-restriction", "memory-protection", "silo-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictSiloDebugCapabilities", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictSiloDebugCapabilities")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictSiloDebugCapabilities", 1)],
        },
        new TweakDef
        {
            Id = "appsilob-enforce-silo-identity-isolation",
            Label = "Enforce Identity Isolation to Prevent Cross-Silo Identity Context Sharing",
            Category = "App Silo Advanced Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Identity isolation enforcement ensures that authentication contexts credentials and identity tokens within one application silo cannot be accessed or impersonated by code running in other silos preventing cross-silo credential theft. Application silos that process authentication may cache tokens or credentials in memory structures that are accessible within the silo and identity isolation prevents these from being visible across silo boundaries. Credential isolation within silos is complementary to Windows Credential Guard but operates at finer granularity ensuring that even within an administrator security context cross-silo credential access is blocked. Authentication operations within silos should use silo-scoped credential stores rather than shared system credential stores to maintain isolation guarantees. Identity events from silo authentication operations should be logged distinctly from host authentication events to provide clear attribution of authentication activity to specific silo contexts. Security reviews should verify that identity isolation policies are correctly configured for all production silo deployments.",
            Tags = ["app-silo", "identity-isolation", "credential-protection", "authentication", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSiloIdentityIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSiloIdentityIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSiloIdentityIsolation", 1)],
        },
    ];
}
