#nullable enable
using RegiLattice.Core.Models;
using RegiLattice.Core.Registry;

namespace RegiLattice.Core.Tweaks;

internal static class ContainerRuntime
{
    private const string ContPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Containers";
    private const string SandboxPolKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Sandbox";
    private const string HyperVKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\HyperV";
    private const string SandboxKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows Sandbox";
    private const string WcifKey = @"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\WindowsContainerIsolation";

    public static IReadOnlyList<TweakDef> Tweaks { get; } =
    [
        new TweakDef
        {
            Id = "cntr-enable-hyper-v-containers",
            Label = "Container: Enable Hyper-V Container Isolation",
            Category = "Virtualization",
            Tags = ["container", "hyper-v", "isolation", "docker", "developer", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            ImpactNote = "Hyper-V isolation provides VM-level security boundaries between containers.",
            Description =
                "Sets ContainerHyperVEngineEnabled=1 in the Windows Containers policy key. "
                + "Permits containers to run with Hyper-V (hardware) isolation rather than "
                + "only process isolation. Required for Docker Desktop's Hyper-V backend "
                + "and Windows Server containers with strong tenant isolation.",
            RegistryKeys = [ContPolKey],
            ApplyOps = [RegOp.SetDword(ContPolKey, "ContainerHyperVEngineEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(ContPolKey, "ContainerHyperVEngineEnabled")],
            DetectOps = [RegOp.CheckDword(ContPolKey, "ContainerHyperVEngineEnabled", 1)],
        },
        new TweakDef
        {
            Id = "cntr-disable-hyper-v-containers",
            Label = "Container: Disable Hyper-V Container Engine",
            Category = "Virtualization",
            Tags = ["container", "hyper-v", "disable", "developer", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disabling Hyper-V isolation restricts containers to process isolation only.",
            Description =
                "Sets ContainerHyperVEngineEnabled=0 in the Containers policy. Prevents "
                + "the Windows Container Manager from using Hyper-V isolation. Containers "
                + "can still run in process-isolation mode. Reduces hypervisor overhead "
                + "on developer machines that only need lightweight containers.",
            RegistryKeys = [ContPolKey],
            ApplyOps = [RegOp.SetDword(ContPolKey, "ContainerHyperVEngineEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(ContPolKey, "ContainerHyperVEngineEnabled")],
            DetectOps = [RegOp.CheckDword(ContPolKey, "ContainerHyperVEngineEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cntr-enable-process-containers",
            Label = "Container: Enable Windows Process-Isolation Container Engine",
            Category = "Virtualization",
            Tags = ["container", "process-isolation", "docker", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 5,
            ImpactNote = "Process-isolated containers share the kernel — lower overhead but weaker isolation.",
            Description =
                "Sets ContainerWindowsEngineEnabled=1 in the Containers policy. Enables "
                + "the Windows process-isolation container engine. Process-isolated containers "
                + "share the host kernel (similar to Linux containers) and are faster to start "
                + "than Hyper-V isolated containers.",
            RegistryKeys = [ContPolKey],
            ApplyOps = [RegOp.SetDword(ContPolKey, "ContainerWindowsEngineEnabled", 1)],
            RemoveOps = [RegOp.DeleteValue(ContPolKey, "ContainerWindowsEngineEnabled")],
            DetectOps = [RegOp.CheckDword(ContPolKey, "ContainerWindowsEngineEnabled", 1)],
        },
        new TweakDef
        {
            Id = "cntr-disable-process-containers",
            Label = "Container: Disable Windows Process-Isolation Container Engine",
            Category = "Virtualization",
            Tags = ["container", "process-isolation", "disable", "developer", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 4,
            SideEffects = "Docker Desktop process-isolation mode (Docker for Windows daemon) will not function.",
            ImpactNote = "Forcing Hyper-V only strengthens security at the cost of container startup speed.",
            Description =
                "Sets ContainerWindowsEngineEnabled=0 in the Containers policy. Disables "
                + "the process-isolation container engine, mandating Hyper-V isolation for "
                + "all containers. Enforces stronger tenant isolation in shared environments.",
            RegistryKeys = [ContPolKey],
            ApplyOps = [RegOp.SetDword(ContPolKey, "ContainerWindowsEngineEnabled", 0)],
            RemoveOps = [RegOp.DeleteValue(ContPolKey, "ContainerWindowsEngineEnabled")],
            DetectOps = [RegOp.CheckDword(ContPolKey, "ContainerWindowsEngineEnabled", 0)],
        },
        new TweakDef
        {
            Id = "cntr-disable-sandbox-vgpu",
            Label = "Container: Disable VGPU in Windows Sandbox",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "gpu", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Disabling VGPU in Sandbox reduces GPU sharing attack surface in multi-tenant scenarios.",
            Description =
                "Sets vGPU=Disable in the Windows Sandbox policy. Prevents the Windows "
                + "Sandbox VM from accessing the host GPU through hardware virtualisation. "
                + "Sandbox continues to operate using software rendering. "
                + "Reduces the GPU attack surface on systems hosting multiple concurrent Sandboxes.",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "VirtualGPU", "Disable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "VirtualGPU")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "VirtualGPU", "Disable")],
        },
        new TweakDef
        {
            Id = "cntr-enable-sandbox-vgpu",
            Label = "Container: Enable VGPU in Windows Sandbox (GPU-Accelerated Sandbox)",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "gpu", "hardware-acceleration", "developer"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 4,
            SafetyRating = 4,
            SideEffects = "GPU-accelerated Sandbox requires compatible GPU drivers and may expose more GPU attack surface.",
            ImpactNote = "GPU-accelerated rendering in Sandbox enables testing graphics apps inside the isolated environment.",
            Description =
                "Sets VirtualGPU=Enable in the Windows Sandbox policy. Enables hardware "
                + "GPU acceleration inside Windows Sandbox. Allows graphics applications "
                + "and web browsers inside Sandbox to use the host's GPU for hardware-accelerated "
                + "rendering. Default: disabled (software rendering).",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "VirtualGPU", "Enable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "VirtualGPU")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "VirtualGPU", "Enable")],
        },
        new TweakDef
        {
            Id = "cntr-disable-sandbox-networking",
            Label = "Container: Disable Networking in Windows Sandbox",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "network", "security", "isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Air-gapped Sandbox cannot exfiltrate data or download malware during analysis.",
            Description =
                "Sets Networking=Disable in the Windows Sandbox policy. Cuts all network "
                + "access for Windows Sandbox sessions. The Sandbox VM is fully air-gapped "
                + "from the host network. Ideal for safe execution of untrusted files "
                + "without risk of C2 communication or data leakage.",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "Networking", "Disable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "Networking")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "Networking", "Disable")],
        },
        new TweakDef
        {
            Id = "cntr-disable-sandbox-clipboard",
            Label = "Container: Disable Clipboard Sharing with Windows Sandbox",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "clipboard", "security", "isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 3,
            SafetyRating = 5,
            ImpactNote = "Clipboard isolation prevents data from leaking in or out of the Sandbox VM.",
            Description =
                "Sets ClipboardRedirection=Disable in the Windows Sandbox policy. Prevents "
                + "cut/copy/paste between the Sandbox VM and the host. Strengthens isolation "
                + "when analysing potentially malicious files or websites inside Sandbox.",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "ClipboardRedirection", "Disable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "ClipboardRedirection")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "ClipboardRedirection", "Disable")],
        },
        new TweakDef
        {
            Id = "cntr-disable-sandbox-printer",
            Label = "Container: Disable Printer Sharing with Windows Sandbox",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "printer", "security", "isolation"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents printer redirection channels from being used as a data exfiltration path.",
            Description =
                "Sets PrinterRedirection=Disable in the Windows Sandbox policy. Prevents "
                + "the Sandbox VM from accessing host printers. Removes the printer "
                + "redirection channel which could theoretically be used to exfiltrate "
                + "data from an untrusted application running in Sandbox.",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "PrinterRedirection", "Disable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "PrinterRedirection")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "PrinterRedirection", "Disable")],
        },
        new TweakDef
        {
            Id = "cntr-disable-sandbox-audio",
            Label = "Container: Disable Audio Input in Windows Sandbox",
            Category = "Virtualization",
            Tags = ["sandbox", "windows-sandbox", "audio", "microphone", "security"],
            NeedsAdmin = true,
            CorpSafe = true,
            ImpactScore = 2,
            SafetyRating = 5,
            ImpactNote = "Prevents malware inside Sandbox from recording audio through the host microphone.",
            Description =
                "Sets AudioInput=Disable in the Windows Sandbox policy. Prevents the "
                + "Sandbox VM from accessing the host's audio input devices (microphone). "
                + "Stops untrusted code running in Sandbox from recording user audio or "
                + "conducting acoustic side-channel attacks.",
            RegistryKeys = [SandboxPolKey],
            ApplyOps = [RegOp.SetString(SandboxPolKey, "AudioInput", "Disable")],
            RemoveOps = [RegOp.DeleteValue(SandboxPolKey, "AudioInput")],
            DetectOps = [RegOp.CheckString(SandboxPolKey, "AudioInput", "Disable")],
        },
    ];
}
