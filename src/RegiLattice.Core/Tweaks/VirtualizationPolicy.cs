// RegiLattice.Core — Tweaks/VirtualizationPolicy.cs
// Sprint 358: Virtualization Policy tweaks (10 tweaks)
// Category: "Virtualization Policy" | Slug: virtz
// Registry: HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class VirtualizationPolicy
{
    private const string Key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";

    internal static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "virtz-restrict-hyper-v-management-to-admins",
            Label = "Restrict Hyper-V Management Operations to Administrator Accounts",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Restricting Hyper-V management to administrator accounts prevents standard users from creating modifying or deleting virtual machines that could be used to run unauthorized software within a virtualized environment. Standard users with Hyper-V management access could create virtual machines that bypass organizational security controls applied to host systems. Unauthorized virtual machines are difficult to monitor and may not have security software installed creating blind spots in endpoint protection coverage. Hyper-V management access should be limited to IT administrators who have a documented business need to create and manage virtual machines. Organizations should implement least-privilege principles for Hyper-V management using delegated administration where possible to grant only the specific capabilities required for each administrative role. Audit logging for Hyper-V management operations should track all VM creation deletion and configuration changes by administrator.",
            Tags = ["hyper-v", "virtualization", "admin-restriction", "least-privilege", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictManagementToAdmins", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictManagementToAdmins")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictManagementToAdmins", 1)],
        },
        new TweakDef
        {
            Id = "virtz-disable-hyper-v-on-workstations",
            Label = "Disable Hyper-V Virtualization Platform on Standard Enterprise Workstations",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            Description =
                "Disabling Hyper-V on standard workstations that do not have a documented business requirement for local virtualization reduces attack surface by removing the virtualization infrastructure that could be used for malicious purposes. Hyper-V enabled workstations can be used by attackers to run virtual machines that bypass host-based security controls and operate as isolated systems on the corporate network. Virtualization-based security features like Credential Guard and Device Guard require Hyper-V to be present so disabling Hyper-V must be weighed against the security benefits those features provide. Organizations should evaluate whether disabling Hyper-V is appropriate for their security model or whether keeping it enabled primarily for VBS security features is the better configuration. Developer workstations and IT administration systems that have legitimate virtualization requirements should be exempted from the general workstation policy. Disabling Hyper-V may affect WSL 2 which uses Hyper-V technology; alternatives using WSLg or WSL 1 should be evaluated if WSL is required.",
            Tags = ["hyper-v", "workstation", "virtualization", "attack-surface", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "DisableHyperVOnWorkstations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "DisableHyperVOnWorkstations")],
            DetectOps = [RegOp.CheckDword(Key, "DisableHyperVOnWorkstations", 1)],
        },
        new TweakDef
        {
            Id = "virtz-enforce-synthetic-device-security",
            Label = "Enforce Security Configuration for Hyper-V Synthetic Devices",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Hyper-V synthetic devices provide paravirtualized device interfaces between guest virtual machines and the hypervisor that can be exploited to break guest isolation if not configured securely. Enforcing security configuration for synthetic devices ensures that guest VMs cannot exploit vulnerabilities in device emulation to gain access to host resources or hypervisor memory. Synthetic network adapters storage controllers and video adapters each have configurable security parameters that should be set to the most restrictive values appropriate for the VM workload. Organizations should apply the principle of least capability to Hyper-V VMs granting only the synthetic devices needed for the VM's function. Security configuration for synthetic devices should be audited as part of the VM provisioning process to ensure that all new VMs are configured with appropriate device security settings. Guest VM security configurations should be periodically reviewed to identify VMs that have accumulated unnecessary synthetic device capabilities.",
            Tags = ["hyper-v", "synthetic-devices", "vm-security", "isolation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSyntheticDeviceSecurity")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSyntheticDeviceSecurity", 1)],
        },
        new TweakDef
        {
            Id = "virtz-enable-vm-snapshot-encryption",
            Label = "Enable Encryption of Virtual Machine Snapshots and Saved States",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Virtual machine snapshots and saved states contain complete memory images of running VMs that may include encryption keys credentials and sensitive application data. Encrypting VM snapshots and saved states ensures that this sensitive data is protected if snapshot files are accessed outside the Hyper-V management context. VM snapshot files stored on shared storage or backup media are particularly vulnerable to unauthorized access if they are not encrypted at the VM level. Shielded VMs in Hyper-V provide the highest level of protection including encryption of VM configuration snapshots and saved states using Host Guardian Service key management. Organizations should implement VM encryption for any virtual machine that processes sensitive regulated data including financial HR and healthcare applications. Key management for VM encryption should be integrated with the organizational key management infrastructure to ensure proper key lifecycle management.",
            Tags = ["hyper-v", "vm-encryption", "snapshots", "saved-states", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableSnapshotEncryption", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableSnapshotEncryption")],
            DetectOps = [RegOp.CheckDword(Key, "EnableSnapshotEncryption", 1)],
        },
        new TweakDef
        {
            Id = "virtz-restrict-vm-clipboard-sharing",
            Label = "Restrict Clipboard Sharing Between Hyper-V Guest and Host",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Clipboard sharing between Hyper-V guests and the host system creates a data transfer channel that can leak sensitive data between VMs and the host or allow malicious code injection through the clipboard. Restricting clipboard sharing prevents accidental data leakage between isolated VMs running different workloads or between VM environments and the host system. VMs running untrusted content or isolated high-security workloads should have clipboard sharing disabled to prevent data from crossing VM isolation boundaries. Malicious applications running in a VM can use clipboard injection to execute code on the host if the user pastes clipboard content from the VM into host applications. Organizations should evaluate clipboard sharing requirements for each VM type and allow it only where legitimate workflow requirements justify the associated risk. Monitoring clipboard sharing events can help detect attempts to use clipboard as a data exfiltration channel between isolated environments.",
            Tags = ["hyper-v", "clipboard", "data-isolation", "vm-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictClipboardSharing", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictClipboardSharing")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictClipboardSharing", 1)],
        },
        new TweakDef
        {
            Id = "virtz-audit-vm-management-operations",
            Label = "Enable Audit Logging for All Hyper-V Virtual Machine Management Operations",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Hyper-V management audit logging records all VM lifecycle events including VM creation startup shutdown deletion and configuration changes providing accountability for VM management operations. Audit trails for VM management are important for detecting unauthorized VM creation that could be used for shadow IT or malicious purposes. VM deletion events should be monitored closely as deletion of VMs may indicate evidence destruction in the context of security incidents. Audit events for Hyper-V management should include the identity of the administrator performing the operation the timestamp and the details of the changed configuration. Organizations should forward Hyper-V audit events to centralized SIEM for correlation with other administrative activity and identity events. VM management audit logs should be retained for a period appropriate to the organization's compliance requirements.",
            Tags = ["hyper-v", "audit", "vm-management", "monitoring", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "AuditVMManagementOperations", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "AuditVMManagementOperations")],
            DetectOps = [RegOp.CheckDword(Key, "AuditVMManagementOperations", 1)],
        },
        new TweakDef
        {
            Id = "virtz-enforce-secure-boot-for-vms",
            Label = "Enforce Secure Boot Configuration for Hyper-V Generation 2 VMs",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enforcing Secure Boot for Hyper-V Generation 2 virtual machines ensures that VMs only boot operating system images that are signed with trusted certificates preventing rootkit-level malware from persisting in VM boot configurations. Guest VM Secure Boot uses Hyper-V virtual firmware to validate the boot chain signature exactly as physical Secure Boot does on bare metal systems. Malicious modifications to VM boot sectors or boot loaders are prevented by Secure Boot enforcement which is particularly important for VMs that are created from templates or imported from external sources. Guest Secure Boot should be configured with appropriate templates for the VM's operating system type. Organizations should define and enforce VM templates that include Secure Boot configuration to ensure all provisioned VMs have this protection enabled from creation. VMs that fail Secure Boot validation should not be allowed to start and alerts should be generated for investigation.",
            Tags = ["hyper-v", "secure-boot", "vm-security", "boot-integrity", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnforceSecureBootForVMs", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnforceSecureBootForVMs")],
            DetectOps = [RegOp.CheckDword(Key, "EnforceSecureBootForVMs", 1)],
        },
        new TweakDef
        {
            Id = "virtz-restrict-vm-network-access",
            Label = "Restrict Hyper-V Virtual Machine Network Access Configuration",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Restricting VM network access through Hyper-V policy limits which network segments virtual machines can be connected to preventing VMs from accessing sensitive network segments that are not appropriate for their function. Virtual machine network placement should be deliberately configured to provide access only to the network segments required for the VM's workload. Production VMs should be isolated from development and test VMs at the network layer to prevent cross-contamination between environments. VMs that process sensitive data should be on network segments with monitoring and DLP capabilities to detect unauthorized data access. Unrestricted VM network configuration allows administrators to connect VMs to any available virtual switch without network policy review which can lead to inadvertent bypassing of network segmentation controls. Network access configuration for Hyper-V VMs should be part of the VM provisioning review process including approval of new network connections.",
            Tags = ["hyper-v", "vm-network", "segmentation", "isolation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "RestrictVMNetworkAccess", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "RestrictVMNetworkAccess")],
            DetectOps = [RegOp.CheckDword(Key, "RestrictVMNetworkAccess", 1)],
        },
        new TweakDef
        {
            Id = "virtz-block-nested-virtualization",
            Label = "Block Nested Virtualization in Hyper-V Guest Virtual Machines",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            Description =
                "Nested virtualization allows virtual machines to run their own hypervisor and create nested VMs which can be used to create isolated execution environments that bypass host security monitoring. Blocking nested virtualization prevents the use of VM-within-VM configurations that increase complexity and make security monitoring and policy enforcement more difficult. Nested virtualization is exploited in some containerization attacks where attackers use nested Hyper-V to create isolated containers that bypass host security controls. The reduced visibility into nested VM operations makes incident investigation significantly more difficult when security events originate from within nested environments. Organizations that have specific legitimate requirements for nested virtualization should isolate those systems and apply additional monitoring rather than broadly enabling nested virtualization. Testing and development environments that require nested virtualization for specific use cases should be treated as high-risk systems with compensating controls.",
            Tags = ["hyper-v", "nested-virtualization", "vm-security", "isolation", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "BlockNestedVirtualization", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "BlockNestedVirtualization")],
            DetectOps = [RegOp.CheckDword(Key, "BlockNestedVirtualization", 1)],
        },
        new TweakDef
        {
            Id = "virtz-configure-vm-memory-protection",
            Label = "Configure Enhanced Memory Protection for Hyper-V Virtual Machines",
            Category = "Virtualization Policy",
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            Description =
                "Enhanced memory protection for Hyper-V VMs ensures that virtual machine memory is isolated from unauthorized access by other VMs and the host management OS in ways that go beyond standard hypervisor isolation. Hypervisor-protected code integrity uses the hypervisor security boundary to protect kernel memory from modification by malicious code running in the VM. Memory protection features ensure that VM memory cannot be directly accessed by processes on the host even when the host has Hyper-V management privileges without going through the hypervisor management API. Shielded VMs provide the highest level of memory protection by encrypting VM memory and preventing host administrators from directly inspecting VM memory contents. Organizations that run VMs with sensitive workloads including regulated data or privileged authentication components should evaluate the appropriate level of memory protection for each VM type. Regular security reviews of VM memory protection configuration ensure that protection levels remain appropriate as VM workloads and threat models evolve.",
            Tags = ["hyper-v", "memory-protection", "vm-isolation", "hypervisor-security", "policy"],
            RegistryKeys = [Key],
            ApplyOps = [RegOp.SetDword(Key, "EnableEnhancedMemoryProtection", 1)],
            RemoveOps = [RegOp.DeleteValue(Key, "EnableEnhancedMemoryProtection")],
            DetectOps = [RegOp.CheckDword(Key, "EnableEnhancedMemoryProtection", 1)],
        },
    ];
}
