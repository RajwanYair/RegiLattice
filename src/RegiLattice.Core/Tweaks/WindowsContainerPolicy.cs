// RegiLattice.Core — Tweaks/WindowsContainerPolicy.cs
// Sprint 311: Windows Container Policy tweaks (10 tweaks)
// Category: "Windows Container Policy" | Slug: wincnt
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsContainerPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "wincnt-disable-container-network-access",
            Label = "Disable Container Unrestricted Network Access",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Windows containers can be configured with different network isolation levels ranging from full host network access to complete isolation. Disabling unrestricted container network access prevents containers from having the same network visibility as the host operating system. Containers with full host network access bypass container network isolation and can reach host-accessible services and network resources. Compromised containers with host network access can attack other hosts and services that would otherwise be outside container reach. Enterprise container policies should enforce network isolation appropriate to the sensitivity of the workload running in each container. Containers requiring specific network connectivity should use explicitly defined virtual networks rather than shared host networking.",
            Tags = ["containers", "network", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictContainerNetworkAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictContainerNetworkAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictContainerNetworkAccess", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-disable-host-device-access",
            Label = "Disable Container Host Device Access",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Containers may be configured to access physical devices attached to the host system including storage devices, USB controllers, and other hardware. Disabling host device access from containers prevents container workloads from directly accessing physical hardware connected to the host. Direct device access from containers can allow container workloads to access sensitive data on host storage devices beyond the container file system. Physical device access bypasses container isolation boundaries and provides a path for malicious containers to affect host hardware state. Enterprise container policies should restrict device access to only explicitly required and approved devices rather than allowing broad hardware access. Containers should access persistent data through volume mounts with appropriate permissions rather than direct device access.",
            Tags = ["containers", "devices", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableDeviceAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableDeviceAccess")],
            DetectOps = [RegOp.CheckDword(Key, "DisableDeviceAccess", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-enforce-isolation-level",
            Label = "Enforce Minimum Container Isolation Level",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Windows containers support different isolation levels from process isolation sharing the host kernel to Hyper-V isolation using separate virtual machine kernels. Enforcing minimum isolation level requirements ensures containers cannot be started with weaker isolation than enterprise policy allows. Process-isolated containers share the Windows kernel with the host making kernel vulnerabilities directly exploitable from within containers. Hyper-V isolated containers use virtual machine boundaries providing much stronger protection against kernel-level exploits. Enterprise security policies should require Hyper-V isolation for containers handling sensitive workloads or serving external traffic. Enforcing minimum isolation through policy prevents developers from inadvertently deploying containers with insufficient isolation guarantees.",
            Tags = ["containers", "isolation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceIsolationLevel", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceIsolationLevel")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceIsolationLevel", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-disable-container-image-pull-insecure",
            Label = "Disable Container Image Pull from Insecure Registries",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Container images can be pulled from registries using either HTTPS secured connections or insecure unencrypted HTTP connections. Disabling insecure container image pulls ensures that all image downloads use encrypted HTTPS connections to verify registry identity. HTTP-sourced container images are vulnerable to man-in-the-middle attacks that can inject malicious code or replace the legitimate image. Insecure registry connections cannot be authenticated making it impossible to verify that the image came from the intended source. Enterprise container policies should require all registry communication to use HTTPS with valid certificates from trusted authorities. Preventing insecure pulls is a fundamental container supply chain security requirement.",
            Tags = ["containers", "registry", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableInsecureRegistryPull", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableInsecureRegistryPull")],
            DetectOps = [RegOp.CheckDword(Key, "DisableInsecureRegistryPull", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-require-image-signing",
            Label = "Require Signed Container Images",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Container image signing allows image publishers to cryptographically sign images ensuring integrity and provenance can be verified before deployment. Requiring signed container images prevents execution of images that have not been signed by a trusted key. Unsigned container images cannot be verified to originate from the expected publisher or remain unmodified since publication. Supply chain attacks on container registries can replace legitimate images with malicious versions that would not have valid signatures. Enterprise container policies should require that only images signed by approved keys can be deployed to production endpoints. Image signing requirements should be combined with a key management policy specifying which signing keys are trusted.",
            Tags = ["containers", "signing", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireImageSigning", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireImageSigning")],
            DetectOps = [RegOp.CheckDword(Key, "RequireImageSigning", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-disable-privileged-containers",
            Label = "Disable Privileged Container Execution",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            Description =
                "Privileged containers run with elevated privileges that grant them access to host capabilities typically restricted from unprivileged containers. Disabling privileged container execution prevents containers from being launched with elevated host privileges that bypass isolation. Privileged containers can often escape container isolation and directly access host resources including file systems and network interfaces. Workloads running in privileged containers are treated essentially as trusted processes rather than isolated workloads. Security vulnerabilities in privileged container workloads have a much higher impact due to their elevated access to host systems. Most legitimate container workloads can be designed to operate without privileged access through appropriate capability management.",
            Tags = ["containers", "privileges", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePrivilegedContainers", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePrivilegedContainers")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePrivilegedContainers", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-disable-container-mounts",
            Label = "Restrict Container Host Volume Mounts",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Container host volume mounts allow containers to access specific directories on the host file system through bind mounts. Restricting volume mounts prevents containers from accessing sensitive host directories that should remain isolated from container workloads. Overly permissive volume mounts can expose host system files, secrets, and certificates to container workloads. Compromised containers with sensitive host mounts can read configuration files, private keys, and security credentials. Container workloads should access only specifically scoped directories with minimal permissions rather than broad host file system access. Volume mount restrictions prevent accidental or malicious access to host system files from within container environments.",
            Tags = ["containers", "mounts", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictHostMounts", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictHostMounts")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictHostMounts", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-enable-container-audit-logging",
            Label = "Enable Container Activity Audit Logging",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Container audit logging records lifecycle events including container start stop and significant operations performed by container workloads. Enabling container audit logging provides visibility into container operations for security monitoring and forensic investigation. Detection of malicious container activity requires that logs of container operations be available to security teams. Container audit events should be forwarded to central logging infrastructure alongside other Windows Event Log security events. Compliance frameworks including SOC 2 and PCI DSS require audit logging of infrastructure operations including containerized workloads. Audit logging overhead is minimal and provides significant security and compliance value.",
            Tags = ["containers", "audit", "logging", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableContainerAuditLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableContainerAuditLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableContainerAuditLogging", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-restrict-registry-access",
            Label = "Restrict Container Host Registry Access",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Container workloads may be configured to access host registry hives directly through registry mounts similar to volume mounts for file system. Restricting host registry access from containers prevents container workloads from reading or modifying host system configuration. Host registry hives contain sensitive configuration data including credentials, certificates, and security policy settings. Containers with host registry access can read security credentials and configuration data from a registry hive mounted as writable. Malicious container workloads modifying the host registry can persist configuration changes that survive container deletion. Containers should use their own isolated registry hives rather than accessing host registry data.",
            Tags = ["containers", "registry", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictRegistryAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictRegistryAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictRegistryAccess", 1)],
        },
        new TweakDef
        {
            Id = "wincnt-disable-container-telemetry",
            Label = "Disable Container Telemetry Collection",
            Category = "Windows Container Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 1,
            SafetyRating = 5,
            Description =
                "The Windows container runtime collects telemetry about container usage patterns including image usage, runtime performance, and configuration details. Disabling container telemetry prevents container deployment and usage data from being transmitted to Microsoft. Container usage patterns and image references represent sensitive information about enterprise application architecture. Software inventory derived from container image references can reveal internal application portfolio and development practices. Enterprise container operations monitoring should be performed through internal tools rather than external telemetry collection. Disabling telemetry does not affect container functionality including creation, execution, or management operations.",
            Tags = ["containers", "telemetry", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableContainerTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableContainerTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableContainerTelemetry", 1)],
        },
    ];
}
