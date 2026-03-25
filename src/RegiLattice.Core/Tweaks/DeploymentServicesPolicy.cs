// RegiLattice.Core — Tweaks/DeploymentServicesPolicy.cs
// Sprint 330: Deployment Services Policy tweaks (10 tweaks)
// Category: "Deployment Services Policy" | Slug: depsvc
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class DeploymentServicesPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\WDS";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "depsvc-disable-multicast",
            Label = "Disable WDS Multicast Image Transfer",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "Windows Deployment Services multicast transfers operating system images to multiple clients simultaneously using multicast UDP network traffic. Disabling multicast transmission prevents the generation of multicast network traffic from WDS servers that can impact other network devices. WDS multicast operates on well-known multicast addresses that require multicast routing infrastructure to function correctly. In environments without proper multicast routing, WDS multicast traffic can generate excessive broadcast traffic or fail to route correctly. Disabling multicast forces WDS to use unicast image transfers which are simpler to troubleshoot and less likely to cause network issues. Organizations that have properly configured multicast infrastructure may enable multicast for efficient simultaneous OS deployment to large numbers of clients.",
            Tags = ["wds", "multicast", "deployment", "network", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableMulticast", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableMulticast")],
            DetectOps = [RegOp.CheckDword(Key, "DisableMulticast", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-disable-pxe-response",
            Label = "Disable WDS PXE Boot Response",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "PXE boot allows network endpoints to boot from a WDS server and receive operating system images instead of booting from local storage. Disabling WDS PXE response prevents unauthorized network boot attempts from connecting to the WDS server. Unauthorized PXE boots could allow an attacker to boot a system into a WDS-provided environment bypassing local security controls. WDS PXE services should only respond to pre-authorized clients and systems requiring legitimate OS deployment. PXE boot in production environments represents an attack vector where adversaries can boot systems into controlled environments to capture credentials or bypass endpoint security. Organizations should restrict PXE responses to pre-authorized MAC addresses when WDS PXE is required for legitimate deployment.",
            Tags = ["wds", "pxe", "boot", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisablePxeResponse", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisablePxeResponse")],
            DetectOps = [RegOp.CheckDword(Key, "DisablePxeResponse", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-require-user-authorization",
            Label = "Require User Authorization for WDS Network Boot",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "WDS network boot without user authorization can process deployment requests without verifying that the requesting system and user are authorized. Requiring user authorization for WDS deployments ensures that only authenticated and authorized users can initiate OS deployments through WDS. Unauthorized WDS deployments could replace a system's operating system with an attacker-controlled image bypassing all security controls. WDS authorization requirements prevent automated attackers from deploying compromised operating system images to corporate endpoints. User authorization should be combined with client device authentication to ensure both the user and the device are authorized for OS deployment. WDS authorization policies integrate with Active Directory to enforce group membership requirements before allowing OS deployment.",
            Tags = ["wds", "authorization", "deployment", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RequireUserAuthorization", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RequireUserAuthorization")],
            DetectOps = [RegOp.CheckDword(Key, "RequireUserAuthorization", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-enable-boot-logging",
            Label = "Enable WDS Boot Session Logging",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "WDS boot session logging records all client connections, image deployment attempts, and deployment outcomes for audit and troubleshooting purposes. Enabling boot logging ensures a complete record of all WDS deployment activity including successful and failed deployments. Boot session logs help identify unauthorized deployment attempts or unusual deployment patterns that may indicate security incidents. WDS deployment logs are valuable for capacity planning and identifying deployment infrastructure problems. Security monitoring of WDS logs should include alerts for deployments outside of authorized maintenance windows or from unauthorized source addresses. Boot logging should be forwarded to the SIEM to correlate deployment events with other security indicators.",
            Tags = ["wds", "logging", "audit", "deployment", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableBootLogging", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableBootLogging")],
            DetectOps = [RegOp.CheckDword(Key, "EnableBootLogging", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-disable-tftp-anonymous-access",
            Label = "Disable Anonymous TFTP Access to WDS Boot Files",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "WDS uses TFTP to serve boot files to PXE-booting clients and by default these files may be accessible via anonymous TFTP connections. Disabling anonymous TFTP access prevents unauthorized clients from downloading WDS boot files without authentication. Boot files themselves may not contain sensitive data but unrestricted TFTP access enables reconnaissance of the deployment infrastructure. TFTP file access should require client authentication to prevent mapping of the WDS boot file structure by attackers. Restricting TFTP access forces deployment clients to authenticate before receiving deployment configuration reducing unauthorized access risk. Organizations should evaluate whether anonymous TFTP is required for their deployment infrastructure or if authentication can be enforced.",
            Tags = ["wds", "tftp", "anonymous", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableAnonymousTftp", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableAnonymousTftp")],
            DetectOps = [RegOp.CheckDword(Key, "DisableAnonymousTftp", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-restrict-image-groups",
            Label = "Restrict WDS Image Groups to Authorized Groups Only",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "WDS image groups organize OS images for deployment and access can be restricted to specific Active Directory security groups. Restricting image group access ensures that only authorized personnel and systems can deploy specific operating system images. Unrestricted image group access allows any authenticated user to initiate deployment of any available OS image including specialized system images. Role-based access to image groups ensures that desktop technicians access desktop images while server images are restricted to server administrators. Image group restrictions prevent employees from initiating unauthorized OS replacements on endpoints they manage. Image access auditing should be enabled alongside image group restrictions to log all access attempts.",
            Tags = ["wds", "image-groups", "access-control", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictImageGroupAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictImageGroupAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictImageGroupAccess", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-enable-driver-injection-restriction",
            Label = "Restrict WDS Driver Injection to Approved Drivers",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "WDS driver injection adds hardware-specific drivers to OS images during deployment providing out-of-box device compatibility. Restricting driver injection to approved driver packages prevents unapproved or potentially malicious drivers from being injected into deployment images. Driver injection without restrictions could allow an attacker who gains access to WDS infrastructure to inject malicious drivers into OS images. Approved driver packages should be tested and validated before adding to the WDS driver store for injection. Driver signing requirements should be enforced for all drivers added to the WDS driver store to prevent injection of unsigned drivers. WDS driver injection policies integrate with Windows Update for Business and Microsoft Update Catalog for validated driver sources.",
            Tags = ["wds", "drivers", "injection", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictDriverInjection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictDriverInjection")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictDriverInjection", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-disable-wds-client-logging",
            Label = "Disable WDS Client-Side Telemetry Logging",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            Description =
                "WDS client telemetry logging can transmit deployment event data from client systems back to Microsoft or deployment management systems. Disabling WDS client telemetry logging reduces the data transmitted about deployment infrastructure and client system configuration. Client telemetry during OS deployment includes hardware enumeration data, deployment timing, and setup configuration details. Reducing telemetry transmission during deployment limits the external visibility into enterprise deployment infrastructure and hardware inventory. WDS client telemetry data is most useful for troubleshooting deployment issues but may expose sensitive infrastructure details if transmitted externally. Organizations that rely on WDS telemetry for deployment health monitoring should route data to internal logging rather than external Microsoft services.",
            Tags = ["wds", "telemetry", "logging", "privacy", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableClientTelemetry", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableClientTelemetry")],
            DetectOps = [RegOp.CheckDword(Key, "DisableClientTelemetry", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-enforce-network-boot-security",
            Label = "Enforce Secure Network Boot Validation",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 5,
            SafetyRating = 5,
            Description =
                "Secure network boot validation ensures that WDS boot images are cryptographically verified before execution preventing deployment of tampered images. Enforcing secure network boot validation prevents boot time attacks where an attacker replaces legitimate WDS boot images with malicious alternatives. Boot image integrity validation uses digital signatures to verify that images have not been modified since signing by the deployment administrator. Secure network boot is essential in environments where WDS infrastructure may be accessible to adversaries with lateral movement capabilities. Boot image signing should use code signing certificates with appropriate access controls to prevent unauthorized image signing. Secure network boot validation should be combined with Secure Boot on client systems to create an end-to-end boot integrity chain.",
            Tags = ["wds", "secure-boot", "validation", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSecureNetworkBoot", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureNetworkBoot")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSecureNetworkBoot", 1)],
        },
        new TweakDef
        {
            Id = "depsvc-limit-wds-server-accessibility",
            Label = "Restrict WDS Server Access to Deployment VLAN",
            Category = "Deployment Services Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "WDS servers should only be accessible from deployment VLANs and networks used for OS deployment activities rather than from all corporate subnets. Restricting WDS server accessibility reduces the attack surface by limiting which network segments can reach the deployment infrastructure. Production VLANs should not have direct access to WDS servers unless those servers are actively used for production endpoint deployment. Deployment infrastructure accessible from all corporate subnets increases the risk of unauthorized deployment attempts or WDS infrastructure exploitation. Network ACLs and host-based firewall rules should restrict WDS port access to deployment VLANs and administrator management networks. WDS server accessibility restrictions should be documented and reviewed regularly as network topology changes.",
            Tags = ["wds", "network-restriction", "vlan", "security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "LimitServerAccessibility", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "LimitServerAccessibility")],
            DetectOps = [RegOp.CheckDword(Key, "LimitServerAccessibility", 1)],
        },
    ];
}
