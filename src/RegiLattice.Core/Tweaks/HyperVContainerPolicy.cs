// RegiLattice.Core — Tweaks/HyperVContainerPolicy.cs
// Hyper-V virtual machine management, container isolation, VM creation, and live migration policy — Sprint 517.
// Category: "Hyper-V Container Policy" | Slug: hvcon
// Registry: HKLM\SOFTWARE\Policies\Microsoft\Windows\HyperV

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class HyperVContainerPolicy
{
    private const string Key    = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\HyperV";
    private const string VmKey  = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\VirtualMachineMonitor";
    private const string CtrKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers";

    public static IReadOnlyList<TweakDef> Tweaks =>
    [
        new TweakDef
        {
            Id           = "hvcon-require-admin-vm-creation",
            Label        = "Require Administrator to Create Hyper-V Virtual Machines",
            Category     = "Hyper-V Container Policy",
            Description  = "Restricts virtual machine creation in Hyper-V to administrator accounts only, preventing standard users from provisioning new VMs that could be used to bypass security policy controls on the host system.",
            Tags         = ["hyper-v", "vm-creation", "admin", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Hyper-V VM creation restricted to admins; standard users cannot provision new virtual machines.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireAdminForVMCreation", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireAdminForVMCreation")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireAdminForVMCreation", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-disable-vm-network-passthrough",
            Label        = "Disable Network Passthrough (SR-IOV) for Hyper-V VMs",
            Category     = "Hyper-V Container Policy",
            Description  = "Prevents Hyper-V virtual machines from using SR-IOV (Single Root I/O Virtualisation) network passthrough, ensuring all VM network traffic flows through the Hyper-V virtual switch for monitoring and filtering.",
            Tags         = ["hyper-v", "sriov", "network-passthrough", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "SR-IOV network passthrough disabled; VM traffic routed through vSwitch for visibility and filtering.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableSRIOVPassthrough", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableSRIOVPassthrough")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableSRIOVPassthrough", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-block-live-migration-plain",
            Label        = "Block Unencrypted Hyper-V Live Migration",
            Category     = "Hyper-V Container Policy",
            Description  = "Prevents Hyper-V live migrations from using the unencrypted migration transport, requiring Kerberos authentication or SMB encryption for all live migration sessions to prevent VM memory interception during migration.",
            Tags         = ["hyper-v", "live-migration", "encryption", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Plaintext live migration blocked; Kerberos/SMB encryption required for all VM live migration sessions.",
            ApplyOps     = [RegOp.SetDword(Key, "RequireEncryptedLiveMigration", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "RequireEncryptedLiveMigration")],
            DetectOps    = [RegOp.CheckDword(Key, "RequireEncryptedLiveMigration", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-set-vm-memory-limit",
            Label        = "Set Maximum Hyper-V VM Memory to 64 GB Per VM",
            Category     = "Hyper-V Container Policy",
            Description  = "Limits the maximum amount of RAM that any single Hyper-V virtual machine can be assigned to 65536 MB (64 GB), preventing individual VMs from monopolising all host memory and providing fair resource sharing.",
            Tags         = ["hyper-v", "memory-limit", "resources", "fairness", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Maximum RAM per Hyper-V VM capped at 64 GB; no single VM can consume all host memory.",
            ApplyOps     = [RegOp.SetDword(Key, "MaxVMMemoryMB", 65536)],
            RemoveOps    = [RegOp.DeleteValue(Key, "MaxVMMemoryMB")],
            DetectOps    = [RegOp.CheckDword(Key, "MaxVMMemoryMB", 65536)],
        },
        new TweakDef
        {
            Id           = "hvcon-enable-secure-boot-for-vms",
            Label        = "Enforce Secure Boot Enabled for All New Hyper-V VMs",
            Category     = "Hyper-V Container Policy",
            Description  = "Requires that all new Hyper-V Generation 2 virtual machines are created with Secure Boot enabled, preventing VMs from being provisioned with Secure Boot disabled and booting unsigned guest operating systems.",
            Tags         = ["hyper-v", "secure-boot", "generation-2", "security", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Secure Boot enforced for all new Gen2 Hyper-V VMs; unsigned guest OS images cannot be booted.",
            ApplyOps     = [RegOp.SetDword(Key, "EnforceSecureBootForNewVMs", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "EnforceSecureBootForNewVMs")],
            DetectOps    = [RegOp.CheckDword(Key, "EnforceSecureBootForNewVMs", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-disable-vm-clipboard",
            Label        = "Disable Clipboard Sharing Between Hyper-V VMs and Host",
            Category     = "Hyper-V Container Policy",
            Description  = "Prevents clipboard data from being passed between Hyper-V virtual machine sessions and the host desktop, eliminating the clipboard as a data exfiltration channel between VM and host environments.",
            Tags         = ["hyper-v", "clipboard", "isolation", "data-protection", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Clipboard sharing between Hyper-V VMs and host disabled; data cannot be copy/pasted across VM boundary.",
            ApplyOps     = [RegOp.SetDword(VmKey, "DisableVMClipboard", 1)],
            RemoveOps    = [RegOp.DeleteValue(VmKey, "DisableVMClipboard")],
            DetectOps    = [RegOp.CheckDword(VmKey, "DisableVMClipboard", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-disable-vm-drives-mount",
            Label        = "Disable VM Drive Mounting from Guest to Host",
            Category     = "Hyper-V Container Policy",
            Description  = "Prevents Hyper-V virtual machines from mounting host filesystem paths via VMBus drive sharing, ensuring guest VMs cannot read host files through the Hyper-V file share integration component.",
            Tags         = ["hyper-v", "drive-mounting", "integration", "isolation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "VM drive mounting from guest to host disabled; Hyper-V file share integration component blocked.",
            ApplyOps     = [RegOp.SetDword(VmKey, "DisableVMDriveSharing", 1)],
            RemoveOps    = [RegOp.DeleteValue(VmKey, "DisableVMDriveSharing")],
            DetectOps    = [RegOp.CheckDword(VmKey, "DisableVMDriveSharing", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-block-container-user-creation",
            Label        = "Block Standard Users from Creating Windows Containers",
            Category     = "Hyper-V Container Policy",
            Description  = "Restricts Windows Container (Hyper-V isolation / process isolation) session creation to administrator accounts, preventing standard users from running containerised workloads that could bypass host security controls.",
            Tags         = ["containers", "windows-container", "standard-user", "isolation", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 4,
            SafetyRating = 5,
            ImpactNote   = "Windows Container creation restricted to admins; standard users cannot run Docker/containerd containers.",
            ApplyOps     = [RegOp.SetDword(CtrKey, "RequireAdminForContainerCreation", 1)],
            RemoveOps    = [RegOp.DeleteValue(CtrKey, "RequireAdminForContainerCreation")],
            DetectOps    = [RegOp.CheckDword(CtrKey, "RequireAdminForContainerCreation", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-disable-hyper-v-telemetry",
            Label        = "Disable Hyper-V and Container Management Telemetry to Microsoft",
            Category     = "Hyper-V Container Policy",
            Description  = "Prevents Hyper-V and Windows Containers from sending VM usage, crash, and performance telemetry to Microsoft.",
            Tags         = ["hyper-v", "containers", "telemetry", "privacy", "microsoft", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Hyper-V and container telemetry to Microsoft disabled; VM usage and performance data not sent to cloud.",
            ApplyOps     = [RegOp.SetDword(Key, "DisableHyperVTelemetry", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "DisableHyperVTelemetry")],
            DetectOps    = [RegOp.CheckDword(Key, "DisableHyperVTelemetry", 1)],
        },
        new TweakDef
        {
            Id           = "hvcon-log-vm-lifecycle-events",
            Label        = "Log Hyper-V VM Lifecycle Events in System Event Log",
            Category     = "Hyper-V Container Policy",
            Description  = "Enables System event log entries for Hyper-V virtual machine creation, deletion, start, stop, and live migration events, providing a complete audit trail of VM lifecycle changes for compliance.",
            Tags         = ["hyper-v", "event-log", "audit", "lifecycle", "policy"],
            NeedsAdmin   = true,
            CorpSafe     = true,
            ImpactScore  = 3,
            SafetyRating = 5,
            ImpactNote   = "Hyper-V VM lifecycle events logged; create/start/stop/migrate events visible in System log for auditing.",
            ApplyOps     = [RegOp.SetDword(Key, "LogVMLifecycleEvents", 1)],
            RemoveOps    = [RegOp.DeleteValue(Key, "LogVMLifecycleEvents")],
            DetectOps    = [RegOp.CheckDword(Key, "LogVMLifecycleEvents", 1)],
        },
    ];
}
