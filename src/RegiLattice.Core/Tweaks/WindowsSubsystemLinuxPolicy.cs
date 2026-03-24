// RegiLattice.Core — Tweaks/WindowsSubsystemLinuxPolicy.cs
// Windows Subsystem for Linux (WSL) Group Policy — Sprint 192.
// Controls WSL2 feature availability, kernel debugging, disk mounting,
// networking access, GPU compute, and virtual TPM via enterprise Group Policy.
// Category: "WSL Policy" | Slug: wslpol
// Registry path: HKLM\SOFTWARE\Policies\Microsoft\Windows\Lxss

namespace RegiLattice.Core.Tweaks;

using RegiLattice.Core.Models;

internal static class WindowsSubsystemLinuxPolicy
{
    private const string LxssKey = @"HKEY_LOCAL_MACHINE\SOFTWARE\Policies\Microsoft\Windows\Lxss";

    public static IReadOnlyList<TweakDef> Tweaks =>
        [
            new TweakDef
            {
                Id = "wslpol-disable-wsl",
                Label = "Disable Windows Subsystem for Linux",
                Category = "WSL Policy",
                Description =
                    "Sets Enabled=0 in the Lxss policy key to disable WSL entirely. Prevents installation or launch of any Linux distributions. Use in high-security or compliance environments where Linux workloads on Windows are prohibited.",
                Tags = ["wsl", "linux", "subsystem", "policy", "enterprise"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 3,
                ImpactNote = "Blocks all WSL distributions from running; developers lose Linux workflow access.",
                ApplyOps = [RegOp.SetDword(LxssKey, "Enabled", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "Enabled")],
                DetectOps = [RegOp.CheckDword(LxssKey, "Enabled", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-kernel-debugging",
                Label = "Block WSL Kernel Debugging",
                Category = "WSL Policy",
                Description =
                    "Sets AllowKernelDebugging=0 in the WSL policy key. Prevents users from attaching kernel debuggers to the WSL2 virtual machine, reducing the attack surface from kernel exploits originating in Linux workloads.",
                Tags = ["wsl", "kernel", "debug", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks kernel debugger attachment to WSL2 VM; no impact on normal WSL usage.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowKernelDebugging", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowKernelDebugging")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowKernelDebugging", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-dev-mode-install",
                Label = "Block WSL Developer Mode Installs",
                Category = "WSL Policy",
                Description =
                    "Sets AllowDeveloperModeInstall=0 in the WSL policy key. Blocks installation of Linux distributions from developer mode sources outside the Microsoft Store, enforcing controlled distribution channels.",
                Tags = ["wsl", "developer", "install", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks sideloaded WSL distributions; Store-sourced distros unaffected.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowDeveloperModeInstall", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowDeveloperModeInstall")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowDeveloperModeInstall", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-custom-kernel",
                Label = "Block WSL Custom Kernel Configuration",
                Category = "WSL Policy",
                Description =
                    "Sets AllowCustomKernelConfiguration=0 in the WSL policy key. Prevents users from replacing the WSL2 kernel with custom builds via .wslconfig, ensuring only the Microsoft-signed kernel runs.",
                Tags = ["wsl", "kernel", "custom", "policy", "integrity"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 5,
                ImpactNote = "Blocks custom WSL kernel; standard WSL operations unaffected.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowCustomKernelConfiguration", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowCustomKernelConfiguration")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowCustomKernelConfiguration", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-disable-disk-mount",
                Label = "Disable WSL Disk Image Mounting",
                Category = "WSL Policy",
                Description =
                    "Sets AllowMount=0 in the WSL policy key. Blocks mounting of physical disks or VHD image files inside WSL2, preventing lateral movement via raw disk access from within a Linux distribution.",
                Tags = ["wsl", "mount", "disk", "security", "policy"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 3,
                SafetyRating = 4,
                ImpactNote = "Prevents 'wsl --mount' command; limits developers who mount external drives in WSL.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowMount", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowMount")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowMount", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-wsl-networking",
                Label = "Block WSL Outbound Networking",
                Category = "WSL Policy",
                Description =
                    "Sets AllowNetworking=0 in the WSL policy key. Disables network access for all WSL2 distributions, isolating Linux workloads from the corporate network and the internet.",
                Tags = ["wsl", "network", "isolation", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 4,
                SafetyRating = 4,
                ImpactNote = "Completely isolates WSL2 network stack; breaks package managers (apt, pip) inside Linux.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowNetworking", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowNetworking")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowNetworking", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-disable-systemd",
                Label = "Disable systemd in WSL",
                Category = "WSL Policy",
                Description =
                    "Sets AllowSystemd=0 in the WSL policy key. Prevents WSL2 distributions from using systemd as PID 1, blocking system service management that could be exploited for persistence mechanisms.",
                Tags = ["wsl", "systemd", "service", "policy", "hardening"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 4,
                ImpactNote = "Blocks systemd-managed services in WSL2; affects distros relying on systemd service units.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowSystemd", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowSystemd")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowSystemd", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-gpu-compute",
                Label = "Block WSL GPU Compute Access",
                Category = "WSL Policy",
                Description =
                    "Sets AllowGPUCompute=0 in the WSL policy key. Disables DirectML and GPU acceleration inside WSL2, preventing Linux processes from accessing GPU compute resources via the host WDDM driver.",
                Tags = ["wsl", "gpu", "compute", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks GPU/DirectML from WSL2; CUDA and ML workloads inside WSL affected.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowGPUCompute", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowGPUCompute")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowGPUCompute", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-disable-dns-tunneling",
                Label = "Disable WSL DNS Tunneling",
                Category = "WSL Policy",
                Description =
                    "Sets AllowDNSTunneling=0 in the WSL policy key. Prevents WSL2 from using DNS-over-HTTPS tunneling mode, ensuring DNS queries from Linux distributions go through standard Windows resolver configuration.",
                Tags = ["wsl", "dns", "tunnel", "policy", "network"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Disables DoH DNS tunneling in WSL2; DNS resolution still works via standard Windows DNS.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowDNSTunneling", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowDNSTunneling")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowDNSTunneling", 0)],
            },
            new TweakDef
            {
                Id = "wslpol-block-vtpm",
                Label = "Block WSL Virtual TPM",
                Category = "WSL Policy",
                Description =
                    "Sets AllowVTPM=0 in the WSL policy key. Blocks Linux distributions from accessing a virtual TPM device inside WSL2. Reduces risk of TPM-based key material in the Linux trust boundary.",
                Tags = ["wsl", "tpm", "virtual", "policy", "security"],
                NeedsAdmin = true,
                CorpSafe = true,
                ImpactScore = 2,
                SafetyRating = 5,
                ImpactNote = "Blocks vTPM access from WSL2; affects WSL distributions requiring TPM attestation.",
                ApplyOps = [RegOp.SetDword(LxssKey, "AllowVTPM", 0)],
                RemoveOps = [RegOp.DeleteValue(LxssKey, "AllowVTPM")],
                DetectOps = [RegOp.CheckDword(LxssKey, "AllowVTPM", 0)],
            },
        ];
}
