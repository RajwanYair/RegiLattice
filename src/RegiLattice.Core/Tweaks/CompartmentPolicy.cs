// RegiLattice.Core — Tweaks/CompartmentPolicy.cs
// Sprint 338: Compartment Policy tweaks (10 tweaks)
// Category: "Compartment Policy" | Slug: compart
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Compartment

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class CompartmentPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Compartment";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "compart-enable-network-compartmentalization",
            Label = "Enable Network Isolation for Application Groups",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Network isolation through compartmentalization controls which applications and process groups can communicate with which network destinations. Enabling network compartmentalization creates enforced boundaries between application groups preventing unrestricted network access. Applications confined to specific network compartments cannot initiate connections to hosts outside their designated network zone reducing lateral movement risk. Network compartmentalization is particularly effective for containing the damage from compromised applications by restricting their outbound connectivity. Containment policy should define network zones that align with the application's legitimate communication requirements and business function. Organizations implementing zero-trust network architecture can use compartmentalization to enforce application-level network segmentation beyond traditional VLAN-based controls.",
            Tags = ["compartment", "network-isolation", "lateral-movement", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableNetworkIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableNetworkIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableNetworkIsolation", 1)],
        },
        new TweakDef
        {
            Id = "compart-restrict-localhost-loopback",
            Label = "Restrict Localhost Loopback Access Between Compartments",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Localhost loopback communication between compartments can be used to bypass network isolation boundaries by routing traffic through the local system. Restricting loopback access between compartments ensures that network isolation controls apply to both external and local communications. Applications in separate security compartments should not be able to communicate through local IPC or loopback unless explicitly permitted by policy. Unrestricted loopback access undermines network compartmentalization by providing a side channel for inter-compartment communication. Loopback restriction enforcement requires applications to use explicit inter-process communication mechanisms that can be monitored and controlled. Organizations should map legitimate loopback dependencies between application groups before enforcing loopback restrictions to prevent breaking application functionality.",
            Tags = ["compartment", "loopback", "network-isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictLoopbackAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictLoopbackAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictLoopbackAccess", 1)],
        },
        new TweakDef
        {
            Id = "compart-enable-process-isolation",
            Label = "Enable Process Memory Isolation Between Windows Compartments",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Process memory isolation between compartments prevents processes in one compartment from accessing or manipulating the memory of processes in other compartments. Enabling process isolation ensures that a compromised process in one security compartment cannot directly exploit processes in adjacent compartments. Memory isolation is a fundamental defense against process injection attacks that are commonly used for credential theft and privilege escalation. Strong isolation boundaries between compartments reduce the blast radius of a single process compromise to only the resources accessible from that compartment. Compartment process isolation complements HVCI for kernel code but addresses user-mode process interaction which HVCI does not directly control. Organizations deploying containerized applications can use compartment isolation to provide security boundaries similar to OS-level containers.",
            Tags = ["compartment", "process-isolation", "memory", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableProcessIsolation", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableProcessIsolation")],
            DetectOps = [RegOp.CheckDword(Key, "EnableProcessIsolation", 1)],
        },
        new TweakDef
        {
            Id = "compart-restrict-file-system-access",
            Label = "Restrict File System Access Based on Compartment Membership",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "File system access restrictions based on compartment membership limit which files and directories each application group can read or write. Restricting file system access by compartment prevents a compromised application from reading sensitive files belonging to other application security zones. Data segregation through file system compartmentalization supports data classification requirements by ensuring applications only access data appropriate to their security classification. Compartment-based file restrictions supplement traditional NTFS ACLs by providing dynamic access control that follows application security group membership. The combination of NTFS permissions and compartment file restrictions creates multiple layers of protection against unauthorized data access. File system compartmentalization should be mapped carefully against application data access requirements to prevent operational disruption.",
            Tags = ["compartment", "file-access", "data-segregation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictFileSystemAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictFileSystemAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictFileSystemAccess", 1)],
        },
        new TweakDef
        {
            Id = "compart-enforce-object-permissions",
            Label = "Enforce Object Access Permissions for Compartment Boundaries",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Compartment object permission enforcement ensures that Windows objects including mutexes, semaphores, and named pipes respect compartment membership for access decisions. Enforcing object access permissions by compartment prevents cross-compartment object attacks where a malicious process creates objects that privileged processes will access. Named pipe impersonation attacks are prevented when pipes are restricted to be accessible only within the same compartment. Shared memory objects between application groups represent a common attack vector that compartment object restrictions can control. Object permission enforcement for compartments integrates with the Windows object manager to apply compartment membership checks transparently. Applications that use shared objects for inter-process communication across compartment boundaries require explicit policy exceptions.",
            Tags = ["compartment", "object-permissions", "ipc", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceObjectPermissions", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceObjectPermissions")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceObjectPermissions", 1)],
        },
        new TweakDef
        {
            Id = "compart-audit-boundary-violations",
            Label = "Enable Audit Logging for Compartment Boundary Violations",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Compartment boundary violation auditing generates events when processes attempt to access resources outside their assigned compartment. Enabling boundary violation auditing provides visibility into attempted cross-compartment access that may indicate malicious activity or application misconfiguration. Violation events help identify applications that have dependencies outside their defined compartment requiring policy adjustments before enforcement. Security teams can use violation data to map legitimate cross-compartment communications and build accurate compartment policy. Repeated violation attempts from the same process targeting sensitive compartment resources indicate potential exploitation activity. Audit mode should be enabled before enforcement mode to allow analysis of violation patterns without disrupting legitimate operations.",
            Tags = ["compartment", "audit", "violation-detection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditBoundaryViolations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditBoundaryViolations")],
            DetectOps = [RegOp.CheckDword(Key, "AuditBoundaryViolations", 1)],
        },
        new TweakDef
        {
            Id = "compart-restrict-registry-access",
            Label = "Restrict Registry Access Based on Application Compartment",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Registry access restrictions by compartment limit which registry hives and keys can be read or modified by applications in each security group. Restricting registry access by compartment prevents compromised applications from reading sensitive configuration including credentials stored in the registry. Applications should only have access to the registry keys required for their legitimate function reducing access to configuration that could be modified for privilege escalation. Compartment-based registry restrictions supplement standard registry ACLs to provide dynamic access control that follows application group membership. Registry access restrictions prevent reconnaissance activities where malware scanned the registry to discover installed software and configuration details. Organizations should map application registry dependencies before enabling compartment-based restrictions to prevent disrupting application functionality.",
            Tags = ["compartment", "registry", "access-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictRegistryAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictRegistryAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictRegistryAccess", 1)],
        },
        new TweakDef
        {
            Id = "compart-disable-cross-compartment-clipboard",
            Label = "Disable Clipboard Sharing Across Security Compartments",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Clipboard sharing across compartments allows data to be transferred between security zones through the Windows clipboard bypassing network isolation controls. Disabling cross-compartment clipboard sharing prevents data from being copied from high-security compartments to lower-security application groups. Clipboard data contains user content including credentials, documents, and configuration that should respect compartment data classification when being transferred. Controlled clipboard sharing through approved transfer mechanisms is preferable to unrestricted clipboard access across compartment boundaries. Users who need to transfer data between compartments should use approved channels that can monitor and log the data exchange. Cross-compartment clipboard restrictions are particularly relevant in environments with classified data handling requirements.",
            Tags = ["compartment", "clipboard", "data-transfer", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableCrossCompartmentClipboard", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableCrossCompartmentClipboard")],
            DetectOps = [RegOp.CheckDword(Key, "DisableCrossCompartmentClipboard", 1)],
        },
        new TweakDef
        {
            Id = "compart-enable-mandatory-integrity",
            Label = "Enforce Mandatory Integrity Control for Compartment Processes",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Mandatory Integrity Control (MIC) assigns integrity levels to processes and objects controlling which processes can modify resources based on their integrity level. Enabling MIC enforcement for compartment processes prevents lower-integrity processes from writing to higher-integrity compartment objects. Internet-facing applications running at low integrity cannot modify files and registry keys owned by higher-integrity compartment processes even with same-user permissions. MIC enforcement is the mechanism behind Protected Mode in Internet Explorer and similar sandboxing techniques for web-facing applications. Compartment MIC enforcement creates an additional layer of protection that supplements discretionary access controls with mandatory controls. Applications in high-trust compartments should run at higher integrity levels with internet-exposed components running at lower integrity.",
            Tags = ["compartment", "mandatory-integrity", "sandboxing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceMandatoryIntegrity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceMandatoryIntegrity")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceMandatoryIntegrity", 1)],
        },
        new TweakDef
        {
            Id = "compart-restrict-interprocess-communication",
            Label = "Restrict IPC Mechanisms Across Compartment Boundaries",
            Category = "Compartment Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Inter-process communication across compartment boundaries allows application groups to exchange data through mechanisms like pipes, sockets, and mailslots bypassing file-level separation. Restricting IPC across compartments ensures that only approved communication channels with appropriate data inspection can bridge compartment boundaries. Uncontrolled IPC across compartment boundaries provides a covert channel for data exfiltration from restricted to unrestricted application zones. IPC restriction policies should define explicit allowed communication paths with appropriate authentication and data validation for cross-compartment channels. Named pipes and sockets that cross compartment boundaries should require authentication from both communicating parties to prevent impersonation. Organizations should use application firewalls or approved message brokers for cross-compartment communication rather than direct IPC mechanisms.",
            Tags = ["compartment", "ipc", "inter-process", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictCrossCompartmentIPC", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictCrossCompartmentIPC")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictCrossCompartmentIPC", 1)],
        },
    ];
}
